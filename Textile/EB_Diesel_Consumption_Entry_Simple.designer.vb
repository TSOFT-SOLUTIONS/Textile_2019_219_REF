<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EB_Diesel_Consumption_Entry_Simple
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
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgv_filter = New System.Windows.Forms.DataGridView()
        Me.dc = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btn_filtershow = New System.Windows.Forms.Button()
        Me.btn_Close_Filter = New System.Windows.Forms.Button()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.txt_OpeningKWH = New System.Windows.Forms.TextBox()
        Me.dtp_FilterTo_date = New System.Windows.Forms.DateTimePicker()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.dtp_FilterFrom_date = New System.Windows.Forms.DateTimePicker()
        Me.pnl_filter = New System.Windows.Forms.Panel()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.lbl_Company = New System.Windows.Forms.Label()
        Me.pnl_back = New System.Windows.Forms.Panel()
        Me.txt_UKG = New System.Windows.Forms.TextBox()
        Me.txt_GSCost_ForProduction = New System.Windows.Forms.TextBox()
        Me.txt_EBCost_ForProduction = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.msk_Time = New System.Windows.Forms.MaskedTextBox()
        Me.txt_Demand = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.lbl_GensetAmount = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txt_RatePerGensetUnit = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txt_Diesel_Used = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.lbl_EbAmount = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lbl_GensetUnits = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lbl_KWAHUnits = New System.Windows.Forms.Label()
        Me.Label38 = New System.Windows.Forms.Label()
        Me.lbl_KWHUnits = New System.Windows.Forms.Label()
        Me.txt_ClosingGenset = New System.Windows.Forms.TextBox()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.txt_OpeningGenset = New System.Windows.Forms.TextBox()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.txt_RatePerEBUnit = New System.Windows.Forms.TextBox()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.txt_PowerFactor = New System.Windows.Forms.TextBox()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.txt_ClosingKWAH = New System.Windows.Forms.TextBox()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.txt_OpeningKWAH = New System.Windows.Forms.TextBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.txt_ClosingKWH = New System.Windows.Forms.TextBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.lbl_RefNo = New System.Windows.Forms.Label()
        Me.btn_close = New System.Windows.Forms.Button()
        Me.btn_save = New System.Windows.Forms.Button()
        Me.dtp_date = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        CType(Me.dgv_filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnl_filter.SuspendLayout()
        Me.pnl_back.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgv_filter
        '
        Me.dgv_filter.AllowUserToAddRows = False
        Me.dgv_filter.AllowUserToDeleteRows = False
        Me.dgv_filter.AllowUserToResizeColumns = False
        Me.dgv_filter.AllowUserToResizeRows = False
        Me.dgv_filter.BackgroundColor = System.Drawing.Color.White
        Me.dgv_filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_filter.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.dc, Me.Column1, Me.Column2, Me.Column4, Me.Column3})
        Me.dgv_filter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgv_filter.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv_filter.Location = New System.Drawing.Point(0, 79)
        Me.dgv_filter.Name = "dgv_filter"
        Me.dgv_filter.RowHeadersVisible = False
        Me.dgv_filter.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_filter.Size = New System.Drawing.Size(584, 214)
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
        Me.dc.Width = 45
        '
        'Column1
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column1.DefaultCellStyle = DataGridViewCellStyle2
        Me.Column1.HeaderText = "Date"
        Me.Column1.Name = "Column1"
        Me.Column1.Width = 80
        '
        'Column2
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle3
        Me.Column2.HeaderText = "EB UNITS"
        Me.Column2.MaxInputLength = 35
        Me.Column2.Name = "Column2"
        Me.Column2.Width = 180
        '
        'Column4
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column4.DefaultCellStyle = DataGridViewCellStyle4
        Me.Column4.HeaderText = "GENSET UNIT"
        Me.Column4.MaxInputLength = 50
        Me.Column4.Name = "Column4"
        Me.Column4.Width = 155
        '
        'Column3
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column3.DefaultCellStyle = DataGridViewCellStyle5
        Me.Column3.HeaderText = "UKG"
        Me.Column3.Name = "Column3"
        Me.Column3.Width = 90
        '
        'btn_filtershow
        '
        Me.btn_filtershow.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_filtershow.ForeColor = System.Drawing.Color.White
        Me.btn_filtershow.Location = New System.Drawing.Point(457, 38)
        Me.btn_filtershow.Name = "btn_filtershow"
        Me.btn_filtershow.Size = New System.Drawing.Size(86, 32)
        Me.btn_filtershow.TabIndex = 5
        Me.btn_filtershow.Text = "SHOW"
        Me.btn_filtershow.UseVisualStyleBackColor = False
        '
        'btn_Close_Filter
        '
        Me.btn_Close_Filter.BackColor = System.Drawing.Color.White
        Me.btn_Close_Filter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn_Close_Filter.FlatAppearance.BorderSize = 0
        Me.btn_Close_Filter.Image = Global.Textile.My.Resources.Resources.Delete2
        Me.btn_Close_Filter.Location = New System.Drawing.Point(559, -1)
        Me.btn_Close_Filter.Name = "btn_Close_Filter"
        Me.btn_Close_Filter.Size = New System.Drawing.Size(26, 27)
        Me.btn_Close_Filter.TabIndex = 41
        Me.btn_Close_Filter.TabStop = False
        Me.btn_Close_Filter.UseVisualStyleBackColor = True
        '
        'Label16
        '
        Me.Label16.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(90, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.Label16.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label16.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.White
        Me.Label16.Location = New System.Drawing.Point(0, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(584, 25)
        Me.Label16.TabIndex = 8
        Me.Label16.Text = "FILTER"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txt_OpeningKWH
        '
        Me.txt_OpeningKWH.BackColor = System.Drawing.Color.Khaki
        Me.txt_OpeningKWH.Location = New System.Drawing.Point(101, 53)
        Me.txt_OpeningKWH.MaxLength = 15
        Me.txt_OpeningKWH.Name = "txt_OpeningKWH"
        Me.txt_OpeningKWH.ReadOnly = True
        Me.txt_OpeningKWH.Size = New System.Drawing.Size(132, 23)
        Me.txt_OpeningKWH.TabIndex = 2
        Me.txt_OpeningKWH.TabStop = False
        Me.txt_OpeningKWH.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'dtp_FilterTo_date
        '
        Me.dtp_FilterTo_date.CustomFormat = "dd-MM-yyyy"
        Me.dtp_FilterTo_date.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtp_FilterTo_date.Location = New System.Drawing.Point(263, 43)
        Me.dtp_FilterTo_date.Name = "dtp_FilterTo_date"
        Me.dtp_FilterTo_date.Size = New System.Drawing.Size(164, 23)
        Me.dtp_FilterTo_date.TabIndex = 1
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.ForeColor = System.Drawing.Color.Blue
        Me.Label14.Location = New System.Drawing.Point(238, 49)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(19, 15)
        Me.Label14.TabIndex = 1
        Me.Label14.Text = "To"
        '
        'dtp_FilterFrom_date
        '
        Me.dtp_FilterFrom_date.CustomFormat = "dd-MM-yyyy"
        Me.dtp_FilterFrom_date.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtp_FilterFrom_date.Location = New System.Drawing.Point(80, 45)
        Me.dtp_FilterFrom_date.Name = "dtp_FilterFrom_date"
        Me.dtp_FilterFrom_date.Size = New System.Drawing.Size(152, 23)
        Me.dtp_FilterFrom_date.TabIndex = 0
        '
        'pnl_filter
        '
        Me.pnl_filter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_filter.Controls.Add(Me.btn_Close_Filter)
        Me.pnl_filter.Controls.Add(Me.Label16)
        Me.pnl_filter.Controls.Add(Me.btn_filtershow)
        Me.pnl_filter.Controls.Add(Me.dgv_filter)
        Me.pnl_filter.Controls.Add(Me.dtp_FilterTo_date)
        Me.pnl_filter.Controls.Add(Me.dtp_FilterFrom_date)
        Me.pnl_filter.Controls.Add(Me.Label14)
        Me.pnl_filter.Controls.Add(Me.Label13)
        Me.pnl_filter.Location = New System.Drawing.Point(784, 99)
        Me.pnl_filter.Name = "pnl_filter"
        Me.pnl_filter.Size = New System.Drawing.Size(586, 295)
        Me.pnl_filter.TabIndex = 35
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.ForeColor = System.Drawing.Color.Blue
        Me.Label13.Location = New System.Drawing.Point(5, 49)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(53, 15)
        Me.Label13.TabIndex = 0
        Me.Label13.Text = "Ref.Date"
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.Label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label11.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label11.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.White
        Me.Label11.Location = New System.Drawing.Point(0, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(727, 35)
        Me.Label11.TabIndex = 34
        Me.Label11.Text = "EB DIESEL CONSUMPTION"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbl_Company
        '
        Me.lbl_Company.AutoSize = True
        Me.lbl_Company.BackColor = System.Drawing.Color.Red
        Me.lbl_Company.Location = New System.Drawing.Point(11, 35)
        Me.lbl_Company.Name = "lbl_Company"
        Me.lbl_Company.Size = New System.Drawing.Size(77, 15)
        Me.lbl_Company.TabIndex = 36
        Me.lbl_Company.Text = "lbl_Company"
        '
        'pnl_back
        '
        Me.pnl_back.AutoSize = True
        Me.pnl_back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_back.Controls.Add(Me.txt_UKG)
        Me.pnl_back.Controls.Add(Me.txt_GSCost_ForProduction)
        Me.pnl_back.Controls.Add(Me.txt_EBCost_ForProduction)
        Me.pnl_back.Controls.Add(Me.Label18)
        Me.pnl_back.Controls.Add(Me.msk_Time)
        Me.pnl_back.Controls.Add(Me.txt_Demand)
        Me.pnl_back.Controls.Add(Me.Label15)
        Me.pnl_back.Controls.Add(Me.Label19)
        Me.pnl_back.Controls.Add(Me.Label17)
        Me.pnl_back.Controls.Add(Me.lbl_GensetAmount)
        Me.pnl_back.Controls.Add(Me.Label9)
        Me.pnl_back.Controls.Add(Me.txt_RatePerGensetUnit)
        Me.pnl_back.Controls.Add(Me.Label10)
        Me.pnl_back.Controls.Add(Me.txt_Diesel_Used)
        Me.pnl_back.Controls.Add(Me.Label12)
        Me.pnl_back.Controls.Add(Me.lbl_EbAmount)
        Me.pnl_back.Controls.Add(Me.Label6)
        Me.pnl_back.Controls.Add(Me.Label8)
        Me.pnl_back.Controls.Add(Me.lbl_GensetUnits)
        Me.pnl_back.Controls.Add(Me.Label5)
        Me.pnl_back.Controls.Add(Me.lbl_KWAHUnits)
        Me.pnl_back.Controls.Add(Me.Label38)
        Me.pnl_back.Controls.Add(Me.lbl_KWHUnits)
        Me.pnl_back.Controls.Add(Me.txt_ClosingGenset)
        Me.pnl_back.Controls.Add(Me.Label32)
        Me.pnl_back.Controls.Add(Me.txt_OpeningGenset)
        Me.pnl_back.Controls.Add(Me.Label33)
        Me.pnl_back.Controls.Add(Me.txt_RatePerEBUnit)
        Me.pnl_back.Controls.Add(Me.Label30)
        Me.pnl_back.Controls.Add(Me.txt_PowerFactor)
        Me.pnl_back.Controls.Add(Me.Label31)
        Me.pnl_back.Controls.Add(Me.Label29)
        Me.pnl_back.Controls.Add(Me.txt_ClosingKWAH)
        Me.pnl_back.Controls.Add(Me.Label25)
        Me.pnl_back.Controls.Add(Me.txt_OpeningKWAH)
        Me.pnl_back.Controls.Add(Me.Label26)
        Me.pnl_back.Controls.Add(Me.txt_ClosingKWH)
        Me.pnl_back.Controls.Add(Me.Label24)
        Me.pnl_back.Controls.Add(Me.lbl_RefNo)
        Me.pnl_back.Controls.Add(Me.btn_close)
        Me.pnl_back.Controls.Add(Me.btn_save)
        Me.pnl_back.Controls.Add(Me.dtp_date)
        Me.pnl_back.Controls.Add(Me.Label1)
        Me.pnl_back.Controls.Add(Me.Label7)
        Me.pnl_back.Controls.Add(Me.Label2)
        Me.pnl_back.Controls.Add(Me.txt_OpeningKWH)
        Me.pnl_back.Controls.Add(Me.Label22)
        Me.pnl_back.Location = New System.Drawing.Point(6, 42)
        Me.pnl_back.Name = "pnl_back"
        Me.pnl_back.Size = New System.Drawing.Size(709, 336)
        Me.pnl_back.TabIndex = 33
        '
        'txt_UKG
        '
        Me.txt_UKG.Location = New System.Drawing.Point(573, 242)
        Me.txt_UKG.Name = "txt_UKG"
        Me.txt_UKG.Size = New System.Drawing.Size(131, 23)
        Me.txt_UKG.TabIndex = 81
        Me.txt_UKG.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txt_UKG.Visible = False
        '
        'txt_GSCost_ForProduction
        '
        Me.txt_GSCost_ForProduction.Location = New System.Drawing.Point(338, 242)
        Me.txt_GSCost_ForProduction.Name = "txt_GSCost_ForProduction"
        Me.txt_GSCost_ForProduction.Size = New System.Drawing.Size(134, 23)
        Me.txt_GSCost_ForProduction.TabIndex = 80
        Me.txt_GSCost_ForProduction.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txt_GSCost_ForProduction.Visible = False
        '
        'txt_EBCost_ForProduction
        '
        Me.txt_EBCost_ForProduction.Location = New System.Drawing.Point(101, 242)
        Me.txt_EBCost_ForProduction.Name = "txt_EBCost_ForProduction"
        Me.txt_EBCost_ForProduction.Size = New System.Drawing.Size(133, 23)
        Me.txt_EBCost_ForProduction.TabIndex = 79
        Me.txt_EBCost_ForProduction.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txt_EBCost_ForProduction.Visible = False
        '
        'Label18
        '
        Me.Label18.ForeColor = System.Drawing.Color.Blue
        Me.Label18.Location = New System.Drawing.Point(478, 16)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(69, 15)
        Me.Label18.TabIndex = 42
        Me.Label18.Text = "TIME"
        '
        'msk_Time
        '
        Me.msk_Time.Font = New System.Drawing.Font("Courier New", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.msk_Time.HidePromptOnLeave = True
        Me.msk_Time.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.msk_Time.Location = New System.Drawing.Point(573, 13)
        Me.msk_Time.Mask = "00:00"
        Me.msk_Time.Name = "msk_Time"
        Me.msk_Time.ShortcutsEnabled = False
        Me.msk_Time.Size = New System.Drawing.Size(131, 24)
        Me.msk_Time.TabIndex = 1
        Me.msk_Time.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.msk_Time.ValidatingType = GetType(Date)
        '
        'txt_Demand
        '
        Me.txt_Demand.Location = New System.Drawing.Point(100, 276)
        Me.txt_Demand.MaxLength = 15
        Me.txt_Demand.Name = "txt_Demand"
        Me.txt_Demand.Size = New System.Drawing.Size(133, 23)
        Me.txt_Demand.TabIndex = 20
        Me.txt_Demand.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txt_Demand.Visible = False
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.ForeColor = System.Drawing.Color.Blue
        Me.Label15.Location = New System.Drawing.Point(2, 279)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(57, 15)
        Me.Label15.TabIndex = 78
        Me.Label15.Text = "DEMAND"
        Me.Label15.Visible = False
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.ForeColor = System.Drawing.Color.Blue
        Me.Label19.Location = New System.Drawing.Point(244, 245)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(52, 15)
        Me.Label19.TabIndex = 75
        Me.Label19.Text = "GS COST"
        Me.Label19.Visible = False
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.ForeColor = System.Drawing.Color.Blue
        Me.Label17.Location = New System.Drawing.Point(5, 245)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(51, 15)
        Me.Label17.TabIndex = 73
        Me.Label17.Text = "EB COST"
        Me.Label17.Visible = False
        '
        'lbl_GensetAmount
        '
        Me.lbl_GensetAmount.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lbl_GensetAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_GensetAmount.Location = New System.Drawing.Point(573, 206)
        Me.lbl_GensetAmount.Name = "lbl_GensetAmount"
        Me.lbl_GensetAmount.Size = New System.Drawing.Size(131, 22)
        Me.lbl_GensetAmount.TabIndex = 16
        Me.lbl_GensetAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.ForeColor = System.Drawing.Color.Black
        Me.Label9.Location = New System.Drawing.Point(478, 210)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(95, 15)
        Me.Label9.TabIndex = 71
        Me.Label9.Text = "GS AMOUNT(Rs)"
        '
        'txt_RatePerGensetUnit
        '
        Me.txt_RatePerGensetUnit.Location = New System.Drawing.Point(338, 207)
        Me.txt_RatePerGensetUnit.MaxLength = 15
        Me.txt_RatePerGensetUnit.Name = "txt_RatePerGensetUnit"
        Me.txt_RatePerGensetUnit.Size = New System.Drawing.Size(134, 23)
        Me.txt_RatePerGensetUnit.TabIndex = 15
        Me.txt_RatePerGensetUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.ForeColor = System.Drawing.Color.Blue
        Me.Label10.Location = New System.Drawing.Point(243, 210)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(91, 15)
        Me.Label10.TabIndex = 70
        Me.Label10.Text = "GEN RATE/UNIT"
        '
        'txt_Diesel_Used
        '
        Me.txt_Diesel_Used.Location = New System.Drawing.Point(101, 207)
        Me.txt_Diesel_Used.MaxLength = 15
        Me.txt_Diesel_Used.Name = "txt_Diesel_Used"
        Me.txt_Diesel_Used.Size = New System.Drawing.Size(133, 23)
        Me.txt_Diesel_Used.TabIndex = 14
        Me.txt_Diesel_Used.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.ForeColor = System.Drawing.Color.Blue
        Me.Label12.Location = New System.Drawing.Point(3, 210)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(73, 15)
        Me.Label12.TabIndex = 69
        Me.Label12.Text = "DIESEL USED"
        '
        'lbl_EbAmount
        '
        Me.lbl_EbAmount.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lbl_EbAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_EbAmount.Location = New System.Drawing.Point(572, 166)
        Me.lbl_EbAmount.Name = "lbl_EbAmount"
        Me.lbl_EbAmount.Size = New System.Drawing.Size(131, 22)
        Me.lbl_EbAmount.TabIndex = 13
        Me.lbl_EbAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.ForeColor = System.Drawing.Color.Black
        Me.Label6.Location = New System.Drawing.Point(478, 170)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(97, 15)
        Me.Label6.TabIndex = 65
        Me.Label6.Text = "EB AMOUNT (Rs)"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ForeColor = System.Drawing.Color.Blue
        Me.Label8.Location = New System.Drawing.Point(478, 245)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(29, 15)
        Me.Label8.TabIndex = 63
        Me.Label8.Text = "UKG"
        Me.Label8.Visible = False
        '
        'lbl_GensetUnits
        '
        Me.lbl_GensetUnits.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lbl_GensetUnits.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_GensetUnits.Location = New System.Drawing.Point(572, 126)
        Me.lbl_GensetUnits.Name = "lbl_GensetUnits"
        Me.lbl_GensetUnits.Size = New System.Drawing.Size(131, 22)
        Me.lbl_GensetUnits.TabIndex = 10
        Me.lbl_GensetUnits.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.Black
        Me.Label5.Location = New System.Drawing.Point(476, 130)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(83, 15)
        Me.Label5.TabIndex = 61
        Me.Label5.Text = "GENSET UNITS"
        '
        'lbl_KWAHUnits
        '
        Me.lbl_KWAHUnits.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lbl_KWAHUnits.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_KWAHUnits.Location = New System.Drawing.Point(572, 89)
        Me.lbl_KWAHUnits.Name = "lbl_KWAHUnits"
        Me.lbl_KWAHUnits.Size = New System.Drawing.Size(131, 22)
        Me.lbl_KWAHUnits.TabIndex = 7
        Me.lbl_KWAHUnits.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label38
        '
        Me.Label38.AutoSize = True
        Me.Label38.ForeColor = System.Drawing.Color.Black
        Me.Label38.Location = New System.Drawing.Point(476, 93)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(76, 15)
        Me.Label38.TabIndex = 59
        Me.Label38.Text = "KWAH UNITS"
        '
        'lbl_KWHUnits
        '
        Me.lbl_KWHUnits.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lbl_KWHUnits.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_KWHUnits.Location = New System.Drawing.Point(572, 53)
        Me.lbl_KWHUnits.Name = "lbl_KWHUnits"
        Me.lbl_KWHUnits.Size = New System.Drawing.Size(131, 22)
        Me.lbl_KWHUnits.TabIndex = 4
        Me.lbl_KWHUnits.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_ClosingGenset
        '
        Me.txt_ClosingGenset.Location = New System.Drawing.Point(338, 127)
        Me.txt_ClosingGenset.MaxLength = 15
        Me.txt_ClosingGenset.Name = "txt_ClosingGenset"
        Me.txt_ClosingGenset.Size = New System.Drawing.Size(132, 23)
        Me.txt_ClosingGenset.TabIndex = 9
        Me.txt_ClosingGenset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.ForeColor = System.Drawing.Color.Blue
        Me.Label32.Location = New System.Drawing.Point(240, 130)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(99, 15)
        Me.Label32.TabIndex = 52
        Me.Label32.Text = "GENSET CLOSING"
        '
        'txt_OpeningGenset
        '
        Me.txt_OpeningGenset.BackColor = System.Drawing.Color.Khaki
        Me.txt_OpeningGenset.Location = New System.Drawing.Point(101, 127)
        Me.txt_OpeningGenset.MaxLength = 15
        Me.txt_OpeningGenset.Name = "txt_OpeningGenset"
        Me.txt_OpeningGenset.ReadOnly = True
        Me.txt_OpeningGenset.Size = New System.Drawing.Size(133, 23)
        Me.txt_OpeningGenset.TabIndex = 8
        Me.txt_OpeningGenset.TabStop = False
        Me.txt_OpeningGenset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.ForeColor = System.Drawing.Color.Blue
        Me.Label33.Location = New System.Drawing.Point(3, 130)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(102, 15)
        Me.Label33.TabIndex = 50
        Me.Label33.Text = "GENSET OPENING"
        '
        'txt_RatePerEBUnit
        '
        Me.txt_RatePerEBUnit.Location = New System.Drawing.Point(338, 167)
        Me.txt_RatePerEBUnit.MaxLength = 15
        Me.txt_RatePerEBUnit.Name = "txt_RatePerEBUnit"
        Me.txt_RatePerEBUnit.Size = New System.Drawing.Size(134, 23)
        Me.txt_RatePerEBUnit.TabIndex = 12
        Me.txt_RatePerEBUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.ForeColor = System.Drawing.Color.Blue
        Me.Label30.Location = New System.Drawing.Point(243, 170)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(81, 15)
        Me.Label30.TabIndex = 48
        Me.Label30.Text = "EB RATE/UNIT"
        '
        'txt_PowerFactor
        '
        Me.txt_PowerFactor.Location = New System.Drawing.Point(101, 167)
        Me.txt_PowerFactor.MaxLength = 15
        Me.txt_PowerFactor.Name = "txt_PowerFactor"
        Me.txt_PowerFactor.Size = New System.Drawing.Size(133, 23)
        Me.txt_PowerFactor.TabIndex = 11
        Me.txt_PowerFactor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.ForeColor = System.Drawing.Color.Blue
        Me.Label31.Location = New System.Drawing.Point(3, 170)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(93, 15)
        Me.Label31.TabIndex = 46
        Me.Label31.Text = "POWER FACTOR"
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.ForeColor = System.Drawing.Color.Black
        Me.Label29.Location = New System.Drawing.Point(478, 57)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(69, 15)
        Me.Label29.TabIndex = 42
        Me.Label29.Text = "KWH UNITS"
        '
        'txt_ClosingKWAH
        '
        Me.txt_ClosingKWAH.Location = New System.Drawing.Point(338, 90)
        Me.txt_ClosingKWAH.MaxLength = 15
        Me.txt_ClosingKWAH.Name = "txt_ClosingKWAH"
        Me.txt_ClosingKWAH.Size = New System.Drawing.Size(132, 23)
        Me.txt_ClosingKWAH.TabIndex = 6
        Me.txt_ClosingKWAH.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.ForeColor = System.Drawing.Color.Blue
        Me.Label25.Location = New System.Drawing.Point(240, 93)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(92, 15)
        Me.Label25.TabIndex = 40
        Me.Label25.Text = "CLOSING KWAH"
        '
        'txt_OpeningKWAH
        '
        Me.txt_OpeningKWAH.BackColor = System.Drawing.Color.Khaki
        Me.txt_OpeningKWAH.Location = New System.Drawing.Point(101, 90)
        Me.txt_OpeningKWAH.MaxLength = 15
        Me.txt_OpeningKWAH.Name = "txt_OpeningKWAH"
        Me.txt_OpeningKWAH.ReadOnly = True
        Me.txt_OpeningKWAH.Size = New System.Drawing.Size(133, 23)
        Me.txt_OpeningKWAH.TabIndex = 5
        Me.txt_OpeningKWAH.TabStop = False
        Me.txt_OpeningKWAH.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.ForeColor = System.Drawing.Color.Blue
        Me.Label26.Location = New System.Drawing.Point(1, 93)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(95, 15)
        Me.Label26.TabIndex = 38
        Me.Label26.Text = "OPENING KWAH"
        '
        'txt_ClosingKWH
        '
        Me.txt_ClosingKWH.Location = New System.Drawing.Point(338, 53)
        Me.txt_ClosingKWH.MaxLength = 15
        Me.txt_ClosingKWH.Name = "txt_ClosingKWH"
        Me.txt_ClosingKWH.Size = New System.Drawing.Size(132, 23)
        Me.txt_ClosingKWH.TabIndex = 3
        Me.txt_ClosingKWH.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.ForeColor = System.Drawing.Color.Blue
        Me.Label24.Location = New System.Drawing.Point(243, 57)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(85, 15)
        Me.Label24.TabIndex = 36
        Me.Label24.Text = "CLOSING KWH"
        '
        'lbl_RefNo
        '
        Me.lbl_RefNo.BackColor = System.Drawing.Color.White
        Me.lbl_RefNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_RefNo.ForeColor = System.Drawing.Color.Black
        Me.lbl_RefNo.Location = New System.Drawing.Point(101, 10)
        Me.lbl_RefNo.Name = "lbl_RefNo"
        Me.lbl_RefNo.Size = New System.Drawing.Size(133, 23)
        Me.lbl_RefNo.TabIndex = 21
        Me.lbl_RefNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(599, 281)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(82, 35)
        Me.btn_close.TabIndex = 22
        Me.btn_close.TabStop = False
        Me.btn_close.Text = "&CLOSE"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'btn_save
        '
        Me.btn_save.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_save.ForeColor = System.Drawing.Color.White
        Me.btn_save.Location = New System.Drawing.Point(493, 281)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(82, 35)
        Me.btn_save.TabIndex = 21
        Me.btn_save.TabStop = False
        Me.btn_save.Text = "&SAVE"
        Me.btn_save.UseVisualStyleBackColor = False
        '
        'dtp_date
        '
        Me.dtp_date.CustomFormat = "dd-MM-yyyy"
        Me.dtp_date.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtp_date.Location = New System.Drawing.Point(338, 14)
        Me.dtp_date.Name = "dtp_date"
        Me.dtp_date.Size = New System.Drawing.Size(134, 23)
        Me.dtp_date.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(8, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(46, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "REF.NO"
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
        Me.Label2.Location = New System.Drawing.Point(242, 14)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(34, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "DATE"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.ForeColor = System.Drawing.Color.Blue
        Me.Label22.Location = New System.Drawing.Point(3, 56)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(88, 15)
        Me.Label22.TabIndex = 34
        Me.Label22.Text = "OPENING KWH"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(677, 114)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(0, 15)
        Me.Label3.TabIndex = 2
        '
        'EB_Diesel_Consumption_Entry_Simple
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(727, 387)
        Me.Controls.Add(Me.pnl_filter)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.lbl_Company)
        Me.Controls.Add(Me.pnl_back)
        Me.Controls.Add(Me.Label3)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "EB_Diesel_Consumption_Entry_Simple"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "EB DIESEL CONSUMPTION"
        CType(Me.dgv_filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnl_filter.ResumeLayout(False)
        Me.pnl_filter.PerformLayout()
        Me.pnl_back.ResumeLayout(False)
        Me.pnl_back.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgv_filter As System.Windows.Forms.DataGridView
    Friend WithEvents btn_filtershow As System.Windows.Forms.Button
    Friend WithEvents btn_Close_Filter As System.Windows.Forms.Button
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents txt_OpeningKWH As System.Windows.Forms.TextBox
    Friend WithEvents dtp_FilterTo_date As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents dtp_FilterFrom_date As System.Windows.Forms.DateTimePicker
    Friend WithEvents pnl_filter As System.Windows.Forms.Panel
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents lbl_Company As System.Windows.Forms.Label
    Friend WithEvents pnl_back As System.Windows.Forms.Panel
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents lbl_RefNo As System.Windows.Forms.Label
    Friend WithEvents btn_close As System.Windows.Forms.Button
    Friend WithEvents btn_save As System.Windows.Forms.Button
    Friend WithEvents dtp_date As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txt_ClosingKWH As System.Windows.Forms.TextBox
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents txt_ClosingKWAH As System.Windows.Forms.TextBox
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents txt_OpeningKWAH As System.Windows.Forms.TextBox
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lbl_GensetUnits As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lbl_KWAHUnits As System.Windows.Forms.Label
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents txt_ClosingGenset As System.Windows.Forms.TextBox
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents txt_OpeningGenset As System.Windows.Forms.TextBox
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents txt_RatePerEBUnit As System.Windows.Forms.TextBox
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents txt_PowerFactor As System.Windows.Forms.TextBox
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents dc As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lbl_EbAmount As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txt_RatePerGensetUnit As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txt_Diesel_Used As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents lbl_GensetAmount As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents txt_Demand As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents lbl_KWHUnits As Label
    Friend WithEvents Label18 As Label
    Friend WithEvents msk_Time As MaskedTextBox
    Friend WithEvents txt_EBCost_ForProduction As TextBox
    Friend WithEvents txt_GSCost_ForProduction As TextBox
    Friend WithEvents txt_UKG As TextBox
End Class
