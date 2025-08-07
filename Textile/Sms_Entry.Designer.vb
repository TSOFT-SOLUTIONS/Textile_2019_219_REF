<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Sms_Entry
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
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txt_response = New System.Windows.Forms.TextBox()
        Me.btnSend_WpSMS = New System.Windows.Forms.Button()
        Me.btn_AttachmentSelection = New System.Windows.Forms.Button()
        Me.txt_Attachment = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txt_Msg = New System.Windows.Forms.TextBox()
        Me.btnSendSMS = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.txt_PhnNo = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.lbl_Heading = New System.Windows.Forms.Label()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.pnl_Back.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnl_Back
        '
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.Label2)
        Me.pnl_Back.Controls.Add(Me.txt_response)
        Me.pnl_Back.Controls.Add(Me.btnSend_WpSMS)
        Me.pnl_Back.Controls.Add(Me.btn_AttachmentSelection)
        Me.pnl_Back.Controls.Add(Me.txt_Attachment)
        Me.pnl_Back.Controls.Add(Me.Label3)
        Me.pnl_Back.Controls.Add(Me.Label1)
        Me.pnl_Back.Controls.Add(Me.Label19)
        Me.pnl_Back.Controls.Add(Me.txt_Msg)
        Me.pnl_Back.Controls.Add(Me.btnSendSMS)
        Me.pnl_Back.Controls.Add(Me.btnClose)
        Me.pnl_Back.Controls.Add(Me.txt_PhnNo)
        Me.pnl_Back.Controls.Add(Me.Label11)
        Me.pnl_Back.Location = New System.Drawing.Point(5, 35)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(480, 285)
        Me.pnl_Back.TabIndex = 35
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.Navy
        Me.Label2.Location = New System.Drawing.Point(12, 193)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(63, 15)
        Me.Label2.TabIndex = 70
        Me.Label2.Text = "RESPONSE"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_response
        '
        Me.txt_response.Enabled = False
        Me.txt_response.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_response.Location = New System.Drawing.Point(107, 185)
        Me.txt_response.MaxLength = 50
        Me.txt_response.Name = "txt_response"
        Me.txt_response.Size = New System.Drawing.Size(352, 23)
        Me.txt_response.TabIndex = 69
        '
        'btnSend_WpSMS
        '
        Me.btnSend_WpSMS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnSend_WpSMS.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSend_WpSMS.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSend_WpSMS.ForeColor = System.Drawing.Color.Navy
        Me.btnSend_WpSMS.Image = Global.Textile.My.Resources.Resources.Whatsapp_Logo1
        Me.btnSend_WpSMS.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSend_WpSMS.Location = New System.Drawing.Point(200, 240)
        Me.btnSend_WpSMS.Name = "btnSend_WpSMS"
        Me.btnSend_WpSMS.Size = New System.Drawing.Size(155, 31)
        Me.btnSend_WpSMS.TabIndex = 68
        Me.btnSend_WpSMS.TabStop = False
        Me.btnSend_WpSMS.Text = "WHATSAPP SMS"
        Me.btnSend_WpSMS.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSend_WpSMS.UseVisualStyleBackColor = True
        '
        'btn_AttachmentSelection
        '
        Me.btn_AttachmentSelection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn_AttachmentSelection.Location = New System.Drawing.Point(435, 156)
        Me.btn_AttachmentSelection.Name = "btn_AttachmentSelection"
        Me.btn_AttachmentSelection.Size = New System.Drawing.Size(25, 23)
        Me.btn_AttachmentSelection.TabIndex = 67
        Me.btn_AttachmentSelection.TabStop = False
        Me.btn_AttachmentSelection.Text = "..."
        Me.btn_AttachmentSelection.UseVisualStyleBackColor = True
        '
        'txt_Attachment
        '
        Me.txt_Attachment.Enabled = False
        Me.txt_Attachment.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Attachment.Location = New System.Drawing.Point(107, 156)
        Me.txt_Attachment.MaxLength = 50
        Me.txt_Attachment.Name = "txt_Attachment"
        Me.txt_Attachment.Size = New System.Drawing.Size(326, 23)
        Me.txt_Attachment.TabIndex = 66
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.Color.Navy
        Me.Label3.Location = New System.Drawing.Point(12, 160)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(80, 15)
        Me.Label3.TabIndex = 65
        Me.Label3.Text = "ATTACHMENT"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Navy
        Me.Label1.Location = New System.Drawing.Point(12, 51)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 15)
        Me.Label1.TabIndex = 60
        Me.Label1.Text = "MESSAGE"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.ForeColor = System.Drawing.Color.Navy
        Me.Label19.Location = New System.Drawing.Point(16, 55)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(0, 15)
        Me.Label19.TabIndex = 59
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_Msg
        '
        Me.txt_Msg.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Msg.Location = New System.Drawing.Point(107, 47)
        Me.txt_Msg.MaxLength = 1000
        Me.txt_Msg.Multiline = True
        Me.txt_Msg.Name = "txt_Msg"
        Me.txt_Msg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txt_Msg.Size = New System.Drawing.Size(352, 103)
        Me.txt_Msg.TabIndex = 1
        '
        'btnSendSMS
        '
        Me.btnSendSMS.BackColor = System.Drawing.Color.Red
        Me.btnSendSMS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnSendSMS.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSendSMS.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSendSMS.ForeColor = System.Drawing.Color.Navy
        Me.btnSendSMS.Image = Global.Textile.My.Resources.Resources.NextUp
        Me.btnSendSMS.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSendSMS.Location = New System.Drawing.Point(88, 240)
        Me.btnSendSMS.Name = "btnSendSMS"
        Me.btnSendSMS.Size = New System.Drawing.Size(106, 31)
        Me.btnSendSMS.TabIndex = 2
        Me.btnSendSMS.TabStop = False
        Me.btnSendSMS.Text = "SEND SMS"
        Me.btnSendSMS.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSendSMS.UseVisualStyleBackColor = False
        Me.btnSendSMS.Visible = False
        '
        'btnClose
        '
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ForeColor = System.Drawing.Color.Navy
        Me.btnClose.Image = Global.Textile.My.Resources.Resources.Close1
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(370, 240)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(89, 31)
        Me.btnClose.TabIndex = 3
        Me.btnClose.TabStop = False
        Me.btnClose.Text = "CLOSE"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'txt_PhnNo
        '
        Me.txt_PhnNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_PhnNo.Location = New System.Drawing.Point(107, 18)
        Me.txt_PhnNo.MaxLength = 50
        Me.txt_PhnNo.Name = "txt_PhnNo"
        Me.txt_PhnNo.Size = New System.Drawing.Size(353, 23)
        Me.txt_PhnNo.TabIndex = 0
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.ForeColor = System.Drawing.Color.Navy
        Me.Label11.Location = New System.Drawing.Point(12, 22)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(88, 15)
        Me.Label11.TabIndex = 19
        Me.Label11.Text = "TO MOBILE NO"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbl_Heading
        '
        Me.lbl_Heading.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.lbl_Heading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_Heading.Dock = System.Windows.Forms.DockStyle.Top
        Me.lbl_Heading.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Heading.ForeColor = System.Drawing.Color.White
        Me.lbl_Heading.Location = New System.Drawing.Point(0, 0)
        Me.lbl_Heading.Name = "lbl_Heading"
        Me.lbl_Heading.Size = New System.Drawing.Size(497, 30)
        Me.lbl_Heading.TabIndex = 36
        Me.lbl_Heading.Text = "MESSAGE"
        Me.lbl_Heading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'Sms_Entry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(497, 331)
        Me.Controls.Add(Me.lbl_Heading)
        Me.Controls.Add(Me.pnl_Back)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Sms_Entry"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Sms"
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_Back.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents txt_Msg As System.Windows.Forms.TextBox
    Friend WithEvents btnSendSMS As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents txt_PhnNo As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents lbl_Heading As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As Label
    Friend WithEvents txt_Attachment As TextBox
    Friend WithEvents btn_AttachmentSelection As Button
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents btnSend_WpSMS As Button
    Friend WithEvents txt_response As TextBox
    Friend WithEvents Label2 As Label
End Class
