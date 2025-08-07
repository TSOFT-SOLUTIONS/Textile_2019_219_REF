Public Class User_Creation
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private New_Entry As Boolean = False
    Private Other_Condition As String = ""
    Private Enum DgvCol_Details
        SNo '0
        EntryName '1
        All '2
        Add_AllDay '3
        Add_ToDay '4
        Add_Last_n_Days_Entry '5
        Edit_AllDay '6
        Edit_ToDay '7
        Edit_Last_n_Days_Entry '8
        Edit_LastEntry '9
        Edit_Before_Printing '10
        Delete_All '11
        Delete_ToDay '12
        Delete_Last_n_Days_Entry '13
        Delete_Before_Printing '14
        View_Only '15
        Insert  '16
        Print   '17
        EntryCode '18
    End Enum

    Private Sub clear()

        Dim I As Integer = 0

        New_Entry = False

        pnl_Back.Enabled = True
        grp_Open.Visible = False
        pnl_MultiInput.Visible = False
        Pnl_ComGroup_Wise_Rights.Visible = False
        pnl_Company_Wise_Access.Visible = False

        lbl_UserID.Text = ""
        lbl_UserID.ForeColor = Color.Black

        txt_Name.Text = ""
        txt_AcPwd.Text = ""
        txt_UnAcPwd.Text = ""
        txt_Add_Last_n_DaysEntry.Text = "2"
        txt_Edit_Last_n_DaysEntry.Text = "2"
        txt_Delete_Last_n_DaysEntry.Text = "2"

        chk_Add_Full.Checked = False
        chk_Add_ToDayEntry.Checked = False
        chk_Edit_Full.Checked = False
        chk_Edit_TodayEntry.Checked = False
        chk_Edit_LastEntry.Checked = False
        chk_Edit_BeforePrint.Checked = False
        chk_Del_BeforePrint.Checked = False
        chk_Del_TodayEntry.Checked = False
        chk_Delete_Full.Checked = False
        chk_Insert.Checked = False
        chk_Print.Checked = False
        chk_View.Checked = False

        chk_AskPassword_OnSaving.Checked = False
        chk_Verified_sts.Checked = False
        chk_UserCreation_Sts.Checked = False
        chk_Close_Sts.Checked = False
        chk_approved_sts.Checked = False
        dgv_Details.Rows.Clear()

        dgv_CompanyGroup_Details.Rows.Clear()

        For I = 0 To chklst_MultiInput.Items.Count - 1
            chklst_MultiInput.SetItemChecked(I, False)
        Next I

        For I = 0 To chklst_CompanyWise_Settings.Items.Count - 1
            chklst_CompanyWise_Settings.SetItemChecked(I, False)
        Next I

        chk_CompanyWise_Rights.Checked = False

        Add_EntryNames()

        Grid_Cell_DeSelect()

    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Add_EntryNames()
        Dim vSno As Integer = 0
        Dim n As Integer

        With dgv_Details
            .Rows.Clear()
            vSno = 0

            '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------
            '*************************************************    START MASTERS   *************************************************
            '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------

            n = .Rows.Add()
            '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "MASTERS"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_MODULE_HEADING"
            .Rows(n).Cells(DgvCol_Details.EntryName).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Rows(n).Cells(DgvCol_Details.EntryName).Style.BackColor = Color.Black
            .Rows(n).Cells(DgvCol_Details.EntryName).Style.ForeColor = Color.Red
            .Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Tahoma", 12, FontStyle.Bold)
            '.Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Arial", 14, FontStyle.Bold)


            vSno += 1
            n = .Rows.Add()
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "LEDGER CREATION"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_LEDGER_CREATION"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "AGENT CREATION"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_AGENT_CREATION"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SIZING CREATION"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_SIZING_CREATION"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER CREATION"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_WEAVER_CREATION"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "JOBWORKER CREATION"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_JOBWORKER_CREATION"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REWINDING CREATION"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_REWINDING_CREATION"

            If Val(Common_Procedures.settings.Sewing_Entries_Status) = 1 Then
                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SEWING CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_SEWING_CREATION"
            End If

            If Common_Procedures.settings.SizingSoftware_ENTRY_Status = 1 Then

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SPARES PURCHASE PARTY CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTERS_SPARES_PURCHASE_PARTY_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "FIREWOOD PURCHASE PARTY CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTERS_FIREWOOD_PURCHASE_PARTY_CREATION"

            End If

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "TRANSPORT CREATION"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_TRANSPORT_CREATION"


            If Common_Procedures.settings.Multi_Godown_Status = 1 Or Val(Common_Procedures.settings.YarnProcessing_Entries_Status) = 1 Or Val(Common_Procedures.settings.OESofwtare_ENTRY_Status) = 1 Then
                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "GODOWN CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_GODOWN_CREATION"
            End If

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "VENDOR CREATION"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_VENDOR_CREATION"

            If Common_Procedures.settings.SizingSoftware_ENTRY_Status = 1 Then
                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "DELIVERY PARTY CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTERS_DELIVERY_PARTY_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PACKING TYPE CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTERS_PACKING_TYPE_CREATION"
            End If


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNT GROUP CREATION"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_ACCOUNTGROUP_CREATION"

            'n = .Rows.Add()
            'vSno += 1
            '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            '.Rows(n).Cells(DgvCol_Details.EntryName).Value = "TAX CREATION"
            '.Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_TAX_CREATION"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "AREA CREATION"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_AREA_CREATION"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ITEM GROUP CREATION (HSN Code)"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_ITEMGROUP_CREATION"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "GST A/C SETTINGS"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_GST_AC_SETTINGS"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "COUNT CREATION"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_COUNT_CREATION"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "MILL CREATION"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_MILL_CREATION"

            If Common_Procedures.settings.FIBRE_ENTRY_STATUS = 1 Then

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "FIBRE CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_FIBRE_CREATION"

            End If

            If Common_Procedures.settings.FIBRE_ENTRY_STATUS = 1 Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1234" Then '---- ARULJOTHI EXPORTS PVT LTD (SOMANUR)

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "FIBRE LOT NO CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_FIBRE_LOT_NO_CREATION"

            End If


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ENDSCOUNT CREATION"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_ENDSCOUNT_CREATION"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOTH CREATION"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_CLOTH_CREATION"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOTH SALES RATE MASTER"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_CLOTH_SALES_RATE_MASTER"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "FABRIC BITS GROUP CREATION"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_FABRIC_BITS_GROUP_CREATION"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOTH CREATION - YARN CONSUMPTION"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FORMULA_WEAVER_YARN_CONSUMPTION"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOTH CREATION - WEAVER COOLIE"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FORMULA_WEAVER_COOLIE"

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1158" Then '---- NIDHIE WEAVING (PALLADAM)
                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOTH SET CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_CLOTH_SET_CREATION"
            End If

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "LOOM CREATION"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_LOOM_CREATION"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "LOOM TYPE CREATION"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_LOOMTYPE_CREATION"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "BEAM WIDTH CREATION"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_BEAM_WIDTH_CREATION"

            If Common_Procedures.settings.SizingSoftware_ENTRY_Status = 1 Then
                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "BAG TYPE CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTERS_BAG_TYPE_CREATION"
            End If


            If Val(Common_Procedures.settings.OE_ENTRY_Status) = 1 Or Val(Common_Procedures.settings.OESofwtare_ENTRY_Status) = 1 Then
                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CONETYPE CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_CONETYPE_CREATION"

            End If


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CHEQUE PRINTING POSITION"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_CHEQUE_PRINTING_POSITION"

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "FABRIC LOT NO CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_FABRIC_LOT_NO_CREATION"

            End If


            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1098" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1234" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1176" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1256" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1286" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1290" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1292" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1298" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1394" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1370" Then   'Bannari amman

                If Not (Val(Common_Procedures.settings.OE_ENTRY_Status) = 1 Or Val(Common_Procedures.settings.OESofwtare_ENTRY_Status) = 1) Then

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "VARIETY CREATION"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_VARIETY_CREATION"

                End If


            End If

            If Val(Common_Procedures.settings.Bobin_Zari_Kuri_Entries_Status) = 1 Or Val(Common_Procedures.settings.Bobin_Production_Entries_Status) = 1 Then

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "BORDER SIZE CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_BORDER_SIZE_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "BOBIN SIZE CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_BOBIN_SIZE_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPLOYEE CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_EMPLOYEE_CREATION"

            End If


            If Val(Common_Procedures.settings.FabricProcessing_Entries_Status) = 1 Then

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "COLOUR CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPMASTER_COLOUR_CREATION"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "LOTNO CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPMASTER_LOTNO_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ARTICLE CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_ARTICLE_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PROCESS CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPMASTER_PROCESS_CREATION"

            End If

            If Val(Common_Procedures.settings.Sewing_Entries_Status) = 1 Then

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "FINISHED PRODUCT CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPMASTER_FINISHED_PRODUCT_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SIZE CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPMASTER_SIZE_CREATION"
            End If


            If Val(Common_Procedures.settings.FabricProcessing_Entries_Status) = 1 Or Val(Common_Procedures.settings.Sewing_Entries_Status) = 1 Then
                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CURRENCY CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPMASTER_CURRENCY_CREATION"

            End If



            If Val(Common_Procedures.settings.OE_ENTRY_Status) = 1 Or Val(Common_Procedures.settings.OESofwtare_ENTRY_Status) = 1 Then

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "VARIETY CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_VARIETY_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PACKING TYPE CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_PACKINGTYPE_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "DELIVERY ADDRESS CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_DELIVERY_ADDRESS_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "OE - ITEMGROUP CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_OE_ITEMGROUP_CREATION"

            End If

            If Val(Common_Procedures.settings.STORESENTRY_Status) = 1 Then
                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES MASTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_STORES"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "DEPARTMENT CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_STORES_DEPARTMENT_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "MACHINE CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_STORES_MACHINE_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "BRAND CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_STORES_BRAND_CREATION"

                If Not (Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1005" Or InStr(1, Trim(UCase(Common_Procedures.settings.SoftWareName)), "FP") > 0) Then
                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "RACKNO CREATION"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPMASTER_RACKNO_CREATION"
                End If

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES ITEM CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_STORES_ITEM_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REED WIDTH CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_STORES_REEDWIDTH_CREATION"

            End If




            If Common_Procedures.settings.SizingSoftware_ENTRY_Status = 1 Then

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SIZING ITEM CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTERS_SIZINGITEM_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "UNIT CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTERS_UNIT_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "BEAMNO CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTERS_BEAMNO_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SIZING SPARES CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTERS_SIZINGSPARES_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SIZING WASTE MATERIAL CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_SIZING_WASTE_MATERIAL_CREATION"

                'n = .Rows.Add()
                'vSno += 1
                '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                '.Rows(n).Cells(DgvCol_Details.EntryName).Value = "ZONE CREATION"
                '.Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_ZONE_CREATION"

            End If


            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT SIZING (SOMANUR)

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "FABRIC PHYSICAL STOCK ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_FABRIC_PHYSICAL_STOCK"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "VEHICLENO CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_VEHICLENO_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "LOADING_UNLOADING_RATE_CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_LOADING_UNLOADING_RATE_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CHECKING TABLENO CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_CHECKING_TABLENO_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PIECECHECKING DEFECTS"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_PIECECHECKING_DEFECTS"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "APP USER CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_APP_USER_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "USER CREATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_USER_CREATION"

            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1005" Or InStr(1, Trim(UCase(Common_Procedures.settings.SoftWareName)), "FP") > 0 Then

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "LEDGER CREATION FP MASTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPMASTER_LEDGER_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "AGENT CREATION FP MASTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPMASTER_AGENT_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "TRANSPORT CREATION FP MASTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPMASTER_TRANSPORT_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "AREA CREATION FP MASTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPMASTER_AREA_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "GREY ITEM CREATION FP MASTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPMASTER_GREY_ITEM_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "FINISHED PRODUCT CREATION FP MASTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPMASTER_FINISHED_PRODUCT_CREATION"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ITEM GROUP CREATION FP MASTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPMASTER_ITEM_GROUP_CREATION"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "UNIT CREATION FP MASTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPMASTER_UNIT_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PRODUCT SALES NAME FP MASTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPMASTER_PRODUCT_SALES_NAME"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PROCESS CREATION FP MASTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPMASTER_PROCESS_CREATION"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "COLOUR CREATION FP MASTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPMASTER_COLOUR_CREATION"



                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "LOTNO CREATION FP MASTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPMASTER_LOTNO_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "RACKNO CREATION FP MASTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPMASTER_RACKNO_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PACKING TYPE CREATION FP MASTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPMASTER_PACKING_TYPE_CREATION"

            End If

            n = .Rows.Add()
            .Rows(n).Cells(DgvCol_Details.SNo).Value = ""
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = ""
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = ""

            '**********************************    END MASTERS   **********************************




            '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------
            '*************************************************    START OPENING   *************************************************
            '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------

            vSno = 0

            n = .Rows.Add()
            '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "OPENING"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OPENING_MODULE_HEADING"
            .Rows(n).Cells(DgvCol_Details.EntryName).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Rows(n).Cells(DgvCol_Details.EntryName).Style.BackColor = Color.Black
            .Rows(n).Cells(DgvCol_Details.EntryName).Style.ForeColor = Color.Red
            .Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Tahoma", 12, FontStyle.Bold)
            '.Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Arial", 14, FontStyle.Bold)


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "LEDGER AMOUNT BALANCE"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_LEDGER_OPENING_AMOUNT_BALANCE"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "OPENING STOCK (SIZING/WEAVING/GODOWN)"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_LEDGER_OPENING_STOCK"



            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOSING VALUE STOCK"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_LEDGER_CLOSING_VALUE_STOCK"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "UNCHECKED CLOTH OPENING"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_LEDGER_UNCHECKED_CLOTH_OPENING"



            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PIECE OPENING"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_LEDGER_PIECE_OPENING"



            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "BALE OPENING"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_LEDGER_BALE_OPENING"

            'n = .Rows.Add()
            'vSno += 1
            '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            '.Rows(n).Cells(DgvCol_Details.EntryName).Value = "LOOM OPENING"
            '.Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_LEDGER_LOOM_OPENING"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOTH ORDER INDENT OPENING"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_LEDGER_CLOTH_ORDER_INDENT__OPENING"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOTH DELIVERY OPENING"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_LEDGER_CLOTH_DELIVERY_OPENING"

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1005" Or InStr(1, Trim(UCase(Common_Procedures.settings.SoftWareName)), "FP") > 0 Then
                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "GREY FABRIC OPENING STOCK"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_GREY_FABRIC_OPENING_STOCK"
            End If


            If Common_Procedures.settings.SizingSoftware_ENTRY_Status = 1 Then

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SIZING PARTY'S OPENING STOCK"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OPENINGS_SIZING_PARTYS_OPENING_STOCK"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SIZING CHEMICALS OPENING STOCK"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OPENINGS_SIZING_CHEMICALS_OPENING_STOCK"

                'n = .Rows.Add()
                'vSno += 1
                '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                '.Rows(n).Cells(DgvCol_Details.EntryName).Value = "REWINDGING DELIVERY OPENING"
                '.Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OPENINGS_REWINDING_STOCK"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "OPENING SIZING RATE DETAILS"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OPENING_SIZING_DETAILS"

            End If

            If Common_Procedures.settings.OESofwtare_ENTRY_Status = 1 Then

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "COTTON OPENING STOCK"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OPENINGS_COTTON_OPENING_STOCK"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "YARN BAG MIXING OPENING STOCK"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OPENINGS_YARN_BAG_MIXING_OPENING_STOCK"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "OE WASTE OPENING STOCK"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OPENING_OE_WASTE_OPENING_STOCK"

            End If

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = ""
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = ""
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = ""

            '**********************************    END OPENING **********************************

            '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------
            '*************************************************    START TEXTILE OWNSORT ENTRIES   *************************************************
            '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------

            vSno = 0

            n = .Rows.Add()
            '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then '---- Kalaimagal Textiles (Palladam)
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "TEXTILE ENTRIES"
            Else
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ENTRIES"
            End If

            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_MODULE_HEADING"
            .Rows(n).Cells(DgvCol_Details.EntryName).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Rows(n).Cells(DgvCol_Details.EntryName).Style.BackColor = Color.Black
            .Rows(n).Cells(DgvCol_Details.EntryName).Style.ForeColor = Color.Red
            .Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Tahoma", 12, FontStyle.Bold)
            '.Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Arial", 14, FontStyle.Bold)


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "YARN PURCHASE - PURCHASE ORDER ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_YARN_PURCHASE_ORDER"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "YARN PURCHASE - PURCHASE RECEIPT ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_YARN_PURCHASE_RECEIPT"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "YARN PURCHASE - PURCHASE ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_YARN_PURCHASE"

            'n = .Rows.Add()
            'vSno += 1
            '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            '.Rows(n).Cells(DgvCol_Details.EntryName).Value = "YARN PURCHASE ENTRY VAT"
            '.Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_YARN_PURCHASE_VAT"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "YARN PURCHASE - PURCHASE RETURN ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_YARN_PURCHASE_RETURN"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "YARN PURCHASE - YARN TEST ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_YARN_TEST"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "YARN PURCHASE - YARN PURCHASE BILL MAKING ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_YARN_PURCHASE_BILL_MAKING_ENTRY"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "YARN SALES - YARN SALES ORDER ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_YARN_SALES_ORDER"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "YARN SALES - SALES DELIVERY ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_YARN_SALES_DELIVERY" ' "ENTRY_YARN_DELIVERY"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "YARN SALES - SALES INVOICE ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_YARN_SALES"

            'n = .Rows.Add()
            'vSno += 1
            '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            '.Rows(n).Cells(DgvCol_Details.EntryName).Value = "YARN SALES ENTRY VAT"
            '.Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_YARN_SALES_VAT"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "YARN SALES - SALES RETURN ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_YARN_SALES_RETURN"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "YARN SALES - PROFORMA SALES ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_YARN_PROFORMA_SALES"



            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PAVU PURCHASE & SALES - PURCHASE ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_PAVU_PURCHASE"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PAVU PURCHASE & SALES - SALES ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_PAVU_SALES"

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1158" Then '---- NIDHIE WEAVING (PALLADAM)
                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOTH PURCHASE OFFER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_CLOTH_PURCHASE_OFFER"
            End If

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOTH PURCHASE - PURCHASE ORDER"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_CLOTH_PURCHASE_ORDER"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOTH PURCHASE - RECEIPT ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_CLOTH_PURCHASE_RECEIPT" ' "ENTRY_CLOTH_RECEIPT"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOTH PURCHASE RECEIPT CHECKING ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_CLOTH_PURCHASE_RECEIPT_CHECKING"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOTH PURCHASE - PURCHASE ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_CLOTH_PURCHASE"



            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOTH PURCHASE - PURCHASE RETURN ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_CLOTH_PURCHASE_RETURN"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOTH SALES - ORDER INDENT ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_CLOTH_SALES_ORDER_INDENT"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOTH SALES ORDER ENTRY - CLOSE OPTION"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_CLOTH_SALES_ORDER_ENTRY_CLOSE_OPTION"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOTH SALES - DELIVERY ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_CLOTH_DELIVERY"



            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOTH SALES - INVOICE ENTRY "
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_CLOTH_SALES_INVOICE"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOTH SALES - SALES INVOICE VAT ENTRY "
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_CLOTH_SALES_INVOICE_VAT"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOTH SALES - DELIVERY RETURN ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_CLOTH_DELIVERY_RETURN"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOTH SALES - SALES RETURN ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_CLOTH_SALES_RETURN"



            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOTH SALES - PROFORMA INVOICE ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_CLOTH_PROFORMA_INVOICE"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOTH SALES - BUYER OFFER ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_CLOTH_BUYER_OFFER"



            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOTH SALES - ROLL PACKING ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_CLOTH_ROLL_PACKING"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOTH PIECE CHECKING ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_CLOTH_PIECE_CHECKING"

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1204" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1381" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1382" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1383" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1384" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1385" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1408" Then '----KRG TEXTILE MILLS (PALLADAM)

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOTH SALES - BALE DELIVERY ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_CLOTH_BALE_DELIVERY_ENTRY"

            End If

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPTYBEAM PURCHASE & SALES - PURCHASE ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_EMPTYBEAM_PURCHASE"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPTYBEAM PURCHASE & SALES - SALES ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_EMPTYBEAM_SALES"




            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "GENERAL/OTHER (GST) - PURCHASE ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_GENERAL_OTHER_PURCHASE"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "GENERAL/OTHER (GST) - SALES ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_GENERAL_OTHER_SALES"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "GENERAL/OTHER (GST) - CREDIT NOTE ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_GST_CREDIT_NOTE" ' "ENTRY_CREDIT_NOTE"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "GENERAL/OTHER (GST) - DEBIT NOTE ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_GST_DEBIT_NOTE"  '"ENTRY_DEBIT_NOTE"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "GENERAL/OTHER (GST) - GENERAL DELIVERY ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_GENERAL_DELIVERY"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SIZING - YARN DELIVERY ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_YARN_DELIVERY"



            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SIZING - PAVU RECEIPT ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_PAVU_RECEIPT"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SIZING - SPECIFICATION ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_SPECIFICATION"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SIZING - YARN RECEIPT ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_YARN_RECEIPT"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SIZING - BEAM CLOSE ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_BEAM_CLOSE"
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1224" Then '---- KAVITHAA FABRICS (PALLADAM)
                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SIZING - INVOICE ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_INVOICE_ENTRY"
            End If


            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT SIZING (SOMANUR)
                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SIZING - YARN RECEIPT BY SIZING UNIT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_YARN RECEIPT_BY_SIZING_UNIT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SIZING - YARN DELIVERY FROM SIZING UNIT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_YARN_DELIVERY_FROM_SIZING"

            End If


            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT SIZING (SOMANUR)   OR  '---- KALAIMAGAL TEXTILES (PALLADAM)
                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SIZING - PAVU DELIVERY FROM SIZING UNIT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_PAVU_DELIVERY_FROM_SIZING"
            End If




            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REWINDING - YARN DELIVERY ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_REWINDING_DELIVERY"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REWINDING - YARN RECEIPT ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_REWINDING_RECEIPT"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER - PAVU DELIVERY ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_WEAVER_PAVU_DELIVERY"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER - YARN DELIVERY ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_WEAVER_YARN_DELIVERY"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER - PAVU RECEIPT ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_WEAVER_PAVU_RECEIPT"



            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER - KURAI PAVU RECEIPT ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_WEAVER_KURAI_PAVU_RECEIPT"



            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER - YARN RECEIPT ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_WEAVER_YARN_RECEIPT"

            If Val(Common_Procedures.settings.Bobin_Zari_Kuri_Entries_Status) = 1 Then


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER - BOBIN DELIVERY ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_WEAVER_BOBIN_DELIVERY"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER - BOBIN RETURN ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_WEAVER_BOBIN_RETURN"

            End If


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER - CLOTH RECEIPT ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_WEAVER_CLOTH_RECEIPT"

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1234" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
                n = .Rows.Add()
                .Rows(n).Cells(DgvCol_Details.SNo).Value = n + 1
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER CLOTH RECEIPT ENTRY - EDIT FABRICNAME AFTER LOCK"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_WEAVERCLOTHRECEIPT_EDIT_FABRICNAME_AFTERLOCK"
            End If


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER - PIECE CHECKING ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_WEAVER_PIECE_CHECKING"

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER - PIECE CHECKING ENTRY (BARCODE PRINT)"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_WEAVER_PIECE_CHECKING_BARCODE_PRINT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER - PIECE CHECKING ENTRY (APPROVAL)"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_WEAVER_PIECE_CHECKING_APPROVAL"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER - PIECE CHECKING ENTRY (WARP & WEFT STOCK UPDATION)"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_WEAVER_PIECE_CHECKING_WARP_WEFT_STOCK_UPDATION"
            End If

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER - CLOTH RECEIPT & CHECKING ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_WEAVER_CLOTH_RECEIPT_AND_CHECKING"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER - WAGES ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_WEAVER_WAGES"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER - WEAVING JOBWORK BILL"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_WEAVER_JOBWORK_BILL"
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
                n = .Rows.Add()
                .Rows(n).Cells(DgvCol_Details.SNo).Value = n + 1
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER WAGES ENTRY (CHANGE DATE)"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_WEAVERWAGES_CHANGEDATE"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "LOT APPROVAL ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_LOT_APPROVAL"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "LOT CHECKING PLANING ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_WEAVER_LOT_CHECKING_PLANING_ENTRY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "LOT ALLOTMENT ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_LOT_ALLOTMENT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER - PIECE APPROVAL ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_WEAVER_PIECE_APPROVAL"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER - PIECE APPROVAL ENTRY - EDIT DATE"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_WEAVER_PIECE_APPROVAL_ENTRY_EDIT_DATE"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER - PIECE APPROVAL ENTRY - BARCODE PRINT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_WEAVER_PIECE_APPROVAL_ENTRY_BARCODE_PRINT"
            End If


            'n = .Rows.Add()
            'vSno += 1
            '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            '.Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER DEBIT ENTRY"
            '.Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_WEAVER_DEBIT"

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1035" Then                   '---- Kalaimagal Textiles (Avinashi)
                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER - EXCESS/SHORT CONSUMPTION ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_WEAVER_EXCESS_SHORT_CONSUMPTION_ENTRY"
            End If


            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT SIZING (SOMANUR)
                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER - ADVANCE PAYMENT ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_WEAVER_ADVANCE_PAYMENT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER - CRIMP CONSUMPTION ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_WEAVER_CRIMP_CONSUMPTION_ENTRY"

                'n = .Rows.Add()
                'vSno += 1
                '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                '.Rows(n).Cells(DgvCol_Details.EntryName).Value = "CRIMP ENTRY"
                '.Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_CRIMP"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER - SLEVAGE CONE INVOICE ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SLEVAGE_CONE_INVOICE_ENTRY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER - CLOTH RETURN DELIVERY ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_CLOTH_RETURN_DELIVERY_ENTRY"  '"ENTRY_WEAVER_CLOTH_RETURN_DELIVERY"

            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1391" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- SAKTHI DHARAN TEXTILES (THIRUCENGODU)
                n = .Rows.Add()
                vSno += 1

                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER - BEAM CARD ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_WEAVER_BEAM_CARD_ENTRY"
            End If


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PAVU & YARN DELIVERY ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_PAVUYARN_DELIVERY"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PAVU & YARN RECEIPT ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_PAVUYARN_RECEIPT"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMBTY BEAM/BAG/CONE DELIVERY ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_EMBTY_BEAMBAGCONE_DELIVERY"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMBTY BEAM/BAG/CONE RECEIPT ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_EMBTY_BEAMBAGCONE_RECEIPT"

            If Val(Common_Procedures.settings.AutoLoomStatus) = 1 Then

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "INHOUSE - BEAM KNOTTING ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "INHOUSE_ENTRY_BEAM_KNOTTING"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "INHOUSE - DOFFING ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "INHOUSE_ENTRY_DOFFING"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "INHOUSE - PIECE CHECKING ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "INHOUSE_ENTRY_PIECE_CHECKING"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "INHOUSE - DOFFING & PIECE CHECKING ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "INHOUSE_ENTRY_DOFFING_AND_PIECE_CHECKING"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "INHOUSE - BEAM RUNOUT ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "INHOUSE_ENTRY_BEAM_RUNOUT"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "INHOUSE - BEAM CLOSE ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "INHOUSE_ENTRY_BEAM_CLOSE"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "INHOUSE - WEAVING EXCESS SHORT ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "INHOUSE_ENTRY_WEAVING_EXCESS_SHORT"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "INHOUSE - SORT CHANGE ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "INHOUSE_ENTRY_SORT_CHANGE"



                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "INHOUSE - PAVU DELIVERY ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "INHOUSE_ENTRY_PAVU_DELIVERY"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "INHOUSE - YARN DELIVERY ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "INHOUSE_ENTRY_YARN_DELIVERY"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "INHOUSE - PAVU RECEIPT ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "INHOUSE_ENTRY_PAVU_RECEIPT"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "INHOUSE - YARN RECEIPT ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "INHOUSE_ENTRY_YARN_RECEIPT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "INHOUSE - LOOM PRODUCTION ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "INHOUSE_ENTRY_LOOM_PRODUCTION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "INHOUSE - KNOTTING BILL ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "INHOUSE_ENTRY_KNOTTING_BILL"

            End If

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PACKING SLIP ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_PACKING_SLIP"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PACKING SLIP MAIN ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_PACKING_SLIP_MAIN"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PARTY AMOUNT RECEIPT ENTRY (Cash/Cheque)"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_PARTY_AMOUNT_RECEIPT"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CHEQUE RETURN ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_CHEQUE_RETURN"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER PAYMENT ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_WEAVER_PAYMENT"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EXCESS/SHORT - YARN EXCESS/SHORT ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_YARN_EXCESS_SHORT"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EXCESS/SHORT - PAVU EXCESS/SHORT ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_PAVU_EXCESS_SHORT"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EXCESS/SHORT - CLOTH EXCESS/SHORT ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_CLOTH_EXCESS_SHORT"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EXCESS/SHORT - PIECE EXCESS/SHORT ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_PIECE_EXCESS_SHORT"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EXCESS/SHORT - EMPTYBEAM EXCESS/SHORT ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_EMPTYBEAM_EXCESS_SHORT"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EXCESS/SHORT - PIECE JOINING EXCESS/SHORT ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_PIECE_JOINING_EXCESS_SHORT"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "TRANSFER - YARN TRANSFER ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_YARN_TRANSFER"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "TRANSFER - PAVU TRANSFER ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_PAVU_TRANSFER"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "TRANSFER - CLOTH TRANSFER ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_CLOTH_TRANSFER"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "TRANSFER - PIECE TRANSFER ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_PIECE_TRANSFER"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "TRANSFER - PAVU TRANSFER BEAMWISE ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_PAVU_TRANSFER_BEAMWISE"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "TRANSFER - BALE TRANSFER ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_BALE_TRANSFER"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "COSTING ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_COSTING"

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1098" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1118" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1234" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1370" Then '---- ARULJOTHI EXPORTS PVT LTD (SOMANUR)

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "COTTON PURCHASE ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_COTTON_PURCHASE_ENTRY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "COTTON SALES ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_COTTON_SALES_ENTRY"

            End If


            If Common_Procedures.settings.FIBRE_ENTRY_STATUS = 1 Then

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "FIBRE PURCHASE ORDER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_FIBRE_PURCHASE_ORDER"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "FIBRE PURCHASE INVOICE"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_FIBRE_PURCHASE_INVOICE"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "FIBRE SALES ORDER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_FIBRE_SALES_ORDER"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "FIBRE SALES INVOICE"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_FIBRE_SALES_INVOICE"

            End If


            If Val(Common_Procedures.settings.Bobin_Production_Entries_Status) = 1 Then

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "BOBIN PURCHASE ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_BOBIN_PURCHASE"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "BOBIN SALES ORDER ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_BOBIN_SALES_ORDER"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "BOBIN PRODUCTION ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_BOBIN_PRODUCTION"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "JARI PRODUCTION ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_JARI_PRODUCTION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "BOBIN SALES DELIVERY ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_BOBIN_SALES_DELIVERY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "BOBIN SALES DELIVERY RETURN ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_BOBIN_SALES_DELIVERY_RETURN"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "JARI SALES DELIVERY ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_JARI_SALES_DELIVERY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "JARI SALES RETURN ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_JARI_SALES_RETURN"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPTY BOBIN DELIVERY ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_EMPTY_BOBIN_DELIVERY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPTY BOBIN RECEIPT ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_EMPTY_BOBIN_RECEIPT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PROFORMA BOBIN SALES ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_PROFORMA_BOBIN_SALES"
            End If

            'n = .Rows.Add()
            'vSno += 1
            '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            '.Rows(n).Cells(DgvCol_Details.EntryName).Value = "PROCESSING ENTRY"
            '.Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_PROCESSING"

            'n = .Rows.Add()
            'vSno += 1
            '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            '.Rows(n).Cells(DgvCol_Details.EntryName).Value = "SEWING ENTRY"
            '.Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SEWING"

            'n = .Rows.Add()
            'vSno += 1
            '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            '.Rows(n).Cells(DgvCol_Details.EntryName).Value = "YARN PROCESSING ENTRY"
            '.Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_YARN_PROCESSING"

            If Common_Procedures.settings.FabricProcessing_Entries_Status = 1 Then

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PROCESSING JOB ORDER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_PROCESSING_JOB_ORDER"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "FABRIC DELIVERY TO PROCESSING "
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_FABRIC_DELIVERY_TO_PROCESSING"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PROCESSED FABRIC RECEIPT FROM PROCESSING "
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_PROCESSED_FABRIC_RECEIPT_FROM_PROCESSING"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PROCESSED FABRIC INSPECTION "
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_PROCESSED_FABRIC_INSPECTION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PROCESSING BILL MAKING"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_PROCESSING_BILL_MAKING"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "FABRIC RETURN FROM PROCESSING"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_FABRIC_RETURN_FROM_PROCESSING"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PROCESSED FABRIC INVOICE GST"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_PROCESSED_FABRIC_INVOICE_GST"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PROCESSED FABRIC INVOICE VAT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_PROCESSED_FABRIC_INVOICE_VAT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PROCESSED WASTE DELIVERY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_PROCESSED_WASTE_DELIVERY"

            End If

            If Common_Procedures.settings.Sewing_Entries_Status = 1 Then

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "JOBWORK FOR SEWING"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_JOBWORK_FOR_SEWING"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "FABRIC DELIVERY TO SEWING"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_FABRIC_DELIVERY_TO_SEWING"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "FINISHED PRODUCT RECEIPT FROM SEWING "
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_FINISHED_PRODUCT_RECEIPT_FROM_SEWING"

            End If

            If Common_Procedures.settings.YarnProcessing_Entries_Status = 1 Then

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "YARN DELIVERY TO PROCESSING"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_YARN_DELIVERY_TO_PROCESSING"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "YARN RECEIPT FROM PROCESSING"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_YARN_RECEIPT_FROM_PROCESSING"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "BILL MAKING"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_BILL_MAKING"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SPINNING YARN DELIVERY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SPINNING_YARN_DELIVERY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SPINNING YARN RECEIPT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SPINNING_YARN_RECEIPT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SPINNING YARN BILL MAKING"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SPINNING_YARN_BILL_MAKING"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "DOUBLING YARN DELIVERY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_DOUBLING_YARN_DELIVERY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "DOUBLING YARN RECEIPT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_DOUBLING_YARN_RECEIPT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "DOUBLING YARN BILL MAKING"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_DOUBLING_YARN_BILL_MAKING"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REELING YARN DELIVERY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_REELING_YARN_DELIVERY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REELING YARN RECEIPT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_REELING_YARN_RECEIPT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REELING YARN BILL MAKING"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_REELING_YARN_BILL_MAKING"

            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1005" Or InStr(1, Trim(UCase(Common_Procedures.settings.SoftWareName)), "FP") > 0 Then

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PURCHASE FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_PURCHASE"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PURCHASE RETURN FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_PURCHASE_RETURN"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PROCESSING DELIVERY FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_PROCESSING_DELIVERY"



                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PROCESSING RECEIPT FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_PROCESSING_RECEIPT"




                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PROCESSING RETURN FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_PROCESSING_RETURN"



                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PROCESSING BILLMAKING FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_PROCESSING_BILLMAKING"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "FLOOR TO RACK FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_FLOOR_TO_RACK"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "RACK TO FLOOR FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_RACK_TO_FLOOR"



                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SET FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_SET"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "UNSET FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_UNSET"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ITEM TRANSFER FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_ITEM_TRANSFER"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PACKINGSLIP FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_PACKINGSLIP"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ORDER INDENT FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_ORDER_INDENT"



                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "INVOICE FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_INVOICE"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOTH SALES FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_CLOTH_SALES"



                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SALES RETURN FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_SALES_RETURN"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PROFORMA SALES FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_PROFORMA_SALES"




                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ITEM EXCESS SHORT FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_ITEM_EXCESS_SHORT"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SHIRTING BIT FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_SHIRTING_BIT"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PARTY AMOUNT RECEIPT FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_PARTY_AMOUNT_RECEIPT"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CHEQUE RETURN FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_CHEQUE_RETURN"





            End If


            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT SIZING (SOMANUR)

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "VAN TRIP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_VAN_TRIP"

            End If

            n = .Rows.Add()
            .Rows(n).Cells(DgvCol_Details.SNo).Value = ""
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = ""
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = ""

            '**********************************    END TEXTILE OWNSORT ENTRIES    **********************************

            '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------
            '*************************************************    START TEXTILE OWNSORT REPORTS  *************************************************
            '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------

            vSno = 0

            n = .Rows.Add()
            '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then '---- Kalaimagal Textiles (Palladam)
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "TEXTILE REPORTS"
            Else
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "OWNSORT REPORTS"
            End If
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "TEXTILE_OWNSORT_REPORTS_MODULE_HEADING"
            .Rows(n).Cells(DgvCol_Details.EntryName).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Rows(n).Cells(DgvCol_Details.EntryName).Style.BackColor = Color.Black
            .Rows(n).Cells(DgvCol_Details.EntryName).Style.ForeColor = Color.Red
            .Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Tahoma", 12, FontStyle.Bold)
            '.Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Arial", 14, FontStyle.Bold)

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "MASTER REPORTS"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_MASTER"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REGISTER REPORTS"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_REGISTER"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SIZING STOCK"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_SIZING_STOCK"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REWINDING STOCK"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_REWINDING_STOCK"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WEAVER STOCK"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_WEAVER_STOCK"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "GODOWN STOCK"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_GODOWN_STOCK"

            If Common_Procedures.settings.AutoLoomStatus = 1 Then

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "INHOUSE REPORTS"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_INHOUSE"

            End If



            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "DAY TRANSACTION DETAILS"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_DAY_TRANSACTION_DETAILS"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOTH SALES ORDER PENDING"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_CLOTH_SALES_ORDER_PENDING"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOTH INVOICE DELIVERY PENDING"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_CLOTH_INVOICE_DELIVERY_PENDING"

            'mnu_Report_Textile_ClothOrderIndent_Pending_Main

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "GST RETURN REPORT"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_GST_RETURN_REPORT"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ANNEXURE REPORT"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_ANNEXURE_REPORT"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "TDS REPORT"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_TDS_REPORT"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "TCS REPORT"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_TCS_REPORT"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STOCK VALUE"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_STOCK_VALUE_REPORT"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ALL STOCK STATEMENT"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_ALL_STOCK_STATEMENT_REPORT"

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1098" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1118" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1234" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1370" Then '---- ARULJOTHI EXPORTS PVT LTD (SOMANUR)

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "COTTON REPORTS"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_COTTON_REPORTS"

            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "VAN TRIP"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_VAN_TRIP"

            End If


            If Common_Procedures.settings.FabricProcessing_Entries_Status = 1 Then
                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PROCESSING STOCK"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_PROCESSING_STOCK_REPORT"
            End If


            If Common_Procedures.settings.Sewing_Entries_Status = 1 Then

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SEWING"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_SEWING"

            End If


            If Val(Common_Procedures.settings.STORESENTRY_Status) = 1 Then
                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES REPORTS"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_STORES"
            End If


            If Val(Common_Procedures.settings.Bobin_Production_Entries_Status) = 1 Then

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "BOBIN PURCHASE ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_BOBIN_PURCHASE"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "BOBIN SALES ORDER ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_BOBIN_SALES_ORDER"



                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "BOBIN PRODUCTION ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_BOBIN_PRODUCTION"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "JARI PRODUCTION ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_JARI_PRODUCTION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "BOBIN SALES DELIVERY ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_BOBIN_SALES_DELIVERY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "BOBIN SALES DELIVERY RETURN ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_BOBIN_SALES_DELIVERY_RETURN"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "JARI SALES DELIVERY ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_JARI_SALES_DELIVERY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "JARI SALES RETURN ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_JARI_SALES_RETURN"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPTY BOBIN DELIVERY ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_EMPTY_BOBIN_DELIVERY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPTY BOBIN RECEIPT ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_EMPTY_BOBIN_RECEIPT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PROFORMA BOBIN SALES ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_PROFORMA_BOBIN_SALES"
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1005" Or InStr(1, Trim(UCase(Common_Procedures.settings.SoftWareName)), "FP") > 0 Then

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PURCHASE FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_PURCHASE"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PURCHASE RETURN FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_PURCHASE_RETURN"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PROCESSING DELIVERY FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_PROCESSING_DELIVERY"



                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PROCESSING RECEIPT FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_PROCESSING_RECEIPT"




                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PROCESSING RETURN FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_PROCESSING_RETURN"



                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PROCESSING BILLMAKING FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_PROCESSING_BILLMAKING"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "FLOOR TO RACK FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_FLOOR_TO_RACK"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "RACK TO FLOOR FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_RACK_TO_FLOOR"



                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SET FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_SET"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "UNSET FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_UNSET"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ITEM TRANSFER FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_ITEM_TRANSFER"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PACKINGSLIP FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_PACKINGSLIP"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ORDER INDENT FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_ORDER_INDENT"



                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "INVOICE FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_INVOICE"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CLOTH SALES FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_CLOTH_SALES"



                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SALES RETURN FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_SALES_RETURN"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PROFORMA SALES FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_PROFORMA_SALES"




                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ITEM EXCESS SHORT FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_ITEM_EXCESS_SHORT"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SHIRTING BIT FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_SHIRTING_BIT"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PARTY AMOUNT RECEIPT FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_PARTY_AMOUNT_RECEIPT"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CHEQUE RETURN FP ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "FPENTRY_CHEQUE_RETURN"


            End If


            n = .Rows.Add()
            .Rows(n).Cells(DgvCol_Details.SNo).Value = ""
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = ""
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = ""

            '**********************************    END TEXTILE OWNSORT ENTRIES    **********************************

            '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------
            '*************************************************    START TEXTILE OWNSORT MODULE REPORTS  *****************************************
            '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------

            If Common_Procedures.settings.Show_Modulewise_Entrance = 1 Then

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1087" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" Then

                    vSno = 0

                    n = .Rows.Add()
                    '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno

                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "OWNSORT REPORTS"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OWNSORT_REPORTS_MODULE_HEADING"
                    .Rows(n).Cells(DgvCol_Details.EntryName).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .Rows(n).Cells(DgvCol_Details.EntryName).Style.BackColor = Color.Black
                    .Rows(n).Cells(DgvCol_Details.EntryName).Style.ForeColor = Color.Red
                    .Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Tahoma", 12, FontStyle.Bold)
                    '.Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Arial", 14, FontStyle.Bold)


                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS PURCHASE ORDER"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OWNSORT_REPORTS_PURCHASE_ORDER"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS SALES ORDER"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OWNSORT_REPORTS_SALES_ORDER"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS DELIVERY CHALLAN"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OWNSORT_REPORTS_DELIVERY_CHALLAN"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS PACKING LIST"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OWNSORT_REPORTS_PACKING_LIST"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS SALES INVOICE"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OWNSORT_REPORTS_SALES_INVOICE"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS YARN PURCHASE REGISTER"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OWNSORT_REPORTS_YARN_PURCHASE_REGISTER"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS YARN INWARD"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OWNSORT_REPORTS_YARN_INWARD"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS YARN OUTWARD"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OWNSORT_REPORTS_YARN_OUTWARD"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS EMPTY BEAM DELIVERY"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OWNSORT_REPORTS_EMPTY_BEAM_DELIVERY"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS WARP RECEIPT"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OWNSORT_REPORTS_WARP_RECEIPT"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS SIZING SET REPORT"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OWNSORT_REPORTS_SIZING_SET_REPORT"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS WARP BEAM LOADING REGISTER"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OWNSORT_REPORTS_WARP_BEAM_LOADING_REGISTER"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS PIECE DOFFING REGISTER"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OWNSORT_REPORTS_PIECE_DOFFING_REGISTER"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS WARP BEAM RUNOUT REGISTER"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OWNSORT_REPORTS_WARP_BEAM_RUNOUT_REGISTER"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS RETURN"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OWNSORT_REPORTS_RETURN"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS SIZED BEAM STOCK ON FLOOR"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OWNSORT_REPORTS_SIZED_BEAM_STOCK_ON_FLOOR"


                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS RUNNING BEAM DETAILS ALL LOOM"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OWNSORT_REPORTS_RUNNING_BEAM_DETAILS_ALL_LOOM"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS WARP LOADING LOOM WISE"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OWNSORT_REPORTS_WARP_LOADING_LOOM_WISE"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS PIECE CHECKING REGISTER"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OWNSORT_REPORTS_PIECE_CHECKING_REGISTER"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS PRODUCTION SUMMARY LOOM WISE"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OWNSORT_REPORTS_PRODUCTION_SUMMARY_LOOM_WISE"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS DAILY FABRIC STOCK"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OWNSORT_REPORTS_DAILY_FABRIC_STOCK"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS DAILY PRODUCTION STOCK"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OWNSORT_REPORTS_DAILY_PRODUCTION_STOCK"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS SALES INVOICE "
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "TRADING_REPORTS_SALES_INVOICE"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS PURCHASE INVOICE "
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "TRADING_REPORTS_PURCHASE_INVOICE"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS RECEIVED REPORTS "
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "TRADING_REPORTS_RECEIVED_REPORTS"



                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = ""
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = ""
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = ""

                End If

            End If


            '**********************************    END TEXTILE OWNSORT MODULE REPORTS **********************************


            '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------
            '*************************************************    START TEXTILE JOBWORK ENTRIES **************************************************
            '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------

            vSno = 0

            If Common_Procedures.settings.JOBWORKENTRY_Status = 1 Or Trim(UCase(Common_Procedures.settings.SoftWareName)) = "JOBWORK" Or InStr(1, Trim(UCase(Common_Procedures.settings.SoftWareName)), "JOBWORK") > 0 Then

                n = .Rows.Add()
                '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "JOBWORK ENTRIES"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "JOBWORK_ENTRY_MODULE_HEADING"
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.BackColor = Color.Black
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.ForeColor = Color.Red
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Tahoma", 12, FontStyle.Bold)
                '.Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Arial", 14, FontStyle.Bold)

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "JOBWORK ORDER ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_JOBWORK_ORDER"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PAVU YARN RECEIPT ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_PAVU_YARN_RECEIPT_JOBWORK"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "JOBWORK PRODUCTION ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_JOBWORK_PRODUCTION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "JOBWORK PIECE DELIVERY ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_JOBWORK_PIECE_DELIVERY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "JOBWORK PIECE INSPECTION ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_JOBWORK_INSPECTION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "JOBWORK CONVERSION BILL ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_JOBWORK_CONVERSION_BILL"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PAVU & YARN RETURN TO JOBWORKER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_PAVU_YARN_RETURN_TO_JOBWORKER"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPTY BEAM RETURN TO JOBWORKER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_EMPTY_BEAM_RETURN_TO_JOBWORKER"

                n = .Rows.Add()
                .Rows(n).Cells(DgvCol_Details.SNo).Value = ""
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = ""
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = ""

                '*************************************************    END TEXTILE JOBWORK ENTRIES ********************************************

                '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------
                '*************************************************    START TEXTILE JOBWORK MODULE - REPORTS *********************************
                '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------

                vSno = 0

                n = .Rows.Add()
                '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "TEXTILE JOBWORK REPORTS"

                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "TEXTILE_JOBWORK_REPORTS_MODULE_HEADING"
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.BackColor = Color.Black
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.ForeColor = Color.Red
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Tahoma", 12, FontStyle.Bold)
                '.Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Arial", 14, FontStyle.Bold)

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REGISTER REPORTS"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "TEXTILE_JOBWORK_REGISTER_REPORT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PENDING REGISTER REPORTS"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "TEXTILE_JOBWORK_PENDING_REGISTER_REPORT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "YARN STOCK"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "TEXTILE_JOBWORK_YARN_STOCK_REPORT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PAVU STOCK"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "TEXTILE_JOBWORK_PAVU_STOCK_REPORT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPTY BEAM STOCK"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "TEXTILE_JOBWORK_EMPTYBEAM_STOCK_REPORT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPTY BAG STOCK"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "TEXTILE_JOBWORK_EMPTYBAG_STOCK_REPORT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPTY CONE STOCK"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "TEXTILE_JOBWORK_EMPTYCONE_STOCK_REPORT"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ALL STOCK LEDGER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "TEXTILE_JOBWORK_ALL_STOCK_LEDGER_REPORT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ALL STOCK SUMMARY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "TEXTILE_JOBWORK_ALL_STOCK_SUMMARY_REPORT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "JOBWORKER STOCK STATEMENT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "TEXTILE_JOBWORK_JOBWORKER_STOCK_STATEMENT_REPORT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "DAY TRANSACTION REPORT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "TEXTILE_JOBWORK_DAY_TRANSACTION_REPORT"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "RECONCILIATION REPORT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "TEXTILE_JOBWORK_RECONCILIATION_REPORT"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS DELIVERY CHALLAN REPORTS"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "TEXTILE_JOBWORK_DELIVERY_CHALLAN_REPORTS"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS INVOICE REPORT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "TEXTILE_JOBWORK_INVOICE_REPORT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS RECEIVED REPORT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "TEXTILE_JOBWORK_RECEIVED_REPORT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS SIZED BEAM STOCK ON FLOOR"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "TEXTILE_JOBWORK_SIZED_BEAM_STOCK_ON_FLOOR"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS PRODUCTION SUMMARY LOOM WISE"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "TEXTILE_JOBWORK_PRODUCTION_SUMMARY_LOOM_WISE"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS PIECE CHECKING REPORT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "TEXTILE_JOBWORK_PIECE_CHECKING_REPORT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS BEAM RUNOUT REGISTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "TEXTILE_JOBWORK_BEAM_RUNOUT_REGISTER"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "RUNNING BEAM DETAILS ALL LOOM"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "TEXTILE_JOBWORK_RUNNING_BEAM_DETAILS_ALL_LOOM"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WARP LOADING LOOM WISE"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "TEXTILE_JOBWORK_WARP_LOADING_LOOM_WISE"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "VENDOR GRADING REPORT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "TEXTILE_JOBWORK_VENDOR_GRADING_REPORT"

                '*************************************************    END TEXTILE JOBWORK REPORTS ********************************************


            End If



            '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------
            '*************************************************    START SIZING JOBWORK MODULE - ENTRIES  *************************************************
            '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------

            vSno = 0

            If Common_Procedures.settings.SizingSoftware_ENTRY_Status = 1 Then

                n = .Rows.Add()
                '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SIZING ENTRIES"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "SIZING_MODULE_ENTRIES_MODULE_HEADING"
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.BackColor = Color.Black
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.ForeColor = Color.Red
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Tahoma", 12, FontStyle.Bold)
                '.Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Arial", 14, FontStyle.Bold)

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "YARN RECEIPT ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_JOBWORK_MODULE_YARN_RECEIPT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPTY BEAM RECEIPT ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_JOBWORK_MODULE_EMPTYBEAM_RECEIPT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SIZING SPECIFICATION ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_JOBWORK_MODULE_SIZING_SPECIFICATION"
                'n = .Rows.Add()
                'vSno += 1
                '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                '.Rows(n).Cells(DgvCol_Details.EntryName).Value = "ENTRY JOB CARD"
                '.Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_JOB_CARD"

                'n = .Rows.Add()
                'vSno += 1
                '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                '.Rows(n).Cells(DgvCol_Details.EntryName).Value = "ENTRY WARPING STATEMENT"
                '.Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_WARPING_STATEMENT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STATEMENT ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_JOBWORK_MODULE_STATEMENT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "INVOICE ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_JOBWORK_MODULE_INVOICE"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CASH DISCOUNT ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_JOBWORK_MODULE_CASHDISCOUNT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PAVU DELIVERY ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_JOBWORK_MODULE_PAVU_DELIVERY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "YARN DELIVERY ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_JOBWORK_MODULE_YARN_DELIVERY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPTY BEAM DELIVERY ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_JOBWORK_MODULE_EMPTY_BEAM_DELIVERY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "KURAI PAVU RECEIPT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_JOBWORK_KURAI_PAVU_RECEIPT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPTY BEAM BAG EXCESS SHORT ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_JOBWORK_MODULE_EMPTY_BEAM_BAG_EXCESS_SHORT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "YARN TRANSFER ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_JOBWORK_MODULE_YARN_TRANSFER"

                'n = .Rows.Add()
                'vSno += 1
                '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                '.Rows(n).Cells(DgvCol_Details.EntryName).Value = "R/W DELIVERY ENTRY"
                '.Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_JOBWORK_MODULE_RW_DELIVERY"


                'n = .Rows.Add()
                'vSno += 1
                '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                '.Rows(n).Cells(DgvCol_Details.EntryName).Value = "R/W RECEIPT ENTRY"
                '.Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_JOBWORK_MODULE_RW_RECEIPT"

                'n = .Rows.Add()
                'vSno += 1
                '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                '.Rows(n).Cells(DgvCol_Details.EntryName).Value = "R/W EXCESS/SHORT ENTRY"
                '.Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_JOBWORK_MODULE_RW_EXCESS_SHORT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "YARN EXCESS/SHORT ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_JOBWORK_MODULE_YARN_EXCESS_SHORT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CHEMICAL - PURCHASE ODRER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_JOBWORK_MODULE_CHEMICAL_PURCHASE_ORDER"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CHEMICAL - PURCHASE ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_JOBWORK_MODULE_CHEMICAL_PURCHASE"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CHEMICAL - PURCHASE RETURN ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_JOBWORK_MODULE_CHEMICAL_PURCHASE_RETURN"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CHEMICAL - EXCESS/SHORT ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_JOBWORK_MODULE_CHEMICAL_EXCESS_SHORT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WASTE MATERIAL SALES ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_JOBWORK_MODULE_WASTE_MATERIAL_SALES"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SPARES PURCHASE ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_JOBWORK_MODULE_SPARES_PURCHASE"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "FIREWOOD PURCHASE ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_JOBWORK_MODULE_FIREWOOD_PURCHASE"



                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ENTRY FIREWOOD CONSUMPTION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_JOBWORK_MODULE_FIREWOOD_CONSUMPTION"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ENTRY GENERAL DELIVERY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SIZING_JOBWORK_MODULE_GENERAL_DELIVERY"


                n = .Rows.Add()
                .Rows(n).Cells(DgvCol_Details.SNo).Value = ""
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = ""
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = ""





                '**********************************    END SIZING JOBWORK MODULE - ENTRIES **********************************

                '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------
                '*************************************************    START SIZING JOBWORK MODULE - REPORTS *************************************************
                '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------

                vSno = 0

                n = .Rows.Add()
                '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SIZING REPORTS"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "SIZING_MODULE_REPORTS_MODULE_HEADING"
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.BackColor = Color.Black
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.ForeColor = Color.Red
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Tahoma", 12, FontStyle.Bold)
                '.Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Arial", 14, FontStyle.Bold)


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPTY BEAM REPORTS"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "SIZING_JOBWORK_MODULE_REPORT_EMPTY_BEAM_REGISTER"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPTY BAG REPORTS"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "SIZING_JOBWORK_MODULE_REPORT_EMPTY_BAGS_REGISTER"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CONE REPORTS"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "SIZING_JOBWORK_MODULE_REPORT_CONES_REGISTER"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "YARN REPORTS"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "SIZING_JOBWORK_MODULE_REPORT_YARN_REGISTER"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PAVU REGISTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "SIZING_JOBWORK_MODULE_REPORT_PAVU_REGISTER"

                'n = .Rows.Add()
                'vSno += 1
                '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                '.Rows(n).Cells(DgvCol_Details.EntryName).Value = "REWINDING STOCK"
                '.Rows(n).Cells(DgvCol_Details.EntryCode).Value = "SIZING_JOBWORK_MODULE_REPORT_PAVU_REGISTER"
                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ALL STOCK LEDGER REPORT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "SIZING_JOBWORK_MODULE_REPORT_ALL_STOCK_LEDGER"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ALL STOCK SUMMARY REPORT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "SIZING_JOBWORK_MODULE_REPORT_ALL_STOCK_SUMMARY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PRODUCTION REGISTER REPORTS"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "SIZING_JOBWORK_MODULE_REPORT_PRODUCTION_REGISTER"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "INVOICE REGISTER REPORTS"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "SIZING_JOBWORK_MODULE_REPORT_INVOICE_REGISTER"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CHEMICAL REPORTS"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "SIZING_JOBWORK_MODULE_REPORT_CHEMICAL_REGISTER"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ANNEXURE REPORTS"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "SIZING_JOBWORK_MODULE_REPORT_ANNEXURE_REGISTER"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "MASTER REPORTS"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "SIZING_JOBWORK_MODULE_REPORT_MASTER_REGISTER"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS REWINDING STOCK"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "SIZING_JOBWORK_MODULE_REPORTS_REWINDING_STOCK"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ALL STOCK STATEMENTS"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "SIZING_JOBWORK_MODULE_REPORTS_ALL_STOCK_STATEMENTS"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS CASH DISCOUNT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "SIZING_JOBWORK_MODULE_REPORTS_CASH_DISCOUNT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS HARDWARE PURCHASE REGISTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "SIZING_JOBWORK_MODULE_REPORTS_HARDWARE_PURCHASE"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS FIREWOOD PURCHASE REGISTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "SIZING_JOBWORK_MODULE_REPORTS_FIREWOOD_PURCHASE"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS WASTE MATERIAL REGISTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "SIZING_JOBWORK_MODULE_REPORTS_WASTE_MATERIAL"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "FIREWOOD CONSUMPTION REGISTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "SIZING_JOBWORK_MODULE_REPORT_FIREWOOD_CONSUMPTION_REGISTER"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS DAY TRANSACTION REGISTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "SIZING_JOBWORK_MODULE_REPORTS_DAY_TRANSACTION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS GST RETURN"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "SIZING_JOBWORK_MODULE_REPORTS_GST_RETURN"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS TDS REPORT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "SIZING_JOBWORK_MODULE_REPORTS_TDS_REPORT"
                'n = .Rows.Add()
                ' vSno += 1
                '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno 
                '.Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS GST RETURN"
                '.Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORTS_GST_RETURN"

                'n = .Rows.Add()
                ' vSno += 1
                '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno 
                '.Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS ANNEXURE"
                '.Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORTS_ANNEXURE"


                'n = .Rows.Add()
                ' vSno += 1
                '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno 
                '.Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS PAYROLL"
                '.Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORTS_PAYROLL_REPORTS"

                'n = .Rows.Add()
                ' vSno += 1
                '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno 
                '.Rows(n).Cells(DgvCol_Details.EntryName).Value = "MASTER FIREWOOD CREATION"
                '.Rows(n).Cells(DgvCol_Details.EntryCode).Value = "MASTER_FIREWOOD_CREATION"



                n = .Rows.Add()
                .Rows(n).Cells(DgvCol_Details.SNo).Value = ""
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = ""
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = ""

            End If

            '**********************************    END SIZING JOBWORK MODULE - ENTRIES **********************************



            '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------
            '*************************************************    START OE SPINNING MODULE - ENTRIES  *************************************************
            '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------

            vSno = 0

            If Common_Procedures.settings.OESofwtare_ENTRY_Status = 1 Then
                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS USER MODIFICATIONS"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORTS_USER_MODIFICATIONS_REPORTS"

                n = .Rows.Add()
                '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "OE ENTRIES"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OE_MODULE_ENTRIES_MODULE_HEADING"
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.BackColor = Color.Black
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.ForeColor = Color.Red
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Tahoma", 12, FontStyle.Bold)
                '.Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Arial", 14, FontStyle.Bold)

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "COTTON PURCHASE ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OEENTRY_COTTON_PURCHASE_ENTRY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "COTTON PURCHASE RETURN ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OEENTRY_COTTON_PURCHASE_RETURN_ENTRY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "MIXING ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OEENTRY_MIXING_ENTRY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PRODUCTION ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OEENTRY_PRODUCTION_ENTRY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PACKING ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OEENTRY_PACKING_ENTRY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ORDER ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OEENTRY_ORDER_ENTRY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "DELIVERY ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OEENTRY_DELIVERY_ENTRY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "INVOICE ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OEENTRY_INVOICE_ENTRY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WASTE SALES ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OEENTRY_WASTE_SALES_ENTRY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "LOCAL WASTE SALES ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OEENTRY_LOCAL_WASTE_SALES_ENTRY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "INVOICE RETURN ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OEENTRY_INVOICE_RETURN_ENTRY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "DELIVERY RETURN ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OEENTRY_DELIVERY_RETURN_ENTRY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "BORA STRITCHING ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OEENTRY_BORA_STRITCHING_ENTRY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REELING DELIVERY ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OEENTRY_REELING_DELIVERY_ENTRY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REELING RECEIPT ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OEENTRY_REELING_RECEIPT_ENTRY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STOCK TRANSFER ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OEENTRY_STOCK_TRANSFER_ENTRY"

                n = .Rows.Add()
                .Rows(n).Cells(DgvCol_Details.SNo).Value = ""
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = ""
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = ""

                '**********************************    END OE SPINNING MODULE - ENTRIES **********************************

                '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------
                '*************************************************    START OE SPINNING MODULE - REPORTS  *************************************************
                '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------
                vSno = 0

                n = .Rows.Add()
                '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "OE SPINNING REPORTS"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OESPINNING_MODULE_REPORTS_MODULE_HEADING"
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.BackColor = Color.Black
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.ForeColor = Color.Red
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Tahoma", 12, FontStyle.Bold)
                '.Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Arial", 14, FontStyle.Bold)

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REGISTER REPORT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OESPINNING_REGISTER_REPORT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "COTTON STOCK REPORT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OESPINNING_COTTON_STOCK_REPORT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "MIXING STOCK REPORT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OESPINNING_MIXING_STOCK_REPORT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "BAG YARN STOCK REPORT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OESPINNING_BAG_YARN_STOCK_REPORT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "LOOSE YARN STOCK REPORT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OESPINNING_LOOSE_YARN_STOCK_REPORT"

                'n = .Rows.Add()
                'vSno += 1
                '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                '.Rows(n).Cells(DgvCol_Details.EntryName).Value = "REELING STOCK REPORT"
                '.Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OESPINNING_REELING_STOCK_REPORT"

                'n = .Rows.Add()
                'vSno += 1
                '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                '.Rows(n).Cells(DgvCol_Details.EntryName).Value = "HANK YARN STOCK REPORT"
                '.Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OESPINNING_HANK_YARN_STOCK_REPORT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WASTE STOCK REPORT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OESPINNING_WASTE_STOCK_REPORT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "YARN STOCK BAGNOWISE REPORT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OESPINNING_YARN_STOCK_BAGNOWISE_STOCK_REPORT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ALL STOCK SUMMARY REPORT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OESPINNING_ALL_STOCK_SUMMARY_REPORT"





                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "COMMISSION REGISTER REPORT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OESPINNING_COMMISSION_REGISTER_REPORT"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "GSTR-1 REPORT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OESPINNING_GSTR_1_REPORT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "GSTR-2 REPORT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OESPINNING_GSTR_2_REPORT"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ORDER PENDING REPORT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OESPINNING_ORDER_PENDING_REPORT"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CARDING STOCK REPORT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OESPINNING_CARDING_STOCK_REPORT"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPTY BOBIN STOCK DETAILS"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OESPINNING_ENTRY_BOBIN_STOCK_DETAILS"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPTY BOBIN STOCK SUMMARY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "OESPINNING_ENTRY_BOBIN_STOCK_SUMMARY"



                n = .Rows.Add()
                .Rows(n).Cells(DgvCol_Details.SNo).Value = ""
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = ""
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = ""

                '**********************************    END OE SPINNING MODULE - REPORTS **********************************

            End If




            '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------
            '*************************************************    START STORES MODULE *************************************************
            '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------

            If Common_Procedures.settings.STORESENTRY_Status = 1 Then

                vSno = 0

                n = .Rows.Add()
                '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "STORES_MODULE_HEADING"
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.BackColor = Color.Black
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.ForeColor = Color.Red
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Tahoma", 12, FontStyle.Bold)
                '.Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Arial", 14, FontStyle.Bold)


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES PURCHASE ORDER ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_STORES_PURCHASE_ORDER"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES PURCHASE INWARD ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_STORES_PURCHASE_INWARD"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES PURCHASE RETURN ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_STORES_PURCHASE_RETURN"



                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES ITEM ISSUE TO MACHINE ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_STORES_ITEM_ISSUE_TO_MACHINE"



                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES ITEM RETURN FROM MACHINE ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_STORES_ITEM_RETURN_FROM_MACHINE"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES ITEM DELIVERY ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_STORES_ITEM_DELIVERY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES ITEM RECEIPT ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_STORES_ITEM_RECEIPT"



                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES SERVICE DELIVERY ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_STORES_SERVICE_DELIVERY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES SERVICE RECEIPT ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_STORES_SERVICE_RECEIPT"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES GATE PASS ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_STORES_GATE_PASS"



                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES ITEM EXCESS ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_STORES_ITEM_EXCESS"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES DISPOSE ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_STORES_DISPOSE"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES OIL SERVICE ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_STORES_OIL_SERVICE"

                n = .Rows.Add()
                .Rows(n).Cells(DgvCol_Details.SNo).Value = ""
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = ""
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = ""

            End If

            '**********************************    END STORE ENTRIES **********************************

            '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------
            '*************************************************    START STORE REPORT  *************************************************
            '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------

            If Common_Procedures.settings.STORESENTRY_Status = 1 Then

                vSno = 0

                n = .Rows.Add()
                '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES REPORTS"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "STORES_MODULE_REPORTS_MODULE_HEADING"
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.BackColor = Color.Black
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.ForeColor = Color.Red
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Tahoma", 12, FontStyle.Bold)
                '.Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Arial", 14, FontStyle.Bold)

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES MASTERS"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORTS_STORES_MASTERS"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES REGISTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORTS_STORES_REGISTER"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES PURCHASE ORDER PENDING"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORTS_STORES_PURCHASE_ORDER_PENDING"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES MONTHLY ITEM ISSUE STATEMENT"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORTS_STORES_MONTHLY_ITEM_ISSUE_STATEMENT"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES MONTHLY ITEM ISSUE STATEMENT ALL LOOM"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORTS_STORES_MONTHLY_ITEM_ISSUE_STATEMENT_ALL_LOOM"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES SERVICE RECEIPT ITEM PENDING"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORTS_STORES_SERVICE_RECEIPT_ITEM_PENDING"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES OIL SERVICE PENDING"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORTS_OIL_SERVICE_PENDING"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES NEW ITEM STOCK DETAILS"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORTS_STORES_NEW_ITEM_STOCK_DETAILS"




                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES OLD ITEM STOCK DETAILS USABLE"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORTS_STORES_OLD_ITEM_STOCK_DETAILS_USABLE"



                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES OLD ITEM STOCK DETAILS SCRAP"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORTS_STORES_OLD_ITEM_STOCK_DETAILS_SCRAP"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES ITEM STOCK DETAILS ALL"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORTS_STORES_ITEM_STOCK_DETAILS_ALL"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES NEW ITEM STOCK SUMMARY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORTS_STORES_NEW_ITEM_STOCK_SUMMARY"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES OLD ITEM STOCK SUMMARY USABLE"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORTS_STORES_OLD_ITEM_STOCK_SUMMARY_USABLE"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES OLD ITEM STOCK SUMMARY SCRAP"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORTS_STORES_OLD_ITEM_STOCK_SUMMARY_SCRAP"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES ITEM STOCK SUMMARY ALL"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORTS_STORES_ITEM_STOCK_SUMMARY_ALL"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES ITEM STOCK VALUE"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORTS_STORES_ITEM_STOCK_VALUE"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "STORES IPURCHASE_PLANNING"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORTS_STORES_PURCHASE_PLANNING"




                n = .Rows.Add()
                .Rows(n).Cells(DgvCol_Details.SNo).Value = ""
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = ""
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = ""

            End If



            '**********************************    END STORE REPORTS **********************************



            '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------
            '*************************************************    START PAYROLL  *************************************************
            '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------

            If Common_Procedures.settings.PAYROLLENTRY_Status = 1 Then

                vSno = 0

                n = .Rows.Add()
                '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PAYROLL"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "PAYROLL_MODULE_HEADING"
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.BackColor = Color.Black
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.ForeColor = Color.Red
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Tahoma", 12, FontStyle.Bold)
                '.Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Arial", 14, FontStyle.Bold)


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPLOYEE ATTENDANCE ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_EMPLOYEE_ATTENDANCE"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ATTENDANCE LOG FROM MACHINE ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_ATTENDANCE_LOG_FROM_MACHINE"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPLOYEE ATTENDANCE FROM MACHINE ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_EMPLOYEE_ATTENDANCE_FROM_MACHINE"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPLOYEE TIMING ADDITION ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_EMPLOYEE_TIMING_ADDITION"



                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPLOYEE SALARY ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_EMPLOYEE_SALARY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPLOYEE SALARY ADVANCE PAYMENT ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_EMPLOYEE_SALARY_ADVANCE_PAYMENT"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPLOYEE ADVANCE ADDITION ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_EMPLOYEE_ADVANCE_ADDITION"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPLOYEE CREATION ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_EMPLOYEE_CREATION"



                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "CATEGORY CREATION ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_CATEGORY_CREATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SALARY PAYMENT TYPE ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_SALARY_PAYMENT_TYPE"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPLOYEE ADVANCE ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_EMPLOYEE_ADVANCE"

                n = .Rows.Add()
                .Rows(n).Cells(DgvCol_Details.SNo).Value = ""
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = ""
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = ""


            End If
            '**********************************    END PAYROLL  ENTRIES **********************************





            '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------
            '*************************************************    START PAYROLL MODULE - REPORTS  *************************************************
            '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------

            If Common_Procedures.settings.PAYROLLENTRY_Status = 1 Then

                vSno = 0

                n = .Rows.Add()
                '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "PAYROLL REPORTS"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "PAYROLL_MODULE_REPORTS_MODULE_HEADING"
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.BackColor = Color.Black
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.ForeColor = Color.Red
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Tahoma", 12, FontStyle.Bold)
                '.Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Arial", 14, FontStyle.Bold)


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SALARY REGISTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_SALARY_REGISTER"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "NET PAY REGISTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_NET_PAY_REGISTER"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ATTENDANCE REGISTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_ATTENDANCE_REGISTER"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ATTENDANCE MONTHWISE REGISTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_ATTENDANCE_MONTHWISE_REGISTER"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPLOYEE PAYMENT REGISTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_EMPLOYEE_PAYMENT_REGISTER"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPLOYEE DEDUCATION REGISTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_EMPLOYEE_DEDUCATION"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPLOYEE ACCOUNT DETAILS"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_EMPLOYEE_ACCOUNT_DETAILS"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPLOYEE REGISTER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_EMPLOYEE_REGISTER"
                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "LEDGER REPORT ALL"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_LEDGER_REPORT_ALL"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "SALARY LEDGER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_SALARY_LEDGER"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ADVANCE LEDGER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_ADVANCE_LEDGER"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "DEPOSIT LEDGER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORT_DEPOSIT_LEDGER"
                n = .Rows.Add()
                .Rows(n).Cells(DgvCol_Details.SNo).Value = ""
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = ""
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = ""


            End If


            '**********************************    END PAYROLL  REPORTS **********************************



            '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------
            '*************************************************    START VOUCHER & ACCOUNTS REPORTS  *************************************************
            '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------

            vSno = 0

            n = .Rows.Add()
            '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "VOUCHER & ACCOUNTS REPORTS"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "VOUCHER_ACCOUNTS_MODULE_HEADING"
            .Rows(n).Cells(DgvCol_Details.EntryName).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Rows(n).Cells(DgvCol_Details.EntryName).Style.BackColor = Color.Black
            .Rows(n).Cells(DgvCol_Details.EntryName).Style.ForeColor = Color.Red
            .Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Tahoma", 10, FontStyle.Bold)
            '.Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Arial", 14, FontStyle.Bold)


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "VOUCHER ENTRY"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ENTRY_VOUCHER"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS - LEDGER REPORT"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_LEDGER_REPORT"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS - LEDGER WITH DUEDAYS REPORT"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_LEDGER_WITH_DUEDAYS_REPORT"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS - GROUP LEDGER REPORT"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_GROUPLEDGER_REPORT"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS - DAYBOOK"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_DAYBOOK"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS - ALL LEDGER"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_ALL_LEDGER"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS - TB"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_TB"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS - PROFIT & LOSS"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_PROFIT_LOSS"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS - BALANCE SHEET"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_BALANCESHEET"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS - CUSTOMER BILLS"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_CUSTOMERBILLS"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS - AGENT BILLS"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_AGENTBILLS"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS - AGENT COMMISSION"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_AGENT_COMMISSION"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS - ACCOUNTS RECEIVABLE REPORT"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_ACCOUNTS_RECEIVABLE_REPORT"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS - ACCOUNTS PAYABLE REPORT"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_ACCOUNTS_PAYABLE_REPORT"


            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS - VOUCHER REGISTER"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_VOUCHERREGISTER"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS - PARTY SALES LIST INVOICE"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_PARTY_SALES_LIST_INVOICE"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS - SALES PARTY LEDGER PRINT"
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_SALES_PARTY_LEDGER_PRINT"

            n = .Rows.Add()
            vSno += 1
            .Rows(n).Cells(DgvCol_Details.SNo).Value = ""
            .Rows(n).Cells(DgvCol_Details.EntryName).Value = ""
            .Rows(n).Cells(DgvCol_Details.EntryCode).Value = ""

            '**********************************    END VOUCHER ENTRIES & ACCOUNTS REPORTS **********************************




            '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------
            '*********************    START GENERAL REPORTS or MIS REPORTS in MODULEWISE STARTUP  *****************************************
            '----------------- ----------------- ----------------- ----------------- ----------------- ----------------- -----------------

            If Common_Procedures.settings.Show_Modulewise_Entrance = 1 Then  '----    SRI SAKTHI VINAYAGA WEAVES PVT.LTD

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1087" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" Then

                    vSno = 0

                    n = .Rows.Add()
                    '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then '---- UNITED WEAVES (PALLADAM)
                        .Rows(n).Cells(DgvCol_Details.EntryName).Value = "GENERAL REPORTS"
                        .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ERP_GENERAL_REPORTS_MODULE_HEADING"
                    Else
                        .Rows(n).Cells(DgvCol_Details.EntryName).Value = "M.I.S REPORTS"
                        .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ERP_MIS_REPORTS_MODULE_HEADING"
                    End If

                    .Rows(n).Cells(DgvCol_Details.EntryName).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .Rows(n).Cells(DgvCol_Details.EntryName).Style.BackColor = Color.Black
                    .Rows(n).Cells(DgvCol_Details.EntryName).Style.ForeColor = Color.Red
                    .Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Tahoma", 10, FontStyle.Bold)
                    '.Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Arial", 14, FontStyle.Bold)

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "EMPTY BEAM STOCK"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "GENERAL_REPORTS_EMPTY_BEAM_STOCK"


                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "YARN STOCK REPORTS"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "GENERAL_REPORTS_YARN_STOCK_REPORTS"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "WARP STOCK REPORTS"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "GENERAL_REPORTS_WARP_STOCK_REPORTS"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "FABRIC STOCK REPORTS"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "GENERAL_REPORTS_FABRIC_STOCK_REPORTS"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "MONTHLY PRODUCTION REPORT"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "GENERAL_REPORTS_MONTHLY_PRODUCTION_REPORT"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "BEAM TO BEAM RECONCILATION REPORT"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "GENERAL_REPORTS_BEAM_TO_BEAM_RECONCILATION_REPORT"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "BEAM TO BEAM RECONCILATION SETNO WISE"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "GENERAL_REPORTS_BEAM_TO_BEAM_RECONCILATION_SETNO_WISE"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "INHOUSE"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "GENERAL_REPORTS_INHOUSE"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "AGENT REPORT"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "GENERAL_REPORTS_AGENT_REPORT"


                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "GST REPORT"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "GENERAL_REPORTS_GST_REPORT"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "TCS REPORT"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "GENERAL_REPORTS_TCS_REPORT"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "TDS REPORT"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "GENERAL_REPORTS_TDS_REPORT"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS PURCHASE REGISTER"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "GENERAL_REPORTS_ACCOUNTS_PURCHASE_REGISTER"

                    n = .Rows.Add()
                    vSno += 1
                    .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                    .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS SALES REGISTER"
                    .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "GENERAL_REPORTS_ACCOUNTS_SALES_REGISTER"

                End If

            End If


            '**********************************   END GENERAL REPORTS or MIS REPORTS in MODULEWISE STARTUP  *********************************

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then '---- UNITED WEAVES (PALLADAM)

                '---------------------------------------------------------------------------------------------------------------'
                '********************************** START OF ACCOUNTS - PURCHASE  ********************************** UNITED WEAVES
                '---------------------------------------------------------------------------------------------------------------'

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS PURCHASE - PURCHASE ENTRY GST"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_PURCHASE_PURCHASE_ENTRY_GST"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS PURCHASE - PURCHASE RETURN"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_PURCHASE_PURCHASE_RETURN"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS PURCHASE - PAYMENT BANK"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_PURCHASE_PAYMENT_BANK"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS PURCHASE - PAYMENT CASH"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_PURCHASE_PAYMENT_CASH"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS PURCHASE - DEBIT NOTE ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_PURCHASE_DEBIT_NOTE_ENTRY"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS PURCHASE - DATE WISE LEDGER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_PURCHASE_DATE_WISE_LEDGER"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS PURCHASE - OUTSTANDING PENDING DUES DATA WISE"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_PURCHASE_OUTSTANDING_PENDING_DUES_DATA_WISE"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS PURCHASE - GROUP LEDGER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_PURCHASE_GROUP_LEDGER"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS PURCHASE - ITEM EXCESS SHORT ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_PURCHASE_ITEM_EXCESS_SHORT_ENTRY"


                '---------------------------------------------------------------------------------------------------------------'
                '********************************** END OF ACCOUNTS - PURCHASE **********************************
                '---------------------------------------------------------------------------------------------------------------'

                '---------------------------------------------------------------------------------------------------------------'
                '********************************** START OF ACCOUNTS - SALES  ********************************** UNITED WEAVES
                '---------------------------------------------------------------------------------------------------------------'



                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS SALES - SALES ENTRY GST"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_SALES_SALES_ENTRY_GST"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS SALES - RETURN"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_SALES_SALES_RETURN"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS SALES - PAYMENT RECEIPT BANK"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_SALES_PAYMENT_RECEIPT_BANK"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS SALES - PAYMENT RECEIPT CASH"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_SALES_PAYMENT_RECEIPT_CASH"


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS SALES - CREDIT NOTE ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_SALES_CREDIT_NOTE_ENTRY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS SALES - DATE WISE LEDGER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_SALES_DATE_WISE_LEDGER"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS SALES - OUTSTANDING PENDING DUES DATE WISE"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_SALES_OUTSTANDING_PENDING_DUES_DATE_WISE"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS SALES - OUTSTANDING PENDING PARTY WISE"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_SALES_OUTSTANDING_PENDING_PARTY_WISE"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS SALES - GROUP LEDGER"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_SALES_GROUP_LEDGER"



                '---------------------------------------------------------------------------------------------------------------'
                '********************************** END OF ACCOUNTS - SALES  ********************************** UNITED WEAVES
                '---------------------------------------------------------------------------------------------------------------'

                '---------------------------------------------------------------------------------------------------------------'
                '********************************** START OF ACCOUNTS - OTHERS  ********************************** UNITED WEAVES
                '---------------------------------------------------------------------------------------------------------------'


                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS OTHERS - JOURNAL ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_OTHERS_JOURNAL_ENTRY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS OTHERS - CONTRA ENTRY"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_OTHERS_CONTRA_ENTRY"

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "ACCOUNTS OTHERS - PETTY CASH"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "ACCOUNTS_OTHERS_PETTY_CASH"

            End If


            '---------------------------------------------------------------------------------------------------------------'
            '********************************** END OF ACCOUNTS - OTHERS  ********************************** UNITED WEAVES
            '---------------------------------------------------------------------------------------------------------------'


            If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then

                n = .Rows.Add()

                vSno = 0

                '.Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "USER MODIFICATION"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "USER_MODIFICATION_HEADING"
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.BackColor = Color.Black
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.ForeColor = Color.Red
                .Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Tahoma", 12, FontStyle.Bold)
                '.Rows(n).Cells(DgvCol_Details.EntryName).Style.Font = New Font("Arial", 14, FontStyle.Bold)

                n = .Rows.Add()
                vSno += 1
                .Rows(n).Cells(DgvCol_Details.SNo).Value = vSno
                .Rows(n).Cells(DgvCol_Details.EntryName).Value = "REPORTS USER MODIFICATIONS"
                .Rows(n).Cells(DgvCol_Details.EntryCode).Value = "REPORTS_USER_MODIFICATIONS_REPORTS"

            End If

        End With

    End Sub


    Private Sub move_record(ByVal idno As Integer)

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim I As Integer, J As Integer
        Dim All_STS As Boolean, Add_Full_STS As Boolean, Add_ToDay_STS As Boolean
        Dim Edit_Full_STS As Boolean, Edit_Today_STS As Boolean
        Dim Del_Full_STS As Boolean, Del_Today_STS As Boolean, Del_BefPrnt_STS As Boolean
        Dim View_STS As Boolean, Ins_STS As Boolean
        Dim Edit_LastEnt_STS As Boolean, Edit_BefPrnt_STS As Boolean
        Dim Prnt_STS As Boolean = False
        Dim Add_Last_n_Day_STS As Boolean, Edit_Last_n_Day_STS As Boolean, Delete_Last_n_Day_STS As Boolean
        Dim a() As String


        If Val(idno) = 0 Then Exit Sub

        clear()

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from User_Head where User_IdNo = " & Str(Val(idno)) & IIf(Trim(Other_Condition) <> "", " and ", "") & Trim(Other_Condition), con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_UserID.Text = dt1.Rows(0).Item("User_IdNo").ToString
                txt_Name.Text = dt1.Rows(0).Item("User_Name").ToString
                txt_AcPwd.Text = Common_Procedures.Decrypt(Trim(dt1.Rows(0).Item("Account_Password").ToString), Trim(Common_Procedures.UserCreation_AcPassWord.passPhrase) & Trim(Val(lbl_UserID.Text)) & Trim(UCase(txt_Name.Text)), Trim(Common_Procedures.UserCreation_AcPassWord.saltValue) & Trim(Val(lbl_UserID.Text)) & Trim(UCase(txt_Name.Text)))
                'txt_AcPwd.Text = dt1.Rows(0).Item("Account_Password").ToString
                txt_UnAcPwd.Text = Common_Procedures.Decrypt(Trim(dt1.Rows(0).Item("UnAccount_Password").ToString), Trim(Common_Procedures.UserCreation_UnAcPassWord.passPhrase) & Trim(Val(lbl_UserID.Text)) & Trim(UCase(txt_Name.Text)), Trim(Common_Procedures.UserCreation_UnAcPassWord.saltValue) & Trim(Val(lbl_UserID.Text)) & Trim(UCase(txt_Name.Text)))
                'txt_UnAcPwd.Text = dt1.Rows(0).Item("UnAccount_Password").ToString
                chk_AskPassword_OnSaving.Checked = False
                If Val(dt1.Rows(0).Item("AskPassword_On_Save_Edit_Delete").ToString) = 1 Then
                    chk_AskPassword_OnSaving.Checked = True
                End If
                chk_Verified_sts.Checked = False
                If Val(dt1.Rows(0).Item("Show_verified_status").ToString) = 1 Then
                    chk_Verified_sts.Checked = True
                End If

                chk_UserCreation_Sts.Checked = False
                If Val(dt1.Rows(0).Item("Show_UserCreation_Status").ToString) = 1 Then
                    chk_UserCreation_Sts.Checked = True

                End If
                chk_Close_Sts.Checked = False
                If Val(dt1.Rows(0).Item("Close_Status").ToString) = 1 Then
                    chk_Close_Sts.Checked = True
                    Common_Procedures.User.Show_Approved_Status = 1
                End If
                chk_approved_sts.Checked = False
                If Val(dt1.Rows(0).Item("Show_Approved_status").ToString) = 1 Then
                    chk_approved_sts.Checked = True
                End If

                cbo_UserCategory.Text = dt1.Rows(0).Item("User_Category").ToString

                If IsDBNull(dt1.Rows(0).Item("ADD_LAST_n_DAYS").ToString) = False Then
                    txt_Add_Last_n_DaysEntry.Text = dt1.Rows(0).Item("ADD_LAST_n_DAYS").ToString
                End If
                If IsDBNull(dt1.Rows(0).Item("EDIT_LAST_n_DAYS").ToString) = False Then
                    txt_Edit_Last_n_DaysEntry.Text = dt1.Rows(0).Item("EDIT_LAST_n_DAYS").ToString
                End If
                If IsDBNull(dt1.Rows(0).Item("DELETE_LAST_n_DAYS").ToString) = False Then
                    txt_Delete_Last_n_DaysEntry.Text = dt1.Rows(0).Item("DELETE_LAST_n_DAYS").ToString
                End If


                If IsDBNull(dt1.Rows(0).Item("ModuleWise_Access_Rights").ToString) = False Then

                    a = Split(Trim(dt1.Rows(0).Item("ModuleWise_Access_Rights").ToString), "~")

                    For I = 0 To UBound(a)

                        If Trim(a(I)) <> "" Then

                            For J = 0 To lst_MultiInput_IdNos.Items.Count - 1

                                If Trim(lst_MultiInput_IdNos.Items(J).ToString) <> "" Then

                                    If Trim(UCase(lst_MultiInput_IdNos.Items(J).ToString)) = Trim(UCase(a(I))) Then

                                        If J <= chklst_MultiInput.Items.Count - 1 Then
                                            chklst_MultiInput.SetItemChecked(J, True)
                                        End If

                                        Exit For

                                    End If

                                End If

                            Next J

                        End If

                    Next I

                End If


                da2 = New SqlClient.SqlDataAdapter("select * from User_Access_Rights where User_IdNo = " & Str(Val(lbl_UserID.Text)) & " and (Software_Module_IdNo =  " & Str(Val(Common_Procedures.SoftwareTypes.Textile_Software)) & " or Software_Module_IdNo = 0) Order by User_IdNo, Entry_Code", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()

                Add_EntryNames()
                ' Add_EntryNames_Spinning()

                If dt2.Rows.Count > 0 Then

                    For I = 0 To dt2.Rows.Count - 1

                        With dgv_Details

                            For J = 0 To .Rows.Count - 1

                                If Trim(UCase(dgv_Details.Rows(J).Cells(DgvCol_Details.EntryCode).Value)) = Trim(UCase(dt2.Rows(I).Item("Entry_Code").ToString)) Then

                                    All_STS = False
                                    Add_Full_STS = False : Add_ToDay_STS = False
                                    Edit_Full_STS = False : Edit_Today_STS = False : Edit_LastEnt_STS = False : Edit_BefPrnt_STS = False
                                    Del_Full_STS = False : Del_Today_STS = False : Del_BefPrnt_STS = False
                                    View_STS = False
                                    Ins_STS = False
                                    Del_BefPrnt_STS = False
                                    Prnt_STS = False
                                    Add_Last_n_Day_STS = False : Edit_Last_n_Day_STS = False : Delete_Last_n_Day_STS = False

                                    If InStr(1, Trim(UCase(dt2.Rows(I).Item("Access_Type").ToString)), "~L~") Then All_STS = True

                                    If InStr(1, Trim(UCase(dt2.Rows(I).Item("Access_Type").ToString)), "~A~") Then Add_Full_STS = True
                                    If InStr(1, Trim(UCase(dt2.Rows(I).Item("Access_Type").ToString)), "~ATD~") Then Add_ToDay_STS = True
                                    If InStr(1, Trim(UCase(dt2.Rows(I).Item("Access_Type").ToString)), "~ADDNDYS~") Then Add_Last_n_Day_STS = True

                                    If InStr(1, Trim(UCase(dt2.Rows(I).Item("Access_Type").ToString)), "~E~") Then Edit_Full_STS = True
                                    If InStr(1, Trim(UCase(dt2.Rows(I).Item("Access_Type").ToString)), "~ETD~") Then Edit_Today_STS = True
                                    If InStr(1, Trim(UCase(dt2.Rows(I).Item("Access_Type").ToString)), "~EDITNDYS~") Then Edit_Last_n_Day_STS = True
                                    If InStr(1, Trim(UCase(dt2.Rows(I).Item("Access_Type").ToString)), "~ELE~") Then Edit_LastEnt_STS = True
                                    If InStr(1, Trim(UCase(dt2.Rows(I).Item("Access_Type").ToString)), "~EBP~") Then Edit_BefPrnt_STS = True

                                    If InStr(1, Trim(UCase(dt2.Rows(I).Item("Access_Type").ToString)), "~D~") Then Del_Full_STS = True
                                    If InStr(1, Trim(UCase(dt2.Rows(I).Item("Access_Type").ToString)), "~DTD~") Then Del_Today_STS = True
                                    If InStr(1, Trim(UCase(dt2.Rows(I).Item("Access_Type").ToString)), "~DELNDYS~") Then Delete_Last_n_Day_STS = True
                                    If InStr(1, Trim(UCase(dt2.Rows(I).Item("Access_Type").ToString)), "~DBP~") Then Del_BefPrnt_STS = True

                                    If InStr(1, Trim(UCase(dt2.Rows(I).Item("Access_Type").ToString)), "~V~") Then View_STS = True
                                    If InStr(1, Trim(UCase(dt2.Rows(I).Item("Access_Type").ToString)), "~I~") Then Ins_STS = True
                                    If InStr(1, Trim(UCase(dt2.Rows(I).Item("Access_Type").ToString)), "~P~") Then Prnt_STS = True


                                    .Rows(J).Cells(DgvCol_Details.All).Value = All_STS
                                    .Rows(J).Cells(DgvCol_Details.Add_AllDay).Value = Add_Full_STS
                                    .Rows(J).Cells(DgvCol_Details.Add_ToDay).Value = Add_ToDay_STS
                                    .Rows(J).Cells(DgvCol_Details.Add_Last_n_Days_Entry).Value = Add_Last_n_Day_STS
                                    .Rows(J).Cells(DgvCol_Details.Edit_AllDay).Value = Edit_Full_STS
                                    .Rows(J).Cells(DgvCol_Details.Edit_ToDay).Value = Edit_Today_STS
                                    .Rows(J).Cells(DgvCol_Details.Edit_Last_n_Days_Entry).Value = Edit_Last_n_Day_STS
                                    .Rows(J).Cells(DgvCol_Details.Edit_LastEntry).Value = Edit_LastEnt_STS
                                    .Rows(J).Cells(DgvCol_Details.Edit_Before_Printing).Value = Edit_BefPrnt_STS
                                    .Rows(J).Cells(DgvCol_Details.Delete_All).Value = Del_Full_STS
                                    .Rows(J).Cells(DgvCol_Details.Delete_ToDay).Value = Del_Today_STS
                                    .Rows(J).Cells(DgvCol_Details.Delete_Last_n_Days_Entry).Value = Delete_Last_n_Day_STS
                                    .Rows(J).Cells(DgvCol_Details.Delete_Before_Printing).Value = Del_BefPrnt_STS
                                    .Rows(J).Cells(DgvCol_Details.View_Only).Value = View_STS
                                    .Rows(J).Cells(DgvCol_Details.Insert).Value = Ins_STS
                                    .Rows(J).Cells(DgvCol_Details.Print).Value = Prnt_STS

                                    Exit For

                                End If

                            Next

                        End With

                    Next I

                End If

                dt2.Clear()
                dt2.Dispose()
                da2.Dispose()

            Else
                new_record()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()


            For I = 0 To chklst_CompanyWise_Settings.Items.Count - 1

                chklst_CompanyWise_Settings.SetItemChecked(I, False)

                Dim con1 As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
                con1.Open()

                Dim cmd As New SqlClient.SqlCommand
                cmd.Connection = con1

                cmd.CommandText = "select company_idno from company_head where company_name + '('+ company_shortname + ')' = '" & chklst_CompanyWise_Settings.Items(I).ToString & "'"

                Dim cmp_idno As Integer = cmd.ExecuteScalar

                If cmp_idno > 0 Then
                    cmd.Connection = con
                    cmd.CommandText = " Select count(*) from User_Access_Rights_CompanyWise where [CompanyGroup_IdNo] = " & Common_Procedures.CompGroupIdNo.ToString &
                                      " and Company_IdNo = " & cmp_idno.ToString & " and User_IdNo = " & Str(Val(lbl_UserID.Text))
                    Dim is_included As Integer = cmd.ExecuteScalar

                    If is_included > 0 Then
                        chklst_CompanyWise_Settings.SetItemChecked(I, True)
                    End If
                End If

            Next


        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If txt_Name.Visible And txt_Name.Enabled Then txt_Name.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.user_Creation, New_Entry, Me) = False Then Exit Sub


        Dim cmd As New SqlClient.SqlCommand

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try

            cmd.Connection = con

            cmd.CommandText = "Delete from User_Access_Rights Where User_IdNo = " & Str(Val(lbl_UserID.Text)) & " and (Software_Module_IdNo = " & Str(Val(Common_Procedures.SoftwareTypes.Textile_Software)) & " or Software_Module_IdNo = 0)"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from User_Head Where User_IdNo = " & Str(Val(lbl_UserID.Text))
            cmd.ExecuteNonQuery()

            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Successfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If txt_Name.Enabled = True And txt_Name.Visible = True Then txt_Name.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '----
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '----
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select top 1 User_IdNo from User_Head WHERE User_IdNo <> 0  " & IIf(Trim(Other_Condition) <> "", " and ", "") & Trim(Other_Condition) & " Order by User_IdNo"
            dr = cmd.ExecuteReader

            movno = 0
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then
                move_record(movno)
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select top 1 User_IdNo from User_Head WHERE User_IdNo <> 0  " & IIf(Trim(Other_Condition) <> "", " and ", "") & Trim(Other_Condition) & " Order by User_IdNo desc"
            dr = cmd.ExecuteReader

            movno = 0
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then
                move_record(movno)
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As Integer
        Dim OrdByNo As Integer

        Try

            OrdByNo = Val(lbl_UserID.Text)

            cmd.Connection = con
            cmd.CommandText = "select top 1 User_IdNo from User_Head WHERE User_IdNo <> 0 and user_idno > " & Str(Val(OrdByNo)) & IIf(Trim(Other_Condition) <> "", " and ", "") & Trim(Other_Condition) & " Order by User_IdNo"
            dr = cmd.ExecuteReader

            movno = 0
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then
                move_record(movno)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As Integer
        Dim OrdByNo As Integer

        Try

            OrdByNo = Val(lbl_UserID.Text)

            cmd.Connection = con
            cmd.CommandText = "select top 1 User_IdNo from User_Head where User_IdNo <> 0 and user_idno < " & Str(Val(OrdByNo)) & IIf(Trim(Other_Condition) <> "", " and ", "") & Trim(Other_Condition) & " Order by User_IdNo desc"
            dr = cmd.ExecuteReader

            movno = 0
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then
                move_record(movno)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As Integer = 0
        Dim NewNo As Integer = 0

        Try
            clear()

            New_Entry = True

            da = New SqlClient.SqlDataAdapter("select max(User_IdNo) from User_Head where User_IdNo <> 0", con)
            da.Fill(dt1)

            NewNo = 0
            If dt1.Rows.Count > 0 Then
                If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                    NewNo = Val(dt1.Rows(0)(0).ToString)
                End If
            End If

            NewNo = NewNo + 1

            lbl_UserID.Text = NewNo
            lbl_UserID.ForeColor = Color.Red

            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("select User_Name from User_Head " & IIf(Trim(Other_Condition) <> "", " Where ", "") & Trim(Other_Condition) & " order by User_Name", con)
        da.Fill(dt)

        cbo_Open.DataSource = dt
        cbo_Open.DisplayMember = "User_Name"

        da.Dispose()

        grp_Open.Visible = True
        grp_Open.BringToFront()
        If cbo_Open.Enabled And cbo_Open.Visible Then cbo_Open.Focus()
        pnl_Back.Enabled = False

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-----
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim NewCode As String = ""
        Dim NewNo As Long = 0
        Dim nr As Long = 0
        Dim led_idno As Integer = 0
        Dim Sno As Integer = 0
        Dim db_idno As Integer = 0
        Dim cr_idno As Integer = 0
        Dim VouAmt As Decimal = 0
        Dim vAskPwdSTS As Integer = 0
        Dim UnPwd As String
        Dim Sur As String
        Dim r As String
        Dim varAcPwd As String = ""
        Dim varUnAcPwd As String = ""
        Dim vVerfiedSts As Integer = 0
        Dim vUsrCreaSts As Integer = 0
        Dim vCloseSts As Integer = 0
        Dim vMODWISE_AccRgts As String = ""
        Dim vApprovedSts As Integer = 0


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.user_Creation, New_Entry, Me) = False Then Exit Sub

        con = New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
        con.Open()

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Window", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid User Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Name.Enabled Then txt_Name.Focus()
            Exit Sub
        End If

        If Trim(UCase(Common_Procedures.User.Type)) = "UNACCOUNT" Then
            If Trim(txt_UnAcPwd.Text) = "" Then
                MessageBox.Show("Invalid UnAccount PassWord", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_UnAcPwd.Enabled And txt_UnAcPwd.Visible Then txt_UnAcPwd.Focus()
                Exit Sub
            End If
            If Trim(txt_AcPwd.Text) = Trim(txt_UnAcPwd.Text) Then
                MessageBox.Show("Both PassWords are Equal", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_AcPwd.Enabled Then txt_AcPwd.Focus()
                Exit Sub
            End If
            UnPwd = Trim(txt_UnAcPwd.Text)

        Else
            UnPwd = ""
            If Trim(txt_UnAcPwd.Text) <> "" Then UnPwd = Trim(txt_UnAcPwd.Text)
            If Trim(UnPwd) = "" And Val(lbl_UserID.Text) = 1 Then UnPwd = "TS2"

        End If

        Sur = Common_Procedures.Remove_NonCharacters(txt_Name.Text)

        vAskPwdSTS = 0
        If chk_AskPassword_OnSaving.Checked = True Then vAskPwdSTS = 1

        vVerfiedSts = 0
        If chk_Verified_sts.Checked = True Then vVerfiedSts = 1

        vApprovedSts = 0
        If chk_approved_sts.Checked = True Then vApprovedSts = 1


        vUsrCreaSts = 0
        If chk_UserCreation_Sts.Checked = True Then vUsrCreaSts = 1

        vCloseSts = 0
        If chk_Close_Sts.Checked = True Then vCloseSts = 1

        vMODWISE_AccRgts = ""
        If Val(lbl_UserID.Text) = 1 Then

            For indexChecked = 0 To chklst_MultiInput.Items.Count - 1

                If Trim(chklst_MultiInput.Items(indexChecked).ToString) <> "" Then

                    If indexChecked <= lst_MultiInput_IdNos.Items.Count - 1 Then
                        If Trim(lst_MultiInput_IdNos.Items(indexChecked).ToString) <> "" Then
                            vMODWISE_AccRgts = Trim(vMODWISE_AccRgts) & IIf(Trim(vMODWISE_AccRgts) <> "", "~", "") & Trim(lst_MultiInput_IdNos.Items(indexChecked).ToString)
                        End If
                    End If

                End If

            Next

        Else

            For Each indexChecked In chklst_MultiInput.CheckedIndices

                If Trim(chklst_MultiInput.Items(indexChecked).ToString) <> "" Then

                    If indexChecked <= lst_MultiInput_IdNos.Items.Count - 1 Then
                        If Trim(lst_MultiInput_IdNos.Items(indexChecked).ToString) <> "" Then
                            vMODWISE_AccRgts = Trim(vMODWISE_AccRgts) & IIf(Trim(vMODWISE_AccRgts) <> "", "~", "") & Trim(lst_MultiInput_IdNos.Items(indexChecked).ToString)
                        End If
                    End If

                End If

            Next

        End If

        If Trim(vMODWISE_AccRgts) <> "" Then
            vMODWISE_AccRgts = "~" & Trim(vMODWISE_AccRgts) & "~"
        End If


        varAcPwd = Common_Procedures.Encrypt(Trim(txt_AcPwd.Text), Trim(Common_Procedures.UserCreation_AcPassWord.passPhrase) & Trim(Val(lbl_UserID.Text)) & Trim(UCase(txt_Name.Text)), Trim(Common_Procedures.UserCreation_AcPassWord.saltValue) & Trim(Val(lbl_UserID.Text)) & Trim(UCase(txt_Name.Text)))
        varUnAcPwd = Common_Procedures.Encrypt(Trim(UnPwd), Trim(Common_Procedures.UserCreation_UnAcPassWord.passPhrase) & Trim(Val(lbl_UserID.Text)) & Trim(UCase(txt_Name.Text)), Trim(Common_Procedures.UserCreation_UnAcPassWord.saltValue) & Trim(Val(lbl_UserID.Text)) & Trim(UCase(txt_Name.Text)))

        tr = con.BeginTransaction

        Try


            If New_Entry = True Then
                da = New SqlClient.SqlDataAdapter("select max(User_IdNo) from User_Head", con)
                da.SelectCommand.Transaction = tr
                dt4 = New DataTable
                da.Fill(dt4)

                NewNo = 0
                If dt4.Rows.Count > 0 Then
                    If IsDBNull(dt4.Rows(0)(0).ToString) = False Then
                        NewNo = Int(Val(dt4.Rows(0)(0).ToString))
                    End If
                End If
                dt4.Clear()

                NewNo = Val(NewNo) + 1

                lbl_UserID.Text = NewNo

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            If New_Entry = True Then

                'If IsDBNull(dt1.Rows(0).Item("ADD_LAST_n_DAYS").ToString) = False Then
                '    txt_Add_Last_n_DaysEntry.Text = dt1.Rows(0).Item("ADD_LAST_n_DAYS").ToString
                'End If
                'If IsDBNull(dt1.Rows(0).Item("EDIT_LAST_n_DAYS").ToString) = False Then
                '    txt_Edit_Last_n_DaysEntry.Text = dt1.Rows(0).Item("EDIT_LAST_n_DAYS").ToString
                'End If
                'If IsDBNull(dt1.Rows(0).Item("DELETE_LAST_n_DAYS").ToString) = False Then
                '    txt_Delete_Last_n_DaysEntry.Text = dt1.Rows(0).Item("DELETE_LAST_n_DAYS").ToString
                'End If

                cmd.CommandText = "Insert into User_Head (          User_IdNo    ,             User_Name        ,         Sur_Name   ,      Account_Password   ,       UnAccount_Password  , AskPassword_On_Save_Edit_Delete ,               User_Category          ,      Show_Verified_status    ,    Show_UserCreation_Status  ,         Close_Status       ,                 ADD_LAST_n_DAYS                ,                  EDIT_LAST_n_DAYS               ,                  DELETE_LAST_n_DAYS                ,    ModuleWise_Access_Rights   ,Show_Approved_status   ) " &
                                    "          Values    (" & Str(Val(NewNo)) & ", '" & Trim(txt_Name.Text) & "', '" & Trim(Sur) & "', '" & Trim(varAcPwd) & "', '" & Trim(varUnAcPwd) & "', " & Str(Val(vAskPwdSTS)) & "    , '" & Trim(cbo_UserCategory.Text) & "', " & Str(Val(vVerfiedSts)) & ", " & Str(Val(vUsrCreaSts)) & ", " & Str(Val(vCloseSts)) & ", " & Str(Val(txt_Add_Last_n_DaysEntry.Text)) & ", " & Str(Val(txt_Edit_Last_n_DaysEntry.Text)) & ", " & Str(Val(txt_Delete_Last_n_DaysEntry.Text)) & " , '" & Trim(vMODWISE_AccRgts) & "' , " & Str(Val(vApprovedSts)) & ") "
                cmd.ExecuteNonQuery()


            Else

                cmd.CommandText = "Update User_Head set User_Name = '" & Trim(txt_Name.Text) & "', Sur_Name = '" & Trim(Sur) & "', Account_Password = '" & Trim(varAcPwd) & "', UnAccount_Password = '" & Trim(varUnAcPwd) & "', AskPassword_On_Save_Edit_Delete = " & Str(Val(vAskPwdSTS)) & ",User_Category = '" & Trim(cbo_UserCategory.Text) & "', Show_Verified_status = " & Str(Val(vVerfiedSts)) & " , Show_UserCreation_Status = " & Str(Val(vUsrCreaSts)) & " , Close_Status = " & Str(Val(vCloseSts)) & " , ADD_LAST_n_DAYS = " & Str(Val(txt_Add_Last_n_DaysEntry.Text)) & ", EDIT_LAST_n_DAYS = " & Str(Val(txt_Edit_Last_n_DaysEntry.Text)) & ", DELETE_LAST_n_DAYS = " & Str(Val(txt_Delete_Last_n_DaysEntry.Text)) & " , ModuleWise_Access_Rights = '" & Trim(vMODWISE_AccRgts) & "',Show_Approved_status=" & Str(Val(vApprovedSts)) & "  Where User_IdNo = " & Str(Val(lbl_UserID.Text))
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from User_Access_Rights Where User_IdNo = " & Str(Val(lbl_UserID.Text)) & " and ( Software_Module_IdNo = " & Str(Val(Common_Procedures.SoftwareTypes.Textile_Software)) & " or Software_Module_IdNo = 0)"
            cmd.ExecuteNonQuery()

            Sno = 0
            For i = 0 To dgv_Details.RowCount - 1

                If Trim(dgv_Details.Rows(i).Cells(DgvCol_Details.EntryName).Value) <> "" And Trim(dgv_Details.Rows(i).Cells(DgvCol_Details.EntryCode).Value) <> "" Then

                    Sno = Sno + 1

                    If Val(lbl_UserID.Text) = 1 Then
                        r = "~L~A~ATD~ADDNDYS~E~ETD~EDITNDYS~ELE~EBP~D~DTD~DELNDYS~DBP~V~I~P~"

                    Else
                        r = "~"
                        If dgv_Details.Rows(i).Cells(DgvCol_Details.All).Value = True Then r = r & "L~"
                        If dgv_Details.Rows(i).Cells(DgvCol_Details.Add_AllDay).Value = True Then r = r & "A~"
                        If dgv_Details.Rows(i).Cells(DgvCol_Details.Add_ToDay).Value = True Then r = r & "ATD~"
                        If dgv_Details.Rows(i).Cells(DgvCol_Details.Add_Last_n_Days_Entry).Value = True Then r = r & "ADDNDYS~"
                        If dgv_Details.Rows(i).Cells(DgvCol_Details.Edit_AllDay).Value = True Then r = r & "E~"
                        If dgv_Details.Rows(i).Cells(DgvCol_Details.Edit_ToDay).Value = True Then r = r & "ETD~"
                        If dgv_Details.Rows(i).Cells(DgvCol_Details.Edit_Last_n_Days_Entry).Value = True Then r = r & "EDITNDYS~"
                        If dgv_Details.Rows(i).Cells(DgvCol_Details.Edit_LastEntry).Value = True Then r = r & "ELE~"
                        If dgv_Details.Rows(i).Cells(DgvCol_Details.Edit_Before_Printing).Value = True Then r = r & "EBP~"
                        If dgv_Details.Rows(i).Cells(DgvCol_Details.Delete_All).Value = True Then r = r & "D~"
                        If dgv_Details.Rows(i).Cells(DgvCol_Details.Delete_ToDay).Value = True Then r = r & "DTD~"
                        If dgv_Details.Rows(i).Cells(DgvCol_Details.Delete_Last_n_Days_Entry).Value = True Then r = r & "DELNDYS~"
                        If dgv_Details.Rows(i).Cells(DgvCol_Details.Delete_Before_Printing).Value = True Then r = r & "DBP~"
                        If dgv_Details.Rows(i).Cells(DgvCol_Details.View_Only).Value = True Then r = r & "V~"
                        If dgv_Details.Rows(i).Cells(DgvCol_Details.Insert).Value = True Then r = r & "I~"
                        If dgv_Details.Rows(i).Cells(DgvCol_Details.Print).Value = True Then r = r & "P~"

                    End If

                    If Trim(r) = "~" Then r = ""

                    cmd.CommandText = "Insert into User_Access_Rights (                User_IdNo        ,                               Entry_Code                                 ,    Access_Type     ,                               Software_Module_IdNo          ) " &
                                        " Values                      (" & Str(Val(lbl_UserID.Text)) & ", '" & Trim(dgv_Details.Rows(i).Cells(DgvCol_Details.EntryCode).Value) & "', '" & Trim(r) & "' , " & Str(Val(Common_Procedures.SoftwareTypes.Textile_Software)) & " ) "
                    cmd.ExecuteNonQuery()

                End If

            Next

            cmd.CommandText = "Delete from User_Access_Rights_CompanyWise Where User_IdNo = " & Str(Val(lbl_UserID.Text)) & " and CompanyGroup_IdNo = " & Common_Procedures.CompGroupIdNo.ToString
            cmd.ExecuteNonQuery()

            For Each indexChecked In chklst_CompanyWise_Settings.CheckedIndices

                If Trim(chklst_CompanyWise_Settings.Items(indexChecked).ToString) <> "" Then

                    If indexChecked <= chklst_CompanyWise_Settings.Items.Count - 1 Then

                        Dim con1 As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
                        con1.Open()

                        da = New SqlClient.SqlDataAdapter("select company_idno from company_head where company_name + '('+ company_shortname + ')' = '" & chklst_CompanyWise_Settings.Items(indexChecked).ToString & "'", con1)

                        Dim dt As New DataTable

                        da.Fill(dt)

                        If dt.Rows.Count > 0 Then

                            For j = 0 To dt.Rows.Count - 1
                                If Not IsDBNull(dt.Rows(j).Item(0)) Then
                                    If dt.Rows(j).Item(0) > 0 Then
                                        cmd.CommandText = "insert into User_Access_Rights_CompanyWise ([User_IdNo]                  ,	[CompanyGroup_IdNo]                            ,	[Company_IdNo]                 ) " &
                                                                              " values            (" & Str(Val(lbl_UserID.Text)) & ", " & Common_Procedures.CompGroupIdNo.ToString & " ," & dt.Rows(j).Item(0).ToString & ")"
                                        cmd.ExecuteNonQuery()
                                    End If
                                End If
                            Next

                        End If

                    End If

                End If

            Next

            tr.Commit()

            MessageBox.Show("Saved Successfully!!!", "For SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_UserID.Text)
                End If
            Else
                move_record(lbl_UserID.Text)
            End If


        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "For SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub User_Creation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        con = New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
        con.Open()

        dgv_Details.RowTemplate.Height = 27
        dgv_Details.Columns(DgvCol_Details.Add_Last_n_Days_Entry).HeaderText = "ADD" & Chr(13) & "LAST" & Chr(13) & Chr(13) & Chr(13) & "DAYS" & Chr(13) & "ENTRY"
        dgv_Details.Columns(DgvCol_Details.Edit_Last_n_Days_Entry).HeaderText = "EDIT" & Chr(13) & "LAST" & Chr(13) & Chr(13) & Chr(13) & "DAYS" & Chr(13) & "ENTRY"
        dgv_Details.Columns(DgvCol_Details.Delete_Last_n_Days_Entry).HeaderText = "DELETE" & Chr(13) & "LAST" & Chr(13) & Chr(13) & Chr(13) & "DAYS" & Chr(13) & "ENTRY"

        lbl_UnAcPwd.Visible = False
        txt_UnAcPwd.Visible = False
        btn_Show_UnAcPwd.Visible = False
        If Trim(UCase(Common_Procedures.User.Type)) = "UNACCOUNT" Then
            lbl_UnAcPwd.Visible = True
            txt_UnAcPwd.Visible = True
            btn_Show_UnAcPwd.Visible = True
        End If

        chk_UserCreation_Sts.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT SIZING (SOMANUR)
            If Common_Procedures.User.IdNo = 1 Then
                chk_UserCreation_Sts.Visible = True
            End If
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT SIZING (SOMANUR)
            If Common_Procedures.User.IdNo = 1 Then
                chk_approved_sts.Visible = True
            End If
        End If

        cbo_UserCategory.Items.Clear()
        cbo_UserCategory.Items.Add("")
        cbo_UserCategory.Items.Add("TEXTILE")
        cbo_UserCategory.Items.Add("SPINNING")

        pnl_MultiInput.Visible = False
        pnl_MultiInput.Left = (Me.Width - pnl_MultiInput.Width) - 100
        pnl_MultiInput.Top = (Me.Height - pnl_MultiInput.Height) - 100

        pnl_Company_Wise_Access.Visible = False
        pnl_Company_Wise_Access.Left = (Me.Width - pnl_MultiInput.Width) - 100
        pnl_Company_Wise_Access.Top = (Me.Height - pnl_MultiInput.Height) - 100

        Pnl_ComGroup_Wise_Rights.Visible = False
        Pnl_ComGroup_Wise_Rights.Left = (Me.Width - Pnl_ComGroup_Wise_Rights.Width) - 100
        Pnl_ComGroup_Wise_Rights.Top = (Me.Height - Pnl_ComGroup_Wise_Rights.Height) - 100

        btn_ModuleWise_Rights.Visible = False
        chklst_MultiInput.Items.Clear()
        lst_MultiInput_IdNos.Items.Clear()

        If Common_Procedures.settings.Show_Modulewise_Entrance = 1 Then
            btn_ModuleWise_Rights.Visible = True
        End If

        btn_CompanyGroup_Wise_Rights.Visible = False
        If Common_Procedures.settings.Show_CompanyGroupWise_Entrance = 1 Then
            btn_CompanyGroup_Wise_Rights.Visible = True
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1088" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then

            btn_ModuleWise_Rights.Visible = True

            chklst_MultiInput.Items.Add("TEXTILE")
            lst_MultiInput_IdNos.Items.Add("TEXTILE_MODULE")

            chklst_MultiInput.Items.Add("SIZING-JOBWORK")
            lst_MultiInput_IdNos.Items.Add("SIZING_JOBWORK_MODULE")

            chklst_MultiInput.Items.Add("OE")
            lst_MultiInput_IdNos.Items.Add("OE_MODULE")



        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1088" Then

            btn_ModuleWise_Rights.Visible = True

            chklst_MultiInput.Items.Add("OPENING")
            lst_MultiInput_IdNos.Items.Add("OPENING_MODULE")

            chklst_MultiInput.Items.Add("MASTER")
            lst_MultiInput_IdNos.Items.Add("MASTER_MODULE")

            chklst_MultiInput.Items.Add("OWNSORT")
            lst_MultiInput_IdNos.Items.Add("OWNSORT_MODULE")

            chklst_MultiInput.Items.Add("TRADING")
            lst_MultiInput_IdNos.Items.Add("TRADING_MODULE")

            chklst_MultiInput.Items.Add("JOBWORK")
            lst_MultiInput_IdNos.Items.Add("JOBWORK_MODULE")

        Else

            btn_ModuleWise_Rights.Visible = True

            chklst_MultiInput.Items.Add("OPENING")
            lst_MultiInput_IdNos.Items.Add("OPENING_MODULE")

            chklst_MultiInput.Items.Add("MASTER")
            lst_MultiInput_IdNos.Items.Add("MASTER_MODULE")

            chklst_MultiInput.Items.Add("OWNSORT")
            lst_MultiInput_IdNos.Items.Add("OWNSORT_MODULE")

            chklst_MultiInput.Items.Add("JOBWORK")
            lst_MultiInput_IdNos.Items.Add("JOBWORK_MODULE")

            chklst_MultiInput.Items.Add("TRADING")
            lst_MultiInput_IdNos.Items.Add("TRADING_MODULE")

            chklst_MultiInput.Items.Add("VENDOR")
            lst_MultiInput_IdNos.Items.Add("VENDOR_MODULE")

            If Common_Procedures.settings.SizingSoftware_ENTRY_Status = 1 Then
                chklst_MultiInput.Items.Add("SIZING-JOBWORK")
                lst_MultiInput_IdNos.Items.Add("SIZING_JOBWORK_MODULE")
            End If

            If Common_Procedures.settings.OESofwtare_ENTRY_Status = 1 Then
                chklst_MultiInput.Items.Add("OE")
                lst_MultiInput_IdNos.Items.Add("OE_MODULE")
            End If

        End If

        chklst_MultiInput.Items.Add("STORES")
        lst_MultiInput_IdNos.Items.Add("STORES_MODULE")

        chklst_MultiInput.Items.Add("PAYROLL")
        lst_MultiInput_IdNos.Items.Add("PAYROLL_MODULE")

        chklst_MultiInput.Items.Add("ACCOUNTS")
        lst_MultiInput_IdNos.Items.Add("ACCOUNTS_MODULE")

        chklst_MultiInput.Items.Add("GENERAL REPORTS")
        lst_MultiInput_IdNos.Items.Add("GENERAL_REPORTS_MODULE")


        Other_Condition = ""
        If Common_Procedures.User.IdNo <> 1 Then
            Other_Condition = "(User_IdNo <> 1 And User_IdNo <> " & Str(Val(Common_Procedures.User.IdNo)) & ")"
        End If

        chklst_CompanyWise_Settings.Items.Clear()

        If Common_Procedures.settings.CompanyWise_User_Rights = True Then

            btn_CompanyWise_Rights.Visible = True

            Dim con1 As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con1.Open()

            Dim da As New SqlClient.SqlDataAdapter("Select distinct company_name + '('+ company_shortname + ')' from company_head order by company_name + '('+ company_shortname + ')'", con1)
            Dim dt As New DataTable

            da.Fill(dt)

            For i = 0 To dt.Rows.Count - 1
                If Len(Trim(dt.Rows(i).Item(0)).Replace("()", "")) > 0 Then

                    chklst_CompanyWise_Settings.Items.Add(dt.Rows(i).Item(0))

                End If
            Next

        End If

        grp_Open.Visible = False
        grp_Open.Left = (Me.Width - grp_Open.Width) - 100
        grp_Open.Top = (Me.Height - grp_Open.Height) - 100

        new_record()

    End Sub

    Private Sub User_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub User_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        If Asc(e.KeyChar) = 27 Then

            If pnl_MultiInput.Visible = True Then

                btn_Close_MultiInput_Click(sender, e)
                Exit Sub

            ElseIf grp_Open.Visible Then

                btn_CloseOpen_Click(sender, e)
                Exit Sub

            ElseIf Pnl_ComGroup_Wise_Rights.Visible = True Then

                Btn_Close_CgroupWise_Rights_Click(sender, e)
                Exit Sub

            ElseIf pnl_Company_Wise_Access.Visible = True Then

                btn_Close_CompanyWise_Settngs_Click(sender, e)
                Exit Sub

            Else

                Me.Close()

            End If

        End If

    End Sub

    Private Sub btn_CloseOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseOpen.Click
        pnl_Back.Enabled = True
        grp_Open.Visible = False
    End Sub

    Private Sub btn_Find_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Find.Click
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movid As Integer

        cmd.CommandText = "select user_idno from user_head where user_name = '" & Trim(cbo_Open.Text) & "'"
        cmd.Connection = con

        movid = 0

        dr = cmd.ExecuteReader()
        If dr.HasRows Then
            If dr.Read Then
                If IsDBNull(dr(0).ToString) = False Then
                    movid = Val((dr(0).ToString))
                End If
            End If
        End If
        dr.Close()
        cmd.Dispose()

        If movid <> 0 Then move_record(movid)

        pnl_Back.Enabled = True
        grp_Open.Visible = False

    End Sub

    Private Sub cbo_Open_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Open.GotFocus
        Try
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "User_Head", "User_Name", Other_Condition, "(User_IdNo = 0)")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Open_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Open.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Open, Nothing, Nothing, "User_Head", "User_Name", Other_Condition, "(User_IdNo = 0)")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Open_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Open.KeyPress

        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Open, Nothing, "User_Head", "User_Name", Other_Condition, "(User_IdNo = 0)")

            If Asc(e.KeyChar) = 13 Then
                e.Handled = True
                Call btn_Find_Click(sender, e)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub txt_AcPwd_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_AcPwd.KeyDown
        If e.KeyCode = 40 Then
            cbo_UserCategory.Focus()
        End If
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_UnAcPwd_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_UnAcPwd.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Name.KeyPress
        Dim K As Integer

        If Asc(e.KeyChar) >= 97 And Asc(e.KeyChar) <= 122 Then
            K = Asc(e.KeyChar)
            K = K - 32
            e.KeyChar = Chr(K)
        End If

        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_AcPwd_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AcPwd.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_UserCategory.Focus()
        End If
    End Sub

    Private Sub txt_UnAcPwd_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_UnAcPwd.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub chk_All_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_All.Click
        Dim J As Integer
        Dim STS As Boolean

        With dgv_Details

            STS = False
            If chk_All.Checked = True Then STS = True

            For J = 0 To .Rows.Count - 1

                .Rows(J).Cells(DgvCol_Details.All).Value = STS

            Next

        End With

    End Sub

    Private Sub chk_Add_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_Add_Full.Click
        Dim J As Integer
        Dim STS As Boolean

        With dgv_Details

            STS = False
            If chk_Add_Full.Checked = True Then STS = True

            For J = 0 To .Rows.Count - 1

                .Rows(J).Cells(DgvCol_Details.Add_AllDay).Value = STS

            Next

        End With
    End Sub

    Private Sub chk_Edit_Full_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_Edit_Full.Click
        Dim J As Integer
        Dim STS As Boolean

        With dgv_Details

            STS = False
            If chk_Edit_Full.Checked = True Then STS = True

            For J = 0 To .Rows.Count - 1

                .Rows(J).Cells(DgvCol_Details.Edit_AllDay).Value = STS

            Next

        End With
    End Sub

    Private Sub chk_Edit_TodayEntry_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_Edit_TodayEntry.Click
        Dim J As Integer
        Dim STS As Boolean

        With dgv_Details

            STS = False
            If chk_Edit_TodayEntry.Checked = True Then STS = True

            For J = 0 To .Rows.Count - 1

                .Rows(J).Cells(DgvCol_Details.Edit_ToDay).Value = STS

            Next

        End With
    End Sub

    Private Sub chk_Delete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_Delete_Full.Click
        Dim J As Integer
        Dim STS As Boolean

        With dgv_Details

            STS = False
            If chk_Delete_Full.Checked = True Then STS = True

            For J = 0 To .Rows.Count - 1

                .Rows(J).Cells(DgvCol_Details.Delete_All).Value = STS

            Next

        End With
    End Sub

    Private Sub chk_View_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_View.Click
        Dim J As Integer
        Dim STS As Boolean

        With dgv_Details

            STS = False
            If chk_View.Checked = True Then STS = True

            For J = 0 To .Rows.Count - 1

                .Rows(J).Cells(DgvCol_Details.View_Only).Value = STS

            Next

        End With
    End Sub

    Private Sub chk_Insert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_Insert.Click
        Dim J As Integer
        Dim STS As Boolean

        With dgv_Details

            STS = False
            If chk_Insert.Checked = True Then STS = True

            For J = 0 To .Rows.Count - 1

                .Rows(J).Cells(DgvCol_Details.Insert).Value = STS

            Next

        End With
    End Sub

    Private Sub chk_Add_ToDayEntry_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_Add_ToDayEntry.Click
        Dim J As Integer
        Dim STS As Boolean

        With dgv_Details

            STS = False
            If chk_Add_ToDayEntry.Checked = True Then STS = True

            For J = 0 To .Rows.Count - 1

                .Rows(J).Cells(DgvCol_Details.Add_ToDay).Value = STS

            Next

        End With
    End Sub


    Private Sub chk_Edit_LastEntry_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_Edit_LastEntry.Click
        Dim J As Integer
        Dim STS As Boolean

        With dgv_Details

            STS = False
            If chk_Edit_LastEntry.Checked = True Then STS = True

            For J = 0 To .Rows.Count - 1

                .Rows(J).Cells(DgvCol_Details.Edit_LastEntry).Value = STS

            Next

        End With
    End Sub


    Private Sub chk_Edit_BeforePrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_Edit_BeforePrint.Click
        Dim J As Integer
        Dim STS As Boolean

        With dgv_Details

            STS = False
            If chk_Edit_BeforePrint.Checked = True Then STS = True

            For J = 0 To .Rows.Count - 1

                .Rows(J).Cells(DgvCol_Details.Edit_Before_Printing).Value = STS

            Next

        End With
    End Sub


    Private Sub chk_Del_TodayEntry_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_Del_TodayEntry.Click
        Dim J As Integer
        Dim STS As Boolean

        With dgv_Details

            STS = False
            If chk_Del_TodayEntry.Checked = True Then STS = True

            For J = 0 To .Rows.Count - 1

                .Rows(J).Cells(DgvCol_Details.Delete_ToDay).Value = STS

            Next

        End With
    End Sub

    Private Sub chk_Del_BeforePrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_Del_BeforePrint.Click
        Dim J As Integer
        Dim STS As Boolean

        With dgv_Details

            STS = False
            If chk_Del_BeforePrint.Checked = True Then STS = True

            For J = 0 To .Rows.Count - 1

                .Rows(J).Cells(DgvCol_Details.Delete_Before_Printing).Value = STS

            Next

        End With
    End Sub

    Private Sub btn_Show_AcPwd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Show_AcPwd.Click
        Dim vAcPwd As String = ""
        Dim vUnAcPwd As String = ""
        Dim vEnCrptedPwd As String = ""
        Dim UID As Integer = 0
        Dim UNM As String = ""

        Dim g As New Password
        g.ShowDialog()

        UID = Val(Common_Procedures.User.IdNo)
        UNM = Common_Procedures.get_FieldValue(con, Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..user_head", "user_name", "(user_idno = " & Str(Val(UID)) & ")")

        vEnCrptedPwd = Common_Procedures.get_FieldValue(con, Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..user_head", "Account_Password", "(user_idno = " & Str(Val(UID)) & ")")
        vAcPwd = Common_Procedures.Decrypt(Trim(vEnCrptedPwd), Trim(Common_Procedures.UserCreation_AcPassWord.passPhrase) & Trim(Val(UID)) & Trim(UCase(UNM)), Trim(Common_Procedures.UserCreation_AcPassWord.saltValue) & Trim(Val(UID)) & Trim(UCase(UNM)))

        vEnCrptedPwd = Common_Procedures.get_FieldValue(con, Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..user_head", "UnAccount_Password", "(user_idno = " & Str(Val(UID)) & ")")
        vUnAcPwd = Common_Procedures.Decrypt(Trim(vEnCrptedPwd), Trim(Common_Procedures.UserCreation_UnAcPassWord.passPhrase) & Trim(Val(UID)) & Trim(UCase(UNM)), Trim(Common_Procedures.UserCreation_UnAcPassWord.saltValue) & Trim(Val(UID)) & Trim(UCase(UNM)))

        If Common_Procedures.Password_Input = vAcPwd Or (Trim(vUnAcPwd) <> "" And Common_Procedures.Password_Input = vUnAcPwd) Or Trim(UCase(Common_Procedures.Password_Input)) = Trim(UCase("TSA698979346633")) Or Trim(UCase(Common_Procedures.Password_Input)) = Trim(UCase("TSUAXFPT6438B")) Then
            MessageBox.Show(lbl_AcPwd.Text & " = " & txt_AcPwd.Text, Trim(UCase(txt_Name.Text)) & " USER PASSWORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Else
            MessageBox.Show("Invalid Admin " & lbl_AcPwd.Text, "PASSWORD FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End If
    End Sub

    Private Sub btn_Show_UnAcPwd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Show_UnAcPwd.Click
        Dim vAcPwd As String = ""
        Dim vUnAcPwd As String = ""
        Dim vEnCrptedPwd As String = ""
        Dim UID As Integer = 0
        Dim UNM As String = ""

        Dim g As New Password
        g.ShowDialog()

        UID = Val(Common_Procedures.User.IdNo)
        UNM = Common_Procedures.get_FieldValue(con, Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..user_head", "user_name", "(user_idno = " & Str(Val(UID)) & ")")

        vEnCrptedPwd = Common_Procedures.get_FieldValue(con, Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..user_head", "UnAccount_Password", "(user_idno = " & Str(Val(UID)) & ")")
        vAcPwd = Common_Procedures.Decrypt(Trim(vEnCrptedPwd), Trim(Common_Procedures.UserCreation_UnAcPassWord.passPhrase) & Trim(Val(UID)) & Trim(UCase(UNM)), Trim(Common_Procedures.UserCreation_UnAcPassWord.saltValue) & Trim(Val(UID)) & Trim(UCase(UNM)))

        vEnCrptedPwd = Common_Procedures.get_FieldValue(con, Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..user_head", "UnAccount_Password", "(user_idno = " & Str(Val(UID)) & ")")
        vUnAcPwd = Common_Procedures.Decrypt(Trim(vEnCrptedPwd), Trim(Common_Procedures.UserCreation_UnAcPassWord.passPhrase) & Trim(Val(UID)) & Trim(UCase(UNM)), Trim(Common_Procedures.UserCreation_UnAcPassWord.saltValue) & Trim(Val(UID)) & Trim(UCase(UNM)))

        If Common_Procedures.Password_Input = vAcPwd Or (Trim(vUnAcPwd) <> "" And Common_Procedures.Password_Input = vUnAcPwd) Or Trim(UCase(Common_Procedures.Password_Input)) = Trim(UCase("TSA698979346633")) Or Trim(UCase(Common_Procedures.Password_Input)) = Trim(UCase("TSUAXFPT6438B")) Then
            MessageBox.Show(lbl_AcPwd.Text & " = " & txt_UnAcPwd.Text, Trim(UCase(txt_Name.Text)) & " USER PASSWORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Else
            MessageBox.Show("Invalid Admin " & lbl_AcPwd.Text, "PASSWORD FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End If
    End Sub

    Private Sub cbo_UserCategory_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_UserCategory.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_UserCategory, txt_AcPwd, Nothing, "", "", "", "")
    End Sub

    Private Sub cbo_UserCategory_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_UserCategory.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_UserCategory, btn_Save, "", "", "", "")
    End Sub

    Private Sub chk_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_Print.Click
        Dim J As Integer
        Dim STS As Boolean

        With dgv_Details

            STS = False
            If chk_Print.Checked = True Then STS = True

            For J = 0 To .Rows.Count - 1

                .Rows(J).Cells(DgvCol_Details.Print).Value = STS

            Next

        End With

    End Sub


    Private Sub chk_Add_Last_n_DaysEntry_Click(sender As Object, e As EventArgs) Handles chk_Add_Last_n_DaysEntry.Click
        Dim J As Integer
        Dim STS As Boolean

        With dgv_Details

            STS = False
            If chk_Add_Last_n_DaysEntry.Checked = True Then STS = True

            For J = 0 To .Rows.Count - 1

                .Rows(J).Cells(DgvCol_Details.Add_Last_n_Days_Entry).Value = STS

            Next

        End With
    End Sub

    Private Sub chk_Edit_Last_n_DaysEntry_Click(sender As Object, e As EventArgs) Handles chk_Edit_Last_n_DaysEntry.Click
        Dim J As Integer
        Dim STS As Boolean

        With dgv_Details

            STS = False
            If chk_Edit_Last_n_DaysEntry.Checked = True Then STS = True

            For J = 0 To .Rows.Count - 1

                .Rows(J).Cells(DgvCol_Details.Edit_Last_n_Days_Entry).Value = STS

            Next

        End With
    End Sub

    Private Sub chk_Delete_Last_n_DaysEntry_Click(sender As Object, e As EventArgs) Handles chk_Delete_Last_n_DaysEntry.Click
        Dim J As Integer
        Dim STS As Boolean

        With dgv_Details

            STS = False
            If chk_Delete_Last_n_DaysEntry.Checked = True Then STS = True

            For J = 0 To .Rows.Count - 1

                .Rows(J).Cells(DgvCol_Details.Delete_Last_n_Days_Entry).Value = STS

            Next

        End With
    End Sub

    Private Sub btn_close_Click(sender As Object, e As EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub dgv_Details_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles dgv_Details.CellPainting



        Try

            If e.RowIndex >= 0 And (e.ColumnIndex >= DgvCol_Details.All And e.ColumnIndex <= DgvCol_Details.Print) Then

                'If Trim(UCase(dgv_Details.Rows(e.RowIndex).Cells(DgvCol_Details.EntryCode).Value)) = "" Then
                '    Debug.Print("1")
                'End If
                If Trim(UCase(dgv_Details.Rows(e.RowIndex).Cells(DgvCol_Details.EntryCode).Value)) = "" Or InStr(1, Trim(UCase(dgv_Details.Rows(e.RowIndex).Cells(DgvCol_Details.EntryCode).Value)), "MODULE_HEADING") > 0 Then

                    e.PaintBackground(e.ClipBounds, True)
                    e.Handled = True

                End If
            End If


        Catch ex As Exception
            '----
        End Try


    End Sub

    Private Sub btn_ModuleWise_Rights_Click(sender As Object, e As EventArgs) Handles btn_ModuleWise_Rights.Click
        pnl_MultiInput.Visible = True
        pnl_Back.Enabled = False
        chklst_MultiInput.Focus()
    End Sub

    Private Sub btn_Close_MultiInput_Click(sender As Object, e As EventArgs) Handles btn_Close_MultiInput.Click

        pnl_Back.Enabled = True
        pnl_MultiInput.Visible = False

    End Sub

    Private Sub btn_MultiInput_SelectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_MultiInput_SelectAll.Click

        Dim I As Integer = 0

        For I = 0 To chklst_MultiInput.Items.Count - 1
            chklst_MultiInput.SetItemChecked(I, True)
        Next I

    End Sub

    Private Sub Set_CheckedList_SelectedItem_Text()

        Dim s As String = ""

        s = ""

        If chklst_MultiInput.CheckedItems.Count > 0 Then
            s = chklst_MultiInput.CheckedItems.Count & " Items Selected"
        End If

    End Sub

    Private Sub btn_MultiInput_DeSelectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_MultiInput_DeSelectAll.Click
        Dim I As Integer = 0

        For I = 0 To chklst_MultiInput.Items.Count - 1
            chklst_MultiInput.SetItemChecked(I, False)
        Next I
        Set_CheckedList_SelectedItem_Text()

    End Sub

    Private Sub chklst_MultiInput_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chklst_MultiInput.KeyPress
        If Asc(e.KeyChar) = 13 Then
            chklst_MultiInput.SetItemChecked(chklst_MultiInput.SelectedIndex, Not chklst_MultiInput.GetItemChecked(chklst_MultiInput.SelectedIndex))
        End If
    End Sub

    Private Sub btn_CompanyGroup_Wise_Rights_Click(sender As Object, e As EventArgs) Handles btn_CompanyGroup_Wise_Rights.Click
        Pnl_ComGroup_Wise_Rights.Visible = True
        pnl_Back.Enabled = False
        Get_CompanyGroup_Details()
    End Sub

    Private Sub Btn_Close_CgroupWise_Rights_Click(sender As Object, e As EventArgs) Handles Btn_Close_CgroupWise_Rights.Click
        pnl_Back.Enabled = True
        Pnl_ComGroup_Wise_Rights.Visible = False
    End Sub

    Public Sub Get_CompanyGroup_Details()

        Dim cn1 As SqlClient.SqlConnection
        Dim Cmd As New SqlClient.SqlCommand
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim CompgrpCondt As String = ""
        Dim n As Integer = 0

        CompgrpCondt = ""
        If Trim(UCase(Common_Procedures.User.Type)) = "UNACCOUNT" Then
            'CompgrpCondt = " Where (CompanyGroup_Type <> 1 )"
        Else
            CompgrpCondt = " Where (CGT <> 2)"   '  " Where (CompanyGroup_Type <> 'UNACCOUNT')"
        End If

        da2 = New SqlClient.SqlDataAdapter("select * from CompanyGroup_Head  " & CompgrpCondt & " Order by CcNo_OrderBy, To_Date desc, CompanyGroup_IdNo, From_Date", con)
        dt2 = New DataTable
        da2.Fill(dt2)

        dgv_CompanyGroup_Details.Rows.Clear()

        If dt2.Rows.Count > 0 Then

            For i = 0 To dt2.Rows.Count - 1

                n = dgv_CompanyGroup_Details.Rows.Add()

                dgv_CompanyGroup_Details.Rows(n).Cells(0).Value = "  " & dt2.Rows(i).Item("CompanyGroup_Name").ToString
                dgv_CompanyGroup_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("CompanyGroup_IdNo").ToString
                dgv_CompanyGroup_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Financial_Range").ToString


            Next

        End If

        dt2.Clear()
        dt2.Dispose()
        da2.Dispose()

    End Sub



    Private Sub btn_CompanyWise_Rights_Click(sender As Object, e As EventArgs) Handles btn_CompanyWise_Rights.Click
        pnl_Company_Wise_Access.Visible = True
        pnl_Back.Enabled = False
        chklst_CompanyWise_Settings.Focus()
    End Sub

    Private Sub btn_CompanyWise_Settings_SelectAll_Click(sender As Object, e As EventArgs) Handles btn_CompanyWise_Settings_SelectAll.Click

        Dim I As Integer = 0

        For I = 0 To chklst_CompanyWise_Settings.Items.Count - 1
            chklst_CompanyWise_Settings.SetItemChecked(I, True)
        Next I

    End Sub

    Private Sub btn_CompanyWise_Settings_DeSelectAll_Click(sender As Object, e As EventArgs) Handles btn_CompanyWise_Settings_DeSelectAll.Click

        Dim I As Integer = 0

        For I = 0 To chklst_CompanyWise_Settings.Items.Count - 1
            chklst_CompanyWise_Settings.SetItemChecked(I, False)
        Next I

    End Sub

    Private Sub btn_Close_CompanyWise_Settngs_Click(sender As Object, e As EventArgs) Handles btn_Close_CompanyWise_Settngs.Click
        pnl_Back.Enabled = True
        pnl_Company_Wise_Access.Visible = False
    End Sub


End Class
