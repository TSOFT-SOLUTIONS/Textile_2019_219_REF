Imports Excel = Microsoft.Office.Interop.Excel
Public Class Item_Creation
    Implements Interface_MDIActions
    Private new_entry As Boolean = False

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private Close_STS As Integer = 0
    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""
    Private Prec_ActCtrl As New Control


    Private Sub clear()
        'Dim obj As Object
        'Dim ctrl As Object
        'Dim gbox As GroupBox

        'For Each obj In Me.Controls
        '    If TypeOf obj Is TextBox Then
        '        obj.text = ""
        '    ElseIf TypeOf obj Is ComboBox Then
        '        obj.text = ""
        '    ElseIf TypeOf obj Is GroupBox Then
        '        gbox = obj
        '        For Each ctrl In gbox.Controls
        '            If TypeOf ctrl Is TextBox Then
        '                ctrl.text = ""
        '            ElseIf TypeOf ctrl Is ComboBox Then
        '                ctrl.text = ""
        '            End If
        '        Next
        '    End If
        'Next

        new_entry = False

        pnl_Back.Enabled = True
        grp_Filter.Visible = False
        grp_Open.Visible = False
        chk_JobWorkStatus.Checked = False


        txt_Code.Text = ""
        txt_CostRate_Excl_Tax.Text = ""
        txt_CostRate_Incl_Tax.Text = ""
        txt_description.Text = ""
        txt_DiscountPercentage.Text = ""
        txt_Sales_GSTRate.Text = ""
        'txt_GSTTaxPerc.Text = ""
        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black
        txt_MinimumStock.Text = ""
        txt_Mrp.Text = ""
        txt_Name.Text = ""
        txt_Rack_No.Text = ""
        txt_SalesRate_Excl_Tax.Text = ""
        txt_SalesProfit_Retail.Text = ""
        txt_SalesProfit_Wholesale.Text = ""
        txt_SalesRate_Retail.Text = ""
        txt_SalesRate_Wholesale.Text = ""
        txt_TamilName.Text = ""
        txt_VatTaxPerc.Text = ""
        txt_VatTaxRate.Text = ""
        'txt_HSNCode.Text = ""
        cbo_DealerName.Text = ""
        'cbo_ItemGroup.Text = ""
        cbo_Open.Text = ""
        cbo_Size.Text = ""
        cbo_Style.Text = ""
        cbo_Unit.Text = Common_Procedures.Unit_IdNoToName(con, 1)

        chk_Close_Status.Checked = False
    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
            Me.ActiveControl.BackColor = Color.Lime  'PaleGreen
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If


        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If IsNothing(Prec_ActCtrl) = False Then
            Prec_ActCtrl.BackColor = Color.White
            Prec_ActCtrl.ForeColor = Color.Black
        End If
    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Filter.CurrentCell) Then dgv_Filter.CurrentCell.Selected = False
    End Sub

    Private Sub TextBoxControl_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyCode = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControl_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub


    Private Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        If Val(idno) = 0 Then Exit Sub

        clear()

        new_entry = False

        Try

            da = New SqlClient.SqlDataAdapter("select a.*, b.ItemGroup_Name, c.Unit_Name, sh.Style_Name, sih.Size_Name , Lh.Ledger_Name as Dealer_Name from Item_Head a LEFT OUTER JOIN ItemGroup_Head b ON a.ItemGroup_IdNo = b.ItemGroup_IdNo LEFT OUTER JOIN Unit_Head c ON a.Unit_IdNo = c.Unit_IdNo LEFT OUTER JOIN Style_Head sh ON a.Item_Style_IdNo = sh.Style_IdNo LEFT OUTER JOIN Size_Head sih ON a.Item_Size_IdNo = sih.size_IdNo LEFT JOIN Ledger_Head Lh ON Lh.Ledger_IdNo = a.Dealer_IdNo  where a.Item_IdNo = " & Str(Val(idno)) & " ", con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0).Item("Item_IdNo").ToString) = False Then
                    lbl_IdNo.Text = dt.Rows(0).Item("Item_IdNo").ToString
                    txt_Name.Text = dt.Rows(0).Item("Item_DisplayName").ToString
                    txt_description.Text = dt.Rows(0).Item("Item_Description").ToString
                    txt_TamilName.Text = dt.Rows(0).Item("Item_Name_Tamil").ToString
                    If Val(dt.Rows(0).Item("Close_Status").ToString) = 1 Then chk_Close_Status.Checked = True
                    txt_Code.Text = dt.Rows(0).Item("Item_Code").ToString
                    cbo_ItemGroup.Text = dt.Rows(0).Item("ItemGroup_Name").ToString
                    cbo_Unit.Text = dt.Rows(0).Item("Unit_Name").ToString
                    txt_MinimumStock.Text = dt.Rows(0).Item("Minimum_Stock").ToString
                    txt_VatTaxPerc.Text = dt.Rows(0).Item("Tax_Percentage").ToString
                    txt_CostRate_Excl_Tax.Text = dt.Rows(0).Item("CostRate_Excl_Tax").ToString
                    txt_CostRate_Incl_Tax.Text = dt.Rows(0).Item("Cost_Rate").ToString
                    txt_SalesRate_Excl_Tax.Text = dt.Rows(0).Item("Sales_Rate").ToString
                    txt_Mrp.Text = dt.Rows(0).Item("MRP_Rate").ToString
                    txt_VatTaxRate.Text = dt.Rows(0).Item("Sale_TaxRate").ToString
                    txt_GSTTaxPerc.Text = dt.Rows(0).Item("Gst_Percentage").ToString
                    txt_Sales_GSTRate.Text = dt.Rows(0).Item("Gst_Rate").ToString
                    txt_DiscountPercentage.Text = dt.Rows(0).Item("Discount_Percentage").ToString
                    txt_Rack_No.Text = dt.Rows(0).Item("rACK_nO").ToString
                    cbo_Style.Text = dt.Rows(0).Item("Style_Name").ToString
                    cbo_Size.Text = dt.Rows(0).Item("Size_Name").ToString
                    cbo_DealerName.Text = dt.Rows(0).Item("Dealer_Name").ToString

                    txt_HSNCode.Text = dt.Rows(0).Item("HSN_Code").ToString

                    txt_SalesProfit_Retail.Text = dt.Rows(0).Item("Sales_Profit_Retail").ToString
                    txt_SalesRate_Retail.Text = dt.Rows(0).Item("Sales_Rate_Retail").ToString
                    txt_SalesProfit_Wholesale.Text = dt.Rows(0).Item("Sales_Profit_Wholesale").ToString
                    txt_SalesRate_Wholesale.Text = dt.Rows(0).Item("Sales_Rate_Wholesale").ToString

                    If Val(dt.Rows(0).Item("Job_Work_Status").ToString) = 1 Then
                        chk_JobWorkStatus.Checked = True
                    Else
                        chk_JobWorkStatus.Checked = False
                    End If

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If txt_Name.Visible And txt_Name.Enabled Then txt_Name.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Item_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Item_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        da = New SqlClient.SqlDataAdapter("select count(*) from Item_Processing_Details where  Item_IdNo = " & Str(Val(lbl_IdNo.Text)) & " and Quantity <> 0 ", con)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count <> 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                If Val(dt.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already used this Item", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If

        Try
            cmd.Connection = con
            cmd.CommandText = "delete from Item_Head where Item_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            new_record()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If txt_Name.Enabled = True And txt_Name.Visible = True Then txt_Name.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("select a.item_idno, a.item_name, b.unit_name, a.Sale_TaxRate from item_head a, unit_head b where a.unit_idno = b.unit_idno Order by a.item_idno", con)
        da.Fill(dt)

        dgv_Filter.Columns.Clear()
        dgv_Filter.DataSource = dt

        dgv_Filter.RowHeadersVisible = False

        dgv_Filter.Columns(0).HeaderText = "IDNO"
        dgv_Filter.Columns(1).HeaderText = "ITEM NAME"
        dgv_Filter.Columns(2).HeaderText = "UNIT"
        dgv_Filter.Columns(2).HeaderText = "Sales_Rate"

        dgv_Filter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        dgv_Filter.Columns(0).FillWeight = 40
        dgv_Filter.Columns(1).FillWeight = 240
        dgv_Filter.Columns(2).FillWeight = 60
        dgv_Filter.Columns(3).FillWeight = 60

        pnl_Back.Enabled = False
        grp_Filter.Visible = True

        dgv_Filter.BringToFront()
        dgv_Filter.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movid As Integer = 0

        Try

            Da = New SqlClient.SqlDataAdapter("select TOP 1 item_idno from item_head Where Item_IdNo <> 0  ORDER BY item_idno", con)
            Dt = New DataTable
            Da.Fill(Dt)

            movid = 0
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movid = Val(Dt.Rows(0)(0).ToString)
                End If
            End If

            If movid <> 0 Then move_record(movid)

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movid As Integer = 0

        Try

            Da = New SqlClient.SqlDataAdapter("select TOP 1 item_idno from item_head Where Item_IdNo <> 0  ORDER BY item_idno DESC", con)
            Dt = New DataTable
            Da.Fill(Dt)

            movid = 0
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movid = Val(Dt.Rows(0)(0).ToString)
                End If
            End If

            If movid <> 0 Then move_record(movid)

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movid As Integer = 0

        Try

            Da = New SqlClient.SqlDataAdapter("select TOP 1 item_idno from item_head Where Item_IdNo > " & Str(Val(lbl_IdNo.Text)) & "   ORDER BY item_idno", con)
            Dt = New DataTable
            Da.Fill(Dt)

            movid = 0
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movid = Val(Dt.Rows(0)(0).ToString)
                End If
            End If

            If movid <> 0 Then move_record(movid)

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movid As Integer = 0

        Try

            Da = New SqlClient.SqlDataAdapter("select TOP 1 item_idno from item_head Where Item_IdNo < " & Str(Val(lbl_IdNo.Text)) & "   ORDER BY item_idno DESC", con)
            Dt = New DataTable
            Da.Fill(Dt)

            movid = 0
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movid = Val(Dt.Rows(0)(0).ToString)
                End If
            End If

            If movid <> 0 Then move_record(movid)

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim newid As Integer = 0

        clear()

        new_entry = True

        da = New SqlClient.SqlDataAdapter("select max(item_idno) from item_head", con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                newid = Val(dt.Rows(0)(0).ToString)
            End If
        End If

        newid = newid + 1

        lbl_IdNo.Text = newid
        lbl_IdNo.ForeColor = Color.Red

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        da = New SqlClient.SqlDataAdapter("select item_name from item_head  order by item_name", con)
        da.Fill(dt)

        'cbo_Open.Items.Clear()

        cbo_Open.DataSource = dt
        cbo_Open.DisplayMember = "item_name"

        grp_Open.Visible = True
        pnl_Back.Enabled = False
        If cbo_Open.Enabled And cbo_Open.Visible Then cbo_Open.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        'MessageBox.Show("insert record")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-----'
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim nr As Long = 0
        Dim itmgrp_id As Integer = 0
        Dim unt_id As Integer = 0
        Dim da1 As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim Job_Sts As Integer = 0
        Dim itmstyl_id As Integer = 0
        Dim itmsiz_id As Integer = 0
        Dim vSurNM As String = ""
        Dim Dlr_Id As Integer = 0


        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Item_Creation, new_entry) = False Then Exit Sub

        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Select Item Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_Name.Enabled Then txt_Name.Focus()
            Exit Sub
        End If

        If Trim(Common_Procedures.settings.CustomerCode) = "1051" Then

            If Trim(txt_Code.Text) = "" Then
                MessageBox.Show("Select Item cODE", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_Code.Enabled Then txt_Code.Focus()
                Exit Sub
            End If

            If Trim(UCase(txt_Code.Text)) <> "" Then
                da1 = New SqlClient.SqlDataAdapter("select a.* from item_head a where a.item_code = '" & Trim(txt_Code.Text) & "'", con)
                dt1 = New DataTable
                da1.Fill(dt1)
                If dt1.Rows.Count > 0 Then
                    If lbl_IdNo.Text <> dt1.Rows(0)("Item_IdNo").ToString Then
                        MessageBox.Show("Duplicate Item Code", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If txt_Code.Enabled Then txt_Code.Focus()
                        Exit Sub
                    End If
                End If
                dt1.Dispose()
                da1.Dispose()
            End If

        End If

        Dlr_Id = Common_Procedures.Ledger_NameToIdNo(con, cbo_DealerName.Text)

        da = New SqlClient.SqlDataAdapter("select itemgroup_idno from itemgroup_head where itemgroup_name = '" & Trim(cbo_ItemGroup.Text) & "'", con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                itmgrp_id = Val(dt.Rows(0)(0).ToString)
            End If
        End If

        dt.Clear()

        If Val(itmgrp_id) = 0 Then
            MessageBox.Show("Select Item Group(HSN CODE)", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ItemGroup.Enabled Then cbo_ItemGroup.Focus()
            Exit Sub
        End If

        da = New SqlClient.SqlDataAdapter("select Style_idno from Style_head where Style_name = '" & Trim(cbo_Style.Text) & "'", con)
        da.Fill(dt3)
        itmstyl_id = 0
        If dt3.Rows.Count > 0 Then
            If IsDBNull(dt3.Rows(0)(0).ToString) = False Then
                itmstyl_id = Val(dt3.Rows(0)(0).ToString)
            End If
        End If

        If cbo_Style.Visible = True And cbo_Style.Enabled = True Then
            If Val(itmstyl_id) = 0 Then
                MessageBox.Show("Select Item Style", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Style.Enabled Then cbo_Style.Focus()
                Exit Sub
            End If
        End If

        itmsiz_id = 0
        da = New SqlClient.SqlDataAdapter("select Size_idno from Size_head where Size_name = '" & Trim(cbo_Size.Text) & "'", con)
        da.Fill(dt4)

        If dt4.Rows.Count > 0 Then
            If IsDBNull(dt4.Rows(0)(0).ToString) = False Then
                itmsiz_id = Val(dt4.Rows(0)(0).ToString)
            End If
        End If

        If cbo_Size.Visible = True And cbo_Size.Enabled = True Then
            If Val(itmsiz_id) = 0 Then
                MessageBox.Show("Select Item Size", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Size.Enabled Then cbo_Size.Focus()
                Exit Sub
            End If
        End If

        Close_STS = 0
        If chk_Close_Status.Checked = True Then Close_STS = 1

        da = New SqlClient.SqlDataAdapter("select unit_idno from unit_head where unit_name = '" & Trim(cbo_Unit.Text) & "'", con)
        da.Fill(dt2)

        unt_id = 0
        If dt2.Rows.Count > 0 Then
            If IsDBNull(dt2.Rows(0)(0).ToString) = False Then
                unt_id = Val(dt2.Rows(0)(0).ToString)
            End If
        End If

        If Val(unt_id) = 0 Then
            MessageBox.Show("Select Unit", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Unit.Enabled Then cbo_Unit.Focus()
            Exit Sub
        End If

        Job_Sts = 0
        If chk_JobWorkStatus.Checked = True Then
            Job_Sts = 1
        End If

        Dim vITM_NM As String = ""

        vITM_NM = Trim(txt_Name.Text)
        If cbo_Style.Visible Then
            If Trim(cbo_Style.Text) <> "" Then
                vITM_NM = Trim(vITM_NM) & " - " & Trim(cbo_Style.Text)
            End If
        End If

        If cbo_Size.Visible Then
            If Trim(cbo_Size.Text) <> "" Then
                vITM_NM = Trim(vITM_NM) & " - " & Trim(cbo_Size.Text)
            End If
        End If

        vSurNM = Common_Procedures.Remove_NonCharacters(vITM_NM)

        tr = con.BeginTransaction

        Try
            cmd.Connection = con
            cmd.Transaction = tr
            cmd.CommandText = "update item_head set Item_Name = '" & Trim(vITM_NM) & "', Sur_Name = '" & Trim(vSurNM) & "', Item_Description = '" & Trim(txt_description.Text) & "' , Item_Code = '" & Trim(txt_Code.Text) & "', Item_Name_Tamil = '" & Trim(txt_TamilName.Text) & "' , ItemGroup_IdNo = " & Str(Val(itmgrp_id)) & ", Unit_IdNo = " & Str(Val(unt_id)) & ", Minimum_Stock = " & Str(Val(txt_MinimumStock.Text)) & ", Tax_Percentage = " & Str(Val(txt_VatTaxPerc.Text)) & ", Sale_TaxRate = " & Str(Val(txt_VatTaxRate.Text)) & ", Sales_Rate = " & Str(Val(txt_SalesRate_Excl_Tax.Text)) & ", Cost_Rate = " & Str(Val(txt_CostRate_Incl_Tax.Text)) & " , MRP_Rate =  " & Str(Val(txt_Mrp.Text)) & " ,Job_Work_Status = " & Val(Job_Sts) & " ,Gst_Percentage = " & Val(txt_GSTTaxPerc.Text) & " ,Gst_Rate =" & Val(txt_Sales_GSTRate.Text) & " ,Discount_Percentage = " & Val(txt_DiscountPercentage.Text) & ", Rack_No = '" & Trim(txt_Rack_No.Text) & "',Sales_Profit_Retail =" & Val(txt_SalesProfit_Retail.Text) & " , Sales_Rate_Retail =" & Val(txt_SalesRate_Retail.Text) & " ,Close_status=" & Str(Val(Close_STS)) & ", Sales_Profit_Wholesale =" & Val(txt_SalesProfit_Wholesale.Text) & " , Sales_Rate_Wholesale =" & Val(txt_SalesRate_Wholesale.Text) & ",  Item_DisplayName = '" & Trim(txt_Name.Text) & "', Item_Size_IdNo =  " & Str(Val(itmsiz_id)) & " , Item_Style_IdNo = " & Str(Val(itmstyl_id)) & " , Dealer_IdNo = " & Trim(Dlr_Id) & ", CostRate_Excl_Tax = " & Str(Val(txt_CostRate_Excl_Tax.Text)) & ", HSN_Code = '" & Trim(txt_HSNCode.Text) & "' Where Item_IdNo = " & Str(Val(lbl_IdNo.Text))

            nr = cmd.ExecuteNonQuery

            If nr <> 0 Then new_entry = False

            If nr = 0 Then
                cmd.CommandText = "Insert into item_head (        Item_IdNo              ,       Item_Name         ,         Sur_Name        ,           Item_Code           ,           ItemGroup_IdNo    ,         Unit_IdNo        ,                 Minimum_Stock          ,                 Tax_Percentage       ,                 Sale_TaxRate         ,                 Sales_Rate                   ,                  Cost_Rate                   ,              Item_Name_Tamil      ,   MRP_Rate                    ,Job_Work_Status      ,  Gst_Percentage                 ,       Gst_Rate                    ,             Item_Description       ,     Discount_Percentage                ,              Rack_No            ,            Sales_Profit_Retail          ,             Sales_Rate_Retail          ,             Sales_Profit_Wholesale          ,             Sales_Rate_Wholesale          ,          Item_DisplayName     ,       Item_Size_IdNo        ,         Item_Style_IdNo      ,      Dealer_IdNo   ,                  CostRate_Excl_Tax           ,Close_Status            ,HSN_Code   ) " & _
                                  " values               (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(vITM_NM) & "' ,  '" & Trim(vSurNM) & "' , '" & Trim(txt_Code.Text) & "' , " & Str(Val(itmgrp_id)) & " , " & Str(Val(unt_id)) & " , " & Str(Val(txt_MinimumStock.Text)) & ", " & Str(Val(txt_VatTaxPerc.Text)) & ", " & Str(Val(txt_VatTaxRate.Text)) & ", " & Str(Val(txt_SalesRate_Excl_Tax.Text)) & ", " & Str(Val(txt_CostRate_Incl_Tax.Text)) & " , '" & Trim(txt_TamilName.Text) & "', " & Str(Val(txt_Mrp.Text)) & "," & Val(Job_Sts) & " ," & Val(txt_GSTTaxPerc.Text) & " ," & Val(txt_Sales_GSTRate.Text) & ",'" & Trim(txt_description.Text) & "'," & Val(txt_DiscountPercentage.Text) & ",'" & Trim(txt_Rack_No.Text) & "' ," & Val(txt_SalesProfit_Retail.Text) & " , " & Val(txt_SalesRate_Retail.Text) & " , " & Val(txt_SalesProfit_Wholesale.Text) & " , " & Val(txt_SalesRate_Wholesale.Text) & " , '" & Trim(txt_Name.Text) & "' , " & Str(Val(itmsiz_id)) & " , " & Str(Val(itmstyl_id)) & " , " & Val(Dlr_Id) & ",  " & Str(Val(txt_CostRate_Excl_Tax.Text)) & " ," & Str(Val(Close_STS)) & ",'" & Trim(txt_HSNCode.Text) & "')"
                cmd.ExecuteNonQuery()
                new_entry = True
            End If

            tr.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(txt_Name.Text)
            Common_Procedures.Master_Return.Master_Type = "ITEM"


            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If

            If new_entry = True Then new_record()

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()

    End Sub

    Private Sub Item_Creation_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ItemGroup.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ITEMGROUP" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_ItemGroup.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If

        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Style.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "STYLE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_Style.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If

        If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Size.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "Brand" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            cbo_Size.Text = Trim(Common_Procedures.Master_Return.Return_Value)
        End If
    End Sub

    Private Sub Item_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If grp_Filter.Visible Then
                Call btn_CloseFilter_Click(sender, e)
                Exit Sub
            ElseIf grp_Open.Visible Then
                Call btnClose_Click(sender, e)
                Exit Sub
            ElseIf MessageBox.Show("Do you want to Close?...", "FOR CLOSE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                Me.Close()
            Else
                Exit Sub
            End If
        End If
    End Sub

    Private Sub Item_Creation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable


        If Trim(Common_Procedures.settings.CustomerCode) = "1003" Then
            Me.Text = "COUNT CREATION"
            lbl_FormHeading.Text = "COUNT CREATION"
            lbl_ItemGroup_Caption.Text = "Item Description"
        End If

        cbo_ItemGroup.Text = ""
        cbo_DealerName.Text = ""
        txt_TamilName.Text = ""
        txt_description.Text = ""
        txt_Rack_No.Text = ""
        txt_HSNCode.Text = ""
        txt_GSTTaxPerc.Text = ""
        txt_description.Left = txt_TamilName.Left
        txt_description.Width = txt_TamilName.Width
        txt_Rack_No.Left = txt_TamilName.Left
        txt_Rack_No.Width = txt_TamilName.Width

        btn_fromExcel.Visible = False
        If Trim(Common_Procedures.settings.CustomerCode) = "1219" Or Trim(Common_Procedures.settings.CustomerCode) = "1235" Then ' vels enterprises
            btn_fromExcel.Visible = True
            txt_TamilName.Visible = False
            lbl_description_Caption.Text = "Description"
            txt_description.Visible = True
        End If
        If Trim(Common_Procedures.settings.CustomerCode) = "1365" Then
            btn_fromExcel.Visible = True

            lbl_FormHeading.Text = "ITEM CREATION  -  SHADE"

            'lbl_Name_Caption.Text = "Shade Name"
            'lbl_ItemGroup_Caption.Text = "Item Group"

        End If

        If Trim(Common_Procedures.settings.CustomerCode) = "1005" Or Trim(Common_Procedures.settings.CustomerCode) = "1167" Then ' f FASHIONS
            txt_TamilName.Visible = False
            lbl_description_Caption.Visible = False
            cbo_Size.Visible = True
            cbo_Style.Visible = True
            lbl_SizeCaption.Visible = True
            lbl_StyleCaption.Visible = True
            btn_fromExcel.Visible = False
            lbl_mrp_Caption.Visible = False
            txt_Mrp.Visible = False
            lbl_Code_Caption.Visible = False

            lbl_Sales_Rate_GST_Caption.Visible = False
            lbl_sales_Rate_Vat_Caption.Visible = False
            txt_Code.Visible = False
            lbl_CostRate_Excl_Tax_Caption.Visible = False
            txt_CostRate_Excl_Tax.Visible = False
            lbl_CostRate_Incl_Tax_Caption.Visible = False
            txt_CostRate_Incl_Tax.Visible = False
            txt_VatTaxRate.Visible = False
            txt_Sales_GSTRate.Visible = False
            txt_SalesRate_Excl_Tax.Visible = False
            lbl_sales_Rate_Excl_Tax_Caption.Visible = False

            cbo_ItemGroup.Width = cbo_Unit.Width
            lbl_DealerName.Visible = True
            cbo_DealerName.Visible = True
        End If

        If Trim(Common_Procedures.settings.CustomerCode) = "1231" Then '  avi
            txt_TamilName.Visible = False
            lbl_description_Caption.Text = "Rack No"
            lbl_DiscountPercCaption.Visible = True
            txt_DiscountPercentage.Visible = True
            txt_Rack_No.Visible = True
        End If

        If Trim(Common_Procedures.settings.CustomerCode) = "1247" Then ' siruvayal organic , CHOLAR AGENCY
            lbl_description_Caption.Visible = False

            lbl_SalesProfit_Retail.Visible = True
            lbl_SalesRate_Retail.Visible = True
            lbl_SalesProfit_Wholesale.Visible = True
            lbl_SalesRate_WholeSale.Visible = True

            txt_SalesProfit_Retail.Visible = True
            txt_SalesRate_Retail.Visible = True
            txt_SalesProfit_Wholesale.Visible = True
            txt_SalesRate_Wholesale.Visible = True

            cbo_ItemGroup.Width = 160
            lbl_DealerName.Visible = True
            cbo_DealerName.Visible = True
            txt_Mrp.Visible = True
            lbl_mrp_Caption.Visible = True
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1218" Then '---ENERGY MAN POWER TECHNOLOGIES INDIA PVT LTD
            cbo_ItemGroup.Width = 160
            lbl_DealerName.Visible = True
            cbo_DealerName.Visible = True
            chk_Close_Status.Visible = True


        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then
            lbl_description_Caption.Visible = False
        End If
        If Trim(Common_Procedures.settings.CustomerCode) = "1366" Or Trim(Common_Procedures.settings.CustomerCode) = "1171" Or Trim(Common_Procedures.settings.CustomerCode) = "1375" Then
            lbl_tamilname_Caption.Visible = True
            txt_TamilName.Visible = True
            lbl_tamilname_Caption.BackColor = Color.SkyBlue
            txt_TamilName.BackColor = Color.White
            btn_Character.Visible = True
            txt_description.Visible = False

        End If
        con.Open()

        da = New SqlClient.SqlDataAdapter("select itemgroup_name from itemgroup_head order by itemgroup_name", con)
        da.Fill(dt1)

        cbo_ItemGroup.Items.Clear()

        cbo_ItemGroup.DataSource = dt1
        cbo_ItemGroup.DisplayMember = "itemgroup_name"
        'cbo_ItemGroup.ValueMember = "itemgroup_idno"

        da = New SqlClient.SqlDataAdapter("select unit_name from unit_head order by unit_name", con)

        da.Fill(dt2)

        cbo_Unit.DataSource = dt2
        cbo_Unit.DisplayMember = "unit_name"
        'cbo_Unit.ValueMember = "unit_idno"

        da = New SqlClient.SqlDataAdapter("select Style_name from Style_head order by Style_name", con)

        da.Fill(dt3)

        cbo_Style.Items.Clear()
        cbo_Style.DataSource = dt3
        cbo_Style.DisplayMember = "Style_name"


        da = New SqlClient.SqlDataAdapter("select Size_name from Size_head order by Size_name", con)

        da.Fill(dt4)

        cbo_Size.Items.Clear()
        cbo_Size.DataSource = dt4
        cbo_Size.DisplayMember = "Size_name"


        grp_Open.Visible = False
        grp_Open.Left = (Me.Width - grp_Open.Width) \ 2
        grp_Open.Top = (Me.Height - grp_Open.Height) \ 2
        grp_Open.BringToFront()

        grp_Filter.Visible = False
        grp_Filter.Left = (Me.Width - grp_Filter.Width) \ 2
        grp_Filter.Top = (Me.Height - grp_Filter.Height) \ 2
        grp_Filter.BringToFront()

        chk_JobWorkStatus.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "2001" Then  'Deva
            chk_JobWorkStatus.Visible = True
            txt_Code.Width = 290
        End If

        AddHandler cbo_DealerName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ItemGroup.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Open.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Size.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Style.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Unit.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Code.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CostRate_Excl_Tax.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CostRate_Incl_Tax.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_description.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscountPercentage.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Sales_GSTRate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_GSTTaxPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_MinimumStock.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Mrp.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rack_No.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SalesRate_Excl_Tax.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SalesProfit_Retail.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SalesProfit_Wholesale.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SalesRate_Retail.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SalesRate_Wholesale.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TamilName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_VatTaxPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_VatTaxRate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_HSNCode.GotFocus, AddressOf ControlGotFocus


        AddHandler cbo_DealerName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ItemGroup.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Open.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Size.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Style.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Unit.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Code.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CostRate_Excl_Tax.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CostRate_Incl_Tax.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_description.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiscountPercentage.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Sales_GSTRate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_GSTTaxPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_MinimumStock.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Mrp.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rack_No.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SalesRate_Excl_Tax.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SalesProfit_Retail.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SalesProfit_Wholesale.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SalesRate_Retail.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SalesRate_Wholesale.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TamilName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_VatTaxPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_VatTaxRate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_HSNCode.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_HSNCode.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_HSNCode.KeyPress, AddressOf TextBoxControl_KeyPress

        AddHandler txt_Code.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_CostRate_Excl_Tax.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_CostRate_Incl_Tax.KeyDown, AddressOf TextBoxControl_KeyDown
        'AddHandler txt_deccription.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_DiscountPercentage.KeyDown, AddressOf TextBoxControl_KeyDown
        'AddHandler txt_GSTRate.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_GSTTaxPerc.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_MinimumStock.KeyDown, AddressOf TextBoxControl_KeyDown
        'AddHandler txt_Mrp.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_Name.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_Rack_No.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_SalesRate_Excl_Tax.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_SalesProfit_Retail.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_SalesProfit_Wholesale.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_SalesRate_Retail.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_SalesRate_Wholesale.KeyDown, AddressOf TextBoxControl_KeyDown
        'AddHandler txt_TamilName.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_VatTaxPerc.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_VatTaxRate.KeyDown, AddressOf TextBoxControl_KeyDown

        AddHandler txt_Code.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_CostRate_Excl_Tax.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_CostRate_Incl_Tax.KeyPress, AddressOf TextBoxControl_KeyPress
        'AddHandler txt_deccription.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_DiscountPercentage.KeyPress, AddressOf TextBoxControl_KeyPress
        'AddHandler txt_GSTRate.KeyPress, AddressOf TextBoxControl_KeyPress
        'AddHandler txt_GSTTaxPerc.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_MinimumStock.KeyPress, AddressOf TextBoxControl_KeyPress
        'AddHandler txt_Mrp.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_Name.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_Rack_No.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_SalesRate_Excl_Tax.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_SalesProfit_Retail.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_SalesProfit_Wholesale.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_SalesRate_Retail.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_SalesRate_Wholesale.KeyPress, AddressOf TextBoxControl_KeyPress
        'AddHandler txt_TamilName.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_VatTaxPerc.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_VatTaxRate.KeyPress, AddressOf TextBoxControl_KeyPress


        new_record()

    End Sub

    Private Sub cbo_Open_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Open.GotFocus
        cbo_Open.BackColor = Color.Lime
        cbo_Open.ForeColor = Color.Blue
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

    Private Sub btn_Find_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Find.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select item_idno from item_head where item_name = '" & Trim(cbo_Open.Text) & "'", con)
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
            MessageBox.Show(ex.Message, "FOR FINDING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        'Me.Height = 400

    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        pnl_Back.Enabled = True
        grp_Open.Visible = False

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
    End Sub

    Private Sub cbo_ItemGroup_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemGroup.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemGroup, Nothing, Nothing, "itemgroup_head", "itemgroup_name", "", "(itemgroup_idno = 0)")
        If e.KeyValue = 38 And cbo_ItemGroup.DroppedDown = False Or (e.Control = True And e.KeyCode = 38) Then
            If txt_Code.Visible = True Then
                e.Handled = True
                txt_Code.Focus()
            Else
                e.Handled = True
                cbo_Size.Focus()
            End If
        End If
        If e.KeyValue = 40 And cbo_ItemGroup.DroppedDown = False Or (e.Control = True And e.KeyCode = 40) Then
            If cbo_DealerName.Visible Then
                e.Handled = True
                cbo_DealerName.Focus()
            Else
                cbo_Unit.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_ItemGroup_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemGroup.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ItemGroup, Nothing, "itemgroup_head", "itemgroup_name", "", "(itemgroup_idno = 0)")
        'Dim Indx As Integer = -1
        'Dim strFindStr As String = ""

        'Try
        '    If Asc(e.KeyChar) = 8 Then
        '        If cbo_ItemGroup.SelectionStart <= 1 Then
        '            cbo_ItemGroup.Text = ""
        '            Exit Sub
        '        End If
        '        If cbo_ItemGroup.SelectionLength = 0 Then
        '            strFindStr = cbo_ItemGroup.Text.Substring(0, cbo_ItemGroup.Text.Length - 1)
        '        Else
        '            strFindStr = cbo_ItemGroup.Text.Substring(0, cbo_ItemGroup.SelectionStart - 1)
        '        End If

        '    Else

        '        If cbo_ItemGroup.SelectionLength = 0 Then
        '            strFindStr = cbo_ItemGroup.Text & e.KeyChar
        '        Else
        '            strFindStr = cbo_ItemGroup.Text.Substring(0, cbo_ItemGroup.SelectionStart) & e.KeyChar
        '        End If

        '    End If

        '    Indx = cbo_ItemGroup.FindString(strFindStr)

        '    If Indx <> -1 Then
        '        cbo_ItemGroup.SelectedText = ""
        '        cbo_ItemGroup.SelectedIndex = Indx
        '        cbo_ItemGroup.SelectionStart = strFindStr.Length
        '        cbo_ItemGroup.SelectionLength = cbo_ItemGroup.Text.Length
        '        e.Handled = True
        '    Else
        '        e.Handled = True

        '    End If

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

        If Asc(e.KeyChar) = 13 Then
            If cbo_DealerName.Visible Then
                e.Handled = True
                cbo_DealerName.Focus()
            Else
                cbo_Unit.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Unit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Unit.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Unit, Nothing, Nothing, "Unit_Head", "Unit_Name", "", "(Unit_IdNo = 0)")

        If (e.KeyValue = 38 And cbo_Unit.DroppedDown = False) Or (e.Control = True And e.KeyCode = 38) Then
            If cbo_DealerName.Visible = True Then
                e.Handled = True
                cbo_DealerName.Focus()
            Else
                e.Handled = True
                cbo_ItemGroup.Focus()
            End If
        End If
        If (e.KeyValue = 40 And cbo_Unit.DroppedDown = False) Or (e.Control = True And e.KeyCode = 40) Then
            If txt_MinimumStock.Visible Then
                e.Handled = True
                txt_MinimumStock.Focus()
            Else
                txt_SalesRate_Excl_Tax.Focus()
            End If
        End If

    End Sub

    Private Sub txt_VatPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Not ((Asc(e.KeyChar) >= 48 And Asc(e.KeyChar) <= 57) Or Asc(e.KeyChar) = 46 Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 13) Then
            e.Handled = True
        End If
    End Sub

    Private Sub btn_OpenFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OpenFilter.Click
        Dim movid As Integer = 0

        Try
            movid = Val(dgv_Filter.CurrentRow.Cells(0).Value)

            If Val(movid) <> 0 Then
                move_record(movid)
                pnl_Back.Enabled = True
                grp_Filter.Visible = False
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)

        End Try
    End Sub

    Private Sub btn_CloseFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseFilter.Click
        pnl_Back.Enabled = True
        grp_Filter.Visible = False

        If txt_Name.Enabled And txt_Name.Visible Then txt_Name.Focus()
    End Sub

    Private Sub dgv_Filter_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
        If e.KeyValue = 13 Then
            Call btn_OpenFilter_Click(sender, e)
        End If
    End Sub

    Private Sub txt_SalesRate_Excl_Tax_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_SalesRate_Excl_Tax.KeyUp
        'txt_VatTaxRate.Text = Format(Val(txt_SalesRate_Excl_Tax.Text) * ((100 + Val(txt_VatTaxPerc.Text)) / 100), "##########0.00")
        txt_Sales_GSTRate.Text = Format(Val(txt_SalesRate_Excl_Tax.Text) * ((100 + Val(txt_GSTTaxPerc.Text)) / 100), "##########0.00")
    End Sub

    Private Sub txt_VatTaxRate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_VatTaxRate.KeyUp
        txt_SalesRate_Excl_Tax.Text = Format(Val(txt_VatTaxRate.Text) * (100 / (100 + Val(txt_VatTaxPerc.Text))), "#########0.00")
    End Sub

    Private Sub txt_VatTaxPerc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_VatTaxPerc.KeyUp
        txt_VatTaxRate.Text = Format(Val(txt_SalesRate_Excl_Tax.Text) * ((100 + Val(txt_VatTaxPerc.Text)) / 100), "########0.00")
    End Sub

    Private Sub txt_VatTaxRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_VatTaxRate.KeyPress
        If Not ((Asc(e.KeyChar) >= 48 And Asc(e.KeyChar) <= 57) Or Asc(e.KeyChar) = 46 Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 13) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_CostRate_Incl_Tax_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CostRate_Incl_Tax.KeyPress
        If Not ((Asc(e.KeyChar) >= 48 And Asc(e.KeyChar) <= 57) Or Asc(e.KeyChar) = 46 Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 13) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_CostRate_Excl_Tax_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CostRate_Excl_Tax.KeyPress
        If Not ((Asc(e.KeyChar) >= 48 And Asc(e.KeyChar) <= 57) Or Asc(e.KeyChar) = 46 Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 13) Then
            e.Handled = True
        End If
    End Sub

    Private Sub cbo_Unit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Unit.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Unit, Nothing, "Unit_Head", "Unit_Name", "", "(Unit_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If txt_MinimumStock.Visible Then
                e.Handled = True
                txt_MinimumStock.Focus()
            Else
                txt_SalesRate_Excl_Tax.Focus()
            End If
        End If
        'Dim FindStr As String = ""
        'Dim Indx As Integer = -1

        'If Asc(e.KeyChar) = 13 Then
        '    SendKeys.Send("{TAB}")
        'End If

        'If Asc(e.KeyChar) = 8 Then
        '    If cbo_Unit.SelectionStart <= 1 Then
        '        cbo_Unit.Text = ""
        '        Exit Sub
        '    End If

        '    If cbo_Unit.SelectionLength = 0 Then
        '        FindStr = cbo_Unit.Text.Substring(0, cbo_Unit.Text.Length - 1)
        '    Else
        '        FindStr = cbo_Unit.Text.Substring(0, cbo_Unit.SelectionStart - 1)
        '    End If

        'Else
        '    If cbo_Unit.SelectionLength = 0 Then
        '        FindStr = cbo_Unit.Text & e.KeyChar
        '    Else
        '        FindStr = cbo_Unit.Text.Substring(0, cbo_Unit.SelectionStart) & e.KeyChar
        '    End If

        'End If

        'Indx = cbo_Unit.FindString(FindStr)

        'If Indx <> -1 Then
        '    cbo_Unit.SelectedText = ""
        '    cbo_Unit.SelectedIndex = Indx
        '    cbo_Unit.SelectionStart = FindStr.Length
        '    cbo_Unit.SelectionLength = cbo_Unit.Text.Length
        'End If
        'e.Handled = True

    End Sub

    Private Sub txt_TaxRate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_VatTaxRate.KeyDown
        'If e.KeyCode = 40 Then
        '    txt_Mrp.Focus()
        'End If
        'If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Rate_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SalesRate_Excl_Tax.KeyPress
        If Not ((Asc(e.KeyChar) >= 48 And Asc(e.KeyChar) <= 57) Or Asc(e.KeyChar) = 46 Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 13) Then
            e.Handled = True
        End If
        'If Asc(e.KeyChar) = 13 Then
        '    SendKeys.Send("{TAB}")
        'End If
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

    Private Sub txt_MinimumStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_MinimumStock.KeyPress
        If Not ((Asc(e.KeyChar) >= 48 And Asc(e.KeyChar) <= 57) Or Asc(e.KeyChar) = 46 Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 13) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_TamilName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TamilName.KeyDown
        If e.KeyCode = 40 Then
            'If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
            '    save_record()
            'End If
            txt_Code.Focus()
        End If
        If e.KeyCode = 38 Then txt_Name.Focus()
    End Sub

    Private Sub txt_TamilName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TamilName.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Code.Focus()
        End If
    End Sub

    Private Sub txt_deccription_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_description.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If
        If e.KeyCode = 38 Then txt_Mrp.Focus()
    End Sub

    Private Sub txt_deccription_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_description.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub

    Private Sub txt_GSTTaxPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_GSTTaxPerc.KeyDown
        'If e.KeyCode = 40 Then
        '    txt_GSTRate.Focus()
        'End If
        'If e.KeyCode = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_GSTTaxPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_GSTTaxPerc.KeyPress
        If Not ((Asc(e.KeyChar) >= 48 And Asc(e.KeyChar) <= 57) Or Asc(e.KeyChar) = 46 Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 13) Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_GSTTaxPerc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_GSTTaxPerc.KeyUp
        txt_Sales_GSTRate.Text = Format(Val(txt_SalesRate_Excl_Tax.Text) * ((100 + Val(txt_GSTTaxPerc.Text)) / 100), "########0.00")
        txt_CostRate_Incl_Tax.Text = Format(Val(txt_CostRate_Excl_Tax.Text) * ((100 + Val(txt_GSTTaxPerc.Text)) / 100), "########0.00")
    End Sub

    Private Sub txt_GSTRate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Sales_GSTRate.KeyDown
        If e.KeyCode = 40 Then
            If txt_Mrp.Visible = True Then
                txt_Mrp.Focus()
            Else
                txt_DiscountPercentage.Focus()
            End If
            'txt_Mrp.Focus()
        End If
        If e.KeyCode = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_GSTRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Sales_GSTRate.KeyPress
        If Not ((Asc(e.KeyChar) >= 48 And Asc(e.KeyChar) <= 57) Or Asc(e.KeyChar) = 46 Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 13) Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If txt_Mrp.Visible = True Then
                txt_Mrp.Focus()
            Else
                If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    txt_Name.Focus()
                End If
                'txt_DiscountPercentage.Focus()
            End If


        End If
    End Sub

    Private Sub txt_GSTRate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Sales_GSTRate.KeyUp
        txt_SalesRate_Excl_Tax.Text = Format(Val(txt_Sales_GSTRate.Text) * (100 / (100 + Val(txt_GSTTaxPerc.Text))), "#########0.00")
    End Sub

    Private Sub cbo_ItemGroup_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemGroup.TextChanged
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim ItmGrpIdNo As Integer = 0

        If Common_Procedures.settings.CustomerCode <> "1308" Then
            ItmGrpIdNo = Val(Common_Procedures.ItemGroup_NameToIdNo(con, Trim(cbo_ItemGroup.Text)))
            If ItmGrpIdNo = 0 Then Exit Sub

            Try

                da = New SqlClient.SqlDataAdapter("select Item_HSN_Code, Item_GST_Percentage from ItemGroup_Head a  where ItemGroup_IdNo = " & Str(Val(ItmGrpIdNo)) & "", con)
                da.Fill(dt)

                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0).Item("Item_GST_Percentage").ToString) = False Then
                        txt_GSTTaxPerc.Text = dt.Rows(0).Item("Item_GST_Percentage").ToString
                    End If
                    If IsDBNull(dt.Rows(0).Item("Item_HSN_Code").ToString) = False Then
                        txt_HSNCode.Text = dt.Rows(0).Item("Item_HSN_Code").ToString
                    End If
                End If

            Catch ex As Exception
                MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try
        End If


    End Sub

    Private Sub btn_fromExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_fromExcel.Click
        Dim FileName As String = ""
        Dim Sts1 As Boolean = False
        Dim Sts2 As Boolean = False
        Dim Sts3 As Boolean = False
        Dim Sts4 As Boolean = False

        'Try

        Dim g As New Password
        g.ShowDialog()

        If Trim(UCase(Common_Procedures.Password_Input)) <> "TSIMP123" Then
            MessageBox.Show("Select Password", "PASSWORD FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        OpenFileDialog1.ShowDialog()
        FileName = OpenFileDialog1.FileName


        If Not IO.File.Exists(FileName) Then
            MessageBox.Show(FileName & " File not found", "File not found...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1167" Then  '---  TEXVALLEY - F FASHIONS (PERUNDURAI)
            getExcelData_ItemGroup_1167(FileName, Sts2)
            getExcelData_Size_1167(FileName, Sts1)
            getExcelData_Style_1167(FileName, Sts1)
            getExcelData_ItemName_1167(FileName, Sts3)

            If Sts1 = True Or Sts2 = True Or Sts3 = True Then
                MessageBox.Show("Imported Sucessfully!!!", "FOR IMPORTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Error on Import", "FOR IMPORTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If

        ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1219" Then ' vels enterprises

            getExcelData_Category_1219(FileName, Sts1)
            getExcelData_ItemGroup_1219(FileName, Sts2)
            getExcelData_ItemName_1219(FileName, Sts3)

            If Sts1 = True And Sts2 = True And Sts3 = True Then
                MessageBox.Show("Imported Sucessfully!!!", "FOR IMPORTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Error on Import", "FOR IMPORTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If

        ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1235" Then ' Kalpana textile
            getExcelData_ItemName_1235(FileName, Sts3)

            If Sts3 = True Then
                MessageBox.Show("Imported Sucessfully!!!", "FOR IMPORTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Error on Import", "FOR IMPORTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If


        ElseIf Trim(Common_Procedures.settings.CustomerCode) = "1365" Then ' NACHIYAR TRADINGS
            getExcelData_UNITHEAD_1365(FileName, Sts4)

            getExcelData_Category_1365(FileName, Sts1)

            getExcelData_ItemGroup_1365(FileName, Sts2)

            getExcelData_ItemName_1365(FileName, Sts3)

            If Sts1 = True And Sts2 = True And Sts3 = True And Sts4 = True Then
                MessageBox.Show("Imported Sucessfully!!!", "FOR IMPORTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            Else
                MessageBox.Show("Error on Import", "FOR IMPORTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        End If



        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

    End Sub

    Private Sub getExcelData_ItemName_1235(ByVal FileName As String, ByRef Sts As Boolean)
        Dim cmd As New SqlClient.SqlCommand
        'Dim tr As SqlClient.SqlTransaction
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable

        Dim xlApp As Excel.Application
        Dim xlWorkBook As Excel.Workbook
        Dim xlWorkSheet As Excel.Worksheet

        Dim RowCnt As Long = 0
        '  Dim FileName As String = ""
        Dim NewCode As String = ""
        Dim ShfId As Integer = 0
        Dim ItmId As Integer = 0
        Dim n As Integer = 0
        Dim Sn As Integer = 0
        Dim itemGRP_Id As Integer = 0
        Dim itemId As Integer = 0
        Dim Sur As String = ""
        Dim Cat_Id As Integer = 0
        Dim itemNM As String = ""


        Try
            xlApp = New Excel.Application
            xlWorkBook = xlApp.Workbooks.Open(FileName)
            xlWorkSheet = xlWorkBook.Worksheets("sheet1")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub

        End Try

        'Try

        With xlWorkSheet
            RowCnt = .Range("A" & .Rows.Count).End(Excel.XlDirection.xlUp).Row
        End With

        'RowCnt = xlWorkSheet.UsedRange.Rows.Count

        If RowCnt <= 1 Then
            MessageBox.Show("Data not found", "Data not found...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If


        For i = 2 To RowCnt

            itemNM = Trim(xlWorkSheet.Cells(i, 1).value)
            itemNM = Replace(itemNM, "'", "`")

            If Trim(itemNM) = "" Then Continue For

            If Trim(UCase(xlWorkSheet.Cells(i, 12).value)) = "TRUE" Then Continue For

            itemId = Val(Common_Procedures.Item_NameToIdNo(con, Trim(itemNM)))

            If itemId <> 0 Then
                Continue For
            End If

            Sur = Common_Procedures.Remove_NonCharacters(Trim(itemNM))

            itemId = Val(Common_Procedures.get_FieldValue(con, "item_head", "Item_IdNo", "(Sur_Name = '" & Trim(Sur) & "')"))
            If itemId <> 0 Then
                Continue For
            End If



            itemGRP_Id = Val(xlWorkSheet.Cells(i, 2).value)

            Cat_Id = 0

            itemId = Val(xlWorkSheet.Cells(i, 11).value)
            If Trim(Common_Procedures.Item_IdNoToName(con, itemId)) <> "" Then
                itemId = Common_Procedures.get_MaxIdNo(con, "item_head", "Item_IdNo", "")
            End If

            cmd.Connection = con


            cmd.CommandText = "Insert into item_head ( Item_IdNo     ,    Item_Name         , Sur_Name           ,  Item_Code ,          ItemGroup_IdNo       , Unit_IdNo , Minimum_Stock  , MRP_Rate , Gst_Percentage       ) " & _
                                    "values (" & Str(Val(itemId)) & ", '" & Trim(itemNM) & "', '" & Trim(Sur) & "',     ''     , " & Str(Val(itemGRP_Id)) & "  ,   1       ,       0        ,   0      ,         0            ) "
            cmd.ExecuteNonQuery()

        Next i

        movelast_record()


        xlWorkBook.Close(False, FileName)
        xlApp.Quit()


        ReleaseComObject(xlWorkSheet)
        ReleaseComObject(xlWorkBook)
        ReleaseComObject(xlApp)

        Sts = True
        'MessageBox.Show("Imported Sucessfully!!!", "FOR IMPORTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        'Catch ex As Exception

        '    Sts = False

        '    xlWorkBook.Close(False, FileName)
        '    xlApp.Quit()


        '    ReleaseComObject(xlWorkSheet)
        '    ReleaseComObject(xlWorkBook)
        '    ReleaseComObject(xlApp)

        '    ' MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try


    End Sub


    Private Sub getExcelData_ItemName_1219(ByVal FileName As String, ByRef Sts As Boolean)
        Dim cmd As New SqlClient.SqlCommand
        'Dim tr As SqlClient.SqlTransaction
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable

        Dim xlApp As Excel.Application
        Dim xlWorkBook As Excel.Workbook
        Dim xlWorkSheet As Excel.Worksheet

        Dim RowCnt As Long = 0
        '  Dim FileName As String = ""
        Dim NewCode As String = ""
        Dim ShfId As Integer = 0
        Dim ItmId As Integer = 0
        Dim n As Integer = 0
        Dim Sn As Integer = 0
        Dim itemGRP_Id As Integer = 0
        Dim itemId As Integer = 0
        Dim Sur As String = ""
        Dim Cat_Id As Integer = 0

        Try
            xlApp = New Excel.Application
            xlWorkBook = xlApp.Workbooks.Open(FileName)
            xlWorkSheet = xlWorkBook.Worksheets("sheet1")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub

        End Try

        Try

            With xlWorkSheet
                RowCnt = .Range("A" & .Rows.Count).End(Excel.XlDirection.xlUp).Row
            End With

            'RowCnt = xlWorkSheet.UsedRange.Rows.Count

            If RowCnt <= 1 Then
                MessageBox.Show("Data not found", "Data not found...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If


            For i = 2 To RowCnt

                itemId = Val(Common_Procedures.Item_NameToIdNo(con, Trim(xlWorkSheet.Cells(i, 4).value)))


                If itemId <> 0 Then Continue For

                Sur = Common_Procedures.Remove_NonCharacters(Trim(xlWorkSheet.Cells(i, 4).value))

                itemGRP_Id = Val(Common_Procedures.ItemGroup_NameToIdNo(con, Trim(xlWorkSheet.Cells(i, 2).value)))

                Cat_Id = Val(Common_Procedures.Cetegory_NameToIdNo(con, Trim(xlWorkSheet.Cells(i, 1).value)))


                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "item_head", "Item_IdNo", "")

                cmd.Connection = con


                cmd.CommandText = "Insert into item_head(Item_IdNo              , Item_Name                                   , Sur_Name            , Item_Code                                    ,          ItemGroup_IdNo       , Unit_IdNo , Minimum_Stock  , MRP_Rate                                   ,Gst_Percentage                             ) " & _
                                        "values (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(xlWorkSheet.Cells(i, 4).value) & "', '" & Trim(Sur) & "', '" & Trim(xlWorkSheet.Cells(i, 3).value) & "', " & Str(Val(itemGRP_Id)) & "  ,   1       ,       0        , " & Val(xlWorkSheet.Cells(i, 7).value) & " ," & Val(xlWorkSheet.Cells(i, 9).value) & " )"
                cmd.ExecuteNonQuery()

            Next i

            movelast_record()


            xlWorkBook.Close(False, FileName)
            xlApp.Quit()


            ReleaseComObject(xlWorkSheet)
            ReleaseComObject(xlWorkBook)
            ReleaseComObject(xlApp)

            Sts = True
            'MessageBox.Show("Imported Sucessfully!!!", "FOR IMPORTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception

            Sts = False

            xlWorkBook.Close(False, FileName)
            xlApp.Quit()


            ReleaseComObject(xlWorkSheet)
            ReleaseComObject(xlWorkBook)
            ReleaseComObject(xlApp)

            ' MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub

    Private Sub getExcelData_ItemGroup_1219(ByVal FileName As String, ByRef Sts As Boolean)
        Dim cmd As New SqlClient.SqlCommand
        'Dim tr As SqlClient.SqlTransaction
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable

        Dim xlApp As Excel.Application
        Dim xlWorkBook As Excel.Workbook
        Dim xlWorkSheet As Excel.Worksheet

        Dim RowCnt As Long = 0
        ' Dim FileName As String = ""
        Dim NewCode As String = ""
        Dim ShfId As Integer = 0
        Dim ItmId As Integer = 0
        Dim n As Integer = 0
        Dim Sn As Integer = 0
        Dim itemGRP_Id As Integer = 0
        Dim Sur As String = ""
        Dim Cat_Id As Integer = 0
        Dim mxId As Integer = 0

        Try
            xlApp = New Excel.Application
            xlWorkBook = xlApp.Workbooks.Open(FileName)
            xlWorkSheet = xlWorkBook.Worksheets("sheet1")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub

        End Try

        Try

            With xlWorkSheet
                RowCnt = .Range("A" & .Rows.Count).End(Excel.XlDirection.xlUp).Row
            End With

            'RowCnt = xlWorkSheet.UsedRange.Rows.Count

            If RowCnt <= 1 Then
                MessageBox.Show("Data not found", "Data not found...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If


            For i = 2 To RowCnt


                itemGRP_Id = Val(Common_Procedures.ItemGroup_NameToIdNo(con, Trim(xlWorkSheet.Cells(i, 2).value)))

                If itemGRP_Id <> 0 Then Continue For

                Cat_Id = Val(Common_Procedures.Cetegory_NameToIdNo(con, Trim(xlWorkSheet.Cells(i, 1).value)))

                mxId = Common_Procedures.get_MaxIdNo(con, "ItemGroup_Head", "itemgroup_idno", "")

                Sur = Common_Procedures.Remove_NonCharacters(Trim(xlWorkSheet.Cells(i, 2).value))

                cmd.Connection = con

                cmd.CommandText = "Insert into ItemGroup_Head(  itemgroup_idno     ,         itemgroup_name                       ,       Item_HSN_Code                         ,       sur_name        , Cetegory_IdNo         , Item_GST_Percentage ) " & _
                                                    "values (" & Str(Val(mxId)) & ", '" & Trim(xlWorkSheet.Cells(i, 2).value) & "','" & Trim(xlWorkSheet.Cells(i, 8).value) & "', '" & Trim(Sur) & "' ," & Str(Val(Cat_Id)) & " ," & Str(Val(xlWorkSheet.Cells(i, 9).value)) & ")"
                cmd.ExecuteNonQuery()

            Next i

            movelast_record()


            xlWorkBook.Close(False, FileName)
            xlApp.Quit()


            ReleaseComObject(xlWorkSheet)
            ReleaseComObject(xlWorkBook)
            ReleaseComObject(xlApp)

            Sts = True

            '  MessageBox.Show("Imported Sucessfully!!!", "FOR IMPORTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception

            Sts = False

            xlWorkBook.Close(False, FileName)
            xlApp.Quit()


            ReleaseComObject(xlWorkSheet)
            ReleaseComObject(xlWorkBook)
            ReleaseComObject(xlApp)


            'MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub

    Private Sub getExcelData_Category_1219(ByVal FileName As String, ByRef Sts As Boolean)
        Dim cmd As New SqlClient.SqlCommand
        'Dim tr As SqlClient.SqlTransaction
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable

        Dim xlApp As Excel.Application
        Dim xlWorkBook As Excel.Workbook
        Dim xlWorkSheet As Excel.Worksheet

        Dim RowCnt As Long = 0
        '  Dim FileName As String = ""
        Dim NewCode As String = ""
        Dim ShfId As Integer = 0
        Dim ItmId As Integer = 0
        Dim n As Integer = 0
        Dim Sn As Integer = 0
        Dim Cat_Id As Integer = 0
        Dim Sur As String = ""
        Dim MxId As Integer = 0

        Try
            xlApp = New Excel.Application
            xlWorkBook = xlApp.Workbooks.Open(FileName)
            xlWorkSheet = xlWorkBook.Worksheets("sheet1")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub

        End Try

        Try
            'OpenFileDialog1.ShowDialog()
            'FileName = OpenFileDialog1.FileName


            With xlWorkSheet
                RowCnt = .Range("A" & .Rows.Count).End(Excel.XlDirection.xlUp).Row
            End With

            'RowCnt = xlWorkSheet.UsedRange.Rows.Count

            If RowCnt <= 1 Then
                MessageBox.Show("Data not found", "Data not found...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If


            For i = 2 To RowCnt


                Cat_Id = Val(Common_Procedures.Cetegory_NameToIdNo(con, Trim(xlWorkSheet.Cells(i, 1).value)))

                If Cat_Id <> 0 Then Continue For


                MxId = Common_Procedures.get_MaxIdNo(con, "Cetegory_Head", "Cetegory_IdNo", "")

                Sur = Common_Procedures.Remove_NonCharacters(Trim(xlWorkSheet.Cells(i, 1).value))

                cmd.Connection = con

                cmd.CommandText = "Insert into Cetegory_Head(Cetegory_IdNo, Cetegory_Name, Sur_Name) values (" & Str(Val(MxId)) & ", '" & Trim(xlWorkSheet.Cells(i, 1).value) & "', '" & Trim(Sur) & "')"
                cmd.ExecuteNonQuery()


            Next i

            movelast_record()


            xlWorkBook.Close(False, FileName)
            xlApp.Quit()


            ReleaseComObject(xlWorkSheet)
            ReleaseComObject(xlWorkBook)
            ReleaseComObject(xlApp)

            Sts = True

            ' MessageBox.Show("Imported Sucessfully!!!", "FOR IMPORTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception

            Sts = False

            xlWorkBook.Close(False, FileName)
            xlApp.Quit()


            ReleaseComObject(xlWorkSheet)
            ReleaseComObject(xlWorkBook)
            ReleaseComObject(xlApp)

            'MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub

    Private Sub ReleaseComObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        End Try
    End Sub

    Private Sub txt_DiscountPercentage_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DiscountPercentage.KeyPress
        If Not ((Asc(e.KeyChar) >= 48 And Asc(e.KeyChar) <= 57) Or Asc(e.KeyChar) = 46 Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 13) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Rack_No_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Rack_No.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If
        End If
        If e.KeyCode = 38 Then txt_DiscountPercentage.Focus()
    End Sub

    Private Sub txt_Rack_No_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rack_No.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Name.Focus()
            End If
        End If
    End Sub

    Private Sub txt_SalesProfit_Retail_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_SalesProfit_Retail.TextChanged
        If Val(txt_CostRate_Incl_Tax.Text) <> 0 Then
            txt_SalesRate_Retail.Text = Format(Val(txt_CostRate_Incl_Tax.Text) + (Val(txt_CostRate_Incl_Tax.Text) * Val(txt_SalesProfit_Retail.Text) / 100), "##############0.00")
        Else
            txt_SalesRate_Retail.Text = ""
        End If
    End Sub

    Private Sub txt_SalesProfit_Wholesale_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_SalesProfit_Wholesale.TextChanged
        If Val(txt_CostRate_Incl_Tax.Text) <> 0 Then
            txt_SalesRate_Wholesale.Text = Format(Val(txt_CostRate_Incl_Tax.Text) + (Val(txt_CostRate_Incl_Tax.Text) * Val(txt_SalesProfit_Wholesale.Text) / 100), "##############0.00")
        Else
            txt_SalesRate_Wholesale.Text = ""
        End If
    End Sub

    Private Sub txt_CostRate_Incl_Tax_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CostRate_Incl_Tax.KeyUp
        txt_CostRate_Excl_Tax.Text = Format(Val(txt_CostRate_Incl_Tax.Text) * (100 / (100 + Val(txt_GSTTaxPerc.Text))), "##########0.00")
    End Sub

    Private Sub txt_CostRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_CostRate_Incl_Tax.TextChanged
        If Val(txt_CostRate_Incl_Tax.Text) <> 0 Then
            If Val(txt_SalesProfit_Retail.Text) <> 0 Then
                txt_SalesRate_Retail.Text = Format(Val(txt_CostRate_Incl_Tax.Text) + (Val(txt_CostRate_Incl_Tax.Text) * Val(txt_SalesProfit_Retail.Text) / 100), "##############0.00")
            End If
            If Val(txt_SalesProfit_Wholesale.Text) <> 0 Then
                txt_SalesRate_Wholesale.Text = Format(Val(txt_CostRate_Incl_Tax.Text) + (Val(txt_CostRate_Incl_Tax.Text) * Val(txt_SalesProfit_Wholesale.Text) / 100), "##############0.00")
            End If

        Else
            txt_SalesRate_Retail.Text = ""
            txt_SalesRate_Wholesale.Text = ""
        End If
    End Sub

    Private Sub btn_SaveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SaveAll.Click
        Dim pwd As String = ""

        Dim g As New Password
        g.ShowDialog()

        pwd = Trim(Common_Procedures.Password_Input)

        If Trim(UCase(pwd)) <> "TSSA7417" Then
            MessageBox.Show("Select Password", "FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        SaveAll_STS = True

        LastNo = ""
        movelast_record()

        LastNo = lbl_IdNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Val(LastNo) = Val(lbl_IdNo.Text) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All Items saved sucessfully", "FOR SAVING ALL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub

    Private Sub cbo_Style_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Style.GotFocus
        cbo_Style.BackColor = Color.Lime
        cbo_Style.ForeColor = Color.Blue
        cbo_Style.SelectionStart = 0
        cbo_Style.SelectionLength = cbo_Style.Text.Length
    End Sub

    Private Sub cbo_Style_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Style.KeyDown
        If e.KeyValue = 38 And cbo_Style.DroppedDown = False Then
            e.Handled = True
            txt_Name.Focus()
            'SendKeys.Send("+{TAB}")
        ElseIf e.KeyValue = 40 And cbo_Style.DroppedDown = False Then
            e.Handled = True
            cbo_Size.Focus()
            'SendKeys.Send("{TAB}")
        ElseIf e.KeyValue <> 13 And cbo_Style.DroppedDown = False Then
            cbo_Style.DroppedDown = True
        End If
    End Sub

    Private Sub cbo_Style_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Style.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Style, Nothing, "Style_Head", "Style_Name", "", "(style_IdNo = 0)")
        Dim FindStr As String = ""
        Dim Indx As Integer = -1

        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If

        If Asc(e.KeyChar) = 8 Then
            If cbo_Style.SelectionStart <= 1 Then
                cbo_Style.Text = ""
                Exit Sub
            End If

            If cbo_Style.SelectionLength = 0 Then
                FindStr = cbo_Style.Text.Substring(0, cbo_Style.Text.Length - 1)
            Else
                FindStr = cbo_Style.Text.Substring(0, cbo_Style.SelectionStart - 1)
            End If

        Else
            If cbo_Style.SelectionLength = 0 Then
                FindStr = cbo_Style.Text & e.KeyChar
            Else
                FindStr = cbo_Style.Text.Substring(0, cbo_Style.SelectionStart) & e.KeyChar
            End If

        End If

        Indx = cbo_Style.FindString(FindStr)

        If Indx <> -1 Then
            cbo_Style.SelectedText = ""
            cbo_Style.SelectedIndex = Indx
            cbo_Style.SelectionStart = FindStr.Length
            cbo_Style.SelectionLength = cbo_Style.Text.Length
        End If
        e.Handled = True


    End Sub

    Private Sub cbo_Style_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Style.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Style_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Style.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Size_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Size.GotFocus
        cbo_Size.BackColor = Color.Lime
        cbo_Size.ForeColor = Color.Blue
        cbo_Size.SelectionStart = 0
        cbo_Size.SelectionLength = cbo_Size.Text.Length
    End Sub


    Private Sub cbo_Size_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Size.KeyDown
        If e.KeyValue = 38 And cbo_Size.DroppedDown = False Then
            e.Handled = True
            cbo_Style.Focus()
            'SendKeys.Send("+{TAB}")
        ElseIf e.KeyValue = 40 And cbo_Size.DroppedDown = False Then
            If txt_Code.Visible = True Then
                e.Handled = True
                txt_Code.Focus()
            Else
                e.Handled = True
                cbo_ItemGroup.Focus()
            End If
            'SendKeys.Send("{TAB}")
        ElseIf e.KeyValue <> 13 And cbo_Size.DroppedDown = False Then
            cbo_Size.DroppedDown = True
        End If
    End Sub

    Private Sub cbo_Size_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Size.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Size, Nothing, "Size_Head", "Size_Name", "", "(Size_IdNo = 0)")
        Dim FindStr As String = ""
        Dim Indx As Integer = -1

        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If

        If Asc(e.KeyChar) = 8 Then
            If cbo_Size.SelectionStart <= 1 Then
                cbo_Size.Text = ""
                Exit Sub
            End If

            If cbo_Size.SelectionLength = 0 Then
                FindStr = cbo_Size.Text.Substring(0, cbo_Size.Text.Length - 1)
            Else
                FindStr = cbo_Size.Text.Substring(0, cbo_Size.SelectionStart - 1)
            End If

        Else
            If cbo_Size.SelectionLength = 0 Then
                FindStr = cbo_Size.Text & e.KeyChar
            Else
                FindStr = cbo_Size.Text.Substring(0, cbo_Size.SelectionStart) & e.KeyChar
            End If

        End If

        Indx = cbo_Size.FindString(FindStr)

        If Indx <> -1 Then
            cbo_Size.SelectedText = ""
            cbo_Size.SelectedIndex = Indx
            cbo_Size.SelectionStart = FindStr.Length
            cbo_Size.SelectionLength = cbo_Size.Text.Length
        End If
        e.Handled = True
    End Sub


    Private Sub cbo_Size_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Size.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Size_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Size.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub



    Private Sub getExcelData_ItemGroup_1167(ByVal FileName As String, ByRef Sts As Boolean)
        Dim cmd As New SqlClient.SqlCommand
        'Dim tr As SqlClient.SqlTransaction
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable

        Dim xlApp As Excel.Application
        Dim xlWorkBook As Excel.Workbook
        Dim xlWorkSheet As Excel.Worksheet

        Dim RowCnt As Long = 0
        ' Dim FileName As String = ""
        Dim NewCode As String = ""
        Dim ShfId As Integer = 0
        Dim ItmId As Integer = 0
        Dim n As Integer = 0
        Dim Sn As Integer = 0
        Dim itemGRP_Id As Integer = 0
        Dim Sur As String = ""
        Dim Cat_Id As Integer = 0
        Dim mxId As Integer = 0
        Dim itemNM As String = ""
        Dim itemHSN As String = ""


        Try
            xlApp = New Excel.Application
            xlWorkBook = xlApp.Workbooks.Open(FileName)
            xlWorkSheet = xlWorkBook.Worksheets("sheet1")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub

        End Try

        Try

            With xlWorkSheet
                RowCnt = .Range("A" & .Rows.Count).End(Excel.XlDirection.xlUp).Row
            End With

            'RowCnt = xlWorkSheet.UsedRange.Rows.Count

            If RowCnt <= 1 Then
                MessageBox.Show("Data not found", "Data not found...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If


            For i = 2 To RowCnt

                itemNM = Trim(xlWorkSheet.Cells(i, 5).value)
                itemNM = Replace(itemNM, "'", "`")

                itemGRP_Id = Val(Common_Procedures.ItemGroup_NameToIdNo(con, Trim(itemNM)))

                If itemGRP_Id <> 0 Then Continue For

                Cat_Id = 0


                Sur = Common_Procedures.Remove_NonCharacters(Trim(itemNM))
                Cat_Id = Val(Common_Procedures.get_FieldValue(con, "ItemGroup_Head", "itemgroup_idno", "(Sur_Name = '" & Trim(Sur) & "')"))
                If Cat_Id <> 0 Then Continue For


                mxId = Common_Procedures.get_MaxIdNo(con, "ItemGroup_Head", "itemgroup_idno", "")

                itemHSN = Trim(xlWorkSheet.Cells(i, 9).value)
                If Trim(itemHSN) = "" Then itemHSN = "--"

                cmd.Connection = con

                cmd.CommandText = "Insert into ItemGroup_Head(  itemgroup_idno     ,         itemgroup_name ,       Item_HSN_Code    ,       sur_name      ,    Cetegory_IdNo         ,                        Item_GST_Percentage     ) " & _
                                                    "values (" & Str(Val(mxId)) & ", '" & Trim(itemNM) & "' , '" & Trim(itemHSN) & "', '" & Trim(Sur) & "' , " & Str(Val(Cat_Id)) & " ," & Str(Val(xlWorkSheet.Cells(i, 8).value)) & " ) "
                cmd.ExecuteNonQuery()

            Next i

            movelast_record()


            xlWorkBook.Close(False, FileName)
            xlApp.Quit()


            ReleaseComObject(xlWorkSheet)
            ReleaseComObject(xlWorkBook)
            ReleaseComObject(xlApp)

            Sts = True

            '  MessageBox.Show("Imported Sucessfully!!!", "FOR IMPORTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception

            Sts = False

            xlWorkBook.Close(False, FileName)
            xlApp.Quit()


            ReleaseComObject(xlWorkSheet)
            ReleaseComObject(xlWorkBook)
            ReleaseComObject(xlApp)


            'MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub



    Private Sub getExcelData_Style_1167(ByVal FileName As String, ByRef Sts As Boolean)
        Dim cmd As New SqlClient.SqlCommand
        'Dim tr As SqlClient.SqlTransaction
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable

        Dim xlApp As Excel.Application
        Dim xlWorkBook As Excel.Workbook
        Dim xlWorkSheet As Excel.Worksheet

        Dim RowCnt As Long = 0
        '  Dim FileName As String = ""
        Dim NewCode As String = ""
        Dim ShfId As Integer = 0
        Dim ItmId As Integer = 0
        Dim n As Integer = 0
        Dim Sn As Integer = 0
        Dim Cat_Id As Integer = 0
        Dim Sur As String = ""
        Dim MxId As Integer = 0
        Dim itemNM As String = ""

        Try
            xlApp = New Excel.Application
            xlWorkBook = xlApp.Workbooks.Open(FileName)
            xlWorkSheet = xlWorkBook.Worksheets("sheet1")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub

        End Try

        Try
            'OpenFileDialog1.ShowDialog()
            'FileName = OpenFileDialog1.FileName


            With xlWorkSheet
                RowCnt = .Range("A" & .Rows.Count).End(Excel.XlDirection.xlUp).Row
            End With

            'RowCnt = xlWorkSheet.UsedRange.Rows.Count

            If RowCnt <= 1 Then
                MessageBox.Show("Data not found", "Data not found...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If


            For i = 2 To RowCnt

                itemNM = Trim(xlWorkSheet.Cells(i, 2).value)
                itemNM = Replace(itemNM, "'", "`")


                Cat_Id = Val(Common_Procedures.Style_NameToIdNo(con, Trim(itemNM)))
                If Cat_Id <> 0 Then Continue For

                Sur = Common_Procedures.Remove_NonCharacters(Trim(itemNM))

                Cat_Id = Val(Common_Procedures.get_FieldValue(con, "Style_Head", "Style_IdNo", "(Sur_Name = '" & Trim(Sur) & "')"))
                If Cat_Id <> 0 Then Continue For

                MxId = Common_Procedures.get_MaxIdNo(con, "Style_Head", "Style_IdNo", "")

                cmd.Connection = con

                cmd.CommandText = "Insert into Style_Head(Style_IdNo, Style_Name, Sur_Name) Values (" & Str(Val(MxId)) & ", '" & Trim(itemNM) & "', '" & Trim(Sur) & "')"
                cmd.ExecuteNonQuery()

            Next i

            movelast_record()


            xlWorkBook.Close(False, FileName)
            xlApp.Quit()


            ReleaseComObject(xlWorkSheet)
            ReleaseComObject(xlWorkBook)
            ReleaseComObject(xlApp)

            Sts = True

            ' MessageBox.Show("Imported Sucessfully!!!", "FOR IMPORTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception

            Sts = False

            xlWorkBook.Close(False, FileName)
            xlApp.Quit()


            ReleaseComObject(xlWorkSheet)
            ReleaseComObject(xlWorkBook)
            ReleaseComObject(xlApp)

            'MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub

    Private Sub getExcelData_Size_1167(ByVal FileName As String, ByRef Sts As Boolean)
        Dim cmd As New SqlClient.SqlCommand
        'Dim tr As SqlClient.SqlTransaction
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable

        Dim xlApp As Excel.Application
        Dim xlWorkBook As Excel.Workbook
        Dim xlWorkSheet As Excel.Worksheet

        Dim RowCnt As Long = 0
        '  Dim FileName As String = ""
        Dim NewCode As String = ""
        Dim ShfId As Integer = 0
        Dim ItmId As Integer = 0
        Dim n As Integer = 0
        Dim Sn As Integer = 0
        Dim Cat_Id As Integer = 0
        Dim Sur As String = ""
        Dim MxId As Integer = 0
        Dim itemNM As String = ""


        Try
            xlApp = New Excel.Application
            xlWorkBook = xlApp.Workbooks.Open(FileName)
            xlWorkSheet = xlWorkBook.Worksheets("sheet1")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub

        End Try

        Try
            'OpenFileDialog1.ShowDialog()
            'FileName = OpenFileDialog1.FileName


            With xlWorkSheet
                RowCnt = .Range("A" & .Rows.Count).End(Excel.XlDirection.xlUp).Row
            End With

            'RowCnt = xlWorkSheet.UsedRange.Rows.Count

            If RowCnt <= 1 Then
                MessageBox.Show("Data not found", "Data not found...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If


            For i = 2 To RowCnt


                itemNM = Trim(xlWorkSheet.Cells(i, 3).value)
                itemNM = Replace(itemNM, "'", "`")

                Cat_Id = Val(Common_Procedures.Size_NameToIdNo(con, Trim(itemNM)))
                If Cat_Id <> 0 Then Continue For

                Sur = Common_Procedures.Remove_NonCharacters(Trim(itemNM))

                Cat_Id = Val(Common_Procedures.get_FieldValue(con, "Size_Head", "Size_IdNo", "(Sur_Name = '" & Trim(Sur) & "')"))
                If Cat_Id <> 0 Then Continue For

                MxId = Common_Procedures.get_MaxIdNo(con, "Size_Head", "Size_IdNo", "")

                cmd.Connection = con

                cmd.CommandText = "Insert into Size_Head(Size_IdNo, Size_Name, Sur_Name) Values (" & Str(Val(MxId)) & ", '" & Trim(itemNM) & "', '" & Trim(Sur) & "')"
                cmd.ExecuteNonQuery()

            Next i

            movelast_record()

            xlWorkBook.Close(False, FileName)
            xlApp.Quit()

            ReleaseComObject(xlWorkSheet)
            ReleaseComObject(xlWorkBook)
            ReleaseComObject(xlApp)

            Sts = True

            ' MessageBox.Show("Imported Sucessfully!!!", "FOR IMPORTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception

            Sts = False

            xlWorkBook.Close(False, FileName)
            xlApp.Quit()


            ReleaseComObject(xlWorkSheet)
            ReleaseComObject(xlWorkBook)
            ReleaseComObject(xlApp)

            'MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub

    Private Sub getExcelData_ItemName_1167(ByVal FileName As String, ByRef Sts As Boolean)
        Dim cmd As New SqlClient.SqlCommand
        'Dim tr As SqlClient.SqlTransaction
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable

        Dim xlApp As Excel.Application
        Dim xlWorkBook As Excel.Workbook
        Dim xlWorkSheet As Excel.Worksheet

        Dim MxId As Long = 0
        Dim RowCnt As Long = 0
        '  Dim FileName As String = ""
        Dim NewCode As String = ""
        Dim ShfId As Integer = 0
        Dim ItmId As Integer = 0
        Dim n As Integer = 0
        Dim Sn As Integer = 0
        Dim itemGRP_Id As Integer = 0
        Dim itemId As Integer = 0
        Dim Sur As String = ""
        Dim Cat_Id As Integer = 0
        Dim itemDispNM As String = ""
        Dim itemNM As String = ""
        Dim itemGrpNM As String = ""
        Dim itemStylNM As String = ""
        Dim itemSizNM As String = ""
        Dim itemStyl_ID As Integer = 0
        Dim itemSiz_ID As Integer = 0



        Try
            xlApp = New Excel.Application
            xlWorkBook = xlApp.Workbooks.Open(FileName)
            xlWorkSheet = xlWorkBook.Worksheets("sheet1")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub

        End Try

        'Try

        With xlWorkSheet
            RowCnt = .Range("A" & .Rows.Count).End(Excel.XlDirection.xlUp).Row
        End With

        'RowCnt = xlWorkSheet.UsedRange.Rows.Count

        If RowCnt <= 1 Then
            MessageBox.Show("Data not found", "Data not found...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If


        For i = 2 To RowCnt

            itemDispNM = Trim(xlWorkSheet.Cells(i, 1).value)
            itemDispNM = Replace(itemDispNM, "'", "`")

            If Trim(itemDispNM) = "" Then Continue For

            itemId = Val(Common_Procedures.Item_NameToIdNo(con, Trim(itemDispNM)))

            If itemId <> 0 Then
                Continue For
            End If

            Sur = Common_Procedures.Remove_NonCharacters(Trim(itemDispNM))

            itemId = Val(Common_Procedures.get_FieldValue(con, "item_head", "Item_IdNo", "(Sur_Name = '" & Trim(itemDispNM) & "')"))
            If itemId <> 0 Then
                Continue For
            End If



            itemStylNM = Trim(xlWorkSheet.Cells(i, 2).value)
            itemStylNM = Replace(itemStylNM, "'", "`")
            itemStyl_ID = Val(Common_Procedures.get_FieldValue(con, "Style_Head", "Style_IdNo", "(Style_Name = '" & Trim(itemStylNM) & "')"))

            itemSizNM = Trim(xlWorkSheet.Cells(i, 3).value)
            itemSizNM = Replace(itemSizNM, "'", "`")
            itemSiz_ID = Val(Common_Procedures.get_FieldValue(con, "Size_Head", "Size_IdNo", "(Size_Name = '" & Trim(itemSizNM) & "')"))

            itemGrpNM = Trim(xlWorkSheet.Cells(i, 5).value)
            itemGrpNM = Replace(itemGrpNM, "'", "`")
            itemGRP_Id = Val(Common_Procedures.get_FieldValue(con, "ItemGroup_Head", "itemgroup_idno", "(itemgroup_name = '" & Trim(itemGrpNM) & "')"))




            itemNM = Trim(itemDispNM)
            If Trim(itemStylNM) <> "" Then
                itemNM = Trim(itemNM) & " - " & Trim(itemStylNM)
            End If
            If Trim(itemSizNM) <> "" Then
                itemNM = Trim(itemNM) & " - " & Trim(itemSizNM)
            End If

            itemId = Val(Common_Procedures.Item_NameToIdNo(con, Trim(itemDispNM)))

            If itemId <> 0 Then
                Continue For
            End If

            Sur = Common_Procedures.Remove_NonCharacters(Trim(itemNM))

            itemId = Val(Common_Procedures.get_FieldValue(con, "item_head", "Item_IdNo", "(Sur_Name = '" & Trim(Sur) & "')"))
            If itemId <> 0 Then
                Continue For
            End If


            Cat_Id = 0

            MxId = Common_Procedures.get_MaxIdNo(con, "Item_Head", "Item_IdNo", "")

            cmd.Connection = con


            cmd.CommandText = "Insert into item_head ( Item_IdNo    ,    Item_Name          ,       Sur_Name     ,                        Item_Code                ,          ItemGroup_IdNo       , Unit_IdNo , Minimum_Stock  ,                           Sales_Rate              ,                          MRP_Rate                ,                          Gst_Percentage          ,      Item_DisplayName     ,    Item_Size_IdNo             ,          Item_Style_IdNo       ) " & _
                                    " Values (" & Str(Val(MxId)) & ", '" & Trim(itemNM) & "', '" & Trim(Sur) & "',  '" & Trim(xlWorkSheet.Cells(i, 4).value) & "'  , " & Str(Val(itemGRP_Id)) & "  ,   1       ,       0        ,   " & Str(Val(xlWorkSheet.Cells(i, 6).value)) & " ,  " & Str(Val(xlWorkSheet.Cells(i, 7).value)) & " ,  " & Str(Val(xlWorkSheet.Cells(i, 8).value)) & " , '" & Trim(itemDispNM) & "', " & Str(Val(itemSiz_ID)) & "  , " & Str(Val(itemStyl_ID)) & "  ) "
            cmd.ExecuteNonQuery()



        Next i

        movelast_record()


        xlWorkBook.Close(False, FileName)
        xlApp.Quit()

        ReleaseComObject(xlWorkSheet)
        ReleaseComObject(xlWorkBook)
        ReleaseComObject(xlApp)

        Sts = True

        'MessageBox.Show("Imported Sucessfully!!!", "FOR IMPORTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        'Catch ex As Exception

        '    Sts = False

        '    xlWorkBook.Close(False, FileName)
        '    xlApp.Quit()


        '    ReleaseComObject(xlWorkSheet)
        '    ReleaseComObject(xlWorkBook)
        '    ReleaseComObject(xlApp)

        '    ' MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

    End Sub

    Private Sub cbo_DealerName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DealerName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_Head", "Ledger_Name", "(Ledger_Type = 'DEALER')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_DealerName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DealerName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DealerName, Nothing, Nothing, "Ledger_Head", "Ledger_Name", "(Ledger_Type = 'DEALER')", "(Ledger_IdNo = 0)")
        If e.KeyCode = 38 And cbo_DealerName.DroppedDown = False Or (e.Control = True And e.KeyCode = 38) Then
            If cbo_ItemGroup.Visible = True Then
                cbo_ItemGroup.Focus()
            Else
                txt_Code.Focus()
            End If
        End If
        If e.KeyCode = 40 And cbo_DealerName.DroppedDown = False Or (e.Control = True And e.KeyCode = 40) Then
            If cbo_Unit.Visible = True Then
                cbo_Unit.Focus()
            Else
                txt_MinimumStock.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_DealerName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DealerName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DealerName, Nothing, "Ledger_Head", "Ledger_Name", "(Ledger_Type = 'DEALER')", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If cbo_Unit.Visible = True Then
                cbo_Unit.Focus()
            Else
                txt_MinimumStock.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_DealerName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DealerName.KeyUp
        If e.Control = False And e.KeyCode = 17 Then
            Common_Procedures.MDI_LedType = "DEALER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_DealerName.Name
            Common_Procedures.Master_Return.Master_Type = ""
            Common_Procedures.Master_Return.Return_Value = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub txt_Mrp_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Mrp.KeyDown
        If e.KeyCode = 38 Then e.Handled = True : txt_Sales_GSTRate.Focus()
        If e.KeyCode = 40 Then
            If txt_DiscountPercentage.Visible = True Then
                txt_DiscountPercentage.Focus()
            ElseIf txt_SalesProfit_Retail.Visible = True Then
                txt_SalesProfit_Retail.Focus()
            Else
                If MessageBox.Show("Do you want to Save?..", "FOR SAVE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    Exit Sub
                End If
            End If

        End If

    End Sub

    Private Sub txt_Mrp_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Mrp.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If txt_DiscountPercentage.Visible = True Then
                txt_DiscountPercentage.Focus()
            ElseIf txt_SalesProfit_Retail.Visible = True Then
                txt_SalesProfit_Retail.Focus()
            Else
                If MessageBox.Show("Do you want to Save?..", "FOR SAVE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Private Sub txt_SalesRate_Wholesale_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_SalesRate_Wholesale.KeyDown
        If e.KeyCode = 38 Then e.Handled = True : txt_Sales_GSTRate.Focus()
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to Save?..", "FOR SAVE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                Exit Sub
            End If

        End If
    End Sub

    Private Sub txt_SalesRate_Wholesale_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SalesRate_Wholesale.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to Save?..", "FOR SAVE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                Exit Sub
            End If
        End If
    End Sub

    Private Sub txt_CostRate_Excl_Tax_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_CostRate_Excl_Tax.KeyUp
        txt_CostRate_Incl_Tax.Text = Format(Val(txt_CostRate_Excl_Tax.Text) * ((100 + Val(txt_GSTTaxPerc.Text)) / 100), "##########0.00")
    End Sub

    Private Sub txt_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Name.KeyPress
        If Asc(e.KeyChar) = 39 Then
            e.Handled = True
        End If
    End Sub

    Private Sub getExcelData_ItemName_1365(ByVal FileName As String, ByRef Sts As Boolean)
        Dim cmd As New SqlClient.SqlCommand
        'Dim tr As SqlClient.SqlTransaction
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable

        Dim xlApp As Excel.Application
        Dim xlWorkBook As Excel.Workbook
        Dim xlWorkSheet As Excel.Worksheet

        Dim RowCnt As Long = 0
        '  Dim FileName As String = ""
        Dim NewCode As String = ""
        Dim ShfId As Integer = 0
        Dim ItmId As Integer = 0
        Dim n As Integer = 0
        Dim Sn As Integer = 0
        Dim itemGRP_Id As Integer = 0
        Dim itemId As Integer = 0
        Dim itemNM As String = ""
        Dim itemCD As String = ""
        Dim vUOM_ID As String = 0
        Dim Sur As String = ""
        Dim vTax_Perc As String = 0
        Dim vSale_ExRate As String = 0
        Dim vPurc_ExRate As String = 0
        Dim vSale_TaxRate As String = 0
        Dim vPurc_TaxRate As String = 0
        Dim vMRP As String = 0
        Dim vHSNCode As String = ""
        Dim ITMGRP_NM As String = ""
        Dim ITMGRP_SURNM As String = ""
        Dim vUOM_NM As String = ""
        Dim vUOM_SURNM As String = ""
        Dim mxId As Integer = 0

        cmd.Connection = con

        Try
            xlApp = New Excel.Application
            xlWorkBook = xlApp.Workbooks.Open(FileName)
            xlWorkSheet = xlWorkBook.Worksheets("sheet1")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub

        End Try

        Try

            With xlWorkSheet
                RowCnt = .Range("A" & .Rows.Count).End(Excel.XlDirection.xlUp).Row
            End With

            'RowCnt = xlWorkSheet.UsedRange.Rows.Count

            If RowCnt <= 1 Then
                MessageBox.Show("Data not found", "Data not found...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If



            For i = 2 To RowCnt

                itemNM = Trim(xlWorkSheet.Cells(i, 1).value)
                itemNM = Replace(itemNM, "'", "`")

                If Trim(itemNM) = "" Then Continue For

                itemId = Val(Common_Procedures.Item_NameToIdNo(con, Trim(itemNM)))

                If itemId <> 0 Then Continue For

                Sur = Common_Procedures.Remove_NonCharacters(Trim(itemNM))

                itemId = Val(Common_Procedures.get_FieldValue(con, "item_head", "Item_IdNo", "(Sur_Name = '" & Trim(Sur) & "')"))
                If itemId <> 0 Then
                    Continue For
                End If


                ITMGRP_NM = Trim(xlWorkSheet.Cells(i, 7).value)
                ITMGRP_NM = Replace(ITMGRP_NM, "'", "`")
                ITMGRP_SURNM = Common_Procedures.Remove_NonCharacters(Trim(ITMGRP_NM))
                itemGRP_Id = Val(Common_Procedures.get_FieldValue(con, "ItemGroup_Head", "itemgroup_idno", "(Sur_Name = '" & Trim(ITMGRP_SURNM) & "')"))
                'itemGRP_Id = Val(Common_Procedures.ItemGroup_NameToIdNo(con, Trim(xlWorkSheet.Cells(i, 7).value)))

                itemCD = ""


                vUOM_NM = Trim(xlWorkSheet.Cells(i, 9).value)
                vUOM_NM = Replace(vUOM_NM, "'", "`")
                vUOM_SURNM = Common_Procedures.Remove_NonCharacters(Trim(vUOM_NM))
                vUOM_ID = Val(Common_Procedures.get_FieldValue(con, "Unit_Head", "unit_idno", "(Sur_Name = '" & Trim(vUOM_SURNM) & "')"))
                'vUOM_ID = Val(Common_Procedures.Cetegory_NameToIdNo(con, Trim(xlWorkSheet.Cells(i, 9).value)))




                vTax_Perc = Val(Trim(xlWorkSheet.Cells(i, 10).value))
                vSale_ExRate = Val(Trim(xlWorkSheet.Cells(i, 11).value))
                vPurc_ExRate = Val(Trim(xlWorkSheet.Cells(i, 12).value))

                vSale_TaxRate = Format(Val(vSale_ExRate) * ((100 + Val(vTax_Perc)) / 100), "##########0.00")
                vPurc_TaxRate = Format(Val(vPurc_ExRate) * ((100 + Val(vTax_Perc)) / 100), "##########0.00")

                vMRP = Val(Trim(xlWorkSheet.Cells(i, 13).value))

                vHSNCode = Trim(xlWorkSheet.Cells(i, 14).value)


                mxId = Common_Procedures.get_MaxIdNo(con, "item_head", "Item_IdNo", "")

                lbl_IdNo.Text = "ITEM  : " & mxId
                Me.Text = lbl_IdNo.Text

                cmd.CommandText = "Insert into item_head (        Item_IdNo     ,       Item_Name         ,         Sur_Name    ,           Item_Code    ,           ItemGroup_IdNo     ,         Unit_IdNo         , Minimum_Stock   ,         Tax_Percentage     ,                 Sale_TaxRate   ,                 Sales_Rate    ,                  Cost_Rate      , Item_Name_Tamil  ,   MRP_Rate            ,  Job_Work_Status ,    Gst_Percentage      ,       Gst_Rate            ,  Item_Description ,     Item_DisplayName   ,       CostRate_Excl_Tax        ,  Close_Status  ,           HSN_Code        ) " & _
                                    "           values   (" & Str(Val(mxId)) & ", '" & Trim(itemNM) & "' ,  '" & Trim(Sur) & "' , '" & Trim(itemCD) & "' , " & Str(Val(itemGRP_Id)) & " , " & Str(Val(vUOM_ID)) & " ,        0        , " & Str(Val(vTax_Perc)) & ", " & Str(Val(vSale_TaxRate)) & ", " & Str(Val(vSale_ExRate)) & ", " & Str(Val(vPurc_TaxRate)) & " ,      ''          , " & Str(Val(vMRP)) & ",        0         , " & Val(vTax_Perc) & " , " & Val(vSale_TaxRate) & ",      ''           , '" & Trim(itemNM) & "' , " & Str(Val(vPurc_ExRate)) & " ,        0       ,  '" & Trim(vHSNCode) & "' )"
                cmd.ExecuteNonQuery()


            Next i

            new_record()

            xlWorkBook.Close(False, FileName)
            xlApp.Quit()


            ReleaseComObject(xlWorkSheet)
            ReleaseComObject(xlWorkBook)
            ReleaseComObject(xlApp)

            Sts = True
            'MessageBox.Show("Imported Sucessfully!!!", "FOR IMPORTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception

            Sts = False

            xlWorkBook.Close(False, FileName)
            xlApp.Quit()

            ReleaseComObject(xlWorkSheet)
            ReleaseComObject(xlWorkBook)
            ReleaseComObject(xlApp)

            ' MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub getExcelData_ItemGroup_1365(ByVal FileName As String, ByRef Sts As Boolean)
        Dim cmd As New SqlClient.SqlCommand
        'Dim tr As SqlClient.SqlTransaction
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable

        Dim xlApp As Excel.Application
        Dim xlWorkBook As Excel.Workbook
        Dim xlWorkSheet As Excel.Worksheet

        Dim RowCnt As Long = 0
        ' Dim FileName As String = ""
        Dim NewCode As String = ""
        Dim ShfId As Integer = 0
        Dim ItmId As Integer = 0
        Dim n As Integer = 0
        Dim Sn As Integer = 0
        Dim itemGRP_NM As String = ""
        Dim itemGRP_Id As Integer = 0
        Dim Sur As String = ""
        Dim Cat_Id As Integer = 0
        Dim mxId As Integer = 0
        Dim Cat_NM As String = ""
        Dim Cat_SURNM As String = ""
        Dim vTax_Perc As String = 0
        Dim vHSNCode As String = ""


        Try
            xlApp = New Excel.Application
            xlWorkBook = xlApp.Workbooks.Open(FileName)
            xlWorkSheet = xlWorkBook.Worksheets("sheet1")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub

        End Try

        Try

            With xlWorkSheet
                RowCnt = .Range("A" & .Rows.Count).End(Excel.XlDirection.xlUp).Row
            End With

            'RowCnt = xlWorkSheet.UsedRange.Rows.Count

            If RowCnt <= 1 Then
                MessageBox.Show("Data not found", "Data not found...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If


            For i = 2 To RowCnt

                itemGRP_NM = Trim(xlWorkSheet.Cells(i, 7).value)
                itemGRP_NM = Replace(itemGRP_NM, "'", "`")

                If Trim(itemGRP_NM) = "" Then Continue For

                itemGRP_Id = Val(Common_Procedures.ItemGroup_NameToIdNo(con, Trim(itemGRP_NM)))

                If itemGRP_Id <> 0 Then Continue For

                Sur = Common_Procedures.Remove_NonCharacters(Trim(itemGRP_NM))

                itemGRP_Id = Val(Common_Procedures.get_FieldValue(con, "ItemGroup_Head", "itemgroup_idno", "(Sur_Name = '" & Trim(Sur) & "')"))

                If itemGRP_Id <> 0 Then
                    Continue For
                End If

                Cat_NM = Trim(xlWorkSheet.Cells(i, 3).value)
                Cat_NM = Replace(Cat_NM, "'", "`")
                Cat_SURNM = Common_Procedures.Remove_NonCharacters(Trim(Cat_NM))
                Cat_Id = Val(Common_Procedures.get_FieldValue(con, "Cetegory_Head", "Cetegory_IdNo", "(Sur_Name = '" & Trim(Cat_SURNM) & "')"))


                vTax_Perc = Val(Trim(xlWorkSheet.Cells(i, 10).value))

                vHSNCode = Trim(xlWorkSheet.Cells(i, 14).value)

                mxId = Common_Procedures.get_MaxIdNo(con, "ItemGroup_Head", "itemgroup_idno", "")

                lbl_IdNo.Text = "ITEMGROUP  : " & mxId
                Me.Text = lbl_IdNo.Text

                cmd.Connection = con

                cmd.CommandText = "Insert into ItemGroup_Head (  itemgroup_idno      ,         itemgroup_name    ,        Item_HSN_Code    ,       sur_name     ,    Cetegory_IdNo         ,       Item_GST_Percentage   ) " & _
                                                    "values   (" & Str(Val(mxId)) & ", '" & Trim(itemGRP_NM) & "', '" & Trim(vHSNCode) & "', '" & Trim(Sur) & "', " & Str(Val(Cat_Id)) & " , " & Str(Val(vTax_Perc)) & " ) "
                cmd.ExecuteNonQuery()

            Next i



            xlWorkBook.Close(False, FileName)
            xlApp.Quit()


            ReleaseComObject(xlWorkSheet)
            ReleaseComObject(xlWorkBook)
            ReleaseComObject(xlApp)

            Sts = True

            '  MessageBox.Show("Imported Sucessfully!!!", "FOR IMPORTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception

            Sts = False

            xlWorkBook.Close(False, FileName)
            xlApp.Quit()


            ReleaseComObject(xlWorkSheet)
            ReleaseComObject(xlWorkBook)
            ReleaseComObject(xlApp)


            'MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub

    Private Sub getExcelData_Category_1365(ByVal FileName As String, ByRef Sts As Boolean)
        Dim cmd As New SqlClient.SqlCommand
        'Dim tr As SqlClient.SqlTransaction
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable

        Dim xlApp As Excel.Application
        Dim xlWorkBook As Excel.Workbook
        Dim xlWorkSheet As Excel.Worksheet

        Dim RowCnt As Long = 0
        '  Dim FileName As String = ""
        Dim NewCode As String = ""
        Dim ShfId As Integer = 0
        Dim ItmId As Integer = 0
        Dim n As Integer = 0
        Dim Sn As Integer = 0
        Dim Cat_NM As String = ""
        Dim Cat_Id As Integer = 0
        Dim Sur As String = ""
        Dim MxId As Integer = 0

        Try
            xlApp = New Excel.Application
            xlWorkBook = xlApp.Workbooks.Open(FileName)
            xlWorkSheet = xlWorkBook.Worksheets("sheet1")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub

        End Try

        'Try
        'OpenFileDialog1.ShowDialog()
        'FileName = OpenFileDialog1.FileName


        With xlWorkSheet
            RowCnt = .Range("A" & .Rows.Count).End(Excel.XlDirection.xlUp).Row
        End With

        'RowCnt = xlWorkSheet.UsedRange.Rows.Count

        If RowCnt <= 1 Then
            MessageBox.Show("Data not found", "Data not found...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If


        For i = 2 To RowCnt


            Cat_NM = Trim(xlWorkSheet.Cells(i, 3).value)
            Cat_NM = Replace(Cat_NM, "'", "`")

            If Trim(Cat_NM) = "" Then Continue For

            Cat_Id = Val(Common_Procedures.Cetegory_NameToIdNo(con, Trim(Cat_NM)))

            If Cat_Id <> 0 Then Continue For

            Sur = Common_Procedures.Remove_NonCharacters(Trim(Cat_NM))

            Cat_Id = Val(Common_Procedures.get_FieldValue(con, "Cetegory_Head", "Cetegory_IdNo", "(Sur_Name = '" & Trim(Sur) & "')"))

            If Cat_Id <> 0 Then
                Continue For
            End If

            MxId = Common_Procedures.get_MaxIdNo(con, "Cetegory_Head", "Cetegory_IdNo", "")

            lbl_IdNo.Text = "CATEGORY  : " & MxId
            Me.Text = lbl_IdNo.Text

            cmd.Connection = con

            cmd.CommandText = "Insert into Cetegory_Head  (         Cetegory_IdNo ,         Cetegory_Name ,         Sur_Name    ) " & _
                                "           Values        ( " & Str(Val(MxId)) & ", '" & Trim(Cat_NM) & "', '" & Trim(Sur) & "' ) "
            cmd.ExecuteNonQuery()

        Next i


        xlWorkBook.Close(False, FileName)
        xlApp.Quit()


        ReleaseComObject(xlWorkSheet)
        ReleaseComObject(xlWorkBook)
        ReleaseComObject(xlApp)

        Sts = True

        ' MessageBox.Show("Imported Sucessfully!!!", "FOR IMPORTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        'Catch ex As Exception

        '    Sts = False

        '    xlWorkBook.Close(False, FileName)
        '    xlApp.Quit()


        '    ReleaseComObject(xlWorkSheet)
        '    ReleaseComObject(xlWorkBook)
        '    ReleaseComObject(xlApp)

        '    'MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try


    End Sub

    Private Sub getExcelData_UNITHEAD_1365(ByVal FileName As String, ByRef Sts As Boolean)
        Dim cmd As New SqlClient.SqlCommand
        'Dim tr As SqlClient.SqlTransaction
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable

        Dim xlApp As Excel.Application
        Dim xlWorkBook As Excel.Workbook
        Dim xlWorkSheet As Excel.Worksheet

        Dim RowCnt As Long = 0
        '  Dim FileName As String = ""
        Dim NewCode As String = ""
        Dim ShfId As Integer = 0
        Dim ItmId As Integer = 0
        Dim n As Integer = 0
        Dim Sn As Integer = 0
        Dim vUOM_NM As String = ""
        Dim vUOM_ID As Integer = 0
        Dim Sur As String = ""
        Dim MxId As Integer = 0

        Try
            xlApp = New Excel.Application
            xlWorkBook = xlApp.Workbooks.Open(FileName)
            xlWorkSheet = xlWorkBook.Worksheets("sheet1")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub

        End Try

        'Try
        'OpenFileDialog1.ShowDialog()
        'FileName = OpenFileDialog1.FileName


        With xlWorkSheet
            RowCnt = .Range("A" & .Rows.Count).End(Excel.XlDirection.xlUp).Row
        End With

        'RowCnt = xlWorkSheet.UsedRange.Rows.Count

        If RowCnt <= 1 Then
            MessageBox.Show("Data not found", "Data not found...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If


        For i = 2 To RowCnt

            vUOM_NM = Trim(xlWorkSheet.Cells(i, 9).value)
            vUOM_NM = Replace(vUOM_NM, "'", "`")

            If Trim(vUOM_NM) = "" Then Continue For

            vUOM_ID = Val(Common_Procedures.Unit_NameToIdNo(con, Trim(vUOM_NM)))

            If vUOM_ID <> 0 Then Continue For

            Sur = Common_Procedures.Remove_NonCharacters(Trim(vUOM_NM))

            vUOM_ID = Val(Common_Procedures.get_FieldValue(con, "Unit_Head", "Unit_IdNo", "(Sur_Name = '" & Trim(Sur) & "')"))

            If vUOM_ID <> 0 Then
                Continue For
            End If

            MxId = Common_Procedures.get_MaxIdNo(con, "Unit_Head", "Unit_IdNo", "")

            lbl_IdNo.Text = "UOM : " & MxId
            Me.Text = lbl_IdNo.Text

            cmd.Connection = con

            cmd.CommandText = "Insert into Unit_Head  (         Unit_IdNo     ,         Unit_Name      ,         Sur_Name    ) " & _
                                "           Values    ( " & Str(Val(MxId)) & ", '" & Trim(vUOM_NM) & "', '" & Trim(Sur) & "' ) "
            cmd.ExecuteNonQuery()

        Next i


        xlWorkBook.Close(False, FileName)
        xlApp.Quit()


        ReleaseComObject(xlWorkSheet)
        ReleaseComObject(xlWorkBook)
        ReleaseComObject(xlApp)

        Sts = True

        ' MessageBox.Show("Imported Sucessfully!!!", "FOR IMPORTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        'Catch ex As Exception

        '    Sts = False

        '    xlWorkBook.Close(False, FileName)
        '    xlApp.Quit()


        '    ReleaseComObject(xlWorkSheet)
        '    ReleaseComObject(xlWorkBook)
        '    ReleaseComObject(xlApp)

        '    'MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try


    End Sub

    Private Sub btn_Character_Click(sender As System.Object, e As System.EventArgs) Handles btn_Character.Click
        System.Diagnostics.Process.Start("C:\WINDOWS\system32\charmap.exe")
    End Sub
End Class