<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Loading_unloading_rate_Creation
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.pnl_back = New System.Windows.Forms.Panel()
        Me.dgv_Details = New System.Windows.Forms.DataGridView()
        Me.dgv_PavuDetails_Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgv_PavuDetails_Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cbo_vehicle = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txt_Cloth_kgs = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txt_Cloth_Loading = New System.Windows.Forms.TextBox()
        Me.txt_Pavu_beam_Loading = New System.Windows.Forms.TextBox()
        Me.txt_Cloth_Unloading = New System.Windows.Forms.TextBox()
        Me.txt_pavu_Beam_Unloading = New System.Windows.Forms.TextBox()
        Me.Btn_save = New System.Windows.Forms.Button()
        Me.Btn_Close = New System.Windows.Forms.Button()
        Me.txt_Empty_Beam_Loading = New System.Windows.Forms.TextBox()
        Me.txt_Empty_Beam_UnLoading = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.pnl_back.SuspendLayout()
        CType(Me.dgv_Details, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Black
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ButtonFace
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(531, 29)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "LOADING AND UNLOADING RATE CREATION"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'pnl_back
        '
        Me.pnl_back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_back.Controls.Add(Me.dgv_Details)
        Me.pnl_back.Controls.Add(Me.Label10)
        Me.pnl_back.Controls.Add(Me.cbo_vehicle)
        Me.pnl_back.Controls.Add(Me.Label8)
        Me.pnl_back.Controls.Add(Me.txt_Cloth_kgs)
        Me.pnl_back.Controls.Add(Me.Label9)
        Me.pnl_back.Controls.Add(Me.Label7)
        Me.pnl_back.Controls.Add(Me.Label6)
        Me.pnl_back.Controls.Add(Me.txt_Cloth_Loading)
        Me.pnl_back.Controls.Add(Me.txt_Pavu_beam_Loading)
        Me.pnl_back.Controls.Add(Me.txt_Cloth_Unloading)
        Me.pnl_back.Controls.Add(Me.txt_pavu_Beam_Unloading)
        Me.pnl_back.Controls.Add(Me.Btn_save)
        Me.pnl_back.Controls.Add(Me.Btn_Close)
        Me.pnl_back.Controls.Add(Me.txt_Empty_Beam_Loading)
        Me.pnl_back.Controls.Add(Me.txt_Empty_Beam_UnLoading)
        Me.pnl_back.Controls.Add(Me.Label3)
        Me.pnl_back.Controls.Add(Me.Label2)
        Me.pnl_back.Location = New System.Drawing.Point(12, 42)
        Me.pnl_back.Name = "pnl_back"
        Me.pnl_back.Size = New System.Drawing.Size(504, 485)
        Me.pnl_back.TabIndex = 4
        '
        'dgv_Details
        '
        Me.dgv_Details.AllowUserToResizeColumns = False
        Me.dgv_Details.AllowUserToResizeRows = False
        Me.dgv_Details.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_Details.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgv_Details.ColumnHeadersHeight = 35
        Me.dgv_Details.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgv_Details.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.dgv_PavuDetails_Column1, Me.dgv_PavuDetails_Column2, Me.Column1, Me.Column2, Me.Column3})
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.Lime
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgv_Details.DefaultCellStyle = DataGridViewCellStyle7
        Me.dgv_Details.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.dgv_Details.EnableHeadersVisualStyles = False
        Me.dgv_Details.Location = New System.Drawing.Point(21, 65)
        Me.dgv_Details.MultiSelect = False
        Me.dgv_Details.Name = "dgv_Details"
        Me.dgv_Details.RowHeadersVisible = False
        Me.dgv_Details.RowHeadersWidth = 15
        Me.dgv_Details.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgv_Details.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_Details.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.dgv_Details.Size = New System.Drawing.Size(465, 132)
        Me.dgv_Details.TabIndex = 4
        Me.dgv_Details.TabStop = False
        '
        'dgv_PavuDetails_Column1
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dgv_PavuDetails_Column1.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgv_PavuDetails_Column1.Frozen = True
        Me.dgv_PavuDetails_Column1.HeaderText = "SL NO"
        Me.dgv_PavuDetails_Column1.MaxInputLength = 20
        Me.dgv_PavuDetails_Column1.Name = "dgv_PavuDetails_Column1"
        Me.dgv_PavuDetails_Column1.ReadOnly = True
        Me.dgv_PavuDetails_Column1.Width = 35
        '
        'dgv_PavuDetails_Column2
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Lime
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_PavuDetails_Column2.DefaultCellStyle = DataGridViewCellStyle3
        Me.dgv_PavuDetails_Column2.HeaderText = "FROM WEIGHT"
        Me.dgv_PavuDetails_Column2.MaxInputLength = 20
        Me.dgv_PavuDetails_Column2.Name = "dgv_PavuDetails_Column2"
        Me.dgv_PavuDetails_Column2.Width = 120
        '
        'Column1
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.Lime
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Column1.DefaultCellStyle = DataGridViewCellStyle4
        Me.Column1.HeaderText = "TO WEIGHT"
        Me.Column1.Name = "Column1"
        '
        'Column2
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.Lime
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle5
        Me.Column2.HeaderText = "LOADING CHARGES"
        Me.Column2.Name = "Column2"
        '
        'Column3
        '
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.Lime
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Column3.DefaultCellStyle = DataGridViewCellStyle6
        Me.Column3.HeaderText = "UNLOADING CHARGES"
        Me.Column3.Name = "Column3"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(18, 22)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(72, 15)
        Me.Label10.TabIndex = 5
        Me.Label10.Text = "VEHICLE NO"
        '
        'cbo_vehicle
        '
        Me.cbo_vehicle.FormattingEnabled = True
        Me.cbo_vehicle.Location = New System.Drawing.Point(98, 19)
        Me.cbo_vehicle.Name = "cbo_vehicle"
        Me.cbo_vehicle.Size = New System.Drawing.Size(388, 23)
        Me.cbo_vehicle.TabIndex = 1
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(181, 367)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(28, 15)
        Me.Label8.TabIndex = 18
        Me.Label8.Text = "Kgs."
        '
        'txt_Cloth_kgs
        '
        Me.txt_Cloth_kgs.Location = New System.Drawing.Point(113, 363)
        Me.txt_Cloth_kgs.Name = "txt_Cloth_kgs"
        Me.txt_Cloth_kgs.Size = New System.Drawing.Size(62, 23)
        Me.txt_Cloth_kgs.TabIndex = 9
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(18, 367)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(43, 15)
        Me.Label9.TabIndex = 16
        Me.Label9.Text = "CLOTH"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(18, 315)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(79, 15)
        Me.Label7.TabIndex = 15
        Me.Label7.Text = "EMPTY BEAM"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(18, 260)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(71, 15)
        Me.Label6.TabIndex = 14
        Me.Label6.Text = "PAVU BEAM"
        '
        'txt_Cloth_Loading
        '
        Me.txt_Cloth_Loading.Location = New System.Drawing.Point(228, 363)
        Me.txt_Cloth_Loading.Name = "txt_Cloth_Loading"
        Me.txt_Cloth_Loading.Size = New System.Drawing.Size(84, 23)
        Me.txt_Cloth_Loading.TabIndex = 10
        '
        'txt_Pavu_beam_Loading
        '
        Me.txt_Pavu_beam_Loading.Location = New System.Drawing.Point(228, 257)
        Me.txt_Pavu_beam_Loading.Name = "txt_Pavu_beam_Loading"
        Me.txt_Pavu_beam_Loading.Size = New System.Drawing.Size(84, 23)
        Me.txt_Pavu_beam_Loading.TabIndex = 5
        '
        'txt_Cloth_Unloading
        '
        Me.txt_Cloth_Unloading.Location = New System.Drawing.Point(340, 363)
        Me.txt_Cloth_Unloading.Name = "txt_Cloth_Unloading"
        Me.txt_Cloth_Unloading.Size = New System.Drawing.Size(84, 23)
        Me.txt_Cloth_Unloading.TabIndex = 11
        '
        'txt_pavu_Beam_Unloading
        '
        Me.txt_pavu_Beam_Unloading.Location = New System.Drawing.Point(340, 257)
        Me.txt_pavu_Beam_Unloading.Name = "txt_pavu_Beam_Unloading"
        Me.txt_pavu_Beam_Unloading.Size = New System.Drawing.Size(84, 23)
        Me.txt_pavu_Beam_Unloading.TabIndex = 6
        '
        'Btn_save
        '
        Me.Btn_save.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.Btn_save.Location = New System.Drawing.Point(235, 437)
        Me.Btn_save.Name = "Btn_save"
        Me.Btn_save.Size = New System.Drawing.Size(86, 30)
        Me.Btn_save.TabIndex = 12
        Me.Btn_save.Text = "SAVE"
        Me.Btn_save.UseVisualStyleBackColor = False
        '
        'Btn_Close
        '
        Me.Btn_Close.BackColor = System.Drawing.Color.IndianRed
        Me.Btn_Close.Location = New System.Drawing.Point(338, 437)
        Me.Btn_Close.Name = "Btn_Close"
        Me.Btn_Close.Size = New System.Drawing.Size(86, 30)
        Me.Btn_Close.TabIndex = 13
        Me.Btn_Close.Text = "CLOSE"
        Me.Btn_Close.UseVisualStyleBackColor = False
        '
        'txt_Empty_Beam_Loading
        '
        Me.txt_Empty_Beam_Loading.Location = New System.Drawing.Point(228, 312)
        Me.txt_Empty_Beam_Loading.Name = "txt_Empty_Beam_Loading"
        Me.txt_Empty_Beam_Loading.Size = New System.Drawing.Size(84, 23)
        Me.txt_Empty_Beam_Loading.TabIndex = 7
        '
        'txt_Empty_Beam_UnLoading
        '
        Me.txt_Empty_Beam_UnLoading.Location = New System.Drawing.Point(340, 312)
        Me.txt_Empty_Beam_UnLoading.Name = "txt_Empty_Beam_UnLoading"
        Me.txt_Empty_Beam_UnLoading.Size = New System.Drawing.Size(84, 23)
        Me.txt_Empty_Beam_UnLoading.TabIndex = 8
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(335, 228)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(104, 15)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "UNLOADING RATE"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(223, 228)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(87, 15)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "LOADING RATE"
        '
        'Loading_unloading_rate_Creation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Linen
        Me.ClientSize = New System.Drawing.Size(531, 539)
        Me.Controls.Add(Me.pnl_back)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.Black
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "Loading_unloading_rate_Creation"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Loading_unloading_rate_Creation"
        Me.pnl_back.ResumeLayout(False)
        Me.pnl_back.PerformLayout()
        CType(Me.dgv_Details, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents pnl_back As System.Windows.Forms.Panel
    Friend WithEvents Btn_save As System.Windows.Forms.Button
    Friend WithEvents Btn_Close As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txt_Cloth_kgs As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txt_Cloth_Loading As System.Windows.Forms.TextBox
    Friend WithEvents txt_Pavu_beam_Loading As System.Windows.Forms.TextBox
    Friend WithEvents txt_Cloth_Unloading As System.Windows.Forms.TextBox
    Friend WithEvents txt_pavu_Beam_Unloading As System.Windows.Forms.TextBox
    Friend WithEvents txt_Empty_Beam_Loading As System.Windows.Forms.TextBox
    Friend WithEvents txt_Empty_Beam_UnLoading As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents cbo_vehicle As System.Windows.Forms.ComboBox
    Friend WithEvents dgv_Details As System.Windows.Forms.DataGridView
    Friend WithEvents dgv_PavuDetails_Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgv_PavuDetails_Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
