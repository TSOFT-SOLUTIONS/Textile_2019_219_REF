<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Accounts_Group_Creation
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Accounts_Group_Creation))
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.Cbo_Group = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lbl_IdNo = New System.Windows.Forms.Label()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.btn_Save = New System.Windows.Forms.Button()
        Me.txt_Name = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dgv_Filter = New System.Windows.Forms.DataGridView()
        Me.btn_CloseFilter = New System.Windows.Forms.Button()
        Me.grp_Find = New System.Windows.Forms.GroupBox()
        Me.btn_Find = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.cbo_Find = New System.Windows.Forms.ComboBox()
        Me.btn_Open = New System.Windows.Forms.Button()
        Me.grp_Filter = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.pnl_Back.SuspendLayout()
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grp_Find.SuspendLayout()
        Me.grp_Filter.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnl_Back
        '
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.Label5)
        Me.pnl_Back.Controls.Add(Me.Label19)
        Me.pnl_Back.Controls.Add(Me.Cbo_Group)
        Me.pnl_Back.Controls.Add(Me.Label4)
        Me.pnl_Back.Controls.Add(Me.lbl_IdNo)
        Me.pnl_Back.Controls.Add(Me.btn_Close)
        Me.pnl_Back.Controls.Add(Me.btn_Save)
        Me.pnl_Back.Controls.Add(Me.txt_Name)
        Me.pnl_Back.Controls.Add(Me.Label2)
        Me.pnl_Back.Controls.Add(Me.Label1)
        Me.pnl_Back.Location = New System.Drawing.Point(9, 44)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(518, 220)
        Me.pnl_Back.TabIndex = 9
        '
        'Cbo_Group
        '
        Me.Cbo_Group.DropDownHeight = 80
        Me.Cbo_Group.FormattingEnabled = True
        Me.Cbo_Group.IntegralHeight = False
        Me.Cbo_Group.Location = New System.Drawing.Point(103, 113)
        Me.Cbo_Group.Name = "Cbo_Group"
        Me.Cbo_Group.Size = New System.Drawing.Size(390, 23)
        Me.Cbo_Group.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Navy
        Me.Label4.Location = New System.Drawing.Point(14, 117)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(41, 15)
        Me.Label4.TabIndex = 23
        Me.Label4.Text = "Group"
        '
        'lbl_IdNo
        '
        Me.lbl_IdNo.BackColor = System.Drawing.Color.White
        Me.lbl_IdNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_IdNo.Location = New System.Drawing.Point(103, 27)
        Me.lbl_IdNo.Name = "lbl_IdNo"
        Me.lbl_IdNo.Size = New System.Drawing.Size(390, 23)
        Me.lbl_IdNo.TabIndex = 22
        Me.lbl_IdNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btn_Close
        '
        Me.btn_Close.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.btn_Close.FlatAppearance.BorderColor = System.Drawing.Color.Blue
        Me.btn_Close.FlatAppearance.BorderSize = 2
        Me.btn_Close.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Yellow
        Me.btn_Close.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime
        Me.btn_Close.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Close.ForeColor = System.Drawing.Color.White
        Me.btn_Close.Location = New System.Drawing.Point(406, 156)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(87, 35)
        Me.btn_Close.TabIndex = 4
        Me.btn_Close.TabStop = False
        Me.btn_Close.Text = "&CLOSE"
        Me.btn_Close.UseVisualStyleBackColor = False
        '
        'btn_Save
        '
        Me.btn_Save.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.btn_Save.FlatAppearance.BorderSize = 2
        Me.btn_Save.FlatAppearance.MouseDownBackColor = System.Drawing.Color.YellowGreen
        Me.btn_Save.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime
        Me.btn_Save.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Save.ForeColor = System.Drawing.Color.White
        Me.btn_Save.Location = New System.Drawing.Point(293, 156)
        Me.btn_Save.Name = "btn_Save"
        Me.btn_Save.Size = New System.Drawing.Size(90, 35)
        Me.btn_Save.TabIndex = 3
        Me.btn_Save.TabStop = False
        Me.btn_Save.Text = "&SAVE"
        Me.btn_Save.UseVisualStyleBackColor = False
        '
        'txt_Name
        '
        Me.txt_Name.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_Name.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Name.Location = New System.Drawing.Point(103, 70)
        Me.txt_Name.MaxLength = 40
        Me.txt_Name.Name = "txt_Name"
        Me.txt_Name.Size = New System.Drawing.Size(390, 23)
        Me.txt_Name.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Navy
        Me.Label2.Location = New System.Drawing.Point(14, 74)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(40, 15)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Name"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Navy
        Me.Label1.Location = New System.Drawing.Point(14, 31)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(31, 15)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Idno"
        '
        'dgv_Filter
        '
        Me.dgv_Filter.AllowUserToAddRows = False
        Me.dgv_Filter.AllowUserToDeleteRows = False
        Me.dgv_Filter.AllowUserToResizeColumns = False
        Me.dgv_Filter.AllowUserToResizeRows = False
        Me.dgv_Filter.BackgroundColor = System.Drawing.Color.White
        Me.dgv_Filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_Filter.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv_Filter.Location = New System.Drawing.Point(15, 22)
        Me.dgv_Filter.Name = "dgv_Filter"
        Me.dgv_Filter.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_Filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Filter.Size = New System.Drawing.Size(482, 156)
        Me.dgv_Filter.TabIndex = 0
        '
        'btn_CloseFilter
        '
        Me.btn_CloseFilter.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_CloseFilter.Image = CType(resources.GetObject("btn_CloseFilter.Image"), System.Drawing.Image)
        Me.btn_CloseFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_CloseFilter.Location = New System.Drawing.Point(414, 184)
        Me.btn_CloseFilter.Name = "btn_CloseFilter"
        Me.btn_CloseFilter.Size = New System.Drawing.Size(83, 29)
        Me.btn_CloseFilter.TabIndex = 34
        Me.btn_CloseFilter.TabStop = False
        Me.btn_CloseFilter.Text = "&Close"
        Me.btn_CloseFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_CloseFilter.UseVisualStyleBackColor = True
        '
        'grp_Find
        '
        Me.grp_Find.Controls.Add(Me.btn_Find)
        Me.grp_Find.Controls.Add(Me.btnClose)
        Me.grp_Find.Controls.Add(Me.cbo_Find)
        Me.grp_Find.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Find.ForeColor = System.Drawing.Color.Navy
        Me.grp_Find.Location = New System.Drawing.Point(9, 279)
        Me.grp_Find.Name = "grp_Find"
        Me.grp_Find.Size = New System.Drawing.Size(518, 179)
        Me.grp_Find.TabIndex = 7
        Me.grp_Find.TabStop = False
        Me.grp_Find.Text = "FINDING"
        '
        'btn_Find
        '
        Me.btn_Find.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.btn_Find.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Find.ForeColor = System.Drawing.Color.White
        Me.btn_Find.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Find.Location = New System.Drawing.Point(318, 141)
        Me.btn_Find.Name = "btn_Find"
        Me.btn_Find.Size = New System.Drawing.Size(83, 32)
        Me.btn_Find.TabIndex = 4
        Me.btn_Find.TabStop = False
        Me.btn_Find.Text = "&FIND"
        Me.btn_Find.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.btnClose.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ForeColor = System.Drawing.Color.White
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(414, 141)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(83, 32)
        Me.btnClose.TabIndex = 5
        Me.btnClose.TabStop = False
        Me.btnClose.Text = "&CLOSE"
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'cbo_Find
        '
        Me.cbo_Find.DropDownHeight = 80
        Me.cbo_Find.FormattingEnabled = True
        Me.cbo_Find.IntegralHeight = False
        Me.cbo_Find.Location = New System.Drawing.Point(15, 22)
        Me.cbo_Find.Name = "cbo_Find"
        Me.cbo_Find.Size = New System.Drawing.Size(482, 23)
        Me.cbo_Find.TabIndex = 3
        '
        'btn_Open
        '
        Me.btn_Open.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Open.Image = CType(resources.GetObject("btn_Open.Image"), System.Drawing.Image)
        Me.btn_Open.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Open.Location = New System.Drawing.Point(318, 184)
        Me.btn_Open.Name = "btn_Open"
        Me.btn_Open.Size = New System.Drawing.Size(83, 29)
        Me.btn_Open.TabIndex = 35
        Me.btn_Open.TabStop = False
        Me.btn_Open.Text = "&Open"
        Me.btn_Open.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Open.UseVisualStyleBackColor = True
        '
        'grp_Filter
        '
        Me.grp_Filter.Controls.Add(Me.btn_Open)
        Me.grp_Filter.Controls.Add(Me.btn_CloseFilter)
        Me.grp_Filter.Controls.Add(Me.dgv_Filter)
        Me.grp_Filter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Filter.Location = New System.Drawing.Point(9, 497)
        Me.grp_Filter.Name = "grp_Filter"
        Me.grp_Filter.Size = New System.Drawing.Size(518, 223)
        Me.grp_Filter.TabIndex = 8
        Me.grp_Filter.TabStop = False
        Me.grp_Filter.Text = "FILTER"
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label3.Font = New System.Drawing.Font("Calibri", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(0, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(542, 35)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "GROUP CREATION"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.ForeColor = System.Drawing.Color.Red
        Me.Label19.Location = New System.Drawing.Point(53, 74)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(13, 15)
        Me.Label19.TabIndex = 299
        Me.Label19.Text = "*"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.Red
        Me.Label5.Location = New System.Drawing.Point(55, 117)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(13, 15)
        Me.Label5.TabIndex = 300
        Me.Label5.Text = "*"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Accounts_Group_Creation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(542, 472)
        Me.Controls.Add(Me.pnl_Back)
        Me.Controls.Add(Me.grp_Find)
        Me.Controls.Add(Me.grp_Filter)
        Me.Controls.Add(Me.Label3)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "Accounts_Group_Creation"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Group_Creation"
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_Back.PerformLayout()
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grp_Find.ResumeLayout(False)
        Me.grp_Filter.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents lbl_IdNo As System.Windows.Forms.Label
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents btn_Save As System.Windows.Forms.Button
    Friend WithEvents txt_Name As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dgv_Filter As System.Windows.Forms.DataGridView
    Friend WithEvents btn_CloseFilter As System.Windows.Forms.Button
    Friend WithEvents grp_Find As System.Windows.Forms.GroupBox
    Friend WithEvents btn_Find As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents cbo_Find As System.Windows.Forms.ComboBox
    Friend WithEvents btn_Open As System.Windows.Forms.Button
    Friend WithEvents grp_Filter As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Cbo_Group As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
End Class
