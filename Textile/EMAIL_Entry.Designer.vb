<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EMAIL_Entry
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.btn_AttachmentSelection = New System.Windows.Forms.Button()
        Me.txt_Attachment = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txt_SubJect = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txt_Msg = New System.Windows.Forms.TextBox()
        Me.btnSendMail = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.txt_PhnNo = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.lbl_Heading = New System.Windows.Forms.Label()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.pnl_Back.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Navy
        Me.Label1.Location = New System.Drawing.Point(17, 142)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 15)
        Me.Label1.TabIndex = 60
        Me.Label1.Text = "MESSAGE"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'pnl_Back
        '
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.btn_AttachmentSelection)
        Me.pnl_Back.Controls.Add(Me.txt_Attachment)
        Me.pnl_Back.Controls.Add(Me.Label3)
        Me.pnl_Back.Controls.Add(Me.txt_SubJect)
        Me.pnl_Back.Controls.Add(Me.Label2)
        Me.pnl_Back.Controls.Add(Me.Label1)
        Me.pnl_Back.Controls.Add(Me.Label19)
        Me.pnl_Back.Controls.Add(Me.txt_Msg)
        Me.pnl_Back.Controls.Add(Me.btnSendMail)
        Me.pnl_Back.Controls.Add(Me.btnClose)
        Me.pnl_Back.Controls.Add(Me.txt_PhnNo)
        Me.pnl_Back.Controls.Add(Me.Label11)
        Me.pnl_Back.Location = New System.Drawing.Point(5, 38)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(480, 316)
        Me.pnl_Back.TabIndex = 37
        '
        'btn_AttachmentSelection
        '
        Me.btn_AttachmentSelection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn_AttachmentSelection.Location = New System.Drawing.Point(436, 98)
        Me.btn_AttachmentSelection.Name = "btn_AttachmentSelection"
        Me.btn_AttachmentSelection.Size = New System.Drawing.Size(25, 23)
        Me.btn_AttachmentSelection.TabIndex = 3
        Me.btn_AttachmentSelection.TabStop = False
        Me.btn_AttachmentSelection.Text = "..."
        Me.btn_AttachmentSelection.UseVisualStyleBackColor = True
        '
        'txt_Attachment
        '
        Me.txt_Attachment.Enabled = False
        Me.txt_Attachment.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Attachment.Location = New System.Drawing.Point(108, 98)
        Me.txt_Attachment.MaxLength = 50
        Me.txt_Attachment.Name = "txt_Attachment"
        Me.txt_Attachment.Size = New System.Drawing.Size(326, 23)
        Me.txt_Attachment.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.Color.Navy
        Me.Label3.Location = New System.Drawing.Point(17, 102)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(80, 15)
        Me.Label3.TabIndex = 64
        Me.Label3.Text = "ATTACHMENT"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_SubJect
        '
        Me.txt_SubJect.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_SubJect.Location = New System.Drawing.Point(108, 58)
        Me.txt_SubJect.MaxLength = 50
        Me.txt_SubJect.Name = "txt_SubJect"
        Me.txt_SubJect.Size = New System.Drawing.Size(353, 23)
        Me.txt_SubJect.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.Navy
        Me.Label2.Location = New System.Drawing.Point(17, 62)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(51, 15)
        Me.Label2.TabIndex = 62
        Me.Label2.Text = "SUBJECT"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.ForeColor = System.Drawing.Color.Navy
        Me.Label19.Location = New System.Drawing.Point(16, 105)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(0, 15)
        Me.Label19.TabIndex = 59
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_Msg
        '
        Me.txt_Msg.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Msg.Location = New System.Drawing.Point(108, 138)
        Me.txt_Msg.MaxLength = 1000
        Me.txt_Msg.Multiline = True
        Me.txt_Msg.Name = "txt_Msg"
        Me.txt_Msg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txt_Msg.Size = New System.Drawing.Size(353, 103)
        Me.txt_Msg.TabIndex = 4
        Me.txt_Msg.Text = "WRITE MESSAGE"
        '
        'btnSendMail
        '
        Me.btnSendMail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnSendMail.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSendMail.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSendMail.ForeColor = System.Drawing.Color.Navy
        Me.btnSendMail.Image = Global.Textile.My.Resources.Resources.NextUp
        Me.btnSendMail.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSendMail.Location = New System.Drawing.Point(226, 258)
        Me.btnSendMail.Name = "btnSendMail"
        Me.btnSendMail.Size = New System.Drawing.Size(123, 31)
        Me.btnSendMail.TabIndex = 5
        Me.btnSendMail.TabStop = False
        Me.btnSendMail.Text = "SEND E-MAIL"
        Me.btnSendMail.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSendMail.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ForeColor = System.Drawing.Color.Navy
        Me.btnClose.Image = Global.Textile.My.Resources.Resources.Close1
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(359, 258)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(100, 31)
        Me.btnClose.TabIndex = 6
        Me.btnClose.TabStop = False
        Me.btnClose.Text = "CLOSE"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'txt_PhnNo
        '
        Me.txt_PhnNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_PhnNo.Location = New System.Drawing.Point(108, 18)
        Me.txt_PhnNo.MaxLength = 50
        Me.txt_PhnNo.Name = "txt_PhnNo"
        Me.txt_PhnNo.Size = New System.Drawing.Size(353, 23)
        Me.txt_PhnNo.TabIndex = 0
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.ForeColor = System.Drawing.Color.Navy
        Me.Label11.Location = New System.Drawing.Point(17, 22)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(22, 15)
        Me.Label11.TabIndex = 19
        Me.Label11.Text = "TO"
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
        Me.lbl_Heading.Size = New System.Drawing.Size(495, 30)
        Me.lbl_Heading.TabIndex = 38
        Me.lbl_Heading.Text = "MAIL  BOX"
        Me.lbl_Heading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'EMAIL_Entry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(495, 366)
        Me.Controls.Add(Me.pnl_Back)
        Me.Controls.Add(Me.lbl_Heading)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "EMAIL_Entry"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MAIL"
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_Back.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents txt_Msg As System.Windows.Forms.TextBox
    Friend WithEvents btnSendMail As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents txt_PhnNo As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents lbl_Heading As System.Windows.Forms.Label
    Friend WithEvents txt_SubJect As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txt_Attachment As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btn_AttachmentSelection As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
End Class
