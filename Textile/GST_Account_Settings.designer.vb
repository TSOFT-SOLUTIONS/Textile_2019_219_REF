<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GST_Account_Settings
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
        Me.txt_GST_Percentage = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.lbl_RefNO = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.cbo_Payable_CGST_Acc = New System.Windows.Forms.ComboBox()
        Me.cbo_Payable_SGST_Acc = New System.Windows.Forms.ComboBox()
        Me.cbo_Payable_IGST_Acc = New System.Windows.Forms.ComboBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cbo_Reverse_CGST_Acc = New System.Windows.Forms.ComboBox()
        Me.cbo_Reverse_SGST_Acc = New System.Windows.Forms.ComboBox()
        Me.cbo_Reverse_IGST_Acc = New System.Windows.Forms.ComboBox()
        Me.cbo_output_CGST_Acc = New System.Windows.Forms.ComboBox()
        Me.cbo_Output_SGST_Acc = New System.Windows.Forms.ComboBox()
        Me.cbo_Output_IGST_Acc = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cbo_Input_CGST_Acc = New System.Windows.Forms.ComboBox()
        Me.cbo_Input_SGST_Acc = New System.Windows.Forms.ComboBox()
        Me.cbo_Input_IGST_Acc = New System.Windows.Forms.ComboBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.btn_close = New System.Windows.Forms.Button()
        Me.btn_save = New System.Windows.Forms.Button()
        Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer()
        Me.LineShape1 = New Microsoft.VisualBasic.PowerPacks.LineShape()
        Me.pnl_Open = New System.Windows.Forms.Panel()
        Me.btn_Open = New System.Windows.Forms.Button()
        Me.btn_OpenClose = New System.Windows.Forms.Button()
        Me.cbo_Find = New System.Windows.Forms.ComboBox()
        Me.pnl_Back.SuspendLayout()
        Me.pnl_Open.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Font = New System.Drawing.Font("Calibri", 14.0!, System.Drawing.FontStyle.Bold)
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(900, 35)
        Me.Label1.TabIndex = 311
        Me.Label1.Text = "GST ACCOUNT SETTINGS"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnl_Back
        '
        Me.pnl_Back.BackColor = System.Drawing.Color.LightSkyBlue
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.txt_GST_Percentage)
        Me.pnl_Back.Controls.Add(Me.Label19)
        Me.pnl_Back.Controls.Add(Me.lbl_RefNO)
        Me.pnl_Back.Controls.Add(Me.Label16)
        Me.pnl_Back.Controls.Add(Me.Label15)
        Me.pnl_Back.Controls.Add(Me.cbo_Payable_CGST_Acc)
        Me.pnl_Back.Controls.Add(Me.cbo_Payable_SGST_Acc)
        Me.pnl_Back.Controls.Add(Me.cbo_Payable_IGST_Acc)
        Me.pnl_Back.Controls.Add(Me.Label12)
        Me.pnl_Back.Controls.Add(Me.Label13)
        Me.pnl_Back.Controls.Add(Me.Label14)
        Me.pnl_Back.Controls.Add(Me.Label9)
        Me.pnl_Back.Controls.Add(Me.Label10)
        Me.pnl_Back.Controls.Add(Me.Label11)
        Me.pnl_Back.Controls.Add(Me.Label5)
        Me.pnl_Back.Controls.Add(Me.Label6)
        Me.pnl_Back.Controls.Add(Me.Label8)
        Me.pnl_Back.Controls.Add(Me.cbo_Reverse_CGST_Acc)
        Me.pnl_Back.Controls.Add(Me.cbo_Reverse_SGST_Acc)
        Me.pnl_Back.Controls.Add(Me.cbo_Reverse_IGST_Acc)
        Me.pnl_Back.Controls.Add(Me.cbo_output_CGST_Acc)
        Me.pnl_Back.Controls.Add(Me.cbo_Output_SGST_Acc)
        Me.pnl_Back.Controls.Add(Me.cbo_Output_IGST_Acc)
        Me.pnl_Back.Controls.Add(Me.Label4)
        Me.pnl_Back.Controls.Add(Me.Label3)
        Me.pnl_Back.Controls.Add(Me.Label2)
        Me.pnl_Back.Controls.Add(Me.cbo_Input_CGST_Acc)
        Me.pnl_Back.Controls.Add(Me.cbo_Input_SGST_Acc)
        Me.pnl_Back.Controls.Add(Me.cbo_Input_IGST_Acc)
        Me.pnl_Back.Controls.Add(Me.Label21)
        Me.pnl_Back.Controls.Add(Me.Label18)
        Me.pnl_Back.Controls.Add(Me.Label7)
        Me.pnl_Back.Controls.Add(Me.btn_close)
        Me.pnl_Back.Controls.Add(Me.btn_save)
        Me.pnl_Back.Controls.Add(Me.ShapeContainer1)
        Me.pnl_Back.Location = New System.Drawing.Point(7, 43)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(885, 354)
        Me.pnl_Back.TabIndex = 310
        '
        'txt_GST_Percentage
        '
        Me.txt_GST_Percentage.Location = New System.Drawing.Point(296, 10)
        Me.txt_GST_Percentage.MaxLength = 10
        Me.txt_GST_Percentage.Name = "txt_GST_Percentage"
        Me.txt_GST_Percentage.Size = New System.Drawing.Size(127, 23)
        Me.txt_GST_Percentage.TabIndex = 0
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.ForeColor = System.Drawing.Color.Blue
        Me.Label19.Location = New System.Drawing.Point(227, 14)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(39, 15)
        Me.Label19.TabIndex = 179
        Me.Label19.Text = "GST %"
        '
        'lbl_RefNO
        '
        Me.lbl_RefNO.BackColor = System.Drawing.Color.White
        Me.lbl_RefNO.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_RefNO.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_RefNO.ForeColor = System.Drawing.Color.Black
        Me.lbl_RefNO.Location = New System.Drawing.Point(84, 10)
        Me.lbl_RefNO.Name = "lbl_RefNO"
        Me.lbl_RefNO.Size = New System.Drawing.Size(88, 23)
        Me.lbl_RefNO.TabIndex = 0
        Me.lbl_RefNO.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.Blue
        Me.Label16.Location = New System.Drawing.Point(12, 14)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(47, 15)
        Me.Label16.TabIndex = 179
        Me.Label16.Text = "Ref No."
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.Color.Maroon
        Me.Label15.Location = New System.Drawing.Point(652, 177)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(77, 15)
        Me.Label15.TabIndex = 212
        Me.Label15.Text = "PAYABLE A/C"
        '
        'cbo_Payable_CGST_Acc
        '
        Me.cbo_Payable_CGST_Acc.DropDownHeight = 100
        Me.cbo_Payable_CGST_Acc.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Payable_CGST_Acc.FormattingEnabled = True
        Me.cbo_Payable_CGST_Acc.IntegralHeight = False
        Me.cbo_Payable_CGST_Acc.Location = New System.Drawing.Point(531, 196)
        Me.cbo_Payable_CGST_Acc.MaxDropDownItems = 15
        Me.cbo_Payable_CGST_Acc.MaxLength = 50
        Me.cbo_Payable_CGST_Acc.Name = "cbo_Payable_CGST_Acc"
        Me.cbo_Payable_CGST_Acc.Size = New System.Drawing.Size(339, 23)
        Me.cbo_Payable_CGST_Acc.Sorted = True
        Me.cbo_Payable_CGST_Acc.TabIndex = 10
        '
        'cbo_Payable_SGST_Acc
        '
        Me.cbo_Payable_SGST_Acc.DropDownHeight = 100
        Me.cbo_Payable_SGST_Acc.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Payable_SGST_Acc.FormattingEnabled = True
        Me.cbo_Payable_SGST_Acc.IntegralHeight = False
        Me.cbo_Payable_SGST_Acc.Location = New System.Drawing.Point(531, 232)
        Me.cbo_Payable_SGST_Acc.MaxDropDownItems = 15
        Me.cbo_Payable_SGST_Acc.MaxLength = 50
        Me.cbo_Payable_SGST_Acc.Name = "cbo_Payable_SGST_Acc"
        Me.cbo_Payable_SGST_Acc.Size = New System.Drawing.Size(339, 23)
        Me.cbo_Payable_SGST_Acc.Sorted = True
        Me.cbo_Payable_SGST_Acc.TabIndex = 11
        '
        'cbo_Payable_IGST_Acc
        '
        Me.cbo_Payable_IGST_Acc.DropDownHeight = 100
        Me.cbo_Payable_IGST_Acc.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Payable_IGST_Acc.FormattingEnabled = True
        Me.cbo_Payable_IGST_Acc.IntegralHeight = False
        Me.cbo_Payable_IGST_Acc.Location = New System.Drawing.Point(531, 267)
        Me.cbo_Payable_IGST_Acc.MaxDropDownItems = 15
        Me.cbo_Payable_IGST_Acc.MaxLength = 50
        Me.cbo_Payable_IGST_Acc.Name = "cbo_Payable_IGST_Acc"
        Me.cbo_Payable_IGST_Acc.Size = New System.Drawing.Size(339, 23)
        Me.cbo_Payable_IGST_Acc.Sorted = True
        Me.cbo_Payable_IGST_Acc.TabIndex = 12
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.Blue
        Me.Label12.Location = New System.Drawing.Point(450, 271)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(52, 15)
        Me.Label12.TabIndex = 208
        Me.Label12.Text = "IGST A/c"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.Color.Blue
        Me.Label13.Location = New System.Drawing.Point(450, 236)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(55, 15)
        Me.Label13.TabIndex = 207
        Me.Label13.Text = "SGST A/c"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.Blue
        Me.Label14.Location = New System.Drawing.Point(450, 200)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(56, 15)
        Me.Label14.TabIndex = 206
        Me.Label14.Text = "CGST A/c"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.Blue
        Me.Label9.Location = New System.Drawing.Point(450, 145)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(52, 15)
        Me.Label9.TabIndex = 205
        Me.Label9.Text = "IGST A/c"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.Blue
        Me.Label10.Location = New System.Drawing.Point(450, 111)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(55, 15)
        Me.Label10.TabIndex = 204
        Me.Label10.Text = "SGST A/c"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.Blue
        Me.Label11.Location = New System.Drawing.Point(450, 74)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(56, 15)
        Me.Label11.TabIndex = 203
        Me.Label11.Text = "CGST A/c"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Blue
        Me.Label5.Location = New System.Drawing.Point(12, 271)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(52, 15)
        Me.Label5.TabIndex = 202
        Me.Label5.Text = "IGST A/c"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Blue
        Me.Label6.Location = New System.Drawing.Point(12, 236)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(55, 15)
        Me.Label6.TabIndex = 201
        Me.Label6.Text = "SGST A/c"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Blue
        Me.Label8.Location = New System.Drawing.Point(12, 200)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(56, 15)
        Me.Label8.TabIndex = 200
        Me.Label8.Text = "CGST A/c"
        '
        'cbo_Reverse_CGST_Acc
        '
        Me.cbo_Reverse_CGST_Acc.DropDownHeight = 100
        Me.cbo_Reverse_CGST_Acc.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Reverse_CGST_Acc.FormattingEnabled = True
        Me.cbo_Reverse_CGST_Acc.IntegralHeight = False
        Me.cbo_Reverse_CGST_Acc.Location = New System.Drawing.Point(531, 70)
        Me.cbo_Reverse_CGST_Acc.MaxDropDownItems = 15
        Me.cbo_Reverse_CGST_Acc.MaxLength = 50
        Me.cbo_Reverse_CGST_Acc.Name = "cbo_Reverse_CGST_Acc"
        Me.cbo_Reverse_CGST_Acc.Size = New System.Drawing.Size(339, 23)
        Me.cbo_Reverse_CGST_Acc.Sorted = True
        Me.cbo_Reverse_CGST_Acc.TabIndex = 7
        '
        'cbo_Reverse_SGST_Acc
        '
        Me.cbo_Reverse_SGST_Acc.DropDownHeight = 100
        Me.cbo_Reverse_SGST_Acc.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Reverse_SGST_Acc.FormattingEnabled = True
        Me.cbo_Reverse_SGST_Acc.IntegralHeight = False
        Me.cbo_Reverse_SGST_Acc.Location = New System.Drawing.Point(531, 107)
        Me.cbo_Reverse_SGST_Acc.MaxDropDownItems = 15
        Me.cbo_Reverse_SGST_Acc.MaxLength = 50
        Me.cbo_Reverse_SGST_Acc.Name = "cbo_Reverse_SGST_Acc"
        Me.cbo_Reverse_SGST_Acc.Size = New System.Drawing.Size(339, 23)
        Me.cbo_Reverse_SGST_Acc.Sorted = True
        Me.cbo_Reverse_SGST_Acc.TabIndex = 8
        '
        'cbo_Reverse_IGST_Acc
        '
        Me.cbo_Reverse_IGST_Acc.DropDownHeight = 100
        Me.cbo_Reverse_IGST_Acc.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Reverse_IGST_Acc.FormattingEnabled = True
        Me.cbo_Reverse_IGST_Acc.IntegralHeight = False
        Me.cbo_Reverse_IGST_Acc.Location = New System.Drawing.Point(531, 144)
        Me.cbo_Reverse_IGST_Acc.MaxDropDownItems = 15
        Me.cbo_Reverse_IGST_Acc.MaxLength = 50
        Me.cbo_Reverse_IGST_Acc.Name = "cbo_Reverse_IGST_Acc"
        Me.cbo_Reverse_IGST_Acc.Size = New System.Drawing.Size(339, 23)
        Me.cbo_Reverse_IGST_Acc.Sorted = True
        Me.cbo_Reverse_IGST_Acc.TabIndex = 9
        '
        'cbo_output_CGST_Acc
        '
        Me.cbo_output_CGST_Acc.DropDownHeight = 100
        Me.cbo_output_CGST_Acc.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_output_CGST_Acc.FormattingEnabled = True
        Me.cbo_output_CGST_Acc.IntegralHeight = False
        Me.cbo_output_CGST_Acc.Location = New System.Drawing.Point(84, 196)
        Me.cbo_output_CGST_Acc.MaxDropDownItems = 15
        Me.cbo_output_CGST_Acc.MaxLength = 50
        Me.cbo_output_CGST_Acc.Name = "cbo_output_CGST_Acc"
        Me.cbo_output_CGST_Acc.Size = New System.Drawing.Size(339, 23)
        Me.cbo_output_CGST_Acc.Sorted = True
        Me.cbo_output_CGST_Acc.TabIndex = 4
        '
        'cbo_Output_SGST_Acc
        '
        Me.cbo_Output_SGST_Acc.DropDownHeight = 100
        Me.cbo_Output_SGST_Acc.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Output_SGST_Acc.FormattingEnabled = True
        Me.cbo_Output_SGST_Acc.IntegralHeight = False
        Me.cbo_Output_SGST_Acc.Location = New System.Drawing.Point(84, 232)
        Me.cbo_Output_SGST_Acc.MaxDropDownItems = 15
        Me.cbo_Output_SGST_Acc.MaxLength = 50
        Me.cbo_Output_SGST_Acc.Name = "cbo_Output_SGST_Acc"
        Me.cbo_Output_SGST_Acc.Size = New System.Drawing.Size(339, 23)
        Me.cbo_Output_SGST_Acc.Sorted = True
        Me.cbo_Output_SGST_Acc.TabIndex = 5
        '
        'cbo_Output_IGST_Acc
        '
        Me.cbo_Output_IGST_Acc.DropDownHeight = 100
        Me.cbo_Output_IGST_Acc.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Output_IGST_Acc.FormattingEnabled = True
        Me.cbo_Output_IGST_Acc.IntegralHeight = False
        Me.cbo_Output_IGST_Acc.Location = New System.Drawing.Point(84, 267)
        Me.cbo_Output_IGST_Acc.MaxDropDownItems = 15
        Me.cbo_Output_IGST_Acc.MaxLength = 50
        Me.cbo_Output_IGST_Acc.Name = "cbo_Output_IGST_Acc"
        Me.cbo_Output_IGST_Acc.Size = New System.Drawing.Size(339, 23)
        Me.cbo_Output_IGST_Acc.Sorted = True
        Me.cbo_Output_IGST_Acc.TabIndex = 6
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Maroon
        Me.Label4.Location = New System.Drawing.Point(637, 51)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(106, 15)
        Me.Label4.TabIndex = 193
        Me.Label4.Text = "REVERSE CHARGES"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Maroon
        Me.Label3.Location = New System.Drawing.Point(215, 177)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(75, 15)
        Me.Label3.TabIndex = 192
        Me.Label3.Text = "OUTPUT A/C"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Maroon
        Me.Label2.Location = New System.Drawing.Point(220, 51)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(64, 15)
        Me.Label2.TabIndex = 191
        Me.Label2.Text = "INPUT A/C"
        '
        'cbo_Input_CGST_Acc
        '
        Me.cbo_Input_CGST_Acc.DropDownHeight = 100
        Me.cbo_Input_CGST_Acc.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Input_CGST_Acc.FormattingEnabled = True
        Me.cbo_Input_CGST_Acc.IntegralHeight = False
        Me.cbo_Input_CGST_Acc.Location = New System.Drawing.Point(84, 70)
        Me.cbo_Input_CGST_Acc.MaxDropDownItems = 15
        Me.cbo_Input_CGST_Acc.MaxLength = 50
        Me.cbo_Input_CGST_Acc.Name = "cbo_Input_CGST_Acc"
        Me.cbo_Input_CGST_Acc.Size = New System.Drawing.Size(339, 23)
        Me.cbo_Input_CGST_Acc.Sorted = True
        Me.cbo_Input_CGST_Acc.TabIndex = 1
        '
        'cbo_Input_SGST_Acc
        '
        Me.cbo_Input_SGST_Acc.DropDownHeight = 100
        Me.cbo_Input_SGST_Acc.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Input_SGST_Acc.FormattingEnabled = True
        Me.cbo_Input_SGST_Acc.IntegralHeight = False
        Me.cbo_Input_SGST_Acc.Location = New System.Drawing.Point(84, 107)
        Me.cbo_Input_SGST_Acc.MaxDropDownItems = 15
        Me.cbo_Input_SGST_Acc.MaxLength = 50
        Me.cbo_Input_SGST_Acc.Name = "cbo_Input_SGST_Acc"
        Me.cbo_Input_SGST_Acc.Size = New System.Drawing.Size(339, 23)
        Me.cbo_Input_SGST_Acc.Sorted = True
        Me.cbo_Input_SGST_Acc.TabIndex = 2
        '
        'cbo_Input_IGST_Acc
        '
        Me.cbo_Input_IGST_Acc.DropDownHeight = 100
        Me.cbo_Input_IGST_Acc.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Input_IGST_Acc.FormattingEnabled = True
        Me.cbo_Input_IGST_Acc.IntegralHeight = False
        Me.cbo_Input_IGST_Acc.Location = New System.Drawing.Point(84, 144)
        Me.cbo_Input_IGST_Acc.MaxDropDownItems = 15
        Me.cbo_Input_IGST_Acc.MaxLength = 50
        Me.cbo_Input_IGST_Acc.Name = "cbo_Input_IGST_Acc"
        Me.cbo_Input_IGST_Acc.Size = New System.Drawing.Size(339, 23)
        Me.cbo_Input_IGST_Acc.Sorted = True
        Me.cbo_Input_IGST_Acc.TabIndex = 3
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.ForeColor = System.Drawing.Color.Blue
        Me.Label21.Location = New System.Drawing.Point(12, 149)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(52, 15)
        Me.Label21.TabIndex = 181
        Me.Label21.Text = "IGST A/c"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.ForeColor = System.Drawing.Color.Blue
        Me.Label18.Location = New System.Drawing.Point(12, 111)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(55, 15)
        Me.Label18.TabIndex = 180
        Me.Label18.Text = "SGST A/c"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Blue
        Me.Label7.Location = New System.Drawing.Point(12, 74)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(56, 15)
        Me.Label7.TabIndex = 179
        Me.Label7.Text = "CGST A/c"
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(90, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btn_close.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(792, 306)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(78, 35)
        Me.btn_close.TabIndex = 14
        Me.btn_close.TabStop = False
        Me.btn_close.Text = "&CLOSE"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'btn_save
        '
        Me.btn_save.BackColor = System.Drawing.Color.FromArgb(CType(CType(5, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.btn_save.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_save.ForeColor = System.Drawing.Color.White
        Me.btn_save.Location = New System.Drawing.Point(693, 306)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(78, 35)
        Me.btn_save.TabIndex = 13
        Me.btn_save.TabStop = False
        Me.btn_save.Text = "&SAVE"
        Me.btn_save.UseVisualStyleBackColor = False
        '
        'ShapeContainer1
        '
        Me.ShapeContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ShapeContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.ShapeContainer1.Name = "ShapeContainer1"
        Me.ShapeContainer1.Shapes.AddRange(New Microsoft.VisualBasic.PowerPacks.Shape() {Me.LineShape1})
        Me.ShapeContainer1.Size = New System.Drawing.Size(883, 352)
        Me.ShapeContainer1.TabIndex = 213
        Me.ShapeContainer1.TabStop = False
        '
        'LineShape1
        '
        Me.LineShape1.Name = "LineShape1"
        Me.LineShape1.SelectionColor = System.Drawing.Color.Black
        Me.LineShape1.X1 = 0
        Me.LineShape1.X2 = 883
        Me.LineShape1.Y1 = 44
        Me.LineShape1.Y2 = 44
        '
        'pnl_Open
        '
        Me.pnl_Open.BackColor = System.Drawing.Color.LightSteelBlue
        Me.pnl_Open.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnl_Open.Controls.Add(Me.btn_Open)
        Me.pnl_Open.Controls.Add(Me.btn_OpenClose)
        Me.pnl_Open.Controls.Add(Me.cbo_Find)
        Me.pnl_Open.Location = New System.Drawing.Point(106, 437)
        Me.pnl_Open.Name = "pnl_Open"
        Me.pnl_Open.Size = New System.Drawing.Size(514, 214)
        Me.pnl_Open.TabIndex = 312
        Me.pnl_Open.TabStop = True
        '
        'btn_Open
        '
        Me.btn_Open.BackColor = System.Drawing.Color.DarkBlue
        Me.btn_Open.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btn_Open.ForeColor = System.Drawing.Color.White
        Me.btn_Open.Location = New System.Drawing.Point(304, 151)
        Me.btn_Open.Name = "btn_Open"
        Me.btn_Open.Size = New System.Drawing.Size(80, 32)
        Me.btn_Open.TabIndex = 3
        Me.btn_Open.TabStop = False
        Me.btn_Open.Text = "&OPEN"
        Me.btn_Open.UseVisualStyleBackColor = False
        '
        'btn_OpenClose
        '
        Me.btn_OpenClose.BackColor = System.Drawing.Color.DarkBlue
        Me.btn_OpenClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btn_OpenClose.ForeColor = System.Drawing.Color.White
        Me.btn_OpenClose.Location = New System.Drawing.Point(397, 151)
        Me.btn_OpenClose.Name = "btn_OpenClose"
        Me.btn_OpenClose.Size = New System.Drawing.Size(80, 32)
        Me.btn_OpenClose.TabIndex = 2
        Me.btn_OpenClose.Text = "&CLOSE"
        Me.btn_OpenClose.UseVisualStyleBackColor = False
        '
        'cbo_Find
        '
        Me.cbo_Find.FormattingEnabled = True
        Me.cbo_Find.Location = New System.Drawing.Point(32, 36)
        Me.cbo_Find.Name = "cbo_Find"
        Me.cbo_Find.Size = New System.Drawing.Size(445, 23)
        Me.cbo_Find.TabIndex = 1
        '
        'GST_Account_Settings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(900, 406)
        Me.Controls.Add(Me.pnl_Open)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.pnl_Back)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Name = "GST_Account_Settings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "GST ACCOUNT SETTINGS"
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_Back.PerformLayout()
        Me.pnl_Open.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents btn_close As System.Windows.Forms.Button
    Friend WithEvents btn_save As System.Windows.Forms.Button
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cbo_Input_CGST_Acc As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_Input_SGST_Acc As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_Input_IGST_Acc As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_Reverse_CGST_Acc As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_Reverse_SGST_Acc As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_Reverse_IGST_Acc As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_output_CGST_Acc As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_Output_SGST_Acc As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_Output_IGST_Acc As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cbo_Payable_CGST_Acc As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_Payable_SGST_Acc As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_Payable_IGST_Acc As System.Windows.Forms.ComboBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
    Friend WithEvents LineShape1 As Microsoft.VisualBasic.PowerPacks.LineShape
    Friend WithEvents txt_GST_Percentage As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents lbl_RefNO As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents pnl_Open As System.Windows.Forms.Panel
    Friend WithEvents btn_Open As System.Windows.Forms.Button
    Friend WithEvents btn_OpenClose As System.Windows.Forms.Button
    Friend WithEvents cbo_Find As System.Windows.Forms.ComboBox
End Class
