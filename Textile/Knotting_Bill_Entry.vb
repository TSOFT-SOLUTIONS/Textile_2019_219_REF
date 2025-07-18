Public Class Knotting_Bill_Entry
    Implements Interface_MDIActions
    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "KNTBL-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String
    Private Filter_RowNo As Integer = -1
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1


    Private Sub Knotting_Bill_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Try

     
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_KnottingAc.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "SALES" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_KnottingAc.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_HSNCode.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_HSNCode.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False
    End Sub

    Private Sub Knotting_Bill_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try

            If Asc(e.KeyChar) = 27 Then
                If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub
                Else
                    Close_Form()
                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Knotting_Bill_Entry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim dt8 As New DataTable
        Dim dt9 As New DataTable


        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'AGENT') order by Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_Ledger.DataSource = dt1
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'AGENT') order by Ledger_DisplayName", con)
        da.Fill(dt2)
        cbo_KnottingAc.DataSource = dt2
        cbo_KnottingAc.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter(" Select HSN_Code from Knotting_Bill_Head ", con)
        da.Fill(dt3)
        cbo_HSNCode.DataSource = dt3
        cbo_HSNCode.DisplayMember = "Ledger_DisplayName"

        Clear()

        new_record()

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        AddHandler lbl_RefNo.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_RefDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Description.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_HSNCode.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_No_of_Beams.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CGSTPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SGSTPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_IGSTPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_KnottingAc.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_BillNo.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_HSNCode.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_FilterFrom_date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_FilterTo_date.GotFocus, AddressOf ControlGotFocus

        AddHandler lbl_RefNo.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_RefDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Description.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_HSNCode.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_No_of_Beams.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CGSTPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SGSTPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_IGSTPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_KnottingAc.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_HSNCode.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_FilterFrom_date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_FilterTo_date.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BillNo.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_Amount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CGSTPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SGSTPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_IGSTPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_No_of_Beams.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Note.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_RefDate.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Description.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_BillNo.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_Amount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CGSTPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_SGSTPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_IGSTPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_No_of_Beams.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Note.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_RefDate.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Description.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_BillNo.KeyPress, AddressOf TextBoxControlKeyPress





        FrmLdSTS = True

        con.Open()
    End Sub
    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim mskdtxbx As MaskedTextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            mskdtxbx = Me.ActiveControl
            mskdtxbx.SelectionStart = 0
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
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.DeepPink
                Prec_ActCtrl.ForeColor = Color.White
            End If
        End If
    End Sub

    Private Sub ControlLostFocus1(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.LightSkyBlue
                Prec_ActCtrl.ForeColor = Color.Blue
            End If
        End If

    End Sub

    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        On Error Resume Next
        If e.KeyValue = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Public Sub Clear()

        lbl_RefNo.Text = ""
        msk_RefDate.Text = ""
        msk_RefDate.SelectionStart = 0
        cbo_Ledger.Text = ""
        cbo_HSNCode.Text = ""
        txt_Description.Text = ""
        txt_CGSTPerc.Text = ""
        txt_SGSTPerc.Text = ""
        txt_IGSTPerc.Text = ""
        lbl_CGST_Amount.Text = ""
        lbl_SGST_Amount.Text = ""
        lbl_NetAmount.Text = ""
        lbl_TaxableValue.Text = ""
        txt_Amount.Text = ""
        cbo_KnottingAc.Text = ""
        txt_No_of_Beams.Text = ""
        txt_Note.Text = ""
        txt_BillNo.Text = ""

        msk_RefDate.Clear()
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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub
    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Yarn_Purchase_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Yarn_Purchase_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Inhouse_Knotting_Bill_Entry, New_Entry, Me, con, "Knotting_Bill_Head", "Knotting_Bill_Code", NewCode, "Knotting_Bill_Date", "(Knotting_Bill_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub







     
        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
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
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Knotting_Bill_Head", "Knotting_Bill_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Knotting_Bill_Code, Company_IdNo, for_OrderBy", trans)

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "delete from Knotting_Bill_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Knotting_Bill_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            '------------VOUCHER-----------
            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)
            '----------------------------
        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            If msk_RefDate.Enabled = True And msk_RefDate.Visible = True Then msk_RefDate.Focus()
        End Try
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where ( Ledger_IdNo = 0 or (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ) order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"


            da = New SqlClient.SqlDataAdapter("select HSN_Code from Knotting_Bill_Head order by HSN_Code", con)
            da.Fill(dt2)
            cbo_Filter_HSNCode.DataSource = dt2
            cbo_Filter_HSNCode.DisplayMember = "HSN_Code"

            dtp_FilterFrom_date.Text = Common_Procedures.Company_FromDate
            dtp_FilterTo_date.Text = Common_Procedures.Company_ToDate

            cbo_Filter_PartyName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_HSNCode.Text = ""
            cbo_Filter_HSNCode.SelectedIndex = -1
            dgv_filter.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Back.Enabled = False
        If Filter_Status = True Then
            If dgv_filter.Rows.Count > 0 And Filter_RowNo >= 0 Then
                dgv_filter.Focus()
                dgv_filter.CurrentCell = dgv_filter.Rows(Filter_RowNo).Cells(0)
                dgv_filter.CurrentCell.Selected = True
            Else
                If dtp_FilterFrom_date.Enabled And dtp_FilterFrom_date.Visible Then dtp_FilterFrom_date.Focus()

            End If

        Else
            If dtp_FilterFrom_date.Enabled And dtp_FilterFrom_date.Visible Then dtp_FilterFrom_date.Focus()

        End If
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        Try

            If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Inhouse_Knotting_Bill_Entry, New_Entry, Me) = False Then Exit Sub




           
            inpno = InputBox("Enter New Ref No.", "FOR NEW Ref NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Knotting_Bill_No from Knotting_Bill_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Knotting_Bill_Code = '" & Trim(InvCode) & "'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Ref No.", "DOES NOT INSERT NEW Ref...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW BILL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Knotting_Bill_No from Knotting_Bill_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & "AND Knotting_Bill_Code  like '" & Trim(Pk_Condition) & "%' and Knotting_Bill_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Knotting_Bill_No", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Clear()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try
    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Knotting_Bill_No from Knotting_Bill_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " AND Knotting_Bill_Code  like '" & Trim(Pk_Condition) & "%' and Knotting_Bill_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Knotting_Bill_No", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Clear()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Knotting_Bill_No from Knotting_Bill_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " AND Knotting_Bill_Code  like '" & Trim(Pk_Condition) & "%' and Knotting_Bill_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Knotting_Bill_No", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Knotting_Bill_No from Knotting_Bill_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " AND Knotting_Bill_Code  like '" & Trim(Pk_Condition) & "%' and Knotting_Bill_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Knotting_Bill_No desc", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable

        Try
            Clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Knotting_Bill_Head", "Knotting_Bill_Code", "For_OrderBy", "Knotting_Bill_Code  like '" & Trim(Pk_Condition) & "%'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_RefNo.ForeColor = Color.Red


            msk_RefDate.Text = Date.Today.ToShortDateString
            msk_RefDate.SelectionStart = 0

            Dt1.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Dt2.Dispose()
            Da1.Dispose()

            If msk_RefDate.Enabled And msk_RefDate.Visible Then msk_RefDate.Focus()
            'If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
        End Try
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        Try

            inpno = InputBox("Enter Ref No.", "FOR FINDING...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Knotting_Bill_No from Knotting_Bill_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Knotting_Bill_Code = '" & Trim(RefCode) & "'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                MessageBox.Show("Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '----
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim PkCode As String = ""
        Dim Led_ID As Integer = 0
        Dim Knot_ID As Integer = 0
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Inhouse_Knotting_Bill_Entry, New_Entry, Me, con, "Knotting_Bill_Head", "Knotting_Bill_Code", NewCode, "Knotting_Bill_Date", "(Knotting_Bill_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Knotting_Bill_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Knotting_Bill_No desc", dtp_RefDate.Value.Date) = False Then Exit Sub



      
        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        If IsDate(msk_RefDate.Text) = False Then
            MessageBox.Show("Invalid Purchase Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_RefDate.Enabled And msk_RefDate.Visible Then msk_RefDate.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_RefDate.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_RefDate.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Purchase Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_RefDate.Enabled And msk_RefDate.Visible Then msk_RefDate.Focus()
            Exit Sub
        End If


        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Val(Led_ID) = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        If Trim(lbl_NetAmount.Text) = "" Then lbl_NetAmount.Text = "0.0"
        Knot_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_KnottingAc.Text)
        If Knot_ID = 0 And Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            MessageBox.Show("Invalid knotting A/c name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_KnottingAc.Enabled And cbo_KnottingAc.Visible Then cbo_KnottingAc.Focus()
            Exit Sub
        End If
        PkCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        tr = con.BeginTransaction

        Try
            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Knotting_Bill_Head", "Knotting_Bill_Code", "For_OrderBy", "Knotting_Bill_Code  like '" & Trim(Pk_Condition) & "%'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If
            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()

            cmd.Parameters.AddWithValue("@PurchaseDate", Convert.ToDateTime(msk_RefDate.Text))
            If New_Entry = True Then
                cmd.CommandText = "Insert into Knotting_Bill_Head ( Knotting_Bill_Code , Company_IdNo                     , for_OrderBy                                                             , Knotting_Bill_No               , Knotting_Bill_Date  , Ledger_IdNo            , Knotting_AccIdNo           ,                      Amount        ,          Note            ,       Taxable_Value     ,                               CGST_Amount       ,                 SGST_Amount           ,        IGST_Amount                        ,          CGST_Perc           ,           SGST_Perc           ,       IGST_Perc            ,         HSN_Code          ,       Net_Amount, Bill_No) " &
                                                   " Values (   '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ",  " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",  '" & Trim(lbl_RefNo.Text) & "',    @PurchaseDate    , " & Str(Val(Led_ID)) & ", " & Str(Val(Knot_ID)) & ", " & Str(Val(txt_Amount.Text)) & " , '" & LTrim(txt_Note.Text) & "'," & Str(Val(lbl_TaxableValue.Text)) & "," & Str(Val(lbl_CGST_Amount.Text)) & "," & Str(Val(lbl_SGST_Amount.Text)) & "," & Str(Val(lbl_IGST_Amount.Text)) & "," & Str(Val(txt_CGSTPerc.Text)) & "," & Str(Val(txt_SGSTPerc.Text)) & "," & Str(Val(txt_IGSTPerc.Text)) & ",'" & Trim(cbo_HSNCode.Text) & "'," & Str(Val(CSng(lbl_NetAmount.Text))) & ", '" & Trim(txt_BillNo.Text) & "') "
                cmd.ExecuteNonQuery()
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            Else
                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Knotting_Bill_Head", "Knotting_Bill_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Knotting_Bill_Code, Company_IdNo, for_OrderBy", tr)

                cmd.CommandText = "Update Knotting_Bill_Head set Knotting_Bill_Date = @PurchaseDate, Ledger_IdNo = " & Str(Val(Led_ID)) & ",  Knotting_AccIdNo = " & Str(Val(Knot_ID)) & ", Amount = " & Str(Val(txt_Amount.Text)) & ", Taxable_Value=" & Str(Val(lbl_TaxableValue.Text)) & ", CGST_Amount=" & Str(Val(lbl_CGST_Amount.Text)) & ",SGST_Amount=" & Str(Val(lbl_SGST_Amount.Text)) & ",IGST_Amount=" & Str(Val(lbl_IGST_Amount.Text)) & ",HSN_Code='" & Trim(cbo_HSNCode.Text) & "',CGST_Perc=" & Str(Val(txt_CGSTPerc.Text)) & ",SGST_Perc=" & Str(Val(txt_SGSTPerc.Text)) & ",IGST_Perc=" & Str(Val(txt_IGSTPerc.Text)) & " ,Net_Amount=" & Str(Val(CSng(lbl_NetAmount.Text))) & ", Bill_No= '" & Trim(txt_BillNo.Text) & "'  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Knotting_Bill_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' "
                cmd.ExecuteNonQuery()
                MessageBox.Show("Updated Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Knotting_Bill_Head", "Knotting_Bill_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Knotting_Bill_Code, Company_IdNo, for_OrderBy", tr)

            '-------------VOUCHER POSTING---------------------
            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            Dim AcPos_ID As Integer = 0

            AcPos_ID = Led_ID

            Dim vNetAmt As Double = Format(Val(CSng(lbl_NetAmount.Text)), "#############0.00")
            Dim vCGSTAmt As Double = Format(Val(CSng(lbl_CGST_Amount.Text)), "#############0.00")
            Dim vSGSTAmt As Double = Format(Val(CSng(lbl_SGST_Amount.Text)), "#############0.00")
            Dim vIGSTAmt As Double = Format(Val(CSng(lbl_IGST_Amount.Text)), "#############0.00")

            vLed_IdNos = AcPos_ID & "|" & Knot_ID & "|" & "24|25|26"

            vVou_Amts = vNetAmt & "|" & -1 * (vNetAmt - (vCGSTAmt + vSGSTAmt + vIGSTAmt)) & "|" & -1 * vCGSTAmt & "|" & -1 * vSGSTAmt & "|" & -1 * vIGSTAmt


            If Common_Procedures.Voucher_Updation(con, "Knot.Bill", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_RefNo.Text), Convert.ToDateTime(msk_RefDate.Text), "Inv No : " & Trim(lbl_RefNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If


            Dim VouBil As String = ""
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), Convert.ToDateTime(msk_RefDate.Text), AcPos_ID, Trim(lbl_RefNo.Text), 0, Val(CSng(lbl_NetAmount.Text)), "DR", Trim(Pk_Condition) & Trim(NewCode), tr, Common_Procedures.SoftwareTypes.Textile_Software)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If


            tr.Commit()

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
            MessageBox.Show("ERROR ON SAVING", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
        End Try
    End Sub
    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String

        Dim LedgerId As Integer
        Dim KnotAcId As Integer

        If Val(no) = 0 Then Exit Sub

        Clear()

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Knotting_Bill_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Knotting_Bill_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Knotting_Bill_No").ToString

                msk_RefDate.Text = dt1.Rows(0).Item("Knotting_Bill_Date").ToString
                msk_RefDate.SelectionStart = 0

                LedgerId = dt1.Rows(0).Item("Ledger_IdNo").ToString
                cbo_Ledger.Text = Common_Procedures.Ledger_IdNoToName(con, LedgerId)

                KnotAcId = dt1.Rows(0).Item("Knotting_AccIdNo").ToString()
                cbo_KnottingAc.Text = Common_Procedures.Ledger_IdNoToName(con, KnotAcId)

                txt_Note.Text = dt1.Rows(0).Item("Note").ToString

                lbl_TaxableValue.Text = Format(Val(dt1.Rows(0).Item("Taxable_Value").ToString), "########0.00")
                txt_Amount.Text = Format(Val(dt1.Rows(0).Item("Amount").ToString), "########0.00")

                txt_CGSTPerc.Text = Format(Val(dt1.Rows(0).Item("CGST_Perc").ToString), "########0.00")
                txt_SGSTPerc.Text = Format(Val(dt1.Rows(0).Item("SGST_Perc").ToString), "########0.00")
                txt_IGSTPerc.Text = Format(Val(dt1.Rows(0).Item("IGST_Perc").ToString), "########0.00")

                lbl_CGST_Amount.Text = Format(Val(dt1.Rows(0).Item("CGST_Amount").ToString), "########0.00")
                lbl_SGST_Amount.Text = Format(Val(dt1.Rows(0).Item("SGST_Amount").ToString), "########0.00")
                lbl_IGST_Amount.Text = Format(Val(dt1.Rows(0).Item("IGST_Amount").ToString), "########0.00")

                cbo_HSNCode.Text = dt1.Rows(0).Item("HSN_Code").ToString
                lbl_NetAmount.Text = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "#########0.00")
                txt_BillNo.Text = dt1.Rows(0).Item("Bill_No").ToString


                lbl_RefNo.ForeColor = Color.Black


            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If msk_RefDate.Visible And msk_RefDate.Enabled Then msk_RefDate.Focus()
            'If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()
        End Try

        NoCalc_Status = False

        New_Entry = False
    End Sub
    Private Sub cbo_HSNCode_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_HSNCode.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Knotting_Bill_Head", "HSN_Code", "", "")
    End Sub
    Private Sub cbo_HSNCode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_HSNCode.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_HSNCode, cbo_KnottingAc, txt_No_of_Beams, "Knotting_Bill_Head", "HSN_Code", "", "")
    End Sub
    Private Sub cbo_HSNCode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_HSNCode.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_HSNCode, txt_No_of_Beams, "Knotting_Bill_Head", "HSN_Code", "", "", False)
    End Sub
    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
        cbo_Ledger.Tag = cbo_Ledger.Text
    End Sub
    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, msk_RefDate, txt_Description, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
    End Sub
    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, txt_Description, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
    End Sub
    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_KnottingAc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_KnottingAc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27 OR AccountsGroup_IdNo = 28 OR AccountsGroup_IdNo = 15 OR AccountsGroup_IdNo = 16)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_KnottingAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_KnottingAc.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_KnottingAc, txt_BillNo, cbo_HSNCode, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27 OR AccountsGroup_IdNo = 28 OR AccountsGroup_IdNo = 15 OR AccountsGroup_IdNo = 16)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_KnottingAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_KnottingAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_KnottingAc, cbo_HSNCode, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27 OR AccountsGroup_IdNo = 28 OR AccountsGroup_IdNo = 15 OR AccountsGroup_IdNo = 16)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_KnottingAc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_KnottingAc.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_KnottingAc.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Close_Form()
    End Sub
    Private Sub NetAmount_Calculation()
        lbl_TaxableValue.Text = Val(txt_Amount.Text)

        lbl_CGST_Amount.Text = ""
        lbl_SGST_Amount.Text = ""
        lbl_IGST_Amount.Text = ""


        lbl_CGST_Amount.Text = Val(txt_CGSTPerc.Text) * Val(lbl_TaxableValue.Text) / 100
        lbl_SGST_Amount.Text = Val(txt_SGSTPerc.Text) * Val(lbl_TaxableValue.Text) / 100
        lbl_IGST_Amount.Text = Val(txt_IGSTPerc.Text) * Val(lbl_TaxableValue.Text) / 100

        lbl_NetAmount.Text = Val(lbl_TaxableValue.Text) + Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text) + Val(lbl_IGST_Amount.Text)

        lbl_NetAmount.Text = Common_Procedures.Currency_Format(lbl_NetAmount.Text)
    End Sub

    Private Sub txt_Amount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Amount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Amount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Amount.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_CGSTPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CGSTPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_CGSTPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_CGSTPerc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_IGSTPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_IGSTPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_IGSTPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_IGSTPerc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_SGSTPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_SGSTPerc.TextChanged
        NetAmount_Calculation()
    End Sub
    Private Sub btn_Filter_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Fliter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, HSN_Code As String
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            HSN_Code = 0

            If IsDate(dtp_FilterFrom_date.Value) = True And IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "Knotting_Bill_Date between '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterFrom_date.Value) = True Then
                Condt = "Knotting_Bill_Date = '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "Knotting_Bill_Date = '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_HSNCode.Text) <> "" Then
                HSN_Code = Trim(cbo_Filter_HSNCode.Text)
            End If
         

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (Ledger_Idno = " & Str(Val(Led_IdNo)) & ")"
            End If

            If Val(HSN_Code) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " HSN_Code = '" & Trim(HSN_Code) & "'"
            End If

            da = New SqlClient.SqlDataAdapter("select * from Knotting_Bill_Head  where company_idno =" & Str(Val(lbl_Company.Tag)) & " and Knotting_Bill_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & "  Order by Knotting_Bill_Date, for_orderby, Knotting_Bill_No", con)
            da.Fill(dt2)

            dgv_filter.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_filter.Rows.Add()

                    'dgv_filter.Rows(n).Cells(0).Value = i + 1
                    dgv_filter.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Knotting_Bill_No").ToString
                    dgv_filter.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Knotting_Bill_Date").ToString), "dd-MM-yyyy")
                    dgv_filter.Rows(n).Cells(2).Value = Common_Procedures.Ledger_IdNoToName(con, dt2.Rows(i).Item("Ledger_IdNo").ToString)
                    dgv_filter.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Description").ToString
                    dgv_filter.Rows(n).Cells(4).Value = dt2.Rows(i).Item("HSN_Code").ToString
                    dgv_filter.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Noof_Beams").ToString
                    dgv_filter.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")
                    dgv_filter.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("CGST_Amount").ToString) + Val(dt2.Rows(i).Item("SGST_Amount").ToString) + Val(dt2.Rows(i).Item("IGST_Amount").ToString), "########0.00")
                    dgv_filter.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.00")

                Next i

            End If

            dt2.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt2.Dispose()
            da.Dispose()

            If dgv_filter.Visible And dgv_filter.Enabled Then dgv_filter.Focus()

        End Try

    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_filter.CurrentRow.Cells(1).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            Filter_RowNo = dgv_filter.CurrentRow.Index
            move_record(movno)
            pnl_Back.Enabled = True
            pnl_Filter.Visible = False
        End If

    End Sub
    Private Sub dgv_filter_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_filter.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_filter.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Note.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                msk_RefDate.Focus()
            End If
        End If
    End Sub

    Private Sub txt_No_of_Beams_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_No_of_Beams.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub dtp_RefDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_RefDate.TextChanged
        If IsDate(dtp_RefDate.Text) = True Then
            msk_RefDate.Text = dtp_RefDate.Text
            msk_RefDate.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_RefDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_RefDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            cbo_Ledger.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            txt_Note.Focus()
        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_RefDate.Text
            vmskSelStrt = msk_RefDate.SelectionStart
        End If

    End Sub

    Private Sub msk_RefDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_RefDate.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_RefDate.Text = Date.Today
            msk_RefDate.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            cbo_Ledger.Focus()
        End If
    End Sub

    Private Sub msk_RefDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_RefDate.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_RefDate.Text = Date.Today
        '    msk_RefDate.SelectionStart = 0
        'End If
        If e.KeyCode = 107 Then
            msk_RefDate.Text = DateAdd("D", 1, Convert.ToDateTime(msk_RefDate.Text))
            msk_RefDate.SelectionStart = 0
        ElseIf e.KeyCode = 109 Then
            msk_RefDate.Text = DateAdd("D", -1, Convert.ToDateTime(msk_RefDate.Text))
            msk_RefDate.SelectionStart = 0
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)

        End If

    End Sub

    Private Sub btn_UserModification_Click(sender As Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub



    Private Sub txt_Description_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Description.KeyDown
        If e.KeyCode = 38 Then
            cbo_Ledger.Focus()
        End If
        If e.KeyCode = 40 Then
            txt_BillNo.Focus()
        End If
    End Sub

    Private Sub txt_Description_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Description.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_BillNo.Focus()
        End If
    End Sub

    Private Sub txt_BillNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_BillNo.KeyDown
        If e.KeyCode = 38 Then
            txt_Description.Focus()
        End If
        If e.KeyCode = 40 Then
            cbo_KnottingAc.Focus()
        End If
    End Sub

    Private Sub txt_BillNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_BillNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_KnottingAc.Focus()
        End If
    End Sub
End Class