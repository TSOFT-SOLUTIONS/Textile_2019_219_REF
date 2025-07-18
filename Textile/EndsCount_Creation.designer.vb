<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EndsCount_Creation
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EndsCount_Creation))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.grp_Find = New System.Windows.Forms.GroupBox()
        Me.btn_Find = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.cbo_Find = New System.Windows.Forms.ComboBox()
        Me.grp_Filter = New System.Windows.Forms.GroupBox()
        Me.btn_Open = New System.Windows.Forms.Button()
        Me.btn_CloseFilter = New System.Windows.Forms.Button()
        Me.dgv_Filter = New System.Windows.Forms.DataGridView()
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.btn_RateDetails = New System.Windows.Forms.Button()
        Me.chk_Close_STS = New System.Windows.Forms.CheckBox()
        Me.cbo_Sizing_EndsCount = New System.Windows.Forms.ComboBox()
        Me.lbl_Sizing = New System.Windows.Forms.Label()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txt_Meters = New System.Windows.Forms.TextBox()
        Me.cbo_Transfer = New System.Windows.Forms.ComboBox()
        Me.lbl_TransferStockTo = New System.Windows.Forms.Label()
        Me.cbo_EndsCountGroup = New System.Windows.Forms.ComboBox()
        Me.cbo_Cotton_Polyester_Jari = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txt_MeterPcs = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.btn_Save = New System.Windows.Forms.Button()
        Me.cbo_StockIn = New System.Windows.Forms.ComboBox()
        Me.lbl_IdNo = New System.Windows.Forms.Label()
        Me.txt_Rate = New System.Windows.Forms.TextBox()
        Me.lbl_Rate_Caption = New System.Windows.Forms.Label()
        Me.cbo_Count = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txt_Ends = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txt_EndsCount = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cbo_Single_Double_Triple = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.pnl_RateDetails = New System.Windows.Forms.Panel()
        Me.btn_Close_rate = New System.Windows.Forms.Button()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.dgv_EndsCountRate_Details = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.grp_Find.SuspendLayout()
        Me.grp_Filter.SuspendLayout()
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnl_Back.SuspendLayout()
        Me.pnl_RateDetails.SuspendLayout()
        CType(Me.dgv_EndsCountRate_Details, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grp_Find
        '
        Me.grp_Find.Controls.Add(Me.btn_Find)
        Me.grp_Find.Controls.Add(Me.btnClose)
        Me.grp_Find.Controls.Add(Me.cbo_Find)
        Me.grp_Find.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Find.Location = New System.Drawing.Point(12, 373)
        Me.grp_Find.Name = "grp_Find"
        Me.grp_Find.Size = New System.Drawing.Size(557, 179)
        Me.grp_Find.TabIndex = 3
        Me.grp_Find.TabStop = False
        Me.grp_Find.Text = "FINDING"
        '
        'btn_Find
        '
        Me.btn_Find.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Find.Image = CType(resources.GetObject("btn_Find.Image"), System.Drawing.Image)
        Me.btn_Find.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Find.Location = New System.Drawing.Point(359, 133)
        Me.btn_Find.Name = "btn_Find"
        Me.btn_Find.Size = New System.Drawing.Size(83, 29)
        Me.btn_Find.TabIndex = 4
        Me.btn_Find.TabStop = False
        Me.btn_Find.Text = "&Find"
        Me.btn_Find.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Find.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Image = Global.Textile.My.Resources.Resources.Close1
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(455, 133)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(83, 29)
        Me.btnClose.TabIndex = 5
        Me.btnClose.TabStop = False
        Me.btnClose.Text = "&Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'cbo_Find
        '
        Me.cbo_Find.BackColor = System.Drawing.Color.White
        Me.cbo_Find.DropDownHeight = 120
        Me.cbo_Find.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Find.FormattingEnabled = True
        Me.cbo_Find.IntegralHeight = False
        Me.cbo_Find.Location = New System.Drawing.Point(15, 22)
        Me.cbo_Find.Name = "cbo_Find"
        Me.cbo_Find.Size = New System.Drawing.Size(523, 23)
        Me.cbo_Find.TabIndex = 3
        '
        'grp_Filter
        '
        Me.grp_Filter.Controls.Add(Me.btn_Open)
        Me.grp_Filter.Controls.Add(Me.btn_CloseFilter)
        Me.grp_Filter.Controls.Add(Me.dgv_Filter)
        Me.grp_Filter.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Filter.Location = New System.Drawing.Point(679, 71)
        Me.grp_Filter.Name = "grp_Filter"
        Me.grp_Filter.Size = New System.Drawing.Size(557, 223)
        Me.grp_Filter.TabIndex = 4
        Me.grp_Filter.TabStop = False
        Me.grp_Filter.Text = "FILTER"
        '
        'btn_Open
        '
        Me.btn_Open.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Open.Image = CType(resources.GetObject("btn_Open.Image"), System.Drawing.Image)
        Me.btn_Open.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Open.Location = New System.Drawing.Point(359, 184)
        Me.btn_Open.Name = "btn_Open"
        Me.btn_Open.Size = New System.Drawing.Size(83, 29)
        Me.btn_Open.TabIndex = 35
        Me.btn_Open.TabStop = False
        Me.btn_Open.Text = "&Open"
        Me.btn_Open.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Open.UseVisualStyleBackColor = True
        '
        'btn_CloseFilter
        '
        Me.btn_CloseFilter.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_CloseFilter.Image = CType(resources.GetObject("btn_CloseFilter.Image"), System.Drawing.Image)
        Me.btn_CloseFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_CloseFilter.Location = New System.Drawing.Point(455, 184)
        Me.btn_CloseFilter.Name = "btn_CloseFilter"
        Me.btn_CloseFilter.Size = New System.Drawing.Size(83, 29)
        Me.btn_CloseFilter.TabIndex = 34
        Me.btn_CloseFilter.TabStop = False
        Me.btn_CloseFilter.Text = "&Close"
        Me.btn_CloseFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_CloseFilter.UseVisualStyleBackColor = True
        '
        'dgv_Filter
        '
        Me.dgv_Filter.AllowUserToAddRows = False
        Me.dgv_Filter.AllowUserToDeleteRows = False
        Me.dgv_Filter.AllowUserToResizeColumns = False
        Me.dgv_Filter.AllowUserToResizeRows = False
        Me.dgv_Filter.BackgroundColor = System.Drawing.Color.White
        Me.dgv_Filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_Filter.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv_Filter.Location = New System.Drawing.Point(15, 22)
        Me.dgv_Filter.MultiSelect = False
        Me.dgv_Filter.Name = "dgv_Filter"
        Me.dgv_Filter.ReadOnly = True
        Me.dgv_Filter.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_Filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Filter.Size = New System.Drawing.Size(523, 156)
        Me.dgv_Filter.TabIndex = 0
        '
        'pnl_Back
        '
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.Label9)
        Me.pnl_Back.Controls.Add(Me.Label6)
        Me.pnl_Back.Controls.Add(Me.btn_RateDetails)
        Me.pnl_Back.Controls.Add(Me.chk_Close_STS)
        Me.pnl_Back.Controls.Add(Me.cbo_Sizing_EndsCount)
        Me.pnl_Back.Controls.Add(Me.lbl_Sizing)
        Me.pnl_Back.Controls.Add(Me.Label23)
        Me.pnl_Back.Controls.Add(Me.Label8)
        Me.pnl_Back.Controls.Add(Me.txt_Meters)
        Me.pnl_Back.Controls.Add(Me.cbo_Transfer)
        Me.pnl_Back.Controls.Add(Me.lbl_TransferStockTo)
        Me.pnl_Back.Controls.Add(Me.cbo_EndsCountGroup)
        Me.pnl_Back.Controls.Add(Me.cbo_Cotton_Polyester_Jari)
        Me.pnl_Back.Controls.Add(Me.Label7)
        Me.pnl_Back.Controls.Add(Me.txt_MeterPcs)
        Me.pnl_Back.Controls.Add(Me.Label17)
        Me.pnl_Back.Controls.Add(Me.btn_Close)
        Me.pnl_Back.Controls.Add(Me.btn_Save)
        Me.pnl_Back.Controls.Add(Me.cbo_StockIn)
        Me.pnl_Back.Controls.Add(Me.lbl_IdNo)
        Me.pnl_Back.Controls.Add(Me.txt_Rate)
        Me.pnl_Back.Controls.Add(Me.lbl_Rate_Caption)
        Me.pnl_Back.Controls.Add(Me.cbo_Count)
        Me.pnl_Back.Controls.Add(Me.Label5)
        Me.pnl_Back.Controls.Add(Me.txt_Ends)
        Me.pnl_Back.Controls.Add(Me.Label4)
        Me.pnl_Back.Controls.Add(Me.txt_EndsCount)
        Me.pnl_Back.Controls.Add(Me.Label2)
        Me.pnl_Back.Controls.Add(Me.Label1)
        Me.pnl_Back.Controls.Add(Me.cbo_Single_Double_Triple)
        Me.pnl_Back.Location = New System.Drawing.Point(6, 42)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(575, 314)
        Me.pnl_Back.TabIndex = 5
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(14, 145)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(134, 28)
        Me.Label9.TabIndex = 346
        Me.Label9.Text = "Stock Maintenance in (PCS/METER)"
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(14, 111)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(134, 28)
        Me.Label6.TabIndex = 345
        Me.Label6.Text = "Stock Maintenance Under (Ends/Count Group)"
        '
        'btn_RateDetails
        '
        Me.btn_RateDetails.BackColor = System.Drawing.Color.Maroon
        Me.btn_RateDetails.ForeColor = System.Drawing.Color.White
        Me.btn_RateDetails.Location = New System.Drawing.Point(451, 218)
        Me.btn_RateDetails.Name = "btn_RateDetails"
        Me.btn_RateDetails.Size = New System.Drawing.Size(86, 28)
        Me.btn_RateDetails.TabIndex = 4
        Me.btn_RateDetails.Text = "Rate Details"
        Me.btn_RateDetails.UseVisualStyleBackColor = False
        Me.btn_RateDetails.Visible = False
        '
        'chk_Close_STS
        '
        Me.chk_Close_STS.Location = New System.Drawing.Point(355, 222)
        Me.chk_Close_STS.Name = "chk_Close_STS"
        Me.chk_Close_STS.Size = New System.Drawing.Size(99, 20)
        Me.chk_Close_STS.TabIndex = 344
        Me.chk_Close_STS.Text = "Close Status"
        Me.chk_Close_STS.UseVisualStyleBackColor = True
        '
        'cbo_Sizing_EndsCount
        '
        Me.cbo_Sizing_EndsCount.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Sizing_EndsCount.FormattingEnabled = True
        Me.cbo_Sizing_EndsCount.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cbo_Sizing_EndsCount.Location = New System.Drawing.Point(119, 221)
        Me.cbo_Sizing_EndsCount.Name = "cbo_Sizing_EndsCount"
        Me.cbo_Sizing_EndsCount.Size = New System.Drawing.Size(157, 23)
        Me.cbo_Sizing_EndsCount.TabIndex = 10
        Me.cbo_Sizing_EndsCount.Visible = False
        '
        'lbl_Sizing
        '
        Me.lbl_Sizing.AutoSize = True
        Me.lbl_Sizing.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbl_Sizing.Location = New System.Drawing.Point(14, 222)
        Me.lbl_Sizing.Name = "lbl_Sizing"
        Me.lbl_Sizing.Size = New System.Drawing.Size(98, 15)
        Me.lbl_Sizing.TabIndex = 343
        Me.lbl_Sizing.Text = "Sizing EndsCount"
        Me.lbl_Sizing.Visible = False
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.ForeColor = System.Drawing.Color.Red
        Me.Label23.Location = New System.Drawing.Point(242, 50)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(13, 15)
        Me.Label23.TabIndex = 300
        Me.Label23.Text = "*"
        Me.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label8.Location = New System.Drawing.Point(406, 50)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(33, 15)
        Me.Label8.TabIndex = 341
        Me.Label8.Text = "Mtrs"
        '
        'txt_Meters
        '
        Me.txt_Meters.BackColor = System.Drawing.Color.White
        Me.txt_Meters.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Meters.Location = New System.Drawing.Point(451, 47)
        Me.txt_Meters.MaxLength = 40
        Me.txt_Meters.Name = "txt_Meters"
        Me.txt_Meters.Size = New System.Drawing.Size(85, 23)
        Me.txt_Meters.TabIndex = 2
        '
        'cbo_Transfer
        '
        Me.cbo_Transfer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Transfer.FormattingEnabled = True
        Me.cbo_Transfer.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cbo_Transfer.Location = New System.Drawing.Point(355, 182)
        Me.cbo_Transfer.Name = "cbo_Transfer"
        Me.cbo_Transfer.Size = New System.Drawing.Size(182, 23)
        Me.cbo_Transfer.TabIndex = 9
        Me.cbo_Transfer.Visible = False
        '
        'lbl_TransferStockTo
        '
        Me.lbl_TransferStockTo.Location = New System.Drawing.Point(282, 182)
        Me.lbl_TransferStockTo.Name = "lbl_TransferStockTo"
        Me.lbl_TransferStockTo.Size = New System.Drawing.Size(69, 31)
        Me.lbl_TransferStockTo.TabIndex = 339
        Me.lbl_TransferStockTo.Text = "Transfer StockTo"
        Me.lbl_TransferStockTo.Visible = False
        '
        'cbo_EndsCountGroup
        '
        Me.cbo_EndsCountGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_EndsCountGroup.DropDownHeight = 125
        Me.cbo_EndsCountGroup.FormattingEnabled = True
        Me.cbo_EndsCountGroup.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cbo_EndsCountGroup.IntegralHeight = False
        Me.cbo_EndsCountGroup.Location = New System.Drawing.Point(159, 114)
        Me.cbo_EndsCountGroup.Name = "cbo_EndsCountGroup"
        Me.cbo_EndsCountGroup.Size = New System.Drawing.Size(378, 23)
        Me.cbo_EndsCountGroup.TabIndex = 5
        '
        'cbo_Cotton_Polyester_Jari
        '
        Me.cbo_Cotton_Polyester_Jari.BackColor = System.Drawing.Color.White
        Me.cbo_Cotton_Polyester_Jari.FormattingEnabled = True
        Me.cbo_Cotton_Polyester_Jari.Location = New System.Drawing.Point(119, 182)
        Me.cbo_Cotton_Polyester_Jari.MaxLength = 35
        Me.cbo_Cotton_Polyester_Jari.Name = "cbo_Cotton_Polyester_Jari"
        Me.cbo_Cotton_Polyester_Jari.Size = New System.Drawing.Size(154, 23)
        Me.cbo_Cotton_Polyester_Jari.TabIndex = 8
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(14, 180)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(94, 30)
        Me.Label7.TabIndex = 95
        Me.Label7.Text = "Cotton/ Polyester/ Jari"
        '
        'txt_MeterPcs
        '
        Me.txt_MeterPcs.Location = New System.Drawing.Point(355, 148)
        Me.txt_MeterPcs.MaxLength = 6
        Me.txt_MeterPcs.Name = "txt_MeterPcs"
        Me.txt_MeterPcs.Size = New System.Drawing.Size(181, 23)
        Me.txt_MeterPcs.TabIndex = 7
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(279, 152)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(70, 15)
        Me.Label17.TabIndex = 91
        Me.Label17.Text = "Meters/Pcs"
        '
        'btn_Close
        '
        Me.btn_Close.BackColor = System.Drawing.Color.DimGray
        Me.btn_Close.FlatAppearance.BorderColor = System.Drawing.Color.Blue
        Me.btn_Close.FlatAppearance.BorderSize = 2
        Me.btn_Close.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Yellow
        Me.btn_Close.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime
        Me.btn_Close.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Close.ForeColor = System.Drawing.Color.White
        Me.btn_Close.Location = New System.Drawing.Point(437, 260)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(100, 35)
        Me.btn_Close.TabIndex = 12
        Me.btn_Close.TabStop = False
        Me.btn_Close.Text = "&CLOSE"
        Me.btn_Close.UseVisualStyleBackColor = False
        '
        'btn_Save
        '
        Me.btn_Save.BackColor = System.Drawing.Color.DimGray
        Me.btn_Save.FlatAppearance.BorderSize = 2
        Me.btn_Save.FlatAppearance.MouseDownBackColor = System.Drawing.Color.YellowGreen
        Me.btn_Save.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime
        Me.btn_Save.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Save.ForeColor = System.Drawing.Color.White
        Me.btn_Save.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Save.Location = New System.Drawing.Point(300, 260)
        Me.btn_Save.Name = "btn_Save"
        Me.btn_Save.Size = New System.Drawing.Size(100, 35)
        Me.btn_Save.TabIndex = 11
        Me.btn_Save.TabStop = False
        Me.btn_Save.Text = "&SAVE"
        Me.btn_Save.UseVisualStyleBackColor = False
        '
        'cbo_StockIn
        '
        Me.cbo_StockIn.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_StockIn.FormattingEnabled = True
        Me.cbo_StockIn.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cbo_StockIn.Location = New System.Drawing.Point(159, 148)
        Me.cbo_StockIn.Name = "cbo_StockIn"
        Me.cbo_StockIn.Size = New System.Drawing.Size(114, 23)
        Me.cbo_StockIn.TabIndex = 6
        '
        'lbl_IdNo
        '
        Me.lbl_IdNo.BackColor = System.Drawing.Color.White
        Me.lbl_IdNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_IdNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_IdNo.Location = New System.Drawing.Point(119, 15)
        Me.lbl_IdNo.Name = "lbl_IdNo"
        Me.lbl_IdNo.Size = New System.Drawing.Size(418, 20)
        Me.lbl_IdNo.TabIndex = 12
        Me.lbl_IdNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txt_Rate
        '
        Me.txt_Rate.BackColor = System.Drawing.Color.White
        Me.txt_Rate.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Rate.Location = New System.Drawing.Point(355, 80)
        Me.txt_Rate.MaxLength = 40
        Me.txt_Rate.Name = "txt_Rate"
        Me.txt_Rate.Size = New System.Drawing.Size(181, 23)
        Me.txt_Rate.TabIndex = 4
        '
        'lbl_Rate_Caption
        '
        Me.lbl_Rate_Caption.AutoSize = True
        Me.lbl_Rate_Caption.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Rate_Caption.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbl_Rate_Caption.Location = New System.Drawing.Point(279, 84)
        Me.lbl_Rate_Caption.Name = "lbl_Rate_Caption"
        Me.lbl_Rate_Caption.Size = New System.Drawing.Size(59, 15)
        Me.lbl_Rate_Caption.TabIndex = 11
        Me.lbl_Rate_Caption.Text = "Rate/Mtr"
        '
        'cbo_Count
        '
        Me.cbo_Count.BackColor = System.Drawing.Color.White
        Me.cbo_Count.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Count.FormattingEnabled = True
        Me.cbo_Count.Location = New System.Drawing.Point(260, 46)
        Me.cbo_Count.Name = "cbo_Count"
        Me.cbo_Count.Size = New System.Drawing.Size(140, 23)
        Me.cbo_Count.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(206, 50)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(40, 15)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Count"
        '
        'txt_Ends
        '
        Me.txt_Ends.BackColor = System.Drawing.Color.White
        Me.txt_Ends.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Ends.Location = New System.Drawing.Point(119, 46)
        Me.txt_Ends.MaxLength = 40
        Me.txt_Ends.Name = "txt_Ends"
        Me.txt_Ends.Size = New System.Drawing.Size(85, 23)
        Me.txt_Ends.TabIndex = 0
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(14, 50)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(32, 15)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Ends"
        '
        'txt_EndsCount
        '
        Me.txt_EndsCount.BackColor = System.Drawing.SystemColors.Window
        Me.txt_EndsCount.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_EndsCount.Location = New System.Drawing.Point(119, 80)
        Me.txt_EndsCount.MaxLength = 40
        Me.txt_EndsCount.Name = "txt_EndsCount"
        Me.txt_EndsCount.ReadOnly = True
        Me.txt_EndsCount.Size = New System.Drawing.Size(150, 23)
        Me.txt_EndsCount.TabIndex = 3
        Me.txt_EndsCount.TabStop = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(14, 84)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(68, 15)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Ends Count"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(14, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(31, 15)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Idno"
        '
        'cbo_Single_Double_Triple
        '
        Me.cbo_Single_Double_Triple.BackColor = System.Drawing.Color.Yellow
        Me.cbo_Single_Double_Triple.FormattingEnabled = True
        Me.cbo_Single_Double_Triple.Location = New System.Drawing.Point(396, 80)
        Me.cbo_Single_Double_Triple.MaxLength = 35
        Me.cbo_Single_Double_Triple.Name = "cbo_Single_Double_Triple"
        Me.cbo_Single_Double_Triple.Size = New System.Drawing.Size(174, 23)
        Me.cbo_Single_Double_Triple.TabIndex = 4
        Me.cbo_Single_Double_Triple.Visible = False
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
        Me.Label3.Size = New System.Drawing.Size(595, 35)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "ENDS/COUNT  CREATION"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnl_RateDetails
        '
        Me.pnl_RateDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_RateDetails.Controls.Add(Me.btn_Close_rate)
        Me.pnl_RateDetails.Controls.Add(Me.ComboBox1)
        Me.pnl_RateDetails.Controls.Add(Me.Label31)
        Me.pnl_RateDetails.Controls.Add(Me.dgv_EndsCountRate_Details)
        Me.pnl_RateDetails.Location = New System.Drawing.Point(769, 341)
        Me.pnl_RateDetails.Name = "pnl_RateDetails"
        Me.pnl_RateDetails.Size = New System.Drawing.Size(373, 161)
        Me.pnl_RateDetails.TabIndex = 306
        Me.pnl_RateDetails.Visible = False
        '
        'btn_Close_rate
        '
        Me.btn_Close_rate.BackColor = System.Drawing.Color.Maroon
        Me.btn_Close_rate.Font = New System.Drawing.Font("Calibri", 10.25!, System.Drawing.FontStyle.Bold)
        Me.btn_Close_rate.ForeColor = System.Drawing.Color.White
        Me.btn_Close_rate.Location = New System.Drawing.Point(329, -1)
        Me.btn_Close_rate.Name = "btn_Close_rate"
        Me.btn_Close_rate.Size = New System.Drawing.Size(43, 25)
        Me.btn_Close_rate.TabIndex = 90
        Me.btn_Close_rate.Text = "X"
        Me.btn_Close_rate.UseVisualStyleBackColor = False
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownHeight = 120
        Me.ComboBox1.DropDownWidth = 124
        Me.ComboBox1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.IntegralHeight = False
        Me.ComboBox1.ItemHeight = 15
        Me.ComboBox1.Location = New System.Drawing.Point(445, 125)
        Me.ComboBox1.MaxDropDownItems = 15
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(167, 23)
        Me.ComboBox1.Sorted = True
        Me.ComboBox1.TabIndex = 35
        Me.ComboBox1.Visible = False
        '
        'Label31
        '
        Me.Label31.AutoEllipsis = True
        Me.Label31.BackColor = System.Drawing.Color.DarkCyan
        Me.Label31.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label31.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label31.Font = New System.Drawing.Font("Calibri", 13.75!, System.Drawing.FontStyle.Bold)
        Me.Label31.ForeColor = System.Drawing.Color.White
        Me.Label31.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.Label31.Location = New System.Drawing.Point(0, 0)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(371, 25)
        Me.Label31.TabIndex = 89
        Me.Label31.Text = "ENDS / COUNT RATE DETAILS"
        Me.Label31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dgv_EndsCountRate_Details
        '
        Me.dgv_EndsCountRate_Details.AllowUserToResizeColumns = False
        Me.dgv_EndsCountRate_Details.AllowUserToResizeRows = False
        Me.dgv_EndsCountRate_Details.BackgroundColor = System.Drawing.Color.Azure
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.DarkCyan
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_EndsCountRate_Details.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgv_EndsCountRate_Details.ColumnHeadersHeight = 35
        Me.dgv_EndsCountRate_Details.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgv_EndsCountRate_Details.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn2, Me.DataGridViewTextBoxColumn3, Me.Column1, Me.Column2})
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.Lime
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgv_EndsCountRate_Details.DefaultCellStyle = DataGridViewCellStyle6
        Me.dgv_EndsCountRate_Details.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.dgv_EndsCountRate_Details.EnableHeadersVisualStyles = False
        Me.dgv_EndsCountRate_Details.Location = New System.Drawing.Point(6, 29)
        Me.dgv_EndsCountRate_Details.Name = "dgv_EndsCountRate_Details"
        Me.dgv_EndsCountRate_Details.RowHeadersVisible = False
        Me.dgv_EndsCountRate_Details.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgv_EndsCountRate_Details.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_EndsCountRate_Details.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.dgv_EndsCountRate_Details.Size = New System.Drawing.Size(361, 118)
        Me.dgv_EndsCountRate_Details.TabIndex = 88
        '
        'DataGridViewTextBoxColumn2
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.NullValue = Nothing
        Me.DataGridViewTextBoxColumn2.DefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridViewTextBoxColumn2.HeaderText = "SNO"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn2.Width = 45
        '
        'DataGridViewTextBoxColumn3
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.Format = "N2"
        DataGridViewCellStyle3.NullValue = Nothing
        Me.DataGridViewTextBoxColumn3.DefaultCellStyle = DataGridViewCellStyle3
        Me.DataGridViewTextBoxColumn3.HeaderText = "FROM DATE"
        Me.DataGridViewTextBoxColumn3.MaxInputLength = 12
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn3.Width = 125
        '
        'Column1
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.Column1.DefaultCellStyle = DataGridViewCellStyle4
        Me.Column1.HeaderText = "TODATE"
        Me.Column1.MaxInputLength = 12
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Visible = False
        Me.Column1.Width = 125
        '
        'Column2
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle5
        Me.Column2.HeaderText = "RATE / Mtr"
        Me.Column2.MaxInputLength = 5
        Me.Column2.Name = "Column2"
        Me.Column2.Width = 125
        '
        'EndsCount_Creation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.PowderBlue
        Me.ClientSize = New System.Drawing.Size(595, 561)
        Me.Controls.Add(Me.pnl_RateDetails)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.pnl_Back)
        Me.Controls.Add(Me.grp_Filter)
        Me.Controls.Add(Me.grp_Find)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "EndsCount_Creation"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Tag = "600, 400"
        Me.Text = "ENDS/COUNT CREATION"
        Me.grp_Find.ResumeLayout(False)
        Me.grp_Filter.ResumeLayout(False)
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_Back.PerformLayout()
        Me.pnl_RateDetails.ResumeLayout(False)
        CType(Me.dgv_EndsCountRate_Details, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grp_Find As System.Windows.Forms.GroupBox
    Friend WithEvents cbo_Find As System.Windows.Forms.ComboBox
    Friend WithEvents btn_Find As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents grp_Filter As System.Windows.Forms.GroupBox
    Friend WithEvents dgv_Filter As System.Windows.Forms.DataGridView
    Friend WithEvents btn_Open As System.Windows.Forms.Button
    Friend WithEvents btn_CloseFilter As System.Windows.Forms.Button
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents btn_Save As System.Windows.Forms.Button
    Friend WithEvents txt_EndsCount As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txt_Ends As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cbo_Count As System.Windows.Forms.ComboBox
    Friend WithEvents txt_Rate As System.Windows.Forms.TextBox
    Friend WithEvents lbl_Rate_Caption As System.Windows.Forms.Label
    Friend WithEvents lbl_IdNo As System.Windows.Forms.Label
    Friend WithEvents txt_MeterPcs As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents cbo_StockIn As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_Cotton_Polyester_Jari As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents cbo_EndsCountGroup As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_Transfer As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_TransferStockTo As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txt_Meters As System.Windows.Forms.TextBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents cbo_Sizing_EndsCount As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_Sizing As System.Windows.Forms.Label
    Friend WithEvents chk_Close_STS As System.Windows.Forms.CheckBox
    Friend WithEvents pnl_RateDetails As System.Windows.Forms.Panel
    Friend WithEvents btn_Close_rate As System.Windows.Forms.Button
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents dgv_EndsCountRate_Details As System.Windows.Forms.DataGridView
    Friend WithEvents btn_RateDetails As System.Windows.Forms.Button
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cbo_Single_Double_Triple As ComboBox
    Friend WithEvents Label6 As Label
    Friend WithEvents Label9 As Label
End Class
