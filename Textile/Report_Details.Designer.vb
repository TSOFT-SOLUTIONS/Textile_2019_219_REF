<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Report_Details
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
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim ReportDataSource4 As Microsoft.Reporting.WinForms.ReportDataSource = New Microsoft.Reporting.WinForms.ReportDataSource()
        Me.ReportTempBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.Report_DataSet = New Textile.Report_DataSet()
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.pnl_ReportDetails = New System.Windows.Forms.Panel()
        Me.dgv_Report = New System.Windows.Forms.DataGridView()
        Me.RptViewer = New Microsoft.Reporting.WinForms.ReportViewer()
        Me.pnl_ReportInputs = New System.Windows.Forms.Panel()
        Me.btn_Email = New System.Windows.Forms.Button()
        Me.btn_Whatsapp = New System.Windows.Forms.Button()
        Me.cbo_Inputs11 = New System.Windows.Forms.ComboBox()
        Me.lbl_Inputs11 = New System.Windows.Forms.Label()
        Me.cbo_Inputs10 = New System.Windows.Forms.ComboBox()
        Me.lbl_Inputs10 = New System.Windows.Forms.Label()
        Me.cbo_Inputs9 = New System.Windows.Forms.ComboBox()
        Me.lbl_Inputs9 = New System.Windows.Forms.Label()
        Me.cbo_Inputs8 = New System.Windows.Forms.ComboBox()
        Me.lbl_Inputs8 = New System.Windows.Forms.Label()
        Me.msk_FromDate = New System.Windows.Forms.MaskedTextBox()
        Me.msk_ToDate = New System.Windows.Forms.MaskedTextBox()
        Me.cbo_Inputs7 = New System.Windows.Forms.ComboBox()
        Me.lbl_Inputs7 = New System.Windows.Forms.Label()
        Me.cbo_Inputs6 = New System.Windows.Forms.ComboBox()
        Me.lbl_Inputs6 = New System.Windows.Forms.Label()
        Me.cbo_Inputs5 = New System.Windows.Forms.ComboBox()
        Me.lbl_Inputs5 = New System.Windows.Forms.Label()
        Me.cbo_Inputs4 = New System.Windows.Forms.ComboBox()
        Me.lbl_Inputs4 = New System.Windows.Forms.Label()
        Me.cbo_Inputs1 = New System.Windows.Forms.ComboBox()
        Me.lbl_Inputs1 = New System.Windows.Forms.Label()
        Me.lbl_ReportHeading = New System.Windows.Forms.Label()
        Me.cbo_Inputs3 = New System.Windows.Forms.ComboBox()
        Me.lbl_Inputs3 = New System.Windows.Forms.Label()
        Me.cbo_Inputs2 = New System.Windows.Forms.ComboBox()
        Me.lbl_Inputs2 = New System.Windows.Forms.Label()
        Me.dtp_ToDate = New System.Windows.Forms.DateTimePicker()
        Me.lbl_ToDate = New System.Windows.Forms.Label()
        Me.btn_Show = New System.Windows.Forms.Button()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.dtp_FromDate = New System.Windows.Forms.DateTimePicker()
        Me.lbl_FromDate = New System.Windows.Forms.Label()
        Me.ReportTempTableAdapter = New Textile.Report_DataSetTableAdapters.ReportTempTableAdapter()
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument()
        Me.PrintDocument2 = New System.Drawing.Printing.PrintDocument()
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog()
        Me.pnl_MultiInput = New System.Windows.Forms.Panel()
        Me.lst_MultiInput_IdNos = New System.Windows.Forms.ListBox()
        Me.chklst_MultiInput = New System.Windows.Forms.CheckedListBox()
        Me.btn_MultiInput_DeSelectAll = New System.Windows.Forms.Button()
        Me.btn_MultiInput_SelectAll = New System.Windows.Forms.Button()
        Me.btn_Close_MultiInput = New System.Windows.Forms.Button()
        Me.lbl_MultiInput_Heading = New System.Windows.Forms.Label()
        CType(Me.ReportTempBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Report_DataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnl_Back.SuspendLayout()
        Me.pnl_ReportDetails.SuspendLayout()
        CType(Me.dgv_Report, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnl_ReportInputs.SuspendLayout()
        Me.pnl_MultiInput.SuspendLayout()
        Me.SuspendLayout()
        '
        'ReportTempBindingSource
        '
        Me.ReportTempBindingSource.DataMember = "ReportTemp"
        Me.ReportTempBindingSource.DataSource = Me.Report_DataSet
        '
        'Report_DataSet
        '
        Me.Report_DataSet.DataSetName = "Report_DataSet"
        Me.Report_DataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'pnl_Back
        '
        Me.pnl_Back.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.pnl_ReportDetails)
        Me.pnl_Back.Controls.Add(Me.pnl_ReportInputs)
        Me.pnl_Back.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnl_Back.Location = New System.Drawing.Point(0, 0)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(1305, 537)
        Me.pnl_Back.TabIndex = 3
        '
        'pnl_ReportDetails
        '
        Me.pnl_ReportDetails.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnl_ReportDetails.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.pnl_ReportDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_ReportDetails.Controls.Add(Me.dgv_Report)
        Me.pnl_ReportDetails.Controls.Add(Me.RptViewer)
        Me.pnl_ReportDetails.Location = New System.Drawing.Point(1, 228)
        Me.pnl_ReportDetails.Name = "pnl_ReportDetails"
        Me.pnl_ReportDetails.Size = New System.Drawing.Size(957, 333)
        Me.pnl_ReportDetails.TabIndex = 15
        '
        'dgv_Report
        '
        Me.dgv_Report.AllowUserToAddRows = False
        Me.dgv_Report.AllowUserToDeleteRows = False
        Me.dgv_Report.AllowUserToResizeColumns = False
        Me.dgv_Report.AllowUserToResizeRows = False
        DataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_Report.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle10
        Me.dgv_Report.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle11.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_Report.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle11
        Me.dgv_Report.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle12.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.Lime
        DataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.Blue
        DataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgv_Report.DefaultCellStyle = DataGridViewCellStyle12
        Me.dgv_Report.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv_Report.Location = New System.Drawing.Point(489, 41)
        Me.dgv_Report.Name = "dgv_Report"
        Me.dgv_Report.ReadOnly = True
        Me.dgv_Report.RowHeadersWidth = 20
        Me.dgv_Report.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Report.Size = New System.Drawing.Size(418, 267)
        Me.dgv_Report.StandardTab = True
        Me.dgv_Report.TabIndex = 12
        Me.dgv_Report.TabStop = False
        Me.dgv_Report.Visible = False
        '
        'RptViewer
        '
        ReportDataSource4.Name = "DataSet1"
        ReportDataSource4.Value = Me.ReportTempBindingSource
        Me.RptViewer.LocalReport.DataSources.Add(ReportDataSource4)
        Me.RptViewer.LocalReport.ReportEmbeddedResource = "Textile.Report_Party_Address_List.rdlc"
        Me.RptViewer.Location = New System.Drawing.Point(34, 41)
        Me.RptViewer.Name = "RptViewer"
        Me.RptViewer.Size = New System.Drawing.Size(431, 345)
        Me.RptViewer.TabIndex = 11
        Me.RptViewer.WaitControlDisplayAfter = 10
        '
        'pnl_ReportInputs
        '
        Me.pnl_ReportInputs.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnl_ReportInputs.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.pnl_ReportInputs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_ReportInputs.Controls.Add(Me.btn_Email)
        Me.pnl_ReportInputs.Controls.Add(Me.btn_Whatsapp)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_Inputs11)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_Inputs11)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_Inputs10)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_Inputs10)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_Inputs9)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_Inputs9)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_Inputs8)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_Inputs8)
        Me.pnl_ReportInputs.Controls.Add(Me.msk_FromDate)
        Me.pnl_ReportInputs.Controls.Add(Me.msk_ToDate)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_Inputs7)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_Inputs7)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_Inputs6)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_Inputs6)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_Inputs5)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_Inputs5)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_Inputs4)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_Inputs4)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_Inputs1)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_Inputs1)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_ReportHeading)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_Inputs3)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_Inputs3)
        Me.pnl_ReportInputs.Controls.Add(Me.cbo_Inputs2)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_Inputs2)
        Me.pnl_ReportInputs.Controls.Add(Me.dtp_ToDate)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_ToDate)
        Me.pnl_ReportInputs.Controls.Add(Me.btn_Show)
        Me.pnl_ReportInputs.Controls.Add(Me.btn_Close)
        Me.pnl_ReportInputs.Controls.Add(Me.dtp_FromDate)
        Me.pnl_ReportInputs.Controls.Add(Me.lbl_FromDate)
        Me.pnl_ReportInputs.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnl_ReportInputs.Location = New System.Drawing.Point(0, 0)
        Me.pnl_ReportInputs.Name = "pnl_ReportInputs"
        Me.pnl_ReportInputs.Size = New System.Drawing.Size(1303, 228)
        Me.pnl_ReportInputs.TabIndex = 2
        '
        'btn_Email
        '
        Me.btn_Email.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btn_Email.BackgroundImage = Global.Textile.My.Resources.Resources.email_logo
        Me.btn_Email.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn_Email.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn_Email.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Email.ForeColor = System.Drawing.Color.White
        Me.btn_Email.Location = New System.Drawing.Point(55, -1)
        Me.btn_Email.Name = "btn_Email"
        Me.btn_Email.Size = New System.Drawing.Size(44, 25)
        Me.btn_Email.TabIndex = 15
        Me.btn_Email.TabStop = False
        Me.btn_Email.UseVisualStyleBackColor = False
        Me.btn_Email.Visible = False
        '
        'btn_Whatsapp
        '
        Me.btn_Whatsapp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btn_Whatsapp.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btn_Whatsapp.BackgroundImage = Global.Textile.My.Resources.Resources.Whatsapp_Logo1
        Me.btn_Whatsapp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn_Whatsapp.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn_Whatsapp.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Whatsapp.ForeColor = System.Drawing.Color.White
        Me.btn_Whatsapp.Location = New System.Drawing.Point(10, -1)
        Me.btn_Whatsapp.Name = "btn_Whatsapp"
        Me.btn_Whatsapp.Size = New System.Drawing.Size(32, 25)
        Me.btn_Whatsapp.TabIndex = 16
        Me.btn_Whatsapp.TabStop = False
        Me.btn_Whatsapp.UseVisualStyleBackColor = False
        Me.btn_Whatsapp.Visible = False
        '
        'cbo_Inputs11
        '
        Me.cbo_Inputs11.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Inputs11.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Inputs11.FormattingEnabled = True
        Me.cbo_Inputs11.Location = New System.Drawing.Point(521, 183)
        Me.cbo_Inputs11.Name = "cbo_Inputs11"
        Me.cbo_Inputs11.Size = New System.Drawing.Size(297, 23)
        Me.cbo_Inputs11.Sorted = True
        Me.cbo_Inputs11.TabIndex = 12
        '
        'lbl_Inputs11
        '
        Me.lbl_Inputs11.AutoSize = True
        Me.lbl_Inputs11.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Inputs11.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Inputs11.Location = New System.Drawing.Point(426, 187)
        Me.lbl_Inputs11.Name = "lbl_Inputs11"
        Me.lbl_Inputs11.Size = New System.Drawing.Size(74, 15)
        Me.lbl_Inputs11.TabIndex = 35
        Me.lbl_Inputs11.Text = "lbl_Inputs11"
        '
        'cbo_Inputs10
        '
        Me.cbo_Inputs10.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Inputs10.FormattingEnabled = True
        Me.cbo_Inputs10.Location = New System.Drawing.Point(107, 183)
        Me.cbo_Inputs10.MaxDropDownItems = 15
        Me.cbo_Inputs10.Name = "cbo_Inputs10"
        Me.cbo_Inputs10.Size = New System.Drawing.Size(297, 23)
        Me.cbo_Inputs10.Sorted = True
        Me.cbo_Inputs10.TabIndex = 11
        '
        'lbl_Inputs10
        '
        Me.lbl_Inputs10.AutoSize = True
        Me.lbl_Inputs10.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Inputs10.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Inputs10.Location = New System.Drawing.Point(12, 187)
        Me.lbl_Inputs10.Name = "lbl_Inputs10"
        Me.lbl_Inputs10.Size = New System.Drawing.Size(67, 15)
        Me.lbl_Inputs10.TabIndex = 34
        Me.lbl_Inputs10.Text = "lbl_Inputs8"
        '
        'cbo_Inputs9
        '
        Me.cbo_Inputs9.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Inputs9.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Inputs9.FormattingEnabled = True
        Me.cbo_Inputs9.Location = New System.Drawing.Point(521, 154)
        Me.cbo_Inputs9.Name = "cbo_Inputs9"
        Me.cbo_Inputs9.Size = New System.Drawing.Size(297, 23)
        Me.cbo_Inputs9.Sorted = True
        Me.cbo_Inputs9.TabIndex = 10
        '
        'lbl_Inputs9
        '
        Me.lbl_Inputs9.AutoSize = True
        Me.lbl_Inputs9.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Inputs9.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Inputs9.Location = New System.Drawing.Point(426, 158)
        Me.lbl_Inputs9.Name = "lbl_Inputs9"
        Me.lbl_Inputs9.Size = New System.Drawing.Size(67, 15)
        Me.lbl_Inputs9.TabIndex = 31
        Me.lbl_Inputs9.Text = "lbl_Inputs9"
        '
        'cbo_Inputs8
        '
        Me.cbo_Inputs8.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Inputs8.FormattingEnabled = True
        Me.cbo_Inputs8.Location = New System.Drawing.Point(107, 154)
        Me.cbo_Inputs8.MaxDropDownItems = 15
        Me.cbo_Inputs8.Name = "cbo_Inputs8"
        Me.cbo_Inputs8.Size = New System.Drawing.Size(297, 23)
        Me.cbo_Inputs8.Sorted = True
        Me.cbo_Inputs8.TabIndex = 9
        '
        'lbl_Inputs8
        '
        Me.lbl_Inputs8.AutoSize = True
        Me.lbl_Inputs8.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Inputs8.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Inputs8.Location = New System.Drawing.Point(12, 158)
        Me.lbl_Inputs8.Name = "lbl_Inputs8"
        Me.lbl_Inputs8.Size = New System.Drawing.Size(67, 15)
        Me.lbl_Inputs8.TabIndex = 30
        Me.lbl_Inputs8.Text = "lbl_Inputs8"
        '
        'msk_FromDate
        '
        Me.msk_FromDate.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.msk_FromDate.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.msk_FromDate.Location = New System.Drawing.Point(107, 32)
        Me.msk_FromDate.Mask = "00-00-0000"
        Me.msk_FromDate.Name = "msk_FromDate"
        Me.msk_FromDate.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.msk_FromDate.Size = New System.Drawing.Size(100, 22)
        Me.msk_FromDate.TabIndex = 0
        '
        'msk_ToDate
        '
        Me.msk_ToDate.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.msk_ToDate.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.msk_ToDate.Location = New System.Drawing.Point(284, 32)
        Me.msk_ToDate.Mask = "00-00-0000"
        Me.msk_ToDate.Name = "msk_ToDate"
        Me.msk_ToDate.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.msk_ToDate.Size = New System.Drawing.Size(100, 22)
        Me.msk_ToDate.TabIndex = 1
        '
        'cbo_Inputs7
        '
        Me.cbo_Inputs7.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Inputs7.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Inputs7.FormattingEnabled = True
        Me.cbo_Inputs7.Location = New System.Drawing.Point(521, 124)
        Me.cbo_Inputs7.Name = "cbo_Inputs7"
        Me.cbo_Inputs7.Size = New System.Drawing.Size(297, 23)
        Me.cbo_Inputs7.Sorted = True
        Me.cbo_Inputs7.TabIndex = 8
        '
        'lbl_Inputs7
        '
        Me.lbl_Inputs7.AutoSize = True
        Me.lbl_Inputs7.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Inputs7.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Inputs7.Location = New System.Drawing.Point(426, 128)
        Me.lbl_Inputs7.Name = "lbl_Inputs7"
        Me.lbl_Inputs7.Size = New System.Drawing.Size(67, 15)
        Me.lbl_Inputs7.TabIndex = 23
        Me.lbl_Inputs7.Text = "lbl_Inputs7"
        '
        'cbo_Inputs6
        '
        Me.cbo_Inputs6.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Inputs6.FormattingEnabled = True
        Me.cbo_Inputs6.Location = New System.Drawing.Point(107, 124)
        Me.cbo_Inputs6.MaxDropDownItems = 15
        Me.cbo_Inputs6.Name = "cbo_Inputs6"
        Me.cbo_Inputs6.Size = New System.Drawing.Size(297, 23)
        Me.cbo_Inputs6.Sorted = True
        Me.cbo_Inputs6.TabIndex = 7
        '
        'lbl_Inputs6
        '
        Me.lbl_Inputs6.AutoSize = True
        Me.lbl_Inputs6.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Inputs6.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Inputs6.Location = New System.Drawing.Point(12, 128)
        Me.lbl_Inputs6.Name = "lbl_Inputs6"
        Me.lbl_Inputs6.Size = New System.Drawing.Size(67, 15)
        Me.lbl_Inputs6.TabIndex = 22
        Me.lbl_Inputs6.Text = "lbl_Inputs6"
        '
        'cbo_Inputs5
        '
        Me.cbo_Inputs5.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Inputs5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Inputs5.FormattingEnabled = True
        Me.cbo_Inputs5.Location = New System.Drawing.Point(521, 93)
        Me.cbo_Inputs5.Name = "cbo_Inputs5"
        Me.cbo_Inputs5.Size = New System.Drawing.Size(297, 23)
        Me.cbo_Inputs5.Sorted = True
        Me.cbo_Inputs5.TabIndex = 6
        '
        'lbl_Inputs5
        '
        Me.lbl_Inputs5.AutoSize = True
        Me.lbl_Inputs5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Inputs5.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Inputs5.Location = New System.Drawing.Point(426, 97)
        Me.lbl_Inputs5.Name = "lbl_Inputs5"
        Me.lbl_Inputs5.Size = New System.Drawing.Size(67, 15)
        Me.lbl_Inputs5.TabIndex = 19
        Me.lbl_Inputs5.Text = "lbl_Inputs5"
        '
        'cbo_Inputs4
        '
        Me.cbo_Inputs4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Inputs4.FormattingEnabled = True
        Me.cbo_Inputs4.Location = New System.Drawing.Point(107, 93)
        Me.cbo_Inputs4.MaxDropDownItems = 15
        Me.cbo_Inputs4.Name = "cbo_Inputs4"
        Me.cbo_Inputs4.Size = New System.Drawing.Size(297, 23)
        Me.cbo_Inputs4.Sorted = True
        Me.cbo_Inputs4.TabIndex = 5
        '
        'lbl_Inputs4
        '
        Me.lbl_Inputs4.AutoSize = True
        Me.lbl_Inputs4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Inputs4.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Inputs4.Location = New System.Drawing.Point(12, 97)
        Me.lbl_Inputs4.Name = "lbl_Inputs4"
        Me.lbl_Inputs4.Size = New System.Drawing.Size(67, 15)
        Me.lbl_Inputs4.TabIndex = 17
        Me.lbl_Inputs4.Text = "lbl_Inputs4"
        '
        'cbo_Inputs1
        '
        Me.cbo_Inputs1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Inputs1.FormattingEnabled = True
        Me.cbo_Inputs1.Location = New System.Drawing.Point(521, 32)
        Me.cbo_Inputs1.MaxDropDownItems = 15
        Me.cbo_Inputs1.Name = "cbo_Inputs1"
        Me.cbo_Inputs1.Size = New System.Drawing.Size(297, 23)
        Me.cbo_Inputs1.Sorted = True
        Me.cbo_Inputs1.TabIndex = 2
        '
        'lbl_Inputs1
        '
        Me.lbl_Inputs1.AutoSize = True
        Me.lbl_Inputs1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Inputs1.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Inputs1.Location = New System.Drawing.Point(426, 36)
        Me.lbl_Inputs1.Name = "lbl_Inputs1"
        Me.lbl_Inputs1.Size = New System.Drawing.Size(67, 15)
        Me.lbl_Inputs1.TabIndex = 15
        Me.lbl_Inputs1.Text = "lbl_Inputs1"
        '
        'lbl_ReportHeading
        '
        Me.lbl_ReportHeading.AutoEllipsis = True
        Me.lbl_ReportHeading.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer))
        Me.lbl_ReportHeading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_ReportHeading.Dock = System.Windows.Forms.DockStyle.Top
        Me.lbl_ReportHeading.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_ReportHeading.ForeColor = System.Drawing.Color.White
        Me.lbl_ReportHeading.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lbl_ReportHeading.Location = New System.Drawing.Point(0, 0)
        Me.lbl_ReportHeading.Name = "lbl_ReportHeading"
        Me.lbl_ReportHeading.Size = New System.Drawing.Size(1301, 26)
        Me.lbl_ReportHeading.TabIndex = 13
        Me.lbl_ReportHeading.Text = "lbl_ReportHeading"
        Me.lbl_ReportHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cbo_Inputs3
        '
        Me.cbo_Inputs3.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Inputs3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Inputs3.FormattingEnabled = True
        Me.cbo_Inputs3.Location = New System.Drawing.Point(521, 62)
        Me.cbo_Inputs3.Name = "cbo_Inputs3"
        Me.cbo_Inputs3.Size = New System.Drawing.Size(297, 23)
        Me.cbo_Inputs3.Sorted = True
        Me.cbo_Inputs3.TabIndex = 4
        '
        'lbl_Inputs3
        '
        Me.lbl_Inputs3.AutoSize = True
        Me.lbl_Inputs3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Inputs3.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Inputs3.Location = New System.Drawing.Point(426, 66)
        Me.lbl_Inputs3.Name = "lbl_Inputs3"
        Me.lbl_Inputs3.Size = New System.Drawing.Size(67, 15)
        Me.lbl_Inputs3.TabIndex = 12
        Me.lbl_Inputs3.Text = "lbl_Inputs3"
        '
        'cbo_Inputs2
        '
        Me.cbo_Inputs2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Inputs2.FormattingEnabled = True
        Me.cbo_Inputs2.Location = New System.Drawing.Point(107, 62)
        Me.cbo_Inputs2.MaxDropDownItems = 15
        Me.cbo_Inputs2.Name = "cbo_Inputs2"
        Me.cbo_Inputs2.Size = New System.Drawing.Size(297, 23)
        Me.cbo_Inputs2.Sorted = True
        Me.cbo_Inputs2.TabIndex = 3
        '
        'lbl_Inputs2
        '
        Me.lbl_Inputs2.AutoSize = True
        Me.lbl_Inputs2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Inputs2.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Inputs2.Location = New System.Drawing.Point(12, 66)
        Me.lbl_Inputs2.Name = "lbl_Inputs2"
        Me.lbl_Inputs2.Size = New System.Drawing.Size(67, 15)
        Me.lbl_Inputs2.TabIndex = 10
        Me.lbl_Inputs2.Text = "lbl_Inputs2"
        '
        'dtp_ToDate
        '
        Me.dtp_ToDate.CalendarFont = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_ToDate.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_ToDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_ToDate.Location = New System.Drawing.Point(383, 32)
        Me.dtp_ToDate.Name = "dtp_ToDate"
        Me.dtp_ToDate.Size = New System.Drawing.Size(21, 22)
        Me.dtp_ToDate.TabIndex = 1
        Me.dtp_ToDate.TabStop = False
        '
        'lbl_ToDate
        '
        Me.lbl_ToDate.AutoSize = True
        Me.lbl_ToDate.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_ToDate.ForeColor = System.Drawing.Color.Blue
        Me.lbl_ToDate.Location = New System.Drawing.Point(244, 36)
        Me.lbl_ToDate.Name = "lbl_ToDate"
        Me.lbl_ToDate.Size = New System.Drawing.Size(26, 15)
        Me.lbl_ToDate.TabIndex = 9
        Me.lbl_ToDate.Text = "To :"
        '
        'btn_Show
        '
        Me.btn_Show.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.btn_Show.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn_Show.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Show.ForeColor = System.Drawing.Color.White
        Me.btn_Show.Location = New System.Drawing.Point(836, 32)
        Me.btn_Show.Name = "btn_Show"
        Me.btn_Show.Size = New System.Drawing.Size(52, 27)
        Me.btn_Show.TabIndex = 13
        Me.btn_Show.TabStop = False
        Me.btn_Show.Text = "&Show"
        Me.btn_Show.UseVisualStyleBackColor = False
        '
        'btn_Close
        '
        Me.btn_Close.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.btn_Close.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn_Close.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Close.ForeColor = System.Drawing.Color.White
        Me.btn_Close.Location = New System.Drawing.Point(931, 32)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(52, 27)
        Me.btn_Close.TabIndex = 14
        Me.btn_Close.TabStop = False
        Me.btn_Close.Text = "&Close"
        Me.btn_Close.UseVisualStyleBackColor = False
        '
        'dtp_FromDate
        '
        Me.dtp_FromDate.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_FromDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_FromDate.Location = New System.Drawing.Point(206, 32)
        Me.dtp_FromDate.Name = "dtp_FromDate"
        Me.dtp_FromDate.Size = New System.Drawing.Size(21, 22)
        Me.dtp_FromDate.TabIndex = 0
        Me.dtp_FromDate.TabStop = False
        '
        'lbl_FromDate
        '
        Me.lbl_FromDate.AutoSize = True
        Me.lbl_FromDate.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_FromDate.ForeColor = System.Drawing.Color.Blue
        Me.lbl_FromDate.Location = New System.Drawing.Point(12, 36)
        Me.lbl_FromDate.Name = "lbl_FromDate"
        Me.lbl_FromDate.Size = New System.Drawing.Size(72, 15)
        Me.lbl_FromDate.TabIndex = 5
        Me.lbl_FromDate.Text = "Date From :"
        '
        'ReportTempTableAdapter
        '
        Me.ReportTempTableAdapter.ClearBeforeFill = True
        '
        'PrintDocument2
        '
        '
        'PrintDialog1
        '
        Me.PrintDialog1.UseEXDialog = True
        '
        'pnl_MultiInput
        '
        Me.pnl_MultiInput.BackColor = System.Drawing.Color.White
        Me.pnl_MultiInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_MultiInput.Controls.Add(Me.lst_MultiInput_IdNos)
        Me.pnl_MultiInput.Controls.Add(Me.chklst_MultiInput)
        Me.pnl_MultiInput.Controls.Add(Me.btn_MultiInput_DeSelectAll)
        Me.pnl_MultiInput.Controls.Add(Me.btn_MultiInput_SelectAll)
        Me.pnl_MultiInput.Controls.Add(Me.btn_Close_MultiInput)
        Me.pnl_MultiInput.Controls.Add(Me.lbl_MultiInput_Heading)
        Me.pnl_MultiInput.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnl_MultiInput.Location = New System.Drawing.Point(1019, 208)
        Me.pnl_MultiInput.Name = "pnl_MultiInput"
        Me.pnl_MultiInput.Size = New System.Drawing.Size(523, 354)
        Me.pnl_MultiInput.TabIndex = 17
        '
        'lst_MultiInput_IdNos
        '
        Me.lst_MultiInput_IdNos.FormattingEnabled = True
        Me.lst_MultiInput_IdNos.ItemHeight = 15
        Me.lst_MultiInput_IdNos.Location = New System.Drawing.Point(24, 89)
        Me.lst_MultiInput_IdNos.Name = "lst_MultiInput_IdNos"
        Me.lst_MultiInput_IdNos.Size = New System.Drawing.Size(97, 79)
        Me.lst_MultiInput_IdNos.TabIndex = 15
        Me.lst_MultiInput_IdNos.Visible = False
        '
        'chklst_MultiInput
        '
        Me.chklst_MultiInput.BackColor = System.Drawing.Color.WhiteSmoke
        Me.chklst_MultiInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.chklst_MultiInput.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.chklst_MultiInput.ForeColor = System.Drawing.Color.Black
        Me.chklst_MultiInput.FormattingEnabled = True
        Me.chklst_MultiInput.Location = New System.Drawing.Point(0, 26)
        Me.chklst_MultiInput.Name = "chklst_MultiInput"
        Me.chklst_MultiInput.Size = New System.Drawing.Size(521, 326)
        Me.chklst_MultiInput.Sorted = True
        Me.chklst_MultiInput.TabIndex = 0
        '
        'btn_MultiInput_DeSelectAll
        '
        Me.btn_MultiInput_DeSelectAll.Location = New System.Drawing.Point(383, 2)
        Me.btn_MultiInput_DeSelectAll.Name = "btn_MultiInput_DeSelectAll"
        Me.btn_MultiInput_DeSelectAll.Size = New System.Drawing.Size(93, 22)
        Me.btn_MultiInput_DeSelectAll.TabIndex = 2
        Me.btn_MultiInput_DeSelectAll.TabStop = False
        Me.btn_MultiInput_DeSelectAll.Text = "&DESELECT ALL"
        Me.btn_MultiInput_DeSelectAll.UseVisualStyleBackColor = True
        '
        'btn_MultiInput_SelectAll
        '
        Me.btn_MultiInput_SelectAll.Location = New System.Drawing.Point(295, 1)
        Me.btn_MultiInput_SelectAll.Name = "btn_MultiInput_SelectAll"
        Me.btn_MultiInput_SelectAll.Size = New System.Drawing.Size(75, 22)
        Me.btn_MultiInput_SelectAll.TabIndex = 1
        Me.btn_MultiInput_SelectAll.TabStop = False
        Me.btn_MultiInput_SelectAll.Text = "SELECT &ALL"
        Me.btn_MultiInput_SelectAll.UseVisualStyleBackColor = True
        '
        'btn_Close_MultiInput
        '
        Me.btn_Close_MultiInput.BackColor = System.Drawing.Color.White
        Me.btn_Close_MultiInput.BackgroundImage = Global.Textile.My.Resources.Resources.Close1
        Me.btn_Close_MultiInput.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn_Close_MultiInput.FlatAppearance.BorderSize = 0
        Me.btn_Close_MultiInput.Location = New System.Drawing.Point(496, 1)
        Me.btn_Close_MultiInput.Name = "btn_Close_MultiInput"
        Me.btn_Close_MultiInput.Size = New System.Drawing.Size(25, 25)
        Me.btn_Close_MultiInput.TabIndex = 3
        Me.btn_Close_MultiInput.TabStop = False
        Me.btn_Close_MultiInput.UseVisualStyleBackColor = True
        '
        'lbl_MultiInput_Heading
        '
        Me.lbl_MultiInput_Heading.AutoEllipsis = True
        Me.lbl_MultiInput_Heading.BackColor = System.Drawing.Color.DarkSlateGray
        Me.lbl_MultiInput_Heading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_MultiInput_Heading.Dock = System.Windows.Forms.DockStyle.Top
        Me.lbl_MultiInput_Heading.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_MultiInput_Heading.ForeColor = System.Drawing.Color.White
        Me.lbl_MultiInput_Heading.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lbl_MultiInput_Heading.Location = New System.Drawing.Point(0, 0)
        Me.lbl_MultiInput_Heading.Name = "lbl_MultiInput_Heading"
        Me.lbl_MultiInput_Heading.Size = New System.Drawing.Size(521, 26)
        Me.lbl_MultiInput_Heading.TabIndex = 14
        Me.lbl_MultiInput_Heading.Text = "lbl_MultiInput_Heading"
        Me.lbl_MultiInput_Heading.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Report_Details
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1305, 537)
        Me.Controls.Add(Me.pnl_MultiInput)
        Me.Controls.Add(Me.pnl_Back)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.Name = "Report_Details"
        Me.Text = "Report_Details"
        CType(Me.ReportTempBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Report_DataSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_ReportDetails.ResumeLayout(False)
        CType(Me.dgv_Report, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnl_ReportInputs.ResumeLayout(False)
        Me.pnl_ReportInputs.PerformLayout()
        Me.pnl_MultiInput.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents pnl_ReportDetails As System.Windows.Forms.Panel
    Friend WithEvents RptViewer As Microsoft.Reporting.WinForms.ReportViewer
    Friend WithEvents pnl_ReportInputs As System.Windows.Forms.Panel
    Friend WithEvents lbl_FromDate As System.Windows.Forms.Label
    Friend WithEvents btn_Show As System.Windows.Forms.Button
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents lbl_ToDate As System.Windows.Forms.Label
    Friend WithEvents lbl_Inputs2 As System.Windows.Forms.Label
    Friend WithEvents lbl_Inputs3 As System.Windows.Forms.Label
    Friend WithEvents cbo_Inputs3 As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_Inputs1 As System.Windows.Forms.Label
    Friend WithEvents cbo_Inputs4 As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_Inputs4 As System.Windows.Forms.Label
    Friend WithEvents cbo_Inputs5 As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_Inputs5 As System.Windows.Forms.Label
    Friend WithEvents lbl_ReportHeading As System.Windows.Forms.Label
    Friend WithEvents ReportTempBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents Report_DataSet As Textile.Report_DataSet
    Friend WithEvents ReportTempTableAdapter As Textile.Report_DataSetTableAdapters.ReportTempTableAdapter
    Friend WithEvents dgv_Report As System.Windows.Forms.DataGridView
    Friend WithEvents cbo_Inputs7 As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_Inputs7 As System.Windows.Forms.Label
    Friend WithEvents cbo_Inputs6 As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_Inputs6 As System.Windows.Forms.Label
    Public WithEvents dtp_FromDate As System.Windows.Forms.DateTimePicker
    Public WithEvents dtp_ToDate As System.Windows.Forms.DateTimePicker
    Public WithEvents cbo_Inputs2 As System.Windows.Forms.ComboBox
    Public WithEvents cbo_Inputs1 As System.Windows.Forms.ComboBox
    Friend WithEvents PrintDocument1 As System.Drawing.Printing.PrintDocument
    Friend WithEvents PrintDocument2 As System.Drawing.Printing.PrintDocument
    Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
    Friend WithEvents pnl_MultiInput As System.Windows.Forms.Panel
    Friend WithEvents lst_MultiInput_IdNos As System.Windows.Forms.ListBox
    Friend WithEvents chklst_MultiInput As System.Windows.Forms.CheckedListBox
    Friend WithEvents btn_MultiInput_DeSelectAll As System.Windows.Forms.Button
    Friend WithEvents btn_MultiInput_SelectAll As System.Windows.Forms.Button
    Friend WithEvents btn_Close_MultiInput As System.Windows.Forms.Button
    Friend WithEvents lbl_MultiInput_Heading As System.Windows.Forms.Label
    Friend WithEvents msk_FromDate As System.Windows.Forms.MaskedTextBox
    Friend WithEvents msk_ToDate As System.Windows.Forms.MaskedTextBox
    Friend WithEvents cbo_Inputs9 As ComboBox
    Friend WithEvents lbl_Inputs9 As Label
    Friend WithEvents cbo_Inputs8 As ComboBox
    Friend WithEvents lbl_Inputs8 As Label
    Friend WithEvents cbo_Inputs11 As ComboBox
    Friend WithEvents lbl_Inputs11 As Label
    Friend WithEvents cbo_Inputs10 As ComboBox
    Friend WithEvents lbl_Inputs10 As Label
    Friend WithEvents btn_Email As Button
    Friend WithEvents btn_Whatsapp As Button
End Class
