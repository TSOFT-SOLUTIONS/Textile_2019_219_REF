Public Class Cloth_Transfer

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "CLTRA-"
    Private Prec_ActCtrl As New Control

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer
    Private vcbo_KeyDwnVal As Double

    Private vPrn_PvuEdsCnt As String
    Private vPrn_PvuTotBms As Integer
    Private vPrn_PvuTotMtrs As Single
    Private vPrn_PvuSetNo As String
    Private vPrn_PvuBmNos1 As String
    Private vPrn_PvuBmNos2 As String
    Private vPrn_PvuBmNos3 As String
    Private vPrn_PvuBmNos4 As String
    Private WithEvents dgtxt_details As New DataGridViewTextBoxEditingControl

    Private Property Rac_IdNo As Integer
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1


    Private vCLO_CONDT As String = ""
    Private FnYearCode1 As String = ""
    Private FnYearCode2 As String = ""

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        vmskOldText = ""
        vmskSelStrt = -1

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        msk_date.Text = ""
        dtp_Date.Text = ""
        cbo_CompanyTo.Text = Common_Procedures.Company_IdNoToShortName(con, lbl_Company.Tag)
        cbo_ClothFrom.Text = ""
        cbo_ClothTo.Text = ""
        cbo_TypeFrom.Text = ""
        cbo_PartyTo.Text = ""
        cbo_PartyFrom.Text = ""

        cbo_TypeTo.Text = ""
        txt_FoldingFrom.Text = ""
        txt_FoldingTo.Text = ""
        txt_Pcs.Text = ""
        txt_MetersFrom.Text = ""
        txt_MetersTo.Text = ""
        txt_remarks.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        ' dgv_Details.Rows.Clear()


        Grid_DeSelect()


        'cbo_GridItemName.Visible = False
        'cbo_GridRackNo.Visible = False


        'cbo_GridItemName.Tag = -1
        'cbo_GridRackNo.Text = -1

        'cbo_GridItemName.Text = ""
        'cbo_GridRackNo.Text = ""


        cbo_ClothSales_OrderCode_forSelection_From.Text = ""
        cbo_ClothSales_OrderCode_forSelection_To.Text = ""

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
    Private Sub Grid_DeSelect()
        On Error Resume Next
        ' dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String


        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name as Cloth_From ,c.Cloth_Name as Cloth_To, d.ClothType_Name as Type_FromName , e.ClothType_Name as Type_ToName, F.Company_ShortName as Company_To_ShortName , g.Ledger_Name , h.Ledger_Name from Cloth_Transfer_Head a INNER JOIN cloth_Head b ON a.Cloth_From_Idno = b.Cloth_IdNo INNER JOIN Cloth_Head c ON a.Cloth_To_Idno = c.Cloth_IdNo LEFT OUTER JOIN clothType_Head d ON a.Type_From = d.ClothType_IdNo LEFT OUTER JOIN clothType_Head e ON a.Type_To = e.ClothType_IdNo LEFT OUTER JOIN Company_Head f ON a.Company_To_IdNo = f.Company_IdNo INNER JOIN Ledger_Head g ON a.LedgerFrom_IdNo = g.Ledger_IdNo INNER JOIN Ledger_Head h ON a.LedgerTo_IdNo = h.Ledger_IdNo Where a.Cloth_Transfer_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Cloth_Transfer_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Cloth_Transfer_Date").ToString
                msk_date.Text = dtp_Date.Text
                cbo_CompanyTo.Text = dt1.Rows(0).Item("Company_To_ShortName").ToString
                cbo_PartyFrom.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("LedgerFrom_IdNo").ToString))
                cbo_PartyTo.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("LedgerTo_IdNo").ToString))
                cbo_ClothFrom.Text = dt1.Rows(0).Item("cloth_From").ToString
                cbo_ClothTo.Text = dt1.Rows(0).Item("Cloth_To").ToString
                cbo_TypeFrom.Text = dt1.Rows(0).Item("Type_FromName").ToString
                cbo_TypeTo.Text = dt1.Rows(0).Item("Type_ToName").ToString
                txt_FoldingFrom.Text = Format(Val(dt1.Rows(0).Item("Folding_From").ToString), "########0.00")
                txt_FoldingTo.Text = Format(Val(dt1.Rows(0).Item("Folding_To").ToString), "########0.00")
                txt_Pcs.Text = Val(dt1.Rows(0).Item("Noof_Pcs").ToString)
                txt_MetersFrom.Text = Format(Val(dt1.Rows(0).Item("Meters_From").ToString), "########0.00")
                txt_MetersTo.Text = Format(Val(dt1.Rows(0).Item("Meters_To").ToString), "########0.00")
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                cbo_ClothSales_OrderCode_forSelection_To.Text = dt1.Rows(0).Item("ClothSales_OrderCode_forSelection_To").ToString
                cbo_ClothSales_OrderCode_forSelection_From.Text = dt1.Rows(0).Item("ClothSales_OrderCode_forSelection_From").ToString

                txt_remarks.Text = dt1.Rows(0).Item("Remarks").ToString


            End If


            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()

    End Sub

    Private Sub Item_Transfer_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ClothFrom.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ClothFrom.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ClothTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ClothTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_TypeFrom.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "RACK" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_TypeFrom.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_TypeTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "RACK" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_TypeTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

                Me.Text = lbl_Heading.text & "  -  " & lbl_Company.Text

                new_record()

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Item_Transfer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

        da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
        da.Fill(dt1)
        cbo_ClothFrom.DataSource = dt1
        cbo_ClothFrom.DisplayMember = "Cloth_Name"

        da = New SqlClient.SqlDataAdapter("select ClothType_Name from ClothType_head order by ClothType_Name", con)
        da.Fill(dt2)
        cbo_TypeFrom.DataSource = dt2
        cbo_TypeFrom.DisplayMember = "ClothType_name"

        da = New SqlClient.SqlDataAdapter("select cloth_Name from cloth_Head  order by Cloth_Name", con)
        da.Fill(dt3)
        cbo_ClothTo.DataSource = dt3
        cbo_ClothTo.DisplayMember = "Cloth_Name"

        da = New SqlClient.SqlDataAdapter("select clothType_Name from ClothType_head order by clothType_Name", con)
        da.Fill(dt4)
        cbo_TypeTo.DataSource = dt4
        cbo_TypeTo.DisplayMember = "ClothType_name"

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_CompanyTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TypeFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TypeTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection_From.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection_To.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_FoldingFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_FoldingTo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_MetersFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_MetersTo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Pcs.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_remarks.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_CompanyTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TypeFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TypeTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection_From.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection_To.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_FoldingFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_FoldingTo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_MetersFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_MetersTo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Pcs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_remarks.LostFocus, AddressOf ControlLostFocus


        AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_FoldingFrom.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_FoldingTo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_MetersFrom.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Pcs.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_FoldingFrom.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_FoldingTo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_MetersFrom.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Pcs.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2


        If Common_Procedures.settings.Show_Sales_OrderNumber_in_ALLEntry_Status <> 1 Then
            lbl_remarks.Top = Label10.Bottom + 20
            txt_remarks.Top = txt_MetersFrom.Bottom + 15
        End If

        If Common_Procedures.settings.Show_Sales_OrderNumber_in_ALLEntry_Status = 1 Then

            lbl_Sales_OrderNo_From.Visible = True
            lbl_Sales_OrderNo_To.Visible = True
            cbo_ClothSales_OrderCode_forSelection_From.Visible = True
            cbo_ClothSales_OrderCode_forSelection_To.Visible = True

            FnYearCode1 = ""
            FnYearCode2 = ""
            Common_Procedures.get_FnYearCode_of_Last_2_Years(FnYearCode1, FnYearCode2)

        Else

            lbl_Sales_OrderNo_From.Visible = False
            lbl_Sales_OrderNo_To.Visible = False
            cbo_ClothSales_OrderCode_forSelection_From.Visible = False
            cbo_ClothSales_OrderCode_forSelection_To.Visible = False


        End If

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Item_Transfer_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Item_Transfer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                Else
                    If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                        Exit Sub

                    Else
                        Close_Form()

                    End If



                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub


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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Qa As Windows.Forms.DialogResult
        Dim vOrdByNo As String = ""
        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Cloth_Transfer_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Cloth_Transfer_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to DELETE", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Cloth_Transfer_Entry, New_Entry, Me, con, "Cloth_Transfer_Head", "Cloth_Transfer_Code", NewCode, "Cloth_Transfer_Date", "(Cloth_Transfer_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        Qa = MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If Qa = Windows.Forms.DialogResult.No Or Qa = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Cloth_Transfer_Head", "Cloth_Transfer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Cloth_Transfer_Code, Company_IdNo, for_OrderBy", trans)


            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Cloth_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Transfer_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If msk_date.Enabled = True And msk_date.Visible = True Then msk_date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then



            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""

            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Filter.BringToFront()
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Cloth_Transfer_No from Cloth_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby,Cloth_Transfer_No", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Cloth_Transfer_No from Cloth_Transfer_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby,Cloth_Transfer_No", con)
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

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Cloth_Transfer_No from Cloth_Transfer_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc,Cloth_Transfer_No desc", con)
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

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Cloth_Transfer_No from Cloth_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc,Cloth_Transfer_No desc", con)
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

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Cloth_Transfer_Head", "Cloth_Transfer_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red

            msk_date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Cloth_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Cloth_Transfer_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Cloth_Transfer_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Cloth_Transfer_Date").ToString
                End If
            End If
            dt1.Clear()

            If msk_date.Enabled And msk_date.Visible Then
                msk_date.Focus()
                msk_date.SelectionStart = 0
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        dt1.Dispose()
        da.Dispose()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        Try

            inpno = InputBox("Enter Ref.No.", "FOR FINDING...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Cloth_Transfer_No from Cloth_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Transfer_Code = '" & Trim(RefCode) & "'", con)
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
                MessageBox.Show("Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String


        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Cloth_Transfer_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Cloth_Transfer_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Cloth_Transfer_Entry, New_Entry, Me) = False Then Exit Sub


        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Cloth_Transfer_No from Cloth_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Transfer_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Ref No", "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim LedFrom_ID As Integer = 0
        Dim LedTo_ID As Integer = 0
        Dim Col_ID As Integer = 0
        Dim cthtyfm_ID As Integer = 0, CthTyTo_Id As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim EntID As String = ""
        Dim Clthfm_ID As Integer = 0
        Dim Clthto_ID As Integer = 0
        Dim CompToIDno As Integer = 0
        Dim vStkDelvTo_ID As Integer = 0, vStkRecFrm_ID As Integer = 0
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT textile (SOMANUR)
            If Common_Procedures.is_OfficeSystem = False Then
                MessageBox.Show("Invalid Entry, Only System administrators can add this entries", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Cloth_Transfer_Entry, New_Entry) = False Then Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Cloth_Transfer_Entry, New_Entry, Me, con, "Cloth_Transfer_Head", "Cloth_Transfer_Code", NewCode, "Cloth_Transfer_Date", "(Cloth_Transfer_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Transfer_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Cloth_Transfer_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        CompToIDno = Common_Procedures.Company_ShortNameToIdNo(con, cbo_CompanyTo.Text)
        If CompToIDno = 0 Then
            MessageBox.Show("Invalid Company To Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_CompanyTo.Enabled And cbo_CompanyTo.Visible Then cbo_CompanyTo.Focus()
            Exit Sub
        End If

        LedFrom_ID = Common_Procedures.Ledger_NameToIdNo(con, cbo_PartyFrom.Text)
        If LedFrom_ID = 0 Then
            MessageBox.Show("Select Party From Name!....", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            cbo_PartyFrom.Focus()
            Exit Sub
        End If
        LedTo_ID = Common_Procedures.Ledger_NameToIdNo(con, cbo_PartyTo.Text)
        If LedTo_ID = 0 Then
            MessageBox.Show("Select Party To Name!....", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            cbo_PartyTo.Focus()
            Exit Sub
        End If

        Clthfm_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothFrom.Text)
        If Clthfm_ID = 0 Then
            MessageBox.Show("Invalid Cloth FromName", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ClothFrom.Enabled And cbo_ClothFrom.Visible Then cbo_ClothFrom.Focus()
            Exit Sub
        End If

        Clthto_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothTo.Text)
        If Clthto_ID = 0 Then
            MessageBox.Show("Invalid Cloth ToName", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ClothTo.Enabled And cbo_ClothTo.Visible Then cbo_ClothTo.Focus()
            Exit Sub
        End If

        If Val(txt_MetersFrom.Text) = 0 Then
            MessageBox.Show("Invalid MetersFrom", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_MetersFrom.Enabled Then txt_MetersFrom.Focus()
            Exit Sub
        End If

        If Common_Procedures.settings.Sales_OrderNumber_compulsory_in_ALLEntry_Status = 1 Then
            If Trim(cbo_ClothSales_OrderCode_forSelection_From.Text) = "" Then
                MessageBox.Show("Invalid From Sales Order No ", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_ClothSales_OrderCode_forSelection_From.Enabled And cbo_ClothSales_OrderCode_forSelection_From.Visible Then cbo_ClothSales_OrderCode_forSelection_From.Focus()
                Exit Sub
            End If
        End If

        If Common_Procedures.settings.Sales_OrderNumber_compulsory_in_ALLEntry_Status = 1 Then
            If Trim(cbo_ClothSales_OrderCode_forSelection_To.Text) = "" Then
                MessageBox.Show("Invalid To Sales Order No ", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_ClothSales_OrderCode_forSelection_To.Enabled And cbo_ClothSales_OrderCode_forSelection_To.Visible Then cbo_ClothSales_OrderCode_forSelection_To.Focus()
                Exit Sub
            End If
        End If

        cthtyfm_ID = Common_Procedures.ClothType_NameToIdNo(con, cbo_TypeFrom.Text)

        CthTyTo_Id = Common_Procedures.ClothType_NameToIdNo(con, cbo_TypeTo.Text)

        tr = con.BeginTransaction


        'Try


        If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Cloth_Transfer_Head", "Cloth_Transfer_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@TransferDate", Convert.ToDateTime(msk_date.Text))


            If New_Entry = True Then

            cmd.CommandText = "Insert into Cloth_Transfer_Head (    Cloth_Transfer_Code,                 Company_IdNo      ,           Cloth_Transfer_No    ,                           for_OrderBy                                   , Cloth_Transfer_Date ,       Company_To_IdNo        ,         Cloth_From_Idno    ,           Cloth_To_Idno    ,              Type_From ,         Type_To         ,             Folding_From          ,             Folding_To          ,        Noof_Pcs           ,             Meters_From         ,            Meters_To          ,                          User_idno      ,     LedgerFrom_IdNo     ,   LedgerTo_IdNo       ,           Remarks                 ,                    ClothSales_OrderCode_forSelection_From       ,                  ClothSales_OrderCode_forSelection_To            ) " &
                                  "Values                          ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & " , '" & Trim(lbl_RefNo.Text) & "' , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & " ,     @TransferDate   , " & Str(Val(CompToIDno)) & " , " & Str(Val(Clthfm_ID)) & ", " & Str(Val(Clthto_ID)) & ", " & Val(cthtyfm_ID) & ", " & Val(CthTyTo_Id) & " , " & Val(txt_FoldingFrom.Text) & " , " & Val(txt_FoldingTo.Text) & " , " & Val(txt_Pcs.Text) & " , " & Val(txt_MetersFrom.Text) & "," & Val(txt_MetersTo.Text) & " ," & Val(Common_Procedures.User.IdNo) & " , " & Val(LedFrom_ID) & " ," & Val(LedTo_ID) & " ,'" & Trim(txt_remarks.Text) & "',  '" & Trim(cbo_ClothSales_OrderCode_forSelection_From.Text) & "',  '" & Trim(cbo_ClothSales_OrderCode_forSelection_To.Text) & "'   )"
            cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Cloth_Transfer_Head", "Cloth_Transfer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Cloth_Transfer_Code, Company_IdNo, for_OrderBy", tr)

            cmd.CommandText = "Update Cloth_Transfer_Head set Cloth_Transfer_Date = @TransferDate, Company_To_IdNo = " & Str(Val(CompToIDno)) & ", Cloth_From_Idno = " & Val(Clthfm_ID) & ", Cloth_To_Idno =  " & Val(Clthto_ID) & ", Type_From = " & Val(cthtyfm_ID) & " , Type_To = " & Val(CthTyTo_Id) & " ,Folding_From = " & Val(txt_FoldingFrom.Text) & " , Folding_To = " & Val(txt_FoldingTo.Text) & " , Noof_Pcs = " & Val(txt_Pcs.Text) & " , Meters_From = " & Val(txt_MetersFrom.Text) & ", Meters_To = " & Val(txt_MetersTo.Text) & " , User_IdNo = " & Val(Common_Procedures.User.IdNo) & " , LedgerFrom_IdNo = " & Val(LedFrom_ID) & " , LedgerTo_IdNo = " & Val(LedTo_ID) & " , Remarks = '" & Trim(txt_remarks.Text) & "' , ClothSales_OrderCode_forSelection_From = '" & Trim(cbo_ClothSales_OrderCode_forSelection_From.Text) & "' , ClothSales_OrderCode_forSelection_To = '" & Trim(cbo_ClothSales_OrderCode_forSelection_To.Text) & "'      Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Transfer_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Cloth_Transfer_Head", "Cloth_Transfer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Cloth_Transfer_Code, Company_IdNo, for_OrderBy", tr)

            Partcls = "Transfer : Ref.No. " & Trim(lbl_RefNo.Text)
            PBlNo = Trim(lbl_RefNo.Text)
            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()



            If Val(txt_MetersFrom.Text) <> 0 Then
                vStkDelvTo_ID = 0 : vStkRecFrm_ID = 0
                If Val(txt_MetersFrom.Text) > 0 Then
                    vStkRecFrm_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)
                Else
                    vStkDelvTo_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)
                End If

            cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code,             Company_IdNo         ,           Reference_No        ,                               for_OrderBy                              , Reference_Date ,                                            StockOff_IdNo  ,         DeliveryTo_Idno       ,          ReceivedFrom_Idno     ,         Entry_ID     ,       Party_Bill_No  ,       Particulars      , Sl_No,           Cloth_Idno       ,                Folding                ,   Meters_Type" & Trim(Val(cthtyfm_ID)) & "          ,          ClothSales_OrderCode_forSelection  ) " &
                                        " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @TransferDate, " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & "," & Str(Val(vStkDelvTo_ID)) & ", " & Str(Val(vStkRecFrm_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   1  , " & Str(Val(Clthfm_ID)) & ", " & Str(Val(txt_FoldingFrom.Text)) & ", " & Str(Math.Abs(Val(txt_MetersFrom.Text))) & " , '" & Trim(cbo_ClothSales_OrderCode_forSelection_From.Text) & "'  ) "
            cmd.ExecuteNonQuery()
            End If

            If Val(txt_MetersTo.Text) <> 0 Then
                vStkDelvTo_ID = 0 : vStkRecFrm_ID = 0
                If Val(txt_MetersTo.Text) > 0 Then
                    vStkDelvTo_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)
                Else
                    vStkRecFrm_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)
                End If

            cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code,             Company_IdNo    ,           Reference_No        ,                               for_OrderBy                              , Reference_Date ,                                            StockOff_IdNo  ,         DeliveryTo_Idno        ,         ReceivedFrom_Idno      ,         Entry_ID     ,       Party_Bill_No  ,       Particulars      , Sl_No,           Cloth_Idno       ,                Folding              ,   Meters_Type" & Trim(Val(CthTyTo_Id)) & "     ,                     ClothSales_OrderCode_forSelection ) " &
                                        " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(CompToIDno)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @TransferDate, " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", " & Str(Val(vStkDelvTo_ID)) & ", " & Str(Val(vStkRecFrm_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   2  , " & Str(Val(Clthto_ID)) & ", " & Str(Val(txt_FoldingTo.Text)) & ", " & Str(Math.Abs(Val(txt_MetersTo.Text))) & "  , '" & Trim(cbo_ClothSales_OrderCode_forSelection_To.Text) & "' ) "
            cmd.ExecuteNonQuery()

            End If

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_RefNo.Text)
                End If
            Else
                move_record(lbl_RefNo.Text)
            End If

        'Catch ex As Exception
        '    tr.Rollback()
        '    MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

        If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()


    End Sub

    Private Sub cbo_ClothFrom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothFrom.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_ClothFrom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothFrom.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothFrom, cbo_PartyTo, cbo_ClothTo, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_ClothFrom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothFrom.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothFrom, cbo_ClothTo, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_ClothFrom_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothFrom.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Dim f As New Cloth_Creation
            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ClothFrom.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_TypeFrom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TypeFrom.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothType_head", "ClothType_Name", "(ClothType_idNo between 1 and 5)", "(ClothType_idNo = 0)")

    End Sub

    Private Sub cbo_TypeFrom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TypeFrom.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TypeFrom, cbo_ClothTo, cbo_TypeTo, "ClothType_head", "ClothType_Name", "(ClothType_idNo between 1 and 5)", "(ClothType_idNo = 0)")

    End Sub

    Private Sub cbo_TypeFrom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TypeFrom.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TypeFrom, cbo_TypeTo, "ClothType_head", "ClothType_Name", "(ClothType_idNo between 1 and 5)", "(ClothType_idNo = 0)")

    End Sub

    Private Sub cbo_ClothTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_ClothTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothTo, cbo_ClothFrom, cbo_TypeFrom, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_ClothTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothTo, cbo_TypeFrom, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_ClothTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothTo.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ClothTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1

            f.Show()

        End If
    End Sub

    Private Sub cbo_TypeTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TypeTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothType_head", "ClothType_Name", "(ClothType_idNo between 1 and 5)", "(ClothType_idNo = 0)")
    End Sub

    Private Sub cbo_TypeTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TypeTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TypeTo, cbo_TypeFrom, txt_FoldingFrom, "ClothType_head", "ClothType_Name", "(ClothType_idNo between 1 and 5)", "(ClothType_idNo = 0)")
    End Sub

    Private Sub cbo_TypeTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TypeTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TypeTo, txt_FoldingFrom, "ClothType_head", "ClothType_Name", "(ClothType_idNo between 1 and 5)", "(ClothType_idNo = 0)")
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        '  Dim Procto_IdNo As Integer, procfm_IdNo As Integer
        Dim Condt As String = ""


        'Try

        Condt = ""


        If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
            Condt = "a.Cloth_Transfer_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
        ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
            Condt = "a.Cloth_Transfer_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
        ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
            Condt = "a.Cloth_Transfer_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
        End If


        da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name as Cloth_From ,c.Cloth_Name as Cloth_To, d.ClothType_Name as Type_FromName , e.ClothType_Name as Type_ToName  from Cloth_Transfer_Head a INNER JOIN cloth_Head b ON a.Cloth_From_Idno = b.Cloth_IdNo INNER JOIN Cloth_Head c ON a.Cloth_To_Idno = c.Cloth_IdNo LEFT OUTER JOIN clothType_Head d ON a.Type_From = d.ClothType_IdNo LEFT OUTER JOIN clothType_Head e ON a.Type_To = e.ClothType_IdNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cloth_Transfer_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Cloth_Transfer_No", con)
        da.Fill(dt2)

        dgv_Filter_Details.Rows.Clear()

        If dt2.Rows.Count > 0 Then

            For i = 0 To dt2.Rows.Count - 1

                n = dgv_Filter_Details.Rows.Add()


                dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Cloth_Transfer_No").ToString
                dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Cloth_Transfer_Date").ToString), "dd-MM-yyyy")
                dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Cloth_From").ToString
                dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Cloth_To").ToString
                dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meters_From").ToString), "########0.00")
                dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Meters_To").ToString), "########0.00")

            Next i

        End If

        dt2.Clear()
        dt2.Dispose()
        da.Dispose()

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

        If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(0).Value)

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

    Private Sub txt_MetersTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_MetersTo.KeyDown
        If e.KeyCode = 40 Then
            If cbo_ClothSales_OrderCode_forSelection_From.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection_From.Focus()
            Else
                txt_remarks.Focus()
            End If
        End If
        If e.KeyCode = 38 Then txt_MetersFrom.Focus()

    End Sub

    Private Sub txt_MetersTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_MetersTo.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then

            'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            '    save_record()
            'Else
            '    msk_date.Focus()
            'End If


            If cbo_ClothSales_OrderCode_forSelection_From.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection_From.Focus()
            Else
                txt_remarks.Focus()
            End If


        End If
    End Sub


    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

    End Sub

    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        print_record()
    End Sub

    Private Sub btn_save_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub msk_date_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
        End If
    End Sub
    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_date.Text = Date.Today
        'End If
        If e.KeyCode = 107 Then
            msk_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
        ElseIf e.KeyCode = 109 Then
            msk_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)

        End If
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

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
        End If
    End Sub
    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then

            msk_date.Text = dtp_Date.Text
            msk_date.SelectionStart = 0
        End If
    End Sub
    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue


        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
        End If
    End Sub

    Private Sub cbo_CompanyTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CompanyTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Company_Head", "Company_ShortName", "", "(Company_IdNo = 0)")
    End Sub

    Private Sub cbo_CompanyTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CompanyTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_CompanyTo, msk_date, cbo_PartyFrom, "Company_Head", "Company_ShortName", "", "(Company_IdNo = 0)")
    End Sub

    Private Sub cbo_CompanyTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CompanyTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_CompanyTo, cbo_PartyFrom, "Company_Head", "Company_ShortName", "", "(Company_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyFrom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyFrom.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )  and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyFrom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyFrom.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyFrom, cbo_CompanyTo, cbo_PartyTo, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )  and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyFrom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyFrom.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyFrom, cbo_PartyTo, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )  and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyFrom_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyFrom.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
        Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyFrom.Name
            Common_Procedures.Master_Return.Master_Type = ""
            Common_Procedures.Master_Return.Return_Value = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_PartyTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )  and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyTo, cbo_PartyFrom, cbo_ClothFrom, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )  and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyTo, cbo_ClothFrom, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )  and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyTo.KeyUp
       If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyTo.Name
            Common_Procedures.Master_Return.Master_Type = ""
            Common_Procedures.Master_Return.Return_Value = ""
            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub txt_remarks_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_remarks_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_remarks.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
        If e.KeyCode = 38 Then
            If cbo_ClothSales_OrderCode_forSelection_To.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection_To.Focus()
            ElseIf txt_MetersTo.Visible = True Then
                txt_MetersTo.Focus()
            Else
                txt_remarks.Focus()
            End If
        End If 'txt_MetersTo.Focus()
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_From_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothSales_OrderCode_forSelection_From.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, cbo_ClothSales_OrderCode_forSelection_To, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_From_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothSales_OrderCode_forSelection_From.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, cbo_ClothSales_OrderCode_forSelection_To, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")


        If (e.KeyValue = 38 And cbo_ClothSales_OrderCode_forSelection_From.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            txt_MetersTo.Focus()
        End If



    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_From_Enter(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection_From.Enter
        vCLO_CONDT = "(ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode1) & "' or ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode2) & "' and Order_Close_Status = 0 )"

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_To_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection_To.SelectedIndexChanged

    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_To_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothSales_OrderCode_forSelection_To.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, txt_remarks, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_To_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothSales_OrderCode_forSelection_To.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, cbo_ClothSales_OrderCode_forSelection_From, txt_remarks, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_To_Enter(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection_To.Enter
        vCLO_CONDT = "(ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode1) & "' or ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode2) & "' and Order_Close_Status = 0 )"

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_From_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection_From.SelectedIndexChanged

    End Sub

    Private Sub txt_remarks_TextChanged(sender As Object, e As EventArgs) Handles txt_remarks.TextChanged

    End Sub
End Class