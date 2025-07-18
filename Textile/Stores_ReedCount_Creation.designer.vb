<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Stores_ReedCount_Creation
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
        Me.grp_find = New System.Windows.Forms.GroupBox()
        Me.btn_FindOpen = New System.Windows.Forms.Button()
        Me.btn_FindClose = New System.Windows.Forms.Button()
        Me.cbo_Find = New System.Windows.Forms.ComboBox()
        Me.grp_Filter = New System.Windows.Forms.GroupBox()
        Me.dgv_Filter = New System.Windows.Forms.DataGridView()
        Me.btn_Filteropen = New System.Windows.Forms.Button()
        Me.btn_FilterClose = New System.Windows.Forms.Button()
        Me.txt_description = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.pnl_back = New System.Windows.Forms.Panel()
        Me.cbo_stockunder = New System.Windows.Forms.ComboBox()
        Me.txt_resultantcount = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.btn_Save = New System.Windows.Forms.Button()
        Me.txt_Name = New System.Windows.Forms.TextBox()
        Me.lbl_IdNo = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.grp_find.SuspendLayout()
        Me.grp_Filter.SuspendLayout()
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnl_back.SuspendLayout()
        Me.SuspendLayout()
        '
        'grp_find
        '
        Me.grp_find.Controls.Add(Me.btn_FindOpen)
        Me.grp_find.Controls.Add(Me.btn_FindClose)
        Me.grp_find.Controls.Add(Me.cbo_Find)
        Me.grp_find.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_find.Location = New System.Drawing.Point(8, 310)
        Me.grp_find.Name = "grp_find"
        Me.grp_find.Size = New System.Drawing.Size(516, 174)
        Me.grp_find.TabIndex = 3
        Me.grp_find.TabStop = False
        Me.grp_find.Text = "FINDING"
        '
        'btn_FindOpen
        '
        Me.btn_FindOpen.BackColor = System.Drawing.Color.Maroon
        Me.btn_FindOpen.ForeColor = System.Drawing.Color.White
        Me.btn_FindOpen.Location = New System.Drawing.Point(315, 131)
        Me.btn_FindOpen.Name = "btn_FindOpen"
        Me.btn_FindOpen.Size = New System.Drawing.Size(77, 28)
        Me.btn_FindOpen.TabIndex = 1
        Me.btn_FindOpen.Text = "&OPEN"
        Me.btn_FindOpen.UseVisualStyleBackColor = False
        '
        'btn_FindClose
        '
        Me.btn_FindClose.BackColor = System.Drawing.Color.Maroon
        Me.btn_FindClose.ForeColor = System.Drawing.Color.White
        Me.btn_FindClose.Location = New System.Drawing.Point(416, 131)
        Me.btn_FindClose.Name = "btn_FindClose"
        Me.btn_FindClose.Size = New System.Drawing.Size(77, 28)
        Me.btn_FindClose.TabIndex = 2
        Me.btn_FindClose.Text = "&CLOSE"
        Me.btn_FindClose.UseVisualStyleBackColor = False
        '
        'cbo_Find
        '
        Me.cbo_Find.FormattingEnabled = True
        Me.cbo_Find.Location = New System.Drawing.Point(18, 25)
        Me.cbo_Find.Name = "cbo_Find"
        Me.cbo_Find.Size = New System.Drawing.Size(475, 23)
        Me.cbo_Find.TabIndex = 5
        '
        'grp_Filter
        '
        Me.grp_Filter.Controls.Add(Me.dgv_Filter)
        Me.grp_Filter.Controls.Add(Me.btn_Filteropen)
        Me.grp_Filter.Controls.Add(Me.btn_FilterClose)
        Me.grp_Filter.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Filter.Location = New System.Drawing.Point(553, 310)
        Me.grp_Filter.Name = "grp_Filter"
        Me.grp_Filter.Size = New System.Drawing.Size(516, 174)
        Me.grp_Filter.TabIndex = 4
        Me.grp_Filter.TabStop = False
        Me.grp_Filter.Text = "Filter"
        '
        'dgv_Filter
        '
        Me.dgv_Filter.BackgroundColor = System.Drawing.Color.White
        Me.dgv_Filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgv_Filter.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgv_Filter.Location = New System.Drawing.Point(18, 25)
        Me.dgv_Filter.Name = "dgv_Filter"
        Me.dgv_Filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Filter.Size = New System.Drawing.Size(475, 108)
        Me.dgv_Filter.TabIndex = 1
        '
        'btn_Filteropen
        '
        Me.btn_Filteropen.BackColor = System.Drawing.Color.Maroon
        Me.btn_Filteropen.ForeColor = System.Drawing.Color.White
        Me.btn_Filteropen.Location = New System.Drawing.Point(317, 139)
        Me.btn_Filteropen.Name = "btn_Filteropen"
        Me.btn_Filteropen.Size = New System.Drawing.Size(77, 28)
        Me.btn_Filteropen.TabIndex = 10
        Me.btn_Filteropen.Text = "&OPEN"
        Me.btn_Filteropen.UseVisualStyleBackColor = False
        '
        'btn_FilterClose
        '
        Me.btn_FilterClose.BackColor = System.Drawing.Color.Maroon
        Me.btn_FilterClose.ForeColor = System.Drawing.Color.White
        Me.btn_FilterClose.Location = New System.Drawing.Point(418, 139)
        Me.btn_FilterClose.Name = "btn_FilterClose"
        Me.btn_FilterClose.Size = New System.Drawing.Size(77, 28)
        Me.btn_FilterClose.TabIndex = 1
        Me.btn_FilterClose.Text = "&CLOSE"
        Me.btn_FilterClose.UseVisualStyleBackColor = False
        '
        'txt_description
        '
        Me.txt_description.Location = New System.Drawing.Point(181, 97)
        Me.txt_description.MaxLength = 35
        Me.txt_description.Name = "txt_description"
        Me.txt_description.Size = New System.Drawing.Size(311, 23)
        Me.txt_description.TabIndex = 1
        Me.txt_description.Text = "txt_description"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(16, 100)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(78, 15)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "DESCRIPTION"
        '
        'pnl_back
        '
        Me.pnl_back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_back.Controls.Add(Me.cbo_stockunder)
        Me.pnl_back.Controls.Add(Me.txt_resultantcount)
        Me.pnl_back.Controls.Add(Me.Label6)
        Me.pnl_back.Controls.Add(Me.Label4)
        Me.pnl_back.Controls.Add(Me.txt_description)
        Me.pnl_back.Controls.Add(Me.Label5)
        Me.pnl_back.Controls.Add(Me.btn_Close)
        Me.pnl_back.Controls.Add(Me.btn_Save)
        Me.pnl_back.Controls.Add(Me.txt_Name)
        Me.pnl_back.Controls.Add(Me.lbl_IdNo)
        Me.pnl_back.Controls.Add(Me.Label3)
        Me.pnl_back.Controls.Add(Me.Label2)
        Me.pnl_back.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnl_back.Location = New System.Drawing.Point(6, 46)
        Me.pnl_back.Name = "pnl_back"
        Me.pnl_back.Size = New System.Drawing.Size(518, 256)
        Me.pnl_back.TabIndex = 1
        '
        'cbo_stockunder
        '
        Me.cbo_stockunder.FormattingEnabled = True
        Me.cbo_stockunder.Location = New System.Drawing.Point(181, 135)
        Me.cbo_stockunder.MaxLength = 35
        Me.cbo_stockunder.Name = "cbo_stockunder"
        Me.cbo_stockunder.Size = New System.Drawing.Size(311, 23)
        Me.cbo_stockunder.TabIndex = 2
        Me.cbo_stockunder.Text = "cbo_stock"
        '
        'txt_resultantcount
        '
        Me.txt_resultantcount.Location = New System.Drawing.Point(181, 173)
        Me.txt_resultantcount.MaxLength = 35
        Me.txt_resultantcount.Name = "txt_resultantcount"
        Me.txt_resultantcount.Size = New System.Drawing.Size(311, 23)
        Me.txt_resultantcount.TabIndex = 3
        Me.txt_resultantcount.Text = "txt-count"
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(16, 135)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(148, 38)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "COMBINE THIS STOCK WITH FOLLWING COUNT"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(16, 176)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(109, 15)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "RESULTANT COUNT"
        '
        'btn_Close
        '
        Me.btn_Close.BackColor = System.Drawing.Color.Maroon
        Me.btn_Close.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Close.ForeColor = System.Drawing.Color.White
        Me.btn_Close.Location = New System.Drawing.Point(417, 211)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(77, 28)
        Me.btn_Close.TabIndex = 5
        Me.btn_Close.Text = "&CLOSE"
        Me.btn_Close.UseVisualStyleBackColor = False
        '
        'btn_Save
        '
        Me.btn_Save.BackColor = System.Drawing.Color.Maroon
        Me.btn_Save.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Save.ForeColor = System.Drawing.Color.White
        Me.btn_Save.Location = New System.Drawing.Point(316, 211)
        Me.btn_Save.Name = "btn_Save"
        Me.btn_Save.Size = New System.Drawing.Size(77, 28)
        Me.btn_Save.TabIndex = 4
        Me.btn_Save.Text = "&SAVE"
        Me.btn_Save.UseVisualStyleBackColor = False
        '
        'txt_Name
        '
        Me.txt_Name.Location = New System.Drawing.Point(181, 59)
        Me.txt_Name.MaxLength = 35
        Me.txt_Name.Name = "txt_Name"
        Me.txt_Name.Size = New System.Drawing.Size(311, 23)
        Me.txt_Name.TabIndex = 0
        Me.txt_Name.Text = "TXT_NAME"
        '
        'lbl_IdNo
        '
        Me.lbl_IdNo.BackColor = System.Drawing.Color.White
        Me.lbl_IdNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_IdNo.Location = New System.Drawing.Point(181, 21)
        Me.lbl_IdNo.Name = "lbl_IdNo"
        Me.lbl_IdNo.Size = New System.Drawing.Size(311, 23)
        Me.lbl_IdNo.TabIndex = 3
        Me.lbl_IdNo.Text = "lbl_IdNo"
        Me.lbl_IdNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(16, 62)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(40, 15)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Name"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(16, 21)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(33, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "IdNo"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Maroon
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(529, 37)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "REED COUNT CREATION"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ReedCount_Creation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.DarkSalmon
        Me.ClientSize = New System.Drawing.Size(529, 307)
        Me.Controls.Add(Me.grp_find)
        Me.Controls.Add(Me.grp_Filter)
        Me.Controls.Add(Me.pnl_back)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "ReedCount_Creation"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "REED COUNT CREATION"
        Me.grp_find.ResumeLayout(False)
        Me.grp_Filter.ResumeLayout(False)
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnl_back.ResumeLayout(False)
        Me.pnl_back.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grp_find As System.Windows.Forms.GroupBox
    Friend WithEvents btn_FindOpen As System.Windows.Forms.Button
    Friend WithEvents btn_FindClose As System.Windows.Forms.Button
    Friend WithEvents cbo_Find As System.Windows.Forms.ComboBox
    Friend WithEvents btn_Filteropen As System.Windows.Forms.Button
    Friend WithEvents btn_FilterClose As System.Windows.Forms.Button
    Friend WithEvents dgv_Filter As System.Windows.Forms.DataGridView
    Friend WithEvents txt_description As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents grp_Filter As System.Windows.Forms.GroupBox
    Friend WithEvents pnl_back As System.Windows.Forms.Panel
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents btn_Save As System.Windows.Forms.Button
    Friend WithEvents txt_Name As System.Windows.Forms.TextBox
    Friend WithEvents lbl_IdNo As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txt_resultantcount As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cbo_stockunder As System.Windows.Forms.ComboBox
End Class
