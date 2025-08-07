<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class New_EMail_Entry
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.btn_save = New System.Windows.Forms.Button()
        Me.btn_AttachmentSelection = New System.Windows.Forms.Button()
        Me.txt_Attachment = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txt_SubJect = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txt_Msg = New System.Windows.Forms.TextBox()
        Me.btnSendMail = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.txt_PhnNo = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.lbl_Heading = New System.Windows.Forms.Label()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.pnl_Select_Party_Name_Details = New System.Windows.Forms.Panel()
        Me.cbo_mail_id = New System.Windows.Forms.ComboBox()
        Me.cbo_Grid_PartyName = New System.Windows.Forms.ComboBox()
        Me.dgv_Party_Name_Details = New System.Windows.Forms.DataGridView()
        Me.btn__Deselect = New System.Windows.Forms.Button()
        Me.btn__Select = New System.Windows.Forms.Button()
        Me.cbo_PartyName = New System.Windows.Forms.ComboBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label64 = New System.Windows.Forms.Label()
        Me.msk_Date = New System.Windows.Forms.MaskedTextBox()
        Me.dtp_Date = New System.Windows.Forms.DateTimePicker()
        Me.lbl_MailNo = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lbl_Company = New System.Windows.Forms.Label()
        Me.cbo_Designation = New System.Windows.Forms.ComboBox()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column79 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.pnl_Back.SuspendLayout()
        Me.pnl_Select_Party_Name_Details.SuspendLayout()
        CType(Me.dgv_Party_Name_Details, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Navy
        Me.Label1.Location = New System.Drawing.Point(7, 142)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 15)
        Me.Label1.TabIndex = 60
        Me.Label1.Text = "MESSAGE"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'pnl_Back
        '
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.btn_save)
        Me.pnl_Back.Controls.Add(Me.btn_AttachmentSelection)
        Me.pnl_Back.Controls.Add(Me.txt_Attachment)
        Me.pnl_Back.Controls.Add(Me.Label3)
        Me.pnl_Back.Controls.Add(Me.txt_SubJect)
        Me.pnl_Back.Controls.Add(Me.Label2)
        Me.pnl_Back.Controls.Add(Me.Label1)
        Me.pnl_Back.Controls.Add(Me.Label19)
        Me.pnl_Back.Controls.Add(Me.txt_Msg)
        Me.pnl_Back.Controls.Add(Me.btnSendMail)
        Me.pnl_Back.Controls.Add(Me.btnClose)
        Me.pnl_Back.Controls.Add(Me.txt_PhnNo)
        Me.pnl_Back.Controls.Add(Me.Label11)
        Me.pnl_Back.Location = New System.Drawing.Point(630, 83)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(470, 339)
        Me.pnl_Back.TabIndex = 37
        '
        'btn_save
        '
        Me.btn_save.BackColor = System.Drawing.Color.FromArgb(CType(CType(5, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.btn_save.ForeColor = System.Drawing.Color.White
        Me.btn_save.Location = New System.Drawing.Point(100, 294)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(82, 31)
        Me.btn_save.TabIndex = 313
        Me.btn_save.TabStop = False
        Me.btn_save.Text = "&SAVE"
        Me.btn_save.UseVisualStyleBackColor = False
        '
        'btn_AttachmentSelection
        '
        Me.btn_AttachmentSelection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn_AttachmentSelection.Location = New System.Drawing.Point(436, 98)
        Me.btn_AttachmentSelection.Name = "btn_AttachmentSelection"
        Me.btn_AttachmentSelection.Size = New System.Drawing.Size(25, 23)
        Me.btn_AttachmentSelection.TabIndex = 3
        Me.btn_AttachmentSelection.TabStop = False
        Me.btn_AttachmentSelection.Text = "..."
        Me.btn_AttachmentSelection.UseVisualStyleBackColor = True
        '
        'txt_Attachment
        '
        Me.txt_Attachment.Enabled = False
        Me.txt_Attachment.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Attachment.Location = New System.Drawing.Point(101, 98)
        Me.txt_Attachment.MaxLength = 50
        Me.txt_Attachment.Name = "txt_Attachment"
        Me.txt_Attachment.Size = New System.Drawing.Size(326, 23)
        Me.txt_Attachment.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.Color.Navy
        Me.Label3.Location = New System.Drawing.Point(7, 102)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(80, 15)
        Me.Label3.TabIndex = 64
        Me.Label3.Text = "ATTACHMENT"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_SubJect
        '
        Me.txt_SubJect.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_SubJect.Location = New System.Drawing.Point(101, 58)
        Me.txt_SubJect.MaxLength = 50
        Me.txt_SubJect.Name = "txt_SubJect"
        Me.txt_SubJect.Size = New System.Drawing.Size(353, 23)
        Me.txt_SubJect.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.Navy
        Me.Label2.Location = New System.Drawing.Point(7, 62)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(51, 15)
        Me.Label2.TabIndex = 62
        Me.Label2.Text = "SUBJECT"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.ForeColor = System.Drawing.Color.Navy
        Me.Label19.Location = New System.Drawing.Point(38, 105)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(0, 15)
        Me.Label19.TabIndex = 59
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_Msg
        '
        Me.txt_Msg.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Msg.Location = New System.Drawing.Point(100, 138)
        Me.txt_Msg.MaxLength = 1000
        Me.txt_Msg.Multiline = True
        Me.txt_Msg.Name = "txt_Msg"
        Me.txt_Msg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txt_Msg.Size = New System.Drawing.Size(352, 145)
        Me.txt_Msg.TabIndex = 4
        Me.txt_Msg.Text = "WRITE MESSAGE"
        '
        'btnSendMail
        '
        Me.btnSendMail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnSendMail.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSendMail.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSendMail.ForeColor = System.Drawing.Color.Navy
        Me.btnSendMail.Image = Global.Textile.My.Resources.Resources.NextUp
        Me.btnSendMail.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSendMail.Location = New System.Drawing.Point(206, 294)
        Me.btnSendMail.Name = "btnSendMail"
        Me.btnSendMail.Size = New System.Drawing.Size(123, 31)
        Me.btnSendMail.TabIndex = 5
        Me.btnSendMail.TabStop = False
        Me.btnSendMail.Text = "SEND E-MAIL"
        Me.btnSendMail.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSendMail.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ForeColor = System.Drawing.Color.Navy
        Me.btnClose.Image = Global.Textile.My.Resources.Resources.Close1
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(348, 294)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(100, 31)
        Me.btnClose.TabIndex = 6
        Me.btnClose.TabStop = False
        Me.btnClose.Text = "CLOSE"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'txt_PhnNo
        '
        Me.txt_PhnNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_PhnNo.Location = New System.Drawing.Point(101, 18)
        Me.txt_PhnNo.MaxLength = 50
        Me.txt_PhnNo.Name = "txt_PhnNo"
        Me.txt_PhnNo.Size = New System.Drawing.Size(353, 23)
        Me.txt_PhnNo.TabIndex = 0
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.ForeColor = System.Drawing.Color.Navy
        Me.Label11.Location = New System.Drawing.Point(7, 22)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(22, 15)
        Me.Label11.TabIndex = 19
        Me.Label11.Text = "TO"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbl_Heading
        '
        Me.lbl_Heading.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.lbl_Heading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_Heading.Dock = System.Windows.Forms.DockStyle.Top
        Me.lbl_Heading.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Heading.ForeColor = System.Drawing.Color.White
        Me.lbl_Heading.Location = New System.Drawing.Point(0, 0)
        Me.lbl_Heading.Name = "lbl_Heading"
        Me.lbl_Heading.Size = New System.Drawing.Size(1109, 30)
        Me.lbl_Heading.TabIndex = 38
        Me.lbl_Heading.Text = "MAIL  BOX"
        Me.lbl_Heading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'pnl_Select_Party_Name_Details
        '
        Me.pnl_Select_Party_Name_Details.BackColor = System.Drawing.Color.SkyBlue
        Me.pnl_Select_Party_Name_Details.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Select_Party_Name_Details.Controls.Add(Me.cbo_Designation)
        Me.pnl_Select_Party_Name_Details.Controls.Add(Me.cbo_mail_id)
        Me.pnl_Select_Party_Name_Details.Controls.Add(Me.cbo_Grid_PartyName)
        Me.pnl_Select_Party_Name_Details.Controls.Add(Me.dgv_Party_Name_Details)
        Me.pnl_Select_Party_Name_Details.Controls.Add(Me.btn__Deselect)
        Me.pnl_Select_Party_Name_Details.Controls.Add(Me.btn__Select)
        Me.pnl_Select_Party_Name_Details.Controls.Add(Me.cbo_PartyName)
        Me.pnl_Select_Party_Name_Details.Controls.Add(Me.Label17)
        Me.pnl_Select_Party_Name_Details.Controls.Add(Me.Label39)
        Me.pnl_Select_Party_Name_Details.Location = New System.Drawing.Point(12, 54)
        Me.pnl_Select_Party_Name_Details.Name = "pnl_Select_Party_Name_Details"
        Me.pnl_Select_Party_Name_Details.Size = New System.Drawing.Size(595, 367)
        Me.pnl_Select_Party_Name_Details.TabIndex = 273
        '
        'cbo_mail_id
        '
        Me.cbo_mail_id.FormattingEnabled = True
        Me.cbo_mail_id.Location = New System.Drawing.Point(438, 274)
        Me.cbo_mail_id.Name = "cbo_mail_id"
        Me.cbo_mail_id.Size = New System.Drawing.Size(131, 23)
        Me.cbo_mail_id.TabIndex = 244
        Me.cbo_mail_id.TabStop = False
        Me.cbo_mail_id.Visible = False
        '
        'cbo_Grid_PartyName
        '
        Me.cbo_Grid_PartyName.FormattingEnabled = True
        Me.cbo_Grid_PartyName.Location = New System.Drawing.Point(272, 274)
        Me.cbo_Grid_PartyName.Name = "cbo_Grid_PartyName"
        Me.cbo_Grid_PartyName.Size = New System.Drawing.Size(131, 23)
        Me.cbo_Grid_PartyName.TabIndex = 243
        Me.cbo_Grid_PartyName.TabStop = False
        Me.cbo_Grid_PartyName.Visible = False
        '
        'dgv_Party_Name_Details
        '
        Me.dgv_Party_Name_Details.AllowUserToAddRows = False
        Me.dgv_Party_Name_Details.AllowUserToResizeColumns = False
        Me.dgv_Party_Name_Details.AllowUserToResizeRows = False
        Me.dgv_Party_Name_Details.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(90, Byte), Integer), CType(CType(90, Byte), Integer))
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(90, Byte), Integer), CType(CType(90, Byte), Integer))
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_Party_Name_Details.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgv_Party_Name_Details.ColumnHeadersHeight = 30
        Me.dgv_Party_Name_Details.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgv_Party_Name_Details.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn4, Me.DataGridViewTextBoxColumn5, Me.Column2, Me.Column3, Me.DataGridViewTextBoxColumn6, Me.Column79, Me.Column1, Me.Column4})
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.Lime
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgv_Party_Name_Details.DefaultCellStyle = DataGridViewCellStyle5
        Me.dgv_Party_Name_Details.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.dgv_Party_Name_Details.EnableHeadersVisualStyles = False
        Me.dgv_Party_Name_Details.Location = New System.Drawing.Point(20, 69)
        Me.dgv_Party_Name_Details.MultiSelect = False
        Me.dgv_Party_Name_Details.Name = "dgv_Party_Name_Details"
        Me.dgv_Party_Name_Details.RowHeadersVisible = False
        Me.dgv_Party_Name_Details.RowHeadersWidth = 15
        Me.dgv_Party_Name_Details.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgv_Party_Name_Details.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_Party_Name_Details.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Party_Name_Details.Size = New System.Drawing.Size(558, 243)
        Me.dgv_Party_Name_Details.TabIndex = 240
        Me.dgv_Party_Name_Details.TabStop = False
        '
        'btn__Deselect
        '
        Me.btn__Deselect.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn__Deselect.ForeColor = System.Drawing.Color.White
        Me.btn__Deselect.Location = New System.Drawing.Point(362, 320)
        Me.btn__Deselect.Name = "btn__Deselect"
        Me.btn__Deselect.Size = New System.Drawing.Size(104, 32)
        Me.btn__Deselect.TabIndex = 68
        Me.btn__Deselect.Text = "&DESELECT ALL"
        Me.btn__Deselect.UseVisualStyleBackColor = False
        '
        'btn__Select
        '
        Me.btn__Select.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn__Select.ForeColor = System.Drawing.Color.White
        Me.btn__Select.Location = New System.Drawing.Point(249, 320)
        Me.btn__Select.Name = "btn__Select"
        Me.btn__Select.Size = New System.Drawing.Size(96, 32)
        Me.btn__Select.TabIndex = 67
        Me.btn__Select.Text = "&SELECT ALL"
        Me.btn__Select.UseVisualStyleBackColor = False
        '
        'cbo_PartyName
        '
        Me.cbo_PartyName.FormattingEnabled = True
        Me.cbo_PartyName.Location = New System.Drawing.Point(95, 39)
        Me.cbo_PartyName.Name = "cbo_PartyName"
        Me.cbo_PartyName.Size = New System.Drawing.Size(430, 23)
        Me.cbo_PartyName.TabIndex = 66
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.ForeColor = System.Drawing.Color.DarkBlue
        Me.Label17.Location = New System.Drawing.Point(17, 42)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(72, 15)
        Me.Label17.TabIndex = 62
        Me.Label17.Text = "Party Name"
        '
        'Label39
        '
        Me.Label39.BackColor = System.Drawing.Color.DeepPink
        Me.Label39.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label39.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label39.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label39.ForeColor = System.Drawing.Color.White
        Me.Label39.Location = New System.Drawing.Point(0, 0)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(593, 24)
        Me.Label39.TabIndex = 43
        Me.Label39.Text = " SELECT PARTY LIST"
        Me.Label39.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Navy
        Me.Label4.Location = New System.Drawing.Point(842, 49)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(33, 15)
        Me.Label4.TabIndex = 308
        Me.Label4.Text = "Date"
        '
        'Label64
        '
        Me.Label64.AutoSize = True
        Me.Label64.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label64.ForeColor = System.Drawing.Color.Red
        Me.Label64.Location = New System.Drawing.Point(878, 49)
        Me.Label64.Name = "Label64"
        Me.Label64.Size = New System.Drawing.Size(13, 15)
        Me.Label64.TabIndex = 309
        Me.Label64.Text = "*"
        Me.Label64.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'msk_Date
        '
        Me.msk_Date.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.msk_Date.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.msk_Date.Location = New System.Drawing.Point(912, 45)
        Me.msk_Date.Mask = "00-00-0000"
        Me.msk_Date.Name = "msk_Date"
        Me.msk_Date.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.msk_Date.Size = New System.Drawing.Size(104, 22)
        Me.msk_Date.TabIndex = 306
        '
        'dtp_Date
        '
        Me.dtp_Date.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Bold)
        Me.dtp_Date.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_Date.Location = New System.Drawing.Point(1015, 45)
        Me.dtp_Date.Name = "dtp_Date"
        Me.dtp_Date.Size = New System.Drawing.Size(19, 22)
        Me.dtp_Date.TabIndex = 307
        Me.dtp_Date.TabStop = False
        '
        'lbl_MailNo
        '
        Me.lbl_MailNo.BackColor = System.Drawing.Color.White
        Me.lbl_MailNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_MailNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_MailNo.Location = New System.Drawing.Point(759, 46)
        Me.lbl_MailNo.Name = "lbl_MailNo"
        Me.lbl_MailNo.Size = New System.Drawing.Size(53, 23)
        Me.lbl_MailNo.TabIndex = 311
        Me.lbl_MailNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Navy
        Me.Label5.Location = New System.Drawing.Point(688, 50)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(49, 15)
        Me.Label5.TabIndex = 310
        Me.Label5.Text = "Mail.No"
        '
        'lbl_Company
        '
        Me.lbl_Company.AutoSize = True
        Me.lbl_Company.BackColor = System.Drawing.Color.Red
        Me.lbl_Company.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Company.Location = New System.Drawing.Point(267, 9)
        Me.lbl_Company.Name = "lbl_Company"
        Me.lbl_Company.Size = New System.Drawing.Size(77, 15)
        Me.lbl_Company.TabIndex = 312
        Me.lbl_Company.Text = "lbl_Company"
        Me.lbl_Company.Visible = False
        '
        'cbo_Designation
        '
        Me.cbo_Designation.FormattingEnabled = True
        Me.cbo_Designation.Location = New System.Drawing.Point(227, 231)
        Me.cbo_Designation.Name = "cbo_Designation"
        Me.cbo_Designation.Size = New System.Drawing.Size(131, 23)
        Me.cbo_Designation.TabIndex = 245
        Me.cbo_Designation.TabStop = False
        Me.cbo_Designation.Visible = False
        '
        'DataGridViewTextBoxColumn4
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.DataGridViewTextBoxColumn4.DefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridViewTextBoxColumn4.HeaderText = "SNO"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        Me.DataGridViewTextBoxColumn4.Width = 35
        '
        'DataGridViewTextBoxColumn5
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.DataGridViewTextBoxColumn5.DefaultCellStyle = DataGridViewCellStyle3
        Me.DataGridViewTextBoxColumn5.HeaderText = "PARTY  NAME"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        Me.DataGridViewTextBoxColumn5.Width = 250
        '
        'Column2
        '
        Me.Column2.HeaderText = "CONTACT PERSON"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Width = 120
        '
        'Column3
        '
        Me.Column3.HeaderText = "DESIGNATION"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        '
        'DataGridViewTextBoxColumn6
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.DataGridViewTextBoxColumn6.DefaultCellStyle = DataGridViewCellStyle4
        Me.DataGridViewTextBoxColumn6.HeaderText = "STS"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.ReadOnly = True
        Me.DataGridViewTextBoxColumn6.Width = 40
        '
        'Column79
        '
        Me.Column79.HeaderText = "Party_idno"
        Me.Column79.Name = "Column79"
        Me.Column79.ReadOnly = True
        Me.Column79.Visible = False
        '
        'Column1
        '
        Me.Column1.HeaderText = "Email_id"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Visible = False
        '
        'Column4
        '
        Me.Column4.HeaderText = "Designation_idno"
        Me.Column4.Name = "Column4"
        Me.Column4.Visible = False
        '
        'New_EMail_Entry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.SkyBlue
        Me.ClientSize = New System.Drawing.Size(1109, 442)
        Me.Controls.Add(Me.lbl_Company)
        Me.Controls.Add(Me.lbl_MailNo)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.pnl_Select_Party_Name_Details)
        Me.Controls.Add(Me.Label64)
        Me.Controls.Add(Me.pnl_Back)
        Me.Controls.Add(Me.msk_Date)
        Me.Controls.Add(Me.dtp_Date)
        Me.Controls.Add(Me.lbl_Heading)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "New_EMail_Entry"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MAIL"
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_Back.PerformLayout()
        Me.pnl_Select_Party_Name_Details.ResumeLayout(False)
        Me.pnl_Select_Party_Name_Details.PerformLayout()
        CType(Me.dgv_Party_Name_Details, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents txt_Msg As System.Windows.Forms.TextBox
    Friend WithEvents btnSendMail As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents txt_PhnNo As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents lbl_Heading As System.Windows.Forms.Label
    Friend WithEvents txt_SubJect As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txt_Attachment As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btn_AttachmentSelection As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents pnl_Select_Party_Name_Details As Panel
    Friend WithEvents dgv_Party_Name_Details As DataGridView
    Friend WithEvents btn__Deselect As Button
    Friend WithEvents btn__Select As Button
    Friend WithEvents cbo_PartyName As ComboBox
    Friend WithEvents Label17 As Label
    Friend WithEvents Label39 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label64 As Label
    Friend WithEvents msk_Date As MaskedTextBox
    Friend WithEvents dtp_Date As DateTimePicker
    Friend WithEvents lbl_MailNo As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents lbl_Company As Label
    Friend WithEvents btn_save As Button
    Friend WithEvents cbo_mail_id As ComboBox
    Friend WithEvents cbo_Grid_PartyName As ComboBox
    Friend WithEvents cbo_Designation As ComboBox
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As DataGridViewTextBoxColumn
    Friend WithEvents Column79 As DataGridViewTextBoxColumn
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Column4 As DataGridViewTextBoxColumn
End Class
