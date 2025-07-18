<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GST_EWB_API_SETTINGS
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtASPUserID = New System.Windows.Forms.TextBox()
        Me.lblASPUserID_Caption = New System.Windows.Forms.Label()
        Me.txtGSPName = New System.Windows.Forms.TextBox()
        Me.lbl_GSPName_Caption = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtBaseURL = New System.Windows.Forms.TextBox()
        Me.lbl_BaseURL_Caption = New System.Windows.Forms.Label()
        Me.txtASPPassword = New System.Windows.Forms.TextBox()
        Me.lblASPPassword_Caption = New System.Windows.Forms.Label()
        Me.txtEWBPassword = New System.Windows.Forms.TextBox()
        Me.lblEWBPassword = New System.Windows.Forms.Label()
        Me.txtEWBUserID = New System.Windows.Forms.TextBox()
        Me.lblEWBUserID = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.txt_ShortName = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txt_Name = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txt_IdNo = New System.Windows.Forms.TextBox()
        Me.txt_GSTIN = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtEWBIRNURL = New System.Windows.Forms.TextBox()
        Me.lbl_EWBIRNURL_Caption = New System.Windows.Forms.Label()
        Me.txtEIBaseURL = New System.Windows.Forms.TextBox()
        Me.lbl_EIBaseURL_Caption = New System.Windows.Forms.Label()
        Me.txtEIAuthURL = New System.Windows.Forms.TextBox()
        Me.lbl_EIAuthURL_Caption = New System.Windows.Forms.Label()
        Me.lbl_CancelEWBURL_Caption = New System.Windows.Forms.Label()
        Me.txtCancelEWBURL = New System.Windows.Forms.TextBox()
        Me.txtEIPassword = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtEIUserID = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txt_EInvoiceGSPName = New System.Windows.Forms.TextBox()
        Me.lbl_EInvoiceGSPName_Caption = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.btn_RestoreDefault = New System.Windows.Forms.Button()
        Me.btn_CheckConnectivity = New System.Windows.Forms.Button()
        Me.lbl_Company = New System.Windows.Forms.Label()
        Me.rtbeInvoiceResponse = New System.Windows.Forms.RichTextBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.btn_CheckConnectivity1 = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.rtbEWBResponse = New System.Windows.Forms.RichTextBox()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtASPUserID
        '
        Me.txtASPUserID.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtASPUserID.Location = New System.Drawing.Point(677, 51)
        Me.txtASPUserID.Name = "txtASPUserID"
        Me.txtASPUserID.Size = New System.Drawing.Size(323, 23)
        Me.txtASPUserID.TabIndex = 3
        Me.txtASPUserID.Visible = False
        '
        'lblASPUserID_Caption
        '
        Me.lblASPUserID_Caption.AutoSize = True
        Me.lblASPUserID_Caption.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lblASPUserID_Caption.ForeColor = System.Drawing.Color.Navy
        Me.lblASPUserID_Caption.Location = New System.Drawing.Point(534, 55)
        Me.lblASPUserID_Caption.Name = "lblASPUserID_Caption"
        Me.lblASPUserID_Caption.Size = New System.Drawing.Size(70, 15)
        Me.lblASPUserID_Caption.TabIndex = 2
        Me.lblASPUserID_Caption.Text = "ASP User ID"
        Me.lblASPUserID_Caption.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblASPUserID_Caption.Visible = False
        '
        'txtGSPName
        '
        Me.txtGSPName.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGSPName.Location = New System.Drawing.Point(141, 115)
        Me.txtGSPName.Name = "txtGSPName"
        Me.txtGSPName.Size = New System.Drawing.Size(323, 23)
        Me.txtGSPName.TabIndex = 7
        Me.txtGSPName.Visible = False
        '
        'lbl_GSPName_Caption
        '
        Me.lbl_GSPName_Caption.AutoSize = True
        Me.lbl_GSPName_Caption.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lbl_GSPName_Caption.ForeColor = System.Drawing.Color.Navy
        Me.lbl_GSPName_Caption.Location = New System.Drawing.Point(23, 119)
        Me.lbl_GSPName_Caption.Name = "lbl_GSPName_Caption"
        Me.lbl_GSPName_Caption.Size = New System.Drawing.Size(92, 15)
        Me.lbl_GSPName_Caption.TabIndex = 6
        Me.lbl_GSPName_Caption.Text = "EWB GSP Name"
        Me.lbl_GSPName_Caption.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lbl_GSPName_Caption.Visible = False
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.DarkSlateGray
        Me.Label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label8.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label8.Font = New System.Drawing.Font("Calibri", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.White
        Me.Label8.Location = New System.Drawing.Point(0, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(1032, 40)
        Me.Label8.TabIndex = 38
        Me.Label8.Text = "GST  e-INVOICE && EWB API SETTINGS"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtBaseURL
        '
        Me.txtBaseURL.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBaseURL.Location = New System.Drawing.Point(141, 161)
        Me.txtBaseURL.Name = "txtBaseURL"
        Me.txtBaseURL.Size = New System.Drawing.Size(323, 23)
        Me.txtBaseURL.TabIndex = 9
        Me.txtBaseURL.Visible = False
        '
        'lbl_BaseURL_Caption
        '
        Me.lbl_BaseURL_Caption.AutoSize = True
        Me.lbl_BaseURL_Caption.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lbl_BaseURL_Caption.ForeColor = System.Drawing.Color.Navy
        Me.lbl_BaseURL_Caption.Location = New System.Drawing.Point(23, 165)
        Me.lbl_BaseURL_Caption.Name = "lbl_BaseURL_Caption"
        Me.lbl_BaseURL_Caption.Size = New System.Drawing.Size(84, 15)
        Me.lbl_BaseURL_Caption.TabIndex = 8
        Me.lbl_BaseURL_Caption.Text = "EWB Base URL"
        Me.lbl_BaseURL_Caption.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lbl_BaseURL_Caption.Visible = False
        '
        'txtASPPassword
        '
        Me.txtASPPassword.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtASPPassword.Location = New System.Drawing.Point(677, 78)
        Me.txtASPPassword.Name = "txtASPPassword"
        Me.txtASPPassword.Size = New System.Drawing.Size(323, 23)
        Me.txtASPPassword.TabIndex = 5
        Me.txtASPPassword.Visible = False
        '
        'lblASPPassword_Caption
        '
        Me.lblASPPassword_Caption.AutoSize = True
        Me.lblASPPassword_Caption.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lblASPPassword_Caption.ForeColor = System.Drawing.Color.Navy
        Me.lblASPPassword_Caption.Location = New System.Drawing.Point(534, 82)
        Me.lblASPPassword_Caption.Name = "lblASPPassword_Caption"
        Me.lblASPPassword_Caption.Size = New System.Drawing.Size(83, 15)
        Me.lblASPPassword_Caption.TabIndex = 4
        Me.lblASPPassword_Caption.Text = "ASP Password"
        Me.lblASPPassword_Caption.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblASPPassword_Caption.Visible = False
        '
        'txtEWBPassword
        '
        Me.txtEWBPassword.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEWBPassword.Location = New System.Drawing.Point(141, 69)
        Me.txtEWBPassword.Name = "txtEWBPassword"
        Me.txtEWBPassword.Size = New System.Drawing.Size(323, 23)
        Me.txtEWBPassword.TabIndex = 13
        '
        'lblEWBPassword
        '
        Me.lblEWBPassword.AutoSize = True
        Me.lblEWBPassword.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lblEWBPassword.ForeColor = System.Drawing.Color.Navy
        Me.lblEWBPassword.Location = New System.Drawing.Point(23, 73)
        Me.lblEWBPassword.Name = "lblEWBPassword"
        Me.lblEWBPassword.Size = New System.Drawing.Size(87, 15)
        Me.lblEWBPassword.TabIndex = 12
        Me.lblEWBPassword.Text = "EWB Password"
        Me.lblEWBPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtEWBUserID
        '
        Me.txtEWBUserID.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEWBUserID.Location = New System.Drawing.Point(141, 23)
        Me.txtEWBUserID.Name = "txtEWBUserID"
        Me.txtEWBUserID.Size = New System.Drawing.Size(323, 23)
        Me.txtEWBUserID.TabIndex = 11
        '
        'lblEWBUserID
        '
        Me.lblEWBUserID.AutoSize = True
        Me.lblEWBUserID.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lblEWBUserID.ForeColor = System.Drawing.Color.Navy
        Me.lblEWBUserID.Location = New System.Drawing.Point(23, 27)
        Me.lblEWBUserID.Name = "lblEWBUserID"
        Me.lblEWBUserID.Size = New System.Drawing.Size(74, 15)
        Me.lblEWBUserID.TabIndex = 10
        Me.lblEWBUserID.Text = "EWB User ID"
        Me.lblEWBUserID.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.Color.Navy
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ForeColor = System.Drawing.Color.White
        Me.btnSave.Location = New System.Drawing.Point(839, 450)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(76, 31)
        Me.btnSave.TabIndex = 24
        Me.btnSave.TabStop = False
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.Color.Navy
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ForeColor = System.Drawing.Color.White
        Me.btnClose.Location = New System.Drawing.Point(928, 450)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(76, 31)
        Me.btnClose.TabIndex = 25
        Me.btnClose.TabStop = False
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'txt_ShortName
        '
        Me.txt_ShortName.Enabled = False
        Me.txt_ShortName.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_ShortName.Location = New System.Drawing.Point(167, 104)
        Me.txt_ShortName.Name = "txt_ShortName"
        Me.txt_ShortName.Size = New System.Drawing.Size(323, 23)
        Me.txt_ShortName.TabIndex = 42
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label6.ForeColor = System.Drawing.Color.Navy
        Me.Label6.Location = New System.Drawing.Point(23, 108)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(127, 15)
        Me.Label6.TabIndex = 44
        Me.Label6.Text = "Company Short Name"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_Name
        '
        Me.txt_Name.Enabled = False
        Me.txt_Name.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Name.Location = New System.Drawing.Point(167, 77)
        Me.txt_Name.Name = "txt_Name"
        Me.txt_Name.Size = New System.Drawing.Size(323, 23)
        Me.txt_Name.TabIndex = 39
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label2.ForeColor = System.Drawing.Color.Navy
        Me.Label2.Location = New System.Drawing.Point(23, 81)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(94, 15)
        Me.Label2.TabIndex = 40
        Me.Label2.Text = "Company Name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label1.ForeColor = System.Drawing.Color.Navy
        Me.Label1.Location = New System.Drawing.Point(23, 54)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(90, 15)
        Me.Label1.TabIndex = 41
        Me.Label1.Text = "Company Id No"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_IdNo
        '
        Me.txt_IdNo.Enabled = False
        Me.txt_IdNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_IdNo.Location = New System.Drawing.Point(167, 50)
        Me.txt_IdNo.Name = "txt_IdNo"
        Me.txt_IdNo.Size = New System.Drawing.Size(323, 23)
        Me.txt_IdNo.TabIndex = 46
        '
        'txt_GSTIN
        '
        Me.txt_GSTIN.Enabled = False
        Me.txt_GSTIN.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_GSTIN.Location = New System.Drawing.Point(167, 132)
        Me.txt_GSTIN.Name = "txt_GSTIN"
        Me.txt_GSTIN.Size = New System.Drawing.Size(323, 23)
        Me.txt_GSTIN.TabIndex = 49
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label3.ForeColor = System.Drawing.Color.Navy
        Me.Label3.Location = New System.Drawing.Point(23, 137)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(39, 15)
        Me.Label3.TabIndex = 48
        Me.Label3.Text = "GSTIN"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtEWBIRNURL
        '
        Me.txtEWBIRNURL.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEWBIRNURL.Location = New System.Drawing.Point(152, 208)
        Me.txtEWBIRNURL.Name = "txtEWBIRNURL"
        Me.txtEWBIRNURL.Size = New System.Drawing.Size(305, 23)
        Me.txtEWBIRNURL.TabIndex = 21
        Me.txtEWBIRNURL.Visible = False
        '
        'lbl_EWBIRNURL_Caption
        '
        Me.lbl_EWBIRNURL_Caption.AutoSize = True
        Me.lbl_EWBIRNURL_Caption.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lbl_EWBIRNURL_Caption.ForeColor = System.Drawing.Color.Navy
        Me.lbl_EWBIRNURL_Caption.Location = New System.Drawing.Point(9, 212)
        Me.lbl_EWBIRNURL_Caption.Name = "lbl_EWBIRNURL_Caption"
        Me.lbl_EWBIRNURL_Caption.Size = New System.Drawing.Size(94, 15)
        Me.lbl_EWBIRNURL_Caption.TabIndex = 20
        Me.lbl_EWBIRNURL_Caption.Text = "EWB by IRN URL"
        Me.lbl_EWBIRNURL_Caption.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lbl_EWBIRNURL_Caption.Visible = False
        '
        'txtEIBaseURL
        '
        Me.txtEIBaseURL.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEIBaseURL.Location = New System.Drawing.Point(152, 171)
        Me.txtEIBaseURL.Name = "txtEIBaseURL"
        Me.txtEIBaseURL.Size = New System.Drawing.Size(305, 23)
        Me.txtEIBaseURL.TabIndex = 19
        Me.txtEIBaseURL.Visible = False
        '
        'lbl_EIBaseURL_Caption
        '
        Me.lbl_EIBaseURL_Caption.AutoSize = True
        Me.lbl_EIBaseURL_Caption.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lbl_EIBaseURL_Caption.ForeColor = System.Drawing.Color.Navy
        Me.lbl_EIBaseURL_Caption.Location = New System.Drawing.Point(9, 175)
        Me.lbl_EIBaseURL_Caption.Name = "lbl_EIBaseURL_Caption"
        Me.lbl_EIBaseURL_Caption.Size = New System.Drawing.Size(108, 15)
        Me.lbl_EIBaseURL_Caption.TabIndex = 18
        Me.lbl_EIBaseURL_Caption.Text = "e-Invoice Base URL"
        Me.lbl_EIBaseURL_Caption.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lbl_EIBaseURL_Caption.Visible = False
        '
        'txtEIAuthURL
        '
        Me.txtEIAuthURL.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEIAuthURL.Location = New System.Drawing.Point(152, 134)
        Me.txtEIAuthURL.Name = "txtEIAuthURL"
        Me.txtEIAuthURL.Size = New System.Drawing.Size(305, 23)
        Me.txtEIAuthURL.TabIndex = 17
        Me.txtEIAuthURL.Visible = False
        '
        'lbl_EIAuthURL_Caption
        '
        Me.lbl_EIAuthURL_Caption.AutoSize = True
        Me.lbl_EIAuthURL_Caption.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lbl_EIAuthURL_Caption.ForeColor = System.Drawing.Color.Navy
        Me.lbl_EIAuthURL_Caption.Location = New System.Drawing.Point(9, 138)
        Me.lbl_EIAuthURL_Caption.Name = "lbl_EIAuthURL_Caption"
        Me.lbl_EIAuthURL_Caption.Size = New System.Drawing.Size(110, 15)
        Me.lbl_EIAuthURL_Caption.TabIndex = 16
        Me.lbl_EIAuthURL_Caption.Text = "e-Invoice Auth URL"
        Me.lbl_EIAuthURL_Caption.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lbl_EIAuthURL_Caption.Visible = False
        '
        'lbl_CancelEWBURL_Caption
        '
        Me.lbl_CancelEWBURL_Caption.AutoSize = True
        Me.lbl_CancelEWBURL_Caption.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lbl_CancelEWBURL_Caption.ForeColor = System.Drawing.Color.Navy
        Me.lbl_CancelEWBURL_Caption.Location = New System.Drawing.Point(24, 211)
        Me.lbl_CancelEWBURL_Caption.Name = "lbl_CancelEWBURL_Caption"
        Me.lbl_CancelEWBURL_Caption.Size = New System.Drawing.Size(94, 15)
        Me.lbl_CancelEWBURL_Caption.TabIndex = 22
        Me.lbl_CancelEWBURL_Caption.Text = "Cancel EWB URL"
        Me.lbl_CancelEWBURL_Caption.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lbl_CancelEWBURL_Caption.Visible = False
        '
        'txtCancelEWBURL
        '
        Me.txtCancelEWBURL.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCancelEWBURL.Location = New System.Drawing.Point(141, 207)
        Me.txtCancelEWBURL.Name = "txtCancelEWBURL"
        Me.txtCancelEWBURL.Size = New System.Drawing.Size(323, 23)
        Me.txtCancelEWBURL.TabIndex = 23
        Me.txtCancelEWBURL.Visible = False
        '
        'txtEIPassword
        '
        Me.txtEIPassword.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEIPassword.Location = New System.Drawing.Point(152, 60)
        Me.txtEIPassword.Name = "txtEIPassword"
        Me.txtEIPassword.Size = New System.Drawing.Size(305, 23)
        Me.txtEIPassword.TabIndex = 27
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label10.ForeColor = System.Drawing.Color.Navy
        Me.Label10.Location = New System.Drawing.Point(9, 64)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(111, 15)
        Me.Label10.TabIndex = 26
        Me.Label10.Text = "e-Invoice Password"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtEIUserID
        '
        Me.txtEIUserID.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEIUserID.Location = New System.Drawing.Point(152, 23)
        Me.txtEIUserID.Name = "txtEIUserID"
        Me.txtEIUserID.Size = New System.Drawing.Size(305, 23)
        Me.txtEIUserID.TabIndex = 25
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label11.ForeColor = System.Drawing.Color.Navy
        Me.Label11.Location = New System.Drawing.Point(9, 27)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(97, 15)
        Me.Label11.TabIndex = 24
        Me.Label11.Text = "e-Invoice User Id"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_EInvoiceGSPName
        '
        Me.txt_EInvoiceGSPName.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_EInvoiceGSPName.Location = New System.Drawing.Point(152, 97)
        Me.txt_EInvoiceGSPName.Name = "txt_EInvoiceGSPName"
        Me.txt_EInvoiceGSPName.Size = New System.Drawing.Size(305, 23)
        Me.txt_EInvoiceGSPName.TabIndex = 15
        Me.txt_EInvoiceGSPName.Visible = False
        '
        'lbl_EInvoiceGSPName_Caption
        '
        Me.lbl_EInvoiceGSPName_Caption.AutoSize = True
        Me.lbl_EInvoiceGSPName_Caption.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lbl_EInvoiceGSPName_Caption.ForeColor = System.Drawing.Color.Navy
        Me.lbl_EInvoiceGSPName_Caption.Location = New System.Drawing.Point(9, 101)
        Me.lbl_EInvoiceGSPName_Caption.Name = "lbl_EInvoiceGSPName_Caption"
        Me.lbl_EInvoiceGSPName_Caption.Size = New System.Drawing.Size(112, 15)
        Me.lbl_EInvoiceGSPName_Caption.TabIndex = 14
        Me.lbl_EInvoiceGSPName_Caption.Text = "eInvoice GSP Name"
        Me.lbl_EInvoiceGSPName_Caption.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lbl_EInvoiceGSPName_Caption.Visible = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtEIUserID)
        Me.GroupBox1.Controls.Add(Me.txt_EInvoiceGSPName)
        Me.GroupBox1.Controls.Add(Me.lbl_EIAuthURL_Caption)
        Me.GroupBox1.Controls.Add(Me.lbl_EInvoiceGSPName_Caption)
        Me.GroupBox1.Controls.Add(Me.txtEIAuthURL)
        Me.GroupBox1.Controls.Add(Me.txtEIPassword)
        Me.GroupBox1.Controls.Add(Me.txtEWBIRNURL)
        Me.GroupBox1.Controls.Add(Me.lbl_EIBaseURL_Caption)
        Me.GroupBox1.Controls.Add(Me.lbl_EWBIRNURL_Caption)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.txtEIBaseURL)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(523, 164)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(481, 246)
        Me.GroupBox1.TabIndex = 50
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "e-Invoice"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtEWBUserID)
        Me.GroupBox2.Controls.Add(Me.lbl_GSPName_Caption)
        Me.GroupBox2.Controls.Add(Me.txtCancelEWBURL)
        Me.GroupBox2.Controls.Add(Me.txtGSPName)
        Me.GroupBox2.Controls.Add(Me.lbl_CancelEWBURL_Caption)
        Me.GroupBox2.Controls.Add(Me.lbl_BaseURL_Caption)
        Me.GroupBox2.Controls.Add(Me.txtBaseURL)
        Me.GroupBox2.Controls.Add(Me.lblEWBUserID)
        Me.GroupBox2.Controls.Add(Me.lblEWBPassword)
        Me.GroupBox2.Controls.Add(Me.txtEWBPassword)
        Me.GroupBox2.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(26, 164)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(481, 246)
        Me.GroupBox2.TabIndex = 51
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "e-Way Bill"
        '
        'btn_RestoreDefault
        '
        Me.btn_RestoreDefault.BackColor = System.Drawing.Color.OrangeRed
        Me.btn_RestoreDefault.ForeColor = System.Drawing.Color.White
        Me.btn_RestoreDefault.Location = New System.Drawing.Point(17, 428)
        Me.btn_RestoreDefault.Name = "btn_RestoreDefault"
        Me.btn_RestoreDefault.Size = New System.Drawing.Size(124, 31)
        Me.btn_RestoreDefault.TabIndex = 1164
        Me.btn_RestoreDefault.TabStop = False
        Me.btn_RestoreDefault.Text = "RESTORE DEFAULT"
        Me.btn_RestoreDefault.UseVisualStyleBackColor = False
        '
        'btn_CheckConnectivity
        '
        Me.btn_CheckConnectivity.BackColor = System.Drawing.Color.DeepPink
        Me.btn_CheckConnectivity.ForeColor = System.Drawing.Color.White
        Me.btn_CheckConnectivity.Location = New System.Drawing.Point(151, 426)
        Me.btn_CheckConnectivity.Name = "btn_CheckConnectivity"
        Me.btn_CheckConnectivity.Size = New System.Drawing.Size(227, 31)
        Me.btn_CheckConnectivity.TabIndex = 1165
        Me.btn_CheckConnectivity.TabStop = False
        Me.btn_CheckConnectivity.Text = "EINV-CHECK CONNECTIVITY WITH IRP"
        Me.btn_CheckConnectivity.UseVisualStyleBackColor = False
        '
        'lbl_Company
        '
        Me.lbl_Company.AutoSize = True
        Me.lbl_Company.BackColor = System.Drawing.Color.Red
        Me.lbl_Company.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Company.Location = New System.Drawing.Point(762, 9)
        Me.lbl_Company.Name = "lbl_Company"
        Me.lbl_Company.Size = New System.Drawing.Size(77, 15)
        Me.lbl_Company.TabIndex = 1166
        Me.lbl_Company.Text = "lbl_Company"
        Me.lbl_Company.Visible = False
        '
        'rtbeInvoiceResponse
        '
        Me.rtbeInvoiceResponse.BackColor = System.Drawing.Color.DarkSlateGray
        Me.rtbeInvoiceResponse.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbeInvoiceResponse.ForeColor = System.Drawing.Color.White
        Me.rtbeInvoiceResponse.Location = New System.Drawing.Point(495, 417)
        Me.rtbeInvoiceResponse.Name = "rtbeInvoiceResponse"
        Me.rtbeInvoiceResponse.Size = New System.Drawing.Size(309, 32)
        Me.rtbeInvoiceResponse.TabIndex = 1167
        Me.rtbeInvoiceResponse.Text = ""
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.ForeColor = System.Drawing.Color.Navy
        Me.Label23.Location = New System.Drawing.Point(429, 423)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(59, 15)
        Me.Label23.TabIndex = 1168
        Me.Label23.Text = "Response"
        '
        'btn_CheckConnectivity1
        '
        Me.btn_CheckConnectivity1.BackColor = System.Drawing.Color.DeepPink
        Me.btn_CheckConnectivity1.ForeColor = System.Drawing.Color.White
        Me.btn_CheckConnectivity1.Location = New System.Drawing.Point(151, 463)
        Me.btn_CheckConnectivity1.Name = "btn_CheckConnectivity1"
        Me.btn_CheckConnectivity1.Size = New System.Drawing.Size(227, 31)
        Me.btn_CheckConnectivity1.TabIndex = 1169
        Me.btn_CheckConnectivity1.TabStop = False
        Me.btn_CheckConnectivity1.Text = "EWB-CHECK CONNECTIVITY WITH IRP"
        Me.btn_CheckConnectivity1.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Navy
        Me.Label4.Location = New System.Drawing.Point(429, 468)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(59, 15)
        Me.Label4.TabIndex = 1171
        Me.Label4.Text = "Response"
        '
        'rtbEWBResponse
        '
        Me.rtbEWBResponse.BackColor = System.Drawing.Color.DarkSlateGray
        Me.rtbEWBResponse.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbEWBResponse.ForeColor = System.Drawing.Color.White
        Me.rtbEWBResponse.Location = New System.Drawing.Point(495, 462)
        Me.rtbEWBResponse.Name = "rtbEWBResponse"
        Me.rtbEWBResponse.Size = New System.Drawing.Size(309, 32)
        Me.rtbEWBResponse.TabIndex = 1170
        Me.rtbEWBResponse.Text = ""
        '
        'GST_EWB_API_SETTINGS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1032, 501)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.rtbEWBResponse)
        Me.Controls.Add(Me.btn_CheckConnectivity1)
        Me.Controls.Add(Me.Label23)
        Me.Controls.Add(Me.rtbeInvoiceResponse)
        Me.Controls.Add(Me.lbl_Company)
        Me.Controls.Add(Me.btn_CheckConnectivity)
        Me.Controls.Add(Me.btn_RestoreDefault)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.txt_GSTIN)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txt_IdNo)
        Me.Controls.Add(Me.txt_ShortName)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txt_Name)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.txtASPPassword)
        Me.Controls.Add(Me.lblASPPassword_Caption)
        Me.Controls.Add(Me.txtASPUserID)
        Me.Controls.Add(Me.lblASPUserID_Caption)
        Me.Controls.Add(Me.Label8)
        Me.Name = "GST_EWB_API_SETTINGS"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtASPUserID As TextBox
    Friend WithEvents lblASPUserID_Caption As Label
    Friend WithEvents txtGSPName As TextBox
    Friend WithEvents lbl_GSPName_Caption As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents txtBaseURL As TextBox
    Friend WithEvents lbl_BaseURL_Caption As Label
    Friend WithEvents txtASPPassword As TextBox
    Friend WithEvents lblASPPassword_Caption As Label
    Friend WithEvents txtEWBPassword As TextBox
    Friend WithEvents lblEWBPassword As Label
    Friend WithEvents txtEWBUserID As TextBox
    Friend WithEvents lblEWBUserID As Label
    Friend WithEvents btnSave As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents txt_ShortName As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents txt_Name As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txt_IdNo As TextBox
    Friend WithEvents txt_GSTIN As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtEWBIRNURL As TextBox
    Friend WithEvents lbl_EWBIRNURL_Caption As Label
    Friend WithEvents txtEIBaseURL As TextBox
    Friend WithEvents lbl_EIBaseURL_Caption As Label
    Friend WithEvents txtEIAuthURL As TextBox
    Friend WithEvents lbl_EIAuthURL_Caption As Label
    Friend WithEvents lbl_CancelEWBURL_Caption As Label
    Friend WithEvents txtCancelEWBURL As TextBox
    Friend WithEvents txtEIPassword As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents txtEIUserID As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents txt_EInvoiceGSPName As TextBox
    Friend WithEvents lbl_EInvoiceGSPName_Caption As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents btn_RestoreDefault As Button
    Friend WithEvents btn_CheckConnectivity As Button
    Friend WithEvents lbl_Company As Label
    Friend WithEvents rtbeInvoiceResponse As RichTextBox
    Friend WithEvents Label23 As Label
    Friend WithEvents btn_CheckConnectivity1 As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents rtbEWBResponse As RichTextBox
End Class
