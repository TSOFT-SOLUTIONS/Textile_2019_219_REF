<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Cheque_Entry
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog()
        Me.dtp_Date = New System.Windows.Forms.DateTimePicker()
        Me.btn_closefilter = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lbl_ReceiptNo = New System.Windows.Forms.Label()
        Me.btn_filtershow = New System.Windows.Forms.Button()
        Me.cbo_Bank = New System.Windows.Forms.ComboBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.dgv_filter = New System.Windows.Forms.DataGridView()
        Me.dc = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_ChequeAmt = New System.Windows.Forms.TextBox()
        Me.cbo_Filter_PartyName = New System.Windows.Forms.ComboBox()
        Me.dtp_FilterFrom_date = New System.Windows.Forms.DateTimePicker()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dtp_FilterTo_date = New System.Windows.Forms.DateTimePicker()
        Me.pnl_back = New System.Windows.Forms.Panel()
        Me.txt_Print_Name = New System.Windows.Forms.TextBox()
        Me.lbl_caption_Agent = New System.Windows.Forms.Label()
        Me.cbo_AgentName = New System.Windows.Forms.ComboBox()
        Me.cbo_ACPayee = New System.Windows.Forms.ComboBox()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.lbl_Day = New System.Windows.Forms.Label()
        Me.txt_Narration = New System.Windows.Forms.TextBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.cbo_PartyName = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.msk_date = New System.Windows.Forms.MaskedTextBox()
        Me.btn_Print = New System.Windows.Forms.Button()
        Me.btn_close = New System.Windows.Forms.Button()
        Me.btn_save = New System.Windows.Forms.Button()
        Me.txt_ChequeNo = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.lbl_Company = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.pnl_filter = New System.Windows.Forms.Panel()
        Me.PrintDocument2 = New System.Drawing.Printing.PrintDocument()
        CType(Me.dgv_filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnl_back.SuspendLayout()
        Me.pnl_filter.SuspendLayout()
        Me.SuspendLayout()
        '
        'PrintDialog1
        '
        Me.PrintDialog1.UseEXDialog = True
        '
        'dtp_Date
        '
        Me.dtp_Date.CustomFormat = ""
        Me.dtp_Date.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_Date.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_Date.Location = New System.Drawing.Point(446, 15)
        Me.dtp_Date.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtp_Date.Name = "dtp_Date"
        Me.dtp_Date.Size = New System.Drawing.Size(19, 22)
        Me.dtp_Date.TabIndex = 0
        Me.dtp_Date.TabStop = False
        '
        'btn_closefilter
        '
        Me.btn_closefilter.Location = New System.Drawing.Point(542, 86)
        Me.btn_closefilter.Name = "btn_closefilter"
        Me.btn_closefilter.Size = New System.Drawing.Size(68, 27)
        Me.btn_closefilter.TabIndex = 5
        Me.btn_closefilter.Text = "&CLOSE"
        Me.btn_closefilter.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.ForeColor = System.Drawing.Color.Blue
        Me.Label9.Location = New System.Drawing.Point(14, 285)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(60, 15)
        Me.Label9.TabIndex = 15
        Me.Label9.Text = "Narration"
        '
        'Label16
        '
        Me.Label16.BackColor = System.Drawing.Color.DimGray
        Me.Label16.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label16.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.White
        Me.Label16.Location = New System.Drawing.Point(0, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(635, 30)
        Me.Label16.TabIndex = 8
        Me.Label16.Text = "FILTER"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ForeColor = System.Drawing.Color.Blue
        Me.Label8.Location = New System.Drawing.Point(488, 19)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(27, 15)
        Me.Label8.TabIndex = 14
        Me.Label8.Text = "Day"
        '
        'lbl_ReceiptNo
        '
        Me.lbl_ReceiptNo.BackColor = System.Drawing.Color.White
        Me.lbl_ReceiptNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_ReceiptNo.Location = New System.Drawing.Point(145, 15)
        Me.lbl_ReceiptNo.Name = "lbl_ReceiptNo"
        Me.lbl_ReceiptNo.Size = New System.Drawing.Size(89, 23)
        Me.lbl_ReceiptNo.TabIndex = 21
        Me.lbl_ReceiptNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btn_filtershow
        '
        Me.btn_filtershow.Location = New System.Drawing.Point(544, 48)
        Me.btn_filtershow.Name = "btn_filtershow"
        Me.btn_filtershow.Size = New System.Drawing.Size(66, 27)
        Me.btn_filtershow.TabIndex = 4
        Me.btn_filtershow.Text = "SHOW"
        Me.btn_filtershow.UseVisualStyleBackColor = True
        '
        'cbo_Bank
        '
        Me.cbo_Bank.DropDownHeight = 125
        Me.cbo_Bank.FormattingEnabled = True
        Me.cbo_Bank.IntegralHeight = False
        Me.cbo_Bank.Location = New System.Drawing.Point(145, 58)
        Me.cbo_Bank.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.cbo_Bank.Name = "cbo_Bank"
        Me.cbo_Bank.Size = New System.Drawing.Size(515, 23)
        Me.cbo_Bank.TabIndex = 1
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.ForeColor = System.Drawing.Color.Blue
        Me.Label10.Location = New System.Drawing.Point(14, 238)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(97, 15)
        Me.Label10.TabIndex = 27
        Me.Label10.Text = "Cheque Amount"
        '
        'dgv_filter
        '
        Me.dgv_filter.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.dgv_filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_filter.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.dc, Me.Column1, Me.Column2, Me.Column4, Me.Column6})
        Me.dgv_filter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgv_filter.Location = New System.Drawing.Point(0, 121)
        Me.dgv_filter.Name = "dgv_filter"
        Me.dgv_filter.RowHeadersVisible = False
        Me.dgv_filter.RowTemplate.Height = 24
        Me.dgv_filter.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_filter.ShowRowErrors = False
        Me.dgv_filter.Size = New System.Drawing.Size(635, 161)
        Me.dgv_filter.TabIndex = 5
        '
        'dc
        '
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dc.DefaultCellStyle = DataGridViewCellStyle1
        Me.dc.HeaderText = "Vou.No"
        Me.dc.MaxInputLength = 8
        Me.dc.Name = "dc"
        Me.dc.ReadOnly = True
        Me.dc.Width = 60
        '
        'Column1
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column1.DefaultCellStyle = DataGridViewCellStyle2
        Me.Column1.HeaderText = "DATE"
        Me.Column1.Name = "Column1"
        '
        'Column2
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle3
        Me.Column2.HeaderText = "BANK NAME"
        Me.Column2.MaxInputLength = 35
        Me.Column2.Name = "Column2"
        Me.Column2.Width = 175
        '
        'Column4
        '
        Me.Column4.HeaderText = "PARTY NAME"
        Me.Column4.Name = "Column4"
        Me.Column4.Width = 175
        '
        'Column6
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column6.DefaultCellStyle = DataGridViewCellStyle4
        Me.Column6.HeaderText = "AMOUNT"
        Me.Column6.Name = "Column6"
        Me.Column6.Width = 95
        '
        'txt_ChequeAmt
        '
        Me.txt_ChequeAmt.Location = New System.Drawing.Point(145, 234)
        Me.txt_ChequeAmt.MaxLength = 20
        Me.txt_ChequeAmt.Name = "txt_ChequeAmt"
        Me.txt_ChequeAmt.Size = New System.Drawing.Size(148, 23)
        Me.txt_ChequeAmt.TabIndex = 5
        '
        'cbo_Filter_PartyName
        '
        Me.cbo_Filter_PartyName.FormattingEnabled = True
        Me.cbo_Filter_PartyName.Location = New System.Drawing.Point(122, 82)
        Me.cbo_Filter_PartyName.Name = "cbo_Filter_PartyName"
        Me.cbo_Filter_PartyName.Size = New System.Drawing.Size(397, 25)
        Me.cbo_Filter_PartyName.TabIndex = 2
        '
        'dtp_FilterFrom_date
        '
        Me.dtp_FilterFrom_date.CustomFormat = "dd-MM-yyyy"
        Me.dtp_FilterFrom_date.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtp_FilterFrom_date.Location = New System.Drawing.Point(122, 45)
        Me.dtp_FilterFrom_date.Name = "dtp_FilterFrom_date"
        Me.dtp_FilterFrom_date.Size = New System.Drawing.Size(116, 24)
        Me.dtp_FilterFrom_date.TabIndex = 0
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(300, 51)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(22, 17)
        Me.Label14.TabIndex = 1
        Me.Label14.Text = "To"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(14, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(74, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Voucher No."
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(12, 86)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(76, 17)
        Me.Label15.TabIndex = 2
        Me.Label15.Text = "Party Name"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(8, 260)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(0, 15)
        Me.Label7.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(256, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(33, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Date"
        '
        'dtp_FilterTo_date
        '
        Me.dtp_FilterTo_date.CustomFormat = "dd-MM-yyyy"
        Me.dtp_FilterTo_date.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtp_FilterTo_date.Location = New System.Drawing.Point(402, 45)
        Me.dtp_FilterTo_date.Name = "dtp_FilterTo_date"
        Me.dtp_FilterTo_date.Size = New System.Drawing.Size(117, 24)
        Me.dtp_FilterTo_date.TabIndex = 1
        '
        'pnl_back
        '
        Me.pnl_back.AutoSize = True
        Me.pnl_back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_back.Controls.Add(Me.txt_Print_Name)
        Me.pnl_back.Controls.Add(Me.lbl_caption_Agent)
        Me.pnl_back.Controls.Add(Me.cbo_AgentName)
        Me.pnl_back.Controls.Add(Me.cbo_ACPayee)
        Me.pnl_back.Controls.Add(Me.Label31)
        Me.pnl_back.Controls.Add(Me.lbl_Day)
        Me.pnl_back.Controls.Add(Me.txt_Narration)
        Me.pnl_back.Controls.Add(Me.Label21)
        Me.pnl_back.Controls.Add(Me.cbo_PartyName)
        Me.pnl_back.Controls.Add(Me.Label5)
        Me.pnl_back.Controls.Add(Me.msk_date)
        Me.pnl_back.Controls.Add(Me.btn_Print)
        Me.pnl_back.Controls.Add(Me.btn_close)
        Me.pnl_back.Controls.Add(Me.btn_save)
        Me.pnl_back.Controls.Add(Me.txt_ChequeNo)
        Me.pnl_back.Controls.Add(Me.Label4)
        Me.pnl_back.Controls.Add(Me.txt_ChequeAmt)
        Me.pnl_back.Controls.Add(Me.Label10)
        Me.pnl_back.Controls.Add(Me.cbo_Bank)
        Me.pnl_back.Controls.Add(Me.lbl_ReceiptNo)
        Me.pnl_back.Controls.Add(Me.dtp_Date)
        Me.pnl_back.Controls.Add(Me.Label9)
        Me.pnl_back.Controls.Add(Me.Label8)
        Me.pnl_back.Controls.Add(Me.Label1)
        Me.pnl_back.Controls.Add(Me.Label7)
        Me.pnl_back.Controls.Add(Me.Label2)
        Me.pnl_back.Controls.Add(Me.Label3)
        Me.pnl_back.Location = New System.Drawing.Point(6, 42)
        Me.pnl_back.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.pnl_back.Name = "pnl_back"
        Me.pnl_back.Size = New System.Drawing.Size(686, 372)
        Me.pnl_back.TabIndex = 30
        '
        'txt_Print_Name
        '
        Me.txt_Print_Name.Location = New System.Drawing.Point(144, 149)
        Me.txt_Print_Name.MaxLength = 200
        Me.txt_Print_Name.Name = "txt_Print_Name"
        Me.txt_Print_Name.Size = New System.Drawing.Size(515, 23)
        Me.txt_Print_Name.TabIndex = 3
        '
        'lbl_caption_Agent
        '
        Me.lbl_caption_Agent.AutoSize = True
        Me.lbl_caption_Agent.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_caption_Agent.ForeColor = System.Drawing.Color.Blue
        Me.lbl_caption_Agent.Location = New System.Drawing.Point(14, 152)
        Me.lbl_caption_Agent.Name = "lbl_caption_Agent"
        Me.lbl_caption_Agent.Size = New System.Drawing.Size(70, 15)
        Me.lbl_caption_Agent.TabIndex = 60
        Me.lbl_caption_Agent.Text = "Print Name"
        '
        'cbo_AgentName
        '
        Me.cbo_AgentName.BackColor = System.Drawing.Color.Red
        Me.cbo_AgentName.DropDownHeight = 80
        Me.cbo_AgentName.FormattingEnabled = True
        Me.cbo_AgentName.IntegralHeight = False
        Me.cbo_AgentName.Location = New System.Drawing.Point(17, 334)
        Me.cbo_AgentName.Name = "cbo_AgentName"
        Me.cbo_AgentName.Size = New System.Drawing.Size(174, 23)
        Me.cbo_AgentName.TabIndex = 3
        Me.cbo_AgentName.Visible = False
        '
        'cbo_ACPayee
        '
        Me.cbo_ACPayee.DropDownHeight = 125
        Me.cbo_ACPayee.FormattingEnabled = True
        Me.cbo_ACPayee.IntegralHeight = False
        Me.cbo_ACPayee.Location = New System.Drawing.Point(145, 187)
        Me.cbo_ACPayee.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.cbo_ACPayee.Name = "cbo_ACPayee"
        Me.cbo_ACPayee.Size = New System.Drawing.Size(515, 23)
        Me.cbo_ACPayee.TabIndex = 4
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.ForeColor = System.Drawing.Color.Blue
        Me.Label31.Location = New System.Drawing.Point(14, 62)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(69, 15)
        Me.Label31.TabIndex = 58
        Me.Label31.Text = "Bank Name"
        '
        'lbl_Day
        '
        Me.lbl_Day.BackColor = System.Drawing.Color.White
        Me.lbl_Day.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_Day.Location = New System.Drawing.Point(547, 15)
        Me.lbl_Day.Name = "lbl_Day"
        Me.lbl_Day.Size = New System.Drawing.Size(113, 23)
        Me.lbl_Day.TabIndex = 57
        Me.lbl_Day.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txt_Narration
        '
        Me.txt_Narration.Location = New System.Drawing.Point(145, 281)
        Me.txt_Narration.MaxLength = 200
        Me.txt_Narration.Name = "txt_Narration"
        Me.txt_Narration.Size = New System.Drawing.Size(515, 23)
        Me.txt_Narration.TabIndex = 7
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.ForeColor = System.Drawing.Color.Blue
        Me.Label21.Location = New System.Drawing.Point(314, 238)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(68, 15)
        Me.Label21.TabIndex = 41
        Me.Label21.Text = "Cheque No"
        '
        'cbo_PartyName
        '
        Me.cbo_PartyName.DropDownHeight = 125
        Me.cbo_PartyName.FormattingEnabled = True
        Me.cbo_PartyName.IntegralHeight = False
        Me.cbo_PartyName.Location = New System.Drawing.Point(145, 105)
        Me.cbo_PartyName.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.cbo_PartyName.Name = "cbo_PartyName"
        Me.cbo_PartyName.Size = New System.Drawing.Size(515, 23)
        Me.cbo_PartyName.TabIndex = 2
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.Blue
        Me.Label5.Location = New System.Drawing.Point(14, 109)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(72, 15)
        Me.Label5.TabIndex = 32
        Me.Label5.Text = "Party Name"
        '
        'msk_date
        '
        Me.msk_date.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.msk_date.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.msk_date.Location = New System.Drawing.Point(307, 15)
        Me.msk_date.Mask = "00-00-0000"
        Me.msk_date.Name = "msk_date"
        Me.msk_date.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.msk_date.Size = New System.Drawing.Size(140, 22)
        Me.msk_date.TabIndex = 0
        '
        'btn_Print
        '
        Me.btn_Print.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_Print.ForeColor = System.Drawing.Color.White
        Me.btn_Print.Location = New System.Drawing.Point(471, 323)
        Me.btn_Print.Name = "btn_Print"
        Me.btn_Print.Size = New System.Drawing.Size(82, 34)
        Me.btn_Print.TabIndex = 19
        Me.btn_Print.TabStop = False
        Me.btn_Print.Text = "&PRINT"
        Me.btn_Print.UseVisualStyleBackColor = False
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(577, 323)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(82, 34)
        Me.btn_close.TabIndex = 20
        Me.btn_close.TabStop = False
        Me.btn_close.Text = "&CLOSE"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'btn_save
        '
        Me.btn_save.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_save.ForeColor = System.Drawing.Color.White
        Me.btn_save.Location = New System.Drawing.Point(365, 323)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(82, 34)
        Me.btn_save.TabIndex = 8
        Me.btn_save.TabStop = False
        Me.btn_save.Text = "&SAVE"
        Me.btn_save.UseVisualStyleBackColor = False
        '
        'txt_ChequeNo
        '
        Me.txt_ChequeNo.Location = New System.Drawing.Point(412, 234)
        Me.txt_ChequeNo.MaxLength = 10
        Me.txt_ChequeNo.Name = "txt_ChequeNo"
        Me.txt_ChequeNo.Size = New System.Drawing.Size(248, 23)
        Me.txt_ChequeNo.TabIndex = 6
        '
        'Label4
        '
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(14, 191)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(113, 43)
        Me.Label4.TabIndex = 25
        Me.Label4.Text = "Ac Payee / NameCheque"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(8, 115)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(0, 15)
        Me.Label3.TabIndex = 2
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.Label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label11.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label11.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.White
        Me.Label11.Location = New System.Drawing.Point(0, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(711, 35)
        Me.Label11.TabIndex = 31
        Me.Label11.Text = "CHEQUE ENTRY"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbl_Company
        '
        Me.lbl_Company.AutoSize = True
        Me.lbl_Company.BackColor = System.Drawing.Color.Red
        Me.lbl_Company.ForeColor = System.Drawing.Color.White
        Me.lbl_Company.Location = New System.Drawing.Point(57, -67)
        Me.lbl_Company.Name = "lbl_Company"
        Me.lbl_Company.Size = New System.Drawing.Size(77, 15)
        Me.lbl_Company.TabIndex = 33
        Me.lbl_Company.Text = "lbl_Company"
        Me.lbl_Company.Visible = False
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(12, 51)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(63, 17)
        Me.Label13.TabIndex = 0
        Me.Label13.Text = "Vou.Date"
        '
        'pnl_filter
        '
        Me.pnl_filter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_filter.Controls.Add(Me.btn_closefilter)
        Me.pnl_filter.Controls.Add(Me.btn_filtershow)
        Me.pnl_filter.Controls.Add(Me.dgv_filter)
        Me.pnl_filter.Controls.Add(Me.cbo_Filter_PartyName)
        Me.pnl_filter.Controls.Add(Me.dtp_FilterTo_date)
        Me.pnl_filter.Controls.Add(Me.dtp_FilterFrom_date)
        Me.pnl_filter.Controls.Add(Me.Label15)
        Me.pnl_filter.Controls.Add(Me.Label14)
        Me.pnl_filter.Controls.Add(Me.Label13)
        Me.pnl_filter.Controls.Add(Me.Label16)
        Me.pnl_filter.Font = New System.Drawing.Font("Calibri", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnl_filter.Location = New System.Drawing.Point(926, 73)
        Me.pnl_filter.Name = "pnl_filter"
        Me.pnl_filter.Size = New System.Drawing.Size(637, 284)
        Me.pnl_filter.TabIndex = 32
        '
        'PrintDocument2
        '
        '
        'Cheque_Entry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(711, 431)
        Me.Controls.Add(Me.pnl_filter)
        Me.Controls.Add(Me.pnl_back)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.lbl_Company)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Cheque_Entry"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "CHEQUEE"
        CType(Me.dgv_filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnl_back.ResumeLayout(False)
        Me.pnl_back.PerformLayout()
        Me.pnl_filter.ResumeLayout(False)
        Me.pnl_filter.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
    Friend WithEvents dtp_Date As System.Windows.Forms.DateTimePicker
    Friend WithEvents btn_closefilter As System.Windows.Forms.Button
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lbl_ReceiptNo As System.Windows.Forms.Label
    Friend WithEvents btn_filtershow As System.Windows.Forms.Button
    Friend WithEvents cbo_Bank As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents dgv_filter As System.Windows.Forms.DataGridView
    Friend WithEvents txt_ChequeAmt As System.Windows.Forms.TextBox
    Friend WithEvents cbo_Filter_PartyName As System.Windows.Forms.ComboBox
    Friend WithEvents dtp_FilterFrom_date As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dtp_FilterTo_date As System.Windows.Forms.DateTimePicker
    Friend WithEvents pnl_back As System.Windows.Forms.Panel
    Friend WithEvents msk_date As System.Windows.Forms.MaskedTextBox
    Friend WithEvents btn_Print As System.Windows.Forms.Button
    Friend WithEvents btn_close As System.Windows.Forms.Button
    Friend WithEvents btn_save As System.Windows.Forms.Button
    Friend WithEvents txt_ChequeNo As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents lbl_Company As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents pnl_filter As System.Windows.Forms.Panel
    Friend WithEvents txt_Narration As System.Windows.Forms.TextBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents cbo_PartyName As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cbo_ACPayee As System.Windows.Forms.ComboBox
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents lbl_Day As System.Windows.Forms.Label
    Friend WithEvents PrintDocument2 As System.Drawing.Printing.PrintDocument
    Friend WithEvents dc As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lbl_caption_Agent As Label
    Friend WithEvents cbo_AgentName As ComboBox
    Friend WithEvents txt_Print_Name As TextBox
End Class
