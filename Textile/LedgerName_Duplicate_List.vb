Public Class LedgerName_Duplicate_List
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)

    Private Sub clear()
        pnl_Back.Enabled = True
        dgv_Details.Rows.Clear()
    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub LoomNo_Production_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated


        Try

            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            new_record()

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub LoomNo_Production_Entry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Text = ""

        con.Open()

    End Sub

    Private Sub LoomNo_Production_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub LoomNo_Production_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub

                Else
                    Me.Close()

                End If

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub


    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        '----
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Exit Sub
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Exit Sub
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        get_Duplicate_LedgerName_List()
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        get_Duplicate_LedgerName_List()

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        get_Duplicate_LedgerName_List()

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        get_Duplicate_LedgerName_List()

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        get_Duplicate_LedgerName_List()
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        '-----
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        get_Duplicate_LedgerName_List()
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub



    Public Sub print_record() Implements Interface_MDIActions.print_record
        '----

    End Sub

    Private Sub btn_REFERESH_Click(sender As Object, e As EventArgs) Handles btn_REFERESH.Click
        get_Duplicate_LedgerName_List()
    End Sub


    Private Sub get_Duplicate_LedgerName_List()
        Dim Cmd As New SqlClient.SqlCommand
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim n As Integer
        Dim SNo As Integer

        Cmd.Connection = con

        Cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Int1) select sur_name, count(*) from Ledger_Head Where sur_name <> '' group by sur_name having count(*) > 1"
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "select Ledger_IdNo, Ledger_Name, Ledger_Type from Ledger_Head Where sur_name IN (Select sq1.Name1 from " & Trim(Common_Procedures.EntryTempTable) & " sq1) Order by Ledger_Name, Ledger_IdNo"
        da1 = New SqlClient.SqlDataAdapter(Cmd)
        dt1 = New DataTable
        da1.Fill(dt1)

        With dgv_Details

            .Rows.Clear()
            SNo = 0

            If dt1.Rows.Count > 0 Then

                For i = 0 To dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1

                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = dt1.Rows(i).Item("Ledger_IdNo").ToString
                    .Rows(n).Cells(2).Value = dt1.Rows(i).Item("Ledger_Name").ToString  'Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(i).Item("Ledger_IdNo").ToString))
                    .Rows(n).Cells(3).Value = dt1.Rows(i).Item("Ledger_Type").ToString

                Next i

            End If

            Grid_Cell_DeSelect()

        End With
    End Sub


End Class