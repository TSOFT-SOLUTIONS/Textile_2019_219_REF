<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class QueryForm
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
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txt_Query = New System.Windows.Forms.TextBox()
        Me.btn_NewQuery = New System.Windows.Forms.Button()
        Me.btn_ExecuteQuery = New System.Windows.Forms.Button()
        Me.txt_Result = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btn_Close = New System.Windows.Forms.Button()
        Me.dgv_Result = New System.Windows.Forms.DataGridView()
        Me.btn_Clear = New System.Windows.Forms.Button()
        Me.txtConnectionString = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btn_DefaultConnection = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        CType(Me.dgv_Result, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label5.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label5.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(0, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(1195, 40)
        Me.Label5.TabIndex = 39
        Me.Label5.Text = "RUN SQL QUERY"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txt_Query
        '
        Me.txt_Query.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Query.Location = New System.Drawing.Point(169, 84)
        Me.txt_Query.Multiline = True
        Me.txt_Query.Name = "txt_Query"
        Me.txt_Query.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txt_Query.Size = New System.Drawing.Size(1005, 74)
        Me.txt_Query.TabIndex = 40
        '
        'btn_NewQuery
        '
        Me.btn_NewQuery.BackColor = System.Drawing.Color.Navy
        Me.btn_NewQuery.Font = New System.Drawing.Font("Candara", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_NewQuery.ForeColor = System.Drawing.Color.White
        Me.btn_NewQuery.Location = New System.Drawing.Point(14, 84)
        Me.btn_NewQuery.Name = "btn_NewQuery"
        Me.btn_NewQuery.Size = New System.Drawing.Size(148, 37)
        Me.btn_NewQuery.TabIndex = 41
        Me.btn_NewQuery.Text = "New Query"
        Me.btn_NewQuery.UseVisualStyleBackColor = False
        '
        'btn_ExecuteQuery
        '
        Me.btn_ExecuteQuery.BackColor = System.Drawing.Color.DimGray
        Me.btn_ExecuteQuery.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_ExecuteQuery.ForeColor = System.Drawing.Color.White
        Me.btn_ExecuteQuery.Location = New System.Drawing.Point(1082, 166)
        Me.btn_ExecuteQuery.Name = "btn_ExecuteQuery"
        Me.btn_ExecuteQuery.Size = New System.Drawing.Size(92, 44)
        Me.btn_ExecuteQuery.TabIndex = 42
        Me.btn_ExecuteQuery.Text = "Execute Query"
        Me.btn_ExecuteQuery.UseVisualStyleBackColor = False
        '
        'txt_Result
        '
        Me.txt_Result.Location = New System.Drawing.Point(169, 163)
        Me.txt_Result.Multiline = True
        Me.txt_Result.Name = "txt_Result"
        Me.txt_Result.ReadOnly = True
        Me.txt_Result.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txt_Result.Size = New System.Drawing.Size(871, 40)
        Me.txt_Result.TabIndex = 43
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(16, 166)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(96, 15)
        Me.Label1.TabIndex = 44
        Me.Label1.Text = "Query Response"
        '
        'btn_Close
        '
        Me.btn_Close.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(90, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btn_Close.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Close.ForeColor = System.Drawing.Color.White
        Me.btn_Close.Location = New System.Drawing.Point(1026, 456)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(148, 37)
        Me.btn_Close.TabIndex = 45
        Me.btn_Close.Text = "&X Close"
        Me.btn_Close.UseVisualStyleBackColor = False
        '
        'dgv_Result
        '
        Me.dgv_Result.BackgroundColor = System.Drawing.Color.White
        Me.dgv_Result.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_Result.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgv_Result.Location = New System.Drawing.Point(20, 240)
        Me.dgv_Result.Name = "dgv_Result"
        Me.dgv_Result.ReadOnly = True
        Me.dgv_Result.Size = New System.Drawing.Size(1160, 209)
        Me.dgv_Result.TabIndex = 46
        '
        'btn_Clear
        '
        Me.btn_Clear.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btn_Clear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Clear.ForeColor = System.Drawing.Color.White
        Me.btn_Clear.Location = New System.Drawing.Point(20, 456)
        Me.btn_Clear.Name = "btn_Clear"
        Me.btn_Clear.Size = New System.Drawing.Size(132, 37)
        Me.btn_Clear.TabIndex = 47
        Me.btn_Clear.Text = "&Clear Results"
        Me.btn_Clear.UseVisualStyleBackColor = False
        '
        'txtConnectionString
        '
        Me.txtConnectionString.Location = New System.Drawing.Point(169, 52)
        Me.txtConnectionString.Name = "txtConnectionString"
        Me.txtConnectionString.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtConnectionString.Size = New System.Drawing.Size(871, 23)
        Me.txtConnectionString.TabIndex = 48
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(16, 55)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(104, 15)
        Me.Label2.TabIndex = 49
        Me.Label2.Text = "Connection String"
        '
        'btn_DefaultConnection
        '
        Me.btn_DefaultConnection.BackColor = System.Drawing.Color.DarkBlue
        Me.btn_DefaultConnection.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_DefaultConnection.ForeColor = System.Drawing.Color.White
        Me.btn_DefaultConnection.Location = New System.Drawing.Point(1053, 44)
        Me.btn_DefaultConnection.Name = "btn_DefaultConnection"
        Me.btn_DefaultConnection.Size = New System.Drawing.Size(127, 37)
        Me.btn_DefaultConnection.TabIndex = 50
        Me.btn_DefaultConnection.Text = "Default Connection"
        Me.btn_DefaultConnection.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(16, 222)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(78, 15)
        Me.Label3.TabIndex = 51
        Me.Label3.Text = "Query Result"
        '
        'QueryForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSkyBlue
        Me.ClientSize = New System.Drawing.Size(1195, 497)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.btn_DefaultConnection)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtConnectionString)
        Me.Controls.Add(Me.btn_Clear)
        Me.Controls.Add(Me.dgv_Result)
        Me.Controls.Add(Me.btn_Close)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txt_Result)
        Me.Controls.Add(Me.btn_ExecuteQuery)
        Me.Controls.Add(Me.btn_NewQuery)
        Me.Controls.Add(Me.txt_Query)
        Me.Controls.Add(Me.Label5)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "QueryForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        CType(Me.dgv_Result, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label5 As Label
    Friend WithEvents txt_Query As TextBox
    Friend WithEvents btn_NewQuery As Button
    Friend WithEvents btn_ExecuteQuery As Button
    Friend WithEvents txt_Result As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents btn_Close As Button
    Friend WithEvents dgv_Result As DataGridView
    Friend WithEvents btn_Clear As Button
    Friend WithEvents txtConnectionString As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents btn_DefaultConnection As Button
    Friend WithEvents Label3 As Label
End Class
