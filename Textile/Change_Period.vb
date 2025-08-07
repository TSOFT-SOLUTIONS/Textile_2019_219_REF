Public Class Change_Period
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private VManual_Auto_STS As String = ""

    Public Sub New(ByVal Manual_Automatic_frmSTS As String)
        VManual_Auto_STS = Manual_Automatic_frmSTS
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        '------
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '------
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '------
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        '------
    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        '------
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        '------
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        '------
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        '------
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        '------
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '------
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        '------
    End Sub

    Private Sub Change_Period_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        FrmLdSTS = False
    End Sub

    Private Sub Change_Period_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then
                Me.Close()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub Change_Period_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim i As Integer = 0
        Dim vFrmYr As Integer = 0
        Dim vToYr As Integer = 0

        FrmLdSTS = True

        con.Open()

        vFrmYr = Val(Microsoft.VisualBasic.Left(Common_Procedures.CompGroupFnRange, 4))
        vToYr = Val(Microsoft.VisualBasic.Right(Common_Procedures.CompGroupFnRange, 4))

        cbo_FromYear.Items.Clear()
        For i = vFrmYr To vToYr
            cbo_FromYear.Items.Add(i)
        Next

        cbo_FromYear.Text = Year(Common_Procedures.Company_FromDate)
        lbl_ToYear.Text = Year(Common_Procedures.Company_ToDate)

        cbo_FromYear.Enabled = True
        chk_Dontaskagain.Visible = False
        btn_ChangePeriod.Visible = True
        If Trim(UCase(VManual_Auto_STS)) = "AUTO" Then
            Me.Text = "CREATE NEXT YEAR"
            cbo_FromYear.Text = Year(Common_Procedures.Company_ToDate)
            lbl_ToYear.Text = Val(cbo_FromYear.Text) + 1
            cbo_FromYear.Enabled = False
            chk_Dontaskagain.Visible = True
            btn_ChangePeriod.Visible = False
        End If

        btn_CreateNextYear.Text = "CREATE NEXT YEAR - (" & Trim(vToYr) & "-" & Trim(vToYr + 1) & ")"

    End Sub

    Private Sub Change_Period_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub cbo_FromYear_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_FromYear.GotFocus
        With cbo_FromYear
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectAll()
        End With
    End Sub

    Private Sub cbo_FromYear_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_FromYear.LostFocus
        With cbo_FromYear
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub btn_ChangePeriod_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ChangePeriod.GotFocus
        With btn_ChangePeriod
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
        End With
    End Sub

    Private Sub btn_ChangePeriod_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ChangePeriod.LostFocus
        With btn_ChangePeriod
            .BackColor = Color.FromArgb(41, 57, 85)
            .ForeColor = Color.White
        End With
    End Sub

    Private Sub btn_close_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.GotFocus
        With btn_close
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
        End With
    End Sub

    Private Sub btn_close_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.LostFocus
        With btn_close
            .BackColor = Color.FromArgb(41, 57, 85)
            .ForeColor = Color.White
        End With
    End Sub

    Private Sub cbo_FromYear_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_FromYear.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_FromYear, Nothing, Nothing, "", "", "", "")
    End Sub

    Private Sub cbo_FromYear_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_FromYear.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_FromYear, Nothing, "", "", "", "")
        End If
    End Sub

    Private Sub cbo_FromYear_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbo_FromYear.TextChanged
        lbl_ToYear.Text = ""
        If Val(cbo_FromYear.Text) <> 0 And Len(Trim(cbo_FromYear.Text)) >= 4 Then
            lbl_ToYear.Text = Val(cbo_FromYear.Text) + 1
        End If
    End Sub

    Private Sub btn_ChangePeriod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_ChangePeriod.Click
        If Common_Procedures.ChangePeriod_Create_NewYear(Me, Val(cbo_FromYear.Text)) = False Then
            If cbo_FromYear.Enabled And cbo_FromYear.Visible Then cbo_FromYear.Focus()
        Else
            btn_close_Click(sender, e)
        End If
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_CreateNextYear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CreateNextYear.Click
        Dim vToYr As Integer = 0

        vToYr = Val(Microsoft.VisualBasic.Right(Common_Procedures.CompGroupFnRange, 4))

        cbo_FromYear.Text = Trim(vToYr)
        lbl_ToYear.Text = Trim(vToYr + 1)

        btn_CreateNextYear.Text = "CREATE NEXT YEAR - (" & Trim(vToYr) & "-" & Trim(vToYr + 1) & ")"
        btn_ChangePeriod_Click(sender, e)
    End Sub

    Private Sub chk_Dontaskagain_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_Dontaskagain.CheckedChanged
        If FrmLdSTS = True Then Exit Sub
        If chk_Dontaskagain.Visible = True Then
            Dim Cn1 As New SqlClient.SqlConnection
            Dim cmd As New SqlClient.SqlCommand
            Dim STS As Integer = 0
            Dim Nr As Long = 0

            Cn1 = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            Cn1.Open()

            cmd.Connection = Cn1

            STS = 0
            If chk_Dontaskagain.Checked = True Then STS = 1

            Nr = 0
            cmd.CommandText = "update settings_head set ChangePeriod_CreateNewYear_Alert_Dont_Ask_Status = " & Str(STS)
            Nr = cmd.ExecuteNonQuery()
            If Nr = 0 Then
                cmd.CommandText = "Insert into settings_head (ChangePeriod_CreateNewYear_Alert_Dont_Ask_Status) values (" & Str(STS) & ")"
                cmd.ExecuteNonQuery()
            End If

            Cn1.Close()
            Cn1.Dispose()

        End If
    End Sub
End Class