Imports Newtonsoft.Json
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.IO
Imports System.Linq
Imports System.Net
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports TaxProEWB.API
Imports System.Diagnostics
Imports Microsoft.VisualBasic.ApplicationServices
Imports System.Net.Mime.MediaTypeNames

Public Class GST_EWB_API_SETTINGS
    Implements Interface_MDIActions

    Public WithEvents EwbSession As EWBSession = New EWBSession()

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)

    Private Prec_ActCtrl As New Control
    Dim New_Entry As Boolean
    Dim ClientId As String
    Dim ClientSecret As String
    Dim GSPUserId As String
    Dim AppKey As String
    Dim AuthToken As String
    Dim TokenExp As String
    Dim SEK As String

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12


        'DisplayApiSettings()
        'DisplayApiLoginDetails()

        'Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub txtGSPName_KeyDown(sender As Object, e As KeyEventArgs) Handles txtGSPName.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txtGSPName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtGSPName.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtASPUserID_TextChanged(sender As Object, e As EventArgs) Handles txtASPUserID.TextChanged

    End Sub

    Private Sub txtASPUserID_KeyDown(sender As Object, e As KeyEventArgs) Handles txtASPUserID.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txtASPPassword_TextChanged(sender As Object, e As EventArgs) Handles txtASPPassword.TextChanged

    End Sub

    Private Sub txtASPPassword_KeyDown(sender As Object, e As KeyEventArgs) Handles txtASPPassword.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txtBaseURL_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtBaseURL.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtBaseURL_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBaseURL.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txtEWBUserID_KeyDown(sender As Object, e As KeyEventArgs) Handles txtEWBUserID.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txtEWBPassword_KeyDown(sender As Object, e As KeyEventArgs) Handles txtEWBPassword.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txtASPUserID_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtASPUserID.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtASPPassword_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtASPPassword.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtEWBUserID_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtEWBUserID.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtEWBPassword_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtEWBPassword.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub GST_EWB_API_SETTINGS_Load(sender As Object, e As EventArgs) Handles Me.Load

        con.Open()

        lblASPUserID_Caption.Visible = False
        txtASPUserID.Visible = False

        lblASPPassword_Caption.Visible = False
        txtASPPassword.Visible = False

        lbl_EInvoiceGSPName_Caption.Visible = False
        txt_EInvoiceGSPName.Visible = False

        lbl_EIAuthURL_Caption.Visible = False
        txtEIAuthURL.Visible = False

        lbl_EIBaseURL_Caption.Visible = False
        txtEIBaseURL.Visible = False

        lbl_EWBIRNURL_Caption.Visible = False
        txtEWBIRNURL.Visible = False

        lbl_GSPName_Caption.Visible = False
        txtGSPName.Visible = False

        lbl_BaseURL_Caption.Visible = False
        txtBaseURL.Visible = False

        lbl_CancelEWBURL_Caption.Visible = False
        txtCancelEWBURL.Visible = False

        If Common_Procedures.is_OfficeSystem = True Then

            lblASPUserID_Caption.Visible = True
            txtASPUserID.Visible = True

            lblASPPassword_Caption.Visible = True
            txtASPPassword.Visible = True

            lbl_EInvoiceGSPName_Caption.Visible = True
            txt_EInvoiceGSPName.Visible = True

            lbl_EIAuthURL_Caption.Visible = True
            txtEIAuthURL.Visible = True

            lbl_EIBaseURL_Caption.Visible = True
            txtEIBaseURL.Visible = True

            lbl_EWBIRNURL_Caption.Visible = True
            txtEWBIRNURL.Visible = True

            lbl_GSPName_Caption.Visible = True
            txtGSPName.Visible = True

            lbl_BaseURL_Caption.Visible = True
            txtBaseURL.Visible = True

            lbl_CancelEWBURL_Caption.Visible = True
            txtCancelEWBURL.Visible = True

        End If


        AddHandler txtASPPassword.GotFocus, AddressOf ControlGotFocus
        AddHandler txtASPUserID.GotFocus, AddressOf ControlGotFocus
        AddHandler txtBaseURL.GotFocus, AddressOf ControlGotFocus
        AddHandler txtEWBPassword.GotFocus, AddressOf ControlGotFocus
        AddHandler txtEWBUserID.GotFocus, AddressOf ControlGotFocus
        AddHandler txtGSPName.GotFocus, AddressOf ControlGotFocus
        AddHandler txtEIUserID.GotFocus, AddressOf ControlGotFocus
        AddHandler txtEIPassword.GotFocus, AddressOf ControlGotFocus
        AddHandler txtEIAuthURL.GotFocus, AddressOf ControlGotFocus
        AddHandler txtEIBaseURL.GotFocus, AddressOf ControlGotFocus
        AddHandler txtEWBIRNURL.GotFocus, AddressOf ControlGotFocus
        AddHandler txtCancelEWBURL.GotFocus, AddressOf ControlGotFocus

        AddHandler txtASPPassword.LostFocus, AddressOf ControlLostFocus
        AddHandler txtASPUserID.LostFocus, AddressOf ControlLostFocus
        AddHandler txtBaseURL.LostFocus, AddressOf ControlLostFocus
        AddHandler txtEWBPassword.LostFocus, AddressOf ControlLostFocus
        AddHandler txtEWBUserID.LostFocus, AddressOf ControlLostFocus
        AddHandler txtGSPName.LostFocus, AddressOf ControlLostFocus
        AddHandler txtEIUserID.LostFocus, AddressOf ControlLostFocus
        AddHandler txtEIPassword.LostFocus, AddressOf ControlLostFocus
        AddHandler txtEIAuthURL.LostFocus, AddressOf ControlLostFocus
        AddHandler txtEIBaseURL.LostFocus, AddressOf ControlLostFocus
        AddHandler txtEWBIRNURL.LostFocus, AddressOf ControlLostFocus
        AddHandler txtCancelEWBURL.LostFocus, AddressOf ControlLostFocus

        'Me.Width = 526
        'Me.Height = 615

        movefirst_record()

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            Prec_ActCtrl.BackColor = Color.White
            Prec_ActCtrl.ForeColor = Color.Black
        End If

    End Sub

    Private Sub clear()

        Dim obj As Object
        Dim ctrl As Object
        Dim grpbx As GroupBox
        Dim pnlbx As Panel

        For Each obj In Me.Controls
            If TypeOf obj Is TextBox Then
                obj.text = ""
            ElseIf TypeOf obj Is ComboBox Then
                obj.text = ""
            ElseIf TypeOf obj Is GroupBox Then
                grpbx = obj
                For Each ctrl In grpbx.Controls
                    If TypeOf ctrl Is TextBox Then
                        ctrl.text = ""
                    ElseIf TypeOf ctrl Is ComboBox Then
                        ctrl.text = ""
                    End If

                Next

            ElseIf TypeOf obj Is Panel Then
                pnlbx = obj
                For Each ctrl In pnlbx.Controls
                    If TypeOf ctrl Is TextBox Then
                        ctrl.text = ""
                    ElseIf TypeOf ctrl Is ComboBox Then
                        ctrl.text = ""
                    End If

                Next

            End If

        Next

        New_Entry = False



    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record

        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Ledger_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Ledger_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub


        If MessageBox.Show("Do you want to delete", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        Try

            cmd.CommandText = "delete from GST_EWB_API_Settings where GSTIN = " & Str(txt_GSTIN.Text)
            cmd.ExecuteNonQuery()

            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Successfully!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE..", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
            Exit Sub

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        '---
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim dt As New DataTable
        Dim nr As Long = 0


        If Common_Procedures.UserRight_Check(Common_Procedures.UR.Ledger_Creation, New_Entry) = False Then Exit Sub

        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid Legder Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled Then txt_Name.Focus()
            Exit Sub
        End If


        trans = con.BeginTransaction

        Try


            cmd.Transaction = trans

            cmd.Connection = con

            'WHERE GSTIN = '" & txt_GSTIN.Text & "'

            cmd.CommandText = "DELETE FROM GST_EWB_API_Settings Where Company_IdNo = " & Val(txt_IdNo.Text).ToString
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into GST_EWB_API_Settings Values ('" & txtGSPName.Text & "', '" & txtASPUserID.Text & "', '" & txtASPPassword.Text & "',  '" & Trim(txtBaseURL.Text) & "','" & Trim(txt_GSTIN.Text) & "','" & Trim(txtEWBUserID.Text) & "', '" & Trim(txtEWBPassword.Text) & "'," &
                                                                         "'" & txtEIUserID.Text & "','" & txtEIPassword.Text & "','" & txtEIAuthURL.Text & "','" & txtEIBaseURL.Text & "','" & txtEWBIRNURL.Text & "','" & txtCancelEWBURL.Text & "','" & txt_EInvoiceGSPName.Text & "'," & Val(txt_IdNo.Text).ToString & " )"
            'MsgBox(cmd.CommandText)
            cmd.ExecuteNonQuery()

            trans.Commit()

            trans.Dispose()
            dt.Dispose()

            move_record(Val(txt_IdNo.Text))

            MessageBox.Show("Successfully Saved", "For SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception

            trans.Rollback()

            MessageBox.Show(ex.Message, "DOES Not SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Exit Sub

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Throw New NotImplementedException()
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Throw New NotImplementedException()
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Throw New NotImplementedException()
    End Sub

    ' Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
    'Throw New NotImplementedException()
    'End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record

        Dim cmd As New SqlClient.SqlCommand
        cmd.Connection = con



        cmd.CommandText = "Select min(company_idno) from company_head where  company_idno <> 0 And company_idno > " & Str(Val(txt_IdNo.Text)) & " And len(COMPANY_GSTINNo) = 15 "


        Dim movid As Integer = 0

        Try
            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar
            End If

            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "For MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record

        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer = 0

        Try
            cmd.Connection = con


            cmd.CommandText = "Select max(company_idno ) from company_head where  company_idno <> 0 And company_idno < " & Str((txt_IdNo.Text)) & " And len(COMPANY_GSTINNo) = 15 "


            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar()
            End If

            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "For MOVING....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record

        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer

        Try
            cmd.Connection = con

            cmd.CommandText = "Select max(Company_IdNo) from Company_head  WHERE COMPANY_IDNO > 0  And len(COMPANY_GSTINNo) = 15 "

            movid = 0

            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar
            End If

            cmd.Dispose()

            If movid <> 0 Then Call move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "For MOVING....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Throw New NotImplementedException()
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record

        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer

        Try
            cmd.Connection = con


            cmd.CommandText = "Select min(Company_IdNo) from Company_head WHERE COMPANY_IDNO > 0  And len(COMPANY_GSTINNo) = 15 "

            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar
            End If

            cmd.Dispose()

            If movid <> 0 Then Call move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "For MOVING....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub move_record(ByVal idno As Integer)

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim SNo As Integer = 0
        Dim n As Integer = 0
        If Val(idno) = 0 Then Exit Sub

        'clear()

        txt_IdNo.Text = ""
        txt_Name.Text = ""
        txt_ShortName.Text = ""
        txt_GSTIN.Text = ""

        da = New SqlClient.SqlDataAdapter("Select C.Company_IdNo,C.Company_Name,C.Company_ShortName,C.Company_GSTINNo,G.* from Company_Head C Left Outer Join GST_EWB_API_Settings G On C.Company_IdNo = G.Company_IdNo Where C.Company_IdNo = " & idno.ToString & " And len(C.COMPANY_GSTINNo) = 15 ", con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then

            txt_IdNo.Text = dt.Rows(0).Item("Company_IdNo")
            txt_Name.Text = dt.Rows(0).Item("Company_Name")
            txt_ShortName.Text = dt.Rows(0).Item("Company_ShortName")
            txt_GSTIN.Text = dt.Rows(0).Item("Company_GSTINNo")

            If Not IsDBNull(dt.Rows(0).Item("GSPNAME")) Then
                txtGSPName.Text = dt.Rows(0).Item("GSPNAME")
            End If
            If Not IsDBNull(dt.Rows(0).Item("ASPUSERID")) Then
                txtASPUserID.Text = dt.Rows(0).Item("ASPUSERID")
            End If
            If Not IsDBNull(dt.Rows(0).Item("ASPPASSWORD")) Then
                txtASPPassword.Text = dt.Rows(0).Item("ASPPASSWORD")
            End If
            If Not IsDBNull(dt.Rows(0).Item("BASEURL")) Then
                txtBaseURL.Text = dt.Rows(0).Item("BASEURL")
            End If
            If Not IsDBNull(dt.Rows(0).Item("EWBUSERID")) Then
                txtEWBUserID.Text = dt.Rows(0).Item("EWBUSERID")
            End If
            If Not IsDBNull(dt.Rows(0).Item("EWBPASSWORD")) Then
                txtEWBPassword.Text = dt.Rows(0).Item("EWBPASSWORD")
            End If
            If Not IsDBNull(dt.Rows(0).Item("eInvoice_UserId")) Then
                txtEIUserID.Text = dt.Rows(0).Item("eInvoice_UserId")
            End If
            If Not IsDBNull(dt.Rows(0).Item("eInvoice_Password")) Then
                txtEIPassword.Text = dt.Rows(0).Item("eInvoice_Password")
            End If
            If Not IsDBNull(dt.Rows(0).Item("eInvoice_AuthURL")) Then
                txtEIAuthURL.Text = dt.Rows(0).Item("eInvoice_AuthURL")
            End If
            If Not IsDBNull(dt.Rows(0).Item("eInvoice_BaseURL")) Then
                txtEIBaseURL.Text = dt.Rows(0).Item("eInvoice_BaseURL")
            End If
            If Not IsDBNull(dt.Rows(0).Item("eInvoice_EWBURL")) Then
                txtEWBIRNURL.Text = dt.Rows(0).Item("eInvoice_EWBURL")
            End If
            If Not IsDBNull(dt.Rows(0).Item("eInvoice_CancelEWBURL")) Then
                txtCancelEWBURL.Text = dt.Rows(0).Item("eInvoice_CancelEWBURL")
            End If
            If Not IsDBNull(dt.Rows(0).Item("e_Invoice_GSPName")) Then
                txt_EInvoiceGSPName.Text = dt.Rows(0).Item("e_Invoice_GSPName")
            End If

        End If

    End Sub

    Private Sub DisplayApiSettings()

        txtGSPName.Text = EwbSession.EwbApiSetting.GSPName
        txtASPUserID.Text = EwbSession.EwbApiSetting.AspUserId
        txtASPPassword.Text = EwbSession.EwbApiSetting.AspPassword
        ClientId = EwbSession.EwbApiSetting.EWBClientId
        ClientSecret = EwbSession.EwbApiSetting.EWBClientSecret
        GSPUserId = EwbSession.EwbApiSetting.EWBGSPUserID
        txtBaseURL.Text = EwbSession.EwbApiSetting.BaseUrl

    End Sub

    Private Sub DisplayApiLoginDetails()

        txt_GSTIN.Text = EwbSession.EwbApiLoginDetails.EwbGstin
        txtEWBUserID.Text = EwbSession.EwbApiLoginDetails.EwbUserID
        txtEWBPassword.Text = EwbSession.EwbApiLoginDetails.EwbPassword
        AppKey = EwbSession.EwbApiLoginDetails.EwbAppKey
        AuthToken = EwbSession.EwbApiLoginDetails.EwbAuthToken
        TokenExp = EwbSession.EwbApiLoginDetails.EwbTokenExp.ToString("dd/MM/yyyy HH:mm:ss")
        SEK = EwbSession.EwbApiLoginDetails.EwbSEK

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        save_record()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub txtEIAuthURL_TextChanged(sender As Object, e As EventArgs) Handles txtEIAuthURL.TextChanged

    End Sub

    Private Sub txtEIAuthURL_KeyDown(sender As Object, e As KeyEventArgs) Handles txtEIAuthURL.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txtEIBaseURL_TextChanged(sender As Object, e As EventArgs) Handles txtEIBaseURL.TextChanged

    End Sub

    Private Sub txtEIBaseURL_KeyDown(sender As Object, e As KeyEventArgs) Handles txtEIBaseURL.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txtEWBIRNURL_TextChanged(sender As Object, e As EventArgs) Handles txtEWBIRNURL.TextChanged

    End Sub

    Private Sub txtEWBIRNURL_KeyDown(sender As Object, e As KeyEventArgs) Handles txtEWBIRNURL.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txtCancelEWBURL_TextChanged(sender As Object, e As EventArgs) Handles txtCancelEWBURL.TextChanged

    End Sub

    Private Sub txtCancelEWBURL_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCancelEWBURL.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txtEIUserID_TextChanged(sender As Object, e As EventArgs) Handles txtEIUserID.TextChanged

    End Sub

    Private Sub txtEIUserID_KeyDown(sender As Object, e As KeyEventArgs) Handles txtEIUserID.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txtEIPassword_TextChanged(sender As Object, e As EventArgs) Handles txtEIPassword.TextChanged

    End Sub

    Private Sub txtEIPassword_KeyDown(sender As Object, e As KeyEventArgs) Handles txtEIPassword.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txtEIAuthURL_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtEIAuthURL.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtEIBaseURL_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtEIBaseURL.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtEWBIRNURL_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtEWBIRNURL.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtCancelEWBURL_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCancelEWBURL.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtEIUserID_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtEIUserID.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtEIPassword_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtEIPassword.KeyPress
        If Asc(e.KeyChar) = 13 Then save_record()
    End Sub

    Private Sub btn_CheckConnectivity_Click(sender As Object, e As EventArgs) Handles btn_CheckConnectivity.Click
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim einv As New eInvoice(Val(txt_IdNo.Text))
        einv.GetAuthToken(rtbeInvoiceResponse)

    End Sub

    Private Sub btn_CheckConnectivity1_Click(sender As Object, e As EventArgs) Handles btn_CheckConnectivity1.Click

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim ewb As New EWB(Val(txt_IdNo.Text))
        EWB.GetAuthToken(rtbEWBResponse)
    End Sub

    Private Sub btn_RestoreDefault_Click(sender As Object, e As EventArgs) Handles btn_RestoreDefault.Click

        txtASPUserID.Text = "1612689919"
        txtASPPassword.Text = "Ruth@2009"

        txt_EInvoiceGSPName.Text = "TaxPro_Production"
        txtEIAuthURL.Text = "https://einvapi.charteredinfo.com/eivital/v1.04"
        txtEIBaseURL.Text = "https://einvapi.charteredinfo.com/eicore/v1.04"
        txtEWBIRNURL.Text = "https://einvapi.charteredinfo.com/eiewb/v1.03"

        txtGSPName.Text = "TaxPro GSP"
        txtBaseURL.Text = "https://einvapi.charteredinfo.com/v1.04"
        txtCancelEWBURL.Text = "https://einvapi.charteredinfo.com/v1.03"
        'txtBaseURL.Text = "https://api.taxprogsp.co.in/v1.03"
        'txtCancelEWBURL.Text = "https://api.taxprogsp.co.in/v1.03"

        If Trim(UCase(txt_GSTIN.Text)) = "33AACCC1596Q002" Then '---- SANDBOX ACCOUNT ( TESTING ACCOUNT )

            '----SANDBOX GSTIN : 33AACCC1596Q002  - Name : SANDBOX ACCOUNT

            txtEIUserID.Text = "TaxProEnvTN"
            txtEIPassword.Text = "abc33*"

            txtEWBUserID.Text = "TaxProEnvTN"
            txtEWBPassword.Text = "abc33*"

            txtASPUserID.Text = "1612689919"
            txtASPPassword.Text = "Ruth@2009"

            txt_EInvoiceGSPName.Text = "TaxPro_Sandbox"
            txtEIAuthURL.Text = "https://gstsandbox.charteredinfo.com/eivital/v1.04"
            txtEIBaseURL.Text = "https://gstsandbox.charteredinfo.com/eicore/v1.04"
            txtEWBIRNURL.Text = "https://gstsandbox.charteredinfo.com/eiewb/v1.03"

            txtGSPName.Text = "TaxPro_Sandbox"
            txtBaseURL.Text = "https://gstsandbox.charteredinfo.com/v1.04"
            txtCancelEWBURL.Text = "https://gstsandbox.charteredinfo.com/v1.03"
            'txtBaseURL.Text = "https://api.taxprogsp.co.in/v1.03"
            'txtCancelEWBURL.Text = "https://api.taxprogsp.co.in/v1.03"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "9999" Then '---- ARANI(TIRUPUR)
            '----GSTIN : 33AAAFF5557D1ZS  - Name : ARNAI EXPORT - SANDBOX ACCOUNT
            txtEIUserID.Text = "Araniexport"
            txtEIPassword.Text = "Araniexport@1234"

            txtEWBUserID.Text = "Araniexport"
            txtEWBPassword.Text = "Araniexport@1234"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1005" Then '---- Jeno Textiles (Somanur)

            If Trim(UCase(txt_GSTIN.Text)) = "33AEGPA7347Q1Z7" Then '---GSTIN : 33AEGPA7347Q1Z7 - Name : ANNAI TEX

                txtEIUserID.Text = "Annaitex_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "Annaitex_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            Else

                txtEIUserID.Text = "JenoTextil_API_TSS"        '---GSTIN : _______________ - Name : JENO TEX
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "JenoTextil_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1033" Then '---- Rajeswari Weaving (Karumanthapatti) - Somanur Sizing


            If Trim(UCase(txt_GSTIN.Text)) = "33AAJFR9651B1Z8" Then '---RAJESWARI WEAVING MILL
                txtEIUserID.Text = "API_RAJESWARI_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_RAJESWARI_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33BFRPJ9704F1ZA" Then '---RAJESWARI WOVENS
                txtEIUserID.Text = "API_RajiWovens_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_RajiWovens_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If



        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1035" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Then '---- Kalaimagal Textiles (Avinashi)

            If Trim(UCase(txt_GSTIN.Text)) = "33AAEFB5497N1Z0" Then 'GSTIN:33AAEFB5497N1Z0 - Name: BAALAJI TEXTILE MILLS
                txtEIUserID.Text = "API_BAALAJIAVI_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_BAALAJIAVI_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "07AAEFB5497N1ZV" Then 'GSTIN:07AAEFB5497N1ZV - Name: BAALAJI TEXTILE MILLS

                txtEIUserID.Text = "API_BAALAJIDEL_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_BAALAJIDEL_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            Else
                txtEIUserID.Text = "API_KALAI_AVI_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_KALAI_AVI_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then '---- Ganesh karthick Textiles (p) Ltd (Somanur)
            If Trim(UCase(txt_GSTIN.Text)) = "33AABCG5806G1ZY" Then
                '----GSTIN : 33AABCG5806G1ZY - Name : GANESH KARTHI TEXTILE PRIVATE LIMITED
                txtEIUserID.Text = "API_GANESHKT_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_GANESHKT_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AABHJ3672C1ZP" Then
                'GSTIN:33AABHJ3672C1ZP - Name: KAVERY INDUSTRIES

                txtEIUserID.Text = "API_KAVERY_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_KAVERY_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AAHFR0771J1Z6" Then
                ''''''''GSTIN :33AAHFR0771J1Z6- Name : ROHINI FABRICS - User : Tax Payer

                'txtEIUserID.Text = ""
                'txtEIPassword.Text = ""

                txtEWBUserID.Text = "rohinifabr_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1063" Then ' ------------Mahadev Agency (somanur) / SARASWATHI FABRICS
            'GSTIN : 33DHDPP9433H1Z6 - Name: SARASWATHI FABRICS

            txtEIUserID.Text = "API_SARASWATHI_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_SARASWATHI_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '----GANAPATHY SPINNING MILL (PALLADAM)

            If Trim(UCase(txt_GSTIN.Text)) = "33AAMFS3696N2ZD" Then 'Sri Loga Textiles

                txtEIUserID.Text = "sriloga231_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "sriloga231_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33BTGPD9041Q1ZD" Then ' Lakshmi textile mills(Palladam)

                txtEIUserID.Text = "API_LTM_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_LTM_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AAFFG1960L1ZE" Then 'GANAPATHY

                txtEIUserID.Text = "API_GANAPATHI_PALDM"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_GANAPATHI_PALDM"
                txtEWBPassword.Text = "RajRock@7417"


            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AEOFS1546K1ZP" Then ' SRI LAKSHMI TEXTILE MILL

                txtEIUserID.Text = "API_LAKSHMITEX_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_LAKSHMITEX_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then ' GSTIN : 33AAEFK6283F1ZE '---- Kalaimagal Textiles (Palladam)
            If Trim(UCase(txt_GSTIN.Text)) = "33AAEFK6283F1ZE" Then  'Kalaimagal Textiles (Palladam)
                txtEIUserID.Text = "API_KALAIMAGAL_TSS"
                txtEIPassword.Text = "Kalaimagal123*"

                txtEWBUserID.Text = "API_KALAIMAGAL_TSS"
                txtEWBPassword.Text = "Kalaimagal123*"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AAWFK3254A1ZH" Then ' KMT Fabrics

                txtEIUserID.Text = "API_KMTFAB_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_KMTFAB_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AAEFB7202P1ZH" Then ' Bagavan Textile

                ' GSTIN :33AAEFB7202P1ZH - Name : SRI BHAGAVAN TEXTILES - User : Tax Payer

                txtEIUserID.Text = "Bhagavan@_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "Bhagavan@_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1140" Then '---- ANANDHARAJA MILLS (AVINASHI)
            '--- GSTIN:33AFRPK5568H2ZZ - Name: ANANDHARAJA MILLS

            txtEIUserID.Text = "API_ANANDHARAJA_TSS"
            txtEIPassword.Text = "RajRock@1716"

            txtEWBUserID.Text = "API_ANANDHARAJA_TSS"
            txtEWBPassword.Text = "RajRock@1716"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then '----KRG TEXTILE MILLS (PALLADAM)
            If Trim(UCase(txt_GSTIN.Text)) = "33AFQPR7703C1ZF" Then

                'gstin : 33AFQPR7703C1ZF NAME : K.R.G. TEXTILE MILLS
                txtEIUserID.Text = "krgtex_API_TSS"
                txtEIPassword.Text = "RajRock@1716"

                txtEWBUserID.Text = "krgtex_API_TSS"
                txtEWBPassword.Text = "RajRock@1716"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AAICK9772G1Z6" Then
                'GSTIN :33AAICK9772G1Z6 - Name : KRG TEXTILE MILLS PRIVATE LIMITED 

                txtEIUserID.Text = "API_KRGTEX_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_KRGTEX_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AAICK9772G2Z5" Then
                'GSTIN:33AAICK9772G2Z5 - Name: KRG Textile MILLS PRIVATE LIMITED (COTTON)

                txtEIUserID.Text = "API_KRGTEXPVTL_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_KRGTEXPVTL_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1235" Then '----SOMANUR KALPANA COTTON (SOMANUR)
            txtEIUserID.Text = "ANAND@2018_API_TSS"
            txtEIPassword.Text = "RajRock@1716"

            txtEWBUserID.Text = "ANAND@2018_API_TSS"
            txtEWBPassword.Text = "RajRock@1716"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '----BRT FABRICS (SOMANUR)
            txtEIUserID.Text = "API_BRT_FABRIC_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_BRT_FABRIC_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1282" Then '----BRT SIZING (SOMANUR)
            txtEIUserID.Text = ""
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = ""
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1357" Then '----Desikanathar Textile (Dindugal)
            txtEIUserID.Text = "API_DESIKA_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_DESIKA_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1395" Then '----SANTHA SPINNING (Somanur)

            If InStr(1, Trim(UCase(Common_Procedures.CompGroupName)), "SANTHA") > 0 And InStr(1, Trim(UCase(Common_Procedures.CompGroupName)), "SPIN") > 0 Then

                txtEIUserID.Text = "API_SANTHA_SPNG_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_SANTHA_SPNG_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33ACVPC9582G1Z6" Then 'SANTHA Exports

                txtEIUserID.Text = "API_SANTHAEXP_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_SANTHAEXP_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AMJPC8662P1ZJ" Then 'SANTHA Exports 
                'GSTIN:33AMJPC8662P1ZJ - Name: SANTHA TEXTILE

                txtEIUserID.Text = "API_SANTHATEX_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_SANTHATEX_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AFFFS6769G1ZM" Then 'SANTHA Exports 
                'GSTIN :33AFFFS6769G1ZM - Name : SANTHA EXPORTS - User : Tax Payer

                txtEIUserID.Text = "33AFFFS676_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "33AFFFS676_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"
            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Then '----PRAKASH TEXTILES
            If Trim(UCase(txt_GSTIN.Text)) = UCase("33AADFP7438A1ZL") Then  '----(1)

                'GSTIN:33AADFP7438A1ZL - Name: PRAKASH TEXTILES
                txtEIUserID.Text = "API_PRAKASH_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_PRAKASH_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = UCase("33AFSPP9722R1ZD") Then   '---(2)

                'GSTIN :33AFSPP9722R1ZD - Name : BEST FABRICS - User : Tax Payer
                txtEIUserID.Text = "prakashsub_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "prakashsub_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1098" Then '----SRI BANNARIAMMAN TEX

            If Trim(UCase(txt_GSTIN.Text)) = UCase("33ADLPV2552Q1ZU") Then  '----(1)
                txtEIUserID.Text = "sbatex_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "sbatex_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = UCase("33BZQPP4649N1ZH") Then   '---(2)

                ' --- GSTIN :33BZQPP4649N1ZH - Name : ASPIRE ENTERPRISES - User : Tax Payer

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "ASPIREENTE_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1101" Then '----AVR MILLS

            txtEWBUserID.Text = "Avrmills_1_API_TSS"
            txtEWBPassword.Text = "RajRock@7417"

            txtEIUserID.Text = "Avrmills_1_API_TSS"
            txtEIPassword.Text = "RajRock@7417"
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1394" Then '----SRI RAMKUMAR TEX

            txtEWBUserID.Text = "APFPS7928M_API_TSS"
            txtEWBPassword.Text = "RajRock@7417"

            txtEIUserID.Text = "APFPS7928M_API_TSS"
            txtEIPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1371" Then '---- RAAY SAN TEXTILES (PALLADAM)
            'Trim(UCase(txt_GSTIN.Text)) = "33AQRPK1942J1ZM" 

            If Trim(UCase(txt_GSTIN.Text)) = "33AQRPK1942J1ZM" Then '---- RAAY SAN TEXTILES (PALLADAM)

                txtEWBUserID.Text = "PMR_AQRPK1_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

                txtEIUserID.Text = "PMR_AQRPK1_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1258" Then '---- JEEVITHA TEXTILES (SOMANUR)
            If Trim(UCase(txt_GSTIN.Text)) = "33EDJPS1112P1ZB" Then
                'GSTIN:33EDJPS1112P1ZB - Name: JEEVITHA TEX

                txtEIUserID.Text = "JEEVITHATE_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "JEEVITHATE_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33CNNPK7602N2ZK" Then

                'GSTIN :33CNNPK7602N2ZK - Name : THASHVIKA FABS - User : Tax Payer

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "thashvikaf_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1370" Then '---- AKIL IMPEX
            'Trim(UCase(txt_GSTIN.Text)) = "33ABNFA4369G1ZC" 
            txtEWBUserID.Text = "AKILIMPEXF_API_TSS"
            txtEWBPassword.Text = "RajRock@7417"
            txtEIUserID.Text = "AKILIMPEXF_API_TSS"
            txtEIPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1027" Then '---- PREM TEXTILE  

            If Trim(UCase(txt_GSTIN.Text)) = "33ANOPA0994L1ZR" Then 'LOGOS EXPORTS
                txtEWBUserID.Text = "Logos@1988_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AATPF4752F1ZP" Then 'Prem Tex
                txtEWBUserID.Text = "sAATPF4752_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1127" Then '---- SivaJothi TEX
            'Trim(UCase(txt_GSTIN.Text)) = "33ACAFS7044J1Z4" 
            txtEWBUserID.Text = "SACAFS7044_API_TSS"
            txtEWBPassword.Text = "RajRock@7417"

            txtEIUserID.Text = "SACAFS7044_API_TSS"
            txtEIPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1148" Then '---- ANAND JOTHI SPINNING

            If Trim(UCase(txt_GSTIN.Text)) = "33AAJFA3461E1ZV" Then   '---- ANAND JOTHI SPINNING
                txtEWBUserID.Text = "ANANDAJOTH_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

                txtEIUserID.Text = "ANANDAJOTH_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AAJFV8029M1ZN" Then '---- Vishnu JOTHI SPINNING
                txtEWBUserID.Text = "sAAJFV8029_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

                txtEIUserID.Text = "sAAJFV8029_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33ACAFS7044J1Z4" Then '---- SivaJothi TEX
                txtEWBUserID.Text = "SACAFS7044_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

                txtEIUserID.Text = "SACAFS7044_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "--1148--" Then '---- Vishnu JOTHI SPINNING
            'Trim(UCase(txt_GSTIN.Text)) = "33AAJFV8029M1ZN" 
            txtEWBUserID.Text = "sAAJFV8029_API_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Then '---- COTEX SPINNERS
            'Trim(UCase(txt_GSTIN.Text)) = "33AAICC0574A1Z8" 
            txtEWBUserID.Text = "COTTEXPVT2_API_TSS"
            txtEWBPassword.Text = "RajRock@7417"

            txtEIUserID.Text = "COTTEXPVT2_API_TSS"
            txtEIPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then '---- UnitedWeaves

            If Trim(UCase(txt_GSTIN.Text)) = "33AABFU9042P1ZR" Then 'United Weaves

                txtEWBUserID.Text = "UWS123_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

                txtEIUserID.Text = "UWS123_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AAXFG2494D1Z6" Then 'Gounder Traders

                txtEWBUserID.Text = "GOUNDERTRA_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

                txtEIUserID.Text = "GOUNDERTRA_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1120" Then '---- MARIA INTERNATIONAL

            If Trim(UCase(txt_GSTIN.Text)) = "33APIPA1659N1ZT" Then '---MARIA INTERNATIONAL (SOMANUR)   
                txtEWBUserID.Text = "MARIYAINT_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

                txtEIUserID.Text = "MARIYAINT_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33BOVPM1858J1ZC" Then '---Allwin fabs
                txtEWBUserID.Text = "allwin83_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

                txtEIUserID.Text = "allwin83_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1274" Then '----ADHIYAMAN WEAVING MILLS (P) LIMITED (SOMANUR)     
            If Trim(UCase(txt_GSTIN.Text)) = "33ABECS1189A1ZP" Then '---- ADHIYAMAN WEAVING MILLS (P) LIMITED (SOMANUR)     
                '----Trim(UCase(txt_GSTIN.Text)) = "33ABECS1189A1ZP" 

                'GSTIN:33ABECS1189A1ZP  - Name: SHRI ADHIYAMAN WEAVING MILLS PRIVATE LIMITED

                txtEIUserID.Text = "adhiyaman2_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "adhiyaman2_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AFPPK7139E1ZD" Then 'SENTHOOR MURUGAN TEXTILES

                'GSTIN:33AFPPK7139E1ZD - Name : SENTHOOR MURUGAN TEXTILES

                txtEIUserID.Text = ""  ' "Amuthan202_API_TSS"
                txtEIPassword.Text = ""  ' "RajRock@7417"

                txtEWBUserID.Text = "Amuthan202_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"
            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AAOCS9536L1ZN" Then '---- SOMANUR KALPANA COTTON(INDIA)PVT.LTD
                txtEIUserID.Text = "ANAND@2018_API_TSS"
                txtEIPassword.Text = "RajRock@1716"

                txtEWBUserID.Text = "ANAND@2018_API_TSS"
                txtEWBPassword.Text = "RajRock@1716"


            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33ADGPV5440G1ZJ" Then '---- KALPANA TEXTILES

                '---GSTIN : 33ADGPV5440G1ZJ - Name : KALPANA TEXTILES

                txtEIUserID.Text = "KALPANA@20_API_TS2"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "KALPANA@20_API_TS2"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)
            'Trim(UCase(txt_GSTIN.Text)) = "33AAJCS8948K1ZO" 
            txtEIUserID.Text = "sakthi_123_API_TSS"
            txtEIPassword.Text = "RajRock@7417"
            txtEWBUserID.Text = "sakthi_123_API_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1158" Then 'NIDHIE WEAVING MILL

            If Trim(UCase(txt_GSTIN.Text)) = "33ABZFM8604L1ZJ" Then

                ' GSTIN :33ABZFM8604L1ZJ - NAME : MULBERRY SYNTHETICS - User : Tax Payer


                txtEIUserID.Text = "ABZFM8604L_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "ABZFM8604L_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            Else

                txtEIUserID.Text = "API_NIDHIE_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_NIDHIE_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If



            '

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1105" Then 'KAVINGANGA WEAVING MILLS PVT LTD

            If Trim(UCase(txt_GSTIN.Text)) = "33AAGCK8538G1ZE" Then ' KavinGANGA WEAVING

                txtEIUserID.Text = "API_GANGA_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_GANGA_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AKDPV7111J1Z4" Then 'YAMUNA WEAVING

                txtEIUserID.Text = "API_YAMUNAWVG_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_YAMUNAWVG_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AAICB0112H1ZD" Then 'BALA GANGA 

                txtEIUserID.Text = "API_BALAGANGA_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_BALAGANGA_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1111" Then 'VELAN SPINNING MILLS INDIA PVT LIMITED
            'Trim(UCase(txt_GSTIN.Text)) = "33AAFCV5231K1Z5" 
            txtEIUserID.Text = "velanspinn_API_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "velanspinn_API_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1041" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1400" Then 'VARUNN EXPORTS      &  JAI SRIRAM TEXTILES
            'Trim(UCase(txt_GSTIN.Text)) = "33AADFV6946P1ZH" 

            If Trim(UCase(txt_GSTIN.Text)) = "33AADFV6946P1ZH" Then
                txtEIUserID.Text = "API_VARUNN_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_VARUNN_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AJHPR3539D1ZB" Then

                '-GSTIN:33AJHPR3539D1ZB - Name: JAI SRIRAM TEXTILES

                txtEIUserID.Text = "API_JAISRIRAM_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_JAISRIRAM_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33ALKPG0015G1ZS" Then

                'GSTIN:33ALKPG0015G1ZS - Name : KANDHAVEL TEXTILES - User : Tax Payer

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "sanju789_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"



            End If
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1277" Then 'SRINATH WEAVING MILLS LLP

            txtEIUserID.Text = "API_SRINATH_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_SRINATH_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1316" Then 'NITHYABHARATH TEXTILE (P) LTD

            If Trim(UCase(txt_GSTIN.Text)) = "33AARFN7812N1ZO" Then 'NITHYA BHARATH
                txtEIUserID.Text = "API_NITHYA_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_NITHYA_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AABCN9809N1Z1" Then 'NITHYA BHARATHI TEXTILE PVT.LTD
                txtEIUserID.Text = "API_NITHBHARATH_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_NITHBHARATH_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1215" Then 'MAHENDRAN TEXTILES

            If Trim(UCase(txt_GSTIN.Text)) = "33ABMFM4213C1ZQ" Then 'Mahendra Mills
                txtEIUserID.Text = "API_MAHMILLS_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_MAHMILLS_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33ABJPL5093Q1Z0" Then 'Mahendra Tex
                txtEIUserID.Text = "API_MAHENDRA_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_MAHENDRA_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1097" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1132" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1397" Then 'SRI GANAPATHY MURUGAN TEXTILE

            If Trim(UCase(txt_GSTIN.Text)) = "33AAHFS5342Q1ZQ" Then 'SRI GANAPATHY MURUGAN TEXTILE
                txtEIUserID.Text = "API_GANAPTHYTEX_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_GANAPTHYTEX_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AAECS2342C1ZY" Then 'SRI GANAPATHY MURUGAN SPINNING MILL
                txtEIUserID.Text = "API_GNPTHYSPNG_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_GNPTHYSPNG_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1234" Then 'ARULJOTHI EXPORTS PVT LTD

            If Trim(UCase(txt_GSTIN.Text)) = "33AADCA7439F1ZV" Then

                txtEIUserID.Text = "API_ARULJOTHI_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_ARULJOTHI_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AABHA0734N1ZM" Then 'GSTIN :33AABHA0734N1ZM - Name : SRI ARUNOTHAYA TEXTIELS

                txtEIUserID.Text = "EWAYAABHA0_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "EWAYAABHA0_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AAICA6572A1Z0" Then 'GSTIN :33AAICA6572A1Z0 - Name : AKSHADHA TRADING (P) LTD

                txtEIUserID.Text = "EWAYAAICA6_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "EWAYAAICA6_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1352" Then ' EMINENT TEXTILE MILLS PRIVATE LIMITED

            txtEIUserID.Text = "API_EMINENT_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_EMINENT_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1187" Then 'SRI SAKTHI VINAYAKA TEXTILES
            'GSTIN :33BZLPS7212R2ZL - Name : SRI SAKTHI VINAYAKA TEXTILES

            txtEIUserID.Text = "SRISAKTHIV_API_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "SRISAKTHIV_API_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1188" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1189" Then 'KALISAMY TEX

            'GST IN: 33APJPC6628J1ZV
            txtEIUserID.Text = "API_KALISAMYTEX_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_KALISAMYTEX_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1342" Then '---- Sri Ram Textiles (Palladam) (ANUPSHARMA CLOTH AGNET)

            'txt_GSTIN.Text = "33ALSPA9970A1Z4"

            txtEIUserID.Text = "API_LAXMIEXP_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_LAXMIEXP_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1214" Then 'CHOLA TEXTILE MILLS

            If Trim(UCase(txt_GSTIN.Text)) = "33AJDPP3779P3ZG" Then ' 'GSTIN :33AJDPP3779P3ZG
                txtEIUserID.Text = "API_CHOLATEX_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_CHOLATEX_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AAIFC9946K1ZZ" Then 'GSTIN:33AAIFC9946K1ZZ - Name: CHOLA TEXTILE MILLS
                txtEIUserID.Text = "API_CHOLATEXTIL_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_CHOLATEXTIL_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1334" Then '---------- SIVASELVI TEXTILES (VANJIPALAYAM)  (OR)  SIVA SELVI TEXTILES (VANJIPALAYAM)

            'GST IN : 33ALDPS2129N1ZX

            If Trim(UCase(txt_GSTIN.Text)) = "33ALDPS2129N1ZX" Then
                txtEIUserID.Text = "API_SIVASELVI_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_SIVASELVI_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33ABHCS1262P1Z1" Then         '2 - GSTIN:33ABHCS1262P1Z1 - Name: SIVASELVI TEXTILES PRIVATE LIMITED

                txtEIUserID.Text = "API_SIVA_SELVI_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_SIVA_SELVI_TSS"
                txtEWBPassword.Text = "RajRock@7417"



            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33ARHPS2982D1ZP" Then         '3 - GSTIN :33ARHPS2982D1ZP - Name : SHIVASELVI TEXMILL - User : Tax Payer


                txtEIUserID.Text = "Shivaselvi_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "Shivaselvi_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1060" Then '---- Selvanayaki Textiles (Karumanthapatti)
            If Trim(UCase(txt_GSTIN.Text)) = "33AGDPR0302F1Z3" Then  '--- GSTIN : 33AGDPR0302F1Z3  -  Selvanayaki Textiles (Karumanthapatti)

                txtEIUserID.Text = "API_SELVANAYAKI_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_SELVANAYAKI_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33ADJFS1198M1ZK" Then  '--- GSTIN : 33ADJFS1198M1ZK - Name : SRI SELVANAYAKI FABRICS 

                txtEIUserID.Text = "Fabrics@12_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "Fabrics@12_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1152" Then '----J.P.R Textile (PALLADAM) or JPR Textile (PALLADAM)

            txtEIUserID.Text = ""
            txtEIPassword.Text = ""

            txtEWBUserID.Text = ""
            txtEWBPassword.Text = ""

            If Trim(UCase(txt_GSTIN.Text)) = "33AHIPC0436B1Z8" Then  'GSTIN : 33AHIPC0436B1Z8 -  ESWAR TEXTILES
                txtEWBUserID.Text = "ESWAR1234@_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AADFJ7689P1ZJ" Then    'GSTIN : 33AADFJ7689P1ZJ  -  JPR TEXTILES
                txtEIUserID.Text = "JPR123@_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "JPR123@_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1084" Then  'K.T.COTTON MILLS
            'GSTIN:33AAHFK4838K1Z3

            txtEIUserID.Text = "API_KTCOTTON_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_KTCOTTON_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1405" Then '---SHANMUGA PRIYA TEX (PALLADAM)
            'gstin - 33ALMPS1420M1ZY

            If Trim(UCase(txt_GSTIN.Text)) = "33ALMPS1420M1ZY" Then
                txtEIUserID.Text = "PMR_ALMPS1_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "PMR_ALMPS1_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33JKNPS4500K1ZR" Then  '--(GST - 2 )

                '--------------GSTIN:33JKNPS4500K1ZR - Name: SRI MURUGAN TEXTILE

                txtEIUserID.Text = "API_SRIMURUGAN_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_SRIMURUGAN_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1393" Then 'ESSAN MILLS
            ' GSTIN : 33ALUPY1938Q1ZX - Name : EESAN MILLS

            'txtEIUserID.Text = "EESANMILLS_API_TSS"
            'txtEIPassword.Text = "RajRock@7417"

            'txtEWBUserID.Text = "EESANMILLS_API_TSS"
            'txtEWBPassword.Text = "RajRock@7417"

            '********************************************************************************

            'GSTIN :33AAHCE0235H1Z3 - Name : EESAN FABTEX PRIVATE LIMITED

            txtEIUserID.Text = "EESANFABTE_API_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "EESANFABTE_API_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1283" Then 'ARUL MURUGAN TEXTILES
            'GSTIN :33AEVPN2602L1Z9        Name  : --ARUL MURUGAN TEXTILES

            If Trim(UCase(txt_GSTIN.Text)) = "33AEVPN2602L1Z9" Then

                txtEIUserID.Text = "SAEVPN2602_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "SAEVPN2602_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33ASQPP0432M2ZG" Then

                '--GSTIN:33ASQPP0432M2ZG - Name : VEL MURUGAN TEXTILES - User : Tax Payer

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "velmurugan_API_TS2"
                txtEWBPassword.Text = "RajRock@7417"


            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then  'M.K. TEXTILES
            'GSTIN:33ADWPK0484D2ZH   'M.K. TEXTILES

            If Trim(UCase(txt_GSTIN.Text)) = "33ADWPK0484D2ZH" Then

                txtEIUserID.Text = "API_MKTEX_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_MKTEX_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33APVPD3254K1ZO" Then    '   GSTIN:33APVPD3254K1ZO - Name: MANOJ TEXTILES

                txtEIUserID.Text = "API_MANOJTEX_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_MANOJTEX_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1438" Then '-----SATHY TEXTILES (SATHYAMANGALAM)
            'GSTIN : 33ACRFS8399F1ZC

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AAYFK7632A1Z9")) Then    '---GSTIN :33AAYFK7632A1Z9 - Name : KVP WEAVES - User : Tax Payer

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "kvpweaves_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33ACRFS8399F1ZC")) Then    '---GSTIN : 33ACRFS8399F1ZC - Name : SATHY TEXTILES (SATHYAMANGALAM)

                txtEIUserID.Text = "API_SATHYTEX_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_SATHYTEX_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33ACRFS8399F2ZB")) Then  '-----GSTIN :33ACRFS8399F2ZB - Name : SATHY TEXTILES (COTTON DIVISION) - User : Tax Payer

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "Sathy_CD_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If



        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1350" Then ' MALAR COTTON 

            txtEIUserID.Text = ""     ' "malarcotto_API_TSS"
            txtEIPassword.Text = ""     '"RajRock@7417"

            txtEWBUserID.Text = ""  ' "malarcotto_API_TSS"
            txtEWBPassword.Text = "" ' "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1118" Then  '---------KASTUR LAXMI MILLS

            If Trim(UCase(txt_GSTIN.Text)) = "33AAXFK6266F1ZW" Then  'GSTIN     : 33AAXFK6266F1ZW -  KASTURLAXMI MILLS TAMILNADU

                txtEIUserID.Text = "API_KASTURLAXMI_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_KASTURLAXMI_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "27AAXFK6266F1ZP" Then  ' Kasturlaxmi Mills - MAHARASHTRA - GSTIN : 27AAXFK6266F1ZP

                txtEIUserID.Text = "API_KASTURMAHA_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_KASTURMAHA_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AEDFS0662R1ZN" Then 'SURYA LAXMI MILLS

                txtEIUserID.Text = "API_SURYALAXMI_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_SURYALAXMI_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AAXFK6262B1Z8" Then 'KALINDI TEX COM


                txtEIUserID.Text = "API_KALINDITEX_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_KALINDITEX_TSS"
                txtEWBPassword.Text = "RajRock@7417"
            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1204" Then '-----KOHINOOR TEXTILE MILLS(PALLADAM)    or   RAJAMURUGAN MILLS (PALLADAM)

            If Trim(UCase(txt_GSTIN.Text)) = "33ACXPT3177C1Z8" Then  'GSTIN     : 33ACXPT3177C1Z8 -  RAJAMURUGAN MILLS

                txtEIUserID.Text = "RAJAMURUGA_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "RAJAMURUGA_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AKPPT9008C1Z1" Then  'GSTIN : 33AKPPT9008C1Z1     - Name : K S T TEXTILES

                txtEIUserID.Text = "ksttex33_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "ksttex33_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1445" Then '-----SR TIRUMALA MLLS (AVINASHI)
            txtEIUserID.Text = "tirumalami_API_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "tirumalami_API_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1029" Then 'ARUL KUMARAN TEXTILES

            If Trim(UCase(txt_GSTIN.Text)) = "33BWFPK8363L1Z4" Then 'GSTIN:33BWFPK8363L1Z4
                txtEIUserID.Text = "API_ARULKUMARAN_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_ARULKUMARAN_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33BBKPS6361Q1ZU" Then 'GSTIN:33BBKPS6361Q1ZU - Name: SHANMUGABALA TEXTILES

                txtEIUserID.Text = "API_SHANMUGABAL_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_SHANMUGABAL_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1421" Then 'G.K TEXTILES

            'GSTIN:33AANFG8006B1ZS

            txtEIUserID.Text = "API_GKTEX_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_GKTEX_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1210" Then 'SRI ABIRAAMI TEXTILE

            '  GSTIN:33AAFFA6043C1Z3 - Name: SRI ABIRAAMI TEXTILE

            txtEIUserID.Text = "API_ABIRAAMI_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_ABIRAAMI_TSS"
            txtEWBPassword.Text = "RajRock@7417"


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1045" Then 'KESAVA LOGU TEXTILES

            If Trim(UCase(txt_GSTIN.Text)) = "33BCGPG1411C1ZK" Then   ' --- unused
                'GSTIN:33BCGPG1411C1ZK - Name: KESAVA LOGU TEXTILES


                txtEIUserID.Text = "API_KESAVALOGU_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_KESAVALOGU_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33BAKPG7925B1ZZ" Then '  --- unused
                'GSTIN:33BAKPG7925B1ZZ - Name: KOMALA SPINNERS

                txtEIUserID.Text = "API_KOMALA_SPIN_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_KOMALA_SPIN_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33ABCFK8995K1ZO" Then   ' ---  KESAVALOGU TEXTILES ( 2 gstin) 

                'GSTIN :33ABCFK8995K1ZO - Name : KESAVALOGU TEXTILES - User : Tax Payer

                txtEIUserID.Text = "KESAVALOGU_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "KESAVALOGU_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1146" Then 'ASHMITHA TEXTILE
            If Trim(UCase(txt_GSTIN.Text)) = "33ABGFA9210R1Z6" Then  '-1
                'GSTIN:33ABGFA9210R1Z6 - Name: ASMITHA TEXTILES

                txtEIUserID.Text = "API_ASMITHATEX_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_ASMITHATEX_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33BBMPM4117N1ZE" Then  '-2
                'GSTIN :33BBMPM4117N1ZE - Name : JAISAKTHI REWINDING

                txtEIUserID.Text = "JAISAKTHI2_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "JAISAKTHI2_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1409" Then 'VIJAYAKALA TEXTILES
            If Trim(UCase(txt_GSTIN.Text)) = "33AADFV6645B1ZE" Then
                'GSTIN:33AADFV6645B1ZE - Name: VIJAYAKALA TEXTILES

                txtEIUserID.Text = "API_VIJAYAKALA_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_VIJAYAKALA_TSS"
                txtEWBPassword.Text = "RajRock@7417"
            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AHUPC4252D1ZM" Then
                'GSTIN :33AHUPC4252D1ZM - Name : KARUNAIVEL IMPEX - User : Tax Payer

                txtEIUserID.Text = "KARUNAIVEL_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "KARUNAIVEL_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"
            End If
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1216" Then 'JAGATHGURU TEXTILES
            If Trim(UCase(txt_GSTIN.Text)) = "33AALFJ0824E1ZR" Then     'GSTIN:33AALFJ0824E1ZR - Name: JAGATH GURU TEXTILE

                txtEIUserID.Text = "API_JAGATHGURU_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_JAGATHGURU_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33EDFPS1370K1ZD" Then       '2 ---GSTIN:33EDFPS1370K1ZD - Name: KADAYESHWARA GREY FABRICS

                txtEIUserID.Text = "API_KADAYESHWAR_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_KADAYESHWAR_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1050" Then 'KUMARAVEL TEXTILES

            If Trim(UCase(txt_GSTIN.Text)) = "33AEUPD9577R1ZC" Then   'GSTIN:33AEUPD9577R1ZC - Name: KUMARAVEL TEXTILES
                txtEIUserID.Text = "API_KUMARAVEL_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_KUMARAVEL_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AOEPV9536Q1Z0" Then 'GSTIN:33AOEPV9536Q1Z0 - Name: AAKASH WEAVING MILL

                txtEIUserID.Text = "API_AAKASH_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_AAKASH_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33ATNPV6443L1ZZ" Then 'GSTIN : 33ATNPV6443L1ZZ      -  Name : V.T.I TEXTILES

                txtEIUserID.Text = "vtitex_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "vtitex_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1191" Then ' A.P TEXTILE MILLS
            'GSTIN:33AMDPP0444J1Z8 - Name: A.P TEXTILE MILLS

            txtEIUserID.Text = "API_APTEXMILL_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_APTEXMILL_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1195" Then '---SREE KASTHURI MILL

            If Trim(UCase(txt_GSTIN.Text)) = "33ABIFS6027L1ZX" Then 'SREE KASTHURI MILL

                txtEIUserID.Text = "API_KASTHURI_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_KASTHURI_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33ALMPS1450R1ZI" Then 'SKM FABS

                txtEIUserID.Text = "API_SKMFABS_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_SKMFABS_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1309" Then '------------ venkatalakshmi

            If Trim(UCase(txt_GSTIN.Text)) = "33AAUFS0955N3ZG" Then '--SRI VENKATALAKSHMI MILLSL

                'GSTIN:33AAUFS0955N3ZG - Name: SRI VENKATALAKSHMI MILLS

                txtEIUserID.Text = "API_VENKATALAK_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_VENKATALAK_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33ALMPS1416M1ZU" Then '--PRANAV TEXTILE MILLS 

                'GSTIN :33ALMPS1416M1ZU - Name : PRANAV TEXTILE MILLS - User : Tax Payer

                txtEIUserID.Text = "ALMPS1416__API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "ALMPS1416__API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1052" Then '---- Shri Vedha Tex 
            'GSTIN:33ABUFS3579F1ZN - Name: SHRI VEDHA TEX

            txtEIUserID.Text = "API_VEDHATEX_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_VEDHATEX_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1253" Then '---- VM MILLS (KARUMATHAMPATTI)
            'GSTIN:33CUPPS3926N1ZU - Name: V M MILLS 

            If Trim(UCase(txt_GSTIN.Text)) = "33CUPPS3926N1ZU" Then


                txtEIUserID.Text = "API_VM_MILLS_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_VM_MILLS_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33BHOPM6981Q1ZA" Then   '--- S.V. WEAVES

                '--GSTIN:33BHOPM6981Q1ZA - Name: S.V.WEAVES


                txtEIUserID.Text = "API_SV_WEAVES_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_SV_WEAVES_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1264" Then '---- SRI SUBBULAKSHMI & C0 (SOMANUR)

            'GSTIN:33AAUFS8184P2Z0 - Name: SRI SUBBULAKSHMI AND CO 

            txtEIUserID.Text = "API_SUBBULAKS_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_SUBBULAKS_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1296" Then '---- SHREE GOMATHI MILL (VANJIPALAYAM) , SG MILL
            'GSTIN:33ALGPR5826C1Z8 - Name: S.G.MILL

            If Trim(UCase(txt_GSTIN.Text)) = "33ALGPR5826C1Z8" Then 'S.G.MILL
                txtEIUserID.Text = "API_SGMILL_VANJ_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_SGMILL_VANJ_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33BCTPM7603M1Z2" Then 'GSTIN:33BCTPM7603M1Z2 - Name: SHREE GOMATHI MILL

                txtEIUserID.Text = "API_GOMATHIMILL_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_GOMATHIMILL_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AAZPL8551C1ZE" Then   'GSTIN:33AAZPL8551C1ZE  - Name GOMATHI TEX MILL 

                txtEIUserID.Text = "gtexmill_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "gtexmill_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1151" Then '---- BHARATHI TEXTILE (TIRUPUR)
            'GSTIN:33AALFS4133K1Z3 - Name: SRI BHARATHI TEXTILES

            txtEIUserID.Text = "API_BHARATHITEX_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_BHARATHITEX_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1238" Then '---- Amman Tex (Mangalam-VelayuthamPalayam)
            '--- GSTIN : 33AHYPG6868P1Z7 - Name : AMMAN TEX

            If Trim(UCase(txt_GSTIN.Text)) = "33AHYPG6868P1Z7" Then '   'GSTIN :33AHYPG6868P1Z7 - Name : AMMAN TEX
                txtEIUserID.Text = "AMMANTEX36_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "AMMANTEX36_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33BWKPK2183F1ZM" Then  ' GSTIN :33BWKPK2183F1ZM - Name : SHRI AMMAN FABRICS

                txtEIUserID.Text = "ammanfabri_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "ammanfabri_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1339" Then '-----SREE MARAKATHAM YARN


            If Trim(UCase(txt_GSTIN.Text)) = "33BLCPC3575L1Z5" Then  '--- GSTIN :33BLCPC3575L1Z5 - Name : SREE MARAKATHAM YARN

                txtEIUserID.Text = "SREEMARAKA_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "SREEMARAKA_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33BLXPS4879L1ZV" Then  '---GSTIN :33BLXPS4879L1ZV - Name : RAMATHAL TEXTILES - User : Tax Payer

                txtEIUserID.Text = "RAMATHALTE_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "RAMATHALTE_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1159" Then 'CARAVAN TEXTILES
            'GSTIN:33AACFC0958P1Z9 - Name: CARAVAN TEXTILES

            txtEIUserID.Text = "API_CARAVANTEX_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_CARAVANTEX_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1451" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1028" Then '---- Chinnu Sizing (Textiles)(Palladam)
            '--- GSTIN : 33AAKFC4707E1ZT - Name: CHINNU SIZING MILLS
            txtEWBUserID.Text = "API_CHINNUSIZ_TSS"
            txtEWBPassword.Text = "RajRock@7417"
            txtEIUserID.Text = "API_CHINNUSIZ_TSS"
            txtEIPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1390" Then '---DURGA TEXTILE (THEKKALUR)
            'GSTIN:33ASVPR5974K1ZP - Name: DURGA TEXTILES

            txtEIUserID.Text = "API_DURGATEX_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_DURGATEX_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Then '---- M.S Textiles (Tirupur)

            If Trim(UCase(txt_GSTIN.Text)) = "33AEPPM9587B1Z3" Then 'GSTIN:33AEPPM9587B1Z3 - Name: M.S.TEXTILES

                txtEIUserID.Text = "API_MSTEXTILES_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_MSTEXTILES_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AKXPS1510J1ZW" Then 'GSTIN:33AKXPS1510J1ZW - Name: SRI SENTHILMURUGAN MILLS

                txtEIUserID.Text = "API_SENTHILMURU_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_SENTHILMURU_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1306" Then '---- Sri Vigneswara Mills (Palladam)
            'GSTIN:33ABOFS5836R1Z8 - Name: SRI VIGNESHWARA MILLS

            txtEIUserID.Text = "API_VIGNESHWARA_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_VIGNESHWARA_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1453" Then '---- VELAN MILLS  

            'GSTIN :33BACPB8394M2ZF - Name : SRI VELAN MILLS

            txtEIUserID.Text = ""
            txtEIPassword.Text = ""

            txtEWBUserID.Text = "BACPB8394M_API_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1455" Then '---- VEERA TEX

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AMRPM2695J1ZG")) Then
                'GSTIN:33AMRPM2695J1ZG - Name: VEERA TEX

                txtEIUserID.Text = "API_VEERATEX_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_VEERATEX_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33BRPPR7138M1Z0")) Then
                'GSTIN :33BRPPR7138M1Z0 - Name : RSS TEX 

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "rsstex123_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AFEPA0270P2ZO")) Then 'ANANTHKUMAR TEXTILE MILLS - 1 GSTIN
                '---GSTIN:33AFEPA0270P2ZO - Name: ANANTHKUMAR TEXTILE MILLS

                txtEIUserID.Text = "API_ANANTHKUMAR_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_ANANTHKUMAR_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AYWPD0115H1ZQ")) Then
                '---GSTIN :33AYWPD0115H1ZQ - Name : SUBHIKSHA TEX - User : Tax Payer

                txtEIUserID.Text = "subikshate_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "subikshate_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33BYCPM8778E1Z5")) Then    'ANANTHKUMAR TEXTILE MILLS - 2 GSTIN
                'GSTIN :33BYCPM8778E1Z5  Name : RAAJAVILVAM TEX
                txtEIUserID.Text = "RAAJAVILVA_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "RAAJAVILVA_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1307" Then '---- Sri Sugam Textile (Karumanthampatti)
            'GSTIN:33ADIFS6668K1ZG - Name: SRI SUGAM TEXTILE

            txtEIUserID.Text = "API_SRISUGAM_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_SRISUGAM_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1185" Then '---- T.S TEXTILE
            'GSTIN:33DYAPS8939F1Z2 - Name: T S TEX

            txtEIUserID.Text = "API_TSTEX_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_TSTEX_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1269" Then '---- SRI SHANMUGA TEXTILES

            'GSTIN:33ALDPR4850K1ZW - Name: SRI SHANMUGA TEXTILES

            txtEIUserID.Text = "API_SHANMUGATEX_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_SHANMUGATEX_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1383" Then '---- SASTTIKA TEX (POOMALUR)   or  VELMURUGAN TEXTILES (POOMALUR)

            '---GSTIN  :  33ETYPK0209L1ZB  - Name : VELMURUGAN TEXTILES

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33ETYPK0209L1ZB")) Then ' - Name : VELMURUGAN TEXTILES

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "Velmurugan_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AJDPN4313R1Z0")) Then ' - Name : SASTTIKA TEX 

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "SASTTIKATE_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33GQIPS4906M1Z9")) Then ' - Name : ASHVIKA WEAVING MILLS ( of SASTTIKA TEX )

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "GQIPS4906M_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1136" Then '---- PS Textiles (Somanur)
            'GSTIN  : 33CLBPS6398M1ZC - Name : P.S.TEXTILES

            txtEIUserID.Text = "API_PS_TEXTILE_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_PS_TEXTILE_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1251" Then '---- SRI SARANYA TEXTILES (THEKKALUR)
            '----GSTIN : 33AATFS3934J1ZQ  -  Name : SRI SARANYA TEXTILES

            txtEIUserID.Text = "API_SARANYATEX_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_SARANYATEX_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1465" Then '---- SARASATHII TEXTIES (SOMANUR)

            'GSTIN:33AJZPK1571E1Z2 - Name: SARASWATHI TEXTILES

            txtEIUserID.Text = "API_SARASWATHII_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_SARASWATHII_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1382" Then '---- VISWAK WEAVING MILLS

            'GSTIN :33BDYPV4492F1ZS - Name : VISWAK WEAVING MILLS

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33BDYPV4492F1ZS")) Then

                txtEIUserID.Text = ""     ' "BDYPV4492F_API_TSS"
                txtEIPassword.Text = ""    ' "RajRock@7417"

                txtEWBUserID.Text = "BDYPV4492F_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1381" Then '---- KRS TEX (PALLADAM) 

            'GSTIN : 33ELNPS1333E1Z6  -  Name : K R S TEXTILES 

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33ELNPS1333E1Z6")) Then

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "KRSTEX_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then 'SREE DHANALAKSHMI TEXTILE

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33ABPFS0629B1ZF")) Then  '--- GSTIN : 33ABPFS0629B1ZF - Name: SREE DHANALAKSHMI TEXTILE

                txtEIUserID.Text = "API_DHANALAKSH_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_DHANALAKSH_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33ACAFS5156K1Z1")) Then  '--- GSTIN : 33ACAFS5156K1Z1 - Name : SHRI GURULAKSHMI TEXTILES 

                txtEIUserID.Text = "GURU1977_API_GSP"
                txtEIPassword.Text = "Guru1977@123"

                txtEWBUserID.Text = "GURU1977_API_GSP"
                txtEWBPassword.Text = "Guru1977@123"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1441" Then 'GRANDMAX

            'GSTIN:33AAICG7351B1ZX - Name: GRANDMAX

            txtEIUserID.Text = "ShastiSudh_API_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "ShastiSudh_API_TSS"
            txtEWBPassword.Text = "RajRock@7417"


            'ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1274" Then '---- KALPANA COTTON AUTOLOOM

            '    'GSTIN :33ADGPV5440G1ZJ - Name : KALPANA TEXTILES

            '    txtEIUserID.Text = ""
            '    txtEIPassword.Text = ""

            '    txtEWBUserID.Text = "KALPANA@20_API_TS2"
            '    txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1404" Then 'MJK TEX
            If Trim(UCase(txt_GSTIN.Text)) = UCase("33ANJPJ9872B1ZX") Then
                'GSTIN :33ANJPJ9872B1ZX - Name : MJK TEXTILES

                txtEIUserID.Text = "mjktex2222_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "mjktex2222_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = UCase("33BIPPM2591C1Z9") Then

                'GSTIN :33BIPPM2591C1Z9 - Name : JVR SPINNING AND WEAVING MILLS 

                txtEIUserID.Text = "BIPPM_2591_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "BIPPM_2591_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1264" Then '---- SRI SUBBULAKSHMI & C0 (SOMANUR)

            '  GSTIN:33AAUFS8184P2Z0 - Name: SRI SUBBULAKSHMI AND CO 

            txtEIUserID.Text = "API_SUBBULAKS_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_SUBBULAKS_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1410" Then '---SAKTHI TEXTILES (ERODE)
            '----GSTIN:33AATFS3172J1ZS - Name: SAKTHY TEXTILE 

            txtEIUserID.Text = "API_SAKTHITEX_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_SAKTHITEX_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1172" Then    '------ SRI RAM WEAVING MILL

            If Trim(UCase(txt_GSTIN.Text)) = UCase("33DCGPK2478R1ZY") Then
                '-GSTIN:33DCGPK2478R1ZY - Name: SRI RAM FABRICS

                txtEIUserID.Text = "API_SRIRAMFAB_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_SRIRAMFAB_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = UCase("33BTBPN5798P1ZU") Then
                'GSTIN :33BTBPN5798P1ZU - Name: SRI RAM WEAVING MILL

                txtEIUserID.Text = "1861983S@s_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "1861983S@s_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1476" Then     ' -------------- NAVEEN TEX

            '--GSTIN:33AHIPT6202A1ZT - Name: NAVEEN TEX

            txtEIUserID.Text = "API_NAVEENTEX_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_NAVEENTEX_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1291" Then     ' -------------- UMMED TEXTILE    /   AND Tirupur Gada Center (TIRUPUR)


            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AUMPG4921C1Z1")) Then  '---GSTIN:33AUMPG4921C1Z1 - Name: UMMED TEXTILE

                txtEIUserID.Text = "API_UMMEDTEX_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_UMMEDTEX_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("24ABEFM0725B1ZY")) Then  '---GSTIN:24ABEFM0725B1ZY - Name: MAHAVIR TEXTILE 

                txtEIUserID.Text = "Mahavir@45_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "Mahavir@45_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1239" Then       '---------------  SRI SATHISH MILLS

            '--GSTIN:33ABVFS1665D1ZZ - Name: SRI SATHISH MILLS

            txtEIUserID.Text = "API_SRISATHISH _TSS"    '-- ONE SPACE INCLUDE 
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_SRISATHISH _TSS"
            txtEWBPassword.Text = "RajRock@7417"



        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1474" Then '-----R.K WEAVES (KODVERI)

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33GSUPS45371ZI")) Then
                'GSTIN :33GSUPS45371ZI - Name : RK WEAVERS

                txtEIUserID.Text = "rkweavers@_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "rkweavers@_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AENFS0788J1ZI")) Then  ' SELVANAYAKI AMMAN TEXTILES

                'GSTIN:33AENFS0788J1ZI - Name: SELVANAYAKI AMMAN TEXTILES

                txtEIUserID.Text = "sAENFS0788_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "sAENFS0788_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1475" Then 'SHRI HV TEX
            'GSTIN : CEIPS2666L1ZZ   Name : SHRI HV TEX

            txtEIUserID.Text = ""
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "CEIPS2666L_API_TSS"
            txtEWBPassword.Text = "RajRock@7417"



        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1291" Then     ' -------------- UMMED TEXTILE    /   AND Tirupur Gada Center (TIRUPUR)
            'GSTIN:33AUMPG4921C1Z1 - Name: UMMED TEXTILE

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AUMPG4921C1Z1")) Then
                txtEIUserID.Text = "API_UMMEDTEX_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_UMMEDTEX_TSS"
                txtEWBPassword.Text = "RajRock@7417"
            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1479" Then      ''-----------     Ramya Tex

            'GSTIN :33JTYPS7267B1ZV - Name : Ramya Tex

            txtEIUserID.Text = ""
            txtEIPassword.Text = ""

            txtEWBUserID.Text = "RAMYATEX20_API_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1273" Then     ' -------------- SHREE DEVI TEXTILES

            '--GSTIN:33ADFFS3899B1ZZ - Name: SHREE DEVI TEXTILES

            txtEIUserID.Text = "API_SHREEDEVI_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_SHREEDEVI_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1377" Then     ' -------------- KURINJHI WEAVING MILLS

            '-----GSTIN:33AGYPJ6774E1ZW - Name: KURINJHI WEAVING MILLS


            txtEIUserID.Text = "API_KURINJHI_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_KURINJHI_TSS"
            txtEWBPassword.Text = "RajRock@7417"




        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1482" Then '---- SENTHUR FAB(PALLADAM)
            '----GSTIN :33BSUPB0471M1ZJ - Name : SHREE SELVAKUMAR MILL 

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33BSUPB0471M1ZJ")) Then    '----GSTIN :33BSUPB0471M1ZJ - Name : SHREE SELVAKUMAR MILL 

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "SHREESELVA_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33ABEFS6826K1ZW")) Then    '---GSTIN :33ABEFS6826K1ZW - Name : SENTHUR TEXTILES - 

                txtEIUserID.Text = "senthurtex_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "senthurtex_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AXVPS2080C2ZB")) Then    '---GSTIN :33AXVPS2080C2ZB - Name : SENTHUR FAB 

                txtEIUserID.Text = "SENTHURFAB_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "SENTHURFAB_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1061" Then '---- Prakash Cottex (Sulur)

            '-----GSTIN:33AAPFP4549J1ZS - Name: PRAKASH COTEX INDIA LLP

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AAPFP4549J1ZS")) Then
                txtEIUserID.Text = "API_PRAKASHTEX_TSS"
                txtEIPassword.Text = "Algo2Prak@25" '"RajRock@7417"

                txtEWBUserID.Text = "API_PRAKASHTEX_TSS"
                txtEWBPassword.Text = "Algo2Prak@25" '"RajRock@7417"  ANOTHER SOFTWARE COMPANY CHANGE THE PASSWORD RajRock@7417  - TO - Algo2Prak@25 ( ALGO SOFTWARE )

                'Algo2Prak@25

            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33ACAPV0723B1Z9")) Then

                txtEIUserID.Text = "GREENCOSYN_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "GREENCOSYN_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1484" Then '---- SR TEXTILE KANIYUR
            'GSTIN:33AGZPP4150B2ZC - Name : S.R.TEXTILES

            txtEIUserID.Text = ""
            txtEIPassword.Text = ""

            txtEWBUserID.Text = "srtextile2_API_TSS"
            txtEWBPassword.Text = "RajRock@7417"


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1461" Then '---- AMBAL TEXTILES (VIJAYAMANGALAM)

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AGWPD2994C1ZA")) Then   '--- GSTIN :33AGWPD2994C1ZA - Name : AMBAL TEXTILES

                txtEIUserID.Text = "kambal2017_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "kambal2017_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AVMPP8165A1ZI")) Then    '---GSTIN :33AVMPP8165A1ZI - Name : ARUTHRA FABRICS

                txtEIUserID.Text = "aruth2017_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "aruth2017_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AIDPV1261H1ZE")) Then    '---GSTIN  : 33AIDPV1261H1ZE - Name : AADHAVAN TEXTILE

                txtEIUserID.Text = "kadh2017_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "kadh2017_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1207" Then     ' -------------- BALAKUMAR TEXTILES

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AMMPB8598G1ZP")) Then
                'GSTIN:33AMMPB8598G1ZP -  Name : BKT FABRICS

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "BKTFABRICS_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If
            'GSTIN:33ACSPC4257D2ZS - Name : BALAKUMAR TEXTILES - 

            'txtEWBUserID.Text = "BKTEXTILES_API_TSS"
            'txtEWBPassword.Text = "RajRock@7417"


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1487" Then ' SV TEXTILE MILLS 
            '   GSTIN:33LEHPS1563Q1ZN - Name : SV Textile MILLS

            txtEIUserID.Text = ""
            txtEIPassword.Text = ""

            txtEWBUserID.Text = "Vishalsv_API_TSS"
            txtEWBPassword.Text = "RajRock@7417"

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1276" Then '  DHANVI IMPEX 
            'GSTIN:33GWHPS1692P1ZN - Name: DHANVI IMPEX

            txtEIUserID.Text = "API_DHANVI_TSS"
            txtEIPassword.Text = "RajRock@7417"

            txtEWBUserID.Text = "API_DHANVI_TSS"
            txtEWBPassword.Text = "RajRock@7417"


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1489" Then '-----K V P WEAVES (ANNUR)

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AAYFK7632A1Z9")) Then    '---GSTIN :33AAYFK7632A1Z9 - Name : KVP WEAVES - User : Tax Payer

                txtEIUserID.Text = "kvpweaves_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "kvpweaves_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1490" Then '---- LAKSHMI  SARASWATHI EXPORTS (THIRUCHENCODE)
            '---GSTIN : 33AGCPR8502J1ZD - Name: TVL. RAJA. PRAVEEN PROP. SRI LAKSHMI SARASWATHI EXPORTS

            If Trim(UCase(txt_GSTIN.Text)) = "33AGCPR8502J1ZD" Then     '---SRI LAKSHMI SARASWATHI EXPORTS

                txtEIUserID.Text = "SLS_339031_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "SLS_339031_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1491" Then '---- JAYAMAHESH TEX (KARUMATHAMPATTI - SEGUDANTAKLI)

            '-----GSTIN :33AJVPJ4055E1Z6 - Name : JAYAMAHESH TEX - User : Tax Payer
            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AJVPJ4055E1Z6")) Then

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "SAJVPJ4055_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1211" Then '---- SREE SAMY TEXTILES
            '-----GSTIN:33BBNPS7221Q1ZY - Name: SREE SAMI TEXTILES ---TEXTILE 
            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33BBNPS7221Q1ZY")) Then

                txtEIUserID.Text = "API_SREESAMITEX_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_SREESAMITEX_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1360" Then '---- Ashoka Textile (63.Velampalayam - Palladam)

            '-----GSTIN : 33AHCPM2698H1Z8
            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AHCPM2698H1Z8")) Then

                txtEIUserID.Text = "AHCPM2698H_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "AHCPM2698H_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1139" Then '---- SIVAKUMAR Textiles (THEKKALUR)


            '------- GSTIN : 33CPHPK6918R1Z6 - Name : SIVAKUMAR TEXTILES

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33CPHPK6918R1Z6")) Then

                txtEIUserID.Text = "API_SIVAKUMARTX_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_SIVAKUMARTX_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1161" Then '---- SHREE HARIRAM COTTON MILLS (KARUMATHAMPATTI)

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33CAXPS1797R1ZA")) Then   '------- GSTIN : 33CAXPS1797R1ZA - Name : JAYAVARSINI TEX

                txtEIUserID.Text = "API_JAYAVARSINI_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_JAYAVARSINI_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1268" Then '---- GAYATHRI TEXTILES (KOVILPALAYAM)

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AAIPV6459J1Z3")) Then   '------- GSTIN : 33AAIPV6459J1Z3 - Name : GAYATHRI TEXTILES

                txtEIUserID.Text = "API_GAYATHRITEX_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_GAYATHRITEX_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1495" Then '---- RAJALAKSHMI SPINNING MILLSS (SOMANUR)


            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AADFR3018C1ZV")) Then   '------- GSTIN : 33AADFR3018C1ZV - Name : RAJALAKSHMI MILLSS

                txtEIUserID.Text = "API_RAJALAKSHMI_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_RAJALAKSHMI_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1162" Then    '------- VIJAY TEXTILES (PALLADAM)

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AAEPE5300R1ZU")) Then   '-------   GSTIN:33AAEPE5300R1ZU - Name: VIJAY TEX

                txtEIUserID.Text = "API_VIJAYTEX_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_VIJAYTEX_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1497" Then '---- SRI S.N TEXTILE(MANGALAM-VELAYUTHAMPALAYAM)

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33BWWPK3771M1ZR")) Then   '-------  GSTIN :33BWWPK3771M1ZR - Name : SRI S.N TEXTIILE

                txtEIUserID.Text = "srisntexti_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "srisntexti_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1498" Then '---- SRI VISHNU TEX (somanur)

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33ADLPT7660B1ZG")) Then   '-------  GSTIN :33ADLPT7660B1ZG - Name : SRI VISHNU TEX 

                txtEIUserID.Text = "srivishnut_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "srivishnut_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1026" Then '---- GSTIN:33DOYPD6128B1Z0 - Name: VIJAYALAKSHMI TEXTILES  

            If Trim(UCase(txt_GSTIN.Text)) = "33DOYPD6128B1Z0" Then

                txtEIUserID.Text = "API_VIJAYALAKSM_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_VIJAYALAKSM_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1303" Then '---- SKSS TEXTILES (SOMANUR)

            If Trim(UCase(txt_GSTIN.Text)) = UCase("33ADDFS3645R1ZK") Then          '---1 
                ' Name : SKSS TEXTILES
                txtEIUserID.Text = "sksstextil_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "sksstextil_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = UCase("33LJLPS4208K1ZO") Then                          '----2
                'GSTIN : 33LJLPS4208K1ZO        Name : KAY YES FABRICS

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "kayyesfab2_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1522" Then '---- BAVANA TEXTILES (somanur)

            If Trim(UCase(txt_GSTIN.Text)) = "33AEAPN0127R1ZI" Then  '--- GSTIN : 33AEAPN0127R1ZI - Name : BAVANA TEXTILES 

                txtEIUserID.Text = "BAVANATEXT_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "BAVANATEXT_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1440" Then

            If Trim(UCase(txt_GSTIN.Text)) = "33ANEPT4940K1ZQ" Then  '---GSTIN :33ANEPT4940K1ZQ - Name : M/S. SUBIKSHA TEXTILES - User : Tax Payer

                txtEIUserID.Text = "abc-338963_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "abc-338963_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33ABUFM9381P1Z4" Then  '---GSTIN :33ABUFM9381P1Z4 - Name : MURUGAN MILLS - User : Tax Payer

                txtEIUserID.Text = "sABUFM9381_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "sABUFM9381_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1266" Then

            If Trim(UCase(txt_GSTIN.Text)) = "33APHPP3473J1ZN" Then  '---GSTIN:33APHPP3473J1ZN - Name: CORAL WEAVER

                txtEIUserID.Text = "API_CORALWEAVES_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "API_CORALWEAVES_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If



        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1256" Then '--- CLS TEXTLES (KARAMADAI)

            If Trim(UCase(txt_GSTIN.Text)) = "33AAKFC2406J1ZQ" Then  '---GSTIN :33AAKFC2406J1ZQ - Name : C.L.S.TEXTILES

                txtEIUserID.Text = "33AAKFC240_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "33AAKFC240_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If



        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1444" Then '--- GOMATHI TEXTILES (MANGALAM-IDUVAI)

            If Trim(UCase(txt_GSTIN.Text)) = "33ASUPV7766Q1Z8" Then  '---GSTIN : 33ASUPV7766Q1Z8 - Name : GOMATHI TEXTILES 

                txtEIUserID.Text = "GOMATHI198_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "GOMATHI198_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1313" Then   '----SRI GURU FABRICS

            'GSTIN :33AQUPK7981L1ZW - Name : SRI GURU FABRICS - User : Tax Payer

            If Trim(UCase(txt_GSTIN.Text)) = "33AQUPK7981L1ZW" Then


                txtEIUserID.Text = "gurufabric_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "gurufabric_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1123" Then  '---SRI NIKITHA SIZING MILLS

            'GSTIN :33ACFFS3952A1ZH - Name : SRI NIKITHA SIZING MILLS - User : Tax Payer


            If Trim(UCase(txt_GSTIN.Text)) = "33ACFFS3952A1ZH" Then


                txtEIUserID.Text = "33ACFFS395_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "33ACFFS395_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33ACFFS3952A1ZH" Then


                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "33ADOPN889_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"


                'GSTIN:33ADOPN8898C1ZZ - Name : SRI SAANTHI SIZING MILL - User : Tax Payer


            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1526" Then '---- VADIVEL TEXTILES (KARUVALUR)


            If Trim(UCase(txt_GSTIN.Text)) = "33AAGFV7354L2ZP" Then  '---GSTIN : 33AAGFV7354L2ZP - Name VADIVEL TEXTILES


                txtEIUserID.Text = "AAGFV7354L_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "AAGFV7354L_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1020" Then '-----SRI MATHESWARAN TEXTILES

            'GSTIN :33GGBPS3521M1Z8-Name: SRI MATHESWARAN TEXTILES - User: Tax Payer


            If Trim(UCase(txt_GSTIN.Text)) = "33GGBPS3521M1Z8" Then


                txtEIUserID.Text = "SRIMATHESW_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "SRIMATHESW_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1530" Then '---- RAJA MOHAN COTTON MILLS (ERODE)

            If Trim(UCase(txt_GSTIN.Text)) = "33AEQPK6199G1Z0" Then   '---GSTIN : 33AEQPK6199G1Z0 - Name : RAJAMOHAN COTTON MILLS 

                txtEIUserID.Text = "RMCM_ERODE_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "RMCM_ERODE_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "37ANDPG2663H1Z3" Then   '---GSTIN :37ANDPG2663H1Z3 - Name : NIKHIL TEXTILES 

                txtEIUserID.Text = "nikhiltext_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "nikhiltext_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33FKRPS6461P1Z1" Then   '---GSTIN :33FKRPS6461P1Z1 - Name : SRI GRISHMA ENTERPRISES 

                txtEIUserID.Text = "Grishug_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "Grishug_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1512" Then '---- SRI SUBRAMANIA TEX(PALLADAM)

            If Trim(UCase(txt_GSTIN.Text)) = "33FVPPS8278K1ZG" Then   '---GSTIN : 33FVPPS8278K1ZG - Name : SRI SUBRAMANIA TEX

                txtEIUserID.Text = "subraman_k_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "subraman_k_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1514" Then '---- SRI bagyalakshmi TEX(somanur)

            If Trim(UCase(txt_GSTIN.Text)) = "33AJBPD0226H1Z1" Then   '---GSTIN :33AJBPD0226H1Z1 - Name : SRI BHAGYALAKSHMI MILLS - User : Tax Payer

                txtEIUserID.Text = "sbmills_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "sbmills_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Then '----  LOURDU MATHA TEXTILE

            If Trim(UCase(txt_GSTIN.Text)) = "33ACNFS9124N1ZI" Then   '------------GSTIN :33ACNFS9124N1ZI - Name : St. LOURDU MATHA TEX - User : Tax Payer

                txtEIUserID.Text = "STLOURDU-9_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "STLOURDU-9_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1469" Then '---- R.S.S TEX(PALLADAM)

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33BRPPR7138M1Z0")) Then
                '--- GSTIN :33BRPPR7138M1Z0 - Name : RSS TEX 

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "rsstex123_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33ACFFA3991D1ZM")) Then ' -- ARAV TEX

                'GSTIN :33ACFFA3991D1ZM - Name : ARAV TEX - User : Tax Payer

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "Aravtex@42_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AUWPJ2311D1ZY")) Then ' -- ARAV TEX

                'GSTIN :33AUWPJ2311D1ZY - Name : ANUGRAH TEXTILES - User : Tax Payer

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "JAYAKUMAR@_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"
            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1539" Then '---- ASIAN ASSOCIATES (COIMBATORE)

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33BFCPS5442C1ZR")) Then
                'GSTIN :33BFCPS5442C1ZR - Name : SHREE SAKTHI TEX - User : TaX

                txtEIUserID.Text = "Sakthi@Tex_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "Sakthi@Tex_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"
            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1414" Then '---- SRI PERIYANAYAKI AMMAN TEXTILES

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33ACDFS6341P1ZR")) Then
                'GSTIN :33ACDFS6341P1ZR   Name : SRI MAHALAKSHMI TEXTILES

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "ACDFS6341P_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33BXPPS8149R1Z8")) Then
                'GSTIN :33BXPPS8149R1Z8  Name : SRI PERIYANAYAKI AMMAN TEXTILES 

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "BXPPS8149R_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1446" Then '---- KALIYA DEVI TEXTILES  & BOMADEVI HANDLOOMS

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AAPFK2480B1ZK")) Then
                'GSTIN :33AAPFK2480B1ZK - Name : KALIYADEVI TEXTILES - User : Tax Payer

                txtEIUserID.Text = "AAPFK2480B_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "AAPFK2480B_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Then '---- KALIYA DEVI TEXTILES  & BOMADEVI HANDLOOMS

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AAPFK2480B1ZK")) Then
                'GSTIN :33AAPFK2480B1ZK - Name : KALIYADEVI TEXTILES - User : Tax Payer

                txtEIUserID.Text = "AAPFK2480B_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "AAPFK2480B_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"
            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33BGNPV0002K1ZG")) Then ' ---- BOMADEVI HANDLOOMS ( BOMADEVI ) -1
                'GSTIN : 33BGNPV0002K1ZG - Name : BOMADEVI HANDLOOMS - User : Tax Payer

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "bomadevi_1_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33HWNPS6963J1ZK")) Then 'SOWDESWARI TEXTILES ( BOMADEVI ) -2
                'GSTIN :33HWNPS6963J1ZK - Name : SOWDESWARI TEXTILES - User : Tax Payer

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "HWNPS6963J_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"
            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1350" Then '---- MALAR COTTON (VALAYAPALAYAM, 63.VELAMPALAYAM)

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33ABMFM8487C1ZY")) Then    ' ----GSTIN :33ABMFM8487C1ZY - Name : MALAR COTTON - User : Tax Payer

                txtEIUserID.Text = "malarcotto_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "malarcotto_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1494" Then '---- SRII SABA TEX (KARUMATHAMPATTI)

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AQHPG0794L1ZO")) Then   '---GSTIN :33AQHPG0794L1ZO - Name : SRII SABA TEX - User : Tax Payer

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "SABATEX_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33BQDPS9310P1ZD")) Then  '---GSTIN :33BQDPS9310P1ZD - Name : THULASI TEXTILE MILL - User : Tax Payer

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "THULASI_TE_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AATFM3426H1Z6")) Then   'GSTIN :33AATFM3426H1Z6 - Name : MANI OMEGA FABRICS  - User : Tax Payer

                txtEIUserID.Text = "Vishakar@5_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "Vishakar@5_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1542" Then '---- G M TEX (THEKKALUR)

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AAPFG6821Q1ZS")) Then   'GSTIN :33AAPFG6821Q1ZS - Name : G M TEX - User : Transporter

                txtEIUserID.Text = "DHILAN@199_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "DHILAN@199_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then  '---- SRI SRINIVASA TEXTILES (PALLADAM)

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AAEFV5019L1Z2")) Then   'GSTIN : 33AAEFV5019L1Z2 - Name : SRI SRINIVASA TEXTILES - User : Tax Payer

                txtEIUserID.Text = "VIRILE2018_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "VIRILE2018_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1546" Then '---- PREETHI TEX (SOMANUR)

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33FVZPS2223C1ZE")) Then   'GSTIN :33FVZPS2223C1ZE - Name : PREETHI TEX - User : Tax Payer

                txtEIUserID.Text = "preethitex_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "preethitex_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1323" Then '---------- MERLIN ROSE TEXTILES (SOMANUR)

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AJBPM6453B1ZN")) Then   'GSTIN : 33AJBPM6453B1ZN - Name : MERLIN ROSE TEXTILES

                txtEIUserID.Text = "MERLINROSE_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "MERLINROSE_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1007" Then '----SRI RAJA TEXTILES (PALLADAM)

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33ABEFS3600B1ZX")) Then   'GSTIN : 33ABEFS3600B1ZX - Name : SRI RAJA TEXTILES - User : Tax Payer

                txtEIUserID.Text = "rajatextil_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "rajatextil_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33ADOPL7053K1Z7")) Then   'GSTIN  : 33ADOPL7053K1Z7 - Name : WEAVEMASTER

                txtEIUserID.Text = "weavemaste_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "weavemaste_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1369" Then '---- ANCHANEYA TEXTILE INDUSTRIES(KARUMATHAMPATTI)


            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33BXTPS6626M1ZK")) Then  '---GSTIN : 33BXTPS6626M1ZK - Name : ANCHANEYA TEXTILE INDUSTRIES - User : Tax Payer

                txtEIUserID.Text = "Anchaneyat_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "Anchaneyat_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33BXTPS6626M3ZI")) Then  '---GSTIN : 33BXTPS6626M3ZI - Name : ANCHANEYA TEXSPINNERS - User : Tax Payer

                txtEIUserID.Text = "anchaneya@_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "anchaneya@_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1242" Then    ' --- SOUTHERN SAREES

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AAOFS5766R1Z5")) Then
                'GSTIN :- 33AAOFS5766R1Z5     NAME :- SOUTHERN SAREESS

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "aaofs5766r_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1329" Then '---------- VASANTHAMANI TEX PALLADAM

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AHOPV6903E1Z5")) Then
                'GSTIN :33AHOPV6903E1Z5 - Name : VASANTHAMANI TEX - User : Tax Payer

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "sAHOPV6903_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1559" Then '---- ( VINOTH TEXTILE & GOWRI MILLS )

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AJJPV5447Q1ZB")) Then '- VINOTH TEX 
                'GSTIN :33AJJPV5447Q1ZB - Name : VINOTH TEX - User : Tax Payer     -   EWB ONLY 

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "VINOTHTEX0_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AJAPV5178P1ZI")) Then '- SRI GOWRI MIILS  
                'GSTIN :33AJAPV5178P1ZI - Name : SRI GOWRI MIILS - User : Tax Payer    --------- -EINVOICE AND  EWB ONLY 

                txtEIUserID.Text = "SRIGOWRIMI_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "SRIGOWRIMI_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"
            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1578" Then '---- AMARNATH MILLS (ERODE)

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33CMYPS0927E1ZP")) Then '--- GSTIN :33CMYPS0927E1ZP - Name : V.P.S.TEXTILES 
                'GSTIN :33CMYPS0927E1ZP - Name : V.P.S.TEXTILES 

                txtEIUserID.Text = "vpstex123_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "vpstex123_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"



            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1582" Then '----  SREEMATHI TEX

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33GYWPS2023F1Z6")) Then '- SREEMATHI TEX

                'GSTIN :33GYWPS2023F1Z6 - Name : SREEMATHI TEX - User : Tax Payer    -   EWB ONLY 

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "sreemathit_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33DYRPR5368Q1Z6")) Then '-----Subraj Mill

                'GSTIN :33DYRPR5368Q1Z6 - Name : SUBRAJ MILLS - User : Tax Payer  -   EWB ONLY 

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "subrajmill_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1583" Then '---------- Finecraft lenins

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AXGPN5960K1Z4")) Then
                'GSTIN: 33AXGPN5960K1Z4 - NAME : Finecraft lenins - User : Tax Payer

                txtEIUserID.Text = "33AXGPN596_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "33AXGPN596_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1547" Then '---------- SHREE SVS FABRICS

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AFDFS1334K1Z4")) Then
                ' GSTIN :33AFDFS1334K1Z4 - Name : SHREE SVS FABRICS - User : Tax Payer

                txtEIUserID.Text = "SSVF@2023_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "SSVF@2023_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1587" Then '--------------- SARVESWARA TEXTILES


            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33ACYFS7967E1ZB")) Then
                'GSTIN :33ACYFS7967E1ZB - Name SARVESWARA TEXTILES : - User : Tax Payer

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "Esarve_100_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1589" Then '--------------- SRI VELAVA TEX


            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33MBDPS4330P1Z3")) Then
                'GSTIN :33MBDPS4330P1Z3 - Name : SRI SATHIYA TEX - User : Tax Payer

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "srisathyat_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33CPYPP2655F1ZF")) Then
                'GSTIN :33CPYPP2655F1ZF - Name : SRI VELAVA TEXTILES - User : Tax Payer

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "srivelavat_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1164" Then '---- SAROJINI TEXTILES - 63 VELAMPALAYAM

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33EDEPS9471Q1ZK")) Then

                'GSTIN :33EDEPS9471Q1ZK - Name : SAROJINI TEXTILES - User : Tax Payer

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "sarojinite_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1497" Then

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33EDEPS9471Q1ZK")) Then

                'GSTIN :33EDEPS9471Q1ZK - Name : SAROJINI TEXTILES - User : Tax Payer

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "sarojinite_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1594" Then

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33BJKPA1942H1ZL")) Then

                'GSTIN :33BJKPA1942H1ZL - Name : ANGATHAL TEXTILES - User : Tax Payer

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "ANGATHAL-1_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33DNDPK8313P2ZN")) Then

                'GSTIN :33DNDPK8313P2ZN - Name : K TEX - User : Tax Payer

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "KTEXVAAGAI_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"



            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33ALWPT0050A1ZC")) Then

                'GSTIN:33ALWPT0050A1ZC - Name : JAI KRISHNAA TEXTILES - User : Tax Payer


                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "ALWPT_0050_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33CEPPS4773H1ZX")) Then

                'GSTIN:33CEPPS4773H1ZX - Name : PONSELVAVINAYAGAR TEX - User : Tax Payer


                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "CEPPS4773H_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            End If



        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1294" Then '--------CHITRA TEX



            If Trim(UCase(txt_GSTIN.Text)) = "33BJBPC2351M2ZJ" Then


                ' GSTIN :33BJBPC2351M2ZJ - Name : SRI MURUGAN TEXTILES - User : Tax Payer

                txtEIUserID.Text = "33BJBPC235_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "33BJBPC235_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            ElseIf Trim(UCase(txt_GSTIN.Text)) = "33AAEPE1429P1ZS" Then

                'GSTIN:33AAEPE1429P1ZS - Name : SANJEEV TEXTILES - User : Tax Payer

                txtEIUserID.Text = "sanjeevela_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "sanjeevela_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"



            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1254" Then '------- SMT FABRICS

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33BCMPT4107Q1Z1")) Then

                'GSTIN :33BCMPT4107Q1Z1 - Name : SRI MAHALAKSHMI TEXTILES - User : Tax Payer

                txtEIUserID.Text = "SMLTEXTILE_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "SMLTEXTILE_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1075" Then '----- J R TEX


            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33BKZPS3308D1ZY")) Then

                'GSTIN :33BKZPS3308D1ZY - Name : J R TEX - User : Tax Payer

                txtEIUserID.Text = "JRTEXTILES_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "JRTEXTILES_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1135" Then '----- MARIYA FABS


            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AFNPP6211J1ZC")) Then

                'GSTIN :33AFNPP6211J1ZC - Name : MARIYA FABS - User : Tax Payer

                txtEIUserID.Text = "MARIYAFABS_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "MARIYAFABS_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1612" Then    '---ISHANVI TEX (PERUNDURAI)

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33BCKPN4467R1ZS")) Then

                'GSTIN :33BCKPN4467R1ZS - Name : ISHANVI TEX - User : Tax Payer

                txtEIUserID.Text = "ISHANVITEX_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "ISHANVITEX_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AJKPP0364K1Z2")) Then

                'GSTIN :33AJKPP0364K1Z2 - Name : SREE NAVEEN TEX - User : Tax Payer

                txtEIUserID.Text = "Naveen-633_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "Naveen-633_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1608" Then '---- SAMANTH TEXTILES (SOMANUR)

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AXIPP5028L1Z7")) Then

                'GSTIN : 33AXIPP5028L1Z7 - Name : SAMANTH TEXTILES - User : Tax Payer

                txtEIUserID.Text = "samanthtex_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "samanthtex_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"



            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33BCYPA8410M1ZA")) Then

                'GSTIN:33BCYPA8410M1ZA - Name : NAVEEN TEX - User : Transporter

                txtEIUserID.Text = "33BCYPA841_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "33BCYPA841_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1254" Then '---- SMT FABRICS (POOMALUR)

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33BCMPT4107Q1Z1")) Then

                'GSTIN :33BCMPT4107Q1Z1 - Name : SRI MAHALAKSHMI TEXTILES - User : Tax Payer

                txtEIUserID.Text = "SMLTEXTILE_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "SMLTEXTILE_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1224" Then  ' ---  KAVITHAA FABRICS

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33APOPS7604D1ZT")) Then

                'GSTIN :33APOPS7604D1ZT - Name : KAVITHAA FABRICS - User : Tax Payer

                txtEIUserID.Text = "KAVITHAA@_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "KAVITHAA@_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AAWFK1543L1ZY")) Then

                'GSTIN :33AAWFK1543L1ZY - Name : KAVITHAA TEXTILES - User : Tax Payer


                txtEIUserID.Text = "KAVITHAATE_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "KAVITHAATE_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33ABNFS4784R1Z4")) Then

                'GSTIN :33ABNFS4784R1Z4 - Name : SWAATHI FABRICS - User : Tax Payer

                txtEIUserID.Text = "KAVITHA@_API_TSS"
                txtEIPassword.Text = "Swaathi@1234"

                txtEWBUserID.Text = "KAVITHA@_API_TSS"
                txtEWBPassword.Text = "Swaathi@1234"


            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1614" Then '' SUPREME TEXTILES


            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33HUKPS1028F1ZM")) Then

                'GSTIN :33HUKPS1028F1ZM - Name : SUPREME TEXTILES - User : Tax Payer 

                txtEIUserID.Text = "TTTUP-1234_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "TTTUP-1234_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1592" Then ' -- LAKSHANA SHREE TEX

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33ESMPP4279L1ZY")) Then

                'GSTIN :33ESMPP4279L1ZY - Name : KIRUTHIK IMPEX - User : Tax Payer

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "33ESMPP427_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33DVQPK6394M1ZO")) Then

                'GSTIN :33DVQPK6394M1ZO - Name : VISMITHA SHREE TEX - User : Tax Payer


                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "VISMITHASH_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AAPPE0782N1ZF")) Then

                'GSTIN :33AAPPE0782N1ZF - Name : LAKSHANA SHREE TEX - User : Tax Payer


                txtEIUserID.Text = "33AAPPE078_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "33AAPPE078_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1257" Then ' -- SENTHIL MURUGAN TEX

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33EHUPS2705B1ZC")) Then

                'GSTIN :33EHUPS2705B1ZC - Name : ADVIK FABRICS - User : Tax Payer

                txtEIUserID.Text = "ADVIKFABRI_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "ADVIKFABRI_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33GKAPB2106N1ZK")) Then

                'GSTIN :33GKAPB2106N1ZK - Name : SENTHIL MURUGAN TEXTILES - User : Tax Payer

                txtEIUserID.Text = "BHAKKIYALA_API_TSS"
                txtEIPassword.Text = "RajRock@7417"

                txtEWBUserID.Text = "BHAKKIYALA_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1625" Then ' -- T.M.T.AUTOMATION

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AYDPT4135C1ZR")) Then

                'GSTIN NO :GSTIN :33AYDPT4135C1ZR - Name : T.M.T.AUTOMATION

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "AYDPT4135C_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"


            ElseIf Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AOTPR4602Q1Z8")) Then

                'GSTIN :33AOTPR4602Q1Z8 - Name : S.R.P TEX - User : Tax Payer

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "33AOTPR460_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1628" Then ' -- SG GROUPS

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33AFNFS4243P1ZE")) Then

                'GSTIN :33AFNFS4243P1ZE - Name : SG GROUPS - User : Tax Payer

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "AYDPT4135C_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1630" Then '---- TIRUPATHI TEXTILE MILLS  

            If Trim(UCase(txt_GSTIN.Text)) = Trim(UCase("33CGSPP0591F1Z5")) Then

                'GSTIN :33CGSPP0591F1Z5 - Name : TIRUPATHI TEXTILE MILLS - User : Tax Payer

                txtEIUserID.Text = ""
                txtEIPassword.Text = ""

                txtEWBUserID.Text = "tirupathi3_API_TSS"
                txtEWBPassword.Text = "RajRock@7417"

            End If

        End If

    End Sub



End Class