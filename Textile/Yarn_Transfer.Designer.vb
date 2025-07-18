<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Yarn_Transfer
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.pnl_back = New System.Windows.Forms.Panel()
        Me.lbl_Sales_OrderNo_To = New System.Windows.Forms.Label()
        Me.cbo_ClothSales_OrderCode_forSelection_To = New System.Windows.Forms.ComboBox()
        Me.lbl_Sales_OrderNo_From = New System.Windows.Forms.Label()
        Me.cbo_ClothSales_OrderCode_forSelection_From = New System.Windows.Forms.ComboBox()
        Me.cbo_Sizing_JobCardNo = New System.Windows.Forms.ComboBox()
        Me.lbl_Sizing_jobcardno_Caption = New System.Windows.Forms.Label()
        Me.cbo_weaving_job_no = New System.Windows.Forms.ComboBox()
        Me.lbl_weaving_job_no = New System.Windows.Forms.Label()
        Me.btn_UserModification = New System.Windows.Forms.Button()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.msk_date = New System.Windows.Forms.MaskedTextBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.cbo_PartyTo = New System.Windows.Forms.ComboBox()
        Me.txt_weightTo = New System.Windows.Forms.TextBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.btn_Print = New System.Windows.Forms.Button()
        Me.txt_weightFrom = New System.Windows.Forms.TextBox()
        Me.txt_cones = New System.Windows.Forms.TextBox()
        Me.txt_bags = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.cbo_CountTo = New System.Windows.Forms.ComboBox()
        Me.cbo_Countfrom = New System.Windows.Forms.ComboBox()
        Me.cbo_TypeFrom = New System.Windows.Forms.ComboBox()
        Me.cbo_TypeTo = New System.Windows.Forms.ComboBox()
        Me.cbo_Millfrom = New System.Windows.Forms.ComboBox()
        Me.cbo_MillTo = New System.Windows.Forms.ComboBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btn_close = New System.Windows.Forms.Button()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.lbl_RefNo = New System.Windows.Forms.Label()
        Me.lbl_Company = New System.Windows.Forms.Label()
        Me.btn_save = New System.Windows.Forms.Button()
        Me.dtp_Date = New System.Windows.Forms.DateTimePicker()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txt_remarks = New System.Windows.Forms.TextBox()
        Me.cbo_PartyFrom = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lbl_remarks = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.dgv_filter = New System.Windows.Forms.DataGridView()
        Me.dc = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btn_filtershow = New System.Windows.Forms.Button()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.dtp_FilterTo_date = New System.Windows.Forms.DateTimePicker()
        Me.dtp_FilterFrom_date = New System.Windows.Forms.DateTimePicker()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.btn_closefilter = New System.Windows.Forms.Button()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.pnl_filter = New System.Windows.Forms.Panel()
        Me.cbo_PartyNameFilter = New System.Windows.Forms.ComboBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument()
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog()
        Me.lbl_UserName = New System.Windows.Forms.Label()
        Me.pnl_back.SuspendLayout()
        CType(Me.dgv_filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnl_filter.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnl_back
        '
        Me.pnl_back.AutoSize = True
        Me.pnl_back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_back.Controls.Add(Me.lbl_Sales_OrderNo_To)
        Me.pnl_back.Controls.Add(Me.cbo_ClothSales_OrderCode_forSelection_To)
        Me.pnl_back.Controls.Add(Me.lbl_Sales_OrderNo_From)
        Me.pnl_back.Controls.Add(Me.cbo_ClothSales_OrderCode_forSelection_From)
        Me.pnl_back.Controls.Add(Me.cbo_Sizing_JobCardNo)
        Me.pnl_back.Controls.Add(Me.lbl_Sizing_jobcardno_Caption)
        Me.pnl_back.Controls.Add(Me.cbo_weaving_job_no)
        Me.pnl_back.Controls.Add(Me.lbl_weaving_job_no)
        Me.pnl_back.Controls.Add(Me.btn_UserModification)
        Me.pnl_back.Controls.Add(Me.Label31)
        Me.pnl_back.Controls.Add(Me.Label30)
        Me.pnl_back.Controls.Add(Me.Label28)
        Me.pnl_back.Controls.Add(Me.Label27)
        Me.pnl_back.Controls.Add(Me.Label26)
        Me.pnl_back.Controls.Add(Me.Label25)
        Me.pnl_back.Controls.Add(Me.Label24)
        Me.pnl_back.Controls.Add(Me.Label23)
        Me.pnl_back.Controls.Add(Me.msk_date)
        Me.pnl_back.Controls.Add(Me.Label22)
        Me.pnl_back.Controls.Add(Me.cbo_PartyTo)
        Me.pnl_back.Controls.Add(Me.txt_weightTo)
        Me.pnl_back.Controls.Add(Me.Label21)
        Me.pnl_back.Controls.Add(Me.btn_Print)
        Me.pnl_back.Controls.Add(Me.txt_weightFrom)
        Me.pnl_back.Controls.Add(Me.txt_cones)
        Me.pnl_back.Controls.Add(Me.txt_bags)
        Me.pnl_back.Controls.Add(Me.Label16)
        Me.pnl_back.Controls.Add(Me.Label15)
        Me.pnl_back.Controls.Add(Me.Label14)
        Me.pnl_back.Controls.Add(Me.cbo_CountTo)
        Me.pnl_back.Controls.Add(Me.cbo_Countfrom)
        Me.pnl_back.Controls.Add(Me.cbo_TypeFrom)
        Me.pnl_back.Controls.Add(Me.cbo_TypeTo)
        Me.pnl_back.Controls.Add(Me.cbo_Millfrom)
        Me.pnl_back.Controls.Add(Me.cbo_MillTo)
        Me.pnl_back.Controls.Add(Me.Label13)
        Me.pnl_back.Controls.Add(Me.Label10)
        Me.pnl_back.Controls.Add(Me.Label4)
        Me.pnl_back.Controls.Add(Me.btn_close)
        Me.pnl_back.Controls.Add(Me.Label12)
        Me.pnl_back.Controls.Add(Me.lbl_RefNo)
        Me.pnl_back.Controls.Add(Me.lbl_Company)
        Me.pnl_back.Controls.Add(Me.btn_save)
        Me.pnl_back.Controls.Add(Me.dtp_Date)
        Me.pnl_back.Controls.Add(Me.Label9)
        Me.pnl_back.Controls.Add(Me.Label8)
        Me.pnl_back.Controls.Add(Me.txt_remarks)
        Me.pnl_back.Controls.Add(Me.cbo_PartyFrom)
        Me.pnl_back.Controls.Add(Me.Label1)
        Me.pnl_back.Controls.Add(Me.Label7)
        Me.pnl_back.Controls.Add(Me.Label2)
        Me.pnl_back.Controls.Add(Me.lbl_remarks)
        Me.pnl_back.Controls.Add(Me.Label3)
        Me.pnl_back.Controls.Add(Me.Label5)
        Me.pnl_back.Controls.Add(Me.Label29)
        Me.pnl_back.Location = New System.Drawing.Point(7, 48)
        Me.pnl_back.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.pnl_back.Name = "pnl_back"
        Me.pnl_back.Size = New System.Drawing.Size(610, 398)
        Me.pnl_back.TabIndex = 28
        '
        'lbl_Sales_OrderNo_To
        '
        Me.lbl_Sales_OrderNo_To.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Sales_OrderNo_To.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Sales_OrderNo_To.Location = New System.Drawing.Point(310, 289)
        Me.lbl_Sales_OrderNo_To.Name = "lbl_Sales_OrderNo_To"
        Me.lbl_Sales_OrderNo_To.Size = New System.Drawing.Size(89, 30)
        Me.lbl_Sales_OrderNo_To.TabIndex = 1507
        Me.lbl_Sales_OrderNo_To.Text = "Sales Order No To"
        Me.lbl_Sales_OrderNo_To.Visible = False
        '
        'cbo_ClothSales_OrderCode_forSelection_To
        '
        Me.cbo_ClothSales_OrderCode_forSelection_To.BackColor = System.Drawing.Color.White
        Me.cbo_ClothSales_OrderCode_forSelection_To.DropDownHeight = 150
        Me.cbo_ClothSales_OrderCode_forSelection_To.DropDownWidth = 150
        Me.cbo_ClothSales_OrderCode_forSelection_To.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_ClothSales_OrderCode_forSelection_To.FormattingEnabled = True
        Me.cbo_ClothSales_OrderCode_forSelection_To.IntegralHeight = False
        Me.cbo_ClothSales_OrderCode_forSelection_To.Location = New System.Drawing.Point(410, 293)
        Me.cbo_ClothSales_OrderCode_forSelection_To.MaxDropDownItems = 15
        Me.cbo_ClothSales_OrderCode_forSelection_To.MaxLength = 50
        Me.cbo_ClothSales_OrderCode_forSelection_To.Name = "cbo_ClothSales_OrderCode_forSelection_To"
        Me.cbo_ClothSales_OrderCode_forSelection_To.Size = New System.Drawing.Size(181, 23)
        Me.cbo_ClothSales_OrderCode_forSelection_To.TabIndex = 1508
        Me.cbo_ClothSales_OrderCode_forSelection_To.Text = "Sales_OrderNo_To"
        Me.cbo_ClothSales_OrderCode_forSelection_To.Visible = False
        '
        'lbl_Sales_OrderNo_From
        '
        Me.lbl_Sales_OrderNo_From.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Sales_OrderNo_From.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Sales_OrderNo_From.Location = New System.Drawing.Point(7, 289)
        Me.lbl_Sales_OrderNo_From.Name = "lbl_Sales_OrderNo_From"
        Me.lbl_Sales_OrderNo_From.Size = New System.Drawing.Size(89, 30)
        Me.lbl_Sales_OrderNo_From.TabIndex = 1505
        Me.lbl_Sales_OrderNo_From.Text = "Sales Order No From"
        Me.lbl_Sales_OrderNo_From.Visible = False
        '
        'cbo_ClothSales_OrderCode_forSelection_From
        '
        Me.cbo_ClothSales_OrderCode_forSelection_From.BackColor = System.Drawing.Color.White
        Me.cbo_ClothSales_OrderCode_forSelection_From.DropDownHeight = 150
        Me.cbo_ClothSales_OrderCode_forSelection_From.DropDownWidth = 150
        Me.cbo_ClothSales_OrderCode_forSelection_From.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_ClothSales_OrderCode_forSelection_From.FormattingEnabled = True
        Me.cbo_ClothSales_OrderCode_forSelection_From.IntegralHeight = False
        Me.cbo_ClothSales_OrderCode_forSelection_From.Location = New System.Drawing.Point(99, 293)
        Me.cbo_ClothSales_OrderCode_forSelection_From.MaxDropDownItems = 15
        Me.cbo_ClothSales_OrderCode_forSelection_From.MaxLength = 50
        Me.cbo_ClothSales_OrderCode_forSelection_From.Name = "cbo_ClothSales_OrderCode_forSelection_From"
        Me.cbo_ClothSales_OrderCode_forSelection_From.Size = New System.Drawing.Size(202, 23)
        Me.cbo_ClothSales_OrderCode_forSelection_From.TabIndex = 1506
        Me.cbo_ClothSales_OrderCode_forSelection_From.Text = "Sales_OrderNo_From"
        Me.cbo_ClothSales_OrderCode_forSelection_From.Visible = False
        '
        'cbo_Sizing_JobCardNo
        '
        Me.cbo_Sizing_JobCardNo.BackColor = System.Drawing.Color.Yellow
        Me.cbo_Sizing_JobCardNo.DropDownHeight = 350
        Me.cbo_Sizing_JobCardNo.DropDownWidth = 350
        Me.cbo_Sizing_JobCardNo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Sizing_JobCardNo.FormattingEnabled = True
        Me.cbo_Sizing_JobCardNo.IntegralHeight = False
        Me.cbo_Sizing_JobCardNo.Location = New System.Drawing.Point(410, 259)
        Me.cbo_Sizing_JobCardNo.MaxDropDownItems = 15
        Me.cbo_Sizing_JobCardNo.MaxLength = 50
        Me.cbo_Sizing_JobCardNo.Name = "cbo_Sizing_JobCardNo"
        Me.cbo_Sizing_JobCardNo.Size = New System.Drawing.Size(181, 23)
        Me.cbo_Sizing_JobCardNo.Sorted = True
        Me.cbo_Sizing_JobCardNo.TabIndex = 14
        Me.cbo_Sizing_JobCardNo.Visible = False
        '
        'lbl_Sizing_jobcardno_Caption
        '
        Me.lbl_Sizing_jobcardno_Caption.AutoSize = True
        Me.lbl_Sizing_jobcardno_Caption.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Sizing_jobcardno_Caption.ForeColor = System.Drawing.Color.Blue
        Me.lbl_Sizing_jobcardno_Caption.Location = New System.Drawing.Point(310, 263)
        Me.lbl_Sizing_jobcardno_Caption.Name = "lbl_Sizing_jobcardno_Caption"
        Me.lbl_Sizing_jobcardno_Caption.Size = New System.Drawing.Size(77, 15)
        Me.lbl_Sizing_jobcardno_Caption.TabIndex = 1228
        Me.lbl_Sizing_jobcardno_Caption.Text = "Sizing Job No"
        Me.lbl_Sizing_jobcardno_Caption.Visible = False
        '
        'cbo_weaving_job_no
        '
        Me.cbo_weaving_job_no.BackColor = System.Drawing.Color.Yellow
        Me.cbo_weaving_job_no.DropDownHeight = 110
        Me.cbo_weaving_job_no.DropDownWidth = 155
        Me.cbo_weaving_job_no.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_weaving_job_no.FormattingEnabled = True
        Me.cbo_weaving_job_no.IntegralHeight = False
        Me.cbo_weaving_job_no.Location = New System.Drawing.Point(99, 259)
        Me.cbo_weaving_job_no.MaxDropDownItems = 15
        Me.cbo_weaving_job_no.MaxLength = 50
        Me.cbo_weaving_job_no.Name = "cbo_weaving_job_no"
        Me.cbo_weaving_job_no.Size = New System.Drawing.Size(202, 23)
        Me.cbo_weaving_job_no.Sorted = True
        Me.cbo_weaving_job_no.TabIndex = 13
        Me.cbo_weaving_job_no.Visible = False
        '
        'lbl_weaving_job_no
        '
        Me.lbl_weaving_job_no.AutoSize = True
        Me.lbl_weaving_job_no.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_weaving_job_no.ForeColor = System.Drawing.Color.Blue
        Me.lbl_weaving_job_no.Location = New System.Drawing.Point(6, 262)
        Me.lbl_weaving_job_no.Name = "lbl_weaving_job_no"
        Me.lbl_weaving_job_no.Size = New System.Drawing.Size(89, 15)
        Me.lbl_weaving_job_no.TabIndex = 1227
        Me.lbl_weaving_job_no.Text = "Weaver job No"
        Me.lbl_weaving_job_no.Visible = False
        '
        'btn_UserModification
        '
        Me.btn_UserModification.BackColor = System.Drawing.Color.OrangeRed
        Me.btn_UserModification.ForeColor = System.Drawing.Color.White
        Me.btn_UserModification.Location = New System.Drawing.Point(26, 363)
        Me.btn_UserModification.Name = "btn_UserModification"
        Me.btn_UserModification.Size = New System.Drawing.Size(103, 25)
        Me.btn_UserModification.TabIndex = 1177
        Me.btn_UserModification.TabStop = False
        Me.btn_UserModification.Text = "MODIFICATION"
        Me.btn_UserModification.UseVisualStyleBackColor = False
        Me.btn_UserModification.Visible = False
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label31.ForeColor = System.Drawing.Color.Red
        Me.Label31.Location = New System.Drawing.Point(340, 16)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(13, 15)
        Me.Label31.TabIndex = 304
        Me.Label31.Text = "*"
        Me.Label31.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label30.ForeColor = System.Drawing.Color.Red
        Me.Label30.Location = New System.Drawing.Point(76, 83)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(13, 15)
        Me.Label30.TabIndex = 303
        Me.Label30.Text = "*"
        Me.Label30.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label28.ForeColor = System.Drawing.Color.Red
        Me.Label28.Location = New System.Drawing.Point(83, 155)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(13, 15)
        Me.Label28.TabIndex = 303
        Me.Label28.Text = "*"
        Me.Label28.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label27.ForeColor = System.Drawing.Color.Red
        Me.Label27.Location = New System.Drawing.Point(362, 153)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(13, 15)
        Me.Label27.TabIndex = 303
        Me.Label27.Text = "*"
        Me.Label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label26.ForeColor = System.Drawing.Color.Red
        Me.Label26.Location = New System.Drawing.Point(372, 224)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(13, 15)
        Me.Label26.TabIndex = 303
        Me.Label26.Text = "*"
        Me.Label26.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label25.ForeColor = System.Drawing.Color.Red
        Me.Label25.Location = New System.Drawing.Point(355, 82)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(13, 15)
        Me.Label25.TabIndex = 303
        Me.Label25.Text = "*"
        Me.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.ForeColor = System.Drawing.Color.Red
        Me.Label24.Location = New System.Drawing.Point(358, 49)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(13, 15)
        Me.Label24.TabIndex = 303
        Me.Label24.Text = "*"
        Me.Label24.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.ForeColor = System.Drawing.Color.Red
        Me.Label23.Location = New System.Drawing.Point(79, 48)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(13, 15)
        Me.Label23.TabIndex = 302
        Me.Label23.Text = "*"
        Me.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'msk_date
        '
        Me.msk_date.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.msk_date.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.msk_date.Location = New System.Drawing.Point(410, 11)
        Me.msk_date.Mask = "00-00-0000"
        Me.msk_date.Name = "msk_date"
        Me.msk_date.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.msk_date.Size = New System.Drawing.Size(168, 22)
        Me.msk_date.TabIndex = 0
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.ForeColor = System.Drawing.Color.Blue
        Me.Label22.Location = New System.Drawing.Point(311, 49)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(51, 15)
        Me.Label22.TabIndex = 2
        Me.Label22.Text = "Party To"
        '
        'cbo_PartyTo
        '
        Me.cbo_PartyTo.DropDownHeight = 250
        Me.cbo_PartyTo.DropDownWidth = 400
        Me.cbo_PartyTo.FormattingEnabled = True
        Me.cbo_PartyTo.IntegralHeight = False
        Me.cbo_PartyTo.Location = New System.Drawing.Point(410, 44)
        Me.cbo_PartyTo.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.cbo_PartyTo.MaxLength = 35
        Me.cbo_PartyTo.Name = "cbo_PartyTo"
        Me.cbo_PartyTo.Size = New System.Drawing.Size(181, 23)
        Me.cbo_PartyTo.TabIndex = 35
        Me.cbo_PartyTo.Text = "Cbo_PartNameTo"
        '
        'txt_weightTo
        '
        Me.txt_weightTo.Location = New System.Drawing.Point(410, 224)
        Me.txt_weightTo.MaxLength = 12
        Me.txt_weightTo.Name = "txt_weightTo"
        Me.txt_weightTo.Size = New System.Drawing.Size(181, 23)
        Me.txt_weightTo.TabIndex = 12
        Me.txt_weightTo.Text = "txt_weightTo"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.ForeColor = System.Drawing.Color.Blue
        Me.Label21.Location = New System.Drawing.Point(310, 228)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(62, 15)
        Me.Label21.TabIndex = 34
        Me.Label21.Text = "Weight To"
        '
        'btn_Print
        '
        Me.btn_Print.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btn_Print.ForeColor = System.Drawing.Color.White
        Me.btn_Print.Location = New System.Drawing.Point(434, 358)
        Me.btn_Print.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btn_Print.Name = "btn_Print"
        Me.btn_Print.Size = New System.Drawing.Size(71, 30)
        Me.btn_Print.TabIndex = 17
        Me.btn_Print.TabStop = False
        Me.btn_Print.Text = "&PRINT"
        Me.btn_Print.UseVisualStyleBackColor = False
        '
        'txt_weightFrom
        '
        Me.txt_weightFrom.Location = New System.Drawing.Point(99, 224)
        Me.txt_weightFrom.MaxLength = 12
        Me.txt_weightFrom.Name = "txt_weightFrom"
        Me.txt_weightFrom.Size = New System.Drawing.Size(202, 23)
        Me.txt_weightFrom.TabIndex = 11
        Me.txt_weightFrom.Text = "txt_weightFrom"
        '
        'txt_cones
        '
        Me.txt_cones.Location = New System.Drawing.Point(410, 187)
        Me.txt_cones.MaxLength = 6
        Me.txt_cones.Name = "txt_cones"
        Me.txt_cones.Size = New System.Drawing.Size(181, 23)
        Me.txt_cones.TabIndex = 10
        Me.txt_cones.Text = "txt_cones"
        '
        'txt_bags
        '
        Me.txt_bags.Location = New System.Drawing.Point(99, 187)
        Me.txt_bags.MaxLength = 6
        Me.txt_bags.Name = "txt_bags"
        Me.txt_bags.Size = New System.Drawing.Size(201, 23)
        Me.txt_bags.TabIndex = 9
        Me.txt_bags.Text = "txt_bags"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.ForeColor = System.Drawing.Color.Blue
        Me.Label16.Location = New System.Drawing.Point(6, 228)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(79, 15)
        Me.Label16.TabIndex = 31
        Me.Label16.Text = "Weight From"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.ForeColor = System.Drawing.Color.Blue
        Me.Label15.Location = New System.Drawing.Point(6, 191)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(31, 15)
        Me.Label15.TabIndex = 30
        Me.Label15.Text = "Bags"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.ForeColor = System.Drawing.Color.Blue
        Me.Label14.Location = New System.Drawing.Point(311, 191)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(40, 15)
        Me.Label14.TabIndex = 29
        Me.Label14.Text = "Cones"
        '
        'cbo_CountTo
        '
        Me.cbo_CountTo.DropDownHeight = 180
        Me.cbo_CountTo.DropDownWidth = 350
        Me.cbo_CountTo.FormattingEnabled = True
        Me.cbo_CountTo.IntegralHeight = False
        Me.cbo_CountTo.Location = New System.Drawing.Point(410, 150)
        Me.cbo_CountTo.MaxLength = 35
        Me.cbo_CountTo.Name = "cbo_CountTo"
        Me.cbo_CountTo.Size = New System.Drawing.Size(181, 23)
        Me.cbo_CountTo.TabIndex = 8
        Me.cbo_CountTo.Text = "cbo_CountTo"
        '
        'cbo_Countfrom
        '
        Me.cbo_Countfrom.DropDownHeight = 180
        Me.cbo_Countfrom.DropDownWidth = 350
        Me.cbo_Countfrom.FormattingEnabled = True
        Me.cbo_Countfrom.IntegralHeight = False
        Me.cbo_Countfrom.Location = New System.Drawing.Point(99, 150)
        Me.cbo_Countfrom.MaxLength = 35
        Me.cbo_Countfrom.Name = "cbo_Countfrom"
        Me.cbo_Countfrom.Size = New System.Drawing.Size(201, 23)
        Me.cbo_Countfrom.TabIndex = 7
        Me.cbo_Countfrom.Text = "cbo_Countfrom"
        '
        'cbo_TypeFrom
        '
        Me.cbo_TypeFrom.FormattingEnabled = True
        Me.cbo_TypeFrom.Location = New System.Drawing.Point(99, 78)
        Me.cbo_TypeFrom.MaxLength = 35
        Me.cbo_TypeFrom.Name = "cbo_TypeFrom"
        Me.cbo_TypeFrom.Size = New System.Drawing.Size(201, 23)
        Me.cbo_TypeFrom.TabIndex = 3
        Me.cbo_TypeFrom.Text = "cbo_Typefrom"
        '
        'cbo_TypeTo
        '
        Me.cbo_TypeTo.FormattingEnabled = True
        Me.cbo_TypeTo.Location = New System.Drawing.Point(410, 78)
        Me.cbo_TypeTo.MaxLength = 35
        Me.cbo_TypeTo.Name = "cbo_TypeTo"
        Me.cbo_TypeTo.Size = New System.Drawing.Size(181, 23)
        Me.cbo_TypeTo.TabIndex = 4
        Me.cbo_TypeTo.Text = "cbo_Typeto"
        '
        'cbo_Millfrom
        '
        Me.cbo_Millfrom.DropDownHeight = 200
        Me.cbo_Millfrom.DropDownWidth = 400
        Me.cbo_Millfrom.FormattingEnabled = True
        Me.cbo_Millfrom.IntegralHeight = False
        Me.cbo_Millfrom.Location = New System.Drawing.Point(99, 113)
        Me.cbo_Millfrom.MaxLength = 35
        Me.cbo_Millfrom.Name = "cbo_Millfrom"
        Me.cbo_Millfrom.Size = New System.Drawing.Size(201, 23)
        Me.cbo_Millfrom.TabIndex = 5
        Me.cbo_Millfrom.Text = "cbo_Millfrom"
        '
        'cbo_MillTo
        '
        Me.cbo_MillTo.DropDownHeight = 200
        Me.cbo_MillTo.DropDownWidth = 450
        Me.cbo_MillTo.FormattingEnabled = True
        Me.cbo_MillTo.IntegralHeight = False
        Me.cbo_MillTo.Location = New System.Drawing.Point(410, 113)
        Me.cbo_MillTo.MaxLength = 35
        Me.cbo_MillTo.Name = "cbo_MillTo"
        Me.cbo_MillTo.Size = New System.Drawing.Size(181, 23)
        Me.cbo_MillTo.TabIndex = 6
        Me.cbo_MillTo.Text = "cbo_MillTo"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.ForeColor = System.Drawing.Color.Blue
        Me.Label13.Location = New System.Drawing.Point(311, 153)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(55, 15)
        Me.Label13.TabIndex = 28
        Me.Label13.Text = "Count To"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.ForeColor = System.Drawing.Color.Blue
        Me.Label10.Location = New System.Drawing.Point(6, 155)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(72, 15)
        Me.Label10.TabIndex = 27
        Me.Label10.Text = "Count From"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(311, 117)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(42, 15)
        Me.Label4.TabIndex = 26
        Me.Label4.Text = "Mill To"
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(527, 358)
        Me.btn_close.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(71, 30)
        Me.btn_close.TabIndex = 18
        Me.btn_close.TabStop = False
        Me.btn_close.Text = "&CLOSE"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.ForeColor = System.Drawing.Color.Blue
        Me.Label12.Location = New System.Drawing.Point(6, 83)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(65, 15)
        Me.Label12.TabIndex = 22
        Me.Label12.Text = "Type From"
        '
        'lbl_RefNo
        '
        Me.lbl_RefNo.BackColor = System.Drawing.Color.White
        Me.lbl_RefNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbl_RefNo.Location = New System.Drawing.Point(99, 11)
        Me.lbl_RefNo.Name = "lbl_RefNo"
        Me.lbl_RefNo.Size = New System.Drawing.Size(201, 23)
        Me.lbl_RefNo.TabIndex = 21
        Me.lbl_RefNo.Text = "lbl_RefNo"
        Me.lbl_RefNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbl_Company
        '
        Me.lbl_Company.AutoSize = True
        Me.lbl_Company.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lbl_Company.Location = New System.Drawing.Point(135, 368)
        Me.lbl_Company.Name = "lbl_Company"
        Me.lbl_Company.Size = New System.Drawing.Size(77, 15)
        Me.lbl_Company.TabIndex = 19
        Me.lbl_Company.Text = "lbl_Company"
        Me.lbl_Company.Visible = False
        '
        'btn_save
        '
        Me.btn_save.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btn_save.ForeColor = System.Drawing.Color.White
        Me.btn_save.Location = New System.Drawing.Point(339, 358)
        Me.btn_save.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(71, 30)
        Me.btn_save.TabIndex = 16
        Me.btn_save.TabStop = False
        Me.btn_save.Text = "&SAVE"
        Me.btn_save.UseVisualStyleBackColor = False
        '
        'dtp_Date
        '
        Me.dtp_Date.CustomFormat = "dd-MM-yyyy"
        Me.dtp_Date.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtp_Date.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtp_Date.Location = New System.Drawing.Point(576, 11)
        Me.dtp_Date.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.dtp_Date.Name = "dtp_Date"
        Me.dtp_Date.Size = New System.Drawing.Size(19, 22)
        Me.dtp_Date.TabIndex = 0
        Me.dtp_Date.TabStop = False
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.ForeColor = System.Drawing.Color.Blue
        Me.Label9.Location = New System.Drawing.Point(6, 118)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(59, 15)
        Me.Label9.TabIndex = 15
        Me.Label9.Text = "Mill From"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ForeColor = System.Drawing.Color.Blue
        Me.Label8.Location = New System.Drawing.Point(6, 48)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(68, 15)
        Me.Label8.TabIndex = 14
        Me.Label8.Text = "Party From"
        '
        'txt_remarks
        '
        Me.txt_remarks.Location = New System.Drawing.Point(99, 329)
        Me.txt_remarks.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txt_remarks.MaxLength = 100
        Me.txt_remarks.Name = "txt_remarks"
        Me.txt_remarks.Size = New System.Drawing.Size(492, 23)
        Me.txt_remarks.TabIndex = 15
        Me.txt_remarks.Text = "txt_Remarks"
        '
        'cbo_PartyFrom
        '
        Me.cbo_PartyFrom.DropDownHeight = 250
        Me.cbo_PartyFrom.DropDownWidth = 400
        Me.cbo_PartyFrom.FormattingEnabled = True
        Me.cbo_PartyFrom.IntegralHeight = False
        Me.cbo_PartyFrom.Location = New System.Drawing.Point(99, 44)
        Me.cbo_PartyFrom.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.cbo_PartyFrom.MaxLength = 35
        Me.cbo_PartyFrom.Name = "cbo_PartyFrom"
        Me.cbo_PartyFrom.Size = New System.Drawing.Size(202, 23)
        Me.cbo_PartyFrom.TabIndex = 1
        Me.cbo_PartyFrom.Text = "cbo_PartyFrom"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(6, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(47, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Ref No."
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(-13, 280)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(0, 15)
        Me.Label7.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(311, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(33, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Date"
        '
        'lbl_remarks
        '
        Me.lbl_remarks.AutoSize = True
        Me.lbl_remarks.ForeColor = System.Drawing.Color.Blue
        Me.lbl_remarks.Location = New System.Drawing.Point(6, 333)
        Me.lbl_remarks.Name = "lbl_remarks"
        Me.lbl_remarks.Size = New System.Drawing.Size(54, 15)
        Me.lbl_remarks.TabIndex = 5
        Me.lbl_remarks.Text = "Remarks"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(-13, 113)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(0, 15)
        Me.Label3.TabIndex = 2
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.Blue
        Me.Label5.Location = New System.Drawing.Point(311, 82)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(48, 15)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Type To"
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.ForeColor = System.Drawing.Color.Red
        Me.Label29.Location = New System.Drawing.Point(50, 188)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(13, 15)
        Me.Label29.TabIndex = 303
        Me.Label29.Text = "*"
        Me.Label29.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'dgv_filter
        '
        Me.dgv_filter.AllowUserToAddRows = False
        Me.dgv_filter.AllowUserToDeleteRows = False
        Me.dgv_filter.BackgroundColor = System.Drawing.Color.White
        Me.dgv_filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_filter.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.dc, Me.Column1, Me.Column2, Me.Column5, Me.Column3})
        Me.dgv_filter.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv_filter.Location = New System.Drawing.Point(-1, 103)
        Me.dgv_filter.Name = "dgv_filter"
        Me.dgv_filter.RowHeadersVisible = False
        Me.dgv_filter.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgv_filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_filter.Size = New System.Drawing.Size(566, 173)
        Me.dgv_filter.TabIndex = 5
        '
        'dc
        '
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dc.DefaultCellStyle = DataGridViewCellStyle1
        Me.dc.HeaderText = "Ref.No"
        Me.dc.MaxInputLength = 8
        Me.dc.Name = "dc"
        Me.dc.ReadOnly = True
        '
        'Column1
        '
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column1.DefaultCellStyle = DataGridViewCellStyle2
        Me.Column1.HeaderText = "Date"
        Me.Column1.Name = "Column1"
        Me.Column1.Width = 90
        '
        'Column2
        '
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle3
        Me.Column2.HeaderText = "PARTY NAME"
        Me.Column2.MaxInputLength = 35
        Me.Column2.Name = "Column2"
        Me.Column2.Width = 200
        '
        'Column5
        '
        Me.Column5.HeaderText = "WEIGHT FROM"
        Me.Column5.Name = "Column5"
        Me.Column5.Width = 80
        '
        'Column3
        '
        Me.Column3.HeaderText = "WEIGHT TO"
        Me.Column3.Name = "Column3"
        Me.Column3.Width = 80
        '
        'btn_filtershow
        '
        Me.btn_filtershow.BackColor = System.Drawing.Color.DimGray
        Me.btn_filtershow.ForeColor = System.Drawing.Color.White
        Me.btn_filtershow.Location = New System.Drawing.Point(425, 32)
        Me.btn_filtershow.Name = "btn_filtershow"
        Me.btn_filtershow.Size = New System.Drawing.Size(50, 57)
        Me.btn_filtershow.TabIndex = 3
        Me.btn_filtershow.Text = "SHOW"
        Me.btn_filtershow.UseVisualStyleBackColor = False
        '
        'Label17
        '
        Me.Label17.BackColor = System.Drawing.Color.DimGray
        Me.Label17.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.ForeColor = System.Drawing.Color.White
        Me.Label17.Location = New System.Drawing.Point(-1, 1)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(566, 23)
        Me.Label17.TabIndex = 8
        Me.Label17.Text = "FILTER"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dtp_FilterTo_date
        '
        Me.dtp_FilterTo_date.CustomFormat = "dd-MM-yyyy"
        Me.dtp_FilterTo_date.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtp_FilterTo_date.Location = New System.Drawing.Point(270, 32)
        Me.dtp_FilterTo_date.Name = "dtp_FilterTo_date"
        Me.dtp_FilterTo_date.Size = New System.Drawing.Size(102, 23)
        Me.dtp_FilterTo_date.TabIndex = 1
        '
        'dtp_FilterFrom_date
        '
        Me.dtp_FilterFrom_date.CustomFormat = "dd-MM-yyyy"
        Me.dtp_FilterFrom_date.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtp_FilterFrom_date.Location = New System.Drawing.Point(78, 32)
        Me.dtp_FilterFrom_date.Name = "dtp_FilterFrom_date"
        Me.dtp_FilterFrom_date.Size = New System.Drawing.Size(102, 23)
        Me.dtp_FilterFrom_date.TabIndex = 0
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(210, 36)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(19, 15)
        Me.Label18.TabIndex = 1
        Me.Label18.Text = "To"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(7, 37)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(33, 15)
        Me.Label19.TabIndex = 0
        Me.Label19.Text = "Date"
        '
        'btn_closefilter
        '
        Me.btn_closefilter.BackColor = System.Drawing.Color.DimGray
        Me.btn_closefilter.ForeColor = System.Drawing.Color.White
        Me.btn_closefilter.Location = New System.Drawing.Point(487, 32)
        Me.btn_closefilter.Name = "btn_closefilter"
        Me.btn_closefilter.Size = New System.Drawing.Size(51, 57)
        Me.btn_closefilter.TabIndex = 4
        Me.btn_closefilter.Text = "&CLOSE"
        Me.btn_closefilter.UseVisualStyleBackColor = False
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(6, 70)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(72, 15)
        Me.Label20.TabIndex = 9
        Me.Label20.Text = "Party Name"
        '
        'pnl_filter
        '
        Me.pnl_filter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_filter.Controls.Add(Me.cbo_PartyNameFilter)
        Me.pnl_filter.Controls.Add(Me.Label20)
        Me.pnl_filter.Controls.Add(Me.btn_closefilter)
        Me.pnl_filter.Controls.Add(Me.Label17)
        Me.pnl_filter.Controls.Add(Me.btn_filtershow)
        Me.pnl_filter.Controls.Add(Me.dgv_filter)
        Me.pnl_filter.Controls.Add(Me.dtp_FilterTo_date)
        Me.pnl_filter.Controls.Add(Me.dtp_FilterFrom_date)
        Me.pnl_filter.Controls.Add(Me.Label18)
        Me.pnl_filter.Controls.Add(Me.Label19)
        Me.pnl_filter.Location = New System.Drawing.Point(922, 130)
        Me.pnl_filter.Name = "pnl_filter"
        Me.pnl_filter.Size = New System.Drawing.Size(566, 277)
        Me.pnl_filter.TabIndex = 30
        '
        'cbo_PartyNameFilter
        '
        Me.cbo_PartyNameFilter.FormattingEnabled = True
        Me.cbo_PartyNameFilter.Location = New System.Drawing.Point(78, 66)
        Me.cbo_PartyNameFilter.MaxLength = 35
        Me.cbo_PartyNameFilter.Name = "cbo_PartyNameFilter"
        Me.cbo_PartyNameFilter.Size = New System.Drawing.Size(294, 23)
        Me.cbo_PartyNameFilter.TabIndex = 2
        Me.cbo_PartyNameFilter.Text = "cbo_PartyName"
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.Label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label11.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label11.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.White
        Me.Label11.Location = New System.Drawing.Point(0, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(625, 40)
        Me.Label11.TabIndex = 29
        Me.Label11.Text = "YARN TRANSFER"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PrintDocument1
        '
        '
        'PrintDialog1
        '
        Me.PrintDialog1.UseEXDialog = True
        '
        'lbl_UserName
        '
        Me.lbl_UserName.AutoSize = True
        Me.lbl_UserName.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(55, Byte), Integer), CType(CType(91, Byte), Integer))
        Me.lbl_UserName.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_UserName.ForeColor = System.Drawing.Color.White
        Me.lbl_UserName.Location = New System.Drawing.Point(515, 9)
        Me.lbl_UserName.Name = "lbl_UserName"
        Me.lbl_UserName.Size = New System.Drawing.Size(105, 19)
        Me.lbl_UserName.TabIndex = 267
        Me.lbl_UserName.Text = "USER : ADMIN"
        '
        'Yarn_Transfer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(625, 460)
        Me.Controls.Add(Me.lbl_UserName)
        Me.Controls.Add(Me.pnl_back)
        Me.Controls.Add(Me.pnl_filter)
        Me.Controls.Add(Me.Label11)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "Yarn_Transfer"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "YARN TRANSFER"
        Me.pnl_back.ResumeLayout(False)
        Me.pnl_back.PerformLayout()
        CType(Me.dgv_filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnl_filter.ResumeLayout(False)
        Me.pnl_filter.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnl_back As System.Windows.Forms.Panel
    Friend WithEvents btn_Print As System.Windows.Forms.Button
    Friend WithEvents txt_weightFrom As System.Windows.Forms.TextBox
    Friend WithEvents txt_cones As System.Windows.Forms.TextBox
    Friend WithEvents txt_bags As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents cbo_CountTo As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_Countfrom As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_TypeFrom As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_TypeTo As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_Millfrom As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_MillTo As System.Windows.Forms.ComboBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btn_close As System.Windows.Forms.Button
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents lbl_RefNo As System.Windows.Forms.Label
    Friend WithEvents lbl_Company As System.Windows.Forms.Label
    Friend WithEvents btn_save As System.Windows.Forms.Button
    Friend WithEvents dtp_Date As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txt_remarks As System.Windows.Forms.TextBox
    Friend WithEvents cbo_PartyFrom As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lbl_remarks As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents dgv_filter As System.Windows.Forms.DataGridView
    Friend WithEvents btn_filtershow As System.Windows.Forms.Button
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents dtp_FilterTo_date As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtp_FilterFrom_date As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents btn_closefilter As System.Windows.Forms.Button
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents pnl_filter As System.Windows.Forms.Panel
    Friend WithEvents cbo_PartyNameFilter As System.Windows.Forms.ComboBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txt_weightTo As System.Windows.Forms.TextBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents PrintDocument1 As System.Drawing.Printing.PrintDocument
    Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
    Friend WithEvents dc As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents cbo_PartyTo As System.Windows.Forms.ComboBox
    Friend WithEvents msk_date As System.Windows.Forms.MaskedTextBox
    Friend WithEvents lbl_UserName As System.Windows.Forms.Label
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents btn_UserModification As System.Windows.Forms.Button
    Friend WithEvents cbo_Sizing_JobCardNo As ComboBox
    Friend WithEvents lbl_Sizing_jobcardno_Caption As Label
    Friend WithEvents cbo_weaving_job_no As ComboBox
    Friend WithEvents lbl_weaving_job_no As Label
    Friend WithEvents lbl_Sales_OrderNo_To As Label
    Friend WithEvents cbo_ClothSales_OrderCode_forSelection_To As ComboBox
    Friend WithEvents lbl_Sales_OrderNo_From As Label
    Friend WithEvents cbo_ClothSales_OrderCode_forSelection_From As ComboBox
End Class
