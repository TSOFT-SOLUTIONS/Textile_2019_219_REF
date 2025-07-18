<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Fibre_Lot_Creation
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Fibre_Lot_Creation))
        Me.dgv_Filter = New System.Windows.Forms.DataGridView()
        Me.grp_Filter = New System.Windows.Forms.GroupBox()
        Me.btn_Filter = New System.Windows.Forms.Button()
        Me.btn_CloseFilter = New System.Windows.Forms.Button()
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.btn_Save = New System.Windows.Forms.Button()
        Me.lbl_LotIdNo = New System.Windows.Forms.Label()
        Me.txt_Lot_No = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lbl_Lot_idno = New System.Windows.Forms.Label()
        Me.cbo_Open = New System.Windows.Forms.ComboBox()
        Me.grp_Open = New System.Windows.Forms.GroupBox()
        Me.btn_CloseOpen = New System.Windows.Forms.Button()
        Me.btn_Open = New System.Windows.Forms.Button()
        Me.lbl_Heading = New System.Windows.Forms.Label()
        Me.chk_CloseStatus = New System.Windows.Forms.CheckBox()
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grp_Filter.SuspendLayout()
        Me.pnl_Back.SuspendLayout()
        Me.grp_Open.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgv_Filter
        '
        Me.dgv_Filter.BackgroundColor = System.Drawing.Color.White
        Me.dgv_Filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_Filter.Location = New System.Drawing.Point(33, 31)
        Me.dgv_Filter.Name = "dgv_Filter"
        Me.dgv_Filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Filter.Size = New System.Drawing.Size(433, 160)
        Me.dgv_Filter.TabIndex = 0
        '
        'grp_Filter
        '
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
        Me.btn_Filter.BackColor = System.Drawing.Color.MidnightBlue
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
        Me.btn_CloseFilter.BackColor = System.Drawing.Color.MidnightBlue
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
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.chk_CloseStatus)
        Me.pnl_Back.Controls.Add(Me.btn_Close)
        Me.pnl_Back.Controls.Add(Me.btn_Save)
        Me.pnl_Back.Controls.Add(Me.lbl_LotIdNo)
        Me.pnl_Back.Controls.Add(Me.txt_Lot_No)
        Me.pnl_Back.Controls.Add(Me.Label2)
        Me.pnl_Back.Controls.Add(Me.lbl_Lot_idno)
        Me.pnl_Back.Location = New System.Drawing.Point(10, 43)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(479, 159)
        Me.pnl_Back.TabIndex = 19
        '
        'btn_Close
        '
        Me.btn_Close.BackColor = System.Drawing.Color.MidnightBlue
        Me.btn_Close.ForeColor = System.Drawing.Color.White
        Me.btn_Close.Image = Global.Textile.My.Resources.Resources.cancel1
        Me.btn_Close.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Close.Location = New System.Drawing.Point(381, 117)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(82, 37)
        Me.btn_Close.TabIndex = 4
        Me.btn_Close.Text = "&CLOSE"
        Me.btn_Close.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Close.UseVisualStyleBackColor = False
        '
        'btn_Save
        '
        Me.btn_Save.BackColor = System.Drawing.Color.MidnightBlue
        Me.btn_Save.ForeColor = System.Drawing.Color.White
        Me.btn_Save.Image = Global.Textile.My.Resources.Resources.SAVE1
        Me.btn_Save.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Save.Location = New System.Drawing.Point(266, 117)
        Me.btn_Save.Name = "btn_Save"
        Me.btn_Save.Size = New System.Drawing.Size(82, 37)
        Me.btn_Save.TabIndex = 3
        Me.btn_Save.Text = "&SAVE     "
        Me.btn_Save.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Save.UseVisualStyleBackColor = False
        '
        'lbl_LotIdNo
        '
        Me.lbl_LotIdNo.BackColor = System.Drawing.Color.White
        Me.lbl_LotIdNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_LotIdNo.Location = New System.Drawing.Point(116, 13)
        Me.lbl_LotIdNo.Name = "lbl_LotIdNo"
        Me.lbl_LotIdNo.Size = New System.Drawing.Size(350, 23)
        Me.lbl_LotIdNo.TabIndex = 3
        Me.lbl_LotIdNo.Text = "lbl_IdNo"
        Me.lbl_LotIdNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txt_Lot_No
        '
        Me.txt_Lot_No.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_Lot_No.Location = New System.Drawing.Point(116, 68)
        Me.txt_Lot_No.MaxLength = 35
        Me.txt_Lot_No.Name = "txt_Lot_No"
        Me.txt_Lot_No.Size = New System.Drawing.Size(350, 23)
        Me.txt_Lot_No.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(17, 71)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(44, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Lot No"
        '
        'lbl_Lot_idno
        '
        Me.lbl_Lot_idno.AutoSize = True
        Me.lbl_Lot_idno.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbl_Lot_idno.Location = New System.Drawing.Point(17, 17)
        Me.lbl_Lot_idno.Name = "lbl_Lot_idno"
        Me.lbl_Lot_idno.Size = New System.Drawing.Size(33, 15)
        Me.lbl_Lot_idno.TabIndex = 0
        Me.lbl_Lot_idno.Text = "IdNo"
        '
        'cbo_Open
        '
        Me.cbo_Open.FormattingEnabled = True
        Me.cbo_Open.Location = New System.Drawing.Point(23, 44)
        Me.cbo_Open.Name = "cbo_Open"
        Me.cbo_Open.Size = New System.Drawing.Size(443, 23)
        Me.cbo_Open.TabIndex = 0
        '
        'grp_Open
        '
        Me.grp_Open.Controls.Add(Me.btn_CloseOpen)
        Me.grp_Open.Controls.Add(Me.btn_Open)
        Me.grp_Open.Controls.Add(Me.cbo_Open)
        Me.grp_Open.Location = New System.Drawing.Point(10, 235)
        Me.grp_Open.Name = "grp_Open"
        Me.grp_Open.Size = New System.Drawing.Size(481, 219)
        Me.grp_Open.TabIndex = 20
        Me.grp_Open.TabStop = False
        Me.grp_Open.Text = "FINIDING"
        '
        'btn_CloseOpen
        '
        Me.btn_CloseOpen.BackColor = System.Drawing.Color.MidnightBlue
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
        Me.btn_Open.BackColor = System.Drawing.Color.MidnightBlue
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
        Me.lbl_Heading.BackColor = System.Drawing.Color.MidnightBlue
        Me.lbl_Heading.Dock = System.Windows.Forms.DockStyle.Top
        Me.lbl_Heading.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Heading.ForeColor = System.Drawing.Color.White
        Me.lbl_Heading.Location = New System.Drawing.Point(0, 0)
        Me.lbl_Heading.Name = "lbl_Heading"
        Me.lbl_Heading.Size = New System.Drawing.Size(508, 35)
        Me.lbl_Heading.TabIndex = 18
        Me.lbl_Heading.Text = "FIBRE LOT CREATION"
        Me.lbl_Heading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'chk_CloseStatus
        '
        Me.chk_CloseStatus.AutoSize = True
        Me.chk_CloseStatus.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chk_CloseStatus.ForeColor = System.Drawing.Color.Black
        Me.chk_CloseStatus.Location = New System.Drawing.Point(20, 127)
        Me.chk_CloseStatus.Name = "chk_CloseStatus"
        Me.chk_CloseStatus.Size = New System.Drawing.Size(92, 19)
        Me.chk_CloseStatus.TabIndex = 26
        Me.chk_CloseStatus.TabStop = False
        Me.chk_CloseStatus.Text = "Close Status"
        Me.chk_CloseStatus.UseVisualStyleBackColor = True
        '
        'Fibre_Lot_Creation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(508, 219)
        Me.Controls.Add(Me.grp_Filter)
        Me.Controls.Add(Me.pnl_Back)
        Me.Controls.Add(Me.grp_Open)
        Me.Controls.Add(Me.lbl_Heading)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Fibre_Lot_Creation"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FIBRE LOT CREATION"
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
    Friend WithEvents lbl_LotIdNo As System.Windows.Forms.Label
    Friend WithEvents txt_Lot_No As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lbl_Lot_idno As System.Windows.Forms.Label
    Friend WithEvents cbo_Open As System.Windows.Forms.ComboBox
    Friend WithEvents grp_Open As System.Windows.Forms.GroupBox
    Friend WithEvents btn_CloseOpen As System.Windows.Forms.Button
    Friend WithEvents btn_Open As System.Windows.Forms.Button
    Friend WithEvents lbl_Heading As System.Windows.Forms.Label
    Friend WithEvents chk_CloseStatus As CheckBox
End Class
