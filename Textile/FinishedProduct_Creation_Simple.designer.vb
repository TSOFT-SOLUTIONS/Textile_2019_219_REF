<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FinishedProduct_Creation_Simple
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FinishedProduct_Creation_Simple))
        Me.txt_Width = New System.Windows.Forms.TextBox()
        Me.cbo_Open = New System.Windows.Forms.ComboBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.txt_Weight_Piece = New System.Windows.Forms.TextBox()
        Me.btn_Find = New System.Windows.Forms.Button()
        Me.dgv_Filter = New System.Windows.Forms.DataGridView()
        Me.btn_CloseFilter = New System.Windows.Forms.Button()
        Me.grp_Filter = New System.Windows.Forms.GroupBox()
        Me.btn_OpenFilter = New System.Windows.Forms.Button()
        Me.grp_Open = New System.Windows.Forms.GroupBox()
        Me.chk_Verified_Status = New System.Windows.Forms.CheckBox()
        Me.btn_close = New System.Windows.Forms.Button()
        Me.btn_save = New System.Windows.Forms.Button()
        Me.lbl_DisplaySlNo = New System.Windows.Forms.Label()
        Me.grp_Back = New System.Windows.Forms.GroupBox()
        Me.cbo_Reconsilation_Meter_Weight = New System.Windows.Forms.ComboBox()
        Me.lbl_Reconsilation_Meter_Weight = New System.Windows.Forms.Label()
        Me.lbl_IdNo = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txt_Meter_Qty = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.txt_MinimumStock = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txt_CostRate = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
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
        Me.Label7 = New System.Windows.Forms.Label()
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grp_Filter.SuspendLayout()
        Me.grp_Open.SuspendLayout()
        Me.grp_Back.SuspendLayout()
        Me.SuspendLayout()
        '
        'txt_Width
        '
        Me.txt_Width.Location = New System.Drawing.Point(134, 196)
        Me.txt_Width.MaxLength = 6
        Me.txt_Width.Name = "txt_Width"
        Me.txt_Width.Size = New System.Drawing.Size(133, 23)
        Me.txt_Width.TabIndex = 6
        Me.txt_Width.Text = "txt_Width"
        '
        'cbo_Open
        '
        Me.cbo_Open.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Open.DropDownHeight = 90
        Me.cbo_Open.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Open.FormattingEnabled = True
        Me.cbo_Open.IntegralHeight = False
        Me.cbo_Open.Location = New System.Drawing.Point(19, 32)
        Me.cbo_Open.Name = "cbo_Open"
        Me.cbo_Open.Size = New System.Drawing.Size(493, 23)
        Me.cbo_Open.Sorted = True
        Me.cbo_Open.TabIndex = 0
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(23, 200)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(41, 15)
        Me.Label12.TabIndex = 16
        Me.Label12.Text = "Width"
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.Color.DarkCyan
        Me.btnClose.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ForeColor = System.Drawing.Color.White
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(428, 197)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(83, 29)
        Me.btnClose.TabIndex = 30
        Me.btnClose.Text = "&Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'txt_Weight_Piece
        '
        Me.txt_Weight_Piece.Location = New System.Drawing.Point(415, 162)
        Me.txt_Weight_Piece.MaxLength = 8
        Me.txt_Weight_Piece.Name = "txt_Weight_Piece"
        Me.txt_Weight_Piece.Size = New System.Drawing.Size(141, 23)
        Me.txt_Weight_Piece.TabIndex = 5
        Me.txt_Weight_Piece.Text = "txt_Weight_Piece"
        '
        'btn_Find
        '
        Me.btn_Find.BackColor = System.Drawing.Color.DarkCyan
        Me.btn_Find.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Find.ForeColor = System.Drawing.Color.White
        Me.btn_Find.Image = CType(resources.GetObject("btn_Find.Image"), System.Drawing.Image)
        Me.btn_Find.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Find.Location = New System.Drawing.Point(332, 197)
        Me.btn_Find.Name = "btn_Find"
        Me.btn_Find.Size = New System.Drawing.Size(83, 29)
        Me.btn_Find.TabIndex = 31
        Me.btn_Find.Text = "&Find"
        Me.btn_Find.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Find.UseVisualStyleBackColor = False
        '
        'dgv_Filter
        '
        Me.dgv_Filter.AllowUserToAddRows = False
        Me.dgv_Filter.AllowUserToDeleteRows = False
        Me.dgv_Filter.AllowUserToResizeColumns = False
        Me.dgv_Filter.AllowUserToResizeRows = False
        Me.dgv_Filter.BackgroundColor = System.Drawing.Color.White
        Me.dgv_Filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_Filter.Dock = System.Windows.Forms.DockStyle.Top
        Me.dgv_Filter.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv_Filter.Location = New System.Drawing.Point(3, 19)
        Me.dgv_Filter.Name = "dgv_Filter"
        Me.dgv_Filter.ReadOnly = True
        Me.dgv_Filter.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_Filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Filter.Size = New System.Drawing.Size(543, 296)
        Me.dgv_Filter.TabIndex = 0
        '
        'btn_CloseFilter
        '
        Me.btn_CloseFilter.BackColor = System.Drawing.Color.DarkCyan
        Me.btn_CloseFilter.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_CloseFilter.ForeColor = System.Drawing.Color.White
        Me.btn_CloseFilter.Location = New System.Drawing.Point(458, 323)
        Me.btn_CloseFilter.Name = "btn_CloseFilter"
        Me.btn_CloseFilter.Size = New System.Drawing.Size(77, 32)
        Me.btn_CloseFilter.TabIndex = 2
        Me.btn_CloseFilter.Text = "&CLOSE"
        Me.btn_CloseFilter.UseVisualStyleBackColor = False
        '
        'grp_Filter
        '
        Me.grp_Filter.Controls.Add(Me.dgv_Filter)
        Me.grp_Filter.Controls.Add(Me.btn_CloseFilter)
        Me.grp_Filter.Controls.Add(Me.btn_OpenFilter)
        Me.grp_Filter.Location = New System.Drawing.Point(646, 102)
        Me.grp_Filter.Name = "grp_Filter"
        Me.grp_Filter.Size = New System.Drawing.Size(549, 362)
        Me.grp_Filter.TabIndex = 38
        Me.grp_Filter.TabStop = False
        Me.grp_Filter.Text = "Filter"
        '
        'btn_OpenFilter
        '
        Me.btn_OpenFilter.BackColor = System.Drawing.Color.DarkCyan
        Me.btn_OpenFilter.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_OpenFilter.ForeColor = System.Drawing.Color.White
        Me.btn_OpenFilter.Location = New System.Drawing.Point(363, 323)
        Me.btn_OpenFilter.Name = "btn_OpenFilter"
        Me.btn_OpenFilter.Size = New System.Drawing.Size(77, 31)
        Me.btn_OpenFilter.TabIndex = 1
        Me.btn_OpenFilter.Text = "&OPEN"
        Me.btn_OpenFilter.UseVisualStyleBackColor = False
        '
        'grp_Open
        '
        Me.grp_Open.BackColor = System.Drawing.Color.Transparent
        Me.grp_Open.Controls.Add(Me.btn_Find)
        Me.grp_Open.Controls.Add(Me.cbo_Open)
        Me.grp_Open.Controls.Add(Me.btnClose)
        Me.grp_Open.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Open.Location = New System.Drawing.Point(314, 544)
        Me.grp_Open.Name = "grp_Open"
        Me.grp_Open.Size = New System.Drawing.Size(539, 247)
        Me.grp_Open.TabIndex = 37
        Me.grp_Open.TabStop = False
        Me.grp_Open.Text = "Finding"
        '
        'chk_Verified_Status
        '
        Me.chk_Verified_Status.AutoSize = True
        Me.chk_Verified_Status.BackColor = System.Drawing.Color.White
        Me.chk_Verified_Status.Location = New System.Drawing.Point(265, 356)
        Me.chk_Verified_Status.Name = "chk_Verified_Status"
        Me.chk_Verified_Status.Size = New System.Drawing.Size(106, 19)
        Me.chk_Verified_Status.TabIndex = 35
        Me.chk_Verified_Status.Text = "Verified Status"
        Me.chk_Verified_Status.UseVisualStyleBackColor = False
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.DarkCyan
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(479, 343)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(77, 32)
        Me.btn_close.TabIndex = 16
        Me.btn_close.TabStop = False
        Me.btn_close.Text = "&CLOSE"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'btn_save
        '
        Me.btn_save.BackColor = System.Drawing.Color.DarkCyan
        Me.btn_save.ForeColor = System.Drawing.Color.White
        Me.btn_save.Location = New System.Drawing.Point(378, 343)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(77, 32)
        Me.btn_save.TabIndex = 15
        Me.btn_save.TabStop = False
        Me.btn_save.Text = "&SAVE"
        Me.btn_save.UseVisualStyleBackColor = False
        '
        'lbl_DisplaySlNo
        '
        Me.lbl_DisplaySlNo.BackColor = System.Drawing.Color.White
        Me.lbl_DisplaySlNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_DisplaySlNo.Location = New System.Drawing.Point(134, 26)
        Me.lbl_DisplaySlNo.Name = "lbl_DisplaySlNo"
        Me.lbl_DisplaySlNo.Size = New System.Drawing.Size(422, 23)
        Me.lbl_DisplaySlNo.TabIndex = 41
        Me.lbl_DisplaySlNo.Text = "lbl_DisplaySlNo"
        Me.lbl_DisplaySlNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'grp_Back
        '
        Me.grp_Back.BackColor = System.Drawing.Color.Transparent
        Me.grp_Back.Controls.Add(Me.cbo_Reconsilation_Meter_Weight)
        Me.grp_Back.Controls.Add(Me.lbl_Reconsilation_Meter_Weight)
        Me.grp_Back.Controls.Add(Me.chk_Verified_Status)
        Me.grp_Back.Controls.Add(Me.btn_close)
        Me.grp_Back.Controls.Add(Me.btn_save)
        Me.grp_Back.Controls.Add(Me.lbl_DisplaySlNo)
        Me.grp_Back.Controls.Add(Me.lbl_IdNo)
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
        Me.grp_Back.Location = New System.Drawing.Point(12, 38)
        Me.grp_Back.Name = "grp_Back"
        Me.grp_Back.Size = New System.Drawing.Size(593, 418)
        Me.grp_Back.TabIndex = 35
        Me.grp_Back.TabStop = False
        '
        'cbo_Reconsilation_Meter_Weight
        '
        Me.cbo_Reconsilation_Meter_Weight.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Reconsilation_Meter_Weight.FormattingEnabled = True
        Me.cbo_Reconsilation_Meter_Weight.Location = New System.Drawing.Point(134, 309)
        Me.cbo_Reconsilation_Meter_Weight.Name = "cbo_Reconsilation_Meter_Weight"
        Me.cbo_Reconsilation_Meter_Weight.Size = New System.Drawing.Size(133, 23)
        Me.cbo_Reconsilation_Meter_Weight.TabIndex = 50
        Me.cbo_Reconsilation_Meter_Weight.Text = "Txt_Meters_Weight"
        Me.cbo_Reconsilation_Meter_Weight.Visible = False
        '
        'lbl_Reconsilation_Meter_Weight
        '
        Me.lbl_Reconsilation_Meter_Weight.Location = New System.Drawing.Point(23, 300)
        Me.lbl_Reconsilation_Meter_Weight.Name = "lbl_Reconsilation_Meter_Weight"
        Me.lbl_Reconsilation_Meter_Weight.Size = New System.Drawing.Size(105, 40)
        Me.lbl_Reconsilation_Meter_Weight.TabIndex = 49
        Me.lbl_Reconsilation_Meter_Weight.Text = "Reconsilation Qty in Meter/Weight"
        Me.lbl_Reconsilation_Meter_Weight.Visible = False
        '
        'lbl_IdNo
        '
        Me.lbl_IdNo.BackColor = System.Drawing.Color.Red
        Me.lbl_IdNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_IdNo.Location = New System.Drawing.Point(162, 379)
        Me.lbl_IdNo.Name = "lbl_IdNo"
        Me.lbl_IdNo.Size = New System.Drawing.Size(77, 23)
        Me.lbl_IdNo.TabIndex = 40
        Me.lbl_IdNo.Text = "lbl_IdNo"
        Me.lbl_IdNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lbl_IdNo.Visible = False
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(300, 166)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(82, 15)
        Me.Label13.TabIndex = 15
        Me.Label13.Text = "Weight/Piece"
        '
        'txt_Meter_Qty
        '
        Me.txt_Meter_Qty.Location = New System.Drawing.Point(134, 162)
        Me.txt_Meter_Qty.MaxLength = 6
        Me.txt_Meter_Qty.Name = "txt_Meter_Qty"
        Me.txt_Meter_Qty.Size = New System.Drawing.Size(133, 23)
        Me.txt_Meter_Qty.TabIndex = 4
        Me.txt_Meter_Qty.Text = "txt_Meter_Qty"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(23, 166)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(68, 15)
        Me.Label14.TabIndex = 11
        Me.Label14.Text = "Meter/Qty"
        '
        'txt_MinimumStock
        '
        Me.txt_MinimumStock.Location = New System.Drawing.Point(415, 196)
        Me.txt_MinimumStock.MaxLength = 12
        Me.txt_MinimumStock.Name = "txt_MinimumStock"
        Me.txt_MinimumStock.Size = New System.Drawing.Size(141, 23)
        Me.txt_MinimumStock.TabIndex = 7
        Me.txt_MinimumStock.Text = "txt_MinimumStock"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(300, 200)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(95, 15)
        Me.Label11.TabIndex = 10
        Me.Label11.Text = "Minimum Stock "
        '
        'txt_CostRate
        '
        Me.txt_CostRate.Location = New System.Drawing.Point(415, 230)
        Me.txt_CostRate.MaxLength = 12
        Me.txt_CostRate.Name = "txt_CostRate"
        Me.txt_CostRate.Size = New System.Drawing.Size(141, 23)
        Me.txt_CostRate.TabIndex = 9
        Me.txt_CostRate.Text = "txt_CostRate"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(300, 234)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(62, 15)
        Me.Label10.TabIndex = 8
        Me.Label10.Text = "Cost Rate "
        '
        'txt_Code
        '
        Me.txt_Code.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_Code.Location = New System.Drawing.Point(134, 128)
        Me.txt_Code.MaxLength = 20
        Me.txt_Code.Name = "txt_Code"
        Me.txt_Code.Size = New System.Drawing.Size(133, 23)
        Me.txt_Code.TabIndex = 2
        Me.txt_Code.Text = "TXT_CODE"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(23, 132)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(35, 15)
        Me.Label9.TabIndex = 7
        Me.Label9.Text = "Code"
        '
        'cbo_Unit
        '
        Me.cbo_Unit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Unit.FormattingEnabled = True
        Me.cbo_Unit.Location = New System.Drawing.Point(415, 128)
        Me.cbo_Unit.Name = "cbo_Unit"
        Me.cbo_Unit.Size = New System.Drawing.Size(141, 23)
        Me.cbo_Unit.TabIndex = 3
        Me.cbo_Unit.Text = "cbo_Unit"
        '
        'cbo_ItemGroup
        '
        Me.cbo_ItemGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_ItemGroup.FormattingEnabled = True
        Me.cbo_ItemGroup.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cbo_ItemGroup.Location = New System.Drawing.Point(134, 94)
        Me.cbo_ItemGroup.Name = "cbo_ItemGroup"
        Me.cbo_ItemGroup.Size = New System.Drawing.Size(422, 23)
        Me.cbo_ItemGroup.TabIndex = 1
        Me.cbo_ItemGroup.Text = "cbo_ItemGroup"
        '
        'txt_TaxRate
        '
        Me.txt_TaxRate.Location = New System.Drawing.Point(415, 264)
        Me.txt_TaxRate.MaxLength = 12
        Me.txt_TaxRate.Name = "txt_TaxRate"
        Me.txt_TaxRate.Size = New System.Drawing.Size(141, 23)
        Me.txt_TaxRate.TabIndex = 11
        Me.txt_TaxRate.Text = "txt_TaxRate"
        '
        'txt_Rate
        '
        Me.txt_Rate.Location = New System.Drawing.Point(134, 264)
        Me.txt_Rate.MaxLength = 12
        Me.txt_Rate.Name = "txt_Rate"
        Me.txt_Rate.Size = New System.Drawing.Size(133, 23)
        Me.txt_Rate.TabIndex = 10
        Me.txt_Rate.Text = "txt_Rate"
        '
        'txt_TaxPerc
        '
        Me.txt_TaxPerc.Location = New System.Drawing.Point(134, 230)
        Me.txt_TaxPerc.MaxLength = 6
        Me.txt_TaxPerc.Name = "txt_TaxPerc"
        Me.txt_TaxPerc.Size = New System.Drawing.Size(133, 23)
        Me.txt_TaxPerc.TabIndex = 8
        Me.txt_TaxPerc.Text = "txt_TaxPerc"
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(300, 264)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(98, 31)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Sales Rate  (Incl.Tax)"
        '
        'txt_Name
        '
        Me.txt_Name.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_Name.Location = New System.Drawing.Point(134, 60)
        Me.txt_Name.MaxLength = 50
        Me.txt_Name.Name = "txt_Name"
        Me.txt_Name.Size = New System.Drawing.Size(422, 23)
        Me.txt_Name.TabIndex = 0
        Me.txt_Name.Text = "TXT_NAME"
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(23, 264)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(90, 31)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Sales Rate  (Excl.Tax)"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(300, 132)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(33, 15)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Unit "
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(23, 234)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(36, 15)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Tax %"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(23, 98)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(70, 15)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Item Group"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(23, 64)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(40, 15)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Name"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(23, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(36, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Id.No"
        '
        'Label7
        '
        Me.Label7.AutoEllipsis = True
        Me.Label7.BackColor = System.Drawing.Color.DarkCyan
        Me.Label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label7.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label7.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.Label7.Location = New System.Drawing.Point(0, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(625, 35)
        Me.Label7.TabIndex = 36
        Me.Label7.Text = "FINISHED PRODUCT CREATION"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'FinishedProduct_Creation_Simple
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Azure
        Me.ClientSize = New System.Drawing.Size(625, 476)
        Me.Controls.Add(Me.grp_Filter)
        Me.Controls.Add(Me.grp_Open)
        Me.Controls.Add(Me.grp_Back)
        Me.Controls.Add(Me.Label7)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FinishedProduct_Creation_Simple"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FINISHED PRODUCT CREATION"
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grp_Filter.ResumeLayout(False)
        Me.grp_Open.ResumeLayout(False)
        Me.grp_Back.ResumeLayout(False)
        Me.grp_Back.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txt_Width As System.Windows.Forms.TextBox
    Friend WithEvents cbo_Open As System.Windows.Forms.ComboBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents txt_Weight_Piece As System.Windows.Forms.TextBox
    Friend WithEvents btn_Find As System.Windows.Forms.Button
    Friend WithEvents dgv_Filter As System.Windows.Forms.DataGridView
    Friend WithEvents btn_CloseFilter As System.Windows.Forms.Button
    Friend WithEvents grp_Filter As System.Windows.Forms.GroupBox
    Friend WithEvents btn_OpenFilter As System.Windows.Forms.Button
    Friend WithEvents grp_Open As System.Windows.Forms.GroupBox
    Friend WithEvents chk_Verified_Status As System.Windows.Forms.CheckBox
    Friend WithEvents btn_close As System.Windows.Forms.Button
    Friend WithEvents btn_save As System.Windows.Forms.Button
    Friend WithEvents lbl_DisplaySlNo As System.Windows.Forms.Label
    Friend WithEvents grp_Back As System.Windows.Forms.GroupBox
    Friend WithEvents lbl_IdNo As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txt_Meter_Qty As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txt_MinimumStock As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txt_CostRate As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txt_Code As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents cbo_Unit As System.Windows.Forms.ComboBox
    Friend WithEvents txt_TaxRate As System.Windows.Forms.TextBox
    Friend WithEvents txt_Rate As System.Windows.Forms.TextBox
    Friend WithEvents txt_TaxPerc As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txt_Name As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents cbo_ItemGroup As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cbo_Reconsilation_Meter_Weight As ComboBox
    Friend WithEvents lbl_Reconsilation_Meter_Weight As Label
End Class
