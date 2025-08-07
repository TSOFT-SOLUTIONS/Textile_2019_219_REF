Imports System.IO

Public Class Company_Creation
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Dim cbo As ComboBox
    Private CompType_Condt As String
    Private Prec_ActCtrl As New Control
    Private TrnTo_DbName As String = ""
    Private vcbo_KeyDwnVal As Double

    Private Sub clear()
        pnl_Factory_Address.Visible = False
        Dim obj As Object

        For Each obj In Me.Controls
            If TypeOf obj Is TextBox Or TypeOf obj Is ComboBox Then
                obj.text = ""
            End If
        Next
        chk_Close_Status.Checked = False

        chk_Textile_Sts.Checked = False
        Chk_Sizing_Sts.Checked = False
        Chk_OE_Sts.Checked = False

        txt_Factory_Address1.Text = ""
        txt_Factory_Address2.Text = ""
        txt_Factory_Address3.Text = ""
        txt_Factory_Address4.Text = ""
        Pic_Company_logo_Image.BackgroundImage = Nothing
        txt_LegalName_Business.Text = ""
        txt_City.Text = ""
        txt_PinCode.Text = ""

        cbo_Sizing_Ledger_Name.Text = ""
        grp_Open.Visible = False
        lbl_CompID.ForeColor = Color.Black
        cbo_CompanyType.Text = "ACCOUNT"


        Pnl_Sizing_Address.Visible = False
        txt_TamilName.Text = ""
        Txt_TamilAddress_1.Text = ""
        Txt_TamilAddress_2.Text = ""
        txt_Siz_Address1.Text = ""
        txt_Siz_Address2.Text = ""
        txt_Siz_Address3.Text = ""
        txt_Siz_Address4.Text = ""
        txt_Siz_City.Text = ""
        txt_Siz_Pincode.Text = ""
        txt_Siz_PhoneNo.Text = ""
        txt_Siz_Email.Text = ""

        txt_UAM_No.Text = ""
        txt_Jurisdiction.Text = ""

        Chk_Tcs_sts.Checked = False ' True
        cbo_State.Text = Common_Procedures.State_IdNoToName(con, 32)

    End Sub
    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim mskdtxbx As MaskedTextBox
        Dim combobx As ComboBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is CheckBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            mskdtxbx = Me.ActiveControl
            mskdtxbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If



        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Me.ActiveControl Is CheckBox Then
                Prec_ActCtrl.BackColor = Color.LightSkyBlue
                Prec_ActCtrl.ForeColor = Color.Blue
            ElseIf TypeOf Me.ActiveControl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(41, 57, 85)
                Prec_ActCtrl.ForeColor = Color.White
            End If
        End If

    End Sub
    'Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    On Error Resume Next
    '    If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    '    If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    'End Sub

    'Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    On Error Resume Next
    '    If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    'End Sub
    Public Sub move_record(ByVal IdNo As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        If Val(IdNo) = 0 Then Exit Sub

        Call clear()

        Try

            da = New SqlClient.SqlDataAdapter("select * from Company_Head Where " & CompType_Condt & IIf(Trim(CompType_Condt) <> "", " and ", "") & " Company_IdNo = " & Str(Val(IdNo)), con)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                lbl_CompID.Text = dt.Rows(0)("Company_IdNo").ToString
                txt_CompanyName.Text = dt.Rows(0)("Company_Name").ToString
                txt_ShortName.Text = dt.Rows(0)("Company_ShortName").ToString
                cbo_CompanyType.Text = dt.Rows(0)("Company_Type").ToString
                txt_ContactName.Text = dt.Rows(0)("Company_ContactPerson").ToString
                txt_Address1.Text = dt.Rows(0)("Company_Address1").ToString
                txt_Address2.Text = dt.Rows(0)("Company_Address2").ToString
                txt_Address3.Text = dt.Rows(0)("Company_Address3").ToString
                txt_Address4.Text = dt.Rows(0)("Company_Address4").ToString
                txt_City.Text = dt.Rows(0)("Company_City").ToString
                txt_PinCode.Text = dt.Rows(0)("Company_PinCode").ToString
                txt_PhoneNo.Text = dt.Rows(0)("Company_PhoneNo").ToString
                txt_FaxNo.Text = dt.Rows(0)("Company_FaxNo").ToString
                txt_TinNo.Text = dt.Rows(0)("Company_TinNo").ToString
                txt_CstNo.Text = dt.Rows(0)("Company_CstNo").ToString
                txt_PanNo.Text = dt.Rows(0)("Company_PanNo").ToString
                txt_EMail.Text = dt.Rows(0)("Company_EMail").ToString
                txt_Bank_Ac_Details.Text = dt.Rows(0)("Company_Bank_Ac_Details").ToString
                txt_Description.Text = dt.Rows(0)("Company_Description").ToString
                txt_Factory_Address1.Text = dt.Rows(0)("Company_Factory_Address1").ToString
                txt_Factory_Address2.Text = dt.Rows(0)("Company_Factory_Address2").ToString
                txt_Factory_Address3.Text = dt.Rows(0)("Company_Factory_Address3").ToString
                txt_Factory_Address4.Text = dt.Rows(0)("Company_Factory_Address4").ToString

                txt_UAM_No.Text = dt.Rows(0)("Company_UAM_No").ToString
                txt_Jurisdiction.Text = dt.Rows(0)("Jurisdiction").ToString

                If Val(dt.Rows(0).Item("Close_Status").ToString) = 1 Then chk_Close_Status.Checked = True
                Chk_Tcs_sts.Checked = False
                If Val(dt.Rows(0).Item("TCS_Company_Status").ToString) = 1 Then Chk_Tcs_sts.Checked = True

                If Val(dt.Rows(0).Item("Tex_Module_Status").ToString) = 1 Then chk_Textile_Sts.Checked = True
                If Val(dt.Rows(0).Item("Siz_Module_Status").ToString) = 1 Then Chk_Sizing_Sts.Checked = True
                If Val(dt.Rows(0).Item("OE_Module_Status").ToString) = 1 Then Chk_OE_Sts.Checked = True

                '-----------------GST ALTER------------------------------------
                txt_GSTIN_No.Text = dt.Rows(0)("Company_GSTinNo").ToString
                txt_CIN_No.Text = dt.Rows(0)("Company_CinNo").ToString
                txt_Website.Text = dt.Rows(0)("Company_Website").ToString
                cbo_Company_Designation.Text = dt.Rows(0)("Company_Owner_Designation").ToString
                cbo_State.Text = Common_Procedures.State_IdNoToName(con, Val(dt.Rows(0)("Company_State_IdNo").ToString))
                '---------------------------------------------------------------

                cbo_Sizing_Ledger_Name.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt.Rows(0).Item("Sizing_To_LedgerIdNo").ToString), , TrnTo_DbName)
                If txt_CompanyName.Enabled And txt_CompanyName.Visible Then txt_CompanyName.Focus()

                txt_LegalName_Business.Text = dt.Rows(0)("Legal_Nameof_Business").ToString

                txt_Siz_Address1.Text = dt.Rows(0)("Sizing_Address1").ToString
                txt_Siz_Address2.Text = dt.Rows(0)("Sizing_Address2").ToString
                txt_Siz_Address3.Text = dt.Rows(0)("Sizing_Address3").ToString
                txt_Siz_Address4.Text = dt.Rows(0)("Sizing_Address4").ToString
                txt_Siz_City.Text = dt.Rows(0)("Sizing_City").ToString
                txt_Siz_Pincode.Text = dt.Rows(0)("Sizing_PinCode").ToString
                txt_Siz_PhoneNo.Text = dt.Rows(0)("Sizing_PhoneNo").ToString
                txt_Siz_Email.Text = dt.Rows(0)("Sizing_EMail").ToString


                txt_TamilName.Text = dt.Rows(0)("Company_Tamil_Name").ToString
                Txt_TamilAddress_1.Text = dt.Rows(0)("Company_Tamil_Address1").ToString
                Txt_TamilAddress_2.Text = dt.Rows(0)("Company_Tamil_Address2").ToString




                If IsDBNull(dt.Rows(0).Item("Company_logo_Image")) = False Then
                    Dim imageData As Byte() = DirectCast(dt.Rows(0).Item("Company_logo_Image"), Byte())
                    If Not imageData Is Nothing Then
                        Using ms As New MemoryStream(imageData, 0, imageData.Length)
                            ms.Write(imageData, 0, imageData.Length)
                            If imageData.Length > 0 Then

                                Pic_Company_logo_Image.BackgroundImage = Image.FromStream(ms)

                            End If
                        End Using
                    End If
                End If


                'if you know the common name


                ' Pic_Colour.BackColor = Color.FromArgb(dt.Rows(0).Item("Company_Head_COlour_Image").ToString)
            End If



        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORD", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub

        End Try

        dt.Clear()

        dt.Dispose()
        da.Dispose()

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim cmd As New SqlClient.SqlCommand
        Dim new_idno As Integer

        clear()
        Chk_Tcs_sts.Checked = True
        Try
            cmd.Connection = con
            cmd.CommandText = "select max(company_idno) from company_head"

            new_idno = Val(cmd.ExecuteScalar())

        Catch ex As Exception
            new_idno = 0

        End Try

        cmd.Dispose()

        lbl_CompID.Text = new_idno + 1

        lbl_CompID.ForeColor = Color.Red

        If txt_CompanyName.Enabled And txt_CompanyName.Visible Then txt_CompanyName.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable
        Dim pwd As String = ""

        Common_Procedures.Password_Input = ""
        Dim g As New Password
        g.ShowDialog()

        If Trim(UCase(Common_Procedures.Password_Input)) <> "TSDCOMP" Then
            MessageBox.Show("Invalid Password", "DOES NOT DELETE COMPANY...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If grp_Open.Visible = True Then
            MessageBox.Show("Close Other Windows", "DOES NOT DELETE COMPANY...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to delete", "FOR DELETION..", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If lbl_CompID.ForeColor = Color.Red Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE COMPANY...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try

            da = New SqlClient.SqlDataAdapter("select count(*) from Voucher_Details where Company_Idno = " & Str(Val(lbl_CompID.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this company", "DOES NOT DELETE COMPANY...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_Yarn_Processing_Details where Company_Idno = " & Str(Val(lbl_CompID.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this company", "DOES NOT DELETE COMPANY...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_SizedPavu_Processing_Details where Company_Idno = " & Str(Val(lbl_CompID.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this company", "DOES NOT DELETE COMPANY...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_Pavu_Processing_Details where Company_Idno = " & Str(Val(lbl_CompID.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this company", "DOES NOT DELETE COMPANY...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_Empty_BeamBagCone_Processing_Details where Company_Idno = " & Str(Val(lbl_CompID.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this company", "DOES NOT DELETE COMPANY...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_Cloth_Processing_Details where Company_Idno = " & Str(Val(lbl_CompID.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this company", "DOES NOT DELETE COMPANY...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            cmd.Connection = con
            cmd.CommandText = "delete from company_head where company_idno = " & Str(Val(lbl_CompID.Text))
            cmd.ExecuteNonQuery()

            new_record()

            MessageBox.Show("Deleted Successfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE COMPANY", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("Select Company_IdNo, Company_Name from Company_Head where " & CompType_Condt & IIf(Trim(CompType_Condt) <> "", " and ", "") & " Company_IdNo <> 0 Order by Company_IdNo", con)

        da.Fill(dt)

        With dgv_Filter

            .Columns.Clear()
            '.Rows.Clear()

            .DataSource = dt
            .RowHeadersVisible = False

            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            '.AlternatingRowsDefaultCellStyle.BackColor = Color.Aquamarine

            .Columns(0).HeaderText = "IDNO"
            .Columns(1).HeaderText = "COMPANY NAME"

            .Columns(0).FillWeight = 35
            .Columns(1).FillWeight = 165

            grp_Filter.Visible = True

            .Focus()

        End With

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer = 0

        Try

            cmd.Connection = con
            cmd.CommandText = "select min(company_idno) from company_head where  " & CompType_Condt & IIf(Trim(CompType_Condt) <> "", " and ", "") & " Company_IdNo <> 0"
            cmd.ExecuteNonQuery()

            movid = 0
            If IsDBNull(cmd.ExecuteScalar) = False Then
                movid = cmd.ExecuteScalar
            End If

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select max(company_idno ) from company_head Where " & CompType_Condt & IIf(Trim(CompType_Condt) <> "", " and ", "") & " company_idno <> 0"

            movid = 0
            If IsDBNull(cmd.ExecuteScalar) = False Then
                movid = cmd.ExecuteScalar
            End If

            If movid <> 0 Then
                move_record(movid)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer = 0

        Try
            cmd.Connection = con
            cmd.CommandText = "select min(company_idno) from company_head where  " & CompType_Condt & IIf(Trim(CompType_Condt) <> "", " and ", "") & " company_idno > " & Str(Val(lbl_CompID.Text))

            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar
            End If

            If movid > 0 Then
                move_record(movid)
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select max(company_idno) from company_head where  " & CompType_Condt & IIf(Trim(CompType_Condt) <> "", " and ", "") & " Company_IdNo <> 0 and company_idno < " & Str(Val(lbl_CompID.Text))

            movid = 0
            If IsDBNull(cmd.ExecuteScalar) = False Then
                movid = cmd.ExecuteScalar
            End If

            If movid <> 0 Then
                move_record(movid)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter("select company_name from company_head  " & IIf(Trim(CompType_Condt) <> "", " Where ", "") & CompType_Condt & " order by company_name", con)
        Dim dt As New DataTable

        da.Fill(dt)

        cbo_Open.DataSource = dt
        cbo_Open.DisplayMember = "company_name"

        grp_Open.Visible = True
        grp_Open.BringToFront()
        cbo_Open.Focus()
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        'MessageBox.Show("insert record")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        'MessageBox.Show("Company creation -  print")
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim nr As Long
        Dim new_entry As Boolean = False
        Dim SurNm As String = ""
        Dim sTATE_iD As Integer = 0

        Dim Sizstk_id As Integer = 0
        Dim Close_STS As Integer = 0

        Dim TCS_STS As Integer = 0

        Dim vTex_Sts As Integer = 0
        Dim vSiz_Sts As Integer = 0
        Dim vOe_Sts As Integer = 0

        Close_STS = 0
        If chk_Close_Status.Checked = True Then Close_STS = 1

        TCS_STS = 0
        If Chk_Tcs_sts.Checked = True Then TCS_STS = 1

        vTex_Sts = 0
        If chk_Textile_Sts.Checked = True Then vTex_Sts = 1

        vSiz_Sts = 0
        If Chk_Sizing_Sts.Checked = True Then vSiz_Sts = 1

        vOe_Sts = 0
        If Chk_OE_Sts.Checked = True Then vOe_Sts = 1



        If Trim(txt_CompanyName.Text) = "" Then
            MessageBox.Show("Invalid company name", "DOES NOT SAVE", MessageBoxButtons.OK)
            Exit Sub
        End If

        If Trim(txt_ShortName.Text) = "" Then
            MessageBox.Show("Invalid Company Short name", "DOES NOT SAVE", MessageBoxButtons.OK)
            If txt_ShortName.Enabled Then txt_ShortName.Focus()
            Exit Sub
        End If

        SurNm = Common_Procedures.Remove_NonCharacters(Trim(txt_CompanyName.Text) & "-" & Trim(txt_ShortName.Text))

        sTATE_iD = Common_Procedures.State_NameToIdNo(con, cbo_State.Text)
        If sTATE_iD = 0 Then
            MessageBox.Show("Invalid State", "DOES NOT SAVE", MessageBoxButtons.OK)
            If cbo_State.Enabled Then cbo_State.Focus()
            Exit Sub
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1417" Then
            Sizstk_id = 0
            If cbo_Sizing_Ledger_Name.Visible Then
                If Trim(cbo_Sizing_Ledger_Name.Text) <> "" Then
                    Sizstk_id = Common_Procedures.Ledger_NameToIdNo(con, cbo_Sizing_Ledger_Name.Text)
                    If Val(Sizstk_id) = 0 Then
                        MessageBox.Show("Invalid Godown Ledger Name", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If cbo_Sizing_Ledger_Name.Enabled Then cbo_Sizing_Ledger_Name.Focus()
                        Exit Sub
                    End If
                End If
            End If

        Else

            Sizstk_id = 0
            If cbo_Sizing_Ledger_Name.Visible Then
                If Trim(cbo_Sizing_Ledger_Name.Text) <> "" Then
                    Sizstk_id = Common_Procedures.Company_NameToIdNo(con, cbo_Sizing_Ledger_Name.Text, TrnTo_DbName)
                    If Val(Sizstk_id) = 0 Then
                        MessageBox.Show("Invalid Sizing Ledger Name", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If cbo_Sizing_Ledger_Name.Enabled Then cbo_Sizing_Ledger_Name.Focus()
                        Exit Sub
                    End If
                End If
            End If

        End If

        If Trim(txt_GSTIN_No.Text) <> "" Then
            txt_GSTIN_No.Text = Trim(txt_GSTIN_No.Text)
            txt_GSTIN_No.Text = Replace(Trim(txt_GSTIN_No.Text), " ", "")
            If Len(Trim(txt_GSTIN_No.Text)) <> 15 Then
                MessageBox.Show("Invalid GSTIN Number  (should be 15 digit)", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_GSTIN_No.Enabled Then txt_GSTIN_No.Focus()
                Exit Sub
            End If
        End If

        Dim ms As New MemoryStream()
        If IsNothing(Pic_Company_logo_Image.BackgroundImage) = False Then
            Dim bitmp As New Bitmap(Pic_Company_logo_Image.BackgroundImage)
            bitmp.Save(ms, Drawing.Imaging.ImageFormat.Jpeg)
        End If
        Dim data As Byte() = ms.GetBuffer()
        Dim p As New SqlClient.SqlParameter("@Company_logo", SqlDbType.Image)
        p.Value = data
        cmd.Parameters.Add(p)
        ms.Dispose()


        trans = con.BeginTransaction

        Try
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "Update Company_Head set Company_Name = '" & Trim(txt_CompanyName.Text) & "', Company_SurName = '" & Trim(SurNm) & "', Company_Address1 = '" & Trim(txt_Address1.Text) & "', Company_Address2 = '" & Trim(txt_Address2.Text) & "', Company_Address3 = '" & Trim(txt_Address3.Text) & "', Company_Address4 = '" & Trim(txt_Address4.Text) & "', Company_City = '" & Trim(txt_City.Text) & "', Company_PinCode = '" & Trim(txt_PinCode.Text) & "', Company_PhoneNo = '" & Trim(txt_PhoneNo.Text) & "',  Company_TinNo = '" & Trim(txt_TinNo.Text) & "', Company_CstNo = '" & Trim(txt_CstNo.Text) & "',Company_PanNo ='" & Trim(txt_PanNo.Text) & "',  Company_ShortName = '" & Trim(txt_ShortName.Text) & "', Company_Type = '" & Trim(cbo_CompanyType.Text) & "', Company_EMail = '" & Trim(txt_EMail.Text) & "', Company_ContactPerson = '" & Trim(txt_ContactName.Text) & "', Company_Bank_Ac_Details = '" & Trim(txt_Bank_Ac_Details.Text) & "', Company_Description = '" & Trim(txt_Description.Text) & "', Company_FaxNo = '" & Trim(txt_FaxNo.Text) & "', Company_Factory_Address1 = '" & Trim(txt_Factory_Address1.Text) & "' , Company_Factory_Address2 ='" & Trim(txt_Factory_Address2.Text) & "', Company_Factory_Address3= '" & Trim(txt_Factory_Address3.Text) & "' , Company_Factory_Address4 = '" & Trim(txt_Factory_Address4.Text) & "', Company_GSTinNo='" & Trim(txt_GSTIN_No.Text) & "', Company_CinNo='" & Trim(txt_CIN_No.Text) & "' ,Company_Owner_Designation='" & Trim(cbo_Company_Designation.Text) & "',Company_Website='" & Trim(txt_Website.Text) & "', Company_State_IdNo= " & Str(sTATE_iD) & ", Sizing_To_LedgerIdNo = " & Str(Sizstk_id) & " , Legal_Nameof_Business = '" & Trim(txt_LegalName_Business.Text) & "' , Sizing_Address1 = '" & Trim(txt_Siz_Address1.Text) & "'  ,  Sizing_Address2 = '" & Trim(txt_Siz_Address2.Text) & "'  , Sizing_Address3 = '" & Trim(txt_Siz_Address3.Text) & "'  , Sizing_Address4 = '" & Trim(txt_Siz_Address4.Text) & "'  , Sizing_City = '" & Trim(txt_Siz_City.Text) & "' , Sizing_PinCode = '" & Trim(txt_Siz_Pincode.Text) & "' ,  Sizing_PhoneNo = '" & Trim(txt_Siz_PhoneNo.Text) & "'  , Sizing_EMail = '" & Trim(txt_Siz_Email.Text) & "' ,Close_Status=" & Str(Val(Close_STS)) & ",TCS_Company_Status=" & Str(Val(TCS_STS)) & " ,Company_logo_Image=@Company_logo , Company_Head_COlour_Image = " & Pic_Colour.BackColor.ToArgb & ",Company_Tamil_Name= '" & Trim(txt_TamilName.Text) & "',Company_Tamil_Address1= '" & Trim(Txt_TamilAddress_1.Text) & "',Company_Tamil_Address2= '" & Trim(Txt_TamilAddress_2.Text) & "' , Tex_Module_Status = " & Str(Val(vTex_Sts)) & " , Siz_Module_Status = " & Str(Val(vSiz_Sts)) & " , OE_Module_Status = " & Str(Val(vOe_Sts)) & " ,Company_UAM_No='" & Trim(txt_UAM_No.Text) & "',Jurisdiction='" & Trim(txt_Jurisdiction.Text) & "',Company_IdNo_For_Report_Cond = " & Str(Val(lbl_CompID.Text)) & " where Company_IdNo = " & Str(Val(lbl_CompID.Text))
            cmd.Connection = con
            cmd.Transaction = trans

            nr = cmd.ExecuteNonQuery

            If nr = 0 Then

                cmd.CommandText = "Insert into Company_Head ( Company_IdNo, Company_Name, Company_SurName, Company_Address1, Company_Address2, Company_Address3, Company_Address4, Company_City, Company_PinCode, Company_PhoneNo,Company_FaxNo, Company_TinNo, Company_CstNo,Company_PanNo, Company_ShortName, Company_Type, Company_EMail, Company_ContactPerson, Company_Bank_Ac_Details, Company_Description ,Company_Factory_Address1   , Company_Factory_Address2   ,  Company_Factory_Address3  ,  Company_Factory_Address4 ,Company_GSTinNo,Company_CinNo,Company_Owner_Designation,Company_Website,Company_State_IdNo, Sizing_To_LedgerIdNo , Legal_Nameof_Business , Sizing_Address1 , Sizing_Address2 , Sizing_Address3 , Sizing_Address4 , Sizing_City , Sizing_PinCode , Sizing_PhoneNo , Sizing_EMail ,Close_Status,TCS_Company_Status,Company_logo_Image,Company_Tamil_Name,Company_Tamil_Address1,Company_Tamil_Address2 , Tex_Module_Status  ,   Siz_Module_Status  ,   OE_Module_Status ,Company_UAM_No ,Jurisdiction,Company_IdNo_For_Report_Cond) values ( " & Str(Val(lbl_CompID.Text)) & ", '" & Trim(txt_CompanyName.Text) & "', '" & Trim(SurNm) & "', '" & Trim(txt_Address1.Text) & "', '" & Trim(txt_Address2.Text) & "', '" & Trim(txt_Address3.Text) & "', '" & Trim(txt_Address4.Text) & "', '" & Trim(txt_City.Text) & "', '" & Trim(txt_PinCode.Text) & "', '" & Trim(txt_PhoneNo.Text) & "','" & Trim(txt_FaxNo.Text) & "',  '" & Trim(txt_TinNo.Text) & "', '" & Trim(txt_CstNo.Text) & "', '" & Trim(txt_PanNo.Text) & "', '" & Trim(txt_ShortName.Text) & "', '" & Trim(cbo_CompanyType.Text) & "', '" & Trim(txt_EMail.Text) & "', '" & Trim(txt_ContactName.Text) & "', '" & Trim(txt_Bank_Ac_Details.Text) & "', '" & Trim(txt_Description.Text) & "','" & Trim(txt_Factory_Address1.Text) & "','" & Trim(txt_Factory_Address2.Text) & "','" & Trim(txt_Factory_Address3.Text) & "','" & Trim(txt_Factory_Address4.Text) & "','" & Trim(txt_GSTIN_No.Text) & "' ,'" & Trim(txt_CIN_No.Text) & "','" & Trim(cbo_Company_Designation.Text) & "','" & Trim(txt_Website.Text) & "', " & Str(sTATE_iD) & ", " & Str(Sizstk_id) & " , '" & Trim(txt_LegalName_Business.Text) & "' , '" & Trim(txt_Siz_Address1.Text) & "' , '" & Trim(txt_Siz_Address2.Text) & "' , '" & Trim(txt_Siz_Address3.Text) & "', '" & Trim(txt_Siz_Address4.Text) & "', '" & Trim(txt_Siz_City.Text) & "' , '" & Trim(txt_Siz_Pincode.Text) & "' , '" & Trim(txt_Siz_PhoneNo.Text) & "', '" & Trim(txt_Siz_Email.Text) & "' ," & Str(Val(Close_STS)) & "," & Str(Val(TCS_STS)) & " ,@Company_logo,'" & Trim(txt_TamilName.Text) & "','" & Trim(Txt_TamilAddress_1.Text) & "','" & Trim(Txt_TamilAddress_2.Text) & "' , " & Str(Val(vTex_Sts)) & "    , " & Str(Val(vSiz_Sts)) & "    ,  " & Str(Val(vOe_Sts)) & "  ,'" & Trim(txt_UAM_No.Text) & "' ,'" & Trim(txt_Jurisdiction.Text) & "'," & Str(Val(lbl_CompID.Text)) & " )"
                cmd.ExecuteNonQuery()

                new_entry = True

            End If

            trans.Commit()

            MessageBox.Show("Saved", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If new_entry = True Then Call new_record()

        Catch ex As Exception
            trans.Rollback()
            If InStr(1, LCase(ex.Message), "duplicate_companyhead_name") > 0 Or InStr(1, LCase(ex.Message), "duplicate_companyhead_surname") > 0 Then
                MessageBox.Show("Duplicate Company Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, LCase(ex.Message), "duplicate_companyhead_shortname") > 0 Then
                MessageBox.Show("Duplicate Company Short Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If
            Exit Sub

        End Try

        If txt_CompanyName.Enabled And txt_CompanyName.Visible Then txt_CompanyName.Focus()

    End Sub


    Private Sub Company_Creation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable

        Dim TrnTo_CmpGrpIdNo As Integer = 0

        If Trim(UCase(Common_Procedures.User.Type)) = "UNACCOUNT" Then
            lbl_CompIDCaption.Visible = True
            lbl_CompID.Visible = True

            lbl_CompanyType.Visible = True
            cbo_CompanyType.Visible = True

            txt_ShortName.Width = 125

            CompType_Condt = ""

        Else

            lbl_CompIDCaption.Visible = False
            lbl_CompID.Visible = False

            lbl_CompanyType.Visible = False
            cbo_CompanyType.Visible = False

            txt_ShortName.Width = txt_CompanyName.Width

            CompType_Condt = "(Company_Type <> 'UNACCOUNT')"

        End If
        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            lbl_Sizing.Text = "Company Godown Name"
        End If
        cbo_Sizing_Ledger_Name.Visible = False
        lbl_Sizing.Visible = False
        If Common_Procedures.settings.Combine_Textile_SizingSOftware = 1 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1417" Then
                TrnTo_DbName = Common_Procedures.get_Company_DataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))
                cbo_Sizing_Ledger_Name.Visible = True
                lbl_Sizing.Visible = True
            Else
                TrnTo_DbName = Common_Procedures.get_Company_SizingDataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))
                cbo_Sizing_Ledger_Name.Visible = True
                lbl_Sizing.Visible = True
            End If

        Else
            TrnTo_DbName = Common_Procedures.get_Company_DataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))
        End If

        If Common_Procedures.settings.CustomerCode = "1408" Then
            btn_Tamil_Address.Visible = True
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1417" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1428" Then
            chk_Textile_Sts.Visible = True
            Chk_Sizing_Sts.Visible = True
            Chk_OE_Sts.Visible = True
        End If

        '---------------


        If cbo_Sizing_Ledger_Name.Visible = False And lbl_Sizing.Visible = False Then



            btn_Save.Location = New Point(419, 497)
            btn_Close.Location = New Point(482, 497)

            btn_Factory_Address.Location = New Point(302, 498)

            Me.Size = New Size(575, 566)




        End If


        '--------------


        da = New SqlClient.SqlDataAdapter("select State_Name from State_Head Order by State_Name", con)
        da.Fill(dt4)
        cbo_State.Items.Clear()
        cbo_State.DataSource = dt4
        cbo_State.DisplayMember = "State_Name"




        grp_Open.Visible = False
        grp_Open.BackColor = Me.BackColor
        grp_Open.Left = (Me.Width - grp_Open.Width) - 50
        grp_Open.Top = (Me.Height - grp_Open.Height) - 50

        grp_Filter.Visible = False
        grp_Filter.BackColor = Me.BackColor
        grp_Filter.Left = (Me.Width - grp_Filter.Width) - 50
        grp_Filter.Top = (Me.Height - grp_Filter.Height) - 50

        pnl_Factory_Address.Visible = False
        pnl_Factory_Address.Left = (Me.Width - pnl_Factory_Address.Width) \ 2
        pnl_Factory_Address.Top = (Me.Height - pnl_Factory_Address.Height) \ 2
        pnl_Factory_Address.BringToFront()

        Pnl_Sizing_Address.Visible = False
        Pnl_Sizing_Address.Left = (Me.Width - Pnl_Sizing_Address.Width) \ 2
        Pnl_Sizing_Address.Top = (Me.Height - Pnl_Sizing_Address.Height) \ 2
        Pnl_Sizing_Address.BringToFront()

        pnl_company_logo.Visible = False
        pnl_company_logo.Left = ((Me.Width - pnl_company_logo.Width) / 2) - 200
        pnl_company_logo.Top = ((Me.Height - pnl_company_logo.Height) / 2) - 50


        pnl_tamil_Address.Visible = False
        pnl_tamil_Address.Left = (Me.Width - pnl_tamil_Address.Width) \ 2
        pnl_tamil_Address.Top = (Me.Height - pnl_tamil_Address.Height) \ 2
        pnl_tamil_Address.BringToFront()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1111" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1118" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1147" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1235" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1290" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1292" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1274" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1234" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1486" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1395" Then '---- 
            btn_Factory_Address.Visible = True
        End If

        con.Open()

        new_record()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
            btn_Sizing_Address.Visible = True
        Else
            btn_Sizing_Address.Visible = False
        End If

        AddHandler txt_Address1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Address2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Address3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Address4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Bank_Ac_Details.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_City.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CompanyName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ContactName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ContactName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CstNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Description.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EMail.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Factory_Address1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Factory_Address2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Factory_Address3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Factory_Address4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_FaxNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_GSTIN_No.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CIN_No.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PanNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PhoneNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ShortName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PinCode.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ShortName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TinNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Website.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Company_Designation.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_CompanyType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Open.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_State.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Sizing_Ledger_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LegalName_Business.GotFocus, AddressOf ControlGotFocus
        '  AddHandler txt_City.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PinCode.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Siz_Address1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Siz_Address2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Siz_Address3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Siz_Address4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Siz_City.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Siz_Pincode.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Siz_PhoneNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Siz_Email.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Sizing_Ledger_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Address1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Address2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Address3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Address4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Bank_Ac_Details.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_City.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CompanyName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ContactName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ContactName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CstNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Description.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EMail.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Factory_Address1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Factory_Address2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Factory_Address3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Factory_Address4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_FaxNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_GSTIN_No.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CIN_No.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PanNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PhoneNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PinCode.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TinNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ShortName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Website.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Company_Designation.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_CompanyType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Open.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_State.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LegalName_Business.LostFocus, AddressOf ControlLostFocus
        '   AddHandler txt_City.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PinCode.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_Siz_Address1.GotFocus, AddressOf ControlLostFocus
        AddHandler txt_Siz_Address2.GotFocus, AddressOf ControlLostFocus
        AddHandler txt_Siz_Address3.GotFocus, AddressOf ControlLostFocus
        AddHandler txt_Siz_Address4.GotFocus, AddressOf ControlLostFocus
        AddHandler txt_Siz_City.GotFocus, AddressOf ControlLostFocus
        AddHandler txt_Siz_Pincode.GotFocus, AddressOf ControlLostFocus
        AddHandler txt_Siz_PhoneNo.GotFocus, AddressOf ControlLostFocus
        AddHandler txt_Siz_Email.GotFocus, AddressOf ControlLostFocus


        AddHandler txt_UAM_No.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_UAM_No.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Jurisdiction.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Jurisdiction.LostFocus, AddressOf ControlLostFocus






        'AddHandler cbo_Open.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler cbo_State.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler cbo_Company_Designation.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler cbo_CompanyType.KeyPress, AddressOf TextBoxControlKeyPress
        '   AddHandler txt_LegalName_Business.KeyPress, AddressOf TextBoxControlKeyPress

        ' AddHandler txt_LegalName_Business.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler cbo_Open.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler cbo_State.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler cbo_Company_Designation.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler cbo_CompanyType.KeyDown, AddressOf TextBoxControlKeyDown
    End Sub

    Private Sub Company_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Company_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Open.Visible = True Then
                grp_Open.Visible = False
                Exit Sub
            End If
            If dgv_Filter.Visible = True Then
                grp_Filter.Visible = False
                Exit Sub
            End If
            If pnl_Factory_Address.Visible = True Then
                btn_Close_Address_Details_Click(sender, e)
                Exit Sub
            End If
            If pnl_company_logo.Visible = True Then
                pnl_company_logo.Visible = False
                Exit Sub
            End If
            If Pnl_Sizing_Address.Visible = True Then
                btn_Siz_Close_Click(sender, e)
                Exit Sub
            ElseIf pnl_tamil_Address.Visible = True Then

                btn_pnl_tamil_Address_Click(sender, e)
                Exit Sub
            End If
            If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                Exit Sub

            Else
                Me.Close()

            End If

        End If
    End Sub
    Private Sub btn_pnl_tamil_Address_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_pnl_Close_tamil_Address.Click
        pnl_tamil_Address.Visible = False

        If txt_CompanyName.Enabled And txt_CompanyName.Visible Then txt_CompanyName.Focus()
    End Sub
    Private Sub txt_CompanyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CompanyName.KeyPress
        If Asc(e.KeyChar) = 34 Or Asc(e.KeyChar) = 39 Then   '-- Single Quotes and double quotes blocked
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_ContactName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ContactName.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Address1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Address1.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Address2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Address2.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Address3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Address3.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Address4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Address4.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_City_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_City.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_PinCode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PinCode.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_PhoneNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PhoneNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub txt_FaxNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_FaxNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub txt_FaxNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_FaxNo.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub txt_TinNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TinNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_ShortName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ShortName.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_EMail_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EMail.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Description_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Description.KeyPress
        If Asc(e.KeyChar) = 13 Then
            'If cbo_Sizing_Ledger_Name.Visible = True Then
            '    cbo_Sizing_Ledger_Name.Focus()
            'Else
            '    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            '        save_record()
            '    Else
            '        txt_CompanyName.Focus()
            '    End If
            'End If     

            txt_Jurisdiction.Focus()

        End If

    End Sub

    Private Sub txt_CompanyName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CompanyName.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_ContactName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_ContactName.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Address1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Address1.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Address2_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Address2.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Address3_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Address3.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Address4_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Address4.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_City_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_City.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_PinCode_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PinCode.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_PhoneNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PhoneNo.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_TinNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TinNo.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_ShortName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_ShortName.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_EMail_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_EMail.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Description_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Description.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        grp_Open.Visible = False
    End Sub

    Private Sub btn_Find_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Find.Click
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim idno As Integer

        Try

            cmd.CommandText = "select company_idno from company_head where company_idno <> 0 and company_name = '" & Trim(cbo_Open.Text) & "'"
            cmd.Connection = con

            dr = cmd.ExecuteReader

            If dr.HasRows() Then
                If dr.Read() Then
                    idno = Val(dr("company_idno"))
                    If Val(idno) <> 0 Then
                        dr.Close()
                        move_record(Val(idno))
                        grp_Open.Visible = False
                    End If
                Else
                    dr.Close()
                End If
            Else
                dr.Close()
            End If

            cmd.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Open_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Open.GotFocus

        With cbo_Open
            .BackColor = Color.LemonChiffon
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = cbo_Open.Text.Length
        End With

    End Sub

    Private Sub cbo_Open_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Open.LostFocus
        cbo_Open.BackColor = Color.White
        cbo_Open.ForeColor = Color.Black
    End Sub

    Private Sub cbo_Open_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Open.KeyDown
        Try
            With cbo_Open
                If e.KeyValue = 38 And .DroppedDown = False Then
                    'e.Handled = True
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    'e.Handled = True
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Open_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Open.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String

        'Try

        With cbo_Open

            If Asc(e.KeyChar) <> 27 Then

                If Asc(e.KeyChar) = 13 Then

                    With cbo_Open
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

                    Call btn_Find_Click(sender, e)

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

                    Condt = IIf(Trim(CompType_Condt) <> "", " Where ", "") & Trim(CompType_Condt)
                    If Trim(FindStr) <> "" Then
                        Condt = " Where  " & CompType_Condt & IIf(Trim(CompType_Condt) <> "", " and ", "") & " (Company_Name like '" & Trim(FindStr) & "%' or Company_Name like '% " & Trim(FindStr) & "%') "
                    End If

                    da = New SqlClient.SqlDataAdapter("select Company_Name from Company_Head " & Condt & " order by Company_Name", con)
                    da.Fill(dt)

                    .DataSource = dt
                    .DisplayMember = "Company_Name"

                    .Text = FindStr

                    .SelectionStart = FindStr.Length

                    e.Handled = True

                    da.Dispose()

                End If

            End If

        End With

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

    End Sub

    Private Sub btn_CloseFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseFilter.Click
        grp_Filter.Visible = False
    End Sub

    Private Sub btn_Open_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Open.Click
        Dim IdNo As Integer

        IdNo = Val(dgv_Filter.CurrentRow.Cells(0).Value)

        If Val(IdNo) <> 0 Then
            Call move_record(IdNo)
            grp_Filter.Visible = False
        End If

    End Sub

    Private Sub dgv_Filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Filter.DoubleClick
        btn_Open_Click(sender, e)
    End Sub

    Private Sub dgv_Filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
        If e.KeyValue = 13 Then
            btn_Open_Click(sender, e)
        End If
    End Sub




    Private Sub txt_CstNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CstNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_CstNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CstNo.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Bank_Ac_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Bank_Ac_Details.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Bank_Ac_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Bank_Ac_Details.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub txt_PanNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PanNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub txt_PanNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PanNo.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Factory_Address1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Factory_Address1.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub


    Private Sub txt_Factory_Address1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Factory_Address1.KeyUp
        If e.KeyCode = Keys.Up Then
            pnl_Factory_Address.Visible = False
            txt_CompanyName.Focus()
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Factory_Address2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Factory_Address2.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Factory_Address2_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Factory_Address2.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Factory_Address3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Factory_Address3.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Factory_Address3_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Factory_Address3.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Factory_Address4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Factory_Address4.KeyPress
        If Asc(e.KeyChar) = 13 Then
            pnl_Factory_Address.Visible = False
            txt_CompanyName.Focus()
        End If
    End Sub

    Private Sub txt_Factory_Address4_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Factory_Address4.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            pnl_Factory_Address.Visible = False
            txt_CompanyName.Focus()
        End If
    End Sub

    Private Sub btn_Factory_Address_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Factory_Address.Click
        pnl_Factory_Address.Visible = True
        txt_Factory_Address1.Focus()
    End Sub

    Private Sub btn_Close_Address_Details_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Address_Details.Click
        pnl_Factory_Address.Visible = False
        txt_CompanyName.Focus()
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub
    '--------------------------------------------------------------
    '-----------------GST ALTER------------------------------------
    '--------------------------------------------------------------

    Private Sub txt_GSTIN_No_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_GSTIN_No.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub txt_CIN_No_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CIN_No.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Website_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Website.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_GSTIN_No_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_GSTIN_No.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            pnl_Factory_Address.Visible = False
            txt_CompanyName.Focus()
        End If
    End Sub

    Private Sub txt_CIN_No_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CIN_No.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            pnl_Factory_Address.Visible = False
            txt_CompanyName.Focus()
        End If
    End Sub

    Private Sub txt_Website_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Website.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            pnl_Factory_Address.Visible = False
            txt_CompanyName.Focus()
        End If
    End Sub

    '--------------------------------------------------------------
    '--------------------------------------------------------------
    '--------------------------------------------------------------

    Private Sub cbo_Company_Designation_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Company_Designation.GotFocus
        With cbo_Company_Designation
            .BackColor = Color.LemonChiffon
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = .Text.Length
        End With
    End Sub

    Private Sub cbo_State_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_State.GotFocus
        With cbo_State
            .BackColor = Color.LemonChiffon
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = .Text.Length
        End With
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "State_Head", "State_Name", "", "(State_Idno = 0)")
    End Sub

    Private Sub cbo_State_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_State.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_State, txt_City, txt_PinCode, "State_Head", "State_Name", "", "(State_Idno = 0)")

    End Sub

    Private Sub cbo_State_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_State.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_State, txt_PinCode, "State_Head", "State_Name", "", "(State_Idno = 0)")

    End Sub

    Private Sub cbo_Company_Designation_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Company_Designation.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Company_Designation, txt_ContactName, txt_Address1, "", "", "", "")
    End Sub

    Private Sub cbo_Company_Designation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Company_Designation.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_State, txt_Address1, "", "", "", "", True)
    End Sub

    Private Sub cbo_Company_Designation_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Company_Designation.LostFocus
        With cbo_Company_Designation
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub cbo_State_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_State.LostFocus
        With cbo_State
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub
    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If TypeOf Me.ActiveControl Is ComboBox Then
            cbo = Me.ActiveControl
            On Error Resume Next
            If e.KeyValue = 38 And cbo.DroppedDown = False Then SendKeys.Send("+{TAB}")
            If e.KeyValue = 40 And cbo.DroppedDown = False Then SendKeys.Send("{TAB}")
        Else
            On Error Resume Next
            If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
            If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        End If

    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If TypeOf Me.ActiveControl Is ComboBox Then
            cbo = Me.ActiveControl
            On Error Resume Next
            If Asc(e.KeyChar) = 13 And cbo.DroppedDown = False Then SendKeys.Send("{TAB}")

        Else
            On Error Resume Next
            If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")

        End If

    End Sub

    Private Sub cbo_CompanyType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CompanyType.GotFocus
        With cbo_CompanyType
            .BackColor = Color.LemonChiffon
            .ForeColor = Color.Blue
            .SelectionStart = 0
            .SelectionLength = .Text.Length
        End With
    End Sub

    Private Sub cbo_CompanyType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CompanyType.KeyDown
        Try
            With cbo_CompanyType
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    txt_ShortName.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    ' txt_ContactName.Focus()
                    txt_LegalName_Business.Focus()
                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_CompanyType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CompanyType.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String
        Dim indx As Integer

        Try

            With cbo_CompanyType

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        If Trim(.Text) <> "" Then
                            If .DroppedDown = True Then
                                If Trim(.SelectedText) <> "" Then
                                    .Text = .GetItemText(.SelectedItem)
                                    '.Text = .SelectedText
                                Else
                                    If .Items.Count > 0 Then
                                        .SelectedIndex = 0
                                        .SelectedItem = .Items(0)
                                        .Text = .GetItemText(.SelectedItem)
                                    End If
                                End If
                            End If
                        End If

                        ' txt_ContactName.Focus()
                        txt_LegalName_Business.Focus()
                        'SendKeys.Send("{TAB}")

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

                        indx = .FindString(FindStr)

                        If indx <> -1 Then
                            .SelectedText = ""
                            .SelectedIndex = indx

                            .SelectionStart = FindStr.Length
                            .SelectionLength = .Text.Length
                            e.Handled = True

                        Else
                            e.Handled = True

                        End If

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_CompanyType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CompanyType.LostFocus
        With cbo_CompanyType
            .BackColor = Color.White
            .ForeColor = Color.Black
        End With
    End Sub
    Private Sub cbo_Sizing_Ledger_Name_Gotfocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Sizing_Ledger_Name.GotFocus

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1417" Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, TrnTo_DbName & "..Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING'  or Ledger_Type = 'WEAVER') and Close_status = 0", "(Ledger_idno = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, TrnTo_DbName & "..Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or Ledger_Type = 'SIZING') and Close_status = 0", "(Ledger_idno = 0)")

        End If


    End Sub

    Private Sub cbo_Sizing_Ledger_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Sizing_Ledger_Name.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1417" Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Sizing_Ledger_Name, txt_Description, Nothing, TrnTo_DbName & "..Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING'  or Ledger_Type = 'WEAVER') and Close_status = 0", "(Ledger_idno = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Sizing_Ledger_Name, txt_Description, Nothing, TrnTo_DbName & "..Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or Ledger_Type = 'SIZING') and Close_status = 0", "(Ledger_idno = 0)")
        End If

        If (e.KeyValue = 40 And cbo_Sizing_Ledger_Name.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_CompanyName.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Sizing_Ledger_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Sizing_Ledger_Name.KeyPress
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1417" Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Sizing_Ledger_Name, Nothing, TrnTo_DbName & "..Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or Ledger_Type = 'GODWON' or Ledger_Type = 'SIZING'  or Ledger_Type = 'WEAVER') and Close_status = 0", "(Ledger_idno = 0)")
        Else

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Sizing_Ledger_Name, Nothing, TrnTo_DbName & "..Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or Ledger_Type = 'SIZING') and Close_status = 0", "(Ledger_idno = 0)")
        End If

        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_CompanyName.Focus()
            End If
        End If
    End Sub

    Private Sub txt_LegalName_Business_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_LegalName_Business.KeyDown
        If e.KeyCode = 38 Then
            txt_ShortName.Focus()
        End If
        If e.KeyCode = 40 Then
            txt_Address1.Focus()
        End If
    End Sub

    Private Sub txt_LegalName_Business_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_LegalName_Business.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Address1.Focus()
        End If
    End Sub

    Private Sub btn_Sizing_Address_Click(sender As Object, e As EventArgs) Handles btn_Sizing_Address.Click
        Pnl_Sizing_Address.Visible = True
        txt_Siz_Address1.Focus()
    End Sub

    Private Sub btn_Siz_Close_Click(sender As Object, e As EventArgs) Handles btn_Siz_Close.Click
        Pnl_Sizing_Address.Visible = False
        txt_CompanyName.Focus()
    End Sub

    Private Sub txt_Siz_Address1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Siz_Address1.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Siz_Address2.Focus()
        End If
    End Sub

    Private Sub txt_Siz_Address1_KeyUp(sender As Object, e As KeyEventArgs) Handles txt_Siz_Address1.KeyUp
        If e.KeyCode = Keys.Up Then
            Pnl_Sizing_Address.Visible = False
            txt_CompanyName.Focus()
        End If
        If e.KeyCode = Keys.Down Then
            '  System.Windows.Forms.SendKeys.Send("{TAB}")
            txt_Siz_Address2.Focus()
        End If
    End Sub

    Private Sub txt_Siz_Address2_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Siz_Address2.KeyDown
        If e.KeyCode = Keys.Up Then
            txt_Siz_Address1.Focus()
        End If
        If e.KeyCode = Keys.Down Then
            '  System.Windows.Forms.SendKeys.Send("{TAB}")
            txt_Siz_Address3.Focus()
        End If
    End Sub

    Private Sub txt_Siz_Address2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Siz_Address2.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Siz_Address3.Focus()
        End If
    End Sub

    Private Sub txt_Siz_Address3_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Siz_Address3.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Siz_Address4.Focus()
        End If
    End Sub

    Private Sub txt_Siz_Address3_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Siz_Address3.KeyDown
        If e.KeyCode = Keys.Up Then
            txt_Siz_Address2.Focus()
        End If
        If e.KeyCode = Keys.Down Then
            txt_Siz_Address4.Focus()
        End If
    End Sub

    Private Sub txt_Siz_Address4_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Siz_Address4.KeyDown
        If e.KeyCode = Keys.Up Then
            txt_Siz_Address3.Focus()
        End If
        If e.KeyCode = Keys.Down Then
            txt_Siz_City.Focus()
        End If
    End Sub

    Private Sub txt_Siz_Address4_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Siz_Address4.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Siz_City.Focus()
        End If
    End Sub

    Private Sub txt_Siz_City_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Siz_City.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Siz_Pincode.Focus()
        End If
    End Sub

    Private Sub txt_Siz_City_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Siz_City.KeyDown
        If e.KeyCode = Keys.Up Then
            txt_Siz_Address4.Focus()
        End If
        If e.KeyCode = Keys.Down Then
            txt_Siz_Pincode.Focus()
        End If
    End Sub

    Private Sub txt_Siz_PhoneNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Siz_PhoneNo.KeyDown
        If e.KeyCode = Keys.Up Then
            txt_Siz_Pincode.Focus()
        End If
        If e.KeyCode = Keys.Down Then
            txt_Siz_Email.Focus()
        End If
    End Sub

    Private Sub txt_Siz_PhoneNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Siz_PhoneNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Siz_Email.Focus()
        End If
    End Sub

    Private Sub txt_Siz_Email_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Siz_Email.KeyPress
        If Asc(e.KeyChar) = 13 Then
            Pnl_Sizing_Address.Visible = False
            txt_CompanyName.Focus()
        End If
    End Sub

    Private Sub txt_Siz_Email_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Siz_Email.KeyDown
        If e.KeyCode = Keys.Up Then
            txt_Siz_PhoneNo.Focus()
        End If
        If e.KeyCode = Keys.Down Then
            Pnl_Sizing_Address.Visible = False
            txt_CompanyName.Focus()
        End If
    End Sub

    Private Sub txt_Siz_Pincode_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Siz_Pincode.KeyDown
        If e.KeyCode = Keys.Up Then
            txt_Siz_City.Focus()
        End If
        If e.KeyCode = Keys.Down Then
            txt_Siz_PhoneNo.Focus()
        End If
    End Sub

    Private Sub txt_Siz_Pincode_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Siz_Pincode.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Siz_PhoneNo.Focus()
        End If
    End Sub

    Private Sub Btn_Compny_logo_Click(sender As Object, e As EventArgs) Handles Btn_Compny_logo.Click
        pnl_company_logo.BringToFront()

        pnl_company_logo.Visible = True

    End Sub
    Private Sub Btn_Qr_Code_Add_Click(sender As Object, e As EventArgs) Handles Btn_Qr_Code_Add.Click
        OpenFileDialog1.FileName = ""
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Pic_Company_logo_Image.BackgroundImage = Image.FromFile(OpenFileDialog1.FileName)
        End If
    End Sub
    Private Sub Btn_Qr_Code_Close_Click(sender As Object, e As EventArgs) Handles Btn_Qr_Code_Close.Click
        Pic_Company_logo_Image.BackgroundImage = Nothing
    End Sub

    Private Sub Btn_Add_Colour_Click(sender As Object, e As EventArgs) Handles Btn_Add_Colour.Click
        ColorDialog1.Color = Nothing

        If ColorDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Pic_Colour.BackColor = ColorDialog1.Color

        End If
    End Sub

    Private Sub Btn_Remove_Colour_Click(sender As Object, e As EventArgs) Handles Btn_Remove_Colour.Click
        Pic_Colour.BackColor = Nothing

    End Sub

    Private Sub btn_Tamil_Address_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Tamil_Address.Click
        pnl_tamil_Address.Visible = True
        pnl_tamil_Address.BringToFront()
        pnl_tamil_Address.Enabled = True
        txt_TamilName.Focus()

    End Sub

    Private Sub Chk_Sizing_Sts_CheckedChanged(sender As Object, e As EventArgs) Handles Chk_Sizing_Sts.CheckedChanged
        If Chk_Sizing_Sts.Checked = True Then
            chk_Textile_Sts.Checked = False
            Chk_OE_Sts.Checked = False
        End If
    End Sub

    Private Sub chk_Textile_Sts_CheckedChanged(sender As Object, e As EventArgs) Handles chk_Textile_Sts.CheckedChanged
        If chk_Textile_Sts.Checked = True Then
            Chk_Sizing_Sts.Checked = False
            Chk_OE_Sts.Checked = False
        End If
    End Sub

    Private Sub Chk_OE_Sts_CheckedChanged(sender As Object, e As EventArgs) Handles Chk_OE_Sts.CheckedChanged
        If Chk_OE_Sts.Checked = True Then
            chk_Textile_Sts.Checked = False
            Chk_Sizing_Sts.Checked = False
        End If
    End Sub

    Private Sub txt_UAM_No_KeyUp(sender As Object, e As KeyEventArgs) Handles txt_UAM_No.KeyUp
        If e.KeyCode = Keys.Up Then
            System.Windows.Forms.SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            txt_EMail.Focus()
        End If
    End Sub

    Private Sub txt_UAM_No_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_UAM_No.KeyPress
        If Asc(e.KeyChar) = 13 Then
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub


    Private Sub txt_Jurisdiction_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Jurisdiction.KeyDown

        If e.KeyValue = 38 Then

            txt_Description.Focus()
        End If

        If e.KeyValue = 40 Then

            If cbo_Sizing_Ledger_Name.Visible = True Then

                cbo_Sizing_Ledger_Name.Focus()

            Else
                If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    txt_CompanyName.Focus()
                End If

            End If


        End If
    End Sub

    Private Sub txt_Jurisdiction_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Jurisdiction.KeyPress


        If Asc(e.KeyChar) = 13 Then
            If cbo_Sizing_Ledger_Name.Visible = True Then

                cbo_Sizing_Ledger_Name.Focus()
            Else
                If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    txt_CompanyName.Focus()
                End If

            End If
        End If

    End Sub

    Private Sub txt_Description_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Description.KeyDown

        If e.KeyValue = 40 Then

            txt_Jurisdiction.Focus()

        End If
    End Sub
End Class