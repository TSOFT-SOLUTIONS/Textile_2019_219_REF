Public Class Check_GST_EInvoice_Connectivity
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)

    Public Sub new_record() Implements Interface_MDIActions.new_record
        '---
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        '---
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        '---
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '---
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '---
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        '---
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer

        Try
            cmd.Connection = con


            cmd.CommandText = "Select min(Company_IdNo) from Company_head WHERE COMPANY_IDNO > 0  And len(COMPANY_GSTINNo) = 15 "


            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar
            End If

            cmd.Dispose()

            If movid <> 0 Then Call move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "For MOVING....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim cmd As New SqlClient.SqlCommand
        cmd.Connection = con



        cmd.CommandText = "Select min(company_idno) from company_head where  company_idno <> 0 And company_idno > " & Str(Val(txt_IdNo.Text)) & " And len(COMPANY_GSTINNo) = 15 "


        Dim movid As Integer = 0

        Try
            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar
            End If

            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "For MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer = 0

        Try
            cmd.Connection = con


            cmd.CommandText = "Select max(company_idno ) from company_head where  company_idno <> 0 And company_idno < " & Str((txt_IdNo.Text)) & " And len(COMPANY_GSTINNo) = 15 "


            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar()
            End If

            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "For MOVING....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer

        Try
            cmd.Connection = con

            cmd.CommandText = "Select max(Company_IdNo) from Company_head  WHERE COMPANY_IDNO > 0  And len(COMPANY_GSTINNo) = 15 "

            movid = 0

            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar
            End If

            cmd.Dispose()

            If movid <> 0 Then Call move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "For MOVING....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '---
    End Sub

    Private Sub btn_close_Click(sender As Object, e As EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_CheckConnectivity_Click(sender As Object, e As EventArgs) Handles btn_CheckConnectivity.Click

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        rtbeInvoiceResponse.Text = ""

        Dim einv As New eInvoice(Val(txt_IdNo.Text))
        einv.GetAuthToken(rtbeInvoiceResponse)

        con.Close()
        con.Dispose()


    End Sub

    Private Sub Check_GST_EInvoice_Connectivity_Load(sender As Object, e As EventArgs) Handles Me.Load
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        movefirst_record()

    End Sub

    Public Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim SNo As Integer = 0
        Dim n As Integer = 0

        If Val(idno) = 0 Then Exit Sub

        txt_IdNo.Text = ""
        txt_Name.Text = ""
        txt_ShortName.Text = ""
        txt_GSTIN.Text = ""
        rtbeInvoiceResponse.Text = ""

        da = New SqlClient.SqlDataAdapter("Select C.Company_IdNo,C.Company_Name,C.Company_ShortName,C.Company_GSTINNo from Company_Head C Where C.Company_IdNo = " & Val(idno.ToString) & " And len(C.COMPANY_GSTINNo) = 15 ", con)
        dt = New DataTable
        da.Fill(dt)

        If dt.Rows.Count > 0 Then

            txt_IdNo.Text = dt.Rows(0).Item("Company_IdNo")
            txt_Name.Text = dt.Rows(0).Item("Company_Name")
            txt_ShortName.Text = dt.Rows(0).Item("Company_ShortName")
            txt_GSTIN.Text = dt.Rows(0).Item("Company_GSTINNo")

        End If
        dt.Clear()
        da.Dispose()


    End Sub

    Private Sub btn_CheckConnectivity1_Click(sender As Object, e As EventArgs) Handles btn_CheckConnectivity1.Click
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        rtbeInvoiceResponse.Text = ""

        Dim ewb As New EWB(Val(txt_IdNo.Text))
        EWB.GetAuthToken(rtbeInvoiceResponse)

        con.Close()
        con.Dispose()

    End Sub
End Class