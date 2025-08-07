Imports Excel = Microsoft.Office.Interop.Excel
Public Class Fabric_Bundle_Entry1



    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private SaveAll_STS As Boolean = False
    Private Pk_Condition As String = "FBUNE-"
    Private vEntryType As String = ""
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Public Shared vEntFnYrCode As String = ""
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private LastNo As String = ""

    Public Sub New(ByVal EntryType As String)
        vEntryType = Trim(UCase(EntryType))
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()

        New_Entry = False
        Insert_Entry = False

        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        msk_date.Text = ""
        msk_date.SelectionStart = 0
        dtp_Date.Text = ""

        cbo_Filter_CountName.Text = ""
        cbo_Filter_MillName.Text = ""
        cbo_Filter_PartyName.Text = ""

        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()

        cbo_Grid_FabricName.Visible = False
        cbo_Grid_FabricName.Top = 2000

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim Msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            Msktxbx = Me.ActiveControl
            Msktxbx.SelectionStart = 0
        End If

        If Me.ActiveControl.Name <> cbo_Grid_FabricName.Name Then
            cbo_Grid_FabricName.Visible = False
            cbo_Grid_FabricName.Top = 2000
        End If

        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Grid_Cell_DeSelect()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            End If
        End If

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

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
        dgv_Details_Total.CurrentCell.Selected = False
        dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Fabric_Bundle_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_FabricName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_FabricName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Fabric_Bundle_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Fabric_Bundle_Entry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim OpYrCode As String = ""

        Me.Text = ""

        If Trim(UCase(vEntryType)) = "OPENING" Then
            Pk_Condition = "FBUOP-"
            Label1.Text = "OPENING BUNDLES"

            OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
            OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

            vEntFnYrCode = Trim(OpYrCode)

            btn_fromExcel.Visible = True
            btn_SaveAll.Visible = True

        Else
            Pk_Condition = "FBUNE-"
            Label1.Text = "BUNDLE ENTRY"
            vEntFnYrCode = Trim(Common_Procedures.FnYearCode)

        End If

        con.Open()

        dtp_Date.Text = ""
        msk_date.Text = ""
        msk_date.SelectionStart = 0

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_FabricName.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_FabricName.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()
    End Sub

    Private Sub Fabric_Bundle_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = dgv_Details


            If IsNothing(dgv1) = False Then

                With dgv1


                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= 4 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                btn_save.Focus()

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
                                msk_date.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(4)

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

    Private Sub Close_Form()
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            lbl_Company.Tag = 0
            lbl_Company.Text = ""
            Me.Text = ""
            Common_Procedures.CompIdNo = 0

            CompCondt = ""
            If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                CompCondt = "Company_Type = 'ACCOUNT'"
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Company_Head where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0", con)
            dt1 = New DataTable
            da.Fill(dt1)

            NoofComps = 0
            If dt1.Rows.Count > 0 Then
                If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                    NoofComps = Val(dt1.Rows(0)(0).ToString)
                End If
            End If
            dt1.Clear()

            If Val(NoofComps) > 1 Then

                Dim f As New Company_Selection
                f.ShowDialog()

                If Val(Common_Procedures.CompIdNo) <> 0 Then

                    da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head where Company_IdNo = " & Str(Val(Common_Procedures.CompIdNo)), con)
                    dt1 = New DataTable
                    da.Fill(dt1)

                    If dt1.Rows.Count > 0 Then
                        If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                            lbl_Company.Tag = Val(dt1.Rows(0)(0).ToString)
                            lbl_Company.Text = Trim(dt1.Rows(0)(1).ToString)
                            Me.Text = Trim(dt1.Rows(0)(1).ToString)
                        End If
                    End If
                    dt1.Clear()
                    dt1.Dispose()
                    da.Dispose()

                    new_record()

                Else
                    Me.Close()

                End If

            Else

                Me.Close()

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

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

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(vEntFnYrCode)

        Try
            da1 = New SqlClient.SqlDataAdapter("select a.* from Fabric_Bundle_Entry_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Fabric_Bundle_Entry_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Fabric_Bundle_Entry_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Fabric_Bundle_Entry_Date").ToString
                msk_date.Text = dtp_Date.Text
                msk_date.SelectionStart = 0
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                txt_BundleNo.Text = dt1.Rows(0).Item("Bundle_No").ToString
                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from Fabric_Bundle_Entry_Details a INNER JOIN Cloth_Head b on a.Cloth_IdNo = b.Cloth_IdNo where a.Fabric_Bundle_Entry_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                        dgv_Details.Rows(n).Cells(2).Value = Val(dt2.Rows(i).Item("Mark").ToString)
                       
                        dgv_Details.Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Total_Pcs").ToString)
                        dgv_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")

                    Next i

                End If

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()

                    .Rows(0).Cells(3).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.000")
                End With

                dt2.Clear()

                dt2.Dispose()
                da2.Dispose()

            Else
                new_record()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(vEntFnYrCode)

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Fabric_Bundle_Entry_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Bundle_Entry_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Fabric_Bundle_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Bundle_Entry_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            If msk_date.Enabled = True And msk_date.Visible = True Then msk_date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'SIZING') order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select count_name from count_head order by count_name", con)
            da.Fill(dt2)
            cbo_Filter_CountName.DataSource = dt2
            cbo_Filter_CountName.DisplayMember = "count_name"

            da = New SqlClient.SqlDataAdapter("select Mill_name from Mill_head order by Mill_name", con)
            da.Fill(dt2)
            cbo_Filter_MillName.DataSource = dt2
            cbo_Filter_MillName.DisplayMember = "Mill_name"


            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_MillName.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            cbo_Filter_MillName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Filter.BringToFront()
        Pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Fabric_Bundle_Entry_No from Fabric_Bundle_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Bundle_Entry_Code like '%/" & Trim(vEntFnYrCode) & "' and Fabric_Bundle_Entry_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby, Fabric_Bundle_Entry_No", con)
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
        Dim OrdByNo As String = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Fabric_Bundle_Entry_No from Fabric_Bundle_Entry_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Bundle_Entry_Code like '%/" & Trim(vEntFnYrCode) & "'  and Fabric_Bundle_Entry_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby, Fabric_Bundle_Entry_No", con)
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
        Dim OrdByNo As String = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Fabric_Bundle_Entry_No from Fabric_Bundle_Entry_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Bundle_Entry_Code like '%/" & Trim(vEntFnYrCode) & "'  and Fabric_Bundle_Entry_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby desc, Fabric_Bundle_Entry_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Fabric_Bundle_Entry_No from Fabric_Bundle_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Bundle_Entry_Code like '%/" & Trim(vEntFnYrCode) & "'  and Fabric_Bundle_Entry_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby desc, Fabric_Bundle_Entry_No desc", con)
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
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Fabric_Bundle_Entry_Head", "Fabric_Bundle_Entry_Code", "For_OrderBy", "(Fabric_Bundle_Entry_Code LIKE '" & Trim(Pk_Condition) & "%')", Val(lbl_Company.Tag), vEntFnYrCode)

            lbl_RefNo.ForeColor = Color.Red

            msk_date.Text = Date.Today.ToShortDateString
            msk_date.SelectionStart = 0
            da = New SqlClient.SqlDataAdapter("select top 1 * from Fabric_Bundle_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Bundle_Entry_Code like '%/" & Trim(vEntFnYrCode) & "'  and Fabric_Bundle_Entry_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby desc, Fabric_Bundle_Entry_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Fabric_Bundle_Entry_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Fabric_Bundle_Entry_Date").ToString
                End If
            End If
            dt1.Clear()

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        dt1.Dispose()
        da.Dispose()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Dc.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(vEntFnYrCode)

            Da = New SqlClient.SqlDataAdapter("select Fabric_Bundle_Entry_No from Fabric_Bundle_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Bundle_Entry_Code = '" & Trim(RecCode) & "'", con)
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

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Sizing_Yarn_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Sizing_Yarn_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Dc No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(vEntFnYrCode)

            Da = New SqlClient.SqlDataAdapter("select Fabric_Bundle_Entry_No from Fabric_Bundle_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Bundle_Entry_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Dc No", "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim vClo_IdNo As Integer = 0
        Dim vTotBndls As Single, vTotPcs As Single, vTotYrds As Single
        Dim EntID As String = ""
        Dim clthStock_In As String = ""
        Dim clthStk_Pcs_Mtr As String = ""


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Common_Procedures.UserRight_Check(Common_Procedures.UR.PackinSlip_Entry, New_Entry) = False Then Exit Sub

        If Pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        If Trim(UCase(vEntryType)) <> "OPENING" Then
            If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
                MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
                Exit Sub
            End If
        End If

        For i = 0 To dgv_Details.RowCount - 1

            If Val(dgv_Details.Rows(i).Cells(3).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(4).Value) <> 0 Then

                vClo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, dgv_Details.Rows(i).Cells(1).Value)
                If Val(vClo_IdNo) = 0 Then
                    MessageBox.Show("Invalid Fabric Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                    dgv_Details.Focus()
                    Exit Sub
                End If

                If Val(dgv_Details.Rows(i).Cells(3).Value) = 0 Then
                    MessageBox.Show("Invalid Pcs", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(3)
                    dgv_Details.Focus()
                    Exit Sub
                End If

            End If

        Next

        vTotBndls = 0 : vTotPcs = 0 : vTotYrds = 0
        If dgv_Details_Total.RowCount > 0 Then
            'vTotBndls = Val(dgv_Details_Total.Rows(0).Cells(3).Value())
            vTotPcs = Val(dgv_Details_Total.Rows(0).Cells(3).Value())
            vTotYrds = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(vEntFnYrCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Fabric_Bundle_Entry_Head", "Fabric_Bundle_Entry_Code", "For_OrderBy", "(Fabric_Bundle_Entry_Code LIKE '" & Trim(Pk_Condition) & "%')", Val(lbl_Company.Tag), vEntFnYrCode, tr)

                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(vEntFnYrCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_date.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into Fabric_Bundle_Entry_Head(Fabric_Bundle_Entry_Code, Company_IdNo, Fabric_Bundle_Entry_No, for_OrderBy, Fabric_Bundle_Entry_Date, Bundle_No, Total_Pcs, Total_Meters, User_idNo) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @EntryDate, '" & Trim(vTotBndls) & "', " & Str(Val(vTotPcs)) & ", " & Str(Val(vTotYrds)) & " ,  " & Val(Common_Procedures.User.IdNo) & " )"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Fabric_Bundle_Entry_Head set Fabric_Bundle_Entry_Date = @EntryDate, Bundle_No = '" & Trim(vTotBndls) & "', Total_Pcs = " & Str(Val(vTotPcs)) & ", Total_Meters = " & Str(Val(vTotYrds)) & " ,  User_idNo = " & Str(Val(Common_Procedures.User.IdNo)) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Bundle_Entry_Code = '" & Trim(NewCode) & "' "
                cmd.ExecuteNonQuery()

            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            If Trim(UCase(vEntryType)) = "OPENING" Then
                Partcls = "Opening Bundle : Ref.No. " & Trim(lbl_RefNo.Text)
            Else
                Partcls = "Bundle Packing : Ref.No. " & Trim(lbl_RefNo.Text)
            End If

            PBlNo = Trim(lbl_RefNo.Text)

            cmd.CommandText = "Delete from Fabric_Bundle_Entry_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Bundle_Entry_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details
                Sno = 0

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(4).Value) <> 0 Then

                        Sno = Sno + 1

                        vClo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        cmd.CommandText = "Insert into Fabric_Bundle_Entry_Details ( Fabric_Bundle_Entry_Code,              Company_IdNo        ,        Fabric_Bundle_Entry_No ,                               for_OrderBy                              , Fabric_Bundle_Entry_Date,            Sl_No     ,               Cloth_IdNo    ,                         Mark                ,                       Total_Pcs           ,                      Total_Meters         ) " & _
                                            "          Values                      ('" & Trim(NewCode) & "'  , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",       @EntryDate           , " & Str(Val(Sno)) & ",  " & Str(Val(vClo_IdNo)) & ", " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & " ) "
                        cmd.ExecuteNonQuery()


                        clthStock_In = ""

                        Da = New SqlClient.SqlDataAdapter("Select Stock_In , Meters_Pcs from  Cloth_Head Where Cloth_idno = " & Str(Val(vClo_IdNo)), con)
                        Da.SelectCommand.Transaction = tr
                        Dt2 = New DataTable
                        Da.Fill(Dt2)
                        If Dt2.Rows.Count > 0 Then
                            clthStock_In = Dt2.Rows(0)("Stock_In").ToString
                        End If
                        Dt2.Clear()

                        clthStk_Pcs_Mtr = 0

                        If Trim(UCase(clthStock_In)) = "PCS" Then
                            clthStk_Pcs_Mtr = Val(.Rows(i).Cells(3).Value)
                        Else
                            clthStk_Pcs_Mtr = Val(.Rows(i).Cells(4).Value)
                        End If

                        cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code         ,             Company_IdNo         ,             Reference_No      ,                               for_OrderBy                              , Reference_Date,                               StockOff_IdNo               ,                               DeliveryTo_Idno             , ReceivedFrom_Idno  ,         Entry_ID     ,      Party_Bill_No   ,      Particulars       ,             Sl_No       ,          Cloth_Idno        , Folding ,                      Mark                , UnChecked_Meters ,                      Bundle              ,                      Pcs                 , Pcs_Per_Bundle , Meters_Type1        , Meters_Type2, Meters_Type3, Meters_Type4, Meters_Type5 ) " & _
                                              "        Values                         ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @EntryDate  , " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ",         0          , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   " & Str(Val(Sno)) & " , " & Str(Val(vClo_IdNo)) & ",   100   , " & Str(Val(.Rows(i).Cells(2).Value)) & ",       0          ,  0                                       , " & Str(Val(.Rows(i).Cells(3).Value)) & ", 0, " & Str(Val(clthStk_Pcs_Mtr)) & ",       0     ,       0     ,       0     ,       0      ) "
                        cmd.ExecuteNonQuery()



                    End If

                Next

            End With

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_RefNo.Text)
                End If
            Else
                move_record(lbl_RefNo.Text)
            End If


        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle

        With dgv_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 1 Then

                If cbo_Grid_FabricName.Visible = False Or Val(cbo_Grid_FabricName.Tag) <> e.RowIndex Then

                    cbo_Grid_FabricName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_FabricName.DataSource = Dt1
                    cbo_Grid_FabricName.DisplayMember = "Cloth_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_FabricName.Left = .Left + rect.Left
                    cbo_Grid_FabricName.Top = .Top + rect.Top

                    cbo_Grid_FabricName.Width = rect.Width
                    cbo_Grid_FabricName.Height = rect.Height
                    cbo_Grid_FabricName.Text = .CurrentCell.Value

                    cbo_Grid_FabricName.Tag = Val(e.RowIndex)
                    cbo_Grid_FabricName.Visible = True

                    cbo_Grid_FabricName.BringToFront()
                    cbo_Grid_FabricName.Focus()

                End If

            Else

                cbo_Grid_FabricName.Visible = False
                cbo_Grid_FabricName.Top = 2000

            End If

        End With
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 6 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Try
            With dgv_Details
                If .Visible Then
                    If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Then
                        ' .Rows(.CurrentCell.RowIndex).Cells(5).Value = Val(.Rows(.CurrentCell.RowIndex).Cells(3).Value) * .Rows(.CurrentCell.RowIndex).Cells(4).Value
                        .Rows(.CurrentCell.RowIndex).Cells(4).Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(2).Value) * .Rows(.CurrentCell.RowIndex).Cells(3).Value, "##########0.00")
                        Total_Calculation()
                    End If
                End If
            End With

        Catch ex As Exception
            '---
        End Try
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

                Total_Calculation()

            End With

        End If

    End Sub

    Private Sub dgv_YarnDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_YarnDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer

        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_Details
                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
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

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotBndls As Single, TotPcs As Single, TotYrds As Single

        Sno = 0
        TotBndls = 0
        TotPcs = 0
        TotYrds = 0
        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(4).Value) <> 0 Then
                    ' TotBndls = TotBndls + Val(.Rows(i).Cells(3).Value)
                    TotPcs = TotPcs + Val(.Rows(i).Cells(3).Value)
                    TotYrds = TotYrds + Val(.Rows(i).Cells(4).Value)
                End If
            Next
        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            '  .Rows(0).Cells(3).Value = Val(TotBndls)
            .Rows(0).Cells(3).Value = Val(TotPcs)
            .Rows(0).Cells(4).Value = Format(Val(TotYrds), "########0.00")
        End With

    End Sub

    Private Sub cbo_Grid_FabricName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_FabricName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_FabricName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_FabricName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_FabricName, Nothing, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
        With dgv_Details
            With dgv_Details

                If (e.KeyValue = 38 And cbo_Grid_FabricName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    If .CurrentCell.RowIndex = 0 Then
                        msk_date.Focus()
                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(4)
                    End If
                End If
                If (e.KeyValue = 40 And cbo_Grid_FabricName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                End If

            End With

        End With
    End Sub

    Private Sub cbo_Grid_FabricName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_FabricName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_FabricName, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details
                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells(1).Value) = "" Then
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        msk_date.Focus()
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If
            End With
        End If

    End Sub

    Private Sub cbo_Grid_FabricName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_FabricName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_FabricName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_FabricName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_FabricName.TextChanged
        Try
            If cbo_Grid_FabricName.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_FabricName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Grid_FabricName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Del_IdNo As Integer, Cnt_IdNo As Integer, Mil_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Del_IdNo = 0
            Cnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Fabric_Bundle_Entry_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Fabric_Bundle_Entry_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Fabric_Bundle_Entry_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If
            If Trim(cbo_Filter_CountName.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_CountName.Text)
            End If

            If Trim(cbo_Filter_MillName.Text) <> "" Then
                Mil_IdNo = Common_Procedures.Mill_NameToIdNo(con, cbo_Filter_MillName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.DeliveryTo_IdNo = " & Str(Val(Led_IdNo)) & ")"
            End If
            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Fabric_Bundle_Entry_Code IN ( select z1.Fabric_Bundle_Entry_Code from Fabric_Bundle_Entry_Details z1 where z1.Count_IdNo = " & Str(Val(Cnt_IdNo)) & " )"
                'Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Count_IdNo = " & Str(Val(Cnt_IdNo)) & ""
            End If

            If Val(Mil_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Fabric_Bundle_Entry_Code IN ( select z2.Fabric_Bundle_Entry_Code from Fabric_Bundle_Entry_Details z2 where z2.Mill_IdNo = " & Str(Val(Mil_IdNo)) & " )"
                'Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Mill_IdNo = " & Str(Val(Mil_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, e.Ledger_Name from Fabric_Bundle_Entry_Head a inner join Ledger_head e on a.DeliveryTo_IdNo = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Fabric_Bundle_Entry_Code like '%/" & Trim(vEntFnYrCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Fabric_Bundle_Entry_Date, a.for_orderby, a.Fabric_Bundle_Entry_No", con)
            'da = New SqlClient.SqlDataAdapter("select a.*, e.Ledger_Name from Fabric_Bundle_Entry_Head a left outer join Fabric_Bundle_Entry_Details b on a.Fabric_Bundle_Entry_Code = b.Fabric_Bundle_Entry_Code left outer join Ledger_head e on a.Ledger_idno = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Fabric_Bundle_Entry_Code like '%/" & Trim(vEntFnYrCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Fabric_Bundle_Entry_Date, a.for_orderby, a.Fabric_Bundle_Entry_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Fabric_Bundle_Entry_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Fabric_Bundle_Entry_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Bags").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Total_Cones").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")

                Next i

            End If

            dt2.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt2.Dispose()
            da.Dispose()

            If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

        End Try

    End Sub


    Private Sub dtp_Filter_Fromdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_Fromdate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            dtp_Filter_ToDate.Focus()
        End If

    End Sub

    Private Sub dtp_Filter_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_ToDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Filter_PartyName.Focus()
        End If
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub


    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_CountName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_CountName, cbo_Filter_PartyName, cbo_Filter_MillName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_CountName, cbo_Filter_MillName, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno)
            Pnl_Back.Enabled = True
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

    Private Sub cbo_Filter_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_MillName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_MillName, cbo_Filter_CountName, btn_Filter_Show, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_MillName, btn_Filter_Show, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Exit Sub
    End Sub

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        If e.KeyCode = 38 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.Rows.Count - 1).Cells(1)
            e.Handled = True
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = 40 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            e.Handled = True
            e.SuppressKeyPress = True
        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
        End If

    End Sub

    Private Sub msk_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            e.Handled = True
        End If
    End Sub

    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.KeyCode = 107 Then
            msk_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
            msk_date.SelectionStart = 0
        ElseIf e.KeyCode = 109 Then
            msk_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
            msk_date.SelectionStart = 0
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        End If

    End Sub

    Private Sub msk_date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_date.TextChanged
        msk_Date_LostFocus(sender, e)
    End Sub

    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_date.LostFocus
        If IsDate(msk_date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_date.Text)) >= 2000 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_date.Text)
                End If
            End If
        End If
    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_date.Text = dtp_Date.Text
            msk_date.SelectionStart = 0
        End If
    End Sub

    Private Sub btn_fromExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_fromExcel.Click
        Dim tr As SqlClient.SqlTransaction
        Dim FileName As String = ""
        Dim Sts1 As Boolean = False
        Dim Sts2 As Boolean = False

        tr = con.BeginTransaction

        Try

            OpenFileDialog1.ShowDialog()
            FileName = OpenFileDialog1.FileName

            If Not IO.File.Exists(FileName) Then
                MessageBox.Show(FileName & " File not found", "File not found...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

            Sts1 = getExcelData_ItemName(FileName, tr)
            If Sts1 = True Then
                Sts2 = getExcelData_OpeningStock(FileName, tr)
            End If
            If Sts1 = True And Sts2 = True Then
                tr.Commit()
                MessageBox.Show("Imported Sucessfully!!!", "FOR IMPORTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            Else
                tr.Rollback()
                MessageBox.Show("Error on Import", "FOR IMPORTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If


        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT IMPORT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            tr.Dispose()

        End Try

    End Sub

    Private Function getExcelData_ItemName(ByVal FileName As String, ByVal tr As SqlClient.SqlTransaction) As Boolean
        Dim cmd As New SqlClient.SqlCommand

        Dim xlApp As Excel.Application
        Dim xlWorkBook As Excel.Workbook
        Dim xlWorkSheet As Excel.Worksheet

        Dim RowCnt As Long = 0

        Dim Sts As Boolean = False

        Dim I As Integer = 0
        Dim vCloth_Idno As Integer = 0
        Dim vCloth_Name As String = ""
        Dim vSur_Name As String = ""

        Sts = False

        xlApp = New Excel.Application
        xlWorkBook = xlApp.Workbooks.Open(FileName)
        xlWorkSheet = xlWorkBook.Worksheets("sheet1")

        Try
            With xlWorkSheet
                RowCnt = .Range("A" & .Rows.Count).End(Excel.XlDirection.xlUp).Row
            End With

            If RowCnt <= 1 Then
                MessageBox.Show("Data not found", "Data not found...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Return Sts
                Exit Function
            End If

            cmd.Connection = con
            cmd.Transaction = tr

            For I = 2 To RowCnt

                vCloth_Name = Trim(xlWorkSheet.Cells(I, 2).value)

                vCloth_Name = Replace(vCloth_Name, "'", "`")

                If Trim(vCloth_Name) = "" Then Continue For

                vSur_Name = Common_Procedures.Remove_NonCharacters(Trim(vCloth_Name))

                vCloth_Idno = Val(Common_Procedures.Cloth_NameToIdNo(con, Trim(vCloth_Name), tr))
                If vCloth_Idno <> 0 Then
                    Continue For
                Else
                    vCloth_Idno = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "cloth_idno", "(Sur_Name = '" & Trim(vSur_Name) & "')", , tr))
                    If vCloth_Idno <> 0 Then Continue For
                End If


                vCloth_Idno = Common_Procedures.get_MaxIdNo(con, "cloth_head", "cloth_idno", "", tr)

                cmd.CommandText = "Insert into Cloth_Head  (             Cloth_Idno      ,            Cloth_Name      ,         Sur_Name         ,  Cloth_Description , Cloth_WarpCount_IdNo  , Cloth_WeftCount_IdNo , Cloth_ReedSpace , Cloth_Reed , Cloth_Pick , Cloth_Width , Weight_Meter_Warp , Weight_Meter_Weft , Beam_Length , Tape_Length , Crimp_Percentage , Wages_For_Type1 , Wages_For_Type2 , Wages_For_Type3 , Wages_For_Type4  , Wages_For_Type5  , Stock_In , Meters_Pcs , ActualCloth_Pick , ActualWeight_Meter_Weft , ActualCrimp_Percentage  , Cloth_StockUnder_IdNo , Sound_Rate  , Seconds_Rate , Bits_Rate  , Other_Rate , Reject_Rate  , Allow_Shortage_Perc , Cloth_Type,   Weave , Article_IdNo  , EndsCount_IdNo , Close_Status  ,   Transfer_To_ClothIdno  , Tamil_Name , ItemGroup_Idno    ) " & _
                                    "          Values      (" & Str(Val(vCloth_Idno)) & ", '" & Trim(vCloth_Name) & "', '" & Trim(vSur_Name) & "',    ''              ,         1             ,          1           ,         0       ,      0     ,     0      ,      0      ,         0         ,          0        ,      0      ,      0      ,        0         ,        0        ,       0         ,        0        ,        0         ,        0         ,  'PCS'   ,   0        ,       0          ,           0             ,          0              ,           0           ,      0      ,      0       ,      0     ,      0     ,       0      ,          0          ,      ''    ,    ''  ,        0      ,        0       ,         0     ,           0              ,     ''     ,            0      ) "
                cmd.ExecuteNonQuery()

            Next I

            Sts = True

        Catch ex As Exception

            Sts = False
            MessageBox.Show(ex.Message, "FOR MASTER CREATION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Finally

            xlWorkBook.Close(False, FileName)
            xlApp.Quit()

            ReleaseComObject(xlWorkSheet)
            ReleaseComObject(xlWorkBook)
            ReleaseComObject(xlApp)

            cmd.Dispose()

        End Try

        Return Sts

    End Function

    Private Function getExcelData_OpeningStock(ByVal FileName As String, ByVal tr As SqlClient.SqlTransaction) As Boolean
        Dim cmd As New SqlClient.SqlCommand

        Dim xlApp As Excel.Application
        Dim xlWorkBook As Excel.Workbook
        Dim xlWorkSheet As Excel.Worksheet

        Dim RowCnt As Long = 0

        Dim Sts As Boolean = False

        Dim I As Integer = 0
        Dim vCloth_Idno As Integer = 0
        Dim vCloth_Name As String = ""
        Dim vSur_Name As String = ""

        Dim OpYrCode As String = ""

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable

        Dim NewCode As String = ""
        Dim Del_Code As String = ""
        Dim RefNo As String = ""
        Dim Prev_RefCode As String = ""
        Dim vPrev_CloIdno As Integer = 0
        Dim Nr As Integer = 0
        Dim SNo As Integer = 0

        Sts = False

        xlApp = New Excel.Application
        xlWorkBook = xlApp.Workbooks.Open(FileName)
        xlWorkSheet = xlWorkBook.Worksheets("sheet1")

        Try
            With xlWorkSheet
                RowCnt = .Range("A" & .Rows.Count).End(Excel.XlDirection.xlUp).Row
            End With

            If RowCnt <= 1 Then
                MessageBox.Show("Data not found", "Data not found...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Return Sts
                Exit Function
            End If

            cmd.Connection = con
            cmd.Transaction = tr

            OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
            OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

            Dim opDate As Date '= "31-12-" & Microsoft.VisualBasic.Left(OpYrCode, 2)

            opDate = #3/31/2017#

            cmd.CommandText = "Truncate table EntryTemp"
            cmd.ExecuteNonQuery()

            Prev_RefCode = ""

            For I = 2 To RowCnt

                vCloth_Name = Trim(xlWorkSheet.Cells(I, 2).value)
                vCloth_Name = Replace(vCloth_Name, "'", "`")
                If Trim(vCloth_Name) = "" Then Continue For

                vSur_Name = Common_Procedures.Remove_NonCharacters(Trim(vCloth_Name))

                vCloth_Idno = Val(Common_Procedures.Cloth_NameToIdNo(con, Trim(vCloth_Name), tr))
                If vCloth_Idno = 0 Then
                    vCloth_Idno = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "cloth_idno", "(Sur_Name = '" & Trim(vSur_Name) & "')", , tr))
                    If vCloth_Idno = 0 Then Continue For
                End If

                cmd.CommandText = "Insert into EntryTemp  (                Name1      ,            Int1              ,         Name2            ,                     Meters1               ,                     Meters2                ,                     Meters3                ,                      Meters4                                                     ) " & _
                                    "          Values     ('" & Trim(vCloth_Name) & "', " & Str(Val(vCloth_Idno)) & ", '" & Trim(vSur_Name) & "', " & Val(xlWorkSheet.Cells(I, 3).value) & ", " & Val(xlWorkSheet.Cells(I, 4).value) & " , " & Val(xlWorkSheet.Cells(I, 5).value) & " ,  " & Val(xlWorkSheet.Cells(I, 4).value) * Val(xlWorkSheet.Cells(I, 5).value) & " ) "
                cmd.ExecuteNonQuery()

            Next I

            cmd.CommandText = "delete from Fabric_Bundle_Entry_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Bundle_Entry_Code LIKE '%/" & Trim(vEntFnYrCode) & "' and Fabric_Bundle_Entry_Code LIKE '" & Trim(Pk_Condition) & "%'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Fabric_Bundle_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Fabric_Bundle_Entry_Code LIKE '%/" & Trim(vEntFnYrCode) & "' and Fabric_Bundle_Entry_Code LIKE '" & Trim(Pk_Condition) & "%'"
            cmd.ExecuteNonQuery()

            da = New SqlClient.SqlDataAdapter("select * from EntryTemp Where Name1 <> '' Order by Name1, Meters1", con)
            If IsNothing(tr) = False Then
                da.SelectCommand.Transaction = tr
            End If
            dt1 = New DataTable
            da.Fill(dt1)

            Prev_RefCode = ""
            vPrev_CloIdno = 0
            If dt1.Rows.Count > 0 Then

                For I = 0 To dt1.Rows.Count - 1

                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(opDate))

                    Prev_RefCode = Trim(Common_Procedures.get_FieldValue(con, "Fabric_Bundle_Entry_Details", "Fabric_Bundle_Entry_Code", "(Cloth_IdNo = " & Val(dt1.Rows(I).Item("Int1").ToString) & ")", lbl_Company.Tag, tr))

                    SNo = 0
                    If Trim(Prev_RefCode) <> "" Then

                        NewCode = Prev_RefCode

                        RefNo = Trim(Common_Procedures.get_FieldValue(con, "Fabric_Bundle_Entry_Head", "Fabric_Bundle_Entry_No", "(Fabric_Bundle_Entry_Code = '" & Trim(NewCode) & "')", , tr))

                        da = New SqlClient.SqlDataAdapter("select max(Sl_No) from Fabric_Bundle_Entry_Details Where Fabric_Bundle_Entry_Code = '" & Trim(NewCode) & "'", con)
                        If IsNothing(tr) = False Then
                            da.SelectCommand.Transaction = tr
                        End If
                        dt2 = New DataTable
                        da.Fill(dt2)

                        If dt2.Rows.Count > 0 Then
                            If IsDBNull(dt2.Rows(0)(0).ToString) = False Then
                                SNo = Val(dt2.Rows(0)(0).ToString)
                            End If
                        End If
                        dt2.Clear()


                    Else

                        RefNo = Common_Procedures.get_MaxCode(con, "Fabric_Bundle_Entry_Head", "Fabric_Bundle_Entry_Code", "For_OrderBy", "(Fabric_Bundle_Entry_Code LIKE '" & Trim(Pk_Condition) & "%')", Val(lbl_Company.Tag), vEntFnYrCode, tr)
                        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(RefNo) & "/" & Trim(vEntFnYrCode)

                        cmd.CommandText = "Insert into Fabric_Bundle_Entry_Head (  Fabric_Bundle_Entry_Code  ,                 Company_IdNo     ,  Fabric_Bundle_Entry_No  ,                               for_OrderBy                     , Fabric_Bundle_Entry_Date  , Total_Bundles ,  Total_Pcs , Total_Meters  ,                                User_idNo       ) " & _
                                            "          Values                   ('" & Trim(NewCode) & "'     , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(RefNo) & "'    , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(RefNo))) & ",      @EntryDate           ,     0         ,      0     ,     0         ,  " & Str(Val(Common_Procedures.User.IdNo)) & " ) "
                        cmd.ExecuteNonQuery()

                    End If


                    SNo = SNo + 1
                    cmd.CommandText = "Insert into Fabric_Bundle_Entry_Details ( Fabric_Bundle_Entry_Code,              Company_IdNo        ,    Fabric_Bundle_Entry_No ,                               for_OrderBy                              , Fabric_Bundle_Entry_Date   ,            Sl_No     ,                         Cloth_IdNo                 ,                        Mark                          ,                        Noof_Bundles                  ,                        Pcs_Per_Bundle                ,                        Total_Pcs                     ,                         Total_Meters                                                              ) " & _
                                        "          Values                      ('" & Trim(NewCode) & "'  , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(RefNo) & "'     , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",       @EntryDate           , " & Str(Val(SNo)) & ",  " & Str(Val(dt1.Rows(I).Item("Int1").ToString)) & ", " & Str(Val(dt1.Rows(I).Item("Meters1").ToString)) & ", " & Str(Val(dt1.Rows(I).Item("Meters2").ToString)) & ", " & Str(Val(dt1.Rows(I).Item("Meters3").ToString)) & ", " & Str(Val(dt1.Rows(I).Item("Meters4").ToString)) & ",  " & Str(Val(dt1.Rows(I).Item("Meters1").ToString) * Val(dt1.Rows(I).Item("Meters4").ToString)) & " ) "
                    cmd.ExecuteNonQuery()


                    cmd.CommandText = "Update Fabric_Bundle_Entry_Head Set Total_Bundles = Total_Bundles  + " & Str(Val(dt1.Rows(I).Item("Meters2").ToString)) & " , Total_Pcs = Total_Pcs + " & Str(Val(dt1.Rows(I).Item("Meters4").ToString)) & ",  Total_Meters = Total_Meters +  (" & Str(Val(dt1.Rows(I).Item("Meters1").ToString) * Val(dt1.Rows(I).Item("Meters4").ToString)) & ")  Where Fabric_Bundle_Entry_Code  = '" & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()

                Next

            End If

            dt1.Clear()
            dt1.Dispose()
            dt2.Dispose()
            da.Dispose()


            Sts = True

        Catch ex As Exception

            Sts = False
            MessageBox.Show(ex.Message, "FOR OPENING STOCK POSTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Finally

            xlWorkBook.Close(False, FileName)
            xlApp.Quit()

            ReleaseComObject(xlWorkSheet)
            ReleaseComObject(xlWorkBook)
            ReleaseComObject(xlApp)

            cmd.Dispose()

        End Try

        Return Sts

    End Function

    Private Sub ReleaseComObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        End Try
    End Sub

    Private Sub btn_SaveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SaveAll.Click
        Dim pwd As String = ""

        Dim g As New Password
        g.ShowDialog()

        pwd = Trim(Common_Procedures.Password_Input)

        If Trim(UCase(pwd)) <> "TSSA7417" Then
            MessageBox.Show("Invalid Password", "FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        SaveAll_STS = True

        LastNo = ""
        movelast_record()

        LastNo = lbl_RefNo.Text

        movefirst_record()
        Timer1.Enabled = True
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_RefNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub

   
End Class
