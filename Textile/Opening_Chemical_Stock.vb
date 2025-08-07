Public Class Opening_Chemical_Stock

    Implements Interface_MDIActions
    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private Pk_Condition As String = "OPENI-"
    Private cmbItmNm As String
    Dim NewCode As String = ""
    Dim OpYrCode As String
    Private Sub clear()

        pnl_Back.Enabled = True
        cmbItmNm = ""
        cbo_ItemName.Text = ""
        cbo_Unit.Text = ""
        txt_OpStock.Text = ""

    End Sub

    Private Sub move_record(ByVal idno As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        If Val(idno) = 0 Then Exit Sub

        clear()

        Try

            cbo_ItemName.Text = Common_Procedures.Item_IdNoToName(con, idno)

            da1 = New SqlClient.SqlDataAdapter("select a.Quantity, b.Item_Name, c.Unit_Name from Stock_Chemical_Processing_Details a INNER JOIN Sizing_Item_Head b ON a.Item_IdNo = b.Item_IdNo LEFT OUTER JOIN Unit_Head c ON b.Unit_IdNo = c.Unit_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Item_IdNo = " & Str(Val(idno)) & " and Reference_Code LIKE '" & Trim(Pk_Condition) & "%' Order by Reference_Date, For_OrderBy, Reference_No", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                cbo_ItemName.Text = dt1.Rows(0).Item("Item_Name").ToString
                cbo_Unit.Text = dt1.Rows(0).Item("Unit_Name").ToString
                txt_OpStock.Text = Val(dt1.Rows(0).Item("Quantity").ToString)
            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If cbo_ItemName.Visible And cbo_ItemName.Enabled Then cbo_ItemName.Focus()

    End Sub

    Private Sub Opening_Chemical_Stock_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Unit.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "UNIT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Unit.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            Common_Procedures.Master_Return.Form_Name = ""
            Common_Procedures.Master_Return.Control_Name = ""
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            If FrmLdSTS = True Then
                lbl_Company.Text = ""
                lbl_Company.Tag = 0
                Common_Procedures.CompIdNo = 0

                Me.Text = ""

                lbl_Company.Text = Common_Procedures.get_Company_From_CompanySelection(con)
                lbl_Company.Tag = Val(Common_Procedures.CompIdNo)

                Me.Text = lbl_Company.Text

                new_record()

            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Opening_Chemical_Stock_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Opening_Chemical_Stock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                lbl_Company.Tag = 0
                lbl_Company.Text = ""
                Me.Text = ""
                Common_Procedures.CompIdNo = 0


                lbl_Company.Text = Common_Procedures.Show_CompanySelection_On_FormClose(con)
                lbl_Company.Tag = Val(Common_Procedures.CompIdNo)
                Me.Text = lbl_Company.Text
                If Val(Common_Procedures.CompIdNo) = 0 Then

                    Me.Close()

                Else

                    new_record()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Opening_Chemical_Stock_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable

        Me.Text = ""
        Me.BackColor = Color.LightSkyBlue
        pnl_Back.BackColor = Me.BackColor

        con.Open()

        Da = New SqlClient.SqlDataAdapter("select item_name from Sizing_Item_Head order by item_name", con)
        Da.Fill(Dt1)
        cbo_ItemName.DataSource = Dt1
        cbo_ItemName.DisplayMember = "item_name"

        Da = New SqlClient.SqlDataAdapter("select unit_name from unit_head order by unit_name", con)
        Da.Fill(Dt2)
        cbo_Unit.DataSource = Dt2
        cbo_Unit.DisplayMember = "unit_name"

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        FrmLdSTS = True
        new_record()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Itm_ID As Integer
        Dim Nr As Integer

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If
    
        Try

            Itm_ID = Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)

            cmd.Connection = con

            OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
            OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Val(Itm_ID)) & "/" & Trim(OpYrCode)

            cmd.CommandText = "Delete from Stock_Chemical_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            Nr = cmd.ExecuteNonQuery()

            If Nr = 0 Then
                MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else

                new_record()

                MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If cbo_ItemName.Enabled = True And cbo_ItemName.Visible = True Then cbo_ItemName.Focus()
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '----
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '----
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        '----
    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        '----
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        '----
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        '----
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record

        Try
            clear()

            If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        '----
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '----
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim nr As Long = 0
        Dim Itm_ID As Integer = 0
        Dim Unt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim OpDate As Date
        Dim OpYrCode As String

        Itm_ID = Common_Procedures.Sizing_Item_NameToIdNo(con, cbo_ItemName.Text)
        If Itm_ID = 0 Then
            MessageBox.Show("Invalid Item Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_ItemName.Enabled Then cbo_ItemName.Focus()
            Exit Sub
        End If

        Unt_ID = Common_Procedures.Unit_NameToIdNo(con, cbo_Unit.Text)

        tr = con.BeginTransaction

        Try

            OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
            OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Val(Itm_ID)) & "/" & Trim(OpYrCode)

            OpDate = CDate("01-04-" & Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4))
            OpDate = DateAdd(DateInterval.Day, -1, OpDate)

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@OpeningDate", OpDate.Date)

            cmd.CommandText = "Delete from Stock_Chemical_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            Sno = 1
            cmd.CommandText = "Insert into Stock_Chemical_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, Ledger_IdNo, Party_Bill_No, SL_No, Item_IdNo , Quantity) " & _
                                " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(Itm_ID) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(Itm_ID))) & ", @OpeningDate, 0, '', " & Str(Val(Sno)) & ", " & Str(Val(Itm_ID)) & " , " & Str(Val(txt_OpStock.Text)) & " )"
            cmd.ExecuteNonQuery()

            tr.Commit()

            move_record(Itm_ID)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)


        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If cbo_ItemName.Enabled And cbo_ItemName.Visible Then cbo_ItemName.Focus()

    End Sub

    Private Sub cbo_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.GotFocus
        With cbo_ItemName
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = cbo_ItemName.Text.Length

            cmbItmNm = Trim(cbo_ItemName.Text)
        End With

    End Sub

    Private Sub cbo_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyDown
        Try
            With cbo_ItemName
                If e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    txt_OpStock.Focus()
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Itm_ID As Integer = 0
        Dim Condt As String
        Dim FindStr As String

        Try

            With cbo_ItemName

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        With cbo_ItemName
                            If Trim(.Text) <> "" Then
                                If .DroppedDown = True Then
                                    If Trim(.SelectedText) <> "" Then
                                        .Text = .SelectedText
                                    Else
                                        If .Items.Count > 0 Then
                                            .SelectedIndex = 0
                                            .SelectedItem = .Items(0)
                                            .Text = .GetItemText(.SelectedItem)
                                        End If
                                    End If
                                End If
                            End If
                        End With

                        If Trim(UCase(cmbItmNm)) <> Trim(UCase(cbo_ItemName.Text)) Then

                            Itm_ID = Common_Procedures.Item_NameToIdNo(con, cbo_ItemName.Text)

                            move_record(Val(Itm_ID))

                            If Trim(cbo_Unit.Text) = "" Then
                                da = New SqlClient.SqlDataAdapter("select b.unit_name from Sizing_Item_Head a, unit_head b where a.item_name = '" & Trim(cbo_ItemName.Text) & "' and a.unit_idno = b.unit_idno", con)
                                dt = New DataTable
                                da.Fill(dt)
                                If dt.Rows.Count > 0 Then
                                    cbo_Unit.Text = dt.Rows(0)("unit_name").ToString
                                End If
                                dt.Dispose()
                                da.Dispose()
                            End If

                            If txt_OpStock.Enabled Then txt_OpStock.Focus()

                        End If
                        txt_OpStock.Focus()

                    Else

                        Condt = ""
                        FindStr = ""

                        If Asc(e.KeyChar) = 8 Then
                            If .SelectionStart <= 1 Then
                                .Text = ""
                            End If

                            If Trim(.Text) <> "" Then
                                If .SelectionLength = 0 Then
                                    FindStr = .Text.Substring(0, .Text.Length - 1)
                                Else
                                    FindStr = .Text.Substring(0, .SelectionStart - 1)
                                End If
                            End If

                        Else
                            If .SelectionLength = 0 Then
                                FindStr = .Text & e.KeyChar
                            Else
                                FindStr = .Text.Substring(0, .SelectionStart) & e.KeyChar
                            End If

                        End If

                        FindStr = LTrim(FindStr)

                        If Trim(FindStr) <> "" Then
                            Condt = " Where item_name like '" & Trim(FindStr) & "%' or item_name like '% " & Trim(FindStr) & "%' "
                        End If

                        da = New SqlClient.SqlDataAdapter("select item_name from Sizing_Item_Head " & Condt & " order by item_name", con)
                        dt = New DataTable
                        da.Fill(dt)

                        .DataSource = dt
                        .DisplayMember = "item_name"

                        .Text = FindStr

                        .SelectionStart = FindStr.Length

                        e.Handled = True

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_ItemName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Sizing_Item_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ItemName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_ItemName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.LostFocus
        cmbItmNm = Trim(cbo_ItemName.Text)
        cbo_ItemName.BackColor = Color.White
        cbo_ItemName.ForeColor = Color.Black
    End Sub

    Private Sub txt_OpStock_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_OpStock.KeyDown
        'If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then cbo_ItemName.Focus()
    End Sub

    Private Sub txt_OpStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_OpStock.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) Then
                save_record()
            End If
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        con.Close()
    End Sub
End Class