Public Class Processing_Bill_Making
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "FPRBM-"
    Private Pk_Condition2 As String = "FPRTS-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
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

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        cbo_Ledger.Text = ""
        txt_billNo.Text = ""
        txt_addless.Text = ""
        lbl_assesable.Text = ""
        cbo_vataccount.Text = ""
        txt_vat.Text = ""
        lbl_vat.Text = ""
        txt_othercharges.Text = ""
        lbl_billamount.Text = ""
        txt_tds.Text = ""
        lbl_tds.Text = ""
        lbl_netamount.Text = ""
        txt_filter_billNo.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        dgv_Details.Rows.Clear()

        Grid_DeSelect()

        cbo_itemgrey.Visible = False
        cbo_itemfp.Visible = False

        cbo_itemgrey.Tag = -1
        cbo_itemfp.Tag = -1

        cbo_itemgrey.Text = ""
        cbo_itemfp.Text = ""


    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        ' dgv_Details.CurrentCell.Selected = False
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

      
        If Me.ActiveControl.Name <> cbo_itemfp.Name Then
            cbo_itemfp.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_itemgrey.Name Then
            cbo_itemgrey.Visible = False
        End If
       
        If Me.ActiveControl.Name <> dgv_Details_Total.Name Then
            Grid_DeSelect()
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

        'Try

        da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name,c.Ledger_Name as VatAC_Name from Cloth_Processing_BillMaking_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.VatAc_IdNo = c.Ledger_IdNo   Where a.ClothProcess_BillMaking_Code = '" & Trim(NewCode) & "'", con)
        da1.Fill(dt1)

        If dt1.Rows.Count > 0 Then
            lbl_RefNo.Text = dt1.Rows(0).Item("ClothProcess_BillMaking_No").ToString
            dtp_Date.Text = dt1.Rows(0).Item("ClothProcess_BillMaking_Date").ToString
            cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString
            txt_billNo.Text = dt1.Rows(0).Item("Bill_No").ToString
            txt_addless.Text = dt1.Rows(0).Item("Add_Less").ToString
            lbl_assesable.Text = dt1.Rows(0).Item("Assesable_Amount").ToString
            cbo_vataccount.Text = dt1.Rows(0).Item("VatAc_Name").ToString
            txt_vat.Text = dt1.Rows(0).Item("Vat_Percentage").ToString
            lbl_vat.Text = dt1.Rows(0).Item("Vat_Amount").ToString
            txt_othercharges.Text = dt1.Rows(0).Item("Other_Charges").ToString
            lbl_billamount.Text = dt1.Rows(0).Item("Bill_Amount").ToString
            txt_tds.Text = dt1.Rows(0).Item("Tds_Perc").ToString
            lbl_tds.Text = dt1.Rows(0).Item("Tds_Amount").ToString
            lbl_netamount.Text = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "########0.00")
            lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

            da2 = New SqlClient.SqlDataAdapter("select a.*, b. Processed_Item_Name as Grey_Item_name,C.Processed_Item_Name as Fp_Item_Name from Cloth_Processing_BillMaking_Details a INNER JOIN Processed_Item_Head b ON  b.Processed_Item_IdNo = a.Item_Idno LEFT OUTER JOIN Processed_Item_Head C ON c.Processed_Item_IdNo = a.Item_To_Idno where a.Cloth_Processing_BillMaking_Code = '" & Trim(NewCode) & "' Order by Sl_No", con)
            dt2 = New DataTable
            da2.Fill(dt2)

            dgv_Details.Rows.Clear()
            SNo = 0

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Details.Rows.Add()

                    SNo = SNo + 1
                    dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                    dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Dc_Rc_No").ToString
                    dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Grey_Item_Name").ToString
                    dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Fp_Item_Name").ToString
                    dgv_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("BillMaking_Meters").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("BillMaking_Weight").ToString), "########0.000")
                    dgv_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                    dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.000")
                    dgv_Details.Rows(n).Cells(8).Value = dt2.Rows(i).Item("Cloth_Processing_Receipt_Code").ToString
                    dgv_Details.Rows(n).Cells(9).Value = dt2.Rows(i).Item("Cloth_Processing_Receipt_SlNo").ToString

                Next i

            End If

            With dgv_Details_Total
                If .RowCount = 0 Then .Rows.Add()
                .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")

                .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "########0.000")
            End With

            Grid_DeSelect()


            dt2.Clear()


            dt2.Dispose()
            da2.Dispose()

        End If

        dt1.Clear()
        dt1.Dispose()
        da1.Dispose()
        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

        If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

    End Sub

    Private Sub Processing_Bill_Making_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_itemfp.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "FINISHEDPRODUCT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_itemfp.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_itemgrey.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "GREYITEM" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_itemgrey.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_vataccount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_vataccount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Processing_Bill_Making_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

        'da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head Where (Processed_Item_Type = 'FP' or Processed_Item_IdNo = 0) order by Processed_Item_Name", con)
        'da.Fill(dt1)
        'cbo_itemfp.DataSource = dt1
        'cbo_itemfp.DisplayMember = "Processed_Item_Name"

        'da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where b.AccountsGroup_IdNo = 12 and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        'da.Fill(dt3)
        'cbo_vataccount.DataSource = dt3
        'cbo_vataccount.DisplayMember = "Ledger_DisplayName"

        'da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) order by Ledger_DisplayName", con)
        'da.Fill(dt6)
        'cbo_Ledger.DataSource = dt6
        'cbo_Ledger.DisplayMember = "Ledger_DisplayName"

        'da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head Where (Processed_Item_Type = 'GREY' or Processed_Item_IdNo = 0) order by Processed_Item_Name", con)
        'da.Fill(dt7)
        'cbo_itemgrey.DataSource = dt7
        'cbo_itemgrey.DisplayMember = "Processed_Item_Name"

        cbo_itemfp.Visible = False
        cbo_itemfp.Visible = False


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_itemfp.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_itemgrey.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_vataccount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_addless.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_billNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_filter_billNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_othercharges.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_tds.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_vat.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_ItemGrey.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_itemfp.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_itemgrey.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_vataccount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_addless.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_billNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_filter_billNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_othercharges.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_tds.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_vat.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_ItemGrey.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_addless.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Note.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_othercharges.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_vat.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_filter_billNo.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_addless.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Note.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_othercharges.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_billNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_vat.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_filter_billNo.KeyPress, AddressOf TextBoxControlKeyPress


        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
       
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

    Private Sub processing_Bill_Making_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Processing_Bill_Making_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
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
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 4 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_billNo.Focus()

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
                                cbo_Ledger.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 4)

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

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Qa As Windows.Forms.DialogResult

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Processing_BillMaking_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Processing_BillMaking_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.FP_Processing_Bill_Making_Entry, New_Entry, Me, con, "Cloth_Processing_BillMaking_Head", "ClothProcess_BillMaking_Code", NewCode, "ClothProcess_BillMaking_Date", "(ClothProcess_BillMaking_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub



        Qa = MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If Qa = Windows.Forms.DialogResult.No Or Qa = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), trans)

            cmd.CommandText = "Update Cloth_Processing_Receipt_Details set Cloth_Processing_BillMaking_Code = '', Cloth_Processing_BillMaking_Increment = Cloth_Processing_BillMaking_Increment - 1 Where Cloth_Processing_BillMaking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Cloth_Processing_BillMaking_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Processing_BillMaking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Cloth_Processing_BillMaking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_BillMaking_Code = '" & Trim(NewCode) & "'"
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

        End Try

        If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head Where (Processed_Item_Type = 'GREY' or Processed_Item_IdNo = 0) order by Processed_Item_Name", con)
            da.Fill(dt2)
            cbo_Filter_ItemGrey.DataSource = dt2
            cbo_Filter_ItemGrey.DisplayMember = "Processed_Item_Name"


            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ItemGrey.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ItemGrey.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 ClothProcess_BillMaking_No from Cloth_Processing_BillMaking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_BillMaking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, ClothProcess_BillMaking_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 ClothProcess_BillMaking_No from Cloth_Processing_BillMaking_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_BillMaking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, ClothProcess_BillMaking_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 ClothProcess_BillMaking_No from Cloth_Processing_BillMaking_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_BillMaking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, ClothProcess_BillMaking_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 ClothProcess_BillMaking_No from Cloth_Processing_BillMaking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_BillMaking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, ClothProcess_BillMaking_No desc", con)
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

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Cloth_Processing_BillMaking_Head", "ClothProcess_BillMaking_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

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
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Ref.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select ClothProcess_BillMaking_No from Cloth_Processing_BillMaking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_BillMaking_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.FP_Processing_BillMaking_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Processing_BillMaking_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.FP_Processing_Bill_Making_Entry, New_Entry, Me) = False Then Exit Sub





        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW REF INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select ClothProcess_BillMaking_No from Cloth_Processing_BillMaking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_BillMaking_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Ref No", "DOES NOT INSERT NEW REF...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW REF...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_ID As Integer = 0
        Dim Col_ID As Integer = 0
        Dim Itfp_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotAmt As Single, vTotMtrs As Single
        Dim Proc_ID As Integer = 0
        Dim Lot_ID As Integer = 0
        Dim vTotWeight As Single
        Dim Tr_ID As Integer = 0
        Dim itgry_id As Integer = 0, vatac_id As Integer = 0
        Dim PcsChkCode As String = ""


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Processing_BillMaking_Entry, New_Entry) = False Then Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.FP_Processing_Bill_Making_Entry, New_Entry, Me, con, "Cloth_Processing_BillMaking_Head", "ClothProcess_BillMaking_Code", NewCode, "ClothProcess_BillMaking_Date", "(ClothProcess_BillMaking_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_BillMaking_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, ClothProcess_BillMaking_No desc", dtp_Date.Value.Date) = False Then Exit Sub




        If pnl_Back.Enabled = False Then
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

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        lbl_UserName.Text = Common_Procedures.User.IdNo
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        With dgv_Details
            For i = 0 To dgv_Details.RowCount - 1
                If Val(.Rows(i).Cells(4).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0 Then

                    If Trim(dgv_Details.Rows(i).Cells(2).Value) = "" Then
                        MessageBox.Show("Invalid GREY Item", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(2)

                        End If
                        Exit Sub
                    End If

                    If Trim(dgv_Details.Rows(i).Cells(3).Value) = "" Then
                        MessageBox.Show("Invalid FP Item", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(3)

                        End If
                        Exit Sub
                    End If

                    If Val(dgv_Details.Rows(i).Cells(4).Value) = 0 Then
                        MessageBox.Show("Invalid Meters..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled Then dgv_Details.Focus()
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
                        Exit Sub
                    End If


                End If

            Next
        End With

        If Trim(txt_billNo.Text) = "" Then
            MessageBox.Show("Invalid Bill No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_billNo.Enabled Then txt_billNo.Focus()
            Exit Sub
        End If

        vatac_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_vataccount.Text)
        If vatac_id = 0 Then
            MessageBox.Show("Invalid Vat A/c", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_vataccount.Enabled Then cbo_vataccount.Focus()
            Exit Sub
        End If


        Total_Calculation()

        vTotMtrs = 0 : vTotWeight = 0 : vTotAmt = 0

        If dgv_Details_Total.RowCount > 0 Then

            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTotWeight = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
            vTotAmt = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
        End If


        tr = con.BeginTransaction


        Try


            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Cloth_Processing_BillMaking_Head", "ClothProcess_BillMaking_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@BillMakingDate", dtp_Date.Value.Date)


            If New_Entry = True Then
                cmd.CommandText = "Insert into Cloth_Processing_BillMaking_Head(ClothProcess_BillMaking_Code, Company_IdNo, ClothProcess_BillMaking_No, for_OrderBy, ClothProcess_BillMaking_Date, Ledger_IdNo, Bill_No, Add_Less, Assesable_Amount,VatAc_IdNo,Vat_Amount,Vat_Percentage,Other_Charges,Bill_Amount,Tds_Perc,Tds_Amount,Net_Amount,Total_Meters,Total_Weight,Total_Amount  ,  User_idNo ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @BillMakingDate, " & Str(Val(Led_ID)) & ",'" & Trim(txt_billNo.Text) & "'," & Val(txt_addless.Text) & "," & Val(lbl_assesable.Text) & "," & Val(vatac_id) & "," & Val(lbl_vat.Text) & "," & Val(txt_vat.Text) & ", " & Val(txt_othercharges.Text) & ", " & Val(lbl_billamount.Text) & ", " & Str(Val(txt_tds.Text)) & ", " & Str(Val(lbl_tds.Text)) & ",  " & Val(lbl_netamount.Text) & ", " & Str(Val(vTotMtrs)) & ", " & Str(Val(vTotWeight)) & "," & Val(vTotAmt) & "," & Val(lbl_UserName.Text) & ")"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Cloth_Processing_BillMaking_Head set ClothProcess_BillMaking_Date = @BillMakingDate, Ledger_IdNo = " & Val(Led_ID) & ", Bill_No = '" & Trim(txt_billNo.Text) & "' ,Add_Less = " & Val(txt_addless.Text) & ", Assesable_Amount = " & Val(lbl_assesable.Text) & ",VatAc_IdNo = " & (vatac_id) & " ,Vat_Amount = " & Val(lbl_vat.Text) & ", Vat_Percentage = " & Val(txt_vat.Text) & ",Other_Charges = " & Val(txt_othercharges.Text) & " , Bill_Amount = " & Val(lbl_billamount.Text) & ",Tds_Perc = " & Val(txt_tds.Text) & " ,Tds_Amount = " & Val(lbl_tds.Text) & ",Net_Amount = " & Val(lbl_netamount.Text) & ", Total_Meters = " & Val(vTotMtrs) & ",Total_Weight = " & Val(vTotWeight) & " , Total_Amount = " & Val(vTotAmt) & " , User_IdNo = " & Val(lbl_UserName.Text) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothProcess_BillMaking_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Cloth_Processing_Receipt_Details set Cloth_Processing_BillMaking_Code = '', Cloth_Processing_BillMaking_Increment = Cloth_Processing_BillMaking_Increment - 1 Where Cloth_Processing_BillMaking_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Update Cloth_Processing_Receipt_Details set Cloth_Processing_BillMaking_Code = '" & Trim(NewCode) & "', Cloth_Processing_BillMaking_Increment = Cloth_Processing_BillMaking_Increment + 1 Where Cloth_Processing_Receipt_Code = '" & Trim(lbl_RecCode.Text) & "'"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "Delete from Cloth_Processing_BillMaking_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Processing_BillMaking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Partcls = "Bill : Ref.No. " & Trim(lbl_RefNo.Text)
            PBlNo = Trim(lbl_RefNo.Text)



            With dgv_Details
                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(4).Value) <> 0 Then
                        Sno = Sno + 1
                        itgry_id = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)
                        Itfp_ID = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Cloth_Processing_BillMaking_Details(Cloth_Processing_BillMaking_Code, Company_IdNo, Cloth_Processing_BillMaking_No, for_OrderBy, Cloth_Processing_BillMaking_Date, Sl_No , Dc_Rc_No , Item_Idno, Item_To_Idno, BillMaking_Meters, BillMaking_Weight, Rate, Amount ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @BillMakingDate," & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(itgry_id)) & ", " & Str(Val(Itfp_ID)) & "," & Val(.Rows(i).Cells(4).Value) & ", " & Val(.Rows(i).Cells(5).Value) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ")"
                        cmd.ExecuteNonQuery()

                        ' If Val(vTotMtrs) > 0 Then
                        'cmd.CommandText = "Insert into Stock_Item_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date,Ledger_IdNo, Party_Bill_No, Sl_No, Particulars, Item_IdNo,Colour_IdNo,Rack_IdNo,Meters) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @BillMakingDate, " & Str(Val(Led_ID)) & ", '" & Trim(PBlNo) & "', " & Str(Val(Sno)) & ",'" & Trim(Partcls) & "' ," & Val(itgry_id) & "," & Val(Col_ID) & ", 0, " & Val(.Rows(i).Cells(4).Value) & " )"
                        'cmd.ExecuteNonQuery()
                        ''End If

                        cmd.CommandText = "Update Cloth_Processing_Receipt_Details set Cloth_Processing_BillMaking_Code = '" & Trim(NewCode) & "', Cloth_Processing_BillMaking_Increment = Cloth_Processing_BillMaking_Increment + 1 Where Cloth_Processing_Receipt_Code = '" & Trim(.Rows(i).Cells(8).Value) & "' and Cloth_Processing_Receipt_Slno = " & Str(Val(.Rows(i).Cells(9).Value))
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), tr)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), tr)

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            If Val(lbl_billamount.Text) <> 0 Then
                vLed_IdNos = Led_ID & "|" & Val(Common_Procedures.CommonLedger.Processing_Charges_Ac)
                vVou_Amts = Val(lbl_billamount.Text) & "|" & -1 * (Val(lbl_billamount.Text))
                If Common_Procedures.Voucher_Updation(con, "Proc.Bill", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_Date.Value.Date, "Bill No. : " & Trim(txt_billNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                    Throw New ApplicationException(ErrMsg)
                    Exit Sub
                End If
            End If

            If Val(lbl_tds.Text) <> 0 Then
                vLed_IdNos = Led_ID & "|" & Val(Common_Procedures.CommonLedger.TDS_Payable_Ac)
                vVou_Amts = -1 * Val(lbl_tds.Text) & "|" & Val(lbl_tds.Text)
                If Common_Procedures.Voucher_Updation(con, "Proc.Tds", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_Date.Value.Date, "Bill No. : " & Trim(txt_billNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                    Throw New ApplicationException(ErrMsg)
                    Exit Sub
                End If
            End If

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()
            If New_Entry = True Then new_record()



            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()



    End Sub

    'Private Sub Item_Grey()
    '    Dim Da As New SqlClient.SqlDataAdapter
    '    Dim Dt As New DataTable
    '    Dim GITID As Integer



    '    GITID = Common_Procedures.Processed_Item_NameToIdNo(con, cbo_itemgrey.Text)



    '    If GITID <> 0 Then

    '        Da = New SqlClient.SqlDataAdapter("select * from Processed_Item_Head where Processed_Item_IdNo = " & Str(Val(GITID)) & " and Processed_Item_Type= 'GREY' ", con)
    '        Da.Fill(Dt)


    '        If Dt.Rows.Count > 0 Then

    '            dgv_Details.CurrentRow.Cells(8).Value = Dt.Rows(0).Item("Meter_Qty").ToString

    '        End If


    '        Dt.Clear()
    '        Dt.Dispose()
    '        Da.Dispose()

    '    End If

    'End Sub

    Private Sub Total_Calculation()
        Dim vTotPcs As Single, vTotMtrs As Single, vtotweight As Single, vtotamt As Single

        Dim i As Integer
        Dim sno As Integer


        vTotPcs = 0 : vTotMtrs = 0 : vtotweight = 0 : sno = 0
        With dgv_Details
            For i = 0 To dgv_Details.Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(0).Value = sno

                If Val(dgv_Details.Rows(i).Cells(4).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(7).Value) <> 0 Then
                    '.Rows(i).Cells(9).Value = Val(dgv_Details.Rows(i).Cells(7).Value) * Val(dgv_Details.Rows(i).Cells(8).Value)

                    vTotMtrs = vTotMtrs + Val(dgv_Details.Rows(i).Cells(4).Value)
                    vtotweight = vtotweight + Val(dgv_Details.Rows(i).Cells(5).Value)
                    vtotamt = vtotamt + Val(dgv_Details.Rows(i).Cells(7).Value)
                End If
            Next
        End With
        If dgv_Details_Total.Rows.Count <= 0 Then dgv_Details_Total.Rows.Add()

        dgv_Details_Total.Rows(0).Cells(4).Value = Format(Val(vTotMtrs), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(5).Value = Format(Val(vtotweight), "#########0.000")
        dgv_Details_Total.Rows(0).Cells(7).Value = Format(Val(vtotamt), "#########0.000")

        NetAmount_Calculation()

    End Sub

    Private Sub NetAmount_Calculation()
        Dim Grsamt As Single = 0
        Dim BlAmt As Single = 0
        Dim NtAmt As Single = 0

        Grsamt = 0
        If dgv_Details_Total.Rows.Count > 0 Then
            Grsamt = Val(dgv_Details_Total.Rows(0).Cells(7).Value)
        End If

        lbl_assesable.Text = Format(Val(Grsamt) + Val(txt_addless.Text), "########0.00")

        lbl_vat.Text = Format(Val(lbl_assesable.Text) * Val(txt_vat.Text) / 100, "#########0.00")

        BlAmt = Val(lbl_assesable.Text) + Val(lbl_vat.Text) + Val(txt_othercharges.Text)
        BlAmt = Format(Val(BlAmt), "#########0")
        lbl_billamount.Text = Format(Val(BlAmt), "#########0.00")

        lbl_tds.Text = Format(Val(lbl_billamount.Text) * Val(txt_tds.Text) / 100, "#########0.00")

        NtAmt = Val(lbl_billamount.Text) - Val(lbl_tds.Text)
        NtAmt = Format(Val(NtAmt), "#########0")
        lbl_netamount.Text = Format(Val(NtAmt), "#########0.00")

    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1)", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, dtp_Date, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1)", "(Ledger_idno = 0)")

        If (e.KeyValue = 40 And cbo_Ledger.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            Else
                txt_billNo.Focus()

            End If
        End If

    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1) ", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to select order:", "FOR ORDER SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

            ElseIf dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

                Else
                    txt_billNo.Focus()

            End If

        End If

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub




    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        With dgv_Details

            If .CurrentCell.ColumnIndex = 4 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
            If .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If
            If .CurrentCell.ColumnIndex = 7 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If

        End With
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        'Dim rect As Rectangle

        With dgv_Details
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            'If e.ColumnIndex = 2 Then

            '    If cbo_itemgrey.Visible = False Or Val(cbo_itemgrey.Tag) <> e.RowIndex Then

            '        cbo_itemfp.Tag = -1
            '        Da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head where Processed_Item_Type = 'GREY ' order by Processed_item_Name", con)
            '        Dt1 = New DataTable
            '        Da.Fill(Dt1)
            '        cbo_itemgrey.DataSource = Dt1
            '        cbo_itemgrey.DisplayMember = "Processed_Item_Name"

            '        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

            '        cbo_itemgrey.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
            '        cbo_itemgrey.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

            '        cbo_itemgrey.Width = rect.Width  ' .CurrentCell.Size.Width
            '        cbo_itemgrey.Height = rect.Height  ' rect.Height
            '        cbo_itemgrey.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

            '        cbo_itemgrey.Tag = Val(e.RowIndex)
            '        cbo_itemgrey.Visible = True

            '        cbo_itemgrey.BringToFront()
            '        cbo_itemgrey.Focus()



            '    End If


            'Else

            '    cbo_itemgrey.Visible = False

            'End If

            'If e.ColumnIndex = 3 Then

            '    If cbo_itemfp.Visible = False Or Val(cbo_itemfp.Tag) <> e.RowIndex Then

            '        cbo_itemfp.Tag = -1
            '        Da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head where Processed_Item_Type = 'FP' order by Processed_Item_Name", con)
            '        Dt2 = New DataTable
            '        Da.Fill(Dt2)
            '        cbo_itemfp.DataSource = Dt2
            '        cbo_itemfp.DisplayMember = "Procesed_Item_Name"

            '        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

            '        cbo_itemfp.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
            '        cbo_itemfp.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
            '        cbo_itemfp.Width = rect.Width  ' .CurrentCell.Size.Width
            '        cbo_itemfp.Height = rect.Height  ' rect.Height

            '        cbo_itemfp.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

            '        cbo_itemfp.Tag = Val(e.RowIndex)
            '        cbo_itemfp.Visible = True

            '        cbo_itemfp.BringToFront()
            '        cbo_itemfp.Focus()


            '    End If

            'Else

            '    cbo_itemfp.Visible = False


            'End If

        End With

    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details

            If .CurrentCell.ColumnIndex = 4 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
            If .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If

            If .CurrentCell.ColumnIndex = 7 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Dim vtotamt As Single = 0

        Try
            With dgv_Details
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If e.ColumnIndex = 4 Or e.ColumnIndex = 5 Or e.ColumnIndex = 6 Then

                            vtotamt = Val(dgv_Details.Rows(e.RowIndex).Cells(5).Value) * Val(dgv_Details.Rows(e.RowIndex).Cells(6).Value)

                            dgv_Details.Rows(e.RowIndex).Cells(7).Value = Format(Val(vtotamt), "#########0.00")

                            Total_Calculation()

                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '----
        End Try


    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown



        With dgv_Details

            If e.KeyCode = Keys.Up Then
                If .CurrentCell.RowIndex = 0 Then
                    .CurrentCell.Selected = False
                    e.SuppressKeyPress = True
                    e.Handled = True


                    cbo_Ledger.Focus()
                End If
            End If

            If e.KeyCode = Keys.Down Then
                If .CurrentCell.RowIndex = .RowCount - 1 Then
                    .CurrentCell.Selected = False
                    e.SuppressKeyPress = True
                    e.Handled = True

                End If



            End If



        End With
    End Sub
    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
    End Sub
    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_details.KeyPress
        If Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 4 Or Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 5 Or Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 6 Or Val(dgv_Details.CurrentCell.ColumnIndex.ToString) = 7 Then

            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                e.Handled = True
            End If
        End If
    End Sub
    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                n = .CurrentRow.Index

                If .Rows.Count = 1 Then
                    For i = 0 To .Columns.Count - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else

                    .Rows.RemoveAt(n)

                End If

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With

            Total_Calculation()
        End If

    End Sub

    Private Sub cbo_vataccount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_vataccount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " ( Ledger_idno = 0 or AccountsGroup_IdNo = 12 and Verified_Status = 1) ", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_vataccount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_vataccount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_vataccount, txt_addless, txt_vat, "Ledger_AlaisHead", "Ledger_DisplayName", " ( Ledger_idno = 0 or AccountsGroup_IdNo = 12 and Verified_Status = 1) ", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_vataccount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_vataccount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_vataccount, txt_vat, "Ledger_AlaisHead", "Ledger_DisplayName", "  ( Ledger_idno = 0 or AccountsGroup_IdNo = 12 and Verified_Status = 1)  ", "(Ledger_idno = 0)")
       
    End Sub
    Private Sub cbo_itemgrey_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_itemgrey.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'GREY' and Verified_Status = 1)", "(Processed_Item_idno = 0)")

    End Sub

    Private Sub cbo_itemgrey_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_itemgrey.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_itemgrey, Nothing, cbo_itemfp, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'GREY' and Verified_Status = 1)", "(Processed_Item_idno = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_itemgrey.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_itemgrey.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_itemgrey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_itemgrey.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_itemgrey, cbo_itemfp, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'GREY' and Verified_Status = 1)", "(Processed_Item_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub

    Private Sub cbo_itemgrey_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_itemgrey.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Grey_Item_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_itemgrey.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub Cbo_itemgrey_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_itemgrey.TextChanged
        Try
            If cbo_itemgrey.Visible Then
                With dgv_Details
                    If Val(cbo_itemgrey.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_itemgrey.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_itemfp_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_itemfp.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP' and Verified_Status = 1)", "(Processed_Item_idno = 0)")

    End Sub

   
    Private Sub cbo_itemfp_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_itemfp.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_itemfp, cbo_itemgrey, Nothing, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP' and Verified_Status = 1)", "(Processed_Item_idno = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_itemfp.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_itemfp.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_itemfp_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_itemfp.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_itemfp, Nothing, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'FP' and Verified_Status = 1)", "(Processed_Item_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub

    Private Sub cbo_itemfp_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_itemfp.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New FinishedProduct_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_itemfp.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub cbo_itemfp_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_itemfp.TextChanged
        Try
            If cbo_itemfp.Visible Then
                With dgv_Details
                    If Val(cbo_itemfp.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_itemfp.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

 
    Private Sub txt_billno_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_billNo.KeyDown
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                dgv_Details.CurrentCell.Selected = True
            Else
                cbo_Ledger.Focus()
            End If
        End If
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub dgv_Details_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.GotFocus
        'dgv_Details.Focus()
        ' dgv_Details.CurrentCell.Selected = True
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim Led_IdNo As Integer, procit_IdNo As Integer
        Dim Condt As String = ""


        Try

            Condt = ""
            Led_IdNo = 0
            procit_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.ClothProcess_BillMaking_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.ClothProcess_BillMaking_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.ClothProcess_BillMaking_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_ItemGrey.Text) <> "" Then
                procit_IdNo = Common_Procedures.Processed_Item_NameToIdNo(con, cbo_Filter_ItemGrey.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If


            If Val(procit_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.ClothProcess_BillMaking_Code IN (select z1.Cloth_Processing_BillMaking_Code from Cloth_Processing_BillMaking_Details z1 where z1.Item_Idno = " & Str(Val(procit_IdNo)) & ")"
            End If

            If Trim(txt_filter_billNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Bill_No = '" & Trim(txt_filter_billNo.Text) & "'"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Cloth_Processing_BillMaking_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothProcess_BillMaking_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.ClothProcess_BillMaking_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("ClothProcess_BillMaking_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("ClothProcess_BillMaking_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    ' dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Processed_Item_Name").ToString
                    ' dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Process_Name").ToString
                    'dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Delivery_Pcs").ToString)
                    ' dgv_Filter_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Delivery_Qty").ToString)
                    dgv_Filter_Details.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.000")

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

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1)", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, txt_filter_billNo, cbo_Filter_ItemGrey, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1)", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ItemGrey, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_ItemGrey_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ItemGrey.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'GREY')", "(Processed_Item_idno = 0)")

    End Sub

    Private Sub cbo_Filter_ItemGrey_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ItemGrey.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ItemGrey, cbo_Filter_PartyName, btn_Filter_Show, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'GREY')", "(Processed_Item_idno = 0)")

    End Sub

    Private Sub cbo_Filter_ItemGrey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ItemGrey.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ItemGrey, btn_Filter_Show, "Processed_Item_Head", "Processed_Item_Name", "(Processed_Item_idno = 0 or Processed_Item_Type = 'GREY')", "(Processed_Item_idno = 0)")

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

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub txt_tds_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_tds.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If

        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub


    Private Sub txt_tds_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_tds.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub txt_addless_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_addless.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_addless_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_addless.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_vat_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_vat.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_vat_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_vat.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_othercharges_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_othercharges.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_othercharges_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_othercharges.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_tds_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_tds.TextChanged
        NetAmount_Calculation()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '----
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim Ent_Rate As Single = 0
        Dim Ent_Wgt As Single = 0
        Dim Ent_Pcs As Single = 0
        Dim NR As Single = 0

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PAVU...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection

            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*, b.Processed_Item_Name as Grey_Item_name, C.Processed_Item_Name as Fp_Item_Name , d.Rate As Ent_Rate from Cloth_Processing_Receipt_Details a LEFT OUTER JOIN Processed_Item_Head b ON  b.Processed_Item_IdNo = a.Item_Idno LEFT OUTER JOIN Processed_Item_Head C ON c.Processed_Item_IdNo = a.Item_To_Idno  LEFT OUTER JOIN Cloth_Processing_BillMaking_Details d ON d.Cloth_Processing_BillMaking_Code = a.Cloth_Processing_Receipt_Code where a.Cloth_Processing_BillMaking_Code = '" & Trim(NewCode) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.Cloth_Processing_Receipt_Date, a.for_orderby, a.Cloth_Processing_Receipt_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    Ent_Rate = 0

                    If IsDBNull(Dt1.Rows(i).Item("Ent_Rate").ToString) = False Then
                        Ent_Pcs = Val(Dt1.Rows(i).Item("Ent_Rate").ToString)
                    End If

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Cloth_Processing_Receipt_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Cloth_Processing_Receipt_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Grey_Item_name").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Fp_Item_Name").ToString
                    .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Receipt_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Receipt_Weight").ToString), "#########0.000")
                    .Rows(n).Cells(7).Value = "1"
                    .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Cloth_Processing_Receipt_Code").ToString
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Cloth_Processing_Receipt_Slno").ToString
                    .Rows(n).Cells(10).Value = Ent_Rate

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            Da = New SqlClient.SqlDataAdapter("select a.*, b.Processed_Item_Name as Grey_Item_name, C.Processed_Item_Name as Fp_Item_Name , d.Rate As Ent_Rate from Cloth_Processing_Receipt_Details a LEFT OUTER JOIN Processed_Item_Head b ON  b.Processed_Item_IdNo = a.Item_Idno LEFT OUTER JOIN Processed_Item_Head C ON c.Processed_Item_IdNo = a.Item_To_Idno  LEFT OUTER JOIN Cloth_Processing_BillMaking_Details d ON d.Cloth_Processing_BillMaking_Code = a.Cloth_Processing_Receipt_Code where a.Cloth_Processing_BillMaking_Code = '' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.Cloth_Processing_Receipt_Date, a.for_orderby, a.Cloth_Processing_Receipt_No", con)
            Dt1 = New DataTable
            NR = Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)


                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Cloth_Processing_Receipt_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Cloth_Processing_Receipt_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Grey_Item_name").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Fp_Item_Name").ToString
                    .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Receipt_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(i).Item("Receipt_Weight").ToString), "#########0.000")
                    .Rows(n).Cells(7).Value = ""
                    .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Cloth_Processing_Receipt_Code").ToString
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Cloth_Processing_Receipt_Slno").ToString
                    .Rows(n).Cells(10).Value = 0
                Next

            End If
            Dt1.Clear()

        End With

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        dgv_Selection.Focus()

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Piece(e.RowIndex)
    End Sub

    Private Sub Select_Piece(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(7).Value = (Val(.Rows(RwIndx).Cells(7).Value) + 1) Mod 2
                If Val(.Rows(RwIndx).Cells(7).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next


                Else
                    .Rows(RwIndx).Cells(7).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next

                End If


            End If

        End With

    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        Dim n As Integer

        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Selection.CurrentCell.RowIndex >= 0 Then

                n = dgv_Selection.CurrentCell.RowIndex

                Select_Piece(n)

                e.Handled = True

            End If
        End If
       
    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Close_Receipt_Selection()
    End Sub

    Private Sub Close_Receipt_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0

        dgv_Details.Rows.Clear()

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(7).Value) = 1 Then

                lbl_RecCode.Text = dgv_Selection.Rows(i).Cells(8).Value

                If dgv_Selection.RowCount > 0 Then

                    For j = 0 To dgv_Selection.RowCount - 1

                        n = dgv_Details.Rows.Add()


                        sno = sno + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(sno)

                        dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(1).Value
                        dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(3).Value
                        dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(4).Value
                        dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(5).Value
                        dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(6).Value
                        dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(10).Value
                        dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(6).Value * dgv_Selection.Rows(i).Cells(10).Value
                        dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(8).Value
                        dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(9).Value

                    Next

                End If
                Dt1.Clear()

                Total_Calculation()


                Exit For

            End If

        Next

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If dgv_Details.Enabled And dgv_Details.Visible Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(6)
                dgv_Details.CurrentCell.Selected = True

            Else
                txt_billNo.Focus()

            End If
        End If

    End Sub

   
   
    Private Sub cbo_vataccount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_vataccount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.MDI_LedType = ""
            Dim f As New LedgerCreation_Processing

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_vataccount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub dgv_Details_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellContentClick

    End Sub
End Class