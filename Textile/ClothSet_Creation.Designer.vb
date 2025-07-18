<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ClothSet_Creation
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ClothSet_Creation))
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.btn_CloseOpen = New System.Windows.Forms.Button()
        Me.btn_Filter = New System.Windows.Forms.Button()
        Me.btn_Save = New System.Windows.Forms.Button()
        Me.btn_CloseFilter = New System.Windows.Forms.Button()
        Me.lbl_IdNo = New System.Windows.Forms.Label()
        Me.lbl_Heading = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.grp_Filter = New System.Windows.Forms.GroupBox()
        Me.dgv_Filter = New System.Windows.Forms.DataGridView()
        Me.txt_Name = New System.Windows.Forms.TextBox()
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.chk_Vendor_Group = New System.Windows.Forms.CheckBox()
        Me.lbl_Company_ShortName_Caption = New System.Windows.Forms.Label()
        Me.cbo_Company_Short_Name = New System.Windows.Forms.ComboBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cbo_Open = New System.Windows.Forms.ComboBox()
        Me.grp_Open = New System.Windows.Forms.GroupBox()
        Me.btn_Open = New System.Windows.Forms.Button()
        Me.chk_CloseStatus = New System.Windows.Forms.CheckBox()
        Me.grp_Filter.SuspendLayout()
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnl_Back.SuspendLayout()
        Me.grp_Open.SuspendLayout()
        Me.SuspendLayout()
        '
        'btn_Close
        '
        Me.btn_Close.BackColor = System.Drawing.Color.MidnightBlue
        Me.btn_Close.ForeColor = System.Drawing.Color.White
        Me.btn_Close.Location = New System.Drawing.Point(357, 121)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(82, 35)
        Me.btn_Close.TabIndex = 2
        Me.btn_Close.Text = "&CLOSE"
        Me.btn_Close.UseVisualStyleBackColor = False
        '
        'btn_CloseOpen
        '
        Me.btn_CloseOpen.BackColor = System.Drawing.Color.MidnightBlue
        Me.btn_CloseOpen.ForeColor = System.Drawing.Color.White
        Me.btn_CloseOpen.Image = Global.Textile.My.Resources.Resources.cancel1
        Me.btn_CloseOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_CloseOpen.Location = New System.Drawing.Point(358, 150)
        Me.btn_CloseOpen.Name = "btn_CloseOpen"
        Me.btn_CloseOpen.Size = New System.Drawing.Size(82, 35)
        Me.btn_CloseOpen.TabIndex = 4
        Me.btn_CloseOpen.Text = "&CLOSE"
        Me.btn_CloseOpen.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_CloseOpen.UseVisualStyleBackColor = False
        '
        'btn_Filter
        '
        Me.btn_Filter.BackColor = System.Drawing.Color.MidnightBlue
        Me.btn_Filter.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Filter.ForeColor = System.Drawing.Color.White
        Me.btn_Filter.Image = CType(resources.GetObject("btn_Filter.Image"), System.Drawing.Image)
        Me.btn_Filter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Filter.Location = New System.Drawing.Point(254, 189)
        Me.btn_Filter.Name = "btn_Filter"
        Me.btn_Filter.Size = New System.Drawing.Size(82, 35)
        Me.btn_Filter.TabIndex = 35
        Me.btn_Filter.TabStop = False
        Me.btn_Filter.Text = "&OPEN"
        Me.btn_Filter.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Filter.UseVisualStyleBackColor = False
        '
        'btn_Save
        '
        Me.btn_Save.BackColor = System.Drawing.Color.MidnightBlue
        Me.btn_Save.ForeColor = System.Drawing.Color.White
        Me.btn_Save.Location = New System.Drawing.Point(242, 121)
        Me.btn_Save.Name = "btn_Save"
        Me.btn_Save.Size = New System.Drawing.Size(82, 35)
        Me.btn_Save.TabIndex = 4
        Me.btn_Save.Text = "&SAVE"
        Me.btn_Save.UseVisualStyleBackColor = False
        '
        'btn_CloseFilter
        '
        Me.btn_CloseFilter.BackColor = System.Drawing.Color.MidnightBlue
        Me.btn_CloseFilter.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_CloseFilter.ForeColor = System.Drawing.Color.White
        Me.btn_CloseFilter.Image = Global.Textile.My.Resources.Resources.cancel1
        Me.btn_CloseFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_CloseFilter.Location = New System.Drawing.Point(358, 189)
        Me.btn_CloseFilter.Name = "btn_CloseFilter"
        Me.btn_CloseFilter.Size = New System.Drawing.Size(82, 35)
        Me.btn_CloseFilter.TabIndex = 34
        Me.btn_CloseFilter.TabStop = False
        Me.btn_CloseFilter.Text = "&CLOSE"
        Me.btn_CloseFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_CloseFilter.UseVisualStyleBackColor = False
        '
        'lbl_IdNo
        '
        Me.lbl_IdNo.BackColor = System.Drawing.Color.White
        Me.lbl_IdNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_IdNo.Location = New System.Drawing.Point(100, 13)
        Me.lbl_IdNo.Name = "lbl_IdNo"
        Me.lbl_IdNo.Size = New System.Drawing.Size(339, 23)
        Me.lbl_IdNo.TabIndex = 3
        Me.lbl_IdNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbl_Heading
        '
        Me.lbl_Heading.BackColor = System.Drawing.Color.MidnightBlue
        Me.lbl_Heading.Dock = System.Windows.Forms.DockStyle.Top
        Me.lbl_Heading.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Heading.ForeColor = System.Drawing.Color.White
        Me.lbl_Heading.Location = New System.Drawing.Point(0, 0)
        Me.lbl_Heading.Name = "lbl_Heading"
        Me.lbl_Heading.Size = New System.Drawing.Size(489, 35)
        Me.lbl_Heading.TabIndex = 18
        Me.lbl_Heading.Text = "CLOTH SET CREATION"
        Me.lbl_Heading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(3, 51)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(40, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Name"
        '
        'grp_Filter
        '
        Me.grp_Filter.Controls.Add(Me.btn_Filter)
        Me.grp_Filter.Controls.Add(Me.btn_CloseFilter)
        Me.grp_Filter.Controls.Add(Me.dgv_Filter)
        Me.grp_Filter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Filter.Location = New System.Drawing.Point(514, 217)
        Me.grp_Filter.Name = "grp_Filter"
        Me.grp_Filter.Size = New System.Drawing.Size(460, 242)
        Me.grp_Filter.TabIndex = 21
        Me.grp_Filter.TabStop = False
        Me.grp_Filter.Text = "FILTER"
        '
        'dgv_Filter
        '
        Me.dgv_Filter.BackgroundColor = System.Drawing.Color.White
        Me.dgv_Filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_Filter.Location = New System.Drawing.Point(33, 29)
        Me.dgv_Filter.Name = "dgv_Filter"
        Me.dgv_Filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Filter.Size = New System.Drawing.Size(407, 145)
        Me.dgv_Filter.TabIndex = 0
        '
        'txt_Name
        '
        Me.txt_Name.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txt_Name.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_Name.Location = New System.Drawing.Point(100, 46)
        Me.txt_Name.MaxLength = 35
        Me.txt_Name.Name = "txt_Name"
        Me.txt_Name.Size = New System.Drawing.Size(339, 23)
        Me.txt_Name.TabIndex = 0
        '
        'pnl_Back
        '
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.chk_CloseStatus)
        Me.pnl_Back.Controls.Add(Me.chk_Vendor_Group)
        Me.pnl_Back.Controls.Add(Me.lbl_Company_ShortName_Caption)
        Me.pnl_Back.Controls.Add(Me.cbo_Company_Short_Name)
        Me.pnl_Back.Controls.Add(Me.Label23)
        Me.pnl_Back.Controls.Add(Me.btn_Close)
        Me.pnl_Back.Controls.Add(Me.btn_Save)
        Me.pnl_Back.Controls.Add(Me.lbl_IdNo)
        Me.pnl_Back.Controls.Add(Me.txt_Name)
        Me.pnl_Back.Controls.Add(Me.Label2)
        Me.pnl_Back.Controls.Add(Me.Label1)
        Me.pnl_Back.Location = New System.Drawing.Point(12, 50)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(460, 168)
        Me.pnl_Back.TabIndex = 19
        '
        'chk_Vendor_Group
        '
        Me.chk_Vendor_Group.AutoSize = True
        Me.chk_Vendor_Group.ForeColor = System.Drawing.Color.Blue
        Me.chk_Vendor_Group.Location = New System.Drawing.Point(336, 84)
        Me.chk_Vendor_Group.Name = "chk_Vendor_Group"
        Me.chk_Vendor_Group.Size = New System.Drawing.Size(103, 19)
        Me.chk_Vendor_Group.TabIndex = 3
        Me.chk_Vendor_Group.Text = "Vendor Group"
        Me.chk_Vendor_Group.UseVisualStyleBackColor = True
        Me.chk_Vendor_Group.Visible = False
        '
        'lbl_Company_ShortName_Caption
        '
        Me.lbl_Company_ShortName_Caption.AutoSize = True
        Me.lbl_Company_ShortName_Caption.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Company_ShortName_Caption.Location = New System.Drawing.Point(3, 86)
        Me.lbl_Company_ShortName_Caption.Name = "lbl_Company_ShortName_Caption"
        Me.lbl_Company_ShortName_Caption.Size = New System.Drawing.Size(94, 15)
        Me.lbl_Company_ShortName_Caption.TabIndex = 303
        Me.lbl_Company_ShortName_Caption.Text = "Company Name"
        Me.lbl_Company_ShortName_Caption.Visible = False
        '
        'cbo_Company_Short_Name
        '
        Me.cbo_Company_Short_Name.FormattingEnabled = True
        Me.cbo_Company_Short_Name.Location = New System.Drawing.Point(100, 82)
        Me.cbo_Company_Short_Name.Name = "cbo_Company_Short_Name"
        Me.cbo_Company_Short_Name.Size = New System.Drawing.Size(224, 23)
        Me.cbo_Company_Short_Name.TabIndex = 2
        Me.cbo_Company_Short_Name.Visible = False
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.ForeColor = System.Drawing.Color.Red
        Me.Label23.Location = New System.Drawing.Point(39, 49)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(13, 15)
        Me.Label23.TabIndex = 301
        Me.Label23.Text = "*"
        Me.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(3, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(33, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "IdNo"
        '
        'cbo_Open
        '
        Me.cbo_Open.FormattingEnabled = True
        Me.cbo_Open.Location = New System.Drawing.Point(23, 44)
        Me.cbo_Open.Name = "cbo_Open"
        Me.cbo_Open.Size = New System.Drawing.Size(417, 23)
        Me.cbo_Open.TabIndex = 0
        '
        'grp_Open
        '
        Me.grp_Open.Controls.Add(Me.btn_CloseOpen)
        Me.grp_Open.Controls.Add(Me.btn_Open)
        Me.grp_Open.Controls.Add(Me.cbo_Open)
        Me.grp_Open.Location = New System.Drawing.Point(12, 248)
        Me.grp_Open.Name = "grp_Open"
        Me.grp_Open.Size = New System.Drawing.Size(460, 211)
        Me.grp_Open.TabIndex = 20
        Me.grp_Open.TabStop = False
        Me.grp_Open.Text = "FINIDING"
        '
        'btn_Open
        '
        Me.btn_Open.BackColor = System.Drawing.Color.MidnightBlue
        Me.btn_Open.ForeColor = System.Drawing.Color.White
        Me.btn_Open.Image = Global.Textile.My.Resources.Resources.OPEN
        Me.btn_Open.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Open.Location = New System.Drawing.Point(243, 150)
        Me.btn_Open.Name = "btn_Open"
        Me.btn_Open.Size = New System.Drawing.Size(82, 35)
        Me.btn_Open.TabIndex = 3
        Me.btn_Open.Text = "&OPEN     "
        Me.btn_Open.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Open.UseVisualStyleBackColor = False
        '
        'chk_CloseStatus
        '
        Me.chk_CloseStatus.AutoSize = True
        Me.chk_CloseStatus.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chk_CloseStatus.ForeColor = System.Drawing.Color.Blue
        Me.chk_CloseStatus.Location = New System.Drawing.Point(6, 130)
        Me.chk_CloseStatus.Name = "chk_CloseStatus"
        Me.chk_CloseStatus.Size = New System.Drawing.Size(92, 19)
        Me.chk_CloseStatus.TabIndex = 304
        Me.chk_CloseStatus.TabStop = False
        Me.chk_CloseStatus.Text = "Close Status"
        Me.chk_CloseStatus.UseVisualStyleBackColor = True
        Me.chk_CloseStatus.Visible = False
        '
        'ClothSet_Creation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(489, 229)
        Me.Controls.Add(Me.lbl_Heading)
        Me.Controls.Add(Me.grp_Filter)
        Me.Controls.Add(Me.pnl_Back)
        Me.Controls.Add(Me.grp_Open)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ClothSet_Creation"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "CLOTH SET CREATION"
        Me.grp_Filter.ResumeLayout(False)
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_Back.PerformLayout()
        Me.grp_Open.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents btn_CloseOpen As System.Windows.Forms.Button
    Friend WithEvents btn_Filter As System.Windows.Forms.Button
    Friend WithEvents btn_Save As System.Windows.Forms.Button
    Friend WithEvents btn_CloseFilter As System.Windows.Forms.Button
    Friend WithEvents lbl_IdNo As System.Windows.Forms.Label
    Friend WithEvents lbl_Heading As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents grp_Filter As System.Windows.Forms.GroupBox
    Friend WithEvents dgv_Filter As System.Windows.Forms.DataGridView
    Friend WithEvents txt_Name As System.Windows.Forms.TextBox
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cbo_Open As System.Windows.Forms.ComboBox
    Friend WithEvents grp_Open As System.Windows.Forms.GroupBox
    Friend WithEvents btn_Open As System.Windows.Forms.Button
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents chk_Vendor_Group As CheckBox
    Friend WithEvents lbl_Company_ShortName_Caption As Label
    Friend WithEvents cbo_Company_Short_Name As ComboBox
    Friend WithEvents chk_CloseStatus As CheckBox
End Class
