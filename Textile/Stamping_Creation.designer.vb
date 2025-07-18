<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Stamping_Creation
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Stamping_Creation))
        Me.dgv_Filter = New System.Windows.Forms.DataGridView()
        Me.grp_Filter = New System.Windows.Forms.GroupBox()
        Me.btn_Filter = New System.Windows.Forms.Button()
        Me.btn_CloseFilter = New System.Windows.Forms.Button()
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.btn_Save = New System.Windows.Forms.Button()
        Me.lbl_IdNo = New System.Windows.Forms.Label()
        Me.txt_Name = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cbo_Open = New System.Windows.Forms.ComboBox()
        Me.grp_Open = New System.Windows.Forms.GroupBox()
        Me.btn_CloseOpen = New System.Windows.Forms.Button()
        Me.btn_Open = New System.Windows.Forms.Button()
        Me.lbl_Heading = New System.Windows.Forms.Label()
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grp_Filter.SuspendLayout()
        Me.pnl_Back.SuspendLayout()
        Me.grp_Open.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgv_Filter
        '
        Me.dgv_Filter.BackgroundColor = System.Drawing.Color.LavenderBlush
        Me.dgv_Filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_Filter.Location = New System.Drawing.Point(33, 31)
        Me.dgv_Filter.Name = "dgv_Filter"
        Me.dgv_Filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Filter.Size = New System.Drawing.Size(433, 160)
        Me.dgv_Filter.TabIndex = 0
        '
        'grp_Filter
        '
        Me.grp_Filter.BackgroundImage = CType(resources.GetObject("grp_Filter.BackgroundImage"), System.Drawing.Image)
        Me.grp_Filter.Controls.Add(Me.btn_Filter)
        Me.grp_Filter.Controls.Add(Me.btn_CloseFilter)
        Me.grp_Filter.Controls.Add(Me.dgv_Filter)
        Me.grp_Filter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Filter.Location = New System.Drawing.Point(532, 61)
        Me.grp_Filter.Name = "grp_Filter"
        Me.grp_Filter.Size = New System.Drawing.Size(481, 259)
        Me.grp_Filter.TabIndex = 21
        Me.grp_Filter.TabStop = False
        Me.grp_Filter.Text = "FILTER"
        '
        'btn_Filter
        '
        Me.btn_Filter.BackColor = System.Drawing.Color.CadetBlue
        Me.btn_Filter.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Filter.ForeColor = System.Drawing.Color.White
        Me.btn_Filter.Image = CType(resources.GetObject("btn_Filter.Image"), System.Drawing.Image)
        Me.btn_Filter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Filter.Location = New System.Drawing.Point(269, 206)
        Me.btn_Filter.Name = "btn_Filter"
        Me.btn_Filter.Size = New System.Drawing.Size(82, 35)
        Me.btn_Filter.TabIndex = 35
        Me.btn_Filter.TabStop = False
        Me.btn_Filter.Text = "&OPEN"
        Me.btn_Filter.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Filter.UseVisualStyleBackColor = False
        '
        'btn_CloseFilter
        '
        Me.btn_CloseFilter.BackColor = System.Drawing.Color.CadetBlue
        Me.btn_CloseFilter.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_CloseFilter.ForeColor = System.Drawing.Color.White
        Me.btn_CloseFilter.Image = Global.Textile.My.Resources.Resources.cancel1
        Me.btn_CloseFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_CloseFilter.Location = New System.Drawing.Point(384, 206)
        Me.btn_CloseFilter.Name = "btn_CloseFilter"
        Me.btn_CloseFilter.Size = New System.Drawing.Size(82, 35)
        Me.btn_CloseFilter.TabIndex = 34
        Me.btn_CloseFilter.TabStop = False
        Me.btn_CloseFilter.Text = "&CLOSE"
        Me.btn_CloseFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_CloseFilter.UseVisualStyleBackColor = False
        '
        'pnl_Back
        '
        Me.pnl_Back.BackColor = System.Drawing.Color.LightSkyBlue
        Me.pnl_Back.BackgroundImage = Global.Textile.My.Resources.Resources.Test_Back_Img
        Me.pnl_Back.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnl_Back.Controls.Add(Me.btn_Close)
        Me.pnl_Back.Controls.Add(Me.btn_Save)
        Me.pnl_Back.Controls.Add(Me.lbl_IdNo)
        Me.pnl_Back.Controls.Add(Me.txt_Name)
        Me.pnl_Back.Controls.Add(Me.Label2)
        Me.pnl_Back.Controls.Add(Me.Label1)
        Me.pnl_Back.Location = New System.Drawing.Point(10, 43)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(479, 210)
        Me.pnl_Back.TabIndex = 19
        '
        'btn_Close
        '
        Me.btn_Close.BackColor = System.Drawing.Color.CadetBlue
        Me.btn_Close.ForeColor = System.Drawing.Color.White
        Me.btn_Close.Image = Global.Textile.My.Resources.Resources.cancel1
        Me.btn_Close.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Close.Location = New System.Drawing.Point(372, 145)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(90, 35)
        Me.btn_Close.TabIndex = 4
        Me.btn_Close.Text = "&CLOSE  "
        Me.btn_Close.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Close.UseVisualStyleBackColor = False
        '
        'btn_Save
        '
        Me.btn_Save.BackColor = System.Drawing.Color.CadetBlue
        Me.btn_Save.ForeColor = System.Drawing.Color.White
        Me.btn_Save.Image = Global.Textile.My.Resources.Resources.SAVE1
        Me.btn_Save.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Save.Location = New System.Drawing.Point(257, 145)
        Me.btn_Save.Name = "btn_Save"
        Me.btn_Save.Size = New System.Drawing.Size(82, 35)
        Me.btn_Save.TabIndex = 3
        Me.btn_Save.Text = "&SAVE     "
        Me.btn_Save.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Save.UseVisualStyleBackColor = False
        '
        'lbl_IdNo
        '
        Me.lbl_IdNo.BackColor = System.Drawing.Color.LavenderBlush
        Me.lbl_IdNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_IdNo.Location = New System.Drawing.Point(113, 33)
        Me.lbl_IdNo.Name = "lbl_IdNo"
        Me.lbl_IdNo.Size = New System.Drawing.Size(350, 23)
        Me.lbl_IdNo.TabIndex = 3
        Me.lbl_IdNo.Text = "lbl_IdNo"
        Me.lbl_IdNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txt_Name
        '
        Me.txt_Name.BackColor = System.Drawing.Color.LavenderBlush
        Me.txt_Name.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txt_Name.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_Name.Location = New System.Drawing.Point(113, 84)
        Me.txt_Name.MaxLength = 35
        Me.txt_Name.Name = "txt_Name"
        Me.txt_Name.Size = New System.Drawing.Size(350, 23)
        Me.txt_Name.TabIndex = 0
        Me.txt_Name.Text = "TXT_NAME"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.PowderBlue
        Me.Label2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label2.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.ImageAlign = System.Drawing.ContentAlignment.TopRight
        Me.Label2.Location = New System.Drawing.Point(14, 86)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(61, 19)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.PowderBlue
        Me.Label1.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(14, 35)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 19)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "IdNo"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cbo_Open
        '
        Me.cbo_Open.BackColor = System.Drawing.Color.LavenderBlush
        Me.cbo_Open.FormattingEnabled = True
        Me.cbo_Open.Location = New System.Drawing.Point(23, 44)
        Me.cbo_Open.Name = "cbo_Open"
        Me.cbo_Open.Size = New System.Drawing.Size(443, 23)
        Me.cbo_Open.TabIndex = 0
        '
        'grp_Open
        '
        Me.grp_Open.BackgroundImage = CType(resources.GetObject("grp_Open.BackgroundImage"), System.Drawing.Image)
        Me.grp_Open.Controls.Add(Me.btn_CloseOpen)
        Me.grp_Open.Controls.Add(Me.btn_Open)
        Me.grp_Open.Controls.Add(Me.cbo_Open)
        Me.grp_Open.Location = New System.Drawing.Point(8, 276)
        Me.grp_Open.Name = "grp_Open"
        Me.grp_Open.Size = New System.Drawing.Size(481, 219)
        Me.grp_Open.TabIndex = 20
        Me.grp_Open.TabStop = False
        Me.grp_Open.Text = "FINIDING"
        '
        'btn_CloseOpen
        '
        Me.btn_CloseOpen.BackColor = System.Drawing.Color.CadetBlue
        Me.btn_CloseOpen.ForeColor = System.Drawing.Color.White
        Me.btn_CloseOpen.Image = Global.Textile.My.Resources.Resources.cancel1
        Me.btn_CloseOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_CloseOpen.Location = New System.Drawing.Point(384, 143)
        Me.btn_CloseOpen.Name = "btn_CloseOpen"
        Me.btn_CloseOpen.Size = New System.Drawing.Size(82, 35)
        Me.btn_CloseOpen.TabIndex = 4
        Me.btn_CloseOpen.Text = "&CLOSE"
        Me.btn_CloseOpen.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_CloseOpen.UseVisualStyleBackColor = False
        '
        'btn_Open
        '
        Me.btn_Open.BackColor = System.Drawing.Color.CadetBlue
        Me.btn_Open.ForeColor = System.Drawing.Color.White
        Me.btn_Open.Image = Global.Textile.My.Resources.Resources.OPEN
        Me.btn_Open.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Open.Location = New System.Drawing.Point(269, 143)
        Me.btn_Open.Name = "btn_Open"
        Me.btn_Open.Size = New System.Drawing.Size(82, 35)
        Me.btn_Open.TabIndex = 3
        Me.btn_Open.Text = "&OPEN     "
        Me.btn_Open.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Open.UseVisualStyleBackColor = False
        '
        'lbl_Heading
        '
        Me.lbl_Heading.BackColor = System.Drawing.Color.LightSteelBlue
        Me.lbl_Heading.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_Heading.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.lbl_Heading.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Heading.ForeColor = System.Drawing.Color.White
        Me.lbl_Heading.Image = Global.Textile.My.Resources.Resources.Test_Back_Img
        Me.lbl_Heading.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.lbl_Heading.Location = New System.Drawing.Point(10, 6)
        Me.lbl_Heading.Name = "lbl_Heading"
        Me.lbl_Heading.Size = New System.Drawing.Size(480, 34)
        Me.lbl_Heading.TabIndex = 18
        Me.lbl_Heading.Text = "STAMPING CREATION"
        Me.lbl_Heading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Stamping_Creation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.BackgroundImage = Global.Textile.My.Resources.Resources.Test_Back_Img
        Me.ClientSize = New System.Drawing.Size(498, 266)
        Me.Controls.Add(Me.grp_Filter)
        Me.Controls.Add(Me.pnl_Back)
        Me.Controls.Add(Me.grp_Open)
        Me.Controls.Add(Me.lbl_Heading)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Stamping_Creation"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "STAMPING_CREATION"
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grp_Filter.ResumeLayout(False)
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_Back.PerformLayout()
        Me.grp_Open.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgv_Filter As System.Windows.Forms.DataGridView
    Friend WithEvents grp_Filter As System.Windows.Forms.GroupBox
    Friend WithEvents btn_Filter As System.Windows.Forms.Button
    Friend WithEvents btn_CloseFilter As System.Windows.Forms.Button
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents btn_Save As System.Windows.Forms.Button
    Friend WithEvents lbl_IdNo As System.Windows.Forms.Label
    Friend WithEvents txt_Name As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cbo_Open As System.Windows.Forms.ComboBox
    Friend WithEvents grp_Open As System.Windows.Forms.GroupBox
    Friend WithEvents btn_CloseOpen As System.Windows.Forms.Button
    Friend WithEvents btn_Open As System.Windows.Forms.Button
    Friend WithEvents lbl_Heading As System.Windows.Forms.Label
End Class
