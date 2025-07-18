<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Correction
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Correction))
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.btn_Save = New System.Windows.Forms.Button()
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.btn_Export = New System.Windows.Forms.Button()
        Me.btn_Import = New System.Windows.Forms.Button()
        Me.dtp_VerifiedTime = New System.Windows.Forms.DateTimePicker()
        Me.lbl_VerifiedTime = New System.Windows.Forms.Label()
        Me.dtp_VerifiedDate = New System.Windows.Forms.DateTimePicker()
        Me.lbl_VerifiedDate = New System.Windows.Forms.Label()
        Me.chk_VerifiedStatus = New System.Windows.Forms.CheckBox()
        Me.lbl_VerifiedStatus = New System.Windows.Forms.Label()
        Me.cbo_AttendedBy = New System.Windows.Forms.ComboBox()
        Me.Cbo_InformedBy = New System.Windows.Forms.ComboBox()
        Me.dtp_Time = New System.Windows.Forms.DateTimePicker()
        Me.dtp_CompletedTime = New System.Windows.Forms.DateTimePicker()
        Me.lbl_CompletedTime = New System.Windows.Forms.Label()
        Me.dtp_ComplededDate = New System.Windows.Forms.DateTimePicker()
        Me.lbl_Completeddate = New System.Windows.Forms.Label()
        Me.chk_CompletedStatus = New System.Windows.Forms.CheckBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cbo_EntryOrReport = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cbo_Type = New System.Windows.Forms.ComboBox()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.Label52 = New System.Windows.Forms.Label()
        Me.dtp_Date = New System.Windows.Forms.DateTimePicker()
        Me.lbl_RefNo = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Rtxt_CorrectionDetails = New System.Windows.Forms.RichTextBox()
        Me.grp_Filter = New System.Windows.Forms.GroupBox()
        Me.btn_Filter = New System.Windows.Forms.Button()
        Me.btn_CloseFilter = New System.Windows.Forms.Button()
        Me.dgv_Filter = New System.Windows.Forms.DataGridView()
        Me.grp_Open = New System.Windows.Forms.GroupBox()
        Me.btn_CloseOpen = New System.Windows.Forms.Button()
        Me.btn_Open = New System.Windows.Forms.Button()
        Me.cbo_Open = New System.Windows.Forms.ComboBox()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.pnl_Back.SuspendLayout()
        Me.grp_Filter.SuspendLayout()
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grp_Open.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label3.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(0, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(896, 33)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "CORRECTION ENTRY"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btn_Close
        '
        Me.btn_Close.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.btn_Close.ForeColor = System.Drawing.Color.White
        Me.btn_Close.Location = New System.Drawing.Point(783, 281)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(66, 28)
        Me.btn_Close.TabIndex = 14
        Me.btn_Close.TabStop = False
        Me.btn_Close.Text = "&CLOSE"
        Me.btn_Close.UseVisualStyleBackColor = False
        '
        'btn_Save
        '
        Me.btn_Save.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.btn_Save.ForeColor = System.Drawing.Color.White
        Me.btn_Save.Location = New System.Drawing.Point(707, 281)
        Me.btn_Save.Name = "btn_Save"
        Me.btn_Save.Size = New System.Drawing.Size(66, 28)
        Me.btn_Save.TabIndex = 13
        Me.btn_Save.TabStop = False
        Me.btn_Save.Text = "&SAVE"
        Me.btn_Save.UseVisualStyleBackColor = False
        '
        'pnl_Back
        '
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.btn_Export)
        Me.pnl_Back.Controls.Add(Me.btn_Import)
        Me.pnl_Back.Controls.Add(Me.dtp_VerifiedTime)
        Me.pnl_Back.Controls.Add(Me.lbl_VerifiedTime)
        Me.pnl_Back.Controls.Add(Me.dtp_VerifiedDate)
        Me.pnl_Back.Controls.Add(Me.lbl_VerifiedDate)
        Me.pnl_Back.Controls.Add(Me.chk_VerifiedStatus)
        Me.pnl_Back.Controls.Add(Me.lbl_VerifiedStatus)
        Me.pnl_Back.Controls.Add(Me.cbo_AttendedBy)
        Me.pnl_Back.Controls.Add(Me.Cbo_InformedBy)
        Me.pnl_Back.Controls.Add(Me.dtp_Time)
        Me.pnl_Back.Controls.Add(Me.dtp_CompletedTime)
        Me.pnl_Back.Controls.Add(Me.lbl_CompletedTime)
        Me.pnl_Back.Controls.Add(Me.dtp_ComplededDate)
        Me.pnl_Back.Controls.Add(Me.lbl_Completeddate)
        Me.pnl_Back.Controls.Add(Me.chk_CompletedStatus)
        Me.pnl_Back.Controls.Add(Me.Label8)
        Me.pnl_Back.Controls.Add(Me.Label7)
        Me.pnl_Back.Controls.Add(Me.Label6)
        Me.pnl_Back.Controls.Add(Me.Label5)
        Me.pnl_Back.Controls.Add(Me.cbo_EntryOrReport)
        Me.pnl_Back.Controls.Add(Me.Label1)
        Me.pnl_Back.Controls.Add(Me.cbo_Type)
        Me.pnl_Back.Controls.Add(Me.Label39)
        Me.pnl_Back.Controls.Add(Me.Label52)
        Me.pnl_Back.Controls.Add(Me.dtp_Date)
        Me.pnl_Back.Controls.Add(Me.lbl_RefNo)
        Me.pnl_Back.Controls.Add(Me.Label2)
        Me.pnl_Back.Controls.Add(Me.Label4)
        Me.pnl_Back.Controls.Add(Me.btn_Close)
        Me.pnl_Back.Controls.Add(Me.btn_Save)
        Me.pnl_Back.Controls.Add(Me.Rtxt_CorrectionDetails)
        Me.pnl_Back.Location = New System.Drawing.Point(12, 36)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(870, 323)
        Me.pnl_Back.TabIndex = 39
        '
        'btn_Export
        '
        Me.btn_Export.BackColor = System.Drawing.Color.Purple
        Me.btn_Export.ForeColor = System.Drawing.Color.White
        Me.btn_Export.Location = New System.Drawing.Point(7, 281)
        Me.btn_Export.Name = "btn_Export"
        Me.btn_Export.Size = New System.Drawing.Size(66, 28)
        Me.btn_Export.TabIndex = 357
        Me.btn_Export.TabStop = False
        Me.btn_Export.Text = "&EXPORT"
        Me.btn_Export.UseVisualStyleBackColor = False
        '
        'btn_Import
        '
        Me.btn_Import.BackColor = System.Drawing.Color.Purple
        Me.btn_Import.ForeColor = System.Drawing.Color.White
        Me.btn_Import.Location = New System.Drawing.Point(79, 281)
        Me.btn_Import.Name = "btn_Import"
        Me.btn_Import.Size = New System.Drawing.Size(66, 28)
        Me.btn_Import.TabIndex = 356
        Me.btn_Import.TabStop = False
        Me.btn_Import.Text = "&IMPORT"
        Me.btn_Import.UseVisualStyleBackColor = False
        Me.btn_Import.Visible = False
        '
        'dtp_VerifiedTime
        '
        Me.dtp_VerifiedTime.CustomFormat = "hh:mm tt"
        Me.dtp_VerifiedTime.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_VerifiedTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtp_VerifiedTime.Location = New System.Drawing.Point(510, 248)
        Me.dtp_VerifiedTime.Name = "dtp_VerifiedTime"
        Me.dtp_VerifiedTime.Size = New System.Drawing.Size(79, 23)
        Me.dtp_VerifiedTime.TabIndex = 12
        Me.dtp_VerifiedTime.Visible = False
        '
        'lbl_VerifiedTime
        '
        Me.lbl_VerifiedTime.AutoSize = True
        Me.lbl_VerifiedTime.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_VerifiedTime.ForeColor = System.Drawing.Color.Black
        Me.lbl_VerifiedTime.Location = New System.Drawing.Point(466, 252)
        Me.lbl_VerifiedTime.Name = "lbl_VerifiedTime"
        Me.lbl_VerifiedTime.Size = New System.Drawing.Size(33, 15)
        Me.lbl_VerifiedTime.TabIndex = 355
        Me.lbl_VerifiedTime.Text = "TIME"
        Me.lbl_VerifiedTime.Visible = False
        '
        'dtp_VerifiedDate
        '
        Me.dtp_VerifiedDate.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_VerifiedDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_VerifiedDate.Location = New System.Drawing.Point(348, 248)
        Me.dtp_VerifiedDate.Name = "dtp_VerifiedDate"
        Me.dtp_VerifiedDate.Size = New System.Drawing.Size(93, 23)
        Me.dtp_VerifiedDate.TabIndex = 11
        Me.dtp_VerifiedDate.Visible = False
        '
        'lbl_VerifiedDate
        '
        Me.lbl_VerifiedDate.AutoSize = True
        Me.lbl_VerifiedDate.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_VerifiedDate.ForeColor = System.Drawing.Color.Black
        Me.lbl_VerifiedDate.Location = New System.Drawing.Point(240, 251)
        Me.lbl_VerifiedDate.Name = "lbl_VerifiedDate"
        Me.lbl_VerifiedDate.Size = New System.Drawing.Size(84, 15)
        Me.lbl_VerifiedDate.TabIndex = 354
        Me.lbl_VerifiedDate.Text = "VERIFIED DATE"
        Me.lbl_VerifiedDate.Visible = False
        '
        'chk_VerifiedStatus
        '
        Me.chk_VerifiedStatus.AutoSize = True
        Me.chk_VerifiedStatus.Enabled = False
        Me.chk_VerifiedStatus.ForeColor = System.Drawing.Color.Blue
        Me.chk_VerifiedStatus.Location = New System.Drawing.Point(136, 250)
        Me.chk_VerifiedStatus.Name = "chk_VerifiedStatus"
        Me.chk_VerifiedStatus.Size = New System.Drawing.Size(80, 21)
        Me.chk_VerifiedStatus.TabIndex = 10
        Me.chk_VerifiedStatus.Text = "VERIFIED"
        Me.chk_VerifiedStatus.UseVisualStyleBackColor = True
        '
        'lbl_VerifiedStatus
        '
        Me.lbl_VerifiedStatus.AutoSize = True
        Me.lbl_VerifiedStatus.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_VerifiedStatus.ForeColor = System.Drawing.Color.Black
        Me.lbl_VerifiedStatus.Location = New System.Drawing.Point(4, 251)
        Me.lbl_VerifiedStatus.Name = "lbl_VerifiedStatus"
        Me.lbl_VerifiedStatus.Size = New System.Drawing.Size(95, 15)
        Me.lbl_VerifiedStatus.TabIndex = 353
        Me.lbl_VerifiedStatus.Text = "VERIFIED STATUS"
        '
        'cbo_AttendedBy
        '
        Me.cbo_AttendedBy.DropDownHeight = 100
        Me.cbo_AttendedBy.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_AttendedBy.FormattingEnabled = True
        Me.cbo_AttendedBy.IntegralHeight = False
        Me.cbo_AttendedBy.Location = New System.Drawing.Point(559, 185)
        Me.cbo_AttendedBy.MaxDropDownItems = 15
        Me.cbo_AttendedBy.MaxLength = 50
        Me.cbo_AttendedBy.Name = "cbo_AttendedBy"
        Me.cbo_AttendedBy.Size = New System.Drawing.Size(305, 23)
        Me.cbo_AttendedBy.Sorted = True
        Me.cbo_AttendedBy.TabIndex = 6
        '
        'Cbo_InformedBy
        '
        Me.Cbo_InformedBy.DropDownHeight = 100
        Me.Cbo_InformedBy.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Cbo_InformedBy.FormattingEnabled = True
        Me.Cbo_InformedBy.IntegralHeight = False
        Me.Cbo_InformedBy.Location = New System.Drawing.Point(136, 185)
        Me.Cbo_InformedBy.MaxDropDownItems = 15
        Me.Cbo_InformedBy.MaxLength = 50
        Me.Cbo_InformedBy.Name = "Cbo_InformedBy"
        Me.Cbo_InformedBy.Size = New System.Drawing.Size(305, 23)
        Me.Cbo_InformedBy.Sorted = True
        Me.Cbo_InformedBy.TabIndex = 5
        '
        'dtp_Time
        '
        Me.dtp_Time.CustomFormat = "hh:mm tt"
        Me.dtp_Time.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_Time.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtp_Time.Location = New System.Drawing.Point(427, 12)
        Me.dtp_Time.Name = "dtp_Time"
        Me.dtp_Time.Size = New System.Drawing.Size(93, 23)
        Me.dtp_Time.TabIndex = 1
        '
        'dtp_CompletedTime
        '
        Me.dtp_CompletedTime.CustomFormat = "hh:mm tt"
        Me.dtp_CompletedTime.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_CompletedTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtp_CompletedTime.Location = New System.Drawing.Point(510, 218)
        Me.dtp_CompletedTime.Name = "dtp_CompletedTime"
        Me.dtp_CompletedTime.Size = New System.Drawing.Size(79, 23)
        Me.dtp_CompletedTime.TabIndex = 9
        Me.dtp_CompletedTime.Visible = False
        '
        'lbl_CompletedTime
        '
        Me.lbl_CompletedTime.AutoSize = True
        Me.lbl_CompletedTime.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_CompletedTime.ForeColor = System.Drawing.Color.Black
        Me.lbl_CompletedTime.Location = New System.Drawing.Point(466, 222)
        Me.lbl_CompletedTime.Name = "lbl_CompletedTime"
        Me.lbl_CompletedTime.Size = New System.Drawing.Size(33, 15)
        Me.lbl_CompletedTime.TabIndex = 347
        Me.lbl_CompletedTime.Text = "TIME"
        Me.lbl_CompletedTime.Visible = False
        '
        'dtp_ComplededDate
        '
        Me.dtp_ComplededDate.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_ComplededDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_ComplededDate.Location = New System.Drawing.Point(348, 218)
        Me.dtp_ComplededDate.Name = "dtp_ComplededDate"
        Me.dtp_ComplededDate.Size = New System.Drawing.Size(93, 23)
        Me.dtp_ComplededDate.TabIndex = 8
        Me.dtp_ComplededDate.Visible = False
        '
        'lbl_Completeddate
        '
        Me.lbl_Completeddate.AutoSize = True
        Me.lbl_Completeddate.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Completeddate.ForeColor = System.Drawing.Color.Black
        Me.lbl_Completeddate.Location = New System.Drawing.Point(240, 221)
        Me.lbl_Completeddate.Name = "lbl_Completeddate"
        Me.lbl_Completeddate.Size = New System.Drawing.Size(103, 15)
        Me.lbl_Completeddate.TabIndex = 345
        Me.lbl_Completeddate.Text = "COMPLETED DATE"
        Me.lbl_Completeddate.Visible = False
        '
        'chk_CompletedStatus
        '
        Me.chk_CompletedStatus.AutoSize = True
        Me.chk_CompletedStatus.ForeColor = System.Drawing.Color.Blue
        Me.chk_CompletedStatus.Location = New System.Drawing.Point(136, 220)
        Me.chk_CompletedStatus.Name = "chk_CompletedStatus"
        Me.chk_CompletedStatus.Size = New System.Drawing.Size(98, 21)
        Me.chk_CompletedStatus.TabIndex = 7
        Me.chk_CompletedStatus.Text = "COMPLETED"
        Me.chk_CompletedStatus.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Black
        Me.Label8.Location = New System.Drawing.Point(4, 221)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(114, 15)
        Me.Label8.TabIndex = 343
        Me.Label8.Text = "COMPLETED STATUS"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Black
        Me.Label7.Location = New System.Drawing.Point(470, 188)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(80, 15)
        Me.Label7.TabIndex = 341
        Me.Label7.Text = "ATTENDED BY"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Black
        Me.Label6.Location = New System.Drawing.Point(4, 188)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(83, 15)
        Me.Label6.TabIndex = 339
        Me.Label6.Text = "INFORMED BY"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Black
        Me.Label5.Location = New System.Drawing.Point(4, 81)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(122, 15)
        Me.Label5.TabIndex = 325
        Me.Label5.Text = "CORRECTION DETAILS"
        '
        'cbo_EntryOrReport
        '
        Me.cbo_EntryOrReport.DropDownHeight = 150
        Me.cbo_EntryOrReport.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_EntryOrReport.FormattingEnabled = True
        Me.cbo_EntryOrReport.IntegralHeight = False
        Me.cbo_EntryOrReport.Location = New System.Drawing.Point(136, 45)
        Me.cbo_EntryOrReport.MaxDropDownItems = 15
        Me.cbo_EntryOrReport.MaxLength = 50
        Me.cbo_EntryOrReport.Name = "cbo_EntryOrReport"
        Me.cbo_EntryOrReport.Size = New System.Drawing.Size(728, 23)
        Me.cbo_EntryOrReport.Sorted = True
        Me.cbo_EntryOrReport.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(4, 48)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(126, 15)
        Me.Label1.TabIndex = 324
        Me.Label1.Text = "ENTRY/REPORT NAME"
        '
        'cbo_Type
        '
        Me.cbo_Type.DropDownHeight = 100
        Me.cbo_Type.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Type.FormattingEnabled = True
        Me.cbo_Type.IntegralHeight = False
        Me.cbo_Type.Location = New System.Drawing.Point(595, 12)
        Me.cbo_Type.MaxDropDownItems = 15
        Me.cbo_Type.MaxLength = 50
        Me.cbo_Type.Name = "cbo_Type"
        Me.cbo_Type.Size = New System.Drawing.Size(269, 23)
        Me.cbo_Type.Sorted = True
        Me.cbo_Type.TabIndex = 2
        '
        'Label39
        '
        Me.Label39.AutoSize = True
        Me.Label39.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label39.ForeColor = System.Drawing.Color.Black
        Me.Label39.Location = New System.Drawing.Point(556, 17)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(33, 15)
        Me.Label39.TabIndex = 322
        Me.Label39.Text = "TYPE"
        '
        'Label52
        '
        Me.Label52.AutoSize = True
        Me.Label52.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label52.ForeColor = System.Drawing.Color.Black
        Me.Label52.Location = New System.Drawing.Point(388, 17)
        Me.Label52.Name = "Label52"
        Me.Label52.Size = New System.Drawing.Size(33, 15)
        Me.Label52.TabIndex = 319
        Me.Label52.Text = "TIME"
        '
        'dtp_Date
        '
        Me.dtp_Date.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_Date.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_Date.Location = New System.Drawing.Point(249, 12)
        Me.dtp_Date.Name = "dtp_Date"
        Me.dtp_Date.Size = New System.Drawing.Size(115, 23)
        Me.dtp_Date.TabIndex = 0
        '
        'lbl_RefNo
        '
        Me.lbl_RefNo.BackColor = System.Drawing.Color.White
        Me.lbl_RefNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_RefNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_RefNo.Location = New System.Drawing.Point(136, 12)
        Me.lbl_RefNo.Name = "lbl_RefNo"
        Me.lbl_RefNo.Size = New System.Drawing.Size(69, 23)
        Me.lbl_RefNo.TabIndex = 318
        Me.lbl_RefNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(4, 17)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(47, 15)
        Me.Label2.TabIndex = 315
        Me.Label2.Text = "REF NO"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(211, 17)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(34, 15)
        Me.Label4.TabIndex = 316
        Me.Label4.Text = "DATE"
        '
        'Rtxt_CorrectionDetails
        '
        Me.Rtxt_CorrectionDetails.Location = New System.Drawing.Point(136, 74)
        Me.Rtxt_CorrectionDetails.Name = "Rtxt_CorrectionDetails"
        Me.Rtxt_CorrectionDetails.Size = New System.Drawing.Size(728, 96)
        Me.Rtxt_CorrectionDetails.TabIndex = 4
        Me.Rtxt_CorrectionDetails.Text = ""
        '
        'grp_Filter
        '
        Me.grp_Filter.Controls.Add(Me.btn_Filter)
        Me.grp_Filter.Controls.Add(Me.btn_CloseFilter)
        Me.grp_Filter.Controls.Add(Me.dgv_Filter)
        Me.grp_Filter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Filter.Location = New System.Drawing.Point(952, 82)
        Me.grp_Filter.Name = "grp_Filter"
        Me.grp_Filter.Size = New System.Drawing.Size(866, 387)
        Me.grp_Filter.TabIndex = 40
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
        Me.btn_Filter.Location = New System.Drawing.Point(690, 350)
        Me.btn_Filter.Name = "btn_Filter"
        Me.btn_Filter.Size = New System.Drawing.Size(72, 31)
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
        Me.btn_CloseFilter.Image = CType(resources.GetObject("btn_CloseFilter.Image"), System.Drawing.Image)
        Me.btn_CloseFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_CloseFilter.Location = New System.Drawing.Point(778, 350)
        Me.btn_CloseFilter.Name = "btn_CloseFilter"
        Me.btn_CloseFilter.Size = New System.Drawing.Size(72, 31)
        Me.btn_CloseFilter.TabIndex = 34
        Me.btn_CloseFilter.TabStop = False
        Me.btn_CloseFilter.Text = "&CLOSE"
        Me.btn_CloseFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_CloseFilter.UseVisualStyleBackColor = False
        '
        'dgv_Filter
        '
        Me.dgv_Filter.BackgroundColor = System.Drawing.Color.White
        Me.dgv_Filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_Filter.Location = New System.Drawing.Point(6, 30)
        Me.dgv_Filter.Name = "dgv_Filter"
        Me.dgv_Filter.ReadOnly = True
        Me.dgv_Filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Filter.Size = New System.Drawing.Size(854, 313)
        Me.dgv_Filter.TabIndex = 0
        '
        'grp_Open
        '
        Me.grp_Open.Controls.Add(Me.btn_CloseOpen)
        Me.grp_Open.Controls.Add(Me.btn_Open)
        Me.grp_Open.Controls.Add(Me.cbo_Open)
        Me.grp_Open.Location = New System.Drawing.Point(20, 439)
        Me.grp_Open.Name = "grp_Open"
        Me.grp_Open.Size = New System.Drawing.Size(481, 219)
        Me.grp_Open.TabIndex = 41
        Me.grp_Open.TabStop = False
        Me.grp_Open.Text = "FINIDING"
        '
        'btn_CloseOpen
        '
        Me.btn_CloseOpen.BackColor = System.Drawing.Color.MidnightBlue
        Me.btn_CloseOpen.ForeColor = System.Drawing.Color.White
        Me.btn_CloseOpen.Image = CType(resources.GetObject("btn_CloseOpen.Image"), System.Drawing.Image)
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
        Me.btn_Open.Image = CType(resources.GetObject("btn_Open.Image"), System.Drawing.Image)
        Me.btn_Open.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Open.Location = New System.Drawing.Point(269, 143)
        Me.btn_Open.Name = "btn_Open"
        Me.btn_Open.Size = New System.Drawing.Size(82, 35)
        Me.btn_Open.TabIndex = 3
        Me.btn_Open.Text = "&OPEN     "
        Me.btn_Open.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Open.UseVisualStyleBackColor = False
        '
        'cbo_Open
        '
        Me.cbo_Open.DropDownHeight = 100
        Me.cbo_Open.FormattingEnabled = True
        Me.cbo_Open.IntegralHeight = False
        Me.cbo_Open.Location = New System.Drawing.Point(23, 44)
        Me.cbo_Open.Name = "cbo_Open"
        Me.cbo_Open.Size = New System.Drawing.Size(443, 23)
        Me.cbo_Open.TabIndex = 0
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'Correction
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.CausesValidation = False
        Me.ClientSize = New System.Drawing.Size(896, 369)
        Me.Controls.Add(Me.grp_Filter)
        Me.Controls.Add(Me.grp_Open)
        Me.Controls.Add(Me.pnl_Back)
        Me.Controls.Add(Me.Label3)
        Me.Font = New System.Drawing.Font("Calibri", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "Correction"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "CORRECTION"
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_Back.PerformLayout()
        Me.grp_Filter.ResumeLayout(False)
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grp_Open.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents btn_Save As System.Windows.Forms.Button
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents Label52 As System.Windows.Forms.Label
    Friend WithEvents dtp_Date As System.Windows.Forms.DateTimePicker
    Friend WithEvents lbl_RefNo As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cbo_Type As System.Windows.Forms.ComboBox
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents cbo_EntryOrReport As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents chk_CompletedStatus As System.Windows.Forms.CheckBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lbl_CompletedTime As System.Windows.Forms.Label
    Friend WithEvents dtp_ComplededDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents lbl_Completeddate As System.Windows.Forms.Label
    Friend WithEvents dtp_CompletedTime As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtp_Time As System.Windows.Forms.DateTimePicker
    Friend WithEvents grp_Filter As System.Windows.Forms.GroupBox
    Friend WithEvents btn_Filter As System.Windows.Forms.Button
    Friend WithEvents btn_CloseFilter As System.Windows.Forms.Button
    Friend WithEvents dgv_Filter As System.Windows.Forms.DataGridView
    Friend WithEvents grp_Open As System.Windows.Forms.GroupBox
    Friend WithEvents btn_CloseOpen As System.Windows.Forms.Button
    Friend WithEvents btn_Open As System.Windows.Forms.Button
    Friend WithEvents cbo_Open As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_AttendedBy As System.Windows.Forms.ComboBox
    Friend WithEvents Cbo_InformedBy As System.Windows.Forms.ComboBox
    Friend WithEvents dtp_VerifiedTime As System.Windows.Forms.DateTimePicker
    Friend WithEvents lbl_VerifiedTime As System.Windows.Forms.Label
    Friend WithEvents dtp_VerifiedDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents lbl_VerifiedDate As System.Windows.Forms.Label
    Friend WithEvents chk_VerifiedStatus As System.Windows.Forms.CheckBox
    Friend WithEvents lbl_VerifiedStatus As System.Windows.Forms.Label
    Friend WithEvents btn_Export As System.Windows.Forms.Button
    Friend WithEvents btn_Import As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Rtxt_CorrectionDetails As System.Windows.Forms.RichTextBox
End Class
