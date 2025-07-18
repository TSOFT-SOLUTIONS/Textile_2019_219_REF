<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Ledger_Creation_AdditionalCharges
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Ledger_Creation_AdditionalCharges))
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.grp_Back = New System.Windows.Forms.GroupBox()
        Me.txt_GSTPerc = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txt_HSNSAC_Code = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.lbl_IdNo = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.cbo_AcGroup = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txt_Name = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.grp_Filter = New System.Windows.Forms.GroupBox()
        Me.btn_Filter = New System.Windows.Forms.Button()
        Me.btn_CloseFilter = New System.Windows.Forms.Button()
        Me.dgv_Filter = New System.Windows.Forms.DataGridView()
        Me.Ledger_IdNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Ledger_Name = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.grp_Open = New System.Windows.Forms.GroupBox()
        Me.btn_Find = New System.Windows.Forms.Button()
        Me.cbo_Open = New System.Windows.Forms.ComboBox()
        Me.btn_CloseOpen = New System.Windows.Forms.Button()
        Me.lbl_Heading = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog()
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.grp_Back.SuspendLayout()
        Me.grp_Filter.SuspendLayout()
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grp_Open.SuspendLayout()
        Me.SuspendLayout()
        '
        'grp_Back
        '
        Me.grp_Back.BackColor = System.Drawing.Color.Transparent
        Me.grp_Back.Controls.Add(Me.txt_GSTPerc)
        Me.grp_Back.Controls.Add(Me.Label5)
        Me.grp_Back.Controls.Add(Me.txt_HSNSAC_Code)
        Me.grp_Back.Controls.Add(Me.Label4)
        Me.grp_Back.Controls.Add(Me.Label20)
        Me.grp_Back.Controls.Add(Me.Label19)
        Me.grp_Back.Controls.Add(Me.lbl_IdNo)
        Me.grp_Back.Controls.Add(Me.btnSave)
        Me.grp_Back.Controls.Add(Me.btnClose)
        Me.grp_Back.Controls.Add(Me.cbo_AcGroup)
        Me.grp_Back.Controls.Add(Me.Label3)
        Me.grp_Back.Controls.Add(Me.txt_Name)
        Me.grp_Back.Controls.Add(Me.Label2)
        Me.grp_Back.Controls.Add(Me.Label1)
        Me.grp_Back.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Back.Location = New System.Drawing.Point(5, 29)
        Me.grp_Back.Name = "grp_Back"
        Me.grp_Back.Size = New System.Drawing.Size(535, 208)
        Me.grp_Back.TabIndex = 1
        Me.grp_Back.TabStop = False
        '
        'txt_GSTPerc
        '
        Me.txt_GSTPerc.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_GSTPerc.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_GSTPerc.Location = New System.Drawing.Point(400, 126)
        Me.txt_GSTPerc.MaxLength = 70
        Me.txt_GSTPerc.Name = "txt_GSTPerc"
        Me.txt_GSTPerc.Size = New System.Drawing.Size(121, 23)
        Me.txt_GSTPerc.TabIndex = 3
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.Navy
        Me.Label5.Location = New System.Drawing.Point(309, 130)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(39, 15)
        Me.Label5.TabIndex = 303
        Me.Label5.Text = "GST %"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_HSNSAC_Code
        '
        Me.txt_HSNSAC_Code.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_HSNSAC_Code.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_HSNSAC_Code.Location = New System.Drawing.Point(141, 126)
        Me.txt_HSNSAC_Code.MaxLength = 70
        Me.txt_HSNSAC_Code.Name = "txt_HSNSAC_Code"
        Me.txt_HSNSAC_Code.Size = New System.Drawing.Size(160, 23)
        Me.txt_HSNSAC_Code.TabIndex = 2
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.Navy
        Me.Label4.Location = New System.Drawing.Point(13, 130)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(88, 15)
        Me.Label4.TabIndex = 301
        Me.Label4.Text = "HSN/SAC Code"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.ForeColor = System.Drawing.Color.Red
        Me.Label20.Location = New System.Drawing.Point(98, 90)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(13, 15)
        Me.Label20.TabIndex = 299
        Me.Label20.Text = "*"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.ForeColor = System.Drawing.Color.Red
        Me.Label19.Location = New System.Drawing.Point(98, 54)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(13, 15)
        Me.Label19.TabIndex = 298
        Me.Label19.Text = "*"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbl_IdNo
        '
        Me.lbl_IdNo.BackColor = System.Drawing.Color.White
        Me.lbl_IdNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_IdNo.Location = New System.Drawing.Point(141, 18)
        Me.lbl_IdNo.Name = "lbl_IdNo"
        Me.lbl_IdNo.Size = New System.Drawing.Size(380, 23)
        Me.lbl_IdNo.TabIndex = 16
        Me.lbl_IdNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ForeColor = System.Drawing.Color.White
        Me.btnSave.Location = New System.Drawing.Point(389, 162)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(60, 28)
        Me.btnSave.TabIndex = 4
        Me.btnSave.TabStop = False
        Me.btnSave.Text = "&Save"
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ForeColor = System.Drawing.Color.White
        Me.btnClose.Location = New System.Drawing.Point(461, 162)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(60, 28)
        Me.btnClose.TabIndex = 5
        Me.btnClose.TabStop = False
        Me.btnClose.Text = "&Close"
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'cbo_AcGroup
        '
        Me.cbo_AcGroup.DropDownHeight = 300
        Me.cbo_AcGroup.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_AcGroup.FormattingEnabled = True
        Me.cbo_AcGroup.IntegralHeight = False
        Me.cbo_AcGroup.Location = New System.Drawing.Point(141, 90)
        Me.cbo_AcGroup.Name = "cbo_AcGroup"
        Me.cbo_AcGroup.Size = New System.Drawing.Size(380, 23)
        Me.cbo_AcGroup.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.Color.Navy
        Me.Label3.Location = New System.Drawing.Point(13, 94)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(63, 15)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "A/c Group"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_Name
        '
        Me.txt_Name.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_Name.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Name.Location = New System.Drawing.Point(141, 54)
        Me.txt_Name.MaxLength = 70
        Me.txt_Name.Name = "txt_Name"
        Me.txt_Name.Size = New System.Drawing.Size(380, 23)
        Me.txt_Name.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.Navy
        Me.Label2.Location = New System.Drawing.Point(13, 58)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(81, 15)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Ledger Name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Navy
        Me.Label1.Location = New System.Drawing.Point(13, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(33, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "IdNo"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grp_Filter
        '
        Me.grp_Filter.Controls.Add(Me.btn_Filter)
        Me.grp_Filter.Controls.Add(Me.btn_CloseFilter)
        Me.grp_Filter.Controls.Add(Me.dgv_Filter)
        Me.grp_Filter.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Filter.Location = New System.Drawing.Point(5, 281)
        Me.grp_Filter.Name = "grp_Filter"
        Me.grp_Filter.Size = New System.Drawing.Size(535, 177)
        Me.grp_Filter.TabIndex = 32
        Me.grp_Filter.TabStop = False
        Me.grp_Filter.Text = "FILTER"
        Me.grp_Filter.Visible = False
        '
        'btn_Filter
        '
        Me.btn_Filter.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Filter.Image = CType(resources.GetObject("btn_Filter.Image"), System.Drawing.Image)
        Me.btn_Filter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Filter.Location = New System.Drawing.Point(386, 308)
        Me.btn_Filter.Name = "btn_Filter"
        Me.btn_Filter.Size = New System.Drawing.Size(83, 29)
        Me.btn_Filter.TabIndex = 33
        Me.btn_Filter.Text = "&Open"
        Me.btn_Filter.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Filter.UseVisualStyleBackColor = True
        '
        'btn_CloseFilter
        '
        Me.btn_CloseFilter.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_CloseFilter.Image = CType(resources.GetObject("btn_CloseFilter.Image"), System.Drawing.Image)
        Me.btn_CloseFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_CloseFilter.Location = New System.Drawing.Point(482, 308)
        Me.btn_CloseFilter.Name = "btn_CloseFilter"
        Me.btn_CloseFilter.Size = New System.Drawing.Size(83, 29)
        Me.btn_CloseFilter.TabIndex = 32
        Me.btn_CloseFilter.Text = "&Close"
        Me.btn_CloseFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_CloseFilter.UseVisualStyleBackColor = True
        '
        'dgv_Filter
        '
        Me.dgv_Filter.AllowUserToAddRows = False
        Me.dgv_Filter.AllowUserToDeleteRows = False
        Me.dgv_Filter.AllowUserToResizeRows = False
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_Filter.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle3
        Me.dgv_Filter.BackgroundColor = System.Drawing.Color.White
        Me.dgv_Filter.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_Filter.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.dgv_Filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgv_Filter.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Ledger_IdNo, Me.Ledger_Name})
        Me.dgv_Filter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgv_Filter.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv_Filter.EnableHeadersVisualStyles = False
        Me.dgv_Filter.Location = New System.Drawing.Point(3, 22)
        Me.dgv_Filter.Name = "dgv_Filter"
        Me.dgv_Filter.ReadOnly = True
        Me.dgv_Filter.RowHeadersWidth = 20
        Me.dgv_Filter.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgv_Filter.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_Filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Filter.Size = New System.Drawing.Size(529, 152)
        Me.dgv_Filter.TabIndex = 0
        '
        'Ledger_IdNo
        '
        Me.Ledger_IdNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Ledger_IdNo.HeaderText = "IDNO"
        Me.Ledger_IdNo.Name = "Ledger_IdNo"
        Me.Ledger_IdNo.ReadOnly = True
        Me.Ledger_IdNo.Width = 104
        '
        'Ledger_Name
        '
        Me.Ledger_Name.HeaderText = "LEDGER NAME"
        Me.Ledger_Name.Name = "Ledger_Name"
        Me.Ledger_Name.ReadOnly = True
        Me.Ledger_Name.Width = 380
        '
        'grp_Open
        '
        Me.grp_Open.Controls.Add(Me.btn_Find)
        Me.grp_Open.Controls.Add(Me.cbo_Open)
        Me.grp_Open.Controls.Add(Me.btn_CloseOpen)
        Me.grp_Open.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Open.Location = New System.Drawing.Point(604, 51)
        Me.grp_Open.Name = "grp_Open"
        Me.grp_Open.Size = New System.Drawing.Size(535, 230)
        Me.grp_Open.TabIndex = 31
        Me.grp_Open.TabStop = False
        Me.grp_Open.Text = "FINDING"
        Me.grp_Open.Visible = False
        '
        'btn_Find
        '
        Me.btn_Find.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Find.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Find.Location = New System.Drawing.Point(340, 197)
        Me.btn_Find.Name = "btn_Find"
        Me.btn_Find.Size = New System.Drawing.Size(83, 29)
        Me.btn_Find.TabIndex = 31
        Me.btn_Find.Text = "&Find"
        Me.btn_Find.UseVisualStyleBackColor = True
        '
        'cbo_Open
        '
        Me.cbo_Open.DropDownHeight = 180
        Me.cbo_Open.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Open.FormattingEnabled = True
        Me.cbo_Open.IntegralHeight = False
        Me.cbo_Open.Location = New System.Drawing.Point(19, 32)
        Me.cbo_Open.Name = "cbo_Open"
        Me.cbo_Open.Size = New System.Drawing.Size(502, 23)
        Me.cbo_Open.Sorted = True
        Me.cbo_Open.TabIndex = 0
        '
        'btn_CloseOpen
        '
        Me.btn_CloseOpen.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_CloseOpen.Location = New System.Drawing.Point(438, 197)
        Me.btn_CloseOpen.Name = "btn_CloseOpen"
        Me.btn_CloseOpen.Size = New System.Drawing.Size(83, 29)
        Me.btn_CloseOpen.TabIndex = 30
        Me.btn_CloseOpen.Text = "&Close"
        Me.btn_CloseOpen.UseVisualStyleBackColor = True
        '
        'lbl_Heading
        '
        Me.lbl_Heading.BackColor = System.Drawing.Color.DarkSlateGray
        Me.lbl_Heading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_Heading.Dock = System.Windows.Forms.DockStyle.Top
        Me.lbl_Heading.Font = New System.Drawing.Font("Calibri", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Heading.ForeColor = System.Drawing.Color.White
        Me.lbl_Heading.Location = New System.Drawing.Point(0, 0)
        Me.lbl_Heading.Name = "lbl_Heading"
        Me.lbl_Heading.Size = New System.Drawing.Size(543, 30)
        Me.lbl_Heading.TabIndex = 33
        Me.lbl_Heading.Text = "ADDITIONAL CHARGES - MASTER"
        Me.lbl_Heading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PrintDialog1
        '
        Me.PrintDialog1.UseEXDialog = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'Ledger_Creation_AdditionalCharges
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.PaleTurquoise
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ClientSize = New System.Drawing.Size(543, 239)
        Me.Controls.Add(Me.grp_Open)
        Me.Controls.Add(Me.grp_Filter)
        Me.Controls.Add(Me.lbl_Heading)
        Me.Controls.Add(Me.grp_Back)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "Ledger_Creation_AdditionalCharges"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "LEDGER CREATION"
        Me.grp_Back.ResumeLayout(False)
        Me.grp_Back.PerformLayout()
        Me.grp_Filter.ResumeLayout(False)
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grp_Open.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grp_Back As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txt_Name As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cbo_AcGroup As System.Windows.Forms.ComboBox
    Friend WithEvents grp_Open As System.Windows.Forms.GroupBox
    Friend WithEvents btn_Find As System.Windows.Forms.Button
    Friend WithEvents cbo_Open As System.Windows.Forms.ComboBox
    Friend WithEvents btn_CloseOpen As System.Windows.Forms.Button
    Friend WithEvents grp_Filter As System.Windows.Forms.GroupBox
    Friend WithEvents btn_Filter As System.Windows.Forms.Button
    Friend WithEvents btn_CloseFilter As System.Windows.Forms.Button
    Friend WithEvents dgv_Filter As System.Windows.Forms.DataGridView
    Friend WithEvents lbl_Heading As System.Windows.Forms.Label
    Friend WithEvents lbl_IdNo As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
    Friend WithEvents PrintDocument1 As System.Drawing.Printing.PrintDocument
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents txt_GSTPerc As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents txt_HSNSAC_Code As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Ledger_IdNo As DataGridViewTextBoxColumn
    Friend WithEvents Ledger_Name As DataGridViewTextBoxColumn
End Class
