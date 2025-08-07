<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Sizing_CashDiscount
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
        Me.pnl_back = New System.Windows.Forms.Panel()
        Me.btn_UserModification = New System.Windows.Forms.Button()
        Me.chk_Printed = New System.Windows.Forms.CheckBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.cbo_DiscountType = New System.Windows.Forms.ComboBox()
        Me.lbl_InvoicedAmt = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.lbl_totaldisc = New System.Windows.Forms.Label()
        Me.dtp_Todate = New System.Windows.Forms.DateTimePicker()
        Me.dtp_fromdate = New System.Windows.Forms.DateTimePicker()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.lbl_discamount = New System.Windows.Forms.Label()
        Me.lbl_invoicekgs = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txt_addless = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.lbl_refno = New System.Windows.Forms.Label()
        Me.lbl_Company = New System.Windows.Forms.Label()
        Me.btn_close = New System.Windows.Forms.Button()
        Me.btn_save = New System.Windows.Forms.Button()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.dtp_date = New System.Windows.Forms.DateTimePicker()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txt_Remarks = New System.Windows.Forms.TextBox()
        Me.txt_amountkg = New System.Windows.Forms.TextBox()
        Me.cbo_partyname = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txt_vatamount = New System.Windows.Forms.Label()
        Me.txt_vatdisamt = New System.Windows.Forms.Label()
        Me.pnl_filter = New System.Windows.Forms.Panel()
        Me.btn_closefilter = New System.Windows.Forms.Button()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.btn_filtershow = New System.Windows.Forms.Button()
        Me.dgv_filter = New System.Windows.Forms.DataGridView()
        Me.dc = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cbo_Filter_PartyName = New System.Windows.Forms.ComboBox()
        Me.dtp_FilterTo_date = New System.Windows.Forms.DateTimePicker()
        Me.dtp_FilterFrom_date = New System.Windows.Forms.DateTimePicker()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.lbl_UserName = New System.Windows.Forms.Label()
        Me.pnl_back.SuspendLayout()
        Me.pnl_filter.SuspendLayout()
        CType(Me.dgv_filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnl_back
        '
        Me.pnl_back.AutoSize = True
        Me.pnl_back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_back.Controls.Add(Me.btn_UserModification)
        Me.pnl_back.Controls.Add(Me.chk_Printed)
        Me.pnl_back.Controls.Add(Me.Label18)
        Me.pnl_back.Controls.Add(Me.Label23)
        Me.pnl_back.Controls.Add(Me.Label21)
        Me.pnl_back.Controls.Add(Me.cbo_DiscountType)
        Me.pnl_back.Controls.Add(Me.lbl_InvoicedAmt)
        Me.pnl_back.Controls.Add(Me.Label20)
        Me.pnl_back.Controls.Add(Me.lbl_totaldisc)
        Me.pnl_back.Controls.Add(Me.dtp_Todate)
        Me.pnl_back.Controls.Add(Me.dtp_fromdate)
        Me.pnl_back.Controls.Add(Me.Label19)
        Me.pnl_back.Controls.Add(Me.Label14)
        Me.pnl_back.Controls.Add(Me.lbl_discamount)
        Me.pnl_back.Controls.Add(Me.lbl_invoicekgs)
        Me.pnl_back.Controls.Add(Me.Label13)
        Me.pnl_back.Controls.Add(Me.txt_addless)
        Me.pnl_back.Controls.Add(Me.Label12)
        Me.pnl_back.Controls.Add(Me.lbl_refno)
        Me.pnl_back.Controls.Add(Me.lbl_Company)
        Me.pnl_back.Controls.Add(Me.btn_close)
        Me.pnl_back.Controls.Add(Me.btn_save)
        Me.pnl_back.Controls.Add(Me.Label10)
        Me.pnl_back.Controls.Add(Me.dtp_date)
        Me.pnl_back.Controls.Add(Me.Label8)
        Me.pnl_back.Controls.Add(Me.txt_Remarks)
        Me.pnl_back.Controls.Add(Me.txt_amountkg)
        Me.pnl_back.Controls.Add(Me.cbo_partyname)
        Me.pnl_back.Controls.Add(Me.Label1)
        Me.pnl_back.Controls.Add(Me.Label7)
        Me.pnl_back.Controls.Add(Me.Label2)
        Me.pnl_back.Controls.Add(Me.Label6)
        Me.pnl_back.Controls.Add(Me.Label3)
        Me.pnl_back.Controls.Add(Me.Label5)
        Me.pnl_back.Controls.Add(Me.Label4)
        Me.pnl_back.Location = New System.Drawing.Point(6, 45)
        Me.pnl_back.Name = "pnl_back"
        Me.pnl_back.Size = New System.Drawing.Size(690, 385)
        Me.pnl_back.TabIndex = 8
        '
        'btn_UserModification
        '
        Me.btn_UserModification.BackColor = System.Drawing.Color.OrangeRed
        Me.btn_UserModification.ForeColor = System.Drawing.Color.White
        Me.btn_UserModification.Location = New System.Drawing.Point(282, 338)
        Me.btn_UserModification.Name = "btn_UserModification"
        Me.btn_UserModification.Size = New System.Drawing.Size(114, 28)
        Me.btn_UserModification.TabIndex = 369
        Me.btn_UserModification.TabStop = False
        Me.btn_UserModification.Text = "MODIFICATION"
        Me.btn_UserModification.UseVisualStyleBackColor = False
        Me.btn_UserModification.Visible = False
        '
        'chk_Printed
        '
        Me.chk_Printed.AutoSize = True
        Me.chk_Printed.BackColor = System.Drawing.Color.LightPink
        Me.chk_Printed.Font = New System.Drawing.Font("Calibri", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chk_Printed.ForeColor = System.Drawing.Color.Red
        Me.chk_Printed.Location = New System.Drawing.Point(133, 351)
        Me.chk_Printed.Name = "chk_Printed"
        Me.chk_Printed.Size = New System.Drawing.Size(72, 19)
        Me.chk_Printed.TabIndex = 304
        Me.chk_Printed.TabStop = False
        Me.chk_Printed.Text = "PRINTED"
        Me.chk_Printed.UseVisualStyleBackColor = False
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.ForeColor = System.Drawing.Color.Red
        Me.Label18.Location = New System.Drawing.Point(366, 30)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(13, 15)
        Me.Label18.TabIndex = 300
        Me.Label18.Text = "*"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.ForeColor = System.Drawing.Color.Red
        Me.Label23.Location = New System.Drawing.Point(82, 70)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(13, 15)
        Me.Label23.TabIndex = 300
        Me.Label23.Text = "*"
        Me.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(14, 186)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(83, 15)
        Me.Label21.TabIndex = 43
        Me.Label21.Text = "Discount Type"
        '
        'cbo_DiscountType
        '
        Me.cbo_DiscountType.FormattingEnabled = True
        Me.cbo_DiscountType.Items.AddRange(New Object() {"PERCENTAGE", "PAISE/KG"})
        Me.cbo_DiscountType.Location = New System.Drawing.Point(133, 182)
        Me.cbo_DiscountType.Name = "cbo_DiscountType"
        Me.cbo_DiscountType.Size = New System.Drawing.Size(180, 23)
        Me.cbo_DiscountType.TabIndex = 4
        '
        'lbl_InvoicedAmt
        '
        Me.lbl_InvoicedAmt.BackColor = System.Drawing.Color.White
        Me.lbl_InvoicedAmt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_InvoicedAmt.Location = New System.Drawing.Point(458, 142)
        Me.lbl_InvoicedAmt.Name = "lbl_InvoicedAmt"
        Me.lbl_InvoicedAmt.Size = New System.Drawing.Size(212, 24)
        Me.lbl_InvoicedAmt.TabIndex = 32
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(338, 146)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(100, 15)
        Me.Label20.TabIndex = 33
        Me.Label20.Text = "Invoiced Amount"
        '
        'lbl_totaldisc
        '
        Me.lbl_totaldisc.BackColor = System.Drawing.Color.White
        Me.lbl_totaldisc.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_totaldisc.Location = New System.Drawing.Point(458, 260)
        Me.lbl_totaldisc.Name = "lbl_totaldisc"
        Me.lbl_totaldisc.Size = New System.Drawing.Size(212, 24)
        Me.lbl_totaldisc.TabIndex = 8
        '
        'dtp_Todate
        '
        Me.dtp_Todate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_Todate.Location = New System.Drawing.Point(458, 103)
        Me.dtp_Todate.Name = "dtp_Todate"
        Me.dtp_Todate.Size = New System.Drawing.Size(212, 23)
        Me.dtp_Todate.TabIndex = 3
        '
        'dtp_fromdate
        '
        Me.dtp_fromdate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_fromdate.Location = New System.Drawing.Point(133, 103)
        Me.dtp_fromdate.Name = "dtp_fromdate"
        Me.dtp_fromdate.Size = New System.Drawing.Size(180, 23)
        Me.dtp_fromdate.TabIndex = 2
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(337, 264)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(83, 15)
        Me.Label19.TabIndex = 31
        Me.Label19.Text = "Total Discount"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(14, 264)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(58, 15)
        Me.Label14.TabIndex = 29
        Me.Label14.Text = "Add/Less"
        '
        'lbl_discamount
        '
        Me.lbl_discamount.BackColor = System.Drawing.Color.White
        Me.lbl_discamount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_discamount.Location = New System.Drawing.Point(458, 221)
        Me.lbl_discamount.Name = "lbl_discamount"
        Me.lbl_discamount.Size = New System.Drawing.Size(212, 24)
        Me.lbl_discamount.TabIndex = 6
        '
        'lbl_invoicekgs
        '
        Me.lbl_invoicekgs.BackColor = System.Drawing.Color.White
        Me.lbl_invoicekgs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_invoicekgs.Location = New System.Drawing.Point(133, 142)
        Me.lbl_invoicekgs.Name = "lbl_invoicekgs"
        Me.lbl_invoicekgs.Size = New System.Drawing.Size(181, 24)
        Me.lbl_invoicekgs.TabIndex = 42
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(337, 225)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(52, 15)
        Me.Label13.TabIndex = 23
        Me.Label13.Text = "Amount"
        '
        'txt_addless
        '
        Me.txt_addless.Location = New System.Drawing.Point(133, 260)
        Me.txt_addless.MaxLength = 35
        Me.txt_addless.Name = "txt_addless"
        Me.txt_addless.Size = New System.Drawing.Size(181, 23)
        Me.txt_addless.TabIndex = 7
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(14, 107)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(65, 15)
        Me.Label12.TabIndex = 22
        Me.Label12.Text = "From Date"
        '
        'lbl_refno
        '
        Me.lbl_refno.BackColor = System.Drawing.Color.White
        Me.lbl_refno.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_refno.Location = New System.Drawing.Point(133, 25)
        Me.lbl_refno.Name = "lbl_refno"
        Me.lbl_refno.Size = New System.Drawing.Size(181, 23)
        Me.lbl_refno.TabIndex = 21
        Me.lbl_refno.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbl_Company
        '
        Me.lbl_Company.AutoSize = True
        Me.lbl_Company.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.lbl_Company.Location = New System.Drawing.Point(14, 350)
        Me.lbl_Company.Name = "lbl_Company"
        Me.lbl_Company.Size = New System.Drawing.Size(77, 15)
        Me.lbl_Company.TabIndex = 19
        Me.lbl_Company.Text = "lbl_Company"
        Me.lbl_Company.Visible = False
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.DimGray
        Me.btn_close.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(568, 335)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(102, 35)
        Me.btn_close.TabIndex = 11
        Me.btn_close.TabStop = False
        Me.btn_close.Text = "&CLOSE"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'btn_save
        '
        Me.btn_save.BackColor = System.Drawing.Color.DimGray
        Me.btn_save.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btn_save.ForeColor = System.Drawing.Color.White
        Me.btn_save.Location = New System.Drawing.Point(458, 335)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(101, 35)
        Me.btn_save.TabIndex = 10
        Me.btn_save.TabStop = False
        Me.btn_save.Text = "&SAVE"
        Me.btn_save.UseVisualStyleBackColor = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(14, 146)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(73, 15)
        Me.Label10.TabIndex = 17
        Me.Label10.Text = "Invoiced Kgs"
        '
        'dtp_date
        '
        Me.dtp_date.CustomFormat = ""
        Me.dtp_date.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_date.Location = New System.Drawing.Point(458, 25)
        Me.dtp_date.Name = "dtp_date"
        Me.dtp_date.Size = New System.Drawing.Size(210, 23)
        Me.dtp_date.TabIndex = 0
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(14, 70)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(72, 15)
        Me.Label8.TabIndex = 14
        Me.Label8.Text = "Party Name"
        '
        'txt_Remarks
        '
        Me.txt_Remarks.Location = New System.Drawing.Point(133, 299)
        Me.txt_Remarks.MaxLength = 50
        Me.txt_Remarks.Name = "txt_Remarks"
        Me.txt_Remarks.Size = New System.Drawing.Size(537, 23)
        Me.txt_Remarks.TabIndex = 9
        '
        'txt_amountkg
        '
        Me.txt_amountkg.Location = New System.Drawing.Point(133, 221)
        Me.txt_amountkg.MaxLength = 35
        Me.txt_amountkg.Name = "txt_amountkg"
        Me.txt_amountkg.Size = New System.Drawing.Size(181, 23)
        Me.txt_amountkg.TabIndex = 5
        '
        'cbo_partyname
        '
        Me.cbo_partyname.FormattingEnabled = True
        Me.cbo_partyname.Location = New System.Drawing.Point(133, 64)
        Me.cbo_partyname.MaxLength = 35
        Me.cbo_partyname.Name = "cbo_partyname"
        Me.cbo_partyname.Size = New System.Drawing.Size(536, 23)
        Me.cbo_partyname.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 29)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Ref.No"
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
        Me.Label2.Location = New System.Drawing.Point(337, 30)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(33, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Date"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(14, 303)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(54, 15)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Remarks"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(14, 89)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(0, 15)
        Me.Label3.TabIndex = 2
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(337, 107)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(48, 15)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "To Date"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(14, 221)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(102, 32)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Discount %  (or)  Paise/Kg"
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.Color.DimGray
        Me.Label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label11.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label11.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.White
        Me.Label11.Location = New System.Drawing.Point(0, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(712, 35)
        Me.Label11.TabIndex = 23
        Me.Label11.Text = "CASH DISCOUNT"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txt_vatamount
        '
        Me.txt_vatamount.BackColor = System.Drawing.Color.White
        Me.txt_vatamount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.txt_vatamount.Location = New System.Drawing.Point(133, 260)
        Me.txt_vatamount.Name = "txt_vatamount"
        Me.txt_vatamount.Size = New System.Drawing.Size(181, 24)
        Me.txt_vatamount.TabIndex = 24
        Me.txt_vatamount.Text = "lbl_vatamount"
        '
        'txt_vatdisamt
        '
        Me.txt_vatdisamt.BackColor = System.Drawing.Color.White
        Me.txt_vatdisamt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.txt_vatdisamt.Location = New System.Drawing.Point(133, 301)
        Me.txt_vatdisamt.Name = "txt_vatdisamt"
        Me.txt_vatdisamt.Size = New System.Drawing.Size(181, 24)
        Me.txt_vatdisamt.TabIndex = 27
        Me.txt_vatdisamt.Text = "lbl_vatdisamt"
        '
        'pnl_filter
        '
        Me.pnl_filter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_filter.Controls.Add(Me.btn_closefilter)
        Me.pnl_filter.Controls.Add(Me.Label16)
        Me.pnl_filter.Controls.Add(Me.btn_filtershow)
        Me.pnl_filter.Controls.Add(Me.dgv_filter)
        Me.pnl_filter.Controls.Add(Me.cbo_Filter_PartyName)
        Me.pnl_filter.Controls.Add(Me.dtp_FilterTo_date)
        Me.pnl_filter.Controls.Add(Me.dtp_FilterFrom_date)
        Me.pnl_filter.Controls.Add(Me.Label15)
        Me.pnl_filter.Controls.Add(Me.Label9)
        Me.pnl_filter.Controls.Add(Me.Label17)
        Me.pnl_filter.Location = New System.Drawing.Point(57, 477)
        Me.pnl_filter.Name = "pnl_filter"
        Me.pnl_filter.Size = New System.Drawing.Size(562, 284)
        Me.pnl_filter.TabIndex = 25
        '
        'btn_closefilter
        '
        Me.btn_closefilter.Location = New System.Drawing.Point(456, 45)
        Me.btn_closefilter.Name = "btn_closefilter"
        Me.btn_closefilter.Size = New System.Drawing.Size(87, 27)
        Me.btn_closefilter.TabIndex = 4
        Me.btn_closefilter.Text = "&CLOSE"
        Me.btn_closefilter.UseVisualStyleBackColor = True
        '
        'Label16
        '
        Me.Label16.BackColor = System.Drawing.Color.DimGray
        Me.Label16.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label16.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.White
        Me.Label16.Location = New System.Drawing.Point(0, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(560, 30)
        Me.Label16.TabIndex = 8
        Me.Label16.Text = "FILTER"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btn_filtershow
        '
        Me.btn_filtershow.Location = New System.Drawing.Point(353, 45)
        Me.btn_filtershow.Name = "btn_filtershow"
        Me.btn_filtershow.Size = New System.Drawing.Size(87, 27)
        Me.btn_filtershow.TabIndex = 3
        Me.btn_filtershow.Text = "SHOW"
        Me.btn_filtershow.UseVisualStyleBackColor = True
        '
        'dgv_filter
        '
        Me.dgv_filter.AllowUserToAddRows = False
        Me.dgv_filter.AllowUserToDeleteRows = False
        Me.dgv_filter.AllowUserToResizeColumns = False
        Me.dgv_filter.AllowUserToResizeRows = False
        Me.dgv_filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_filter.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.dc, Me.Column1, Me.Column2, Me.Column5})
        Me.dgv_filter.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv_filter.Location = New System.Drawing.Point(12, 122)
        Me.dgv_filter.Name = "dgv_filter"
        Me.dgv_filter.RowHeadersVisible = False
        Me.dgv_filter.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_filter.Size = New System.Drawing.Size(531, 148)
        Me.dgv_filter.TabIndex = 5
        '
        'dc
        '
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dc.DefaultCellStyle = DataGridViewCellStyle1
        Me.dc.HeaderText = "Ref.No"
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
        Me.Column1.HeaderText = "Ref.Date"
        Me.Column1.Name = "Column1"
        Me.Column1.Width = 90
        '
        'Column2
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle3
        Me.Column2.HeaderText = "PARTY NAME"
        Me.Column2.MaxInputLength = 35
        Me.Column2.Name = "Column2"
        Me.Column2.Width = 250
        '
        'Column5
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column5.DefaultCellStyle = DataGridViewCellStyle4
        Me.Column5.HeaderText = "TOTAL DISC"
        Me.Column5.Name = "Column5"
        Me.Column5.Width = 110
        '
        'cbo_Filter_PartyName
        '
        Me.cbo_Filter_PartyName.FormattingEnabled = True
        Me.cbo_Filter_PartyName.Location = New System.Drawing.Point(93, 82)
        Me.cbo_Filter_PartyName.Name = "cbo_Filter_PartyName"
        Me.cbo_Filter_PartyName.Size = New System.Drawing.Size(450, 23)
        Me.cbo_Filter_PartyName.TabIndex = 2
        Me.cbo_Filter_PartyName.Text = "cbo_Filter_PartyName"
        '
        'dtp_FilterTo_date
        '
        Me.dtp_FilterTo_date.CustomFormat = "dd-MM-yyyy"
        Me.dtp_FilterTo_date.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtp_FilterTo_date.Location = New System.Drawing.Point(231, 47)
        Me.dtp_FilterTo_date.Name = "dtp_FilterTo_date"
        Me.dtp_FilterTo_date.Size = New System.Drawing.Size(102, 23)
        Me.dtp_FilterTo_date.TabIndex = 1
        '
        'dtp_FilterFrom_date
        '
        Me.dtp_FilterFrom_date.CustomFormat = "dd-MM-yyyy"
        Me.dtp_FilterFrom_date.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_FilterFrom_date.Location = New System.Drawing.Point(93, 47)
        Me.dtp_FilterFrom_date.Name = "dtp_FilterFrom_date"
        Me.dtp_FilterFrom_date.Size = New System.Drawing.Size(98, 23)
        Me.dtp_FilterFrom_date.TabIndex = 0
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(12, 86)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(72, 15)
        Me.Label15.TabIndex = 2
        Me.Label15.Text = "Party Name"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(201, 51)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(19, 15)
        Me.Label9.TabIndex = 1
        Me.Label9.Text = "To"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(12, 51)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(53, 15)
        Me.Label17.TabIndex = 0
        Me.Label17.Text = "Ref.Date"
        '
        'lbl_UserName
        '
        Me.lbl_UserName.AutoSize = True
        Me.lbl_UserName.BackColor = System.Drawing.Color.DimGray
        Me.lbl_UserName.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_UserName.ForeColor = System.Drawing.Color.White
        Me.lbl_UserName.Location = New System.Drawing.Point(591, 9)
        Me.lbl_UserName.Name = "lbl_UserName"
        Me.lbl_UserName.Size = New System.Drawing.Size(105, 19)
        Me.lbl_UserName.TabIndex = 268
        Me.lbl_UserName.Text = "USER : ADMIN"
        '
        'Sizing_CashDiscount
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(712, 445)
        Me.Controls.Add(Me.lbl_UserName)
        Me.Controls.Add(Me.pnl_filter)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.pnl_back)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "Sizing_CashDiscount"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "CASH DISCOUNT"
        Me.pnl_back.ResumeLayout(False)
        Me.pnl_back.PerformLayout()
        Me.pnl_filter.ResumeLayout(False)
        Me.pnl_filter.PerformLayout()
        CType(Me.dgv_filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnl_back As System.Windows.Forms.Panel
    Friend WithEvents lbl_totaldisc As System.Windows.Forms.Label
    Friend WithEvents dtp_Todate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtp_fromdate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents lbl_discamount As System.Windows.Forms.Label
    Friend WithEvents lbl_invoicekgs As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txt_addless As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents lbl_refno As System.Windows.Forms.Label
    Friend WithEvents lbl_Company As System.Windows.Forms.Label
    Friend WithEvents btn_close As System.Windows.Forms.Button
    Friend WithEvents btn_save As System.Windows.Forms.Button
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents dtp_date As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txt_Remarks As System.Windows.Forms.TextBox
    Friend WithEvents txt_amountkg As System.Windows.Forms.TextBox
    Friend WithEvents cbo_partyname As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txt_vatamount As System.Windows.Forms.Label
    Friend WithEvents txt_vatdisamt As System.Windows.Forms.Label
    Friend WithEvents pnl_filter As System.Windows.Forms.Panel
    Friend WithEvents btn_closefilter As System.Windows.Forms.Button
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents btn_filtershow As System.Windows.Forms.Button
    Friend WithEvents dgv_filter As System.Windows.Forms.DataGridView
    Friend WithEvents dc As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cbo_Filter_PartyName As System.Windows.Forms.ComboBox
    Friend WithEvents dtp_FilterTo_date As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtp_FilterFrom_date As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents lbl_InvoicedAmt As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents cbo_DiscountType As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_UserName As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents chk_Printed As System.Windows.Forms.CheckBox
    Friend WithEvents btn_UserModification As System.Windows.Forms.Button
End Class
