<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Auto_Correct_PartyLedger_Bill_to_Bill_and_BalanceOnly
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
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.btn_Search_LedgerName = New System.Windows.Forms.Button()
        Me.lbl_LedgerGroup_Caption = New System.Windows.Forms.Label()
        Me.cbo_LedgerName_Search = New System.Windows.Forms.ComboBox()
        Me.btn_AutoCorrect_Selected = New System.Windows.Forms.Button()
        Me.chk_SelectAll = New System.Windows.Forms.CheckBox()
        Me.btn_REFERESH = New System.Windows.Forms.Button()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.dgv_Details = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column11 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.pnl_Back.SuspendLayout()
        CType(Me.dgv_Details, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnl_Back
        '
        Me.pnl_Back.BackColor = System.Drawing.Color.LightSkyBlue
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.btn_Search_LedgerName)
        Me.pnl_Back.Controls.Add(Me.lbl_LedgerGroup_Caption)
        Me.pnl_Back.Controls.Add(Me.cbo_LedgerName_Search)
        Me.pnl_Back.Controls.Add(Me.btn_AutoCorrect_Selected)
        Me.pnl_Back.Controls.Add(Me.chk_SelectAll)
        Me.pnl_Back.Controls.Add(Me.btn_REFERESH)
        Me.pnl_Back.Controls.Add(Me.btn_Close)
        Me.pnl_Back.Controls.Add(Me.dgv_Details)
        Me.pnl_Back.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnl_Back.Location = New System.Drawing.Point(6, 42)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(812, 463)
        Me.pnl_Back.TabIndex = 270
        '
        'btn_Search_LedgerName
        '
        Me.btn_Search_LedgerName.BackColor = System.Drawing.Color.Firebrick
        Me.btn_Search_LedgerName.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn_Search_LedgerName.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn_Search_LedgerName.ForeColor = System.Drawing.Color.White
        Me.btn_Search_LedgerName.Location = New System.Drawing.Point(442, 15)
        Me.btn_Search_LedgerName.Name = "btn_Search_LedgerName"
        Me.btn_Search_LedgerName.Size = New System.Drawing.Size(77, 23)
        Me.btn_Search_LedgerName.TabIndex = 1
        Me.btn_Search_LedgerName.TabStop = False
        Me.btn_Search_LedgerName.Text = "SEARCH...."
        Me.btn_Search_LedgerName.UseVisualStyleBackColor = False
        '
        'lbl_LedgerGroup_Caption
        '
        Me.lbl_LedgerGroup_Caption.AutoSize = True
        Me.lbl_LedgerGroup_Caption.BackColor = System.Drawing.Color.Transparent
        Me.lbl_LedgerGroup_Caption.ForeColor = System.Drawing.Color.Navy
        Me.lbl_LedgerGroup_Caption.Location = New System.Drawing.Point(26, 19)
        Me.lbl_LedgerGroup_Caption.Name = "lbl_LedgerGroup_Caption"
        Me.lbl_LedgerGroup_Caption.Size = New System.Drawing.Size(81, 15)
        Me.lbl_LedgerGroup_Caption.TabIndex = 1188
        Me.lbl_LedgerGroup_Caption.Text = "Ledger Name"
        Me.lbl_LedgerGroup_Caption.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cbo_LedgerName_Search
        '
        Me.cbo_LedgerName_Search.BackColor = System.Drawing.Color.White
        Me.cbo_LedgerName_Search.DropDownHeight = 450
        Me.cbo_LedgerName_Search.DropDownWidth = 600
        Me.cbo_LedgerName_Search.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_LedgerName_Search.FormattingEnabled = True
        Me.cbo_LedgerName_Search.IntegralHeight = False
        Me.cbo_LedgerName_Search.Location = New System.Drawing.Point(111, 15)
        Me.cbo_LedgerName_Search.Name = "cbo_LedgerName_Search"
        Me.cbo_LedgerName_Search.Size = New System.Drawing.Size(325, 23)
        Me.cbo_LedgerName_Search.TabIndex = 0
        '
        'btn_AutoCorrect_Selected
        '
        Me.btn_AutoCorrect_Selected.BackColor = System.Drawing.Color.DimGray
        Me.btn_AutoCorrect_Selected.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn_AutoCorrect_Selected.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn_AutoCorrect_Selected.ForeColor = System.Drawing.Color.White
        Me.btn_AutoCorrect_Selected.Location = New System.Drawing.Point(599, 11)
        Me.btn_AutoCorrect_Selected.Name = "btn_AutoCorrect_Selected"
        Me.btn_AutoCorrect_Selected.Size = New System.Drawing.Size(195, 31)
        Me.btn_AutoCorrect_Selected.TabIndex = 2
        Me.btn_AutoCorrect_Selected.TabStop = False
        Me.btn_AutoCorrect_Selected.Text = "AUTO CORRECT  -  SELECTED"
        Me.btn_AutoCorrect_Selected.UseVisualStyleBackColor = False
        '
        'chk_SelectAll
        '
        Me.chk_SelectAll.AutoSize = True
        Me.chk_SelectAll.Location = New System.Drawing.Point(740, 64)
        Me.chk_SelectAll.Name = "chk_SelectAll"
        Me.chk_SelectAll.Size = New System.Drawing.Size(15, 14)
        Me.chk_SelectAll.TabIndex = 1185
        Me.chk_SelectAll.UseVisualStyleBackColor = True
        '
        'btn_REFERESH
        '
        Me.btn_REFERESH.BackColor = System.Drawing.Color.MediumVioletRed
        Me.btn_REFERESH.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn_REFERESH.ForeColor = System.Drawing.Color.White
        Me.btn_REFERESH.Location = New System.Drawing.Point(14, 417)
        Me.btn_REFERESH.Name = "btn_REFERESH"
        Me.btn_REFERESH.Size = New System.Drawing.Size(93, 31)
        Me.btn_REFERESH.TabIndex = 4
        Me.btn_REFERESH.TabStop = False
        Me.btn_REFERESH.Text = "REFERSH LIST"
        Me.btn_REFERESH.UseVisualStyleBackColor = False
        '
        'btn_Close
        '
        Me.btn_Close.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(90, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btn_Close.ForeColor = System.Drawing.Color.White
        Me.btn_Close.Location = New System.Drawing.Point(702, 417)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(92, 31)
        Me.btn_Close.TabIndex = 5
        Me.btn_Close.TabStop = False
        Me.btn_Close.Text = "&Close"
        Me.btn_Close.UseVisualStyleBackColor = False
        '
        'dgv_Details
        '
        Me.dgv_Details.AllowUserToAddRows = False
        Me.dgv_Details.AllowUserToResizeColumns = False
        Me.dgv_Details.AllowUserToResizeRows = False
        Me.dgv_Details.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle4.BackColor = System.Drawing.Color.SeaGreen
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.SeaGreen
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_Details.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.dgv_Details.ColumnHeadersHeight = 33
        Me.dgv_Details.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgv_Details.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column8, Me.Column3, Me.Column4, Me.Column11})
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.Lime
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgv_Details.DefaultCellStyle = DataGridViewCellStyle6
        Me.dgv_Details.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.dgv_Details.EnableHeadersVisualStyles = False
        Me.dgv_Details.Location = New System.Drawing.Point(14, 53)
        Me.dgv_Details.MultiSelect = False
        Me.dgv_Details.Name = "dgv_Details"
        Me.dgv_Details.RowHeadersVisible = False
        Me.dgv_Details.RowTemplate.Height = 23
        Me.dgv_Details.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_Details.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.dgv_Details.Size = New System.Drawing.Size(780, 353)
        Me.dgv_Details.TabIndex = 3
        Me.dgv_Details.TabStop = False
        '
        'Column1
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Column1.DefaultCellStyle = DataGridViewCellStyle5
        Me.Column1.HeaderText = "S.NO"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column1.Width = 50
        '
        'Column2
        '
        Me.Column2.HeaderText = "IDNO"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column2.Visible = False
        '
        'Column8
        '
        Me.Column8.HeaderText = "LEDGER NAME"
        Me.Column8.Name = "Column8"
        Me.Column8.ReadOnly = True
        Me.Column8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column8.Width = 550
        '
        'Column3
        '
        Me.Column3.HeaderText = "BALANCE ONLY  AMOUNT"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        Me.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column3.Visible = False
        Me.Column3.Width = 120
        '
        'Column4
        '
        Me.Column4.HeaderText = "BILL-TO-BILL  AMOUNT"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        Me.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column4.Visible = False
        Me.Column4.Width = 120
        '
        'Column11
        '
        Me.Column11.HeaderText = "SELECT STATUS"
        Me.Column11.Name = "Column11"
        Me.Column11.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Column11.Width = 150
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.DarkSlateGray
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(819, 35)
        Me.Label1.TabIndex = 269
        Me.Label1.Text = "PARTY LEDGER  -  AUTO CORRECT  BILL-TO-BILL  &&  BALANCE-ONLY"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Auto_Correct_PartyLedger_Bill_to_Bill_and_BalanceOnly
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(819, 507)
        Me.Controls.Add(Me.pnl_Back)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Auto_Correct_PartyLedger_Bill_to_Bill_and_BalanceOnly"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "PARTY LEDGER  -  AUTO CORRECT  BILL-TO-BILL  &  BALANCE-ONLY"
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_Back.PerformLayout()
        CType(Me.dgv_Details, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents dgv_Details As System.Windows.Forms.DataGridView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btn_REFERESH As Button
    Friend WithEvents chk_SelectAll As CheckBox
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents Column8 As DataGridViewTextBoxColumn
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
    Friend WithEvents Column4 As DataGridViewTextBoxColumn
    Friend WithEvents Column11 As DataGridViewCheckBoxColumn
    Friend WithEvents btn_AutoCorrect_Selected As Button
    Friend WithEvents btn_Search_LedgerName As Button
    Friend WithEvents lbl_LedgerGroup_Caption As Label
    Friend WithEvents cbo_LedgerName_Search As ComboBox
End Class
