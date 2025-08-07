<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class StockDisplay_ProcessedItem
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btn_Close_StockDisplay = New System.Windows.Forms.Button()
        Me.lbl_TotalStock = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lbl_OnRackStock = New System.Windows.Forms.Label()
        Me.lbl_OnFloorStock = New System.Windows.Forms.Label()
        Me.lbl_ItemName = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.SkyBlue
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.btn_Close_StockDisplay)
        Me.Panel1.Controls.Add(Me.lbl_TotalStock)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.lbl_OnRackStock)
        Me.Panel1.Controls.Add(Me.lbl_OnFloorStock)
        Me.Panel1.Controls.Add(Me.lbl_ItemName)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(359, 119)
        Me.Panel1.TabIndex = 0
        '
        'btn_Close_StockDisplay
        '
        Me.btn_Close_StockDisplay.BackColor = System.Drawing.Color.White
        Me.btn_Close_StockDisplay.BackgroundImage = Global.Textile.My.Resources.Resources.Close1
        Me.btn_Close_StockDisplay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn_Close_StockDisplay.FlatAppearance.BorderSize = 0
        Me.btn_Close_StockDisplay.Location = New System.Drawing.Point(333, -1)
        Me.btn_Close_StockDisplay.Name = "btn_Close_StockDisplay"
        Me.btn_Close_StockDisplay.Size = New System.Drawing.Size(25, 25)
        Me.btn_Close_StockDisplay.TabIndex = 41
        Me.btn_Close_StockDisplay.TabStop = False
        Me.btn_Close_StockDisplay.UseVisualStyleBackColor = True
        '
        'lbl_TotalStock
        '
        Me.lbl_TotalStock.BackColor = System.Drawing.Color.White
        Me.lbl_TotalStock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_TotalStock.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_TotalStock.Location = New System.Drawing.Point(238, 85)
        Me.lbl_TotalStock.Name = "lbl_TotalStock"
        Me.lbl_TotalStock.Size = New System.Drawing.Size(120, 33)
        Me.lbl_TotalStock.TabIndex = 8
        Me.lbl_TotalStock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.Pink
        Me.Label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label8.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Blue
        Me.Label8.Location = New System.Drawing.Point(238, 64)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(120, 21)
        Me.Label8.TabIndex = 7
        Me.Label8.Text = "TOTAL STOCK"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbl_OnRackStock
        '
        Me.lbl_OnRackStock.BackColor = System.Drawing.Color.White
        Me.lbl_OnRackStock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_OnRackStock.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_OnRackStock.Location = New System.Drawing.Point(119, 85)
        Me.lbl_OnRackStock.Name = "lbl_OnRackStock"
        Me.lbl_OnRackStock.Size = New System.Drawing.Size(120, 33)
        Me.lbl_OnRackStock.TabIndex = 5
        Me.lbl_OnRackStock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbl_OnFloorStock
        '
        Me.lbl_OnFloorStock.BackColor = System.Drawing.Color.White
        Me.lbl_OnFloorStock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbl_OnFloorStock.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_OnFloorStock.Location = New System.Drawing.Point(0, 85)
        Me.lbl_OnFloorStock.Name = "lbl_OnFloorStock"
        Me.lbl_OnFloorStock.Size = New System.Drawing.Size(120, 33)
        Me.lbl_OnFloorStock.TabIndex = 4
        Me.lbl_OnFloorStock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbl_ItemName
        '
        Me.lbl_ItemName.BackColor = System.Drawing.Color.White
        Me.lbl_ItemName.Dock = System.Windows.Forms.DockStyle.Top
        Me.lbl_ItemName.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_ItemName.Location = New System.Drawing.Point(0, 24)
        Me.lbl_ItemName.Name = "lbl_ItemName"
        Me.lbl_ItemName.Size = New System.Drawing.Size(357, 40)
        Me.lbl_ItemName.TabIndex = 3
        Me.lbl_ItemName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Pink
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label3.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Blue
        Me.Label3.Location = New System.Drawing.Point(119, 64)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(120, 21)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "ON RACK"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Pink
        Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(0, 64)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(120, 21)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "ON FLOOR"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.MediumVioletRed
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(357, 24)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "ITEM NAME"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Timer1
        '
        Me.Timer1.Interval = 10
        '
        'StockDisplay_ProcessedItem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.SkyBlue
        Me.ClientSize = New System.Drawing.Size(359, 121)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.Name = "StockDisplay_ProcessedItem"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "StockDisplay_ProcessedItem"
        Me.TopMost = True
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lbl_OnRackStock As System.Windows.Forms.Label
    Friend WithEvents lbl_OnFloorStock As System.Windows.Forms.Label
    Friend WithEvents lbl_ItemName As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lbl_TotalStock As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents btn_Close_StockDisplay As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
End Class
