<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Grey_Item_Creation
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
        Me.txt_Width = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txt_Weight_Piece = New System.Windows.Forms.TextBox()
        Me.btn_CloseFilter = New System.Windows.Forms.Button()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txt_Meter_Qty = New System.Windows.Forms.TextBox()
        Me.grp_Open = New System.Windows.Forms.GroupBox()
        Me.btn_Find = New System.Windows.Forms.Button()
        Me.cbo_Open = New System.Windows.Forms.ComboBox()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.btn_close = New System.Windows.Forms.Button()
        Me.btn_save = New System.Windows.Forms.Button()
        Me.lbl_DisplaySlNo = New System.Windows.Forms.Label()
        Me.lbl_IdNo = New System.Windows.Forms.Label()
        Me.cbo_Grid_FinishedProduct = New System.Windows.Forms.ComboBox()
        Me.dgv_Details = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_MinimumStock = New System.Windows.Forms.TextBox()
        Me.dgv_Filter = New System.Windows.Forms.DataGridView()
        Me.grp_Filter = New System.Windows.Forms.GroupBox()
        Me.btn_OpenFilter = New System.Windows.Forms.Button()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txt_CostRate = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.grp_Back = New System.Windows.Forms.GroupBox()
        Me.chk_Verified_Status = New System.Windows.Forms.CheckBox()
        Me.cbo_LotNo = New System.Windows.Forms.ComboBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.txt_Code = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cbo_Unit = New System.Windows.Forms.ComboBox()
        Me.cbo_ItemGroup = New System.Windows.Forms.ComboBox()
        Me.txt_TaxRate = New System.Windows.Forms.TextBox()
        Me.txt_Rate = New System.Windows.Forms.TextBox()
        Me.txt_TaxPerc = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txt_Name = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.grp_Open.SuspendLayout()
        CType(Me.dgv_Details, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grp_Filter.SuspendLayout()
        Me.grp_Back.SuspendLayout()
        Me.SuspendLayout()
        '
        'txt_Width
        '
        Me.txt_Width.Location = New System.Drawing.Point(95, 228)
        Me.txt_Width.MaxLength = 6
        Me.txt_Width.Name = "txt_Width"
        Me.txt_Width.Size = New System.Drawing.Size(173, 23)
        Me.txt_Width.TabIndex = 7
        Me.txt_Width.Text = "txt_Width"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.ForeColor = System.Drawing.Color.Navy
        Me.Label12.Location = New System.Drawing.Point(20, 232)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(41, 15)
        Me.Label12.TabIndex = 16
        Me.Label12.Text = "Width"
        '
        'txt_Weight_Piece
        '
        Me.txt_Weight_Piece.Location = New System.Drawing.Point(430, 195)
        Me.txt_Weight_Piece.MaxLength = 8
        Me.txt_Weight_Piece.Name = "txt_Weight_Piece"
        Me.txt_Weight_Piece.Size = New System.Drawing.Size(173, 23)
        Me.txt_Weight_Piece.TabIndex = 6
        Me.txt_Weight_Piece.Text = "txt_Weight_Piece"
        '
        'btn_CloseFilter
        '
        Me.btn_CloseFilter.BackColor = System.Drawing.Color.Gray
        Me.btn_CloseFilter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btn_CloseFilter.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_CloseFilter.ForeColor = System.Drawing.Color.White
        Me.btn_CloseFilter.Location = New System.Drawing.Point(493, 356)
        Me.btn_CloseFilter.Name = "btn_CloseFilter"
        Me.btn_CloseFilter.Size = New System.Drawing.Size(106, 40)
        Me.btn_CloseFilter.TabIndex = 2
        Me.btn_CloseFilter.Text = "&CLOSE"
        Me.btn_CloseFilter.UseVisualStyleBackColor = False
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.ForeColor = System.Drawing.Color.Navy
        Me.Label13.Location = New System.Drawing.Point(322, 199)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(82, 15)
        Me.Label13.TabIndex = 15
        Me.Label13.Text = "Weight/Piece"
        '
        'txt_Meter_Qty
        '
        Me.txt_Meter_Qty.Location = New System.Drawing.Point(95, 195)
        Me.txt_Meter_Qty.MaxLength = 6
        Me.txt_Meter_Qty.Name = "txt_Meter_Qty"
        Me.txt_Meter_Qty.Size = New System.Drawing.Size(173, 23)
        Me.txt_Meter_Qty.TabIndex = 5
        Me.txt_Meter_Qty.Text = "txt_Meter_Qty"
        '
        'grp_Open
        '
        Me.grp_Open.BackColor = System.Drawing.Color.Transparent
        Me.grp_Open.Controls.Add(Me.btn_Find)
        Me.grp_Open.Controls.Add(Me.cbo_Open)
        Me.grp_Open.Controls.Add(Me.btnClose)
        Me.grp_Open.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Open.ForeColor = System.Drawing.Color.Black
        Me.grp_Open.Location = New System.Drawing.Point(652, 501)
        Me.grp_Open.Name = "grp_Open"
        Me.grp_Open.Size = New System.Drawing.Size(601, 285)
        Me.grp_Open.TabIndex = 35
        Me.grp_Open.TabStop = False
        Me.grp_Open.Text = "Finding"
        '
        'btn_Find
        '
        Me.btn_Find.BackColor = System.Drawing.Color.Gray
        Me.btn_Find.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btn_Find.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Find.ForeColor = System.Drawing.Color.White
        Me.btn_Find.Location = New System.Drawing.Point(387, 227)
        Me.btn_Find.Name = "btn_Find"
        Me.btn_Find.Size = New System.Drawing.Size(87, 40)
        Me.btn_Find.TabIndex = 31
        Me.btn_Find.Text = "&Find"
        Me.btn_Find.UseVisualStyleBackColor = False
        '
        'cbo_Open
        '
        Me.cbo_Open.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Open.DropDownHeight = 90
        Me.cbo_Open.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Open.ForeColor = System.Drawing.Color.Black
        Me.cbo_Open.FormattingEnabled = True
        Me.cbo_Open.IntegralHeight = False
        Me.cbo_Open.Location = New System.Drawing.Point(22, 37)
        Me.cbo_Open.Name = "cbo_Open"
        Me.cbo_Open.Size = New System.Drawing.Size(564, 23)
        Me.cbo_Open.Sorted = True
        Me.cbo_Open.TabIndex = 0
        Me.cbo_Open.Text = "cbo_Open"
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.Color.Gray
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnClose.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ForeColor = System.Drawing.Color.White
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(499, 227)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(87, 40)
        Me.btnClose.TabIndex = 30
        Me.btnClose.Text = "&Close"
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.ForeColor = System.Drawing.Color.Navy
        Me.Label14.Location = New System.Drawing.Point(20, 199)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(68, 15)
        Me.Label14.TabIndex = 11
        Me.Label14.Text = "Meter/Qty"
        '
        'Label7
        '
        Me.Label7.AutoEllipsis = True
        Me.Label7.BackColor = System.Drawing.Color.Gray
        Me.Label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label7.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.Label7.Location = New System.Drawing.Point(-264, -138)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(695, 44)
        Me.Label7.TabIndex = 34
        Me.Label7.Text = "FINISHED PRODUCT CREATION"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.DimGray
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(518, 471)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(87, 35)
        Me.btn_close.TabIndex = 16
        Me.btn_close.TabStop = False
        Me.btn_close.Text = "&CLOSE"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'btn_save
        '
        Me.btn_save.BackColor = System.Drawing.Color.DimGray
        Me.btn_save.ForeColor = System.Drawing.Color.White
        Me.btn_save.Location = New System.Drawing.Point(417, 471)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(87, 35)
        Me.btn_save.TabIndex = 15
        Me.btn_save.TabStop = False
        Me.btn_save.Text = "&SAVE"
        Me.btn_save.UseVisualStyleBackColor = False
        '
        'lbl_DisplaySlNo
        '
        Me.lbl_DisplaySlNo.BackColor = System.Drawing.Color.White
        Me.lbl_DisplaySlNo.ForeColor = System.Drawing.Color.Black
        Me.lbl_DisplaySlNo.Location = New System.Drawing.Point(95, 30)
        Me.lbl_DisplaySlNo.Name = "lbl_DisplaySlNo"
        Me.lbl_DisplaySlNo.Size = New System.Drawing.Size(508, 23)
        Me.lbl_DisplaySlNo.TabIndex = 41
        Me.lbl_DisplaySlNo.Text = "lbl_DisplaySlNo"
        Me.lbl_DisplaySlNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbl_IdNo
        '
        Me.lbl_IdNo.BackColor = System.Drawing.Color.Red
        Me.lbl_IdNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_IdNo.Location = New System.Drawing.Point(159, 477)
        Me.lbl_IdNo.Name = "lbl_IdNo"
        Me.lbl_IdNo.Size = New System.Drawing.Size(111, 23)
        Me.lbl_IdNo.TabIndex = 40
        Me.lbl_IdNo.Text = "lbl_IdNo"
        Me.lbl_IdNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lbl_IdNo.Visible = False
        '
        'cbo_Grid_FinishedProduct
        '
        Me.cbo_Grid_FinishedProduct.DropDownHeight = 90
        Me.cbo_Grid_FinishedProduct.DropDownWidth = 124
        Me.cbo_Grid_FinishedProduct.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Grid_FinishedProduct.FormattingEnabled = True
        Me.cbo_Grid_FinishedProduct.IntegralHeight = False
        Me.cbo_Grid_FinishedProduct.ItemHeight = 15
        Me.cbo_Grid_FinishedProduct.Location = New System.Drawing.Point(95, 407)
        Me.cbo_Grid_FinishedProduct.MaxDropDownItems = 20
        Me.cbo_Grid_FinishedProduct.Name = "cbo_Grid_FinishedProduct"
        Me.cbo_Grid_FinishedProduct.Size = New System.Drawing.Size(175, 23)
        Me.cbo_Grid_FinishedProduct.Sorted = True
        Me.cbo_Grid_FinishedProduct.TabIndex = 13
        Me.cbo_Grid_FinishedProduct.Text = "cbo_Grid_FinishedProduct"
        '
        'dgv_Details
        '
        Me.dgv_Details.AllowUserToAddRows = False
        Me.dgv_Details.AllowUserToResizeColumns = False
        Me.dgv_Details.AllowUserToResizeRows = False
        Me.dgv_Details.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.DimGray
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.DimGray
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_Details.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgv_Details.ColumnHeadersHeight = 35
        Me.dgv_Details.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgv_Details.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3})
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.Color.Navy
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgv_Details.DefaultCellStyle = DataGridViewCellStyle5
        Me.dgv_Details.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv_Details.EnableHeadersVisualStyles = False
        Me.dgv_Details.Location = New System.Drawing.Point(20, 327)
        Me.dgv_Details.Name = "dgv_Details"
        Me.dgv_Details.ReadOnly = True
        Me.dgv_Details.RowHeadersVisible = False
        Me.dgv_Details.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgv_Details.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_Details.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.dgv_Details.Size = New System.Drawing.Size(583, 134)
        Me.dgv_Details.TabIndex = 14
        '
        'Column1
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column1.DefaultCellStyle = DataGridViewCellStyle2
        Me.Column1.HeaderText = "S.NO"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column1.Width = 35
        '
        'Column2
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.Format = "N2"
        DataGridViewCellStyle3.NullValue = Nothing
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle3
        Me.Column2.HeaderText = "FINISHED PRODUCT NAME"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column2.Width = 310
        '
        'Column3
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.Format = "N2"
        DataGridViewCellStyle4.NullValue = Nothing
        Me.Column3.DefaultCellStyle = DataGridViewCellStyle4
        Me.Column3.HeaderText = "ITEM GROUP"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        Me.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column3.Width = 220
        '
        'txt_MinimumStock
        '
        Me.txt_MinimumStock.Location = New System.Drawing.Point(430, 228)
        Me.txt_MinimumStock.MaxLength = 12
        Me.txt_MinimumStock.Name = "txt_MinimumStock"
        Me.txt_MinimumStock.Size = New System.Drawing.Size(173, 23)
        Me.txt_MinimumStock.TabIndex = 8
        Me.txt_MinimumStock.Text = "txt_MinimumStock"
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
        Me.dgv_Filter.Location = New System.Drawing.Point(12, 25)
        Me.dgv_Filter.Name = "dgv_Filter"
        Me.dgv_Filter.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_Filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Filter.Size = New System.Drawing.Size(587, 320)
        Me.dgv_Filter.TabIndex = 0
        '
        'grp_Filter
        '
        Me.grp_Filter.Controls.Add(Me.dgv_Filter)
        Me.grp_Filter.Controls.Add(Me.btn_CloseFilter)
        Me.grp_Filter.Controls.Add(Me.btn_OpenFilter)
        Me.grp_Filter.Location = New System.Drawing.Point(652, 74)
        Me.grp_Filter.Name = "grp_Filter"
        Me.grp_Filter.Size = New System.Drawing.Size(618, 418)
        Me.grp_Filter.TabIndex = 36
        Me.grp_Filter.TabStop = False
        '
        'btn_OpenFilter
        '
        Me.btn_OpenFilter.BackColor = System.Drawing.Color.Gray
        Me.btn_OpenFilter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btn_OpenFilter.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_OpenFilter.ForeColor = System.Drawing.Color.White
        Me.btn_OpenFilter.Location = New System.Drawing.Point(360, 356)
        Me.btn_OpenFilter.Name = "btn_OpenFilter"
        Me.btn_OpenFilter.Size = New System.Drawing.Size(106, 40)
        Me.btn_OpenFilter.TabIndex = 1
        Me.btn_OpenFilter.Text = "&OPEN"
        Me.btn_OpenFilter.UseVisualStyleBackColor = False
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.ForeColor = System.Drawing.Color.Navy
        Me.Label11.Location = New System.Drawing.Point(322, 232)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(95, 15)
        Me.Label11.TabIndex = 10
        Me.Label11.Text = "Minimum Stock "
        '
        'txt_CostRate
        '
        Me.txt_CostRate.Location = New System.Drawing.Point(430, 261)
        Me.txt_CostRate.MaxLength = 12
        Me.txt_CostRate.Name = "txt_CostRate"
        Me.txt_CostRate.Size = New System.Drawing.Size(173, 23)
        Me.txt_CostRate.TabIndex = 10
        Me.txt_CostRate.Text = "txt_CostRate"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.ForeColor = System.Drawing.Color.Navy
        Me.Label10.Location = New System.Drawing.Point(322, 264)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(62, 15)
        Me.Label10.TabIndex = 8
        Me.Label10.Text = "Cost Rate "
        '
        'grp_Back
        '
        Me.grp_Back.BackColor = System.Drawing.Color.Transparent
        Me.grp_Back.Controls.Add(Me.chk_Verified_Status)
        Me.grp_Back.Controls.Add(Me.cbo_LotNo)
        Me.grp_Back.Controls.Add(Me.Label15)
        Me.grp_Back.Controls.Add(Me.btn_close)
        Me.grp_Back.Controls.Add(Me.lbl_DisplaySlNo)
        Me.grp_Back.Controls.Add(Me.btn_save)
        Me.grp_Back.Controls.Add(Me.lbl_IdNo)
        Me.grp_Back.Controls.Add(Me.cbo_Grid_FinishedProduct)
        Me.grp_Back.Controls.Add(Me.dgv_Details)
        Me.grp_Back.Controls.Add(Me.txt_Width)
        Me.grp_Back.Controls.Add(Me.Label12)
        Me.grp_Back.Controls.Add(Me.txt_Weight_Piece)
        Me.grp_Back.Controls.Add(Me.Label13)
        Me.grp_Back.Controls.Add(Me.txt_Meter_Qty)
        Me.grp_Back.Controls.Add(Me.Label14)
        Me.grp_Back.Controls.Add(Me.txt_MinimumStock)
        Me.grp_Back.Controls.Add(Me.Label11)
        Me.grp_Back.Controls.Add(Me.txt_CostRate)
        Me.grp_Back.Controls.Add(Me.Label10)
        Me.grp_Back.Controls.Add(Me.txt_Code)
        Me.grp_Back.Controls.Add(Me.Label9)
        Me.grp_Back.Controls.Add(Me.cbo_Unit)
        Me.grp_Back.Controls.Add(Me.cbo_ItemGroup)
        Me.grp_Back.Controls.Add(Me.txt_TaxRate)
        Me.grp_Back.Controls.Add(Me.txt_Rate)
        Me.grp_Back.Controls.Add(Me.txt_TaxPerc)
        Me.grp_Back.Controls.Add(Me.Label8)
        Me.grp_Back.Controls.Add(Me.txt_Name)
        Me.grp_Back.Controls.Add(Me.Label6)
        Me.grp_Back.Controls.Add(Me.Label4)
        Me.grp_Back.Controls.Add(Me.Label5)
        Me.grp_Back.Controls.Add(Me.Label3)
        Me.grp_Back.Controls.Add(Me.Label2)
        Me.grp_Back.Controls.Add(Me.Label1)
        Me.grp_Back.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Back.ForeColor = System.Drawing.Color.Navy
        Me.grp_Back.Location = New System.Drawing.Point(6, 36)
        Me.grp_Back.Name = "grp_Back"
        Me.grp_Back.Size = New System.Drawing.Size(630, 525)
        Me.grp_Back.TabIndex = 33
        Me.grp_Back.TabStop = False
        '
        'chk_Verified_Status
        '
        Me.chk_Verified_Status.AutoSize = True
        Me.chk_Verified_Status.BackColor = System.Drawing.Color.White
        Me.chk_Verified_Status.CausesValidation = False
        Me.chk_Verified_Status.Location = New System.Drawing.Point(305, 480)
        Me.chk_Verified_Status.Name = "chk_Verified_Status"
        Me.chk_Verified_Status.Size = New System.Drawing.Size(106, 19)
        Me.chk_Verified_Status.TabIndex = 44
        Me.chk_Verified_Status.Text = "Verified Status"
        Me.chk_Verified_Status.UseVisualStyleBackColor = False
        '
        'cbo_LotNo
        '
        Me.cbo_LotNo.FormattingEnabled = True
        Me.cbo_LotNo.Location = New System.Drawing.Point(95, 162)
        Me.cbo_LotNo.Name = "cbo_LotNo"
        Me.cbo_LotNo.Size = New System.Drawing.Size(173, 23)
        Me.cbo_LotNo.TabIndex = 3
        Me.cbo_LotNo.Text = "cbo_LotNo"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.ForeColor = System.Drawing.Color.Navy
        Me.Label15.Location = New System.Drawing.Point(20, 133)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(35, 15)
        Me.Label15.TabIndex = 43
        Me.Label15.Text = "Code"
        '
        'txt_Code
        '
        Me.txt_Code.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_Code.Location = New System.Drawing.Point(95, 129)
        Me.txt_Code.MaxLength = 20
        Me.txt_Code.Name = "txt_Code"
        Me.txt_Code.Size = New System.Drawing.Size(508, 23)
        Me.txt_Code.TabIndex = 2
        Me.txt_Code.Text = "TXT_CODE"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.ForeColor = System.Drawing.Color.Navy
        Me.Label9.Location = New System.Drawing.Point(20, 166)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(44, 15)
        Me.Label9.TabIndex = 7
        Me.Label9.Text = "Lot No"
        '
        'cbo_Unit
        '
        Me.cbo_Unit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Unit.FormattingEnabled = True
        Me.cbo_Unit.Location = New System.Drawing.Point(430, 162)
        Me.cbo_Unit.Name = "cbo_Unit"
        Me.cbo_Unit.Size = New System.Drawing.Size(173, 23)
        Me.cbo_Unit.TabIndex = 4
        Me.cbo_Unit.Text = "cbo_Unit"
        '
        'cbo_ItemGroup
        '
        Me.cbo_ItemGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_ItemGroup.ForeColor = System.Drawing.Color.Black
        Me.cbo_ItemGroup.FormattingEnabled = True
        Me.cbo_ItemGroup.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cbo_ItemGroup.Location = New System.Drawing.Point(95, 96)
        Me.cbo_ItemGroup.Name = "cbo_ItemGroup"
        Me.cbo_ItemGroup.Size = New System.Drawing.Size(508, 23)
        Me.cbo_ItemGroup.TabIndex = 1
        Me.cbo_ItemGroup.Text = "cbo_ItemGroup"
        '
        'txt_TaxRate
        '
        Me.txt_TaxRate.Location = New System.Drawing.Point(430, 294)
        Me.txt_TaxRate.MaxLength = 12
        Me.txt_TaxRate.Name = "txt_TaxRate"
        Me.txt_TaxRate.Size = New System.Drawing.Size(173, 23)
        Me.txt_TaxRate.TabIndex = 12
        Me.txt_TaxRate.Text = "txt_TaxRate"
        '
        'txt_Rate
        '
        Me.txt_Rate.Location = New System.Drawing.Point(95, 294)
        Me.txt_Rate.MaxLength = 12
        Me.txt_Rate.Name = "txt_Rate"
        Me.txt_Rate.Size = New System.Drawing.Size(173, 23)
        Me.txt_Rate.TabIndex = 11
        Me.txt_Rate.Text = "txt_Rate"
        '
        'txt_TaxPerc
        '
        Me.txt_TaxPerc.Location = New System.Drawing.Point(95, 261)
        Me.txt_TaxPerc.MaxLength = 6
        Me.txt_TaxPerc.Name = "txt_TaxPerc"
        Me.txt_TaxPerc.Size = New System.Drawing.Size(173, 23)
        Me.txt_TaxPerc.TabIndex = 9
        Me.txt_TaxPerc.Text = "txt_TaxPerc"
        '
        'Label8
        '
        Me.Label8.ForeColor = System.Drawing.Color.Navy
        Me.Label8.Location = New System.Drawing.Point(322, 294)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(97, 36)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Sales Rate  (Incl.Tax)"
        '
        'txt_Name
        '
        Me.txt_Name.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_Name.ForeColor = System.Drawing.Color.Black
        Me.txt_Name.Location = New System.Drawing.Point(95, 63)
        Me.txt_Name.MaxLength = 50
        Me.txt_Name.Name = "txt_Name"
        Me.txt_Name.Size = New System.Drawing.Size(508, 23)
        Me.txt_Name.TabIndex = 0
        Me.txt_Name.Text = "TXT_NAME"
        '
        'Label6
        '
        Me.Label6.ForeColor = System.Drawing.Color.Navy
        Me.Label6.Location = New System.Drawing.Point(20, 294)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(105, 36)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Sales Rate  (Excl.Tax)"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.Navy
        Me.Label4.Location = New System.Drawing.Point(322, 166)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(33, 15)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Unit "
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.Navy
        Me.Label5.Location = New System.Drawing.Point(20, 264)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(36, 15)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Tax %"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.Color.Navy
        Me.Label3.Location = New System.Drawing.Point(20, 100)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(70, 15)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Item Group"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.Navy
        Me.Label2.Location = New System.Drawing.Point(20, 67)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(40, 15)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Name"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Navy
        Me.Label1.Location = New System.Drawing.Point(20, 34)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(33, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "IdNo"
        '
        'Label16
        '
        Me.Label16.AutoEllipsis = True
        Me.Label16.BackColor = System.Drawing.Color.DimGray
        Me.Label16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label16.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label16.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.White
        Me.Label16.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.Label16.Location = New System.Drawing.Point(0, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(649, 35)
        Me.Label16.TabIndex = 37
        Me.Label16.Text = "GREY ITEM CREATION"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Grey_Item_Creation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.PowderBlue
        Me.ClientSize = New System.Drawing.Size(649, 577)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.grp_Open)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.grp_Filter)
        Me.Controls.Add(Me.grp_Back)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Grey_Item_Creation"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "GRY ITEM CREATION"
        Me.grp_Open.ResumeLayout(False)
        CType(Me.dgv_Details, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grp_Filter.ResumeLayout(False)
        Me.grp_Back.ResumeLayout(False)
        Me.grp_Back.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txt_Width As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txt_Weight_Piece As System.Windows.Forms.TextBox
    Friend WithEvents btn_CloseFilter As System.Windows.Forms.Button
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txt_Meter_Qty As System.Windows.Forms.TextBox
    Friend WithEvents grp_Open As System.Windows.Forms.GroupBox
    Friend WithEvents btn_Find As System.Windows.Forms.Button
    Friend WithEvents cbo_Open As System.Windows.Forms.ComboBox
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btn_close As System.Windows.Forms.Button
    Friend WithEvents btn_save As System.Windows.Forms.Button
    Friend WithEvents lbl_DisplaySlNo As System.Windows.Forms.Label
    Friend WithEvents lbl_IdNo As System.Windows.Forms.Label
    Friend WithEvents cbo_Grid_FinishedProduct As System.Windows.Forms.ComboBox
    Friend WithEvents dgv_Details As System.Windows.Forms.DataGridView
    Friend WithEvents txt_MinimumStock As System.Windows.Forms.TextBox
    Friend WithEvents dgv_Filter As System.Windows.Forms.DataGridView
    Friend WithEvents grp_Filter As System.Windows.Forms.GroupBox
    Friend WithEvents btn_OpenFilter As System.Windows.Forms.Button
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txt_CostRate As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents grp_Back As System.Windows.Forms.GroupBox
    Friend WithEvents cbo_LotNo As System.Windows.Forms.ComboBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txt_Code As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents cbo_Unit As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_ItemGroup As System.Windows.Forms.ComboBox
    Friend WithEvents txt_TaxRate As System.Windows.Forms.TextBox
    Friend WithEvents txt_Rate As System.Windows.Forms.TextBox
    Friend WithEvents txt_TaxPerc As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txt_Name As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents chk_Verified_Status As System.Windows.Forms.CheckBox
End Class
