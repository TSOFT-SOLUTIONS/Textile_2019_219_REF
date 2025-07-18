
Public Class FinishedProduct_Creation_Simple

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False
    Private Prec_ActCtrl As New Control
    Private Verified_STS As Integer = 0
    Private vcbo_KeyDwnVal As Double


    Private Sub clear()

        'Insert_Entry = False
        grp_Back.Enabled = True
        grp_Filter.Visible = False
        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black
        lbl_DisplaySlNo.Text = ""
        lbl_DisplaySlNo.ForeColor = Color.Black
        txt_Code.Text = ""
        txt_CostRate.Text = ""
        txt_TaxPerc.Text = ""
        txt_Rate.Text = ""
        txt_TaxRate.Text = ""
        txt_Meter_Qty.Text = ""
        txt_Weight_Piece.Text = ""
        txt_Width.Text = ""
        txt_MinimumStock.Text = ""
        cbo_ItemGroup.Text = ""
        cbo_Unit.Text = ""
        txt_Name.Text = ""
        If Trim(Common_Procedures.settings.CustomerCode) = "1558 " Then ' --- SOTEXPA
            cbo_Reconsilation_Meter_Weight.Text = "WEIGHT"
        Else
            cbo_Reconsilation_Meter_Weight.Text = "METER"
        End If
        chk_Verified_Status.Checked = False
 
        New_Entry = False

        grp_Open.Visible = False
       
        New_Entry = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        Me.ActiveControl.BackColor = Color.Lime   'Color.FromArgb(128, 128, 255)  ' Color.LightBlue  ' Color.lime
        Me.ActiveControl.ForeColor = Color.Blue

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

       

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            Prec_ActCtrl.BackColor = Color.White
            Prec_ActCtrl.ForeColor = Color.Black
        End If

        'Me.ActiveControl.BackColor = Color.White
        'Me.ActiveControl.ForeColor = Color.Black
    End Sub
    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        On Error Resume Next
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub
    

    Private Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
     
        If Val(idno) = 0 Then Exit Sub

        clear()

        Try

            da = New SqlClient.SqlDataAdapter("select a.*, b.ItemGroup_Name, c.Unit_Name from Processed_Item_Head a LEFT OUTER JOIN ItemGroup_Head b ON a.Processed_ItemGroup_IdNo = b.ItemGroup_IdNo LEFT OUTER JOIN Unit_Head c ON a.Unit_IdNo = c.Unit_IdNo where a.Processed_Item_IdNo = " & Str(Val(idno)) & " and Processed_Item_Type = 'FP'", con)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then

                If IsDBNull(dt.Rows(0).Item("Processed_Item_IdNo").ToString) = False Then
                    lbl_IdNo.Text = dt.Rows(0).Item("Processed_Item_IdNo").ToString
                    lbl_DisplaySlNo.Text = dt.Rows(0).Item("Processed_Item_DisplaySlNo").ToString
                    txt_Name.Text = dt.Rows(0).Item("Processed_Item_Nm").ToString
                    txt_Code.Text = dt.Rows(0).Item("Processed_Item_Code").ToString

                    cbo_ItemGroup.Text = dt.Rows(0).Item("ItemGroup_Name").ToString
                    cbo_Unit.Text = dt.Rows(0).Item("Unit_Name").ToString

                    cbo_Reconsilation_Meter_Weight.Text = dt.Rows(0).Item("Reconsilation_Meter_Weight").ToString

                    If Val(dt.Rows(0).Item("Meter_Qty").ToString) <> 0 Then
                        txt_Meter_Qty.Text = Format(Val(dt.Rows(0).Item("Meter_Qty").ToString), "#######0.00")
                    End If
                    ' txt_Meter_Qty.Text = dt.Rows(0).Item("Meter_Qty").ToString
                    If Val(dt.Rows(0).Item("Weight_Piece").ToString) <> 0 Then
                        txt_Weight_Piece.Text = dt.Rows(0).Item("Weight_Piece").ToString
                    End If
                    If Val(dt.Rows(0).Item("Width").ToString) <> 0 Then
                        txt_Width.Text = dt.Rows(0).Item("Width").ToString
                    End If
                    If Val(dt.Rows(0).Item("Minimum_Stock").ToString) <> 0 Then
                        txt_MinimumStock.Text = dt.Rows(0).Item("Minimum_Stock").ToString
                    End If
                    If Val(dt.Rows(0).Item("Tax_Percentage").ToString) <> 0 Then
                        txt_TaxPerc.Text = dt.Rows(0).Item("Tax_Percentage").ToString
                    End If
                    If Val(dt.Rows(0).Item("Sale_TaxRate").ToString) <> 0 Then
                        txt_TaxRate.Text = dt.Rows(0).Item("Sale_TaxRate").ToString
                    End If
                    If Val(dt.Rows(0).Item("Sales_Rate").ToString) <> 0 Then
                        txt_Rate.Text = dt.Rows(0).Item("Sales_Rate").ToString
                    End If
                    If Val(dt.Rows(0).Item("Cost_Rate").ToString) <> 0 Then
                        txt_CostRate.Text = dt.Rows(0).Item("Cost_Rate").ToString

                    End If
                    If Val(dt.Rows(0).Item("Verified_Status").ToString) = 1 Then chk_Verified_Status.Checked = True


                    End If
                    End If

                  
            dt.Clear()
            dt.Dispose()


        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try


        If txt_Name.Visible And txt_Name.Enabled Then txt_Name.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.FinishedProduct_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.FinishedProduct_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If
        If New_Entry = True Then
            MessageBox.Show("This is new entry", "DOES NOT DELETION....", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_Item_Processing_Details where Item_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Finished Product", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If
            dt.Clear()

            da = New SqlClient.SqlDataAdapter("select count(*) from FinishedProduct_Order_Details where FinishedProduct_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Finished Product", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If
            dt.Clear()

            da = New SqlClient.SqlDataAdapter("select count(*) from FinishedProduct_Invoice_Details where FinishedProduct_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Finished Product", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If
            dt.Clear()

            da = New SqlClient.SqlDataAdapter("select count(*) from Ledger_ItemName_Details where Item_Idno = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Finished Product", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If
            dt.Clear()

            da = New SqlClient.SqlDataAdapter("select count(*) from Processed_Item_Details where Finished_Product_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Finished Product", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If
            dt.Clear()

            cmd.Connection = con
            cmd.CommandText = "delete from Processed_Item_Head where Processed_Item_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            new_record()

            MessageBox.Show("Deleted Successfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If txt_Name.Enabled = True And txt_Name.Visible = True Then txt_Name.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("select a.Processed_Item_IdNo, a.Processed_Item_Name, b.unit_name, a.Sale_TaxRate from Processed_Item_Head a, unit_head b where a.Processed_Item_Type = 'FP' and a.unit_idno = b.unit_idno Order by a.Processed_Item_IdNo", con)
        da.Fill(dt)

        dgv_Filter.Columns.Clear()
        dgv_Filter.DataSource = dt

        dgv_Filter.RowHeadersVisible = False

        dgv_Filter.Columns(0).HeaderText = "IDNO"
        dgv_Filter.Columns(1).HeaderText = "ITEM NAME"
        dgv_Filter.Columns(2).HeaderText = "UNIT"
        dgv_Filter.Columns(3).HeaderText = "SALES RATE"

        dgv_Filter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        dgv_Filter.Columns(0).FillWeight = 40
        dgv_Filter.Columns(1).FillWeight = 240
        dgv_Filter.Columns(2).FillWeight = 60
        dgv_Filter.Columns(3).FillWeight = 60

        grp_Back.Enabled = False
        grp_Filter.Visible = True

        dgv_Filter.BringToFront()
        dgv_Filter.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movid As Integer = 0

        Try
            cmd.Connection = con
            cmd.CommandText = "select TOP 1 Processed_Item_IdNo from Processed_Item_Head WHERE Processed_Item_Type = 'FP' ORDER BY Processed_Item_DisplaySlNo"
            dr = cmd.ExecuteReader

            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movid = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movid As Integer = 0

        Try
            cmd.Connection = con
            cmd.CommandText = "select TOP 1 Processed_Item_IdNo from Processed_Item_Head WHERE Processed_Item_Type = 'FP' ORDER BY Processed_Item_DisplaySlNo desc"
            dr = cmd.ExecuteReader

            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movid = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record

        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movid As Integer = 0

        Try
            cmd.Connection = con
            cmd.CommandText = "select TOP 1 Processed_Item_IdNo from Processed_Item_Head WHERE Processed_Item_DisplaySlNo > " & Val(lbl_DisplaySlNo.Text) & " and  Processed_Item_Type = 'FP' ORDER BY Processed_Item_DisplaySlNo"
            dr = cmd.ExecuteReader

            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movid = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movid As Integer = 0

        Try
            cmd.Connection = con
            cmd.CommandText = "select TOP 1 Processed_Item_IdNo from Processed_Item_Head WHERE Processed_Item_DisplaySlNo < " & Val(lbl_DisplaySlNo.Text) & " and  Processed_Item_Type = 'FP' ORDER BY Processed_Item_DisplaySlNo desc"
            dr = cmd.ExecuteReader

            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movid = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        clear()

        Try

            New_Entry = True

            lbl_IdNo.Text = Val(Common_Procedures.get_MaxIdNo(con, "Processed_Item_Head", "Processed_Item_IdNo", ""))
            lbl_IdNo.ForeColor = Color.Red

            lbl_DisplaySlNo.Text = Val(Common_Procedures.get_MaxIdNo(con, "Processed_Item_Head", "Processed_Item_DisplaySlNo", "(Processed_Item_Type= 'FP')"))
            lbl_DisplaySlNo.ForeColor = Color.Red

            Da1 = New SqlClient.SqlDataAdapter("select a.* from Processed_Item_Head a where a.Processed_Item_Idno <> 0 order by  Processed_Item_Idno desc", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                If Dt1.Rows(0).Item("Reconsilation_Meter_Weight").ToString <> "" Then cbo_Reconsilation_Meter_Weight.Text = Dt1.Rows(0).Item("Reconsilation_Meter_Weight").ToString
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR New RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head where Processed_Item_IdNo = 0 Or Processed_Item_Type = 'FP' order by Processed_Item_Name", con)
            da.Fill(dt)

        'cbo_Open.Items.Clear()

        cbo_Open.DataSource = dt
        cbo_Open.DisplayMember = "Processed_Item_Name"

        grp_Open.Visible = True
        grp_Open.BringToFront()
        If cbo_Open.Enabled And cbo_Open.Visible Then cbo_Open.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        'MessageBox.Show("insert record")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '---
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim ProdName As String = ""
        Dim Sur As String = ""
        Dim Nr As Long = 0
        Dim ItmGrp_ID As Integer = 0
        Dim Unt_Id As Integer = 0
        Dim PSalNm_ID As Integer = 0
        Dim Cmp_ID As Integer = 0
        Dim SlNo As Integer = 0

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.FinishedProduct_Creation, New_Entry) = False Then Exit Sub

        If grp_Back.Enabled = False Then
            MessageBox.Show("Close other window", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Sur = Common_Procedures.Remove_NonCharacters(Trim(txt_Name.Text))
        If Trim(Sur) = "" Then
            MessageBox.Show("Invalid Item Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled Then txt_Name.Focus()
            Exit Sub
        End If

        ItmGrp_ID = Common_Procedures.ItemGroup_NameToIdNo(con, cbo_ItemGroup.Text)
        If Val(ItmGrp_ID) = 0 Then
            MessageBox.Show("Invalid ItemGroup", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Unit.Enabled Then cbo_Unit.Focus()
            Exit Sub
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1558" Then

            If Trim(txt_Code.Text) = "" Then
                MessageBox.Show("Invalid Item Code", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If txt_Code.Enabled Then txt_Code.Focus()
                Exit Sub
            End If

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1558" Then
            If cbo_Reconsilation_Meter_Weight.Text = "" Then cbo_Reconsilation_Meter_Weight.Text = "WEIGHT"
        Else
            If cbo_Reconsilation_Meter_Weight.Text = "" Then cbo_Reconsilation_Meter_Weight.Text = "METER"
        End If


        Unt_Id = Common_Procedures.Unit_NameToIdNo(con, cbo_Unit.Text)
        If Val(Unt_Id) = 0 Then
            MessageBox.Show("Invalid Unit", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Unit.Enabled Then cbo_Unit.Focus()
            Exit Sub
        End If

        Verified_STS = 0
        If chk_Verified_Status.Checked = True Then Verified_STS = 1

        If Trim(txt_Code.Text) <> "" Then
            ProdName = Trim(txt_Code.Text) & "-" & Trim(txt_Name.Text)
        Else
            ProdName = Trim(txt_Name.Text)
        End If

        tr = con.BeginTransaction

        Try
            cmd.Connection = con
            cmd.Transaction = tr

           
            If New_Entry = True Then

                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "Processed_Item_Head", "Processed_Item_IdNo", "", tr)

                lbl_DisplaySlNo.Text = Common_Procedures.get_MaxIdNo(con, "Processed_Item_Head", "Processed_Item_DisplaySlNo", "(Processed_Item_Type= 'FP')", tr)

                cmd.CommandText = "Insert into Processed_Item_Head ( Processed_Item_IdNo, Processed_Item_DisplaySlNo, Processed_Item_Type, Processed_Item_Name, Processed_Item_Nm, Sur_Name, Processed_Item_Code, Processed_ItemGroup_IdNo, Unit_IdNo, Tax_Percentage, Sale_TaxRate, Sales_Rate, Cost_Rate, Minimum_Stock, Meter_Qty, Weight_Piece, Width, Verified_Status, Reconsilation_Meter_Weight ) values (" & Str(Val(lbl_IdNo.Text)) & ", " & Val(lbl_DisplaySlNo.Text) & ", 'FP', '" & Trim(ProdName) & "', '" & Trim(txt_Name.Text) & "', '" & Trim(Sur) & "', '" & Trim(txt_Code.Text) & "', " & Str(Val(ItmGrp_ID)) & ", " & Str(Val(Unt_Id)) & ", " & Str(Val(txt_TaxPerc.Text)) & ", " & Str(Val(txt_TaxRate.Text)) & ", " & Str(Val(txt_Rate.Text)) & ", " & Str(Val(txt_CostRate.Text)) & ", " & Str(Val(txt_MinimumStock.Text)) & "," & Val(txt_Meter_Qty.Text) & "," & Val(txt_Weight_Piece.Text) & "," & Val(txt_Width.Text) & ", " & Val(Verified_STS) & " , '" & Trim(cbo_Reconsilation_Meter_Weight.Text) & "' )"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "update Processed_Item_Head set Processed_Item_Name = '" & Trim(ProdName) & "', Processed_Item_Nm = '" & Trim(txt_Name.Text) & "', Sur_Name = '" & Trim(Sur) & "', Processed_Item_Code = '" & Trim(txt_Code.Text) & "', Processed_ItemGroup_IdNo = " & Str(Val(ItmGrp_ID)) & ", Unit_IdNo = " & Str(Val(Unt_Id)) & ", Meter_Qty = " & Val(txt_Meter_Qty.Text) & ", Weight_Piece = " & Val(txt_Weight_Piece.Text) & ", Width = " & Val(txt_Width.Text) & ", Minimum_Stock = " & Str(Val(txt_MinimumStock.Text)) & ", Tax_Percentage = " & Str(Val(txt_TaxPerc.Text)) & ", Sale_TaxRate = " & Str(Val(txt_TaxRate.Text)) & ", Sales_Rate = " & Str(Val(txt_Rate.Text)) & ", Cost_Rate = " & Str(Val(txt_CostRate.Text)) & ", Verified_Status = " & Val(Verified_STS) & ", Reconsilation_Meter_Weight = '" & Trim(cbo_Reconsilation_Meter_Weight.Text) & "'   Where Processed_Item_IdNo = " & Str(Val(lbl_IdNo.Text))
                cmd.ExecuteNonQuery()

            End If

            tr.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(ProdName)       ' Trim(txt_Name.Text)
            Common_Procedures.Master_Return.Master_Type = "FINISHEDPRODUCT"

            MessageBox.Show("Saved Successfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

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
            If InStr(1, Trim(LCase(ex.Message)), "ix_processed_item_head") > 0 Then
                MessageBox.Show("Duplicate Finished Product", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Finally
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        End Try

    End Sub

    Private Sub FinishedProduct_Creation_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ItemGroup.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ITEMGROUP" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ItemGroup.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Unit.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "UNIT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Unit.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            

            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Item_Creation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        '  Dim CompCondt As String

        Me.Text = ""

        'If Val(Common_Procedures.User.IdNo) <> 1 And Common_Procedures.UR.Finished_Product_Verifition = "" Then chk_Verified_Status.Enabled = False

        con.Open()

        'da = New SqlClient.SqlDataAdapter("select itemgroup_name from itemgroup_head order by itemgroup_name", con)
        'da.Fill(dt1)
        'cbo_ItemGroup.DataSource = dt1
        'cbo_ItemGroup.DisplayMember = "itemgroup_name"

        'da = New SqlClient.SqlDataAdapter("select unit_name from unit_head order by unit_name", con)
        'da.Fill(dt2)
        'cbo_Unit.DataSource = dt2
        'cbo_Unit.DisplayMember = "unit_name"



        lbl_Reconsilation_Meter_Weight.Visible = False
        lbl_Reconsilation_Meter_Weight.Visible = False


        'grp_Open.Visible = False
        'grp_Open.Left = (Me.Width - grp_Open.Width) - 10
        'grp_Open.Top = (Me.Height - grp_Open.Height) - 35  ' 20
        grp_Open.Visible = False
        grp_Open.Left = (Me.Width - grp_Open.Width) \ 2
        grp_Open.Top = ((Me.Height - grp_Open.Height) \ 2) + 10

        grp_Filter.Visible = False
        grp_Filter.Left = (Me.Width - grp_Filter.Width) \ 2
        grp_Filter.Top = ((Me.Height - grp_Filter.Height) \ 2) + 10

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1558" Then

            lbl_Reconsilation_Meter_Weight.Visible = True
            cbo_Reconsilation_Meter_Weight.Visible = True

        End If


        cbo_Reconsilation_Meter_Weight.Items.Clear()
        cbo_Reconsilation_Meter_Weight.Items.Add("METER")
        cbo_Reconsilation_Meter_Weight.Items.Add("WEIGHT")

        AddHandler txt_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ItemGroup.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Code.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Unit.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Verified_Status.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Meter_Qty.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Weight_Piece.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Width.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_MinimumStock.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TaxPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CostRate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TaxRate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Open.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Reconsilation_Meter_Weight.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ItemGroup.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Code.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Unit.LostFocus, AddressOf ControlLostFocus
        AddHandler chk_Verified_Status.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Meter_Qty.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Weight_Piece.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Width.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_MinimumStock.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TaxPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CostRate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TaxRate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Open.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Reconsilation_Meter_Weight.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Weight_Piece.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Meter_Qty.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Width.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_MinimumStock.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TaxPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CostRate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Code.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Name.KeyDown, AddressOf TextBoxControlKeyDown



        AddHandler txt_Weight_Piece.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Meter_Qty.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Width.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_MinimumStock.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TaxPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CostRate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Rate.KeyPress, AddressOf TextBoxControlKeyPress

        Prec_ActCtrl = Nothing

        new_record()

    End Sub

    Private Sub Item_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Filter.Visible Then
                Call btn_CloseFilter_Click(sender, e)
                Exit Sub

            ElseIf grp_Open.Visible Then
                Call btnClose_Click(sender, e)
                Exit Sub

            
            Else
                If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub

                Else
                    Me.Close()
                End If

            End If

        End If
    End Sub

    

    Private Sub cbo_Open_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Open.GotFocus
        With cbo_Open
            '.BackColor = Color.lime
            '.ForeColor = Color.Blue
            .DroppedDown = True
        End With
    End Sub

    Private Sub cbo_Open_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Open.KeyPress
        Dim Indx As Integer
        Dim FindStr As String

        If Asc(e.KeyChar) = 13 Then
            btn_Find_Click(sender, e)
        End If

        If Asc(e.KeyChar) = 8 Then
            If cbo_Open.SelectionStart <= 1 Then
                cbo_Open.Text = ""
                Exit Sub
            End If

            If cbo_Open.SelectionLength = 0 Then
                FindStr = cbo_Open.Text.Substring(0, cbo_Open.Text.Length - 1)
            Else
                FindStr = cbo_Open.Text.Substring(0, cbo_Open.SelectionStart - 1)
            End If

        Else

            If cbo_Open.SelectionLength = 0 Then
                FindStr = cbo_Open.Text & e.KeyChar
            Else
                FindStr = cbo_Open.Text.Substring(0, cbo_Open.SelectionStart) & e.KeyChar
            End If

        End If

        Indx = cbo_Open.FindString(FindStr)

        If Indx <> -1 Then
            cbo_Open.SelectedText = ""
            cbo_Open.SelectedIndex = Indx
            cbo_Open.SelectionStart = FindStr.Length
            cbo_Open.SelectionLength = cbo_Open.Text.Length
        End If

        e.Handled = True

    End Sub

  
   

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Name.KeyPress
        If Asc(e.KeyChar) = 39 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub btn_Find_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Find.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select Processed_Item_IdNo from Processed_Item_Head where Processed_Item_Name = '" & Trim(cbo_Open.Text) & "'", con)
            da.Fill(dt)
            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If movid <> 0 Then
                move_record(movid)
                btnClose_Click(sender, e)
            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "FOR FINDING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        grp_Back.Enabled = True
        grp_Open.Visible = False
        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
    End Sub

    Private Sub cbo_ItemGroup_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemGroup.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "itemgroup_head", "itemgroup_name", "", "")

    End Sub

    Private Sub cbo_ItemGroup_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemGroup.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemGroup, txt_Name, txt_Code, "itemgroup_head", "itemgroup_name", "", "")

    End Sub

    Private Sub cbo_ItemGroup_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemGroup.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ItemGroup, txt_Code, "itemgroup_head", "itemgroup_name", "", "")


    End Sub

  

    Private Sub cbo_Unit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Unit.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Unit_Head", "Unit_Name", "", "")

    End Sub

    Private Sub cbo_Unit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Unit.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Unit, txt_Code, txt_Meter_Qty, "Unit_Head", "Unit_Name", "", "")

    End Sub
    Private Sub cbo_Unit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Unit.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Unit, txt_Meter_Qty, "Unit_Head", "Unit_Name", "", "")

    End Sub
    

    

    Private Sub txt_VatPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaxPerc.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub btn_OpenFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OpenFilter.Click
        Dim movid As Integer = 0

        Try
            movid = Val(dgv_Filter.CurrentRow.Cells(0).Value)

            If Val(movid) <> 0 Then
                move_record(movid)
                grp_Back.Enabled = True
                grp_Filter.Visible = False
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)

        End Try
    End Sub

    Private Sub btn_CloseFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseFilter.Click
        grp_Back.Enabled = True
        grp_Filter.Visible = False
    End Sub

    Private Sub dgv_Filter_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter.CellDoubleClick
        Call btn_OpenFilter_Click(sender, e)
    End Sub

    Private Sub dgv_Filter_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
        If e.KeyValue = 13 Then
            Call btn_OpenFilter_Click(sender, e)
        End If
    End Sub

    

   

    Private Sub txt_Rate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Rate.KeyUp
        txt_TaxRate.Text = Format(Val(txt_Rate.Text) * ((100 + Val(txt_TaxPerc.Text)) / 100), "##########0.00")
    End Sub

    Private Sub txt_TaxRate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxRate.KeyDown

        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then


            If cbo_Reconsilation_Meter_Weight.Enabled And cbo_Reconsilation_Meter_Weight.Visible = True Then
                cbo_Reconsilation_Meter_Weight.Focus()

            Else

                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                End If

            End If
        End If
    End Sub

   
    Private Sub txt_TaxRate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxRate.KeyUp
        txt_Rate.Text = Format(Val(txt_TaxRate.Text) * (100 / (100 + Val(txt_TaxPerc.Text))), "#########0.00")
    End Sub

    Private Sub txt_TaxPerc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxPerc.KeyUp
        txt_TaxRate.Text = Format(Val(txt_Rate.Text) * ((100 + Val(txt_TaxPerc.Text)) / 100), "########0.00")
    End Sub

    Private Sub txt_TaxRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaxRate.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then
            e.Handled = True
        End If

        If Asc(e.KeyChar) = 13 Then

            If cbo_Reconsilation_Meter_Weight.Enabled And cbo_Reconsilation_Meter_Weight.Visible = True Then
                cbo_Reconsilation_Meter_Weight.Focus()

            Else

                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                End If
            End If
        End If
    End Sub



    Private Sub txt_Code_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Code.KeyPress
        If Asc(e.KeyChar) = 34 Or Asc(e.KeyChar) = 39 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub



   

    Private Sub txt_CostRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CostRate.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then
            e.Handled = True
        End If

    End Sub



    

    Private Sub txt_Rate_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then
            e.Handled = True
        End If


    End Sub

    

    Private Sub cbo_ItemGroup_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemGroup.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New ItemGroup_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ItemGroup.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

   

    Private Sub cbo_Unit_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Unit.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Unit_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Unit.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

  

   
    Private Sub txt_MinimumStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_MinimumStock.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then
            e.Handled = True
        End If
    End Sub

   
    Private Sub txt_Width_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Width.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then
            e.Handled = True
        End If

    End Sub

  

    Private Sub txt_Weight_Piece_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Weight_Piece.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then
            e.Handled = True
        End If
    End Sub

    

    Private Sub txt_Meter_Qty_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Meter_Qty.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub cbo_Unit_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbo_Unit.SelectedIndexChanged

    End Sub

    Private Sub txt_TaxRate_TextChanged(sender As Object, e As EventArgs) Handles txt_TaxRate.TextChanged

    End Sub

    Private Sub cbo_Reconsilation_Meter_Weight_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Reconsilation_Meter_Weight.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub

    Private Sub cbo_Reconsilation_Meter_Weight_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Reconsilation_Meter_Weight.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If

        If e.KeyCode = 38 Then
            txt_TaxRate.Focus()
        End If

    End Sub
End Class