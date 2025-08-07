<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Delivery_Party_Creation
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Delivery_Party_Creation))
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.cbo_Open = New System.Windows.Forms.ComboBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.cbo_State = New System.Windows.Forms.ComboBox()
        Me.txt_GSTIN_No = New System.Windows.Forms.TextBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.chk_Close_Status = New System.Windows.Forms.CheckBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txt_Mail = New System.Windows.Forms.TextBox()
        Me.chk_Show_In_AllEntry = New System.Windows.Forms.CheckBox()
        Me.txt_MobileSms = New System.Windows.Forms.TextBox()
        Me.grp_Open = New System.Windows.Forms.GroupBox()
        Me.btn_Find = New System.Windows.Forms.Button()
        Me.btn_CloseOpen = New System.Windows.Forms.Button()
        Me.Ledger_Name = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Ledger_IdNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btn_CloseFilter = New System.Windows.Forms.Button()
        Me.dgv_Filter = New System.Windows.Forms.DataGridView()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lbl_IdNo = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.cbo_Area = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.grp_Filter = New System.Windows.Forms.GroupBox()
        Me.btn_Filter = New System.Windows.Forms.Button()
        Me.txt_AlaisName = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.txt_CstNo = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txt_TinNo = New System.Windows.Forms.TextBox()
        Me.lbl_Heading = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txt_PhoneNo = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txt_Address4 = New System.Windows.Forms.TextBox()
        Me.txt_Address3 = New System.Windows.Forms.TextBox()
        Me.txt_Address2 = New System.Windows.Forms.TextBox()
        Me.txt_Address1 = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txt_Name = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.grp_Back = New System.Windows.Forms.GroupBox()
        Me.grp_Open.SuspendLayout()
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grp_Filter.SuspendLayout()
        Me.grp_Back.SuspendLayout()
        Me.SuspendLayout()
        '
        'cbo_Open
        '
        Me.cbo_Open.DropDownHeight = 125
        Me.cbo_Open.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Open.FormattingEnabled = True
        Me.cbo_Open.IntegralHeight = False
        Me.cbo_Open.Location = New System.Drawing.Point(19, 32)
        Me.cbo_Open.Name = "cbo_Open"
        Me.cbo_Open.Size = New System.Drawing.Size(493, 23)
        Me.cbo_Open.Sorted = True
        Me.cbo_Open.TabIndex = 0
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.ForeColor = System.Drawing.Color.Navy
        Me.Label15.Location = New System.Drawing.Point(18, 275)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(36, 15)
        Me.Label15.TabIndex = 298
        Me.Label15.Text = "State"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cbo_State
        '
        Me.cbo_State.DropDownHeight = 99
        Me.cbo_State.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_State.FormattingEnabled = True
        Me.cbo_State.IntegralHeight = False
        Me.cbo_State.Location = New System.Drawing.Point(141, 271)
        Me.cbo_State.MaxDropDownItems = 4
        Me.cbo_State.MaxLength = 50
        Me.cbo_State.Name = "cbo_State"
        Me.cbo_State.Size = New System.Drawing.Size(512, 23)
        Me.cbo_State.TabIndex = 7
        Me.cbo_State.Text = "cbo_State"
        '
        'txt_GSTIN_No
        '
        Me.txt_GSTIN_No.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_GSTIN_No.Location = New System.Drawing.Point(141, 335)
        Me.txt_GSTIN_No.MaxLength = 50
        Me.txt_GSTIN_No.Name = "txt_GSTIN_No"
        Me.txt_GSTIN_No.Size = New System.Drawing.Size(512, 23)
        Me.txt_GSTIN_No.TabIndex = 10
        Me.txt_GSTIN_No.Text = "txt_GSTIN_No"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.ForeColor = System.Drawing.Color.Navy
        Me.Label22.Location = New System.Drawing.Point(16, 339)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(61, 15)
        Me.Label22.TabIndex = 297
        Me.Label22.Text = "GSTIN No "
        '
        'chk_Close_Status
        '
        Me.chk_Close_Status.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chk_Close_Status.ForeColor = System.Drawing.Color.Black
        Me.chk_Close_Status.Location = New System.Drawing.Point(279, 436)
        Me.chk_Close_Status.Name = "chk_Close_Status"
        Me.chk_Close_Status.Size = New System.Drawing.Size(132, 24)
        Me.chk_Close_Status.TabIndex = 29
        Me.chk_Close_Status.TabStop = False
        Me.chk_Close_Status.Text = "Close Status"
        Me.chk_Close_Status.UseVisualStyleBackColor = True
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.ForeColor = System.Drawing.Color.Navy
        Me.Label12.Location = New System.Drawing.Point(18, 403)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(54, 15)
        Me.Label12.TabIndex = 28
        Me.Label12.Text = "E-Mail ID"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_Mail
        '
        Me.txt_Mail.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Mail.Location = New System.Drawing.Point(141, 399)
        Me.txt_Mail.MaxLength = 35
        Me.txt_Mail.Name = "txt_Mail"
        Me.txt_Mail.Size = New System.Drawing.Size(514, 23)
        Me.txt_Mail.TabIndex = 13
        Me.txt_Mail.Text = "txt_Mail"
        '
        'chk_Show_In_AllEntry
        '
        Me.chk_Show_In_AllEntry.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chk_Show_In_AllEntry.ForeColor = System.Drawing.Color.Black
        Me.chk_Show_In_AllEntry.Location = New System.Drawing.Point(141, 436)
        Me.chk_Show_In_AllEntry.Name = "chk_Show_In_AllEntry"
        Me.chk_Show_In_AllEntry.Size = New System.Drawing.Size(132, 24)
        Me.chk_Show_In_AllEntry.TabIndex = 26
        Me.chk_Show_In_AllEntry.TabStop = False
        Me.chk_Show_In_AllEntry.Text = "Show In All Entry"
        Me.chk_Show_In_AllEntry.UseVisualStyleBackColor = True
        '
        'txt_MobileSms
        '
        Me.txt_MobileSms.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_MobileSms.Location = New System.Drawing.Point(438, 303)
        Me.txt_MobileSms.MaxLength = 35
        Me.txt_MobileSms.Name = "txt_MobileSms"
        Me.txt_MobileSms.Size = New System.Drawing.Size(215, 23)
        Me.txt_MobileSms.TabIndex = 9
        Me.txt_MobileSms.Text = "txt_MobileSms"
        '
        'grp_Open
        '
        Me.grp_Open.Controls.Add(Me.btn_Find)
        Me.grp_Open.Controls.Add(Me.cbo_Open)
        Me.grp_Open.Controls.Add(Me.btn_CloseOpen)
        Me.grp_Open.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Open.Location = New System.Drawing.Point(736, 507)
        Me.grp_Open.Name = "grp_Open"
        Me.grp_Open.Size = New System.Drawing.Size(534, 247)
        Me.grp_Open.TabIndex = 35
        Me.grp_Open.TabStop = False
        Me.grp_Open.Text = "Finding"
        Me.grp_Open.Visible = False
        '
        'btn_Find
        '
        Me.btn_Find.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Find.Image = CType(resources.GetObject("btn_Find.Image"), System.Drawing.Image)
        Me.btn_Find.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Find.Location = New System.Drawing.Point(332, 197)
        Me.btn_Find.Name = "btn_Find"
        Me.btn_Find.Size = New System.Drawing.Size(83, 29)
        Me.btn_Find.TabIndex = 31
        Me.btn_Find.Text = "&Find"
        Me.btn_Find.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Find.UseVisualStyleBackColor = True
        '
        'btn_CloseOpen
        '
        Me.btn_CloseOpen.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_CloseOpen.Image = CType(resources.GetObject("btn_CloseOpen.Image"), System.Drawing.Image)
        Me.btn_CloseOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_CloseOpen.Location = New System.Drawing.Point(428, 197)
        Me.btn_CloseOpen.Name = "btn_CloseOpen"
        Me.btn_CloseOpen.Size = New System.Drawing.Size(83, 29)
        Me.btn_CloseOpen.TabIndex = 30
        Me.btn_CloseOpen.Text = "&Close"
        Me.btn_CloseOpen.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_CloseOpen.UseVisualStyleBackColor = True
        '
        'Ledger_Name
        '
        Me.Ledger_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader
        Me.Ledger_Name.FillWeight = 160.0!
        Me.Ledger_Name.HeaderText = "LEDGER NAME"
        Me.Ledger_Name.Name = "Ledger_Name"
        Me.Ledger_Name.ReadOnly = True
        Me.Ledger_Name.Width = 110
        '
        'Ledger_IdNo
        '
        Me.Ledger_IdNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader
        Me.Ledger_IdNo.FillWeight = 40.0!
        Me.Ledger_IdNo.HeaderText = "LEDGER IDNO"
        Me.Ledger_IdNo.Name = "Ledger_IdNo"
        Me.Ledger_IdNo.ReadOnly = True
        Me.Ledger_IdNo.Width = 105
        '
        'btn_CloseFilter
        '
        Me.btn_CloseFilter.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_CloseFilter.Image = CType(resources.GetObject("btn_CloseFilter.Image"), System.Drawing.Image)
        Me.btn_CloseFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_CloseFilter.Location = New System.Drawing.Point(482, 308)
        Me.btn_CloseFilter.Name = "btn_CloseFilter"
        Me.btn_CloseFilter.Size = New System.Drawing.Size(83, 29)
        Me.btn_CloseFilter.TabIndex = 32
        Me.btn_CloseFilter.Text = "&Close"
        Me.btn_CloseFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_CloseFilter.UseVisualStyleBackColor = True
        '
        'dgv_Filter
        '
        Me.dgv_Filter.AllowUserToAddRows = False
        Me.dgv_Filter.AllowUserToDeleteRows = False
        Me.dgv_Filter.AllowUserToResizeColumns = False
        Me.dgv_Filter.AllowUserToResizeRows = False
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_Filter.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle3
        Me.dgv_Filter.BackgroundColor = System.Drawing.Color.White
        Me.dgv_Filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_Filter.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Ledger_IdNo, Me.Ledger_Name})
        Me.dgv_Filter.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv_Filter.Location = New System.Drawing.Point(14, 30)
        Me.dgv_Filter.Name = "dgv_Filter"
        Me.dgv_Filter.ReadOnly = True
        Me.dgv_Filter.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_Filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Filter.Size = New System.Drawing.Size(551, 271)
        Me.dgv_Filter.TabIndex = 0
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ForeColor = System.Drawing.Color.Navy
        Me.Label8.Location = New System.Drawing.Point(361, 307)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(64, 15)
        Me.Label8.TabIndex = 17
        Me.Label8.Text = "Mobile No"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbl_IdNo
        '
        Me.lbl_IdNo.BackColor = System.Drawing.Color.White
        Me.lbl_IdNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_IdNo.Location = New System.Drawing.Point(141, 15)
        Me.lbl_IdNo.Name = "lbl_IdNo"
        Me.lbl_IdNo.Size = New System.Drawing.Size(514, 23)
        Me.lbl_IdNo.TabIndex = 16
        Me.lbl_IdNo.Text = "lbl_IdNo"
        Me.lbl_IdNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnSave
        '
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ForeColor = System.Drawing.Color.Navy
        Me.btnSave.Image = Global.Textile.My.Resources.Resources.SAVE1
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(488, 431)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(76, 31)
        Me.btnSave.TabIndex = 14
        Me.btnSave.TabStop = False
        Me.btnSave.Text = "Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'cbo_Area
        '
        Me.cbo_Area.DropDownHeight = 75
        Me.cbo_Area.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Area.FormattingEnabled = True
        Me.cbo_Area.IntegralHeight = False
        Me.cbo_Area.Location = New System.Drawing.Point(141, 111)
        Me.cbo_Area.Name = "cbo_Area"
        Me.cbo_Area.Size = New System.Drawing.Size(514, 23)
        Me.cbo_Area.TabIndex = 2
        Me.cbo_Area.Text = "cbo_Area"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.ForeColor = System.Drawing.Color.Navy
        Me.Label7.Location = New System.Drawing.Point(18, 115)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(69, 15)
        Me.Label7.TabIndex = 14
        Me.Label7.Text = "Area Name"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grp_Filter
        '
        Me.grp_Filter.Controls.Add(Me.btn_Filter)
        Me.grp_Filter.Controls.Add(Me.btn_CloseFilter)
        Me.grp_Filter.Controls.Add(Me.dgv_Filter)
        Me.grp_Filter.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Filter.Location = New System.Drawing.Point(736, 121)
        Me.grp_Filter.Name = "grp_Filter"
        Me.grp_Filter.Size = New System.Drawing.Size(597, 345)
        Me.grp_Filter.TabIndex = 36
        Me.grp_Filter.TabStop = False
        Me.grp_Filter.Text = "Filter"
        Me.grp_Filter.Visible = False
        '
        'btn_Filter
        '
        Me.btn_Filter.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Filter.Image = CType(resources.GetObject("btn_Filter.Image"), System.Drawing.Image)
        Me.btn_Filter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Filter.Location = New System.Drawing.Point(386, 308)
        Me.btn_Filter.Name = "btn_Filter"
        Me.btn_Filter.Size = New System.Drawing.Size(83, 29)
        Me.btn_Filter.TabIndex = 33
        Me.btn_Filter.Text = "&Open"
        Me.btn_Filter.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Filter.UseVisualStyleBackColor = True
        '
        'txt_AlaisName
        '
        Me.txt_AlaisName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_AlaisName.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_AlaisName.Location = New System.Drawing.Point(141, 79)
        Me.txt_AlaisName.MaxLength = 40
        Me.txt_AlaisName.Name = "txt_AlaisName"
        Me.txt_AlaisName.Size = New System.Drawing.Size(514, 23)
        Me.txt_AlaisName.TabIndex = 1
        Me.txt_AlaisName.Text = "TXT_ALAISNAME"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.ForeColor = System.Drawing.Color.Navy
        Me.Label6.Location = New System.Drawing.Point(18, 83)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(116, 15)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Delivery Alais Name"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnClose
        '
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ForeColor = System.Drawing.Color.Navy
        Me.btnClose.Image = Global.Textile.My.Resources.Resources.cancel1
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(579, 431)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(76, 31)
        Me.btnClose.TabIndex = 15
        Me.btnClose.TabStop = False
        Me.btnClose.Text = "Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'txt_CstNo
        '
        Me.txt_CstNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_CstNo.Location = New System.Drawing.Point(438, 367)
        Me.txt_CstNo.MaxLength = 35
        Me.txt_CstNo.Name = "txt_CstNo"
        Me.txt_CstNo.Size = New System.Drawing.Size(216, 23)
        Me.txt_CstNo.TabIndex = 12
        Me.txt_CstNo.Text = "txt_CstNo"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.ForeColor = System.Drawing.Color.Navy
        Me.Label11.Location = New System.Drawing.Point(361, 371)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(43, 15)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "Cst No"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_TinNo
        '
        Me.txt_TinNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_TinNo.Location = New System.Drawing.Point(141, 367)
        Me.txt_TinNo.MaxLength = 35
        Me.txt_TinNo.Name = "txt_TinNo"
        Me.txt_TinNo.Size = New System.Drawing.Size(208, 23)
        Me.txt_TinNo.TabIndex = 11
        Me.txt_TinNo.Text = "txt_TinNo"
        '
        'lbl_Heading
        '
        Me.lbl_Heading.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.lbl_Heading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_Heading.Dock = System.Windows.Forms.DockStyle.Top
        Me.lbl_Heading.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Heading.ForeColor = System.Drawing.Color.White
        Me.lbl_Heading.Location = New System.Drawing.Point(0, 0)
        Me.lbl_Heading.Name = "lbl_Heading"
        Me.lbl_Heading.Size = New System.Drawing.Size(708, 30)
        Me.lbl_Heading.TabIndex = 37
        Me.lbl_Heading.Text = "DELIVERY PARTY  CREATION"
        Me.lbl_Heading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.ForeColor = System.Drawing.Color.Navy
        Me.Label10.Location = New System.Drawing.Point(18, 371)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(42, 15)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "Tin No"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_PhoneNo
        '
        Me.txt_PhoneNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_PhoneNo.Location = New System.Drawing.Point(141, 303)
        Me.txt_PhoneNo.MaxLength = 35
        Me.txt_PhoneNo.Name = "txt_PhoneNo"
        Me.txt_PhoneNo.Size = New System.Drawing.Size(209, 23)
        Me.txt_PhoneNo.TabIndex = 8
        Me.txt_PhoneNo.Text = "txt_PhoneNo"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.ForeColor = System.Drawing.Color.Navy
        Me.Label9.Location = New System.Drawing.Point(18, 307)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(61, 15)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Phone No"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_Address4
        '
        Me.txt_Address4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Address4.Location = New System.Drawing.Point(141, 239)
        Me.txt_Address4.MaxLength = 35
        Me.txt_Address4.Name = "txt_Address4"
        Me.txt_Address4.Size = New System.Drawing.Size(514, 23)
        Me.txt_Address4.TabIndex = 6
        Me.txt_Address4.Text = "txt_Address4"
        '
        'txt_Address3
        '
        Me.txt_Address3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Address3.Location = New System.Drawing.Point(141, 207)
        Me.txt_Address3.MaxLength = 35
        Me.txt_Address3.Name = "txt_Address3"
        Me.txt_Address3.Size = New System.Drawing.Size(514, 23)
        Me.txt_Address3.TabIndex = 5
        Me.txt_Address3.Text = "txt_Address3"
        '
        'txt_Address2
        '
        Me.txt_Address2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Address2.Location = New System.Drawing.Point(141, 175)
        Me.txt_Address2.MaxLength = 35
        Me.txt_Address2.Name = "txt_Address2"
        Me.txt_Address2.Size = New System.Drawing.Size(514, 23)
        Me.txt_Address2.TabIndex = 4
        Me.txt_Address2.Text = "txt_Address2"
        '
        'txt_Address1
        '
        Me.txt_Address1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Address1.Location = New System.Drawing.Point(141, 143)
        Me.txt_Address1.MaxLength = 35
        Me.txt_Address1.Name = "txt_Address1"
        Me.txt_Address1.Size = New System.Drawing.Size(514, 23)
        Me.txt_Address1.TabIndex = 3
        Me.txt_Address1.Text = "txt_Address1"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.Navy
        Me.Label5.Location = New System.Drawing.Point(18, 147)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(51, 15)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Address"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txt_Name
        '
        Me.txt_Name.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_Name.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Name.Location = New System.Drawing.Point(141, 47)
        Me.txt_Name.MaxLength = 40
        Me.txt_Name.Name = "txt_Name"
        Me.txt_Name.Size = New System.Drawing.Size(514, 23)
        Me.txt_Name.TabIndex = 0
        Me.txt_Name.Text = "TXT_NAME"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.Navy
        Me.Label2.Location = New System.Drawing.Point(18, 51)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(88, 15)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Delivery Name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Navy
        Me.Label1.Location = New System.Drawing.Point(18, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(33, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "IdNo"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grp_Back
        '
        Me.grp_Back.BackColor = System.Drawing.Color.Transparent
        Me.grp_Back.Controls.Add(Me.Label15)
        Me.grp_Back.Controls.Add(Me.cbo_State)
        Me.grp_Back.Controls.Add(Me.txt_GSTIN_No)
        Me.grp_Back.Controls.Add(Me.Label22)
        Me.grp_Back.Controls.Add(Me.chk_Close_Status)
        Me.grp_Back.Controls.Add(Me.Label12)
        Me.grp_Back.Controls.Add(Me.txt_Mail)
        Me.grp_Back.Controls.Add(Me.chk_Show_In_AllEntry)
        Me.grp_Back.Controls.Add(Me.txt_MobileSms)
        Me.grp_Back.Controls.Add(Me.Label8)
        Me.grp_Back.Controls.Add(Me.lbl_IdNo)
        Me.grp_Back.Controls.Add(Me.btnSave)
        Me.grp_Back.Controls.Add(Me.cbo_Area)
        Me.grp_Back.Controls.Add(Me.Label7)
        Me.grp_Back.Controls.Add(Me.txt_AlaisName)
        Me.grp_Back.Controls.Add(Me.Label6)
        Me.grp_Back.Controls.Add(Me.btnClose)
        Me.grp_Back.Controls.Add(Me.txt_CstNo)
        Me.grp_Back.Controls.Add(Me.Label11)
        Me.grp_Back.Controls.Add(Me.txt_TinNo)
        Me.grp_Back.Controls.Add(Me.Label10)
        Me.grp_Back.Controls.Add(Me.txt_PhoneNo)
        Me.grp_Back.Controls.Add(Me.Label9)
        Me.grp_Back.Controls.Add(Me.txt_Address4)
        Me.grp_Back.Controls.Add(Me.txt_Address3)
        Me.grp_Back.Controls.Add(Me.txt_Address2)
        Me.grp_Back.Controls.Add(Me.txt_Address1)
        Me.grp_Back.Controls.Add(Me.Label5)
        Me.grp_Back.Controls.Add(Me.txt_Name)
        Me.grp_Back.Controls.Add(Me.Label2)
        Me.grp_Back.Controls.Add(Me.Label1)
        Me.grp_Back.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Back.Location = New System.Drawing.Point(7, 26)
        Me.grp_Back.Name = "grp_Back"
        Me.grp_Back.Size = New System.Drawing.Size(677, 474)
        Me.grp_Back.TabIndex = 34
        Me.grp_Back.TabStop = False
        '
        'Delivery_Party_Creation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.CausesValidation = False
        Me.ClientSize = New System.Drawing.Size(708, 517)
        Me.Controls.Add(Me.grp_Open)
        Me.Controls.Add(Me.grp_Filter)
        Me.Controls.Add(Me.lbl_Heading)
        Me.Controls.Add(Me.grp_Back)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Delivery_Party_Creation"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "DELIVERY CREATION"
        Me.grp_Open.ResumeLayout(False)
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grp_Filter.ResumeLayout(False)
        Me.grp_Back.ResumeLayout(False)
        Me.grp_Back.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cbo_Open As System.Windows.Forms.ComboBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents cbo_State As System.Windows.Forms.ComboBox
    Friend WithEvents txt_GSTIN_No As System.Windows.Forms.TextBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents chk_Close_Status As System.Windows.Forms.CheckBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txt_Mail As System.Windows.Forms.TextBox
    Friend WithEvents chk_Show_In_AllEntry As System.Windows.Forms.CheckBox
    Friend WithEvents txt_MobileSms As System.Windows.Forms.TextBox
    Friend WithEvents grp_Open As System.Windows.Forms.GroupBox
    Friend WithEvents btn_Find As System.Windows.Forms.Button
    Friend WithEvents btn_CloseOpen As System.Windows.Forms.Button
    Friend WithEvents Ledger_Name As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Ledger_IdNo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents btn_CloseFilter As System.Windows.Forms.Button
    Friend WithEvents dgv_Filter As System.Windows.Forms.DataGridView
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lbl_IdNo As System.Windows.Forms.Label
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents cbo_Area As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents grp_Filter As System.Windows.Forms.GroupBox
    Friend WithEvents btn_Filter As System.Windows.Forms.Button
    Friend WithEvents txt_AlaisName As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents txt_CstNo As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txt_TinNo As System.Windows.Forms.TextBox
    Friend WithEvents lbl_Heading As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txt_PhoneNo As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txt_Address4 As System.Windows.Forms.TextBox
    Friend WithEvents txt_Address3 As System.Windows.Forms.TextBox
    Friend WithEvents txt_Address2 As System.Windows.Forms.TextBox
    Friend WithEvents txt_Address1 As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txt_Name As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents grp_Back As System.Windows.Forms.GroupBox
End Class
