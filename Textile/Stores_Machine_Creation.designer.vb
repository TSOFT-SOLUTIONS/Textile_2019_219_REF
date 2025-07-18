<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Stores_Machine_Creation
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
        Me.grp_Find = New System.Windows.Forms.GroupBox()
        Me.cbo_Find = New System.Windows.Forms.ComboBox()
        Me.btn_FindClose = New System.Windows.Forms.Button()
        Me.btn_FindOpen = New System.Windows.Forms.Button()
        Me.pnl_back = New System.Windows.Forms.Panel()
        Me.txt_Oil_Sevice = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txt_Name = New System.Windows.Forms.TextBox()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.btn_Save = New System.Windows.Forms.Button()
        Me.lbl_IdNo = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.grp_Filter = New System.Windows.Forms.GroupBox()
        Me.btn_FilterClose = New System.Windows.Forms.Button()
        Me.btn_FilterOpen = New System.Windows.Forms.Button()
        Me.dgv_Filter = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.grp_Find.SuspendLayout()
        Me.pnl_back.SuspendLayout()
        Me.grp_Filter.SuspendLayout()
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grp_Find
        '
        Me.grp_Find.Controls.Add(Me.cbo_Find)
        Me.grp_Find.Controls.Add(Me.btn_FindClose)
        Me.grp_Find.Controls.Add(Me.btn_FindOpen)
        Me.grp_Find.Location = New System.Drawing.Point(6, 245)
        Me.grp_Find.Name = "grp_Find"
        Me.grp_Find.Size = New System.Drawing.Size(457, 220)
        Me.grp_Find.TabIndex = 2
        Me.grp_Find.TabStop = False
        Me.grp_Find.Text = "FINDING"
        '
        'cbo_Find
        '
        Me.cbo_Find.FormattingEnabled = True
        Me.cbo_Find.Location = New System.Drawing.Point(26, 31)
        Me.cbo_Find.Name = "cbo_Find"
        Me.cbo_Find.Size = New System.Drawing.Size(409, 23)
        Me.cbo_Find.TabIndex = 4
        '
        'btn_FindClose
        '
        Me.btn_FindClose.BackColor = System.Drawing.Color.DimGray
        Me.btn_FindClose.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btn_FindClose.Location = New System.Drawing.Point(360, 153)
        Me.btn_FindClose.Name = "btn_FindClose"
        Me.btn_FindClose.Size = New System.Drawing.Size(75, 35)
        Me.btn_FindClose.TabIndex = 6
        Me.btn_FindClose.Text = "&CLOSE"
        Me.btn_FindClose.UseVisualStyleBackColor = False
        '
        'btn_FindOpen
        '
        Me.btn_FindOpen.BackColor = System.Drawing.Color.DimGray
        Me.btn_FindOpen.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btn_FindOpen.Location = New System.Drawing.Point(257, 153)
        Me.btn_FindOpen.Name = "btn_FindOpen"
        Me.btn_FindOpen.Size = New System.Drawing.Size(75, 35)
        Me.btn_FindOpen.TabIndex = 5
        Me.btn_FindOpen.Text = "&Open"
        Me.btn_FindOpen.UseVisualStyleBackColor = False
        '
        'pnl_back
        '
        Me.pnl_back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_back.Controls.Add(Me.txt_Oil_Sevice)
        Me.pnl_back.Controls.Add(Me.Label3)
        Me.pnl_back.Controls.Add(Me.txt_Name)
        Me.pnl_back.Controls.Add(Me.btn_Close)
        Me.pnl_back.Controls.Add(Me.btn_Save)
        Me.pnl_back.Controls.Add(Me.lbl_IdNo)
        Me.pnl_back.Controls.Add(Me.Label2)
        Me.pnl_back.Controls.Add(Me.Label1)
        Me.pnl_back.Location = New System.Drawing.Point(6, 42)
        Me.pnl_back.Name = "pnl_back"
        Me.pnl_back.Size = New System.Drawing.Size(457, 200)
        Me.pnl_back.TabIndex = 1
        '
        'txt_Oil_Sevice
        '
        Me.txt_Oil_Sevice.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_Oil_Sevice.Location = New System.Drawing.Point(105, 109)
        Me.txt_Oil_Sevice.MaxLength = 10
        Me.txt_Oil_Sevice.Name = "txt_Oil_Sevice"
        Me.txt_Oil_Sevice.Size = New System.Drawing.Size(329, 23)
        Me.txt_Oil_Sevice.TabIndex = 1
        Me.txt_Oil_Sevice.Text = "TXT_OIL_SEVICE"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(10, 113)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(87, 15)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Oil Service Day"
        '
        'txt_Name
        '
        Me.txt_Name.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_Name.Location = New System.Drawing.Point(105, 63)
        Me.txt_Name.MaxLength = 35
        Me.txt_Name.Name = "txt_Name"
        Me.txt_Name.Size = New System.Drawing.Size(329, 23)
        Me.txt_Name.TabIndex = 0
        Me.txt_Name.Text = "TXT_NAME"
        '
        'btn_Close
        '
        Me.btn_Close.BackColor = System.Drawing.Color.DimGray
        Me.btn_Close.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btn_Close.Location = New System.Drawing.Point(359, 150)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(75, 35)
        Me.btn_Close.TabIndex = 3
        Me.btn_Close.Text = "&CLOSE"
        Me.btn_Close.UseVisualStyleBackColor = False
        '
        'btn_Save
        '
        Me.btn_Save.BackColor = System.Drawing.Color.DimGray
        Me.btn_Save.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btn_Save.Location = New System.Drawing.Point(256, 150)
        Me.btn_Save.Name = "btn_Save"
        Me.btn_Save.Size = New System.Drawing.Size(75, 35)
        Me.btn_Save.TabIndex = 2
        Me.btn_Save.Text = "&SAVE"
        Me.btn_Save.UseVisualStyleBackColor = False
        '
        'lbl_IdNo
        '
        Me.lbl_IdNo.BackColor = System.Drawing.Color.White
        Me.lbl_IdNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_IdNo.Location = New System.Drawing.Point(105, 12)
        Me.lbl_IdNo.Name = "lbl_IdNo"
        Me.lbl_IdNo.Size = New System.Drawing.Size(329, 23)
        Me.lbl_IdNo.TabIndex = 2
        Me.lbl_IdNo.Text = "lbl_IdNo"
        Me.lbl_IdNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(10, 67)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(89, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Machine Name"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(33, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "IdNo"
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.DimGray
        Me.Label4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label4.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label4.Location = New System.Drawing.Point(0, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(473, 30)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "MACHINE CREATION"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grp_Filter
        '
        Me.grp_Filter.Controls.Add(Me.btn_FilterClose)
        Me.grp_Filter.Controls.Add(Me.btn_FilterOpen)
        Me.grp_Filter.Controls.Add(Me.dgv_Filter)
        Me.grp_Filter.Location = New System.Drawing.Point(488, 171)
        Me.grp_Filter.Name = "grp_Filter"
        Me.grp_Filter.Size = New System.Drawing.Size(459, 222)
        Me.grp_Filter.TabIndex = 5
        Me.grp_Filter.TabStop = False
        Me.grp_Filter.Text = "FILTER"
        '
        'btn_FilterClose
        '
        Me.btn_FilterClose.BackColor = System.Drawing.Color.DimGray
        Me.btn_FilterClose.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_FilterClose.ForeColor = System.Drawing.Color.White
        Me.btn_FilterClose.Location = New System.Drawing.Point(362, 181)
        Me.btn_FilterClose.Name = "btn_FilterClose"
        Me.btn_FilterClose.Size = New System.Drawing.Size(75, 35)
        Me.btn_FilterClose.TabIndex = 7
        Me.btn_FilterClose.TabStop = False
        Me.btn_FilterClose.Text = "&Close"
        Me.btn_FilterClose.UseVisualStyleBackColor = False
        '
        'btn_FilterOpen
        '
        Me.btn_FilterOpen.BackColor = System.Drawing.Color.DimGray
        Me.btn_FilterOpen.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_FilterOpen.ForeColor = System.Drawing.Color.White
        Me.btn_FilterOpen.Location = New System.Drawing.Point(260, 181)
        Me.btn_FilterOpen.Name = "btn_FilterOpen"
        Me.btn_FilterOpen.Size = New System.Drawing.Size(75, 35)
        Me.btn_FilterOpen.TabIndex = 6
        Me.btn_FilterOpen.TabStop = False
        Me.btn_FilterOpen.Text = "&Open"
        Me.btn_FilterOpen.UseVisualStyleBackColor = False
        '
        'dgv_Filter
        '
        Me.dgv_Filter.AllowUserToAddRows = False
        Me.dgv_Filter.AllowUserToDeleteRows = False
        Me.dgv_Filter.BackgroundColor = System.Drawing.Color.White
        Me.dgv_Filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_Filter.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2})
        Me.dgv_Filter.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv_Filter.Location = New System.Drawing.Point(25, 22)
        Me.dgv_Filter.Name = "dgv_Filter"
        Me.dgv_Filter.ReadOnly = True
        Me.dgv_Filter.RowHeadersVisible = False
        Me.dgv_Filter.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_Filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Filter.Size = New System.Drawing.Size(412, 143)
        Me.dgv_Filter.TabIndex = 0
        '
        'Column1
        '
        Me.Column1.HeaderText = "IDNO"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Width = 50
        '
        'Column2
        '
        Me.Column2.HeaderText = "MACHINE  NAME"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Width = 340
        '
        'Machine_Creation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.PeachPuff
        Me.ClientSize = New System.Drawing.Size(473, 473)
        Me.Controls.Add(Me.grp_Filter)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.grp_Find)
        Me.Controls.Add(Me.pnl_back)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Machine_Creation"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MACHINE CREATION"
        Me.grp_Find.ResumeLayout(False)
        Me.pnl_back.ResumeLayout(False)
        Me.pnl_back.PerformLayout()
        Me.grp_Filter.ResumeLayout(False)
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grp_Find As System.Windows.Forms.GroupBox
    Friend WithEvents cbo_Find As System.Windows.Forms.ComboBox
    Friend WithEvents btn_FindClose As System.Windows.Forms.Button
    Friend WithEvents btn_FindOpen As System.Windows.Forms.Button
    Friend WithEvents pnl_back As System.Windows.Forms.Panel
    Friend WithEvents txt_Name As System.Windows.Forms.TextBox
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents btn_Save As System.Windows.Forms.Button
    Friend WithEvents lbl_IdNo As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents grp_Filter As System.Windows.Forms.GroupBox
    Friend WithEvents btn_FilterClose As System.Windows.Forms.Button
    Friend WithEvents btn_FilterOpen As System.Windows.Forms.Button
    Friend WithEvents dgv_Filter As System.Windows.Forms.DataGridView
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_Oil_Sevice As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
