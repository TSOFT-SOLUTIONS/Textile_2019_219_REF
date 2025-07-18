<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Hardware_Creation
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Hardware_Creation))
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.txt_MinimumStock = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txt_CostRate = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txt_Code = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cbo_Unit = New System.Windows.Forms.ComboBox()
        Me.cbo_ItemGroup = New System.Windows.Forms.ComboBox()
        Me.txt_TaxRate = New System.Windows.Forms.TextBox()
        Me.txt_Rate = New System.Windows.Forms.TextBox()
        Me.txt_TaxPerc = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txt_Name = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txt_IdNo = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dgv_Filter = New System.Windows.Forms.DataGridView()
        Me.btn_CloseFilter = New System.Windows.Forms.Button()
        Me.btn_OpenFilter = New System.Windows.Forms.Button()
        Me.grp_Filter = New System.Windows.Forms.GroupBox()
        Me.btn_Find = New System.Windows.Forms.Button()
        Me.cbo_Open = New System.Windows.Forms.ComboBox()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.grp_Open = New System.Windows.Forms.GroupBox()
        Me.pnl_Back = New System.Windows.Forms.Panel()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grp_Filter.SuspendLayout()
        Me.grp_Open.SuspendLayout()
        Me.pnl_Back.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.Color.DimGray
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ForeColor = System.Drawing.Color.White
        Me.btnSave.Image = Global.Textile.My.Resources.Resources.SAVE1
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(415, 307)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(76, 30)
        Me.btnSave.TabIndex = 9
        Me.btnSave.TabStop = False
        Me.btnSave.Text = "Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btn_Close
        '
        Me.btn_Close.BackColor = System.Drawing.Color.DimGray
        Me.btn_Close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btn_Close.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn_Close.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Close.ForeColor = System.Drawing.Color.White
        Me.btn_Close.Image = Global.Textile.My.Resources.Resources.cancel1
        Me.btn_Close.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Close.Location = New System.Drawing.Point(515, 307)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(76, 30)
        Me.btn_Close.TabIndex = 10
        Me.btn_Close.TabStop = False
        Me.btn_Close.Text = "Close"
        Me.btn_Close.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Close.UseVisualStyleBackColor = False
        '
        'txt_MinimumStock
        '
        Me.txt_MinimumStock.Location = New System.Drawing.Point(454, 184)
        Me.txt_MinimumStock.MaxLength = 12
        Me.txt_MinimumStock.Name = "txt_MinimumStock"
        Me.txt_MinimumStock.Size = New System.Drawing.Size(137, 23)
        Me.txt_MinimumStock.TabIndex = 4
        Me.txt_MinimumStock.Text = "txt_MinimumStock"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(327, 188)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(95, 15)
        Me.Label11.TabIndex = 30
        Me.Label11.Text = "Minimum Stock "
        '
        'txt_CostRate
        '
        Me.txt_CostRate.Location = New System.Drawing.Point(454, 225)
        Me.txt_CostRate.MaxLength = 12
        Me.txt_CostRate.Name = "txt_CostRate"
        Me.txt_CostRate.Size = New System.Drawing.Size(137, 23)
        Me.txt_CostRate.TabIndex = 6
        Me.txt_CostRate.Text = "txt_CostRate"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(327, 229)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(62, 15)
        Me.Label10.TabIndex = 29
        Me.Label10.Text = "Cost Rate "
        '
        'txt_Code
        '
        Me.txt_Code.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_Code.Location = New System.Drawing.Point(128, 102)
        Me.txt_Code.MaxLength = 20
        Me.txt_Code.Name = "txt_Code"
        Me.txt_Code.Size = New System.Drawing.Size(463, 23)
        Me.txt_Code.TabIndex = 1
        Me.txt_Code.Text = "TXT_CODE"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(25, 106)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(35, 15)
        Me.Label9.TabIndex = 27
        Me.Label9.Text = "Code"
        '
        'cbo_Unit
        '
        Me.cbo_Unit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Unit.FormattingEnabled = True
        Me.cbo_Unit.Location = New System.Drawing.Point(128, 184)
        Me.cbo_Unit.Name = "cbo_Unit"
        Me.cbo_Unit.Size = New System.Drawing.Size(157, 23)
        Me.cbo_Unit.TabIndex = 3
        Me.cbo_Unit.Text = "cbo_Unit"
        '
        'cbo_ItemGroup
        '
        Me.cbo_ItemGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_ItemGroup.FormattingEnabled = True
        Me.cbo_ItemGroup.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cbo_ItemGroup.Location = New System.Drawing.Point(128, 143)
        Me.cbo_ItemGroup.Name = "cbo_ItemGroup"
        Me.cbo_ItemGroup.Size = New System.Drawing.Size(463, 23)
        Me.cbo_ItemGroup.TabIndex = 2
        Me.cbo_ItemGroup.Text = "cbo_ItemGroup"
        '
        'txt_TaxRate
        '
        Me.txt_TaxRate.Location = New System.Drawing.Point(454, 266)
        Me.txt_TaxRate.MaxLength = 12
        Me.txt_TaxRate.Name = "txt_TaxRate"
        Me.txt_TaxRate.Size = New System.Drawing.Size(137, 23)
        Me.txt_TaxRate.TabIndex = 8
        Me.txt_TaxRate.Text = "txt_TaxRate"
        '
        'txt_Rate
        '
        Me.txt_Rate.Location = New System.Drawing.Point(128, 266)
        Me.txt_Rate.MaxLength = 12
        Me.txt_Rate.Name = "txt_Rate"
        Me.txt_Rate.Size = New System.Drawing.Size(157, 23)
        Me.txt_Rate.TabIndex = 7
        Me.txt_Rate.Text = "txt_Rate"
        '
        'txt_TaxPerc
        '
        Me.txt_TaxPerc.Location = New System.Drawing.Point(128, 225)
        Me.txt_TaxPerc.MaxLength = 6
        Me.txt_TaxPerc.Name = "txt_TaxPerc"
        Me.txt_TaxPerc.Size = New System.Drawing.Size(157, 23)
        Me.txt_TaxPerc.TabIndex = 5
        Me.txt_TaxPerc.Text = "txt_TaxPerc"
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(327, 266)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(98, 31)
        Me.Label8.TabIndex = 13
        Me.Label8.Text = "Sales Rate  (Incl.Tax)"
        '
        'txt_Name
        '
        Me.txt_Name.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txt_Name.Location = New System.Drawing.Point(128, 61)
        Me.txt_Name.MaxLength = 50
        Me.txt_Name.Name = "txt_Name"
        Me.txt_Name.Size = New System.Drawing.Size(463, 23)
        Me.txt_Name.TabIndex = 0
        Me.txt_Name.Text = "TXT_NAME"
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(25, 266)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(90, 31)
        Me.Label6.TabIndex = 14
        Me.Label6.Text = "Sales Rate (Excl.Tax)"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(25, 188)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(30, 15)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Unit"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(25, 229)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(36, 15)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "Tax %"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(25, 147)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(70, 15)
        Me.Label3.TabIndex = 18
        Me.Label3.Text = "Item Group"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(25, 66)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(40, 15)
        Me.Label2.TabIndex = 15
        Me.Label2.Text = "Name"
        '
        'txt_IdNo
        '
        Me.txt_IdNo.Enabled = False
        Me.txt_IdNo.Location = New System.Drawing.Point(128, 20)
        Me.txt_IdNo.Name = "txt_IdNo"
        Me.txt_IdNo.Size = New System.Drawing.Size(463, 23)
        Me.txt_IdNo.TabIndex = 16
        Me.txt_IdNo.Text = "txt_IdNo"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(25, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(23, 15)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "No"
        '
        'dgv_Filter
        '
        Me.dgv_Filter.AllowUserToAddRows = False
        Me.dgv_Filter.AllowUserToDeleteRows = False
        Me.dgv_Filter.AllowUserToResizeColumns = False
        Me.dgv_Filter.AllowUserToResizeRows = False
        Me.dgv_Filter.BackgroundColor = System.Drawing.Color.White
        Me.dgv_Filter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_Filter.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv_Filter.Location = New System.Drawing.Point(9, 22)
        Me.dgv_Filter.Name = "dgv_Filter"
        Me.dgv_Filter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgv_Filter.Size = New System.Drawing.Size(562, 251)
        Me.dgv_Filter.TabIndex = 0
        '
        'btn_CloseFilter
        '
        Me.btn_CloseFilter.BackColor = System.Drawing.Color.LightBlue
        Me.btn_CloseFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_CloseFilter.ForeColor = System.Drawing.Color.Blue
        Me.btn_CloseFilter.Location = New System.Drawing.Point(477, 283)
        Me.btn_CloseFilter.Name = "btn_CloseFilter"
        Me.btn_CloseFilter.Size = New System.Drawing.Size(94, 32)
        Me.btn_CloseFilter.TabIndex = 2
        Me.btn_CloseFilter.Text = "&CLOSE"
        Me.btn_CloseFilter.UseVisualStyleBackColor = False
        '
        'btn_OpenFilter
        '
        Me.btn_OpenFilter.BackColor = System.Drawing.Color.LightBlue
        Me.btn_OpenFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_OpenFilter.ForeColor = System.Drawing.Color.Blue
        Me.btn_OpenFilter.Location = New System.Drawing.Point(363, 283)
        Me.btn_OpenFilter.Name = "btn_OpenFilter"
        Me.btn_OpenFilter.Size = New System.Drawing.Size(96, 31)
        Me.btn_OpenFilter.TabIndex = 1
        Me.btn_OpenFilter.Text = "&OPEN"
        Me.btn_OpenFilter.UseVisualStyleBackColor = False
        '
        'grp_Filter
        '
        Me.grp_Filter.Controls.Add(Me.dgv_Filter)
        Me.grp_Filter.Controls.Add(Me.btn_CloseFilter)
        Me.grp_Filter.Controls.Add(Me.btn_OpenFilter)
        Me.grp_Filter.Location = New System.Drawing.Point(670, 72)
        Me.grp_Filter.Name = "grp_Filter"
        Me.grp_Filter.Size = New System.Drawing.Size(590, 327)
        Me.grp_Filter.TabIndex = 36
        Me.grp_Filter.TabStop = False
        Me.grp_Filter.Text = "FILTER"
        '
        'btn_Find
        '
        Me.btn_Find.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Find.Image = CType(resources.GetObject("btn_Find.Image"), System.Drawing.Image)
        Me.btn_Find.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Find.Location = New System.Drawing.Point(332, 180)
        Me.btn_Find.Name = "btn_Find"
        Me.btn_Find.Size = New System.Drawing.Size(77, 29)
        Me.btn_Find.TabIndex = 31
        Me.btn_Find.Text = "   &Find"
        Me.btn_Find.UseVisualStyleBackColor = True
        '
        'cbo_Open
        '
        Me.cbo_Open.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbo_Open.DropDownHeight = 90
        Me.cbo_Open.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_Open.FormattingEnabled = True
        Me.cbo_Open.IntegralHeight = False
        Me.cbo_Open.Location = New System.Drawing.Point(19, 32)
        Me.cbo_Open.Name = "cbo_Open"
        Me.cbo_Open.Size = New System.Drawing.Size(475, 23)
        Me.cbo_Open.Sorted = True
        Me.cbo_Open.TabIndex = 0
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(417, 180)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(77, 29)
        Me.btnClose.TabIndex = 30
        Me.btnClose.Text = "   &Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'grp_Open
        '
        Me.grp_Open.BackColor = System.Drawing.Color.LightBlue
        Me.grp_Open.Controls.Add(Me.btn_Find)
        Me.grp_Open.Controls.Add(Me.cbo_Open)
        Me.grp_Open.Controls.Add(Me.btnClose)
        Me.grp_Open.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp_Open.Location = New System.Drawing.Point(43, 443)
        Me.grp_Open.Name = "grp_Open"
        Me.grp_Open.Size = New System.Drawing.Size(512, 227)
        Me.grp_Open.TabIndex = 35
        Me.grp_Open.TabStop = False
        Me.grp_Open.Text = "Finding"
        '
        'pnl_Back
        '
        Me.pnl_Back.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnl_Back.Controls.Add(Me.Label12)
        Me.pnl_Back.Controls.Add(Me.Label23)
        Me.pnl_Back.Controls.Add(Me.btnSave)
        Me.pnl_Back.Controls.Add(Me.btn_Close)
        Me.pnl_Back.Controls.Add(Me.txt_MinimumStock)
        Me.pnl_Back.Controls.Add(Me.Label11)
        Me.pnl_Back.Controls.Add(Me.txt_CostRate)
        Me.pnl_Back.Controls.Add(Me.Label10)
        Me.pnl_Back.Controls.Add(Me.txt_Code)
        Me.pnl_Back.Controls.Add(Me.Label9)
        Me.pnl_Back.Controls.Add(Me.cbo_Unit)
        Me.pnl_Back.Controls.Add(Me.cbo_ItemGroup)
        Me.pnl_Back.Controls.Add(Me.txt_TaxRate)
        Me.pnl_Back.Controls.Add(Me.txt_Rate)
        Me.pnl_Back.Controls.Add(Me.txt_TaxPerc)
        Me.pnl_Back.Controls.Add(Me.Label8)
        Me.pnl_Back.Controls.Add(Me.txt_Name)
        Me.pnl_Back.Controls.Add(Me.Label6)
        Me.pnl_Back.Controls.Add(Me.Label4)
        Me.pnl_Back.Controls.Add(Me.Label5)
        Me.pnl_Back.Controls.Add(Me.Label3)
        Me.pnl_Back.Controls.Add(Me.Label2)
        Me.pnl_Back.Controls.Add(Me.txt_IdNo)
        Me.pnl_Back.Controls.Add(Me.Label1)
        Me.pnl_Back.Location = New System.Drawing.Point(6, 50)
        Me.pnl_Back.Name = "pnl_Back"
        Me.pnl_Back.Size = New System.Drawing.Size(617, 360)
        Me.pnl_Back.TabIndex = 37
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.Red
        Me.Label12.Location = New System.Drawing.Point(53, 188)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(13, 15)
        Me.Label12.TabIndex = 300
        Me.Label12.Text = "*"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.ForeColor = System.Drawing.Color.Red
        Me.Label23.Location = New System.Drawing.Point(61, 66)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(13, 15)
        Me.Label23.TabIndex = 300
        Me.Label23.Text = "*"
        Me.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label7
        '
        Me.Label7.AutoEllipsis = True
        Me.Label7.BackColor = System.Drawing.Color.DimGray
        Me.Label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label7.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label7.Font = New System.Drawing.Font("Calibri", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.Label7.Location = New System.Drawing.Point(0, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(640, 40)
        Me.Label7.TabIndex = 34
        Me.Label7.Text = "HARDWARE CREATION"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Hardware_Creation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightBlue
        Me.CausesValidation = False
        Me.ClientSize = New System.Drawing.Size(640, 423)
        Me.Controls.Add(Me.grp_Filter)
        Me.Controls.Add(Me.grp_Open)
        Me.Controls.Add(Me.pnl_Back)
        Me.Controls.Add(Me.Label7)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Hardware_Creation"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "HARDWARE CREATION"
        CType(Me.dgv_Filter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grp_Filter.ResumeLayout(False)
        Me.grp_Open.ResumeLayout(False)
        Me.pnl_Back.ResumeLayout(False)
        Me.pnl_Back.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents txt_MinimumStock As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txt_CostRate As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txt_Code As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents cbo_Unit As System.Windows.Forms.ComboBox
    Friend WithEvents cbo_ItemGroup As System.Windows.Forms.ComboBox
    Friend WithEvents txt_TaxRate As System.Windows.Forms.TextBox
    Friend WithEvents txt_Rate As System.Windows.Forms.TextBox
    Friend WithEvents txt_TaxPerc As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txt_Name As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txt_IdNo As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dgv_Filter As System.Windows.Forms.DataGridView
    Friend WithEvents btn_CloseFilter As System.Windows.Forms.Button
    Friend WithEvents btn_OpenFilter As System.Windows.Forms.Button
    Friend WithEvents grp_Filter As System.Windows.Forms.GroupBox
    Friend WithEvents btn_Find As System.Windows.Forms.Button
    Friend WithEvents cbo_Open As System.Windows.Forms.ComboBox
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents grp_Open As System.Windows.Forms.GroupBox
    Friend WithEvents pnl_Back As System.Windows.Forms.Panel
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
End Class
