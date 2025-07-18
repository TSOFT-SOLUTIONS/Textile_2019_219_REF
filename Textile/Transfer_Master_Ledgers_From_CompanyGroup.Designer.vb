<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Transfer_Master_Ledgers_From_CompanyGroup
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
        Me.lbl_DbIdNo_From_Caption = New System.Windows.Forms.Label()
        Me.btn_Transfer = New System.Windows.Forms.Button()
        Me.txt_DbIdNo_From = New System.Windows.Forms.TextBox()
        Me.cbo_DBFrom_Textile = New System.Windows.Forms.ComboBox()
        Me.lbl_DBFrom_Textile_Caption = New System.Windows.Forms.Label()
        Me.cbo_DBFrom_Sizing = New System.Windows.Forms.ComboBox()
        Me.lbl_DBFrom_Sizing_Caption = New System.Windows.Forms.Label()
        Me.cbo_DBFrom_OE = New System.Windows.Forms.ComboBox()
        Me.lbl_DBFrom_OE_Caption = New System.Windows.Forms.Label()
        Me.btn_Transfer_Textile = New System.Windows.Forms.Button()
        Me.btn_Transfer_Sizing = New System.Windows.Forms.Button()
        Me.btn_Transfer_OE = New System.Windows.Forms.Button()
        Me.btn_Import_From_Excel = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.SuspendLayout()
        '
        'lbl_DbIdNo_From_Caption
        '
        Me.lbl_DbIdNo_From_Caption.AutoSize = True
        Me.lbl_DbIdNo_From_Caption.ForeColor = System.Drawing.Color.Navy
        Me.lbl_DbIdNo_From_Caption.Location = New System.Drawing.Point(24, 33)
        Me.lbl_DbIdNo_From_Caption.Name = "lbl_DbIdNo_From_Caption"
        Me.lbl_DbIdNo_From_Caption.Size = New System.Drawing.Size(134, 15)
        Me.lbl_DbIdNo_From_Caption.TabIndex = 0
        Me.lbl_DbIdNo_From_Caption.Text = "Company Group From :"
        '
        'btn_Transfer
        '
        Me.btn_Transfer.Location = New System.Drawing.Point(414, 25)
        Me.btn_Transfer.Name = "btn_Transfer"
        Me.btn_Transfer.Size = New System.Drawing.Size(87, 30)
        Me.btn_Transfer.TabIndex = 1
        Me.btn_Transfer.Text = "&TRANSFER"
        Me.btn_Transfer.UseVisualStyleBackColor = True
        '
        'txt_DbIdNo_From
        '
        Me.txt_DbIdNo_From.Location = New System.Drawing.Point(166, 29)
        Me.txt_DbIdNo_From.Name = "txt_DbIdNo_From"
        Me.txt_DbIdNo_From.Size = New System.Drawing.Size(227, 23)
        Me.txt_DbIdNo_From.TabIndex = 0
        '
        'cbo_DBFrom_Textile
        '
        Me.cbo_DBFrom_Textile.FormattingEnabled = True
        Me.cbo_DBFrom_Textile.Location = New System.Drawing.Point(166, 76)
        Me.cbo_DBFrom_Textile.Name = "cbo_DBFrom_Textile"
        Me.cbo_DBFrom_Textile.Size = New System.Drawing.Size(227, 23)
        Me.cbo_DBFrom_Textile.TabIndex = 2
        Me.cbo_DBFrom_Textile.Visible = False
        '
        'lbl_DBFrom_Textile_Caption
        '
        Me.lbl_DBFrom_Textile_Caption.AutoSize = True
        Me.lbl_DBFrom_Textile_Caption.ForeColor = System.Drawing.Color.Black
        Me.lbl_DBFrom_Textile_Caption.Location = New System.Drawing.Point(24, 80)
        Me.lbl_DBFrom_Textile_Caption.Name = "lbl_DBFrom_Textile_Caption"
        Me.lbl_DBFrom_Textile_Caption.Size = New System.Drawing.Size(128, 15)
        Me.lbl_DBFrom_Textile_Caption.TabIndex = 3
        Me.lbl_DBFrom_Textile_Caption.Text = "Textile Database From"
        Me.lbl_DBFrom_Textile_Caption.Visible = False
        '
        'cbo_DBFrom_Sizing
        '
        Me.cbo_DBFrom_Sizing.FormattingEnabled = True
        Me.cbo_DBFrom_Sizing.Location = New System.Drawing.Point(168, 116)
        Me.cbo_DBFrom_Sizing.Name = "cbo_DBFrom_Sizing"
        Me.cbo_DBFrom_Sizing.Size = New System.Drawing.Size(225, 23)
        Me.cbo_DBFrom_Sizing.TabIndex = 4
        Me.cbo_DBFrom_Sizing.Visible = False
        '
        'lbl_DBFrom_Sizing_Caption
        '
        Me.lbl_DBFrom_Sizing_Caption.AutoSize = True
        Me.lbl_DBFrom_Sizing_Caption.ForeColor = System.Drawing.Color.Black
        Me.lbl_DBFrom_Sizing_Caption.Location = New System.Drawing.Point(24, 120)
        Me.lbl_DBFrom_Sizing_Caption.Name = "lbl_DBFrom_Sizing_Caption"
        Me.lbl_DBFrom_Sizing_Caption.Size = New System.Drawing.Size(122, 15)
        Me.lbl_DBFrom_Sizing_Caption.TabIndex = 5
        Me.lbl_DBFrom_Sizing_Caption.Text = "Sizing Database From"
        Me.lbl_DBFrom_Sizing_Caption.Visible = False
        '
        'cbo_DBFrom_OE
        '
        Me.cbo_DBFrom_OE.FormattingEnabled = True
        Me.cbo_DBFrom_OE.Location = New System.Drawing.Point(169, 156)
        Me.cbo_DBFrom_OE.Name = "cbo_DBFrom_OE"
        Me.cbo_DBFrom_OE.Size = New System.Drawing.Size(224, 23)
        Me.cbo_DBFrom_OE.TabIndex = 6
        Me.cbo_DBFrom_OE.Visible = False
        '
        'lbl_DBFrom_OE_Caption
        '
        Me.lbl_DBFrom_OE_Caption.AutoSize = True
        Me.lbl_DBFrom_OE_Caption.ForeColor = System.Drawing.Color.Black
        Me.lbl_DBFrom_OE_Caption.Location = New System.Drawing.Point(24, 160)
        Me.lbl_DBFrom_OE_Caption.Name = "lbl_DBFrom_OE_Caption"
        Me.lbl_DBFrom_OE_Caption.Size = New System.Drawing.Size(107, 15)
        Me.lbl_DBFrom_OE_Caption.TabIndex = 7
        Me.lbl_DBFrom_OE_Caption.Text = "OE Database From"
        Me.lbl_DBFrom_OE_Caption.Visible = False
        '
        'btn_Transfer_Textile
        '
        Me.btn_Transfer_Textile.Location = New System.Drawing.Point(414, 72)
        Me.btn_Transfer_Textile.Name = "btn_Transfer_Textile"
        Me.btn_Transfer_Textile.Size = New System.Drawing.Size(87, 30)
        Me.btn_Transfer_Textile.TabIndex = 8
        Me.btn_Transfer_Textile.Text = "&TRANSFER"
        Me.btn_Transfer_Textile.UseVisualStyleBackColor = True
        Me.btn_Transfer_Textile.Visible = False
        '
        'btn_Transfer_Sizing
        '
        Me.btn_Transfer_Sizing.Location = New System.Drawing.Point(414, 112)
        Me.btn_Transfer_Sizing.Name = "btn_Transfer_Sizing"
        Me.btn_Transfer_Sizing.Size = New System.Drawing.Size(87, 30)
        Me.btn_Transfer_Sizing.TabIndex = 9
        Me.btn_Transfer_Sizing.Text = "&TRANSFER"
        Me.btn_Transfer_Sizing.UseVisualStyleBackColor = True
        Me.btn_Transfer_Sizing.Visible = False
        '
        'btn_Transfer_OE
        '
        Me.btn_Transfer_OE.Location = New System.Drawing.Point(414, 152)
        Me.btn_Transfer_OE.Name = "btn_Transfer_OE"
        Me.btn_Transfer_OE.Size = New System.Drawing.Size(87, 30)
        Me.btn_Transfer_OE.TabIndex = 10
        Me.btn_Transfer_OE.Text = "&TRANSFER"
        Me.btn_Transfer_OE.UseVisualStyleBackColor = True
        Me.btn_Transfer_OE.Visible = False
        '
        'btn_Import_From_Excel
        '
        Me.btn_Import_From_Excel.Location = New System.Drawing.Point(349, 72)
        Me.btn_Import_From_Excel.Name = "btn_Import_From_Excel"
        Me.btn_Import_From_Excel.Size = New System.Drawing.Size(152, 30)
        Me.btn_Import_From_Excel.TabIndex = 11
        Me.btn_Import_From_Excel.Text = "&IMPORT FROM EXCEL"
        Me.btn_Import_From_Excel.UseVisualStyleBackColor = True
        Me.btn_Import_From_Excel.Visible = False
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'Transfer_Master_Ledgers_From_CompanyGroup
        '
        Me.AcceptButton = Me.btn_Transfer
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(529, 203)
        Me.Controls.Add(Me.btn_Transfer_OE)
        Me.Controls.Add(Me.btn_Transfer_Sizing)
        Me.Controls.Add(Me.btn_Transfer_Textile)
        Me.Controls.Add(Me.cbo_DBFrom_OE)
        Me.Controls.Add(Me.lbl_DBFrom_OE_Caption)
        Me.Controls.Add(Me.cbo_DBFrom_Sizing)
        Me.Controls.Add(Me.lbl_DBFrom_Sizing_Caption)
        Me.Controls.Add(Me.cbo_DBFrom_Textile)
        Me.Controls.Add(Me.lbl_DBFrom_Textile_Caption)
        Me.Controls.Add(Me.txt_DbIdNo_From)
        Me.Controls.Add(Me.btn_Transfer)
        Me.Controls.Add(Me.lbl_DbIdNo_From_Caption)
        Me.Controls.Add(Me.btn_Import_From_Excel)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Transfer_Master_Ledgers_From_CompanyGroup"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "TRANSFER  MASTERS"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lbl_DbIdNo_From_Caption As System.Windows.Forms.Label
    Friend WithEvents btn_Transfer As System.Windows.Forms.Button
    Friend WithEvents txt_DbIdNo_From As System.Windows.Forms.TextBox
    Friend WithEvents cbo_DBFrom_Textile As ComboBox
    Friend WithEvents lbl_DBFrom_Textile_Caption As Label
    Friend WithEvents cbo_DBFrom_Sizing As ComboBox
    Friend WithEvents lbl_DBFrom_Sizing_Caption As Label
    Friend WithEvents cbo_DBFrom_OE As ComboBox
    Friend WithEvents lbl_DBFrom_OE_Caption As Label
    Friend WithEvents btn_Transfer_Textile As Button
    Friend WithEvents btn_Transfer_Sizing As Button
    Friend WithEvents btn_Transfer_OE As Button
    Friend WithEvents btn_Import_From_Excel As Button
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
End Class
