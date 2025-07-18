Public Class LoomNo_Creation
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False
    Private vcbo_KeyDwnVal As Double
    Private Sub clear()

        New_Entry = False

        pnl_Back.Enabled = True
        grp_Find.Visible = False
        grp_Filter.Visible = False

        Me.Height = 360  ' 284

        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black

        txt_orderBy.Text = ""
        txt_Name.Text = ""
        cbo_LoomType.Text = ""
        txt_NoofBeams.Text = "2"
        txt_ProdMtrs_Day.Text = ""
        cbo_Find.Text = ""
        cbo_CompanyShort_Name.Text = ""
        Cbo_Quality_Name.Text = ""


        Chk_Multi_EndsCount.Checked = False
        'dgv_Filter.Rows.Clear()

    End Sub

    Public Sub move_record(ByVal idno As Integer)
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader

        If Val(idno) = 0 Then Exit Sub

        clear()

        Try
            cmd.Connection = con
            cmd.CommandText = "select a.*,b.LoomType_Name, tZ.Company_SHortName ,ch.Cloth_name from Loom_Head a LEFT OUTER JOIN LoomType_Head b ON a.LoomType_IdNo = b.LoomType_IdNo LEFT outer JOIn COmpany_Head tZ on tZ.COmpany_idno = a.Loom_CompanyIdno LEFT OUTER JOIN cloth_head Ch on a.Cloth_Idno = Ch.Cloth_Idno where a.Loom_IdNo = " & Str(Val(idno))

            dr = cmd.ExecuteReader

            If dr.HasRows Then
                If dr.Read() Then
                    lbl_IdNo.Text = dr("Loom_IdNo").ToString()
                    txt_Name.Text = dr("Loom_Name").ToString()
                    cbo_LoomType.Text = dr("LoomType_Name").ToString()
                    txt_NoofBeams.Text = dr("Noof_Input_Beams").ToString()
                    txt_ProdMtrs_Day.Text = dr("Lomm_Production_Capacity_Day").ToString()
                    cbo_CompanyShort_Name.Text = dr("Company_SHortName").ToString()
                    txt_orderBy.Text = dr("LmNo_OrderBy").ToString()
                    If Val(dr("Multiple_EndsCount_Selection_Status").ToString) = 1 Then Chk_Multi_EndsCount.Checked = True

                    Cbo_Quality_Name.Text = dr("Cloth_name").ToString()


                End If
            End If

            dr.Close()

            cmd.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            cmd.Dispose()

            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        End Try

    End Sub

    Private Sub LoomNo_Creation_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_LoomType.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LOOMTYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_LoomType.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            Common_Procedures.Master_Return.Form_Name = ""
            Common_Procedures.Master_Return.Control_Name = ""
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""


            new_record()


        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try



    End Sub

    Private Sub LoomNo_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Find.Visible Then
                btnClose_Click(sender, e)
            ElseIf grp_Filter.Visible Then
                btn_CloseFilter_Click(sender, e)
            Else
                Me.Close()
            End If
        End If
    End Sub

    Private Sub LoomNo_Creation_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Height = 360  ' 284 ' 197
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        con.Open()

        Chk_Multi_EndsCount.Visible = False
        Cbo_Quality_Name.Visible = False
        lbl_Quality_name.Visible = False


        da = New SqlClient.SqlDataAdapter("select LoomType_Name from LoomType_Head order by LoomType_Name", con)
        da.Fill(dt1)
        cbo_LoomType.DataSource = dt1
        cbo_LoomType.DisplayMember = "LoomType_Name"

        If Trim(Common_Procedures.settings.CustomerCode) = "1155" Or Trim(Common_Procedures.settings.CustomerCode) = "1608" Then
            Label7.Visible = True
            cbo_CompanyShort_Name.Visible = True
        Else
            Label7.Visible = False
            cbo_CompanyShort_Name.Visible = False
        End If

        If Trim(Common_Procedures.settings.CustomerCode) = "1414" Then ' ---  mahalakshmi textile

            Cbo_Quality_Name.Visible = True
            lbl_Quality_name.Visible = True
            Cbo_Quality_Name.BackColor = Color.White

            lbl_Quality_name.Left = Label7.Left
            Cbo_Quality_Name.Left = cbo_CompanyShort_Name.Left
            Cbo_Quality_Name.Width = cbo_CompanyShort_Name.Width

        End If


        If Common_Procedures.settings.Cloth_WarpConsumption_Multiple_EndsCount_Status = 1 Then
            Chk_Multi_EndsCount.Visible = True
        End If

        new_record()

    End Sub

    Private Sub LoomNo_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Loom_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Loom_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Loom_Creation, New_Entry, Me) = False Then Exit Sub



        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        Try

            da = New SqlClient.SqlDataAdapter("select count(*) from Beam_Knotting_Head where Loom_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Loom", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_SizedPavu_Processing_Details where Loom_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Loom", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If
            dt.Dispose()
            da.Dispose()

            da = New SqlClient.SqlDataAdapter("select count(*) from Weaver_Cloth_Receipt_Head where Loom_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Loom", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If
            dt.Dispose()
            da.Dispose()

            cmd.Connection = con
            cmd.CommandText = "delete from Loom_Head where Loom_IdNo = " & Str(Val(lbl_IdNo.Text))

            cmd.ExecuteNonQuery()


            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Successfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim da As New SqlClient.SqlDataAdapter("select Loom_IdNo, Loom_Name from Loom_Head where Loom_IdNo <> 0 order by Loom_IdNo", con)
        Dim dt As New DataTable

        da.Fill(dt)

        With dgv_Filter

            .Columns.Clear()
            .DataSource = dt

            .RowHeadersVisible = False

            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            .Columns(0).HeaderText = "IDNO"
            .Columns(1).HeaderText = "LOOM NO"

            .Columns(0).FillWeight = 40
            .Columns(1).FillWeight = 160

        End With

        new_record()

        grp_Filter.Visible = True
        grp_Filter.Left = grp_Find.Left
        grp_Filter.Top = grp_Find.Top

        pnl_Back.Enabled = False

        If dgv_Filter.Enabled And dgv_Filter.Visible Then dgv_Filter.Focus()

        Me.Height = 560 ' 400

        dt.Dispose()
        da.Dispose()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(Loom_IdNo) from Loom_Head Where Loom_IdNo <> 0", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movid) <> 0 Then move_record(movid)

            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movid As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select max(Loom_IdNo) from Loom_Head WHERE Loom_IdNo <> 0"

            dr = cmd.ExecuteReader

            movid = 0
            If dr.HasRows Then
                If dr.Read() Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movid = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE....", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movid As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select min(Loom_IdNo) from Loom_Head where Loom_IdNo > " & Str(Val(lbl_IdNo.Text))

            dr = cmd.ExecuteReader()

            movid = 0
            If dr.HasRows Then
                If dr.Read() Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movid = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        Try
            da = New SqlClient.SqlDataAdapter("select max(Loom_IdNo) from Loom_Head where Loom_IdNo < " & Str(Val(lbl_IdNo.Text)) & " and Loom_IdNo <> 0 ", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Dispose()
            da.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable





        Try

            clear()

            New_Entry = True

            lbl_IdNo.ForeColor = Color.Red

            lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Loom_Head", "Loom_IdNo", "")

            If Val(lbl_IdNo.Text) <= 20 Then lbl_IdNo.Text = 21

            Da1 = New SqlClient.SqlDataAdapter("select Top 1 a.*, b.LoomType_Name from Loom_Head a LEFT OUTER JOIN LoomType_Head b ON a.LoomType_IdNo = b.LoomType_IdNo Where a.Loom_IdNo <> 0 Order by a.Loom_IdNo desc", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If Dt1.Rows(0).Item("LoomType_Name").ToString <> "" Then cbo_LoomType.Text = Dt1.Rows(0).Item("LoomType_Name").ToString
                If Dt1.Rows(0).Item("Noof_Input_Beams").ToString <> "" Then txt_NoofBeams.Text = Val(Dt1.Rows(0).Item("Noof_Input_Beams").ToString)
                If Dt1.Rows(0).Item("Lomm_Production_Capacity_Day").ToString <> "" Then txt_ProdMtrs_Day.Text = Val(Dt1.Rows(0).Item("Lomm_Production_Capacity_Day").ToString)
            End If
            Dt1.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da1.Dispose()

            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
        End Try



    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter("select Loom_Name from Loom_Head order by Loom_Name", con)
        Dim dt As New DataTable

        da.Fill(dt)

        cbo_Find.DataSource = dt
        cbo_Find.DisplayMember = "Loom_Name"

        new_record()

        grp_Find.Visible = True
        pnl_Back.Enabled = False

        If cbo_Find.Enabled And cbo_Find.Visible Then cbo_Find.Focus()
        Me.Height = 530  ' 480 ' 355

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        'MessageBox.Show("insert record")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '--- No Printing
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Sur As String
        Dim LomTy_ID As Integer = 0
        Dim Comp_Id As Integer = 0
        Dim vMULTIENDSCNTSTS As Integer
        Dim Quality_ID = 0

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Loom_Creation, New_Entry) = False Then Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Loom_Creation, New_Entry, Me) = False Then Exit Sub

        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid Name", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        Sur = Common_Procedures.Remove_NonCharacters(Trim(txt_Name.Text))

        Da = New SqlClient.SqlDataAdapter("select * from Loom_Head where Loom_IdNo <> " & Str(Val(lbl_IdNo.Text)) & " and Sur_Name = '" & Trim(Sur) & "'", con)
        Dt = New DataTable
        Da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            MessageBox.Show("Duplicate Loom No.", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        Dt.Clear()

        If Val(txt_NoofBeams.Text) = 0 Then
            txt_NoofBeams.Text = 2
        End If

        If Val(txt_ProdMtrs_Day.Text) = 0 Then
            txt_ProdMtrs_Day.Text = 100
        End If
        LomTy_ID = Common_Procedures.LoomType_NameToIdNo(con, cbo_LoomType.Text)

        Comp_Id = Common_Procedures.Company_ShortNameToIdNo(con, cbo_CompanyShort_Name.Text)

        vMULTIENDSCNTSTS = 0
        If Chk_Multi_EndsCount.Checked = True Then vMULTIENDSCNTSTS = 1

        If Val(txt_orderBy.Text) = 0 Then txt_orderBy.Text = lbl_IdNo.Text

        If Cbo_Quality_Name.Visible And Cbo_Quality_Name.Enabled Then

            Quality_ID = Common_Procedures.Cloth_NameToIdNo(con, Cbo_Quality_Name.Text)

            If Quality_ID = 0 Then
                MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If Cbo_Quality_Name.Visible Then Cbo_Quality_Name.Focus()
                Exit Sub
            End If

        End If


        trans = con.BeginTransaction

        Try
            cmd.Connection = con
            cmd.Transaction = trans

            If New_Entry = True Then

                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Loom_Head", "Loom_IdNo", "", trans)
                If Val(lbl_IdNo.Text) <= 20 Then lbl_IdNo.Text = 21

                cmd.CommandText = "Insert into Loom_Head(Loom_IdNo, Loom_Name, sur_name,loomType_IdNo, Noof_Input_Beams, Lomm_Production_Capacity_Day, Loom_CompanyIdno , LmNo_OrderBy, Beam_Knotting_Code, Multiple_EndsCount_Selection_Status ,Cloth_Idno ) values (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(txt_Name.Text) & "', '" & Trim(Sur) & "'," & Val(LomTy_ID) & ", " & Str(Val(txt_NoofBeams.Text)) & ", " & Str(Val(txt_ProdMtrs_Day.Text)) & " , " & Val(Comp_Id) & " , " & Str(Val(txt_orderBy.Text)) & " , '' ,  " & Str(Val(vMULTIENDSCNTSTS)) & " ,  " & Str(Val(Quality_ID)) & "  )"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "update Loom_Head set Loom_Name = '" & Trim(txt_Name.Text) & "', sur_name = '" & Trim(Sur) & "',LoomType_IdNo = " & Val(LomTy_ID) & " , Noof_Input_Beams = " & Str(Val(txt_NoofBeams.Text)) & ", Lomm_Production_Capacity_Day = " & Str(Val(txt_ProdMtrs_Day.Text)) & " , Loom_CompanyIdno = " & Val(Comp_Id) & " , LmNo_OrderBy = " & Str(Val(txt_orderBy.Text)) & " , Multiple_EndsCount_Selection_Status = " & Str(Val(vMULTIENDSCNTSTS)) & " ,Cloth_Idno =  " & Str(Val(Quality_ID)) & "  where Loom_IdNo = " & Str(Val(lbl_IdNo.Text))
                cmd.ExecuteNonQuery()

            End If

            trans.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(txt_Name.Text)
            Common_Procedures.Master_Return.Master_Type = "LOOMNO"



            MessageBox.Show("Saved Successfully!!!", "FOR SAVING....", MessageBoxButtons.OK, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_IdNo.Text)
                End If
            Else
                move_record(lbl_IdNo.Text)
            End If


        Catch ex As Exception
            trans.Rollback()

            If InStr(1, Trim(LCase(ex.Message)), "ix_loom_head") > 0 Then
                MessageBox.Show("Duplicate Loom No", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Finally
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        End Try

    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btn_Find_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Find.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        da = New SqlClient.SqlDataAdapter("select Loom_IdNo from Loom_Head where Loom_Name = '" & Trim(cbo_Find.Text) & "'", con)
        da.Fill(dt)

        movid = 0
        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                movid = Val(dt.Rows(0)(0).ToString)
            End If
        End If

        dt.Dispose()
        da.Dispose()

        If movid <> 0 Then
            move_record(movid)
        Else
            new_record()
        End If

        btnClose_Click(sender, e)

    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Height = 360  ' 284 ' 197
        pnl_Back.Enabled = True
        grp_Find.Visible = False
        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
    End Sub

    Private Sub cbo_Find_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Find.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")
    End Sub

    Private Sub cbo_Find_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Find.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Find, Nothing, Nothing, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Find_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Find.KeyPress

        Try

            With cbo_Find

                If Asc(e.KeyChar) <> 27 Then

                    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Find, Nothing, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")

                    If Asc(e.KeyChar) = 13 Then

                        btn_Find_Click(sender, e)

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_LoomType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LoomType.GotFocus
        With cbo_LoomType
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectAll()
        End With
        'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "LoomType_Head", "LoomType_Name", "", "(LoomType_IdNo = 0)")
    End Sub
    Private Sub cbo_colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LoomType.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LoomType, txt_Name, txt_NoofBeams, "LoomType_Head", "LoomType_Name", "", "(LoomType_IdNo = 0)")

    End Sub

    Private Sub cbo_LoomType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_LoomType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LoomType, txt_NoofBeams, "loomType_Head", "LoomType_Name", "", "(loomType_IdNo = 0)")

    End Sub

    Private Sub cbo_LoomType_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LoomType.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New LoomType_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_LoomType.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub btn_CloseFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseFilter.Click

        pnl_Back.Enabled = True
        grp_Filter.Visible = False
        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        Me.Height = 360  ' 284 '197

    End Sub

    Private Sub btn_Open_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Open.Click
        Dim movid As Integer

        movid = 0
        If IsDBNull((dgv_Filter.CurrentRow.Cells(0).Value)) = False Then
            movid = Val(dgv_Filter.CurrentRow.Cells(0).Value)
        End If

        If Val(movid) <> 0 Then
            move_record(movid)
            btn_CloseFilter_Click(sender, e)
        End If

    End Sub

    Private Sub dgv_Filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Filter.DoubleClick
        btn_Open_Click(sender, e)
    End Sub

    Private Sub dgv_Filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
        If e.KeyCode = Keys.Enter Then
            btn_Open_Click(sender, e)
        End If
    End Sub

    Private Sub txt_Name_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Name.GotFocus
        With txt_Name
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectAll()
        End With
    End Sub

    Private Sub txt_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Name.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then txt_ProdMtrs_Day.Focus() ' SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Name.KeyPress
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_NoofBeams_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_NoofBeams.GotFocus
        With txt_NoofBeams
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectAll()
        End With
    End Sub

    Private Sub txt_NoofBeams_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_NoofBeams.LostFocus
        With txt_NoofBeams
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub txt_Name_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Name.LostFocus
        With txt_Name
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub txt_NoofBeams_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_NoofBeams.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_NoofBeams_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_NoofBeams.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_ProdMtrs_Day_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_ProdMtrs_Day.GotFocus
        With txt_ProdMtrs_Day
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectAll()
        End With
    End Sub

    Private Sub txt_ProdMtrs_Day_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_ProdMtrs_Day.LostFocus
        With txt_ProdMtrs_Day
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub txt_ProdMtrs_Day_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_ProdMtrs_Day.KeyDown
        If e.KeyCode = 40 Then btn_Save.Focus() ' SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_ProdMtrs_Day_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ProdMtrs_Day.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If cbo_CompanyShort_Name.Visible Then
                cbo_CompanyShort_Name.Focus()
            ElseIf Cbo_Quality_Name.Visible Then
                Cbo_Quality_Name.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    txt_Name.Focus()
                End If
            End If

        End If
    End Sub

    Private Sub cbo_LoomType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LoomType.LostFocus
        With cbo_LoomType
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub cbo_CompanyShort_Name_GotFocus(sender As Object, e As EventArgs) Handles cbo_CompanyShort_Name.GotFocus
        With cbo_CompanyShort_Name
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectAll()
        End With
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "company_Head", "Company_ShortName", "", "(Company_IdNo = 0)")
    End Sub

    Private Sub cbo_CompanyShort_Name_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_CompanyShort_Name.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_CompanyShort_Name, txt_NoofBeams, btn_Save, "company_Head", "Company_ShortName", "", "(Company_IdNo = 0)")
    End Sub

    Private Sub cbo_CompanyShort_Name_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_CompanyShort_Name.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_CompanyShort_Name, Nothing, "company_Head", "Company_ShortName", "", "(Company_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If

        End If
    End Sub

    Private Sub cbo_CompanyShort_Name_LostFocus(sender As Object, e As EventArgs) Handles cbo_CompanyShort_Name.LostFocus
        With cbo_CompanyShort_Name
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub txt_orderBy_GotFocus(sender As Object, e As EventArgs) Handles txt_orderBy.GotFocus
        With txt_orderBy
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectAll()
        End With
    End Sub

    Private Sub txt_orderBy_LostFocus(sender As Object, e As EventArgs) Handles txt_orderBy.LostFocus
        With txt_orderBy
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub txt_orderBy_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_orderBy.KeyDown
        If e.KeyCode = 38 Then
            txt_ProdMtrs_Day.Focus()
        End If

        If e.KeyCode = 40 Then
            txt_Name.Focus()
        End If

    End Sub

    Private Sub txt_orderBy_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_orderBy.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Name.Focus()
        End If
    End Sub
    Private Sub Cbo_Quality_Name_GotFocus(sender As Object, e As EventArgs) Handles Cbo_Quality_Name.GotFocus
        With Cbo_Quality_Name
            .BackColor = Color.Lime
            .ForeColor = Color.Blue
            .SelectAll()
        End With
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_head", "Cloth_Name", "(Close_Status=0)", "(Cloth_Idno = 0)")
    End Sub

    Private Sub Cbo_Quality_Name_KeyDown(sender As Object, e As KeyEventArgs) Handles Cbo_Quality_Name.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Quality_Name, txt_NoofBeams, btn_Save, "Cloth_head", "Cloth_Name", "(Close_Status=0)", "(Cloth_Idno = 0)")
    End Sub

    Private Sub Cbo_Quality_Name_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Cbo_Quality_Name.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Quality_Name, Nothing, "Cloth_head", "Cloth_Name", "(Close_Status=0)", "(Cloth_Idno = 0)")

        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If

        End If
    End Sub

    Private Sub Cbo_Quality_Name_Name(sender As Object, e As EventArgs) Handles Cbo_Quality_Name.LostFocus
        With Cbo_Quality_Name
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub
End Class