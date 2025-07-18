<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Yarn_Lot_Creation
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
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog()
        Me.cbo_Filter_Count = New System.Windows.Forms.ComboBox()
        Me.btn_closefilter = New System.Windows.Forms.Button()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.btn_filtershow = New System.Windows.Forms.Button()
        Me.dgv_filter = New System.Windows.Forms.DataGridView()
        Me.dc = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument()
        Me.btn_close = New System.Windows.Forms.Button()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.pnl_filter = New System.Windows.Forms.Panel()
        Me.cbo_Filter_Mill = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.btn_save = New System.Windows.Forms.Button()
        Me.pnl_back = New System.Windows.Forms.Panel()
        Me.txt_LotNo = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.cbo_Mill = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txt_Rate = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.lbl_RefNo = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cbo_Count = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lbl_Company = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        CType(Me.dgv_filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnl_filter.SuspendLayout()
        Me.pnl_back.SuspendLayout()
        Me.SuspendLayout()
        '
        'PrintDialog1
        '
        Me.PrintDialog1.UseEXDialog = True
        '
        'cbo_Filter_Count
        '
        Me.cbo_Filter_Count.FormattingEnabled = True
        Me.cbo_Filter_Count.Location = New System.Drawing.Point(93, 49)
        Me.cbo_Filter_Count.Name = "cbo_Filter_Count"
        Me.cbo_Filter_Count.Size = New System.Drawing.Size(245, 23)
        Me.cbo_Filter_Count.TabIndex = 2
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
        Me.dgv_filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_filter.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.dc, Me.Column2, Me.Column5, Me.Column1})
        Me.dgv_filter.Location = New System.Drawing.Point(12, 107)
        Me.dgv_filter.Name = "dgv_filter"
        Me.dgv_filter.RowHeadersVisible = False
        Me.dgv_filter.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_filter.Size = New System.Drawing.Size(531, 163)
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
        'Column2
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle2
        Me.Column2.HeaderText = "LOT No"
        Me.Column2.MaxInputLength = 35
        Me.Column2.Name = "Column2"
        '
        'Column5
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column5.DefaultCellStyle = DataGridViewCellStyle3
        Me.Column5.HeaderText = "COUNT"
        Me.Column5.Name = "Column5"
        '
        'Column1
        '
        Me.Column1.HeaderText = "MILL"
        Me.Column1.Name = "Column1"
        Me.Column1.Width = 200
        '
        'PrintDocument1
        '
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(502, 172)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(73, 30)
        Me.btn_close.TabIndex = 15
        Me.btn_close.TabStop = False
        Me.btn_close.Text = "&CLOSE"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.Label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label11.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label11.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.White
        Me.Label11.Location = New System.Drawing.Point(0, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(596, 35)
        Me.Label11.TabIndex = 27
        Me.Label11.Text = "YARN LOT CREATION"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(12, 53)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(40, 15)
        Me.Label15.TabIndex = 2
        Me.Label15.Text = "Count"
        '
        'pnl_filter
        '
        Me.pnl_filter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_filter.Controls.Add(Me.cbo_Filter_Mill)
        Me.pnl_filter.Controls.Add(Me.Label6)
        Me.pnl_filter.Controls.Add(Me.btn_closefilter)
        Me.pnl_filter.Controls.Add(Me.Label16)
        Me.pnl_filter.Controls.Add(Me.btn_filtershow)
        Me.pnl_filter.Controls.Add(Me.dgv_filter)
        Me.pnl_filter.Controls.Add(Me.cbo_Filter_Count)
        Me.pnl_filter.Controls.Add(Me.Label15)
        Me.pnl_filter.Location = New System.Drawing.Point(20, 359)
        Me.pnl_filter.Name = "pnl_filter"
        Me.pnl_filter.Size = New System.Drawing.Size(562, 284)
        Me.pnl_filter.TabIndex = 28
        '
        'cbo_Filter_Mill
        '
        Me.cbo_Filter_Mill.FormattingEnabled = True
        Me.cbo_Filter_Mill.Location = New System.Drawing.Point(93, 78)
        Me.cbo_Filter_Mill.Name = "cbo_Filter_Mill"
        Me.cbo_Filter_Mill.Size = New System.Drawing.Size(245, 23)
        Me.cbo_Filter_Mill.TabIndex = 9
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(11, 82)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(63, 15)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "Mill Name"
        '
        'btn_save
        '
        Me.btn_save.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_save.ForeColor = System.Drawing.Color.White
        Me.btn_save.Location = New System.Drawing.Point(429, 172)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(73, 30)
        Me.btn_save.TabIndex = 13
        Me.btn_save.TabStop = False
        Me.btn_save.Text = "&SAVE"
        Me.btn_save.UseVisualStyleBackColor = False
        '
        'pnl_back
        '
        Me.pnl_back.AutoSize = True
        Me.pnl_back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_back.Controls.Add(Me.Label7)
        Me.pnl_back.Controls.Add(Me.Label4)
        Me.pnl_back.Controls.Add(Me.txt_LotNo)
        Me.pnl_back.Controls.Add(Me.Label2)
        Me.pnl_back.Controls.Add(Me.Label17)
        Me.pnl_back.Controls.Add(Me.cbo_Mill)
        Me.pnl_back.Controls.Add(Me.Label5)
        Me.pnl_back.Controls.Add(Me.btn_close)
        Me.pnl_back.Controls.Add(Me.btn_save)
        Me.pnl_back.Controls.Add(Me.txt_Rate)
        Me.pnl_back.Controls.Add(Me.Label10)
        Me.pnl_back.Controls.Add(Me.lbl_RefNo)
        Me.pnl_back.Controls.Add(Me.Label8)
        Me.pnl_back.Controls.Add(Me.cbo_Count)
        Me.pnl_back.Controls.Add(Me.Label1)
        Me.pnl_back.Controls.Add(Me.Label3)
        Me.pnl_back.Location = New System.Drawing.Point(6, 42)
        Me.pnl_back.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.pnl_back.Name = "pnl_back"
        Me.pnl_back.Size = New System.Drawing.Size(586, 216)
        Me.pnl_back.TabIndex = 26
        '
        'txt_LotNo
        '
        Me.txt_LotNo.Location = New System.Drawing.Point(393, 13)
        Me.txt_LotNo.MaxLength = 15
        Me.txt_LotNo.Name = "txt_LotNo"
        Me.txt_LotNo.Size = New System.Drawing.Size(182, 23)
        Me.txt_LotNo.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(308, 17)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(44, 15)
        Me.Label2.TabIndex = 307
        Me.Label2.Text = "Lot No"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.ForeColor = System.Drawing.Color.Blue
        Me.Label17.Location = New System.Drawing.Point(11, 97)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(58, 15)
        Me.Label17.TabIndex = 305
        Me.Label17.Text = "Millname"
        '
        'cbo_Mill
        '
        Me.cbo_Mill.DropDownHeight = 175
        Me.cbo_Mill.FormattingEnabled = True
        Me.cbo_Mill.IntegralHeight = False
        Me.cbo_Mill.Location = New System.Drawing.Point(111, 93)
        Me.cbo_Mill.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.cbo_Mill.MaxLength = 35
        Me.cbo_Mill.Name = "cbo_Mill"
        Me.cbo_Mill.Size = New System.Drawing.Size(464, 23)
        Me.cbo_Mill.TabIndex = 304
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Red
        Me.Label5.Location = New System.Drawing.Point(51, 53)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(13, 15)
        Me.Label5.TabIndex = 302
        Me.Label5.Text = "*"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_Rate
        '
        Me.txt_Rate.Location = New System.Drawing.Point(110, 134)
        Me.txt_Rate.MaxLength = 20
        Me.txt_Rate.Name = "txt_Rate"
        Me.txt_Rate.Size = New System.Drawing.Size(182, 23)
        Me.txt_Rate.TabIndex = 3
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.ForeColor = System.Drawing.Color.Blue
        Me.Label10.Location = New System.Drawing.Point(15, 138)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(32, 15)
        Me.Label10.TabIndex = 27
        Me.Label10.Text = "Rate"
        '
        'lbl_RefNo
        '
        Me.lbl_RefNo.BackColor = System.Drawing.Color.White
        Me.lbl_RefNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_RefNo.Location = New System.Drawing.Point(111, 13)
        Me.lbl_RefNo.Name = "lbl_RefNo"
        Me.lbl_RefNo.Size = New System.Drawing.Size(167, 23)
        Me.lbl_RefNo.TabIndex = 21
        Me.lbl_RefNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ForeColor = System.Drawing.Color.Blue
        Me.Label8.Location = New System.Drawing.Point(11, 57)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(40, 15)
        Me.Label8.TabIndex = 14
        Me.Label8.Text = "Count"
        '
        'cbo_Count
        '
        Me.cbo_Count.DropDownHeight = 175
        Me.cbo_Count.FormattingEnabled = True
        Me.cbo_Count.IntegralHeight = False
        Me.cbo_Count.Location = New System.Drawing.Point(111, 53)
        Me.cbo_Count.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.cbo_Count.MaxLength = 35
        Me.cbo_Count.Name = "cbo_Count"
        Me.cbo_Count.Size = New System.Drawing.Size(464, 23)
        Me.cbo_Count.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(11, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(47, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Ref No."
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(8, 115)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(0, 15)
        Me.Label3.TabIndex = 2
        '
        'lbl_Company
        '
        Me.lbl_Company.AutoSize = True
        Me.lbl_Company.BackColor = System.Drawing.Color.Red
        Me.lbl_Company.ForeColor = System.Drawing.Color.White
        Me.lbl_Company.Location = New System.Drawing.Point(57, -79)
        Me.lbl_Company.Name = "lbl_Company"
        Me.lbl_Company.Size = New System.Drawing.Size(77, 15)
        Me.lbl_Company.TabIndex = 29
        Me.lbl_Company.Text = "lbl_Company"
        Me.lbl_Company.Visible = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Red
        Me.Label4.Location = New System.Drawing.Point(69, 93)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(13, 15)
        Me.Label4.TabIndex = 308
        Me.Label4.Text = "*"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Red
        Me.Label7.Location = New System.Drawing.Point(352, 13)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(13, 15)
        Me.Label7.TabIndex = 309
        Me.Label7.Text = "*"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Yarn_Lot_Creation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(596, 265)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.pnl_filter)
        Me.Controls.Add(Me.pnl_back)
        Me.Controls.Add(Me.lbl_Company)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Yarn_Lot_Creation"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "YARN LOT CREATION"
        CType(Me.dgv_filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnl_filter.ResumeLayout(False)
        Me.pnl_filter.PerformLayout()
        Me.pnl_back.ResumeLayout(False)
        Me.pnl_back.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
    Friend WithEvents cbo_Filter_Count As System.Windows.Forms.ComboBox
    Friend WithEvents btn_closefilter As System.Windows.Forms.Button
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents btn_filtershow As System.Windows.Forms.Button
    Friend WithEvents dgv_filter As System.Windows.Forms.DataGridView
    Friend WithEvents PrintDocument1 As System.Drawing.Printing.PrintDocument
    Friend WithEvents btn_close As System.Windows.Forms.Button
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents pnl_filter As System.Windows.Forms.Panel
    Friend WithEvents btn_save As System.Windows.Forms.Button
    Friend WithEvents pnl_back As System.Windows.Forms.Panel
    Friend WithEvents txt_Rate As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents lbl_RefNo As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cbo_Count As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lbl_Company As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label17 As Label
    Friend WithEvents cbo_Mill As ComboBox
    Friend WithEvents txt_LotNo As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents cbo_Filter_Mill As ComboBox
    Friend WithEvents Label6 As Label
    Friend WithEvents dc As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents Column5 As DataGridViewTextBoxColumn
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Label7 As Label
    Friend WithEvents Label4 As Label
End Class
