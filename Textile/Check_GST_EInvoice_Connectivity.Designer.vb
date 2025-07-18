<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Check_GST_EInvoice_Connectivity
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.txt_GSTIN = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txt_IdNo = New System.Windows.Forms.TextBox()
        Me.txt_ShortName = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txt_Name = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btn_close = New System.Windows.Forms.Button()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.rtbeInvoiceResponse = New System.Windows.Forms.RichTextBox()
        Me.btn_CheckConnectivity = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btn_CheckConnectivity1 = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.btn_CheckConnectivity1)
        Me.Panel1.Controls.Add(Me.txt_GSTIN)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.txt_IdNo)
        Me.Panel1.Controls.Add(Me.txt_ShortName)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.txt_Name)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.btn_close)
        Me.Panel1.Controls.Add(Me.Label23)
        Me.Panel1.Controls.Add(Me.rtbeInvoiceResponse)
        Me.Panel1.Controls.Add(Me.btn_CheckConnectivity)
        Me.Panel1.Location = New System.Drawing.Point(6, 54)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(620, 403)
        Me.Panel1.TabIndex = 0
        '
        'txt_GSTIN
        '
        Me.txt_GSTIN.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txt_GSTIN.Enabled = False
        Me.txt_GSTIN.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_GSTIN.Location = New System.Drawing.Point(159, 130)
        Me.txt_GSTIN.Name = "txt_GSTIN"
        Me.txt_GSTIN.Size = New System.Drawing.Size(444, 23)
        Me.txt_GSTIN.TabIndex = 1179
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label3.ForeColor = System.Drawing.Color.Navy
        Me.Label3.Location = New System.Drawing.Point(10, 134)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(39, 15)
        Me.Label3.TabIndex = 1178
        Me.Label3.Text = "GSTIN"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_IdNo
        '
        Me.txt_IdNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txt_IdNo.Enabled = False
        Me.txt_IdNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_IdNo.Location = New System.Drawing.Point(159, 19)
        Me.txt_IdNo.Name = "txt_IdNo"
        Me.txt_IdNo.Size = New System.Drawing.Size(444, 23)
        Me.txt_IdNo.TabIndex = 1177
        '
        'txt_ShortName
        '
        Me.txt_ShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txt_ShortName.Enabled = False
        Me.txt_ShortName.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_ShortName.Location = New System.Drawing.Point(159, 93)
        Me.txt_ShortName.Name = "txt_ShortName"
        Me.txt_ShortName.Size = New System.Drawing.Size(444, 23)
        Me.txt_ShortName.TabIndex = 1175
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label6.ForeColor = System.Drawing.Color.Navy
        Me.Label6.Location = New System.Drawing.Point(10, 97)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(127, 15)
        Me.Label6.TabIndex = 1176
        Me.Label6.Text = "Company Short Name"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_Name
        '
        Me.txt_Name.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txt_Name.Enabled = False
        Me.txt_Name.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Name.Location = New System.Drawing.Point(159, 56)
        Me.txt_Name.Name = "txt_Name"
        Me.txt_Name.Size = New System.Drawing.Size(444, 23)
        Me.txt_Name.TabIndex = 1172
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label2.ForeColor = System.Drawing.Color.Navy
        Me.Label2.Location = New System.Drawing.Point(10, 60)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(94, 15)
        Me.Label2.TabIndex = 1173
        Me.Label2.Text = "Company Name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label4.ForeColor = System.Drawing.Color.Navy
        Me.Label4.Location = New System.Drawing.Point(10, 23)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(90, 15)
        Me.Label4.TabIndex = 1174
        Me.Label4.Text = "Company Id No"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(90, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(508, 345)
        Me.btn_close.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(95, 40)
        Me.btn_close.TabIndex = 1171
        Me.btn_close.TabStop = False
        Me.btn_close.Text = "&CLOSE"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.ForeColor = System.Drawing.Color.Navy
        Me.Label23.Location = New System.Drawing.Point(10, 173)
        Me.Label23.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(135, 15)
        Me.Label23.TabIndex = 1170
        Me.Label23.Text = "e-Invoice Site Response"
        '
        'rtbeInvoiceResponse
        '
        Me.rtbeInvoiceResponse.BackColor = System.Drawing.Color.White
        Me.rtbeInvoiceResponse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.rtbeInvoiceResponse.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtbeInvoiceResponse.ForeColor = System.Drawing.Color.Black
        Me.rtbeInvoiceResponse.Location = New System.Drawing.Point(159, 167)
        Me.rtbeInvoiceResponse.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.rtbeInvoiceResponse.Name = "rtbeInvoiceResponse"
        Me.rtbeInvoiceResponse.Size = New System.Drawing.Size(444, 132)
        Me.rtbeInvoiceResponse.TabIndex = 1169
        Me.rtbeInvoiceResponse.Text = ""
        '
        'btn_CheckConnectivity
        '
        Me.btn_CheckConnectivity.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.btn_CheckConnectivity.ForeColor = System.Drawing.Color.White
        Me.btn_CheckConnectivity.Location = New System.Drawing.Point(159, 308)
        Me.btn_CheckConnectivity.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btn_CheckConnectivity.Name = "btn_CheckConnectivity"
        Me.btn_CheckConnectivity.Size = New System.Drawing.Size(308, 40)
        Me.btn_CheckConnectivity.TabIndex = 1168
        Me.btn_CheckConnectivity.TabStop = False
        Me.btn_CheckConnectivity.Text = "CHECK CONNECTIVITY WITH  GST E-INVOICE"
        Me.btn_CheckConnectivity.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(642, 40)
        Me.Label1.TabIndex = 263
        Me.Label1.Text = "GST  e-INVOICE && E-WAY BILL CONNECTION"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btn_CheckConnectivity1
        '
        Me.btn_CheckConnectivity1.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.btn_CheckConnectivity1.ForeColor = System.Drawing.Color.White
        Me.btn_CheckConnectivity1.Location = New System.Drawing.Point(159, 352)
        Me.btn_CheckConnectivity1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btn_CheckConnectivity1.Name = "btn_CheckConnectivity1"
        Me.btn_CheckConnectivity1.Size = New System.Drawing.Size(308, 40)
        Me.btn_CheckConnectivity1.TabIndex = 1180
        Me.btn_CheckConnectivity1.TabStop = False
        Me.btn_CheckConnectivity1.Text = "CHECK CONNECTIVITY WITH GST  EWB"
        Me.btn_CheckConnectivity1.UseVisualStyleBackColor = False
        '
        'Check_GST_EInvoice_Connectivity
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(642, 469)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Name = "Check_GST_EInvoice_Connectivity"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "CHECK GST E-INVOICE CONNECTIVITY"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents rtbeInvoiceResponse As RichTextBox
    Friend WithEvents btn_CheckConnectivity As Button
    Friend WithEvents Label23 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents btn_close As Button
    Friend WithEvents txt_GSTIN As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txt_IdNo As TextBox
    Friend WithEvents txt_ShortName As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents txt_Name As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents btn_CheckConnectivity1 As Button
End Class
