<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Knotting_Bill_Entry
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
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.btn_UserModification = New System.Windows.Forms.Button()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txt_CGSTPerc = New System.Windows.Forms.TextBox()
        Me.txt_IGSTPerc = New System.Windows.Forms.TextBox()
        Me.txt_SGSTPerc = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.cbo_KnottingAc = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label61 = New System.Windows.Forms.Label()
        Me.lbl_SGST_Amount = New System.Windows.Forms.Label()
        Me.lbl_IGST_Amount = New System.Windows.Forms.Label()
        Me.lbl_CGST_Amount = New System.Windows.Forms.Label()
        Me.Label59 = New System.Windows.Forms.Label()
        Me.cbo_HSNCode = New System.Windows.Forms.ComboBox()
        Me.cbo_Ledger = New System.Windows.Forms.ComboBox()
        Me.dtp_RefDate = New System.Windows.Forms.DateTimePicker()
        Me.btn_close = New System.Windows.Forms.Button()
        Me.btn_save = New System.Windows.Forms.Button()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.txt_Note = New System.Windows.Forms.TextBox()
        Me.txt_Amount = New System.Windows.Forms.TextBox()
        Me.txt_No_of_Beams = New System.Windows.Forms.TextBox()
        Me.lbl_TaxableValue = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.lbl_NetAmount = New System.Windows.Forms.Label()
        Me.txt_Description = New System.Windows.Forms.TextBox()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.Label38 = New System.Windows.Forms.Label()
        Me.msk_RefDate = New System.Windows.Forms.MaskedTextBox()
        Me.lbl_RefNo = New System.Windows.Forms.Label()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.Label43 = New System.Windows.Forms.Label()
        Me.Label44 = New System.Windows.Forms.Label()
        Me.lbl_grid_GstPerc = New System.Windows.Forms.Label()
        Me.lbl_RoundOff = New System.Windows.Forms.Label()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.pnl_Filter = New System.Windows.Forms.Panel()
        Me.btn_Fliter_Close = New System.Windows.Forms.Button()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cbo_Filter_HSNCode = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btn_Filter_Show = New System.Windows.Forms.Button()
        Me.dgv_filter = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column9 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column11 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cbo_Filter_PartyName = New System.Windows.Forms.ComboBox()
        Me.dtp_FilterTo_date = New System.Windows.Forms.DateTimePicker()
        Me.dtp_FilterFrom_date = New System.Windows.Forms.DateTimePicker()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lbl_Company = New System.Windows.Forms.Label()
        Me.txt_BillNo = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.pnl_Back.SuspendLayout()
        Me.pnl_Filter.SuspendLayout()
        CType(Me.dgv_filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.DimGray
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label3.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(0, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(750, 35)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "KNOTTING BILL ENTRY"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnl_Back
        '
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.txt_BillNo)
        Me.pnl_Back.Controls.Add(Me.Label14)
        Me.pnl_Back.Controls.Add(Me.btn_UserModification)
        Me.pnl_Back.Controls.Add(Me.Label13)
        Me.pnl_Back.Controls.Add(Me.Label8)
        Me.pnl_Back.Controls.Add(Me.txt_CGSTPerc)
        Me.pnl_Back.Controls.Add(Me.txt_IGSTPerc)
        Me.pnl_Back.Controls.Add(Me.txt_SGSTPerc)
        Me.pnl_Back.Controls.Add(Me.Label12)
        Me.pnl_Back.Controls.Add(Me.Label11)
        Me.pnl_Back.Controls.Add(Me.Label33)
        Me.pnl_Back.Controls.Add(Me.cbo_KnottingAc)
        Me.pnl_Back.Controls.Add(Me.Label1)
        Me.pnl_Back.Controls.Add(Me.Label7)
        Me.pnl_Back.Controls.Add(Me.Label61)
        Me.pnl_Back.Controls.Add(Me.lbl_SGST_Amount)
        Me.pnl_Back.Controls.Add(Me.lbl_IGST_Amount)
        Me.pnl_Back.Controls.Add(Me.lbl_CGST_Amount)
        Me.pnl_Back.Controls.Add(Me.Label59)
        Me.pnl_Back.Controls.Add(Me.cbo_HSNCode)
        Me.pnl_Back.Controls.Add(Me.cbo_Ledger)
        Me.pnl_Back.Controls.Add(Me.dtp_RefDate)
        Me.pnl_Back.Controls.Add(Me.btn_close)
        Me.pnl_Back.Controls.Add(Me.btn_save)
        Me.pnl_Back.Controls.Add(Me.Label26)
        Me.pnl_Back.Controls.Add(Me.txt_Note)
        Me.pnl_Back.Controls.Add(Me.txt_Amount)
        Me.pnl_Back.Controls.Add(Me.txt_No_of_Beams)
        Me.pnl_Back.Controls.Add(Me.lbl_TaxableValue)
        Me.pnl_Back.Controls.Add(Me.Label9)
        Me.pnl_Back.Controls.Add(Me.lbl_NetAmount)
        Me.pnl_Back.Controls.Add(Me.txt_Description)
        Me.pnl_Back.Controls.Add(Me.Label31)
        Me.pnl_Back.Controls.Add(Me.Label34)
        Me.pnl_Back.Controls.Add(Me.Label35)
        Me.pnl_Back.Controls.Add(Me.Label36)
        Me.pnl_Back.Controls.Add(Me.Label38)
        Me.pnl_Back.Controls.Add(Me.msk_RefDate)
        Me.pnl_Back.Controls.Add(Me.lbl_RefNo)
        Me.pnl_Back.Controls.Add(Me.Label42)
        Me.pnl_Back.Controls.Add(Me.Label43)
        Me.pnl_Back.Controls.Add(Me.Label44)
        Me.pnl_Back.Location = New System.Drawing.Point(7, 44)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(718, 307)
        Me.pnl_Back.TabIndex = 7
        '
        'btn_UserModification
        '
        Me.btn_UserModification.BackColor = System.Drawing.Color.OrangeRed
        Me.btn_UserModification.ForeColor = System.Drawing.Color.White
        Me.btn_UserModification.Location = New System.Drawing.Point(39, 277)
        Me.btn_UserModification.Name = "btn_UserModification"
        Me.btn_UserModification.Size = New System.Drawing.Size(103, 25)
        Me.btn_UserModification.TabIndex = 1178
        Me.btn_UserModification.TabStop = False
        Me.btn_UserModification.Text = "MODIFICATION"
        Me.btn_UserModification.UseVisualStyleBackColor = False
        Me.btn_UserModification.Visible = False
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.Color.Red
        Me.Label13.Location = New System.Drawing.Point(230, 14)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(13, 15)
        Me.Label13.TabIndex = 330
        Me.Label13.Text = "*"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Red
        Me.Label8.Location = New System.Drawing.Point(459, 16)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(13, 15)
        Me.Label8.TabIndex = 330
        Me.Label8.Text = "*"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_CGSTPerc
        '
        Me.txt_CGSTPerc.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_CGSTPerc.Location = New System.Drawing.Point(100, 163)
        Me.txt_CGSTPerc.MaxLength = 5
        Me.txt_CGSTPerc.Name = "txt_CGSTPerc"
        Me.txt_CGSTPerc.Size = New System.Drawing.Size(88, 23)
        Me.txt_CGSTPerc.TabIndex = 8
        Me.txt_CGSTPerc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_IGSTPerc
        '
        Me.txt_IGSTPerc.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_IGSTPerc.Location = New System.Drawing.Point(102, 203)
        Me.txt_IGSTPerc.MaxLength = 5
        Me.txt_IGSTPerc.Name = "txt_IGSTPerc"
        Me.txt_IGSTPerc.Size = New System.Drawing.Size(86, 23)
        Me.txt_IGSTPerc.TabIndex = 10
        Me.txt_IGSTPerc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_SGSTPerc
        '
        Me.txt_SGSTPerc.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_SGSTPerc.Location = New System.Drawing.Point(473, 161)
        Me.txt_SGSTPerc.MaxLength = 5
        Me.txt_SGSTPerc.Name = "txt_SGSTPerc"
        Me.txt_SGSTPerc.Size = New System.Drawing.Size(74, 23)
        Me.txt_SGSTPerc.TabIndex = 9
        Me.txt_SGSTPerc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.Blue
        Me.Label12.Location = New System.Drawing.Point(210, 168)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(16, 15)
        Me.Label12.TabIndex = 329
        Me.Label12.Text = "%"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.Blue
        Me.Label11.Location = New System.Drawing.Point(553, 165)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(16, 15)
        Me.Label11.TabIndex = 328
        Me.Label11.Text = "%"
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label33.ForeColor = System.Drawing.Color.Blue
        Me.Label33.Location = New System.Drawing.Point(210, 210)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(16, 15)
        Me.Label33.TabIndex = 327
        Me.Label33.Text = "%"
        '
        'cbo_KnottingAc
        '
        Me.cbo_KnottingAc.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_KnottingAc.FormattingEnabled = True
        Me.cbo_KnottingAc.Location = New System.Drawing.Point(137, 88)
        Me.cbo_KnottingAc.Name = "cbo_KnottingAc"
        Me.cbo_KnottingAc.Size = New System.Drawing.Size(244, 23)
        Me.cbo_KnottingAc.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(5, 92)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(123, 15)
        Me.Label1.TabIndex = 326
        Me.Label1.Text = "Knotting Charges A/C"
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Blue
        Me.Label7.Location = New System.Drawing.Point(5, 205)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(71, 21)
        Me.Label7.TabIndex = 321
        Me.Label7.Text = "IGST Value"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label61
        '
        Me.Label61.BackColor = System.Drawing.Color.Transparent
        Me.Label61.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label61.ForeColor = System.Drawing.Color.Blue
        Me.Label61.Location = New System.Drawing.Point(390, 162)
        Me.Label61.Name = "Label61"
        Me.Label61.Size = New System.Drawing.Size(78, 21)
        Me.Label61.TabIndex = 320
        Me.Label61.Text = "SGST Value"
        Me.Label61.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbl_SGST_Amount
        '
        Me.lbl_SGST_Amount.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lbl_SGST_Amount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_SGST_Amount.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_SGST_Amount.Location = New System.Drawing.Point(576, 161)
        Me.lbl_SGST_Amount.Name = "lbl_SGST_Amount"
        Me.lbl_SGST_Amount.Size = New System.Drawing.Size(126, 23)
        Me.lbl_SGST_Amount.TabIndex = 319
        Me.lbl_SGST_Amount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbl_IGST_Amount
        '
        Me.lbl_IGST_Amount.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lbl_IGST_Amount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_IGST_Amount.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_IGST_Amount.Location = New System.Drawing.Point(255, 205)
        Me.lbl_IGST_Amount.Name = "lbl_IGST_Amount"
        Me.lbl_IGST_Amount.Size = New System.Drawing.Size(126, 23)
        Me.lbl_IGST_Amount.TabIndex = 318
        Me.lbl_IGST_Amount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbl_CGST_Amount
        '
        Me.lbl_CGST_Amount.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lbl_CGST_Amount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_CGST_Amount.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_CGST_Amount.Location = New System.Drawing.Point(255, 164)
        Me.lbl_CGST_Amount.Name = "lbl_CGST_Amount"
        Me.lbl_CGST_Amount.Size = New System.Drawing.Size(126, 23)
        Me.lbl_CGST_Amount.TabIndex = 315
        Me.lbl_CGST_Amount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label59
        '
        Me.Label59.BackColor = System.Drawing.Color.Transparent
        Me.Label59.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label59.ForeColor = System.Drawing.Color.Blue
        Me.Label59.Location = New System.Drawing.Point(5, 163)
        Me.Label59.Name = "Label59"
        Me.Label59.Size = New System.Drawing.Size(76, 21)
        Me.Label59.TabIndex = 314
        Me.Label59.Text = "CGST Value"
        Me.Label59.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbo_HSNCode
        '
        Me.cbo_HSNCode.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_HSNCode.FormattingEnabled = True
        Me.cbo_HSNCode.Location = New System.Drawing.Point(469, 88)
        Me.cbo_HSNCode.Name = "cbo_HSNCode"
        Me.cbo_HSNCode.Size = New System.Drawing.Size(238, 23)
        Me.cbo_HSNCode.TabIndex = 5
        Me.cbo_HSNCode.Text = "cbo_HSNCode"
        '
        'cbo_Ledger
        '
        Me.cbo_Ledger.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Ledger.FormattingEnabled = True
        Me.cbo_Ledger.Location = New System.Drawing.Point(483, 12)
        Me.cbo_Ledger.Name = "cbo_Ledger"
        Me.cbo_Ledger.Size = New System.Drawing.Size(224, 23)
        Me.cbo_Ledger.TabIndex = 2
        '
        'dtp_RefDate
        '
        Me.dtp_RefDate.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_RefDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_RefDate.Location = New System.Drawing.Point(368, 12)
        Me.dtp_RefDate.Name = "dtp_RefDate"
        Me.dtp_RefDate.Size = New System.Drawing.Size(16, 22)
        Me.dtp_RefDate.TabIndex = 301
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(90, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(630, 272)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(73, 29)
        Me.btn_close.TabIndex = 17
        Me.btn_close.TabStop = False
        Me.btn_close.Text = "&CLOSE"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'btn_save
        '
        Me.btn_save.BackColor = System.Drawing.Color.FromArgb(CType(CType(5, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.btn_save.ForeColor = System.Drawing.Color.White
        Me.btn_save.Location = New System.Drawing.Point(538, 272)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(73, 29)
        Me.btn_save.TabIndex = 16
        Me.btn_save.TabStop = False
        Me.btn_save.Text = "&SAVE"
        Me.btn_save.UseVisualStyleBackColor = False
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label26.ForeColor = System.Drawing.Color.Blue
        Me.Label26.Location = New System.Drawing.Point(6, 246)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(35, 15)
        Me.Label26.TabIndex = 126
        Me.Label26.Text = "Note"
        '
        'txt_Note
        '
        Me.txt_Note.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txt_Note.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Note.Location = New System.Drawing.Point(100, 240)
        Me.txt_Note.MaxLength = 500
        Me.txt_Note.Name = "txt_Note"
        Me.txt_Note.Size = New System.Drawing.Size(604, 23)
        Me.txt_Note.TabIndex = 15
        '
        'txt_Amount
        '
        Me.txt_Amount.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Amount.Location = New System.Drawing.Point(255, 126)
        Me.txt_Amount.MaxLength = 12
        Me.txt_Amount.Name = "txt_Amount"
        Me.txt_Amount.Size = New System.Drawing.Size(126, 23)
        Me.txt_Amount.TabIndex = 7
        Me.txt_Amount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_No_of_Beams
        '
        Me.txt_No_of_Beams.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_No_of_Beams.Location = New System.Drawing.Point(100, 126)
        Me.txt_No_of_Beams.MaxLength = 12
        Me.txt_No_of_Beams.Name = "txt_No_of_Beams"
        Me.txt_No_of_Beams.Size = New System.Drawing.Size(88, 23)
        Me.txt_No_of_Beams.TabIndex = 6
        Me.txt_No_of_Beams.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lbl_TaxableValue
        '
        Me.lbl_TaxableValue.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lbl_TaxableValue.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_TaxableValue.Location = New System.Drawing.Point(473, 126)
        Me.lbl_TaxableValue.MaxLength = 11
        Me.lbl_TaxableValue.Name = "lbl_TaxableValue"
        Me.lbl_TaxableValue.ReadOnly = True
        Me.lbl_TaxableValue.Size = New System.Drawing.Size(233, 23)
        Me.lbl_TaxableValue.TabIndex = 14
        Me.lbl_TaxableValue.TabStop = False
        Me.lbl_TaxableValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.Blue
        Me.Label9.Location = New System.Drawing.Point(388, 129)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(80, 15)
        Me.Label9.TabIndex = 287
        Me.Label9.Text = "Taxable Value"
        '
        'lbl_NetAmount
        '
        Me.lbl_NetAmount.BackColor = System.Drawing.Color.Gainsboro
        Me.lbl_NetAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_NetAmount.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_NetAmount.Location = New System.Drawing.Point(517, 196)
        Me.lbl_NetAmount.Name = "lbl_NetAmount"
        Me.lbl_NetAmount.Size = New System.Drawing.Size(188, 40)
        Me.lbl_NetAmount.TabIndex = 282
        Me.lbl_NetAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_Description
        '
        Me.txt_Description.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Description.Location = New System.Drawing.Point(100, 50)
        Me.txt_Description.MaxLength = 11
        Me.txt_Description.Name = "txt_Description"
        Me.txt_Description.Size = New System.Drawing.Size(431, 23)
        Me.txt_Description.TabIndex = 3
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label31.ForeColor = System.Drawing.Color.Blue
        Me.Label31.Location = New System.Drawing.Point(395, 200)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(94, 19)
        Me.Label31.TabIndex = 278
        Me.Label31.Text = "Net Amount"
        '
        'Label34
        '
        Me.Label34.AutoSize = True
        Me.Label34.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label34.ForeColor = System.Drawing.Color.Blue
        Me.Label34.Location = New System.Drawing.Point(5, 54)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(69, 15)
        Me.Label34.TabIndex = 276
        Me.Label34.Text = "Description"
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label35.ForeColor = System.Drawing.Color.Blue
        Me.Label35.Location = New System.Drawing.Point(5, 127)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(79, 15)
        Me.Label35.TabIndex = 131
        Me.Label35.Text = "No. of Beams"
        '
        'Label36
        '
        Me.Label36.AutoSize = True
        Me.Label36.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label36.ForeColor = System.Drawing.Color.Blue
        Me.Label36.Location = New System.Drawing.Point(194, 130)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(52, 15)
        Me.Label36.TabIndex = 129
        Me.Label36.Text = "Amount"
        '
        'Label38
        '
        Me.Label38.AutoSize = True
        Me.Label38.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label38.ForeColor = System.Drawing.Color.Blue
        Me.Label38.Location = New System.Drawing.Point(388, 93)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(63, 15)
        Me.Label38.TabIndex = 125
        Me.Label38.Text = "HSN / SAC"
        '
        'msk_RefDate
        '
        Me.msk_RefDate.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.msk_RefDate.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.msk_RefDate.Location = New System.Drawing.Point(249, 12)
        Me.msk_RefDate.Mask = "00-00-0000"
        Me.msk_RefDate.Name = "msk_RefDate"
        Me.msk_RefDate.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.msk_RefDate.Size = New System.Drawing.Size(119, 22)
        Me.msk_RefDate.TabIndex = 1
        '
        'lbl_RefNo
        '
        Me.lbl_RefNo.BackColor = System.Drawing.Color.White
        Me.lbl_RefNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_RefNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_RefNo.Location = New System.Drawing.Point(100, 12)
        Me.lbl_RefNo.Name = "lbl_RefNo"
        Me.lbl_RefNo.Size = New System.Drawing.Size(88, 23)
        Me.lbl_RefNo.TabIndex = 120
        Me.lbl_RefNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label42
        '
        Me.Label42.AutoSize = True
        Me.Label42.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label42.ForeColor = System.Drawing.Color.Blue
        Me.Label42.Location = New System.Drawing.Point(5, 17)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(47, 15)
        Me.Label42.TabIndex = 117
        Me.Label42.Text = "Ref No."
        '
        'Label43
        '
        Me.Label43.AutoSize = True
        Me.Label43.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label43.ForeColor = System.Drawing.Color.Blue
        Me.Label43.Location = New System.Drawing.Point(388, 16)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(72, 15)
        Me.Label43.TabIndex = 116
        Me.Label43.Text = "Party Name"
        '
        'Label44
        '
        Me.Label44.AutoSize = True
        Me.Label44.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label44.ForeColor = System.Drawing.Color.Blue
        Me.Label44.Location = New System.Drawing.Point(199, 14)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(33, 15)
        Me.Label44.TabIndex = 119
        Me.Label44.Text = "Date"
        '
        'lbl_grid_GstPerc
        '
        Me.lbl_grid_GstPerc.AutoSize = True
        Me.lbl_grid_GstPerc.BackColor = System.Drawing.Color.Red
        Me.lbl_grid_GstPerc.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_grid_GstPerc.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lbl_grid_GstPerc.Location = New System.Drawing.Point(208, 379)
        Me.lbl_grid_GstPerc.Name = "lbl_grid_GstPerc"
        Me.lbl_grid_GstPerc.Size = New System.Drawing.Size(95, 15)
        Me.lbl_grid_GstPerc.TabIndex = 324
        Me.lbl_grid_GstPerc.Text = "lbl_grid_GstPerc"
        Me.lbl_grid_GstPerc.Visible = False
        '
        'lbl_RoundOff
        '
        Me.lbl_RoundOff.BackColor = System.Drawing.Color.Red
        Me.lbl_RoundOff.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_RoundOff.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_RoundOff.Location = New System.Drawing.Point(406, 375)
        Me.lbl_RoundOff.Name = "lbl_RoundOff"
        Me.lbl_RoundOff.Size = New System.Drawing.Size(82, 23)
        Me.lbl_RoundOff.TabIndex = 20
        Me.lbl_RoundOff.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lbl_RoundOff.Visible = False
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.BackColor = System.Drawing.Color.Red
        Me.Label32.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label32.ForeColor = System.Drawing.Color.Blue
        Me.Label32.Location = New System.Drawing.Point(330, 383)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(62, 15)
        Me.Label32.TabIndex = 275
        Me.Label32.Text = "Round Off"
        Me.Label32.Visible = False
        '
        'pnl_Filter
        '
        Me.pnl_Filter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Filter.Controls.Add(Me.btn_Fliter_Close)
        Me.pnl_Filter.Controls.Add(Me.Label10)
        Me.pnl_Filter.Controls.Add(Me.cbo_Filter_HSNCode)
        Me.pnl_Filter.Controls.Add(Me.Label2)
        Me.pnl_Filter.Controls.Add(Me.btn_Filter_Show)
        Me.pnl_Filter.Controls.Add(Me.dgv_filter)
        Me.pnl_Filter.Controls.Add(Me.cbo_Filter_PartyName)
        Me.pnl_Filter.Controls.Add(Me.dtp_FilterTo_date)
        Me.pnl_Filter.Controls.Add(Me.dtp_FilterFrom_date)
        Me.pnl_Filter.Controls.Add(Me.Label4)
        Me.pnl_Filter.Controls.Add(Me.Label5)
        Me.pnl_Filter.Controls.Add(Me.Label6)
        Me.pnl_Filter.Location = New System.Drawing.Point(42, 416)
        Me.pnl_Filter.Name = "pnl_Filter"
        Me.pnl_Filter.Size = New System.Drawing.Size(669, 317)
        Me.pnl_Filter.TabIndex = 40
        Me.pnl_Filter.Visible = False
        '
        'btn_Fliter_Close
        '
        Me.btn_Fliter_Close.BackColor = System.Drawing.Color.White
        Me.btn_Fliter_Close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn_Fliter_Close.FlatAppearance.BorderSize = 0
        Me.btn_Fliter_Close.Image = Global.Textile.My.Resources.Resources.Delete2
        Me.btn_Fliter_Close.Location = New System.Drawing.Point(641, -1)
        Me.btn_Fliter_Close.Name = "btn_Fliter_Close"
        Me.btn_Fliter_Close.Size = New System.Drawing.Size(26, 27)
        Me.btn_Fliter_Close.TabIndex = 41
        Me.btn_Fliter_Close.TabStop = False
        Me.btn_Fliter_Close.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.ForeColor = System.Drawing.Color.Blue
        Me.Label10.Location = New System.Drawing.Point(21, 86)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(37, 13)
        Me.Label10.TabIndex = 45
        Me.Label10.Text = "Colour"
        '
        'cbo_Filter_HSNCode
        '
        Me.cbo_Filter_HSNCode.FormattingEnabled = True
        Me.cbo_Filter_HSNCode.Location = New System.Drawing.Point(69, 83)
        Me.cbo_Filter_HSNCode.Name = "cbo_Filter_HSNCode"
        Me.cbo_Filter_HSNCode.Size = New System.Drawing.Size(230, 21)
        Me.cbo_Filter_HSNCode.TabIndex = 44
        Me.cbo_Filter_HSNCode.Text = "cbo_Filter_Colour"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label2.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(0, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(667, 29)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "FILTER"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btn_Filter_Show
        '
        Me.btn_Filter_Show.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_Filter_Show.ForeColor = System.Drawing.Color.White
        Me.btn_Filter_Show.Location = New System.Drawing.Point(416, 75)
        Me.btn_Filter_Show.Name = "btn_Filter_Show"
        Me.btn_Filter_Show.Size = New System.Drawing.Size(148, 24)
        Me.btn_Filter_Show.TabIndex = 5
        Me.btn_Filter_Show.Text = "SHOW"
        Me.btn_Filter_Show.UseVisualStyleBackColor = False
        '
        'dgv_filter
        '
        Me.dgv_filter.AllowUserToAddRows = False
        Me.dgv_filter.AllowUserToDeleteRows = False
        Me.dgv_filter.AllowUserToResizeColumns = False
        Me.dgv_filter.AllowUserToResizeRows = False
        Me.dgv_filter.BackgroundColor = System.Drawing.Color.White
        Me.dgv_filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_filter.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn2, Me.DataGridViewTextBoxColumn3, Me.Column5, Me.Column6, Me.Column7, Me.Column8, Me.Column9, Me.Column11})
        Me.dgv_filter.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv_filter.Location = New System.Drawing.Point(3, 114)
        Me.dgv_filter.Name = "dgv_filter"
        Me.dgv_filter.RowHeadersVisible = False
        Me.dgv_filter.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_filter.Size = New System.Drawing.Size(661, 178)
        Me.dgv_filter.TabIndex = 5
        '
        'DataGridViewTextBoxColumn1
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGridViewTextBoxColumn1.DefaultCellStyle = DataGridViewCellStyle4
        Me.DataGridViewTextBoxColumn1.FillWeight = 63.55932!
        Me.DataGridViewTextBoxColumn1.HeaderText = "Ref.No"
        Me.DataGridViewTextBoxColumn1.MaxInputLength = 8
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Width = 45
        '
        'DataGridViewTextBoxColumn2
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGridViewTextBoxColumn2.DefaultCellStyle = DataGridViewCellStyle5
        Me.DataGridViewTextBoxColumn2.FillWeight = 95.27658!
        Me.DataGridViewTextBoxColumn2.HeaderText = "Date"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.Width = 67
        '
        'DataGridViewTextBoxColumn3
        '
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGridViewTextBoxColumn3.DefaultCellStyle = DataGridViewCellStyle6
        Me.DataGridViewTextBoxColumn3.FillWeight = 265.3847!
        Me.DataGridViewTextBoxColumn3.HeaderText = "Party Name"
        Me.DataGridViewTextBoxColumn3.MaxInputLength = 35
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.Width = 120
        '
        'Column5
        '
        Me.Column5.FillWeight = 111.8394!
        Me.Column5.HeaderText = "Item"
        Me.Column5.Name = "Column5"
        Me.Column5.Width = 80
        '
        'Column6
        '
        Me.Column6.FillWeight = 98.22652!
        Me.Column6.HeaderText = "HSN CODE"
        Me.Column6.Name = "Column6"
        Me.Column6.Width = 69
        '
        'Column7
        '
        Me.Column7.FillWeight = 67.77271!
        Me.Column7.HeaderText = "NO OF BEAMS"
        Me.Column7.Name = "Column7"
        Me.Column7.Width = 58
        '
        'Column8
        '
        Me.Column8.FillWeight = 63.98219!
        Me.Column8.HeaderText = "AMOUNT"
        Me.Column8.Name = "Column8"
        Me.Column8.Width = 80
        '
        'Column9
        '
        Me.Column9.FillWeight = 66.62056!
        Me.Column9.HeaderText = "GST"
        Me.Column9.Name = "Column9"
        Me.Column9.Width = 48
        '
        'Column11
        '
        Me.Column11.FillWeight = 84.45099!
        Me.Column11.HeaderText = "Net Amount"
        Me.Column11.Name = "Column11"
        Me.Column11.Width = 80
        '
        'cbo_Filter_PartyName
        '
        Me.cbo_Filter_PartyName.FormattingEnabled = True
        Me.cbo_Filter_PartyName.Location = New System.Drawing.Point(395, 45)
        Me.cbo_Filter_PartyName.Name = "cbo_Filter_PartyName"
        Me.cbo_Filter_PartyName.Size = New System.Drawing.Size(258, 21)
        Me.cbo_Filter_PartyName.TabIndex = 3
        Me.cbo_Filter_PartyName.Text = "cbo_Filter_PartyName"
        '
        'dtp_FilterTo_date
        '
        Me.dtp_FilterTo_date.CustomFormat = "dd-MM-yyyy"
        Me.dtp_FilterTo_date.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtp_FilterTo_date.Location = New System.Drawing.Point(198, 45)
        Me.dtp_FilterTo_date.Name = "dtp_FilterTo_date"
        Me.dtp_FilterTo_date.Size = New System.Drawing.Size(101, 20)
        Me.dtp_FilterTo_date.TabIndex = 1
        '
        'dtp_FilterFrom_date
        '
        Me.dtp_FilterFrom_date.CustomFormat = "dd-MM-yyyy"
        Me.dtp_FilterFrom_date.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtp_FilterFrom_date.Location = New System.Drawing.Point(69, 45)
        Me.dtp_FilterFrom_date.Name = "dtp_FilterFrom_date"
        Me.dtp_FilterFrom_date.Size = New System.Drawing.Size(101, 20)
        Me.dtp_FilterFrom_date.TabIndex = 0
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(320, 49)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(62, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Party Name"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.Blue
        Me.Label5.Location = New System.Drawing.Point(173, 49)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(20, 13)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "To"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.ForeColor = System.Drawing.Color.Blue
        Me.Label6.Location = New System.Drawing.Point(19, 49)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(50, 13)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Ref.Date"
        '
        'lbl_Company
        '
        Me.lbl_Company.AutoSize = True
        Me.lbl_Company.BackColor = System.Drawing.Color.Lime
        Me.lbl_Company.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Company.Location = New System.Drawing.Point(10, 9)
        Me.lbl_Company.Name = "lbl_Company"
        Me.lbl_Company.Size = New System.Drawing.Size(77, 15)
        Me.lbl_Company.TabIndex = 38
        Me.lbl_Company.Text = "lbl_Company"
        Me.lbl_Company.Visible = False
        '
        'txt_BillNo
        '
        Me.txt_BillNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_BillNo.Location = New System.Drawing.Point(623, 50)
        Me.txt_BillNo.MaxLength = 30
        Me.txt_BillNo.Name = "txt_BillNo"
        Me.txt_BillNo.Size = New System.Drawing.Size(84, 23)
        Me.txt_BillNo.TabIndex = 1179
        Me.txt_BillNo.Text = "txt_BillNo"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.Blue
        Me.Label14.Location = New System.Drawing.Point(557, 55)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(42, 15)
        Me.Label14.TabIndex = 1180
        Me.Label14.Text = "Bill No"
        '
        'Knotting_Bill_Entry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(750, 359)
        Me.Controls.Add(Me.pnl_Filter)
        Me.Controls.Add(Me.lbl_Company)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.pnl_Back)
        Me.Controls.Add(Me.Label32)
        Me.Controls.Add(Me.lbl_RoundOff)
        Me.Controls.Add(Me.lbl_grid_GstPerc)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.Name = "Knotting_Bill_Entry"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = " "
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_Back.PerformLayout()
        Me.pnl_Filter.ResumeLayout(False)
        Me.pnl_Filter.PerformLayout()
        CType(Me.dgv_filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents txt_Amount As System.Windows.Forms.TextBox
    Friend WithEvents txt_No_of_Beams As System.Windows.Forms.TextBox
    Friend WithEvents lbl_TaxableValue As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents lbl_RoundOff As System.Windows.Forms.Label
    Friend WithEvents lbl_NetAmount As System.Windows.Forms.Label
    Friend WithEvents txt_Description As System.Windows.Forms.TextBox
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents msk_RefDate As System.Windows.Forms.MaskedTextBox
    Friend WithEvents lbl_RefNo As System.Windows.Forms.Label
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents Label43 As System.Windows.Forms.Label
    Friend WithEvents Label44 As System.Windows.Forms.Label
    Friend WithEvents lbl_Company As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents btn_close As System.Windows.Forms.Button
    Friend WithEvents btn_save As System.Windows.Forms.Button
    Friend WithEvents txt_Note As System.Windows.Forms.TextBox
    Friend WithEvents dtp_RefDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents pnl_Filter As System.Windows.Forms.Panel
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents cbo_Filter_HSNCode As System.Windows.Forms.ComboBox
    Friend WithEvents btn_Fliter_Close As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btn_Filter_Show As System.Windows.Forms.Button
    Friend WithEvents dgv_filter As System.Windows.Forms.DataGridView
    Friend WithEvents cbo_Filter_PartyName As System.Windows.Forms.ComboBox
    Friend WithEvents dtp_FilterTo_date As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtp_FilterFrom_date As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cbo_Ledger As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_HSNCode As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_CGST_Amount As System.Windows.Forms.Label
    Friend WithEvents Label59 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label61 As System.Windows.Forms.Label
    Friend WithEvents lbl_SGST_Amount As System.Windows.Forms.Label
    Friend WithEvents lbl_IGST_Amount As System.Windows.Forms.Label
    Friend WithEvents lbl_grid_GstPerc As System.Windows.Forms.Label
    Friend WithEvents cbo_KnottingAc As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents txt_CGSTPerc As System.Windows.Forms.TextBox
    Friend WithEvents txt_IGSTPerc As System.Windows.Forms.TextBox
    Friend WithEvents txt_SGSTPerc As System.Windows.Forms.TextBox
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column7 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column8 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column9 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column11 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents btn_UserModification As System.Windows.Forms.Button
    Friend WithEvents txt_BillNo As TextBox
    Friend WithEvents Label14 As Label
End Class
