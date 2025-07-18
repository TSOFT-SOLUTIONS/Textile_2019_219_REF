<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Entrance
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Entrance))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lbl_TrialPeriod_Warning = New System.Windows.Forms.Label()
        Me.lbl_Restore_Database = New System.Windows.Forms.Label()
        Me.btn_Register = New System.Windows.Forms.Button()
        Me.btn_Decrypt = New System.Windows.Forms.Button()
        Me.btn_Encrypt = New System.Windows.Forms.Button()
        Me.btn_Open = New System.Windows.Forms.Button()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.btn_Create = New System.Windows.Forms.Button()
        Me.dgv_Details = New System.Windows.Forms.DataGridView()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.Panel1.SuspendLayout()
        CType(Me.dgv_Details, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.lbl_TrialPeriod_Warning)
        Me.Panel1.Controls.Add(Me.lbl_Restore_Database)
        Me.Panel1.Controls.Add(Me.btn_Register)
        Me.Panel1.Controls.Add(Me.btn_Decrypt)
        Me.Panel1.Controls.Add(Me.btn_Encrypt)
        Me.Panel1.Controls.Add(Me.btn_Open)
        Me.Panel1.Controls.Add(Me.btn_Close)
        Me.Panel1.Controls.Add(Me.Button4)
        Me.Panel1.Controls.Add(Me.btn_Create)
        Me.Panel1.Controls.Add(Me.dgv_Details)
        Me.Panel1.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.Location = New System.Drawing.Point(8, 102)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(823, 339)
        Me.Panel1.TabIndex = 0
        '
        'lbl_TrialPeriod_Warning
        '
        Me.lbl_TrialPeriod_Warning.BackColor = System.Drawing.Color.Black
        Me.lbl_TrialPeriod_Warning.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lbl_TrialPeriod_Warning.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_TrialPeriod_Warning.ForeColor = System.Drawing.Color.Red
        Me.lbl_TrialPeriod_Warning.Location = New System.Drawing.Point(0, 315)
        Me.lbl_TrialPeriod_Warning.Name = "lbl_TrialPeriod_Warning"
        Me.lbl_TrialPeriod_Warning.Size = New System.Drawing.Size(821, 22)
        Me.lbl_TrialPeriod_Warning.TabIndex = 29
        Me.lbl_TrialPeriod_Warning.Text = "Trial software, please update license key"
        Me.lbl_TrialPeriod_Warning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lbl_TrialPeriod_Warning.Visible = False
        '
        'lbl_Restore_Database
        '
        Me.lbl_Restore_Database.AutoSize = True
        Me.lbl_Restore_Database.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lbl_Restore_Database.Font = New System.Drawing.Font("Calibri", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Restore_Database.ForeColor = System.Drawing.Color.MidnightBlue
        Me.lbl_Restore_Database.Location = New System.Drawing.Point(735, 172)
        Me.lbl_Restore_Database.Name = "lbl_Restore_Database"
        Me.lbl_Restore_Database.Size = New System.Drawing.Size(43, 13)
        Me.lbl_Restore_Database.TabIndex = 28
        Me.lbl_Restore_Database.Text = "&Restore"
        '
        'btn_Register
        '
        Me.btn_Register.Location = New System.Drawing.Point(708, 295)
        Me.btn_Register.Name = "btn_Register"
        Me.btn_Register.Size = New System.Drawing.Size(97, 38)
        Me.btn_Register.TabIndex = 27
        Me.btn_Register.Text = "&REGISTER"
        Me.btn_Register.UseVisualStyleBackColor = True
        Me.btn_Register.Visible = False
        '
        'btn_Decrypt
        '
        Me.btn_Decrypt.Location = New System.Drawing.Point(708, 259)
        Me.btn_Decrypt.Name = "btn_Decrypt"
        Me.btn_Decrypt.Size = New System.Drawing.Size(97, 38)
        Me.btn_Decrypt.TabIndex = 26
        Me.btn_Decrypt.Text = "Decrypt"
        Me.btn_Decrypt.UseVisualStyleBackColor = True
        Me.btn_Decrypt.Visible = False
        '
        'btn_Encrypt
        '
        Me.btn_Encrypt.Location = New System.Drawing.Point(708, 222)
        Me.btn_Encrypt.Name = "btn_Encrypt"
        Me.btn_Encrypt.Size = New System.Drawing.Size(97, 35)
        Me.btn_Encrypt.TabIndex = 25
        Me.btn_Encrypt.Text = "Encrypt"
        Me.btn_Encrypt.UseVisualStyleBackColor = True
        Me.btn_Encrypt.Visible = False
        '
        'btn_Open
        '
        Me.btn_Open.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.btn_Open.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Open.ForeColor = System.Drawing.Color.White
        Me.btn_Open.Location = New System.Drawing.Point(708, 20)
        Me.btn_Open.Name = "btn_Open"
        Me.btn_Open.Size = New System.Drawing.Size(97, 37)
        Me.btn_Open.TabIndex = 1
        Me.btn_Open.TabStop = False
        Me.btn_Open.Text = "&OPEN"
        Me.btn_Open.UseVisualStyleBackColor = False
        '
        'btn_Close
        '
        Me.btn_Close.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(90, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btn_Close.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Close.ForeColor = System.Drawing.Color.White
        Me.btn_Close.Location = New System.Drawing.Point(708, 130)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(97, 37)
        Me.btn_Close.TabIndex = 3
        Me.btn_Close.TabStop = False
        Me.btn_Close.Text = "&CLOSE"
        Me.btn_Close.UseVisualStyleBackColor = False
        '
        'Button4
        '
        Me.Button4.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.Button4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button4.ForeColor = System.Drawing.Color.White
        Me.Button4.Location = New System.Drawing.Point(708, 185)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(97, 37)
        Me.Button4.TabIndex = 4
        Me.Button4.TabStop = False
        Me.Button4.Text = "&DB TOOLS"
        Me.Button4.UseVisualStyleBackColor = False
        Me.Button4.Visible = False
        '
        'btn_Create
        '
        Me.btn_Create.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btn_Create.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Create.ForeColor = System.Drawing.Color.White
        Me.btn_Create.Location = New System.Drawing.Point(708, 63)
        Me.btn_Create.Name = "btn_Create"
        Me.btn_Create.Size = New System.Drawing.Size(97, 61)
        Me.btn_Create.TabIndex = 2
        Me.btn_Create.TabStop = False
        Me.btn_Create.Text = "CREATE &NEW COMPANY GROUP"
        Me.btn_Create.UseVisualStyleBackColor = False
        '
        'dgv_Details
        '
        Me.dgv_Details.AllowUserToAddRows = False
        Me.dgv_Details.AllowUserToDeleteRows = False
        Me.dgv_Details.AllowUserToResizeColumns = False
        Me.dgv_Details.AllowUserToResizeRows = False
        Me.dgv_Details.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.Navy
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Navy
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_Details.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgv_Details.ColumnHeadersHeight = 28
        Me.dgv_Details.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgv_Details.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column2, Me.Column1, Me.Column3, Me.Column4})
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgv_Details.DefaultCellStyle = DataGridViewCellStyle6
        Me.dgv_Details.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv_Details.EnableHeadersVisualStyles = False
        Me.dgv_Details.Location = New System.Drawing.Point(8, 20)
        Me.dgv_Details.MultiSelect = False
        Me.dgv_Details.Name = "dgv_Details"
        Me.dgv_Details.ReadOnly = True
        Me.dgv_Details.RowHeadersVisible = False
        Me.dgv_Details.RowHeadersWidth = 15
        Me.dgv_Details.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgv_Details.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_Details.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Details.Size = New System.Drawing.Size(691, 295)
        Me.dgv_Details.TabIndex = 0
        '
        'Column2
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle2
        Me.Column2.HeaderText = "COMPANY GROUP NAME"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Width = 360
        '
        'Column1
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column1.DefaultCellStyle = DataGridViewCellStyle3
        Me.Column1.HeaderText = "ID"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Width = 60
        '
        'Column3
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Column3.DefaultCellStyle = DataGridViewCellStyle4
        Me.Column3.HeaderText = "YEAR"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        Me.Column3.Width = 117
        '
        'Column4
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.Color.Red
        Me.Column4.DefaultCellStyle = DataGridViewCellStyle5
        Me.Column4.HeaderText = "CREATE NEXT YEAR"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        Me.Column4.Width = 130
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.Label4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label4.Font = New System.Drawing.Font("Berlin Sans FB", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(0, 40)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(864, 19)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "TSOFT Textile Weaving Information System"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label3.Font = New System.Drawing.Font("Bauhaus 93", 30.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(0, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(864, 40)
        Me.Label3.TabIndex = 12
        Me.Label3.Text = "TSOFT TWIS"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.Label5.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Label5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(0, 509)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(864, 15)
        Me.Label5.TabIndex = 15
        Me.Label5.Text = "Email : info@tsoftsolutions.in        WebSite : www.tsoftsolutions.in"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoEllipsis = True
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(90, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.Label1.Location = New System.Drawing.Point(0, 59)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(864, 34)
        Me.Label1.TabIndex = 24
        Me.Label1.Text = "COMPANY GROUP SELECTION"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.Label6.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Label6.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(0, 448)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(864, 43)
        Me.Label6.TabIndex = 26
        Me.Label6.Text = "Developed by  :  TSOFT Solutions ,  61/44 , S.V Colony ,  2nd Street, Near Sakthi" &
    " jewellery , Mettupalayam Bus  Stop,  P.N Road,  Tirupur -  2."
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.Label8.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Label8.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Cyan
        Me.Label8.Location = New System.Drawing.Point(0, 491)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(864, 18)
        Me.Label8.TabIndex = 25
        Me.Label8.Text = "Phone : 85084 03222, 85084 03221, 85084 03229, 85084 03230, 0421-4337417"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'Entrance
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(864, 524)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "Entrance"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Tag = "731, 563"
        Me.Text = "TSOFT TWIS ( TSOFT Textile Weaving Information System )"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.dgv_Details, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents dgv_Details As System.Windows.Forms.DataGridView
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents btn_Create As System.Windows.Forms.Button
    Friend WithEvents btn_Open As System.Windows.Forms.Button
    Friend WithEvents btn_Encrypt As System.Windows.Forms.Button
    Friend WithEvents btn_Decrypt As System.Windows.Forms.Button
    Friend WithEvents btn_Register As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lbl_Restore_Database As System.Windows.Forms.Label
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents lbl_TrialPeriod_Warning As System.Windows.Forms.Label
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
    Friend WithEvents Column4 As DataGridViewButtonColumn
End Class
