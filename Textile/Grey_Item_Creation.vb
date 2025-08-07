Public Class Grey_Item_Creation
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False
    Private Prec_ActCtrl As New Control
    Private Verified_STS As Integer = 0


    Private Sub clear()

        ' Insert_Entry = False
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
        cbo_Grid_FinishedProduct.Text = ""
        txt_MinimumStock.Text = ""
        cbo_ItemGroup.Text = ""
        cbo_Unit.Text = ""
        txt_Name.Text = ""

        cbo_LotNo.Text = ""
        dgv_Details.Rows.Clear()
        dgv_Details.Rows.Add()

        cbo_Grid_FinishedProduct.Visible = False
        chk_Verified_Status.Checked = False
        New_Entry = False

        grp_Open.Visible = False

        New_Entry = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        Me.ActiveControl.BackColor = Color.lime
        Me.ActiveControl.ForeColor = Color.Blue

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

        If Me.ActiveControl.Name <> cbo_Grid_FinishedProduct.Name Then
            cbo_Grid_FinishedProduct.Visible = False
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

    Private Sub Grid_DeSelect()
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim slno As Integer, n As Integer

        If Val(idno) = 0 Then Exit Sub

        clear()

        Try

            da = New SqlClient.SqlDataAdapter("select a.*, b.ItemGroup_Name, c.Unit_Name, d.Lot_No from Processed_Item_Head a LEFT OUTER JOIN ItemGroup_Head b ON a.Processed_ItemGroup_IdNo = b.ItemGroup_IdNo LEFT OUTER JOIN Unit_Head c ON a.Unit_IdNo = c.Unit_IdNo LEFT OUTER JOIN Lot_Head d ON a.Lot_IdNo = d.Lot_IdNo where a.Processed_Item_IdNo = " & Str(Val(idno)) & " and Processed_Item_Type = 'GREY'", con)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then

                lbl_IdNo.Text = dt.Rows(0).Item("Processed_Item_IdNo").ToString
                lbl_DisplaySlNo.Text = dt.Rows(0).Item("Processed_Item_DisplaySlNo").ToString
                txt_Name.Text = dt.Rows(0).Item("Processed_Item_Nm").ToString
                txt_Code.Text = dt.Rows(0).Item("Processed_Item_Code").ToString
                cbo_ItemGroup.Text = dt.Rows(0).Item("ItemGroup_Name").ToString
                cbo_LotNo.Text = dt.Rows(0).Item("Lot_No").ToString
                cbo_Unit.Text = dt.Rows(0).Item("Unit_Name").ToString
                txt_Meter_Qty.Text = dt.Rows(0).Item("Meter_Qty").ToString
                txt_Weight_Piece.Text = dt.Rows(0).Item("Weight_Piece").ToString
                txt_Width.Text = dt.Rows(0).Item("Width").ToString
                txt_MinimumStock.Text = dt.Rows(0).Item("Minimum_Stock").ToString
                txt_TaxPerc.Text = dt.Rows(0).Item("Tax_Percentage").ToString
                txt_TaxRate.Text = dt.Rows(0).Item("Sale_TaxRate").ToString
                txt_Rate.Text = dt.Rows(0).Item("Sales_Rate").ToString
                txt_CostRate.Text = dt.Rows(0).Item("Cost_Rate").ToString
                If Val(dt.Rows(0).Item("Verified_Status").ToString) = 1 Then chk_Verified_Status.Checked = True

                da = New SqlClient.SqlDataAdapter("select b.processed_Item_Name, c.ItemGroup_Name from Processed_Item_Details a INNER JOIN Processed_Item_Head b on a.Finished_Product_IdNo = b.Processed_Item_IdNo LEFT OUTER JOIN ItemGroup_Head c ON b.Processed_ItemGroup_IdNo = c.ItemGroup_IdNo Where a.Processed_Item_IdNo = " & Str(Val(idno)), con)
                da.Fill(dt2)

                dgv_Details.Rows.Clear()
                slno = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        slno = slno + 1

                        dgv_Details.Rows(n).Cells(0).Value = Val(slno)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("processed_Item_Name").ToString
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("ItemGroup_Name").ToString

                    Next i

                    For i = 0 To dgv_Details.RowCount - 1
                        dgv_Details.Rows(i).Cells(0).Value = Val(i) + 1
                    Next
                End If

                dgv_Details.Rows.Add()

            End If
            dt.Clear()
            dt.Dispose()

            Grid_DeSelect()


        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        cbo_Grid_FinishedProduct.Text = ""
        cbo_Grid_FinishedProduct.Visible = False

        If txt_Name.Visible And txt_Name.Enabled Then txt_Name.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.FP_Grey_Item_Creation, New_Entry, Me) = False Then Exit Sub

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.GreyItem_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.GreyItem_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If grp_Back.Enabled = False Then
            MessageBox.Show("Close all other windows", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

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
                        MessageBox.Show("Already used this Grey Fabric", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
                        MessageBox.Show("Already used this Grey Fabric", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
                        MessageBox.Show("Already used this Grey Fabric", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
                        MessageBox.Show("Already used this Grey Fabric", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If
            dt.Clear()

            cmd.Connection = con

            cmd.CommandText = "delete from Processed_Item_Details where Processed_Item_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Processed_Item_Head where Processed_Item_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If txt_Name.Enabled = True And txt_Name.Visible = True Then txt_Name.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("select a.Processed_Item_IdNo, a.Processed_Item_Name, b.unit_name, a.Sale_TaxRate from Processed_Item_Head a, unit_head b where a.unit_idno = b.unit_idno and a.Processed_Item_Type = 'GREY' Order by a.Processed_Item_IdNo", con)
        da.Fill(dt)

        dgv_Filter.Columns.Clear()
        dgv_Filter.DataSource = dt

        dgv_Filter.RowHeadersVisible = False

        dgv_Filter.Columns(0).HeaderText = "IDNO"
        dgv_Filter.Columns(1).HeaderText = "ITEM NAME"
        dgv_Filter.Columns(2).HeaderText = "UNIT"
        dgv_Filter.Columns(3).HeaderText = "SALES RATE "

        dgv_Filter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        dgv_Filter.Columns(0).FillWeight = 30
        dgv_Filter.Columns(1).FillWeight = 130
        dgv_Filter.Columns(2).FillWeight = 50
        dgv_Filter.Columns(3).FillWeight = 50

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
            cmd.CommandText = "select TOP 1 Processed_Item_IdNo from Processed_Item_Head WHERE Processed_Item_Type = 'GREY' ORDER BY Processed_Item_DisplaySlNo"
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
            cmd.CommandText = "select TOP 1 Processed_Item_IdNo from Processed_Item_Head WHERE Processed_Item_Type = 'GREY' ORDER BY Processed_Item_DisplaySlNo desc"
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
            cmd.CommandText = "select TOP 1 Processed_Item_IdNo from Processed_Item_Head WHERE Processed_Item_DisplaySlNo > " & Val(lbl_DisplaySlNo.Text) & " and  Processed_Item_Type = 'GREY' ORDER BY Processed_Item_DisplaySlNo"
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
            cmd.CommandText = "select TOP 1 Processed_Item_IdNo from Processed_Item_Head WHERE Processed_Item_DisplaySlNo < " & Val(lbl_DisplaySlNo.Text) & " and  Processed_Item_Type = 'GREY' ORDER BY Processed_Item_DisplaySlNo desc"
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
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim newid As Integer = 0

        clear()

        Try

            New_Entry = True

            lbl_IdNo.Text = Val(Common_Procedures.get_MaxIdNo(con, "Processed_Item_Head", "Processed_Item_IdNo", ""))
            lbl_IdNo.ForeColor = Color.Red

            lbl_DisplaySlNo.Text = Val(Common_Procedures.get_MaxIdNo(con, "Processed_Item_Head", "Processed_Item_DisplaySlNo", "(Processed_Item_Type= 'GREY')"))
            lbl_DisplaySlNo.ForeColor = Color.Red

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head Where Processed_Item_IdNo = 0 or Processed_Item_Type = 'GREY' order by Processed_Item_Name", con)
        da.Fill(dt)
        cbo_Open.DataSource = dt
        cbo_Open.DisplayMember = "Processed_Item_Name"

        grp_Open.Visible = True
        grp_Back.Enabled = False
        If cbo_Open.Enabled And cbo_Open.Visible Then cbo_Open.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '---MessageBox.Show("insert record")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '---MessageBox.Show("print record")
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim Sur As String = ""
        Dim nr As Long = 0
        Dim itmgrp_id As Integer = 0
        Dim unt_id As Integer = 0
        Dim fp_id As Integer = 0
        Dim Lt_id As Integer = 0
        Dim cmp_id As Integer = 0
        Dim Slno As Integer = 0
        Dim ProdName As String = ""

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.GreyItem_Creation, New_Entry) = False Then Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.FP_Grey_Item_Creation, New_Entry, Me) = False Then Exit Sub

      

        If grp_Back.Enabled = False Then
            MessageBox.Show("Close all other windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Sur = Common_Procedures.Remove_NonCharacters(Trim(txt_Name.Text))
        If Trim(Sur) = "" Then
            MessageBox.Show("Invalid Item Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled Then txt_Name.Focus()
            Exit Sub
        End If

        itmgrp_id = Val(Common_Procedures.ItemGroup_NameToIdNo(con, cbo_ItemGroup.Text))
        If Val(itmgrp_id) = 0 Then
            MessageBox.Show("Invalid ItemGroup", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_ItemGroup.Enabled Then cbo_ItemGroup.Focus()
            Exit Sub
        End If

        Lt_id = Val(Common_Procedures.Lot_NoToIdNo(con, cbo_LotNo.Text))
        'If Val(Lt_id) = 0 Then
        '    MessageBox.Show("Invalid Lot No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_LotNo.Enabled Then cbo_LotNo.Focus()
        '    Exit Sub
        'End If

        unt_id = Val(Common_Procedures.Unit_NameToIdNo(con, cbo_Unit.Text))
        If Val(unt_id) = 0 Then
            MessageBox.Show("Invalid Unit", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Unit.Enabled Then cbo_Unit.Focus()
            Exit Sub
        End If

        ProdName = Trim(txt_Name.Text)
        If Trim(txt_Code.Text) <> "" Then
            ProdName = Trim(txt_Code.Text) & " - " & Trim(txt_Name.Text)
        End If

        Verified_STS = 0
        If chk_Verified_Status.Checked = True Then Verified_STS = 1


        tr = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = tr

            If New_Entry = True Then

                lbl_IdNo.Text = Val(Common_Procedures.get_MaxIdNo(con, "Processed_Item_Head", "Processed_Item_IdNo", "", tr))

                lbl_DisplaySlNo.Text = Val(Common_Procedures.get_MaxIdNo(con, "Processed_Item_Head", "Processed_Item_DisplaySlNo", "(Processed_Item_Type= 'GREY')", tr))

                cmd.CommandText = "Insert into Processed_Item_Head ( Processed_Item_IdNo, Processed_Item_DisplaySlNo, Processed_Item_Type, Processed_Item_Name, Processed_Item_Nm, Sur_Name, Processed_Item_Code, Processed_ItemGroup_IdNo, Lot_IdNo, Unit_IdNo, Tax_Percentage, Sale_TaxRate, Sales_Rate, Cost_Rate, Minimum_Stock, Meter_Qty, Weight_Piece, Width,Verified_Status) values (" & Str(Val(lbl_IdNo.Text)) & ", " & Val(lbl_DisplaySlNo.Text) & ", 'GREY', '" & Trim(ProdName) & "', '" & Trim(txt_Name.Text) & "', '" & Trim(Sur) & "', '" & Trim(txt_Code.Text) & "', " & Str(Val(itmgrp_id)) & ", " & Str(Val(Lt_id)) & ", " & Str(Val(unt_id)) & ", " & Str(Val(txt_TaxPerc.Text)) & ", " & Str(Val(txt_TaxRate.Text)) & ", " & Str(Val(txt_Rate.Text)) & ", " & Str(Val(txt_CostRate.Text)) & ", " & Str(Val(txt_MinimumStock.Text)) & ", " & Val(txt_Meter_Qty.Text) & ", " & Val(txt_Weight_Piece.Text) & ", " & Val(txt_Width.Text) & "," & Val(Verified_STS) & ")"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "update Processed_Item_Head set Processed_Item_Name = '" & Trim(ProdName) & "', Processed_Item_Nm = '" & Trim(txt_Name.Text) & "', Sur_Name = '" & Trim(Sur) & "', Processed_Item_Code = '" & Trim(txt_Code.Text) & "', Processed_ItemGroup_IdNo = " & Str(Val(itmgrp_id)) & ",lot_IdNo = " & Str(Val(Lt_id)) & ", Unit_IdNo = " & Str(Val(unt_id)) & ", Meter_Qty = " & Val(txt_Meter_Qty.Text) & ", Weight_Piece = " & Val(txt_Weight_Piece.Text) & ", Width = " & Val(txt_Width.Text) & ", Minimum_Stock = " & Str(Val(txt_MinimumStock.Text)) & ", Tax_Percentage = " & Str(Val(txt_TaxPerc.Text)) & ", Sale_TaxRate = " & Str(Val(txt_TaxRate.Text)) & ", Sales_Rate = " & Str(Val(txt_Rate.Text)) & ", Cost_Rate = " & Str(Val(txt_CostRate.Text)) & ",Verified_Status = " & Val(Verified_STS) & " Where Processed_Item_IdNo = " & Str(Val(lbl_IdNo.Text))
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "delete from Processed_Item_details where Processed_Item_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            With dgv_Details
                Slno = 0
                For i = 0 To .RowCount - 1

                    fp_id = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                    If Val(fp_id) <> 0 Then

                        Slno = Slno + 1

                        cmd.CommandText = "Insert into Processed_Item_Details( Processed_Item_IdNo, sl_No, Finished_Product_IdNo) values (" & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(Slno)) & ", " & Str(Val(fp_id)) & ")"
                        cmd.ExecuteNonQuery()
                    End If
                Next

            End With

            tr.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(txt_Name.Text)
            Common_Procedures.Master_Return.Master_Type = "GREYITEM"



            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

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
                MessageBox.Show("Duplicate Item Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Private Sub Grey_Item_Creation_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ItemGroup.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ITEMGROUP" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ItemGroup.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Unit.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "UNIT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Unit.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_LotNo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LOT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_LotNo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_FinishedProduct.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "FINISHEDPRODUCT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_FinishedProduct.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Grey_Item_Creation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable

        Me.Text = ""

        ' If Val(Common_Procedures.User.IdNo) <> 1 And Common_Procedures.UR.Grey_Fabric_Verifition = "" Then chk_Verified_Status.Enabled = False

        con.Open()

        'da = New SqlClient.SqlDataAdapter("select itemgroup_name from itemgroup_head order by itemgroup_name", con)
        'da.Fill(dt1)
        'cbo_ItemGroup.Items.Clear()
        'cbo_ItemGroup.DataSource = dt1
        'cbo_ItemGroup.DisplayMember = "itemgroup_name"

        'da = New SqlClient.SqlDataAdapter("select Lot_no from Lot_head order by Lot_no", con)
        'da.Fill(dt)
        'cbo_LotNo.DataSource = dt
        'cbo_LotNo.DisplayMember = "Lot_no"

        'da = New SqlClient.SqlDataAdapter("select unit_name from unit_head order by unit_name", con)
        'da.Fill(dt2)
        'cbo_Unit.DataSource = dt2
        'cbo_Unit.DisplayMember = "unit_name"

        'da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head where Processed_Item_Idno = 0 or Processed_Item_Type = 'GREY' order by Processed_Item_Name", con)
        'da.Fill(dt3)
        'cbo_Grid_FinishedProduct.DataSource = dt3
        'cbo_Grid_FinishedProduct.DisplayMember = "Processed_Item_Name"
        'cbo_Grid_FinishedProduct.Visible = False

        grp_Open.Visible = False
        grp_Open.Left = (Me.Width - grp_Open.Width) \ 2
        grp_Open.Top = ((Me.Height - grp_Open.Height) \ 2) + 100

        grp_Filter.Visible = False
        grp_Filter.Left = (Me.Width - grp_Filter.Width)
        grp_Filter.Top = (Me.Height - grp_Filter.Height) - 20

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
        AddHandler cbo_LotNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_FinishedProduct.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Open.GotFocus, AddressOf ControlGotFocus

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
        AddHandler cbo_LotNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_FinishedProduct.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Open.LostFocus, AddressOf ControlLostFocus

        new_record()

    End Sub

    Private Sub Grey_Item_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Grey_Item_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
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

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        If keyData = Keys.Enter Then

            On Error Resume Next

            If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

                dgv1 = dgv_Details

                With dgv1

                    If .CurrentCell.ColumnIndex >= 1 Then

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

                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

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

    Private Sub cbo_Open_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Open.GotFocus
        cbo_Open.DroppedDown = True
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

    Private Sub txt_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Name.KeyDown

        If e.KeyValue = 40 Then
            SendKeys.Send("{TAB}")
        End If
        If e.KeyValue = 38 Then
            If dgv_Details.Rows.Count = 0 Then dgv_Details.Rows.Add()
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        End If
    End Sub

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Name.KeyPress
        If Asc(e.KeyChar) = 39 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btn_Find_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Find.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select Processed_Item_IdNo from Processed_Item_Head where Processed_Item_Name = '" & Trim(cbo_Open.Text) & "'", con)
            da.Fill(dt)

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

        'Me.Height = 400

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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemGroup, txt_Name, txt_Code, "itemgroup_head", "itemgroup_name", "", "")

    End Sub

    Private Sub cbo_ItemGroup_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemGroup.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ItemGroup, txt_Code, "itemgroup_head", "itemgroup_name", "", "")

    End Sub

    Private Sub cbo_Unit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Unit.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Unit_Head", "Unit_Name", "", "")

    End Sub

    Private Sub cbo_Unit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Unit.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Unit, cbo_LotNo, txt_Meter_Qty, "Unit_Head", "Unit_Name", "", "")

    End Sub
    Private Sub cbo_Unit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Unit.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Unit, txt_Meter_Qty, "Unit_Head", "Unit_Name", "", "")

    End Sub
    Private Sub txt_VatPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxPerc.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_VatPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaxPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
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

    Private Sub dgv_Filter_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
        If e.KeyValue = 13 Then
            Call btn_OpenFilter_Click(sender, e)
        End If
    End Sub

    Private Sub txt_Rate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Rate.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Rate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Rate.KeyUp
        txt_TaxRate.Text = Format(Val(txt_Rate.Text) * ((100 + Val(txt_TaxPerc.Text)) / 100), "##########0.00")
    End Sub

    Private Sub txt_TaxRate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxRate.KeyUp
        txt_Rate.Text = Format(Val(txt_TaxRate.Text) * (100 / (100 + Val(txt_TaxPerc.Text))), "#########0.00")
    End Sub

    Private Sub txt_TaxPerc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxPerc.KeyUp
        txt_TaxRate.Text = Format(Val(txt_Rate.Text) * ((100 + Val(txt_TaxPerc.Text)) / 100), "########0.00")
    End Sub

    Private Sub txt_TaxRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaxRate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then

            If dgv_Details.Rows.Count <= 0 Then dgv_Details.Rows.Add()
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            'SendKeys.Send("{TAB}")
            'If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) Then
            '    save_record()
            'End If
        End If
    End Sub

    Private Sub txt_Code_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Code.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Code_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Code.KeyPress
        If Asc(e.KeyChar) = 34 Or Asc(e.KeyChar) = 39 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_CostRate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CostRate.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_CostRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CostRate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_TaxRate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TaxRate.KeyDown
        If e.KeyValue = 40 Then
            If dgv_Details.Rows.Count = 0 Then dgv_Details.Rows.Add()
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        End If
        If e.KeyValue = 38 Then
            SendKeys.Send("+{TAB}")
        End If
    End Sub

    Private Sub txt_Rate_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count = 0 Then dgv_Details.Rows.Add()
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        End If

    End Sub

    Private Sub txt_MinimumStock_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_MinimumStock.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_MinimumStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_MinimumStock.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub cbo_Grid_FinishedProduct_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_FinishedProduct.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP')", "(Processed_Item_idno = 0)")

    End Sub

    Private Sub cbo_Grid_FinishedProduct_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_FinishedProduct.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_FinishedProduct, Nothing, Nothing, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP')", "(Processed_Item_idno = 0)")

        Try
            With cbo_Grid_FinishedProduct
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True

                    If Val(dgv_Details.CurrentCell.RowIndex) <= 0 Then
                        txt_TaxRate.Focus()

                    Else
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex - 1).Cells(1)

                    End If

                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True

                    If dgv_Details.CurrentCell.ColumnIndex >= 1 Then

                        If dgv_Details.CurrentCell.RowIndex >= dgv_Details.Rows.Count - 1 Then
                            btn_save.Focus()

                        Else
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex + 1).Cells(1)

                        End If

                    Else
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(dgv_Details.CurrentCell.ColumnIndex + 1)

                    End If

                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Grid_FinishedProduct_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_FinishedProduct.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_FinishedProduct, Nothing, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP')", "(Processed_Item_idno = 0)")

        With cbo_Grid_FinishedProduct

            If Asc(e.KeyChar) = 13 Then

                If dgv_Details.CurrentRow.Index = dgv_Details.RowCount - 1 And dgv_Details.CurrentCell.ColumnIndex >= 1 And Trim(dgv_Details.CurrentRow.Cells(1).Value) = "" Then
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        txt_Name.Focus()
                    End If

                Else
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex + 1).Cells(dgv_Details.CurrentCell.ColumnIndex)

                End If
            End If
        End With

    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        With dgv_Details
            If .CurrentCell.ColumnIndex = 1 Then

                If cbo_Grid_FinishedProduct.Visible = False Or Val(cbo_Grid_FinishedProduct.Tag) <> e.RowIndex Then

                    cbo_Grid_FinishedProduct.Tag = -100

                    Da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head where Processed_Item_Idno = 0 or Processed_Item_Type = 'GREY' order by Processed_Item_Name", con)
                    Da.Fill(Dt1)
                    cbo_Grid_FinishedProduct.DataSource = Dt1
                    cbo_Grid_FinishedProduct.DisplayMember = "Processed_Item_Name"

                    cbo_Grid_FinishedProduct.Left = .Left + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_FinishedProduct.Top = .Top + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_FinishedProduct.Width = .CurrentCell.Size.Width
                    cbo_Grid_FinishedProduct.Text = Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_FinishedProduct.Tag = Val(.CurrentCell.RowIndex)
                    cbo_Grid_FinishedProduct.Visible = True

                    cbo_Grid_FinishedProduct.Focus()
                    cbo_Grid_FinishedProduct.BringToFront()


                End If

            Else
                cbo_Grid_FinishedProduct.Visible = False


            End If

        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged

        Try
            With dgv_Details
                If (.CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2) And Trim(.CurrentCell.Value.ToString) <> "" Then
                    If .CurrentRow.Index = .Rows.Count - 1 Then
                        .Rows.Add()
                    End If
                End If
            End With

        Catch ex As Exception
            '-----

        End Try

    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        On Error Resume Next

        With dgv_Details
            If e.KeyCode = Keys.Up Then
                If .CurrentRow.Index = 0 Then
                    txt_TaxRate.Focus()
                End If
            End If

            If e.KeyCode = Keys.Left Then
                If .CurrentCell.RowIndex = 0 And .CurrentCell.ColumnIndex <= 1 Then
                    txt_TaxRate.Focus()
                End If
            End If

        End With
    End Sub

    Private Sub cbo_Grid_FinishedProduct_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_FinishedProduct.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New FinishedProduct_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_FinishedProduct.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Item_Code_textChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_FinishedProduct.TextChanged
        Try
            If cbo_Grid_FinishedProduct.Visible = True Then
                With dgv_Details
                    If Val(cbo_Grid_FinishedProduct.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_FinishedProduct.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
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

    Private Sub txt_Width_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Width.KeyDown
        If e.KeyValue = 40 Then
            SendKeys.Send("{TAB}")
        End If
        If e.KeyValue = 38 Then
            SendKeys.Send("+{TAB}")
        End If
    End Sub

    Private Sub txt_Width_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Width.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")

        End If

    End Sub

    Private Sub txt_Weight_Piece_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Weight_Piece.KeyDown
        If e.KeyValue = 40 Then
            SendKeys.Send("{TAB}")
        End If
        If e.KeyValue = 38 Then
            SendKeys.Send("+{TAB}")
        End If
    End Sub

    Private Sub txt_Weight_Piece_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Weight_Piece.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_Meter_Qty_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Meter_Qty.KeyDown
        If e.KeyValue = 40 Then
            SendKeys.Send("{TAB}")
        End If
        If e.KeyValue = 38 Then
            SendKeys.Send("+{TAB}")
        End If
    End Sub

    Private Sub txt_Meter_Qty_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Meter_Qty.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub



    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub cbo_LotNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LotNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Lot_Head", "Lot_No", "", "(Lot_Idno=0)")

    End Sub

    Private Sub cbo_LotNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LotNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LotNo, txt_Code, cbo_Unit, "Lot_Head", "Lot_No", "", "(Lot_Idno=0)")

    End Sub

    Private Sub cbo_LotNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_LotNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LotNo, cbo_Unit, "Lot_Head", "Lot_No", "", "(Lot_Idno=0)")

    End Sub

    Private Sub cbo_LotNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LotNo.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New LotNo_creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_LotNo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_ItemGroup_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemGroup.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
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
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Unit_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Unit.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

End Class