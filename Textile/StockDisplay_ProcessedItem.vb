Public Class StockDisplay_ProcessedItem

    Private Sub StockDisplay_ProcessedItem_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Me.Left = Screen.PrimaryScreen.WorkingArea.Width - 17 - Me.Width
        Me.Top = Screen.PrimaryScreen.WorkingArea.Height - 92 - Me.Height
    End Sub

    Private Sub StockDisplay_ProcessedItem_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Timer1.Enabled = False

        Me.Left = Screen.PrimaryScreen.WorkingArea.Width - 17 - Me.Width
        Me.Top = Screen.PrimaryScreen.WorkingArea.Height - 92 - Me.Height

        lbl_ItemName.Text = "  " & Trim(Common_Procedures.StockDisplay_ProcessedItem_Name)
        lbl_OnFloorStock.Text = Val(Common_Procedures.StockDisplay_ProcessedItem_OnFloorStock)
        lbl_OnRackStock.Text = Val(Common_Procedures.StockDisplay_ProcessedItem_OnRackStock)
        lbl_TotalStock.Text = Val(lbl_OnFloorStock.Text) + Val(lbl_OnRackStock.Text)

    End Sub

    Private Sub StockDisplay_ProcessedItem_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            Me.Close()
        End If
    End Sub

    Private Sub btn_Close_StockDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_StockDisplay.Click
        Me.Close()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Me.Close()
    End Sub

End Class