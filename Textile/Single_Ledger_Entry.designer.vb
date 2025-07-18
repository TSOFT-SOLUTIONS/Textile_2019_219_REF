<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Single_Ledger_Entry
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
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.dgv_Details_Total = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumn23 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column9 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column10 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column11 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column12 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column13 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btn_Show = New System.Windows.Forms.Button()
        Me.btn_Print = New System.Windows.Forms.Button()
        Me.btn_save = New System.Windows.Forms.Button()
        Me.dgv_Details = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column14 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column15 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column16 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.msk_Todate = New System.Windows.Forms.MaskedTextBox()
        Me.dtp_ToDate = New System.Windows.Forms.DateTimePicker()
        Me.msk_Fromdate = New System.Windows.Forms.MaskedTextBox()
        Me.dtp_FromDate = New System.Windows.Forms.DateTimePicker()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lbl_Ledger_Name = New System.Windows.Forms.Label()
        Me.cbo_AccountName = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lbl_Company = New System.Windows.Forms.Label()
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument()
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog()
        Me.pnl_Print = New System.Windows.Forms.Panel()
        Me.btn_Print_Empty = New System.Windows.Forms.Button()
        Me.btn_Print_Cancel = New System.Windows.Forms.Button()
        Me.btn_Print_Entry = New System.Windows.Forms.Button()
        Me.btn_Close_Print = New System.Windows.Forms.Button()
        Me.lbl_PrintPanel_Caption = New System.Windows.Forms.Label()
        Me.pnl_Back.SuspendLayout()
        CType(Me.dgv_Details_Total, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgv_Details, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnl_Print.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnl_Back
        '
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.btn_Close)
        Me.pnl_Back.Controls.Add(Me.dgv_Details_Total)
        Me.pnl_Back.Controls.Add(Me.btn_Show)
        Me.pnl_Back.Controls.Add(Me.btn_Print)
        Me.pnl_Back.Controls.Add(Me.btn_save)
        Me.pnl_Back.Controls.Add(Me.dgv_Details)
        Me.pnl_Back.Controls.Add(Me.Label1)
        Me.pnl_Back.Controls.Add(Me.msk_Todate)
        Me.pnl_Back.Controls.Add(Me.dtp_ToDate)
        Me.pnl_Back.Controls.Add(Me.msk_Fromdate)
        Me.pnl_Back.Controls.Add(Me.dtp_FromDate)
        Me.pnl_Back.Controls.Add(Me.Label4)
        Me.pnl_Back.Controls.Add(Me.lbl_Ledger_Name)
        Me.pnl_Back.Controls.Add(Me.cbo_AccountName)
        Me.pnl_Back.Location = New System.Drawing.Point(4, 40)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(968, 478)
        Me.pnl_Back.TabIndex = 5
        '
        'btn_Close
        '
        Me.btn_Close.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_Close.ForeColor = System.Drawing.Color.White
        Me.btn_Close.Location = New System.Drawing.Point(874, 431)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(73, 30)
        Me.btn_Close.TabIndex = 60
        Me.btn_Close.TabStop = False
        Me.btn_Close.Text = "&Close"
        Me.btn_Close.UseVisualStyleBackColor = False
        '
        'dgv_Details_Total
        '
        Me.dgv_Details_Total.AllowUserToAddRows = False
        Me.dgv_Details_Total.AllowUserToDeleteRows = False
        Me.dgv_Details_Total.AllowUserToResizeColumns = False
        Me.dgv_Details_Total.AllowUserToResizeRows = False
        Me.dgv_Details_Total.BackgroundColor = System.Drawing.Color.Gainsboro
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.DarkSlateGray
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_Details_Total.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgv_Details_Total.ColumnHeadersHeight = 35
        Me.dgv_Details_Total.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgv_Details_Total.ColumnHeadersVisible = False
        Me.dgv_Details_Total.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn23, Me.Column8, Me.Column9, Me.Column10, Me.Column11, Me.Column12, Me.Column13})
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.Color.Gainsboro
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.Gainsboro
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgv_Details_Total.DefaultCellStyle = DataGridViewCellStyle6
        Me.dgv_Details_Total.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv_Details_Total.Enabled = False
        Me.dgv_Details_Total.EnableHeadersVisualStyles = False
        Me.dgv_Details_Total.Location = New System.Drawing.Point(11, 400)
        Me.dgv_Details_Total.MultiSelect = False
        Me.dgv_Details_Total.Name = "dgv_Details_Total"
        Me.dgv_Details_Total.RowHeadersVisible = False
        Me.dgv_Details_Total.RowHeadersWidth = 15
        Me.dgv_Details_Total.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgv_Details_Total.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.dgv_Details_Total.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.dgv_Details_Total.Size = New System.Drawing.Size(936, 25)
        Me.dgv_Details_Total.TabIndex = 59
        Me.dgv_Details_Total.TabStop = False
        '
        'DataGridViewTextBoxColumn23
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGridViewTextBoxColumn23.DefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridViewTextBoxColumn23.Frozen = True
        Me.DataGridViewTextBoxColumn23.HeaderText = "DATE"
        Me.DataGridViewTextBoxColumn23.Name = "DataGridViewTextBoxColumn23"
        Me.DataGridViewTextBoxColumn23.ReadOnly = True
        '
        'Column8
        '
        Me.Column8.HeaderText = "PARTICULARS"
        Me.Column8.Name = "Column8"
        Me.Column8.Width = 420
        '
        'Column9
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column9.DefaultCellStyle = DataGridViewCellStyle3
        Me.Column9.HeaderText = "DEBIT"
        Me.Column9.Name = "Column9"
        '
        'Column10
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column10.DefaultCellStyle = DataGridViewCellStyle4
        Me.Column10.HeaderText = "CREDIT"
        Me.Column10.Name = "Column10"
        '
        'Column11
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column11.DefaultCellStyle = DataGridViewCellStyle5
        Me.Column11.HeaderText = "BALANCE"
        Me.Column11.Name = "Column11"
        '
        'Column12
        '
        Me.Column12.HeaderText = "PRINT"
        Me.Column12.Name = "Column12"
        Me.Column12.Width = 50
        '
        'Column13
        '
        Me.Column13.HeaderText = "PAGE NO"
        Me.Column13.Name = "Column13"
        Me.Column13.Width = 50
        '
        'btn_Show
        '
        Me.btn_Show.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_Show.ForeColor = System.Drawing.Color.White
        Me.btn_Show.Location = New System.Drawing.Point(882, 3)
        Me.btn_Show.Name = "btn_Show"
        Me.btn_Show.Size = New System.Drawing.Size(65, 30)
        Me.btn_Show.TabIndex = 18
        Me.btn_Show.TabStop = False
        Me.btn_Show.Text = "SHOW"
        Me.btn_Show.UseVisualStyleBackColor = False
        '
        'btn_Print
        '
        Me.btn_Print.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_Print.ForeColor = System.Drawing.Color.White
        Me.btn_Print.Location = New System.Drawing.Point(798, 431)
        Me.btn_Print.Name = "btn_Print"
        Me.btn_Print.Size = New System.Drawing.Size(73, 30)
        Me.btn_Print.TabIndex = 16
        Me.btn_Print.TabStop = False
        Me.btn_Print.Text = "&PRINT"
        Me.btn_Print.UseVisualStyleBackColor = False
        '
        'btn_save
        '
        Me.btn_save.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_save.ForeColor = System.Drawing.Color.White
        Me.btn_save.Location = New System.Drawing.Point(719, 431)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(73, 30)
        Me.btn_save.TabIndex = 15
        Me.btn_save.TabStop = False
        Me.btn_save.Text = "&SAVE"
        Me.btn_save.UseVisualStyleBackColor = False
        '
        'dgv_Details
        '
        Me.dgv_Details.AllowUserToAddRows = False
        Me.dgv_Details.AllowUserToDeleteRows = False
        Me.dgv_Details.AllowUserToResizeColumns = False
        Me.dgv_Details.AllowUserToResizeRows = False
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_Details.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle7
        Me.dgv_Details.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle8.BackColor = System.Drawing.Color.MidnightBlue
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle8.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_Details.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle8
        Me.dgv_Details.ColumnHeadersHeight = 25
        Me.dgv_Details.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.Column4, Me.Column5, Me.Column6, Me.Column7, Me.Column14, Me.Column15, Me.Column16})
        DataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle15.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle15.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle15.SelectionBackColor = System.Drawing.Color.SteelBlue
        DataGridViewCellStyle15.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgv_Details.DefaultCellStyle = DataGridViewCellStyle15
        Me.dgv_Details.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.dgv_Details.EnableHeadersVisualStyles = False
        Me.dgv_Details.Location = New System.Drawing.Point(11, 36)
        Me.dgv_Details.Name = "dgv_Details"
        Me.dgv_Details.RowHeadersVisible = False
        Me.dgv_Details.RowHeadersWidth = 20
        Me.dgv_Details.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Details.Size = New System.Drawing.Size(936, 380)
        Me.dgv_Details.StandardTab = True
        Me.dgv_Details.TabIndex = 14
        Me.dgv_Details.TabStop = False
        '
        'Column1
        '
        Me.Column1.HeaderText = "DATE"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        '
        'Column2
        '
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle9
        Me.Column2.HeaderText = "ACCOUNTS DETAILS"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Width = 420
        '
        'Column3
        '
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column3.DefaultCellStyle = DataGridViewCellStyle10
        Me.Column3.HeaderText = "CREDIT"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        '
        'Column4
        '
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column4.DefaultCellStyle = DataGridViewCellStyle11
        Me.Column4.HeaderText = "DEBIT"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        '
        'Column5
        '
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column5.DefaultCellStyle = DataGridViewCellStyle12
        Me.Column5.HeaderText = "BALANCE"
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        '
        'Column6
        '
        DataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Column6.DefaultCellStyle = DataGridViewCellStyle13
        Me.Column6.HeaderText = "PRINT"
        Me.Column6.Name = "Column6"
        Me.Column6.Width = 50
        '
        'Column7
        '
        DataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Column7.DefaultCellStyle = DataGridViewCellStyle14
        Me.Column7.HeaderText = "PAGE"
        Me.Column7.Name = "Column7"
        Me.Column7.ReadOnly = True
        Me.Column7.Width = 50
        '
        'Column14
        '
        Me.Column14.HeaderText = "voucher_code"
        Me.Column14.Name = "Column14"
        Me.Column14.Visible = False
        '
        'Column15
        '
        Me.Column15.HeaderText = "voucher_type"
        Me.Column15.Name = "Column15"
        Me.Column15.Visible = False
        '
        'Column16
        '
        Me.Column16.HeaderText = "row_no"
        Me.Column16.Name = "Column16"
        Me.Column16.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(728, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(22, 15)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "TO"
        '
        'msk_Todate
        '
        Me.msk_Todate.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.msk_Todate.Location = New System.Drawing.Point(758, 6)
        Me.msk_Todate.Mask = "00-00-0000"
        Me.msk_Todate.Name = "msk_Todate"
        Me.msk_Todate.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.msk_Todate.Size = New System.Drawing.Size(102, 22)
        Me.msk_Todate.TabIndex = 12
        '
        'dtp_ToDate
        '
        Me.dtp_ToDate.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_ToDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_ToDate.Location = New System.Drawing.Point(858, 6)
        Me.dtp_ToDate.Name = "dtp_ToDate"
        Me.dtp_ToDate.Size = New System.Drawing.Size(17, 22)
        Me.dtp_ToDate.TabIndex = 11
        Me.dtp_ToDate.TabStop = False
        '
        'msk_Fromdate
        '
        Me.msk_Fromdate.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.msk_Fromdate.Location = New System.Drawing.Point(584, 6)
        Me.msk_Fromdate.Mask = "00-00-0000"
        Me.msk_Fromdate.Name = "msk_Fromdate"
        Me.msk_Fromdate.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.msk_Fromdate.Size = New System.Drawing.Size(102, 22)
        Me.msk_Fromdate.TabIndex = 9
        '
        'dtp_FromDate
        '
        Me.dtp_FromDate.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_FromDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_FromDate.Location = New System.Drawing.Point(685, 6)
        Me.dtp_FromDate.Name = "dtp_FromDate"
        Me.dtp_FromDate.Size = New System.Drawing.Size(18, 22)
        Me.dtp_FromDate.TabIndex = 8
        Me.dtp_FromDate.TabStop = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(505, 11)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(70, 15)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "DATE FROM"
        '
        'lbl_Ledger_Name
        '
        Me.lbl_Ledger_Name.AutoSize = True
        Me.lbl_Ledger_Name.Location = New System.Drawing.Point(8, 11)
        Me.lbl_Ledger_Name.Name = "lbl_Ledger_Name"
        Me.lbl_Ledger_Name.Size = New System.Drawing.Size(98, 15)
        Me.lbl_Ledger_Name.TabIndex = 1
        Me.lbl_Ledger_Name.Text = "ACCOUNT NAME"
        '
        'cbo_AccountName
        '
        Me.cbo_AccountName.FormattingEnabled = True
        Me.cbo_AccountName.Location = New System.Drawing.Point(112, 6)
        Me.cbo_AccountName.Name = "cbo_AccountName"
        Me.cbo_AccountName.Size = New System.Drawing.Size(373, 23)
        Me.cbo_AccountName.TabIndex = 0
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label3.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(0, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(986, 26)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "SINGLE LEDGER"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbl_Company
        '
        Me.lbl_Company.AutoSize = True
        Me.lbl_Company.BackColor = System.Drawing.Color.Red
        Me.lbl_Company.Location = New System.Drawing.Point(134, 8)
        Me.lbl_Company.Name = "lbl_Company"
        Me.lbl_Company.Size = New System.Drawing.Size(77, 15)
        Me.lbl_Company.TabIndex = 131
        Me.lbl_Company.Text = "lbl_Company"
        Me.lbl_Company.Visible = False
        '
        'PrintDocument1
        '
        '
        'PrintDialog1
        '
        Me.PrintDialog1.UseEXDialog = True
        '
        'pnl_Print
        '
        Me.pnl_Print.BackColor = System.Drawing.Color.DarkCyan
        Me.pnl_Print.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Print.Controls.Add(Me.btn_Print_Empty)
        Me.pnl_Print.Controls.Add(Me.btn_Print_Cancel)
        Me.pnl_Print.Controls.Add(Me.btn_Print_Entry)
        Me.pnl_Print.Controls.Add(Me.btn_Close_Print)
        Me.pnl_Print.Controls.Add(Me.lbl_PrintPanel_Caption)
        Me.pnl_Print.Location = New System.Drawing.Point(1086, 237)
        Me.pnl_Print.Name = "pnl_Print"
        Me.pnl_Print.Size = New System.Drawing.Size(329, 115)
        Me.pnl_Print.TabIndex = 262
        '
        'btn_Print_Empty
        '
        Me.btn_Print_Empty.BackColor = System.Drawing.Color.DarkSlateGray
        Me.btn_Print_Empty.ForeColor = System.Drawing.Color.White
        Me.btn_Print_Empty.Location = New System.Drawing.Point(118, 51)
        Me.btn_Print_Empty.Name = "btn_Print_Empty"
        Me.btn_Print_Empty.Size = New System.Drawing.Size(93, 32)
        Me.btn_Print_Empty.TabIndex = 45
        Me.btn_Print_Empty.Text = "EMPTY PRINT "
        Me.btn_Print_Empty.UseVisualStyleBackColor = False
        '
        'btn_Print_Cancel
        '
        Me.btn_Print_Cancel.BackColor = System.Drawing.Color.DarkSlateGray
        Me.btn_Print_Cancel.ForeColor = System.Drawing.Color.White
        Me.btn_Print_Cancel.Location = New System.Drawing.Point(217, 51)
        Me.btn_Print_Cancel.Name = "btn_Print_Cancel"
        Me.btn_Print_Cancel.Size = New System.Drawing.Size(83, 32)
        Me.btn_Print_Cancel.TabIndex = 46
        Me.btn_Print_Cancel.Text = "&CANCEL"
        Me.btn_Print_Cancel.UseVisualStyleBackColor = False
        '
        'btn_Print_Entry
        '
        Me.btn_Print_Entry.BackColor = System.Drawing.Color.DarkSlateGray
        Me.btn_Print_Entry.ForeColor = System.Drawing.Color.White
        Me.btn_Print_Entry.Location = New System.Drawing.Point(27, 51)
        Me.btn_Print_Entry.Name = "btn_Print_Entry"
        Me.btn_Print_Entry.Size = New System.Drawing.Size(85, 32)
        Me.btn_Print_Entry.TabIndex = 44
        Me.btn_Print_Entry.Text = "PRINT"
        Me.btn_Print_Entry.UseVisualStyleBackColor = False
        '
        'btn_Close_Print
        '
        Me.btn_Close_Print.BackColor = System.Drawing.Color.White
        Me.btn_Close_Print.BackgroundImage = Global.Textile.My.Resources.Resources.Close1
        Me.btn_Close_Print.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn_Close_Print.FlatAppearance.BorderSize = 0
        Me.btn_Close_Print.Location = New System.Drawing.Point(300, 0)
        Me.btn_Close_Print.Name = "btn_Close_Print"
        Me.btn_Close_Print.Size = New System.Drawing.Size(27, 25)
        Me.btn_Close_Print.TabIndex = 47
        Me.btn_Close_Print.TabStop = False
        Me.btn_Close_Print.UseVisualStyleBackColor = True
        '
        'lbl_PrintPanel_Caption
        '
        Me.lbl_PrintPanel_Caption.BackColor = System.Drawing.Color.DarkSlateGray
        Me.lbl_PrintPanel_Caption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_PrintPanel_Caption.Dock = System.Windows.Forms.DockStyle.Top
        Me.lbl_PrintPanel_Caption.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_PrintPanel_Caption.ForeColor = System.Drawing.Color.White
        Me.lbl_PrintPanel_Caption.Location = New System.Drawing.Point(0, 0)
        Me.lbl_PrintPanel_Caption.Name = "lbl_PrintPanel_Caption"
        Me.lbl_PrintPanel_Caption.Size = New System.Drawing.Size(327, 24)
        Me.lbl_PrintPanel_Caption.TabIndex = 43
        Me.lbl_PrintPanel_Caption.Text = "PREVIEW OPTION"
        Me.lbl_PrintPanel_Caption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Single_Ledger_Entry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(986, 530)
        Me.Controls.Add(Me.pnl_Print)
        Me.Controls.Add(Me.lbl_Company)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.pnl_Back)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Single_Ledger_Entry"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SINGLE LEDGER"
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_Back.PerformLayout()
        CType(Me.dgv_Details_Total, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgv_Details, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnl_Print.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lbl_Ledger_Name As System.Windows.Forms.Label
    Friend WithEvents cbo_AccountName As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents msk_Todate As System.Windows.Forms.MaskedTextBox
    Friend WithEvents dtp_ToDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents msk_Fromdate As System.Windows.Forms.MaskedTextBox
    Friend WithEvents dtp_FromDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents dgv_Details As System.Windows.Forms.DataGridView
    Friend WithEvents lbl_Company As System.Windows.Forms.Label
    Friend WithEvents btn_Print As System.Windows.Forms.Button
    Friend WithEvents btn_save As System.Windows.Forms.Button
    Friend WithEvents btn_Show As System.Windows.Forms.Button
    Friend WithEvents dgv_Details_Total As System.Windows.Forms.DataGridView
    Friend WithEvents DataGridViewTextBoxColumn23 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column8 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column9 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column10 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column11 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column12 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column13 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PrintDocument1 As System.Drawing.Printing.PrintDocument
    Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
    Friend WithEvents pnl_Print As System.Windows.Forms.Panel
    Friend WithEvents btn_Print_Empty As System.Windows.Forms.Button
    Friend WithEvents btn_Print_Cancel As System.Windows.Forms.Button
    Friend WithEvents btn_Print_Entry As System.Windows.Forms.Button
    Friend WithEvents btn_Close_Print As System.Windows.Forms.Button
    Friend WithEvents lbl_PrintPanel_Caption As System.Windows.Forms.Label
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column7 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column14 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column15 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column16 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents btn_Close As System.Windows.Forms.Button
End Class
