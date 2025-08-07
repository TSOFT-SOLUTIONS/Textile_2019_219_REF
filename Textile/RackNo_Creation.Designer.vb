<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RackNo_Creation
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
        Me.pnl_back = New System.Windows.Forms.Panel()
        Me.txt_RackNo = New System.Windows.Forms.TextBox()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.btn_Save = New System.Windows.Forms.Button()
        Me.lbl_IdNo = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dgv_Filter = New System.Windows.Forms.DataGridView()
        Me.cbo_Find = New System.Windows.Forms.ComboBox()
        Me.grp_Find = New System.Windows.Forms.GroupBox()
        Me.btn_FindClose = New System.Windows.Forms.Button()
        Me.btn_FindOpen = New System.Windows.Forms.Button()
        Me.grp_Filter = New System.Windows.Forms.GroupBox()
        Me.btn_FilterOpen = New System.Windows.Forms.Button()
        Me.btn_FilterClose = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.chk_Close = New System.Windows.Forms.CheckBox()
        Me.pnl_back.SuspendLayout()
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grp_Find.SuspendLayout()
        Me.grp_Filter.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnl_back
        '
        Me.pnl_back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_back.Controls.Add(Me.chk_Close)
        Me.pnl_back.Controls.Add(Me.txt_RackNo)
        Me.pnl_back.Controls.Add(Me.btn_Close)
        Me.pnl_back.Controls.Add(Me.btn_Save)
        Me.pnl_back.Controls.Add(Me.lbl_IdNo)
        Me.pnl_back.Controls.Add(Me.Label2)
        Me.pnl_back.Controls.Add(Me.Label1)
        Me.pnl_back.Location = New System.Drawing.Point(12, 51)
        Me.pnl_back.Name = "pnl_back"
        Me.pnl_back.Size = New System.Drawing.Size(339, 168)
        Me.pnl_back.TabIndex = 0
        '
        'txt_RackNo
        '
        Me.txt_RackNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_RackNo.Location = New System.Drawing.Point(96, 69)
        Me.txt_RackNo.MaxLength = 35
        Me.txt_RackNo.Name = "txt_RackNo"
        Me.txt_RackNo.Size = New System.Drawing.Size(225, 23)
        Me.txt_RackNo.TabIndex = 0
        Me.txt_RackNo.Text = "TXT_RACKNO"
        '
        'btn_Close
        '
        Me.btn_Close.BackColor = System.Drawing.SystemColors.ButtonShadow
        Me.btn_Close.Location = New System.Drawing.Point(246, 116)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(75, 23)
        Me.btn_Close.TabIndex = 2
        Me.btn_Close.Text = "&CLOSE"
        Me.btn_Close.UseVisualStyleBackColor = False
        '
        'btn_Save
        '
        Me.btn_Save.BackColor = System.Drawing.SystemColors.ButtonShadow
        Me.btn_Save.Location = New System.Drawing.Point(152, 116)
        Me.btn_Save.Name = "btn_Save"
        Me.btn_Save.Size = New System.Drawing.Size(75, 23)
        Me.btn_Save.TabIndex = 1
        Me.btn_Save.Text = "&SAVE"
        Me.btn_Save.UseVisualStyleBackColor = False
        '
        'lbl_IdNo
        '
        Me.lbl_IdNo.BackColor = System.Drawing.Color.White
        Me.lbl_IdNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_IdNo.Location = New System.Drawing.Point(96, 26)
        Me.lbl_IdNo.Name = "lbl_IdNo"
        Me.lbl_IdNo.Size = New System.Drawing.Size(222, 23)
        Me.lbl_IdNo.TabIndex = 2
        Me.lbl_IdNo.Text = "lbl_IdNo"
        Me.lbl_IdNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 72)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(47, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "RackNo"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(33, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "IdNo"
        '
        'dgv_Filter
        '
        Me.dgv_Filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_Filter.Location = New System.Drawing.Point(16, 21)
        Me.dgv_Filter.Name = "dgv_Filter"
        Me.dgv_Filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Filter.Size = New System.Drawing.Size(305, 107)
        Me.dgv_Filter.TabIndex = 8
        '
        'cbo_Find
        '
        Me.cbo_Find.FormattingEnabled = True
        Me.cbo_Find.Location = New System.Drawing.Point(16, 24)
        Me.cbo_Find.Name = "cbo_Find"
        Me.cbo_Find.Size = New System.Drawing.Size(305, 23)
        Me.cbo_Find.TabIndex = 7
        '
        'grp_Find
        '
        Me.grp_Find.Controls.Add(Me.cbo_Find)
        Me.grp_Find.Controls.Add(Me.btn_FindClose)
        Me.grp_Find.Controls.Add(Me.btn_FindOpen)
        Me.grp_Find.Location = New System.Drawing.Point(12, 237)
        Me.grp_Find.Name = "grp_Find"
        Me.grp_Find.Size = New System.Drawing.Size(339, 133)
        Me.grp_Find.TabIndex = 0
        Me.grp_Find.TabStop = False
        Me.grp_Find.Text = "FINDING"
        '
        'btn_FindClose
        '
        Me.btn_FindClose.BackColor = System.Drawing.SystemColors.ButtonShadow
        Me.btn_FindClose.Location = New System.Drawing.Point(246, 71)
        Me.btn_FindClose.Name = "btn_FindClose"
        Me.btn_FindClose.Size = New System.Drawing.Size(75, 23)
        Me.btn_FindClose.TabIndex = 7
        Me.btn_FindClose.Text = "&CLOSE"
        Me.btn_FindClose.UseVisualStyleBackColor = False
        '
        'btn_FindOpen
        '
        Me.btn_FindOpen.BackColor = System.Drawing.SystemColors.ButtonShadow
        Me.btn_FindOpen.Location = New System.Drawing.Point(152, 71)
        Me.btn_FindOpen.Name = "btn_FindOpen"
        Me.btn_FindOpen.Size = New System.Drawing.Size(75, 23)
        Me.btn_FindOpen.TabIndex = 6
        Me.btn_FindOpen.Text = "&OPEN"
        Me.btn_FindOpen.UseVisualStyleBackColor = False
        '
        'grp_Filter
        '
        Me.grp_Filter.Controls.Add(Me.dgv_Filter)
        Me.grp_Filter.Controls.Add(Me.btn_FilterOpen)
        Me.grp_Filter.Controls.Add(Me.btn_FilterClose)
        Me.grp_Filter.Location = New System.Drawing.Point(378, 237)
        Me.grp_Filter.Name = "grp_Filter"
        Me.grp_Filter.Size = New System.Drawing.Size(339, 163)
        Me.grp_Filter.TabIndex = 0
        Me.grp_Filter.TabStop = False
        Me.grp_Filter.Text = "FILTER"
        '
        'btn_FilterOpen
        '
        Me.btn_FilterOpen.BackColor = System.Drawing.SystemColors.ButtonShadow
        Me.btn_FilterOpen.Location = New System.Drawing.Point(152, 134)
        Me.btn_FilterOpen.Name = "btn_FilterOpen"
        Me.btn_FilterOpen.Size = New System.Drawing.Size(75, 23)
        Me.btn_FilterOpen.TabIndex = 8
        Me.btn_FilterOpen.Text = "&OPEN"
        Me.btn_FilterOpen.UseVisualStyleBackColor = False
        '
        'btn_FilterClose
        '
        Me.btn_FilterClose.BackColor = System.Drawing.SystemColors.ButtonShadow
        Me.btn_FilterClose.Location = New System.Drawing.Point(246, 134)
        Me.btn_FilterClose.Name = "btn_FilterClose"
        Me.btn_FilterClose.Size = New System.Drawing.Size(75, 23)
        Me.btn_FilterClose.TabIndex = 9
        Me.btn_FilterClose.Text = "&CLOSE"
        Me.btn_FilterClose.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.SystemColors.ButtonShadow
        Me.Label4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label4.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(0, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(370, 36)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "RACKNO CREATION"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'chk_Close
        '
        Me.chk_Close.AutoSize = True
        Me.chk_Close.BackColor = System.Drawing.Color.White
        Me.chk_Close.Location = New System.Drawing.Point(16, 120)
        Me.chk_Close.Name = "chk_Close"
        Me.chk_Close.Size = New System.Drawing.Size(55, 19)
        Me.chk_Close.TabIndex = 3
        Me.chk_Close.Text = "&Close"
        Me.chk_Close.UseVisualStyleBackColor = False
        '
        'RackNo_Creation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.ClientSize = New System.Drawing.Size(370, 398)
        Me.Controls.Add(Me.grp_Filter)
        Me.Controls.Add(Me.grp_Find)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.pnl_back)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MinimizeBox = False
        Me.Name = "RackNo_Creation"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "RACKNO CREATION"
        Me.pnl_back.ResumeLayout(False)
        Me.pnl_back.PerformLayout()
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grp_Find.ResumeLayout(False)
        Me.grp_Filter.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnl_back As System.Windows.Forms.Panel
    Friend WithEvents dgv_Filter As System.Windows.Forms.DataGridView
    Friend WithEvents cbo_Find As System.Windows.Forms.ComboBox
    Friend WithEvents txt_RackNo As System.Windows.Forms.TextBox
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents btn_Save As System.Windows.Forms.Button
    Friend WithEvents lbl_IdNo As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents grp_Find As System.Windows.Forms.GroupBox
    Friend WithEvents btn_FindOpen As System.Windows.Forms.Button
    Friend WithEvents btn_FindClose As System.Windows.Forms.Button
    Friend WithEvents grp_Filter As System.Windows.Forms.GroupBox
    Friend WithEvents btn_FilterOpen As System.Windows.Forms.Button
    Friend WithEvents btn_FilterClose As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents chk_Close As System.Windows.Forms.CheckBox
End Class
