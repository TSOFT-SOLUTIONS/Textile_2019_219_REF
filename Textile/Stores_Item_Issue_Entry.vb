Imports System.Security.Cryptography.X509Certificates
Imports System.Net
Imports System.Net.Security
Imports System.Net.Mail

Public Class Stores_Item_Issue_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "ITISS-"
    Private cbo_KeyDwnVal As Double
    Private Prec_ActCtrl As New Control
    Private dgv_DrawNo As String = ""
    Private vCbo_ItmNm As String = ""
    Private vCbo_BrandNm As String = ""
    Private vCbo_Item_OLD_NEW_Nm As String = ""
    Private vCloPic_STS As Boolean = False

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer


    Private Sub clear()
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        New_Entry = False

        lbl_DcNo.Text = ""
        lbl_DcNo.ForeColor = Color.Black
        cbo_New_Old.Text = "NEW"
        cbo_Received.Text = ""
        cbo_Issued.Text = ""

        dgv_Details.Rows.Clear()

        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()



        cbo_Grid_Department.Text = False
        cbo_Grid_Item.Visible = False
        cbo_Grid_Brand.Text = False
        cbo_Grid_Machine.Visible = False
        cbo_Grid_Unit.Visible = False

        PictureBox1.Image = Nothing
        vCloPic_STS = False

        dgv_DrawNo = ""
        vCbo_ItmNm = ""
        vCbo_BrandNm = ""
        vCbo_Item_OLD_NEW_Nm = ""

        lbl_AvailableStock.Tag = 0
        lbl_AvailableStock.Text = ""


    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.PaleGreen
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()

        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then

            msktxbx = Me.ActiveControl
            msktxbx.SelectionStart = 0
        End If

        If Me.ActiveControl.Name <> cbo_Grid_Department.Name Then
            cbo_Grid_Department.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Item.Name Then
            cbo_Grid_Item.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Brand.Name Then
            cbo_Grid_Brand.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Machine.Name Then
            cbo_Grid_Machine.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Unit.Name Then
            cbo_Grid_Unit.Visible = False
        End If
        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Grid_DeSelect()
            pnl_Picture.Visible = False
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            End If
        End If

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
    End Sub

    Private Sub Item_Issue_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer = 0
        Dim CompCondt As String = ""

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Department.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "DEPARTMENT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Department.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Item.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ITEM" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Item.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Brand.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BRAND" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Brand.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Unit.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "UNIT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Unit.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Machine.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MACHINE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Machine.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

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

    Private Sub Item_Issue_Entry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable

        Me.Text = ""

        con.Open()

        cbo_New_Old.Items.Clear()
        cbo_New_Old.Items.Add("NEW")
        cbo_New_Old.Items.Add("OLD")

        da = New SqlClient.SqlDataAdapter("select Department_Name from Department_Head order by Department_Name", con)
        da.Fill(dt1)
        cbo_Grid_Department.DataSource = dt1
        cbo_Grid_Department.DisplayMember = "Department_Name"

        da = New SqlClient.SqlDataAdapter("select Item_DisplayName from Stores_Item_AlaisHead order by Item_DisplayName", con)
        da.Fill(dt2)
        cbo_Grid_Item.DataSource = dt2
        cbo_Grid_Item.DisplayMember = "Item_Name"

        da = New SqlClient.SqlDataAdapter("select Brand_Name from Brand_Head order by Brand_Name", con)
        da.Fill(dt3)
        cbo_Grid_Brand.DataSource = dt3
        cbo_Grid_Brand.DisplayMember = "Brand_Name"

        da = New SqlClient.SqlDataAdapter("select Unit_Name from Unit_Head order by Unit_Name", con)
        da.Fill(dt4)
        cbo_Grid_Unit.DataSource = dt4
        cbo_Grid_Unit.DisplayMember = "Unit_Name"

        da = New SqlClient.SqlDataAdapter("select Machine_Name from Machine_Head order by Machine_Name", con)
        da.Fill(dt5)
        cbo_Grid_Machine.DataSource = dt5
        cbo_Grid_Machine.DisplayMember = "Machine_Name"

        da = New SqlClient.SqlDataAdapter("select distinct(Issued_Name) from Stores_Item_Issue_Head order by Issued_Name", con)
        da.Fill(dt6)
        cbo_Issued.DataSource = dt6
        cbo_Issued.DisplayMember = "Issued_Name"

        da = New SqlClient.SqlDataAdapter("select distinct(Received_Name) from Stores_Item_Issue_Head order by Received_Name ", con)
        da.Fill(dt7)
        cbo_Received.DataSource = dt7
        cbo_Received.DisplayMember = "Received_Name"

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        pnl_Picture.Visible = False
        pnl_Picture.Left = (Me.Width - pnl_Picture.Width) - 25
        pnl_Picture.Top = (Me.Height - pnl_Picture.Height) - 50
        pnl_Picture.BringToFront()

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1391" Then  '--------Sakthidharan
            dgv_Details.Columns(6).ReadOnly = False
        Else
            dgv_Details.Columns(6).ReadOnly = True
        End If

        AddHandler dtp_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_New_Old.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Department.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Issued.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Received.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Item.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Brand.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Unit.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Machine.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Item.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Cancel.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_New_Old.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Department.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Issued.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Received.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Item.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Brand.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Unit.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Machine.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Item.LostFocus, AddressOf ControlLostFocus


        AddHandler cbo_Filter_MachineName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Department.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_MachineName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Department.LostFocus, AddressOf ControlLostFocus








        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Item_Issue_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Item_Issue_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                Else
                    Close_Form()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Close_Form()
        Try

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

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        On Error Resume Next

        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            ElseIf pnl_Back.Enabled = True Then
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = False Then

                With dgv1

                    If keyData = Keys.Enter Or keyData = Keys.Down Then


                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                    save_record()

                                Else
                                    dtp_date.Focus()
                                    Return True
                                    Exit Function

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                cbo_Issued.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If

                End With

            Else

                Return MyBase.ProcessCmdKey(msg, keyData)

            End If

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)


        Try


            da1 = New SqlClient.SqlDataAdapter("select a.* from Stores_Item_Issue_Head a  Where a.Issue_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_DcNo.Text = dt1.Rows(0).Item("Issue_No").ToString
                dtp_date.Text = dt1.Rows(0).Item("Issue_Date").ToString
                cbo_New_Old.Text = dt1.Rows(0).Item("New_Old").ToString
                vCbo_Item_OLD_NEW_Nm = Trim(cbo_New_Old.Text)
                cbo_Received.Text = dt1.Rows(0).Item("Received_name").ToString
                cbo_Issued.Text = dt1.Rows(0).Item("Issued_Name").ToString

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_name, b.Drawing_No, c.Department_name, d.Unit_name, e.Machine_name, f.Brand_Name from Stores_Item_Issue_Details a INNER JOIN Stores_Item_Head b ON a.Item_idno = b.Item_idno LEFT OUTER JOIN Department_Head c ON b.Department_idno = c.Department_idno LEFT OUTER JOIN Unit_Head d ON a.Unit_idno = d.Unit_idno LEFT OUTER JOIN Machine_Head e ON a.Machine_idno = e.Machine_idno LEFT OUTER JOIN Brand_Head f ON a.Brand_idno = f.Brand_idno    where a.Issue_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1

                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)

                        If IsDBNull(dt2.Rows(i).Item("Department_name").ToString) = False Then
                            If Trim(dt2.Rows(i).Item("Department_name").ToString) <> "" Then
                                dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Department_name").ToString
                            Else
                                dgv_Details.Rows(n).Cells(1).Value = Common_Procedures.Department_IdNoToName(con, 1)
                            End If
                        End If

                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Drawing_No").ToString
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Item_name").ToString
                        dgv_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Brand_name").ToString

                        dgv_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Quantity").ToString)
                        'dgv_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Quantity").ToString), "########0")



                        dgv_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Unit_name").ToString
                        dgv_Details.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Rate").ToString
                        dgv_Details.Rows(n).Cells(8).Value = dt2.Rows(i).Item("Amount").ToString
                        dgv_Details.Rows(n).Cells(9).Value = dt2.Rows(i).Item("Machine_name").ToString

                    Next i

                End If

                With dgv_Details_Total
                    .Rows.Clear()
                    .Rows.Add()

                    .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Quantity").ToString)
                    .Rows(0).Cells(8).Value = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "########0")

                End With

                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Dispose()
            da1.Dispose()

            Grid_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If dtp_date.Visible And dtp_date.Enabled Then dtp_date.Focus()

        End Try

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Store_Item_Issue_To_machine, New_Entry, Me, con, "Stores_Item_Issue_Head", "Issue_Code", NewCode, "Issue_Date", "(Issue_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_Stores_Item_Issue, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_Stores_Item_Issue, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other windows", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = con.BeginTransaction
        Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Stores_Item_Issue_Head", "Issue_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Issue_Code, Company_IdNo, for_OrderBy", trans)

        Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Stores_Item_Issue_Details", "Issue_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "Item_IdNo, Brand_IdNo, Quantity, Unit_idNo, Machine_idNo", "Sl_No", "Issue_Code, For_OrderBy, Company_IdNo, Issue_No, Issue_Date, Ledger_Idno", trans)

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "Delete from Stores_Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stores_Item_Issue_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Issue_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Stores_Item_Issue_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Issue_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            If dtp_date.Enabled = True And dtp_date.Visible = True Then dtp_date.Focus()

        End Try
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Item_DisplayName from Stores_Item_AlaisHead order by Item_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_Item.DataSource = dt1
            cbo_Filter_Item.DisplayMember = "Item_DisplayName"

            cbo_Filter_Item.Text = ""
            cbo_Filter_Item.SelectedIndex = -1

            da = New SqlClient.SqlDataAdapter("select Machine_Name  from machine_head order by Machine_Name", con)
            da.Fill(dt2)
            cbo_Filter_MachineName.DataSource = dt2
            cbo_Filter_MachineName.DisplayMember = "Machine_Name"

            cbo_Filter_MachineName.Text = ""
            cbo_Filter_MachineName.SelectedIndex = -1


            da = New SqlClient.SqlDataAdapter("select Department_Name from Department_Head order by Department_Name", con)
            da.Fill(dt3)
            cbo_Filter_Department.DataSource = dt3
            cbo_Filter_Department.DisplayMember = "Department_Name"

            cbo_Filter_Department.Text = ""
            cbo_Filter_Department.SelectedIndex = -1

            dt1.Clear()
            dt2.Clear()
            dt3.Clear()
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        Try
            If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Store_Item_Issue_To_machine, New_Entry, Me) = False Then Exit Sub

            ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_Stores_Item_Issue, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_Stores_Item_Issue, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

            inpno = InputBox("Enter New Dc.No.", "FOR NEW NO INSERTION...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Issue_No from Stores_Item_Issue_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Issue_Code = '" & Trim(RefCode) & "'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid DC No", "DOES NOT INSERT NEW NO...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_DcNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Issue_No from Stores_Item_Issue_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Issue_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Issue_No", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Clear()
            dt.Dispose()
            da.Dispose()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Issue_No from Stores_Item_Issue_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Issue_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Issue_No", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Issue_No from Stores_Item_Issue_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Issue_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Issue_No desc", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Issue_No from Stores_Item_Issue_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Issue_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Issue_No desc", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "Stores_Item_Issue_Head", "Issue_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_DcNo.ForeColor = Color.Red

            If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        dt.Dispose()
        da.Dispose()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        Try

            inpno = InputBox("Enter Dc.No.", "FOR FINDING...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Issue_No from Stores_Item_Issue_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Issue_Code = '" & Trim(RefCode) & "'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                MessageBox.Show("Dc No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Dep_ID As Integer = 0
        Dim Item_ID As Integer = 0
        Dim Machine_ID As Integer = 0
        Dim Unit_ID As Integer = 0
        Dim Brand_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim vTotqty As Single = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim Qty_New As Single = 0
        Dim Qty_Old_Usble As Single = 0
        Dim Qty_Old_Scrp As Single = 0
        Dim vOrdByNo As String = ""
        Dim vTotAmount As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text)

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Entry_Stores_Item_Issue, New_Entry) = False Then Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Store_Item_Issue_To_machine, New_Entry, Me, con, "Stores_Item_Issue_Head", "Issue_Code", NewCode, "Issue_Date", "(Issue_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Issue_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Issue_No desc", dtp_date.Value.Date) = False Then Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_date.Enabled Then dtp_date.Focus()
            Exit Sub
        End If

        If Not (dtp_date.Value.Date >= Common_Procedures.Company_FromDate And dtp_date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_date.Enabled Then dtp_date.Focus()
            Exit Sub
        End If

        For i = 0 To dgv_Details.RowCount - 1

            If Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Then

                Item_ID = Common_Procedures.itemalais_NameToIdNo(con, dgv_Details.Rows(i).Cells(3).Value)
                If Item_ID = 0 Then
                    MessageBox.Show("Invalid Item Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(3)
                        dgv_Details.CurrentCell.Selected = True
                    End If
                    Exit Sub
                End If

                Brand_ID = Common_Procedures.Brand_NameToIdNo(con, dgv_Details.Rows(i).Cells(4).Value)
                If Brand_ID = 0 Then
                    MessageBox.Show("Invalid Brand Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(4)
                        dgv_Details.CurrentCell.Selected = True
                    End If
                    Exit Sub
                End If

                If dgv_Details.Rows(i).Cells(5).Value() = 0 Then
                    MessageBox.Show("Invalid Issue Quantity", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(5)
                        dgv_Details.CurrentCell.Selected = True
                    End If
                    Exit Sub
                End If

                Unit_ID = Common_Procedures.Unit_NameToIdNo(con, dgv_Details.Rows(i).Cells(6).Value)
                If Unit_ID = 0 Then
                    MessageBox.Show("Invalid Unit Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(6)
                        dgv_Details.CurrentCell.Selected = True
                    End If
                    Exit Sub
                End If
            End If
        Next

        vTotqty = 0
        vTotAmount = 0


        If dgv_Details_Total.RowCount > 0 Then
            vTotqty = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
            vTotAmount = Val(dgv_Details_Total.Rows(0).Cells(8).Value())
        End If

        If vTotqty = 0 Then
            MessageBox.Show("Invalid Issue Quantity", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dgv_Details.Enabled And dgv_Details.Visible Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(5)
            End If
            Exit Sub
        End If

        tr = con.BeginTransaction

        ' Try

        If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "Stores_Item_Issue_Head", "Issue_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@DcDate", dtp_date.Value.Date)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Stores_Item_Issue_Head(Issue_Code, Company_IdNo, Issue_No, for_OrderBy, Issue_Date, New_Old, Issued_Name, Received_Name, Total_Quantity,Total_Amount) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @DcDate, '" & Trim(cbo_New_Old.Text) & "', '" & Trim(cbo_Issued.Text) & "', '" & Trim(cbo_Received.Text) & "', " & Str(Val(vTotqty)) & " , " & Str(Val(vTotAmount)) & ")"
                cmd.ExecuteNonQuery()

            Else
                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Stores_Item_Issue_Head", "Issue_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Issue_Code, Company_IdNo, for_OrderBy", tr)

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Stores_Item_Issue_Details", "Issue_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Item_IdNo, Brand_IdNo, Quantity, Unit_idNo, Machine_idNo", "Sl_No", "Issue_Code, For_OrderBy, Company_IdNo, Issue_No, Issue_Date, Ledger_Idno", tr)


                cmd.CommandText = "Update Stores_Item_Issue_Head set Issued_Name = '" & Trim(cbo_Issued.Text) & "', Issue_Date= @DcDate, New_Old = '" & Trim(cbo_New_Old.Text) & "', Received_Name = '" & Trim(cbo_Received.Text) & "', Total_Quantity = " & Str(Val(vTotqty)) & ",Total_Amount= " & Str(Val(vTotAmount)) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Issue_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Stores_Item_Issue_Head", "Issue_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Issue_Code, Company_IdNo, for_OrderBy", tr)

           
            EntID = Trim(Pk_Condition) & Trim(lbl_DcNo.Text)
            PBlNo = Trim(lbl_DcNo.Text)
            Partcls = "Issue : Dc.No. " & Trim(lbl_DcNo.Text)

            cmd.CommandText = "Delete from Stores_Item_Issue_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Issue_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stores_Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details
                Sno = 0
                For i = 0 To dgv_Details.RowCount - 1

                    If Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Then

                        Sno = Sno + 1

                        Item_ID = Common_Procedures.itemalais_NameToIdNo(con, dgv_Details.Rows(i).Cells(3).Value, tr)

                        Brand_ID = Common_Procedures.Brand_NameToIdNo(con, dgv_Details.Rows(i).Cells(4).Value, tr)

                        Unit_ID = Common_Procedures.Unit_NameToIdNo(con, dgv_Details.Rows(i).Cells(6).Value, tr)

                        Machine_ID = Common_Procedures.Machine_NameToIdNo(con, dgv_Details.Rows(i).Cells(9).Value, tr)

                        cmd.CommandText = "Insert into Stores_Item_Issue_Details ( Issue_Code, Company_IdNo, Issue_No, for_OrderBy, Issue_Date, Sl_No, Item_IdNo, Brand_IdNo, Quantity, Unit_idNo, Machine_idNo,Rate,Amount) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @DcDate, " & Str(Val(Sno)) & ", " & Str(Val(Item_ID)) & ", " & Str(Val(Brand_ID)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Val(Unit_ID) & ", " & Val(Machine_ID) & " ," & Str(Val(.Rows(i).Cells(7).Value)) & "," & Str(Val(.Rows(i).Cells(8).Value)) & ")"
                        cmd.ExecuteNonQuery()

                        Qty_New = 0
                        Qty_Old_Usble = 0
                        Qty_Old_Scrp = 0

                        If Trim(UCase(cbo_New_Old.Text)) = "OLD" Then
                            Qty_Old_Usble = Val(.Rows(i).Cells(5).Value)

                        Else
                            Qty_New = Val(.Rows(i).Cells(5).Value)

                        End If

                        cmd.CommandText = "Insert into Stores_Stock_Item_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, Ledger_IdNo, Entry_ID, Party_Bill_No, Particulars, Sl_No, Item_IdNo, Unit_IdNo, Brand_IdNo, Quantity_New, Quantity_Old_Usable, Quantity_Old_Scrap) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @DcDate, 0, '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Item_ID)) & ", " & Str(Val(Unit_ID)) & ", " & Str(Val(Brand_ID)) & ", " & Str(-1 * Val(Qty_New)) & ", " & Str(-1 * Val(Qty_Old_Usble)) & ", " & Str(-1 * Val(Qty_Old_Scrp)) & " )"
                        cmd.ExecuteNonQuery()

                    End If

                Next
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Stores_Item_Issue_Details", "Issue_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Item_IdNo, Brand_IdNo, Quantity, Unit_idNo, Machine_idNo", "Sl_No", "Issue_Code, For_OrderBy, Company_IdNo, Issue_No, Issue_Date, Ledger_Idno", tr)

            End With


            tr.Commit()

            move_record(lbl_DcNo.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
            new_record()
        'Catch ex As Exception
        '    tr.Rollback()
        '    MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'Finally

        '    Dt1.Dispose()
        '    Da.Dispose()
        '    tr.Dispose()

        '    If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()

        'End Try

    End Sub

    Private Sub cbo_Grid_Department_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Department.KeyDown

        cbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Department, Nothing, Nothing, "Department_HEAD", "Department_name", "", "(Department_idno = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Department.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                e.Handled = True
                If .CurrentRow.Index <= 0 Then
                    cbo_Issued.Focus()

                Else
                    .CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index - 1).Cells(.ColumnCount - 1)

                End If

            End If


            If (e.KeyValue = 40 And cbo_Grid_Department.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                e.Handled = True
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_date.Focus()
                    End If

                Else
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(2)

                End If
            End If

        End With
    End Sub

    Private Sub cbo_Grid_Department_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Department.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Department, Nothing, "Department_Head", "Department_name", "", "(Department_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            e.Handled = True

            With dgv_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_date.Focus()
                    End If

                Else
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(2)

                End If
            End With

        End If
    End Sub

    Private Sub cbo_Received_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Received.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Received, cbo_New_Old, cbo_Issued, "Stores_Item_Issue_Head", "Received_name", "", "")
    End Sub

    Private Sub cbo_Received_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Received.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Received, cbo_Issued, "Stores_Item_Issue_Head", "Received_Name", "", "", False)
    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim item_nm As String
        Dim Unt_nm As String
        Dim dno As String
        Dim dep_idno As Integer = 0

        If e.ColumnIndex = 2 Then

            If Trim(dgv_Details.Rows(e.RowIndex).Cells(3).Value) = "" Or Trim(dgv_DrawNo) <> Trim(dgv_Details.Rows(e.RowIndex).Cells(2).Value) Then

                dep_idno = Common_Procedures.Department_NameToIdNo(con, dgv_Details.Rows(e.RowIndex).Cells(1).Value)
                dno = dgv_Details.Rows(e.RowIndex).Cells(2).Value

                Da = New SqlClient.SqlDataAdapter("select a.Item_name, b.unit_name from Stores_Item_Head a left outer join unit_head b on a.unit_idno = b.unit_idno where a.department_idno = " & Str(Val(dep_idno)) & " and a.drawing_no = '" & Trim(dno) & "'", con)
                Da.Fill(Dt)

                item_nm = ""
                Unt_nm = ""
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        item_nm = Trim(Dt.Rows(0).Item("Item_name").ToString)
                        Unt_nm = Trim(Dt.Rows(0).Item("unit_name").ToString)
                    End If
                End If

                Dt.Dispose()
                Da.Dispose()

                dgv_Details.Rows(e.RowIndex).Cells(3).Value = Trim(item_nm)
                dgv_Details.Rows(e.RowIndex).Cells(6).Value = Trim(Unt_nm)

            End If

        End If

    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Dt5 As New DataTable
        Dim rect As Rectangle
        Dim dep_idno As Integer = 0
        Dim Condt As String

        With dgv_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If


            dgv_DrawNo = dgv_Details.Rows(e.RowIndex).Cells(2).Value

            If e.ColumnIndex = 1 Then

                If cbo_Grid_Department.Visible = False Or Val(cbo_Grid_Department.Tag) <> e.RowIndex Then

                    cbo_Grid_Department.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Department_Name from Department_Head order by Department_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_Department.DataSource = Dt1
                    cbo_Grid_Department.DisplayMember = "Department_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Department.Left = .Left + rect.Left
                    cbo_Grid_Department.Top = .Top + rect.Top

                    cbo_Grid_Department.Width = rect.Width
                    cbo_Grid_Department.Height = rect.Height
                    cbo_Grid_Department.Text = .CurrentCell.Value

                    cbo_Grid_Department.Tag = Val(e.RowIndex)
                    cbo_Grid_Department.Visible = True

                    cbo_Grid_Department.BringToFront()
                    cbo_Grid_Department.Focus()

                End If

            Else
                cbo_Grid_Department.Visible = False

            End If

            If e.ColumnIndex = 2 And vCloPic_STS = False Then
                btn_ShowPicture_Click(sender, e)
            Else
                pnl_Picture.Visible = False
            End If

            If e.ColumnIndex = 3 Then

                If cbo_Grid_Item.Visible = False Or Val(cbo_Grid_Item.Tag) <> e.RowIndex Then

                    dep_idno = Common_Procedures.Department_NameToIdNo(con, Trim(cbo_Grid_Department.Text))

                    Condt = ""
                    If dep_idno <> 0 And dep_idno <> 1 Then Condt = " Where (Item_idno = 0 or Department_idno = " & Str(Val(dep_idno)) & ")"

                    cbo_Grid_Item.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Item_DisplayName from Stores_Item_AlaisHead " & Condt & " order by Item_DisplayName", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt2)
                    cbo_Grid_Item.DataSource = Dt2
                    cbo_Grid_Item.DisplayMember = "Item_DisplayName"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Item.Left = .Left + rect.Left
                    cbo_Grid_Item.Top = .Top + rect.Top

                    cbo_Grid_Item.Width = rect.Width
                    cbo_Grid_Item.Height = rect.Height
                    cbo_Grid_Item.Text = .CurrentCell.Value

                    cbo_Grid_Item.Tag = Val(e.RowIndex)
                    cbo_Grid_Item.Visible = True

                    cbo_Grid_Item.BringToFront()
                    cbo_Grid_Item.Focus()

                End If

            Else
                cbo_Grid_Item.Visible = False

            End If


            If e.ColumnIndex = 4 Then

                If cbo_Grid_Brand.Visible = False Or Val(cbo_Grid_Brand.Tag) <> e.RowIndex Then

                    cbo_Grid_Brand.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Brand_Name from Brand_Head order by Brand_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt3)
                    cbo_Grid_Brand.DataSource = Dt3
                    cbo_Grid_Brand.DisplayMember = "Brand_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Brand.Left = .Left + rect.Left
                    cbo_Grid_Brand.Top = .Top + rect.Top

                    cbo_Grid_Brand.Width = rect.Width
                    cbo_Grid_Brand.Height = rect.Height
                    cbo_Grid_Brand.Text = .CurrentCell.Value

                    cbo_Grid_Brand.Tag = Val(e.RowIndex)
                    cbo_Grid_Brand.Visible = True

                    cbo_Grid_Brand.BringToFront()
                    cbo_Grid_Brand.Focus()


                End If


            Else
                cbo_Grid_Brand.Visible = False

            End If


            If e.ColumnIndex = 5 Or e.ColumnIndex = 7 Then

                If e.ColumnIndex = 5 Then

                    pnl_AvailableStock.Visible = True

                    Get_Rate_From_Item_Name()

                    Show_Item_CurrentStock()

                Else

                    pnl_AvailableStock.Visible = False

                End If

                Amount_Calculation()
            Else

                pnl_AvailableStock.Visible = False

            End If

            If e.ColumnIndex = 9 Then

                If cbo_Grid_Machine.Visible = False Or Val(cbo_Grid_Machine.Tag) <> e.RowIndex Then

                    cbo_Grid_Machine.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Machine_Name from Machine_Head order by Machine_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt5)
                    cbo_Grid_Machine.DataSource = Dt5
                    cbo_Grid_Machine.DisplayMember = "Machine_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Machine.Left = .Left + rect.Left
                    cbo_Grid_Machine.Top = .Top + rect.Top

                    cbo_Grid_Machine.Width = rect.Width
                    cbo_Grid_Machine.Height = rect.Height
                    cbo_Grid_Machine.Text = .CurrentCell.Value

                    cbo_Grid_Machine.Tag = Val(e.RowIndex)
                    cbo_Grid_Machine.Visible = True

                    cbo_Grid_Machine.BringToFront()
                    cbo_Grid_Machine.Focus()


                End If


            Else
                cbo_Grid_Machine.Visible = False

            End If



        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        On Error Resume Next
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 7 Then
                    TotalQuantity_Calculation()
                    Amount_Calculation()
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter

        dgv_Details.EditingControl.BackColor = Color.PaleGreen
        dgv_Details.EditingControl.ForeColor = Color.Blue

        dgtxt_Details.SelectAll()

    End Sub
    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 7 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If
            End If
        End With

    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        With dgv_Details

            If e.KeyCode = Keys.Left Then
                If .CurrentCell.ColumnIndex <= 1 Then
                    If .CurrentCell.RowIndex = 0 Then
                        cbo_Issued.Focus()
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)
                    End If
                End If
            End If

        End With

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                n = .CurrentRow.Index

                If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With

        End If
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub cbo_Issued_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Issued.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Issued, cbo_Received, Nothing, "Stores_Item_Issue_Head", "Issued_Name", "", "")
        If (e.KeyValue = 40 And cbo_Issued.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        End If
    End Sub

    Private Sub cbo_Issued_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Issued.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Issued, Nothing, "Stores_Item_Issue_Head", "Issued_Name", "", "", False)
        If Asc(e.KeyChar) = 13 Then

            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

        End If
    End Sub

    Private Sub cbo_Grid_Item_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Item.GotFocus
        vCbo_ItmNm = Trim(cbo_Grid_Item.Text)
    End Sub

    Private Sub cbo_Grid_Item_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Item.KeyDown
        Dim dep_idno As Integer = 0
        Dim Condt As String

        cbo_KeyDwnVal = e.KeyValue

        dep_idno = Common_Procedures.Department_NameToIdNo(con, Trim(cbo_Grid_Department.Text))

        Condt = ""
        If dep_idno <> 0 And dep_idno <> 1 Then Condt = "(Department_idno = " & Str(Val(dep_idno)) & ")"

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Item, Nothing, Nothing, "Stores_Item_AlaisHead", "Item_DisplayName", Condt, "(Item_idno = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Item.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Item.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_Grid_Item_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Item.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dno_nm As String
        Dim Unt_nm As String
        Dim Dep_nm As String
        Dim dep_idno As Integer = 0
        Dim Itm_idno As Integer = 0
        Dim Condt As String
        Dim New_Rate As Double = 0
        Dim Brnd_nm As String

        dep_idno = Common_Procedures.Department_NameToIdNo(con, Trim(cbo_Grid_Department.Text))


        Condt = ""
        If dep_idno <> 0 And dep_idno <> 1 Then Condt = "(Department_idno = " & Str(Val(dep_idno)) & ")"

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Item, Nothing, "Stores_Item_AlaisHead", "Item_DisplayName", Condt, "(Item_idno = 0)")

        If Asc(e.KeyChar) = 13 Then

            If Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(1).Value) = "" Or Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(2).Value) = "" Or Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(6).Value) = "" Or Trim(UCase(vCbo_ItmNm)) <> Trim(UCase(cbo_Grid_Item.Text)) Then

                Itm_idno = Common_Procedures.itemalais_NameToIdNo(con, Trim(cbo_Grid_Item.Text))

                da = New SqlClient.SqlDataAdapter("select a.Drawing_No, b.unit_name, c.department_name from Stores_Item_Head a left outer join unit_head b on a.unit_idno = b.unit_idno left outer join Department_Head c ON a.Department_IdNo = c.Department_IdNo  Where a.item_IdNo = " & Str(Val(Itm_idno)), con)
                da.Fill(dt)

                Dep_nm = ""
                dno_nm = ""
                Unt_nm = ""
                New_Rate = 0
                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                        Dep_nm = Trim(dt.Rows(0).Item("department_name").ToString)
                        dno_nm = Trim(dt.Rows(0).Item("Drawing_No").ToString)
                        Unt_nm = Trim(dt.Rows(0).Item("unit_name").ToString)




                        If Trim(Common_Procedures.settings.CustomerCode) = "1391" Then
                            'If Val(Brand_idno) <> 0 Then
                            da1 = New SqlClient.SqlDataAdapter("select a.rate, bh.Brand_Name from Stores_Item_details a left outer join Brand_Head bh on a.Brand_Idno = bh.Brand_Idno Where a.item_IdNo = " & Str(Val(Itm_idno)), con)
                            'da1 = New SqlClient.SqlDataAdapter("select a.rate from Stores_Item_details a Where a.item_IdNo = " & Str(Val(Itm_idno)) & " and a.Brand_IdNo = " & Str(Val(Brand_idno)), con)
                                da1.Fill(dt1)
                            If dt1.Rows.Count = 1 Then
                                    If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                                        New_Rate = Val(dt1.Rows(0).Item("Rate").ToString)
                                    Brnd_nm = Trim(dt1.Rows(0).Item("Brand_Name").ToString)

                                End If
                                    End If
                                End If

                                dt1.Dispose()
                                da1.Dispose()



                    End If
                End If

                dt.Dispose()
                da.Dispose()

                dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(1).Value = Trim(Dep_nm)
                dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(2).Value = Trim(dno_nm)
                dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(6).Value = Trim(Unt_nm)
                dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(7).Value = Trim(New_Rate)



            End If

        End If

        If Asc(e.KeyChar) = 13 Then
            dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells.Item(3).Value = Trim(cbo_Grid_Item.Text)
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(4)
        End If

    End Sub

    Private Sub cbo_Grid_Unit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Unit.KeyDown
        cbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Unit, Nothing, cbo_Grid_Machine, "Unit_Head", "Unit_name", "", "(Unit_idno = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Unit.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(5)

            End If

            If (e.KeyValue = 40 And cbo_Grid_Unit.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If Trim(.Rows(.CurrentRow.Index).Cells(6).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then

                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(7)

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_Item_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Item.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
        If e.Control = False And e.KeyValue = 17 And cbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Stores_Item_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Item.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Item_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Item.TextChanged
        Try
            If cbo_Grid_Item.Visible Then

                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_Item.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(3).Value = Trim(cbo_Grid_Item.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_Unit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Unit.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Unit, cbo_Grid_Machine, "Unit_Head", "Unit_name", "", "(Unit_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells.Item(6).Value = Trim(cbo_Grid_Unit.Text)
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(7)

        End If
    End Sub

    Private Sub cbo_Grid_Unit_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Unit.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
        If e.Control = False And e.KeyValue = 17 And cbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Unit_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Unit.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Unit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Unit.TextChanged
        Try
            If cbo_Grid_Unit.Visible Then

                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_Unit.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 6 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Unit.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_Machine_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Machine.KeyDown
        cbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Machine, cbo_Grid_Unit, Nothing, "Machine_Head", "Machine_name", "", "(Machine_idno = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Machine.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(8)

            End If

            If (e.KeyValue = 40 And cbo_Grid_Machine.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentRow.Index = .Rows.Count - 1 Then

                    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_date.Focus()
                    End If

                Else
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index + 1).Cells(1)

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_Machine_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Machine.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Machine, Nothing, "Machine_Head", "Machine_name", "", "(Machine_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(7).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_date.Focus()
                    End If

                Else
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index + 1).Cells(1)

                End If
            End With
        End If
    End Sub

    Private Sub cbo_Grid_Machine_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Machine.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
        If e.Control = False And e.KeyValue = 17 And cbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Stores_Machine_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Machine.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub


    Private Sub cbo_Grid_Machine_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Machine.TextChanged
        Try
            If cbo_Grid_Machine.Visible Then

                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_Machine.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 9 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Machine.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim Item_IdNo As Integer
        Dim Condt As String = ""
        Dim Machine_Idno As Integer
        Dim Depart_Idno As Integer

        Try

            Condt = ""
            Item_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Issue_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Issue_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Issue_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_Item.Text) <> "" Then
                Item_IdNo = Common_Procedures.itemalais_NameToIdNo(con, cbo_Filter_Item.Text)
            End If

            If Trim(cbo_Filter_Department.Text) <> "" Then
                Depart_Idno = Common_Procedures.Department_NameToIdNo(con, cbo_Filter_Department.Text)
            End If

            If Trim(cbo_Filter_MachineName.Text) <> "" Then
                Machine_Idno = Common_Procedures.Machine_NameToIdNo(con, cbo_Filter_MachineName.Text)
            End If


            If Val(Item_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Item_IdNo = " & Str(Val(Item_IdNo))
            End If

            If Val(Depart_Idno) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " dh.Department_idno = " & Str(Val(Depart_Idno))

            End If

            If Val(Machine_Idno) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " e.Machine_idno = " & Str(Val(Machine_Idno))
            End If


            da = New SqlClient.SqlDataAdapter("select a.*, b.quantity, c.item_name, d.unit_name, e.Machine_Name,dh.Department_Name from Stores_Item_Issue_Head a left outer join Stores_Item_Issue_Details b on a.issue_code = b.issue_code left outer join Stores_Item_Head c on b.item_idno = c.item_idno left outer join unit_head d on b.unit_idno = d.unit_idno left outer join Machine_head e on b.Machine_idno = e.Machine_idno LEFT outer join Department_Head dh on c.Department_IdNo=dh.Department_Idno  where a.company_idno =" & Str(Val(lbl_Company.Tag)) & " and a.issue_code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by Issue_Date, for_orderby, Issue_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    '       dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Issue_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Issue_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Item_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Quantity").ToString), )
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Unit_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Machine_Name").ToString

                Next i

            End If

            dt2.Clear()
            dt2.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()
    End Sub

    Private Sub cbo_Filter_Item_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Item.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Item, cbo_Filter_Department, cbo_Filter_MachineName, "Stores_Item_AlaisHead", "Item_DisplayName", "", "(Item_idno = 0)")
    End Sub

    Private Sub cbo_Filter_Item_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Item.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Item, cbo_Filter_MachineName, "Stores_Item_AlaisHead", "Item_DisplayName", "", "(Item_idno = 0)")
    End Sub

    Private Sub dtp_date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_date.KeyDown
        If e.KeyValue = 40 Then
            e.Handled = True
            SendKeys.Send("{TAB}")
        End If
        If e.KeyValue = 38 Then
            e.Handled = True
            btn_Cancel.Focus()
        End If
    End Sub

    Private Sub dtp_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno)
            pnl_Back.Enabled = True
            pnl_Filter.Visible = False
        End If

    End Sub
    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub cbo_Department_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Department.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If

        If e.Control = False And e.KeyValue = 17 And cbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Stores_Department_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Department.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Brand_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Brand.KeyDown
        cbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Brand, Nothing, Nothing, "Brand_Head", "Brandname", "", "(Brand_idno = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Brand.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)

            End If

            If (e.KeyValue = 40 And cbo_Grid_Brand.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

            End If

        End With
    End Sub

    Private Sub cbo_Grid_Brand_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Brand.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Brand, Nothing, "Brand_HEAD", "Brand_name", "", "(Brand_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells.Item(4).Value = Trim(cbo_Grid_Brand.Text)
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(5)

        End If
    End Sub

    Private Sub cbo_Grid_Brand_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Brand.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
        If e.Control = False And e.KeyValue = 17 And cbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Stores_Brand_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Brand.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Department_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Department.TextChanged
        Try
            If cbo_Grid_Department.Visible Then

                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_Department.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Department.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dtp_Filter_Fromdate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Filter_Fromdate.KeyDown
        If e.KeyValue = 40 Then
            e.Handled = True
            SendKeys.Send("{TAB}")
        End If
        If e.KeyValue = 38 Then
            btn_Filter_Show.Focus()
        End If
    End Sub

    Private Sub dtp_Filter_Fromdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_Fromdate.KeyPress

        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub dtp_Filter_ToDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Filter_ToDate.KeyDown
        If e.KeyValue = 40 Then
            e.Handled = True
            SendKeys.Send("{TAB}")
        End If
        If e.KeyValue = 38 Then
            e.Handled = True
            SendKeys.Send("+{TAB}")
        End If
    End Sub

    Private Sub dtp_Filter_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_ToDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub TotalQuantity_Calculation()
        Dim Sno As Integer
        Dim Totqty As Single

        Sno = 0
        Totqty = 0
        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(5).Value) <> 0 Then
                    Totqty = Totqty + Val(.Rows(i).Cells(5).Value)
                End If

            Next
        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1391" Then
                .Rows(0).Cells(5).Value = Format(Val(Totqty), "########0.00")
            Else
                .Rows(0).Cells(5).Value = Format(Val(Totqty), "########0")
            End If




        End With

    End Sub

    Private Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btn_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Me.Close()
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Store_Item_Issue_To_machine, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Stores_Item_Issue_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Issue_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try
                PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                    PrintDocument1.Print()
                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try


        Else
            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1

                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from Stores_Item_Issue_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Issue_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_name, d.Unit_name, e.Machine_name, f.Brand_Name from Stores_Item_Issue_Details a INNER JOIN Stores_Item_Head b ON a.Item_idno = b.Item_idno LEFT OUTER JOIN Unit_Head d ON a.Unit_idno = d.Unit_idno LEFT OUTER JOIN Machine_Head e ON a.Machine_idno = e.Machine_idno LEFT OUTER JOIN Brand_Head f ON a.Brand_idno = f.Brand_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Issue_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        Printing_Format1(e)


    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim ItmNm1 As String, ItmNm2 As String

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            If ps.Width = 800 And ps.Height = 600 Then
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                PpSzSTS = True
                Exit For
            End If
        Next

        If PpSzSTS = False Then

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    e.PageSettings.PaperSize = ps
                    PpSzSTS = True
                    Exit For
                End If
            Next

            If PpSzSTS = False Then
                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        e.PageSettings.PaperSize = ps
                        Exit For
                    End If
                Next
            End If

        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 30
            .Top = 30
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 11, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With
        If PrintDocument1.DefaultPageSettings.Landscape = True Then
            With PrintDocument1.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1391" Then
            NoofItems_PerPage = 35
        Else
            NoofItems_PerPage = 8
        End If



        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(35) : ClAr(2) = 180 : ClAr(3) = 80 : ClAr(4) = 80 : ClAr(5) = 60 : ClAr(6) = 90 : ClAr(7) = 90
        ClAr(8) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6))

        TxtHgt = 19

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 35 Then
                            For I = 35 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 35
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Brand_Name").ToString, LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Quantity").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 20, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Unit_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 12, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Machine_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 20, CurY, 0, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim C1 As Single
        Dim W1 As Single
        Dim S1 As Single

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_name, d.Unit_name, e.Machine_name, f.Brand_Name from Stores_Item_Issue_Details a INNER JOIN Stores_Item_Head b ON a.Item_idno = b.Item_idno LEFT OUTER JOIN Unit_Head d ON a.Unit_idno = d.Unit_idno LEFT OUTER JOIN Machine_Head e ON a.Machine_idno = e.Machine_idno LEFT OUTER JOIN Brand_Head f ON a.Brand_idno = f.Brand_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Issue_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
        W1 = e.Graphics.MeasureString("D.C DATE  : ", pFont).Width
        S1 = e.Graphics.MeasureString("Received From :  ", pFont).Width


        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "Issued To", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "M/s." & prn_HdDt.Rows(0).Item("Issued_Name").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)

        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Issue_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Received From", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "M/s." & prn_HdDt.Rows(0).Item("Received_Name").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "Received From :  " & "M/s." & prn_HdDt.Rows(0).Item("Received_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Issue_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ITEM NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BRAND", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "QUANTITY", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 20, CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MACHINE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 60, CurY, 2, ClAr(8), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font


        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 20, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 50, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 20, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 20, LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 20, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 20, LnAr(3))

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub btn_EnLargePicture_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_EnLargePicture.Click

        If IsNothing(PictureBox1.Image) = False Then

            EnlargePicture.Text = "IMAGE   -   " & dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(1).Value
            EnlargePicture.PictureBox2.ClientSize = PictureBox1.Image.Size
            EnlargePicture.PictureBox2.Image = CType(PictureBox1.Image.Clone, Image)
            EnlargePicture.ShowDialog()

            dgv_Details.Focus()
            dgv_Details.CurrentCell.Selected = True

        End If

    End Sub

    Private Sub btn_ShowPicture_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_ShowPicture.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dep_IdNo As Integer

        Dep_IdNo = Common_Procedures.Department_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(1).Value)

        PictureBox1.Image = Nothing
        pnl_Picture.Visible = False

        If Val(Dep_IdNo) <> 0 Then

            Da = New SqlClient.SqlDataAdapter("select * from Department_head a where Department_idno <> 1 and Department_idno = " & Str(Val(Dep_IdNo)), con)
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                If IsDBNull(Dt1.Rows(0).Item("Department_Image")) = False Then
                    Dim imageData As Byte() = DirectCast(Dt1.Rows(0).Item("Department_Image"), Byte())
                    If Not imageData Is Nothing Then
                        Using ms As New System.IO.MemoryStream(imageData, 0, imageData.Length)
                            ms.Write(imageData, 0, imageData.Length)
                            If imageData.Length > 0 Then

                                PictureBox1.Image = Image.FromStream(ms)

                                pnl_Picture.Visible = True
                                pnl_Picture.BringToFront()

                            End If
                        End Using
                    End If
                End If

            End If

        End If

        Dt1.Dispose()
        Da.Dispose()

    End Sub

    Private Sub btn_ClosePicture_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ClosePicture.Click
        vCloPic_STS = True
        pnl_Picture.Visible = False
        dgv_Details.Focus()
        dgv_Details.CurrentCell.Selected = True
        vCloPic_STS = False
    End Sub

    Private Sub btn_ToPDF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_ToPDF.Click
        Dim ps As Printing.PrinterSettings = New Printing.PrinterSettings
        ps.PrinterName = "doPDF v7"
        PrintDocument1.PrinterSettings = ps
        PrintDocument1.Print()
    End Sub

    Private Sub btn_Mail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Mail.Click
        Try
            Dim SmtpServer As New SmtpClient()
            Dim mail As New MailMessage()
            Dim MailTxt As String

            MailTxt = "Item Issue Entry Added/Edited" & vbCrLf & vbCrLf

            MailTxt = MailTxt & "ISS.NO:" & Trim(lbl_DcNo.Text) & vbCrLf & "ISS.DATE:" & Trim(dtp_date.Text) & vbCrLf

            For i = 0 To dgv_Details.Rows.Count - 1

                If Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Then
                    MailTxt = Trim(MailTxt) & IIf(Trim(MailTxt) <> "", vbCrLf, "") & Trim(dgv_Details.Rows(i).Cells(3).Value) & "       -          " & Val(dgv_Details.Rows(i).Cells(5).Value) & " " & Trim(dgv_Details.Rows(i).Cells(6).Value)
                End If

            Next

            SmtpServer.Port = 587
            SmtpServer.Host = "smtp.gmail.com"
            SmtpServer.UseDefaultCredentials = False
            SmtpServer.EnableSsl = True

            SmtpServer.Credentials = New Net.NetworkCredential("tsoft.tirupur@gmail.com", "GOLD@tn39av7417")
            'SmtpServer.Credentials = New Net.NetworkCredential("varalakshmithanges@gmail.com", "GOLD@tn39av7417")
            'SmtpServer.Credentials = New Net.NetworkCredential("varalakshmithanges@gmail.com", "gold&VL@19=rj")
            'SmtpServer.Credentials = New Net.NetworkCredential("t.thanges@gmail.com", "gold@tn39av7417")
            'SmtpServer.Credentials = New Net.NetworkCredential("t.thanges@gmail.com", "cikysrpmkzbwliuc")
            'SmtpServer.Credentials = New Net.NetworkCredential("varalakshmithanges@gmail.com", "thanges19")
            'SmtpServer.Credentials = New Net.NetworkCredential("tsoft.tirupur@gmail.com", "8508403221")
            'SmtpServer.Credentials = New Net.NetworkCredential("t.thanges@gmail.com", "rj17052012")

            mail = New MailMessage()
            'mail.From = New MailAddress("varalakshmithanges@gmail.com")
            mail.From = New MailAddress("tsoft.tirupur@gmail.com")
            'mail.From = New MailAddress("t.thanges@gmail.com")
            'srirajatex@gmail.com
            mail.To.Add("thanges@rediffmail.com")
            mail.Subject = "Store entry added/edited"
            mail.Body = Trim(MailTxt)
            SmtpServer.Send(mail)

            MessageBox.Show("Mail send Sucessfully", "FOR MAILING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND MAIL...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_Sms_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Sms.Click
        Dim request As HttpWebRequest
        Dim response As HttpWebResponse = Nothing
        Dim url As String
        Dim i As Integer = 0
        Dim smstxt As String = ""


        Try

            smstxt = "ISS.NO:" & Trim(lbl_DcNo.Text) & ";" & "ISS.DATE:" & Trim(dtp_date.Text) & ";"

            For i = 0 To dgv_Details.Rows.Count - 1

                If Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Then
                    smstxt = Trim(smstxt) & IIf(Trim(smstxt) <> "", "%2C+", "") & Trim(dgv_Details.Rows(i).Cells(3).Value) & "-" & Val(dgv_Details.Rows(i).Cells(5).Value) & " " & Trim(dgv_Details.Rows(i).Cells(6).Value)
                End If

            Next

            url = "http://sms.shamsoft.in/app/smsapi/index.php?key=355C7A0B5595B2&routeid=73&type=text&contacts=8508403222&senderid=WEBSMS&msg=" & Trim(smstxt)

            'THIS IS OK
            'url = "http://sms.shamsoft.in/app/smsapi/index.php?key=355C7A0B5595B2&routeid=73&type=text&contacts=8508403222&senderid=WEBSMS&msg=Hello+People%2C+have+a+great+day"

            'url = "http://sms.shamsoft.in/app/smsapi/index.php?key=355C7A0B5595B2&routeid=14&type=text&contacts=97656XXXXX,98012XXXXX&senderid=DEMO&msg=Hello+People%2C+have+a+great+day"

            'url = "http://103.16.101.52:8080/bulksms/bulksms?username=nila-jenial&password=nila123&type=0&dlr=1&destination=918508403222&source=JENIAL&message=testmsg"

            'url = "http://103.16.101.52:8080/bulksms/bulksms?username=nila-jenial&password=nila123&type=0&dlr=1&destination=918508403222&source=JENIAL&message=testmsg"

            request = DirectCast(WebRequest.Create(url), HttpWebRequest)

            response = DirectCast(request.GetResponse(), HttpWebResponse)

            MessageBox.Show("Response: " & response.StatusDescription)

            'WebBrowser1.Navigate("http://103.16.101.52:8080/bulksms/bulksms?username=nila-jenial&password=nila123&type=0&dlr=1&destination=918508403222&source=JENIAL&message=testmsg")
            'MsgBox("sms send")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try



    End Sub

    Private Sub dtp_date_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_date.ValueChanged

    End Sub

    Private Sub cbo_Received_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbo_Received.SelectedIndexChanged

    End Sub

    Private Sub cbo_New_Old_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_New_Old.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_New_Old, dtp_date, cbo_Received, "", "", "", "")
    End Sub

    Private Sub cbo_New_Old_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_New_Old.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_New_Old, cbo_Received, "", "", "", "")
    End Sub

    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub
    Private Sub Amount_Calculation()


        Dim vTotAmount As String = ""

        vTotAmount = 0

        With dgv_Details

            For i = 0 To .RowCount - 1


                If .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 7 Then
                    .Rows(i).Cells(8).Value = Format(Val(.Rows(i).Cells(5).Value) * Val(.Rows(i).Cells(7).Value), "##########.##")
                    vTotAmount = vTotAmount + Val(.Rows(i).Cells(8).Value)
                End If
            Next
        End With

        With dgv_Details_Total

            .Rows(0).Cells(8).Value = Format(Val(vTotAmount), "#########.##")

        End With
    End Sub
    Private Sub dgtxt_Details_TextChanged(sender As Object, e As EventArgs) Handles dgtxt_Details.TextChanged

        'With dgv_Details

        '    If .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 7 Then

        '        Amount_Calculation()

        '    End If
        'End With

        Try
            With dgv_Details

                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                    End If
                End If
            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub
    Private Sub cbo_Filter_Department_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Filter_Department.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Department, dtp_Filter_ToDate, cbo_Filter_Item, "Department_Head", "Department_Name", "", "(Department_Idno = 0)")
    End Sub

    Private Sub cbo_Filter_Department_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Filter_Department.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Department, cbo_Filter_Item, "Department_Head", "Department_Name", "", "(Department_Idno = 0)")
    End Sub
    Private Sub cbo_Filter_Department_GotFocus(sender As Object, e As EventArgs) Handles cbo_Filter_Department.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Department_Head", "Department_Name", "", "(Department_Idno = 0)")
    End Sub
    Private Sub cbo_Filter_MachineName_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Filter_MachineName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_MachineName, cbo_Filter_Item, btn_Filter_Show, "machine_head", "Machine_Name", "", "(machine_Idno = 0)")
    End Sub

    Private Sub cbo_Filter_MachineName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Filter_MachineName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_MachineName, btn_Filter_Show, "machine_head", "Machine_Name", "", "(machine_Idno = 0)")
    End Sub

    Private Sub cbo_Filter_MachineName_GotFocus(sender As Object, e As EventArgs) Handles cbo_Filter_MachineName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "machine_head", "Machine_Name", "", "(machine_Idno = 0)")
    End Sub
    Private Sub Get_Rate_From_Item_Name()
        Dim vRate_FieldName = ""
        Dim Brand_idno As Integer = 0
        Dim Itm_idno As Integer = 0

        If dgv_Details.RowCount = 0 Then Exit Sub
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

        If Trim(cbo_New_Old.Text) = "OLD" Then
            vRate_FieldName = "Rate_Old"
        Else
            vRate_FieldName = "Rate"
        End If


        If Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(1).Value) <> "" Or Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(2).Value) <> "" Or Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(6).Value) <> "" Then
            If Trim(vCbo_BrandNm) <> Trim(cbo_Grid_Brand.Text) Or Trim(vCbo_Item_OLD_NEW_Nm) <> Trim(cbo_New_Old.Text) Or Trim(UCase(vCbo_ItmNm)) <> Trim(UCase(cbo_Grid_Item.Text)) Then

                Itm_idno = Common_Procedures.itemalais_NameToIdNo(con, Trim(cbo_Grid_Item.Text))

                If Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(4).Value) <> "" Then

                    Brand_idno = Common_Procedures.Brand_NameToIdNo(con, Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(4).Value))

                End If
                'If Val(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(7).Value) = 0 Or Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(7).Value) = "" Then

                dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(7).Value = Common_Procedures.get_FieldValue(con, "Stores_item_Details", " " & vRate_FieldName & " ", " Brand_IdNo = " & Val(Brand_idno) & " and item_IdNo = " & Str(Val(Itm_idno)) & " ")

                'End If
            End If
        End If
    End Sub

    Private Sub cbo_New_Old_Leave(sender As Object, e As EventArgs) Handles cbo_New_Old.Leave
        Get_Rate_From_Item_Name()
    End Sub
    Private Sub cbo_Grid_Brand_GotFocus(sender As Object, e As EventArgs) Handles cbo_Grid_Brand.GotFocus
        vCbo_BrandNm = Trim(cbo_Grid_Brand.Text)
    End Sub
    Private Sub cbo_New_Old_GotFocus(sender As Object, e As EventArgs) Handles cbo_New_Old.GotFocus
        vCbo_Item_OLD_NEW_Nm = Trim(cbo_New_Old.Text)
    End Sub
    Private Sub Show_Item_CurrentStock()
        Dim vItemID As Integer
        Dim CurStk As Decimal
        Dim vITEM_New_Old = ""
        Dim Brand_idno As Integer = 0



        If Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(3).Value) <> "" And Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(4).Value) <> "" Then

            vItemID = Common_Procedures.Item_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(3).Value)
            Brand_idno = Common_Procedures.Brand_NameToIdNo(con, Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(4).Value))

            If Val(lbl_AvailableStock.Tag) <> Val(vItemID) Then
                lbl_AvailableStock.Tag = 0
                lbl_AvailableStock.Text = ""
                If Val(vItemID) <> 0 Then
                    CurStk = Common_Procedures.Get_Stores_Item_CurrentStock(con, Val(lbl_Company.Tag), vItemID, Brand_idno, Trim(cbo_New_Old.Text))
                    lbl_AvailableStock.Tag = vItemID
                    lbl_AvailableStock.Text = CurStk
                End If
            End If

        Else
            lbl_AvailableStock.Tag = 0
            lbl_AvailableStock.Text = ""

        End If
    End Sub

    Private Sub dgv_Details_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Details.CellContentClick

    End Sub

    Private Sub cbo_Grid_Department_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Grid_Department.SelectedIndexChanged

    End Sub

    Private Sub cbo_Grid_Item_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Grid_Item.SelectedIndexChanged

    End Sub

    Private Sub cbo_Issued_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Issued.SelectedIndexChanged

    End Sub
End Class