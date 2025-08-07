Imports Microsoft.VisualBasic.PowerPacks
Public Class Menu_List

    Private Sub Menu_List_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        btn_ownsort.Focus()
    End Sub

    Private Sub Menu_List_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1088" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "--1155--" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1417" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then

            Me.Height = 650
            Me.Width = 600

            bth_gen_reports.Visible = True
            bth_gen_reports.Left = btn_Sizing.Left
            bth_gen_reports.Top = btn_Sizing.Top

            btn_Stores.Visible = True
            btn_Stores.Left = btn_jobwork.Left
            btn_Stores.Top = btn_jobwork.Top

            btn_Accounts.Visible = True
            btn_Accounts.Left = btn_Vendor.Left
            btn_Accounts.Top = btn_Vendor.Top

            btn_payroll.Visible = True
            btn_payroll.Left = btn_payroll.Left
            btn_payroll.Top = btn_payroll.Top

            btn_OE.Visible = True
            btn_OE.Left = btn_ownsort.Left
            btn_OE.Top = btn_ownsort.Top

            btn_Sizing.Visible = True
            btn_Sizing.Left = btn_master.Left
            btn_Sizing.Top = btn_master.Top

            btn_ownsort.Visible = True
            btn_ownsort.Left = btn_opening.Left
            btn_ownsort.Top = btn_opening.Top

            btn_exit.Left = 600 - btn_exit.Width - 30

            Btn_Company.Visible = False
            btn_settings.Visible = False
            btn_exit.Visible = True
            btn_opening.Visible = False
            btn_master.Visible = False
            btn_trading.Visible = False
            btn_jobwork.Visible = False
            btn_Vendor.Visible = False

            btn_ownsort.Text = "TEXTILE"
            bth_gen_reports.Text = "MIS Reports"


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then

            Me.Height = 550
            Me.Width = 600

            bth_gen_reports.Visible = True
            bth_gen_reports.Left = btn_payroll.Left
            bth_gen_reports.Top = btn_payroll.Top

            btn_Stores.Visible = True
            btn_Stores.Left = btn_jobwork.Left
            btn_Stores.Top = btn_jobwork.Top

            btn_Accounts.Visible = True
            btn_Accounts.Left = btn_Vendor.Left
            btn_Accounts.Top = btn_Vendor.Top


            btn_OE.Visible = True
            btn_OE.Text = "SPINNING"
            btn_OE.Left = btn_ownsort.Left
            btn_OE.Top = btn_ownsort.Top

            btn_Sizing.Visible = True
            btn_Sizing.Left = btn_master.Left
            btn_Sizing.Top = btn_master.Top

            btn_ownsort.Visible = True
            btn_ownsort.Left = btn_opening.Left
            btn_ownsort.Top = btn_opening.Top

            btn_exit.Left = 600 - btn_exit.Width - 30

            Btn_Company.Visible = False
            btn_settings.Visible = False
            btn_exit.Visible = True
            btn_opening.Visible = False
            btn_master.Visible = False
            btn_trading.Visible = False
            btn_jobwork.Visible = False
            btn_Vendor.Visible = False
            btn_payroll.Visible = False

            btn_ownsort.Text = "TEXTILE"
            bth_gen_reports.Text = "MIS Reports"

        Else

            Me.Width = 780 '857
            Me.Height = 650

            btn_Vendor.Enabled = True

            btn_jobwork.Enabled = False
            If Common_Procedures.settings.JOBWORKENTRY_Status = 1 Then
                btn_jobwork.Enabled = True
            End If

            btn_payroll.Enabled = False
            If Common_Procedures.settings.PAYROLLENTRY_Status = 1 Then
                btn_payroll.Enabled = True
            End If

            btn_Stores.Enabled = False
            If Common_Procedures.settings.STORESENTRY_Status = 1 Then
                btn_Stores.Enabled = True
            End If

            btn_Sizing.Enabled = False
            If Common_Procedures.settings.SizingSoftware_ENTRY_Status = 1 Then
                btn_Sizing.Enabled = True
            End If

            btn_OE.Enabled = False
            If Common_Procedures.settings.OESofwtare_ENTRY_Status = 1 Then
                btn_OE.Enabled = True
            End If

            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then '---- UNITED WEAVES (PALLADAM)
            btn_trading.Image = My.Resources.Trading_logo_new
            btn_jobwork.Image = My.Resources.jobwork_logo_new
            btn_opening.Image = My.Resources.Opening_logo_new
            btn_ownsort.Image = My.Resources.ownSort_logo_new
            btn_Accounts.Image = My.Resources.Accounts_logo_new
            'End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1186" Then
                bth_gen_reports.Text = "MIS Reports"
            End If

            If Common_Procedures.settings.Bobin_Zari_Kuri_Entries_Status = 1 Then
                btn_OE.Visible = True
                btn_OE.Enabled = True
                btn_OE.Text = "BOBIN"
            End If

        End If


        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- UNITED WEAVES (PALLADAM)

        '    Me.Width = 780 '857
        '    Me.Height = 650

        '    btn_Sizing.Enabled = False
        '    btn_OE.Enabled = False
        '    btn_Vendor.Enabled = False

        '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Then
        '        btn_Sizing.Enabled = True
        '        btn_Vendor.Enabled = True
        '    End If

        '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then '---- UNITED WEAVES (PALLADAM)

        '        btn_trading.Image = My.Resources.Trading_logo_new
        '        btn_jobwork.Image = My.Resources.jobwork_logo_new
        '        btn_opening.Image = My.Resources.Opening_logo_new
        '        btn_ownsort.Image = My.Resources.ownSort_logo_new
        '        btn_Accounts.Image = My.Resources.Accounts_logo_new

        '    End If

        '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then
        '        bth_gen_reports.Text = "MIS Reports"
        '    End If

        'Else

        '    Me.Height = 650
        '    Me.Width = 600

        '    bth_gen_reports.Visible = True
        '    bth_gen_reports.Left = btn_Sizing.Left
        '    bth_gen_reports.Top = btn_Sizing.Top

        '    btn_Stores.Visible = True
        '    btn_Stores.Left = btn_jobwork.Left
        '    btn_Stores.Top = btn_jobwork.Top

        '    btn_Accounts.Visible = True
        '    btn_Accounts.Left = btn_Vendor.Left
        '    btn_Accounts.Top = btn_Vendor.Top

        '    btn_payroll.Visible = True
        '    btn_payroll.Left = btn_payroll.Left
        '    btn_payroll.Top = btn_payroll.Top

        '    btn_OE.Visible = True
        '    btn_OE.Left = btn_ownsort.Left
        '    btn_OE.Top = btn_ownsort.Top

        '    btn_Sizing.Visible = True
        '    btn_Sizing.Left = btn_master.Left
        '    btn_Sizing.Top = btn_master.Top

        '    btn_ownsort.Visible = True
        '    btn_ownsort.Left = btn_opening.Left
        '    btn_ownsort.Top = btn_opening.Top

        '    btn_exit.Left = 600 - btn_exit.Width - 30

        '    Btn_Company.Visible = False
        '    btn_settings.Visible = False
        '    btn_exit.Visible = True
        '    btn_opening.Visible = False
        '    btn_master.Visible = False
        '    btn_trading.Visible = False
        '    btn_jobwork.Visible = False
        '    btn_Vendor.Visible = False

        '    btn_ownsort.Text = "TEXTILE"
        '    bth_gen_reports.Text = "MIS Reports"

        'End If


        btn_ShowAll_Modules.Visible = False
        If Common_Procedures.Office_System_Status = True Then
            btn_ShowAll_Modules.Visible = True
        End If

        If Common_Procedures.User.IdNo = 1 Then

            btn_opening.Enabled = True
            btn_master.Enabled = True
            btn_ownsort.Enabled = True
            btn_trading.Enabled = True
            If Common_Procedures.settings.JOBWORKENTRY_Status = 1 Then
                btn_jobwork.Enabled = True
            Else
                btn_jobwork.Enabled = False
            End If

            If Common_Procedures.settings.PAYROLLENTRY_Status = 1 Then
                btn_payroll.Enabled = True
            Else
                btn_payroll.Enabled = False
            End If

            btn_Accounts.Enabled = True
            If Common_Procedures.settings.SizingSoftware_ENTRY_Status = 1 Then
                btn_Sizing.Enabled = True
            Else
                btn_Sizing.Enabled = False
            End If
            If Common_Procedures.settings.OESofwtare_ENTRY_Status = 1 Or Common_Procedures.settings.Bobin_Zari_Kuri_Entries_Status = 1 Then
                btn_OE.Enabled = True
            Else
                btn_OE.Enabled = False
            End If

            bth_gen_reports.Enabled = True
            If Common_Procedures.settings.STORESENTRY_Status = 1 Then
                btn_Stores.Enabled = True
            Else
                btn_Stores.Enabled = False
            End If

            btn_Vendor.Enabled = True

        Else

            If InStr(1, Trim(UCase(Common_Procedures.User.ModuleWise_AccessRights)), Trim(UCase("OPENING_MODULE"))) > 0 Then
                btn_opening.Enabled = True
            Else
                btn_opening.Enabled = False
            End If
            If InStr(1, Trim(UCase(Common_Procedures.User.ModuleWise_AccessRights)), Trim(UCase("MASTER_MODULE"))) > 0 Then
                btn_master.Enabled = True
            Else
                btn_master.Enabled = False
            End If
            If InStr(1, Trim(UCase(Common_Procedures.User.ModuleWise_AccessRights)), Trim(UCase("TEXTILE_MODULE"))) > 0 Or InStr(1, Trim(UCase(Common_Procedures.User.ModuleWise_AccessRights)), Trim(UCase("OWNSORT_MODULE"))) > 0 Then
                btn_ownsort.Enabled = True
            Else
                btn_ownsort.Enabled = False
            End If
            If InStr(1, Trim(UCase(Common_Procedures.User.ModuleWise_AccessRights)), Trim(UCase("TRADING_MODULE"))) > 0 Then
                btn_trading.Enabled = True
            Else
                btn_trading.Enabled = False
            End If
            If InStr(1, Trim(UCase(Common_Procedures.User.ModuleWise_AccessRights)), Trim(UCase("JOBWORK_MODULE"))) > 0 Then
                btn_jobwork.Enabled = True
            Else
                btn_jobwork.Enabled = False
            End If
            If InStr(1, Trim(UCase(Common_Procedures.User.ModuleWise_AccessRights)), Trim(UCase("PAYROLL_MODULE"))) > 0 Then
                btn_payroll.Enabled = True
            Else
                btn_payroll.Enabled = False
            End If
            If InStr(1, Trim(UCase(Common_Procedures.User.ModuleWise_AccessRights)), Trim(UCase("ACCOUNTS_MODULE"))) > 0 Then
                btn_Accounts.Enabled = True
            Else
                btn_Accounts.Enabled = False
            End If
            If Common_Procedures.settings.SizingSoftware_ENTRY_Status = 1 And InStr(1, Trim(UCase(Common_Procedures.User.ModuleWise_AccessRights)), Trim(UCase("SIZING_JOBWORK_MODULE"))) > 0 Then
                btn_Sizing.Enabled = True
            Else
                btn_Sizing.Enabled = False
            End If
            If Common_Procedures.settings.OESofwtare_ENTRY_Status = 1 And InStr(1, Trim(UCase(Common_Procedures.User.ModuleWise_AccessRights)), Trim(UCase("OE_MODULE"))) > 0 Then
                btn_OE.Enabled = True
            Else
                btn_OE.Enabled = False
            End If
            If InStr(1, Trim(UCase(Common_Procedures.User.ModuleWise_AccessRights)), Trim(UCase("GENERAL_REPORTS_MODULE"))) > 0 Then
                bth_gen_reports.Enabled = True
            Else
                bth_gen_reports.Enabled = False
            End If
            If InStr(1, Trim(UCase(Common_Procedures.User.ModuleWise_AccessRights)), Trim(UCase("STORES_MODULE"))) > 0 Then
                btn_Stores.Enabled = True
            Else
                btn_Stores.Enabled = False
            End If
            If InStr(1, Trim(UCase(Common_Procedures.User.ModuleWise_AccessRights)), Trim(UCase("VENDOR_MODULE"))) > 0 Then
                btn_Vendor.Enabled = True
            Else
                btn_Vendor.Enabled = False
            End If

        End If


    End Sub

    Private Sub btn_master_Click(sender As System.Object, e As System.EventArgs) Handles btn_master.Click

        Common_Procedures.SoftwareType_Opened = Common_Procedures.SoftwareTypes.Textile_Software

        MDIParent1.lbl_Menu_name.Text = "MASTER"

        MDIParent1.Show()

        MDIParent1.mnu_CompanyMain.Visible = False
        MDIParent1.mnu_Opening_Main.Visible = False
        MDIParent1.mnu_Entry_Textile_Main.Visible = False
        MDIParent1.mnu_Vendor_Entry_Vendor_Main.Visible = False
        MDIParent1.mnu_Entry_JobWork_Main.Visible = False
        MDIParent1.mnu_Voucher_Main.Visible = False
        MDIParent1.mnu_Report_Textile_Main.Visible = False
        MDIParent1.mnu_Report_JobWork_Main.Visible = False
        MDIParent1.mnu_Reports_OE_Software_Main.Visible = False
        MDIParent1.mnu_Accounts_Main.Visible = False
        MDIParent1.mnu_New_ownSort_Reports_Main.Visible = False
        MDIParent1.mnu_Master_Textile_JobWork_Main.Visible = True
        MDIParent1.mnu_new_own_Sort_Main.Visible = False
        MDIParent1.mnu_new_Trading_Reports_Main.Visible = False
        MDIParent1.mnu_New_master_Reports_Main.Visible = True
        MDIParent1.mnu_Home.Visible = True
        MDIParent1.Mnu_General_Reports_Main.Visible = False
        MDIParent1.mnu_Entry_PayRoll_Main.Visible = False
        MDIParent1.mnu_New_Payroll_Reports_Main.Visible = False
        MDIParent1.mnu_Voucher_Main.Visible = False
        MDIParent1.mnu_Billing_Purchase_Entry.Visible = False
        MDIParent1.mnu_Billing_Reports.Visible = False

        MDIParent1.mnu_Opening_Sizing_Software_Main.Visible = False
        MDIParent1.mnu_Master_Sizing_Software_Main.Visible = False
        MDIParent1.mnu_Entries_Sizing_Software_Main.Visible = False
        MDIParent1.mnu_Reports_Sizing_Software_Main.Visible = False

        MDIParent1.mnu_Master_FP_Processing_Main.Visible = False
        MDIParent1.mnu_Entry_FP_Processing_Main.Visible = False
        MDIParent1.mnu_Report_FP_Processing_Main.Visible = False

        MDIParent1.mnu_Entry_Bobin_Main.Visible = False
        MDIParent1.mnu_Report_Bobin_Main.Visible = False

        MDIParent1.mnu_WindowsMenu_Main.Visible = True
        MDIParent1.ToolsMenu.Visible = False

        MDIParent1.lbl_Menu_name.Text = "MASTER"
        MDIParent1.Text = MDIParent1.lbl_Menu_name.Text & "  -  " & Common_Procedures.CompGroupIdNo & ". " & Common_Procedures.CompGroupName & " (" & Common_Procedures.CompGroupFnRange & ")           -            " & Common_Procedures.Company_FromDate & "   TO   " & Common_Procedures.Company_ToDate
        MDIParent1.lbl_CompanyName.Text = "TSOFT ERP"
        MDIParent1.lbl_SoftwareName.Text = "TSOFT Textile ERP Solutions"

        Me.Close()
    End Sub

    Private Sub btn_jobwork_Click(sender As System.Object, e As System.EventArgs) Handles btn_jobwork.Click

        Common_Procedures.SoftwareType_Opened = Common_Procedures.SoftwareTypes.Textile_Software
        MDIParent1.lbl_Menu_name.Text = "JOBWORK"
        MDIParent1.Show()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then  '---- UNITED WEAVES (PALLADAM)
            MDIParent1.mnu_new_Jobwork_Main.Visible = True
            MDIParent1.mnu_new_Jobwork_Reports_Main.Visible = True

            MDIParent1.mnu_Entry_JobWork_Main.Visible = False
            MDIParent1.mnu_Report_JobWork_Main.Visible = False

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then
                MDIParent1.mnu_Entry_JobWork_Main.Visible = True
            End If

        Else

            MDIParent1.mnu_Entry_JobWork_Main.Visible = True
            MDIParent1.mnu_Report_JobWork_Main.Visible = True

            MDIParent1.mnu_new_Jobwork_Main.Visible = False
            MDIParent1.mnu_new_Jobwork_Reports_Main.Visible = False

        End If



        MDIParent1.mnu_CompanyMain.Visible = False
        MDIParent1.mnu_Opening_Main.Visible = False
        MDIParent1.mnu_Entry_Textile_Main.Visible = False
        MDIParent1.mnu_Master_Textile_JobWork_Main.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then '------------- SRINIVASA TEXTILE
            '------------- MYTHRA TEXTILE -  changed the settings for demo purpose only with SRINIVAS SETTINGS
            MDIParent1.mnu_Master_Textile_JobWork_Main.Visible = True
        End If
        MDIParent1.mnu_Voucher_Main.Visible = False
        MDIParent1.mnu_Report_Textile_Main.Visible = False
        MDIParent1.mnu_New_master_Reports_Main.Visible = False
        MDIParent1.mnu_Reports_OE_Software_Main.Visible = False
        MDIParent1.mnu_Accounts_Main.Visible = False
        MDIParent1.mnu_Home.Visible = True
        MDIParent1.mnu_Vendor_Entry_Vendor_Main.Visible = False

        MDIParent1.mnu_New_Trading_Main.Visible = False
        MDIParent1.Mnu_General_Reports_Main.Visible = False

        MDIParent1.mnu_New_ownSort_Reports_Main.Visible = False
        MDIParent1.mnu_new_Trading_Reports_Main.Visible = False
        MDIParent1.mnu_Entry_PayRoll_Main.Visible = False
        MDIParent1.mnu_New_Payroll_Reports_Main.Visible = False
        MDIParent1.mnu_new_own_Sort_Main.Visible = False
        MDIParent1.mnu_Voucher_Main.Visible = False
        MDIParent1.mnu_Billing_Purchase_Entry.Visible = False
        MDIParent1.mnu_Billing_Reports.Visible = False

        MDIParent1.mnu_Opening_Sizing_Software_Main.Visible = False
        MDIParent1.mnu_Master_Sizing_Software_Main.Visible = False
        MDIParent1.mnu_Entries_Sizing_Software_Main.Visible = False
        MDIParent1.mnu_Reports_Sizing_Software_Main.Visible = False

        MDIParent1.mnu_Master_FP_Processing_Main.Visible = False
        MDIParent1.mnu_Entry_FP_Processing_Main.Visible = False
        MDIParent1.mnu_Report_FP_Processing_Main.Visible = False

        MDIParent1.mnu_Entry_Bobin_Main.Visible = False
        MDIParent1.mnu_Report_Bobin_Main.Visible = False

        MDIParent1.mnu_WindowsMenu_Main.Visible = True
        MDIParent1.ToolsMenu.Visible = False

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then
            MDIParent1.mnu_Report_Textile_Main.Visible = True
            MDIParent1.Mnu_General_Reports_Main.Visible = True
            MDIParent1.mnu_Report_JobWork_Main.Visible = True
        End If

        MDIParent1.lbl_Menu_name.Text = "JOBWORK"
        MDIParent1.Text = MDIParent1.lbl_Menu_name.Text & "  -  " & Common_Procedures.CompGroupIdNo & ". " & Common_Procedures.CompGroupName & " (" & Common_Procedures.CompGroupFnRange & ")           -            " & Common_Procedures.Company_FromDate & "   TO   " & Common_Procedures.Company_ToDate
        MDIParent1.lbl_CompanyName.Text = "TSOFT ERP"
        MDIParent1.lbl_SoftwareName.Text = "TSOFT Textile ERP Solutions"

        Me.Close()

    End Sub

    Private Sub btn_trading_Click(sender As Object, e As System.EventArgs) Handles btn_trading.Click

        Common_Procedures.SoftwareType_Opened = Common_Procedures.SoftwareTypes.Textile_Software
        MDIParent1.lbl_Menu_name.Text = "TRADING"
        MDIParent1.Show()
        MDIParent1.mnu_CompanyMain.Visible = False
        MDIParent1.mnu_Opening_Main.Visible = False
        MDIParent1.mnu_Entry_Textile_Main.Visible = False
        MDIParent1.mnu_Master_Textile_JobWork_Main.Visible = False
        MDIParent1.mnu_Voucher_Main.Visible = False
        MDIParent1.mnu_Report_Textile_Main.Visible = False
        MDIParent1.mnu_New_master_Reports_Main.Visible = False
        MDIParent1.mnu_Reports_OE_Software_Main.Visible = False
        MDIParent1.mnu_Accounts_Main.Visible = False
        MDIParent1.mnu_new_Trading_Reports_Main.Visible = True
        MDIParent1.mnu_Home.Visible = True

        MDIParent1.mnu_New_Trading_Main.Visible = True
        MDIParent1.mnu_Vendor_Entry_Vendor_Main.Visible = False
        MDIParent1.mnu_Entry_JobWork_Main.Visible = False
        MDIParent1.mnu_Report_JobWork_Main.Visible = False
        MDIParent1.mnu_New_ownSort_Reports_Main.Visible = False
        MDIParent1.Mnu_General_Reports_Main.Visible = False
        MDIParent1.mnu_Entry_PayRoll_Main.Visible = False
        MDIParent1.mnu_New_Payroll_Reports_Main.Visible = False
        MDIParent1.mnu_new_own_Sort_Main.Visible = False
        MDIParent1.mnu_Voucher_Main.Visible = False
        MDIParent1.mnu_Billing_Purchase_Entry.Visible = False
        MDIParent1.mnu_Billing_Reports.Visible = False

        MDIParent1.mnu_Opening_Sizing_Software_Main.Visible = False
        MDIParent1.mnu_Master_Sizing_Software_Main.Visible = False
        MDIParent1.mnu_Entries_Sizing_Software_Main.Visible = False
        MDIParent1.mnu_Reports_Sizing_Software_Main.Visible = False

        MDIParent1.mnu_Master_FP_Processing_Main.Visible = False
        MDIParent1.mnu_Entry_FP_Processing_Main.Visible = False
        MDIParent1.mnu_Report_FP_Processing_Main.Visible = False

        MDIParent1.mnu_Entry_Bobin_Main.Visible = False
        MDIParent1.mnu_Report_Bobin_Main.Visible = False

        MDIParent1.mnu_WindowsMenu_Main.Visible = True
        MDIParent1.ToolsMenu.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then '-----UNITED WEAVES
            MDIParent1.mnu_Trading_Fibre_Entry_Main.Visible = True
            MDIParent1.mnu_Trading_Fibre_Reports_Main.Visible = True
        Else
            MDIParent1.mnu_Trading_Fibre_Entry_Main.Visible = False
            MDIParent1.mnu_Trading_Fibre_Reports_Main.Visible = False
        End If
        MDIParent1.lbl_Menu_name.Text = "TRADING"
        MDIParent1.Text = MDIParent1.lbl_Menu_name.Text & "  -  " & Common_Procedures.CompGroupIdNo & ". " & Common_Procedures.CompGroupName & " (" & Common_Procedures.CompGroupFnRange & ")           -            " & Common_Procedures.Company_FromDate & "   TO   " & Common_Procedures.Company_ToDate
        MDIParent1.lbl_CompanyName.Text = "TSOFT ERP"
        MDIParent1.lbl_SoftwareName.Text = "TSOFT Textile ERP Solutions"

        Me.Close()
    End Sub

    Private Sub btn_Accounts_Click(sender As System.Object, e As System.EventArgs) Handles btn_Accounts.Click

        Common_Procedures.SoftwareType_Opened = Common_Procedures.SoftwareTypes.Accounts_Software
        MDIParent1.lbl_Menu_name.Text = "ACCOUNTS"

        MDIParent1.Show()

        MDIParent1.lbl_Menu_name.Text = "ACCOUNTS"
        MDIParent1.Text = MDIParent1.lbl_Menu_name.Text & "  -  " & Common_Procedures.CompGroupIdNo & ". " & Common_Procedures.CompGroupName & " (" & Common_Procedures.CompGroupFnRange & ")           -            " & Common_Procedures.Company_FromDate & "   TO   " & Common_Procedures.Company_ToDate
        MDIParent1.lbl_CompanyName.Text = "TSOFT ERP"
        MDIParent1.lbl_SoftwareName.Text = "TSOFT Textile ERP Solutions"

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then  '---- UNITED WEAVES (PALLADAM)
            MDIParent1.mnu_Billing_Purchase_Entry.Visible = True
            MDIParent1.mnu_Billing_Sales_Entry.Visible = True
            MDIParent1.mnu_Billing_Other_Voucher_Entry_Main.Visible = True

            MDIParent1.mnu_CompanyMain.Visible = False

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then  '---- UNITED WEAVES (PALLADAM)
                MDIParent1.mnu_Voucher_Main.Visible = False
                MDIParent1.mnu_Accounts_Main.Visible = False

            Else
                MDIParent1.mnu_Voucher_Main.Visible = True
                MDIParent1.mnu_Accounts_Main.Visible = True

            End If

        Else

            MDIParent1.mnu_Billing_Purchase_Entry.Visible = False
            MDIParent1.mnu_Billing_Sales_Entry.Visible = False
            MDIParent1.mnu_Billing_Other_Voucher_Entry_Main.Visible = False

            MDIParent1.mnu_CompanyMain.Visible = True

            MDIParent1.mnu_Voucher_Main.Visible = True
            MDIParent1.mnu_Accounts_Main.Visible = True

        End If

        MDIParent1.mnu_Home.Visible = True

        MDIParent1.mnu_Opening_Main.Visible = False
        MDIParent1.mnu_Entry_Textile_Main.Visible = False
        MDIParent1.mnu_Master_Textile_JobWork_Main.Visible = False

        MDIParent1.mnu_Vendor_Entry_Vendor_Main.Visible = False

        MDIParent1.mnu_Report_Textile_Main.Visible = False
        MDIParent1.mnu_New_master_Reports_Main.Visible = False
        MDIParent1.mnu_Reports_OE_Software_Main.Visible = False

        MDIParent1.mnu_Billing_Reports.Visible = False
        MDIParent1.mnu_Vendor_Entry_Vendor_Main.Visible = False

        MDIParent1.mnu_new_Trading_Reports_Main.Visible = False
        MDIParent1.mnu_Entry_PayRoll_Main.Visible = False
        MDIParent1.mnu_New_Payroll_Reports_Main.Visible = False
        MDIParent1.mnu_New_Trading_Main.Visible = False

        MDIParent1.mnu_Entry_JobWork_Main.Visible = False
        MDIParent1.mnu_Report_JobWork_Main.Visible = False
        MDIParent1.mnu_New_ownSort_Reports_Main.Visible = False
        MDIParent1.Mnu_General_Reports_Main.Visible = False
        MDIParent1.mnu_new_own_Sort_Main.Visible = False

        MDIParent1.mnu_Opening_Sizing_Software_Main.Visible = False
        MDIParent1.mnu_Master_Sizing_Software_Main.Visible = False
        MDIParent1.mnu_Entries_Sizing_Software_Main.Visible = False
        MDIParent1.mnu_Reports_Sizing_Software_Main.Visible = False

        MDIParent1.mnu_Master_FP_Processing_Main.Visible = False
        MDIParent1.mnu_Entry_FP_Processing_Main.Visible = False
        MDIParent1.mnu_Report_FP_Processing_Main.Visible = False

        MDIParent1.mnu_Entry_Bobin_Main.Visible = False
        MDIParent1.mnu_Report_Bobin_Main.Visible = False

        MDIParent1.mnu_WindowsMenu_Main.Visible = True
        MDIParent1.ToolsMenu.Visible = False

        Me.Close()

    End Sub

    Private Sub btn_payroll_Click(sender As System.Object, e As System.EventArgs) Handles btn_payroll.Click
        Dim cn1 As SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand
        Dim vSPAutBckupEXE As String

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then   '---- Kalaimagal Textiles (Palladam)

            vSPAutBckupEXE = Trim(Common_Procedures.AppPath) & "\Payroll\Payroll.exe"
            If Trim(vSPAutBckupEXE) <> "" Then
                If System.IO.File.Exists(vSPAutBckupEXE) = False Then

                    'MessageBox.Show("Invalid : Payroll Module not installed properly", "INVALID PAYROLL EXE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub


                Else

                    cn1 = New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
                    cn1.Open()
                    cmd.Connection = cn1

                    cmd.CommandText = "delete from pyr_luh where csno = '" & Trim(Common_Procedures.HDD_SERIALNO) & "'"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "insert into pyr_luh(csno, usid, cgid) values ('" & Trim(Common_Procedures.HDD_SERIALNO) & "', " & Str(Val(Common_Procedures.User.IdNo)) & ", " & Str(Val(Common_Procedures.CompGroupIdNo)) & ")"
                    cmd.ExecuteNonQuery()

                    cmd.Dispose()

                    cn1.Close()
                    cn1.Dispose()

                    Shell(vSPAutBckupEXE, AppWinStyle.NormalFocus)

                    'Me.Close()
                    'Application.Exit()
                    'End
                    Exit Sub

                End If

            End If

        Else

            Common_Procedures.SoftwareType_Opened = Common_Procedures.SoftwareTypes.PayRoll_Software
            MDIParent1.lbl_Menu_name.Text = "PAYROLL"

            MDIParent1.Show()

            MDIParent1.lbl_Menu_name.Text = "PAYROLL"
            MDIParent1.Text = MDIParent1.lbl_Menu_name.Text & "  -  " & Common_Procedures.CompGroupIdNo & ". " & Common_Procedures.CompGroupName & " (" & Common_Procedures.CompGroupFnRange & ")           -            " & Common_Procedures.Company_FromDate & "   TO   " & Common_Procedures.Company_ToDate
            MDIParent1.lbl_CompanyName.Text = "TSOFT ERP"
            MDIParent1.lbl_SoftwareName.Text = "TSOFT Textile ERP Solutions"


            MDIParent1.mnu_Entry_PayRoll_Main.Visible = True
            MDIParent1.mnu_New_Payroll_Reports_Main.Visible = True

            MDIParent1.mnu_Home.Visible = True

            MDIParent1.mnu_CompanyMain.Visible = True
            MDIParent1.mnu_Opening_Main.Visible = False
            MDIParent1.mnu_Entry_Textile_Main.Visible = False
            MDIParent1.mnu_Master_Textile_JobWork_Main.Visible = False
            MDIParent1.mnu_Voucher_Main.Visible = False
            MDIParent1.mnu_Report_Textile_Main.Visible = False
            MDIParent1.mnu_New_master_Reports_Main.Visible = False
            MDIParent1.mnu_Reports_OE_Software_Main.Visible = False
            MDIParent1.mnu_Accounts_Main.Visible = False
            MDIParent1.mnu_Voucher_Main.Visible = False

            MDIParent1.mnu_new_Trading_Reports_Main.Visible = False
            MDIParent1.mnu_New_Trading_Main.Visible = False

            MDIParent1.mnu_Entry_JobWork_Main.Visible = False
            MDIParent1.mnu_Report_JobWork_Main.Visible = False
            MDIParent1.mnu_Vendor_Entry_Vendor_Main.Visible = False
            MDIParent1.mnu_New_ownSort_Reports_Main.Visible = False
            MDIParent1.mnu_Billing_Purchase_Entry.Visible = False
            MDIParent1.mnu_Billing_Reports.Visible = False
            MDIParent1.mnu_new_own_Sort_Main.Visible = False
            MDIParent1.Mnu_General_Reports_Main.Visible = False

            MDIParent1.mnu_Opening_Sizing_Software_Main.Visible = False
            MDIParent1.mnu_Master_Sizing_Software_Main.Visible = False
            MDIParent1.mnu_Entries_Sizing_Software_Main.Visible = False
            MDIParent1.mnu_Reports_Sizing_Software_Main.Visible = False

            MDIParent1.mnu_Master_FP_Processing_Main.Visible = False
            MDIParent1.mnu_Entry_FP_Processing_Main.Visible = False
            MDIParent1.mnu_Report_FP_Processing_Main.Visible = False

            MDIParent1.mnu_Entry_Bobin_Main.Visible = False
            MDIParent1.mnu_Report_Bobin_Main.Visible = False

            MDIParent1.mnu_WindowsMenu_Main.Visible = True
            MDIParent1.ToolsMenu.Visible = True

            Me.Close()

        End If


    End Sub

    Private Sub btn_ownsort_Click(sender As System.Object, e As System.EventArgs) Handles btn_ownsort.Click

        Common_Procedures.SoftwareType_Opened = Common_Procedures.SoftwareTypes.Textile_Software
        MDIParent1.lbl_Menu_name.Text = btn_ownsort.Text

        MDIParent1.Show()

        'MDIParent1.BackgroundImage = Textile.My.Resources.Resources.Mdi_Background
        'MDIParent1.BackColor = Color.FromArgb(28, 55, 91)
        'MDIParent1.BackgroundImageLayout = ImageLayout.Stretch

        MDIParent1.lbl_Menu_name.Text = btn_ownsort.Text
        MDIParent1.Text = MDIParent1.lbl_Menu_name.Text & "  -  " & Common_Procedures.CompGroupIdNo & ". " & Common_Procedures.CompGroupName & " (" & Common_Procedures.CompGroupFnRange & ")           -            " & Common_Procedures.Company_FromDate & "   TO   " & Common_Procedures.Company_ToDate
        MDIParent1.lbl_CompanyName.Text = "TSOFT ERP"
        MDIParent1.lbl_SoftwareName.Text = "TSOFT Textile ERP Solutions"

        MDIParent1.mnu_Home.Visible = True

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then  '---- UNITED WEAVES (PALLADAM)

            MDIParent1.mnu_new_own_Sort_Main.Visible = True
            MDIParent1.mnu_New_ownSort_Reports_Main.Visible = True

            MDIParent1.mnu_new_own_Sort_Main.Text = "Entry     "
            MDIParent1.mnu_New_ownSort_Reports_Main.Text = "Reports     "

            MDIParent1.mnu_Master_Textile_JobWork_Main.Visible = False
            MDIParent1.mnu_Entry_Textile_Main.Visible = False
            MDIParent1.mnu_Report_Textile_Main.Visible = False

            MDIParent1.mnu_Entry_JobWork_Main.Visible = False
            MDIParent1.mnu_Report_JobWork_Main.Visible = False

            MDIParent1.mnu_Voucher_Main.Visible = False
            MDIParent1.mnu_Accounts_Main.Visible = False

            MDIParent1.mnu_CompanyMain.Visible = False
            MDIParent1.mnu_Opening_Main.Visible = False

            MDIParent1.mnu_WindowsMenu_Main.Visible = True
            MDIParent1.ToolsMenu.Visible = False


        Else

            MDIParent1.mnu_CompanyMain.Visible = True
            MDIParent1.mnu_Action_Main.Visible = True
            MDIParent1.mnu_Master_Textile_JobWork_Main.Visible = True
            MDIParent1.mnu_Opening_Main.Visible = True
            MDIParent1.mnu_Entry_Textile_Main.Visible = True
            MDIParent1.mnu_Report_Textile_Main.Visible = True

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then '---- SRI SRINIVASA TEXTILES (PALLADAM)
                MDIParent1.mnu_Entry_JobWork_Main.Visible = False
                MDIParent1.mnu_Report_JobWork_Main.Visible = False
            Else
                MDIParent1.mnu_Entry_JobWork_Main.Visible = True
                MDIParent1.mnu_Report_JobWork_Main.Visible = True
            End If



            MDIParent1.mnu_Voucher_Main.Visible = True
            MDIParent1.mnu_Accounts_Main.Visible = True

            MDIParent1.mnu_New_ownSort_Reports_Main.Visible = False
            MDIParent1.mnu_new_own_Sort_Main.Visible = False

            MDIParent1.Mnu_General_Reports_Main.Visible = False
            MDIParent1.mnu_new_Trading_Reports_Main.Visible = False

            MDIParent1.mnu_WindowsMenu_Main.Visible = True
            MDIParent1.ToolsMenu.Visible = True

            MDIParent1.mnu_ExitMenu_Main.Visible = True

        End If

        MDIParent1.mnu_New_master_Reports_Main.Visible = False

        MDIParent1.mnu_Vendor_Entry_Vendor_Main.Visible = False

        MDIParent1.mnu_Master_OE_Software_Main.Visible = False
        MDIParent1.mnu_Reports_OE_Software_Main.Visible = False

        MDIParent1.mnu_New_Trading_Main.Visible = False

        MDIParent1.mnu_new_Trading_Reports_Main.Visible = False

        MDIParent1.Mnu_General_Reports_Main.Visible = False

        MDIParent1.mnu_Entry_PayRoll_Main.Visible = False
        MDIParent1.mnu_New_Payroll_Reports_Main.Visible = False

        MDIParent1.mnu_Billing_Purchase_Entry.Visible = False
        MDIParent1.mnu_Billing_Reports.Visible = False

        MDIParent1.mnu_Entries_Sizing_Software_Main.Visible = False
        MDIParent1.mnu_Reports_Sizing_Software_Main.Visible = False

        MDIParent1.mnu_Master_FP_Processing_Main.Visible = False
        MDIParent1.mnu_Entry_FP_Processing_Main.Visible = False
        MDIParent1.mnu_Report_FP_Processing_Main.Visible = False

        MDIParent1.mnu_Entry_Bobin_Main.Visible = False
        MDIParent1.mnu_Report_Bobin_Main.Visible = False

        MDIParent1.mnu_Entry_Rewinding.Visible = False

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then  '----    SRI SAKTHI VINAYAGA WEAVES PVT.LTD
            MDIParent1.mnu_New_ownSort_Reports_Main.Visible = True
            MDIParent1.Mnu_General_Reports_Main.Visible = True
            MDIParent1.mnu_new_Trading_Reports_Main.Visible = True

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)

            MDIParent1.mnu_Master_Textile_JobWork_Main.Visible = True
            MDIParent1.mnu_Master_Textile_JobWork_Main.Text = "Master     "

            MDIParent1.mnu_New_ownSort_Reports_Main.Visible = True
            MDIParent1.mnu_New_ownSort_Reports_Main.Text = "OwnSort Reports     "

            MDIParent1.mnu_Report_Textile_Main.Visible = True
            MDIParent1.mnu_Report_Textile_Main.Text = "General Reports     "

            MDIParent1.Mnu_General_Reports_Main.Visible = True
            MDIParent1.Mnu_General_Reports_Main.Text = "MIS Reports     "

        End If

        Me.Close()

    End Sub

    Private Sub btn_opening_Click(sender As System.Object, e As System.EventArgs) Handles btn_opening.Click

        Common_Procedures.SoftwareType_Opened = Common_Procedures.SoftwareTypes.Textile_Software
        MDIParent1.lbl_Menu_name.Text = "OPENING"

        MDIParent1.Show()

        MDIParent1.mnu_CompanyMain.Visible = False
        MDIParent1.mnu_Opening_Main.Visible = False
        MDIParent1.mnu_Entry_Textile_Main.Visible = False
        MDIParent1.mnu_Master_Textile_JobWork_Main.Visible = False
        MDIParent1.mnu_Voucher_Main.Visible = False
        MDIParent1.mnu_Report_Textile_Main.Visible = False
        MDIParent1.mnu_New_master_Reports_Main.Visible = False
        MDIParent1.mnu_Reports_OE_Software_Main.Visible = False
        MDIParent1.mnu_Accounts_Main.Visible = False
        MDIParent1.mnu_Voucher_Main.Visible = False
        MDIParent1.mnu_Home.Visible = True

        MDIParent1.mnu_New_Trading_Main.Visible = False

        MDIParent1.mnu_Entry_JobWork_Main.Visible = False
        MDIParent1.mnu_Report_JobWork_Main.Visible = False

        MDIParent1.mnu_New_ownSort_Reports_Main.Visible = False

        MDIParent1.mnu_new_own_Sort_Main.Visible = False

        MDIParent1.mnu_Opening_Main.Visible = True

        MDIParent1.mnu_Vendor_Entry_Vendor_Main.Visible = False

        MDIParent1.mnu_Entry_PayRoll_Main.Visible = False
        MDIParent1.mnu_New_Payroll_Reports_Main.Visible = False
        MDIParent1.Mnu_General_Reports_Main.Visible = False
        MDIParent1.mnu_new_Trading_Reports_Main.Visible = False
        MDIParent1.mnu_Billing_Purchase_Entry.Visible = False
        MDIParent1.mnu_Billing_Reports.Visible = False

        MDIParent1.mnu_Master_FP_Processing_Main.Visible = False
        MDIParent1.mnu_Entry_FP_Processing_Main.Visible = False
        MDIParent1.mnu_Report_FP_Processing_Main.Visible = False

        MDIParent1.mnu_Entry_Bobin_Main.Visible = False
        MDIParent1.mnu_Report_Bobin_Main.Visible = False

        MDIParent1.mnu_Reports_Sizing_Software_Main.Visible = False
        MDIParent1.mnu_WindowsMenu_Main.Visible = True
        MDIParent1.ToolsMenu.Visible = False
        MDIParent1.lbl_Menu_name.Text = "OPENING"
        MDIParent1.Text = MDIParent1.lbl_Menu_name.Text & "  -  " & Common_Procedures.CompGroupIdNo & ". " & Common_Procedures.CompGroupName & " (" & Common_Procedures.CompGroupFnRange & ")           -            " & Common_Procedures.Company_FromDate & "   TO   " & Common_Procedures.Company_ToDate
        MDIParent1.lbl_CompanyName.Text = "TSOFT ERP"
        MDIParent1.lbl_SoftwareName.Text = "TSOFT Textile ERP Solutions"

        Me.Close()
    End Sub

    Private Sub bth_gen_reports_Click(sender As System.Object, e As System.EventArgs) Handles bth_gen_reports.Click
        Common_Procedures.SoftwareType_Opened = Common_Procedures.SoftwareTypes.Textile_Software
        MDIParent1.lbl_Menu_name.Text = "GENERAL REPORTS"
        MDIParent1.Show()
        MDIParent1.mnu_CompanyMain.Visible = False
        MDIParent1.mnu_Opening_Main.Visible = False
        MDIParent1.mnu_Entry_Textile_Main.Visible = False
        MDIParent1.mnu_Master_Textile_JobWork_Main.Visible = False
        MDIParent1.mnu_Voucher_Main.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then  '---- MANI OMEGA FABRICS (THIRUCHENKODU)
            MDIParent1.mnu_Report_Textile_Main.Visible = True
            MDIParent1.mnu_Report_Textile_Main.Text = "General Reports"
            MDIParent1.mnu_Report_Textile_WeaverStock_Main_LN.Visible = False
            MDIParent1.mnu_Report_Textile_WeaverStock_Main.Visible = False

            MDIParent1.mnu_Report_Textile_Register_Weaver_Main_LN.Visible = False
            MDIParent1.mnu_Report_Textile_Register_Weaver_Main.Visible = False

        Else
            MDIParent1.mnu_Report_Textile_Main.Visible = False

        End If
        MDIParent1.mnu_New_master_Reports_Main.Visible = False
        MDIParent1.mnu_Reports_OE_Software_Main.Visible = False
        MDIParent1.mnu_Accounts_Main.Visible = False


        MDIParent1.mnu_Home.Visible = True
        MDIParent1.mnu_Voucher_Main.Visible = False
        MDIParent1.mnu_New_Trading_Main.Visible = False

        MDIParent1.mnu_Entry_JobWork_Main.Visible = False
        MDIParent1.mnu_Report_JobWork_Main.Visible = False

        MDIParent1.mnu_New_ownSort_Reports_Main.Visible = False
        MDIParent1.mnu_Vendor_Entry_Vendor_Main.Visible = False

        MDIParent1.mnu_new_own_Sort_Main.Visible = False
        MDIParent1.mnu_Opening_Main.Visible = False

        MDIParent1.mnu_Entry_PayRoll_Main.Visible = False
        MDIParent1.mnu_New_Payroll_Reports_Main.Visible = False
        MDIParent1.mnu_Billing_Purchase_Entry.Visible = False
        MDIParent1.mnu_Billing_Reports.Visible = False
        MDIParent1.mnu_new_Trading_Reports_Main.Visible = False
        MDIParent1.Mnu_General_Reports_Main.Visible = True
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then  '---- MANI OMEGA FABRICS (THIRUCHENKODU)
            MDIParent1.mnu_Report_Textile_Main.Visible = True
            MDIParent1.mnu_Report_Textile_Main.Text = "General Reports"

            MDIParent1.Mnu_General_Reports_Main.Visible = True
            MDIParent1.Mnu_General_Reports_Main.Text = "MIS Reports"
        End If

        MDIParent1.mnu_Master_Sizing_Software_Main.Visible = False
        MDIParent1.mnu_Opening_Sizing_Software_Main.Visible = False
        MDIParent1.mnu_Entries_Sizing_Software_Main.Visible = False
        MDIParent1.mnu_Reports_Sizing_Software_Main.Visible = False

        MDIParent1.mnu_Master_FP_Processing_Main.Visible = False
        MDIParent1.mnu_Entry_FP_Processing_Main.Visible = False
        MDIParent1.mnu_Report_FP_Processing_Main.Visible = False

        MDIParent1.mnu_Entry_Bobin_Main.Visible = False
        MDIParent1.mnu_Report_Bobin_Main.Visible = False

        MDIParent1.mnu_WindowsMenu_Main.Visible = True
        MDIParent1.ToolsMenu.Visible = False
        MDIParent1.lbl_Menu_name.Text = "GENERAL REPORTS"
        MDIParent1.Text = MDIParent1.lbl_Menu_name.Text & "  -  " & Common_Procedures.CompGroupIdNo & ". " & Common_Procedures.CompGroupName & " (" & Common_Procedures.CompGroupFnRange & ")           -            " & Common_Procedures.Company_FromDate & "   TO   " & Common_Procedures.Company_ToDate
        MDIParent1.lbl_CompanyName.Text = "TSOFT ERP"
        MDIParent1.lbl_SoftwareName.Text = "TSOFT Textile ERP Solutions"

        Me.Close()

    End Sub



    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub



    Private Sub btn_settings_Click(sender As System.Object, e As System.EventArgs) Handles btn_settings.Click
        MDIParent1.lbl_Menu_name.Text = "SETTINGS"
        MDIParent1.Show()
        MDIParent1.mnu_CompanyMain.Visible = False
        MDIParent1.mnu_Opening_Main.Visible = False
        MDIParent1.mnu_Entry_Textile_Main.Visible = False
        MDIParent1.mnu_Master_Textile_JobWork_Main.Visible = False
        MDIParent1.mnu_Voucher_Main.Visible = False
        MDIParent1.mnu_Report_Textile_Main.Visible = False
        MDIParent1.mnu_New_master_Reports_Main.Visible = False
        MDIParent1.mnu_Reports_OE_Software_Main.Visible = False
        MDIParent1.mnu_Accounts_Main.Visible = False

        MDIParent1.mnu_Home.Visible = True
        MDIParent1.mnu_Voucher_Main.Visible = False
        MDIParent1.mnu_New_Trading_Main.Visible = False

        MDIParent1.mnu_Entry_JobWork_Main.Visible = False
        MDIParent1.mnu_Report_JobWork_Main.Visible = False

        MDIParent1.mnu_New_ownSort_Reports_Main.Visible = False

        MDIParent1.mnu_new_own_Sort_Main.Visible = False
        MDIParent1.mnu_Opening_Main.Visible = False

        MDIParent1.mnu_Master_FP_Processing_Main.Visible = False
        MDIParent1.mnu_Entry_FP_Processing_Main.Visible = False
        MDIParent1.mnu_Report_FP_Processing_Main.Visible = False

        MDIParent1.mnu_Entry_Bobin_Main.Visible = False
        MDIParent1.mnu_Report_Bobin_Main.Visible = False

        MDIParent1.mnu_Entry_PayRoll_Main.Visible = False
        MDIParent1.mnu_New_Payroll_Reports_Main.Visible = False
        MDIParent1.mnu_Billing_Purchase_Entry.Visible = False
        MDIParent1.mnu_Billing_Reports.Visible = False
        MDIParent1.mnu_new_Trading_Reports_Main.Visible = False
        MDIParent1.Mnu_General_Reports_Main.Visible = False
        MDIParent1.mnu_Reports_Sizing_Software_Main.Visible = False
        MDIParent1.mnu_WindowsMenu_Main.Visible = True
        MDIParent1.ToolsMenu.Visible = True
        MDIParent1.lbl_Menu_name.Text = "SETTINGS"
        MDIParent1.Text = MDIParent1.lbl_Menu_name.Text & "  -  " & Common_Procedures.CompGroupIdNo & ". " & Common_Procedures.CompGroupName & " (" & Common_Procedures.CompGroupFnRange & ")           -            " & Common_Procedures.Company_FromDate & "   TO   " & Common_Procedures.Company_ToDate
        MDIParent1.lbl_CompanyName.Text = "TSOFT ERP"
        MDIParent1.lbl_SoftwareName.Text = "TSOFT Textile ERP Solutions"

        Me.Close()
    End Sub

    Private Sub btn_exit_Click(sender As Object, e As System.EventArgs) Handles btn_exit.Click
        Entrance.Show()
        Me.Close()

    End Sub

    Private Sub Btn_Company_Click(sender As System.Object, e As System.EventArgs) Handles Btn_Company.Click
        MDIParent1.lbl_Menu_name.Text = "COMPANY CREATION"
        MDIParent1.Show()
        MDIParent1.mnu_CompanyMain.Visible = True
        MDIParent1.mnu_Opening_Main.Visible = False
        MDIParent1.mnu_Entry_Textile_Main.Visible = False
        MDIParent1.mnu_Master_Textile_JobWork_Main.Visible = False
        MDIParent1.mnu_Voucher_Main.Visible = False
        MDIParent1.mnu_Report_Textile_Main.Visible = False
        MDIParent1.mnu_New_master_Reports_Main.Visible = False
        MDIParent1.mnu_Reports_OE_Software_Main.Visible = False
        MDIParent1.mnu_Accounts_Main.Visible = False

        MDIParent1.mnu_Home.Visible = True
        MDIParent1.mnu_Voucher_Main.Visible = False
        MDIParent1.mnu_New_Trading_Main.Visible = False

        MDIParent1.mnu_Entry_JobWork_Main.Visible = False
        MDIParent1.mnu_Report_JobWork_Main.Visible = False

        MDIParent1.mnu_New_ownSort_Reports_Main.Visible = False

        MDIParent1.mnu_new_own_Sort_Main.Visible = False
        MDIParent1.mnu_Opening_Main.Visible = False

        MDIParent1.mnu_Entry_PayRoll_Main.Visible = False
        MDIParent1.mnu_New_Payroll_Reports_Main.Visible = False
        MDIParent1.mnu_Billing_Purchase_Entry.Visible = False
        MDIParent1.mnu_Billing_Reports.Visible = False
        MDIParent1.mnu_new_Trading_Reports_Main.Visible = False
        MDIParent1.Mnu_General_Reports_Main.Visible = False

        MDIParent1.mnu_Master_FP_Processing_Main.Visible = False
        MDIParent1.mnu_Entry_FP_Processing_Main.Visible = False
        MDIParent1.mnu_Report_FP_Processing_Main.Visible = False

        MDIParent1.mnu_Entry_Bobin_Main.Visible = False
        MDIParent1.mnu_Report_Bobin_Main.Visible = False

        MDIParent1.mnu_WindowsMenu_Main.Visible = True
        MDIParent1.ToolsMenu.Visible = False

        MDIParent1.mnu_Action_Show_Dashboard.Visible = True

        MDIParent1.mnu_Reports_Sizing_Software_Main.Visible = False


        MDIParent1.lbl_Menu_name.Text = "COMPANY CREATION"
        MDIParent1.Text = MDIParent1.lbl_Menu_name.Text & "  -  " & Common_Procedures.CompGroupIdNo & ". " & Common_Procedures.CompGroupName & " (" & Common_Procedures.CompGroupFnRange & ")           -            " & Common_Procedures.Company_FromDate & "   TO   " & Common_Procedures.Company_ToDate
        MDIParent1.lbl_CompanyName.Text = "TSOFT ERP"
        MDIParent1.lbl_SoftwareName.Text = "TSOFT Textile ERP Solutions"


        Me.Close()

    End Sub


    Private Sub btn_OE_Click(sender As System.Object, e As System.EventArgs) Handles btn_OE.Click

        Common_Procedures.SoftwareType_Opened = Common_Procedures.SoftwareTypes.OE_Software
        MDIParent1.lbl_Menu_name.Text = "SPINNING"

        MDIParent1.Show()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1088" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1417" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then
            MDIParent1.BackgroundImage = Textile.My.Resources.Resources.backnew2
            MDIParent1.BackColor = Color.FromArgb(28, 55, 91)
            MDIParent1.BackgroundImageLayout = ImageLayout.Stretch
        End If

        If Common_Procedures.settings.Bobin_Zari_Kuri_Entries_Status = 1 And btn_OE.Text = "BOBIN" Then
            MDIParent1.lbl_Menu_name.Text = "BOBIN"

            MDIParent1.mnu_Entry_Bobin_Main.Visible = True
            MDIParent1.mnu_Report_Bobin_Main.Visible = True
            MDIParent1.mnu_Entry_Bobin_Main.Text = "&Entries     "
            MDIParent1.mnu_Report_Bobin_Main.Text = "&Reports     "

            MDIParent1.mnu_Voucher_Main.Visible = False
            MDIParent1.mnu_Accounts_Main.Visible = False
            MDIParent1.ToolsMenu.Visible = False

        Else

            MDIParent1.lbl_Menu_name.Text = "OE SPINNING"

            MDIParent1.mnu_CompanyMain.Visible = True

            MDIParent1.mnu_Master_OE_Software_Main.Text = "&Master     "
            MDIParent1.mnu_Opening_OE_Software_Main.Text = "&Opening     "
            MDIParent1.mnu_Entries_OE_Software_Main.Text = "&Entries     "
            MDIParent1.mnu_Reports_OE_Software_Main.Text = "&Reports     "

            MDIParent1.mnu_Master_OE_Software_Main.Visible = True
            MDIParent1.mnu_Opening_OE_Software_Main.Visible = True
            MDIParent1.mnu_Entries_OE_Software_Main.Visible = True
            MDIParent1.mnu_Reports_OE_Software_Main.Visible = True

            MDIParent1.mnu_Voucher_Main.Visible = True
            MDIParent1.mnu_Accounts_Main.Visible = True

            MDIParent1.ToolsMenu.Visible = True


            MDIParent1.mnu_Entry_Bobin_Main.Visible = False
            MDIParent1.mnu_Report_Bobin_Main.Visible = False

        End If

        MDIParent1.Text = MDIParent1.lbl_Menu_name.Text & "  -  " & Common_Procedures.CompGroupIdNo & ". " & Common_Procedures.CompGroupName & " (" & Common_Procedures.CompGroupFnRange & ")           -            " & Common_Procedures.Company_FromDate & "   TO   " & Common_Procedures.Company_ToDate
        MDIParent1.lbl_CompanyName.Text = "TSOFT ERP"
        MDIParent1.lbl_SoftwareName.Text = "TSOFT Textile ERP Solutions"

        MDIParent1.mnu_Action_Main.Visible = True

        MDIParent1.mnu_WindowsMenu_Main.Visible = True
        MDIParent1.mnu_Home.Visible = True
        MDIParent1.mnu_ExitMenu_Main.Visible = True


        MDIParent1.mnu_Opening_Main.Visible = False
        MDIParent1.mnu_Entry_Textile_Main.Visible = False
        MDIParent1.mnu_Master_Textile_JobWork_Main.Visible = False

        MDIParent1.mnu_Report_Textile_Main.Visible = False
        MDIParent1.mnu_New_master_Reports_Main.Visible = False

        MDIParent1.mnu_New_Trading_Main.Visible = False

        MDIParent1.mnu_Entry_JobWork_Main.Visible = False
        MDIParent1.mnu_Report_JobWork_Main.Visible = False

        MDIParent1.mnu_New_ownSort_Reports_Main.Visible = False

        MDIParent1.mnu_new_own_Sort_Main.Visible = False
        MDIParent1.mnu_Opening_Main.Visible = False

        MDIParent1.mnu_Vendor_Entry_Vendor_Main.Visible = False

        MDIParent1.mnu_Entry_PayRoll_Main.Visible = False
        MDIParent1.mnu_New_Payroll_Reports_Main.Visible = False
        MDIParent1.mnu_Billing_Purchase_Entry.Visible = False
        MDIParent1.mnu_Billing_Reports.Visible = False
        MDIParent1.mnu_new_Trading_Reports_Main.Visible = False
        MDIParent1.Mnu_General_Reports_Main.Visible = False

        MDIParent1.mnu_Action_Show_Dashboard.Visible = False

        MDIParent1.mnu_Entry_OE_Main.Visible = False

        MDIParent1.mnu_Master_Sizing_Software_Main.Visible = False
        MDIParent1.mnu_Opening_Sizing_Software_Main.Visible = False
        MDIParent1.mnu_Entries_Sizing_Software_Main.Visible = False
        MDIParent1.mnu_Reports_Sizing_Software_Main.Visible = False

        MDIParent1.mnu_Master_FP_Processing_Main.Visible = False
        MDIParent1.mnu_Entry_FP_Processing_Main.Visible = False
        MDIParent1.mnu_Report_FP_Processing_Main.Visible = False

        MDIParent1.mnu_Entry_Stores_Main.Visible = False
        MDIParent1.mnu_Reports_Stores_Main.Visible = False

        MDIParent1.Mnu_MIS_Reports_Main.Visible = False

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then 'KRG TEXTILE MILLS

            MDIParent1.mnu_OE_Entry_Cotton_Purchase.Text = "A. Fibre Purchase"
            MDIParent1.mnu_OE_Reports_Cotton_Stock.Text = "B. Fibre Stock"
            MDIParent1.mnu_OE_Reports_Register_Cotton_Purchase_Register.Text = "A. Fibre Purchase Register"
            MDIParent1.mnu_OE_Reports_Register_Cotton_purchase_Summary.Text = "B. Fibre Purchase Summary"
            MDIParent1.mnu_OE_Report_Cotton_Stock_Details.Text = "A. Fibre Stock Register"
            MDIParent1.mnu_OE_Reports_Cotton_Stock_Summary.Text = "B. Fibre Stock Summary"
            MDIParent1.mnu_Report_Cotton_Stock_Jobwork_Cotton_Stock_Register.Text = "C. Jobwork Fibre Stock Register"
            MDIParent1.mnu_CottonStockLotwiseI.Text = "D. Fibre Stock Lotwise Type I"
            MDIParent1.mnu_CottonStockLotwiseII.Text = "E. Fibre Stock Lotwise Type II"

            MDIParent1.Mnu_Oe_Spinning_Production_Entry.Visible = False
            MDIParent1.Mnu_Oe_Spinning_Production_Entry.Tag = "INVISIBLE"

            MDIParent1.mnu_OE_master_TipType_Creation.Visible = True
            MDIParent1.mnu_OE_master_TipType_Creation.Tag = ""

            MDIParent1.mnu_OE_master_Manufacture_Creation.Visible = True
            MDIParent1.mnu_OE_master_Manufacture_Creation.Tag = ""

            MDIParent1.mnu_OE_master_Machine_Model_Creation.Visible = True
            MDIParent1.mnu_OE_master_Machine_Model_Creation.Tag = ""

            MDIParent1.mnu_OE_Entry_Vor_Prod_CountDetails_Carding.Visible = True
            MDIParent1.mnu_OE_Entry_Vor_Prod_CountDetails_Carding.Tag = ""

            MDIParent1.mnu_OE_Entry_Vor_Prod_CountDetails_Drawing.Visible = True
            MDIParent1.mnu_OE_Entry_Vor_Prod_CountDetails_Drawing.Tag = ""

            MDIParent1.mnu_OE_Entry_Vor_Prod_CountDetails_Vortex.Visible = True
            MDIParent1.mnu_OE_Entry_Vor_Prod_CountDetails_Vortex.Tag = ""

            MDIParent1.mnu_OE_Entry_Vor_Prod_MachineDetails_Carding.Visible = True
            MDIParent1.mnu_OE_Entry_Vor_Prod_MachineDetails_Carding.Tag = ""

            MDIParent1.mnu_OE_Entry_Vor_Prod_MachineDetails_Drawing.Visible = True
            MDIParent1.mnu_OE_Entry_Vor_Prod_MachineDetails_Drawing.Tag = ""

            MDIParent1.mnu_OE_Entry_Vor_Prod_MachineDetails_Vortex.Visible = True
            MDIParent1.mnu_OE_Entry_Vor_Prod_MachineDetails_Vortex.Tag = ""

            MDIParent1.Mnu_OE_Entry_Spinning_Vortex_Production_Main.Visible = True
            MDIParent1.Mnu_OE_Entry_Spinning_Vortex_Production_Main.Tag = ""

            MDIParent1.Mnu_OE_Entry_Spinning_Vortex_Production_Entry.Visible = True
            MDIParent1.Mnu_OE_Entry_Spinning_Vortex_Production_Entry.Tag = ""

            MDIParent1.mnu_OE_Entry_Vortex_Production_Count_Details.Visible = True
            MDIParent1.mnu_OE_Entry_Vortex_Production_Count_Details.Tag = ""

            MDIParent1.mnu_OE_Entry_Vortex_Production_Machine_Details.Visible = True
            MDIParent1.mnu_OE_Entry_Vortex_Production_Machine_Details.Tag = ""


        End If
        Me.Close()

    End Sub

    Private Sub btn_Sizing_Click(sender As Object, e As EventArgs) Handles btn_Sizing.Click

        Common_Procedures.SoftwareType_Opened = Common_Procedures.SoftwareTypes.Sizing_Software
        MDIParent1.lbl_Menu_name.Text = "SIZING"
        MDIParent1.Show()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1078" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1088" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1417" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then
            MDIParent1.BackgroundImage = Textile.My.Resources.Resources.BackGround1
            MDIParent1.BackgroundImageLayout = ImageLayout.Stretch
            MDIParent1.BackColor = Color.White
        End If

        MDIParent1.lbl_Menu_name.Text = "SIZING"
        MDIParent1.Text = MDIParent1.lbl_Menu_name.Text & "  -  " & Common_Procedures.CompGroupIdNo & ". " & Common_Procedures.CompGroupName & " (" & Common_Procedures.CompGroupFnRange & ")           -            " & Common_Procedures.Company_FromDate & "   TO   " & Common_Procedures.Company_ToDate
        MDIParent1.lbl_CompanyName.Text = "TSOFT ERP"
        MDIParent1.lbl_SoftwareName.Text = "TSOFT Textile ERP Solutions"

        MDIParent1.mnu_CompanyMain.Visible = True
        MDIParent1.mnu_Action_Main.Visible = True


        MDIParent1.mnu_Master_Sizing_Software_Main.Text = "&Master     "
        MDIParent1.mnu_Opening_Sizing_Software_Main.Text = "&Opening     "
        MDIParent1.mnu_Entries_Sizing_Software_Main.Text = "&Entries     "
        MDIParent1.mnu_Reports_Sizing_Software_Main.Text = "&Reports     "

        MDIParent1.mnu_Master_Sizing_Software_Main.Visible = True
        MDIParent1.mnu_Opening_Sizing_Software_Main.Visible = True
        MDIParent1.mnu_Entries_Sizing_Software_Main.Visible = True
        MDIParent1.mnu_Reports_Sizing_Software_Main.Visible = True


        MDIParent1.mnu_Voucher_Main.Visible = True
        MDIParent1.mnu_Accounts_Main.Visible = True


        MDIParent1.mnu_Vendor_Entry_Vendor_Main.Visible = False

        MDIParent1.mnu_WindowsMenu_Main.Visible = True
        MDIParent1.ToolsMenu.Visible = True
        MDIParent1.mnu_Home.Visible = True
        MDIParent1.mnu_ExitMenu_Main.Visible = True


        'MDIParent1.mnu_Opening_Main.Visible = False
        MDIParent1.mnu_Entry_Textile_Main.Visible = False
        MDIParent1.mnu_Master_Textile_JobWork_Main.Visible = False

        MDIParent1.mnu_Report_Textile_Main.Visible = False
        MDIParent1.mnu_New_master_Reports_Main.Visible = False

        MDIParent1.mnu_New_Trading_Main.Visible = False

        MDIParent1.mnu_Entry_JobWork_Main.Visible = False
        MDIParent1.mnu_Report_JobWork_Main.Visible = False

        MDIParent1.mnu_New_ownSort_Reports_Main.Visible = False

        MDIParent1.mnu_new_own_Sort_Main.Visible = False
        MDIParent1.mnu_Opening_Main.Visible = False

        MDIParent1.mnu_Entry_PayRoll_Main.Visible = False
        MDIParent1.mnu_New_Payroll_Reports_Main.Visible = False
        MDIParent1.mnu_Billing_Purchase_Entry.Visible = False
        MDIParent1.mnu_Billing_Reports.Visible = False
        MDIParent1.mnu_new_Trading_Reports_Main.Visible = False
        MDIParent1.Mnu_General_Reports_Main.Visible = False



        MDIParent1.mnu_Action_Show_Dashboard.Visible = False

        MDIParent1.mnu_Entries_OE_Software_Main.Visible = False
        MDIParent1.mnu_Reports_OE_Software_Main.Visible = False
        MDIParent1.mnu_Entry_OE_Main.Visible = False
        MDIParent1.mnu_Entry_Bobin_Main.Visible = False
        MDIParent1.mnu_Report_Bobin_Main.Visible = False


        Me.Close()

    End Sub

    Private Sub btn_Stores_Click(sender As Object, e As EventArgs) Handles btn_Stores.Click

        Common_Procedures.SoftwareType_Opened = Common_Procedures.SoftwareTypes.Stores_Software
        MDIParent1.lbl_Menu_name.Text = "STORES"

        MDIParent1.Show()

        MDIParent1.lbl_Menu_name.Text = "STORES"
        MDIParent1.Text = MDIParent1.lbl_Menu_name.Text & "  -  " & Common_Procedures.CompGroupIdNo & ". " & Common_Procedures.CompGroupName & " (" & Common_Procedures.CompGroupFnRange & ")           -            " & Common_Procedures.Company_FromDate & "   TO   " & Common_Procedures.Company_ToDate
        MDIParent1.lbl_CompanyName.Text = "TSOFT ERP"
        MDIParent1.lbl_SoftwareName.Text = "TSOFT Textile ERP Solutions"

        MDIParent1.mnu_Entry_Stores_Main.Visible = True
        MDIParent1.mnu_Reports_Stores_Main.Visible = True
        MDIParent1.mnu_Home.Visible = True

        MDIParent1.mnu_Opening_Main.Visible = True
        MDIParent1.mnu_Opening_LedgerAmountBalance.Visible = False
        MDIParent1.mnu_Opening_Textile_OpeningStock.Visible = False
        MDIParent1.Mnu_Opening_Fibre_Stock.Visible = False
        MDIParent1.mnu_Opening_Textile_Closing_StockValue.Visible = False
        MDIParent1.mnu_Opening_Textile_UnChecked_Piece_Bale_Ln.Visible = False
        MDIParent1.mnu_Opening_Textile_UnCheckedClothOpening.Visible = False
        MDIParent1.mnu_Opening_Textile_PieceOpening.Visible = False
        MDIParent1.mnu_Opening_Textile_BaleOpening.Visible = False
        MDIParent1.mnu_Opening_Textile_LoomOpening.Visible = False
        MDIParent1.mnu_Opening_ClothSales_Order_Ln.Visible = False
        MDIParent1.mnu_Opening_ClothSales_Order.Visible = False
        MDIParent1.mnu_Opening_Cloth_Delivery.Visible = False
        MDIParent1.mnu_Opening_FP_Ln.Visible = False
        MDIParent1.mnu_Opening_FP_GodownOpening_GreyItem_Stock.Visible = False
        MDIParent1.mnu_Opening_FP_GodownOpening_FinishedProduct_Stock.Visible = False
        MDIParent1.mnu_Opening_Textile_Godown_ProcessedFabric_OpeningStock_Ln.Visible = False
        MDIParent1.mnu_Opening_Textile_Godown_ProcessedFabric_OpeningStock_Rolls_Bundles.Visible = False
        MDIParent1.mnu_Opening_Textile_Godown_ProcessedFabric_OpeningStock_Unchecked.Visible = False
        MDIParent1.mnu_Opening_ProcessingDelivery_Ln.Visible = False
        MDIParent1.mnu_Opening_Proc_ProcessingDelivery_Opening.Visible = False
        MDIParent1.mnu_Opening_SewingDelivery_Ln.Visible = False
        MDIParent1.mnu_Opening_SewingDelivery_Opening.Visible = False
        MDIParent1.mnu_Opening_Stores_Ln.Visible = False
        MDIParent1.mnu_Opening_StoresGodownOpeningStock.Visible = True
        MDIParent1.mnu_Opening_OE_Ln.Visible = False
        MDIParent1.mnu_Opening_OE_OpeningStock.Visible = False
        MDIParent1.mnu_Opening_Textile_Bundle_Entry_Ln.Visible = False
        MDIParent1.mnu_Opening_Textile_Bundle_Entry.Visible = False

        MDIParent1.mnu_CompanyMain.Visible = False

        MDIParent1.mnu_Entry_Textile_Main.Visible = False
        MDIParent1.mnu_Master_Textile_JobWork_Main.Visible = True
        MDIParent1.mnu_Voucher_Main.Visible = False
        MDIParent1.mnu_Report_Textile_Main.Visible = False
        MDIParent1.mnu_New_master_Reports_Main.Visible = False
        MDIParent1.mnu_Reports_OE_Software_Main.Visible = False
        MDIParent1.mnu_Accounts_Main.Visible = False

        MDIParent1.mnu_Vendor_Entry_Vendor_Main.Visible = False

        MDIParent1.mnu_New_Trading_Main.Visible = False

        MDIParent1.mnu_Entry_JobWork_Main.Visible = False
        MDIParent1.mnu_Report_JobWork_Main.Visible = False

        MDIParent1.mnu_New_ownSort_Reports_Main.Visible = False
        MDIParent1.mnu_new_Trading_Reports_Main.Visible = False
        MDIParent1.mnu_new_own_Sort_Main.Visible = False
        MDIParent1.Mnu_General_Reports_Main.Visible = False
        MDIParent1.mnu_Entry_PayRoll_Main.Visible = False
        MDIParent1.mnu_New_Payroll_Reports_Main.Visible = False
        MDIParent1.mnu_Voucher_Main.Visible = False
        MDIParent1.mnu_Billing_Purchase_Entry.Visible = False
        MDIParent1.mnu_Billing_Reports.Visible = False

        MDIParent1.mnu_Master_Sizing_Software_Main.Visible = False
        MDIParent1.mnu_Opening_Sizing_Software_Main.Visible = False
        MDIParent1.mnu_Entries_Sizing_Software_Main.Visible = False
        MDIParent1.mnu_Reports_Sizing_Software_Main.Visible = False

        MDIParent1.mnu_Master_FP_Processing_Main.Visible = False
        MDIParent1.mnu_Entry_FP_Processing_Main.Visible = False
        MDIParent1.mnu_Report_FP_Processing_Main.Visible = False

        MDIParent1.mnu_Entry_Bobin_Main.Visible = False
        MDIParent1.mnu_Report_Bobin_Main.Visible = False

        MDIParent1.mnu_WindowsMenu_Main.Visible = True
        MDIParent1.ToolsMenu.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then
            MDIParent1.ToolsMenu.Visible = True
        End If



        Me.Close()

    End Sub

    Private Sub btn_ShowAll_Modules_Click(sender As Object, e As EventArgs) Handles btn_ShowAll_Modules.Click
        Common_Procedures.SoftwareType_Opened = 0
        MDIParent1.lbl_Menu_name.Text = sender.Text

        MDIParent1.Show()

        MDIParent1.lbl_Menu_name.Text = sender.Text
        MDIParent1.Text = MDIParent1.lbl_Menu_name.Text & "  -  " & Common_Procedures.CompGroupIdNo & ". " & Common_Procedures.CompGroupName & " (" & Common_Procedures.CompGroupFnRange & ")           -            " & Common_Procedures.Company_FromDate & "   TO   " & Common_Procedures.Company_ToDate
        MDIParent1.lbl_CompanyName.Text = "TSOFT ERP"
        MDIParent1.lbl_SoftwareName.Text = "TSOFT Textile ERP Solutions"

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then  '---- UNITED WEAVES (PALLADAM)

            MDIParent1.mnu_CompanyMain.Visible = True
            MDIParent1.mnu_Action_Main.Visible = True

            MDIParent1.mnu_Master_Textile_JobWork_Main.Visible = True
            MDIParent1.mnu_Opening_Main.Visible = True

            MDIParent1.mnu_CompanyMain.Text = "Comp"
            MDIParent1.mnu_Action_Main.Text = "Act"

            MDIParent1.mnu_Master_Textile_JobWork_Main.Text = "OS.Mas"
            MDIParent1.mnu_Opening_Main.Text = "Opng"

            MDIParent1.mnu_New_master_Reports_Main.Visible = True
            MDIParent1.mnu_new_own_Sort_Main.Visible = True
            MDIParent1.mnu_New_ownSort_Reports_Main.Visible = True

            MDIParent1.mnu_Entry_Textile_Main.Visible = True
            MDIParent1.mnu_Entry_Textile_Main.Text = "Ent"

            MDIParent1.mnu_Report_Textile_Main.Visible = True
            MDIParent1.mnu_Report_Textile_Main.Text = "Rep"

            MDIParent1.mnu_new_own_Sort_Main.Text = "OS.Et"
            MDIParent1.mnu_New_ownSort_Reports_Main.Text = "OS.Rp"
            MDIParent1.mnu_New_master_Reports_Main.Text = "Mas.Rp"

            MDIParent1.mnu_New_Trading_Main.Visible = True
            MDIParent1.mnu_new_Trading_Reports_Main.Visible = True

            MDIParent1.mnu_New_Trading_Main.Text = "Trd.Et"
            MDIParent1.mnu_new_Trading_Reports_Main.Text = "Trd.Rp"

            MDIParent1.mnu_new_Jobwork_Main.Visible = True
            MDIParent1.mnu_new_Jobwork_Reports_Main.Visible = True

            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1186" Then  '---- UNITED WEAVES (PALLADAM)
            MDIParent1.mnu_Vendor_Entry_Vendor_Main.Visible = True
            MDIParent1.mnu_Vendor_Entry_Vendor_Main.Text = "Ven.Et"
            'End If

            MDIParent1.mnu_new_Jobwork_Main.Text = "JbWk.Et"
            MDIParent1.mnu_new_Jobwork_Reports_Main.Text = "JbWk.Rp"

            MDIParent1.mnu_Entry_Stores_Main.Visible = True
            MDIParent1.mnu_Reports_Stores_Main.Visible = True

            MDIParent1.mnu_Entry_Stores_Main.Text = "Sto.Et"
            MDIParent1.mnu_Reports_Stores_Main.Text = "Sto.Rp"

            MDIParent1.mnu_Entry_PayRoll_Main.Visible = True
            MDIParent1.mnu_New_Payroll_Reports_Main.Visible = True

            MDIParent1.mnu_Entry_PayRoll_Main.Text = "Pay.Et"
            MDIParent1.mnu_New_Payroll_Reports_Main.Text = "Pay.Rp"

            MDIParent1.mnu_Voucher_Main.Visible = True
            MDIParent1.mnu_Accounts_Main.Visible = True

            MDIParent1.mnu_Voucher_Main.Text = "Vou.Et"
            MDIParent1.mnu_Accounts_Main.Text = "Vou.Ac.Rp"

            MDIParent1.mnu_Billing_Purchase_Entry.Visible = True
            MDIParent1.mnu_Billing_Sales_Entry.Visible = True
            MDIParent1.mnu_Billing_Other_Voucher_Entry_Main.Visible = True
            MDIParent1.mnu_Billing_Reports.Visible = True
            MDIParent1.Mnu_General_Reports_Main.Visible = True

            MDIParent1.mnu_Billing_Purchase_Entry.Text = "Ac.Pu.Et"
            MDIParent1.mnu_Billing_Sales_Entry.Text = "Ac.Sl.Et"
            MDIParent1.mnu_Billing_Other_Voucher_Entry_Main.Text = "Ac.Oth.Ent"
            MDIParent1.mnu_Billing_Reports.Text = "Ac.Rep"
            MDIParent1.Mnu_General_Reports_Main.Text = "Gen.Rep"

            MDIParent1.mnu_Entry_JobWork_Main.Visible = False
            MDIParent1.mnu_Report_JobWork_Main.Visible = False

            MDIParent1.mnu_Master_OE_Software_Main.Visible = False
            MDIParent1.mnu_Opening_OE_Software_Main.Visible = False
            MDIParent1.mnu_Entries_OE_Software_Main.Visible = False
            MDIParent1.mnu_Reports_OE_Software_Main.Visible = False

            MDIParent1.mnu_Master_Sizing_Software_Main.Visible = False
            MDIParent1.mnu_Opening_Sizing_Software_Main.Visible = False
            MDIParent1.mnu_Entries_Sizing_Software_Main.Visible = False
            MDIParent1.mnu_Reports_Sizing_Software_Main.Visible = False

            MDIParent1.mnu_WindowsMenu_Main.Visible = True
            MDIParent1.ToolsMenu.Visible = True
            MDIParent1.mnu_Home.Visible = True
            MDIParent1.mnu_ExitMenu_Main.Visible = True

            MDIParent1.mnu_WindowsMenu_Main.Text = "Win"
            MDIParent1.ToolsMenu.Text = "Tls"
            MDIParent1.mnu_Home.Text = "Hom"
            MDIParent1.mnu_ExitMenu_Main.Text = "Ext"

            MDIParent1.mnu_Master_FP_Processing_Main.Visible = False
            MDIParent1.mnu_Entry_FP_Processing_Main.Visible = False
            MDIParent1.mnu_Report_FP_Processing_Main.Visible = False

            MDIParent1.mnu_Entry_Rewinding.Visible = False
            MDIParent1.mnu_Entry_Bobin_Main.Visible = False
            MDIParent1.mnu_Report_Bobin_Main.Visible = False

        Else

            MDIParent1.mnu_CompanyMain.Visible = True
            MDIParent1.mnu_Action_Main.Visible = True

            MDIParent1.mnu_Master_Textile_JobWork_Main.Visible = True
            MDIParent1.mnu_Opening_Main.Visible = True
            MDIParent1.mnu_Entry_Textile_Main.Visible = True
            MDIParent1.mnu_Report_Textile_Main.Visible = True
            MDIParent1.mnu_Entry_JobWork_Main.Visible = True
            MDIParent1.mnu_Report_JobWork_Main.Visible = True

            MDIParent1.mnu_CompanyMain.Text = "Comp"
            MDIParent1.mnu_Action_Main.Text = "Act"

            MDIParent1.mnu_Master_Textile_JobWork_Main.Text = "Tx.Ms"
            MDIParent1.mnu_Opening_Main.Text = "Tx.Op"
            MDIParent1.mnu_Entry_Textile_Main.Text = "Tx.Et"
            MDIParent1.mnu_Report_Textile_Main.Text = "Tx.Rp"
            MDIParent1.mnu_Entry_JobWork_Main.Text = "Tx.JW.Et"
            MDIParent1.mnu_Report_JobWork_Main.Text = "Tx.JW.Rp"


            MDIParent1.mnu_Master_OE_Software_Main.Visible = True
            MDIParent1.mnu_Opening_OE_Software_Main.Visible = True
            MDIParent1.mnu_Entries_OE_Software_Main.Visible = True
            MDIParent1.mnu_Reports_OE_Software_Main.Visible = True

            MDIParent1.mnu_Master_OE_Software_Main.Text = "&OE.Ms"
            MDIParent1.mnu_Opening_OE_Software_Main.Text = "&OE.Op"
            MDIParent1.mnu_Entries_OE_Software_Main.Text = "&OE.Et"
            MDIParent1.mnu_Reports_OE_Software_Main.Text = "&OE.Rp"


            MDIParent1.mnu_Master_Sizing_Software_Main.Visible = True
            MDIParent1.mnu_Opening_Sizing_Software_Main.Visible = True
            MDIParent1.mnu_Entries_Sizing_Software_Main.Visible = True
            MDIParent1.mnu_Reports_Sizing_Software_Main.Visible = True

            MDIParent1.mnu_Master_Sizing_Software_Main.Text = "Sz.Ms"
            MDIParent1.mnu_Opening_Sizing_Software_Main.Text = "Sz.Op"
            MDIParent1.mnu_Entries_Sizing_Software_Main.Text = "Sz.Et"
            MDIParent1.mnu_Reports_Sizing_Software_Main.Text = "Sz.Rp"

            MDIParent1.mnu_Entry_Stores_Main.Visible = True
            MDIParent1.mnu_Reports_Stores_Main.Visible = True

            MDIParent1.mnu_Entry_Stores_Main.Text = "Sto.Et"
            MDIParent1.mnu_Reports_Stores_Main.Text = "Sto.Rp"

            MDIParent1.mnu_Entry_PayRoll_Main.Visible = True
            MDIParent1.mnu_New_Payroll_Reports_Main.Visible = True

            MDIParent1.mnu_Entry_PayRoll_Main.Text = "Pay.Et"
            MDIParent1.mnu_New_Payroll_Reports_Main.Text = "Pay.Rp"

            MDIParent1.mnu_Voucher_Main.Visible = True
            MDIParent1.mnu_Accounts_Main.Visible = True

            MDIParent1.mnu_Voucher_Main.Text = "Vou.Et"
            MDIParent1.mnu_Accounts_Main.Text = "Ac.Rp"

            MDIParent1.mnu_WindowsMenu_Main.Visible = True
            MDIParent1.ToolsMenu.Visible = True
            MDIParent1.mnu_Home.Visible = True
            MDIParent1.mnu_ExitMenu_Main.Visible = True


            MDIParent1.mnu_WindowsMenu_Main.Text = "Win"
            MDIParent1.ToolsMenu.Text = "Tool"
            MDIParent1.mnu_Home.Text = "Hom"
            MDIParent1.mnu_ExitMenu_Main.Text = "Ext"


            MDIParent1.mnu_New_master_Reports_Main.Visible = False

            MDIParent1.mnu_New_ownSort_Reports_Main.Visible = False
            MDIParent1.mnu_new_own_Sort_Main.Visible = False

            MDIParent1.mnu_New_Trading_Main.Visible = False
            MDIParent1.mnu_new_Trading_Reports_Main.Visible = False

            MDIParent1.mnu_Billing_Purchase_Entry.Visible = False
            MDIParent1.mnu_Billing_Reports.Visible = False

            MDIParent1.Mnu_General_Reports_Main.Visible = False

            MDIParent1.mnu_Master_FP_Processing_Main.Visible = False
            MDIParent1.mnu_Entry_FP_Processing_Main.Visible = False
            MDIParent1.mnu_Report_FP_Processing_Main.Visible = False

            MDIParent1.mnu_Entry_Rewinding.Visible = False
            MDIParent1.mnu_Entry_Bobin_Main.Visible = False
            MDIParent1.mnu_Report_Bobin_Main.Visible = False

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1027" Then '---- Prem Textile (Somanur)
            MDIParent1.mnu_Entry_Bobin_Main.Visible = True
            MDIParent1.mnu_Report_Bobin_Main.Visible = True

            MDIParent1.mnu_Entry_Bobin_Main.Text = "Bob.Et "
            MDIParent1.mnu_Report_Bobin_Main.Text = "Bob.Rep "

        Else
            MDIParent1.mnu_Entry_Bobin_Main.Visible = False
            MDIParent1.mnu_Report_Bobin_Main.Visible = False
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1005" Then '---- Jeno Textiles (Somanur)
            MDIParent1.mnu_Master_FP_Processing_Main.Visible = True
            MDIParent1.mnu_Entry_FP_Processing_Main.Visible = True
            MDIParent1.mnu_Report_FP_Processing_Main.Visible = True
        Else
            MDIParent1.mnu_Master_FP_Processing_Main.Visible = False
            MDIParent1.mnu_Entry_FP_Processing_Main.Visible = False
            MDIParent1.mnu_Report_FP_Processing_Main.Visible = False
        End If

        MDIParent1.lbl_Menu_name.Visible = False

        Me.Close()

    End Sub

    Private Sub Menu_List_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If MessageBox.Show("Do you want to Close ?", "FOR CLOSE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                Me.Close()
            End If
        End If
    End Sub

    Private Sub btn_Vendor_Click(sender As Object, e As EventArgs) Handles btn_Vendor.Click

        Common_Procedures.SoftwareType_Opened = Common_Procedures.SoftwareTypes.Textile_Software
        MDIParent1.lbl_Menu_name.Text = btn_Vendor.Text

        MDIParent1.Show()

        'MDIParent1.BackgroundImage = Textile.My.Resources.Resources.Mdi_Background
        'MDIParent1.BackColor = Color.FromArgb(28, 55, 91)
        'MDIParent1.BackgroundImageLayout = ImageLayout.Stretch

        MDIParent1.lbl_Menu_name.Text = btn_Vendor.Text
        MDIParent1.Text = MDIParent1.lbl_Menu_name.Text & "  -  " & Common_Procedures.CompGroupIdNo & ". " & Common_Procedures.CompGroupName & " (" & Common_Procedures.CompGroupFnRange & ")           -            " & Common_Procedures.Company_FromDate & "   TO   " & Common_Procedures.Company_ToDate
        MDIParent1.lbl_CompanyName.Text = "TSOFT ERP"
        MDIParent1.lbl_SoftwareName.Text = "TSOFT Textile ERP Solutions"

        MDIParent1.mnu_Home.Visible = True

        MDIParent1.mnu_Vendor_Entry_Vendor_Main.Visible = True
        'MDIParent1.mnu_New_ownSort_Reports_Main.Visible = True

        MDIParent1.mnu_Vendor_Entry_Vendor_Main.Visible = True
        MDIParent1.mnu_Vendor_Entry_Vendor_Main.Text = "Entry     "
        'MDIParent1.mnu_New_ownSort_Reports_Main.Text = "Reports     "

        MDIParent1.mnu_Master_Textile_JobWork_Main.Visible = False
        MDIParent1.mnu_Entry_Textile_Main.Visible = False
        MDIParent1.mnu_Report_Textile_Main.Visible = True

        MDIParent1.mnu_Entry_JobWork_Main.Visible = False
        MDIParent1.mnu_Report_JobWork_Main.Visible = False

        MDIParent1.mnu_Voucher_Main.Visible = False
        MDIParent1.mnu_Accounts_Main.Visible = False

        MDIParent1.mnu_CompanyMain.Visible = False
        MDIParent1.mnu_Opening_Main.Visible = False

        MDIParent1.mnu_WindowsMenu_Main.Visible = True
        MDIParent1.ToolsMenu.Visible = False

        MDIParent1.mnu_New_master_Reports_Main.Visible = False

        MDIParent1.mnu_Master_OE_Software_Main.Visible = False
        MDIParent1.mnu_Reports_OE_Software_Main.Visible = False

        MDIParent1.mnu_New_Trading_Main.Visible = False

        MDIParent1.mnu_new_Trading_Reports_Main.Visible = False

        MDIParent1.Mnu_General_Reports_Main.Visible = False

        MDIParent1.mnu_Entry_PayRoll_Main.Visible = False
        MDIParent1.mnu_New_Payroll_Reports_Main.Visible = False

        MDIParent1.mnu_Billing_Purchase_Entry.Visible = False
        MDIParent1.mnu_Billing_Reports.Visible = False

        MDIParent1.mnu_Entries_Sizing_Software_Main.Visible = False
        MDIParent1.mnu_Reports_Sizing_Software_Main.Visible = False

        MDIParent1.mnu_Master_FP_Processing_Main.Visible = False
        MDIParent1.mnu_Entry_FP_Processing_Main.Visible = False
        MDIParent1.mnu_Report_FP_Processing_Main.Visible = False

        MDIParent1.mnu_Entry_Bobin_Main.Visible = False
        MDIParent1.mnu_Report_Bobin_Main.Visible = False

        MDIParent1.mnu_Entry_Rewinding.Visible = False

        MDIParent1.mnu_Report_Textile_For_MD.Visible = False
        MDIParent1.mnu_Report_Textile_For_MD.Tag = "INVISIBLE"

        MDIParent1.mnu_Report_Textile_For_MD_Ln.Visible = False
        MDIParent1.mnu_Report_Textile_For_MD_Ln.Tag = "INVISIBLE"

        MDIParent1.mnu_Report_Textile_Masters_Main.Visible = False
        MDIParent1.mnu_Report_Textile_Masters_Main.Tag = "INVISIBLE"

        MDIParent1.mnu_Report_Textile_Masters_Main_Ln.Visible = False
        MDIParent1.mnu_Report_Textile_Masters_Main_Ln.Tag = "INVISIBLE"

        MDIParent1.mnu_Report_Textile_Register_Main.Visible = True
        MDIParent1.mnu_Report_Textile_Register_Main.Tag = "INVISIBLE"

        MDIParent1.mnu_Report_Ln1.Visible = False
        MDIParent1.mnu_Report_Ln1.Tag = "INVISIBLE"

        MDIParent1.mnu_Report_Textile_Sizing_Stock_Main.Visible = False
        MDIParent1.mnu_Report_Textile_Sizing_Stock_Main.Tag = "INVISIBLE"

        MDIParent1.mnu_Report_Ln2.Visible = False
        MDIParent1.mnu_Report_Ln2.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_RewindingStock_Main.Visible = False
        MDIParent1.mnu_Report_Textile_RewindingStock_Main.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_WeaverStock_Main_LN.Visible = True
        MDIParent1.mnu_Report_Textile_WeaverStock_Main_LN.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_WeaverStock_Main.Visible = True
        MDIParent1.mnu_Report_Textile_WeaverStock_Main.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Ln4.Visible = False
        MDIParent1.mnu_Report_Ln4.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_GodownStock_Main.Visible = False
        MDIParent1.mnu_Report_Textile_GodownStock_Main.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_InHouse_Ln.Visible = False
        MDIParent1.mnu_Report_Textile_InHouse_Ln.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_InHouse_Main.Visible = False
        MDIParent1.mnu_Report_Textile_InHouse_Main.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_Day_Transaction_Details_Ln.Visible = False
        MDIParent1.mnu_Report_Textile_Day_Transaction_Details_Ln.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_Day_Transaction_Main.Visible = False
        MDIParent1.mnu_Report_Textile_Day_Transaction_Main.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Ln6.Visible = False
        MDIParent1.mnu_Report_Ln6.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_ClothOrderIndent_Pending_Main.Visible = False
        MDIParent1.mnu_Report_Textile_ClothOrderIndent_Pending_Main.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_ClothDeliveryPending.Visible = False
        MDIParent1.mnu_Report_Textile_ClothDeliveryPending.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Ln7.Visible = False
        MDIParent1.mnu_Report_Ln7.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_GST_Return_Reports.Visible = False
        MDIParent1.mnu_Report_GST_Return_Reports.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_AnnexureReport_Main.Visible = False
        MDIParent1.mnu_Report_Textile_AnnexureReport_Main.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Ln8.Visible = False
        MDIParent1.mnu_Report_Ln8.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_TDSReport_Main.Visible = False
        MDIParent1.mnu_Report_Textile_TDSReport_Main.Tag = "INVISIBLE"
        MDIParent1.mnu_Reports_Textile_TCS_Report_Main.Visible = False
        MDIParent1.mnu_Reports_Textile_TCS_Report_Main.Tag = "INVISIBLE"
        MDIParent1.mnu_report_ln11.Visible = False
        MDIParent1.mnu_report_ln11.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_StockValue.Visible = False
        MDIParent1.mnu_Report_Textile_StockValue.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Ln10.Visible = False
        MDIParent1.mnu_Report_Ln10.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_All_StockStatement.Visible = False
        MDIParent1.mnu_Report_Textile_All_StockStatement.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_Processing_Stock_Main_Ln.Visible = False
        MDIParent1.mnu_Report_Textile_Processing_Stock_Main_Ln.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_Processing_Stock_Main.Visible = False
        MDIParent1.mnu_Report_Textile_Processing_Stock_Main.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_Yarn_Process_Main1_Ln.Visible = False
        MDIParent1.mnu_Report_Textile_Yarn_Process_Main1.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_Sewing_Stock_Main_Ln.Visible = False
        MDIParent1.mnu_Report_Textile_Sewing_Stock_Main_Ln.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_Sewing_Stock_Main.Visible = False
        MDIParent1.mnu_Report_Textile_Sewing_Stock_Main.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_PayRoll_Reports_Main_Ln.Visible = False
        MDIParent1.mnu_Report_Textile_PayRoll_Reports_Main_Ln.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_PayRoll_Reports_Main.Visible = False
        MDIParent1.mnu_Report_Textile_PayRoll_Reports_Main.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_CottonReport_Main1_Ln.Visible = False
        MDIParent1.mnu_Report_Textile_CottonReport_Main1_Ln.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_CottonReport_Main1.Visible = False
        MDIParent1.mnu_Report_Textile_CottonReport_Main1.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_YarnReport_LN.Visible = False
        MDIParent1.mnu_Report_Textile_YarnReport_LN.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_Yarn_Report.Visible = False
        MDIParent1.mnu_Report_Textile_Yarn_Report.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_WasteReport_Main.Visible = False
        MDIParent1.mnu_Report_Textile_WasteReport_Main.Tag = "INVISIBLE"
        MDIParent1.mnu_Reports_VanTrip_Main_LN.Visible = False
        MDIParent1.mnu_Reports_VanTrip_Main_LN.Tag = "INVISIBLE"
        MDIParent1.mnu_Reports_VanTrip_Main.Visible = False
        MDIParent1.mnu_Reports_VanTrip_Main.Tag = "INVISIBLE"
        MDIParent1.mnu_Reports_Actual_Costing_Main.Visible = False
        MDIParent1.mnu_Reports_Actual_Costing_Main.Tag = "INVISIBLE"
        MDIParent1.mnu_Reports_User_Modifications_Main.Visible = False
        MDIParent1.mnu_Reports_User_Modifications_Main.Tag = "INVISIBLE"

        MDIParent1.mnu_Report_Textile_Register_Yarn_Main.Visible = False
        MDIParent1.mnu_Report_Textile_Register_Yarn_Main.Tag = "INVISIBLE"
        MDIParent1.ToolStripMenuItem27.Visible = False
        MDIParent1.ToolStripMenuItem27.Tag = "INVISIBLE"
        MDIParent1.CSizingSpecificationRegisterToolStripMenuItem.Visible = False
        MDIParent1.CSizingSpecificationRegisterToolStripMenuItem.Tag = "INVISIBLE"
        MDIParent1.ToolStripMenuItem28.Visible = False
        MDIParent1.ToolStripMenuItem28.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_Register_PavuDelivery_Register_Main.Visible = True
        MDIParent1.mnu_Report_Textile_For_MD.Tag = ""
        MDIParent1.mnu_Report_Textile_Register_PavuReceipt_Register_Main.Visible = True
        MDIParent1.mnu_Report_Textile_Register_PavuReceipt_Register_Main.Tag = ""
        MDIParent1.mnu_Report_Textile_Register_PavuDeliverty_SetandBeamwise.Visible = True
        MDIParent1.mnu_Report_Textile_Register_PavuDeliverty_SetandBeamwise.Tag = ""
        MDIParent1.ToolStripMenuItem29.Visible = True
        MDIParent1.ToolStripMenuItem29.Tag = ""
        MDIParent1.mnu_Report_Textile_Register_YarnDelivery_Register_Main.Visible = True
        MDIParent1.mnu_Report_Textile_Register_YarnDelivery_Register_Main.Tag = ""
        MDIParent1.mnu_Report_Textile_Register_YarnReceipt_Register_Main.Visible = True
        MDIParent1.mnu_Report_Textile_Register_YarnReceipt_Register_Main.Tag = ""
        MDIParent1.ToolStripMenuItem30.Visible = True
        MDIParent1.ToolStripMenuItem30.Tag = ""
        MDIParent1.HEmptyBeamRegisterToolStripMenuItem.Visible = True
        MDIParent1.HEmptyBeamRegisterToolStripMenuItem.Tag = ""
        MDIParent1.IEmptyBeamReceiptRegisterToolStripMenuItem.Visible = True
        MDIParent1.IEmptyBeamReceiptRegisterToolStripMenuItem.Tag = ""
        MDIParent1.ToolStripMenuItem31.Visible = False
        MDIParent1.ToolStripMenuItem31.Tag = "INVISIBLE"
        MDIParent1.JEmptyBagDeliveryRegisterToolStripMenuItem.Visible = False
        MDIParent1.JEmptyBagDeliveryRegisterToolStripMenuItem.Tag = "INVISIBLE"
        MDIParent1.KEmptyBagReceiptRegisterToolStripMenuItem.Visible = False
        MDIParent1.KEmptyBagReceiptRegisterToolStripMenuItem.Tag = "INVISIBLE"
        MDIParent1.ToolStripMenuItem32.Visible = False
        MDIParent1.ToolStripMenuItem32.Tag = "INVISIBLE"
        MDIParent1.LEmptyConeDeliveryRegisterToolStripMenuItem.Visible = False
        MDIParent1.LEmptyConeDeliveryRegisterToolStripMenuItem.Tag = "INVISIBLE"
        MDIParent1.MEmptyConeReceiptRegisterToolStripMenuItem.Visible = False
        MDIParent1.MEmptyConeReceiptRegisterToolStripMenuItem.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_Register_Weaver_Main_LN.Visible = True
        MDIParent1.mnu_Report_Textile_Register_Weaver_Main_LN.Tag = ""

        MDIParent1.mnu_Report_Textile_Register_Weaver_Main.Visible = True
        MDIParent1.mnu_Report_Textile_Register_Weaver_Main.Tag = ""
        MDIParent1.ToolStripSeparator24.Visible = False
        MDIParent1.ToolStripSeparator24.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_Register_Cloth_Purchase_Main.Visible = False
        MDIParent1.mnu_Report_Textile_Register_Cloth_Purchase_Main.Tag = "INVISIBLE"
        MDIParent1.ToolStripMenuItem34.Visible = False
        MDIParent1.ToolStripMenuItem34.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_Register_Cloth_Sales_Main.Visible = False
        MDIParent1.mnu_Report_Textile_Register_Cloth_Sales_Main.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_Register_Yarn_Sales_Main.Visible = False
        MDIParent1.mnu_Report_Textile_Register_Yarn_Sales_Main.Tag = "INVISIBLE"
        MDIParent1.ToolStripSeparator21.Visible = False
        MDIParent1.ToolStripSeparator21.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_Register_Transfer_Main.Visible = False
        MDIParent1.mnu_Report_Textile_Register_Transfer_Main.Tag = "INVISIBLE"
        MDIParent1.ToolStripSeparator22.Visible = False
        MDIParent1.ToolStripSeparator22.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_Register_ExcessShort_Main.Visible = False
        MDIParent1.mnu_Report_Textile_Register_ExcessShort_Main.Tag = "INVISIBLE"
        MDIParent1.ToolStripMenuItem52.Visible = False
        MDIParent1.ToolStripMenuItem52.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_PackingSlip.Visible = False
        MDIParent1.mnu_Report_Textile_PackingSlip.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_Bobin_Ln.Visible = False
        MDIParent1.mnu_Report_Textile_Bobin_Ln.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_Bobin_Main.Visible = False
        MDIParent1.mnu_Report_Textile_Bobin_Main.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_FabricProcessing_Registers_Ln.Visible = False
        MDIParent1.mnu_Report_Textile_FabricProcessing_Registers_Ln.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_ProcessingRegisters_Main.Visible = False
        MDIParent1.mnu_Report_Textile_ProcessingRegisters_Main.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_FabricProcessing_Registers_Main.Visible = False
        MDIParent1.mnu_Report_Textile_FabricProcessing_Registers_Main.Tag = "INVISIBLE"
        MDIParent1.ToolStripMenuItem94.Visible = False
        MDIParent1.ToolStripMenuItem94.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Transport_Register.Visible = False
        MDIParent1.mnu_Report_Transport_Register.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Transport_Register_All.Visible = False
        MDIParent1.mnu_Report_Transport_Register_All.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Transport_Register_Single.Visible = False
        MDIParent1.mnu_Report_Transport_Register_Single.Tag = "INVISIBLE"
        MDIParent1.ToolStripSeparator181.Visible = False
        MDIParent1.ToolStripSeparator181.Tag = "INVISIBLE"
        MDIParent1.MnuReportRegisterGeneralOtherPurchaseSalesGSTToolStripMenuItem_Main.Visible = False
        MDIParent1.MnuReportRegisterGeneralOtherPurchaseSalesGSTToolStripMenuItem_Main.Tag = "INVISIBLE"
        MDIParent1.mnu_Reports_Firewood_Register.Visible = False
        MDIParent1.mnu_Reports_Firewood_Register.Tag = "INVISIBLE"
        MDIParent1.mnu_Entry_Sizing_BeamCard_Register.Visible = False
        MDIParent1.mnu_Entry_Sizing_BeamCard_Register.Tag = "INVISIBLE"
        MDIParent1.mnu_Entry_Sizing_BeamCard_Details.Visible = False
        MDIParent1.mnu_Entry_Sizing_BeamCard_Details.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Register_Pavu_Purchase.Visible = False
        MDIParent1.mnu_Report_Register_Pavu_Purchase.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Register_Pavu_Sales.Visible = False
        MDIParent1.mnu_Report_Register_Pavu_Sales.Tag = "INVISIBLE"
        MDIParent1.mnu_Report_Textile_Register_Cotton_Purchase_sales_Register_Main.Visible = False
        MDIParent1.mnu_Report_Textile_Register_Cotton_Purchase_sales_Register_Main.Tag = "INVISIBLE"
        'MDIParent1.mnu_Report_Textile_For_MD.Visible = False
        'MDIParent1.mnu_Report_Textile_For_MD.Tag = "INVISIBLE"
        'MDIParent1.mnu_Report_Textile_For_MD.Visible = False
        'MDIParent1.mnu_Report_Textile_For_MD.Tag = "INVISIBLE"
        'MDIParent1.mnu_Report_Textile_For_MD.Visible = False
        'MDIParent1.mnu_Report_Textile_For_MD.Tag = "INVISIBLE"
        'MDIParent1.mnu_Report_Textile_For_MD.Visible = False
        'MDIParent1.mnu_Report_Textile_For_MD.Tag = "INVISIBLE"
        'MDIParent1.mnu_Report_Textile_For_MD.Visible = False
        'MDIParent1.mnu_Report_Textile_For_MD.Tag = "INVISIBLE"
        'MDIParent1.mnu_Report_Textile_For_MD.Visible = False
        'MDIParent1.mnu_Report_Textile_For_MD.Tag = "INVISIBLE"
        'MDIParent1.mnu_Report_Textile_For_MD.Visible = False
        'MDIParent1.mnu_Report_Textile_For_MD.Tag = "INVISIBLE"


        Me.Close()

    End Sub
End Class