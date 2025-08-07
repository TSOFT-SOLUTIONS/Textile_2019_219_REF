Public Class OE_Stock_Transfer_Entry

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "STKTR-"
    Private Pk_Condition1 As String = "TRSTK-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String
    Private WithEvents dgtxt_PavuDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_YarnDetails As New DataGridViewTextBoxEditingControl
    Private dgv_ActiveCtrl_Name As String

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Pack_Selection.Visible = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        cbo_GodownFrom.Text = ""
        Cbo_CountFrom.Text = ""
        cbo_CoNETYPEFrom.Text = ""
        cbo_GodownTo.Text = ""
        Cbo_CountTo.Text = ""
        cbo_CoNETYPETo.Text = ""


        cbo_Filter_CountName.Text = ""
        cbo_Filter_PartyName.Text = ""
        cbo_Filter_Colour.Text = ""

        dgv_Details.Rows.Clear()
        dgv_Details.Rows.Add()

        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_Colour.Text = ""
            cbo_Filter_Colour.SelectedIndex = -1
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        cbo_CoNETYPEFrom.Enabled = True
        cbo_CoNETYPEFrom.BackColor = Color.White

        Cbo_CountFrom.Enabled = True
        Cbo_CountFrom.BackColor = Color.White

        cbo_CoNETYPETo.Enabled = True
        cbo_CoNETYPETo.BackColor = Color.White

        Cbo_CountTo.Enabled = True
        Cbo_CountTo.BackColor = Color.White

        cbo_GodownFrom.Enabled = True
        cbo_GodownFrom.BackColor = Color.White

        cbo_GodownTo.Enabled = True
        cbo_GodownTo.BackColor = Color.White

        btn_Pack_Selection.Enabled = True
        ' dgv_PavuDetails.ReadOnly = False

        Grid_Cell_DeSelect()

        NoCalc_Status = False
        dgv_ActiveCtrl_Name = ""

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim txtbx As TextBox
        Dim combobx As ComboBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
            Me.ActiveControl.BackColor = Color.Lime
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
    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
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

    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        On Error Resume Next
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub Stock_Transfer_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated


        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_GodownFrom.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_GodownFrom.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_CoNETYPEFrom.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CONETYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_CoNETYPEFrom.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_CountFrom.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                Cbo_CountFrom.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Stock_Transfer_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Stock_Transfer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Pack_Selection.Visible = True Then
                    btn_Pack_Close_Selection_Click(sender, e)
                    Exit Sub
                Else
                    Close_Form()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Stock_Transfer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        Me.Text = ""

        con.Open()

        dtp_Date.Text = ""

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Pack_Selection.Visible = False
        pnl_Pack_Selection.Left = (Me.Width - pnl_Pack_Selection.Width) \ 2
        pnl_Pack_Selection.Top = (Me.Height - pnl_Pack_Selection.Height) \ 2
        pnl_Pack_Selection.BringToFront()


        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_GodownFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_GodownTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_CoNETYPEFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_CountFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_CoNETYPETo.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_CountTo.GotFocus, AddressOf ControlGotFocus


        AddHandler cbo_Filter_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Colour.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_GodownFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_GodownTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_CoNETYPEFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_CountFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_CoNETYPETo.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_CountTo.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Colour.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()
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

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim LockSTS As Boolean = False
        Dim n As Integer = 0
        Dim I As Integer = 0, J As Integer = 0
        Dim SNo As Integer

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Stock_Transfer_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Stock_Transfer_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Stock_Transfer_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Stock_Transfer_Date").ToString

                cbo_GodownFrom.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("LedgerFrom_IdNo").ToString))
                cbo_GodownTo.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("LedgerTo_IdNo").ToString))
                cbo_CoNETYPEFrom.Text = Common_Procedures.Conetype_IdNoToName(con, Val(dt1.Rows(0).Item("ConeTypeFrom_IdNo").ToString))
                Cbo_CountFrom.Text = Common_Procedures.Count_IdNoToName(con, Val(dt1.Rows(0).Item("CountFrom_IdNo").ToString))
                cbo_CoNETYPETo.Text = Common_Procedures.Conetype_IdNoToName(con, Val(dt1.Rows(0).Item("ConeTypeTo_IdNo").ToString))
                Cbo_CountTo.Text = Common_Procedures.Count_IdNoToName(con, Val(dt1.Rows(0).Item("CountTo_IdNo").ToString))

                da2 = New SqlClient.SqlDataAdapter("select a.* , b.Cotton_Invoice_Code as lock_Code from Stock_Transfer_Details a left outer join Cotton_Packing_Details b ON b.Cotton_Packing_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Bag_No = b.Bag_No where a.Company_IdNo =  " & Str(Val(lbl_Company.Tag)) & " and a.Stock_Transfer_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For I = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(I).Item("Bag_No").ToString
                        dgv_Details.Rows(n).Cells(2).Value = Format(Val(dt2.Rows(I).Item("Gross_Weight").ToString), "########0.000")
                        dgv_Details.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(I).Item("Tare_Weight").ToString), "########0.000")
                        dgv_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(I).Item("Net_Weight").ToString), "########0.000")

                        dgv_Details.Rows(n).Cells(5).Value = dt2.Rows(I).Item("Bag_Code_Old").ToString
                        dgv_Details.Rows(n).Cells(6).Value = dt2.Rows(I).Item("Cotton_Packing_Code").ToString
                        dgv_Details.Rows(n).Cells(7).Value = dt2.Rows(I).Item("Details_Slno").ToString
                        dgv_Details.Rows(n).Cells(8).Value = dt2.Rows(I).Item("lock_Code").ToString

                        If Trim(dgv_Details.Rows(n).Cells(8).Value) <> "" Then
                            For J = 0 To dgv_Details.ColumnCount - 1
                                dgv_Details.Rows(n).Cells(J).Style.BackColor = Color.LightGray
                            Next J
                            LockSTS = True
                        End If

                    Next I

                End If

                TotalPavu_Calculation()

            End If

            dt2.Clear()

            If LockSTS = True Then

                cbo_CoNETYPEFrom.Enabled = False
                cbo_CoNETYPEFrom.BackColor = Color.LightGray

                Cbo_CountFrom.Enabled = False
                Cbo_CountFrom.BackColor = Color.LightGray

                cbo_CoNETYPETo.Enabled = False
                cbo_CoNETYPETo.BackColor = Color.LightGray

                Cbo_CountTo.Enabled = False
                Cbo_CountTo.BackColor = Color.LightGray

                cbo_GodownFrom.Enabled = False
                cbo_GodownFrom.BackColor = Color.LightGray

                cbo_GodownTo.Enabled = False
                cbo_GodownTo.BackColor = Color.LightGray


                btn_Pack_Selection.Enabled = False
                'dgv_PavuDetails.ReadOnly = True

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

            Grid_Cell_DeSelect()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.OEENTRY_STOCK_TRANSFER_ENTRY, New_Entry, Me, con, "Stock_Transfer_Head", "Stock_Transfer_Code", NewCode, "Stock_Transfer_Date", "(Stock_Transfer_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select * from Cotton_Packing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Packing_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and  Cotton_invoice_Code <> ''", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Cotton_invoice_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("Cotton_invoice_Code").ToString) <> "" Then
                    MessageBox.Show("Already Bag Delivery ", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        Dt1.Dispose()
        Da.Dispose()
        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "Update Cotton_Packing_Details set Cotton_Invoice_Code = '', Cotton_Invoice_Increment = Cotton_Invoice_Increment - 1  Where Cotton_Invoice_Code =  '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Cotton_Packing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Packing_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition1) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Stock_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Stock_Transfer_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Stock_Transfer_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Stock_Transfer_Code = '" & Trim(NewCode) & "'"
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

            If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""


            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 Stock_Transfer_No from Stock_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Stock_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Stock_Transfer_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Stock_Transfer_No from Stock_Transfer_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Stock_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Stock_Transfer_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Stock_Transfer_No from Stock_Transfer_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Stock_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Stock_Transfer_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Stock_Transfer_No from Stock_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Stock_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Stock_Transfer_No desc", con)
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

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Stock_Transfer_Head", "Stock_Transfer_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        dt.Dispose()
        da.Dispose()

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView
        Dim i As Integer

        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing
            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            ElseIf dgv_ActiveCtrl_Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = True Then
                Return MyBase.ProcessCmdKey(msg, keyData)
                Exit Function
            End If

            With dgv1

                If dgv1.Name = dgv_Details.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 5 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then

                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                    save_record()
                                Else
                                    dtp_Date.Focus()
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(2)

                            End If

                        Else

                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)


                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                cbo_CoNETYPEFrom.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 5)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If

                End If

            End With

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Bag.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Stock_Transfer_No from Stock_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Stock_Transfer_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Bag No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Stock_Transfer, "~L~") = 0 And InStr(Common_Procedures.UR.Stock_Transfer, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.OEENTRY_STOCK_TRANSFER_ENTRY, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Bag No.", "FOR NEW BAG NO INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Stock_Transfer_No from Stock_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Stock_Transfer_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Bag No", "DOES NOT INSERT NEW BAG NO...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW BAG NO...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Pak_ID As Integer = 0
        Dim LedFrm_ID As Integer = 0
        Dim LedTo_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim CTyFrm_idno As Integer = 0
        Dim Cntfrm_ID As Integer = 0
        Dim CTyto_idno As Integer = 0
        Dim Cntto_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Nr As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim YCnt_ID As Integer = 0
        Dim YMil_ID As Integer = 0
        Dim EntID As String = ""
        Dim vTotBags As Single = 0
        Dim vTotGrsWgt As Single = 0
        Dim vTotTareWgt As Single = 0
        Dim vTotNetWgt As Single = 0
        Dim Dup_SetNoBmNo As String = ""
        Dim Bag_Code As String = ""

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.OEENTRY_STOCK_TRANSFER_ENTRY, New_Entry, Me, con, "Stock_Transfer_Head", "Stock_Transfer_Code", NewCode, "Stock_Transfer_Date", "(Stock_Transfer_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Stock_Transfer_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Stock_Transfer_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        If Pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        LedFrm_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_GodownFrom.Text)
        If Val(LedFrm_ID) = 0 Then
            MessageBox.Show("Invalid Godown From Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_GodownFrom.Enabled And cbo_GodownFrom.Visible Then cbo_GodownFrom.Focus()
            Exit Sub
        End If

        LedTo_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_GodownTo.Text)
        If Val(LedTo_ID) = 0 Then
            MessageBox.Show("Invalid Godown To Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_GodownTo.Enabled And cbo_GodownTo.Visible Then cbo_GodownTo.Focus()
            Exit Sub
        End If

        Cntfrm_ID = Common_Procedures.Count_NameToIdNo(con, Cbo_CountFrom.Text)
        If Val(Cntfrm_ID) = 0 Then
            MessageBox.Show("Invalid COUNT From", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If Cbo_CountFrom.Enabled And Cbo_CountFrom.Visible Then Cbo_CountFrom.Focus()
            Exit Sub
        End If

        Cntto_ID = Common_Procedures.Count_NameToIdNo(con, Cbo_CountTo.Text)
        If Val(Cntto_ID) = 0 Then
            MessageBox.Show("Invalid COUNT To", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If Cbo_CountTo.Enabled And Cbo_CountTo.Visible Then Cbo_CountTo.Focus()
            Exit Sub
        End If

        CTyFrm_idno = Common_Procedures.ConeType_NameToIdNo(con, cbo_CoNETYPEFrom.Text)
        If Val(CTyFrm_idno) = 0 Then
            MessageBox.Show("Invalid Cone Type From", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_CoNETYPEFrom.Enabled And cbo_CoNETYPEFrom.Visible Then cbo_CoNETYPEFrom.Focus()
            Exit Sub
        End If

        CTyto_idno = Common_Procedures.ConeType_NameToIdNo(con, cbo_CoNETYPETo.Text)
        If Val(CTyto_idno) = 0 Then
            MessageBox.Show("Invalid Cone Type To", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_CoNETYPETo.Enabled And cbo_CoNETYPETo.Visible Then cbo_CoNETYPETo.Focus()
            Exit Sub
        End If


        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(3).Value) > 0 Then

                    If Trim(.Rows(i).Cells(1).Value) = "" Then
                        MessageBox.Show("Invalid Bag No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If


                    If InStr(1, Trim(UCase(Dup_SetNoBmNo)), "~" & Trim(UCase(.Rows(i).Cells(1).Value)) & "~") > 0 Then
                        MessageBox.Show("Duplicate BagNo ", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    Dup_SetNoBmNo = Trim(Dup_SetNoBmNo) & "~" & Trim(UCase(.Rows(i).Cells(1).Value)) & "~"

                End If

            Next i
        End With

        TotalPavu_Calculation()

        vTotBags = 0 : vTotGrsWgt = 0 : vTotNetWgt = 0 : vTotTareWgt = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotBags = Val(dgv_Details_Total.Rows(0).Cells(1).Value())
            vTotGrsWgt = Val(dgv_Details_Total.Rows(0).Cells(2).Value())
            vTotTareWgt = Val(dgv_Details_Total.Rows(0).Cells(3).Value())
            vTotNetWgt = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
        End If


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Stock_Transfer_Head", "Stock_Transfer_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@PakDate", dtp_Date.Value.Date)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Stock_Transfer_Head(Stock_Transfer_Code, Company_IdNo, Stock_Transfer_No, for_OrderBy, Stock_Transfer_Date, LedgerFrom_IdNo , LedgerTo_IdNo , CountFrom_Idno , CountTo_Idno  ,ConeTypeFrom_IdNo , ConeTypeTo_IdNo ,  Total_Gross_Weight, Total_Tare_Weight , Total_Net_Weight , Total_Bags  ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @PakDate, " & Str(Val(LedFrm_ID)) & ", " & Str(Val(LedTo_ID)) & ",   " & Str(Val(Cntfrm_ID)) & "," & Str(Val(Cntto_ID)) & " ," & Str(Val(CTyFrm_idno)) & "," & Str(Val(CTyto_idno)) & ",  " & Str(Val(vTotGrsWgt)) & "  ,   " & Str(Val(vTotTareWgt)) & " , " & Str(Val(vTotNetWgt)) & " ," & Str(Val(vTotBags)) & " )"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Stock_Transfer_Head set Stock_Transfer_Date = @PakDate, LedgerFrom_IdNo = " & Str(Val(LedFrm_ID)) & ",LedgerTo_IdNo = " & Str(Val(LedTo_ID)) & ", CountFrom_Idno = " & Str(Val(Cntfrm_ID)) & " ,CountTo_Idno = " & Str(Val(Cntto_ID)) & ", ConeTypeFrom_IdNo = " & Str(Val(CTyFrm_idno)) & " ,ConeTypeTo_IdNo = " & Str(Val(CTyto_idno)) & ",  Total_Bags =" & Str(Val(vTotBags)) & " , Total_Gross_Weight = " & Str(Val(vTotGrsWgt)) & "  , Total_Tare_Weight = " & Str(Val(vTotTareWgt)) & "  , Total_Net_Weight = " & Str(Val(vTotNetWgt)) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Stock_Transfer_Code = '" & Trim(NewCode) & "' "
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Cotton_Packing_Details set Cotton_Invoice_Code = '', Cotton_Invoice_Increment = Cotton_Invoice_Increment - 1  Where Cotton_Invoice_Code =  '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition1) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Transfer_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Stock_Transfer_Code = '" & Trim(NewCode) & "' and  Cotton_invoice_Code = '' "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Cotton_Packing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Packing_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and  Cotton_invoice_Code = '' "
            cmd.ExecuteNonQuery()

            With dgv_Details
                Sno = 0
                For i = 0 To dgv_Details.RowCount - 1

                    If Trim(dgv_Details.Rows(i).Cells(1).Value) <> "" Then

                        Sno = Sno + 1

                        Bag_Code = ""
                        Bag_Code = Trim(Pk_Condition) & Trim(.Rows(i).Cells(5).Value)

                        Nr = 0
                        cmd.CommandText = "update Stock_Transfer_Details set Stock_Transfer_Date = @PakDate, Sl_No = " & Str(Val(Sno)) & ", Ledger_IdNo =" & Str(Val(LedTo_ID)) & " , Count_IdNo =" & Str(Val(Cntto_ID)) & "  ,  ConeType_IdNo   =" & Str(Val(CTyto_idno)) & "     , Bag_No = '" & Trim(Val(.Rows(i).Cells(1).Value)) & "',  Gross_Weight = " & Val(.Rows(i).Cells(2).Value) & ",Tare_Weight = " & Val(.Rows(i).Cells(3).Value) & ",Net_Weight = " & Val(.Rows(i).Cells(4).Value) & " where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Stock_Transfer_Code =  '" & Trim(NewCode) & "' and Details_Slno =  " & Str(Val(.Rows(i).Cells(7).Value)) & ""
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then

                            cmd.CommandText = "Insert into Stock_Transfer_Details( Stock_Transfer_Code       , Company_IdNo   ,               Stock_Transfer_No      , for_OrderBy       ,           Stock_Transfer_Date      ,           Sl_No ,         Ledger_IdNo      ,       Count_IdNo            ,            ConeType_IdNo                            ,       Bag_No                   ,                    Bag_Code   ,   Gross_Weight              ,  Tare_Weight                            , Net_Weight                            ,       Bag_Code_Old                      ,  Cotton_Packing_Code ) " &
                                                                " Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @PakDate, " & Str(Val(Sno)) & " , " & Str(Val(Pak_ID)) & ",  " & Str(Val(Cntto_ID)) & "," & Str(Val(CTyto_idno)) & ",  '" & Trim(.Rows(i).Cells(1).Value) & "' , '" & Trim(Bag_Code) & "' , " & Str(Val(.Rows(i).Cells(2).Value)) & "  ,  " & Str(Val(.Rows(i).Cells(3).Value)) & " , " & Str(Val(.Rows(i).Cells(4).Value)) & " , '" & Trim(.Rows(i).Cells(5).Value) & "' , '" & Trim(.Rows(i).Cells(6).Value) & "'  )"
                            cmd.ExecuteNonQuery()

                        End If

                        Nr = 0
                        cmd.CommandText = "Update Cotton_Packing_Details set Cotton_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' , Cotton_Invoice_Increment = Cotton_Invoice_Increment + 1 Where Bag_Code = '" & Trim(.Rows(i).Cells(5).Value) & "' AND  Cotton_packing_Code= '" & Trim(.Rows(i).Cells(6).Value) & "' and Count_IdNo = " & Val(Cntfrm_ID) & " and ConeType_idNo = " & Val(CTyFrm_idno) & ""
                        Nr = cmd.ExecuteNonQuery()
                        If Nr = 0 Then
                            Throw New ApplicationException("Mismatch of Cone Or ConeType Details")
                            Exit Sub
                        End If

                        Nr = 0
                        cmd.CommandText = "update Cotton_Packing_Details set Cotton_Packing_Date = @PakDate, Sl_No = " & Str(Val(Sno)) & ", Ledger_IdNo =" & Str(Val(Pak_ID)) & " , Count_IdNo =" & Str(Val(Cntto_ID)) & "  ,  ConeType_IdNo   =" & Str(Val(CTyto_idno)) & "     , Bag_No = '" & Trim(Val(.Rows(i).Cells(1).Value)) & "',  Gross_Weight = " & Val(.Rows(i).Cells(2).Value) & ",Tare_Weight = " & Val(.Rows(i).Cells(3).Value) & ", Net_Weight = " & Val(.Rows(i).Cells(4).Value) & " , StockAt_IdNo = " & Str(Val(LedTo_ID)) & " where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Packing_Code =  '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Bag_Code = '" & Trim(Bag_Code) & "' "
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then

                            cmd.CommandText = "Insert into Cotton_Packing_Details( Cotton_Packing_Code       , Company_IdNo   ,               Cotton_Packing_No      , for_OrderBy       ,           Cotton_Packing_Date      ,           Sl_No ,         Ledger_IdNo      ,       Count_IdNo   ,        ConeType_IdNo                       ,       Bag_No                   ,          Bag_Code  ,  Gross_Weight              ,  Tare_Weight                    , Net_Weight    , StockAt_IdNo   ) " &
                                                                " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @PakDate, " & Str(Val(Sno)) & " , " & Str(Val(Pak_ID)) & ",  " & Str(Val(Cntto_ID)) & "," & Str(Val(CTyto_idno)) & ",  '" & Trim(.Rows(i).Cells(1).Value) & "' , '" & Trim(Bag_Code) & "' , " & Str(Val(.Rows(i).Cells(2).Value)) & "  ,  " & Str(Val(.Rows(i).Cells(3).Value)) & " , " & Str(Val(.Rows(i).Cells(4).Value)) & " , " & Str(Val(LedTo_ID)) & " )"
                            cmd.ExecuteNonQuery()

                        End If

                        Partcls = "Yarn Transfer: " & Trim(.Rows(i).Cells(1).Value)
                        PBlNo = Trim(lbl_RefNo.Text)
                        EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)

                        cmd.CommandText = "Insert into Stock_Yarn_Processing_Details          (SoftwareType_IdNo                                         , Reference_Code                        ,             Company_IdNo                 ,    Reference_No        ,                               For_OrderBy               ,            Reference_Date,         Entry_ID   ,        Party_Bill_No   ,      Sl_No      ,            Count_idNo   ,         ConeType_IdNo  ,             Bags,                     Bag_No                      ,   Weight          ,                    StockAt_IdNo   ) " &
                                                                  "   Values  (" & Str(Val(Common_Procedures.SoftwareTypes.OE_Software)) & " , '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",    @PakDate   , '" & Trim(EntID) & "' ,'" & Trim(PBlNo) & "'," & Str(Val(Sno)) & ", " & Str(Val(Cntto_ID)) & "," & Str(Val(CTyto_idno)) & ",  1  ,   '" & Trim(.Rows(i).Cells(1).Value) & "' , " & Str(Val(.Rows(i).Cells(4).Value)) & "," & Str(Val(LedTo_ID)) & "  )"
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With

            ' Partcls = "Pack : Bag.No. " & Trim(.Rows(i).Cells(1).Value)
            PBlNo = "Yarn Transfer:" & Trim(lbl_RefNo.Text)
            EntID = Trim(Pk_Condition1) & Trim(lbl_RefNo.Text)

            cmd.CommandText = "Insert into Stock_Yarn_Processing_Details (               SoftwareType_IdNo  ,                                    Reference_Code                        ,           Company_IdNo                 ,    Reference_No        ,                               For_OrderBy               ,             Reference_Date,          Entry_ID   ,      Party_Bill_No   ,          Sl_No      ,            Count_idNo   ,          ConeType_IdNo  ,          Bags,     Bag_No                     ,                       Weight  ,               StockAt_IdNo  ) " &
                                                      "   Values  (" & Str(Val(Common_Procedures.SoftwareTypes.OE_Software)) & " , '" & Trim(Pk_Condition1) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",    @PakDate   , '" & Trim(EntID) & "' ,'" & Trim(PBlNo) & "'," & Str(Val(Sno)) & ", " & Str(Val(Cntfrm_ID)) & "," & Str(Val(CTyFrm_idno)) & ",  " & Str(-1 * Val(vTotBags)) & "  ,   '' , " & Str(-1 * Val(vTotNetWgt)) & "  , " & Str(Val(LedFrm_ID)) & ")"
            cmd.ExecuteNonQuery()

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
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

            If InStr(1, ex.Message, "IX_Stock_Transfer_Details") > 0 Then
                MessageBox.Show("Duplicate BagNo", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If



        Finally
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub

    Private Sub cbo_GodownFrom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_GodownFrom.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'GODOWN' and  AccountsGroup_IdNo = 9)", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_GodownFrom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GodownFrom.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_GodownFrom, dtp_Date, cbo_GodownTo, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'GODOWN' and  AccountsGroup_IdNo = 9)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_GodownFrom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_GodownFrom.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_GodownFrom, cbo_GodownTo, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'GODOWN' and  AccountsGroup_IdNo = 9)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_GodownFrom_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GodownFrom.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "GODOWN"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_GodownFrom.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_GodownTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_GodownTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'GODOWN' and  AccountsGroup_IdNo = 9)", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_GodownTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GodownTo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_GodownTo, dtp_Date, Cbo_CountFrom, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'GODOWN' and  AccountsGroup_IdNo = 9)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_GodownTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_GodownTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_GodownTo, Cbo_CountFrom, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'GODOWN' and  AccountsGroup_IdNo = 9)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_GodownTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GodownTo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "GODOWN"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_GodownFrom.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_CoNETYPEto_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CoNETYPETo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ConeType_head", "ConeType_nAME", "", "(ConeType_IdNo = 0)")

    End Sub

    Private Sub cbo_CoNETYPEtor_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CoNETYPETo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_CoNETYPETo, cbo_CoNETYPEFrom, Nothing, "ConeType_head", "ConeType_nAME", "", "(ConeType_IdNo = 0)")
        If (e.KeyValue = 40 And cbo_CoNETYPETo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(2)

            Else
                btn_save.Focus()

            End If
        End If
    End Sub

    Private Sub cbo_CoNETYPEto_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CoNETYPETo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_CoNETYPETo, Nothing, "ConeType_head", "ConeType_nAME", "", "(ConeType_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to select Pack  :", "FOR PACKING SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Pack_Selection_Click(sender, e)
            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(2)

                Else
                    btn_save.Focus()

                End If
            End If
        End If
    End Sub

    Private Sub cbo_CoNETYPEto_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CoNETYPETo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New ConeType_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_CoNETYPEFrom.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CoNETYPEFrom.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ConeType_head", "ConeType_nAME", "", "(ConeType_IdNo = 0)")

    End Sub
    Private Sub cbo_Colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CoNETYPEFrom.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_CoNETYPEFrom, Cbo_CountTo, cbo_CoNETYPETo, "ConeType_head", "ConeType_nAME", "", "(ConeType_IdNo = 0)")
    End Sub

    Private Sub cbo_Colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CoNETYPEFrom.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_CoNETYPEFrom, cbo_CoNETYPETo, "ConeType_head", "ConeType_nAME", "", "(ConeType_IdNo = 0)")
    End Sub

    Private Sub cbo_Colour_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CoNETYPEFrom.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New ConeType_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_CoNETYPEFrom.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Count_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_CountFrom.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "count_Head", "count_Name", "", "(count_IdNo = 0)")

    End Sub
    Private Sub cbo_Count_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_CountFrom.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_CountFrom, cbo_GodownTo, Cbo_CountTo, "count_Head", "count_Name", "", "(count_IdNo = 0)")

    End Sub

    Private Sub cbo_Count_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_CountFrom.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_CountFrom, Cbo_CountTo, "count_Head", "count_Name", "", "(count_IdNo = 0)")
    End Sub

    Private Sub cbo_Count_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_CountFrom.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_CountFrom.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub Cbo_Countto_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_CountTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "count_Head", "count_Name", "", "(count_IdNo = 0)")
    End Sub

    Private Sub Cbo_Countto_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_CountTo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_CountTo, cbo_GodownTo, cbo_CoNETYPEFrom, "count_Head", "count_Name", "", "(count_IdNo = 0)")
    End Sub

    Private Sub Cbo_Countto_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_CountTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_CountTo, cbo_CoNETYPEFrom, "count_Head", "count_Name", "", "(count_IdNo = 0)")
    End Sub

    Private Sub Cbo_Countto_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_CountTo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_CountFrom.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
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
        Dim Pak_IdNo As Integer, Col_IdNo As Integer, Cnt_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Col_IdNo = 0
            Cnt_IdNo = 0
            Pak_IdNo = 0
            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Stock_Transfer_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Stock_Transfer_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Stock_Transfer_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Pak_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If
            If Trim(cbo_Filter_CountName.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_CountName.Text)
            End If
            If Trim(cbo_Filter_Colour.Text) <> "" Then
                Col_IdNo = Common_Procedures.ConeType_NameToIdNo(con, cbo_Filter_Colour.Text)
            End If

            If Val(Pak_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(Pak_IdNo)) & ")"
            End If
            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.count_IdNo = " & Str(Val(Cnt_IdNo)) & " )"
            End If

            If Val(Col_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.ConeType_IdNo = " & Str(Val(Col_IdNo)) & ")"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name ,c.* ,d.* from Stock_Transfer_Head a inner join Ledger_head b on a.Ledger_idno = b.Ledger_idno LEFT OUTER join Count_head c on a.Count_idno = c.Count_idno LEFT OUTER JOIN ConeType_head d on a.ConeType_IdNo = d.ConeType_IdNo where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Stock_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Stock_Transfer_Date, a.for_orderby, a.Stock_Transfer_No", con)
            'da = New SqlClient.SqlDataAdapter("select a.*, e.Ledger_Name from Weaver_Yarn_Delivery_Head a left outer join Weaver_Yarn_Delivery_Details b on a.Weaver_Yarn_Delivery_Code = b.Weaver_Yarn_Delivery_Code left outer join Ledger_head e on a.Ledger_idno = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Weaver_Yarn_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Weaver_Yarn_Delivery_Date, a.for_orderby, a.Weaver_Yarn_Delivery_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Stock_Transfer_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Stock_Transfer_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Count_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("ConeType_nAME").ToString

                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Total_Net_Weight").ToString), "########0.000")

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

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, cbo_Filter_CountName, cbo_Filter_Colour, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_Colour, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "count_Head", "count_Name", "", "(count_IdNo = 0)")

    End Sub


    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_CountName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_CountName, dtp_Filter_ToDate, cbo_Filter_PartyName, "count_Head", "count_Name", "", "(count_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_CountName, cbo_Filter_PartyName, "count_Head", "count_Name", "", "(count_IdNo = 0)")

    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(0).Value)

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
    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.OEENTRY_STOCK_TRANSFER_ENTRY, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Stock_Transfer_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Stock_Transfer_Code = '" & Trim(NewCode) & "'", con)
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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* , c.* from Stock_Transfer_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Stock_Transfer_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Endscount_name  from Stock_Transfer_Head a LEFT OUTER JOIN EndsCount_Head b ON a.Endscount_idno = b.Endscount_idno  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Stock_Transfer_Code = '" & Trim(NewCode) & "'", con)
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
        Dim p1Font As Font
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
        Dim d1 As Single


        p1Font = New Font("Calibri", 11, FontStyle.Bold)

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


        d1 = e.Graphics.MeasureString("Endscount Name   : ", pFont).Width

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(35) : ClAr(2) = 225 : ClAr(3) = 180 : ClAr(4) = 100 : ClAr(5) = 120
        ClAr(6) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5))


        TxtHgt = 19

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = NoofDets + 1


                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "Endscount Name", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + d1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("EndsCount_Name").ToString, LMargin + d1 + 30, CurY, 0, 0, p1Font)

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "Pcs", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + d1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString, LMargin + d1 + 30, CurY, 0, 0, p1Font)

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "Meters", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + d1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString, LMargin + d1 + 30, CurY, 0, 0, p1Font)

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "Ex/sht Type", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + d1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Excess_Short").ToString, LMargin + d1 + 30, CurY, 0, 0, p1Font)

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "Amount ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + d1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "#######0.00"), LMargin + d1 + 30, CurY, 0, 0, p1Font)


                NoofDets = NoofDets + 1

                prn_DetIndx = prn_DetIndx + 1

                CurY = CurY + TxtHgt + 10

                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            End If

            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)



        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim C1, N1, M1, W1 As Single

        PageNo = PageNo + 1

        CurY = TMargin


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
        Common_Procedures.Print_To_PrintDocument(e, "YARN EXCESS SHORT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3)


        Try

            N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
            W1 = e.Graphics.MeasureString("SET NO  :  ", pFont).Width

            M1 = ClAr(1) + ClAr(2) + ClAr(3)

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 11, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "TO :  M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "REF.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Stock_Transfer_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Stock_Transfer_Date").ToString), "dd-MM-yyyy"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + 380, CurY, LMargin + 380, LnAr(2))

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font


        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub cbo_Filter_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Colour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ConeType_head", "ConeType_nAME", "", "(ConeType_IdNo = 0)")

    End Sub
    Private Sub cbo_Filter_Colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Colour.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Colour, cbo_Filter_PartyName, btn_Filter_Show, "ConeType_head", "ConeType_nAME", "", "(ConeType_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_Colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Colour.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Colour, btn_Filter_Show, "ConeType_head", "ConeType_nAME", "", "(ConeType_IdNo = 0)")
    End Sub
    Private Sub TotalPavu_Calculation()
        Dim Sno As Integer
        Dim TotGrsWgt As Single, TotTareWgt As Single, TotNetWgt As Single, TotBags As Single

        If FrmLdSTS = True Then Exit Sub

        Sno = 0

        TotGrsWgt = 0
        TotTareWgt = 0
        TotNetWgt = 0
        TotBags = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Trim(.Rows(i).Cells(1).Value) <> "" Then
                    TotBags = TotBags + 1
                    TotGrsWgt = TotGrsWgt + Val(.Rows(i).Cells(2).Value)
                    TotTareWgt = TotTareWgt + Val(.Rows(i).Cells(3).Value)
                    TotNetWgt = TotNetWgt + Val(.Rows(i).Cells(4).Value)
                End If
            Next
        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(1).Value = Val(TotBags)
            .Rows(0).Cells(2).Value = Format(Val(TotGrsWgt), "########0.000")
            .Rows(0).Cells(3).Value = Format(Val(TotTareWgt), "########0.000")
            .Rows(0).Cells(4).Value = Format(Val(TotNetWgt), "########0.000")

        End With

    End Sub

    Private Sub dgv_PavuDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_PavuDetails_CellLeave(sender, e)
    End Sub

    Private Sub dgv_PavuDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        With dgv_Details
            dgv_ActiveCtrl_Name = .Name

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            'If e.RowIndex > 0 And e.ColumnIndex = 1 Then
            '    If Val(.CurrentRow.Cells(1).Value) = 0 And e.RowIndex = .RowCount - 1 Then
            '        .CurrentRow.Cells(1).Value = Val(.Rows(e.RowIndex - 1).Cells(1).Value) + 1
            '        '.CurrentRow.Cells(2).Value = .Rows(e.RowIndex - 1).Cells(2).Value
            '        '.CurrentRow.Cells(3).Value = .Rows(e.RowIndex - 1).Cells(3).Value
            '        '.Rows.Add()
            '    End If
            '    If e.ColumnIndex = 1 And e.RowIndex = .RowCount - 1 And Val(.CurrentRow.Cells(2).Value) = 0 And Val(.CurrentRow.Cells(3).Value) = 0 Then
            '        .CurrentRow.Cells(1).Value = Val(.Rows(e.RowIndex - 1).Cells(1).Value) + 1
            '        '.CurrentRow.Cells(2).Value = .Rows(e.RowIndex - 1).Cells(2).Value
            '        '.CurrentRow.Cells(3).Value = .Rows(e.RowIndex - 1).Cells(3).Value
            '        '.Rows.Add()
            '    End If
            'End If

            If e.RowIndex > 0 And e.ColumnIndex = 1 Then
                If Val(.CurrentRow.Cells(1).Value) = 0 And e.RowIndex = .RowCount - 1 Then
                    .CurrentRow.Cells(1).Value = Val(.Rows(e.RowIndex - 1).Cells(1).Value) + 1
                    .CurrentRow.Cells(2).Value = .Rows(e.RowIndex - 1).Cells(2).Value
                    .CurrentRow.Cells(3).Value = .Rows(e.RowIndex - 1).Cells(3).Value
                    .CurrentRow.Cells(4).Value = .Rows(e.RowIndex - 1).Cells(4).Value
                    '.Rows.Add()
                End If
                If e.ColumnIndex = 1 And e.RowIndex = .RowCount - 1 And Val(.CurrentRow.Cells(2).Value) = 0 And Val(.CurrentRow.Cells(3).Value) = 0 And Val(.CurrentRow.Cells(4).Value) = 0 Then
                    .CurrentRow.Cells(1).Value = Val(.Rows(e.RowIndex - 1).Cells(1).Value) + 1
                    .CurrentRow.Cells(2).Value = .Rows(e.RowIndex - 1).Cells(2).Value
                    .CurrentRow.Cells(3).Value = .Rows(e.RowIndex - 1).Cells(3).Value
                    .CurrentRow.Cells(4).Value = .Rows(e.RowIndex - 1).Cells(4).Value
                    '.Rows.Add()
                End If
            End If
        End With
    End Sub

    Private Sub dgv_PavuDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 4 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_PavuDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Try

            If FrmLdSTS = True Then Exit Sub
            ' If MovSTS = True Then Exit Sub

            With dgv_Details

                If .Visible Then

                    If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

                    If (.CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4) Then
                        'If .CurrentRow.Index = .Rows.Count - 1 Then
                        '    .Rows.Add()

                        'End If
                        If Val(.CurrentCell.ColumnIndex) = 2 Or Val(.CurrentCell.ColumnIndex) = 3 Then
                            .Rows(.CurrentCell.RowIndex).Cells(4).Value = Val(.Rows(.CurrentCell.RowIndex).Cells(2).Value) - Val(.Rows(.CurrentCell.RowIndex).Cells(3).Value)
                        End If
                        If (.CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4) Then
                            TotalPavu_Calculation()
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '----

        End Try

    End Sub


    Private Sub dgv_PavuDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_PavuDetails = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_PavuDetails_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.GotFocus
        '--
    End Sub

    Private Sub dgv_PavuDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_PavuDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                If Trim(.Rows(.CurrentRow.Index).Cells(8).Value) = "" Then

                    n = .CurrentRow.Index

                    If .Rows.Count = 1 Then
                        For i = 0 To .Columns.Count - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else

                        .Rows.RemoveAt(n)

                    End If

                    TotalPavu_Calculation()

                Else
                    MessageBox.Show("Already Bag delivered", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub

                End If

            End With

        End If

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_PavuDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer

        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub
    Private Sub dgtxt_PavuDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_PavuDetails.Enter
        dgv_ActiveCtrl_Name = dgv_Details.Name
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgv_Details.SelectAll()
    End Sub

    Private Sub dgtxt_PavuDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_PavuDetails.KeyDown
        If Trim(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(8).Value) <> "" Then
            e.SuppressKeyPress = True
            e.Handled = True
        End If
    End Sub

    Private Sub dgtxt_PavuDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_PavuDetails.KeyPress
        'If (dgv_PavuDetails.Rows(dgv_PavuDetails.CurrentCell.RowIndex).Cells(5).Value) <> "" Then
        '    e.Handled = True
        'Else
        If dgv_Details.CurrentCell.ColumnIndex = 2 Or dgv_Details.CurrentCell.ColumnIndex = 3 Then
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        End If
        ' End If
    End Sub

    Private Sub dgtxt_PavuDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_PavuDetails.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_PavuDetails_KeyUp(sender, e)
        End If
    End Sub
    Private Sub dgtxt_PavuDetails_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_PavuDetails.TextChanged
        Try
            With dgv_Details

                If .Visible Then
                    If .Rows.Count > 0 Then

                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_PavuDetails.Text)

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

    Private Sub btn_Pack_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Pack_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim Cnt_IdNo As Integer
        Dim Gdwn_IdNo As Integer
        Dim CnTy_IdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim Ent_Pcs As Single = 0
        Dim Ent_Mtrs As Single = 0, Ent_ShtMtrs As Single = 0

        Gdwn_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_GodownFrom.Text)

        If Gdwn_IdNo = 0 Then
            MessageBox.Show("Invalid Godown Name", "DOES NOT SELECT PACKING SELECTION...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_GodownFrom.Enabled And cbo_GodownFrom.Visible Then cbo_GodownFrom.Focus()
            Exit Sub
        End If

        Cnt_IdNo = Common_Procedures.Count_NameToIdNo(con, Cbo_CountFrom.Text)

        If Cnt_IdNo = 0 Then
            MessageBox.Show("Invalid Count Name", "DOES NOT SELECT PACKING SELECTION...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If Cbo_CountFrom.Enabled And Cbo_CountFrom.Visible Then Cbo_CountFrom.Focus()
            Exit Sub
        End If

        CnTy_IdNo = Common_Procedures.ConeType_NameToIdNo(con, cbo_CoNETYPEFrom.Text)

        If CnTy_IdNo = 0 Then
            MessageBox.Show("Invalid ConeType Name", "DOES NOT SELECT PACKING SELECTION...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_CoNETYPEFrom.Enabled And cbo_CoNETYPEFrom.Visible Then cbo_CoNETYPEFrom.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
            CompIDCondt = ""
        End If


        With dgv_packSelection

            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*,b.* from Cotton_Packing_Details A LEFT OUTER JOIN Cotton_Delivery_Details b ON a.Cotton_Packing_Code = b.Cotton_Packing_Code and a.Bag_Code = b.Bag_Code where a.Cotton_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Count_Idno = " & Str(Val(Cnt_IdNo)) & " and a.Conetype_Idno = " & Str(Val(CnTy_IdNo)) & " and a.StockAt_IdNo = " & Str(Val(Gdwn_IdNo)) & " order by  a.Cotton_Packing_Date, a.for_orderby , a.Cotton_Packing_No ", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Bag_No").ToString
                    .Rows(n).Cells(2).Value = Format(Val(Dt1.Rows(i).Item("Net_Weight").ToString), "#########0.000")
                    .Rows(n).Cells(3).Value = "1"
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Bag_Code").ToString
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Cotton_Packing_Code").ToString
                    .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Gross_Weight").ToString), "#########0.000")
                    .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("Tare_Weight").ToString), "#########0.000")

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            Da = New SqlClient.SqlDataAdapter("select a.* from Cotton_Packing_Details a   where a.Cotton_Invoice_Code  = '' and a.Count_Idno = " & Str(Val(Cnt_IdNo)) & " and a.Conetype_Idno = " & Str(Val(CnTy_IdNo)) & " and a.StockAt_IdNo = " & Str(Val(Gdwn_IdNo)) & " order by a.Cotton_Packing_Date, a.for_orderby ,  a.Cotton_Packing_No ", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    '.Rows(n).Cells(0).Value = Val(SNo)


                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Bag_No").ToString
                    .Rows(n).Cells(2).Value = Format(Val(Dt1.Rows(i).Item("Net_Weight").ToString), "#########0.000")
                    .Rows(n).Cells(3).Value = ""
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Bag_Code").ToString
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Cotton_packing_Code").ToString
                    .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Gross_Weight").ToString), "#########0.000")
                    .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("Tare_Weight").ToString), "#########0.000")

                Next

            End If
            Dt1.Clear()

        End With

        pnl_Pack_Selection.Visible = True
        Pnl_Back.Enabled = False
        dgv_packSelection.Focus()

    End Sub

    Private Sub dgv_Pack_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_packSelection.CellClick
        Select_PackPiece(e.RowIndex)
    End Sub

    Private Sub Select_PackPiece(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_packSelection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(3).Value = (Val(.Rows(RwIndx).Cells(3).Value) + 1) Mod 2
                If Val(.Rows(RwIndx).Cells(3).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next


                Else
                    .Rows(RwIndx).Cells(3).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next

                End If


            End If

        End With

    End Sub

    Private Sub dgv_PackSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_packSelection.KeyDown
        Dim n As Integer

        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_packSelection.CurrentCell.RowIndex >= 0 Then

                n = dgv_packSelection.CurrentCell.RowIndex

                Select_PackPiece(n)

                e.Handled = True

            End If
        End If

    End Sub

    Private Sub btn_Pack_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Pack_Close_Selection.Click
        Close_Pack_Selection()
    End Sub

    Private Sub Close_Pack_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0
        With dgv_Details
            dgv_Details.Rows.Clear()

            For i = 0 To dgv_packSelection.RowCount - 1

                If Val(dgv_packSelection.Rows(i).Cells(3).Value) = 1 Then

                    n = .Rows.Add()

                    sno = sno + 1
                    .Rows(n).Cells(0).Value = Val(sno)
                    .Rows(n).Cells(1).Value = dgv_packSelection.Rows(i).Cells(1).Value
                    .Rows(n).Cells(2).Value = dgv_packSelection.Rows(i).Cells(6).Value
                    .Rows(n).Cells(3).Value = dgv_packSelection.Rows(i).Cells(7).Value
                    .Rows(n).Cells(4).Value = dgv_packSelection.Rows(i).Cells(2).Value
                    .Rows(n).Cells(5).Value = dgv_packSelection.Rows(i).Cells(4).Value
                    .Rows(n).Cells(6).Value = dgv_packSelection.Rows(i).Cells(5).Value

                End If
                TotalPavu_Calculation()
            Next
        End With
        Pnl_Back.Enabled = True
        pnl_Pack_Selection.Visible = False

        If dgv_Details.Rows.Count > 0 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(2)

        Else
            btn_save.Focus()

        End If

    End Sub

End Class