<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Stores_Department_Creation
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
        Me.txt_Name = New System.Windows.Forms.TextBox()
        Me.grp_Find = New System.Windows.Forms.GroupBox()
        Me.cbo_Find = New System.Windows.Forms.ComboBox()
        Me.btn_FindClose = New System.Windows.Forms.Button()
        Me.btn_FindOpen = New System.Windows.Forms.Button()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.grp_Filter = New System.Windows.Forms.GroupBox()
        Me.btn_FilterClose = New System.Windows.Forms.Button()
        Me.btn_FilterOpen = New System.Windows.Forms.Button()
        Me.dgv_Filter = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btn_Save = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.pnl_back = New System.Windows.Forms.Panel()
        Me.btn_EnLargePhoto = New System.Windows.Forms.Button()
        Me.btn_BrowsePhoto = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.lbl_IdNo = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.grp_Find.SuspendLayout()
        Me.grp_Filter.SuspendLayout()
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnl_back.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txt_Name
        '
        Me.txt_Name.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_Name.Location = New System.Drawing.Point(142, 64)
        Me.txt_Name.MaxLength = 35
        Me.txt_Name.Name = "txt_Name"
        Me.txt_Name.Size = New System.Drawing.Size(325, 23)
        Me.txt_Name.TabIndex = 1
        Me.txt_Name.Text = "TXT_NAME"
        '
        'grp_Find
        '
        Me.grp_Find.Controls.Add(Me.cbo_Find)
        Me.grp_Find.Controls.Add(Me.btn_FindClose)
        Me.grp_Find.Controls.Add(Me.btn_FindOpen)
        Me.grp_Find.Location = New System.Drawing.Point(583, 58)
        Me.grp_Find.Name = "grp_Find"
        Me.grp_Find.Size = New System.Drawing.Size(450, 253)
        Me.grp_Find.TabIndex = 7
        Me.grp_Find.TabStop = False
        Me.grp_Find.Text = "FINDING"
        '
        'cbo_Find
        '
        Me.cbo_Find.FormattingEnabled = True
        Me.cbo_Find.Location = New System.Drawing.Point(19, 34)
        Me.cbo_Find.Name = "cbo_Find"
        Me.cbo_Find.Size = New System.Drawing.Size(408, 23)
        Me.cbo_Find.TabIndex = 6
        '
        'btn_FindClose
        '
        Me.btn_FindClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_FindClose.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btn_FindClose.Location = New System.Drawing.Point(357, 197)
        Me.btn_FindClose.Name = "btn_FindClose"
        Me.btn_FindClose.Size = New System.Drawing.Size(70, 30)
        Me.btn_FindClose.TabIndex = 8
        Me.btn_FindClose.Text = "&CLOSE"
        Me.btn_FindClose.UseVisualStyleBackColor = False
        '
        'btn_FindOpen
        '
        Me.btn_FindOpen.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_FindOpen.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btn_FindOpen.Location = New System.Drawing.Point(277, 197)
        Me.btn_FindOpen.Name = "btn_FindOpen"
        Me.btn_FindOpen.Size = New System.Drawing.Size(70, 30)
        Me.btn_FindOpen.TabIndex = 7
        Me.btn_FindOpen.Text = "&OPEN"
        Me.btn_FindOpen.UseVisualStyleBackColor = False
        '
        'btn_Close
        '
        Me.btn_Close.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_Close.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btn_Close.Location = New System.Drawing.Point(381, 452)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(87, 32)
        Me.btn_Close.TabIndex = 4
        Me.btn_Close.Text = "&CLOSE"
        Me.btn_Close.UseVisualStyleBackColor = False
        '
        'grp_Filter
        '
        Me.grp_Filter.Controls.Add(Me.btn_FilterClose)
        Me.grp_Filter.Controls.Add(Me.btn_FilterOpen)
        Me.grp_Filter.Controls.Add(Me.dgv_Filter)
        Me.grp_Filter.Location = New System.Drawing.Point(583, 342)
        Me.grp_Filter.Name = "grp_Filter"
        Me.grp_Filter.Size = New System.Drawing.Size(450, 285)
        Me.grp_Filter.TabIndex = 9
        Me.grp_Filter.TabStop = False
        Me.grp_Filter.Text = "FILTER"
        '
        'btn_FilterClose
        '
        Me.btn_FilterClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_FilterClose.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_FilterClose.ForeColor = System.Drawing.Color.White
        Me.btn_FilterClose.Location = New System.Drawing.Point(354, 243)
        Me.btn_FilterClose.Name = "btn_FilterClose"
        Me.btn_FilterClose.Size = New System.Drawing.Size(75, 30)
        Me.btn_FilterClose.TabIndex = 10
        Me.btn_FilterClose.TabStop = False
        Me.btn_FilterClose.Text = "&CLOSE"
        Me.btn_FilterClose.UseVisualStyleBackColor = False
        '
        'btn_FilterOpen
        '
        Me.btn_FilterOpen.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_FilterOpen.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_FilterOpen.ForeColor = System.Drawing.Color.White
        Me.btn_FilterOpen.Location = New System.Drawing.Point(273, 243)
        Me.btn_FilterOpen.Name = "btn_FilterOpen"
        Me.btn_FilterOpen.Size = New System.Drawing.Size(75, 30)
        Me.btn_FilterOpen.TabIndex = 9
        Me.btn_FilterOpen.TabStop = False
        Me.btn_FilterOpen.Text = "&OPEN"
        Me.btn_FilterOpen.UseVisualStyleBackColor = False
        '
        'dgv_Filter
        '
        Me.dgv_Filter.AllowUserToAddRows = False
        Me.dgv_Filter.AllowUserToDeleteRows = False
        Me.dgv_Filter.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_Filter.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgv_Filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_Filter.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2})
        Me.dgv_Filter.Dock = System.Windows.Forms.DockStyle.Top
        Me.dgv_Filter.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv_Filter.Location = New System.Drawing.Point(3, 19)
        Me.dgv_Filter.Name = "dgv_Filter"
        Me.dgv_Filter.ReadOnly = True
        Me.dgv_Filter.RowHeadersVisible = False
        Me.dgv_Filter.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_Filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Filter.Size = New System.Drawing.Size(444, 215)
        Me.dgv_Filter.TabIndex = 0
        '
        'Column1
        '
        Me.Column1.HeaderText = "IDNO"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Width = 60
        '
        'Column2
        '
        Me.Column2.HeaderText = "DEPARTMENT  NAME"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Width = 400
        '
        'btn_Save
        '
        Me.btn_Save.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_Save.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btn_Save.Location = New System.Drawing.Point(278, 453)
        Me.btn_Save.Name = "btn_Save"
        Me.btn_Save.Size = New System.Drawing.Size(87, 32)
        Me.btn_Save.TabIndex = 2
        Me.btn_Save.Text = "&SAVE"
        Me.btn_Save.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.Label4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label4.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label4.Location = New System.Drawing.Point(0, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(508, 35)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "DEPARTMENT CREACTION"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(18, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(33, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "IdNo"
        '
        'pnl_back
        '
        Me.pnl_back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_back.Controls.Add(Me.btn_EnLargePhoto)
        Me.pnl_back.Controls.Add(Me.btn_BrowsePhoto)
        Me.pnl_back.Controls.Add(Me.PictureBox1)
        Me.pnl_back.Controls.Add(Me.txt_Name)
        Me.pnl_back.Controls.Add(Me.btn_Close)
        Me.pnl_back.Controls.Add(Me.btn_Save)
        Me.pnl_back.Controls.Add(Me.lbl_IdNo)
        Me.pnl_back.Controls.Add(Me.Label2)
        Me.pnl_back.Controls.Add(Me.Label1)
        Me.pnl_back.Location = New System.Drawing.Point(6, 48)
        Me.pnl_back.Name = "pnl_back"
        Me.pnl_back.Size = New System.Drawing.Size(495, 501)
        Me.pnl_back.TabIndex = 6
        '
        'btn_EnLargePhoto
        '
        Me.btn_EnLargePhoto.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_EnLargePhoto.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_EnLargePhoto.ForeColor = System.Drawing.Color.White
        Me.btn_EnLargePhoto.Location = New System.Drawing.Point(18, 163)
        Me.btn_EnLargePhoto.Name = "btn_EnLargePhoto"
        Me.btn_EnLargePhoto.Size = New System.Drawing.Size(108, 32)
        Me.btn_EnLargePhoto.TabIndex = 47
        Me.btn_EnLargePhoto.TabStop = False
        Me.btn_EnLargePhoto.Text = "En&large Photo"
        Me.btn_EnLargePhoto.UseVisualStyleBackColor = False
        '
        'btn_BrowsePhoto
        '
        Me.btn_BrowsePhoto.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.btn_BrowsePhoto.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_BrowsePhoto.ForeColor = System.Drawing.Color.White
        Me.btn_BrowsePhoto.Location = New System.Drawing.Point(18, 107)
        Me.btn_BrowsePhoto.Name = "btn_BrowsePhoto"
        Me.btn_BrowsePhoto.Size = New System.Drawing.Size(108, 32)
        Me.btn_BrowsePhoto.TabIndex = 46
        Me.btn_BrowsePhoto.TabStop = False
        Me.btn_BrowsePhoto.Text = "Browse  &Photo"
        Me.btn_BrowsePhoto.UseVisualStyleBackColor = False
        '
        'PictureBox1
        '
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox1.Location = New System.Drawing.Point(142, 107)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(325, 325)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 45
        Me.PictureBox1.TabStop = False
        Me.PictureBox1.Tag = ""
        '
        'lbl_IdNo
        '
        Me.lbl_IdNo.BackColor = System.Drawing.Color.White
        Me.lbl_IdNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_IdNo.Location = New System.Drawing.Point(142, 21)
        Me.lbl_IdNo.Name = "lbl_IdNo"
        Me.lbl_IdNo.Size = New System.Drawing.Size(325, 23)
        Me.lbl_IdNo.TabIndex = 0
        Me.lbl_IdNo.Text = "lbl_IdNo"
        Me.lbl_IdNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(18, 68)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(111, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Department Name"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        Me.OpenFileDialog1.Filter = "Image Files(*.GIF;*.JPG;*.JPEG;*.BMP;*.PNG)| *.GIF;*.JPG;*.JPEG;*.BMP;*.PNG"
        '
        'Stores_Department_Creation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.PaleTurquoise
        Me.ClientSize = New System.Drawing.Size(508, 559)
        Me.Controls.Add(Me.grp_Filter)
        Me.Controls.Add(Me.grp_Find)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.pnl_back)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.Name = "Stores_Department_Creation"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "DEPARTMENT CREACTION"
        Me.grp_Find.ResumeLayout(False)
        Me.grp_Filter.ResumeLayout(False)
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnl_back.ResumeLayout(False)
        Me.pnl_back.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txt_Name As System.Windows.Forms.TextBox
    Friend WithEvents grp_Find As System.Windows.Forms.GroupBox
    Friend WithEvents cbo_Find As System.Windows.Forms.ComboBox
    Friend WithEvents btn_FindClose As System.Windows.Forms.Button
    Friend WithEvents btn_FindOpen As System.Windows.Forms.Button
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents grp_Filter As System.Windows.Forms.GroupBox
    Friend WithEvents btn_FilterClose As System.Windows.Forms.Button
    Friend WithEvents btn_FilterOpen As System.Windows.Forms.Button
    Friend WithEvents dgv_Filter As System.Windows.Forms.DataGridView
    Friend WithEvents btn_Save As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents pnl_back As System.Windows.Forms.Panel
    Friend WithEvents lbl_IdNo As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btn_EnLargePhoto As System.Windows.Forms.Button
    Friend WithEvents btn_BrowsePhoto As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
