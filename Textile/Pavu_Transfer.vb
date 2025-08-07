Public Class Pavu_Transfer
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "FPBM-"
    Private Prec_ActCtrl As New Control
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer

    Private vPrn_PvuEdsCnt As String
    Private vPrn_PvuTotBms As Integer
    Private vPrn_PvuTotMtrs As Single
    Private vPrn_PvuSetNo As String
    Private vPrn_PvuBmNos1 As String
    Private vPrn_PvuBmNos2 As String
    Private vPrn_PvuBmNos3 As String
    Private vPrn_PvuBmNos4 As String
    Private WithEvents dgtxt_details As New DataGridViewTextBoxEditingControl
    Private cbo_KeyDwnVal As Double
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Private vLed_ID_Cond As Integer = 0
    Private vCLO_CONDT As String = ""
    Private FnYearCode1 As String = ""
    Private FnYearCode2 As String = ""


    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        msk_Date.Text = ""
        dtp_Date.Text = ""
        msk_Date.Text = dtp_Date.Text
        cbo_PartyFrom.Text = ""
        cbo_PartyTo.Text = ""
        cbo_EndsCountFrom.Text = ""

        cbo_EndscountTo.Text = ""
        txt_MetersFrom.Text = ""
        txt_MetersTo.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))



        cbo_weaving_job_no.Text = ""
        cbo_Sizing_JobCardNo.Text = ""
        txt_remarks.Text = ""

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

        'Try

        da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as PartyName ,c.EndsCount_Name as EndsCountName_From , d.EndsCount_Name as EndsCountName_To ,e.Ledger_Name from Pavu_transfer_Head a INNER JOIN Ledger_Head b ON a.Ledger_Idno = b.Ledger_IdNo LEFT OUTER JOIN  EndsCount_Head c ON a.EndsCountIdno_From = c.EndsCount_IdNo LEFT OUTER JOIN  EndsCount_Head D ON a.EndsCountIdno_To = d.EndsCount_IdNo INNER JOIN Ledger_Head e ON a.LedgerTo_IdNo = e.Ledger_IdNo  Where a.Pavu_Transfer_Code = '" & Trim(NewCode) & "'", con)
        da1.Fill(dt1)

        If dt1.Rows.Count > 0 Then
            lbl_RefNo.Text = dt1.Rows(0).Item("Pavu_Transfer_No").ToString
            dtp_Date.Text = dt1.Rows(0).Item("Pavu_Transfer_Date").ToString
            msk_Date.Text = dtp_Date.Text
            cbo_PartyFrom.Text = dt1.Rows(0).Item("PartyName").ToString
            'cbo_PartyFrom.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("LedgerFrom_IdNo").ToString))
            cbo_PartyTo.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("LedgerTo_IdNo").ToString))
            cbo_EndsCountFrom.Text = dt1.Rows(0).Item("EndsCountName_From").ToString
            cbo_EndscountTo.Text = dt1.Rows(0).Item("endsCountName_To").ToString
            txt_MetersFrom.Text = Format(Val(dt1.Rows(0).Item("Meters_From").ToString), "########0.00")
            txt_MetersTo.Text = Format(Val(dt1.Rows(0).Item("Meters_To").ToString), "########0.00")
            lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))


            cbo_weaving_job_no.Text = dt1.Rows(0).Item("Weaving_JobCode_forSelection").ToString
            cbo_Sizing_JobCardNo.Text = dt1.Rows(0).Item("Sizing_JobCode_forSelection").ToString
            txt_remarks.Text = dt1.Rows(0).Item("Remarks").ToString

            cbo_ClothSales_OrderCode_forSelection_To.Text = dt1.Rows(0).Item("ClothSales_OrderCode_forSelection_To").ToString
            cbo_ClothSales_OrderCode_forSelection_From.Text = dt1.Rows(0).Item("ClothSales_OrderCode_forSelection_From").ToString

        End If

        dt1.Clear()
        dt1.Dispose()
        da1.Dispose()

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Private Sub Pavu_Transfer_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated


        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyFrom.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyFrom.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_EndsCountFrom.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_EndsCountFrom.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_EndscountTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_EndscountTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Pavu_Transfer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Text = ""

        con.Open()

        lbl_weaving_job_no.Visible = False
        cbo_weaving_job_no.Visible = False


        lbl_Sizing_jobcardno_Caption.Visible = False
        cbo_Sizing_JobCardNo.Visible = False

        lbl_Sales_OrderNo_From.Visible = False
        lbl_Sales_OrderNo_To.Visible = False
        cbo_ClothSales_OrderCode_forSelection_From.Visible = False
        cbo_ClothSales_OrderCode_forSelection_To.Visible = False


        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        If Common_Procedures.settings.Show_Weaver_JobCard_Entry_STATUS = 1 Then

            lbl_weaving_job_no.Visible = True
            cbo_weaving_job_no.Visible = True
            cbo_weaving_job_no.BackColor = Color.White

        End If


        If Common_Procedures.settings.Show_Sizing_JobCard_Entry_Status = 1 Then

            lbl_Sizing_jobcardno_Caption.Visible = True
            cbo_Sizing_JobCardNo.Visible = True
            cbo_Sizing_JobCardNo.BackColor = Color.White

            If lbl_weaving_job_no.Visible = False And cbo_weaving_job_no.Visible = False Then
                lbl_Sizing_jobcardno_Caption.Left = Label2.Left
                cbo_Sizing_JobCardNo.Left = lbl_RefNo.Left
                cbo_Sizing_JobCardNo.Width = txt_remarks.Width

            End If

        End If

        If cbo_weaving_job_no.Visible = True And cbo_Sizing_JobCardNo.Visible = False Then
            cbo_weaving_job_no.Width = txt_remarks.Width
        End If



        If cbo_weaving_job_no.Visible = False And cbo_Sizing_JobCardNo.Visible = False And Common_Procedures.settings.Show_Sales_OrderNumber_in_ALLEntry_Status <> 1 Then
            lbl_remarks.Top = Label12.Bottom + 20
            txt_remarks.Top = Label12.Bottom + 15
        End If



        If Common_Procedures.settings.Show_Sales_OrderNumber_in_ALLEntry_Status <> 1 Then
            lbl_remarks.Top = lbl_weaving_job_no.Bottom + 20
            txt_remarks.Top = lbl_weaving_job_no.Bottom + 15
        End If

        If Common_Procedures.settings.Show_Sales_OrderNumber_in_ALLEntry_Status <> 1 And Common_Procedures.settings.Show_Sizing_JobCard_Entry_Status <> 1 Then
            lbl_remarks.Top = Label12.Bottom + 15
            txt_remarks.Top = txt_MetersFrom.Bottom + 10
        End If



        If Common_Procedures.settings.Show_Sales_OrderNumber_in_ALLEntry_Status = 1 Then

            lbl_Sales_OrderNo_From.Visible = True
            lbl_Sales_OrderNo_To.Visible = True
            cbo_ClothSales_OrderCode_forSelection_From.Visible = True
            cbo_ClothSales_OrderCode_forSelection_To.Visible = True

            FnYearCode1 = ""
            FnYearCode2 = ""
            Common_Procedures.get_FnYearCode_of_Last_2_Years(FnYearCode1, FnYearCode2)


            If cbo_Sizing_JobCardNo.Visible = False Then

                lbl_Sales_OrderNo_From.Top = Label12.Bottom + 20
                lbl_Sales_OrderNo_To.Top = Label3.Bottom + 20
                cbo_ClothSales_OrderCode_forSelection_From.Top = txt_MetersFrom.Bottom + 20
                cbo_ClothSales_OrderCode_forSelection_To.Top = txt_MetersTo.Bottom + 20


                lbl_remarks.Top = lbl_weaving_job_no.Bottom + 20
                txt_remarks.Top = cbo_weaving_job_no.Bottom + 15

            End If

        Else

            lbl_Sales_OrderNo_From.Visible = False
            lbl_Sales_OrderNo_To.Visible = False
            cbo_ClothSales_OrderCode_forSelection_From.Visible = False
            cbo_ClothSales_OrderCode_forSelection_To.Visible = False

        End If




        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EndsCountFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EndscountTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Party.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyTo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_MetersFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_MetersTo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection_From.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection_To.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EndsCountFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EndscountTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Party.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyTo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_MetersFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_MetersTo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection_From.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection_To.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_MetersFrom.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_MetersFrom.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler cbo_weaving_job_no.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Sizing_JobCardNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_weaving_job_no.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Sizing_JobCardNo.LostFocus, AddressOf ControlLostFocus

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Item_Transfer_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Item_Transfer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub
                ElseIf MessageBox.Show("Do you want to Close?...", "FOR CLOSE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    Close_Form()
                Else
                    Exit Sub
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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Transfer_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Transfer_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Pavu_Transfer_Entry, New_Entry, Me, con, "Pavu_transfer_Head", "Pavu_transfer_Code", NewCode, "Pavu_transfer_Date", "(Pavu_transfer_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


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

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Pavu_transfer_Head", "Pavu_Transfer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Pavu_Transfer_Code, Company_IdNo, for_OrderBy", trans)

            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()

            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Pavu_Stock) = 1 Then

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno    , EndsCount_IdNo ) " &
                                          " Select                               'PAVU', Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_Idno, EndsCount_IdNo from Stock_Pavu_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and DeliveryTo_Idno <> 0"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Pavu_transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Transfer_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Pavu_Stock) = 1 Then
                If Common_Procedures.Check_is_Negative_Stock_Status(con, trans) = True Then Exit Sub
            End If

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

        If msk_Date.Enabled = True And msk_Date.Visible = True Then msk_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_Party.DataSource = dt1
            cbo_Filter_Party.DisplayMember = "Ledger_DisplayName"





            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_Party.Text = ""

            cbo_Filter_Party.SelectedIndex = -1

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

            da = New SqlClient.SqlDataAdapter("select top 1 Pavu_Transfer_No from Pavu_transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby,Pavu_Transfer_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Pavu_Transfer_No from Pavu_transfer_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby,Pavu_Transfer_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Pavu_Transfer_No from Pavu_transfer_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc,Pavu_Transfer_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Pavu_Transfer_No from Pavu_transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc,Pavu_Transfer_No desc", con)
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

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Pavu_transfer_Head", "Pavu_Transfer_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red


            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Pavu_transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Pavu_transfer_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Pavu_transfer_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("Pavu_transfer_Date").ToString
                End If
            End If
            dt1.Clear()

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus() : msk_Date.SelectionStart = 0

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

            Da = New SqlClient.SqlDataAdapter("select Pavu_Transfer_No from Pavu_transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Transfer_Code = '" & Trim(RefCode) & "'", con)
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


        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Transfer_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Transfer_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Pavu_Transfer_Entry, New_Entry, Me) = False Then Exit Sub


        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Pavu_Transfer_No from Pavu_transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Transfer_Code = '" & Trim(RecCode) & "'", con)
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
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim EndsTo_ID As Integer = 0
        Dim EndsFr_ID As Integer = 0
        Dim Stk_DelvIdNo As Integer = 0, Stk_RecIdNo As Integer = 0
        Dim Ledtype As String = ""

        Dim vOrdByNo As String = ""
        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.Pavu_Transfer_Entry, New_Entry) = False Then Exit Sub

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Pavu_Transfer_Entry, New_Entry, Me, con, "Pavu_transfer_Head", "Pavu_transfer_Code", NewCode, "Pavu_transfer_Date", "(Pavu_transfer_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Pavu_transfer_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Pavu_transfer_No desc", dtp_Date.Value.Date) = False Then Exit Sub


        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
            Exit Sub
        End If

        LedFrom_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyFrom.Text)
        If LedFrom_ID = 0 Then
            MessageBox.Show("Invalid Item FromName", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_PartyFrom.Enabled And cbo_PartyFrom.Visible Then cbo_PartyFrom.Focus()
            Exit Sub
        End If

        LedTo_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyTo.Text)
        If LedTo_ID = 0 Then
            MessageBox.Show("Select Party To Name!...", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            cbo_PartyTo.Focus()
            Exit Sub
        End If

        EndsFr_ID = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCountFrom.Text)
        EndsTo_ID = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndscountTo.Text)

        lbl_UserName.Text = Common_Procedures.User.IdNo

        If Val(txt_MetersFrom.Text) < 0 Then
            MessageBox.Show("Invalid MetersFrom", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_MetersFrom.Enabled Then txt_MetersFrom.Focus()
            Exit Sub
        End If
        If Val(txt_MetersTo.Text) < 0 Then
            MessageBox.Show("Invalid Meters To", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_MetersTo.Enabled Then txt_MetersTo.Focus()
            Exit Sub
        End If


        If Trim(cbo_weaving_job_no.Text) <> "" Then
            If Common_Procedures.Cross_Checking_For_Weaving_Job_Code_For_Selecion(con, Val(LedFrom_ID), Trim(cbo_weaving_job_no.Text), Nothing, Val(EndsFr_ID)) = True Then
                MessageBox.Show("MisMatch of Party Job No Details", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_PartyFrom.Enabled Then cbo_PartyFrom.Focus()
                Exit Sub
            End If
        End If
        If Trim(cbo_Sizing_JobCardNo.Text) <> "" Then
            If Common_Procedures.Cross_Checking_For_Sizing_Job_Code_For_Selecion(con, Val(LedFrom_ID), Trim(cbo_Sizing_JobCardNo.Text), Nothing, Val(EndsFr_ID)) = True Then
                MessageBox.Show("MisMatch of Party Job No Details", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_PartyFrom.Enabled Then cbo_PartyFrom.Focus()
                Exit Sub
            End If
        End If

        If Common_Procedures.settings.Sales_OrderNumber_compulsory_in_ALLEntry_Status = 1 Then
            If Trim(cbo_ClothSales_OrderCode_forSelection_From.Text) = "" Then
                MessageBox.Show("Invalid Sales Order No From", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_ClothSales_OrderCode_forSelection_From.Enabled And cbo_ClothSales_OrderCode_forSelection_From.Visible Then cbo_ClothSales_OrderCode_forSelection_From.Focus()
                Exit Sub
            End If
        End If

        If Common_Procedures.settings.Sales_OrderNumber_compulsory_in_ALLEntry_Status = 1 Then
            If Trim(cbo_ClothSales_OrderCode_forSelection_To.Text) = "" Then
                MessageBox.Show("Invalid Sales Order No To", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_ClothSales_OrderCode_forSelection_To.Enabled And cbo_ClothSales_OrderCode_forSelection_To.Visible Then cbo_ClothSales_OrderCode_forSelection_To.Focus()
                Exit Sub
            End If
        End If


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Pavu_transfer_Head", "Pavu_Transfer_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@PavuDate", Convert.ToDateTime(msk_Date.Text))


            If New_Entry = True Then
                cmd.CommandText = "Insert into Pavu_transfer_Head ( Pavu_Transfer_Code     ,                 Company_IdNo      ,          Pavu_Transfer_No      ,                           for_OrderBy                                  , Pavu_Transfer_Date ,         Ledger_IdNo     ,   EndsCountIdNo_From  ,      EndsCountIdNo_To  ,            Meters_From          ,            Meters_To          ,             User_idNo          ,         LedgerTo_IdNo   ,     Weaving_JobCode_forSelection         ,Sizing_JobCode_forSelection           ,               Remarks             ,                   ClothSales_OrderCode_forSelection_From       ,                  ClothSales_OrderCode_forSelection_To            ) " &
                                  "Values                         ('" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & " , '" & Trim(lbl_RefNo.Text) & "' , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",      @PavuDate     , " & Val(LedFrom_ID) & " ," & Val(EndsFr_ID) & " , " & Val(EndsTo_ID) & " ," & Val(txt_MetersFrom.Text) & " ," & Val(txt_MetersTo.Text) & " , " & Val(lbl_UserName.Text) & " , " & Val(LedTo_ID) & "  ,'" & Trim(cbo_weaving_job_no.Text) & "' , '" & Trim(cbo_Sizing_JobCardNo.Text) & "', '" & Trim(txt_remarks.Text) & "' , '" & Trim(cbo_ClothSales_OrderCode_forSelection_From.Text) & "',  '" & Trim(cbo_ClothSales_OrderCode_forSelection_To.Text) & "'   )"
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Pavu_transfer_Head", "Pavu_Transfer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Pavu_Transfer_Code, Company_IdNo, for_OrderBy", tr)

                If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Pavu_Stock) = 1 Then
                    cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno    , EndsCount_IdNo ) " &
                                          " Select                               'PAVU', Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_Idno, EndsCount_IdNo from Stock_Pavu_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and DeliveryTo_Idno <> 0"
                    cmd.ExecuteNonQuery()
                End If

                cmd.CommandText = "Update Pavu_transfer_Head set Pavu_Transfer_Date = @PavuDate, Ledger_Idno = " & Val(LedFrom_ID) & ", EndsCountIdno_From = " & Val(EndsFr_ID) & " , EndsCountIdNo_To = " & Val(EndsTo_ID) & " , Meters_From = " & Val(txt_MetersFrom.Text) & ", Meters_to = " & Val(txt_MetersTo.Text) & "  ,  User_IdNo =  " & Val(lbl_UserName.Text) & " , LedgerTo_IdNo = " & Val(LedTo_ID) & "  ,Weaving_JobCode_forSelection =  '" & Trim(cbo_weaving_job_no.Text) & "' ,Sizing_JobCode_forSelection = '" & Trim(cbo_Sizing_JobCardNo.Text) & "' , Remarks = '" & Trim(txt_remarks.Text) & "' , ClothSales_OrderCode_forSelection_From = '" & Trim(cbo_ClothSales_OrderCode_forSelection_From.Text) & "' , ClothSales_OrderCode_forSelection_To = '" & Trim(cbo_ClothSales_OrderCode_forSelection_To.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Pavu_Transfer_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()


            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Pavu_transfer_Head", "Pavu_Transfer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Pavu_Transfer_Code, Company_IdNo, for_OrderBy", tr)

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Partcls = "Pavu Tfr : Ref.No. " & Trim(lbl_RefNo.Text) & " Remarks : " & Trim(txt_remarks.Text)
            PBlNo = Trim(lbl_RefNo.Text)


            If Val(txt_MetersTo.Text) > 0 Then

                Ledtype = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(LedFrom_ID)) & ")", , tr)

                Stk_DelvIdNo = 0 : Stk_RecIdNo = 0
                If Trim(UCase(Ledtype)) = "JOBWORKER" Then
                    Stk_DelvIdNo = LedFrom_ID

                Else

                    Stk_RecIdNo = LedFrom_ID

                End If

                Sno = Sno + 1
                cmd.CommandText = "Insert into Stock_Pavu_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno,DeliveryToIdno_ForParticulars,ReceivedFromIdno_ForParticulars, Cloth_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters, Weaving_JobCode_forSelection  , Sizing_JobCode_forSelection , ClothSales_OrderCode_forSelection ) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @PavuDate, " & Str(Val(Stk_DelvIdNo)) & ", " & Str(Val(Stk_RecIdNo)) & ", " & Str(Val(LedFrom_ID)) & "," & Str(Val(LedFrom_ID)) & ", 0, '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & "," & Str(Val(EndsFr_ID)) & ",  0, " & Str(Val(txt_MetersFrom.Text)) & " ,'" & Trim(cbo_weaving_job_no.Text) & "' ,'" & Trim(cbo_Sizing_JobCardNo.Text) & "' , '" & Trim(cbo_ClothSales_OrderCode_forSelection_From.Text) & "'  )"
                cmd.ExecuteNonQuery()

                Ledtype = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(LedTo_ID)) & ")", , tr)
                Stk_DelvIdNo = 0 : Stk_RecIdNo = 0
                If Trim(UCase(Ledtype)) = "JOBWORKER" Then
                    Stk_RecIdNo = LedTo_ID

                Else

                    Stk_DelvIdNo = LedTo_ID

                End If

                Sno = Sno + 1
                cmd.CommandText = "Insert into Stock_Pavu_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno,DeliveryToIdno_ForParticulars,ReceivedFromIdno_ForParticulars, Cloth_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters, Weaving_JobCode_forSelection   , Sizing_JobCode_forSelection , ClothSales_OrderCode_forSelection ) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @PavuDate, " & Str(Val(Stk_DelvIdNo)) & ", " & Str(Val(Stk_RecIdNo)) & ", " & Str(Val(LedFrom_ID)) & "," & Str(Val(LedFrom_ID)) & ", 0, '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & "," & Str(Val(EndsTo_ID)) & ",  0, " & Str(Val(txt_MetersTo.Text)) & ",'" & Trim(cbo_weaving_job_no.Text) & "' ,'" & Trim(cbo_Sizing_JobCardNo.Text) & "', '" & Trim(cbo_ClothSales_OrderCode_forSelection_To.Text) & "'  )"
                cmd.ExecuteNonQuery()

            End If

            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Pavu_Stock) = 1 Then

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno      , EndsCount_IdNo ) " &
                                          " Select                               'PAVU', Reference_Code, Reference_Date, Company_IdNo, ReceivedFrom_Idno, EndsCount_IdNo from Stock_Pavu_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and ReceivedFrom_Idno <> 0"
                cmd.ExecuteNonQuery()

                If Common_Procedures.Check_is_Negative_Stock_Status(con, tr) = True Then Exit Sub
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

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()



    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyFrom.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )  and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyFrom.KeyDown
        cbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyFrom, msk_Date, cbo_PartyTo, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )  and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyFrom.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyFrom, cbo_PartyTo, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )  and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyFrom.KeyUp
        If e.Control = False And e.KeyValue = 17 And cbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = ""

            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyFrom.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_EndsCountFrom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EndsCountFrom.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "EndsCount_Name")

    End Sub


    Private Sub cbo_EndsCountFrom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCountFrom.KeyDown
        cbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EndsCountFrom, cbo_PartyTo, cbo_EndscountTo, "EndsCount_Head", "EndsCount_Name", "", "EndsCount_Name")

    End Sub

    Private Sub cbo_EndsCountFrom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EndsCountFrom.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EndsCountFrom, cbo_EndscountTo, "EndsCount_Head", "EndsCount_Name", "", "EndsCount_Name")

    End Sub

    Private Sub cbo_EndscountTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EndscountTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "EndsCount_Name")

    End Sub

    Private Sub cbo_EndsCountTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndscountTo.KeyDown
        cbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EndscountTo, cbo_EndsCountFrom, txt_MetersFrom, "EndsCount_Head", "EndsCount_Name", "", "EndsCount_Name")

    End Sub

    Private Sub cbo_EndsCountTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EndscountTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EndscountTo, txt_MetersFrom, "EndsCount_Head", "EndsCount_Name", "", "EndsCount_Name")

    End Sub

    Private Sub cbo_EndsCountTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndscountTo.KeyUp
        If e.Control = False And e.KeyValue = 17 And cbo_KeyDwnVal = e.KeyValue Then
            Dim f As New EndsCount_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_EndscountTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub cbo_EndsCountFrom_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCountFrom.KeyUp
        If e.Control = False And e.KeyValue = 17 And cbo_KeyDwnVal = e.KeyValue Then
            Dim f As New EndsCount_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_EndsCountFrom.Name
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
        Me.Close()
    End Sub


    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim Led_IdNo As Integer, procfm_IdNo As Integer
        Dim Condt As String = ""


        Try

            Condt = ""
            Led_IdNo = 0
            procfm_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Pavu_Transfer_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Pavu_Transfer_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Pavu_Transfer_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_Party.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_Party.Text)
            End If


            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_Idno = " & Str(Val(Led_IdNo))
            End If



            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as PartyName ,c.EndsCount_Name as EndsCountName_From , d.EndsCount_Name as EndsCountName_To from Pavu_transfer_Head a INNER JOIN Ledger_Head b ON a.Ledger_Idno = b.Ledger_IdNo LEFT OUTER JOIN  EndsCount_Head c ON a.EndsCountIdno_From = c.EndsCount_IdNo LEFT OUTER JOIN  EndsCount_Head D ON a.EndsCountIdno_From = d.EndsCount_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Pavu_Transfer_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Pavu_Transfer_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Pavu_Transfer_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Pavu_Transfer_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("PartyName").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Meters_From").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meters_to").ToString), "########0.00")

                Next i

            End If

            dt2.Clear()
            dt2.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

    End Sub

    Private Sub cbo_Filter_Party_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Party.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or AccountsGroup_IdNo = 10 )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_Party_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Party.KeyDown
      
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Party, dtp_Filter_ToDate, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or AccountsGroup_IdNo = 10 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_Party_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Party.KeyPress
      
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Party, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_IdNo = 0 or AccountsGroup_IdNo = 10 )", "(Ledger_idno = 0)")

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
            'btn_save.Focus()
            If cbo_weaving_job_no.Visible = True Then
                cbo_weaving_job_no.Focus()
            ElseIf cbo_Sizing_JobCardNo.Visible = True Then
                cbo_Sizing_JobCardNo.Focus()
            ElseIf cbo_ClothSales_OrderCode_forSelection_From.Visible = True Then
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
            If cbo_weaving_job_no.Visible = True Then
                cbo_weaving_job_no.Focus()
            ElseIf cbo_Sizing_JobCardNo.Visible = True Then
                cbo_Sizing_JobCardNo.Focus()
            ElseIf cbo_ClothSales_OrderCode_forSelection_From.Visible = True Then
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
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-----
    End Sub


    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        print_record()
    End Sub

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        cbo_KeyDwnVal = e.KeyValue


        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If

    End Sub

    Private Sub msk_Date_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_Date.Text = Date.Today
            msk_Date.SelectionStart = 0
        End If
    End Sub


    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        'If e.Control = False And e.KeyValue = 17 And cbo_KeyDwnVal = e.KeyValue Then
        '    msk_Date.Text = Date.Today
        'End If
        If e.KeyCode = 107 Then
            msk_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Date.Text))
        ElseIf e.KeyCode = 109 Then
            msk_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Date.Text))
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)

        End If

    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If FrmLdSTS = True Then Exit Sub
        If IsDate(dtp_Date.Text) = True Then
            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_Date.LostFocus
        If FrmLdSTS = True Then Exit Sub
        If IsDate(msk_Date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_Date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_Date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) >= 2000 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_Date.Text)
                End If
            End If

        End If
    End Sub

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And cbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
        End If
    End Sub

    Private Sub cbo_PartyTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )  and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyTo, cbo_PartyFrom, cbo_EndsCountFrom, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )  and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyTo, cbo_EndsCountFrom, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or Ledger_Type = 'SIZING' or Ledger_Type = 'REWINDING' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )  and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyTo.KeyUp
        If e.Control = False And e.KeyValue = 17 And cbo_KeyDwnVal = e.KeyValue Then
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
    Private Sub cbo_weaving_job_no_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_weaving_job_no.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_weaving_job_no, txt_MetersTo, Nothing, "Weaving_JobCard_Head", "Weaving_JobCode_forSelection", " ledger_idno = " & Val(vLed_ID_Cond) & "  ", "(Ledger_IdNo = 0)")
        If (e.KeyValue = 40 And cbo_Sizing_JobCardNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_Sizing_JobCardNo.Visible Then
                cbo_Sizing_JobCardNo.Focus()
            Else
                txt_remarks.Focus()
            End If

        End If
    End Sub
    Private Sub cbo_weaving_job_no_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_weaving_job_no.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_weaving_job_no, Nothing, "Weaving_JobCard_Head", "Weaving_JobCode_forSelection", " ledger_idno = " & Val(vLed_ID_Cond) & "  ", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If cbo_Sizing_JobCardNo.Visible Then
                cbo_Sizing_JobCardNo.Focus()
            Else
                txt_remarks.Focus()
            End If

        End If
    End Sub
    Private Sub cbo_weaving_job_no_GotFocus(sender As Object, e As EventArgs) Handles cbo_weaving_job_no.GotFocus
        vLed_ID_Cond = 0
        If Trim(cbo_PartyFrom.Text) <> "" Then
            vLed_ID_Cond = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyFrom.Text)
        End If

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaving_JobCard_Head", "Weaving_JobCode_forSelection", " ledger_idno = " & Val(vLed_ID_Cond) & "  ", "(Ledger_IdNo = 0)")
    End Sub
    Private Sub cbo_Sizing_JobCardNo_GotFocus(sender As Object, e As EventArgs) Handles cbo_Sizing_JobCardNo.GotFocus
        vLed_ID_Cond = 0
        If Trim(cbo_PartyFrom.Text) <> "" Then
            vLed_ID_Cond = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyFrom.Text)
        End If

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Sizing_JobCard_Head", "Sizing_JobCode_forSelection", " ledger_idno = " & Val(vLed_ID_Cond) & " ", "(Ledger_IdNo = 0)")
    End Sub
    Private Sub cbo_Sizing_JobCardNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Sizing_JobCardNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Sizing_JobCardNo, txt_remarks, "Sizing_JobCard_Head", "Sizing_JobCode_forSelection", " ledger_idno = " & Val(vLed_ID_Cond) & " ", "(Ledger_IdNo = 0)")
    End Sub
    Private Sub cbo_Sizing_JobCardNo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Sizing_JobCardNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Sizing_JobCardNo, Nothing, txt_remarks, "Sizing_JobCard_Head", "Sizing_JobCode_forSelection", " ledger_idno = " & Val(vLed_ID_Cond) & " ", "(Ledger_IdNo = 0)")
        If (e.KeyValue = 38 And cbo_Sizing_JobCardNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_weaving_job_no.Visible Then
                cbo_weaving_job_no.Focus()
            Else
                txt_MetersTo.Focus()
            End If

        End If
    End Sub
    Private Sub txt_remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_remarks.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If
        If e.KeyCode = 38 Then 'SendKeys.Send("+{TAB}")

            If cbo_ClothSales_OrderCode_forSelection_To.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection_To.Focus()
            ElseIf cbo_Sizing_JobCardNo.Visible = True Then
                cbo_Sizing_JobCardNo.Focus()
            Else
                txt_MetersTo.Focus()
            End If

        End If

    End Sub


    Private Sub txt_remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub

    Private Sub cbo_Sizing_JobCardNo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Sizing_JobCardNo.SelectedIndexChanged

    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_From_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothSales_OrderCode_forSelection_From.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, cbo_ClothSales_OrderCode_forSelection_To, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")

    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_From_Enter(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection_From.Enter
        vCLO_CONDT = "(ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode1) & "' or ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode2) & "' and Order_Close_Status = 0 )"

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_From_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothSales_OrderCode_forSelection_From.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, cbo_ClothSales_OrderCode_forSelection_To, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")

        If (e.KeyValue = 38 And cbo_ClothSales_OrderCode_forSelection_From.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            If cbo_Sizing_JobCardNo.Visible = True Then
                cbo_Sizing_JobCardNo.Focus()
            Else
                txt_MetersTo.Focus()
            End If

        End If



    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_To_Enter(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection_To.Enter
        vCLO_CONDT = "(ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode1) & "' or ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode2) & "' and Order_Close_Status = 0 )"

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_To_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothSales_OrderCode_forSelection_To.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, txt_remarks, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_To_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothSales_OrderCode_forSelection_To.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, cbo_ClothSales_OrderCode_forSelection_From, txt_remarks, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")

    End Sub

    Private Sub txt_MetersTo_TextChanged(sender As Object, e As EventArgs) Handles txt_MetersTo.TextChanged

    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_From_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection_From.SelectedIndexChanged

    End Sub

    Private Sub Pavu_Transfer_LocationChanged(sender As Object, e As EventArgs) Handles Me.LocationChanged

    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_To_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection_To.SelectedIndexChanged

    End Sub
End Class