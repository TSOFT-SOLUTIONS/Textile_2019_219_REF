<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Item_Creation
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Item_Creation))
        Me.grp_Open = New System.Windows.Forms.GroupBox()
        Me.btn_Find = New System.Windows.Forms.Button()
        Me.cbo_Open = New System.Windows.Forms.ComboBox()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.grp_Filter = New System.Windows.Forms.GroupBox()
        Me.dgv_Filter = New System.Windows.Forms.DataGridView()
        Me.btn_CloseFilter = New System.Windows.Forms.Button()
        Me.btn_OpenFilter = New System.Windows.Forms.Button()
        Me.lbl_FormHeading = New System.Windows.Forms.Label()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.txt_TamilName = New System.Windows.Forms.TextBox()
        Me.btn_Character = New System.Windows.Forms.Button()
        Me.txt_HSNCode = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.chk_Close_Status = New System.Windows.Forms.CheckBox()
        Me.lbl_IdNo = New System.Windows.Forms.Label()
        Me.txt_Rack_No = New System.Windows.Forms.TextBox()
        Me.lbl_CostRate_Incl_Tax_Caption = New System.Windows.Forms.Label()
        Me.txt_CostRate_Excl_Tax = New System.Windows.Forms.TextBox()
        Me.lbl_CostRate_Excl_Tax_Caption = New System.Windows.Forms.Label()
        Me.cbo_DealerName = New System.Windows.Forms.ComboBox()
        Me.cbo_Size = New System.Windows.Forms.ComboBox()
        Me.cbo_Style = New System.Windows.Forms.ComboBox()
        Me.lbl_SizeCaption = New System.Windows.Forms.Label()
        Me.txt_description = New System.Windows.Forms.TextBox()
        Me.btn_SaveAll = New System.Windows.Forms.Button()
        Me.txt_SalesRate_Wholesale = New System.Windows.Forms.TextBox()
        Me.lbl_SalesRate_WholeSale = New System.Windows.Forms.Label()
        Me.txt_SalesProfit_Wholesale = New System.Windows.Forms.TextBox()
        Me.lbl_SalesProfit_Wholesale = New System.Windows.Forms.Label()
        Me.txt_SalesRate_Retail = New System.Windows.Forms.TextBox()
        Me.lbl_SalesRate_Retail = New System.Windows.Forms.Label()
        Me.txt_SalesProfit_Retail = New System.Windows.Forms.TextBox()
        Me.lbl_SalesProfit_Retail = New System.Windows.Forms.Label()
        Me.txt_DiscountPercentage = New System.Windows.Forms.TextBox()
        Me.lbl_DiscountPercCaption = New System.Windows.Forms.Label()
        Me.btn_fromExcel = New System.Windows.Forms.Button()
        Me.txt_Sales_GSTRate = New System.Windows.Forms.TextBox()
        Me.txt_GSTTaxPerc = New System.Windows.Forms.TextBox()
        Me.lbl_Sales_Rate_GST_Caption = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.btn_save = New System.Windows.Forms.Button()
        Me.chk_JobWorkStatus = New System.Windows.Forms.CheckBox()
        Me.txt_Mrp = New System.Windows.Forms.TextBox()
        Me.lbl_mrp_Caption = New System.Windows.Forms.Label()
        Me.lbl_description_Caption = New System.Windows.Forms.Label()
        Me.txt_MinimumStock = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txt_CostRate_Incl_Tax = New System.Windows.Forms.TextBox()
        Me.txt_Code = New System.Windows.Forms.TextBox()
        Me.lbl_Code_Caption = New System.Windows.Forms.Label()
        Me.cbo_Unit = New System.Windows.Forms.ComboBox()
        Me.cbo_ItemGroup = New System.Windows.Forms.ComboBox()
        Me.txt_SalesRate_Excl_Tax = New System.Windows.Forms.TextBox()
        Me.txt_Name = New System.Windows.Forms.TextBox()
        Me.lbl_sales_Rate_Excl_Tax_Caption = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lbl_DealerName = New System.Windows.Forms.Label()
        Me.lbl_ItemGroup_Caption = New System.Windows.Forms.Label()
        Me.lbl_Name_Caption = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lbl_StyleCaption = New System.Windows.Forms.Label()
        Me.lbl_tamilname_Caption = New System.Windows.Forms.Label()
        Me.txt_VatTaxRate = New System.Windows.Forms.TextBox()
        Me.txt_VatTaxPerc = New System.Windows.Forms.TextBox()
        Me.lbl_sales_Rate_Vat_Caption = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.grp_Open.SuspendLayout()
        Me.grp_Filter.SuspendLayout()
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnl_Back.SuspendLayout()
        Me.SuspendLayout()
        '
        'grp_Open
        '
        Me.grp_Open.BackColor = System.Drawing.Color.WhiteSmoke
        Me.grp_Open.Controls.Add(Me.btn_Find)
        Me.grp_Open.Controls.Add(Me.cbo_Open)
        Me.grp_Open.Controls.Add(Me.btnClose)
        Me.grp_Open.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Open.Location = New System.Drawing.Point(1194, 371)
        Me.grp_Open.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.grp_Open.Name = "grp_Open"
        Me.grp_Open.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.grp_Open.Size = New System.Drawing.Size(898, 308)
        Me.grp_Open.TabIndex = 31
        Me.grp_Open.TabStop = False
        Me.grp_Open.Text = "FINDING"
        '
        'btn_Find
        '
        Me.btn_Find.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Find.Image = CType(resources.GetObject("btn_Find.Image"), System.Drawing.Image)
        Me.btn_Find.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Find.Location = New System.Drawing.Point(604, 220)
        Me.btn_Find.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btn_Find.Name = "btn_Find"
        Me.btn_Find.Size = New System.Drawing.Size(120, 51)
        Me.btn_Find.TabIndex = 31
        Me.btn_Find.Text = "&Find"
        Me.btn_Find.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Find.UseVisualStyleBackColor = True
        '
        'cbo_Open
        '
        Me.cbo_Open.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Open.DropDownHeight = 90
        Me.cbo_Open.Font = New System.Drawing.Font("Cambria", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Open.FormattingEnabled = True
        Me.cbo_Open.IntegralHeight = False
        Me.cbo_Open.Location = New System.Drawing.Point(24, 43)
        Me.cbo_Open.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cbo_Open.Name = "cbo_Open"
        Me.cbo_Open.Size = New System.Drawing.Size(835, 35)
        Me.cbo_Open.Sorted = True
        Me.cbo_Open.TabIndex = 0
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(741, 220)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(120, 51)
        Me.btnClose.TabIndex = 30
        Me.btnClose.Text = "&Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'grp_Filter
        '
        Me.grp_Filter.BackColor = System.Drawing.Color.WhiteSmoke
        Me.grp_Filter.Controls.Add(Me.dgv_Filter)
        Me.grp_Filter.Controls.Add(Me.btn_CloseFilter)
        Me.grp_Filter.Controls.Add(Me.btn_OpenFilter)
        Me.grp_Filter.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Filter.Location = New System.Drawing.Point(1194, 734)
        Me.grp_Filter.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.grp_Filter.Name = "grp_Filter"
        Me.grp_Filter.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.grp_Filter.Size = New System.Drawing.Size(898, 462)
        Me.grp_Filter.TabIndex = 32
        Me.grp_Filter.TabStop = False
        Me.grp_Filter.Text = "FILTER"
        '
        'dgv_Filter
        '
        Me.dgv_Filter.AllowUserToOrderColumns = True
        Me.dgv_Filter.BackgroundColor = System.Drawing.Color.White
        Me.dgv_Filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_Filter.Location = New System.Drawing.Point(21, 35)
        Me.dgv_Filter.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgv_Filter.Name = "dgv_Filter"
        Me.dgv_Filter.RowHeadersWidth = 62
        Me.dgv_Filter.Size = New System.Drawing.Size(858, 342)
        Me.dgv_Filter.TabIndex = 0
        '
        'btn_CloseFilter
        '
        Me.btn_CloseFilter.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btn_CloseFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_CloseFilter.ForeColor = System.Drawing.Color.Blue
        Me.btn_CloseFilter.Location = New System.Drawing.Point(741, 394)
        Me.btn_CloseFilter.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btn_CloseFilter.Name = "btn_CloseFilter"
        Me.btn_CloseFilter.Size = New System.Drawing.Size(120, 51)
        Me.btn_CloseFilter.TabIndex = 2
        Me.btn_CloseFilter.Text = "&CLOSE"
        Me.btn_CloseFilter.UseVisualStyleBackColor = False
        '
        'btn_OpenFilter
        '
        Me.btn_OpenFilter.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btn_OpenFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_OpenFilter.ForeColor = System.Drawing.Color.Blue
        Me.btn_OpenFilter.Location = New System.Drawing.Point(594, 394)
        Me.btn_OpenFilter.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btn_OpenFilter.Name = "btn_OpenFilter"
        Me.btn_OpenFilter.Size = New System.Drawing.Size(120, 51)
        Me.btn_OpenFilter.TabIndex = 1
        Me.btn_OpenFilter.Text = "&OPEN"
        Me.btn_OpenFilter.UseVisualStyleBackColor = False
        '
        'lbl_FormHeading
        '
        Me.lbl_FormHeading.AutoEllipsis = True
        Me.lbl_FormHeading.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.lbl_FormHeading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_FormHeading.Dock = System.Windows.Forms.DockStyle.Top
        Me.lbl_FormHeading.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_FormHeading.ForeColor = System.Drawing.Color.White
        Me.lbl_FormHeading.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lbl_FormHeading.Location = New System.Drawing.Point(0, 0)
        Me.lbl_FormHeading.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_FormHeading.Name = "lbl_FormHeading"
        Me.lbl_FormHeading.Size = New System.Drawing.Size(966, 48)
        Me.lbl_FormHeading.TabIndex = 1
        Me.lbl_FormHeading.Text = "ITEM CREATION"
        Me.lbl_FormHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'Timer1
        '
        Me.Timer1.Interval = 200
        '
        'pnl_Back
        '
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.txt_TamilName)
        Me.pnl_Back.Controls.Add(Me.btn_Character)
        Me.pnl_Back.Controls.Add(Me.txt_HSNCode)
        Me.pnl_Back.Controls.Add(Me.Label3)
        Me.pnl_Back.Controls.Add(Me.chk_Close_Status)
        Me.pnl_Back.Controls.Add(Me.lbl_IdNo)
        Me.pnl_Back.Controls.Add(Me.txt_Rack_No)
        Me.pnl_Back.Controls.Add(Me.lbl_CostRate_Incl_Tax_Caption)
        Me.pnl_Back.Controls.Add(Me.txt_CostRate_Excl_Tax)
        Me.pnl_Back.Controls.Add(Me.lbl_CostRate_Excl_Tax_Caption)
        Me.pnl_Back.Controls.Add(Me.cbo_DealerName)
        Me.pnl_Back.Controls.Add(Me.cbo_Size)
        Me.pnl_Back.Controls.Add(Me.cbo_Style)
        Me.pnl_Back.Controls.Add(Me.lbl_SizeCaption)
        Me.pnl_Back.Controls.Add(Me.txt_description)
        Me.pnl_Back.Controls.Add(Me.btn_SaveAll)
        Me.pnl_Back.Controls.Add(Me.txt_SalesRate_Wholesale)
        Me.pnl_Back.Controls.Add(Me.lbl_SalesRate_WholeSale)
        Me.pnl_Back.Controls.Add(Me.txt_SalesProfit_Wholesale)
        Me.pnl_Back.Controls.Add(Me.lbl_SalesProfit_Wholesale)
        Me.pnl_Back.Controls.Add(Me.txt_SalesRate_Retail)
        Me.pnl_Back.Controls.Add(Me.lbl_SalesRate_Retail)
        Me.pnl_Back.Controls.Add(Me.txt_SalesProfit_Retail)
        Me.pnl_Back.Controls.Add(Me.lbl_SalesProfit_Retail)
        Me.pnl_Back.Controls.Add(Me.txt_DiscountPercentage)
        Me.pnl_Back.Controls.Add(Me.lbl_DiscountPercCaption)
        Me.pnl_Back.Controls.Add(Me.btn_fromExcel)
        Me.pnl_Back.Controls.Add(Me.txt_Sales_GSTRate)
        Me.pnl_Back.Controls.Add(Me.txt_GSTTaxPerc)
        Me.pnl_Back.Controls.Add(Me.lbl_Sales_Rate_GST_Caption)
        Me.pnl_Back.Controls.Add(Me.Label15)
        Me.pnl_Back.Controls.Add(Me.btn_Close)
        Me.pnl_Back.Controls.Add(Me.btn_save)
        Me.pnl_Back.Controls.Add(Me.chk_JobWorkStatus)
        Me.pnl_Back.Controls.Add(Me.txt_Mrp)
        Me.pnl_Back.Controls.Add(Me.lbl_mrp_Caption)
        Me.pnl_Back.Controls.Add(Me.lbl_description_Caption)
        Me.pnl_Back.Controls.Add(Me.txt_MinimumStock)
        Me.pnl_Back.Controls.Add(Me.Label11)
        Me.pnl_Back.Controls.Add(Me.txt_CostRate_Incl_Tax)
        Me.pnl_Back.Controls.Add(Me.txt_Code)
        Me.pnl_Back.Controls.Add(Me.lbl_Code_Caption)
        Me.pnl_Back.Controls.Add(Me.cbo_Unit)
        Me.pnl_Back.Controls.Add(Me.cbo_ItemGroup)
        Me.pnl_Back.Controls.Add(Me.txt_SalesRate_Excl_Tax)
        Me.pnl_Back.Controls.Add(Me.txt_Name)
        Me.pnl_Back.Controls.Add(Me.lbl_sales_Rate_Excl_Tax_Caption)
        Me.pnl_Back.Controls.Add(Me.Label4)
        Me.pnl_Back.Controls.Add(Me.lbl_DealerName)
        Me.pnl_Back.Controls.Add(Me.lbl_ItemGroup_Caption)
        Me.pnl_Back.Controls.Add(Me.lbl_Name_Caption)
        Me.pnl_Back.Controls.Add(Me.Label1)
        Me.pnl_Back.Controls.Add(Me.lbl_StyleCaption)
        Me.pnl_Back.Controls.Add(Me.lbl_tamilname_Caption)
        Me.pnl_Back.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnl_Back.Location = New System.Drawing.Point(10, 62)
        Me.pnl_Back.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(940, 782)
        Me.pnl_Back.TabIndex = 33
        '
        'txt_TamilName
        '
        Me.txt_TamilName.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txt_TamilName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_TamilName.Location = New System.Drawing.Point(206, 122)
        Me.txt_TamilName.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txt_TamilName.Name = "txt_TamilName"
        Me.txt_TamilName.Size = New System.Drawing.Size(691, 35)
        Me.txt_TamilName.TabIndex = 1
        Me.txt_TamilName.Text = "txt_TamilName"
        Me.txt_TamilName.Visible = False
        '
        'btn_Character
        '
        Me.btn_Character.BackColor = System.Drawing.Color.DeepPink
        Me.btn_Character.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn_Character.ForeColor = System.Drawing.Color.White
        Me.btn_Character.Location = New System.Drawing.Point(898, 123)
        Me.btn_Character.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btn_Character.Name = "btn_Character"
        Me.btn_Character.Size = New System.Drawing.Size(39, 35)
        Me.btn_Character.TabIndex = 82
        Me.btn_Character.TabStop = False
        Me.btn_Character.Text = "..."
        Me.btn_Character.UseVisualStyleBackColor = False
        Me.btn_Character.Visible = False
        '
        'txt_HSNCode
        '
        Me.txt_HSNCode.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_HSNCode.Location = New System.Drawing.Point(620, 331)
        Me.txt_HSNCode.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txt_HSNCode.MaxLength = 10
        Me.txt_HSNCode.Name = "txt_HSNCode"
        Me.txt_HSNCode.Size = New System.Drawing.Size(258, 31)
        Me.txt_HSNCode.TabIndex = 10
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Blue
        Me.Label3.Location = New System.Drawing.Point(458, 337)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(93, 24)
        Me.Label3.TabIndex = 116
        Me.Label3.Text = "HSN Code"
        '
        'chk_Close_Status
        '
        Me.chk_Close_Status.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chk_Close_Status.ForeColor = System.Drawing.Color.Black
        Me.chk_Close_Status.Location = New System.Drawing.Point(290, 714)
        Me.chk_Close_Status.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.chk_Close_Status.Name = "chk_Close_Status"
        Me.chk_Close_Status.Size = New System.Drawing.Size(198, 37)
        Me.chk_Close_Status.TabIndex = 114
        Me.chk_Close_Status.TabStop = False
        Me.chk_Close_Status.Text = "Close Status"
        Me.chk_Close_Status.UseVisualStyleBackColor = True
        Me.chk_Close_Status.Visible = False
        '
        'lbl_IdNo
        '
        Me.lbl_IdNo.BackColor = System.Drawing.Color.White
        Me.lbl_IdNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_IdNo.Location = New System.Drawing.Point(206, 17)
        Me.lbl_IdNo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_IdNo.Name = "lbl_IdNo"
        Me.lbl_IdNo.Size = New System.Drawing.Size(692, 34)
        Me.lbl_IdNo.TabIndex = 113
        Me.lbl_IdNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txt_Rack_No
        '
        Me.txt_Rack_No.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Rack_No.Location = New System.Drawing.Point(596, 540)
        Me.txt_Rack_No.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txt_Rack_No.Name = "txt_Rack_No"
        Me.txt_Rack_No.Size = New System.Drawing.Size(301, 31)
        Me.txt_Rack_No.TabIndex = 19
        Me.txt_Rack_No.Text = "txt_Rack_No"
        Me.txt_Rack_No.Visible = False
        '
        'lbl_CostRate_Incl_Tax_Caption
        '
        Me.lbl_CostRate_Incl_Tax_Caption.AutoSize = True
        Me.lbl_CostRate_Incl_Tax_Caption.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_CostRate_Incl_Tax_Caption.ForeColor = System.Drawing.Color.Blue
        Me.lbl_CostRate_Incl_Tax_Caption.Location = New System.Drawing.Point(458, 389)
        Me.lbl_CostRate_Incl_Tax_Caption.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_CostRate_Incl_Tax_Caption.Name = "lbl_CostRate_Incl_Tax_Caption"
        Me.lbl_CostRate_Incl_Tax_Caption.Size = New System.Drawing.Size(188, 24)
        Me.lbl_CostRate_Incl_Tax_Caption.TabIndex = 112
        Me.lbl_CostRate_Incl_Tax_Caption.Text = "Cost Rate ( Incl. GST )"
        '
        'txt_CostRate_Excl_Tax
        '
        Me.txt_CostRate_Excl_Tax.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_CostRate_Excl_Tax.Location = New System.Drawing.Point(206, 383)
        Me.txt_CostRate_Excl_Tax.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txt_CostRate_Excl_Tax.MaxLength = 12
        Me.txt_CostRate_Excl_Tax.Name = "txt_CostRate_Excl_Tax"
        Me.txt_CostRate_Excl_Tax.Size = New System.Drawing.Size(223, 31)
        Me.txt_CostRate_Excl_Tax.TabIndex = 11
        '
        'lbl_CostRate_Excl_Tax_Caption
        '
        Me.lbl_CostRate_Excl_Tax_Caption.AutoSize = True
        Me.lbl_CostRate_Excl_Tax_Caption.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_CostRate_Excl_Tax_Caption.ForeColor = System.Drawing.Color.Blue
        Me.lbl_CostRate_Excl_Tax_Caption.Location = New System.Drawing.Point(28, 389)
        Me.lbl_CostRate_Excl_Tax_Caption.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_CostRate_Excl_Tax_Caption.Name = "lbl_CostRate_Excl_Tax_Caption"
        Me.lbl_CostRate_Excl_Tax_Caption.Size = New System.Drawing.Size(170, 24)
        Me.lbl_CostRate_Excl_Tax_Caption.TabIndex = 110
        Me.lbl_CostRate_Excl_Tax_Caption.Text = "Cost Rate (Excl.Tax)"
        '
        'cbo_DealerName
        '
        Me.cbo_DealerName.DropDownHeight = 200
        Me.cbo_DealerName.FormattingEnabled = True
        Me.cbo_DealerName.IntegralHeight = False
        Me.cbo_DealerName.Location = New System.Drawing.Point(620, 226)
        Me.cbo_DealerName.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cbo_DealerName.Name = "cbo_DealerName"
        Me.cbo_DealerName.Size = New System.Drawing.Size(277, 32)
        Me.cbo_DealerName.TabIndex = 6
        Me.cbo_DealerName.Text = "cbo_DealerName"
        Me.cbo_DealerName.Visible = False
        '
        'cbo_Size
        '
        Me.cbo_Size.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Size.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Size.FormattingEnabled = True
        Me.cbo_Size.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cbo_Size.Location = New System.Drawing.Point(620, 122)
        Me.cbo_Size.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cbo_Size.Name = "cbo_Size"
        Me.cbo_Size.Size = New System.Drawing.Size(277, 32)
        Me.cbo_Size.TabIndex = 2
        Me.cbo_Size.Visible = False
        '
        'cbo_Style
        '
        Me.cbo_Style.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Style.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Style.FormattingEnabled = True
        Me.cbo_Style.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cbo_Style.Location = New System.Drawing.Point(206, 122)
        Me.cbo_Style.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cbo_Style.Name = "cbo_Style"
        Me.cbo_Style.Size = New System.Drawing.Size(223, 32)
        Me.cbo_Style.TabIndex = 1
        Me.cbo_Style.Visible = False
        '
        'lbl_SizeCaption
        '
        Me.lbl_SizeCaption.AutoSize = True
        Me.lbl_SizeCaption.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_SizeCaption.ForeColor = System.Drawing.Color.Blue
        Me.lbl_SizeCaption.Location = New System.Drawing.Point(458, 128)
        Me.lbl_SizeCaption.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_SizeCaption.Name = "lbl_SizeCaption"
        Me.lbl_SizeCaption.Size = New System.Drawing.Size(42, 24)
        Me.lbl_SizeCaption.TabIndex = 108
        Me.lbl_SizeCaption.Text = "Size"
        Me.lbl_SizeCaption.Visible = False
        '
        'txt_description
        '
        Me.txt_description.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_description.Location = New System.Drawing.Point(206, 540)
        Me.txt_description.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txt_description.Name = "txt_description"
        Me.txt_description.Size = New System.Drawing.Size(332, 31)
        Me.txt_description.TabIndex = 18
        Me.txt_description.Text = "txt_description"
        Me.txt_description.Visible = False
        '
        'btn_SaveAll
        '
        Me.btn_SaveAll.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_SaveAll.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_SaveAll.ForeColor = System.Drawing.Color.White
        Me.btn_SaveAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_SaveAll.Location = New System.Drawing.Point(200, 705)
        Me.btn_SaveAll.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btn_SaveAll.Name = "btn_SaveAll"
        Me.btn_SaveAll.Size = New System.Drawing.Size(120, 54)
        Me.btn_SaveAll.TabIndex = 106
        Me.btn_SaveAll.TabStop = False
        Me.btn_SaveAll.Text = "SAVE A&LL"
        Me.btn_SaveAll.UseVisualStyleBackColor = False
        Me.btn_SaveAll.Visible = False
        '
        'txt_SalesRate_Wholesale
        '
        Me.txt_SalesRate_Wholesale.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_SalesRate_Wholesale.Location = New System.Drawing.Point(620, 645)
        Me.txt_SalesRate_Wholesale.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txt_SalesRate_Wholesale.MaxLength = 12
        Me.txt_SalesRate_Wholesale.Name = "txt_SalesRate_Wholesale"
        Me.txt_SalesRate_Wholesale.Size = New System.Drawing.Size(277, 31)
        Me.txt_SalesRate_Wholesale.TabIndex = 23
        Me.txt_SalesRate_Wholesale.Visible = False
        '
        'lbl_SalesRate_WholeSale
        '
        Me.lbl_SalesRate_WholeSale.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_SalesRate_WholeSale.ForeColor = System.Drawing.Color.Blue
        Me.lbl_SalesRate_WholeSale.Location = New System.Drawing.Point(458, 638)
        Me.lbl_SalesRate_WholeSale.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_SalesRate_WholeSale.Name = "lbl_SalesRate_WholeSale"
        Me.lbl_SalesRate_WholeSale.Size = New System.Drawing.Size(140, 46)
        Me.lbl_SalesRate_WholeSale.TabIndex = 105
        Me.lbl_SalesRate_WholeSale.Text = "Sales Rate (WholeSale)"
        Me.lbl_SalesRate_WholeSale.Visible = False
        '
        'txt_SalesProfit_Wholesale
        '
        Me.txt_SalesProfit_Wholesale.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_SalesProfit_Wholesale.Location = New System.Drawing.Point(171, 645)
        Me.txt_SalesProfit_Wholesale.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txt_SalesProfit_Wholesale.MaxLength = 12
        Me.txt_SalesProfit_Wholesale.Name = "txt_SalesProfit_Wholesale"
        Me.txt_SalesProfit_Wholesale.Size = New System.Drawing.Size(258, 31)
        Me.txt_SalesProfit_Wholesale.TabIndex = 22
        Me.txt_SalesProfit_Wholesale.Visible = False
        '
        'lbl_SalesProfit_Wholesale
        '
        Me.lbl_SalesProfit_Wholesale.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_SalesProfit_Wholesale.ForeColor = System.Drawing.Color.Blue
        Me.lbl_SalesProfit_Wholesale.Location = New System.Drawing.Point(28, 638)
        Me.lbl_SalesProfit_Wholesale.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_SalesProfit_Wholesale.Name = "lbl_SalesProfit_Wholesale"
        Me.lbl_SalesProfit_Wholesale.Size = New System.Drawing.Size(140, 46)
        Me.lbl_SalesProfit_Wholesale.TabIndex = 104
        Me.lbl_SalesProfit_Wholesale.Text = "Sales Profit % (WholeSale)"
        Me.lbl_SalesProfit_Wholesale.Visible = False
        '
        'txt_SalesRate_Retail
        '
        Me.txt_SalesRate_Retail.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_SalesRate_Retail.Location = New System.Drawing.Point(620, 592)
        Me.txt_SalesRate_Retail.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txt_SalesRate_Retail.MaxLength = 12
        Me.txt_SalesRate_Retail.Name = "txt_SalesRate_Retail"
        Me.txt_SalesRate_Retail.Size = New System.Drawing.Size(277, 31)
        Me.txt_SalesRate_Retail.TabIndex = 21
        Me.txt_SalesRate_Retail.Visible = False
        '
        'lbl_SalesRate_Retail
        '
        Me.lbl_SalesRate_Retail.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_SalesRate_Retail.ForeColor = System.Drawing.Color.Blue
        Me.lbl_SalesRate_Retail.Location = New System.Drawing.Point(458, 586)
        Me.lbl_SalesRate_Retail.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_SalesRate_Retail.Name = "lbl_SalesRate_Retail"
        Me.lbl_SalesRate_Retail.Size = New System.Drawing.Size(140, 46)
        Me.lbl_SalesRate_Retail.TabIndex = 103
        Me.lbl_SalesRate_Retail.Text = "Sales Rate (Retail)"
        Me.lbl_SalesRate_Retail.Visible = False
        '
        'txt_SalesProfit_Retail
        '
        Me.txt_SalesProfit_Retail.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_SalesProfit_Retail.Location = New System.Drawing.Point(171, 592)
        Me.txt_SalesProfit_Retail.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txt_SalesProfit_Retail.MaxLength = 12
        Me.txt_SalesProfit_Retail.Name = "txt_SalesProfit_Retail"
        Me.txt_SalesProfit_Retail.Size = New System.Drawing.Size(258, 31)
        Me.txt_SalesProfit_Retail.TabIndex = 20
        Me.txt_SalesProfit_Retail.Visible = False
        '
        'lbl_SalesProfit_Retail
        '
        Me.lbl_SalesProfit_Retail.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_SalesProfit_Retail.ForeColor = System.Drawing.Color.Blue
        Me.lbl_SalesProfit_Retail.Location = New System.Drawing.Point(28, 586)
        Me.lbl_SalesProfit_Retail.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_SalesProfit_Retail.Name = "lbl_SalesProfit_Retail"
        Me.lbl_SalesProfit_Retail.Size = New System.Drawing.Size(140, 46)
        Me.lbl_SalesProfit_Retail.TabIndex = 102
        Me.lbl_SalesProfit_Retail.Text = "Sales Profit % (Retail)"
        Me.lbl_SalesProfit_Retail.Visible = False
        '
        'txt_DiscountPercentage
        '
        Me.txt_DiscountPercentage.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_DiscountPercentage.Location = New System.Drawing.Point(651, 488)
        Me.txt_DiscountPercentage.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txt_DiscountPercentage.MaxLength = 12
        Me.txt_DiscountPercentage.Name = "txt_DiscountPercentage"
        Me.txt_DiscountPercentage.Size = New System.Drawing.Size(246, 31)
        Me.txt_DiscountPercentage.TabIndex = 16
        Me.txt_DiscountPercentage.Visible = False
        '
        'lbl_DiscountPercCaption
        '
        Me.lbl_DiscountPercCaption.AutoSize = True
        Me.lbl_DiscountPercCaption.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_DiscountPercCaption.ForeColor = System.Drawing.Color.Blue
        Me.lbl_DiscountPercCaption.Location = New System.Drawing.Point(458, 494)
        Me.lbl_DiscountPercCaption.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_DiscountPercCaption.Name = "lbl_DiscountPercCaption"
        Me.lbl_DiscountPercCaption.Size = New System.Drawing.Size(163, 24)
        Me.lbl_DiscountPercCaption.TabIndex = 101
        Me.lbl_DiscountPercCaption.Text = "Discount % (Sales)"
        Me.lbl_DiscountPercCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lbl_DiscountPercCaption.Visible = False
        '
        'btn_fromExcel
        '
        Me.btn_fromExcel.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_fromExcel.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_fromExcel.ForeColor = System.Drawing.Color.White
        Me.btn_fromExcel.Location = New System.Drawing.Point(28, 705)
        Me.btn_fromExcel.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btn_fromExcel.Name = "btn_fromExcel"
        Me.btn_fromExcel.Size = New System.Drawing.Size(135, 54)
        Me.btn_fromExcel.TabIndex = 97
        Me.btn_fromExcel.TabStop = False
        Me.btn_fromExcel.Text = "FROM EXCEL"
        Me.btn_fromExcel.UseVisualStyleBackColor = False
        Me.btn_fromExcel.Visible = False
        '
        'txt_Sales_GSTRate
        '
        Me.txt_Sales_GSTRate.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Sales_GSTRate.Location = New System.Drawing.Point(651, 435)
        Me.txt_Sales_GSTRate.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txt_Sales_GSTRate.MaxLength = 12
        Me.txt_Sales_GSTRate.Name = "txt_Sales_GSTRate"
        Me.txt_Sales_GSTRate.Size = New System.Drawing.Size(246, 31)
        Me.txt_Sales_GSTRate.TabIndex = 14
        '
        'txt_GSTTaxPerc
        '
        Me.txt_GSTTaxPerc.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_GSTTaxPerc.Location = New System.Drawing.Point(206, 331)
        Me.txt_GSTTaxPerc.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txt_GSTTaxPerc.MaxLength = 6
        Me.txt_GSTTaxPerc.Name = "txt_GSTTaxPerc"
        Me.txt_GSTTaxPerc.Size = New System.Drawing.Size(223, 31)
        Me.txt_GSTTaxPerc.TabIndex = 9
        '
        'lbl_Sales_Rate_GST_Caption
        '
        Me.lbl_Sales_Rate_GST_Caption.AutoSize = True
        Me.lbl_Sales_Rate_GST_Caption.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Sales_Rate_GST_Caption.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Sales_Rate_GST_Caption.Location = New System.Drawing.Point(458, 442)
        Me.lbl_Sales_Rate_GST_Caption.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_Sales_Rate_GST_Caption.Name = "lbl_Sales_Rate_GST_Caption"
        Me.lbl_Sales_Rate_GST_Caption.Size = New System.Drawing.Size(193, 24)
        Me.lbl_Sales_Rate_GST_Caption.TabIndex = 99
        Me.lbl_Sales_Rate_GST_Caption.Text = "Sales Rate ( Incl. GST )"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.Color.Blue
        Me.Label15.Location = New System.Drawing.Point(28, 337)
        Me.Label15.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(67, 24)
        Me.Label15.TabIndex = 100
        Me.Label15.Text = "GST  %"
        '
        'btn_Close
        '
        Me.btn_Close.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_Close.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Close.ForeColor = System.Drawing.Color.White
        Me.btn_Close.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Close.Location = New System.Drawing.Point(778, 705)
        Me.btn_Close.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(120, 54)
        Me.btn_Close.TabIndex = 24
        Me.btn_Close.TabStop = False
        Me.btn_Close.Text = "&CLOSE"
        Me.btn_Close.UseVisualStyleBackColor = False
        '
        'btn_save
        '
        Me.btn_save.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_save.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_save.ForeColor = System.Drawing.Color.White
        Me.btn_save.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_save.Location = New System.Drawing.Point(620, 705)
        Me.btn_save.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(120, 54)
        Me.btn_save.TabIndex = 23
        Me.btn_save.TabStop = False
        Me.btn_save.Text = "&SAVE"
        Me.btn_save.UseVisualStyleBackColor = False
        '
        'chk_JobWorkStatus
        '
        Me.chk_JobWorkStatus.AutoSize = True
        Me.chk_JobWorkStatus.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chk_JobWorkStatus.Location = New System.Drawing.Point(621, 177)
        Me.chk_JobWorkStatus.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.chk_JobWorkStatus.Name = "chk_JobWorkStatus"
        Me.chk_JobWorkStatus.Size = New System.Drawing.Size(110, 28)
        Me.chk_JobWorkStatus.TabIndex = 4
        Me.chk_JobWorkStatus.Text = "JobWork"
        Me.chk_JobWorkStatus.UseVisualStyleBackColor = True
        Me.chk_JobWorkStatus.Visible = False
        '
        'txt_Mrp
        '
        Me.txt_Mrp.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Mrp.Location = New System.Drawing.Point(206, 488)
        Me.txt_Mrp.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txt_Mrp.MaxLength = 12
        Me.txt_Mrp.Name = "txt_Mrp"
        Me.txt_Mrp.Size = New System.Drawing.Size(223, 31)
        Me.txt_Mrp.TabIndex = 15
        Me.txt_Mrp.Visible = False
        '
        'lbl_mrp_Caption
        '
        Me.lbl_mrp_Caption.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_mrp_Caption.ForeColor = System.Drawing.Color.Blue
        Me.lbl_mrp_Caption.Location = New System.Drawing.Point(28, 491)
        Me.lbl_mrp_Caption.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_mrp_Caption.Name = "lbl_mrp_Caption"
        Me.lbl_mrp_Caption.Size = New System.Drawing.Size(154, 28)
        Me.lbl_mrp_Caption.TabIndex = 84
        Me.lbl_mrp_Caption.Text = "MRP ( Rate ) "
        Me.lbl_mrp_Caption.Visible = False
        '
        'lbl_description_Caption
        '
        Me.lbl_description_Caption.AutoSize = True
        Me.lbl_description_Caption.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_description_Caption.ForeColor = System.Drawing.Color.Blue
        Me.lbl_description_Caption.Location = New System.Drawing.Point(28, 546)
        Me.lbl_description_Caption.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_description_Caption.Name = "lbl_description_Caption"
        Me.lbl_description_Caption.Size = New System.Drawing.Size(118, 24)
        Me.lbl_description_Caption.TabIndex = 82
        Me.lbl_description_Caption.Text = "Name  Tamil "
        Me.lbl_description_Caption.Visible = False
        '
        'txt_MinimumStock
        '
        Me.txt_MinimumStock.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_MinimumStock.Location = New System.Drawing.Point(620, 278)
        Me.txt_MinimumStock.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txt_MinimumStock.MaxLength = 12
        Me.txt_MinimumStock.Name = "txt_MinimumStock"
        Me.txt_MinimumStock.Size = New System.Drawing.Size(277, 31)
        Me.txt_MinimumStock.TabIndex = 8
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.Blue
        Me.Label11.Location = New System.Drawing.Point(458, 285)
        Me.Label11.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(146, 24)
        Me.Label11.TabIndex = 80
        Me.Label11.Text = "Minimum Stock "
        '
        'txt_CostRate_Incl_Tax
        '
        Me.txt_CostRate_Incl_Tax.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_CostRate_Incl_Tax.Location = New System.Drawing.Point(651, 383)
        Me.txt_CostRate_Incl_Tax.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txt_CostRate_Incl_Tax.MaxLength = 12
        Me.txt_CostRate_Incl_Tax.Name = "txt_CostRate_Incl_Tax"
        Me.txt_CostRate_Incl_Tax.Size = New System.Drawing.Size(247, 31)
        Me.txt_CostRate_Incl_Tax.TabIndex = 12
        '
        'txt_Code
        '
        Me.txt_Code.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_Code.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Code.Location = New System.Drawing.Point(206, 174)
        Me.txt_Code.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txt_Code.MaxLength = 20
        Me.txt_Code.Name = "txt_Code"
        Me.txt_Code.Size = New System.Drawing.Size(691, 31)
        Me.txt_Code.TabIndex = 3
        '
        'lbl_Code_Caption
        '
        Me.lbl_Code_Caption.AutoSize = True
        Me.lbl_Code_Caption.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Code_Caption.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Code_Caption.Location = New System.Drawing.Point(28, 180)
        Me.lbl_Code_Caption.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_Code_Caption.Name = "lbl_Code_Caption"
        Me.lbl_Code_Caption.Size = New System.Drawing.Size(86, 24)
        Me.lbl_Code_Caption.TabIndex = 74
        Me.lbl_Code_Caption.Text = "Bar Code"
        '
        'cbo_Unit
        '
        Me.cbo_Unit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Unit.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Unit.FormattingEnabled = True
        Me.cbo_Unit.Location = New System.Drawing.Point(206, 278)
        Me.cbo_Unit.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cbo_Unit.Name = "cbo_Unit"
        Me.cbo_Unit.Size = New System.Drawing.Size(223, 32)
        Me.cbo_Unit.TabIndex = 7
        '
        'cbo_ItemGroup
        '
        Me.cbo_ItemGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_ItemGroup.DropDownHeight = 200
        Me.cbo_ItemGroup.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_ItemGroup.FormattingEnabled = True
        Me.cbo_ItemGroup.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cbo_ItemGroup.IntegralHeight = False
        Me.cbo_ItemGroup.Location = New System.Drawing.Point(206, 226)
        Me.cbo_ItemGroup.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cbo_ItemGroup.Name = "cbo_ItemGroup"
        Me.cbo_ItemGroup.Size = New System.Drawing.Size(691, 32)
        Me.cbo_ItemGroup.TabIndex = 5
        Me.cbo_ItemGroup.Text = "cbo_ItemGroup"
        '
        'txt_SalesRate_Excl_Tax
        '
        Me.txt_SalesRate_Excl_Tax.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_SalesRate_Excl_Tax.Location = New System.Drawing.Point(206, 435)
        Me.txt_SalesRate_Excl_Tax.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txt_SalesRate_Excl_Tax.MaxLength = 12
        Me.txt_SalesRate_Excl_Tax.Name = "txt_SalesRate_Excl_Tax"
        Me.txt_SalesRate_Excl_Tax.Size = New System.Drawing.Size(223, 31)
        Me.txt_SalesRate_Excl_Tax.TabIndex = 13
        '
        'txt_Name
        '
        Me.txt_Name.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_Name.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Name.Location = New System.Drawing.Point(206, 69)
        Me.txt_Name.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txt_Name.MaxLength = 50
        Me.txt_Name.Name = "txt_Name"
        Me.txt_Name.Size = New System.Drawing.Size(691, 31)
        Me.txt_Name.TabIndex = 0
        '
        'lbl_sales_Rate_Excl_Tax_Caption
        '
        Me.lbl_sales_Rate_Excl_Tax_Caption.AutoSize = True
        Me.lbl_sales_Rate_Excl_Tax_Caption.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_sales_Rate_Excl_Tax_Caption.ForeColor = System.Drawing.Color.Blue
        Me.lbl_sales_Rate_Excl_Tax_Caption.Location = New System.Drawing.Point(28, 442)
        Me.lbl_sales_Rate_Excl_Tax_Caption.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_sales_Rate_Excl_Tax_Caption.Name = "lbl_sales_Rate_Excl_Tax_Caption"
        Me.lbl_sales_Rate_Excl_Tax_Caption.Size = New System.Drawing.Size(175, 24)
        Me.lbl_sales_Rate_Excl_Tax_Caption.TabIndex = 63
        Me.lbl_sales_Rate_Excl_Tax_Caption.Text = "Sales Rate (Excl.Tax)"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(28, 285)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(57, 24)
        Me.Label4.TabIndex = 60
        Me.Label4.Text = "Unit :"
        '
        'lbl_DealerName
        '
        Me.lbl_DealerName.AutoSize = True
        Me.lbl_DealerName.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_DealerName.ForeColor = System.Drawing.Color.Blue
        Me.lbl_DealerName.Location = New System.Drawing.Point(460, 232)
        Me.lbl_DealerName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_DealerName.Name = "lbl_DealerName"
        Me.lbl_DealerName.Size = New System.Drawing.Size(119, 24)
        Me.lbl_DealerName.TabIndex = 64
        Me.lbl_DealerName.Text = "Dealer Name"
        Me.lbl_DealerName.Visible = False
        '
        'lbl_ItemGroup_Caption
        '
        Me.lbl_ItemGroup_Caption.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_ItemGroup_Caption.ForeColor = System.Drawing.Color.Blue
        Me.lbl_ItemGroup_Caption.Location = New System.Drawing.Point(28, 220)
        Me.lbl_ItemGroup_Caption.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_ItemGroup_Caption.Name = "lbl_ItemGroup_Caption"
        Me.lbl_ItemGroup_Caption.Size = New System.Drawing.Size(134, 46)
        Me.lbl_ItemGroup_Caption.TabIndex = 64
        Me.lbl_ItemGroup_Caption.Text = "Item Group (HSN CODE)"
        '
        'lbl_Name_Caption
        '
        Me.lbl_Name_Caption.AutoSize = True
        Me.lbl_Name_Caption.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Name_Caption.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Name_Caption.Location = New System.Drawing.Point(28, 75)
        Me.lbl_Name_Caption.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_Name_Caption.Name = "lbl_Name_Caption"
        Me.lbl_Name_Caption.Size = New System.Drawing.Size(59, 24)
        Me.lbl_Name_Caption.TabIndex = 67
        Me.lbl_Name_Caption.Text = "Name"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(28, 23)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(50, 24)
        Me.Label1.TabIndex = 65
        Me.Label1.Text = "IdNo"
        '
        'lbl_StyleCaption
        '
        Me.lbl_StyleCaption.AutoSize = True
        Me.lbl_StyleCaption.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_StyleCaption.ForeColor = System.Drawing.Color.Blue
        Me.lbl_StyleCaption.Location = New System.Drawing.Point(28, 128)
        Me.lbl_StyleCaption.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_StyleCaption.Name = "lbl_StyleCaption"
        Me.lbl_StyleCaption.Size = New System.Drawing.Size(50, 24)
        Me.lbl_StyleCaption.TabIndex = 107
        Me.lbl_StyleCaption.Text = "Style"
        Me.lbl_StyleCaption.Visible = False
        '
        'lbl_tamilname_Caption
        '
        Me.lbl_tamilname_Caption.AutoSize = True
        Me.lbl_tamilname_Caption.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.lbl_tamilname_Caption.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_tamilname_Caption.ForeColor = System.Drawing.Color.Blue
        Me.lbl_tamilname_Caption.Location = New System.Drawing.Point(28, 128)
        Me.lbl_tamilname_Caption.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_tamilname_Caption.Name = "lbl_tamilname_Caption"
        Me.lbl_tamilname_Caption.Size = New System.Drawing.Size(108, 24)
        Me.lbl_tamilname_Caption.TabIndex = 117
        Me.lbl_tamilname_Caption.Text = "Tamil Name"
        Me.lbl_tamilname_Caption.Visible = False
        '
        'txt_VatTaxRate
        '
        Me.txt_VatTaxRate.BackColor = System.Drawing.Color.Red
        Me.txt_VatTaxRate.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_VatTaxRate.Location = New System.Drawing.Point(1148, 137)
        Me.txt_VatTaxRate.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txt_VatTaxRate.MaxLength = 12
        Me.txt_VatTaxRate.Name = "txt_VatTaxRate"
        Me.txt_VatTaxRate.Size = New System.Drawing.Size(238, 31)
        Me.txt_VatTaxRate.TabIndex = 81
        Me.txt_VatTaxRate.Visible = False
        '
        'txt_VatTaxPerc
        '
        Me.txt_VatTaxPerc.BackColor = System.Drawing.Color.Red
        Me.txt_VatTaxPerc.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_VatTaxPerc.Location = New System.Drawing.Point(1148, 62)
        Me.txt_VatTaxPerc.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txt_VatTaxPerc.MaxLength = 6
        Me.txt_VatTaxPerc.Name = "txt_VatTaxPerc"
        Me.txt_VatTaxPerc.Size = New System.Drawing.Size(238, 31)
        Me.txt_VatTaxPerc.TabIndex = 79
        Me.txt_VatTaxPerc.Visible = False
        '
        'lbl_sales_Rate_Vat_Caption
        '
        Me.lbl_sales_Rate_Vat_Caption.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_sales_Rate_Vat_Caption.ForeColor = System.Drawing.Color.Blue
        Me.lbl_sales_Rate_Vat_Caption.Location = New System.Drawing.Point(1022, 131)
        Me.lbl_sales_Rate_Vat_Caption.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_sales_Rate_Vat_Caption.Name = "lbl_sales_Rate_Vat_Caption"
        Me.lbl_sales_Rate_Vat_Caption.Size = New System.Drawing.Size(117, 54)
        Me.lbl_sales_Rate_Vat_Caption.TabIndex = 62
        Me.lbl_sales_Rate_Vat_Caption.Text = "Sales Rate       ( Incl. VAT )"
        Me.lbl_sales_Rate_Vat_Caption.Visible = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Blue
        Me.Label5.Location = New System.Drawing.Point(1059, 66)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(66, 24)
        Me.Label5.TabIndex = 61
        Me.Label5.Text = "VAT  %"
        Me.Label5.Visible = False
        '
        'Item_Creation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(966, 863)
        Me.Controls.Add(Me.pnl_Back)
        Me.Controls.Add(Me.grp_Filter)
        Me.Controls.Add(Me.grp_Open)
        Me.Controls.Add(Me.lbl_FormHeading)
        Me.Controls.Add(Me.txt_VatTaxPerc)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txt_VatTaxRate)
        Me.Controls.Add(Me.lbl_sales_Rate_Vat_Caption)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Item_Creation"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ITEM CREATION"
        Me.grp_Open.ResumeLayout(False)
        Me.grp_Filter.ResumeLayout(False)
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_Back.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents grp_Open As System.Windows.Forms.GroupBox
    Friend WithEvents btn_Find As System.Windows.Forms.Button
    Friend WithEvents cbo_Open As System.Windows.Forms.ComboBox
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents grp_Filter As System.Windows.Forms.GroupBox
    Friend WithEvents dgv_Filter As System.Windows.Forms.DataGridView
    Friend WithEvents btn_CloseFilter As System.Windows.Forms.Button
    Friend WithEvents btn_OpenFilter As System.Windows.Forms.Button
    Friend WithEvents lbl_FormHeading As System.Windows.Forms.Label
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents cbo_Size As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_Style As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_SizeCaption As System.Windows.Forms.Label
    Friend WithEvents lbl_StyleCaption As System.Windows.Forms.Label
    Friend WithEvents btn_SaveAll As System.Windows.Forms.Button
    Friend WithEvents txt_SalesRate_Wholesale As System.Windows.Forms.TextBox
    Friend WithEvents lbl_SalesRate_WholeSale As System.Windows.Forms.Label
    Friend WithEvents txt_SalesProfit_Wholesale As System.Windows.Forms.TextBox
    Friend WithEvents lbl_SalesProfit_Wholesale As System.Windows.Forms.Label
    Friend WithEvents txt_SalesRate_Retail As System.Windows.Forms.TextBox
    Friend WithEvents lbl_SalesRate_Retail As System.Windows.Forms.Label
    Friend WithEvents txt_SalesProfit_Retail As System.Windows.Forms.TextBox
    Friend WithEvents lbl_SalesProfit_Retail As System.Windows.Forms.Label
    Friend WithEvents txt_Rack_No As System.Windows.Forms.TextBox
    Friend WithEvents txt_DiscountPercentage As System.Windows.Forms.TextBox
    Friend WithEvents lbl_DiscountPercCaption As System.Windows.Forms.Label
    Friend WithEvents txt_description As System.Windows.Forms.TextBox
    Friend WithEvents btn_fromExcel As System.Windows.Forms.Button
    Friend WithEvents txt_Sales_GSTRate As System.Windows.Forms.TextBox
    Friend WithEvents txt_GSTTaxPerc As System.Windows.Forms.TextBox
    Friend WithEvents lbl_Sales_Rate_GST_Caption As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents btn_save As System.Windows.Forms.Button
    Friend WithEvents chk_JobWorkStatus As System.Windows.Forms.CheckBox
    Friend WithEvents txt_Mrp As System.Windows.Forms.TextBox
    Friend WithEvents lbl_mrp_Caption As System.Windows.Forms.Label
    Friend WithEvents txt_TamilName As System.Windows.Forms.TextBox
    Friend WithEvents lbl_description_Caption As System.Windows.Forms.Label
    Friend WithEvents txt_MinimumStock As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txt_CostRate_Incl_Tax As System.Windows.Forms.TextBox
    Friend WithEvents txt_Code As System.Windows.Forms.TextBox
    Friend WithEvents lbl_Code_Caption As System.Windows.Forms.Label
    Friend WithEvents cbo_Unit As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_ItemGroup As System.Windows.Forms.ComboBox
    Friend WithEvents txt_VatTaxRate As System.Windows.Forms.TextBox
    Friend WithEvents txt_SalesRate_Excl_Tax As System.Windows.Forms.TextBox
    Friend WithEvents txt_VatTaxPerc As System.Windows.Forms.TextBox
    Friend WithEvents lbl_sales_Rate_Vat_Caption As System.Windows.Forms.Label
    Friend WithEvents txt_Name As System.Windows.Forms.TextBox
    Friend WithEvents lbl_sales_Rate_Excl_Tax_Caption As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lbl_ItemGroup_Caption As System.Windows.Forms.Label
    Friend WithEvents lbl_Name_Caption As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lbl_DealerName As System.Windows.Forms.Label
    Friend WithEvents cbo_DealerName As System.Windows.Forms.ComboBox
    Friend WithEvents txt_CostRate_Excl_Tax As System.Windows.Forms.TextBox
    Friend WithEvents lbl_CostRate_Excl_Tax_Caption As System.Windows.Forms.Label
    Friend WithEvents lbl_CostRate_Incl_Tax_Caption As System.Windows.Forms.Label
    Friend WithEvents lbl_IdNo As System.Windows.Forms.Label
    Friend WithEvents chk_Close_Status As System.Windows.Forms.CheckBox
    Friend WithEvents txt_HSNCode As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lbl_tamilname_Caption As System.Windows.Forms.Label
    Friend WithEvents btn_Character As System.Windows.Forms.Button
End Class
