<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Sizing_Ledger_Rate_Details
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
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.lbl_IdNo = New System.Windows.Forms.Label()
        Me.cbo_Ledger = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument()
        Me.lbl_Heading = New System.Windows.Forms.Label()
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.txt_DiscountRate = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cbo_DiscountType = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txt_WeldingCharge = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txt_RewindingCharge = New System.Windows.Forms.TextBox()
        Me.txt_PackingCharge = New System.Windows.Forms.TextBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.tab_Main = New System.Windows.Forms.TabControl()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.cbo_Count = New System.Windows.Forms.ComboBox()
        Me.dgv_RateDetails = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumn9 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn22 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn23 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn24 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn25 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lbl_Company = New System.Windows.Forms.Label()
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog()
        Me.pnl_Back.SuspendLayout()
        Me.tab_Main.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        CType(Me.dgv_RateDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lbl_IdNo
        '
        Me.lbl_IdNo.BackColor = System.Drawing.Color.White
        Me.lbl_IdNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_IdNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_IdNo.Location = New System.Drawing.Point(142, 13)
        Me.lbl_IdNo.Name = "lbl_IdNo"
        Me.lbl_IdNo.Size = New System.Drawing.Size(365, 23)
        Me.lbl_IdNo.TabIndex = 0
        Me.lbl_IdNo.Text = "lbl_IdNo"
        Me.lbl_IdNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbo_Ledger
        '
        Me.cbo_Ledger.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Ledger.FormattingEnabled = True
        Me.cbo_Ledger.Location = New System.Drawing.Point(142, 43)
        Me.cbo_Ledger.MaxDropDownItems = 15
        Me.cbo_Ledger.Name = "cbo_Ledger"
        Me.cbo_Ledger.Size = New System.Drawing.Size(365, 23)
        Me.cbo_Ledger.Sorted = True
        Me.cbo_Ledger.TabIndex = 0
        Me.cbo_Ledger.Text = "cbo_Ledger"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Blue
        Me.Label5.Location = New System.Drawing.Point(19, 47)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(88, 15)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "Ledger Name :"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(19, 17)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(40, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "IdNo :"
        '
        'lbl_Heading
        '
        Me.lbl_Heading.AutoEllipsis = True
        Me.lbl_Heading.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.lbl_Heading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_Heading.Dock = System.Windows.Forms.DockStyle.Top
        Me.lbl_Heading.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Heading.ForeColor = System.Drawing.Color.White
        Me.lbl_Heading.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lbl_Heading.Location = New System.Drawing.Point(0, 0)
        Me.lbl_Heading.Name = "lbl_Heading"
        Me.lbl_Heading.Size = New System.Drawing.Size(552, 30)
        Me.lbl_Heading.TabIndex = 37
        Me.lbl_Heading.Text = "LEDGER RATE DETAILS"
        Me.lbl_Heading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnl_Back
        '
        Me.pnl_Back.AutoScroll = True
        Me.pnl_Back.AutoSize = True
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.txt_DiscountRate)
        Me.pnl_Back.Controls.Add(Me.Label4)
        Me.pnl_Back.Controls.Add(Me.cbo_DiscountType)
        Me.pnl_Back.Controls.Add(Me.Label3)
        Me.pnl_Back.Controls.Add(Me.txt_WeldingCharge)
        Me.pnl_Back.Controls.Add(Me.Label1)
        Me.pnl_Back.Controls.Add(Me.Label10)
        Me.pnl_Back.Controls.Add(Me.txt_RewindingCharge)
        Me.pnl_Back.Controls.Add(Me.txt_PackingCharge)
        Me.pnl_Back.Controls.Add(Me.btnSave)
        Me.pnl_Back.Controls.Add(Me.btnClose)
        Me.pnl_Back.Controls.Add(Me.tab_Main)
        Me.pnl_Back.Controls.Add(Me.lbl_IdNo)
        Me.pnl_Back.Controls.Add(Me.cbo_Ledger)
        Me.pnl_Back.Controls.Add(Me.Label5)
        Me.pnl_Back.Controls.Add(Me.Label2)
        Me.pnl_Back.Enabled = False
        Me.pnl_Back.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnl_Back.Location = New System.Drawing.Point(6, 42)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(529, 479)
        Me.pnl_Back.TabIndex = 36
        '
        'txt_DiscountRate
        '
        Me.txt_DiscountRate.Location = New System.Drawing.Point(431, 397)
        Me.txt_DiscountRate.MaxLength = 20
        Me.txt_DiscountRate.Name = "txt_DiscountRate"
        Me.txt_DiscountRate.Size = New System.Drawing.Size(76, 23)
        Me.txt_DiscountRate.TabIndex = 10
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(253, 400)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(82, 15)
        Me.Label4.TabIndex = 165
        Me.Label4.Text = "Cash Discount"
        '
        'cbo_DiscountType
        '
        Me.cbo_DiscountType.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_DiscountType.FormattingEnabled = True
        Me.cbo_DiscountType.Location = New System.Drawing.Point(348, 396)
        Me.cbo_DiscountType.MaxDropDownItems = 15
        Me.cbo_DiscountType.Name = "cbo_DiscountType"
        Me.cbo_DiscountType.Size = New System.Drawing.Size(80, 23)
        Me.cbo_DiscountType.Sorted = True
        Me.cbo_DiscountType.TabIndex = 9
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Blue
        Me.Label3.Location = New System.Drawing.Point(13, 400)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(93, 15)
        Me.Label3.TabIndex = 163
        Me.Label3.Text = "Welding Charge"
        '
        'txt_WeldingCharge
        '
        Me.txt_WeldingCharge.Location = New System.Drawing.Point(128, 396)
        Me.txt_WeldingCharge.MaxLength = 20
        Me.txt_WeldingCharge.Name = "txt_WeldingCharge"
        Me.txt_WeldingCharge.Size = New System.Drawing.Size(117, 23)
        Me.txt_WeldingCharge.TabIndex = 8
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(253, 364)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 15)
        Me.Label1.TabIndex = 161
        Me.Label1.Text = "Packing Charge"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.Blue
        Me.Label10.Location = New System.Drawing.Point(13, 364)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(105, 15)
        Me.Label10.TabIndex = 160
        Me.Label10.Text = "Rewinding Charge"
        '
        'txt_RewindingCharge
        '
        Me.txt_RewindingCharge.Location = New System.Drawing.Point(128, 360)
        Me.txt_RewindingCharge.MaxLength = 20
        Me.txt_RewindingCharge.Name = "txt_RewindingCharge"
        Me.txt_RewindingCharge.Size = New System.Drawing.Size(117, 23)
        Me.txt_RewindingCharge.TabIndex = 6
        '
        'txt_PackingCharge
        '
        Me.txt_PackingCharge.Location = New System.Drawing.Point(348, 360)
        Me.txt_PackingCharge.MaxLength = 20
        Me.txt_PackingCharge.Name = "txt_PackingCharge"
        Me.txt_PackingCharge.Size = New System.Drawing.Size(159, 23)
        Me.txt_PackingCharge.TabIndex = 7
        '
        'btnSave
        '
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ForeColor = System.Drawing.Color.Navy
        Me.btnSave.Image = Global.Textile.My.Resources.Resources.SAVE1
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(338, 432)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(76, 31)
        Me.btnSave.TabIndex = 8
        Me.btnSave.TabStop = False
        Me.btnSave.Text = "Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ForeColor = System.Drawing.Color.Navy
        Me.btnClose.Image = Global.Textile.My.Resources.Resources.cancel1
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(431, 433)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(76, 31)
        Me.btnClose.TabIndex = 9
        Me.btnClose.TabStop = False
        Me.btnClose.Text = "Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'tab_Main
        '
        Me.tab_Main.Controls.Add(Me.TabPage3)
        Me.tab_Main.Location = New System.Drawing.Point(22, 85)
        Me.tab_Main.Name = "tab_Main"
        Me.tab_Main.SelectedIndex = 0
        Me.tab_Main.Size = New System.Drawing.Size(485, 262)
        Me.tab_Main.TabIndex = 41
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.cbo_Count)
        Me.TabPage3.Controls.Add(Me.dgv_RateDetails)
        Me.TabPage3.Location = New System.Drawing.Point(4, 24)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(477, 234)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "RATE DETAILS"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'cbo_Count
        '
        Me.cbo_Count.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Count.FormattingEnabled = True
        Me.cbo_Count.Location = New System.Drawing.Point(33, 63)
        Me.cbo_Count.Name = "cbo_Count"
        Me.cbo_Count.Size = New System.Drawing.Size(182, 23)
        Me.cbo_Count.Sorted = True
        Me.cbo_Count.TabIndex = 12
        Me.cbo_Count.Text = "cbo_Count"
        Me.cbo_Count.Visible = False
        '
        'dgv_RateDetails
        '
        Me.dgv_RateDetails.AllowUserToResizeColumns = False
        Me.dgv_RateDetails.AllowUserToResizeRows = False
        Me.dgv_RateDetails.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_RateDetails.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgv_RateDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgv_RateDetails.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn9, Me.DataGridViewTextBoxColumn22, Me.DataGridViewTextBoxColumn23, Me.DataGridViewTextBoxColumn24, Me.DataGridViewTextBoxColumn25})
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.Lime
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgv_RateDetails.DefaultCellStyle = DataGridViewCellStyle7
        Me.dgv_RateDetails.Dock = System.Windows.Forms.DockStyle.Top
        Me.dgv_RateDetails.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.dgv_RateDetails.Location = New System.Drawing.Point(3, 3)
        Me.dgv_RateDetails.MultiSelect = False
        Me.dgv_RateDetails.Name = "dgv_RateDetails"
        Me.dgv_RateDetails.RowHeadersVisible = False
        Me.dgv_RateDetails.RowHeadersWidth = 15
        Me.dgv_RateDetails.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgv_RateDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_RateDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.dgv_RateDetails.Size = New System.Drawing.Size(471, 226)
        Me.dgv_RateDetails.TabIndex = 14
        Me.dgv_RateDetails.TabStop = False
        '
        'DataGridViewTextBoxColumn9
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGridViewTextBoxColumn9.DefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridViewTextBoxColumn9.Frozen = True
        Me.DataGridViewTextBoxColumn9.HeaderText = "S.NO"
        Me.DataGridViewTextBoxColumn9.Name = "DataGridViewTextBoxColumn9"
        Me.DataGridViewTextBoxColumn9.ReadOnly = True
        Me.DataGridViewTextBoxColumn9.Width = 45
        '
        'DataGridViewTextBoxColumn22
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.DataGridViewTextBoxColumn22.DefaultCellStyle = DataGridViewCellStyle3
        Me.DataGridViewTextBoxColumn22.HeaderText = "COUNT"
        Me.DataGridViewTextBoxColumn22.Name = "DataGridViewTextBoxColumn22"
        Me.DataGridViewTextBoxColumn22.ReadOnly = True
        '
        'DataGridViewTextBoxColumn23
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.DataGridViewTextBoxColumn23.DefaultCellStyle = DataGridViewCellStyle4
        Me.DataGridViewTextBoxColumn23.HeaderText = "ENDS FROM"
        Me.DataGridViewTextBoxColumn23.MaxInputLength = 20
        Me.DataGridViewTextBoxColumn23.Name = "DataGridViewTextBoxColumn23"
        '
        'DataGridViewTextBoxColumn24
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.DataGridViewTextBoxColumn24.DefaultCellStyle = DataGridViewCellStyle5
        Me.DataGridViewTextBoxColumn24.HeaderText = "ENDS TO"
        Me.DataGridViewTextBoxColumn24.MaxInputLength = 20
        Me.DataGridViewTextBoxColumn24.Name = "DataGridViewTextBoxColumn24"
        '
        'DataGridViewTextBoxColumn25
        '
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle6.NullValue = Nothing
        Me.DataGridViewTextBoxColumn25.DefaultCellStyle = DataGridViewCellStyle6
        Me.DataGridViewTextBoxColumn25.HeaderText = "RATE"
        Me.DataGridViewTextBoxColumn25.MaxInputLength = 15
        Me.DataGridViewTextBoxColumn25.Name = "DataGridViewTextBoxColumn25"
        '
        'lbl_Company
        '
        Me.lbl_Company.AutoSize = True
        Me.lbl_Company.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lbl_Company.ForeColor = System.Drawing.Color.Red
        Me.lbl_Company.Location = New System.Drawing.Point(482, 30)
        Me.lbl_Company.Name = "lbl_Company"
        Me.lbl_Company.Size = New System.Drawing.Size(77, 15)
        Me.lbl_Company.TabIndex = 33
        Me.lbl_Company.Text = "lbl_Company"
        Me.lbl_Company.Visible = False
        '
        'PrintDialog1
        '
        Me.PrintDialog1.UseEXDialog = True
        '
        'Sizing_Ledger_Rate_Details
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(552, 537)
        Me.Controls.Add(Me.lbl_Heading)
        Me.Controls.Add(Me.pnl_Back)
        Me.Controls.Add(Me.lbl_Company)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "Sizing_Ledger_Rate_Details"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "LEDGER RATE DETAILS"
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_Back.PerformLayout()
        Me.tab_Main.ResumeLayout(False)
        Me.TabPage3.ResumeLayout(False)
        CType(Me.dgv_RateDetails, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lbl_IdNo As System.Windows.Forms.Label
    Friend WithEvents cbo_Ledger As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents PrintDocument1 As System.Drawing.Printing.PrintDocument
    Friend WithEvents lbl_Heading As System.Windows.Forms.Label
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents lbl_Company As System.Windows.Forms.Label
    Friend WithEvents tab_Main As System.Windows.Forms.TabControl
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents cbo_Count As System.Windows.Forms.ComboBox
    Friend WithEvents dgv_RateDetails As System.Windows.Forms.DataGridView
    Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cbo_DiscountType As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txt_WeldingCharge As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txt_RewindingCharge As System.Windows.Forms.TextBox
    Friend WithEvents txt_PackingCharge As System.Windows.Forms.TextBox
    Friend WithEvents DataGridViewTextBoxColumn9 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn22 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn23 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn24 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn25 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_DiscountRate As System.Windows.Forms.TextBox
End Class
