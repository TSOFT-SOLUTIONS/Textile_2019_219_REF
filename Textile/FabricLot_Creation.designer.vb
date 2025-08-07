<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class fabric_lotno_creation
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(fabric_lotno_creation))
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btn_Save = New System.Windows.Forms.Button()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Cbo_ClothName = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cbo_warp_count = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cbo_warp_millname = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cbo_weft_count = New System.Windows.Forms.ComboBox()
        Me.lbl_RefNo = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.cbo_weft_millname = New System.Windows.Forms.ComboBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txt_warp_lotno = New System.Windows.Forms.TextBox()
        Me.txt_fabric_lotno = New System.Windows.Forms.TextBox()
        Me.dtp_Date = New System.Windows.Forms.DateTimePicker()
        Me.msk_date = New System.Windows.Forms.MaskedTextBox()
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.txt_Weft_LotNo = New System.Windows.Forms.TextBox()
        Me.lbl_Weft_LotNo = New System.Windows.Forms.Label()
        Me.grp_Find = New System.Windows.Forms.GroupBox()
        Me.btn_Find = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.cbo_Find = New System.Windows.Forms.ComboBox()
        Me.grp_Filter = New System.Windows.Forms.GroupBox()
        Me.btn_Open = New System.Windows.Forms.Button()
        Me.btn_CloseFilter = New System.Windows.Forms.Button()
        Me.dgv_Filter = New System.Windows.Forms.DataGridView()
        Me.pnl_Back.SuspendLayout()
        Me.grp_Find.SuspendLayout()
        Me.grp_Filter.SuspendLayout()
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label3.Font = New System.Drawing.Font("Calibri", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(0, 0)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(490, 40)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "FABRIC LOTNO CREATION"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Navy
        Me.Label1.Location = New System.Drawing.Point(17, 22)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(44, 15)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Ref No"
        '
        'btn_Save
        '
        Me.btn_Save.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.btn_Save.FlatAppearance.BorderSize = 2
        Me.btn_Save.FlatAppearance.MouseDownBackColor = System.Drawing.Color.YellowGreen
        Me.btn_Save.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime
        Me.btn_Save.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Save.ForeColor = System.Drawing.Color.White
        Me.btn_Save.Location = New System.Drawing.Point(220, 364)
        Me.btn_Save.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btn_Save.Name = "btn_Save"
        Me.btn_Save.Size = New System.Drawing.Size(102, 35)
        Me.btn_Save.TabIndex = 9
        Me.btn_Save.TabStop = False
        Me.btn_Save.Text = "&SAVE"
        Me.btn_Save.UseVisualStyleBackColor = False
        '
        'btn_Close
        '
        Me.btn_Close.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.btn_Close.FlatAppearance.BorderColor = System.Drawing.Color.Blue
        Me.btn_Close.FlatAppearance.BorderSize = 2
        Me.btn_Close.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Yellow
        Me.btn_Close.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime
        Me.btn_Close.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Close.ForeColor = System.Drawing.Color.White
        Me.btn_Close.Location = New System.Drawing.Point(347, 364)
        Me.btn_Close.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(99, 35)
        Me.btn_Close.TabIndex = 10
        Me.btn_Close.TabStop = False
        Me.btn_Close.Text = "&CLOSE"
        Me.btn_Close.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Red
        Me.Label4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Navy
        Me.Label4.Location = New System.Drawing.Point(579, 43)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(30, 15)
        Me.Label4.TabIndex = 23
        Me.Label4.Text = "Sort"
        Me.Label4.Visible = False
        '
        'Cbo_ClothName
        '
        Me.Cbo_ClothName.BackColor = System.Drawing.Color.Red
        Me.Cbo_ClothName.DropDownHeight = 80
        Me.Cbo_ClothName.FormattingEnabled = True
        Me.Cbo_ClothName.IntegralHeight = False
        Me.Cbo_ClothName.Location = New System.Drawing.Point(637, 43)
        Me.Cbo_ClothName.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Cbo_ClothName.Name = "Cbo_ClothName"
        Me.Cbo_ClothName.Size = New System.Drawing.Size(302, 23)
        Me.Cbo_ClothName.TabIndex = 1
        Me.Cbo_ClothName.Visible = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Navy
        Me.Label6.Location = New System.Drawing.Point(254, 22)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(33, 15)
        Me.Label6.TabIndex = 302
        Me.Label6.Text = "Date"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Navy
        Me.Label7.Location = New System.Drawing.Point(17, 68)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(73, 15)
        Me.Label7.TabIndex = 303
        Me.Label7.Text = "Warp Count"
        '
        'cbo_warp_count
        '
        Me.cbo_warp_count.DropDownHeight = 80
        Me.cbo_warp_count.FormattingEnabled = True
        Me.cbo_warp_count.IntegralHeight = False
        Me.cbo_warp_count.Location = New System.Drawing.Point(144, 63)
        Me.cbo_warp_count.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.cbo_warp_count.Name = "cbo_warp_count"
        Me.cbo_warp_count.Size = New System.Drawing.Size(302, 23)
        Me.cbo_warp_count.TabIndex = 2
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Navy
        Me.Label8.Location = New System.Drawing.Point(17, 114)
        Me.Label8.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(93, 15)
        Me.Label8.TabIndex = 305
        Me.Label8.Text = "Warp MillName"
        '
        'cbo_warp_millname
        '
        Me.cbo_warp_millname.DropDownHeight = 80
        Me.cbo_warp_millname.FormattingEnabled = True
        Me.cbo_warp_millname.IntegralHeight = False
        Me.cbo_warp_millname.Location = New System.Drawing.Point(144, 106)
        Me.cbo_warp_millname.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.cbo_warp_millname.Name = "cbo_warp_millname"
        Me.cbo_warp_millname.Size = New System.Drawing.Size(302, 23)
        Me.cbo_warp_millname.TabIndex = 3
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.Navy
        Me.Label9.Location = New System.Drawing.Point(17, 157)
        Me.Label9.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(73, 15)
        Me.Label9.TabIndex = 307
        Me.Label9.Text = "Warp LatNo"
        '
        'cbo_weft_count
        '
        Me.cbo_weft_count.DropDownHeight = 80
        Me.cbo_weft_count.FormattingEnabled = True
        Me.cbo_weft_count.IntegralHeight = False
        Me.cbo_weft_count.Location = New System.Drawing.Point(144, 192)
        Me.cbo_weft_count.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.cbo_weft_count.Name = "cbo_weft_count"
        Me.cbo_weft_count.Size = New System.Drawing.Size(302, 23)
        Me.cbo_weft_count.TabIndex = 5
        '
        'lbl_RefNo
        '
        Me.lbl_RefNo.BackColor = System.Drawing.Color.White
        Me.lbl_RefNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_RefNo.Location = New System.Drawing.Point(144, 17)
        Me.lbl_RefNo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_RefNo.Name = "lbl_RefNo"
        Me.lbl_RefNo.Size = New System.Drawing.Size(79, 23)
        Me.lbl_RefNo.TabIndex = 309
        Me.lbl_RefNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Navy
        Me.Label2.Location = New System.Drawing.Point(17, 200)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(70, 15)
        Me.Label2.TabIndex = 310
        Me.Label2.Text = "Weft Count"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.Navy
        Me.Label11.Location = New System.Drawing.Point(17, 243)
        Me.Label11.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(90, 15)
        Me.Label11.TabIndex = 311
        Me.Label11.Text = "Weft MillName"
        '
        'cbo_weft_millname
        '
        Me.cbo_weft_millname.DropDownHeight = 80
        Me.cbo_weft_millname.FormattingEnabled = True
        Me.cbo_weft_millname.IntegralHeight = False
        Me.cbo_weft_millname.Location = New System.Drawing.Point(144, 235)
        Me.cbo_weft_millname.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.cbo_weft_millname.Name = "cbo_weft_millname"
        Me.cbo_weft_millname.Size = New System.Drawing.Size(302, 23)
        Me.cbo_weft_millname.TabIndex = 6
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.Navy
        Me.Label12.Location = New System.Drawing.Point(17, 324)
        Me.Label12.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(76, 15)
        Me.Label12.TabIndex = 313
        Me.Label12.Text = "Fabric LotNo"
        '
        'txt_warp_lotno
        '
        Me.txt_warp_lotno.Location = New System.Drawing.Point(144, 149)
        Me.txt_warp_lotno.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txt_warp_lotno.Name = "txt_warp_lotno"
        Me.txt_warp_lotno.Size = New System.Drawing.Size(302, 23)
        Me.txt_warp_lotno.TabIndex = 4
        '
        'txt_fabric_lotno
        '
        Me.txt_fabric_lotno.Location = New System.Drawing.Point(144, 321)
        Me.txt_fabric_lotno.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txt_fabric_lotno.Name = "txt_fabric_lotno"
        Me.txt_fabric_lotno.Size = New System.Drawing.Size(302, 23)
        Me.txt_fabric_lotno.TabIndex = 8
        '
        'dtp_Date
        '
        Me.dtp_Date.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_Date.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_Date.Location = New System.Drawing.Point(423, 18)
        Me.dtp_Date.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.dtp_Date.Name = "dtp_Date"
        Me.dtp_Date.Size = New System.Drawing.Size(23, 22)
        Me.dtp_Date.TabIndex = 1
        Me.dtp_Date.TabStop = False
        '
        'msk_date
        '
        Me.msk_date.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.msk_date.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.msk_date.Location = New System.Drawing.Point(311, 18)
        Me.msk_date.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.msk_date.Mask = "00-00-0000"
        Me.msk_date.Name = "msk_date"
        Me.msk_date.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.msk_date.Size = New System.Drawing.Size(118, 22)
        Me.msk_date.TabIndex = 0
        '
        'pnl_Back
        '
        Me.pnl_Back.BackColor = System.Drawing.Color.LightSkyBlue
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.txt_Weft_LotNo)
        Me.pnl_Back.Controls.Add(Me.lbl_Weft_LotNo)
        Me.pnl_Back.Controls.Add(Me.msk_date)
        Me.pnl_Back.Controls.Add(Me.dtp_Date)
        Me.pnl_Back.Controls.Add(Me.txt_fabric_lotno)
        Me.pnl_Back.Controls.Add(Me.txt_warp_lotno)
        Me.pnl_Back.Controls.Add(Me.Label12)
        Me.pnl_Back.Controls.Add(Me.cbo_weft_millname)
        Me.pnl_Back.Controls.Add(Me.Label11)
        Me.pnl_Back.Controls.Add(Me.Label2)
        Me.pnl_Back.Controls.Add(Me.lbl_RefNo)
        Me.pnl_Back.Controls.Add(Me.cbo_weft_count)
        Me.pnl_Back.Controls.Add(Me.Label9)
        Me.pnl_Back.Controls.Add(Me.cbo_warp_millname)
        Me.pnl_Back.Controls.Add(Me.Label8)
        Me.pnl_Back.Controls.Add(Me.cbo_warp_count)
        Me.pnl_Back.Controls.Add(Me.Label7)
        Me.pnl_Back.Controls.Add(Me.Label6)
        Me.pnl_Back.Controls.Add(Me.btn_Close)
        Me.pnl_Back.Controls.Add(Me.btn_Save)
        Me.pnl_Back.Controls.Add(Me.Label1)
        Me.pnl_Back.Location = New System.Drawing.Point(13, 54)
        Me.pnl_Back.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(465, 425)
        Me.pnl_Back.TabIndex = 12
        '
        'txt_Weft_LotNo
        '
        Me.txt_Weft_LotNo.Location = New System.Drawing.Point(144, 278)
        Me.txt_Weft_LotNo.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.txt_Weft_LotNo.Name = "txt_Weft_LotNo"
        Me.txt_Weft_LotNo.Size = New System.Drawing.Size(302, 23)
        Me.txt_Weft_LotNo.TabIndex = 7
        '
        'lbl_Weft_LotNo
        '
        Me.lbl_Weft_LotNo.AutoSize = True
        Me.lbl_Weft_LotNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Weft_LotNo.ForeColor = System.Drawing.Color.Navy
        Me.lbl_Weft_LotNo.Location = New System.Drawing.Point(17, 286)
        Me.lbl_Weft_LotNo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_Weft_LotNo.Name = "lbl_Weft_LotNo"
        Me.lbl_Weft_LotNo.Size = New System.Drawing.Size(71, 15)
        Me.lbl_Weft_LotNo.TabIndex = 318
        Me.lbl_Weft_LotNo.Text = "Weft LotNo"
        '
        'grp_Find
        '
        Me.grp_Find.Controls.Add(Me.btn_Find)
        Me.grp_Find.Controls.Add(Me.btnClose)
        Me.grp_Find.Controls.Add(Me.cbo_Find)
        Me.grp_Find.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Find.Location = New System.Drawing.Point(637, 87)
        Me.grp_Find.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.grp_Find.Name = "grp_Find"
        Me.grp_Find.Padding = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.grp_Find.Size = New System.Drawing.Size(483, 210)
        Me.grp_Find.TabIndex = 15
        Me.grp_Find.TabStop = False
        Me.grp_Find.Text = "FINDING"
        '
        'btn_Find
        '
        Me.btn_Find.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Find.Image = CType(resources.GetObject("btn_Find.Image"), System.Drawing.Image)
        Me.btn_Find.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Find.Location = New System.Drawing.Point(248, 155)
        Me.btn_Find.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btn_Find.Name = "btn_Find"
        Me.btn_Find.Size = New System.Drawing.Size(97, 33)
        Me.btn_Find.TabIndex = 4
        Me.btn_Find.TabStop = False
        Me.btn_Find.Text = "&Find"
        Me.btn_Find.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Find.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Image = Global.Textile.My.Resources.Resources.Close1
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(360, 155)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(97, 33)
        Me.btnClose.TabIndex = 5
        Me.btnClose.TabStop = False
        Me.btnClose.Text = "&Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'cbo_Find
        '
        Me.cbo_Find.FormattingEnabled = True
        Me.cbo_Find.Location = New System.Drawing.Point(18, 25)
        Me.cbo_Find.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.cbo_Find.Name = "cbo_Find"
        Me.cbo_Find.Size = New System.Drawing.Size(430, 23)
        Me.cbo_Find.TabIndex = 3
        '
        'grp_Filter
        '
        Me.grp_Filter.Controls.Add(Me.btn_Open)
        Me.grp_Filter.Controls.Add(Me.btn_CloseFilter)
        Me.grp_Filter.Controls.Add(Me.dgv_Filter)
        Me.grp_Filter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Filter.Location = New System.Drawing.Point(637, 316)
        Me.grp_Filter.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.grp_Filter.Name = "grp_Filter"
        Me.grp_Filter.Padding = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.grp_Filter.Size = New System.Drawing.Size(483, 257)
        Me.grp_Filter.TabIndex = 16
        Me.grp_Filter.TabStop = False
        Me.grp_Filter.Text = "FILTER"
        '
        'btn_Open
        '
        Me.btn_Open.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Open.Image = CType(resources.GetObject("btn_Open.Image"), System.Drawing.Image)
        Me.btn_Open.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Open.Location = New System.Drawing.Point(248, 212)
        Me.btn_Open.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btn_Open.Name = "btn_Open"
        Me.btn_Open.Size = New System.Drawing.Size(97, 33)
        Me.btn_Open.TabIndex = 35
        Me.btn_Open.TabStop = False
        Me.btn_Open.Text = "&Open"
        Me.btn_Open.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Open.UseVisualStyleBackColor = True
        '
        'btn_CloseFilter
        '
        Me.btn_CloseFilter.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_CloseFilter.Image = CType(resources.GetObject("btn_CloseFilter.Image"), System.Drawing.Image)
        Me.btn_CloseFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_CloseFilter.Location = New System.Drawing.Point(360, 212)
        Me.btn_CloseFilter.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btn_CloseFilter.Name = "btn_CloseFilter"
        Me.btn_CloseFilter.Size = New System.Drawing.Size(97, 33)
        Me.btn_CloseFilter.TabIndex = 34
        Me.btn_CloseFilter.TabStop = False
        Me.btn_CloseFilter.Text = "&Close"
        Me.btn_CloseFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_CloseFilter.UseVisualStyleBackColor = True
        '
        'dgv_Filter
        '
        Me.dgv_Filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_Filter.Location = New System.Drawing.Point(18, 25)
        Me.dgv_Filter.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.dgv_Filter.Name = "dgv_Filter"
        Me.dgv_Filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Filter.Size = New System.Drawing.Size(440, 180)
        Me.dgv_Filter.TabIndex = 0
        '
        'fabric_lotno_creation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(490, 491)
        Me.Controls.Add(Me.grp_Filter)
        Me.Controls.Add(Me.grp_Find)
        Me.Controls.Add(Me.pnl_Back)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Cbo_ClothName)
        Me.Controls.Add(Me.Label4)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "fabric_lotno_creation"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Tag = "528, 568"
        Me.Text = "FABRIC LOTNO CREATION"
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_Back.PerformLayout()
        Me.grp_Find.ResumeLayout(False)
        Me.grp_Filter.ResumeLayout(False)
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label3 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents btn_Save As Button
    Friend WithEvents btn_Close As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents Cbo_ClothName As ComboBox
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents cbo_warp_count As ComboBox
    Friend WithEvents Label8 As Label
    Friend WithEvents cbo_warp_millname As ComboBox
    Friend WithEvents Label9 As Label
    Friend WithEvents cbo_weft_count As ComboBox
    Friend WithEvents lbl_RefNo As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents cbo_weft_millname As ComboBox
    Friend WithEvents Label12 As Label
    Friend WithEvents txt_warp_lotno As TextBox
    Friend WithEvents txt_fabric_lotno As TextBox
    Friend WithEvents dtp_Date As DateTimePicker
    Friend WithEvents msk_date As MaskedTextBox
    Friend WithEvents pnl_Back As Panel
    Friend WithEvents grp_Find As GroupBox
    Friend WithEvents btn_Find As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents cbo_Find As ComboBox
    Friend WithEvents grp_Filter As GroupBox
    Friend WithEvents btn_Open As Button
    Friend WithEvents btn_CloseFilter As Button
    Friend WithEvents dgv_Filter As DataGridView
    Friend WithEvents txt_Weft_LotNo As TextBox
    Friend WithEvents lbl_Weft_LotNo As Label
End Class
