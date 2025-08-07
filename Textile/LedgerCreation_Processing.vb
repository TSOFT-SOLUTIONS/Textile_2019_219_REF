Imports System.Security.Cryptography.X509Certificates
Imports System.Net
Imports System.Net.Security
Imports System.Net.Mail


Public Class LedgerCreation_Processing
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False
    Private vLedType As String
    Private Prec_ActCtrl As New Control
    Private Verified_STS As Integer = 0

    Private Sub clear()

        New_Entry = False

        pnl_Back.Enabled = True
        grp_Open.Visible = False
        grp_Filter.Visible = False

        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black

        txt_Name.Text = ""
        txt_AlaisName.Text = ""
        cbo_AcGroup.Text = Common_Procedures.AccountsGroup_IdNoToName(con, 10)
        cbo_BillType.Text = "BALANCE ONLY"
        cbo_Area.Text = ""
        txt_Address1.Text = ""
        txt_Address2.Text = ""
        txt_Address3.Text = ""
        txt_Address4.Text = ""
        txt_PhoneNo.Text = ""
        txt_FaxNo.Text = ""
        txt_MobileNo.Text = ""
        txt_EMailID.Text = ""
        txt_TinNo.Text = ""
        txt_CstNo.Text = ""
        txt_PanNo.Text = ""
        txt_ContactPerson.Text = ""
        cbo_PackingType.Text = ""
        cbo_Agent.Text = ""
        txt_MobileSms.Text = ""
        txt_BillingType.Text = ""
        cbo_StickerType.Text = ""
        txt_MrpPerc.Text = ""
        cbo_PriceList.Text = ""
        txt_DiscPercentage.Text = ""
        txt_Duedate.Text = ""

        cbo_Transport.Text = ""
        txt_Remarks.Text = ""

        dgv_Details.Rows.Clear()
        chk_Verified_Status.Checked = False
        cbo_Grid_ItemName.Visible = False
        cbo_Grid_ItemName.Text = ""

        cbo_State.Text = ""
        txt_GSTIN_No.Text = ""

        cbo_Open.Text = ""

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        Me.ActiveControl.BackColor = Color.Lime
        Me.ActiveControl.ForeColor = Color.Blue

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

        If Me.ActiveControl.Name <> cbo_Grid_ItemName.Name Then
            cbo_Grid_ItemName.Visible = False
        End If
        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Grid_DeSelect()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            Prec_ActCtrl.BackColor = Color.White
            Prec_ActCtrl.ForeColor = Color.Black
        End If
    End Sub

    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        'dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Public Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Dt2 As New DataTable
        Dim SNo As Integer = 0
        Dim n As Integer = 0

        If Val(idno) = 0 Then Exit Sub

        clear()

        da = New SqlClient.SqlDataAdapter("select a.*, b.AccountsGroup_Name, c.Area_Name from ledger_head a LEFT OUTER JOIN Area_Head c ON a.Area_IdNo = c.Area_IdNo, AccountsGroup_Head b where a.ledger_idno = " & Str(Val(idno)) & " and a.ledger_type = '" & Trim(vLedType) & "' and a.AccountsGroup_IdNo = b.AccountsGroup_IdNo", con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            lbl_IdNo.Text = dt.Rows(0).Item("Ledger_IdNo").ToString
            txt_Name.Text = dt.Rows(0).Item("Ledger_MainName").ToString
            txt_AlaisName.Text = dt.Rows(0).Item("Ledger_AlaisName").ToString
            cbo_Area.Text = dt.Rows(0)("Area_Name").ToString
            cbo_AcGroup.Text = dt.Rows(0)("AccountsGroup_Name").ToString
            cbo_BillType.Text = dt.Rows(0)("Bill_Type").ToString
            txt_Address1.Text = dt.Rows(0)("Ledger_Address1").ToString
            txt_Address2.Text = dt.Rows(0)("Ledger_Address2").ToString
            txt_Address3.Text = dt.Rows(0)("Ledger_Address3").ToString
            txt_Address4.Text = dt.Rows(0)("Ledger_Address4").ToString
            txt_PhoneNo.Text = dt.Rows(0)("Ledger_PhoneNo").ToString
            txt_FaxNo.Text = dt.Rows(0)("Ledger_FaxNo").ToString
            txt_MobileNo.Text = dt.Rows(0)("Ledger_MobileNo").ToString
            txt_EMailID.Text = dt.Rows(0)("Ledger_Emailid").ToString
            txt_TinNo.Text = dt.Rows(0)("Ledger_TinNo").ToString
            txt_CstNo.Text = dt.Rows(0)("Ledger_CstNo").ToString
            txt_PanNo.Text = dt.Rows(0)("Pan_No").ToString
            txt_ContactPerson.Text = dt.Rows(0)("Contact_Person").ToString
            cbo_PackingType.Text = Common_Procedures.PackingType_IdNoToName(con, Val(dt.Rows(0)("PackingType_CompanyIdNo").ToString))
            cbo_Agent.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt.Rows(0)("Ledger_AgentIdNo").ToString))
            cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt.Rows(0).Item("Transport_IdNo").ToString))
            txt_Remarks.Text = dt.Rows(0)("Note").ToString

            txt_MobileSms.Text = dt.Rows(0)("MobileNo_Sms").ToString
            txt_BillingType.Text = dt.Rows(0)("Billing_Type").ToString
            cbo_StickerType.Text = dt.Rows(0)("Sticker_Type").ToString
            cbo_PriceList.Text = Common_Procedures.Price_List_IdNoToName(con, Val(dt.Rows(0).Item("PriceList_IdNo").ToString))
            txt_MrpPerc.Text = dt.Rows(0)("Mrp_Perc").ToString


            '-----------------GST ALTER------------------------------------
            txt_GSTIN_No.Text = dt.Rows(0)("Ledger_GSTinNo").ToString
            cbo_State.Text = Common_Procedures.State_IdNoToName(con, Val(dt.Rows(0)("Ledger_State_IdNo").ToString))
            '---------------------------------------------------------------

            txt_DiscPercentage.Text = dt.Rows(0)("Disc_Percentage").ToString
            txt_Duedate.Text = dt.Rows(0)("Duedate").ToString

            If Val(dt.Rows(0).Item("Verified_Status").ToString) = 1 Then chk_Verified_Status.Checked = True

            da = New SqlClient.SqlDataAdapter("select a.*, b.Processed_Item_Name from Ledger_ItemName_Details a INNER JOIN Processed_Item_Head b ON a.Item_Idno = b.Processed_Item_IdNo where a.Ledger_IdNo = " & Val(idno), con)
            da.Fill(Dt2)

            dgv_Details.Rows.Clear()
            SNo = 0

            If Dt2.Rows.Count > 0 Then

                For i = 0 To Dt2.Rows.Count - 1

                    n = dgv_Details.Rows.Add()

                    SNo = SNo + 1
                    dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                    dgv_Details.Rows(n).Cells(1).Value = Dt2.Rows(i).Item("Processed_Item_Name").ToString
                    dgv_Details.Rows(n).Cells(2).Value = Dt2.Rows(i).Item("Party_ItemName").ToString
                    dgv_Details.Rows(n).Cells(3).Value = Dt2.Rows(i).Item("Rate_Disc_Percentage").ToString
                    dgv_Details.Rows(n).Cells(4).Value = Dt2.Rows(i).Item("Rate_Disc_Amount").ToString

                Next i

                For i = 0 To dgv_Details.RowCount - 1
                    dgv_Details.Rows(i).Cells(0).Value = Val(i) + 1
                Next

            End If
            Dt2.Clear()
            Dt2.Dispose()

        End If

        dt.Dispose()
        da.Dispose()

        Grid_DeSelect()

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record

        Call clear()

        lbl_IdNo.ForeColor = Color.Red
        New_Entry = True

        lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "ledger_head", "ledger_idno", "")

        If Val(lbl_IdNo.Text) < 101 Then lbl_IdNo.Text = 101

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable
        '
        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Ledger_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Ledger_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.FP_Ledger_Creation, New_Entry, Me) = False Then Exit Sub



      
        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to delete", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is new entry", "DOES NOT DELETION....", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try

            If Val(lbl_IdNo.Text) < 101 Then
                MessageBox.Show("Could not this default ledger", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Voucher_Details where Ledger_Idno = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_Item_Processing_Details where Delivery_PartyIdNo = " & Str(Val(lbl_IdNo.Text)) & " and Received_PartyIdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_Empty_BeamBagCone_Processing_Details where DeliveryTo_Idno = " & Str(Val(lbl_IdNo.Text)) & " and ReceivedFrom_Idno = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_Pavu_Processing_Details where DeliveryTo_Idno = " & Str(Val(lbl_IdNo.Text)) & " and ReceivedFrom_Idno = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_Yarn_Processing_Details where DeliveryTo_Idno = " & Str(Val(lbl_IdNo.Text)) & " and ReceivedFrom_Idno = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ledger", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If

            cmd.Connection = con

            cmd.CommandText = "delete from Ledger_ItemName_Details where Ledger_Idno = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Ledger_AlaisHead where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from ledger_head where ledger_idno = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.Dispose()
            dt.Dispose()
            da.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE..", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
            Exit Sub

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        Dim da As New SqlClient.SqlDataAdapter("select ledger_idno, ledger_name from ledger_head where ledger_idno <> 0 and ledger_type = '" & Trim(vLedType) & "' order by ledger_idno", con)
        Dim dt As New DataTable

        da.Fill(dt)

        dgv_Filter.Columns.Clear()
        dgv_Filter.DataSource = dt
        dgv_Filter.RowHeadersVisible = False

        dgv_Filter.Columns(0).HeaderText = "IDNO"
        dgv_Filter.Columns(1).HeaderText = "LEDGER NAME"

        dgv_Filter.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        dgv_Filter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        dgv_Filter.Columns(0).FillWeight = 35
        dgv_Filter.Columns(1).FillWeight = 165

        grp_Filter.Visible = True
        grp_Filter.BringToFront()
        dgv_Filter.Focus()

        pnl_Back.Enabled = False

        da.Dispose()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer

        Try

            cmd.Connection = con
            cmd.CommandText = "select min(ledger_idno) from ledger_head Where ledger_idno <> 0 and ledger_type = '" & Trim(vLedType) & "'"

            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar
            End If

            cmd.Dispose()

            If movid <> 0 Then Call move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select max(ledger_idno) from ledger_head where ledger_idno <> 0 and ledger_type = '" & Trim(vLedType) & "'"

            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar
            End If

            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim cmd As New SqlClient.SqlCommand("select min(ledger_idno) from ledger_head where ledger_idno <> 0 and ledger_idno > " & Str(Val(lbl_IdNo.Text)) & " and ledger_type = '" & Trim(vLedType) & "'", con)
        Dim movid As Integer = 0

        Try
            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar
            End If

            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim movid As Integer = 0

        Try
            cmd.Connection = con
            cmd.CommandText = "select max(ledger_idno ) from ledger_head where ledger_idno <> 0 and ledger_idno < " & Str((lbl_IdNo.Text)) & " and ledger_type = '" & Trim(vLedType) & "'"

            movid = 0
            If IsDBNull(cmd.ExecuteScalar()) = False Then
                movid = cmd.ExecuteScalar()
            End If

            cmd.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING....", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_type = '" & Trim(vLedType) & "') order by Ledger_DisplayName", con)
        da.Fill(dt)

        cbo_Open.DataSource = dt
        cbo_Open.DisplayMember = "Ledger_DisplayName"

        da.Dispose()

        grp_Open.Visible = True
        grp_Open.BringToFront()
        If cbo_Open.Enabled And cbo_Open.Visible Then cbo_Open.Focus()
        pnl_Back.Enabled = False

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        'MessageBox.Show("insert record")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        'MessageBox.Show("Ledger creation  -  print")
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim dt As New DataTable
        Dim nr As Long = 0
        Dim acgrp_idno As Integer = 0
        Dim ar_idno As Integer = 0
        Dim trns_idno As Integer = 0
        Dim vPRICELIST_ID As Integer
        Dim Parnt_CD As String = ""
        Dim LedName As String = ""
        Dim SurName As String = ""
        Dim LedArName As String = ""
        Dim LedPhNo As String = ""
        Dim Sno As Integer = 0
        Dim pack_id As Integer = 0
        Dim Ag_IdNo As Integer = 0
        Dim Itm_ID As Integer = 0
        Dim sTATE_iD As Integer = 0

        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.Ledger_Creation, New_Entry) = False Then Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.FP_Ledger_Creation, New_Entry, Me) = False Then Exit Sub



        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        SurName = Common_Procedures.Remove_NonCharacters(txt_Name.Text)
        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid Legder Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled Then txt_Name.Focus()
            Exit Sub
        End If

        acgrp_idno = Common_Procedures.AccountsGroup_NameToIdNo(con, cbo_AcGroup.Text)
        If acgrp_idno = 0 Then
            MessageBox.Show("Invalid Accounts Group", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_AcGroup.Enabled Then cbo_AcGroup.Focus()
            Exit Sub
        End If

        Parnt_CD = Common_Procedures.AccountsGroup_IdNoToCode(con, acgrp_idno)

        If Trim(cbo_BillType.Text) = "" Then
            cbo_BillType.Text = "BALANCE ONLY"
            'MessageBox.Show("Invalid Bill Type", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            'If cbo_BillType.Enabled And cbo_BillType.Visible Then cbo_BillType.Focus()
            'Exit Sub
        End If

        vPRICELIST_ID = Common_Procedures.Price_List_NameToIdNo(con, cbo_PriceList.Text)

        With dgv_Details

            For i = 0 To .RowCount - 1

                Itm_ID = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(i).Cells(1).Value)


                If Val(Itm_ID) <> 0 Then

                    If Trim(.Rows(i).Cells(2).Value) = "" Then

                        MessageBox.Show("Invalid Party Item Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(2)
                            dgv_Details.CurrentCell.Selected = True
                        End If
                        Exit Sub

                    End If

                End If

            Next

        End With

        pack_id = Common_Procedures.PackingType_NameToIdNo(con, cbo_PackingType.Text)
        Ag_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)

        ar_idno = Common_Procedures.Area_NameToIdNo(con, cbo_Area.Text)
        trns_idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)

        LedName = Trim(txt_Name.Text)
        If Val(ar_idno) <> 0 Then
            LedName = Trim(txt_Name.Text) & " (" & Trim(cbo_Area.Text) & ")"
        End If

        SurName = Common_Procedures.Remove_NonCharacters(LedName)

        Verified_STS = 0
        If chk_Verified_Status.Checked = True Then Verified_STS = 1

        sTATE_iD = Common_Procedures.State_NameToIdNo(con, cbo_State.Text)


        tr = con.BeginTransaction

        Try

            cmd.Transaction = tr

            cmd.Connection = con

            If New_Entry = True Then
                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "ledger_head", "ledger_idno", "", tr)
                If Val(lbl_IdNo.Text) < 101 Then lbl_IdNo.Text = 101

                cmd.CommandText = "Insert into ledger_head ( Ledger_IdNo, Ledger_Name, Sur_Name, Ledger_MainName, Ledger_AlaisName, Area_IdNo, AccountsGroup_IdNo, Parent_Code, Bill_Type, Ledger_Address1, Ledger_Address2, Ledger_Address3, Ledger_Address4, Ledger_PhoneNo, Ledger_TinNo, Ledger_CstNo, Ledger_Type, Pan_No, Ledger_Emailid, Ledger_FaxNo, Ledger_MobileNo, Contact_Person, PackingType_CompanyIdNo, Ledger_AgentIdNo, Transport_Idno, Note , MobileNo_Sms ,Billing_Type , Sticker_Type , Mrp_Perc,Verified_Status,Ledger_GSTinNo,Ledger_State_IdNo ,Disc_Percentage, Duedate, PriceList_IdNo) Values (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(LedName) & "', '" & Trim(SurName) & "', '" & Trim(txt_Name.Text) & "', '" & Trim(txt_AlaisName.Text) & "', " & Str(Val(ar_idno)) & ", " & Str(Val(acgrp_idno)) & ", '" & Trim(Parnt_CD) & "', '" & Trim(cbo_BillType.Text) & "', '" & Trim(txt_Address1.Text) & "', '" & Trim(txt_Address2.Text) & "', '" & Trim(txt_Address3.Text) & "', '" & Trim(txt_Address4.Text) & "', '" & Trim(txt_PhoneNo.Text) & "', '" & Trim(txt_TinNo.Text) & "', '" & Trim(txt_CstNo.Text) & "', '" & Trim(vLedType) & "', '" & Trim(txt_PanNo.Text) & "', '" & Trim(txt_EMailID.Text) & "', '" & Trim(txt_FaxNo.Text) & "', '" & Trim(txt_MobileNo.Text) & "', '" & Trim(txt_ContactPerson.Text) & "', " & Str(Val(pack_id)) & ", " & Str(Val(Ag_IdNo)) & ", " & Str(Val(trns_idno)) & ", '" & Trim(txt_Remarks.Text) & "' , '" & Trim(txt_MobileSms.Text) & "' , '" & Trim(txt_BillingType.Text) & "' ,'" & Trim(cbo_StickerType.Text) & "' , '" & Trim(txt_MrpPerc.Text) & "'," & Val(Verified_STS) & ",'" & Trim(txt_GSTIN_No.Text) & "'," & Str(sTATE_iD) & "," & Str(Val(txt_DiscPercentage.Text)) & " ," & Str(Val(txt_Duedate.Text)) & "," & Str(Val(vPRICELIST_ID)) & ")"

            Else
                cmd.CommandText = "Update ledger_head set Ledger_Name = '" & Trim(LedName) & "', Sur_Name = '" & Trim(SurName) & "', Ledger_MainName = '" & Trim(txt_Name.Text) & "', Ledger_AlaisName = '" & Trim(txt_AlaisName.Text) & "', Area_IdNo = " & Str(Val(ar_idno)) & ", AccountsGroup_IdNo = " & Str(Val(acgrp_idno)) & ", Parent_Code = '" & Trim(Parnt_CD) & "', Bill_Type = '" & Trim(cbo_BillType.Text) & "', Ledger_Address1 = '" & Trim(txt_Address1.Text) & "', Ledger_Address2 = '" & Trim(txt_Address2.Text) & "', Ledger_Address3 = '" & Trim(txt_Address3.Text) & "', Ledger_Address4 = '" & Trim(txt_Address4.Text) & "', Ledger_PhoneNo = '" & Trim(txt_PhoneNo.Text) & "', Ledger_TinNo = '" & Trim(txt_TinNo.Text) & "', Ledger_CstNo = '" & Trim(txt_CstNo.Text) & "', Pan_No = '" & Trim(txt_PanNo.Text) & "', Ledger_Emailid = '" & Trim(txt_EMailID.Text) & "', Ledger_FaxNo = '" & Trim(txt_FaxNo.Text) & "', Ledger_MobileNo = '" & Trim(txt_MobileNo.Text) & "', Contact_Person = '" & Trim(txt_ContactPerson.Text) & "', PackingType_CompanyIdNo = " & Str(Val(pack_id)) & ", Ledger_AgentIdNo = " & Str(Val(Ag_IdNo)) & ", Transport_Idno = " & Str(Val(trns_idno)) & ", Note = '" & Trim(txt_Remarks.Text) & "' , MobileNo_Sms =  '" & Trim(txt_MobileSms.Text) & "' ,Billing_Type = '" & Trim(txt_BillingType.Text) & "' , Sticker_Type = '" & Trim(cbo_StickerType.Text) & "' , Mrp_Perc =  '" & Trim(txt_MrpPerc.Text) & "', Verified_Status = " & Val(Verified_STS) & ",Ledger_GSTinNo='" & Trim(txt_GSTIN_No.Text) & "',Ledger_State_IdNo=" & Str(sTATE_iD) & " , Disc_Percentage = " & Str(Val(txt_DiscPercentage.Text)) & " ,Duedate = " & Str(Val(txt_Duedate.Text)) & " , PriceList_IdNo = " & Str(Val(vPRICELIST_ID)) & " where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text))

            End If

            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Ledger_AlaisHead where Ledger_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            LedArName = Trim(txt_Name.Text)
            If Val(ar_idno) <> 0 Then
                LedArName = Trim(txt_Name.Text) & " (" & Trim(cbo_Area.Text) & ")"
            End If

            cmd.CommandText = "Insert into Ledger_AlaisHead(Ledger_IdNo, Sl_No, Ledger_DisplayName, AccountsGroup_IdNo, Ledger_Type , Verified_Status , Area_IdNo ) Values (" & Str(Val(lbl_IdNo.Text)) & ", 1, '" & Trim(LedArName) & "', " & Str(Val(acgrp_idno)) & ", '" & Trim(vLedType) & "' , " & Val(Verified_STS) & " , " & Str(Val(ar_idno)) & ")"
            cmd.ExecuteNonQuery()

            If Trim(txt_AlaisName.Text) <> "" Then
                LedArName = Trim(txt_AlaisName.Text)
                If Val(ar_idno) <> 0 Then
                    LedArName = Trim(txt_AlaisName.Text) & " (" & Trim(cbo_Area.Text) & ")"
                End If

                cmd.CommandText = "Insert into Ledger_AlaisHead(Ledger_IdNo, Sl_No, Ledger_DisplayName, AccountsGroup_IdNo, Ledger_Type , Verified_Status , Area_IdNo ) Values (" & Str(Val(lbl_IdNo.Text)) & ", 2, '" & Trim(LedArName) & "', " & Str(Val(acgrp_idno)) & ", '" & Trim(vLedType) & "' , " & Val(Verified_STS) & " , " & Str(Val(ar_idno)) & ")"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "delete from Ledger_ItemName_Details where Ledger_Idno = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0

                For i = 0 To .RowCount - 1

                    Itm_ID = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                    If Val(Itm_ID) <> 0 And Trim(.Rows(i).Cells(1).Value) <> "" Then

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Ledger_ItemName_Details(Ledger_Idno, sl_No, Item_Idno, Party_ItemName ,Rate_Disc_Percentage ,Rate_Disc_Amount) values (" & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(Sno)) & ", " & Str(Val(Itm_ID)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "' ," & Val(.Rows(i).Cells(3).Value) & "," & Val(.Rows(i).Cells(4).Value) & " )"
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With

            tr.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(LedName)
            Common_Procedures.Master_Return.Master_Type = "LEDGER"



            MessageBox.Show("Sucessfully Saved", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)


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
            tr.Rollback()

            If InStr(1, Trim(LCase(ex.Message)), "ix_ledger_head") > 0 Then
                MessageBox.Show("Duplicate Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), "ix_ledger_alaishead") > 0 Then
                MessageBox.Show("Duplicate Ledger Alais Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Finally


            tr.Dispose()
            dt.Dispose()
            cmd.Dispose()

            If txt_Name.Enabled Then txt_Name.Focus()

        End Try

    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        save_record()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub LedgerCreation_Processing_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Area.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "AREA" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_Area.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If
        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If
        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Agent.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "AGENT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_Agent.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If

        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_ItemName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "FINISHEDPRODUCT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_Grid_ItemName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If

        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PackingType.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COMPANY" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_PackingType.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If

        Common_Procedures.Master_Return.Master_Type = ""
        Common_Procedures.Master_Return.Return_Value = ""

    End Sub

    Private Sub LedgerCreation_Processing_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim CompCondt As String = ""

        vLedType = Trim(Common_Procedures.MDI_LedType)

        If Val(Common_Procedures.User.IdNo) <> 1 And Common_Procedures.UR.Ledger_Verifition = "" Then chk_Verified_Status.Enabled = False


        Me.BackColor = Color.LightSkyBlue
        lbl_Heading.Text = "LEDGER CREATION"
        Me.Text = "LEDGER CREATION"

        con.Open()

        da = New SqlClient.SqlDataAdapter("select AccountsGroup_Name from AccountsGroup_Head Order by AccountsGroup_Name", con)
        'da.Fill(dt1)
        'cbo_AcGroup.DataSource = dt1
        'cbo_AcGroup.DisplayMember = "AccountsGroup_Name"

        cbo_BillType.Items.Clear()
        cbo_BillType.Items.Add("BALANCE ONLY")
        cbo_BillType.Items.Add("BILL TO BILL")


        da = New SqlClient.SqlDataAdapter("select distinct(Sticker_Type) from Ledger_Head order by Sticker_Type", con)
        da.Fill(dt1)
        cbo_StickerType.DataSource = dt1
        cbo_StickerType.DisplayMember = "Sticker_Type"

        'da = New SqlClient.SqlDataAdapter("select Area_Name from Area_Head Order by Area_Name", con)
        'da.Fill(dt2)
        'cbo_Area.DataSource = dt2
        'cbo_Area.DisplayMember = "Area_Name"

        'da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head Order by Processed_Item_Name", con)
        'da.Fill(dt3)
        'cbo_Grid_ItemName.DataSource = dt3
        'cbo_Grid_ItemName.DisplayMember = "Processed_Item_Name"

        CompCondt = ""
        If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
            CompCondt = " Where (Company_Type <> 'UNACCOUNT' or Company_IdNo = 0)"
        End If

        'da = New SqlClient.SqlDataAdapter("select Company_ShortName from Company_Head  " & CompCondt & " order by Company_ShortName", con)
        'da.Fill(dt4)
        'cbo_PackingType.DataSource = dt4
        'cbo_PackingType.DisplayMember = "Company_ShortName"

        'da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where (ledger_type = 'AGENT' or Ledger_IdNo = 0) order by Ledger_DisplayName", con)
        'da.Fill(dt5)
        'cbo_Agent.DataSource = dt5
        'cbo_Agent.DisplayMember = "Ledger_DisplayName"

        da.Dispose()

        grp_Open.Visible = False
        grp_Open.Left = (Me.Width - grp_Open.Width) - 100
        grp_Open.Top = (Me.Height - grp_Open.Height) - 100

        grp_Filter.Visible = False
        grp_Filter.Left = (Me.Width - grp_Filter.Width) - 100
        grp_Filter.Top = (Me.Height - grp_Filter.Height) - 100

        AddHandler txt_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AlaisName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_AcGroup.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BillType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Area.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Verified_Status.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Address1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Address2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Address3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Address4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PhoneNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_FaxNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_MobileNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EMailID.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TinNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CstNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PanNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ContactPerson.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PackingType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Agent.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_MobileSms.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BillingType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_StickerType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PriceList.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_MrpPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Open.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_GSTIN_No.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_State.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscPercentage.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Duedate.GotFocus, AddressOf ControlGotFocus



        AddHandler cbo_Open.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AlaisName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_AcGroup.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BillType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Area.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_Verified_Status.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Address1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Address2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Address3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Address4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PhoneNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_FaxNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_MobileNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EMailID.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TinNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CstNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PanNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ContactPerson.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PackingType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Agent.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_MobileSms.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BillingType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_StickerType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PriceList.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_MrpPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_GSTIN_No.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_State.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiscPercentage.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Duedate.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_AlaisName.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Address1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Address2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Address3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Address4.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PhoneNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_FaxNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_MobileNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_EMailID.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TinNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CstNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PanNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ContactPerson.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_MobileSms.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BillingType.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Remarks.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_MrpPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_GSTIN_No.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_Name.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AlaisName.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Address1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Address2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Address3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Address4.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PhoneNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_FaxNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_MobileNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_EMailID.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TinNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CstNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PanNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ContactPerson.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_MobileSms.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BillingType.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Remarks.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_GSTIN_No.KeyPress, AddressOf TextBoxControlKeyPress

        new_record()

    End Sub

    Private Sub LedgerCreation_Processing_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
        Common_Procedures.MDI_LedType = ""
    End Sub

    Private Sub LedgerCreation_Processing_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Open.Visible Then
                btn_CloseOpen_Click(sender, e)
            ElseIf grp_Filter.Visible Then
                btn_CloseFilter_Click(sender, e)
            Else
                Me.Close()
            End If

        End If
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        If keyData = Keys.Enter Then

            On Error Resume Next

            If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

                dgv1 = dgv_Details

                With dgv1

                    If .CurrentCell.ColumnIndex = .ColumnCount - 1 Then

                        If .CurrentCell.RowIndex = .RowCount - 1 Then

                            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                save_record()

                            Else
                                txt_Name.Focus()
                                Return True
                                Exit Function

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                        End If

                    Else

                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And (Trim(.CurrentRow.Cells(1).Value) = "" And Trim(.CurrentRow.Cells(2).Value) = "") Then
                            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                save_record()

                            Else
                                txt_Name.Focus()
                                Return True
                                Exit Function

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If


                    End If

                End With

            Else

                Return MyBase.ProcessCmdKey(msg, keyData)
                'SendKeys.Send("{TAB}")

            End If

            Return True

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Private Sub btn_CloseOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseOpen.Click
        pnl_Back.Enabled = True
        grp_Open.Visible = False
        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
    End Sub

    Private Sub btn_Find_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Find.Click
        Dim movid As Integer


        movid = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Open.Text)

        If movid <> 0 Then move_record(movid)

        pnl_Back.Enabled = True
        grp_Open.Visible = False

    End Sub

    Private Sub cbo_Open_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Open.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '" & Trim(vLedType) & "')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Open_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Open.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Open, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '" & Trim(vLedType) & "')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Open_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Open.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Open, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '" & Trim(vLedType) & "')", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            Call btn_Find_Click(sender, e)
        End If
    End Sub

    Private Sub btn_CloseFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseFilter.Click
        grp_Filter.Visible = False
    End Sub

    Private Sub btn_Filter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter.Click
        Dim idno As Integer

        idno = Val(dgv_Filter.CurrentRow.Cells(0).Value)

        If Val(idno) <> 0 Then
            move_record(idno)
            pnl_Back.Enabled = True
            grp_Filter.Visible = False
        End If

    End Sub

    Private Sub dgv_Filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
        If e.KeyValue = 13 Then
            Call btn_Filter_Click(sender, e)
        End If
    End Sub

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Name.KeyPress
        If Asc(e.KeyChar) = 39 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Name.KeyDown
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 38 Then
            If dgv_Details.Rows.Count <= 0 Then dgv_Details.Rows.Add()
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        End If
    End Sub

    Private Sub cbo_AcGroup_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_AcGroup.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "AccountsGroup_Head", "AccountsGroup_Name", "", "")

    End Sub

    Private Sub cbo_AcGroup_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_AcGroup.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_AcGroup, txt_AlaisName, cbo_BillType, "AccountsGroup_Head", "AccountsGroup_Name", "", "")
    End Sub

    Private Sub cbo_AcGroup_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_AcGroup.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_AcGroup, cbo_BillType, "AccountsGroup_Head", "AccountsGroup_Name", "", "")
    End Sub

    Private Sub cbo_BillType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BillType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_BillType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BillType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BillType, cbo_AcGroup, cbo_Area, "", "", "", "")
    End Sub

    Private Sub cbo_BillType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BillType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BillType, cbo_Area, "", "", "", "")
    End Sub

    Private Sub txt_AlaisName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AlaisName.KeyPress
        If Asc(e.KeyChar) = 34 Or Asc(e.KeyChar) = 39 Then
            e.Handled = True
        End If
    End Sub

    Private Sub cbo_Area_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Area.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Area_Head", "Area_Name", "", "")

    End Sub

    Private Sub cbo_Area_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Area.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Area, cbo_BillType, txt_Address1, "Area_Head", "Area_Name", "", "")
    End Sub

    Private Sub cbo_Area_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Area.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Area, txt_Address1, "Area_Head", "Area_Name", "", "")

    End Sub

    Private Sub cbo_Area_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Area.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Area_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Area.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_PackingType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PackingType.GotFocus
        'Dim CompCondt As String

        'CompCondt = ""
        'If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
        '    CompCondt = "(Company_Type <> 'UNACCOUNT')"
        'End If

        'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Company_Head", "Company_ShortName", CompCondt, "(Company_IdNo = 0)")
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Packing_Type_Head", "Packing_Type_Name", "", "(Packing_Type_IdNo = 0)")

    End Sub

    Private Sub cbo_PackingType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PackingType.KeyDown
        'Dim CompCondt As String

        'CompCondt = ""
        'If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
        '    CompCondt = "(Company_Type <> 'UNACCOUNT')"
        'End If

        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PackingType, txt_ContactPerson, cbo_Agent, "Company_Head", "Company_ShortName", CompCondt, "(Company_IdNo = 0)")
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PackingType, txt_ContactPerson, cbo_Agent, "Packing_Type_Head", "Packing_Type_Name", "", "(Packing_Type_IdNo = 0)")

    End Sub

    Private Sub cbo_PackingType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PackingType.KeyPress
        'Dim CompCondt As String

        'CompCondt = ""
        'If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
        '    CompCondt = "(Company_Type <> 'UNACCOUNT')"
        'End If

        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PackingType, cbo_Agent, "Company_Head", "Company_ShortName", CompCondt, "(Company_IdNo = 0)")
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PackingType, cbo_Agent, "Packing_Type_Head", "Packing_Type_Name", "", "(Packing_Type_IdNo = 0)")

    End Sub
    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT ' and Verified_Status = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_Agent, txt_Remarks, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT' and Verified_Status = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, txt_Remarks, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT' and Verified_Status = 1)", "(Ledger_IdNo = 0)")

    End Sub
    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Common_Procedures.MDI_LedType = "TRANSPORT"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Transport.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub
    Private Sub cbo_Agent_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Agent.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Agent_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Agent, cbo_PackingType, cbo_Transport, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Agent_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Agent.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Agent, cbo_Transport, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub
    Private Sub txt_MrpPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_MrpPerc.KeyDown
        Dim Position As Integer = 0
        Dim LIne As Integer = 0
        Dim TotLInes As Integer = 0

        If e.Control = True And e.KeyCode = 40 Then
            'dgv_Details.Focus()
            'If dgv_Details.Rows.Count = 0 Then dgv_Details.Rows.Add()
            'dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            'SendKeys.Send("{TAB}")
            txt_DiscPercentage.Focus()
        End If

        'If e.KeyCode = 38 Then
        '    SendKeys.Send("+{TAB}")
        'End If

        'If e.KeyCode = 40 Then
        '    TotLInes = txt_Remarks.GetLineFromCharIndex(1)
        '    Position = txt_Remarks.SelectionStart
        '    LIne = txt_Remarks.GetLineFromCharIndex(Position)

        '    If LIne = TotLInes Then
        '        dgv_Details.Focus()
        '        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        '    End If
        'End If

        'If e.KeyCode = 38 Then
        '    Position = txt_Remarks.SelectionStart
        '    LIne = txt_Remarks.GetLineFromCharIndex(Position)
        '    If LIne <= 1 Then
        '        txt_Transport.Focus()
        '    End If
        'End If
    End Sub

    Private Sub txt_MrpPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_MrpPerc.KeyPress
        'Dim ar() As String

        If Asc(e.KeyChar) = 13 Then
            '    ar = Split(Trim(txt_Remarks.Text), Environment.NewLine.ToString)

            '    If UBound(ar) >= 0 Then
            '        If Trim(ar(UBound(ar))) = "" Then
            '            dgv_Details.Focus()
            '            If dgv_Details.Rows.Count = 0 Then dgv_Details.Rows.Add()
            '            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            '            e.Handled = True
            cbo_PriceList.Focus()
            'txt_DiscPercentage.Focus()
            '        End If
            '    End If
        End If
    End Sub


    Private Sub txt_Name_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Name.TextChanged

    End Sub

    Private Sub cbo_Grid_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ItemName.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Processed_Item_Head", "Processed_Item_Name", "", "(Processed_Item_IdNo = 0)")

    End Sub

    Private Sub cbo_Grid_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ItemName.KeyDown
        Try
            With cbo_Grid_ItemName
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True

                    If Val(dgv_Details.CurrentCell.RowIndex) <= 0 Then
                        txt_MrpPerc.Focus()

                    Else
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex - 1).Cells(2)

                    End If

                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True

                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(dgv_Details.CurrentCell.ColumnIndex + 1)
                    dgv_Details.CurrentCell.Selected = True

                ElseIf e.KeyValue = 46 Then

                    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_ItemName, Nothing, Nothing, "Processed_Item_Head", "Processed_Item_Name", "", "(Processed_Item_IdNo = 0)")


                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 Then



                    If .DroppedDown = False Then .DroppedDown = True

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Grid_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_ItemName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_ItemName, Nothing, "Processed_Item_Head", "Processed_Item_Name", "", "(Processed_Item_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Grid_ItemName.Text)

            If dgv_Details.CurrentRow.Index = dgv_Details.RowCount - 1 And dgv_Details.CurrentCell.ColumnIndex >= 1 And Trim(dgv_Details.CurrentRow.Cells(1).Value) = "" Then
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    txt_Name.Focus()
                End If

            Else
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(dgv_Details.CurrentCell.ColumnIndex + 1)
                dgv_Details.CurrentCell.Selected = True

            End If
        End If
    End Sub

    Private Sub cbo_Grid_ItemName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ItemName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New FinishedProduct_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_ItemName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_ItemName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ItemName.TextChanged
        Try
            If cbo_Grid_ItemName.Visible Then
                With dgv_Details
                    If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                    If Val(cbo_Grid_ItemName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_ItemName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "TSOFT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable

        With dgv_Details
            If e.ColumnIndex = 1 Then

                If cbo_Grid_ItemName.Visible = False Or Val(cbo_Grid_ItemName.Tag) <> e.RowIndex Then

                    cbo_Grid_ItemName.Tag = -100

                    Da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head order by Processed_Item_Name", con)
                    Da.Fill(Dt1)
                    cbo_Grid_ItemName.DataSource = Dt1
                    cbo_Grid_ItemName.DisplayMember = "Processed_Item_Name"

                    cbo_Grid_ItemName.Left = .Left + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_ItemName.Top = .Top + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_ItemName.Width = .CurrentCell.Size.Width
                    cbo_Grid_ItemName.Text = Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_ItemName.Tag = Val(.CurrentCell.RowIndex)
                    cbo_Grid_ItemName.Visible = True

                    cbo_Grid_ItemName.BringToFront()
                    cbo_Grid_ItemName.Focus()

                End If


            Else
                cbo_Grid_ItemName.Visible = False

            End If

        End With

    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        On Error Resume Next

        With dgv_Details
            If e.KeyCode = Keys.Up Then
                If .CurrentRow.Index <= 0 Then
                    txt_Remarks.Focus()
                End If
            End If

            If e.KeyCode = Keys.Left Then
                If .CurrentCell.RowIndex <= 0 And .CurrentCell.ColumnIndex <= 1 Then
                    txt_Remarks.Focus()
                End If
            End If

        End With

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer
        Dim i As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details
                If .CurrentRow.Index = .RowCount - 1 Then
                    For i = 1 To .ColumnCount - 1
                        .Rows(.CurrentRow.Index).Cells(i).Value = ""
                    Next

                Else
                    n = .CurrentRow.Index
                    .Rows.RemoveAt(n)

                End If

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With

        End If
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        Grid_DeSelect()
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        With dgv_Details
            If Val(.Rows(.RowCount - 1).Cells(0).Value) = 0 Then
                .Rows(.RowCount - 1).Cells(0).Value = .RowCount
            End If
        End With
    End Sub

    Private Sub cbo_Agent_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Agent_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Agent.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_PackingType_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PackingType.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Company_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PackingType.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub


    Private Sub btn_SendSMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SendSMS.Click
        Dim smstxt As String = ""
        Dim PhNo As String = ""
        Dim SMS_SenderID As String = ""
        Dim SMS_Key As String = ""
        Dim SMS_RouteID As String = ""
        Dim SMS_Type As String = ""

        Try

            PhNo = Trim(txt_MobileSms.Text)

            smstxt = Trim(txt_Name.Text)
            If Trim(txt_Address1.Text) <> "" Then smstxt = smstxt & "%2C+" & Trim(txt_Address1.Text)
            If Trim(txt_Address2.Text) <> "" Then smstxt = smstxt & "%2C+" & Trim(txt_Address2.Text)
            If Trim(txt_Address3.Text) <> "" Then smstxt = smstxt & "%2C+" & Trim(txt_Address3.Text)
            If Trim(txt_Address4.Text) <> "" Then smstxt = smstxt & "%2C+" & Trim(txt_Address4.Text)
            If Trim(txt_TinNo.Text) <> "" Then smstxt = smstxt & "%2C+" & "TIN NO : " & Trim(txt_TinNo.Text)
            If Trim(txt_PhoneNo.Text) <> "" Or Trim(txt_MobileNo.Text) <> "" Then smstxt = smstxt & "%2C+" & "PHONE NO : " & Trim(txt_PhoneNo.Text) & "%2C+" & Trim(txt_MobileNo.Text)

            SMS_SenderID = ""
            SMS_Key = ""
            SMS_RouteID = ""
            SMS_Type = ""

            Common_Procedures.get_SMS_Provider_Details(con, 0, SMS_SenderID, SMS_Key, SMS_RouteID, SMS_Type)

            Sms_Entry.SMSProvider_SenderID = SMS_SenderID
            Sms_Entry.SMSProvider_Key = SMS_Key
            Sms_Entry.SMSProvider_RouteID = SMS_RouteID
            Sms_Entry.SMSProvider_Type = SMS_Type

            Sms_Entry.vSmsPhoneNo = Trim(PhNo)
            Sms_Entry.vSmsMessage = Trim(smstxt)

            Dim f1 As New Sms_Entry
            f1.MdiParent = MDIParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_StickerType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_StickerType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_Head", "Sticker_Type", "", "")
    End Sub

    Private Sub cbo_StickerType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_StickerType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_StickerType, txt_BillingType, txt_MrpPerc, "Ledger_Head", "Sticker_Type", "", "")

    End Sub

    Private Sub cbo_StickerType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_StickerType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_StickerType, txt_MrpPerc, "Ledger_Head", "Sticker_Type", "", "", False)
    End Sub
    Private Sub cbo_State_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_State.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "State_Head", "State_Name", "", "(State_Idno = 0)")

    End Sub

    Private Sub cbo_State_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_State.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_State, txt_Address4, txt_PhoneNo, "State_Head", "State_Name", "", "(State_Idno = 0)")
    End Sub

    Private Sub cbo_State_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_State.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_State, txt_PhoneNo, "State_Head", "State_Name", "", "(State_Idno = 0)")
    End Sub

    Private Sub txt_DiscPercentage_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DiscPercentage.KeyDown
        If e.KeyCode = 38 Then
            cbo_PriceList.Focus()
        End If
        If e.KeyCode = 40 Then
            'dgv_Details.Focus()
            'If dgv_Details.Rows.Count = 0 Then dgv_Details.Rows.Add()
            'dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            txt_Duedate.Focus()
        End If
    End Sub

    Private Sub txt_Duedate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Duedate.KeyDown
        If e.KeyCode = 38 Then
            txt_DiscPercentage.Focus()
        End If
        If e.KeyCode = 40 Then
            dgv_Details.Focus()
            If dgv_Details.Rows.Count = 0 Then dgv_Details.Rows.Add()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        End If
    End Sub

    Private Sub txt_Duedate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Duedate.KeyPress


        If Asc(e.KeyChar) = 13 Then


            dgv_Details.Focus()
            If dgv_Details.Rows.Count = 0 Then dgv_Details.Rows.Add()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            e.Handled = True

        End If

    End Sub

    Private Sub txt_DiscPercentage_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DiscPercentage.KeyPress


        If Asc(e.KeyChar) = 13 Then

            txt_Duedate.Focus()

        End If
    End Sub

    Private Sub cbo_PriceList_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PriceList.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Garments_Price_List_Head", "Price_List_Name", "", "(Price_List_IdNo=0)")
    End Sub

    Private Sub cbo_PriceList_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PriceList.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, txt_MrpPerc, txt_DiscPercentage, "Garments_Price_List_Head", "Price_List_Name", "", "(Price_List_IdNo=0)")
    End Sub

    Private Sub cbo_PriceList_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PriceList.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, txt_DiscPercentage, "Garments_Price_List_Head", "Price_List_Name", "", "(Price_List_IdNo=0)")
    End Sub

End Class
