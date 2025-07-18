<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Grey_Fabric_Opening_Stock
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
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.btn_close = New System.Windows.Forms.Button()
        Me.btn_save = New System.Windows.Forms.Button()
        Me.txt_Meters = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lbl_IdNo = New System.Windows.Forms.Label()
        Me.cbo_GreyFabricName = New System.Windows.Forms.ComboBox()
        Me.txt_Pcs = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lbl_Heading = New System.Windows.Forms.Label()
        Me.lbl_Company = New System.Windows.Forms.Label()
        Me.lbl_UserName = New System.Windows.Forms.Label()
        Me.pnl_Back.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnl_Back
        '
        Me.pnl_Back.AutoScroll = True
        Me.pnl_Back.AutoSize = True
        Me.pnl_Back.BackColor = System.Drawing.Color.FromArgb(CType(CType(249, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.btn_close)
        Me.pnl_Back.Controls.Add(Me.btn_save)
        Me.pnl_Back.Controls.Add(Me.txt_Meters)
        Me.pnl_Back.Controls.Add(Me.Label1)
        Me.pnl_Back.Controls.Add(Me.lbl_IdNo)
        Me.pnl_Back.Controls.Add(Me.cbo_GreyFabricName)
        Me.pnl_Back.Controls.Add(Me.txt_Pcs)
        Me.pnl_Back.Controls.Add(Me.Label19)
        Me.pnl_Back.Controls.Add(Me.Label5)
        Me.pnl_Back.Controls.Add(Me.Label2)
        Me.pnl_Back.Enabled = False
        Me.pnl_Back.Location = New System.Drawing.Point(6, 42)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(498, 258)
        Me.pnl_Back.TabIndex = 1
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(409, 204)
        Me.btn_close.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(71, 30)
        Me.btn_close.TabIndex = 4
        Me.btn_close.TabStop = False
        Me.btn_close.Text = "&CLOSE"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'btn_save
        '
        Me.btn_save.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btn_save.ForeColor = System.Drawing.Color.White
        Me.btn_save.Location = New System.Drawing.Point(312, 204)
        Me.btn_save.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(71, 30)
        Me.btn_save.TabIndex = 3
        Me.btn_save.TabStop = False
        Me.btn_save.Text = "&SAVE"
        Me.btn_save.UseVisualStyleBackColor = False
        '
        'txt_Meters
        '
        Me.txt_Meters.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Meters.Location = New System.Drawing.Point(141, 154)
        Me.txt_Meters.MaxLength = 12
        Me.txt_Meters.Name = "txt_Meters"
        Me.txt_Meters.Size = New System.Drawing.Size(339, 23)
        Me.txt_Meters.TabIndex = 2
        Me.txt_Meters.Text = "txt_Meters"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.MediumBlue
        Me.Label1.Location = New System.Drawing.Point(12, 158)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(47, 15)
        Me.Label1.TabIndex = 35
        Me.Label1.Text = "Meters"
        '
        'lbl_IdNo
        '
        Me.lbl_IdNo.BackColor = System.Drawing.Color.White
        Me.lbl_IdNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_IdNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_IdNo.Location = New System.Drawing.Point(141, 18)
        Me.lbl_IdNo.Name = "lbl_IdNo"
        Me.lbl_IdNo.Size = New System.Drawing.Size(339, 23)
        Me.lbl_IdNo.TabIndex = 0
        Me.lbl_IdNo.Text = "lbl_IdNo"
        Me.lbl_IdNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbo_GreyFabricName
        '
        Me.cbo_GreyFabricName.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_GreyFabricName.FormattingEnabled = True
        Me.cbo_GreyFabricName.Location = New System.Drawing.Point(141, 63)
        Me.cbo_GreyFabricName.MaxDropDownItems = 15
        Me.cbo_GreyFabricName.Name = "cbo_GreyFabricName"
        Me.cbo_GreyFabricName.Size = New System.Drawing.Size(339, 23)
        Me.cbo_GreyFabricName.Sorted = True
        Me.cbo_GreyFabricName.TabIndex = 0
        Me.cbo_GreyFabricName.Text = "cbo_GreyFabric Name"
        '
        'txt_Pcs
        '
        Me.txt_Pcs.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Pcs.Location = New System.Drawing.Point(141, 109)
        Me.txt_Pcs.MaxLength = 8
        Me.txt_Pcs.Name = "txt_Pcs"
        Me.txt_Pcs.Size = New System.Drawing.Size(339, 23)
        Me.txt_Pcs.TabIndex = 1
        Me.txt_Pcs.Text = "txt_pcs"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.ForeColor = System.Drawing.Color.MediumBlue
        Me.Label19.Location = New System.Drawing.Point(12, 113)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(24, 15)
        Me.Label19.TabIndex = 1
        Me.Label19.Text = "Pcs"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.MediumBlue
        Me.Label5.Location = New System.Drawing.Point(12, 67)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(104, 15)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "Grey Fabric Name"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.MediumBlue
        Me.Label2.Location = New System.Drawing.Point(12, 22)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(36, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "IdNo "
        '
        'lbl_Heading
        '
        Me.lbl_Heading.AutoEllipsis = True
        Me.lbl_Heading.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.lbl_Heading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_Heading.Dock = System.Windows.Forms.DockStyle.Top
        Me.lbl_Heading.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Heading.ForeColor = System.Drawing.Color.White
        Me.lbl_Heading.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lbl_Heading.Location = New System.Drawing.Point(0, 0)
        Me.lbl_Heading.Name = "lbl_Heading"
        Me.lbl_Heading.Size = New System.Drawing.Size(521, 35)
        Me.lbl_Heading.TabIndex = 35
        Me.lbl_Heading.Text = "GREY FABRIC OPENING STOCK"
        Me.lbl_Heading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbl_Company
        '
        Me.lbl_Company.AutoSize = True
        Me.lbl_Company.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lbl_Company.ForeColor = System.Drawing.Color.Red
        Me.lbl_Company.Location = New System.Drawing.Point(42, 9)
        Me.lbl_Company.Name = "lbl_Company"
        Me.lbl_Company.Size = New System.Drawing.Size(77, 15)
        Me.lbl_Company.TabIndex = 36
        Me.lbl_Company.Text = "lbl_Company"
        Me.lbl_Company.Visible = False
        '
        'lbl_UserName
        '
        Me.lbl_UserName.AutoSize = True
        Me.lbl_UserName.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(55, Byte), Integer), CType(CType(91, Byte), Integer))
        Me.lbl_UserName.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_UserName.ForeColor = System.Drawing.Color.White
        Me.lbl_UserName.Location = New System.Drawing.Point(409, 9)
        Me.lbl_UserName.Name = "lbl_UserName"
        Me.lbl_UserName.Size = New System.Drawing.Size(105, 19)
        Me.lbl_UserName.TabIndex = 268
        Me.lbl_UserName.Text = "USER : ADMIN"
        '
        'Grey_Fabric_Opening_Stock
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(249, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(521, 319)
        Me.Controls.Add(Me.lbl_UserName)
        Me.Controls.Add(Me.lbl_Company)
        Me.Controls.Add(Me.lbl_Heading)
        Me.Controls.Add(Me.pnl_Back)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Grey_Fabric_Opening_Stock"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "GREY FABRIC OPENING STOCK"
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_Back.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents lbl_IdNo As System.Windows.Forms.Label
    Friend WithEvents cbo_GreyFabricName As System.Windows.Forms.ComboBox
    Friend WithEvents txt_Pcs As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txt_Meters As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lbl_Heading As System.Windows.Forms.Label
    Friend WithEvents lbl_Company As System.Windows.Forms.Label
    Friend WithEvents btn_close As System.Windows.Forms.Button
    Friend WithEvents btn_save As System.Windows.Forms.Button
    Friend WithEvents lbl_UserName As System.Windows.Forms.Label
End Class
