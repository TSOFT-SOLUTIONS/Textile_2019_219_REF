Public Class Waste_Fabric_Sales

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "WSFBS-"
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

        lbl_InvNo.Text = ""
        lbl_InvNo.ForeColor = Color.Black
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        dtp_Date.Text = ""
        cbo_PartyName.Text = ""
        cbo_ProcessedFabricName.Text = ""
        cbo_SalesAc.Text = ""
        cbo_Colour.Text = ""
        txt_Pcs.Text = ""
        txt_Meters.Text = ""
        txt_weight.Text = ""
        txt_Rate.Text = ""
        lbl_Amount.Text = ""
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

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name  from Waste_Fabric_Sales_head a INNER JOIN Ledger_Head b ON a.LEdger_Idno = b.Ledger_IdNo   Where a.Waste_Fabric_Sales_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_InvNo.Text = dt1.Rows(0).Item("Waste_Fabric_Sales_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Waste_Fabric_Sales_Date").ToString
                cbo_PartyName.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_SalesAc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("SalesAc_IdNo").ToString))
                cbo_ProcessedFabricName.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Fabric_IdNo").ToString))
                cbo_Colour.Text = Common_Procedures.Colour_IdNoToName(con, Val(dt1.Rows(0).Item("Colour_IdNo").ToString))
                txt_Pcs.Text = Val(dt1.Rows(0).Item("Pcs").ToString)
                txt_weight.Text = Format(Val(dt1.Rows(0).Item("Weight").ToString), "########0.00")
                txt_Meters.Text = Format(Val(dt1.Rows(0).Item("Meters").ToString), "########0.00")
                txt_Rate.Text = Format(Val(dt1.Rows(0).Item("Rate").ToString), "########0.00")
                lbl_Amount.Text = Format(Val(dt1.Rows(0).Item("Amount").ToString), "########0.00")
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

    End Sub

    Private Sub Waste_Fabric_Sales_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_SalesAc.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "SALES" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_SalesAc.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ProcessedFabricName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH NAME" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ProcessedFabricName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Colour.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COLOUR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Colour.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Waste_Fabric_Sales_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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



        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_SalesAc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ProcessedFabricName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Colour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Fabric_Name.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Meters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Pcs.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_weight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_SalesAc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ProcessedFabricName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Colour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Fabric_Name.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Meters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Pcs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_weight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus


        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Pcs.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Meters.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Pcs.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Meters.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_weight.KeyPress, AddressOf TextBoxControlKeyPress
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

    Private Sub Waste_Fabric_Sales_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
        'Common_Procedures.Hide_CurrentStock_Display()

    End Sub

    Private Sub Waste_Fabric_Sales_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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

    Public Sub delete_record() Implements Interface_MDIActions.delete_record

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Qa As Windows.Forms.DialogResult

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Item_Transfer_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Item_Transfer_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

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

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Waste_Fabric_Sales_head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Waste_Fabric_Sales_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            'If Val(Common_Procedures.settings.NegativeStock_Restriction) = 1 Then

            '    If Common_Procedures.Check_is_Negative_Stock_Status(con, trans) = True Then Exit Sub

            'End If

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




            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_Fabric_Name.Text = ""
            cbo_Filter_PartyName.Text = ""

            cbo_Filter_Fabric_Name.SelectedIndex = -1
            cbo_Filter_PartyName.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 Waste_Fabric_Sales_No from Waste_Fabric_Sales_head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Waste_Fabric_Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby,Waste_Fabric_Sales_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Waste_Fabric_Sales_No from Waste_Fabric_Sales_head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Waste_Fabric_Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby,Waste_Fabric_Sales_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Waste_Fabric_Sales_No from Waste_Fabric_Sales_head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Waste_Fabric_Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc,Waste_Fabric_Sales_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Waste_Fabric_Sales_No from Waste_Fabric_Sales_head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Waste_Fabric_Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc,Waste_Fabric_Sales_No desc", con)
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

            lbl_InvNo.Text = Common_Procedures.get_MaxCode(con, "Waste_Fabric_Sales_head", "Waste_Fabric_Sales_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_InvNo.ForeColor = Color.Red

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
        Dim RefCode As String

        Try

            inpno = InputBox("Enter Inv.No.", "FOR FINDING...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Waste_Fabric_Sales_No from Waste_Fabric_Sales_head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Waste_Fabric_Sales_Code = '" & Trim(RefCode) & "'", con)
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
                MessageBox.Show("Inv No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Item_Transfer_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Item_Transfer_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub



        Try

            inpno = InputBox("Enter New Inv No.", "FOR NEW INV INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Waste_Fabric_Sales_No from Waste_Fabric_Sales_head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Waste_Fabric_Sales_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Inv No", "DOES NOT INSERT NEW REF...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_InvNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW INV...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim Itfp_ID As Integer = 0, Rac_IdNo As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim EntID As String = ""
        Dim Sales_ID As Integer = 0
        Dim Fb_ID As Integer = 0
        Dim Racfrm_Id As Integer = 0
        Dim Racto_Id As Integer = 0

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.Item_Transfer_Entry, New_Entry) = False Then Exit Sub


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

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
            Exit Sub
        End If

        Fb_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ProcessedFabricName.Text)
        If Fb_ID = 0 Then
            MessageBox.Show("Invalid Fabric Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_SalesAc.Enabled And cbo_SalesAc.Visible Then cbo_SalesAc.Focus()
            Exit Sub
        End If

        If Val(txt_Meters.Text) = 0 Then
            MessageBox.Show("Invalid Meters", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Meters.Enabled Then txt_Meters.Focus()
            Exit Sub
        End If

        Sales_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_SalesAc.Text)
        Col_ID = Common_Procedures.Colour_NameToIdNo(con, cbo_Colour.Text)
        lbl_UserName.Text = Common_Procedures.User.IdNo

        tr = con.BeginTransaction


        Try


            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_InvNo.Text = Common_Procedures.get_MaxCode(con, "Waste_Fabric_Sales_head", "Waste_Fabric_Sales_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@WasteDate", dtp_Date.Value.Date)
            'cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            'cmd.ExecuteNonQuery()


            If New_Entry = True Then
                cmd.CommandText = "Insert into Waste_Fabric_Sales_head (Waste_Fabric_Sales_Code, Company_IdNo,Waste_Fabric_Sales_No, for_OrderBy,Waste_Fabric_Sales_Date,Ledger_Idno ,SalesAc_Idno, Fabric_IdNo , Colour_IdNo , Pcs ,  Meters     , Weight, Rate   ,  Amount , User_idno ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ", @WasteDate, " & Val(Led_ID) & "," & Str(Val(Sales_ID)) & ", " & Str(Val(Fb_ID)) & ", " & Str(Val(Col_ID)) & "," & Val(txt_Pcs.Text) & "," & Val(txt_Meters.Text) & "," & Val(txt_weight.Text) & "," & Val(txt_Rate.Text) & "," & Val(lbl_Amount.Text) & "," & Val(lbl_UserName.Text) & " )"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Waste_Fabric_Sales_head set Waste_Fabric_Sales_Date = @WasteDate, Ledger_Idno = " & Val(Led_ID) & ", SalesAc_Idno =  " & Val(Sales_ID) & ", Fabric_IdNo = " & Str(Val(Fb_ID)) & " , Colour_IdNo = " & Str(Val(Col_ID)) & " , Pcs = " & Val(txt_Pcs.Text) & ", Meters = " & Val(txt_Meters.Text) & ", Weight = " & Val(txt_weight.Text) & ",Rate = " & Val(txt_Rate.Text) & ", Amount = " & Val(lbl_Amount.Text) & " , User_idNo = " & Val(lbl_UserName.Text) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Waste_Fabric_Sales_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()


            End If
            Partcls = "Bill : Inv.No. " & Trim(lbl_InvNo.Text)
            PBlNo = Trim(lbl_InvNo.Text)
            EntID = Trim(Pk_Condition) & Trim(lbl_InvNo.Text)


            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code,         Company_IdNo                       ,           Reference_No        ,                               for_OrderBy                              , Reference_Date  , DeliveryTo_Idno                                           , ReceivedFrom_Idno   ,         Entry_ID     ,       Party_Bill_No  ,       Particulars        , Sl_No ,   Cloth_Idno           ,   Meters_Type5                    ,StockOff_IdNo                                                ,Reject_Pcs                                 ,    Colour_IdNo      ,   Reject_Weight) " & _
                                                " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ",  @WasteDate     , " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ",   0                 , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "'  , 3      ," & Str(Fb_ID) & " ," & Str(Val(txt_Meters.Text)) & ",   " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & " ," & Str(Val(txt_Pcs.Text)) & "," & Str(Col_ID) & "," & Str(Val(txt_weight.Text)) & ") "
            cmd.ExecuteNonQuery()


            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            Dim AcPos_ID As Integer = 0


            AcPos_ID = Led_ID

            vLed_IdNos = AcPos_ID & "|" & Sales_ID
            vVou_Amts = -1 * Val(CSng(lbl_Amount.Text)) & "|" & Val(CSng(lbl_Amount.Text))
            If Common_Procedures.Voucher_Updation(con, "Wst.Fab.Sale", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_InvNo.Text), dtp_Date.Value.Date, "Inv No : " & Trim(lbl_InvNo.Text) & ", Mtrs : " & Trim(Format(Val(txt_Meters.Text), "#########0.00")), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                Throw New ApplicationException(ErrMsg)
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

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, dtp_Date, cbo_SalesAc, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, cbo_SalesAc, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New FinishedProduct_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_ProcessedFabricName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ProcessedFabricName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "(Cloth_Type = 'PROCESSED FABRIC')", "(Cloth_IdNo = 0)")

    End Sub

    Private Sub cbo_ProcessedFabricName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ProcessedFabricName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ProcessedFabricName, cbo_SalesAc, cbo_Colour, "Cloth_Head", "Cloth_Name", "(Cloth_Type = 'PROCESSED FABRIC')", "(Cloth_IdNo = 0)")

    End Sub

    Private Sub cbo_ProcessedFabricName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ProcessedFabricName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ProcessedFabricName, cbo_Colour, "Cloth_Head", "Cloth_Name", "(Cloth_Type = 'PROCESSED FABRIC')", "(Cloth_IdNo = 0)")

    End Sub

    Private Sub cbo_SalesAc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SalesAc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_SalesAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SalesAc, cbo_PartyName, cbo_ProcessedFabricName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_SalesAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SalesAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SalesAc, cbo_ProcessedFabricName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_SalesAc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAc.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_SalesAc.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1

            f.Show()

        End If
    End Sub

    Private Sub cbo_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Colour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")

    End Sub


    Private Sub cbo_Colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Colour, cbo_ProcessedFabricName, txt_Pcs, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")

    End Sub

    Private Sub cbo_Colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Colour.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Colour, txt_Pcs, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            'Show_Item_CurrentStock()
            txt_Pcs.Focus()
        End If
    End Sub

    Private Sub cbo_Colour_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Color_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Colour.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_ProcessedFabricName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ProcessedFabricName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ProcessedFabricName.Name
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
        Dim Led_IdNo As Integer, Fb_IdNo As Integer
        Dim Condt As String = ""

        'Try

        Condt = ""
        Led_IdNo = 0
        Fb_IdNo = 0

        If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
            Condt = "a.Waste_Fabric_Sales_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
        ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
            Condt = "a.Waste_Fabric_Sales_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
        ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
            Condt = "a.Waste_Fabric_Sales_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
        End If

        If Trim(cbo_Filter_PartyName.Text) <> "" Then
            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
        End If
        If Trim(cbo_Filter_Fabric_Name.Text) <> "" Then
            Fb_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Filter_Fabric_Name.Text)
        End If

        If Val(Led_IdNo) <> 0 Then
            Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_idNo = " & Str(Val(Led_IdNo))
        End If

        If Val(Fb_IdNo) <> 0 Then
            Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Fabric_Idno = '" & Trim(Fb_IdNo) & "'"
        End If

        da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name,c.Cloth_Name from Waste_Fabric_Sales_head a INNER JOIN Ledger_Head b on a.Ledger_Idno = b.Ledger_IdNo INNER JOIN Cloth_Head c on C.Cloth_Idno = a.Fabric_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Waste_Fabric_Sales_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Waste_Fabric_Sales_No", con)
        da.Fill(dt2)

        dgv_Filter_Details.Rows.Clear()

        If dt2.Rows.Count > 0 Then

            For i = 0 To dt2.Rows.Count - 1

                n = dgv_Filter_Details.Rows.Add()


                dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Waste_Fabric_Sales_No").ToString
                dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Waste_Fabric_Sales_Date").ToString), "dd-MM-yyyy")
                dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.00")

            Next i

        End If

        dt2.Clear()
        dt2.Dispose()
        da.Dispose()

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

        If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

    End Sub

    Private Sub cbo_Filter_Fabric_Name_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Fabric_Name.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "(Cloth_Type = 'PROCESSED FABRIC')", "(Cloth_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_Fabric_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Fabric_Name.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Fabric_Name, cbo_Filter_PartyName, btn_Filter_Show, "Cloth_Head", "Cloth_Name", "(Cloth_Type = 'PROCESSED FABRIC')", "(Cloth_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_Fabric_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Fabric_Name.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Fabric_Name, btn_Filter_Show, "Cloth_Head", "Cloth_Name", "(Cloth_Type = 'PROCESSED FABRIC')", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_ItemFrom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_RackNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_Fabric_Name, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_RackNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_Fabric_Name, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")

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

    

    Private Sub txt_Weight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_weight.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then
            e.Handled = True
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
        '--
    End Sub

    

    

    Private Sub txt_Pcs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Pcs.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then
            e.Handled = True
        End If
    End Sub
    'Private Sub Show_Item_CurrentStock()
    '    Dim vItemID As Integer

    '    With cbo_PartyName

    '        vItemID = Common_Procedures.Processed_Item_NameToIdNo(con, cbo_PartyName.Text)

    '        If Val(vItemID) = 0 Then Exit Sub

    '        If Val(vItemID) <> 0 Then
    '            Common_Procedures.Show_ProcessedItem_CurrentStock_Display(con, Val(lbl_Company.Tag), Val(Common_Procedures.CommonLedger.Godown_Ac), vItemID)
    '        End If

    '    End With


    'End Sub



    Private Sub txt_Meters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Meters.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Rate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Rate.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
        If e.KeyCode = 38 Then
            'Show_Item_CurrentStock()
            txt_weight.Focus()
        End If
    End Sub

    Private Sub txt_Rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then
            e.Handled = True
        End If

        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If


    End Sub

    Private Sub txt_Rate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Rate.TextChanged
        lbl_Amount.Text = Format((Val(txt_weight.Text) + Val(txt_Rate.Text)), "##########0.00")
    End Sub
End Class