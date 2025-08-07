<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DashBoard_1005
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgv_OverDueInvoices = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewLinkColumn1 = New System.Windows.Forms.DataGridViewLinkColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgv_OverDueBills = New System.Windows.Forms.DataGridView()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewLinkColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lbl_OverDue_Bills = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.lbl_Inv_31to60 = New System.Windows.Forms.Label()
        Me.lbl_Inv_61to90 = New System.Windows.Forms.Label()
        Me.lbl_Inv_91to120 = New System.Windows.Forms.Label()
        Me.lbl_Inv_Above120 = New System.Windows.Forms.Label()
        Me.lbl_Inv_1to30 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.lbl_Bill_1to30 = New System.Windows.Forms.Label()
        Me.lbl_Bill_Above120 = New System.Windows.Forms.Label()
        Me.lbl_Bill_91to120 = New System.Windows.Forms.Label()
        Me.lbl_Bill_61to90 = New System.Windows.Forms.Label()
        Me.lbl_Bill_31to60 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.pnl_OverDue = New System.Windows.Forms.Panel()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        CType(Me.dgv_OverDueInvoices, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgv_OverDueBills, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnl_Back.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.pnl_OverDue.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgv_OverDueInvoices
        '
        Me.dgv_OverDueInvoices.AllowDrop = True
        Me.dgv_OverDueInvoices.AllowUserToAddRows = False
        Me.dgv_OverDueInvoices.AllowUserToDeleteRows = False
        Me.dgv_OverDueInvoices.AllowUserToResizeColumns = False
        Me.dgv_OverDueInvoices.AllowUserToResizeRows = False
        Me.dgv_OverDueInvoices.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dgv_OverDueInvoices.BackgroundColor = System.Drawing.Color.WhiteSmoke
        Me.dgv_OverDueInvoices.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgv_OverDueInvoices.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal
        Me.dgv_OverDueInvoices.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.DarkGray
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(4, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(122, Byte), Integer))
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(4, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(122, Byte), Integer))
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_OverDueInvoices.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgv_OverDueInvoices.ColumnHeadersHeight = 25
        Me.dgv_OverDueInvoices.ColumnHeadersVisible = False
        Me.dgv_OverDueInvoices.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.DataGridViewTextBoxColumn4, Me.DataGridViewTextBoxColumn5, Me.DataGridViewLinkColumn1, Me.Column3})
        Me.dgv_OverDueInvoices.Cursor = System.Windows.Forms.Cursors.Hand
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(24, Byte), Integer), CType(CType(152, Byte), Integer), CType(CType(203, Byte), Integer))
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgv_OverDueInvoices.DefaultCellStyle = DataGridViewCellStyle4
        Me.dgv_OverDueInvoices.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv_OverDueInvoices.Location = New System.Drawing.Point(6, 19)
        Me.dgv_OverDueInvoices.Name = "dgv_OverDueInvoices"
        Me.dgv_OverDueInvoices.ReadOnly = True
        Me.dgv_OverDueInvoices.RowHeadersVisible = False
        Me.dgv_OverDueInvoices.RowHeadersWidth = 40
        Me.dgv_OverDueInvoices.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_OverDueInvoices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_OverDueInvoices.Size = New System.Drawing.Size(386, 436)
        Me.dgv_OverDueInvoices.TabIndex = 8
        '
        'Column1
        '
        Me.Column1.HeaderText = "*"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Width = 15
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.HeaderText = "OVERDUE INVOICES"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        Me.DataGridViewTextBoxColumn4.Width = 230
        '
        'DataGridViewTextBoxColumn5
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(199, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(232, Byte), Integer))
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black
        Me.DataGridViewTextBoxColumn5.DefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridViewTextBoxColumn5.HeaderText = ""
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        Me.DataGridViewTextBoxColumn5.Width = 90
        '
        'DataGridViewLinkColumn1
        '
        Me.DataGridViewLinkColumn1.ActiveLinkColor = System.Drawing.Color.Lime
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.DataGridViewLinkColumn1.DefaultCellStyle = DataGridViewCellStyle3
        Me.DataGridViewLinkColumn1.HeaderText = ""
        Me.DataGridViewLinkColumn1.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.DataGridViewLinkColumn1.Name = "DataGridViewLinkColumn1"
        Me.DataGridViewLinkColumn1.ReadOnly = True
        Me.DataGridViewLinkColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewLinkColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DataGridViewLinkColumn1.Visible = False
        Me.DataGridViewLinkColumn1.VisitedLinkColor = System.Drawing.Color.FromArgb(CType(CType(202, Byte), Integer), CType(CType(199, Byte), Integer), CType(CType(167, Byte), Integer))
        Me.DataGridViewLinkColumn1.Width = 70
        '
        'Column3
        '
        Me.Column3.HeaderText = "Ledger_idno"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        Me.Column3.Visible = False
        '
        'dgv_OverDueBills
        '
        Me.dgv_OverDueBills.AllowDrop = True
        Me.dgv_OverDueBills.AllowUserToAddRows = False
        Me.dgv_OverDueBills.AllowUserToDeleteRows = False
        Me.dgv_OverDueBills.AllowUserToResizeColumns = False
        Me.dgv_OverDueBills.AllowUserToResizeRows = False
        Me.dgv_OverDueBills.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dgv_OverDueBills.BackgroundColor = System.Drawing.Color.White
        Me.dgv_OverDueBills.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgv_OverDueBills.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal
        Me.dgv_OverDueBills.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.Color.DarkGray
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(4, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(122, Byte), Integer))
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(4, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(122, Byte), Integer))
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_OverDueBills.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle5
        Me.dgv_OverDueBills.ColumnHeadersHeight = 25
        Me.dgv_OverDueBills.ColumnHeadersVisible = False
        Me.dgv_OverDueBills.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column2, Me.DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn2, Me.DataGridViewTextBoxColumn3, Me.Column4})
        Me.dgv_OverDueBills.Cursor = System.Windows.Forms.Cursors.Hand
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle8.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(24, Byte), Integer), CType(CType(152, Byte), Integer), CType(CType(203, Byte), Integer))
        DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgv_OverDueBills.DefaultCellStyle = DataGridViewCellStyle8
        Me.dgv_OverDueBills.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv_OverDueBills.Location = New System.Drawing.Point(6, 18)
        Me.dgv_OverDueBills.Name = "dgv_OverDueBills"
        Me.dgv_OverDueBills.ReadOnly = True
        Me.dgv_OverDueBills.RowHeadersVisible = False
        Me.dgv_OverDueBills.RowHeadersWidth = 40
        Me.dgv_OverDueBills.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_OverDueBills.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_OverDueBills.Size = New System.Drawing.Size(429, 438)
        Me.dgv_OverDueBills.TabIndex = 7
        '
        'Column2
        '
        Me.Column2.HeaderText = "*"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Width = 15
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "OVERDUE INVOICES"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Width = 230
        '
        'DataGridViewTextBoxColumn2
        '
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(CType(CType(199, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(232, Byte), Integer))
        DataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black
        Me.DataGridViewTextBoxColumn2.DefaultCellStyle = DataGridViewCellStyle6
        Me.DataGridViewTextBoxColumn2.HeaderText = ""
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Width = 90
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.ActiveLinkColor = System.Drawing.Color.Lime
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.DataGridViewTextBoxColumn3.DefaultCellStyle = DataGridViewCellStyle7
        Me.DataGridViewTextBoxColumn3.HeaderText = ""
        Me.DataGridViewTextBoxColumn3.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DataGridViewTextBoxColumn3.VisitedLinkColor = System.Drawing.Color.FromArgb(CType(CType(202, Byte), Integer), CType(CType(199, Byte), Integer), CType(CType(167, Byte), Integer))
        Me.DataGridViewTextBoxColumn3.Width = 70
        '
        'Column4
        '
        Me.Column4.HeaderText = "Ledger_idno"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        Me.Column4.Visible = False
        '
        'lbl_OverDue_Bills
        '
        Me.lbl_OverDue_Bills.AutoSize = True
        Me.lbl_OverDue_Bills.BackColor = System.Drawing.Color.Transparent
        Me.lbl_OverDue_Bills.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_OverDue_Bills.ForeColor = System.Drawing.Color.MidnightBlue
        Me.lbl_OverDue_Bills.Location = New System.Drawing.Point(3, -2)
        Me.lbl_OverDue_Bills.Name = "lbl_OverDue_Bills"
        Me.lbl_OverDue_Bills.Size = New System.Drawing.Size(147, 16)
        Me.lbl_OverDue_Bills.TabIndex = 6
        Me.lbl_OverDue_Bills.Text = "OverDue Sales Bills"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.MidnightBlue
        Me.Label2.Location = New System.Drawing.Point(12, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(172, 16)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "OverDue Purchase Bills"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.MidnightBlue
        Me.Label8.Location = New System.Drawing.Point(8, 12)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(107, 16)
        Me.Label8.TabIndex = 10
        Me.Label8.Text = "Purchase Bills"
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.White
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.749999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(6, Byte), Integer), CType(CType(123, Byte), Integer), CType(CType(126, Byte), Integer))
        Me.Label9.Location = New System.Drawing.Point(9, 33)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(96, 23)
        Me.Label9.TabIndex = 11
        Me.Label9.Tag = ""
        Me.Label9.Text = "1-30 days overdue"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.Color.White
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.749999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.FromArgb(CType(CType(6, Byte), Integer), CType(CType(123, Byte), Integer), CType(CType(126, Byte), Integer))
        Me.Label10.Location = New System.Drawing.Point(9, 59)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(96, 23)
        Me.Label10.TabIndex = 12
        Me.Label10.Tag = ""
        Me.Label10.Text = "31-60 days overdue"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.Color.White
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.749999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.FromArgb(CType(CType(6, Byte), Integer), CType(CType(123, Byte), Integer), CType(CType(126, Byte), Integer))
        Me.Label11.Location = New System.Drawing.Point(9, 85)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(96, 23)
        Me.Label11.TabIndex = 13
        Me.Label11.Tag = ""
        Me.Label11.Text = "61-90 days overdue"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label12
        '
        Me.Label12.BackColor = System.Drawing.Color.White
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.749999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.FromArgb(CType(CType(6, Byte), Integer), CType(CType(123, Byte), Integer), CType(CType(126, Byte), Integer))
        Me.Label12.Location = New System.Drawing.Point(9, 111)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(96, 23)
        Me.Label12.TabIndex = 14
        Me.Label12.Tag = ""
        Me.Label12.Text = "91-120 days overdue"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label13
        '
        Me.Label13.BackColor = System.Drawing.Color.White
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.749999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.Color.FromArgb(CType(CType(6, Byte), Integer), CType(CType(123, Byte), Integer), CType(CType(126, Byte), Integer))
        Me.Label13.Location = New System.Drawing.Point(9, 137)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(96, 23)
        Me.Label13.TabIndex = 15
        Me.Label13.Tag = ""
        Me.Label13.Text = "> 120 days overdue"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbl_Inv_31to60
        '
        Me.lbl_Inv_31to60.BackColor = System.Drawing.Color.FromArgb(CType(CType(199, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.lbl_Inv_31to60.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Inv_31to60.ForeColor = System.Drawing.Color.Black
        Me.lbl_Inv_31to60.Location = New System.Drawing.Point(111, 59)
        Me.lbl_Inv_31to60.Name = "lbl_Inv_31to60"
        Me.lbl_Inv_31to60.Size = New System.Drawing.Size(154, 23)
        Me.lbl_Inv_31to60.TabIndex = 17
        Me.lbl_Inv_31to60.Tag = ""
        Me.lbl_Inv_31to60.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbl_Inv_61to90
        '
        Me.lbl_Inv_61to90.BackColor = System.Drawing.Color.FromArgb(CType(CType(199, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.lbl_Inv_61to90.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Inv_61to90.ForeColor = System.Drawing.Color.Black
        Me.lbl_Inv_61to90.Location = New System.Drawing.Point(111, 85)
        Me.lbl_Inv_61to90.Name = "lbl_Inv_61to90"
        Me.lbl_Inv_61to90.Size = New System.Drawing.Size(154, 23)
        Me.lbl_Inv_61to90.TabIndex = 18
        Me.lbl_Inv_61to90.Tag = ""
        Me.lbl_Inv_61to90.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbl_Inv_91to120
        '
        Me.lbl_Inv_91to120.BackColor = System.Drawing.Color.FromArgb(CType(CType(199, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.lbl_Inv_91to120.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Inv_91to120.ForeColor = System.Drawing.Color.Black
        Me.lbl_Inv_91to120.Location = New System.Drawing.Point(111, 111)
        Me.lbl_Inv_91to120.Name = "lbl_Inv_91to120"
        Me.lbl_Inv_91to120.Size = New System.Drawing.Size(154, 23)
        Me.lbl_Inv_91to120.TabIndex = 19
        Me.lbl_Inv_91to120.Tag = ""
        Me.lbl_Inv_91to120.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbl_Inv_Above120
        '
        Me.lbl_Inv_Above120.BackColor = System.Drawing.Color.FromArgb(CType(CType(199, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.lbl_Inv_Above120.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Inv_Above120.ForeColor = System.Drawing.Color.Black
        Me.lbl_Inv_Above120.Location = New System.Drawing.Point(111, 137)
        Me.lbl_Inv_Above120.Name = "lbl_Inv_Above120"
        Me.lbl_Inv_Above120.Size = New System.Drawing.Size(154, 23)
        Me.lbl_Inv_Above120.TabIndex = 20
        Me.lbl_Inv_Above120.Tag = ""
        Me.lbl_Inv_Above120.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbl_Inv_1to30
        '
        Me.lbl_Inv_1to30.BackColor = System.Drawing.Color.FromArgb(CType(CType(199, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.lbl_Inv_1to30.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Inv_1to30.ForeColor = System.Drawing.Color.Black
        Me.lbl_Inv_1to30.Location = New System.Drawing.Point(111, 33)
        Me.lbl_Inv_1to30.Name = "lbl_Inv_1to30"
        Me.lbl_Inv_1to30.Size = New System.Drawing.Size(154, 23)
        Me.lbl_Inv_1to30.TabIndex = 21
        Me.lbl_Inv_1to30.Tag = ""
        Me.lbl_Inv_1to30.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.BackColor = System.Drawing.Color.Transparent
        Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.ForeColor = System.Drawing.Color.MidnightBlue
        Me.Label20.Location = New System.Drawing.Point(10, 171)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(82, 16)
        Me.Label20.TabIndex = 10
        Me.Label20.Text = "Sales Bills"
        '
        'lbl_Bill_1to30
        '
        Me.lbl_Bill_1to30.BackColor = System.Drawing.Color.FromArgb(CType(CType(199, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.lbl_Bill_1to30.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Bill_1to30.ForeColor = System.Drawing.Color.Black
        Me.lbl_Bill_1to30.Location = New System.Drawing.Point(113, 196)
        Me.lbl_Bill_1to30.Name = "lbl_Bill_1to30"
        Me.lbl_Bill_1to30.Size = New System.Drawing.Size(152, 23)
        Me.lbl_Bill_1to30.TabIndex = 33
        Me.lbl_Bill_1to30.Tag = ""
        Me.lbl_Bill_1to30.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbl_Bill_Above120
        '
        Me.lbl_Bill_Above120.BackColor = System.Drawing.Color.FromArgb(CType(CType(199, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.lbl_Bill_Above120.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Bill_Above120.ForeColor = System.Drawing.Color.Black
        Me.lbl_Bill_Above120.Location = New System.Drawing.Point(113, 300)
        Me.lbl_Bill_Above120.Name = "lbl_Bill_Above120"
        Me.lbl_Bill_Above120.Size = New System.Drawing.Size(152, 23)
        Me.lbl_Bill_Above120.TabIndex = 32
        Me.lbl_Bill_Above120.Tag = ""
        Me.lbl_Bill_Above120.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbl_Bill_91to120
        '
        Me.lbl_Bill_91to120.BackColor = System.Drawing.Color.FromArgb(CType(CType(199, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.lbl_Bill_91to120.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Bill_91to120.ForeColor = System.Drawing.Color.Black
        Me.lbl_Bill_91to120.Location = New System.Drawing.Point(113, 274)
        Me.lbl_Bill_91to120.Name = "lbl_Bill_91to120"
        Me.lbl_Bill_91to120.Size = New System.Drawing.Size(152, 23)
        Me.lbl_Bill_91to120.TabIndex = 31
        Me.lbl_Bill_91to120.Tag = ""
        Me.lbl_Bill_91to120.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbl_Bill_61to90
        '
        Me.lbl_Bill_61to90.BackColor = System.Drawing.Color.FromArgb(CType(CType(199, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.lbl_Bill_61to90.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Bill_61to90.ForeColor = System.Drawing.Color.Black
        Me.lbl_Bill_61to90.Location = New System.Drawing.Point(113, 248)
        Me.lbl_Bill_61to90.Name = "lbl_Bill_61to90"
        Me.lbl_Bill_61to90.Size = New System.Drawing.Size(152, 23)
        Me.lbl_Bill_61to90.TabIndex = 30
        Me.lbl_Bill_61to90.Tag = ""
        Me.lbl_Bill_61to90.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbl_Bill_31to60
        '
        Me.lbl_Bill_31to60.BackColor = System.Drawing.Color.FromArgb(CType(CType(199, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.lbl_Bill_31to60.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Bill_31to60.ForeColor = System.Drawing.Color.Black
        Me.lbl_Bill_31to60.Location = New System.Drawing.Point(113, 222)
        Me.lbl_Bill_31to60.Name = "lbl_Bill_31to60"
        Me.lbl_Bill_31to60.Size = New System.Drawing.Size(152, 23)
        Me.lbl_Bill_31to60.TabIndex = 29
        Me.lbl_Bill_31to60.Tag = ""
        Me.lbl_Bill_31to60.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label18
        '
        Me.Label18.BackColor = System.Drawing.Color.White
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.749999!, System.Drawing.FontStyle.Bold)
        Me.Label18.ForeColor = System.Drawing.Color.FromArgb(CType(CType(6, Byte), Integer), CType(CType(123, Byte), Integer), CType(CType(126, Byte), Integer))
        Me.Label18.Location = New System.Drawing.Point(10, 300)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(97, 23)
        Me.Label18.TabIndex = 27
        Me.Label18.Tag = ""
        Me.Label18.Text = "> 120 days overdue"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label19
        '
        Me.Label19.BackColor = System.Drawing.Color.White
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.749999!, System.Drawing.FontStyle.Bold)
        Me.Label19.ForeColor = System.Drawing.Color.FromArgb(CType(CType(6, Byte), Integer), CType(CType(123, Byte), Integer), CType(CType(126, Byte), Integer))
        Me.Label19.Location = New System.Drawing.Point(10, 274)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(97, 23)
        Me.Label19.TabIndex = 26
        Me.Label19.Tag = ""
        Me.Label19.Text = "91-120 days overdue"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label21
        '
        Me.Label21.BackColor = System.Drawing.Color.White
        Me.Label21.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.749999!, System.Drawing.FontStyle.Bold)
        Me.Label21.ForeColor = System.Drawing.Color.FromArgb(CType(CType(6, Byte), Integer), CType(CType(123, Byte), Integer), CType(CType(126, Byte), Integer))
        Me.Label21.Location = New System.Drawing.Point(10, 248)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(97, 23)
        Me.Label21.TabIndex = 25
        Me.Label21.Tag = ""
        Me.Label21.Text = "61-90 days overdue"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label22
        '
        Me.Label22.BackColor = System.Drawing.Color.White
        Me.Label22.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.749999!, System.Drawing.FontStyle.Bold)
        Me.Label22.ForeColor = System.Drawing.Color.FromArgb(CType(CType(6, Byte), Integer), CType(CType(123, Byte), Integer), CType(CType(126, Byte), Integer))
        Me.Label22.Location = New System.Drawing.Point(10, 222)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(97, 23)
        Me.Label22.TabIndex = 24
        Me.Label22.Tag = ""
        Me.Label22.Text = "31-60 days overdue"
        Me.Label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label23
        '
        Me.Label23.BackColor = System.Drawing.Color.White
        Me.Label23.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.749999!, System.Drawing.FontStyle.Bold)
        Me.Label23.ForeColor = System.Drawing.Color.FromArgb(CType(CType(6, Byte), Integer), CType(CType(123, Byte), Integer), CType(CType(126, Byte), Integer))
        Me.Label23.Location = New System.Drawing.Point(10, 196)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(97, 23)
        Me.Label23.TabIndex = 23
        Me.Label23.Tag = ""
        Me.Label23.Text = "1-30 days overdue"
        Me.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btn_Close
        '
        Me.btn_Close.BackColor = System.Drawing.Color.Transparent
        Me.btn_Close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.btn_Close.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btn_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btn_Close.FlatAppearance.BorderSize = 0
        Me.btn_Close.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn_Close.ForeColor = System.Drawing.Color.White
        Me.btn_Close.Location = New System.Drawing.Point(1134, 9)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(39, 23)
        Me.btn_Close.TabIndex = 12
        Me.btn_Close.Text = "<<"
        Me.btn_Close.UseVisualStyleBackColor = False
        '
        'pnl_Back
        '
        Me.pnl_Back.AutoScroll = True
        Me.pnl_Back.BackColor = System.Drawing.Color.Transparent
        Me.pnl_Back.Controls.Add(Me.Panel5)
        Me.pnl_Back.Controls.Add(Me.Label6)
        Me.pnl_Back.Controls.Add(Me.pnl_OverDue)
        Me.pnl_Back.Controls.Add(Me.Label14)
        Me.pnl_Back.Controls.Add(Me.Panel1)
        Me.pnl_Back.Controls.Add(Me.btn_Close)
        Me.pnl_Back.Location = New System.Drawing.Point(0, 0)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(1285, 534)
        Me.pnl_Back.TabIndex = 38
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.Label2)
        Me.Panel5.Controls.Add(Me.dgv_OverDueInvoices)
        Me.Panel5.Location = New System.Drawing.Point(477, 35)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(407, 471)
        Me.Panel5.TabIndex = 42
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("MS Reference Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Firebrick
        Me.Label6.Location = New System.Drawing.Point(85, 7)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(321, 22)
        Me.Label6.TabIndex = 39
        Me.Label6.Text = "Overdue Purchase && Sales Bills"
        '
        'pnl_OverDue
        '
        Me.pnl_OverDue.BackColor = System.Drawing.Color.Transparent
        Me.pnl_OverDue.Controls.Add(Me.dgv_OverDueBills)
        Me.pnl_OverDue.Controls.Add(Me.lbl_OverDue_Bills)
        Me.pnl_OverDue.ForeColor = System.Drawing.Color.White
        Me.pnl_OverDue.Location = New System.Drawing.Point(21, 34)
        Me.pnl_OverDue.Name = "pnl_OverDue"
        Me.pnl_OverDue.Size = New System.Drawing.Size(450, 472)
        Me.pnl_OverDue.TabIndex = 9
        '
        'Label14
        '
        Me.Label14.BackColor = System.Drawing.Color.Transparent
        Me.Label14.Font = New System.Drawing.Font("MS Reference Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.Firebrick
        Me.Label14.Location = New System.Drawing.Point(476, 7)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(357, 25)
        Me.Label14.TabIndex = 39
        Me.Label14.Text = "Payable && Receivable (Bill to Bill)"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Transparent
        Me.Panel1.Controls.Add(Me.lbl_Bill_1to30)
        Me.Panel1.Controls.Add(Me.Label22)
        Me.Panel1.Controls.Add(Me.lbl_Bill_Above120)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.lbl_Bill_91to120)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.lbl_Bill_61to90)
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.lbl_Bill_31to60)
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.Label18)
        Me.Panel1.Controls.Add(Me.Label12)
        Me.Panel1.Controls.Add(Me.Label19)
        Me.Panel1.Controls.Add(Me.Label13)
        Me.Panel1.Controls.Add(Me.Label21)
        Me.Panel1.Controls.Add(Me.lbl_Inv_31to60)
        Me.Panel1.Controls.Add(Me.lbl_Inv_61to90)
        Me.Panel1.Controls.Add(Me.Label23)
        Me.Panel1.Controls.Add(Me.lbl_Inv_91to120)
        Me.Panel1.Controls.Add(Me.Label20)
        Me.Panel1.Controls.Add(Me.lbl_Inv_Above120)
        Me.Panel1.Controls.Add(Me.lbl_Inv_1to30)
        Me.Panel1.Location = New System.Drawing.Point(890, 32)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(308, 370)
        Me.Panel1.TabIndex = 34
        '
        'DashBoard_1005
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(151, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.CancelButton = Me.btn_Close
        Me.ClientSize = New System.Drawing.Size(1293, 518)
        Me.Controls.Add(Me.pnl_Back)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "DashBoard_1005"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "DashBoard"
        CType(Me.dgv_OverDueInvoices, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgv_OverDueBills, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnl_Back.ResumeLayout(False)
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.pnl_OverDue.ResumeLayout(False)
        Me.pnl_OverDue.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lbl_OverDue_Bills As System.Windows.Forms.Label
    Friend WithEvents dgv_OverDueBills As System.Windows.Forms.DataGridView
    Friend WithEvents dgv_OverDueInvoices As System.Windows.Forms.DataGridView
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents lbl_Inv_31to60 As System.Windows.Forms.Label
    Friend WithEvents lbl_Inv_61to90 As System.Windows.Forms.Label
    Friend WithEvents lbl_Inv_91to120 As System.Windows.Forms.Label
    Friend WithEvents lbl_Inv_Above120 As System.Windows.Forms.Label
    Friend WithEvents lbl_Inv_1to30 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents lbl_Bill_1to30 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lbl_Bill_91to120 As System.Windows.Forms.Label
    Friend WithEvents lbl_Bill_61to90 As System.Windows.Forms.Label
    Friend WithEvents lbl_Bill_31to60 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents lbl_Bill_Above120 As System.Windows.Forms.Label
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewLinkColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents pnl_OverDue As System.Windows.Forms.Panel
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewLinkColumn1 As System.Windows.Forms.DataGridViewLinkColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
End Class
