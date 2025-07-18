<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Amount_Receipt_entry
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
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.pnl_back = New System.Windows.Forms.Panel()
        Me.pnl_CurrentBalance = New System.Windows.Forms.Panel()
        Me.lbl_CurrentBalance = New System.Windows.Forms.Label()
        Me.dtp_voucher_date = New System.Windows.Forms.DateTimePicker()
        Me.Msk_Date_Voucher = New System.Windows.Forms.MaskedTextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txt_Remarks = New System.Windows.Forms.TextBox()
        Me.msk_Date = New System.Windows.Forms.MaskedTextBox()
        Me.dtp_cheque_date = New System.Windows.Forms.DateTimePicker()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cbo_chequeBank = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lbl_RecNo = New System.Windows.Forms.Label()
        Me.txt_Amount = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.cbo_PaymentMode = New System.Windows.Forms.ComboBox()
        Me.cbo_DebitorAccount = New System.Windows.Forms.ComboBox()
        Me.btn_close = New System.Windows.Forms.Button()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txt_ReferenceNo = New System.Windows.Forms.TextBox()
        Me.cbo_PartyName = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btn_Print = New System.Windows.Forms.Button()
        Me.btn_SaveAll = New System.Windows.Forms.Button()
        Me.lbl_VouNo = New System.Windows.Forms.Label()
        Me.lbl_Company = New System.Windows.Forms.Label()
        Me.pnl_filter = New System.Windows.Forms.Panel()
        Me.cbo_EmployeeFilter = New System.Windows.Forms.ComboBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.btn_closefilter = New System.Windows.Forms.Button()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.btn_filtershow = New System.Windows.Forms.Button()
        Me.dgv_filter = New System.Windows.Forms.DataGridView()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dtp_FilterTo_date = New System.Windows.Forms.DateTimePicker()
        Me.dtp_FilterFrom_date = New System.Windows.Forms.DateTimePicker()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.lbl_Heading = New System.Windows.Forms.Label()
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog()
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.btn_save = New System.Windows.Forms.Button()
        Me.pnl_back.SuspendLayout()
        Me.pnl_CurrentBalance.SuspendLayout()
        Me.pnl_filter.SuspendLayout()
        CType(Me.dgv_filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnl_back
        '
        Me.pnl_back.AutoSize = True
        Me.pnl_back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_back.Controls.Add(Me.pnl_CurrentBalance)
        Me.pnl_back.Controls.Add(Me.dtp_voucher_date)
        Me.pnl_back.Controls.Add(Me.Msk_Date_Voucher)
        Me.pnl_back.Controls.Add(Me.Label4)
        Me.pnl_back.Controls.Add(Me.txt_Remarks)
        Me.pnl_back.Controls.Add(Me.msk_Date)
        Me.pnl_back.Controls.Add(Me.dtp_cheque_date)
        Me.pnl_back.Controls.Add(Me.Label10)
        Me.pnl_back.Controls.Add(Me.cbo_chequeBank)
        Me.pnl_back.Controls.Add(Me.Label9)
        Me.pnl_back.Controls.Add(Me.Label6)
        Me.pnl_back.Controls.Add(Me.lbl_RecNo)
        Me.pnl_back.Controls.Add(Me.txt_Amount)
        Me.pnl_back.Controls.Add(Me.Label16)
        Me.pnl_back.Controls.Add(Me.cbo_PaymentMode)
        Me.pnl_back.Controls.Add(Me.cbo_DebitorAccount)
        Me.pnl_back.Controls.Add(Me.btn_close)
        Me.pnl_back.Controls.Add(Me.Label12)
        Me.pnl_back.Controls.Add(Me.btn_save)
        Me.pnl_back.Controls.Add(Me.Label8)
        Me.pnl_back.Controls.Add(Me.txt_ReferenceNo)
        Me.pnl_back.Controls.Add(Me.cbo_PartyName)
        Me.pnl_back.Controls.Add(Me.Label1)
        Me.pnl_back.Controls.Add(Me.Label7)
        Me.pnl_back.Controls.Add(Me.Label2)
        Me.pnl_back.Controls.Add(Me.Label3)
        Me.pnl_back.Controls.Add(Me.Label5)
        Me.pnl_back.Location = New System.Drawing.Point(8, 44)
        Me.pnl_back.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.pnl_back.Name = "pnl_back"
        Me.pnl_back.Size = New System.Drawing.Size(698, 317)
        Me.pnl_back.TabIndex = 28
        '
        'pnl_CurrentBalance
        '
        Me.pnl_CurrentBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_CurrentBalance.Controls.Add(Me.lbl_CurrentBalance)
        Me.pnl_CurrentBalance.Location = New System.Drawing.Point(19, 269)
        Me.pnl_CurrentBalance.Name = "pnl_CurrentBalance"
        Me.pnl_CurrentBalance.Size = New System.Drawing.Size(265, 30)
        Me.pnl_CurrentBalance.TabIndex = 49
        Me.pnl_CurrentBalance.Visible = False
        '
        'lbl_CurrentBalance
        '
        Me.lbl_CurrentBalance.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lbl_CurrentBalance.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lbl_CurrentBalance.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_CurrentBalance.ForeColor = System.Drawing.Color.Red
        Me.lbl_CurrentBalance.Location = New System.Drawing.Point(0, 0)
        Me.lbl_CurrentBalance.Name = "lbl_CurrentBalance"
        Me.lbl_CurrentBalance.Size = New System.Drawing.Size(263, 28)
        Me.lbl_CurrentBalance.TabIndex = 0
        Me.lbl_CurrentBalance.Text = "Current Balance  :  1,23,45,67,989.00"
        Me.lbl_CurrentBalance.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dtp_voucher_date
        '
        Me.dtp_voucher_date.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_voucher_date.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_voucher_date.Location = New System.Drawing.Point(644, 19)
        Me.dtp_voucher_date.Name = "dtp_voucher_date"
        Me.dtp_voucher_date.Size = New System.Drawing.Size(21, 22)
        Me.dtp_voucher_date.TabIndex = 43
        '
        'Msk_Date_Voucher
        '
        Me.Msk_Date_Voucher.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Msk_Date_Voucher.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.Msk_Date_Voucher.Location = New System.Drawing.Point(477, 19)
        Me.Msk_Date_Voucher.Mask = "00-00-0000"
        Me.Msk_Date_Voucher.Name = "Msk_Date_Voucher"
        Me.Msk_Date_Voucher.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.Msk_Date_Voucher.Size = New System.Drawing.Size(168, 22)
        Me.Msk_Date_Voucher.TabIndex = 1
        Me.Msk_Date_Voucher.ValidatingType = GetType(Date)
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(16, 221)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(54, 15)
        Me.Label4.TabIndex = 41
        Me.Label4.Text = "Remarks"
        '
        'txt_Remarks
        '
        Me.txt_Remarks.Location = New System.Drawing.Point(133, 213)
        Me.txt_Remarks.MaxLength = 500
        Me.txt_Remarks.Name = "txt_Remarks"
        Me.txt_Remarks.Size = New System.Drawing.Size(533, 23)
        Me.txt_Remarks.TabIndex = 9
        '
        'msk_Date
        '
        Me.msk_Date.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.msk_Date.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.msk_Date.Location = New System.Drawing.Point(477, 172)
        Me.msk_Date.Mask = "00-00-0000"
        Me.msk_Date.Name = "msk_Date"
        Me.msk_Date.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.msk_Date.Size = New System.Drawing.Size(168, 22)
        Me.msk_Date.TabIndex = 8
        Me.msk_Date.ValidatingType = GetType(Date)
        '
        'dtp_cheque_date
        '
        Me.dtp_cheque_date.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_cheque_date.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_cheque_date.Location = New System.Drawing.Point(644, 172)
        Me.dtp_cheque_date.Name = "dtp_cheque_date"
        Me.dtp_cheque_date.Size = New System.Drawing.Size(21, 22)
        Me.dtp_cheque_date.TabIndex = 39
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.ForeColor = System.Drawing.Color.Blue
        Me.Label10.Location = New System.Drawing.Point(362, 176)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(78, 15)
        Me.Label10.TabIndex = 38
        Me.Label10.Text = "Cheque Date"
        '
        'cbo_chequeBank
        '
        Me.cbo_chequeBank.DropDownHeight = 150
        Me.cbo_chequeBank.FormattingEnabled = True
        Me.cbo_chequeBank.IntegralHeight = False
        Me.cbo_chequeBank.Location = New System.Drawing.Point(133, 172)
        Me.cbo_chequeBank.MaxLength = 35
        Me.cbo_chequeBank.Name = "cbo_chequeBank"
        Me.cbo_chequeBank.Size = New System.Drawing.Size(189, 23)
        Me.cbo_chequeBank.TabIndex = 7
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.ForeColor = System.Drawing.Color.Blue
        Me.Label9.Location = New System.Drawing.Point(16, 176)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(114, 15)
        Me.Label9.TabIndex = 36
        Me.Label9.Text = "Cheque Bank Name"
        '
        'Label6
        '
        Me.Label6.ForeColor = System.Drawing.Color.Blue
        Me.Label6.Location = New System.Drawing.Point(362, 134)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(93, 34)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Reference No / Cheque No."
        '
        'lbl_RecNo
        '
        Me.lbl_RecNo.BackColor = System.Drawing.Color.Gainsboro
        Me.lbl_RecNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_RecNo.Location = New System.Drawing.Point(133, 18)
        Me.lbl_RecNo.Name = "lbl_RecNo"
        Me.lbl_RecNo.Size = New System.Drawing.Size(189, 23)
        Me.lbl_RecNo.TabIndex = 0
        Me.lbl_RecNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txt_Amount
        '
        Me.txt_Amount.Location = New System.Drawing.Point(133, 95)
        Me.txt_Amount.MaxLength = 20
        Me.txt_Amount.Name = "txt_Amount"
        Me.txt_Amount.Size = New System.Drawing.Size(189, 23)
        Me.txt_Amount.TabIndex = 3
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.ForeColor = System.Drawing.Color.Blue
        Me.Label16.Location = New System.Drawing.Point(16, 95)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(52, 15)
        Me.Label16.TabIndex = 31
        Me.Label16.Text = "Amount"
        '
        'cbo_PaymentMode
        '
        Me.cbo_PaymentMode.FormattingEnabled = True
        Me.cbo_PaymentMode.Items.AddRange(New Object() {" ", "CASH", "CHEQUE"})
        Me.cbo_PaymentMode.Location = New System.Drawing.Point(477, 91)
        Me.cbo_PaymentMode.MaxLength = 35
        Me.cbo_PaymentMode.Name = "cbo_PaymentMode"
        Me.cbo_PaymentMode.Size = New System.Drawing.Size(189, 23)
        Me.cbo_PaymentMode.TabIndex = 4
        '
        'cbo_DebitorAccount
        '
        Me.cbo_DebitorAccount.DropDownHeight = 150
        Me.cbo_DebitorAccount.FormattingEnabled = True
        Me.cbo_DebitorAccount.IntegralHeight = False
        Me.cbo_DebitorAccount.Location = New System.Drawing.Point(133, 130)
        Me.cbo_DebitorAccount.MaxLength = 35
        Me.cbo_DebitorAccount.Name = "cbo_DebitorAccount"
        Me.cbo_DebitorAccount.Size = New System.Drawing.Size(189, 23)
        Me.cbo_DebitorAccount.TabIndex = 5
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(581, 266)
        Me.btn_close.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(85, 33)
        Me.btn_close.TabIndex = 11
        Me.btn_close.TabStop = False
        Me.btn_close.Text = "&CLOSE"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.ForeColor = System.Drawing.Color.Blue
        Me.Label12.Location = New System.Drawing.Point(360, 95)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(105, 15)
        Me.Label12.TabIndex = 22
        Me.Label12.Text = "Mode of Payment"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ForeColor = System.Drawing.Color.Blue
        Me.Label8.Location = New System.Drawing.Point(16, 59)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(72, 15)
        Me.Label8.TabIndex = 14
        Me.Label8.Text = "Party Name"
        '
        'txt_ReferenceNo
        '
        Me.txt_ReferenceNo.Location = New System.Drawing.Point(477, 130)
        Me.txt_ReferenceNo.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txt_ReferenceNo.MaxLength = 20
        Me.txt_ReferenceNo.Name = "txt_ReferenceNo"
        Me.txt_ReferenceNo.Size = New System.Drawing.Size(189, 23)
        Me.txt_ReferenceNo.TabIndex = 6
        '
        'cbo_PartyName
        '
        Me.cbo_PartyName.DropDownHeight = 160
        Me.cbo_PartyName.FormattingEnabled = True
        Me.cbo_PartyName.IntegralHeight = False
        Me.cbo_PartyName.Location = New System.Drawing.Point(133, 55)
        Me.cbo_PartyName.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.cbo_PartyName.MaxLength = 35
        Me.cbo_PartyName.Name = "cbo_PartyName"
        Me.cbo_PartyName.Size = New System.Drawing.Size(533, 23)
        Me.cbo_PartyName.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(16, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(91, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Receipt Vou.No"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(9, 300)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(0, 15)
        Me.Label7.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(362, 22)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(81, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Voucher Date"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 133)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(0, 15)
        Me.Label3.TabIndex = 2
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.Blue
        Me.Label5.Location = New System.Drawing.Point(16, 134)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(79, 15)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Our Bank A/c"
        '
        'btn_Print
        '
        Me.btn_Print.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_Print.ForeColor = System.Drawing.Color.White
        Me.btn_Print.Location = New System.Drawing.Point(485, 384)
        Me.btn_Print.Name = "btn_Print"
        Me.btn_Print.Size = New System.Drawing.Size(85, 33)
        Me.btn_Print.TabIndex = 9
        Me.btn_Print.TabStop = False
        Me.btn_Print.Text = "&PRINT"
        Me.btn_Print.UseVisualStyleBackColor = False
        Me.btn_Print.Visible = False
        '
        'btn_SaveAll
        '
        Me.btn_SaveAll.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.btn_SaveAll.ForeColor = System.Drawing.Color.White
        Me.btn_SaveAll.Location = New System.Drawing.Point(28, 378)
        Me.btn_SaveAll.Name = "btn_SaveAll"
        Me.btn_SaveAll.Size = New System.Drawing.Size(85, 30)
        Me.btn_SaveAll.TabIndex = 11
        Me.btn_SaveAll.TabStop = False
        Me.btn_SaveAll.Text = "&SAVE ALL"
        Me.btn_SaveAll.UseVisualStyleBackColor = False
        Me.btn_SaveAll.Visible = False
        '
        'lbl_VouNo
        '
        Me.lbl_VouNo.BackColor = System.Drawing.Color.Gainsboro
        Me.lbl_VouNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_VouNo.Location = New System.Drawing.Point(185, 383)
        Me.lbl_VouNo.Name = "lbl_VouNo"
        Me.lbl_VouNo.Size = New System.Drawing.Size(102, 23)
        Me.lbl_VouNo.TabIndex = 21
        Me.lbl_VouNo.Text = "lbl_VouNo"
        Me.lbl_VouNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lbl_VouNo.Visible = False
        '
        'lbl_Company
        '
        Me.lbl_Company.AutoSize = True
        Me.lbl_Company.BackColor = System.Drawing.Color.Red
        Me.lbl_Company.Location = New System.Drawing.Point(45, 12)
        Me.lbl_Company.Name = "lbl_Company"
        Me.lbl_Company.Size = New System.Drawing.Size(77, 15)
        Me.lbl_Company.TabIndex = 19
        Me.lbl_Company.Text = "lbl_Company"
        Me.lbl_Company.Visible = False
        '
        'pnl_filter
        '
        Me.pnl_filter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_filter.Controls.Add(Me.cbo_EmployeeFilter)
        Me.pnl_filter.Controls.Add(Me.Label20)
        Me.pnl_filter.Controls.Add(Me.btn_closefilter)
        Me.pnl_filter.Controls.Add(Me.Label17)
        Me.pnl_filter.Controls.Add(Me.btn_filtershow)
        Me.pnl_filter.Controls.Add(Me.dgv_filter)
        Me.pnl_filter.Controls.Add(Me.dtp_FilterTo_date)
        Me.pnl_filter.Controls.Add(Me.dtp_FilterFrom_date)
        Me.pnl_filter.Controls.Add(Me.Label18)
        Me.pnl_filter.Controls.Add(Me.Label19)
        Me.pnl_filter.Location = New System.Drawing.Point(743, 213)
        Me.pnl_filter.Name = "pnl_filter"
        Me.pnl_filter.Size = New System.Drawing.Size(599, 304)
        Me.pnl_filter.TabIndex = 30
        '
        'cbo_EmployeeFilter
        '
        Me.cbo_EmployeeFilter.DropDownHeight = 90
        Me.cbo_EmployeeFilter.DropDownWidth = 250
        Me.cbo_EmployeeFilter.FormattingEnabled = True
        Me.cbo_EmployeeFilter.IntegralHeight = False
        Me.cbo_EmployeeFilter.Location = New System.Drawing.Point(100, 86)
        Me.cbo_EmployeeFilter.MaxLength = 35
        Me.cbo_EmployeeFilter.Name = "cbo_EmployeeFilter"
        Me.cbo_EmployeeFilter.Size = New System.Drawing.Size(294, 23)
        Me.cbo_EmployeeFilter.TabIndex = 2
        Me.cbo_EmployeeFilter.Text = "cbo_MemberFilter"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(12, 90)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(72, 15)
        Me.Label20.TabIndex = 9
        Me.Label20.Text = "Party Name"
        '
        'btn_closefilter
        '
        Me.btn_closefilter.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btn_closefilter.ForeColor = System.Drawing.Color.White
        Me.btn_closefilter.Location = New System.Drawing.Point(509, 44)
        Me.btn_closefilter.Name = "btn_closefilter"
        Me.btn_closefilter.Size = New System.Drawing.Size(75, 65)
        Me.btn_closefilter.TabIndex = 4
        Me.btn_closefilter.Text = "&CLOSE"
        Me.btn_closefilter.UseVisualStyleBackColor = False
        '
        'Label17
        '
        Me.Label17.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.Label17.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.ForeColor = System.Drawing.Color.White
        Me.Label17.Location = New System.Drawing.Point(-1, 0)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(599, 30)
        Me.Label17.TabIndex = 8
        Me.Label17.Text = "FILTER"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btn_filtershow
        '
        Me.btn_filtershow.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btn_filtershow.ForeColor = System.Drawing.Color.White
        Me.btn_filtershow.Location = New System.Drawing.Point(422, 44)
        Me.btn_filtershow.Name = "btn_filtershow"
        Me.btn_filtershow.Size = New System.Drawing.Size(75, 65)
        Me.btn_filtershow.TabIndex = 3
        Me.btn_filtershow.Text = "SHOW"
        Me.btn_filtershow.UseVisualStyleBackColor = False
        '
        'dgv_filter
        '
        Me.dgv_filter.AllowUserToAddRows = False
        Me.dgv_filter.AllowUserToDeleteRows = False
        Me.dgv_filter.BackgroundColor = System.Drawing.Color.White
        Me.dgv_filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_filter.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column3, Me.Column1, Me.Column2, Me.Column5})
        Me.dgv_filter.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv_filter.Location = New System.Drawing.Point(11, 123)
        Me.dgv_filter.Name = "dgv_filter"
        Me.dgv_filter.RowHeadersVisible = False
        Me.dgv_filter.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_filter.Size = New System.Drawing.Size(573, 161)
        Me.dgv_filter.TabIndex = 5
        '
        'Column3
        '
        Me.Column3.HeaderText = "VOU.NO"
        Me.Column3.Name = "Column3"
        Me.Column3.Width = 80
        '
        'Column1
        '
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column1.DefaultCellStyle = DataGridViewCellStyle4
        Me.Column1.HeaderText = "DATE"
        Me.Column1.Name = "Column1"
        Me.Column1.Width = 80
        '
        'Column2
        '
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle5
        Me.Column2.HeaderText = "PARTY NAME"
        Me.Column2.MaxInputLength = 35
        Me.Column2.Name = "Column2"
        Me.Column2.Width = 200
        '
        'Column5
        '
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column5.DefaultCellStyle = DataGridViewCellStyle6
        Me.Column5.HeaderText = "AMOUNT"
        Me.Column5.Name = "Column5"
        '
        'dtp_FilterTo_date
        '
        Me.dtp_FilterTo_date.CustomFormat = "dd-MM-yyyy"
        Me.dtp_FilterTo_date.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtp_FilterTo_date.Location = New System.Drawing.Point(292, 44)
        Me.dtp_FilterTo_date.Name = "dtp_FilterTo_date"
        Me.dtp_FilterTo_date.Size = New System.Drawing.Size(102, 23)
        Me.dtp_FilterTo_date.TabIndex = 1
        '
        'dtp_FilterFrom_date
        '
        Me.dtp_FilterFrom_date.CustomFormat = "dd-MM-yyyy"
        Me.dtp_FilterFrom_date.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtp_FilterFrom_date.Location = New System.Drawing.Point(100, 44)
        Me.dtp_FilterFrom_date.Name = "dtp_FilterFrom_date"
        Me.dtp_FilterFrom_date.Size = New System.Drawing.Size(102, 23)
        Me.dtp_FilterFrom_date.TabIndex = 0
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(232, 48)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(19, 15)
        Me.Label18.TabIndex = 1
        Me.Label18.Text = "To"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(12, 48)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(33, 15)
        Me.Label19.TabIndex = 0
        Me.Label19.Text = "Date"
        '
        'lbl_Heading
        '
        Me.lbl_Heading.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.lbl_Heading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_Heading.Dock = System.Windows.Forms.DockStyle.Top
        Me.lbl_Heading.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Heading.ForeColor = System.Drawing.Color.White
        Me.lbl_Heading.Location = New System.Drawing.Point(0, 0)
        Me.lbl_Heading.Name = "lbl_Heading"
        Me.lbl_Heading.Size = New System.Drawing.Size(734, 35)
        Me.lbl_Heading.TabIndex = 29
        Me.lbl_Heading.Text = "VOUCHER RECEIPT"
        Me.lbl_Heading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PrintDialog1
        '
        Me.PrintDialog1.UseEXDialog = True
        '
        'PrintDocument1
        '
        '
        'Timer1
        '
        '
        'btn_save
        '
        Me.btn_save.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btn_save.ForeColor = System.Drawing.Color.White
        Me.btn_save.Location = New System.Drawing.Point(459, 266)
        Me.btn_save.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(85, 33)
        Me.btn_save.TabIndex = 10
        Me.btn_save.TabStop = False
        Me.btn_save.Text = "&SAVE"
        Me.btn_save.UseVisualStyleBackColor = False
        '
        'Amount_Receipt_entry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(734, 378)
        Me.Controls.Add(Me.lbl_Company)
        Me.Controls.Add(Me.pnl_back)
        Me.Controls.Add(Me.btn_Print)
        Me.Controls.Add(Me.btn_SaveAll)
        Me.Controls.Add(Me.pnl_filter)
        Me.Controls.Add(Me.lbl_Heading)
        Me.Controls.Add(Me.lbl_VouNo)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Amount_Receipt_entry"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "VOUCHER RECEIPT"
        Me.pnl_back.ResumeLayout(False)
        Me.pnl_back.PerformLayout()
        Me.pnl_CurrentBalance.ResumeLayout(False)
        Me.pnl_filter.ResumeLayout(False)
        Me.pnl_filter.PerformLayout()
        CType(Me.dgv_filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnl_back As System.Windows.Forms.Panel
    Friend WithEvents txt_Amount As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents cbo_PaymentMode As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_DebitorAccount As System.Windows.Forms.ComboBox
    Friend WithEvents btn_close As System.Windows.Forms.Button
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents lbl_VouNo As System.Windows.Forms.Label
    Friend WithEvents lbl_Company As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txt_ReferenceNo As System.Windows.Forms.TextBox
    Friend WithEvents cbo_PartyName As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents dgv_filter As System.Windows.Forms.DataGridView
    Friend WithEvents btn_filtershow As System.Windows.Forms.Button
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents dtp_FilterTo_date As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtp_FilterFrom_date As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents btn_closefilter As System.Windows.Forms.Button
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents pnl_filter As System.Windows.Forms.Panel
    Friend WithEvents cbo_EmployeeFilter As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_Heading As System.Windows.Forms.Label
    Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
    Friend WithEvents PrintDocument1 As System.Drawing.Printing.PrintDocument
    Friend WithEvents btn_Print As System.Windows.Forms.Button
    Friend WithEvents btn_SaveAll As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents lbl_RecNo As System.Windows.Forms.Label
    Friend WithEvents cbo_chequeBank As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents msk_Date As System.Windows.Forms.MaskedTextBox
    Friend WithEvents dtp_cheque_date As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txt_Remarks As System.Windows.Forms.TextBox
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dtp_voucher_date As System.Windows.Forms.DateTimePicker
    Friend WithEvents Msk_Date_Voucher As System.Windows.Forms.MaskedTextBox
    Friend WithEvents pnl_CurrentBalance As System.Windows.Forms.Panel
    Friend WithEvents lbl_CurrentBalance As System.Windows.Forms.Label
    Friend WithEvents btn_save As Button
End Class
