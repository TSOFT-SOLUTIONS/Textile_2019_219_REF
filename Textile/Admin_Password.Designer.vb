<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Admin_Password
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Admin_Password))
        Me.txt_Password = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btn_Ok = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'txt_Password
        '
        Me.txt_Password.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Password.Location = New System.Drawing.Point(28, 35)
        Me.txt_Password.MaxLength = 30
        Me.txt_Password.Name = "txt_Password"
        Me.txt_Password.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txt_Password.Size = New System.Drawing.Size(254, 27)
        Me.txt_Password.TabIndex = 0
        Me.txt_Password.Text = "123456"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(28, 11)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(236, 19)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "ENTER ADMIN LOGIN PASSWORD"
        '
        'btn_Ok
        '
        Me.btn_Ok.BackColor = System.Drawing.Color.White
        Me.btn_Ok.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btn_Ok.FlatAppearance.BorderSize = 2
        Me.btn_Ok.FlatAppearance.MouseDownBackColor = System.Drawing.Color.YellowGreen
        Me.btn_Ok.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime
        Me.btn_Ok.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Ok.ForeColor = System.Drawing.Color.Maroon
        Me.btn_Ok.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Ok.Location = New System.Drawing.Point(306, 11)
        Me.btn_Ok.Name = "btn_Ok"
        Me.btn_Ok.Size = New System.Drawing.Size(63, 51)
        Me.btn_Ok.TabIndex = 1
        Me.btn_Ok.TabStop = False
        Me.btn_Ok.Text = "OK"
        Me.btn_Ok.UseVisualStyleBackColor = False
        '
        'Admin_Password
        '
        Me.AcceptButton = Me.btn_Ok
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.CancelButton = Me.btn_Ok
        Me.CausesValidation = False
        Me.ClientSize = New System.Drawing.Size(382, 74)
        Me.Controls.Add(Me.btn_Ok)
        Me.Controls.Add(Me.txt_Password)
        Me.Controls.Add(Me.Label2)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Admin_Password"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ADMIN PASSWORD"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txt_Password As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btn_Ok As System.Windows.Forms.Button
End Class
