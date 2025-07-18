Imports System.IO
Imports System.ServiceProcess
Public Class Entrance

    Private Declare Function GetLocaleInfo Lib "kernel32" Alias "GetLocaleInfoA" (ByVal Locale As Long, ByVal LCType As Long, ByVal lpLCData As String, ByVal cchData As Long) As Long
    Private Declare Function SetLocaleInfo Lib "kernel32" Alias "SetLocaleInfoA" (ByVal Locale As Long, ByVal LCType As Long, ByVal lpLCData As String) As Long
    Private Const LOCALE_USER_DEFAULT As Long = &H400
    Private Const LOCALE_SSHORTDATE = &H1F

    Private DyIPFailCount As Integer = 0

    Private strEncryptedText As String = ""
    Private strDecrptedText As String = ""

    Private vNEWEXE_Status As Boolean = False

    Private vLockDate_Frm_LicFile As Date = #12/12/2099#
    Private vLockStatus_Frm_LicFile As Boolean = False

    Private strPASSWORDINPUT As String = ""

    Private vCSND_servtype As String = ""
    Private vCSND_InstNm As String = ""
    Private vCSND_DefPath As String = ""
    Private vCSND_vSYSNm As String = ""
    Private vCSND_vSysSlNo As String = ""
    Private vCSND_vExeFileNm As String = ""

    Private Sub Entrance_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim cn1 As SqlClient.SqlConnection
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim vSPAutBckupEXE As String
        'Dim pth2 As String, vT1 As String
        'Dim fs As FileStream
        'Dim r As StreamReader
        'Dim w As StreamWriter
        'Dim INC As Integer = 0

        Try

            Common_Procedures.First_Opened_Today = False

            If InStr(1, Trim(LCase(Application.StartupPath)), "\bin\debug") > 0 Then
                Common_Procedures.AppPath = Replace(Trim(LCase(Application.StartupPath)), "\bin\debug", "")
            Else
                Common_Procedures.AppPath = Application.StartupPath
            End If

            'pth2 = Trim(Common_Procedures.AppPath) & "\setpl.txt"   '---Software Execution Time Plan
            'If File.Exists(pth2) = True Then
            '    File.Delete(pth2)
            'End If

            'fs = New FileStream(pth2, FileMode.Create)
            'w = New StreamWriter(fs)

            'INC = INC + 1
            'vT1 = INC & ". Entrance_Load - STARTS - " & Now
            'w.WriteLine(Trim(vT1))

            'MessageBox.Show("Entrance_Load-1")

            Common_Procedures.Office_System_Status = Common_Procedures.is_OfficeSystem

            Common_Procedures.Entrance_SQL_PassWord.passPhrase = "T.ThanGesWaran"
            Common_Procedures.Entrance_SQL_PassWord.saltValue = "N.VaRaLakshmi"

            Common_Procedures.UserCreation_AcPassWord.passPhrase = "Tsoft_Ac_User_Name"
            Common_Procedures.UserCreation_AcPassWord.saltValue = "Tsoft_Ac_PassWord"

            Common_Procedures.UserCreation_UnAcPassWord.passPhrase = "Tsoft_UnAc_User_Name"
            Common_Procedures.UserCreation_UnAcPassWord.saltValue = "Tsoft_UnAc_PassWord"

            Common_Procedures.SoftWareRegister.passPhrase = "Solutions-IXC0307249"
            Common_Procedures.SoftWareRegister.saltValue = "GOLD-15101979"

            Common_Procedures.SoftWareLock.passPhrase = "T.ThangaSamy-333932282910"
            Common_Procedures.SoftWareLock.saltValue = "T.ThangaRethinam-301117610386"

            Common_Procedures.RegisterPassWord.passPhrase = "VaraLakshmi-855091626425"
            Common_Procedures.RegisterPassWord.saltValue = "VARALI-05081986"

            Common_Procedures.ONLINE_OMS_CONNECTION_CHECK.passPhrase = "Rajeswari-893423371413"
            Common_Procedures.ONLINE_OMS_CONNECTION_CHECK.saltValue = "RAJE-17052012"

            Common_Procedures.ACTUAL_CUSTOMER_CODE_NUMBER.passPhrase = "T.Rockshi"
            Common_Procedures.ACTUAL_CUSTOMER_CODE_NUMBER.saltValue = "ROCK-16112017"

            Common_Procedures.settings.Last_Opened_Date = #1/1/2000#

            'INC = INC + 1
            'vT1 = INC & ". Change_System_DateTime_To_Internet_DateTime - " & Now
            'w.WriteLine(Trim(vT1))
            'Change_System_DateTime_To_Internet_DateTime()

            'INC = INC + 1
            'vT1 = INC & ". Check_Software_Registration   - " & Now
            'w.WriteLine(Trim(vT1))
            'MessageBox.Show("Entrance_Load-2")
            'Check_Software_Registration()

            'MessageBox.Show("Entrance_Load-4")
            'INC = INC + 1
            'vT1 = INC & ". Get_ServerDetails - ENDS - " & Now
            'w.WriteLine(Trim(vT1))

            Get_ServerDetails()

            'INC = INC + 1
            'vT1 = INC & ". Get_ServerDetails - ENDS - " & Now
            'w.WriteLine(Trim(vT1))


            'INC = INC + 1
            'vT1 = INC & ". Server_System_Status - START - " & Now
            'w.WriteLine(Trim(vT1))

            Common_Procedures.Server_System_Status = Common_Procedures.is_ServerSystem()

            'INC = INC + 1
            'vT1 = INC & ". Server_System_Status - END - " & Now
            'w.WriteLine(Trim(vT1))

            'INC = INC + 1
            'vT1 = INC & ". Copy_Supporting_Files - START - " & Now
            'w.WriteLine(Trim(vT1))

            Copy_Supporting_Files()

            'INC = INC + 1
            'vT1 = INC & ". Copy_Supporting_Files - END - " & Now
            'w.WriteLine(Trim(vT1))

            'INC = INC + 1
            'vT1 = INC & ". Design_CompanyGroup_Details_Grid - START - " & Now
            'w.WriteLine(Trim(vT1))

            Design_CompanyGroup_Details_Grid()

            'INC = INC + 1
            'vT1 = INC & ". Design_CompanyGroup_Details_Grid - END - " & Now
            'w.WriteLine(Trim(vT1))


            If Trim(Common_Procedures.ServerName) = "" Then
                MessageBox.Show("Invalid Connection File Details", "INVALID SERVER DETAILS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Me.Close()
                Application.Exit()
                End
                Exit Sub
            End If


            'INC = INC + 1
            'vT1 = INC & ". GET Connection_String - START - " & Now
            'w.WriteLine(Trim(vT1))

            Common_Procedures.ConnectionString_Master = Common_Procedures.Create_Sql_ConnectionString("master")
            Common_Procedures.ConnectionString_CompanyGroupdetails = Common_Procedures.Create_Sql_ConnectionString(Common_Procedures.CompanyDetailsDataBaseName)
            Common_Procedures.Connection_String = ""

            'INC = INC + 1
            'vT1 = INC & ". GET Connection_String - END - " & Now
            'w.WriteLine(Trim(vT1))


            'INC = INC + 1
            'vT1 = INC & ". Connect_to_Master_Databases - START - " & Now
            'w.WriteLine(Trim(vT1))

            Connect_to_Master_Databases()

            'INC = INC + 1
            'vT1 = INC & ". Connect_to_Master_Databases - END - " & Now
            'w.WriteLine(Trim(vT1))


            'INC = INC + 1
            'vT1 = INC & ". Attach_Existing_Databases - start - " & Now
            'w.WriteLine(Trim(vT1))

            'MessageBox.Show("Entrance_Load-5")
            Attach_Existing_Databases()

            'INC = INC + 1
            'vT1 = INC & ". Attach_Existing_Databases - END - " & Now
            'w.WriteLine(Trim(vT1))


            'INC = INC + 1
            'vT1 = INC & ". Check_Create_CompanyGroupDetails_DB - start - " & Now
            'w.WriteLine(Trim(vT1))

            'MessageBox.Show("Entrance_Load-6")
            Check_Create_CompanyGroupDetails_DB()

            'INC = INC + 1
            'vT1 = INC & ". Check_Create_CompanyGroupDetails_DB - END - " & Now
            'w.WriteLine(Trim(vT1))


            'INC = INC + 1
            'vT1 = INC & ". Check is_Database_File_Exists ( IF USB ) - START - " & Now
            'w.WriteLine(Trim(vT1))

            'MessageBox.Show("Entrance_Load-7")
            If Trim(UCase(Common_Procedures.ServerDataBaseLocation_InExTernalUSB)) = "USB" Then
                If Common_Procedures.is_Database_File_Exists(Common_Procedures.CompanyDetailsDataBaseName) = False Then
                    MessageBox.Show("Invalid Database File - " & Common_Procedures.CompanyDetailsDataBaseName, "INVALID DB FILE DETAILS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Application.Exit()
                    Exit Sub
                End If
            End If

            'INC = INC + 1
            'vT1 = INC & ". Check is_Database_File_Exists ( IF USB ) - END - " & Now
            'w.WriteLine(Trim(vT1))

            'INC = INC + 1
            'vT1 = INC & ". OPEN CONNECTION TO COMPANYGROUP DB  - START - " & Now
            'w.WriteLine(Trim(vT1))

            'MessageBox.Show("Entrance_Load-7.1")
            cn1 = New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
            cn1.Open()

            'INC = INC + 1
            'vT1 = INC & ". OPEN CONNECTION TO COMPANYGROUP DB  - END - " & Now
            'w.WriteLine(Trim(vT1))

            'INC = INC + 1
            'vT1 = INC & ". Get_CompanyGroupDetails_SettingsValue  - START - " & Now
            'w.WriteLine(Trim(vT1))

            'MessageBox.Show("Entrance_Load-8")
            Get_CompanyGroupDetails_SettingsValue(cn1)

            'INC = INC + 1
            'vT1 = INC & ". Get_CompanyGroupDetails_SettingsValue  - END - " & Now
            'w.WriteLine(Trim(vT1))


            'INC = INC + 1
            'vT1 = INC & ". get_Customer_Settings  - START - " & Now
            'w.WriteLine(Trim(vT1))

            Common_Procedures.get_Customer_Settings()

            'INC = INC + 1
            'vT1 = INC & ". get_Customer_Settings  - END - " & Now
            'w.WriteLine(Trim(vT1))

            'MessageBox.Show("CC : " & Common_Procedures.settings.CustomerCode, "CUSTOMER DETAILS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            'INC = INC + 1
            'vT1 = INC & ". Change_DateFormat  - START - " & Now
            'w.WriteLine(Trim(vT1))

            'MessageBox.Show("Entrance_Load-3")
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1469" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1455" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1512" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1139" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1334" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1335" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1336" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1382" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1387" Then '---- SIVAKUMAR Textiles (THEKKALUR)
                Change_DateFormat()
            End If

            'INC = INC + 1
            'vT1 = INC & ". Change_DateFormat  - END - " & Now
            'w.WriteLine(Trim(vT1))


            'INC = INC + 1
            'vT1 = INC & ". Check_Software_Registration  - START - " & Now
            'w.WriteLine(Trim(vT1))

            'MessageBox.Show("Entrance_Load-2")
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1037" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1038" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1087" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1267" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1464" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1490" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1587" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1608" Then
                Check_Software_Registration()
            End If

            'INC = INC + 1
            'vT1 = INC & ". Check_Software_Registration  - END - " & Now
            'w.WriteLine(Trim(vT1))


            'INC = INC + 1
            'vT1 = INC & ". Check_Update_Exe_File_DateTime  - START - " & Now
            'w.WriteLine(Trim(vT1))

            Check_Update_Exe_File_DateTime(cn1)

            'INC = INC + 1
            'vT1 = INC & ". Check_Update_Exe_File_DateTime  - END - " & Now
            'w.WriteLine(Trim(vT1))


            'INC = INC + 1
            'vT1 = INC & ". Check_Update_SystemDateTime  - START - " & Now
            'w.WriteLine(Trim(vT1))

            'MessageBox.Show("Entrance_Load-9")
            Check_Update_SystemDateTime(cn1)


            'INC = INC + 1
            'vT1 = INC & ". Check_Update_SystemDateTime  - END - " & Now
            'w.WriteLine(Trim(vT1))


            'INC = INC + 1
            'vT1 = INC & ". Update_System_Name_Exe_DateTime  - START - " & Now
            'w.WriteLine(Trim(vT1))

            vCSND_servtype = ""
            vCSND_InstNm = ""
            vCSND_DefPath = ""
            vCSND_vSYSNm = ""
            vCSND_vSysSlNo = ""
            vCSND_vExeFileNm = ""

            Update_System_Name_Exe_DateTime(cn1)

            'INC = INC + 1
            'vT1 = INC & ". Update_System_Name_Exe_DateTime  - END - " & Now
            'w.WriteLine(Trim(vT1))

            'INC = INC + 1
            'vT1 = INC & ". Get___DataBase_Details  - START - " & Now
            'w.WriteLine(Trim(vT1))

            'Get_ONLINE_OMS_DataBase_Details()

            'INC = INC + 1
            'vT1 = INC & ". Get___DataBase_Details  - END - " & Now
            'w.WriteLine(Trim(vT1))

            'INC = INC + 1
            'vT1 = INC & ". get_UserName_Password  - START - " & Now
            'w.WriteLine(Trim(vT1))

            'MessageBox.Show("Entrance_Load-10")
            Common_Procedures.get_UserName_Password(Me)

            'INC = INC + 1
            'vT1 = INC & ". get_UserName_Password  - END - " & Now
            'w.WriteLine(Trim(vT1))

            cn1.Close()
            cn1.Dispose()


            'MessageBox.Show("Entrance_Load-11-LAST")


            'INC = INC + 1
            'vT1 = INC & ". Check_Pendrive_in_ServerSystem  - START - " & Now
            'w.WriteLine(Trim(vT1))

            Check_Pendrive_in_ServerSystem()

            'INC = INC + 1
            'vT1 = INC & ". Check_Pendrive_in_ServerSystem  - END - " & Now
            'w.WriteLine(Trim(vT1))


            'INC = INC + 1
            'vT1 = INC & ". CHECK TSOFT_AutoBackUP - START - " & Now
            'w.WriteLine(Trim(vT1))

            If Common_Procedures.Server_System_Status = True Then
                If Common_Procedures.Office_System_Status = False Then
                    vSPAutBckupEXE = Trim(Common_Procedures.AppPath) & "\TSOFT_AutoBackUP.exe"
                    If System.IO.File.Exists(vSPAutBckupEXE) = False Then
                        MessageBox.Show("Invalid : TSOFT_AutoBackUP.exe not installed", "INVALID TSOFT AUTOBACKUP...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Me.Close()
                        Application.Exit()
                        End
                        Exit Sub

                    Else

                        If Common_Procedures.First_Opened_Today = True Then
                            If Trim(vSPAutBckupEXE) <> "" Then
                                Shell(vSPAutBckupEXE, AppWinStyle.Hide)
                            End If
                        End If

                    End If
                End If
            End If

            'INC = INC + 1
            'vT1 = INC & ". CHECK TSOFT_AutoBackUP - END - " & Now
            'w.WriteLine(Trim(vT1))

            'INC = INC + 1
            'vT1 = INC & ". Entrance_Load - END - " & Now
            'w.WriteLine(Trim(vT1))

            'w.Close()
            'fs.Close()
            'w.Dispose()
            'fs.Dispose()

        Catch ex As Exception


            'INC = INC + 1
            'vT1 = INC & ". Entrance_Load - END - " & Now
            'w.WriteLine(Trim(vT1))

            'w.Close()
            'fs.Close()
            'w.Dispose()
            'fs.Dispose()

            MessageBox.Show(ex.Message, "DOES NOT OPEN...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            '----


        End Try

    End Sub

    Private Sub Entrance_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim cn1 As SqlClient.SqlConnection
        Dim Cmd As New SqlClient.SqlCommand
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim n As Integer = 0
        Dim CompgrpCondt As String = ""
        Dim DBName As String = ""
        Dim ShowSTS As Boolean = False
        Dim vLASTYR As Integer = 0
        Dim vChngYrSTSSTS As Boolean
        Dim vNOBUTTON_sts As Boolean
        Dim vCOLHIDESTS As Boolean = True


        cn1 = New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
        cn1.Open()

        Get_CompanyGroupDetails_SettingsValue(cn1)

        CompgrpCondt = ""
        If Trim(UCase(Common_Procedures.User.Type)) = "UNACCOUNT" Then
            'CompgrpCondt = " Where (CompanyGroup_Type <> 1 )"
        Else
            CompgrpCondt = " Where (CGT <> 2)"   '  " Where (CompanyGroup_Type <> 'UNACCOUNT')"
        End If

        If Common_Procedures.Office_System_Status = True Then

            Cmd.Connection = cn1

            Cmd.CommandText = "Update CompanyGroup_Head set CcNo_OrderBy = 1"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Update CompanyGroup_Head set CcNo_OrderBy = 0 where Cc_No = '" & Trim(Common_Procedures.settings.CustomerCode) & "'"
            Cmd.ExecuteNonQuery()

        End If


        '---CGT = 0 - ACCOUNT & UNACCOUNT
        '---CGT = 1 - ACCOUNT
        '---CGT = 2 - UNACCOUNT

        da2 = New SqlClient.SqlDataAdapter("select * from CompanyGroup_Head  " & CompgrpCondt & " Order by CcNo_OrderBy, To_Date desc, CompanyGroup_IdNo, From_Date", cn1)
        dt2 = New DataTable
        da2.Fill(dt2)

        dgv_Details.Rows.Clear()

        If dt2.Rows.Count > 0 Then

            For i = 0 To dt2.Rows.Count - 1

                DBName = Common_Procedures.get_Company_DataBaseName(Trim(Val(dt2.Rows(i).Item("CompanyGroup_IdNo").ToString)))

                ShowSTS = False
                If Trim(UCase(Common_Procedures.ServerDataBaseLocation_InExTernalUSB)) = "USB" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1097" Then
                    If Common_Procedures.is_Database_File_Exists(DBName) = True Then ShowSTS = True
                Else
                    ShowSTS = True
                End If

                If ShowSTS = True Then

                    n = dgv_Details.Rows.Add()

                    dgv_Details.Rows(n).Cells(0).Value = "  " & dt2.Rows(i).Item("CompanyGroup_Name").ToString
                    dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("CompanyGroup_IdNo").ToString
                    dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Financial_Range").ToString

                    If dgv_Details.Columns(3).Visible = True Then

                        vLASTYR = Val(Microsoft.VisualBasic.Right(Trim(dgv_Details.Rows(n).Cells(2).Value), 4))

                        vNOBUTTON_sts = True

                        If vLASTYR = Year(Now) Then
                            vChngYrSTSSTS = Common_Procedures.is_Separate_New_CompanyGroup_Created_for_NextYear(cn1, Val(dt2.Rows(i).Item("CompanyGroup_IdNo").ToString), Trim(dt2.Rows(i).Item("CompanyGroup_Name").ToString))
                            If vChngYrSTSSTS = True Then
                                dgv_Details.Rows(n).Cells(3).Value = "Create (" & vLASTYR & "-" & vLASTYR + 1 & ")"
                                vNOBUTTON_sts = False
                            End If
                        End If

                        If vNOBUTTON_sts = True Then
                            dgv_Details.Rows(n).Cells(3) = New DataGridViewTextBoxCell
                            dgv_Details.Rows(n).Cells(3).ReadOnly = True
                            dgv_Details.Rows(n).Cells(3).Value = ""
                        End If

                    End If

                End If

            Next i

            If dgv_Details.Columns(3).Visible = True Then
                vCOLHIDESTS = True
                For i = 0 To dgv_Details.Rows.Count - 1
                    If Trim(dgv_Details.Rows(i).Cells(3).Value) <> "" Then
                        vCOLHIDESTS = False
                        Exit For
                    End If
                Next
                If vCOLHIDESTS = True Then
                    dgv_Details.Columns(3).Visible = False
                    dgv_Details.Columns(0).Width = dgv_Details.Columns(0).Width + dgv_Details.Columns(3).Width
                End If
            End If


        End If

        If dgv_Details.Enabled = True And dgv_Details.Visible = True Then
            If dgv_Details.Rows.Count = 0 Then dgv_Details.Rows.Add()
            Dim selectedRow As DataGridViewRow = dgv_Details.Rows(0)
            selectedRow.Selected = True
            dgv_Details.Focus()
        End If

        lbl_TrialPeriod_Warning.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1234" Then '---- ARULJOTHI EXPORTS PVT LTD (SOMANUR)
            lbl_TrialPeriod_Warning.Visible = True
        End If

        dt2.Dispose()
        da2.Dispose()

        cn1.Close()
        cn1.Dispose()



    End Sub

    Private Sub Entrance_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try

            'strPASSWORDINPUT = Trim(strPASSWORDINPUT) & e.KeyChar
            'If Trim(strPASSWORDINPUT) <> "" Then
            '    'MessageBox.Show(strPASSWORDINPUT)
            'End If


            If Asc(e.KeyChar) = 27 Then
                If MessageBox.Show("Do you want to Close?", "FOR CLOSING SOFTWARE...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                    Me.Close()
                    Application.Exit()
                    End
                End If
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_Create_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Create.Click
        If dgv_Details.Rows.Count = 0 Then dgv_Details.Rows.Add()
        dgv_Details.Focus()
        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)

        Dim f As New CompanyGroup_Creation
        f.ShowDialog()
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
        Application.Exit()
        End
    End Sub

    Private Sub btn_Open_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Open.Click
        Dim cn1 As SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand
        Dim IdNo As Integer, Nr As Integer, I As Integer

        Try

            IdNo = Trim(dgv_Details.CurrentRow.Cells(1).Value)

            Common_Procedures.CompGroupIdNo = 0
            Common_Procedures.CompGroupName = ""
            Common_Procedures.CompGroupFnRange = ""

            Common_Procedures.Connection_String = ""
            Common_Procedures.DataBaseName = ""

            If Val(IdNo) <> 0 Then

                Common_Procedures.CompGroupIdNo = Val(IdNo)
                Common_Procedures.CompGroupName = Trim(dgv_Details.CurrentRow.Cells(0).Value)
                Common_Procedures.CompGroupFnRange = Trim(dgv_Details.CurrentRow.Cells(2).Value)

                Common_Procedures.DataBaseName = Common_Procedures.get_Company_DataBaseName(Trim(Val(IdNo)))

                Common_Procedures.Connection_String = Common_Procedures.Create_Sql_ConnectionString(Common_Procedures.DataBaseName)

                If Trim(UCase(Common_Procedures.ServerDataBaseLocation_InExTernalUSB)) = "USB" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1097" Then
                    If Common_Procedures.is_Database_File_Exists(Common_Procedures.DataBaseName) = False Then
                        MessageBox.Show("Invalid Database File - " & Common_Procedures.DataBaseName, "INVALID COMPANY GROUP SELECTION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If

                cn1 = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
                cn1.Open()

                cmd.Connection = cn1

                cmd.CommandText = "Update FinancialRange_Head set Financial_Range = '" & Trim(Common_Procedures.CompGroupFnRange) & "'"
                Nr = cmd.ExecuteNonQuery()
                If Nr = 0 Then
                    cmd.CommandText = "Insert into FinancialRange_Head(Financial_Range) values ('" & Trim(Common_Procedures.CompGroupFnRange) & "')"
                    cmd.ExecuteNonQuery()
                End If


                cmd.Dispose()
                cn1.Dispose()

                'If vNEWEXE_Status = True Then

                '    If Not (Trim(UCase(Common_Procedures.ServerWindowsLogin)) = "IP" Or Trim(UCase(Common_Procedures.ServerWindowsLogin)) = "SIP" Or Trim(UCase(Common_Procedures.ServerWindowsLogin)) = "DIP") Then

                '        If Common_Procedures.Office_System_Status = False Then

                '            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" Then

                '                For I = 1 To 2

                '                    FieldsCheck.vFldsChk_From_CompGroupCreation_Status = True
                '                    FieldsCheck.vFldsChk_All_Status = True
                '                    FieldsCheck.FieldsCheck_5()
                '                    'FieldsCheck.FieldsCheck_All()
                '                    FieldsCheck.vFldsChk_All_Status = True
                '                    FieldsCheck.vFldsChk_From_CompGroupCreation_Status = False

                '                Next

                '            End If

                '        End If

                '    End If

                'End If

                Common_Procedures.get_Customer_Settings()

                Common_Procedures.ReportTempTable = ""
                Common_Procedures.ReportTempSubTable = ""
                Common_Procedures.ReportTempSimpleTable = ""
                Common_Procedures.EntryTempTable = ""
                Common_Procedures.EntryTempSubTable = ""
                Common_Procedures.EntryTempSimpleTable = ""
                Common_Procedures.TempTable1 = ""
                Common_Procedures.TempTable2 = ""
                Common_Procedures.TempTable3 = ""
                Common_Procedures.TempTable_for_CrossChecking_PcsBale1 = ""
                Common_Procedures.TempTable_for_Closed_SalesOrders = ""
                FieldsCheck.set_ReportTemps_Name()
                If FieldsCheck.Check_is_ReportTemps_Table_EXISTS(Common_Procedures.ReportTempTable) = False Then
                    FieldsCheck.Field_Check_ReportTemp()
                End If
                If Trim(Common_Procedures.ReportTempTable) = "" Then Common_Procedures.ReportTempTable = "ReportTemp"
                If Trim(Common_Procedures.ReportTempSubTable) = "" Then Common_Procedures.ReportTempSubTable = "ReportTempSub"
                If Trim(Common_Procedures.ReportTempSimpleTable) = "" Then Common_Procedures.ReportTempSimpleTable = "ReportTemp_Simple"
                If Trim(Common_Procedures.EntryTempTable) = "" Then Common_Procedures.EntryTempTable = "EntryTemp"
                If Trim(Common_Procedures.EntryTempSubTable) = "" Then Common_Procedures.EntryTempSubTable = "EntryTempSub"
                If Trim(Common_Procedures.EntryTempSimpleTable) = "" Then Common_Procedures.EntryTempSimpleTable = "EntryTemp_Simple"
                If Trim(Common_Procedures.TempTable1) = "" Then Common_Procedures.TempTable1 = "TempTable1"
                If Trim(Common_Procedures.TempTable2) = "" Then Common_Procedures.TempTable2 = "TempTable2"
                If Trim(Common_Procedures.TempTable3) = "" Then Common_Procedures.TempTable3 = "TempTable3"
                If Trim(Common_Procedures.TempTable_for_CrossChecking_PcsBale1) = "" Then Common_Procedures.TempTable_for_CrossChecking_PcsBale1 = "TempTable_for_CrossChecking_PcsBale1"
                If Trim(Common_Procedures.TempTable_for_Closed_SalesOrders) = "" Then Common_Procedures.TempTable_for_Closed_SalesOrders = "TempTable_for_Closed_SalesOrders"

                'MessageBox.Show(Trim(Common_Procedures.ReportTempTable), "REPORTTEMP TABLE NAME....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Common_Procedures.SoftwareType_Opened = 0
                Common_Procedures.SoftwareType_Opened = 0

                If Common_Procedures.settings.Show_Modulewise_Entrance = 1 Then


                    'If Common_Procedures.settings.CustomerCode = "1186" Then
                    Menu_List.Show()
                    'Else
                    '    MDIParent1.Show()
                    'End If

                Else

                    Common_Procedures.SoftwareType_Opened = Common_Procedures.SoftwareTypes.Textile_Software
                        MDIParent1.Show()

                    End If

                    Me.Hide()

                Else
                    MessageBox.Show("Select Company Group Name", "INVALID COMPANY GROUP SELECTION....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR IN COMPANYGROUP SELECTION....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.DoubleClick
        btn_Open_Click(sender, e)
    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        If e.KeyCode = 13 Then
            btn_Open_Click(sender, e)
        End If
    End Sub

    Private Sub Check_Software_Registration()
        Dim vEncryptd_LicenseCode_Frm_File As String = ""
        Dim vLicenseCode_Frm_File As String = ""
        Dim vSysSlNo As String = ""
        Dim vSysNo_FrmFile As String = ""
        Dim a() As String

        If Common_Procedures.Office_System_Status = True Then
            Exit Sub
        End If

        vEncryptd_LicenseCode_Frm_File = Get_Codes_From_LicenseFile()

        vLicenseCode_Frm_File = Common_Procedures.Decrypt(Trim(vEncryptd_LicenseCode_Frm_File), Trim(Common_Procedures.SoftWareRegister.passPhrase), Trim(Common_Procedures.SoftWareRegister.saltValue))

        vSysNo_FrmFile = ""
        vLockStatus_Frm_LicFile = False

        If Trim(vLicenseCode_Frm_File) <> "" Then
            a = Split(vLicenseCode_Frm_File, "~@~")
            If UBound(a) >= 0 Then vSysNo_FrmFile = Trim(a(0))
            If UBound(a) >= 1 Then
                If IsDate(a(1)) = True Then
                    vLockDate_Frm_LicFile = a(1)
                    vLockStatus_Frm_LicFile = True
                End If
            End If

        End If

        vSysSlNo = Common_Procedures.GetDriveSerialNumber(Microsoft.VisualBasic.Left(Application.StartupPath, 2))

        If Trim(vSysSlNo) = "" Then

            If Trim(UCase(vSysNo_FrmFile)) <> Trim(UCase("1061-388D4E18")) And Trim(UCase(vSysNo_FrmFile)) <> Trim(UCase("1234-6218239A")) And Trim(UCase(vSysNo_FrmFile)) <> Trim(UCase("1391-A4DB533E")) And Trim(UCase(vSysNo_FrmFile)) <> Trim(UCase("1391-C47D7A63")) And Trim(UCase(vSysNo_FrmFile)) <> Trim(UCase("1307-344493D8")) And Trim(UCase(vSysNo_FrmFile)) <> Trim(UCase("1239-8A625CB6")) And Trim(UCase(vSysNo_FrmFile)) <> Trim(UCase("1428-E65B724B")) And Trim(UCase(vSysNo_FrmFile)) <> Trim(UCase("1428-4E0B1408")) And Trim(UCase(vSysNo_FrmFile)) <> Trim(UCase("1264-F2422742")) And Trim(UCase(vSysNo_FrmFile)) <> Trim(UCase("1224-44A7626C")) And Trim(UCase(vSysNo_FrmFile)) <> Trim(UCase("1589-68CB25D4")) And Trim(UCase(vSysNo_FrmFile)) <> Trim(UCase("1589-6E446034")) And Trim(UCase(vSysNo_FrmFile)) <> Trim(UCase("1608-6E446034")) Then
                MessageBox.Show("Invalid System Serial No", "DOES NOT CHECK REGISTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Me.Close()
                Application.Exit()
                End
                Exit Sub

            Else

                Common_Procedures.HDD_SERIALNO = Trim(UCase(vSysNo_FrmFile))
                Exit Sub

            End If

        End If

        Common_Procedures.HDD_SERIALNO = Trim(UCase(vSysSlNo))

        If Trim(UCase(vSysSlNo)) <> Trim(UCase(vSysNo_FrmFile)) Then
            MessageBox.Show("Invalid License Code", "INVALID REGISTRATION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Me.Close()
            Application.Exit()
            End
            Exit Sub
        End If

        ''OSPath = Path.GetPathRoot(Environment.SystemDirectory)
        ''OSPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        ''OSPath = Path.GetPathRoot(Environment.CurrentDirectory)
        ''MessageBox.Show(OSPath, "WINDOWS PATH...", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub Change_DateFormat()
        Dim DD_Format As String
        Try

            DD_Format = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern()

            If Trim(DD_Format) <> "dd/MM/yyyy" Then
                Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\Control Panel\International", "sShortDate", "dd/MM/yyyy")
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE CHANGING DATE FORMAT IN CONTROL PANEL..", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Get_ServerDetails()
        Dim pth As String
        Dim pth2 As String
        Dim ConStr As String
        Dim a() As String
        Dim fs As FileStream
        Dim r As StreamReader
        Dim w As StreamWriter
        Dim Conn_File_Create_STS As Boolean = False
        Dim vDbNameInConnFile As String = ""
        Dim vsqlpwd_auto_encrpt_char As String = ""
        Dim vSQLInstNm As String = ""
        Dim vSQLpwd As String = ""


        Try

            pth = Trim(Common_Procedures.AppPath) & "\connection.ini"

            Common_Procedures.ServerName = ""
            Common_Procedures.ServerPassword = ""
            Common_Procedures.ServerWindowsLogin = ""
            Common_Procedures.SqlServer_PortNumber = ""
            Common_Procedures.ServerDataBaseLocation_InExTernalUSB = ""
            Common_Procedures.ServerLoginID = ""
            Common_Procedures.Server_ONLine_CCNo = ""

            Common_Procedures.ConnectionString_CompanyGroupdetails = ""
            Common_Procedures.Connection_String = ""
            Common_Procedures.DataBaseName = ""

            If File.Exists(pth) = False Then
                vSQLInstNm = InputBox("Enter Sql Instance Name : ", "SQL CONNECTION DETAILS....", "tsoft2014")
                If Trim(vSQLInstNm) = "" Then vSQLInstNm = "tsoft2014"

                vSQLpwd = "A0a+NXLTUaoY+6d13TrkiA=="   '--- "tsoftsql"

                fs = New FileStream(pth, FileMode.Create)
                w = New StreamWriter(fs)
                w.WriteLine(SystemInformation.ComputerName & "\" & Trim(vSQLInstNm) & "," & Trim(vSQLpwd))
                w.Close()
                fs.Close()
                w.Dispose()
                fs.Dispose()
            End If

LOOP1:
            ConStr = ""
            If File.Exists(pth) = True Then
                fs = New FileStream(pth, FileMode.Open)
                r = New StreamReader(fs)
                ConStr = r.ReadLine
                r.Close()
                fs.Close()
                r.Dispose()
                fs.Dispose()
            End If

            vDbNameInConnFile = ""
            If Trim(ConStr) <> "" Then
                a = Split(ConStr, ",")
                If UBound(a) >= 0 Then Common_Procedures.ServerName = Trim(a(0))
                If UBound(a) >= 1 Then Common_Procedures.ServerPassword = Trim(a(1))
                If UBound(a) >= 2 Then Common_Procedures.ServerWindowsLogin = Trim(a(2))
                If UBound(a) >= 3 Then
                    If Trim(a(3)) <> "" Then
                        If InStr(1, Trim(UCase(a(3))), "TSOFT") > 0 Then
                            'If InStr(1, Trim(UCase(a(3))), "TSOFT") > 0 And InStr(1, Trim(UCase(a(3))), "COMPANYGROUP") > 0 Then
                            vDbNameInConnFile = Trim(a(3))
                            Common_Procedures.CompanyDetailsDataBaseName = Trim(vDbNameInConnFile)
                        End If
                    End If
                End If
                If UBound(a) >= 4 Then Common_Procedures.ServerDataBaseLocation_InExTernalUSB = Trim(a(4))
                If UBound(a) >= 5 Then Common_Procedures.SqlServer_PortNumber = Trim(a(5))

                If UBound(a) >= 6 Then Common_Procedures.ServerLoginID = Trim(a(6))
                If Trim(Common_Procedures.ServerLoginID) = "" Then Common_Procedures.ServerLoginID = "sa"

                If UBound(a) >= 7 Then Common_Procedures.Server_ONLine_CCNo = Trim(a(7))

            End If

            btn_Create.Visible = True
            If Trim(UCase(Common_Procedures.ServerWindowsLogin)) = "ONLINE" And Trim(Common_Procedures.Server_ONLine_CCNo) <> "" Then
                Get_ONLINE_DataBase_Details()
                If Common_Procedures.Office_System_Status = False Then
                    btn_Create.Visible = False
                End If
            End If

            vsqlpwd_auto_encrpt_char = "[{<SQLPASSWORD>}]"
            If Microsoft.VisualBasic.Right(Trim(UCase(Common_Procedures.ServerPassword)), 2) <> "==" And InStr(1, Trim(UCase(Common_Procedures.ServerPassword)), Trim(UCase(vsqlpwd_auto_encrpt_char))) > 0 And Conn_File_Create_STS = False Then

                Common_Procedures.ServerPassword = Replace(Common_Procedures.ServerPassword, vsqlpwd_auto_encrpt_char, "")
                Common_Procedures.ServerPassword = Common_Procedures.Encrypt(Trim(Common_Procedures.ServerPassword), Trim(Common_Procedures.Entrance_SQL_PassWord.passPhrase), Trim(Common_Procedures.Entrance_SQL_PassWord.saltValue))

                fs = New FileStream(pth, FileMode.Create)
                w = New StreamWriter(fs)
                w.WriteLine(Trim(Common_Procedures.ServerName) & "," & Trim(Common_Procedures.ServerPassword) & "," & Trim(Common_Procedures.ServerWindowsLogin) & "," & Trim(vDbNameInConnFile) & "," & Trim(Common_Procedures.ServerDataBaseLocation_InExTernalUSB) & "," & Trim(Common_Procedures.SqlServer_PortNumber))
                w.Close()
                fs.Close()
                w.Dispose()
                fs.Dispose()

                Conn_File_Create_STS = True

            End If

            Common_Procedures.ServerPassword = Common_Procedures.Decrypt(Trim(Common_Procedures.ServerPassword), Trim(Common_Procedures.Entrance_SQL_PassWord.passPhrase), Trim(Common_Procedures.Entrance_SQL_PassWord.saltValue))

            Common_Procedures.settings.Last_Opened_Date = #1/1/2000#

            pth2 = Trim(Common_Procedures.AppPath) & "\lod.txt"

            If File.Exists(pth2) = False Then
                fs = New FileStream(pth2, FileMode.Create)
                w = New StreamWriter(fs)
                w.WriteLine(Date.Today.ToShortDateString)
                w.Close()
                fs.Close()
                w.Dispose()
                fs.Dispose()
            End If

            ConStr = ""
            If File.Exists(pth2) = True Then
                fs = New FileStream(pth2, FileMode.Open)
                r = New StreamReader(fs)
                ConStr = r.ReadLine
                r.Close()
                fs.Close()
                r.Dispose()
                fs.Dispose()
            End If

            If Trim(ConStr) <> "" Then
                If IsDate(ConStr) = True Then
                    If IsDate(ConStr) = True Then
                        Common_Procedures.settings.Last_Opened_Date = CDate(ConStr)
                    End If
                End If
            End If

            If DateDiff(DateInterval.Day, Common_Procedures.settings.Last_Opened_Date, Date.Today) > 0 Then
                If File.Exists(pth2) = True Then
                    File.Delete(pth2)
                End If
                If File.Exists(pth2) = False Then
                    fs = New FileStream(pth2, FileMode.Create)
                    w = New StreamWriter(fs)
                    w.WriteLine(Date.Today.ToShortDateString)
                    w.Close()
                    fs.Close()
                    w.Dispose()
                    fs.Dispose()
                End If
                Common_Procedures.First_Opened_Today = True

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT OPEN...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Attach_Existing_Databases()
        Dim cn1 As SqlClient.SqlConnection
        Dim Cmd As New SqlClient.SqlCommand
        Dim da1 As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim i As Integer = 0
        Dim NR As Integer = 0
        Dim DefPath As String, MdfPath As String, LdfPath As String, DbName As String
        Dim vfileName As String = ""
        Dim vMDFLastModified1 As Date
        Dim vLDFLastModified2 As Date

        If Common_Procedures.Server_System_Status = False Then Exit Sub

        On Error GoTo LOOP1

        cn1 = New SqlClient.SqlConnection(Common_Procedures.ConnectionString_Master)
        cn1.Open()

        Cmd.Connection = cn1

        On Error GoTo 0
        On Error GoTo - 1
        On Error GoTo LOOP2

        da1 = New SqlClient.SqlDataAdapter("select * from sysdatabases where name LIKE 'tsoft%'", cn1)
        dt1 = New DataTable
        da1.Fill(dt1)
        If dt1.Rows.Count = 0 Then

            da1 = New SqlClient.SqlDataAdapter("SELECT * FROM sysdatabases WHERE name = 'master'", cn1)
            dt2 = New DataTable
            da1.Fill(dt2)
            If dt2.Rows.Count > 0 Then

                If IsDBNull(dt2.Rows(0).Item("FileName").ToString) = False Then

                    DefPath = Replace(LCase(dt2.Rows(0).Item("FileName").ToString), "\master_data.mdf", "")
                    DefPath = Replace(LCase(dt2.Rows(0).Item("FileName").ToString), "\master.mdf", "")

                    Dim vFolderINFO As New System.IO.DirectoryInfo(DefPath)
                    Dim vFilesINFO_INFolder As System.IO.FileInfo() = vFolderINFO.GetFiles
                    'Dim vFilesINFolder As String() = Directory.GetFiles(DefPath)
                    Dim vFile1 As System.IO.FileInfo

                    For Each vFile1 In vFilesINFO_INFolder

                        vfileName = vFile1.Name

                        If LCase(vfileName) Like "tsoft*.mdf" Then

                            DbName = vfileName
                            DbName = Replace(LCase(DbName), "_data.mdf", "")
                            DbName = Replace(LCase(DbName), ".mdf", "")

                            MdfPath = vFile1.FullName
                            LdfPath = DefPath & "\" & Trim(DbName) & "_log.ldf"

                            If File.Exists(LdfPath) = True Then
                                vMDFLastModified1 = System.IO.File.GetLastWriteTime(MdfPath).ToShortDateString()
                                vLDFLastModified2 = System.IO.File.GetLastWriteTime(LdfPath).ToShortDateString()
                                If DateDiff(DateInterval.Minute, vMDFLastModified1, vLDFLastModified2) = 0 Then
                                    Cmd.CommandText = "sp_attach_db '" & Trim(DbName) & "', '" & Trim(MdfPath) & "', '" & Trim(LdfPath) & "'"
                                    Cmd.ExecuteNonQuery()

                                Else
                                    If Trim(LdfPath) <> "" Then If File.Exists(LdfPath) = True Then File.Delete(LdfPath)
                                    Cmd.CommandText = "sp_attach_single_file_db '" & Trim(DbName) & "', '" & Trim(MdfPath) & "'"
                                    Cmd.ExecuteNonQuery()
                                End If

                            Else
                                Cmd.CommandText = "sp_attach_single_file_db '" & Trim(DbName) & "', '" & Trim(MdfPath) & "'"
                                Cmd.ExecuteNonQuery()

                            End If

                        End If

                    Next vFile1

                End If

            End If
            dt2.Clear()

        End If
        dt1.Clear()


        dt1.Dispose()
        dt2.Dispose()
        da1.Dispose()

        Cmd.Dispose()

        cn1.Close()
        cn1.Dispose()

        Exit Sub

LOOP1:
        MessageBox.Show("Invalid Master Database Connection..." & Chr(13) & Err.Description, "ERROR WHILE ATTACHING DATABASE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        Me.Close()
        Application.Exit()
        End
        Exit Sub

LOOP2:
        MessageBox.Show(Err.Description, "ERROR IN ATTACHING DATABASE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        Resume Next

    End Sub

    Private Sub Check_Create_CompanyGroupDetails_DB()
        Dim cn2 As SqlClient.SqlConnection
        Dim da2 As SqlClient.SqlDataAdapter
        Dim dt2 As DataTable
        Dim Nr As Integer
        Dim pth As String
        Dim fs As FileStream
        Dim w As StreamWriter
        Dim sInpIP As String = ""

LOOP1:

        Try

            cn2 = New SqlClient.SqlConnection(Common_Procedures.ConnectionString_Master)
            cn2.Open()

            Try

                da2 = New SqlClient.SqlDataAdapter("Select name from sysdatabases where name = '" & Trim(Common_Procedures.CompanyDetailsDataBaseName) & "'", cn2)
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
                    Create_CompanyDetails_Database(cn2)
                End If

                cn2.Close()
                cn2.Dispose()

            Catch ex As Exception
                MessageBox.Show(ex.Message, "ERROR IN CHECKING/CREATING COMPANYGROUP DETAILS DB...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.Close()
                Application.Exit()
                End

            End Try

        Catch ex As Exception

            If InStr(1, Trim(LCase(ex.Message)), Trim(LCase("a network-related or instance-specific error occurred while establishing a connection to sQL server"))) > 0 Then
                If Trim(UCase(Common_Procedures.ServerWindowsLogin)) = "DIP" Then
                    If DyIPFailCount = 0 Then

                        sInpIP = InputBox("Enter Server System IP address :", "FOR CORRECT SERVER SYSTEM IP ADDRESS..")

                        If Trim(sInpIP) <> "" Then
                            pth = Trim(Common_Procedures.AppPath) & "\connection.ini"

                            If File.Exists(pth) = True Then
                                File.Delete(pth)
                            End If

                            Common_Procedures.ServerName = Trim(sInpIP)

                            fs = New FileStream(pth, FileMode.Create)
                            w = New StreamWriter(fs)
                            w.WriteLine(Trim(Common_Procedures.ServerName) & "," & Trim(Common_Procedures.ServerPassword) & ",DIP")
                            w.Close()
                            fs.Close()
                            w.Dispose()
                            fs.Dispose()

                        End If

                        Common_Procedures.ConnectionString_Master = Common_Procedures.Create_Sql_ConnectionString("master")
                        Common_Procedures.ConnectionString_CompanyGroupdetails = Common_Procedures.Create_Sql_ConnectionString(Common_Procedures.CompanyDetailsDataBaseName)
                        Common_Procedures.Connection_String = ""

                        DyIPFailCount = DyIPFailCount + 1

                        GoTo LOOP1

                    End If

                Else
                    MessageBox.Show(ex.Message, "INVALID MASTER DATABASE CONNECTION...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Me.Close()
                    Application.Exit()
                    End

                End If

            Else
                MessageBox.Show(ex.Message, "INVALID MASTER DATABASE CONNECTION...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.Close()
                Application.Exit()
                End

            End If

        End Try

    End Sub

    Private Sub Create_CompanyDetails_Database(ByVal cnmas As SqlClient.SqlConnection)
        Dim Cn1 As SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand

        cmd.Connection = cnmas

        cmd.CommandText = "Create Database " & Trim(Common_Procedures.CompanyDetailsDataBaseName)
        cmd.ExecuteNonQuery()

        Cn1 = New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)

        Cn1.Open()

        'cmd.Connection = Cn1

        'cmd.CommandText = "CREATE TABLE [CompanyGroup_Head] ( [CompanyGroup_IdNo] [smallint] NOT NULL, [CompanyGroup_Name] [varchar](100) NOT NULL, [From_Date] [smalldatetime] NOT NULL, [To_Date] [smalldatetime] NOT NULL, [Financial_Range] [varchar](10) NOT NULL, CONSTRAINT [PK_CompanyGroup_Head] PRIMARY KEY CLUSTERED ( [CompanyGroup_IdNo] ) ON [PRIMARY] ) ON [PRIMARY] "
        'cmd.ExecuteNonQuery()

        'cmd.CommandText = "CREATE TABLE [Settings_Head] ( [Auto_SlNo] [int] IDENTITY(1,1) NOT NULL, [SoftWare_UserType] [varchar](50) NULL CONSTRAINT [DF_Settings_Head_Software_Type] DEFAULT (''), [Sdd] [smalldatetime] NULL, [SddName] [varchar](100) NULL CONSTRAINT [DF_SettingsHead_SddName] DEFAULT (''), [Cc_No] [varchar](50) NULL DEFAULT ('0001'), [S_Name] [varchar](50) NULL DEFAULT (''), CONSTRAINT [PK_Settings_Head] PRIMARY KEY CLUSTERED ( [Auto_SlNo] ) ON [PRIMARY] ) ON [PRIMARY]"
        'cmd.ExecuteNonQuery()

        'cmd.CommandText = "CREATE TABLE [User_Head] ( [User_IdNo] [smallint] NOT NULL, [User_Name] [varchar](50) NOT NULL, [Sur_Name] [varchar](50) NOT NULL, [Account_Password] [varchar](50) NULL CONSTRAINT [DF_User_Head_Account_Password]  DEFAULT (''), [UnAccount_Password] [varchar](50) NULL CONSTRAINT [DF_User_Head_UnAccount_Password]  DEFAULT (''), CONSTRAINT [PK_User_Head] PRIMARY KEY CLUSTERED  ( [User_IdNo] ) ON [PRIMARY] ) ON [PRIMARY] "
        'cmd.ExecuteNonQuery()

        'cmd.CommandText = "CREATE TABLE [User_Access_Rights] ( [User_IdNo] [smallint] NOT NULL, [Entry_Code] [varchar](100) NOT NULL, [Access_Type] [varchar](50) NULL, CONSTRAINT [PK_User_Access_Details] PRIMARY KEY NONCLUSTERED  ( [User_IdNo], [Entry_Code] ) ON [PRIMARY] ) ON [PRIMARY]"
        'cmd.ExecuteNonQuery()

        'cmd.CommandText = "CREATE TABLE [AutoBackup_Path_Head] ( [Auto_SlNo] [int] IDENTITY(1,1) NOT NULL,	[Computer_Name] [varchar](100) NOT NULL,	[App_Path] [varchar](500) NULL,  CONSTRAINT [PK_AutoBackup_Path_Head] PRIMARY KEY CLUSTERED (  [Computer_Name] ) ON [PRIMARY] ) ON [PRIMARY]"
        'cmd.ExecuteNonQuery()

        'cmd.Dispose()

        Call FieldCheck_CompanyGroupDetails_Db(Cn1)

        Call DefaultValues_CompanyGroupDetails_Db(Cn1)

        Cn1.Close()
        Cn1.Dispose()

    End Sub


    Private Sub FieldCheck_CompanyGroupDetails_Db(ByVal cn1 As SqlClient.SqlConnection)
        Dim cmd As New SqlClient.SqlCommand
        Dim Dat As Date
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim ServrNm As String = ""
        Dim Nr As Long = 0
        Dim vFileDtTm As Date
        Dim vPATHName1 As String
        Dim pth2 As String
        Dim fs As FileStream
        Dim r As StreamReader
        Dim w As StreamWriter
        Dim vACCNO As String, vCCNO As String, vCCNO_ENCRYP_STR As String



        On Error Resume Next

        cmd.Connection = cn1

        '----------------------------------------------------------------------------------------------------

        cmd.CommandText = "CREATE TABLE [CompanyGroup_Head] ( [CompanyGroup_IdNo] [smallint] NOT NULL, [CompanyGroup_Name] [varchar](100) NOT NULL, [From_Date] [smalldatetime] NOT NULL, [To_Date] [smalldatetime] NOT NULL, [Financial_Range] [varchar](10) NOT NULL, CONSTRAINT [PK_CompanyGroup_Head] PRIMARY KEY CLUSTERED ( [CompanyGroup_IdNo] ) ON [PRIMARY] ) ON [PRIMARY] "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [Settings_Head] ( [Auto_SlNo] [int] IDENTITY(1,1) NOT NULL, [SoftWare_UserType] [varchar](50) NULL CONSTRAINT [DF_Settings_Head_Software_Type] DEFAULT (''), [Sdd] [smalldatetime] NULL, [SddName] [varchar](100) NULL CONSTRAINT [DF_SettingsHead_SddName] DEFAULT (''), [Cc_No] [varchar](50) NULL DEFAULT ('0001'), [S_Name] [varchar](50) NULL DEFAULT (''), CONSTRAINT [PK_Settings_Head] PRIMARY KEY CLUSTERED ( [Auto_SlNo] ) ON [PRIMARY] ) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [User_Head] ( [User_IdNo] [smallint] NOT NULL, [User_Name] [varchar](50) NOT NULL, [Sur_Name] [varchar](50) NOT NULL, [Account_Password] [varchar](50) NULL CONSTRAINT [DF_User_Head_Account_Password]  DEFAULT (''), [UnAccount_Password] [varchar](50) NULL CONSTRAINT [DF_User_Head_UnAccount_Password]  DEFAULT (''), CONSTRAINT [PK_User_Head] PRIMARY KEY CLUSTERED  ( [User_IdNo] ) ON [PRIMARY] ) ON [PRIMARY] "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [User_Access_Rights] ( [User_IdNo] [smallint] NOT NULL, [Entry_Code] [varchar](500) NOT NULL, [Access_Type] [varchar](100) NULL, CONSTRAINT [PK_User_Access_Details] PRIMARY KEY NONCLUSTERED  ( [User_IdNo], [Entry_Code] ) ON [PRIMARY] ) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [AutoBackup_Path_Head] ( [Auto_SlNo] [int] IDENTITY(1,1) NOT NULL,	[Computer_Name] [varchar](100) NOT NULL,	[App_Path] [varchar](500) NULL,  CONSTRAINT [PK_AutoBackup_Path_Head] PRIMARY KEY CLUSTERED (  [Computer_Name] ) ON [PRIMARY] ) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [eInvoice_eWay_Bill_Details] ( [CompanyGroup_IdNo] [int] Not NULL, [Year_Code] [varchar](50) Not NULL, [Entry_Type] [varchar](50) Not NULL, [Document_Code] [varchar](50) Not NULL, [EInvoice_IRN_No] [varchar](100) NULL  DEFAULT (''), [EInvoice_ACK_No] [varchar](100) NULL  DEFAULT (''), [EInvoice_ACK_Date] [varchar](50) NULL  DEFAULT (''), [EInvoice_IRN_QRCode_Image] [image] NULL  DEFAULT (''), [EWay_BillNo] [varchar](100) NULL  DEFAULT (''), [EWay_BillDate] [varchar](50) NULL  DEFAULT (''), CONSTRAINT [PK_eInvoice_eWay_Bill_Details] PRIMARY KEY CLUSTERED ( [CompanyGroup_IdNo] , [Year_Code] , [Entry_Type] , [Document_Code]  )  ON [PRIMARY] ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        '----PayRoll Login User Head  -  Computer_SerialNo , User_IdNo, CompanyGroup_IdNo
        cmd.CommandText = "CREATE TABLE [pyr_luh] ( [Auto_SlNo] [int] IDENTITY(1,1) NOT NULL,	[csno] [varchar](100) NOT NULL, [usid] smallint NULL,  [cgid] smallint NULL,  CONSTRAINT [PK_pyr_luh] PRIMARY KEY CLUSTERED ( [csno] ) ON [PRIMARY] ) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        '----------------------------------------------------------------------------------------------------

        cmd.CommandText = "Alter table User_Access_Rights DROP CONSTRAINT [PK_User_Access_Details]"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Alter table User_Access_Rights alter column Entry_Code varchar(500) NOT NULL"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table User_Access_Rights add Software_Module_IdNo smallint"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update User_Access_Rights set Software_Module_IdNo = 0 Where Software_Module_IdNo is Null"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Alter table User_Access_Rights alter column Software_Module_IdNo smallint NOT NULL"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "ALTER TABLE User_Access_Rights Add CONSTRAINT [PK_User_Access_Details] PRIMARY KEY CLUSTERED ( [User_IdNo]  , [Entry_Code] , [Software_Module_IdNo] ) ON [PRIMARY] "
        cmd.ExecuteNonQuery()

        '----------------------------------------------------------------------------------------------------

        cmd.CommandText = "Alter table User_Head add ModuleWise_Access_Rights varchar(2000) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update User_Head set ModuleWise_Access_Rights = '' Where ModuleWise_Access_Rights is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table e_Invoice_Refresh add CompanyGroup_IdNo int Default 0"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "CREATE TABLE [AWS_KEY_SETTINGS] ( [AWS_ACCESS_KEY] [varchar](900) NOT NULL, [AWS_SECRET_KEY] [varchar](1000) NULL, 	[AWS_BUCKET_FOR_DB] [varchar](100) NULL, 	[AWS_BUCKET_FOR_SW] [varchar](100) NULL, 	[AWS_BUCKET_FOR_DOWNLOADER] [varchar](100) NULL, 	[AWS_FOLDER_FOR_SW_PROGRAMS] [varchar](250) NULL, 	[AWS_FOLDER_FOR_SW_REPORTS] [varchar](250) NULL,  CONSTRAINT [PK_AWS_KEY_SETTINGS] PRIMARY KEY CLUSTERED  ( [AWS_ACCESS_KEY] ) ON [PRIMARY] ) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "truncate table [AWS_KEY_SETTINGS]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "INSERT into [AWS_KEY_SETTINGS] (   [AWS_ACCESS_KEY]   ,     [AWS_SECRET_KEY]                      ,  [AWS_BUCKET_FOR_DB]  ,   [AWS_BUCKET_FOR_SW] , [AWS_BUCKET_FOR_DOWNLOADER],  [AWS_FOLDER_FOR_SW_PROGRAMS],  [AWS_FOLDER_FOR_SW_REPORTS] ) " &
                            " VALUES                      ('AKIA2OEHJFCG3IP6ZBCZ', 'G//jrH+3YNGGOppl0MSWM84vXswBkbbr/qUOfFG4',   'armtdbfiles'       , 'tsoftsoftwareupdates',         'tsoftdownloader'  , 'textile/programfiles'       ,     'textile/reports'        ) "
        cmd.ExecuteNonQuery()

        'cmd.CommandText = "INSERT into [AWS_KEY_SETTINGS] (   [AWS_ACCESS_KEY]   ,     [AWS_SECRET_KEY]                      ,  [AWS_BUCKET_FOR_DB],   [AWS_BUCKET_FOR_SW]     , [AWS_BUCKET_FOR_DOWNLOADER],  [AWS_FOLDER_FOR_SW_PROGRAMS]   ,  [AWS_FOLDER_FOR_SW_REPORTS] ) " &
        '                    " VALUES                      ('AKIAQDQ3Z6TGQHMBRQHE', 'fPlLN+G33py9c59k35eQRYBtZ4cTdDX4w8oFHw9n',     'ndbfiles'      , 'tsoftsoftwarefordownload',     'tsoftdownloader'      , 'tsofttextile2019/programfiles' ,   'tsofttextile2019/reports' )"
        'cmd.ExecuteNonQuery()

        'cmd.CommandText = "INSERT into [AWS_KEY_SETTINGS] (   [AWS_ACCESS_KEY]   ,     [AWS_SECRET_KEY]                      ,  [AWS_BUCKET_FOR_DB],   [AWS_BUCKET_FOR_SW]     , [AWS_BUCKET_FOR_DOWNLOADER],  [AWS_FOLDER_FOR_SW_PROGRAMS],  [AWS_FOLDER_FOR_SW_REPORTS] ) " &
        '                    " VALUES                      ('AKIAQDQ3Z6TGQHMBRQHE', 'fPlLN+G33py9c59k35eQRYBtZ4cTdDX4w8oFHw9n',     'ndbfiles'      , 'tsoftsoftwarefordownload', 'tsoftsoftwaredownloader'  , 'tsofttextile2019/programfiles'       ,     'tsofttextile2019/reports'        )"
        'cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Settings_Head add ExeFile_DateTime datetime"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Settings_Head set ExeFile_DateTime = '1/1/2000' Where ExeFile_DateTime is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table CompanyGroup_Head add Transfer_To_CompanyGroupIdNo int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update CompanyGroup_Head set Transfer_To_CompanyGroupIdNo = 0 Where Transfer_To_CompanyGroupIdNo is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table User_Head add Show_Verified_Status Int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update User_Head set Show_Verified_Status = 0 Where Show_Verified_Status  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table User_Head add Show_UserCreation_Status Int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update User_Head set Show_UserCreation_Status = 0 Where Show_UserCreation_Status is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table User_Head add Close_Status Int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update User_Head set Close_Status = 0 Where Close_Status is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table User_Head add ADD_LAST_n_DAYS Int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update User_Head set ADD_LAST_n_DAYS = 0 Where ADD_LAST_n_DAYS is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table User_Head add EDIT_LAST_n_DAYS Int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update User_Head set EDIT_LAST_n_DAYS = 0 Where EDIT_LAST_n_DAYS is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table User_Head add DELETE_LAST_n_DAYS Int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update User_Head set DELETE_LAST_n_DAYS = 0 Where DELETE_LAST_n_DAYS is Null"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "CREATE TABLE [System_name_details] ( [Type] [varchar](100) NULL default (''), [Computer_name] [varchar](500) NOT NULL, [Computer_serialNo] [varchar](100) NOT NULL, [Sql_Instance_name] [varchar](1000) NULL default (''), [Sql_data_path] [varchar](1000) NULL default (''), [Software_Path] [varchar](1000) NULL default (''), [Software_Exe_name] [varchar](100) NULL default (''), [Exe_Date_time] [smalldatetime] NULL, CONSTRAINT [PK_System_name_details] PRIMARY KEY CLUSTERED ( [Computer_name] ) ON [PRIMARY]  , CONSTRAINT [IX_System_name_details] UNIQUE NONCLUSTERED ( [Computer_serialNo]  ) ON [PRIMARY] ) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table System_name_details add Software_Path varchar(1000) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update System_name_details set Software_Path = '' Where Software_Path is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table System_name_details add Last_Opened_SystemDateTime datetime"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table CompanyGroup_Head add Cc_No varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update CompanyGroup_Head set Cc_No = '' Where Cc_No is Null"
        cmd.ExecuteNonQuery()


        cmd.CommandText = "Alter table CompanyGroup_Head add CcNo_OrderBy numeric(18,2) default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update CompanyGroup_Head set CcNo_OrderBy = 0 Where CcNo_OrderBy is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE [AutoBackup_Path_Head] ( [Auto_SlNo] [int] IDENTITY(1,1) NOT NULL,	[Computer_Name] [varchar](100) NOT NULL,	[App_Path] [varchar](500) NULL,  CONSTRAINT [PK_AutoBackup_Path_Head] PRIMARY KEY CLUSTERED ( [Computer_Name] ) ON [PRIMARY] ) ON [PRIMARY]"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "  ALTER TABLE Settings_Head ALTER COLUMN Autobackup_Path_Server varchar(500)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Settings_Head add Autobackup_Path_Server varchar(500) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Settings_Head set Autobackup_Path_Server = '' Where Autobackup_Path_Server is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Settings_Head add Autobackup_Path_Client1 varchar(500) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Settings_Head set Autobackup_Path_Client1 = '' Where Autobackup_Path_Client1 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Settings_Head add Autobackup_Path_Client2 varchar(500) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Settings_Head set Autobackup_Path_Client2 = '' Where Autobackup_Path_Client2 is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Settings_Head add Autobackup_PenDrive_Path_Server varchar(500) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Settings_Head set Autobackup_PenDrive_Path_Server = '' Where Autobackup_PenDrive_Path_Server is Null"
        cmd.ExecuteNonQuery()

        'ServrNm = Common_Procedures.get_Server_SystemName()
        'If Trim(UCase(ServrNm)) = Trim(UCase(SystemInformation.ComputerName)) Then
        If Common_Procedures.Server_System_Status = True Then

            If Not (Trim(UCase(Common_Procedures.ServerWindowsLogin)) = "IP" Or Trim(UCase(Common_Procedures.ServerWindowsLogin)) = "SIP" Or Trim(UCase(Common_Procedures.ServerWindowsLogin)) = "DIP" Or Trim(UCase(Common_Procedures.ServerWindowsLogin)) = "ONLINE") Then

                Nr = 0
                cmd.CommandText = "update Settings_Head set Autobackup_Path_Server = '" & Trim(Common_Procedures.AppPath) & "'"
                Nr = cmd.ExecuteNonQuery()
                If Nr = 0 Then
                    cmd.CommandText = "Insert into Settings_Head ( Autobackup_Path_Server ) values ('" & Trim(Common_Procedures.AppPath) & "')"
                    cmd.ExecuteNonQuery()
                End If

                Dim vNOPENDRIVE_DATE_STR As String
                vNOPENDRIVE_DATE_STR = Common_Procedures.Encrypt(Now, Trim(Common_Procedures.ONLINE_OMS_CONNECTION_CHECK.passPhrase), Trim(Common_Procedures.ONLINE_OMS_CONNECTION_CHECK.saltValue))

                cmd.CommandText = "update Settings_Head set Autobackup_PenDrive_Path_Server = ''"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Settings_Head set nops = 1, nopd = '" & Trim(vNOPENDRIVE_DATE_STR) & "' Where nops = 0"
                Nr = cmd.ExecuteNonQuery()

                vPATHName1 = ""

                pth2 = Trim(Common_Procedures.AppPath) & "\pdp.txt"

                If File.Exists(pth2) = True Then
                    File.Delete(pth2)
                End If
                If File.Exists(pth2) = False Then
                    fs = New FileStream(pth2, FileMode.Create)
                    w = New StreamWriter(fs)
                    w.WriteLine(Trim(vPATHName1))
                    w.Close()
                    fs.Close()
                    w.Dispose()
                    fs.Dispose()
                End If

                Dim allDrives() As DriveInfo = DriveInfo.GetDrives()

                Dim d As DriveInfo

                For Each d In allDrives

                    If d.IsReady = True Then



                        If d.DriveType = DriveType.Removable Then

                            vPATHName1 = Trim(d.Name) & "TSOFT\Auto_BackUP"

                            If System.IO.Directory.Exists(vPATHName1) = False Then
                                System.IO.Directory.CreateDirectory(vPATHName1)
                            End If

                            If System.IO.Directory.Exists(vPATHName1) = True Then

                                Nr = 0
                                cmd.CommandText = "update Settings_Head set Autobackup_PenDrive_Path_Server = '" & Trim(vPATHName1) & "', nops = 0, nopd = ''"
                                Nr = cmd.ExecuteNonQuery()
                                If Nr = 0 Then
                                    cmd.CommandText = "Insert into Settings_Head ( Autobackup_PenDrive_Path_Server , nops, nopd ) values ('" & Trim(vPATHName1) & "', 0, '')"
                                    cmd.ExecuteNonQuery()
                                End If

                                If File.Exists(pth2) = True Then
                                    File.Delete(pth2)
                                End If
                                If File.Exists(pth2) = False Then
                                    fs = New FileStream(pth2, FileMode.Create)
                                    w = New StreamWriter(fs)
                                    w.WriteLine(Trim(vPATHName1))
                                    w.Close()
                                    fs.Close()
                                    w.Dispose()
                                    fs.Dispose()
                                End If

                                Exit For

                            End If

                        End If

                    End If

                Next

            End If

        End If

        cmd.CommandText = "Alter table CompanyGroup_Head add CGT Int default 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update CompanyGroup_Head set CGT  = 0 Where CGT  is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Settings_Head add Auto_SlNo [int] IDENTITY (1, 1) NOT NULL"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Settings_Head add SoftWare_UserType varchar(50) default 'SINGLE USER'"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Settings_Head set SoftWare_UserType = 'SINGLE USER' Where SoftWare_UserType is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Settings_Head add Sdd smalldatetime"
        cmd.ExecuteNonQuery()
        Dat = #1/1/2000#
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@SystemDate", Dat.ToShortDateString)
        cmd.CommandText = "Update Settings_Head set Sdd = @SystemDate Where Sdd is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Settings_Head add SddName varchar(100) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Settings_Head set SddName = '' Where SddName is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Settings_Head add Cc_No varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Settings_Head set Cc_No = '' Where Cc_No is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Settings_Head add S_Name varchar(50) default ''"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Settings_Head set S_Name = '' Where S_Name is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Settings_Head add ooccd varchar(500) default ''"     '--- ONLINE_OMS_CONNECTION_CHECK_DATE 
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Settings_Head set ooccd = '' Where ooccd is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Settings_Head add accno varchar(500) default ''"     '---Actual_Customer_Code_Number
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Settings_Head set accno = '' Where accno is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Settings_Head add nops tinyint default 0"            '---- NO_PENDRIVE_STATUS
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Settings_Head set nops = 0 Where nops is Null"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Alter table Settings_Head add nopd varchar(500) default ''"      '---- NO_PENDRIVE_DATE
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Update Settings_Head set nopd = '' Where nopd is Null"
        cmd.ExecuteNonQuery()


        cmd.Parameters.Clear()

        vCCNO = ""
        vACCNO = ""

        da1 = New SqlClient.SqlDataAdapter("select * from Settings_Head", cn1)
        dt1 = New DataTable
        da1.Fill(dt1)

        If dt1.Rows.Count = 0 Then
            cmd.CommandText = "truncate table Settings_Head"
            cmd.ExecuteNonQuery()

            Dat = #1/1/2000#
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@SystemDate", Dat.ToShortDateString)

            cmd.CommandText = "Insert into Settings_Head(SoftWare_UserType, Sdd, SddName ) values ('SINGLE USER', @SystemDate, '" & Trim(SystemInformation.ComputerName) & "') "
            cmd.ExecuteNonQuery()

        Else

            If IsDBNull(dt1.Rows(0).Item("Cc_No").ToString) = False Then
                vCCNO = dt1.Rows(0).Item("Cc_No").ToString()
                vCCNO = Microsoft.VisualBasic.Left(Trim(vCCNO), 4)
            End If
            If IsDBNull(dt1.Rows(0).Item("accno").ToString) = False Then
                vACCNO = dt1.Rows(0).Item("accno").ToString()
            End If

        End If
        dt1.Clear()

        If Trim(vACCNO) = "" Then

            vCCNO_ENCRYP_STR = ""
            If Trim(vCCNO) <> "" And Trim(vCCNO) <> "0001" And Val(vCCNO) <> 0 And Val(vCCNO) <> 1 Then
                vCCNO_ENCRYP_STR = Common_Procedures.Encrypt(Trim(vCCNO), Trim(Common_Procedures.ACTUAL_CUSTOMER_CODE_NUMBER.passPhrase), Trim(Common_Procedures.ACTUAL_CUSTOMER_CODE_NUMBER.saltValue))
            End If

            If Trim(vCCNO_ENCRYP_STR) <> "" Then

                Nr = 0
                cmd.CommandText = "update settings_head set accno = '" & Trim(vCCNO_ENCRYP_STR) & "'"
                Nr = cmd.ExecuteNonQuery()
                If Nr = 0 Then
                    cmd.CommandText = "insert into settings_head (accno) values ('" & Trim(vCCNO_ENCRYP_STR) & "')"
                    cmd.ExecuteNonQuery()
                End If

            End If

        End If

        da1 = New SqlClient.SqlDataAdapter("select * from User_Head Where User_IdNo = 1", cn1)
        dt1 = New DataTable
        da1.Fill(dt1)
        If dt1.Rows.Count = 0 Then

            cmd.CommandText = "Delete from User_Head where user_idno = 0 or user_idno = 1"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into User_Head(User_IdNo, User_Name, Sur_Name, Account_Password, UnAccount_Password) values (0, '', '', '', '') "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into User_Head(User_IdNo, User_Name, Sur_Name, Account_Password, UnAccount_Password) values (1, 'Admin', 'Admin', 'TSOFT', 'TS2') "
            cmd.ExecuteNonQuery()

        End If
        dt1.Clear()

        dt1.Dispose()
        da1.Dispose()

        cmd.Dispose()

    End Sub

    Private Sub DefaultValues_CompanyGroupDetails_Db(ByVal cn1 As SqlClient.SqlConnection)
        Dim cmd As New SqlClient.SqlCommand
        Dim Dat As Date

        cmd.Connection = cn1

        cmd.CommandText = "truncate table Settings_Head"
        cmd.ExecuteNonQuery()

        Dat = #1/1/2000#
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@SystemDate", Dat.ToShortDateString)

        cmd.CommandText = "Insert into Settings_Head(SoftWare_UserType, Sdd, SddName) values ('SINGLE USER', @SystemDate, '" & Trim(SystemInformation.ComputerName) & "') "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Delete from User_Head where user_idno = 0 or user_idno = 1"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into User_Head(User_IdNo, User_Name, Sur_Name, Account_Password, UnAccount_Password) values (0, '', '', '', '') "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into User_Head(User_IdNo, User_Name, Sur_Name, Account_Password, UnAccount_Password) values (1, 'Admin', 'Admin', 'r483BKHNEss1mgh7jPnS6w==', 'ZI0ukC2Q5dmh46/zr1m9SA==') "
        'cmd.CommandText = "Insert into User_Head(User_IdNo, User_Name, Sur_Name, Account_Password, UnAccount_Password) values (1, 'Admin', 'Admin', 'TSOFT', 'TS2') "
        cmd.ExecuteNonQuery()

        cmd.Dispose()

    End Sub

    Private Sub Check_Update_SystemDateTime(ByVal Cn2 As SqlClient.SqlConnection)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vDBdate_SDD As Date = #1/1/2000#
        Dim vSYSDate As Date = #1/1/1980#
        Dim vSERVERdate As Date = #1/1/1981#
        Dim lckdt As Date
        Dim Nr As Integer
        Dim DatChkSTS As Boolean = False
        Dim vFileDtTm As Date
        Dim dttm1 As Date = #1/1/2000#
        Dim vFileDatTimSTS As Boolean = False

        Try

            DatChkSTS = True
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "--1018--" Then '---- MK TEXTILES (PALLADAM)
                lckdt = #07/21/2026#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1050" Then '---- Kumaravel Textiles (Palladam)
                lckdt = #09/30/2025#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- GANAPATHY SPINNING MILLS
                lckdt = #01/14/2026#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1075" Then '---- JR Textiles (Somanur) Stantly   (or)  M.S FABRICS
                lckdt = #03/30/2026#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1113" Then  '---- NIRUBAA FABRIC MILLS (COIMBATORE)
                lckdt = #3/3/2020#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1118" Then  '---- KASTUR LAXMI MILLS
                lckdt = #9/14/2026#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1120" Then   '----- ALLWIN FABS (or) MARIA INTERNATIONAL (SOMANUR)
                lckdt = #10/19/2025#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1124" Then '---- Aravind Textiles (Somanur)
                lckdt = #3/3/2022#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1129" Then '---- BALAJI TEXTILES (AVINASHI)
                lckdt = #7/28/2026#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1135" Then '---- Maria Fab (Karumathampatti)
                lckdt = #7/10/2026#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1136" Then '---- PS Textiles (Somanur)
                lckdt = #7/30/2026#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1139" Then '---- SIVAKUMAR TEXTILES (THEKKALUR)
                lckdt = #11/22/2025#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1140" Then '---- ANANTHARAJA TEXTILES (AVINASHI)
                lckdt = #9/15/2026#


            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1149" Then '---- SRI MAYAVA TEX (TIRUPUR)
                lckdt = #10/15/2025#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1164" Then '----SAROJINI TEXTILES (63 VELAMPALAYAM)
                lckdt = #10/15/2026#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1169" Then '---- Sri Ganesha Textiles (Somanur)
                lckdt = #05/30/2026#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Then '------ T.S TEXTILE (SOMANUR)   /   TS TEXTILE (SOMANUR)
                lckdt = #8/14/2026#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Then '-----SRI SAKTHIVINAYAGA TEXTILES - AYYANKOIL
                lckdt = #6/29/2026#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1202" Then '-----VINAYAKA TEXTILES (KARUVALUR)
                lckdt = #3/3/2022#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1203" Then '---- MAHADEVI TEXTILES (THEKKALUR)
                lckdt = #12/12/2025#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1213" Then '-----SRI KARUNAMBIGAI TEXTILES (SOMANUR)
                lckdt = #9/9/2022#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1223" Then '---- SUNLAAND AUTO WEAVE (SOMANUR) or SUNLAND AUTO WEAVE (SOMANUR) or  SUN LAND AUTO WEAVE (SOMANUR) 
                lckdt = #4/4/2022#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1249" Then '---- VAIPAV TEXTILES PVT LTD (SOMANUR)
                lckdt = #8/30/2026#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1251" Then '---- SRI SARANYA TEXTILES (THEKKALUR)
                lckdt = #8/23/2026#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1266" Then '---- CORAL WEAVERS (PALLADAM)
                lckdt = #9/25/2025#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1273" Then   ' ---- SHREE DEVI TEXTILES (KARUVALUR)
                lckdt = #08/23/2026#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1276" Then '---- DHANVI IMPEX (SOMANUR)
                lckdt = #09/28/2026#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1283" Then '----ARUL MURUGAN TEXTILES (SOMANUR)
                lckdt = #9/21/2026#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1290" Then '---- ARJUNA TEXTILES (SOMANUR)
                lckdt = #3/3/2022#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1292" Then '---- SARVESWARA SPINNING (SOMANUR)
                lckdt = #3/3/2022#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1294" Then '---- CHITRA TEX (SOMANUR)
                lckdt = #7/25/2026#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1305" Then '---- VAKUL EXPORTS (SOMANUR)
                lckdt = #6/29/2026#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1306" Then '---- SRI VIGNESHWARA MILLS (PALLADAM)
                lckdt = #8/8/2026#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1312" Then '---------- SREE VENGATESWARA FABRICS (ERODE)  - SREE VENKATESWARA FABRICS (ERODE)
                lckdt = #5/12/2022#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1313" Then '----------  Sri Guru Fabrics(SOMANUR)
                lckdt = #10/17/2026#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1319" Then '---- AMMAN TRADERS (SOMANUR)   and  SRI JAYANTHI TEXTILES (SOMANUR)
                lckdt = #9/16/2026#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1325" Then '---------- G S ELECTRONICS (KARUVALUR)
                lckdt = #5/5/2022#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1331" Then '---------- SUBASRI TEXTILES (AVINASHI)  SUBA SRI TEXTILES (AVINASHI)
                lckdt = #7/27/2026#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1358" Then '---- Parameshwari Textile (Avinashi)
                lckdt = #9/23/2024#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1394" Then '----SRI RAMKUMAR TEX (SOMANUR)
                lckdt = #11/15/2026#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1398" Then '---- A R TRADERS (BHAVANI)
                lckdt = #10/21/2026#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1401" Then '---- GUNASUNDARI FIREWOODS (SOMANUR)
                lckdt = #12/22/2026#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1404" Then '---- MJK TEXTILES (PALLADAM)
                lckdt = #8/13/2025#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1410" Then '---- SATHY TEXTILES (PALLADAM)
                lckdt = #8/13/2025#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1441" Then '-----GRANDMAX WEAVES PRIVATE LIMITED (AVINASHI)
                lckdt = #6/14/2023#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1452" Then '---- THANGAM C TEX (KARUMATHAMPATTI)
                lckdt = #05/16/2026#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1453" Then '---- VELAN MILLS(PALLADAM)
                lckdt = #6/28/2025#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1455" Then '---- Veera Tex (Palladam)
                lckdt = #8/20/2025#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1465" Then '---- SARASATHII TEXTIES (SOMANUR)
                lckdt = #6/30/2026#

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1469" Then '---- R.S.S TEX(PALLADAM)
                'lckdt = #05/28/2025#
                lckdt = #11/11/2025# '- -- for arav tex

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1494" Then '---- DHARA TEX (TIRUPUR)
                lckdt = #8/10/2026#

            ElseIf Val(Common_Procedures.settings.CustomerDBCode) < 1000 Or Val(Common_Procedures.settings.CustomerDBCode) > 9999 Then
                lckdt = #4/30/2026#

            Else

                DatChkSTS = False

                If vLockStatus_Frm_LicFile = True Then

                    lckdt = vLockDate_Frm_LicFile  ' #11/11/2099#
                    DatChkSTS = vLockStatus_Frm_LicFile

                Else

                    vFileDtTm = get_File_Modified_DateTime(Application.ExecutablePath)
                    vFileDatTimSTS = False
                    If Trim(vFileDtTm) <> "" Then
                        If IsDate(vFileDtTm) = True Then
                            If DateDiff(DateInterval.Day, dttm1, vFileDtTm) > 0 Then
                                lckdt = DateAdd(DateInterval.Day, 675, vFileDtTm)
                                vFileDatTimSTS = True
                                DatChkSTS = True
                            End If
                        End If
                    End If

                    If vFileDatTimSTS = False Then
                        lckdt = #11/11/2099#
                        DatChkSTS = False
                    End If

                End If

            End If

            If Trim(Common_Procedures.settings.Sdd) <> "" Then
                If IsDate(Common_Procedures.settings.Sdd) = True Then
                    vDBdate_SDD = Common_Procedures.settings.Sdd
                End If
            End If

            If Common_Procedures.Server_System_Status = False Then

                vSERVERdate = Common_Procedures.get_Server_Date(Cn2)
                vSYSDate = Now.Date

                If DateDiff("d", vSYSDate, vSERVERdate) <> 0 Then
                    Try
                        Change_System_DateTime_To_Internet_DateTime()
                    Catch ex As Exception
                        '-----
                    End Try
                End If

                System.Threading.Thread.Sleep(2000)

                vSYSDate = Now.Date

                If DateDiff("d", vSYSDate, vSERVERdate) <> 0 Then
                    GoTo GOTOLABEL1
                    'MessageBox.Show("You must set the correct date (" & Format(vSYSDate, "dd/MM/yyyy") & ") on the client system, as in the server(" & Format(vSERVERdate, "dd/MM/yyyy") & ").", "ERROR IN SYSTEM DATE FORMAT CHECKING....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    'Me.Close()
                    'Application.Exit()
                    'End
                End If

            End If

            If vLockStatus_Frm_LicFile = True Or DatChkSTS = True Or (Val(Common_Procedures.settings.CustomerDBCode) < 1000 Or Val(Common_Procedures.settings.CustomerDBCode) > 9999) Then

                vSYSDate = Now.Date  ' Common_Procedures.get_Server_Date(Cn2)

                If DateDiff("d", vDBdate_SDD, vSYSDate) < 0 Then
                    Try
                        Change_System_DateTime_To_Internet_DateTime()
                        System.Threading.Thread.Sleep(1000)
                    Catch ex As Exception
                        '-----
                    End Try
                End If

                vSYSDate = Now.Date  ' Common_Procedures.get_Server_Date(Cn2)

                If DateDiff("d", vDBdate_SDD, vSYSDate) < 0 Then
                    GoTo GOTOLABEL1
                    'MessageBox.Show("Invalid System Date - Set correct date in specified format (dd/MM/yyyy)", "ERROR IN SYSTEM DATE FORMAT CHECKING....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    'Me.Close()
                    'Application.Exit()
                    'End
                End If

            End If

            vSYSDate = Now.Date

            If DateDiff("d", vDBdate_SDD, vSYSDate) > 0 Then

                Try
                    Change_System_DateTime_To_Internet_DateTime()
                Catch ex As Exception
                    '-----
                End Try

                System.Threading.Thread.Sleep(100)

                vSYSDate = Now.Date

                If DateDiff("d", vDBdate_SDD, vSYSDate) > 0 Then

                    vDBdate_SDD = vSYSDate
                    Common_Procedures.settings.Sdd = vSYSDate

                    cmd.Connection = Cn2

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@SystemDate", vDBdate_SDD.Date)

                    Nr = 0
                    cmd.CommandText = "Update Settings_Head set Sdd = @SystemDate, SddName = '" & Trim(SystemInformation.ComputerName) & "'"
                    Nr = cmd.ExecuteNonQuery()

                    If Nr = 0 Then
                        cmd.CommandText = "Insert into Settings_Head(Sdd, SddName) values (@SystemDate, '" & Trim(SystemInformation.ComputerName) & "')"
                        cmd.ExecuteNonQuery()
                    End If

                End If

            End If

            If Common_Procedures.Office_System_Status = True Then
                Exit Sub
            End If

            If vLockStatus_Frm_LicFile = True Or DatChkSTS = True Or (Val(Common_Procedures.settings.CustomerDBCode) < 1000 Or Val(Common_Procedures.settings.CustomerDBCode) > 9999) Then

                If DateDiff("d", lckdt.ToShortDateString, Date.Today.ToShortDateString) > 0 Then

                    If vLockStatus_Frm_LicFile = True Then
                        MessageBox.Show("SoftWare Trial Period Expires", "TSOFT SOLUTIONS...", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error)

                    Else

GOTOLABEL1:
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1149" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1203" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1350" Then   '---- MALAR COTTON (VALAYAPALAYAM, 63.VELAMPALAYAM)
                            MessageBox.Show("The instruction at '0x00000082', Attempt to use a file handle to an open disk partition for an operation other than raw disk I/O." & vbCrLf & vbCrLf & "Click on OK to terminate the Program ", "Textile.exe - Application Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error)
                        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1075" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1101" Then '---- AVR MILLS PRIVATE LMITED (GOBI)
                            MessageBox.Show("The instruction at '0x04b0242c' referenced memory at '0x04b0242c'." & vbCrLf & "The memory could not be 'written'.  " & vbCrLf & vbCrLf & "Click on OK to terminate the Program ", "Textile.exe - Application Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error)
                        Else
                            MessageBox.Show("Database '" & Common_Procedures.DataBaseName & "' cannot be opened. It has been marked SUSPECT by recovery. See the SQL Server errorlog for more information. (Microsoft SQL Server, Error: 926)", "TSOFT TEXTILE...", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error)
                            'MessageBox.Show("Error in loading file", "TSOFT TEXTILE", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If

                    End If

                    Me.Close()
                    Application.Exit()
                    End

                End If

            End If

            Demo_Data_Checking(Cn2)


        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR IN DATE CHECKING", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        cmd.Dispose()

    End Sub

    Private Sub Get_CompanyGroupDetails_SettingsValue(ByVal Cn1 As SqlClient.SqlConnection)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        Try

            Call FieldCheck_CompanyGroupDetails_Db(Cn1)

            Common_Procedures.settings.CompanyName = ""
            Common_Procedures.settings.CustomerCode = ""
            Common_Procedures.settings.CustomerDBCode = ""
            Common_Procedures.settings.SoftWareName = ""
            Common_Procedures.settings.AutoBackUp_Date = #1/1/1900#
            Common_Procedures.settings.Autobackup_PenDrive_Path_Server = ""
            Common_Procedures.settings.SoftWare_UserType = ""
            Common_Procedures.settings.Sdd = #1/1/2000#
            Common_Procedures.settings.ExeFile_DateTime = #1/1/2000#

            da1 = New SqlClient.SqlDataAdapter("select * from Settings_Head", Cn1)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                If IsDBNull(dt1.Rows(0).Item("SoftWare_UserType").ToString) = False Then
                    Common_Procedures.settings.SoftWare_UserType = dt1.Rows(0).Item("SoftWare_UserType").ToString()
                End If
                If IsDBNull(dt1.Rows(0).Item("Sdd").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Sdd").ToString) <> "" Then
                        If IsDate(dt1.Rows(0).Item("Sdd").ToString) = True Then
                            Common_Procedures.settings.Sdd = dt1.Rows(0).Item("sdd")
                        End If
                    End If
                End If
                If IsDBNull(dt1.Rows(0).Item("ExeFile_DateTime").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("ExeFile_DateTime").ToString) <> "" Then
                        If IsDate(dt1.Rows(0).Item("ExeFile_DateTime").ToString) = True Then
                            Common_Procedures.settings.ExeFile_DateTime = dt1.Rows(0).Item("ExeFile_DateTime")
                        End If
                    End If
                End If
                If IsDBNull(dt1.Rows(0).Item("Cc_No").ToString) = False Then
                    Common_Procedures.settings.CustomerDBCode = dt1.Rows(0).Item("Cc_No").ToString()
                    Common_Procedures.settings.CustomerCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.settings.CustomerDBCode), 4)
                End If
                If IsDBNull(dt1.Rows(0).Item("S_Name").ToString) = False Then
                    Common_Procedures.settings.SoftWareName = dt1.Rows(0).Item("S_Name").ToString()
                End If
                If IsDBNull(dt1.Rows(0).Item("Autobackup_PenDrive_Path_Server").ToString) = False Then
                    Common_Procedures.settings.Autobackup_PenDrive_Path_Server = dt1.Rows(0).Item("Autobackup_PenDrive_Path_Server").ToString()
                End If
            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR IN GETTING SETTINGS VALUES...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    'Private Sub get_UserName_Password(ByVal cn1 As SqlClient.SqlConnection, ByVal SoftWare_UserType As String)

    '    Common_Procedures.User.IdNo = 0
    '    Common_Procedures.User.Name = ""
    '    Common_Procedures.User.Type = ""

    '    Login.ShowDialog()

    '    If Val(Common_Procedures.User.IdNo) <> 0 Then
    '        get_User_AccessRights(cn1)

    '    Else
    '        MessageBox.Show("Invalid Login", "LOGIN FAILED...", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        Me.Close()
    '        Application.Exit()
    '        End

    '    End If

    'End Sub

    'Private Sub get_User_AccessRights(ByVal Cn1 As SqlClient.SqlConnection)
    '    Dim Da As New SqlClient.SqlDataAdapter
    '    Dim Dt1 As New DataTable

    '    Common_Procedures.UR.Formula_Weaver_Yarn_Consumption = ""
    '    Common_Procedures.UR.Formula_Weaver_Coolie = ""

    '    Common_Procedures.UR.Ledger_Verifition = ""
    '    'Common_Procedures.UR.Finished_Product_Verifition = ""
    '    'Common_Procedures.UR.Grey_Fabric_Verifition = ""

    '    Common_Procedures.UR.Weaver_Creation = ""
    '    '  Common_Procedures.UR.Jobworker_Creation = ""
    '    Common_Procedures.UR.Rewinding_Creation = ""
    '    Common_Procedures.UR.Sizing_Creation = ""
    '    Common_Procedures.UR.Count_Creation = ""
    '    Common_Procedures.UR.Mill_Creation = ""
    '    Common_Procedures.UR.Endscount_Creation = ""
    '    Common_Procedures.UR.cloth_Creation = ""
    '    Common_Procedures.UR.Loom_Creation = ""
    '    Common_Procedures.UR.Masters_AccountsGroup_Creations = ""
    '    Common_Procedures.UR.Masters_Clothset_Creation = ""
    '    Common_Procedures.UR.Masters_LoomType_Creation = ""
    '    Common_Procedures.UR.Masters_Beam_Width_Creation = ""
    '    Common_Procedures.UR.Masters_Vendor_Creation = ""
    '    Common_Procedures.UR.Master_Tax_Creation = ""
    '    Common_Procedures.UR.Masters_BorderSize_Creation = ""
    '    Common_Procedures.UR.Masters_Employee_Simple_Creation = ""
    '    Common_Procedures.UR.Masters_Color_Creation = ""
    '    Common_Procedures.UR.Masters_Market_Status_Creation = ""
    '    Common_Procedures.UR.Masters_Vareity_Creation = ""
    '    Common_Procedures.UR.Master_Stores_Master_Creation = ""
    '    Common_Procedures.UR.Opening_Closing_Stock_Value = ""

    '    Common_Procedures.UR.Textile_OpeningStock = ""
    '    Common_Procedures.UR.Jobwork_OpeningStock = ""
    '    Common_Procedures.UR.Piece_OpeningStock = ""
    '    Common_Procedures.UR.Bale_OpeningStock = ""
    '    Common_Procedures.UR.Yarn_Purchase_Entry = ""
    '    Common_Procedures.UR.Cloth_Purchase_Entry = ""
    '    Common_Procedures.UR.Cloth_Receipt_Entry = ""
    '    Common_Procedures.UR.EmptyBeam_Purchase_Entry = ""
    '    Common_Procedures.UR.Sizing_Yarn_Delivery_Entry = ""
    '    Common_Procedures.UR.Sizing_Yarn_Receipt_Entry = ""
    '    Common_Procedures.UR.Sizing_Specification_Entry = ""
    '    Common_Procedures.UR.Rewinding_Delivery_Entry = ""
    '    Common_Procedures.UR.Rewinding_Receipt_Entry = ""
    '    Common_Procedures.UR.Weaver_Pavu_Delivery_Entry = ""
    '    Common_Procedures.UR.Weaver_Yarn_Delivery_Entry = ""
    '    Common_Procedures.UR.Weaver_Pavu_Rceipt_Entry = ""
    '    Common_Procedures.UR.Weaver_Yarn_Rceipt_Entry = ""
    '    Common_Procedures.UR.Weaver_Piece_Checking_Entry = ""
    '    Common_Procedures.UR.Weaver_Wages_Entry = ""
    '    Common_Procedures.UR.Weaver_Cloth_Rceipt_Entry = ""
    '    Common_Procedures.UR.Inhouse_Beam_Close_Entry = ""
    '    Common_Procedures.UR.Inhouse_Beam_Knotting_Entry = ""
    '    Common_Procedures.UR.Inhouse_Beam_Runout_Entry = ""
    '    Common_Procedures.UR.Inhouse_Doffing_Entry = ""
    '    Common_Procedures.UR.Inhouse_Piece_Checking_Entry = ""
    '    Common_Procedures.UR.Weaver_Cloth_Rceipt_Entry = ""
    '    Common_Procedures.UR.PavuYarn_Delivery_Entry = ""
    '    Common_Procedures.UR.PavuYarn_Receipt_Entry = ""
    '    Common_Procedures.UR.Empty_BeamBagCone_Delivery_Entry = ""
    '    Common_Procedures.UR.Empty_BeamBagCone_Receipt_Entry = ""
    '    Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry = ""
    '    Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry_No_Invoice = ""
    '    Common_Procedures.UR.ClothSales_Cloth_Invoice_Entry = ""
    '    Common_Procedures.UR.ClothSales_Delivery_Return_Entry = ""
    '    Common_Procedures.UR.ClothSales_Order_Entry = ""
    '    Common_Procedures.UR.ClothSales_Sales_Return_Entry = ""
    '    Common_Procedures.UR.Yarn_Sales_Entry = ""
    '    Common_Procedures.UR.Weaver_Bobin_Delivery_Entry = ""
    '    Common_Procedures.UR.EmptyBeam_Sales_Entry = ""
    '    Common_Procedures.UR.Yarn_Transfer_Entry = ""
    '    Common_Procedures.UR.Pavu_Transfer_Entry = ""
    '    Common_Procedures.UR.Cloth_Transfer_Entry = ""
    '    Common_Procedures.UR.Yarn_Excess_Short_Entry = ""
    '    Common_Procedures.UR.Pavu_Excess_Short_Entry = ""
    '    Common_Procedures.UR.Cloth_Excess_Short_Entry = ""
    '    Common_Procedures.UR.Report_Register = ""
    '    Common_Procedures.UR.Report_Rewinding_Stock = ""
    '    Common_Procedures.UR.Report_Sizing_Stock = ""
    '    Common_Procedures.UR.Report_Weaver_Stock = ""
    '    Common_Procedures.UR.Report_Godown_Stock = ""
    '    Common_Procedures.UR.Report_Cloth_Order_Pending = ""
    '    Common_Procedures.UR.Report_Cloth_Delivery_Pending = ""
    '    Common_Procedures.UR.Report_Annexure = ""
    '    Common_Procedures.UR.Report_TDS = ""

    '    Common_Procedures.UR.JobWork_Order_Entry = ""
    '    Common_Procedures.UR.JobWork_PavuYarn_Receipt = ""
    '    Common_Procedures.UR.JobWork_Production_Entry = ""
    '    Common_Procedures.UR.JobWork_Inspection_Entry = ""
    '    Common_Procedures.UR.JobWork_Conversion_Bill = ""
    '    Common_Procedures.UR.JobWork_PavuYarn_Return = ""
    '    Common_Procedures.UR.JobWork_Delivery_Entry = ""
    '    Common_Procedures.UR.JobWork_EmptyBeam_Return = ""

    '    Common_Procedures.UR.Entry_Processing_Job_Order = ""
    '    Common_Procedures.UR.Entry_Fabric_DeliveryTo_Processing = ""
    '    Common_Procedures.UR.Entry_ProcessedFabric_ReceiptFrom_Processing = ""
    '    Common_Procedures.UR.Entry_Processed_Fabric_Inspection = ""
    '    Common_Procedures.UR.Entry_Processing_Bill_Making = ""
    '    Common_Procedures.UR.Entry_Processing_Return = ""
    '    Common_Procedures.UR.Entry_Processing_Fabric_Invoice = ""
    '    Common_Procedures.UR.Entry_Fabric_DeliveryTo_Sewing = ""
    '    Common_Procedures.UR.Entry_FinishedProduct_ReceiptFrom_Sewing = ""



    '    Common_Procedures.UR.Master_Stores_Machine_Creation = ""
    '    Common_Procedures.UR.Master_Stores_Brand_Creation = ""
    '    Common_Procedures.UR.Master_Stores_Item_Creation = ""
    '    Common_Procedures.UR.Master_Stores_Unit_Creation = ""
    '    Common_Procedures.UR.Master_Stores_ReedCount_Creation = ""
    '    Common_Procedures.UR.Master_Stores_ReedWidth_Creation = ""

    '    Common_Procedures.UR.Master_Opening_Stock = ""

    '    Common_Procedures.UR.Entry_Stores_Purchase_Order = ""
    '    Common_Procedures.UR.Entry_Stores_Purchase_Inward = ""
    '    Common_Procedures.UR.Entry_Stores_Purchase_Return = ""
    '    Common_Procedures.UR.Entry_Stores_Item_Issue = ""
    '    Common_Procedures.UR.Entry_Stores_Item_Return = ""
    '    Common_Procedures.UR.Entry_Stores_Item_Delivery = ""
    '    Common_Procedures.UR.Entry_Stores_Item_Receipt = ""
    '    Common_Procedures.UR.Entry_Stores_Service_Item_Delivery = ""
    '    Common_Procedures.UR.Entry_Stores_Service_Item_Receipt = ""
    '    Common_Procedures.UR.Entry_Stores_GatePass = ""
    '    Common_Procedures.UR.Entry_Stores_Item_Excess_Short = ""
    '    Common_Procedures.UR.Entry_Stores_Item_Dispose = ""
    '    Common_Procedures.UR.Entry_Stores_Oil_Service = ""


    '    Common_Procedures.UR.Report_JobWork_PavuYarn_Receipt = ""
    '    Common_Procedures.UR.Report_JobWork_PavuYarn_Return = ""
    '    Common_Procedures.UR.Report_JobWork_production = ""
    '    Common_Procedures.UR.Report_JobWork_Piece_Inspection = ""
    '    Common_Procedures.UR.Report_JobWork_Piece_Delivery = ""
    '    Common_Procedures.UR.Report_JobWork_Conversion_Bill = ""
    '    Common_Procedures.UR.Report_JobWork_Piece_Inspection_Pending = ""
    '    Common_Procedures.UR.Report_JobWork_Piece_Delivery_pending = ""
    '    Common_Procedures.UR.Report_JobWork_Conversion_Bill_Pending = ""

    '    Common_Procedures.UR.Report_Jobwork_Yarn_Stock = ""
    '    Common_Procedures.UR.Report_Jobwork_Pavu_Stock = ""
    '    Common_Procedures.UR.Report_Jobwork_EmptyBag_Stock = ""
    '    Common_Procedures.UR.Report_Jobwork_EmptyBeam_Stock = ""
    '    Common_Procedures.UR.Report_Jobwork_EmptyCone_Stock = ""

    '    Common_Procedures.UR.Report_Jobwork_All_Stock_Ledger = ""
    '    Common_Procedures.UR.Report_Jobwork_All_Stock_Summary = ""

    '    Common_Procedures.UR.Ledger_Creation = ""
    '    Common_Procedures.UR.Agent_Creation = ""
    '    Common_Procedures.UR.Transport_Creation = ""
    '    Common_Procedures.UR.Area_Creation = ""
    '    Common_Procedures.UR.GreyItem_Creation = ""
    '    Common_Procedures.UR.FinishedProduct_Creation = ""
    '    Common_Procedures.UR.ItemGroup_Creation = ""
    '    Common_Procedures.UR.Unit_Creation = ""
    '    Common_Procedures.UR.Product_Sales_Name_Creation = ""
    '    Common_Procedures.UR.Process_Creation = ""
    '    Common_Procedures.UR.Colour_Creation = ""
    '    Common_Procedures.UR.LotNo_Creation = ""
    '    Common_Procedures.UR.RackNo_Creation = ""
    '    Common_Procedures.UR.Ledger_Opening_Balance = ""
    '    Common_Procedures.UR.Godown_OpeningStock = ""
    '    Common_Procedures.UR.Processing_Delivery_Opening = ""
    '    Common_Procedures.UR.FP_Purchase_Entry = ""
    '    Common_Procedures.UR.FP_PurchaseReturn_Entry = ""
    '    Common_Procedures.UR.Processing_Delivery_Entry = ""
    '    Common_Procedures.UR.Processing_Receipt_Entry = ""
    '    Common_Procedures.UR.Processing_Return_Entry = ""
    '    Common_Procedures.UR.Processing_BillMaking_Entry = ""
    '    Common_Procedures.UR.Delivery_Entry_Floor_To_Rack = ""
    '    Common_Procedures.UR.Return_Entry_Rack_To_Floor = ""
    '    Common_Procedures.UR.Set_Entry = ""
    '    Common_Procedures.UR.UnSet_Entry = ""
    '    Common_Procedures.UR.Item_Transfer_Entry = ""
    '    Common_Procedures.UR.PackinSlip_Entry = ""
    '    Common_Procedures.UR.Invoice_Entry = ""
    '    Common_Procedures.UR.Order_Entry = ""
    '    Common_Procedures.UR.Cash_Sales_Entry = ""
    '    Common_Procedures.UR.Sales_Return_Entry = ""
    '    Common_Procedures.UR.Party_Receipt_Entry_Cheque_Cash = ""
    '    Common_Procedures.UR.Cheque_Return_Entry = ""
    '    Common_Procedures.UR.Voucher_Entry = ""
    '    Common_Procedures.UR.Accounts_Ledger_Report = ""
    '    Common_Procedures.UR.Accounts_GroupLedger_Report = ""
    '    Common_Procedures.UR.Accounts_DayBook = ""
    '    Common_Procedures.UR.Accounts_AllLedger = ""
    '    Common_Procedures.UR.Accounts_TB = ""
    '    Common_Procedures.UR.Accounts_Profit_Loss = ""
    '    Common_Procedures.UR.Accounts_BalanceSheet = ""
    '    Common_Procedures.UR.Accounts_CustomerBills = ""
    '    Common_Procedures.UR.Accounts_AgentBills = ""
    '    Common_Procedures.UR.Accounts_AgentCommission = ""
    '    Common_Procedures.UR.Report_Purchase_Register = ""
    '    Common_Procedures.UR.Report_PurchaseReturn_Register = ""
    '    Common_Procedures.UR.Report_Delivery_Register_Floor_To_Rack = ""
    '    Common_Procedures.UR.Report_Return_Register_Rack_To_Floor = ""
    '    Common_Procedures.UR.Report_Processing_Delivery_Register = ""
    '    Common_Procedures.UR.Report_Processing_Receipt_Register = ""
    '    Common_Procedures.UR.Report_Processing_Return_Register = ""
    '    Common_Procedures.UR.Report_Processing_Billmaking_Register = ""
    '    Common_Procedures.UR.Report_Processing_Receipt_Pending_Register = ""
    '    Common_Procedures.UR.Report_Processing_Billmaking_Pending_Register = ""
    '    Common_Procedures.UR.Report_SetEntry_Register = ""
    '    Common_Procedures.UR.Report_UnSetEntry_Register = ""
    '    Common_Procedures.UR.Report_ItemTransfer_Register = ""
    '    Common_Procedures.UR.Report_PackingSlip_Register = ""
    '    Common_Procedures.UR.Report_Invoice_Register = ""
    '    Common_Procedures.UR.Report_CashSales_Register = ""
    '    Common_Procedures.UR.Report_SalesReturn_Register = ""
    '    Common_Procedures.UR.Report_PartyReceipt_Register_Cash_Cheque = ""
    '    Common_Procedures.UR.Report_ItemList = ""
    '    Common_Procedures.UR.Report_FP_Registers = ""
    '    Common_Procedures.UR.Report_PartyList = ""
    '    Common_Procedures.UR.Report_Stock_Register = ""
    '    Common_Procedures.UR.Report_Stock_Summary = ""
    '    Common_Procedures.UR.Report_Stock_Value = ""
    '    Common_Procedures.UR.Report_Minimum_Stock_Value = ""
    '    Common_Procedures.UR.Report_GreyFabric_Stock = ""
    '    Common_Procedures.UR.Report_FinishedProduct_Stock = ""
    '    Common_Procedures.UR.Bobin_Sales_Delivery_Entry = ""
    '    Common_Procedures.UR.Jari_Sales_Delivery_Entry = ""
    '    Common_Procedures.UR.Bobin_Purchase_Entry = ""
    '    Common_Procedures.UR.fabric_physical_stock_Entry = ""
    '    If Val(Common_Procedures.User.IdNo) = 1 Then

    '        Common_Procedures.UR.Formula_Weaver_Yarn_Consumption = "~L~"
    '        Common_Procedures.UR.Formula_Weaver_Coolie = "~L~"


    '        Common_Procedures.UR.Sizing_Creation = "~L~"
    '        Common_Procedures.UR.Weaver_Creation = "~L~"
    '        Common_Procedures.UR.Jobworker_Creation = "~L~"
    '        Common_Procedures.UR.Rewinding_Creation = "~L~"
    '        Common_Procedures.UR.Count_Creation = "~L~"
    '        Common_Procedures.UR.Mill_Creation = "~L~"
    '        Common_Procedures.UR.Endscount_Creation = "~L~"
    '        Common_Procedures.UR.cloth_Creation = "~L~"
    '        Common_Procedures.UR.Loom_Creation = "~L~"
    '        Common_Procedures.UR.Masters_AccountsGroup_Creations = "~L~"
    '        Common_Procedures.UR.Masters_Clothset_Creation = "~L~"
    '        Common_Procedures.UR.Masters_Clothset_Creation = "~L~"
    '        Common_Procedures.UR.Masters_Beam_Width_Creation = "~L~"
    '        Common_Procedures.UR.Masters_Vendor_Creation = "~L~"
    '        Common_Procedures.UR.Masters_Employee_Simple_Creation = "~L~"
    '        Common_Procedures.UR.Masters_Color_Creation = "~L~"
    '        Common_Procedures.UR.Masters_Market_Status_Creation = "~L~"
    '        Common_Procedures.UR.Masters_Vareity_Creation = "~L~"
    '        Common_Procedures.UR.Master_Stores_Master_Creation = "~L~"
    '        Common_Procedures.UR.Opening_Closing_Stock_Value = "~L~"

    '        Common_Procedures.UR.Master_Tax_Creation = "~L~"
    '        Common_Procedures.UR.Masters_BorderSize_Creation = "~L~"

    '        Common_Procedures.UR.Textile_OpeningStock = "~L~"
    '        Common_Procedures.UR.Jobwork_OpeningStock = "~L~"
    '        Common_Procedures.UR.Piece_OpeningStock = "~L~"
    '        Common_Procedures.UR.Bale_OpeningStock = "~L~"
    '        Common_Procedures.UR.Yarn_Purchase_Entry = "~L~"
    '        Common_Procedures.UR.Cloth_Purchase_Entry = "~L~"
    '        Common_Procedures.UR.Cloth_Receipt_Entry = "~L~"
    '        Common_Procedures.UR.EmptyBeam_Purchase_Entry = "~L~"

    '        Common_Procedures.UR.Sizing_Yarn_Delivery_Entry = "~L~"
    '        Common_Procedures.UR.Sizing_Yarn_Receipt_Entry = "~L~"
    '        Common_Procedures.UR.Sizing_Specification_Entry = "~L~"
    '        Common_Procedures.UR.Rewinding_Delivery_Entry = "~L~"
    '        Common_Procedures.UR.Rewinding_Receipt_Entry = "~L~"
    '        Common_Procedures.UR.Weaver_Pavu_Delivery_Entry = "~L~"
    '        Common_Procedures.UR.Weaver_Yarn_Delivery_Entry = "~L~"
    '        Common_Procedures.UR.Weaver_Pavu_Rceipt_Entry = "~L~"
    '        Common_Procedures.UR.Weaver_Yarn_Rceipt_Entry = "~L~"
    '        Common_Procedures.UR.Weaver_Piece_Checking_Entry = "~L~"
    '        Common_Procedures.UR.Weaver_Wages_Entry = "~L~"
    '        Common_Procedures.UR.Weaver_Cloth_Rceipt_Entry = "~L~"
    '        Common_Procedures.UR.Inhouse_Beam_Close_Entry = "~L~"
    '        Common_Procedures.UR.Inhouse_Beam_Knotting_Entry = "~L~"
    '        Common_Procedures.UR.Inhouse_Beam_Runout_Entry = "~L~"
    '        Common_Procedures.UR.Inhouse_Doffing_Entry = "~L~"
    '        Common_Procedures.UR.Inhouse_Piece_Checking_Entry = "~L~"
    '        Common_Procedures.UR.Weaver_Cloth_Rceipt_Entry = "~L~"
    '        Common_Procedures.UR.PavuYarn_Delivery_Entry = "~L~"
    '        Common_Procedures.UR.PavuYarn_Receipt_Entry = "~L~"
    '        Common_Procedures.UR.Empty_BeamBagCone_Delivery_Entry = "~L~"
    '        Common_Procedures.UR.Empty_BeamBagCone_Receipt_Entry = "~L~"
    '        Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry = "~L~"
    '        Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry_No_Invoice = "~L~"
    '        Common_Procedures.UR.ClothSales_Cloth_Invoice_Entry = "~L~"
    '        Common_Procedures.UR.ClothSales_Delivery_Return_Entry = "~L~"
    '        Common_Procedures.UR.ClothSales_Order_Entry = "~L~"
    '        Common_Procedures.UR.ClothSales_Sales_Return_Entry = "~L~"
    '        Common_Procedures.UR.Yarn_Sales_Entry = "~L~"
    '        Common_Procedures.UR.Weaver_Bobin_Delivery_Entry = "~L~"
    '        Common_Procedures.UR.EmptyBeam_Sales_Entry = "~L~"
    '        Common_Procedures.UR.Yarn_Transfer_Entry = "~L~"
    '        Common_Procedures.UR.Pavu_Transfer_Entry = "~L~"
    '        Common_Procedures.UR.Cloth_Transfer_Entry = "~L~"
    '        Common_Procedures.UR.Yarn_Excess_Short_Entry = "~L~"
    '        Common_Procedures.UR.Pavu_Excess_Short_Entry = "~L~"
    '        Common_Procedures.UR.Cloth_Excess_Short_Entry = "~L~"

    '        Common_Procedures.UR.Report_Register = "~L~"
    '        Common_Procedures.UR.Report_Rewinding_Stock = "~L~"
    '        Common_Procedures.UR.Report_Sizing_Stock = "~L~"
    '        Common_Procedures.UR.Report_Weaver_Stock = "~L~"
    '        Common_Procedures.UR.Report_Godown_Stock = "~L~"
    '        Common_Procedures.UR.Report_Cloth_Order_Pending = "~L~"
    '        Common_Procedures.UR.Report_Cloth_Delivery_Pending = "~L~"
    '        Common_Procedures.UR.Report_Annexure = "~L~"
    '        Common_Procedures.UR.Report_TDS = "~L~"


    '        Common_Procedures.UR.JobWork_Order_Entry = "~L~"
    '        Common_Procedures.UR.JobWork_PavuYarn_Receipt = "~L~"
    '        Common_Procedures.UR.JobWork_Production_Entry = "~L~"
    '        Common_Procedures.UR.JobWork_Inspection_Entry = "~L~"
    '        Common_Procedures.UR.JobWork_Conversion_Bill = "~L~"
    '        Common_Procedures.UR.JobWork_PavuYarn_Return = "~L~"
    '        Common_Procedures.UR.JobWork_Delivery_Entry = "~L~"
    '        Common_Procedures.UR.JobWork_EmptyBeam_Return = "~L~"

    '        Common_Procedures.UR.Entry_Processing_Job_Order = "~L~"
    '        Common_Procedures.UR.Entry_Fabric_DeliveryTo_Processing = "~L~"
    '        Common_Procedures.UR.Entry_ProcessedFabric_ReceiptFrom_Processing = "~L~"
    '        Common_Procedures.UR.Entry_Processed_Fabric_Inspection = "~L~"
    '        Common_Procedures.UR.Entry_Processing_Bill_Making = "~L~"
    '        Common_Procedures.UR.Entry_Processing_Return = "~L~"
    '        Common_Procedures.UR.Entry_Processing_Fabric_Invoice = "~L~"
    '        Common_Procedures.UR.Entry_Fabric_DeliveryTo_Sewing = "~L~"
    '        Common_Procedures.UR.Entry_FinishedProduct_ReceiptFrom_Sewing = "~L~"


    '        Common_Procedures.UR.Master_Stores_Department_Creation = "~L~"
    '        Common_Procedures.UR.Master_Stores_Machine_Creation = "~L~"
    '        Common_Procedures.UR.Master_Stores_Brand_Creation = "~L~"
    '        Common_Procedures.UR.Master_Stores_Item_Creation = "~L~"
    '        Common_Procedures.UR.Master_Stores_Unit_Creation = "~L~"
    '        Common_Procedures.UR.Master_Stores_ReedCount_Creation = "~L~"
    '        Common_Procedures.UR.Master_Stores_ReedWidth_Creation = "~L~"

    '        Common_Procedures.UR.Master_Opening_Stock = "~L~"

    '        Common_Procedures.UR.Entry_Stores_Purchase_Order = "~L~"
    '        Common_Procedures.UR.Entry_Stores_Purchase_Inward = "~L~"
    '        Common_Procedures.UR.Entry_Stores_Purchase_Return = "~L~"
    '        Common_Procedures.UR.Entry_Stores_Item_Issue = "~L~"
    '        Common_Procedures.UR.Entry_Stores_Item_Return = "~L~"
    '        Common_Procedures.UR.Entry_Stores_Item_Delivery = "~L~"
    '        Common_Procedures.UR.Entry_Stores_Item_Receipt = "~L~"
    '        Common_Procedures.UR.Entry_Stores_Service_Item_Delivery = "~L~"
    '        Common_Procedures.UR.Entry_Stores_Service_Item_Receipt = "~L~"
    '        Common_Procedures.UR.Entry_Stores_GatePass = "~L~"
    '        Common_Procedures.UR.Entry_Stores_Item_Excess_Short = "~L~"
    '        Common_Procedures.UR.Entry_Stores_Item_Dispose = "~L~"
    '        Common_Procedures.UR.Entry_Stores_Oil_Service = "~L~"

    '        Common_Procedures.UR.Report_JobWork_PavuYarn_Receipt = "~L~"
    '        Common_Procedures.UR.Report_JobWork_PavuYarn_Return = "~L~"
    '        Common_Procedures.UR.Report_JobWork_production = "~L~"
    '        Common_Procedures.UR.Report_JobWork_Piece_Inspection = "~L~"
    '        Common_Procedures.UR.Report_JobWork_Piece_Delivery = "~L~"
    '        Common_Procedures.UR.Report_JobWork_Conversion_Bill = "~L~"
    '        Common_Procedures.UR.Report_JobWork_Piece_Inspection_Pending = "~L~"
    '        Common_Procedures.UR.Report_JobWork_Piece_Delivery_pending = "~L~"
    '        Common_Procedures.UR.Report_JobWork_Conversion_Bill_Pending = "~L~"

    '        Common_Procedures.UR.Report_Jobwork_Yarn_Stock = "~L~"
    '        Common_Procedures.UR.Report_Jobwork_Pavu_Stock = "~L~"
    '        Common_Procedures.UR.Report_Jobwork_EmptyBag_Stock = "~L~"
    '        Common_Procedures.UR.Report_Jobwork_EmptyBeam_Stock = "~L~"
    '        Common_Procedures.UR.Report_Jobwork_EmptyCone_Stock = "~L~"

    '        Common_Procedures.UR.Report_Jobwork_All_Stock_Ledger = "~L~"
    '        Common_Procedures.UR.Report_Jobwork_All_Stock_Summary = "~L~"

    '        Common_Procedures.UR.Ledger_Creation = "~L~"
    '        Common_Procedures.UR.Agent_Creation = "~L~"
    '        Common_Procedures.UR.Transport_Creation = "~L~"
    '        Common_Procedures.UR.Area_Creation = "~L~"
    '        Common_Procedures.UR.GreyItem_Creation = "~L~"
    '        Common_Procedures.UR.FinishedProduct_Creation = "~L~"
    '        Common_Procedures.UR.ItemGroup_Creation = "~L~"
    '        Common_Procedures.UR.Unit_Creation = "~L~"
    '        Common_Procedures.UR.Product_Sales_Name_Creation = "~L~"
    '        Common_Procedures.UR.Process_Creation = "~L~"
    '        Common_Procedures.UR.Colour_Creation = "~L~"
    '        Common_Procedures.UR.LotNo_Creation = "~L~"
    '        Common_Procedures.UR.RackNo_Creation = "~L~"
    '        Common_Procedures.UR.Ledger_Opening_Balance = "~L~"
    '        Common_Procedures.UR.Godown_OpeningStock = "~L~"
    '        Common_Procedures.UR.Processing_Delivery_Opening = "~L~"
    '        Common_Procedures.UR.FP_Purchase_Entry = "~L~"
    '        Common_Procedures.UR.FP_PurchaseReturn_Entry = "~L~"
    '        Common_Procedures.UR.Processing_Delivery_Entry = "~L~"
    '        Common_Procedures.UR.Processing_Receipt_Entry = "~L~"
    '        Common_Procedures.UR.Processing_Return_Entry = "~L~"
    '        Common_Procedures.UR.Processing_BillMaking_Entry = "~L~"
    '        Common_Procedures.UR.Delivery_Entry_Floor_To_Rack = "~L~"
    '        Common_Procedures.UR.Return_Entry_Rack_To_Floor = "~L~"
    '        Common_Procedures.UR.Set_Entry = "~L~"
    '        Common_Procedures.UR.UnSet_Entry = "~L~"
    '        Common_Procedures.UR.Item_Transfer_Entry = "~L~"
    '        Common_Procedures.UR.PackinSlip_Entry = "~L~"
    '        Common_Procedures.UR.Invoice_Entry = "~L~"
    '        Common_Procedures.UR.Order_Entry = "~L~"
    '        Common_Procedures.UR.Cash_Sales_Entry = "~L~"
    '        Common_Procedures.UR.Sales_Return_Entry = "~L~"
    '        Common_Procedures.UR.Party_Receipt_Entry_Cheque_Cash = "~L~"
    '        Common_Procedures.UR.Cheque_Return_Entry = "~L~"
    '        Common_Procedures.UR.Voucher_Entry = "~L~"
    '        Common_Procedures.UR.Accounts_Ledger_Report = "~L~"
    '        Common_Procedures.UR.Accounts_GroupLedger_Report = "~L~"
    '        Common_Procedures.UR.Accounts_DayBook = "~L~"
    '        Common_Procedures.UR.Accounts_AllLedger = "~L~"
    '        Common_Procedures.UR.Accounts_TB = "~L~"
    '        Common_Procedures.UR.Accounts_Profit_Loss = "~L~"
    '        Common_Procedures.UR.Accounts_BalanceSheet = "~L~"
    '        Common_Procedures.UR.Accounts_CustomerBills = "~L~"
    '        Common_Procedures.UR.Accounts_AgentBills = "~L~"
    '        Common_Procedures.UR.Accounts_AgentCommission = "~L~"
    '        Common_Procedures.UR.Report_Purchase_Register = "~L~"
    '        Common_Procedures.UR.Report_PurchaseReturn_Register = "~L~"
    '        Common_Procedures.UR.Report_Delivery_Register_Floor_To_Rack = "~L~"
    '        Common_Procedures.UR.Report_Return_Register_Rack_To_Floor = "~L~"
    '        Common_Procedures.UR.Report_Processing_Delivery_Register = "~L~"
    '        Common_Procedures.UR.Report_Processing_Receipt_Register = "~L~"
    '        Common_Procedures.UR.Report_Processing_Return_Register = "~L~"
    '        Common_Procedures.UR.Report_Processing_Billmaking_Register = "~L~"
    '        Common_Procedures.UR.Report_Processing_Receipt_Pending_Register = "~L~"
    '        Common_Procedures.UR.Report_Processing_Billmaking_Pending_Register = "~L~"
    '        Common_Procedures.UR.Report_SetEntry_Register = "~L~"
    '        Common_Procedures.UR.Report_UnSetEntry_Register = "~L~"
    '        Common_Procedures.UR.Report_ItemTransfer_Register = "~L~"
    '        Common_Procedures.UR.Report_PackingSlip_Register = "~L~"
    '        Common_Procedures.UR.Report_Invoice_Register = "~L~"
    '        Common_Procedures.UR.Report_CashSales_Register = "~L~"
    '        Common_Procedures.UR.Report_SalesReturn_Register = "~L~"
    '        Common_Procedures.UR.Report_PartyReceipt_Register_Cash_Cheque = "~L~"
    '        Common_Procedures.UR.Report_ItemList = "~L~"
    '        Common_Procedures.UR.Report_FP_Registers = "~L~"
    '        Common_Procedures.UR.Report_PartyList = "~L~"
    '        Common_Procedures.UR.Report_Stock_Register = "~L~"
    '        Common_Procedures.UR.Report_Stock_Summary = "~L~"
    '        Common_Procedures.UR.Report_Stock_Value = "~L~"
    '        Common_Procedures.UR.Report_Minimum_Stock_Value = "~L~"
    '        Common_Procedures.UR.Report_GreyFabric_Stock = "~L~"
    '        Common_Procedures.UR.Report_FinishedProduct_Stock = "~L~"
    '        Common_Procedures.UR.Bobin_Sales_Delivery_Entry = "~L~"
    '        Common_Procedures.UR.Jari_Sales_Delivery_Entry = "~L~"
    '        Common_Procedures.UR.Bobin_Purchase_Entry = "~L~"
    '        Common_Procedures.UR.fabric_physical_stock_Entry = "~L~"
    '    Else

    '        Da = New SqlClient.SqlDataAdapter("select * from User_Access_Rights where user_idno = " & Str(Val(Common_Procedures.User.IdNo)), Cn1)
    '        Dt1 = New DataTable
    '        Da.Fill(Dt1)

    '        If Dt1.Rows.Count > 0 Then

    '            For i = 0 To Dt1.Rows.Count - 1

    '                Select Case Trim(UCase(Dt1.Rows(i).Item("Entry_Code").ToString))
    '                    Case "FORMULA_WEAVER_YARN_CONSUMPTION"
    '                        Common_Procedures.UR.Formula_Weaver_Yarn_Consumption = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "FORMULA_WEAVER_COOLIE"
    '                        Common_Procedures.UR.Formula_Weaver_Coolie = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_LEDGER_VERIFICATION"
    '                        Common_Procedures.UR.Ledger_Verifition = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_FINISHED_PRODUCT_VERIFICATION"
    '                        Common_Procedures.UR.Finished_Product_Verifition = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_GREY_FABRIC_VERIFICATION"
    '                        Common_Procedures.UR.Grey_Fabric_Verifition = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_SIZING_CREATION"
    '                        Common_Procedures.UR.Sizing_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_JOBWORKER_CREATION"
    '                        Common_Procedures.UR.Jobworker_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_WEAVER_CREATION"
    '                        Common_Procedures.UR.Weaver_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_REWINDING_CREATION"
    '                        Common_Procedures.UR.Rewinding_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_COUNT_CREATION"
    '                        Common_Procedures.UR.Count_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_MILL_CREATION"
    '                        Common_Procedures.UR.Mill_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_ENDSCOUNT_CREATION"
    '                        Common_Procedures.UR.Endscount_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_CLOTH_CREATION"
    '                        Common_Procedures.UR.cloth_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_CLOTHSET_CREATION"
    '                        Common_Procedures.UR.Masters_Clothset_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_LOOM_CREATION"
    '                        Common_Procedures.UR.Loom_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_LOOM_TYPE_CREATION"
    '                        Common_Procedures.UR.Masters_LoomType_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_ACCOUNTSGROUP_CREATION"
    '                        Common_Procedures.UR.Masters_AccountsGroup_Creations = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_TAX_CREATION"
    '                        Common_Procedures.UR.Master_Tax_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_BORDER_SIZE_CREATION"
    '                        Common_Procedures.UR.Masters_BorderSize_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_VENDOR_CREATION"
    '                        Common_Procedures.UR.Masters_Vendor_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_EMPLOYEE_SIMPLE_CREATION"
    '                        Common_Procedures.UR.Masters_Employee_Simple_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_COLOR_CREATION"
    '                        Common_Procedures.UR.Masters_Color_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_MARKET_STATUS_CREATION"
    '                        Common_Procedures.UR.Masters_Market_Status_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_VARIETY_CREATION"
    '                        Common_Procedures.UR.Masters_Vareity_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_STORES_MASTERS_CREATION"
    '                        Common_Procedures.UR.Master_Stores_Master_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_LEDGER_OPENING_STOCK"
    '                        Common_Procedures.UR.Textile_OpeningStock = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_LEDGER_JOBWORK_OPENING_STOCK"
    '                        Common_Procedures.UR.Jobwork_OpeningStock = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_PIECE_OPENING_STOCK"
    '                        Common_Procedures.UR.Piece_OpeningStock = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_BALE_OPENING_STOCK"
    '                        Common_Procedures.UR.Bale_OpeningStock = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_BEAM_WIDTH_CREATION"
    '                        Common_Procedures.UR.Masters_Beam_Width_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_YARN_PURCHASE"
    '                        Common_Procedures.UR.Yarn_Purchase_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_CLOTH_RECEIPT"
    '                        Common_Procedures.UR.Cloth_Receipt_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_CLOTH_PURCHASE"
    '                        Common_Procedures.UR.Cloth_Purchase_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_EMBTYBEAM_PURCHASE"
    '                        Common_Procedures.UR.EmptyBeam_Purchase_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_SIZING_YARN_DELIVERY"
    '                        Common_Procedures.UR.Sizing_Yarn_Delivery_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_SIZING_SPECIFICATION"
    '                        Common_Procedures.UR.Sizing_Specification_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_SIZING_YARN_RECEIPT"
    '                        Common_Procedures.UR.Sizing_Yarn_Receipt_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_REWINDING_DELIVERY"
    '                        Common_Procedures.UR.Rewinding_Delivery_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_REWINDING_RECEIPT"
    '                        Common_Procedures.UR.Rewinding_Receipt_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_WEAVER_YARN_DELIVERY"
    '                        Common_Procedures.UR.Weaver_Yarn_Delivery_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_WEAVER_YARN_RECEIPT"
    '                        Common_Procedures.UR.Weaver_Yarn_Rceipt_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_WEAVER_PAVU_DELIVERY"
    '                        Common_Procedures.UR.Weaver_Pavu_Delivery_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_WEAVER_PAVU_RECEIPT"
    '                        Common_Procedures.UR.Weaver_Pavu_Rceipt_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_WEAVER_CLOTH_RECEIPT"
    '                        Common_Procedures.UR.Weaver_Cloth_Rceipt_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_WEAVER_PIECE_CHECKING_RECEIPT"
    '                        Common_Procedures.UR.Weaver_Piece_Checking_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_WEAVER_WAGES"
    '                        Common_Procedures.UR.Weaver_Wages_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_INHOUSE_BEAM_KNOTTING"
    '                        Common_Procedures.UR.Inhouse_Beam_Knotting_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_INHOUSE_DOFFING"
    '                        Common_Procedures.UR.Inhouse_Doffing_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_INHOUSE_PIECE_CHECKING"
    '                        Common_Procedures.UR.Inhouse_Piece_Checking_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_INHOUSE_BEAM_RUNOUT"
    '                        Common_Procedures.UR.Inhouse_Beam_Runout_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_INHOUSE_BEAM_CLOSE"
    '                        Common_Procedures.UR.Inhouse_Beam_Close_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_PAVUYARN_DELIVERY"
    '                        Common_Procedures.UR.PavuYarn_Delivery_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_PAVUYARN_RECEIPT"
    '                        Common_Procedures.UR.PavuYarn_Receipt_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_EMBTY_BEAMBAGCONE_DELIVERY"
    '                        Common_Procedures.UR.Empty_BeamBagCone_Delivery_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_EMBTY_BEAMBAGCONE_RECEIPT"
    '                        Common_Procedures.UR.Empty_BeamBagCone_Receipt_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_CLOTHSALES_ORDER"
    '                        Common_Procedures.UR.ClothSales_Order_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_CLOTHSALES_CLOTH_DELIVERY"
    '                        Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_CLOTHSALES_CLOTH_DELIVERY_NO_INVOICE"
    '                        Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry_No_Invoice = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_CLOTHSALES_CLOTH_INVOICE"
    '                        Common_Procedures.UR.ClothSales_Cloth_Invoice_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_CLOTHSALES_CLOTHDELIVERY_RETURN"
    '                        Common_Procedures.UR.ClothSales_Delivery_Return_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_CLOTHSALES_RETURN"
    '                        Common_Procedures.UR.ClothSales_Sales_Return_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_YARN_SALES"
    '                        Common_Procedures.UR.Yarn_Sales_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_WEAVER_BOBIN_DELIVERY"
    '                        Common_Procedures.UR.Weaver_Bobin_Delivery_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_EMPTYBEAM_SALES"
    '                        Common_Procedures.UR.EmptyBeam_Sales_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_YARN_TRANSFER"
    '                        Common_Procedures.UR.Yarn_Transfer_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_PAVU_TRANSFER"
    '                        Common_Procedures.UR.Pavu_Transfer_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_CLOTH_TRANSFER"
    '                        Common_Procedures.UR.Cloth_Transfer_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_YARN_EXCESS_SHORT"
    '                        Common_Procedures.UR.Yarn_Excess_Short_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_PAVU_EXCESS_SHORT"
    '                        Common_Procedures.UR.Pavu_Excess_Short_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_CLOTH_EXCESS_SHORT"
    '                        Common_Procedures.UR.Cloth_Excess_Short_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_PROCESSING_JOB_ORDER"
    '                        Common_Procedures.UR.Entry_Processing_Job_Order = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_FABRIC_DELIVERYTO_PROCESSING"
    '                        Common_Procedures.UR.Entry_Fabric_DeliveryTo_Processing = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_PROCESSEDFABRIC_RECEIPTFROM_PROCESSING"
    '                        Common_Procedures.UR.Entry_ProcessedFabric_ReceiptFrom_Processing = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_PROCESSED_FABRIC_INSPECTION"
    '                        Common_Procedures.UR.Entry_Processed_Fabric_Inspection = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_PROCESSING_BILL_MAKING"

    '                        Common_Procedures.UR.Entry_Processing_Bill_Making = Dt1.Rows(i).Item("Access_Type").ToString


    '                    Case "ENTRY_PROCESSING_RETURN"
    '                        Common_Procedures.UR.Entry_Processing_Return = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_PROCESSING_FABRIC_INVOICE"

    '                        Common_Procedures.UR.Entry_Processing_Fabric_Invoice = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_FABRIC_DELIVERYTO_SEWING"
    '                        Common_Procedures.UR.Entry_Fabric_DeliveryTo_Sewing = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_FINISHEDPRODUCT_RECEIPTFROM_SEWING"
    '                        Common_Procedures.UR.Entry_FinishedProduct_ReceiptFrom_Sewing = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_REGISTER"
    '                        Common_Procedures.UR.Report_Register = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_SIZING_STOCK"
    '                        Common_Procedures.UR.Report_Sizing_Stock = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_REWINDING_STOCK"
    '                        Common_Procedures.UR.Report_Rewinding_Stock = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_WEAVER_STOCK"
    '                        Common_Procedures.UR.Report_Weaver_Stock = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_GODOWN_STOCK"
    '                        Common_Procedures.UR.Report_Godown_Stock = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_CLOTH_ORDER_PENDING"
    '                        Common_Procedures.UR.Report_Cloth_Order_Pending = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_CLOTH_DELIVERY_PENDING"
    '                        Common_Procedures.UR.Report_Cloth_Delivery_Pending = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_ANNEXURE_REPORT"
    '                        Common_Procedures.UR.Report_Annexure = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_TDS_REPORT"
    '                        Common_Procedures.UR.Report_TDS = Dt1.Rows(i).Item("Access_Type").ToString

    '                        'JOBWORK---------

    '                    Case "ENTRY_JOBWORK_ORDER"
    '                        Common_Procedures.UR.JobWork_Order_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_JOBWORK_PAVUYARN_RECEIPT"
    '                        Common_Procedures.UR.JobWork_PavuYarn_Receipt = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_JOBWORK_PRODUCTION"
    '                        Common_Procedures.UR.JobWork_Production_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_JOBWORK_DELIVERY"
    '                        Common_Procedures.UR.JobWork_Delivery_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_JOBWORK_INSPECTION"
    '                        Common_Procedures.UR.JobWork_Inspection_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_JOBWORK_CONVERSION_BILL"
    '                        Common_Procedures.UR.JobWork_Conversion_Bill = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_JOBWORK_PAVUYARN_RETURN"
    '                        Common_Procedures.UR.JobWork_PavuYarn_Return = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_JOBWORK_EMPTYBEAM_RETURN"
    '                        Common_Procedures.UR.JobWork_EmptyBeam_Return = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_JOBWORK_PAVUYARN_RECEIPT"
    '                        Common_Procedures.UR.Report_JobWork_PavuYarn_Receipt = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_JOBWORK_PAVUYARN_RETURN"
    '                        Common_Procedures.UR.Report_JobWork_PavuYarn_Return = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_JOBWORK_PRODUCTION"
    '                        Common_Procedures.UR.Report_JobWork_production = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_JOBWORK_PIECE_DELIVERY"
    '                        Common_Procedures.UR.Report_JobWork_Piece_Delivery = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_JOBWORK_PIECE_INSPECTION"
    '                        Common_Procedures.UR.Report_JobWork_Piece_Inspection = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_JOBWORK_CONVERSION_BILL"
    '                        Common_Procedures.UR.Report_JobWork_Conversion_Bill = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_JOBWORK_PIECE_DELIVERY_PENDING"
    '                        Common_Procedures.UR.Report_JobWork_Piece_Delivery_pending = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_JOBWORK_PIECE_INSPECTION_PENDING"
    '                        Common_Procedures.UR.Report_JobWork_Piece_Inspection_Pending = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_JOBWORK_CONVERSION_BILL_PENDING"
    '                        Common_Procedures.UR.Report_JobWork_Conversion_Bill_Pending = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_JOBWORK_YARN_STOCK_REPORT"
    '                        Common_Procedures.UR.Report_Jobwork_Yarn_Stock = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_JOBWORK_PAVU_STOCK_REPORT"
    '                        Common_Procedures.UR.Report_Jobwork_Pavu_Stock = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_JOBWORK_EMPTYBEAM_STOCK_REPORT"
    '                        Common_Procedures.UR.Report_Jobwork_EmptyBeam_Stock = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_JOBWORK_EMPTYBAG_STOCK_REPORT"
    '                        Common_Procedures.UR.Report_Jobwork_EmptyBag_Stock = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_JOBWORK_EMPTYCONE_STOCK_REPORT"
    '                        Common_Procedures.UR.Report_Jobwork_EmptyCone_Stock = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_JOBWORK_ALL_STOCK_LEDGER"
    '                        Common_Procedures.UR.Report_Jobwork_All_Stock_Ledger = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_JOBWORK_ALL_STOCK_SUMMARY"
    '                        Common_Procedures.UR.Report_Jobwork_All_Stock_Summary = Dt1.Rows(i).Item("Access_Type").ToString

    '                        'FP-------------
    '                    Case "MASTER_LEDGER_CREATION"
    '                        Common_Procedures.UR.Ledger_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_AGENT_CREATION"
    '                        Common_Procedures.UR.Agent_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_TRANSPORT_CREATION"
    '                        Common_Procedures.UR.Transport_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_AREA_CREATION"
    '                        Common_Procedures.UR.Area_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_GREYITEM_CREATION"
    '                        Common_Procedures.UR.GreyItem_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_FINISHEDPRODUCT_CREATION"
    '                        Common_Procedures.UR.FinishedProduct_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_ITEMGROUP_CREATION"
    '                        Common_Procedures.UR.ItemGroup_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_UNIT_CREATION"
    '                        Common_Procedures.UR.Unit_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_PRODUCT_SALES_NAME_CREATION"
    '                        Common_Procedures.UR.Product_Sales_Name_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_PROCESS_CREATION"
    '                        Common_Procedures.UR.Process_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_COLOUR_CREATION"
    '                        Common_Procedures.UR.Colour_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_LOTNO_CREATION"
    '                        Common_Procedures.UR.LotNo_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_RACKNO_CREATION"
    '                        Common_Procedures.UR.RackNo_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_LEDGER_OPENING_BALANCE"
    '                        Common_Procedures.UR.Ledger_Opening_Balance = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_GODOWN_OPENINGSTOCK"
    '                        Common_Procedures.UR.Godown_OpeningStock = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_PROCESSING_DELIVERY_OPENING"
    '                        Common_Procedures.UR.Processing_Delivery_Opening = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_FP_PURCHASE"
    '                        Common_Procedures.UR.FP_Purchase_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_PURCHASERETURN"
    '                        Common_Procedures.UR.FP_PurchaseReturn_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_PROCESSING_DELIVERY"
    '                        Common_Procedures.UR.Processing_Delivery_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_PROCESSING_RECEIPT"
    '                        Common_Procedures.UR.Processing_Receipt_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_PROCESSING_RETURN"
    '                        Common_Procedures.UR.Processing_Return_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_PROCESSING_BILLMAKING"
    '                        Common_Procedures.UR.Processing_BillMaking_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_DELIVERY_FLOOR_TO_RACK"
    '                        Common_Procedures.UR.Delivery_Entry_Floor_To_Rack = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_RETURN_RACK_TO_FLOOR"
    '                        Common_Procedures.UR.Return_Entry_Rack_To_Floor = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_SETENTRY"
    '                        Common_Procedures.UR.Set_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_UNSETENTRY"
    '                        Common_Procedures.UR.UnSet_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_ITEM_TRANSFER"
    '                        Common_Procedures.UR.Item_Transfer_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_PACKINGSLIP"
    '                        Common_Procedures.UR.PackinSlip_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_INVOICE"
    '                        Common_Procedures.UR.Invoice_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_ORDER"
    '                        Common_Procedures.UR.Order_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_CASHSALES"
    '                        Common_Procedures.UR.Cash_Sales_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_SALESRETURN"
    '                        Common_Procedures.UR.Sales_Return_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_PARTY_RECEIPT_CASH_CHEQUE"
    '                        Common_Procedures.UR.Party_Receipt_Entry_Cheque_Cash = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_CHEQUE_RETURN"
    '                        Common_Procedures.UR.Cheque_Return_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_VOUCHER"
    '                        Common_Procedures.UR.Voucher_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ACCOUNTS_LEDGER_REPORT"
    '                        Common_Procedures.UR.Accounts_Ledger_Report = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ACCOUNTS_GROUPLEDGER_REPORT"
    '                        Common_Procedures.UR.Accounts_GroupLedger_Report = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ACCOUNTS_DAYBOOK"
    '                        Common_Procedures.UR.Accounts_DayBook = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ACCOUNTS_ALLLEDGER"
    '                        Common_Procedures.UR.Accounts_AllLedger = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ACCOUNTS_TB"
    '                        Common_Procedures.UR.Accounts_TB = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ACCOUNTS_PROFIT_LOSS"
    '                        Common_Procedures.UR.Accounts_Profit_Loss = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ACCOUNTS_BALANCESHEET"
    '                        Common_Procedures.UR.Accounts_BalanceSheet = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ACCOUNTS_CUSTOMERBILLS"
    '                        Common_Procedures.UR.Accounts_CustomerBills = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ACCOUNTS_AGENTBILLS"
    '                        Common_Procedures.UR.Accounts_AgentBills = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ACCOUNTS_AGENTCOMMISSION"
    '                        Common_Procedures.UR.Accounts_AgentCommission = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_PURCHASE_REGISTER"
    '                        Common_Procedures.UR.Report_Purchase_Register = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_PURCHASERETURN_REGISTER"
    '                        Common_Procedures.UR.Report_PurchaseReturn_Register = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_DELIVERY_REGISTER_FLOOR_TO_RACK"
    '                        Common_Procedures.UR.Report_Delivery_Register_Floor_To_Rack = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_RETURN_REGISTER_RACK_TO_FLOOR"
    '                        Common_Procedures.UR.Report_Return_Register_Rack_To_Floor = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_PROCESSING_DELIVERY_REGISTER"
    '                        Common_Procedures.UR.Report_Processing_Delivery_Register = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_PROCESSING_RECEIPT_REGISTER"
    '                        Common_Procedures.UR.Report_Processing_Receipt_Register = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_PROCESSING_RETURN_REGISTER"
    '                        Common_Procedures.UR.Report_Processing_Return_Register = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_PROCESSING_BILLMAKING_REGISTER"
    '                        Common_Procedures.UR.Report_Processing_Billmaking_Register = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_PROCESSING_RECEIPT_PENDING_REGISTER"
    '                        Common_Procedures.UR.Report_Processing_Receipt_Pending_Register = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_PROCESSING_BILLMAKING_PENDING_REGISTER"
    '                        Common_Procedures.UR.Report_Processing_Billmaking_Pending_Register = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_SET_ENTRY_REGISTER"
    '                        Common_Procedures.UR.Report_SetEntry_Register = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_UNSET_ENTRY_REGISTER"
    '                        Common_Procedures.UR.Report_UnSetEntry_Register = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_ITEMTRANSFER_REGISTER"
    '                        Common_Procedures.UR.Report_ItemTransfer_Register = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_PACKINGSLIP_REGISTER"
    '                        Common_Procedures.UR.Report_PackingSlip_Register = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_INVOICE_REGISTER"
    '                        Common_Procedures.UR.Report_Invoice_Register = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_CASHSALES_REGISTER"
    '                        Common_Procedures.UR.Report_CashSales_Register = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_SALESRETURN_REGISTER"
    '                        Common_Procedures.UR.Report_SalesReturn_Register = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_PARTY_RECEIPT_CASH_CHEQUE_REGISTER"
    '                        Common_Procedures.UR.Report_PartyReceipt_Register_Cash_Cheque = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_FP_REGISTERS"
    '                        Common_Procedures.UR.Report_FP_Registers = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_ITEMLIST"
    '                        Common_Procedures.UR.Report_ItemList = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_PARTYLIST"
    '                        Common_Procedures.UR.Report_PartyList = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_STOCK_REGISTER"
    '                        Common_Procedures.UR.Report_Stock_Register = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_STOCK_SUMMARY"
    '                        Common_Procedures.UR.Report_Stock_Summary = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_STOCK_VALUE"
    '                        Common_Procedures.UR.Report_Stock_Value = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_MINIMUM_STOCK_LEVEL"
    '                        Common_Procedures.UR.Report_Minimum_Stock_Value = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_GREYFABRIC_STOCK"
    '                        Common_Procedures.UR.Report_GreyFabric_Stock = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "REPORT_FINISHED_PRODUCT_STOCK"
    '                        Common_Procedures.UR.Report_FinishedProduct_Stock = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_STORES_DEPARTMENT_CREATION"
    '                        Common_Procedures.UR.Master_Stores_Department_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_STORES_MACHINE_CREATION"
    '                        Common_Procedures.UR.Master_Stores_Machine_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_STORES_BRAND_CREATION"
    '                        Common_Procedures.UR.Master_Stores_Brand_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_STORES_ITEM_CREATION"
    '                        Common_Procedures.UR.Master_Stores_Item_Creation = Dt1.Rows(i).Item("Access_Type").ToString



    '                    Case "MASTER_STORES_REEDCOUNT_CREATION"
    '                        Common_Procedures.UR.Master_Stores_ReedCount_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_STORES_REEDWIDTH_CREATION"
    '                        Common_Procedures.UR.Master_Stores_ReedWidth_Creation = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "MASTER_OPENING_STOCK"
    '                        Common_Procedures.UR.Master_Opening_Stock = Dt1.Rows(i).Item("Access_Type").ToString


    '                    Case "ENTRY_STORES_PURCHASE_ORDER"
    '                        Common_Procedures.UR.Entry_Stores_Purchase_Order = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_STORES_PURCHASE_INWARD"
    '                        Common_Procedures.UR.Entry_Stores_Purchase_Inward = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_STORES_PURCHASE_RETURN"
    '                        Common_Procedures.UR.Entry_Stores_Purchase_Return = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_STORES_ITEMISSUE_TO_MACHINE"
    '                        Common_Procedures.UR.Entry_Stores_Item_Issue = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_STORES_ITEMRETURN_FROM_MACHINE"
    '                        Common_Procedures.UR.Entry_Stores_Item_Return = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_STORES_ITEM_DELIVERY"
    '                        Common_Procedures.UR.Entry_Stores_Item_Delivery = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_STORES_ITEM_RECEIPT"
    '                        Common_Procedures.UR.Entry_Stores_Item_Receipt = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_STORES_SERVICE_ITEM_DELIVERY"
    '                        Common_Procedures.UR.Entry_Stores_Service_Item_Delivery = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_STORES_SERVICE_ITEM_RECEIPT"
    '                        Common_Procedures.UR.Entry_Stores_Service_Item_Receipt = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_STORES_GATE_PASS"
    '                        Common_Procedures.UR.Entry_Stores_GatePass = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_STORES_ITEM_EXCESS_SHORT"
    '                        Common_Procedures.UR.Entry_Stores_Item_Excess_Short = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_STORES_ITEM_DISPOSE"
    '                        Common_Procedures.UR.Entry_Stores_Item_Dispose = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_STORES_OIL_SERVICE"
    '                        Common_Procedures.UR.Entry_Stores_Oil_Service = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_BOBIN_SALES_DELIVERY"
    '                        Common_Procedures.UR.Bobin_Sales_Delivery_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_JARI_SALES_DELIVERY"
    '                        Common_Procedures.UR.Jari_Sales_Delivery_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_PURCHASE_DELIVERY"
    '                        Common_Procedures.UR.Bobin_Purchase_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                    Case "ENTRY_FABRIC_PHYSICAL_STOCK"
    '                        Common_Procedures.UR.fabric_physical_stock_Entry = Dt1.Rows(i).Item("Access_Type").ToString

    '                End Select

    '            Next

    '        End If

    '    End If

    '    Dt1.Dispose()
    '    Da.Dispose()

    'End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        'MsgBox("CommonAppDataPath = " & Application.CommonAppDataPath)
        'MsgBox("ExecutablePath = " & Application.ExecutablePath)
        'MsgBox("StartupPath = " & Application.StartupPath)
        'MsgBox("UserAppDataPath = " & Application.UserAppDataPath)
        'MsgBox("LocalUserAppDataPath = " & Application.LocalUserAppDataPath)
    End Sub

    Private Sub btn_Encrypt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Encrypt.Click
        Dim YrTxt As String = ""

        YrTxt = InputBox("Enter Your Text", "FOR ENCRYPTION...")
        strEncryptedText = Common_Procedures.Encrypt(YrTxt, Trim(Common_Procedures.Entrance_SQL_PassWord.passPhrase), Trim(Common_Procedures.Entrance_SQL_PassWord.passPhrase))
        YrTxt = InputBox("Encrypted Chipper Text is ", "FOR ENCRYPTION...", strEncryptedText)
        'MessageBox.Show(strEncryptedText)

        '""  = "6MaToWZFd8gFqn7IRvJpSg=="
        '"ts1415"  = "Xt69CyjgNt3eyc3SM7iexQ=="
        '"tsoftsql"  = "A0a+NXLTUaoY+6d13TrkiA=="
        '"TSOFTSQL"  = "AJsAV/xSlZC23ITrL/XPhQ=="
    End Sub

    Private Sub btn_Decrypt_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Decrypt.Click
        strDecrptedText = Common_Procedures.Decrypt(strEncryptedText, Trim(Common_Procedures.Entrance_SQL_PassWord.passPhrase), Trim(Common_Procedures.Entrance_SQL_PassWord.passPhrase))
        MessageBox.Show(strDecrptedText)
    End Sub

    Private Sub btn_Register_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Register.Click
        'Dim f As New Tsoft_Register_Encryption_DeCrption_Form
        'f.Show()
    End Sub

    Private Sub Demo_Data_Checking(ByVal Cn1 As SqlClient.SqlConnection)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim Nr As Integer = 0
        Dim DataCount As Integer = 0
        Dim LedCount As Integer = 0
        Dim vDBName As String = ""

        If Val(Common_Procedures.settings.CustomerDBCode) > 1000 And Val(Common_Procedures.settings.CustomerDBCode) < 9999 Then
            Exit Sub
        End If

        DataCount = 0
        LedCount = 0

        da1 = New SqlClient.SqlDataAdapter("Select * from CompanyGroup_Head Order by CompanyGroup_IdNo", Cn1)
        dt1 = New DataTable
        da1.Fill(dt1)
        If dt1.Rows.Count > 0 Then

            For I = 0 To dt1.Rows.Count - 1

                vDBName = Common_Procedures.get_Company_DataBaseName(Trim(Val(dt1.Rows(I).Item("CompanyGroup_IdNo").ToString)))

                da1 = New SqlClient.SqlDataAdapter("Select * from master..sysdatabases Where name = '" & Trim(vDBName) & "'", Cn1)
                dt2 = New DataTable
                da1.Fill(dt2)
                If dt2.Rows.Count > 0 Then

                    da1 = New SqlClient.SqlDataAdapter("Select count(Voucher_Code) from " & vDBName & "..voucher_head", Cn1)
                    dt3 = New DataTable
                    da1.Fill(dt3)
                    If dt3.Rows.Count > 0 Then
                        If Not IsDBNull(dt3.Rows(0).Item(0)) Then
                            DataCount = DataCount + Val(dt3.Rows(0).Item(0))
                        End If
                    End If
                    dt3.Clear()

                    da1 = New SqlClient.SqlDataAdapter("Select count(Reference_Code) from " & vDBName & "..Stock_Pavu_Processing_Details", Cn1)
                    dt3 = New DataTable
                    da1.Fill(dt3)
                    If dt3.Rows.Count > 0 Then
                        If Not IsDBNull(dt3.Rows(0).Item(0)) Then
                            DataCount = DataCount + Val(dt3.Rows(0).Item(0))
                        End If
                    End If
                    dt3.Clear()

                    da1 = New SqlClient.SqlDataAdapter("Select count(Reference_Code) from " & vDBName & "..Stock_Yarn_Processing_Details", Cn1)
                    dt3 = New DataTable
                    da1.Fill(dt3)
                    If dt3.Rows.Count > 0 Then
                        If Not IsDBNull(dt3.Rows(0).Item(0)) Then
                            DataCount = DataCount + Val(dt3.Rows(0).Item(0))
                        End If
                    End If
                    dt3.Clear()

                    da1 = New SqlClient.SqlDataAdapter("Select count(Reference_Code) from " & vDBName & "..Stock_Cloth_Processing_Details", Cn1)
                    dt3 = New DataTable
                    da1.Fill(dt3)
                    If dt3.Rows.Count > 0 Then
                        If Not IsDBNull(dt3.Rows(0).Item(0)) Then
                            DataCount = DataCount + Val(dt3.Rows(0).Item(0))
                        End If
                    End If
                    dt3.Clear()

                    da1 = New SqlClient.SqlDataAdapter("Select count(Ledger_IdNo) from " & vDBName & "..ledger_head where Ledger_IdNo > 100", Cn1)
                    dt3 = New DataTable
                    da1.Fill(dt3)
                    If dt3.Rows.Count > 0 Then
                        If Not IsDBNull(dt3.Rows(0).Item(0)) Then
                            LedCount = LedCount + Val(dt3.Rows(0).Item(0))
                        End If
                    End If
                    dt3.Clear()

                End If
                dt2.Clear()

            Next I

        End If

        dt1.Clear()

        If LedCount > 100 Or DataCount > 500 Then
            MessageBox.Show("SoftWare Trial Period Expires", "TSOFT SOLUTIONS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Me.Close()
            Application.Exit()
            End
        End If

        dt1.Dispose()
        dt2.Dispose()
        dt3.Dispose()
        da1.Dispose()

    End Sub

    Private Sub lbl_Restore_Database_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbl_Restore_Database.Click
        Dim cn1 As SqlClient.SqlConnection
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As SqlClient.SqlDataAdapter
        Dim Dt1 As DataTable
        Dim BackUp_FlName As String
        Dim Resdb_name As String
        Dim ResDB_MDF_Name As String, ResDB_LDF_Name As String
        Dim ResDB_MDF_FilePath As String, ResDB_LDF_FilePath As String
        Dim BackUp_File_MDF_Name As String, BackUp_File_LDFName As String

        Dim g As New Password
        g.ShowDialog()

        If Trim(UCase(Common_Procedures.Password_Input)) <> "TS397417" Then
            MessageBox.Show("Invalid Password", "PASSWORD FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        OpenFileDialog1.ShowDialog()
        BackUp_FlName = OpenFileDialog1.FileName

        If Trim(BackUp_FlName) = "" Then
            MessageBox.Show("Invalid FileName", "DOES NOT RESTORE DATABASE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        cn1 = New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
        cn1.Open()
        Resdb_name = cn1.Database
        cn1.Close()

        cn1 = New SqlClient.SqlConnection(Common_Procedures.ConnectionString_Master)
        cn1.Open()

        ResDB_MDF_Name = "" : ResDB_LDF_Name = ""
        ResDB_MDF_FilePath = "" : ResDB_LDF_FilePath = ""

        Da1 = New SqlClient.SqlDataAdapter("select * from sysdatabases where name = '" & Trim(Resdb_name) & "'", cn1)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            If IsDBNull(Dt1.Rows(0).Item("FileName").ToString) = False Then

                ResDB_MDF_Name = Dt1.Rows(0).Item("Name").ToString

                If InStr(1, LCase(ResDB_MDF_Name), "_data") > 0 Then
                    ResDB_LDF_Name = Replace(LCase(ResDB_MDF_Name), "_data", "_log")
                Else
                    ResDB_LDF_Name = Trim(LCase(ResDB_MDF_Name)) & "_log"
                End If

                ResDB_MDF_FilePath = Dt1.Rows(0).Item("FileName").ToString

                If InStr(1, LCase(ResDB_MDF_FilePath), "_data.mdf") > 0 Then
                    ResDB_LDF_FilePath = Replace(LCase(ResDB_MDF_FilePath), "_data.mdf", "_log.ldf")
                Else
                    ResDB_LDF_FilePath = Replace(LCase(ResDB_MDF_FilePath), ".mdf", "_log.ldf")
                End If

            End If

        End If

        BackUp_File_MDF_Name = ""
        BackUp_File_LDFName = ""

        Da1 = New SqlClient.SqlDataAdapter("exec(N'RESTORE FILELISTONLY FROM DISK=N''" & Trim(BackUp_FlName) & "'' WITH FILE=1')", cn1)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("LogicalName").ToString) = False Then
                If Trim(UCase(Dt1.Rows(0).Item("Type").ToString)) = "D" Then
                    BackUp_File_MDF_Name = (Dt1.Rows(0).Item("LogicalName").ToString)
                Else
                    BackUp_File_LDFName = (Dt1.Rows(0).Item("LogicalName").ToString)
                End If
            End If
            If IsDBNull(Dt1.Rows(1).Item("LogicalName").ToString) = False Then
                If Trim(UCase(Dt1.Rows(1).Item("Type").ToString)) = "D" Then
                    BackUp_File_MDF_Name = (Dt1.Rows(1).Item("LogicalName").ToString)
                Else
                    BackUp_File_LDFName = (Dt1.Rows(1).Item("LogicalName").ToString)
                End If
            End If
        End If

        cmd.Connection = cn1

        cmd.CommandText = "ALTER DATABASE " & Trim(Resdb_name) & " SET OFFLINE WITH ROLLBACK IMMEDIATE"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "RESTORE DATABASE " & Trim(Resdb_name) & " FROM DISK = '" & BackUp_FlName & "' WITH MOVE '" & BackUp_File_MDF_Name & "' TO '" & ResDB_MDF_FilePath & "', MOVE '" & BackUp_File_LDFName & "' TO '" & ResDB_LDF_FilePath & "', REPLACE"
        cmd.ExecuteNonQuery()

        Dt1.Dispose()
        Da1.Dispose()

        cmd.Dispose()

        cn1.Close()
        cn1.Dispose()

        MessageBox.Show("Restores Successfully!!", "FOR COMPANYDETAILS DATABASE RESTORED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Entrance_Activated(sender, e)

    End Sub

    Private Sub lbl_Restore_Database_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_Restore_Database.MouseHover
        lbl_Restore_Database.BackColor = Color.Lime
    End Sub

    Private Sub lbl_Restore_Database_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_Restore_Database.MouseLeave
        lbl_Restore_Database.BackColor = Color.LightSkyBlue
    End Sub

    Private Sub lbl_Restore_Database_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbl_Restore_Database.MouseMove
        lbl_Restore_Database.BackColor = Color.Lime
    End Sub

    Private Sub Change_System_DateTime_To_Internet_DateTime()
        Try

            If Not (Trim(UCase(Common_Procedures.settings.CustomerCode)) = "9999") Then '---- UNITED WEAVES (PALLADAM)
                Exit Sub
            End If

            If My.Computer.Network.IsAvailable = True Then
                If My.Computer.Network.Ping("www.Google.com") = True Then
                    If My.Computer.Network.Ping("www.Google.com") = True Then
                        If Daytime.WindowsClockIncorrect() Then
                            Daytime.SetWindowsClock(Daytime.GetTime())
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            '-----
        End Try
    End Sub

    Private Sub Ceate_PrintBatchFile()
        Dim fs As FileStream
        Dim sw As StreamWriter

        Try

            Common_Procedures.Dos_Printing_FileName_Path = Path.GetPathRoot(Common_Procedures.AppPath) & "print.txt"

            Common_Procedures.Dos_Print_BatchFileName_Path = Trim(Common_Procedures.AppPath) & "\print.bat"

            If File.Exists(Common_Procedures.Dos_Print_BatchFileName_Path) = False Then
                fs = New FileStream(Common_Procedures.Dos_Print_BatchFileName_Path, FileMode.Create)
                sw = New StreamWriter(fs)
                sw.WriteLine("Print " & Common_Procedures.Dos_Printing_FileName_Path & " > prn")
                'sw.WriteLine("type " & Common_Procedures.Dos_Printing_FileName_Path & " > LPT3")
                'sw.WriteLine("type " & Common_Procedures.Dos_Printing_FileName_Path & " > prn")
                sw.Close()
                fs.Close()
                sw.Dispose()
                fs.Dispose()
            End If


            Common_Procedures.Dos_PrintPreView_BatchFileName_Path = Trim(Common_Procedures.AppPath) & "\preview.bat"

            If File.Exists(Common_Procedures.Dos_PrintPreView_BatchFileName_Path) = False Then
                fs = New FileStream(Common_Procedures.Dos_PrintPreView_BatchFileName_Path, FileMode.Create)
                sw = New StreamWriter(fs)
                sw.WriteLine("edit " & Common_Procedures.Dos_Printing_FileName_Path)
                sw.Close()
                fs.Close()
                sw.Dispose()
                fs.Dispose()
            End If

        Catch ex As Exception
            '------

        Finally
            '------

        End Try

    End Sub

    Public Sub Update_System_Name_Exe_DateTime(ByVal Cn1 As SqlClient.SqlConnection)
        Dim cmd As New SqlClient.SqlCommand
        Dim vSYSNm As String = ""
        Dim vExeFileNm As String = ""
        Dim Nr As Integer = 0
        Dim vDttm As Date
        Dim DefPath As String = ""
        Dim servtype As String = ""
        Dim InstNm As String = ""
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim vSysSlNo As String = ""

        Try

            If Common_Procedures.Office_System_Status = True Then
                Exit Sub
            End If

            vSYSNm = SystemInformation.ComputerName
            vSysSlNo = Common_Procedures.GetDriveSerialNumber(Microsoft.VisualBasic.Left(Application.StartupPath, 2))

            'vSYSNm = Get
            vExeFileNm = Application.ExecutablePath
            vExeFileNm = IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath)

            vDttm = File.GetLastWriteTime(Application.ExecutablePath)

            cmd.Connection = Cn1

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@ExeDate", vDttm)

            cmd.Parameters.AddWithValue("@sysdatetime", Now)

            InstNm = Microsoft.VisualBasic.Right(Common_Procedures.ServerName, Len(Common_Procedures.ServerName) - InStr(1, Common_Procedures.ServerName, "\"))

            If Common_Procedures.Server_System_Status = True Then
                servtype = "SERVER"
            Else
                servtype = "CLIENT"
            End If

            DefPath = ""
            If Trim(UCase(servtype)) = "SERVER" Then

                cmd.CommandText = "Update System_name_Details set [Type] = 'CLIENT'"
                cmd.ExecuteNonQuery()


                Da1 = New SqlClient.SqlDataAdapter("SELECT * FROM master..sysdatabases WHERE name = 'master'", Cn1)
                Dt2 = New DataTable
                Da1.Fill(Dt2)
                If Dt2.Rows.Count > 0 Then

                    If IsDBNull(Dt2.Rows(0).Item("FileName").ToString) = False Then

                        DefPath = Replace(LCase(Dt2.Rows(0).Item("FileName").ToString), "\master_data.mdf", "")
                        DefPath = Replace(LCase(Dt2.Rows(0).Item("FileName").ToString), "\master.mdf", "")

                    End If

                End If

            End If

            cmd.CommandText = "delete from System_name_Details Where Computer_SerialNo = '" & Trim(vSysSlNo) & "'"
            Nr = cmd.ExecuteNonQuery()

            'If Nr = 0 Then
            cmd.CommandText = "delete from System_name_Details Where Computer_name = '" & Trim(vSYSNm) & "'"
            Nr = cmd.ExecuteNonQuery()
            'End If

            cmd.CommandText = "Insert into System_name_Details (      Type                 ,    Sql_Instance_name   ,     Sql_Data_path      ,     Computer_name     ,      Computer_SerialNo  ,      Software_Exe_Name    , Exe_Date_Time ,                       Software_Path       , Last_Opened_SystemDateTime   ) " &
                              " Values                         ( '" & Trim(servtype) & "'  , '" & Trim(InstNm) & "' , '" & Trim(DefPath) & "', '" & Trim(vSYSNm) & "', '" & Trim(vSysSlNo) & "', '" & Trim(vExeFileNm) & "',     @ExeDate  , '" & Trim(Common_Procedures.AppPath) & "' ,          @sysdatetime        ) "
            cmd.ExecuteNonQuery()

            vCSND_servtype = servtype
            vCSND_InstNm = InstNm
            vCSND_DefPath = DefPath
            vCSND_vSYSNm = vSYSNm
            vCSND_vSysSlNo = vSysSlNo
            vCSND_vExeFileNm = vExeFileNm

        Catch ex As Exception
            If InStr(1, Trim(LCase(ex.Message)), Trim(LCase("ix_System_name_Details"))) > 0 Then
                MessageBox.Show("Duplicate System Name", "ERROR IN UPDATING SYSTEM NAMES...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "ERROR IN UPDATING SYSTEM NAMES...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Finally
            '-----

        End Try
    End Sub

    Public Function get_File_Modified_DateTime(vFileName As String) As Date
        Dim fi As New IO.FileInfo(vFileName)
        Dim exists As Boolean = fi.Exists
        If fi.Exists = True Then
            Dim updatedTime As DateTime = fi.LastWriteTime
            Return updatedTime
            Exit Function
        End If
        Return Nothing
    End Function

    Private Sub Check_Update_Exe_File_DateTime(ByVal Cn2 As SqlClient.SqlConnection)
        Dim cmd As New SqlClient.SqlCommand
        Dim dat1 As Date
        Dim dat2 As Date
        Dim Nr As Integer
        Dim I As Integer
        Dim DatChkSTS As Boolean = False
        Dim vFileDtTm As Date
        Dim dttm1 As Date = #1/1/2000#
        Dim vFileDatTimSTS As Boolean = False

        Try

            If Common_Procedures.Office_System_Status = True Then
                Exit Sub
            End If

            vNEWEXE_Status = False

            dat1 = #1/1/2000#
            If IsDate(Common_Procedures.settings.ExeFile_DateTime) Then dat1 = Common_Procedures.settings.ExeFile_DateTime

            vFileDtTm = get_File_Modified_DateTime(Application.ExecutablePath)

            dat2 = #1/1/2000#
            If IsDate(vFileDtTm) Then dat2 = vFileDtTm

            If DateDiff(DateInterval.Minute, dat1, dat2) > 1 Then

                cmd.Connection = Cn2

                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@ExeFileDateTime", dat2)

                Nr = 0
                cmd.CommandText = "Update Settings_Head set ExeFile_DateTime = @ExeFileDateTime"
                Nr = cmd.ExecuteNonQuery()

                If Nr = 0 Then
                    cmd.CommandText = "Insert into Settings_Head(ExeFile_DateTime) Values (@ExeFileDateTime)"
                    cmd.ExecuteNonQuery()
                End If

                vNEWEXE_Status = True

            ElseIf DateDiff(DateInterval.Minute, dat1, dat2) < -1 Then

                If Common_Procedures.settings.Dont_Open_Software_if_Software_Updates_Available = 1 Then
                    Dim mymsgbox As New Tsoft_MessageBox("Software Updates Available" & Chr(13) & "Software Updated on " & Format(Common_Procedures.settings.ExeFile_DateTime, "dd-MM-yyyy hh:mm tt"), "OK,UPDATE FROM SERVER,UPDATE ONLINE,CLOSE", "FOR SOFTWARE UPDATES....", "Contact System admin to update software", MesssageBoxIcons.Errors, 1, 4)
                    mymsgbox.ShowDialog()
                    End
                Else
                    MessageBox.Show("Software Updates Available" & Chr(13) & "Software Updated on " & Format(Common_Procedures.settings.ExeFile_DateTime, "dd-MM-yyyy hh:mm tt"), "FOR SOFTWARE UPDATES....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR IN UPDATING EXE FILE DATE/TIME", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            cmd.Dispose()

        End Try

    End Sub

    Private Function Get_Codes_From_LicenseFile() As String
        Dim pth As String
        Dim fs As FileStream
        Dim r As StreamReader
        Dim vLicCode As String = ""

        Get_Codes_From_LicenseFile = vLicCode

        Try

            If InStr(1, Trim(LCase(Application.StartupPath)), "\bin\debug") > 0 Then
                Common_Procedures.AppPath = Replace(Trim(LCase(Application.StartupPath)), "\bin\debug", "")
            Else
                Common_Procedures.AppPath = Application.StartupPath
            End If

            pth = Trim(Common_Procedures.AppPath) & "\license.ini"

            vLicCode = ""

            If File.Exists(pth) = False Then
                Get_Codes_From_LicenseFile = vLicCode
                MessageBox.Show("Invalid  License - License File not exists", "INVALID REGISTERATION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Me.Close()
                Application.Exit()
                End
                Exit Function
            End If

            If File.Exists(pth) = True Then
                fs = New FileStream(pth, FileMode.Open)
                r = New StreamReader(fs)
                vLicCode = r.ReadLine
                r.Close()
                fs.Close()
                r.Dispose()
                fs.Dispose()
            End If

            Get_Codes_From_LicenseFile = vLicCode

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT OPEN...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Function

    Private Sub Copy_Supporting_Files()
        Dim fs As FileStream
        Dim sw As StreamWriter
        Dim pth As String
        Dim FontsPath As String = ""
        Dim FontsFilenamepath As String = ""

        Try

            pth = Trim(Common_Procedures.AppPath) & "\Script.sql"
            If File.Exists(pth) = False Then
                File.WriteAllText(pth, My.Resources.Script)
            End If

            Common_Procedures.Dos_Printing_FileName_Path = Path.GetPathRoot(Common_Procedures.AppPath) & "print.txt"

            Common_Procedures.Dos_Print_BatchFileName_Path = Trim(Common_Procedures.AppPath) & "\print.bat"

            If File.Exists(Common_Procedures.Dos_Print_BatchFileName_Path) = False Then
                fs = New FileStream(Common_Procedures.Dos_Print_BatchFileName_Path, FileMode.Create)
                sw = New StreamWriter(fs)
                sw.WriteLine("Print " & Common_Procedures.Dos_Printing_FileName_Path & " > prn")
                'sw.WriteLine("type " & Common_Procedures.Dos_Printing_FileName_Path & " > LPT3")
                'sw.WriteLine("type " & Common_Procedures.Dos_Printing_FileName_Path & " > prn")
                sw.Close()
                fs.Close()
                sw.Dispose()
                fs.Dispose()
            End If

            Common_Procedures.Dos_PrintPreView_BatchFileName_Path = Trim(Common_Procedures.AppPath) & "\preview.bat"

            If File.Exists(Common_Procedures.Dos_PrintPreView_BatchFileName_Path) = False Then
                fs = New FileStream(Common_Procedures.Dos_PrintPreView_BatchFileName_Path, FileMode.Create)
                sw = New StreamWriter(fs)
                sw.WriteLine("edit " & Common_Procedures.Dos_Printing_FileName_Path)
                sw.Close()
                fs.Close()
                sw.Dispose()
                fs.Dispose()
            End If

            pth = Trim(Common_Procedures.AppPath) & "\AWSSDK.Core.dll"
            If File.Exists(pth) = False Then
                File.WriteAllBytes(pth, My.Resources.AWSSDK_Core)
            End If


            pth = Trim(Common_Procedures.AppPath) & "\AWSSDK.S3.dll"
            If File.Exists(pth) = False Then
                File.WriteAllBytes(pth, My.Resources.AWSSDK_S3)
            End If

            FontsPath = Environment.GetFolderPath(Environment.SpecialFolder.Fonts)



            'FontsFilenamepath = Trim(FontsPath) & "\baamini.ttf"
            'pth = Trim(Common_Procedures.AppPath) & "\baamini.ttf"
            'If File.Exists(pth) = False Then
            '    File.WriteAllBytes(pth, My.Resources.baamini)
            'End If
            'If File.Exists(FontsFilenamepath) = False Then
            '    File.WriteAllBytes(FontsFilenamepath, My.Resources.baamini)
            'End If

            pth = Trim(Common_Procedures.AppPath) & "\SaiIndira.ttf"
            FontsFilenamepath = Trim(FontsPath) & "\SaiIndira.ttf"
            If File.Exists(pth) = False Then
                File.WriteAllBytes(pth, My.Resources.SaiIndira)
            End If
            If File.Exists(FontsFilenamepath) = False Then
                File.WriteAllBytes(FontsFilenamepath, My.Resources.SaiIndira)
            End If

            pth = Trim(Common_Procedures.AppPath) & "\Tam_ss2.ttf"
            FontsFilenamepath = Trim(FontsPath) & "\Tam_ss2.ttf"
            If File.Exists(pth) = False Then
                File.WriteAllBytes(pth, My.Resources.Tam_ss2)
            End If
            If File.Exists(FontsFilenamepath) = False Then
                File.WriteAllBytes(FontsFilenamepath, My.Resources.Tam_ss2)
            End If

            'Americana Std.otf
            'Americana Std Roman

            pth = Trim(Common_Procedures.AppPath) & "\Americana Std Roman.otf"
            FontsFilenamepath = Trim(FontsPath) & "\Americana Std Roman.otf"

            If File.Exists(pth) = False Then
                File.WriteAllBytes(pth, My.Resources.Americana_Std_Roman)
            End If
            If File.Exists(FontsFilenamepath) = False Then
                File.WriteAllBytes(FontsFilenamepath, My.Resources.Americana_Std_Roman)
            End If

            pth = Trim(Common_Procedures.AppPath) & "\netuse.bat"
            If File.Exists(pth) = True Then
                Shell(pth, AppWinStyle.Hide)
            End If


        Catch ex As Exception
            '------
            ' MessageBox.Show(ex.Message)
        Finally
            '------

        End Try

    End Sub

    Private Sub Start_SQL_Server()

        'Dim str As String = Nothing

        ''Declare and create an instance of the ManagedComputer object that represents the WMI Provider services.

        'Dim mc As ManagedComputer
        'mc = New ManagedComputer("ACER")

        ''Iterate through each service registered with the WMI Provider.

        'Dim svc As Service

        'For Each svc In mc.Services

        '    If UCase(svc.Name).Contains("$NOVA2014") Then

        '        If svc.ServiceState = ServiceState.Running Then

        '            svc.Stop()
        '            Do Until svc.ServiceState = ServiceState.Running
        '                str = "The Curent State of " & svc.Name & " is " & svc.ServiceState
        '                svc.Refresh()
        '            Loop

        '            'Start the service and report on the status continuously until it has started.
        '            svc.Stop()
        '            Do Until svc.ServiceState = ServiceState.Stopped
        '                str = "The Curent State of " & svc.Name & " is " & svc.ServiceState
        '                svc.Refresh()
        '            Loop

        '            MsgBox(svc.Name & " stopped.")

        '        Else
        '            MsgBox("SQL SERVER IS NOT RUNNING.")
        '        End If

        '    End If
        'Next

        '---------------------------------

        Dim vSQLInstNm As String, vSQLServiceNm As String
        vSQLInstNm = Common_Procedures.get_SQLServer_InstanceName

        If Trim(vSQLInstNm) <> "" Then
            vSQLServiceNm = "MSSQL$" & Trim(vSQLInstNm)

        Else
            vSQLServiceNm = "MSSQLSERVER"

        End If


        Dim scServices As ServiceController()
        scServices = ServiceController.GetServices()

        For Each scTemp As ServiceController In scServices

            'If scTemp.ServiceName.Contains("NOVA2014") Then
            '    MsgBox(scTemp.ServiceName)
            'End If

            'MsgBox(scTemp.ServiceName)

            If scTemp.ServiceName.Contains(vSQLServiceNm) Then
                ' Display properties for the Simple Service sample
                ' from the ServiceBase example.
                Dim sc As ServiceController = New ServiceController(scTemp.ServiceName)
                'Console.WriteLine("Status = " & sc.Status)
                'Console.WriteLine("Can Pause and Continue = " & sc.CanPauseAndContinue)
                'Console.WriteLine("Can ShutDown = " & sc.CanShutdown)
                'Console.WriteLine("Can Stop = " & sc.CanStop)

                'If sc.Status = ServiceControllerStatus.Stopped Then
                '    sc.Start()

                '    While sc.Status = ServiceControllerStatus.Stopped
                '        'Thread.Sleep(1000)
                '        sc.Refresh()
                '    End While
                'End If

                Dim cnt As Integer = 0

                If sc.Status = ServiceControllerStatus.Stopped Then

                    sc.Start()

                    Do

                        System.Threading.Thread.Sleep(500)
                        sc.Refresh()
                        cnt += 1

                    Loop Until sc.Status = ServiceControllerStatus.Running Or cnt <= 20

                    System.Threading.Thread.Sleep(1000)

                End If

                Exit For

                'If sc.Status = ServiceControllerStatus.Running Then
                '    sc.Stop()

                '    While sc.Status = ServiceControllerStatus.Running
                '        'Thread.Sleep(1000)
                '        sc.Refresh()
                '    End While

                'End If


                ' Issue custom commands to the service
                ' enum SimpleServiceCustomCommands
                '    { StopWorker = 128, RestartWorker, CheckWorker };
                'sc.ExecuteCommand(START)
                'sc.ExecuteCommand(SimpleServiceCustomCommands.RestartWorker)
                'sc.Stop()
                'sc.Pause()

                'While sc.Status <> ServiceControllerStatus.Paused
                '    'Thread.Sleep(1000)
                '    sc.Refresh()
                'End While

                'Console.WriteLine("Status = " & sc.Status)
                'sc.[Continue]()

                'While sc.Status = ServiceControllerStatus.Paused
                '    'Thread.Sleep(1000)
                '    sc.Refresh()
                'End While

                'Console.WriteLine("Status = " & sc.Status)
                'sc.[Stop]()

                'While sc.Status <> ServiceControllerStatus.Stopped
                '    'Thread.Sleep(1000)
                '    sc.Refresh()
                'End While

                'Console.WriteLine("Status = " & sc.Status)
                'Dim argArray As String() = New String() {"ServiceController arg1", "ServiceController arg2"}
                'sc.Start(argArray)

                'While sc.Status = ServiceControllerStatus.Stopped
                '    'Thread.Sleep(1000)
                '    sc.Refresh()
                'End While

                'Console.WriteLine("Status = " & sc.Status)
                '' Display the event log entries for the custom commands
                '' and the start arguments.
                'Dim el As EventLog = New EventLog("Application")
                'Dim elec As EventLogEntryCollection = el.Entries

                'For Each ele As EventLogEntry In elec
                '    If ele.Source.IndexOf("SimpleService.OnCustomCommand") >= 0 Or ele.Source.IndexOf("SimpleService.Arguments") >= 0 Then Console.WriteLine(ele.Message)
                'Next

            End If
        Next

    End Sub

    Private Sub Connect_to_Master_Databases()
        Dim cn1 As SqlClient.SqlConnection
        Dim vSQL_REStartSTS As Boolean

        Try

            vSQL_REStartSTS = False

LOOP1:
            cn1 = New SqlClient.SqlConnection(Common_Procedures.ConnectionString_Master)
            cn1.Open()

            cn1.Close()

        Catch ex1 As Exception

            Try

                If vSQL_REStartSTS = False Then
                    Start_SQL_Server()
                    vSQL_REStartSTS = True
                    GoTo LOOP1
                End If

            Catch ex2 As Exception
                MessageBox.Show("Invalid Master Database Connection..." & Chr(13) & Err.Description, "ERROR WHILE CONNECTING SQL SERVER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Me.Close()
                Application.Exit()
                End
                Exit Sub

            End Try


        Finally



        End Try

        cn1.Dispose()

    End Sub

    Private Sub Design_CompanyGroup_Details_Grid()
        Dim vMONID As Integer = Month(Now)

        If Not (vMONID = 3 Or vMONID = 4) Then
            dgv_Details.Columns(3).Visible = False
            dgv_Details.Columns(0).Width = dgv_Details.Columns(0).Width + dgv_Details.Columns(3).Width
        End If

    End Sub

    Private Sub dgv_Details_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Details.CellContentClick
        Dim IdNo As Integer = 0
        Dim vNEWYRCODE As String = ""

        Try

            With dgv_Details

                If .Visible Then

                    If .Rows.Count > 0 Then

                        If IsNothing(.CurrentCell) Then Exit Sub

                        If e.ColumnIndex = 3 Then

                            IdNo = Val(dgv_Details.CurrentRow.Cells(1).Value)
                            vNEWYRCODE = dgv_Details.CurrentRow.Cells(3).Value

                            Common_Procedures.CompGroupIdNo = 0
                            Common_Procedures.CompGroupName = ""
                            Common_Procedures.CompGroupFnRange = ""

                            Common_Procedures.Connection_String = ""
                            Common_Procedures.DataBaseName = ""

                            Common_Procedures.CompIdNo = 0

                            Common_Procedures.FnRange = ""
                            Common_Procedures.FnYearCode = ""

                            If Val(IdNo) <> 0 And Trim(vNEWYRCODE) <> "" Then

                                Common_Procedures.CompGroupIdNo = Val(IdNo)
                                Common_Procedures.CompGroupName = Trim(dgv_Details.CurrentRow.Cells(0).Value)
                                Common_Procedures.CompGroupFnRange = Trim(dgv_Details.CurrentRow.Cells(2).Value)
                                Common_Procedures.FnRange = Common_Procedures.CompGroupFnRange
                                Common_Procedures.DataBaseName = Common_Procedures.get_Company_DataBaseName(Trim(Val(IdNo)))

                                Common_Procedures.Connection_String = Common_Procedures.Create_Sql_ConnectionString(Common_Procedures.DataBaseName)


                                If Trim(UCase(Common_Procedures.ServerDataBaseLocation_InExTernalUSB)) = "USB" Then
                                    If Common_Procedures.is_Database_File_Exists(Common_Procedures.DataBaseName) = False Then
                                        MessageBox.Show("Invalid Database File - " & Common_Procedures.DataBaseName, "INVALID COMPANY GROUP SELECTION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                        Exit Sub
                                    End If
                                End If

                                Dim vToYr As Integer = 0
                                vToYr = Val(Microsoft.VisualBasic.Right(Common_Procedures.CompGroupFnRange, 4))
                                Common_Procedures.ChangePeriod_Create_NewYear(Me, vToYr)

                            Else

                                If Val(IdNo) = 0 Then
                                    MessageBox.Show("Select Company Group Name", "INVALID COMPANY GROUP SELECTION....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                    Exit Sub
                                End If

                            End If

                        End If

                    End If

                End If

            End With

        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub Check_Pendrive_in_ServerSystem()
        Dim vLOCKDYS As Integer = -1

        Try

            If Common_Procedures.settings.PENDRIVE_BACKUP_OPTION_NONEED = 1 Then Exit Sub

            If Common_Procedures.Office_System_Status = True Then
                Exit Sub
            End If

            If (Trim(UCase(Common_Procedures.ServerWindowsLogin)) = "IP" Or Trim(UCase(Common_Procedures.ServerWindowsLogin)) = "SIP" Or Trim(UCase(Common_Procedures.ServerWindowsLogin)) = "DIP" Or Trim(UCase(Common_Procedures.ServerWindowsLogin)) = "ONLINE") Then
                Exit Sub
            End If

            vLOCKDYS = GET_NOPENDRIVE_DATE_FROM_SETTINGS()

            If vLOCKDYS > 0 Then

                Dim mymsgbox As New Tsoft_MessageBox("If the pendrive is not connected to a server computer, the software will not open after " & vLOCKDYS & " days", "OK,CANCEL", "FOR SOFTWARE AUTO BACKUP...", "To backup software data automatically, Connect the pendrive to a server computer", MesssageBoxIcons.Exclamations, 1)
                mymsgbox.ShowDialog()

            End If

        Catch ex As Exception
            '---

        End Try

    End Sub

    Private Function GET_NOPENDRIVE_DATE_FROM_SETTINGS() As String
        Dim cn2 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vNOPENDRIVE_STS As Integer
        Dim vNOPENDRIVE_DATE_STR As String
        Dim dttm1 As Date = #1/1/2000#
        Dim vNOPENDRIVE_DTTM As Date = #2/2/2000#
        Dim vDYS As Integer = 10
        Dim lckdt As Date
        Dim vRETDYS As Integer = -1

        cn2.Open()

        vNOPENDRIVE_STS = 0
        vNOPENDRIVE_DATE_STR = ""

        Da = New SqlClient.SqlDataAdapter("select nops, nopd from settings_head Where Autobackup_PenDrive_Path_Server = ''", cn2)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vNOPENDRIVE_STS = Val(Dt.Rows(0)(0).ToString)
            End If
            If IsDBNull(Dt.Rows(0)(1).ToString) = False Then
                vNOPENDRIVE_DATE_STR = Dt.Rows(0)(1).ToString
            End If
        End If
        Dt.Clear()
        Dt.Dispose()
        Da.Dispose()

        cn2.Close()
        cn2.Dispose()

        If vNOPENDRIVE_STS = 1 Then

            If Trim(vNOPENDRIVE_DATE_STR) <> "" Then

                vNOPENDRIVE_DATE_STR = Common_Procedures.Decrypt(vNOPENDRIVE_DATE_STR, Trim(Common_Procedures.ONLINE_OMS_CONNECTION_CHECK.passPhrase), Trim(Common_Procedures.ONLINE_OMS_CONNECTION_CHECK.saltValue))
                If IsDate(vNOPENDRIVE_DATE_STR) = True Then
                    vNOPENDRIVE_DTTM = CDate(vNOPENDRIVE_DATE_STR)
                End If

                If Trim(vNOPENDRIVE_DTTM) <> "" Then
                    If IsDate(vNOPENDRIVE_DTTM) = True Then
                        If DateDiff(DateInterval.Day, dttm1, vNOPENDRIVE_DTTM) > 0 Then
                            lckdt = DateAdd(DateInterval.Day, vDYS, vNOPENDRIVE_DTTM)
                            If DateDiff("d", lckdt.ToShortDateString, Date.Today.ToShortDateString) > 0 Then
                                Dim mymsgbox As New Tsoft_MessageBox("To open the software" & Chr(13) & "Connect the pendrive to a server computer", "OK,CANCEL", "FOR SOFTWARE AUTO BACKUP...", "", MesssageBoxIcons.Errors, 1)
                                mymsgbox.ShowDialog()
                                End
                            Else
                                vRETDYS = DateDiff("d", Date.Today.ToShortDateString, lckdt.ToShortDateString)
                            End If
                        End If
                    End If
                End If

            End If

        End If

        Return vRETDYS

    End Function

    Private Sub Get_ONLINE_DataBase_Details()

        btn_Create.Visible = False
        If Common_Procedures.Office_System_Status = True Then
            btn_Create.Visible = True
        End If

        Common_Procedures.ServerName = ""
        Common_Procedures.ServerLoginID = ""
        Common_Procedures.ServerPassword = ""
        Common_Procedures.ServerDataBaseLocation_InExTernalUSB = ""
        Common_Procedures.SqlServer_PortNumber = ""
        Common_Procedures.CompanyDetailsDataBaseName = ""

        Select Case Trim(UCase(Common_Procedures.Server_ONLine_CCNo))
            Case "--9999--" ' ------------TSOFT SOLUTIONS  - (OMS) 
                Common_Procedures.ServerName = "aruldb-1.ck8snfynsqbs.ap-south-1.rds.amazonaws.com"
                Common_Procedures.ServerLoginID = "arul"
                Common_Procedures.ServerPassword = "Angel_2011"
                Common_Procedures.ServerPassword = Common_Procedures.Encrypt(Trim(Common_Procedures.ServerPassword), Trim(Common_Procedures.Entrance_SQL_PassWord.passPhrase), Trim(Common_Procedures.Entrance_SQL_PassWord.saltValue))
                Common_Procedures.CompanyDetailsDataBaseName = "TSoft_9999_OMS_1"

            Case "--1120--" '----- ALLWIN FABS (or) MARIA INTERNATIONAL (SOMANUR)

                Common_Procedures.ServerName = "armtechnova.ciq6ztquqotv.ap-south-1.rds.amazonaws.com"
                Common_Procedures.ServerLoginID = "arul"
                Common_Procedures.ServerPassword = "Angel_2011"

                'Common_Procedures.ServerName = "system2\tsoft2014"
                'Common_Procedures.ServerLoginID = "sa"
                'Common_Procedures.ServerPassword = "tsoftsql"

                Common_Procedures.ServerPassword = Common_Procedures.Encrypt(Trim(Common_Procedures.ServerPassword), Trim(Common_Procedures.Entrance_SQL_PassWord.passPhrase), Trim(Common_Procedures.Entrance_SQL_PassWord.saltValue))
                Common_Procedures.CompanyDetailsDataBaseName = "tsoft_1120_textile_1"

            Case "1254" '----- SMT FABRICS (MANGALAM-POOMALUR)

                Common_Procedures.ServerName = "armtechnova.ciq6ztquqotv.ap-south-1.rds.amazonaws.com"
                Common_Procedures.ServerLoginID = "arul"
                Common_Procedures.ServerPassword = "Angel_2011"

                Common_Procedures.ServerPassword = Common_Procedures.Encrypt(Trim(Common_Procedures.ServerPassword), Trim(Common_Procedures.Entrance_SQL_PassWord.passPhrase), Trim(Common_Procedures.Entrance_SQL_PassWord.saltValue))
                Common_Procedures.CompanyDetailsDataBaseName = "tsoft_1254_textile_1"

            Case "--1307--" '---- Sri Sugam Textile (Karumanthampatti)
                Common_Procedures.ServerName = "armtechnova.ciq6ztquqotv.ap-south-1.rds.amazonaws.com"
                Common_Procedures.ServerLoginID = "arul"
                Common_Procedures.ServerPassword = "Angel_2011"

                'Common_Procedures.ServerName = "thanges\tsoft2014"
                'Common_Procedures.ServerLoginID = "sa"
                'Common_Procedures.ServerPassword = "tsoftsql"

                Common_Procedures.ServerPassword = Common_Procedures.Encrypt(Trim(Common_Procedures.ServerPassword), Trim(Common_Procedures.Entrance_SQL_PassWord.passPhrase), Trim(Common_Procedures.Entrance_SQL_PassWord.saltValue))
                Common_Procedures.CompanyDetailsDataBaseName = "TSoft_1307_Textile_1"


            Case "1513" '---- NIKHIL TEXTILES   -  (  SRI GRISHMA ENTERPRISES ) (COIMBATORE - NEELAMBUR)

                Common_Procedures.ServerName = "armtechnova.ciq6ztquqotv.ap-south-1.rds.amazonaws.com"
                Common_Procedures.ServerLoginID = "arul"
                Common_Procedures.ServerPassword = "Angel_2011"

                Common_Procedures.ServerPassword = Common_Procedures.Encrypt(Trim(Common_Procedures.ServerPassword), Trim(Common_Procedures.Entrance_SQL_PassWord.passPhrase), Trim(Common_Procedures.Entrance_SQL_PassWord.saltValue))
                Common_Procedures.CompanyDetailsDataBaseName = "tsoft_1513_textile_1"

            Case "1513_2" '---- NIKHIL TEXTILES   -  (  SRI GRISHMA ENTERPRISES ) (COIMBATORE - NEELAMBUR)

                Common_Procedures.ServerName = "armtechnova.ciq6ztquqotv.ap-south-1.rds.amazonaws.com"
                Common_Procedures.ServerLoginID = "arul"
                Common_Procedures.ServerPassword = "Angel_2011"

                Common_Procedures.ServerPassword = Common_Procedures.Encrypt(Trim(Common_Procedures.ServerPassword), Trim(Common_Procedures.Entrance_SQL_PassWord.passPhrase), Trim(Common_Procedures.Entrance_SQL_PassWord.saltValue))
                Common_Procedures.CompanyDetailsDataBaseName = "tsoft_1513_textile_2"

            Case "--1516--" '---- P S ENTERPRISES (KANPUR)
                Common_Procedures.ServerName = "armtechnova.ciq6ztquqotv.ap-south-1.rds.amazonaws.com"
                Common_Procedures.ServerLoginID = "arul"
                Common_Procedures.ServerPassword = "Angel_2011"

                'Common_Procedures.ServerName = "system2\tsoft2014"
                'Common_Procedures.ServerLoginID = "sa"
                'Common_Procedures.ServerPassword = "tsoftsql"

                Common_Procedures.ServerPassword = Common_Procedures.Encrypt(Trim(Common_Procedures.ServerPassword), Trim(Common_Procedures.Entrance_SQL_PassWord.passPhrase), Trim(Common_Procedures.Entrance_SQL_PassWord.saltValue))
                Common_Procedures.CompanyDetailsDataBaseName = "tsoft_1516_textile_1"

            Case "--1520--" '----- SRI RAINBOW COTTON FABRIC (KARUR)

                Common_Procedures.ServerName = "armtechnova.ciq6ztquqotv.ap-south-1.rds.amazonaws.com"
                Common_Procedures.ServerLoginID = "arul"
                Common_Procedures.ServerPassword = "Angel_2011"

                'Common_Procedures.ServerName = "thanges\tsoft2014"
                'Common_Procedures.ServerLoginID = "sa"
                'Common_Procedures.ServerPassword = "tsoftsql"

                Common_Procedures.ServerPassword = Common_Procedures.Encrypt(Trim(Common_Procedures.ServerPassword), Trim(Common_Procedures.Entrance_SQL_PassWord.passPhrase), Trim(Common_Procedures.Entrance_SQL_PassWord.saltValue))
                Common_Procedures.CompanyDetailsDataBaseName = "tsoft_1520_textile_1"


            Case "1592" '----- LAKSHANA SHREE TEX (POOMALAR)

                Common_Procedures.ServerName = "armtechnova.ciq6ztquqotv.ap-south-1.rds.amazonaws.com"
                Common_Procedures.ServerLoginID = "arul"
                Common_Procedures.ServerPassword = "Angel_2011"

                Common_Procedures.ServerPassword = Common_Procedures.Encrypt(Trim(Common_Procedures.ServerPassword), Trim(Common_Procedures.Entrance_SQL_PassWord.passPhrase), Trim(Common_Procedures.Entrance_SQL_PassWord.saltValue))
                Common_Procedures.CompanyDetailsDataBaseName = "tsoft_1592_textile_1"

            Case "1118" '----- KASTUR LAXMI MILLS (COIMBATORE)

                Common_Procedures.ServerName = "armtechnova.ciq6ztquqotv.ap-south-1.rds.amazonaws.com"
                Common_Procedures.ServerLoginID = "arul"
                Common_Procedures.ServerPassword = "Angel_2011"

                Common_Procedures.ServerPassword = Common_Procedures.Encrypt(Trim(Common_Procedures.ServerPassword), Trim(Common_Procedures.Entrance_SQL_PassWord.passPhrase), Trim(Common_Procedures.Entrance_SQL_PassWord.saltValue))
                Common_Procedures.CompanyDetailsDataBaseName = "tsoft_1118_textile_1"

        End Select

    End Sub

    Private Sub Get_ONLINE_OMS_DataBase_Details()

        Exit Sub

        'MsgBox("START-1")

        If Common_Procedures.settings.NO_INTERNET_CONNECTION = 1 Then
            Check_oocdt() '----Check ONLICE OMS CONNECTION DATETIME
            Exit Sub
        End If

        'MsgBox("2")

        'If Common_Procedures.Office_System_Status = True Then
        '    Exit Sub
        'End If

        Try

            If My.Computer.Network.IsAvailable = True Then

                'If My.Computer.Network.Ping("www.Google.com") = True Then

                'MsgBox("3")

                Common_Procedures.OMS_ServerName = "armtechnova.ciq6ztquqotv.ap-south-1.rds.amazonaws.com"
                'Common_Procedures.ServerName = "aruldb-1.ck8snfynsqbs.ap-south-1.rds.amazonaws.com"
                Common_Procedures.OMS_ServerLoginID = "arul"
                Common_Procedures.OMS_ServerPassword = "Angel_2011"
                'Common_Procedures.OMS_ServerPassword = Common_Procedures.Encrypt(Trim(Common_Procedures.ServerPassword), Trim(Common_Procedures.Entrance_SQL_PassWord.passPhrase), Trim(Common_Procedures.Entrance_SQL_PassWord.saltValue))
                Common_Procedures.OMS_ServerWindowsLogin = ""
                Common_Procedures.OMS_SqlServer_PortNumber = ""
                Common_Procedures.OMS_DataBaseName = "TSoft_9999_OMS_1"

                If Val(Common_Procedures.OMS_SqlServer_PortNumber) = 0 Then Common_Procedures.OMS_SqlServer_PortNumber = "1433"
                If Trim(Common_Procedures.OMS_ServerLoginID) = "" Then Common_Procedures.OMS_ServerLoginID = "sa"
                Common_Procedures.OMS_Connection_String = "Data Source=" & Trim(Common_Procedures.OMS_ServerName) & "," & Trim(Val(Common_Procedures.OMS_SqlServer_PortNumber)) & ";Network Library=DBMSSOCN;Initial Catalog=" & Trim(Common_Procedures.OMS_DataBaseName) & ";User ID=" & Trim(Common_Procedures.OMS_ServerLoginID) & ";Password=" & Trim(Common_Procedures.OMS_ServerPassword) & ";Integrated Security=False;Connect Timeout=60"

                Try

                    Dim cn1 As SqlClient.SqlConnection
                    cn1 = New SqlClient.SqlConnection(Common_Procedures.OMS_Connection_String)
                    cn1.Open()

                    'MsgBox("4")

                    '----------OMS CODING HERE

                    Checkol(cn1)

                    'MsgBox("5")

                    cn1.Close()
                    cn1.Dispose()

                    Update_ooccdt()

                    'MsgBox("6")

                Catch ex As Exception

                    'MsgBox("7")
                    Check_oocdt() '----Check ONLICE OMS CONNECTION DATETIME
                    'MsgBox("8")

                End Try

                'Else
                '    MsgBox("9")
                '    Check_oocdt() '----Check ONLICE OMS CONNECTION DATETIME
                '    MsgBox("10")

                'End If

            Else
                'MsgBox("11")
                Check_oocdt() '----Check ONLICE OMS CONNECTION DATETIME
                'MsgBox("12")

            End If


        Catch ex As Exception
            'MsgBox("13")
            Check_oocdt() '----Check ONLICE OMS CONNECTION DATETIME
            'MsgBox("14")

        End Try



    End Sub

    Private Sub Update_ooccdt() '----Update ONLINE OMS CONNECTION CHECK DATETIME
        Dim cn2 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
        Dim cmd As New SqlClient.SqlCommand
        Dim Nr As Integer
        Dim vOCCDATE_STR As String

        Try

            'MsgBox("201-START")

            cn2.Open()

            cmd.Connection = cn2

            'MsgBox("202")

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@sysdate", Now)

            'MsgBox("203")

            vOCCDATE_STR = Common_Procedures.Encrypt(Now, Trim(Common_Procedures.ONLINE_OMS_CONNECTION_CHECK.passPhrase), Trim(Common_Procedures.ONLINE_OMS_CONNECTION_CHECK.saltValue))

            'MsgBox("204-" & Trim(vOCCDATE_STR))

            Nr = 0
            cmd.CommandText = "update settings_head set ooccd = '" & Trim(vOCCDATE_STR) & "'"
            Nr = cmd.ExecuteNonQuery()

            'MsgBox("205")

            If Nr = 0 Then

                'MsgBox("206")

                cmd.CommandText = "insert into settings_head (ooccd) values ('" & Trim(vOCCDATE_STR) & "')"
                cmd.ExecuteNonQuery()

                'MsgBox("207")

            End If

            cmd.Dispose()

            cn2.Close()
            cn2.Dispose()

            'MsgBox("208")

        Catch ex As Exception
            'MsgBox("209-END")
            MessageBox.Show(ex.Message, "ERROR IN UPDATING OOCCDT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)


        End Try

    End Sub

    Private Sub Check_oocdt() '----Check ONLICE OMS CONNECTION DATETIME
        Dim cn3 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
        Dim lckdt As Date
        Dim vOOCCDTTM As Date = #2/2/2000#
        Dim dttm1 As Date = #1/1/2000#
        Dim vDYS As Integer = 0
        Dim vONLICE_OMS_CONN_CHECK_DATETIME As String = ""

        'MsgBox("101")

        vDYS = 90
        If Common_Procedures.settings.NO_INTERNET_CONNECTION = 1 Then vDYS = 300

        cn3.Open()
        vONLICE_OMS_CONN_CHECK_DATETIME = Common_Procedures.get_FieldValue(cn3, "settings_head", "ooccd", "")
        cn3.Close()
        cn3.Dispose()

        'MsgBox("102")

        If Trim(vONLICE_OMS_CONN_CHECK_DATETIME) <> "" Then

            'MsgBox("103")

            vONLICE_OMS_CONN_CHECK_DATETIME = Common_Procedures.Decrypt(vONLICE_OMS_CONN_CHECK_DATETIME, Trim(Common_Procedures.ONLINE_OMS_CONNECTION_CHECK.passPhrase), Trim(Common_Procedures.ONLINE_OMS_CONNECTION_CHECK.saltValue))

            'MsgBox("104")

            If IsDate(vONLICE_OMS_CONN_CHECK_DATETIME) = True Then
                vOOCCDTTM = CDate(vONLICE_OMS_CONN_CHECK_DATETIME)
                'MsgBox("105")
            End If

            'MsgBox("106")

        Else
            Update_ooccdt()
            Exit Sub

        End If

        'MsgBox("107")

        If IsDate(vOOCCDTTM) = True Then
            If DateDiff(DateInterval.Day, dttm1, vOOCCDTTM) > 0 Then
                lckdt = DateAdd(DateInterval.Day, vDYS, vOOCCDTTM)
                If DateDiff("d", lckdt.ToShortDateString, Date.Today.ToShortDateString) > 0 Then

                    'MsgBox("108 - " & lckdt.ToShortDateString)

                    Dim vERRMSG As String = "A network - related Or instance - specific error occurred while establishing a connection to SQL Server. The server was Not found Or was Not accessible. Verify that the instance name Is correct And that the SQL Server Is configured to allow remote connections. (provided: Named Pipes Provider, error: 40- Could Not open a connection to the SQL Server) (Microsoft SQL Server, Error: 2)."
                    MessageBox.Show(vERRMSG, "TSOFT TEXTILE...", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error)
                    End
                End If
            End If

        Else
            Update_ooccdt()

        End If

    End Sub

    Private Sub Checkol(ByVal Cn3 As SqlClient.SqlConnection) '----Check Online Lock
        Dim cn2 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim vACCNO As String, vCCNO_ENCRYP_STR As String
        Dim vEXEDttm As DateTime
        Dim dttm1 As Date = #11/30/2022#
        Dim lckdt As Date = #1/1/2000#
        Dim Nr As Long

        'MsgBox("301-start")

        cn2.Open()

        vACCNO = 0
        vCCNO_ENCRYP_STR = ""

        Da = New SqlClient.SqlDataAdapter("Select accno from settings_head", cn2)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                vCCNO_ENCRYP_STR = Dt.Rows(0)(0).ToString
            End If
        End If
        Dt.Clear()


        vACCNO = ""

        If Trim(vCCNO_ENCRYP_STR) <> "" Then

            'MsgBox("302")

            vACCNO = Common_Procedures.Decrypt(vCCNO_ENCRYP_STR, Trim(Common_Procedures.ACTUAL_CUSTOMER_CODE_NUMBER.passPhrase), Trim(Common_Procedures.ACTUAL_CUSTOMER_CODE_NUMBER.saltValue))

            If Trim(vACCNO) <> "" Then

                cmd.Connection = Cn3

                'MsgBox("303")

                '-----------------------------------------------------------------

                vEXEDttm = File.GetLastWriteTime(Application.ExecutablePath)

                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@ExeDate", vEXEDttm)

                cmd.Parameters.AddWithValue("@sysdatetime", Now)


                '---- Customer_System_name_Details
                cmd.CommandText = "delete from CSND Where  accno = '" & Trim(vACCNO) & "' and Soft_IdNo  = " & Str(Val(Common_Procedures.Software_IdNo)) & " and Computer_SerialNo = '" & Trim(vCSND_vSysSlNo) & "'"
                Nr = cmd.ExecuteNonQuery()

                cmd.CommandText = "delete from CSND Where accno = '" & Trim(vACCNO) & "' and Soft_IdNo  = " & Str(Val(Common_Procedures.Software_IdNo)) & " and Computer_name = '" & Trim(vCSND_vSYSNm) & "'"
                Nr = cmd.ExecuteNonQuery()


                cmd.CommandText = "Insert into CSND (            accno      ,                                Soft_IdNo           ,                Type           ,         Sql_Instance_name   ,          Sql_Data_path       ,      Computer_name          ,      Computer_SerialNo        ,           Software_Exe_Name     , Exe_Date_Time ,                       Software_Path       , Last_Opened_SystemDateTime ) " &
                              " Values              ( '" & Trim(vACCNO) & "',  " & Str(Val(Common_Procedures.Software_IdNo)) & " , '" & Trim(vCSND_servtype) & "', '" & Trim(vCSND_InstNm) & "', '" & Trim(vCSND_DefPath) & "', '" & Trim(vCSND_vSYSNm) & "', '" & Trim(vCSND_vSysSlNo) & "', '" & Trim(vCSND_vExeFileNm) & "',     @ExeDate  , '" & Trim(Common_Procedures.AppPath) & "' ,          @sysdatetime      ) "
                cmd.ExecuteNonQuery()

                '------------------------------------------------------------------------

                Da = New SqlClient.SqlDataAdapter("select CLD from CLSH Where accno = '" & Trim(vACCNO) & "'", Cn3)  '---Customer Lock Settings Head
                Dt = New DataTable
                Da.Fill(Dt)

                'MsgBox("304")

                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        lckdt = Dt.Rows(0)(0)

                        If IsDate(lckdt) = True Then

                            If DateDiff(DateInterval.Day, dttm1, lckdt) > 0 Then

                                If DateDiff(DateInterval.Minute, lckdt, Now) > 0 Then

                                    Dim vERRMSG As String = "A network - related Or instance - specific error occurred while establishing a connection to SQL Server. The server was Not found Or was Not accessible. Verify that the instance name Is correct And that the SQL Server Is configured to allow remote connections. (provided: Named Pipes Provider, error: 41- Could Not open a connection to the SQL Server) (Microsoft SQL Server, Error: 3)."
                                    MessageBox.Show(vERRMSG, "TSOFT TEXTILE...", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error)
                                    End
                                End If

                            End If

                        End If

                    End If

                End If
                Dt.Clear()

                'MsgBox("305")

            End If

        End If

        Dt.Dispose()
        Da.Dispose()

        cn2.Close()
        cn2.Dispose()

        'MsgBox("306-end")

    End Sub

End Class
