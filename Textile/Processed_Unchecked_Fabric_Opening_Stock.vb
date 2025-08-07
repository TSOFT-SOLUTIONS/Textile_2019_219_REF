Public Class Processed_Unchecked_Fabric_Opening_Stock
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)

    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "OPENI-"
    Private OpYrCode As String = ""
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer

    Private Property dtp_Filter_Fromdate As Object

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        cbo_ProcessName.Text = ""
        cbo_Processed_Fabric_Name.Text = ""
        cbo_ColourName.Text = ""

        cbo_Filter_Process.Text = ""
        cbo_FilterProcessedFabric.Text = ""

        txt_Meters.Text = ""
        txt_Weight.Text = ""
        txt_Pcs.Text = ""

        If Filter_Status = False Then

            cbo_FilterProcessedFabric.Text = ""
            cbo_Filter_Process.Text = ""
            cbo_FilterProcessedFabric.SelectedIndex = -1
            cbo_Filter_Process.SelectedIndex = -1
            cbo_Filter_Colour.Text = ""
            cbo_Filter_Colour.SelectedIndex = -1

            dgv_Filter_Details.Rows.Clear()
        End If

        ''cbo_ProcessName.Enabled = True
        ''cbo_ProcessName.BackColor = Color.White

        ''cbo_Processed_Fabric_Name.Enabled = True
        ''cbo_Processed_Fabric_Name.BackColor = Color.White

        ''txt_Quantity.Enabled = True
        ''txt_Quantity.BackColor = Color.White

        ''txt_Meters.Enabled = True
        ''txt_Meters.BackColor = Color.White

        ''txt_Weight.Enabled = True
        ''txt_Weight.BackColor = Color.White

        NoCalc_Status = False

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

    Private Sub Cloth_Bale_Bundle_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ColourName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COLOUR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ColourName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ProcessName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "PROCESS" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ProcessName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Processed_Fabric_Name.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Processed_Fabric_Name.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Bale_Opening_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Text = ""

        con.Open()


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        AddHandler cbo_Processed_Fabric_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ProcessName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ColourName.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Meters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Weight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Pcs.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_FilterProcessedFabric.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Process.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Colour.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_ProcessName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Processed_Fabric_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ColourName.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Meters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Weight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Pcs.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_FilterProcessedFabric.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Process.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Colour.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Meters.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Weight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Pcs.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Meters.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Pcs.KeyPress, AddressOf TextBoxControlKeyPress



        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
        OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

        Filter_Status = False
        FrmLdSTS = True
        new_record()
    End Sub

    Private Sub Bale_Opening_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name()
    End Sub

    Private Sub Bale_Opening_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
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

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim LockSTS As Boolean = False

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(OpYrCode)

        Try
            da1 = New SqlClient.SqlDataAdapter("select a.*  from Processed_Unchecked_Fabric_Opening_Head a   Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Processed_Unchecked_Fabric_Opening_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Processed_Unchecked_Fabric_Opening_No").ToString
                cbo_ProcessName.Text = Common_Procedures.Process_IdNoToName(con, Val(dt1.Rows(0).Item("Process_IdNo").ToString))
                cbo_Processed_Fabric_Name.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Fabric_IdNo").ToString))
                cbo_ColourName.Text = Common_Procedures.Colour_IdNoToName(con, Val(dt1.Rows(0).Item("Colour_IdNo").ToString))
                txt_Pcs.Text = Val(dt1.Rows(0).Item("No_Of_Pcs").ToString)
                If Val(dt1.Rows(0).Item("Meters").ToString) <> 0 Then
                    txt_Meters.Text = Format(Val(dt1.Rows(0).Item("Meters").ToString), "##########0.00")
                End If
                txt_Weight.Text = Format(Val(dt1.Rows(0).Item("Weight").ToString), "##########0.000")

                'LockSTS = False
                'If IsDBNull(dt1.Rows(0).Item("Delivery_Code").ToString) = False Then
                '    If Trim(dt1.Rows(0).Item("Delivery_Code").ToString) <> "" Then
                '        LockSTS = True
                '    End If
                'End If

                'If LockSTS = True Then
                '    cbo_ProcessName.Enabled = False
                '    cbo_ProcessName.BackColor = Color.LightGray

                '    cbo_Processed_Fabric_Name.Enabled = False
                '    cbo_Processed_Fabric_Name.BackColor = Color.LightGray

                '    txt_Quantity.Enabled = False
                '    txt_Quantity.BackColor = Color.LightGray

                '    txt_Meters.Enabled = False
                '    txt_Meters.BackColor = Color.LightGray

                '    txt_Weight.Enabled = False
                '    txt_Weight.BackColor = Color.LightGray

                'End If

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If cbo_Processed_Fabric_Name.Visible And cbo_Processed_Fabric_Name.Enabled Then cbo_Processed_Fabric_Name.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Bale_OpeningStock, "~L~") = 0 And InStr(Common_Procedures.UR.Bale_OpeningStock, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(OpYrCode)

        'Da = New SqlClient.SqlDataAdapter("select count(*) from Processed_Fabric_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_Code = '" & Trim(NewCode) & "' and Delivery_Code <> ''", con)
        'Dt1 = New DataTable
        'Da.Fill(Dt1)
        'If Dt1.Rows.Count > 0 Then
        '    If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
        '        If Val(Dt1.Rows(0)(0).ToString) > 0 Then
        '            MessageBox.Show("Already this bale delivered", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '            Exit Sub
        '        End If
        '    End If
        'End If
        ''Dt1.Clear()

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            'cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Processed_Unchecked_Fabric_Opening_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Unchecked_Fabric_Opening_Code = '" & Trim(NewCode) & "'"
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

            If cbo_Processed_Fabric_Name.Enabled = True And cbo_Processed_Fabric_Name.Visible = True Then cbo_Processed_Fabric_Name.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            cbo_FilterProcessedFabric.Text = ""
            cbo_Filter_Process.Text = ""
            cbo_Filter_Colour.Text = ""


            cbo_FilterProcessedFabric.SelectedIndex = -1
            cbo_Filter_Process.SelectedIndex = -1
            cbo_Filter_Colour.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Filter.BringToFront()
        Pnl_Back.Enabled = False
        If cbo_FilterProcessedFabric.Enabled And cbo_FilterProcessedFabric.Visible Then cbo_FilterProcessedFabric.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Processed_Unchecked_Fabric_Opening_No from Processed_Unchecked_Fabric_Opening_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Unchecked_Fabric_Opening_Code like '%/" & Trim(OpYrCode) & "' Order by for_Orderby, Processed_Unchecked_Fabric_Opening_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Processed_Unchecked_Fabric_Opening_No from Processed_Unchecked_Fabric_Opening_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Unchecked_Fabric_Opening_Code like '%/" & Trim(OpYrCode) & "' Order by for_Orderby, Processed_Unchecked_Fabric_Opening_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Processed_Unchecked_Fabric_Opening_No from Processed_Unchecked_Fabric_Opening_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Unchecked_Fabric_Opening_Code like '%/" & Trim(OpYrCode) & "' Order by for_Orderby desc, Processed_Unchecked_Fabric_Opening_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Processed_Unchecked_Fabric_Opening_No from Processed_Unchecked_Fabric_Opening_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Unchecked_Fabric_Opening_Code like '%/" & Trim(OpYrCode) & "' Order by for_OrderBy desc, Processed_Unchecked_Fabric_Opening_No desc", con)
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

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Processed_Unchecked_Fabric_Opening_Head", "Processed_Unchecked_Fabric_Opening_Code", "for_OrderBy", "", Val(lbl_Company.Tag), OpYrCode)

            lbl_RefNo.ForeColor = Color.Red

            If cbo_Processed_Fabric_Name.Enabled And cbo_Processed_Fabric_Name.Visible Then cbo_Processed_Fabric_Name.Focus()

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

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(OpYrCode)

            Da = New SqlClient.SqlDataAdapter("select Processed_Unchecked_Fabric_Opening_No from Processed_Unchecked_Fabric_Opening_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Unchecked_Fabric_Opening_Code = '" & Trim(RecCode) & "'", con)
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

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW REF INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(OpYrCode)

            Da = New SqlClient.SqlDataAdapter("select Processed_Unchecked_Fabric_Opening_No from Processed_Unchecked_Fabric_Opening_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Unchecked_Fabric_Opening_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Ref No", "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Col_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim Led_ID As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim PFb_ID As Integer = 0
        Dim Proc_ID As Integer = 0
        Dim EntID As String = ""
        Dim OpDate As Date
        Dim stkof_idno As Integer = 0
        Dim Led_type As String = 0

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Piece_OpeningStock, New_Entry) = False Then Exit Sub

        If Pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If IsDate(dtp_Date.Text) = False Then
        '    MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
        '    Exit Sub
        'End If

        'If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
        '    MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
        '    Exit Sub
        'End If

        PFb_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Processed_Fabric_Name.Text)
        If PFb_ID = 0 Then
            MessageBox.Show("Invalid Processed Unchecked Fabric Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Processed_Fabric_Name.Enabled Then cbo_Processed_Fabric_Name.Focus()
            Exit Sub
        End If

        Col_ID = Common_Procedures.Colour_NameToIdNo(con, cbo_ColourName.Text)
        If Col_ID = 0 Then
            MessageBox.Show("Invalid Colour Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_ColourName.Enabled And cbo_ColourName.Visible Then cbo_ColourName.Focus()
            Exit Sub
        End If

        Proc_ID = Common_Procedures.Process_NameToIdNo(con, cbo_ProcessName.Text)
        If Proc_ID = 0 Then
            MessageBox.Show("Invalid Process Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_ProcessName.Enabled And cbo_ProcessName.Visible Then cbo_ProcessName.Focus()
            Exit Sub
        End If

        tr = con.BeginTransaction

        Try

            OpDate = CDate("01-04-" & Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4))
            OpDate = DateAdd(DateInterval.Day, -1, OpDate)

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(OpYrCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Processed_Unchecked_Fabric_Opening_Head", "Processed_Unchecked_Fabric_Opening_Code", "for_OrderBy", "", Val(lbl_Company.Tag), OpYrCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(OpYrCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@OpDate", OpDate)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Processed_Unchecked_Fabric_Opening_Head ( Processed_Unchecked_Fabric_Opening_Code, Company_IdNo, Processed_Unchecked_Fabric_Opening_No, Processed_Unchecked_Fabric_Opening_Date,for_OrderBy,  Colour_IdNo  , Fabric_IdNo, Process_IdNo, No_Of_Pcs,  Meters, Weight ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "',  @OpDate," & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", " & Str(Val(Col_ID)) & "," & Val(PFb_ID) & ", " & Str(Val(Proc_ID)) & ", " & Str(Val(txt_Pcs.Text)) & ",  " & Str(Val(txt_Meters.Text)) & ", " & Str(Val(txt_Weight.Text)) & " ) "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Processed_Unchecked_Fabric_Opening_Head set Processed_Unchecked_Fabric_Opening_Date = @OpDate, Colour_IdNo =  " & Val(Col_ID) & " , Fabric_IdNo = " & Str(Val(PFb_ID)) & ", Process_IdNo = " & Str(Val(Proc_ID)) & ", No_Of_Pcs = " & Str(Val(txt_Pcs.Text)) & ", Meters = " & Str(Val(txt_Meters.Text)) & ", Weight = " & Str(Val(txt_Weight.Text)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Processed_Unchecked_Fabric_Opening_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            Dt1.Dispose()
            Da1.Dispose()

            stkof_idno = Val(Common_Procedures.CommonLedger.Godown_Ac)

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Val(txt_Meters.Text) <> 0 Then
                cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code    ,             Company_IdNo         ,              Reference_No     ,                               for_OrderBy                              , Reference_Date,       StockOff_IdNo      ,                                       DeliveryTo_Idno     , ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No,          Cloth_Idno      ,  Folding   , UnChecked_Meters, Meters_Type1                 , Meters_Type2 , Meters_Type3 , Meters_Type4, Meters_Type5      ,Weight       ) " & _
                                            " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",     @OpDate   , " & Val(stkof_idno) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ",         0        ,    ''   ,       ''     ,     ''     ,   1  , " & Str(Val(PFb_ID)) & ",  0          ,        0        ,  " & Str(Val(txt_Meters.Text)) & " ,      0       ,       0      ,      0      ,     0        ," & Str(Val(txt_Weight.Text)) & ") "
                cmd.ExecuteNonQuery()
            End If

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
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If cbo_Processed_Fabric_Name.Enabled And cbo_Processed_Fabric_Name.Visible Then cbo_Processed_Fabric_Name.Focus()

        End Try

    End Sub

    Private Sub cbo_Processed_Fabric_Name_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Processed_Fabric_Name.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "(Cloth_Type = 'PROCESSED FABRIC')", "(Cloth_IdNo=0)")
    End Sub




    Private Sub cbo_Processed_Fabric_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Processed_Fabric_Name.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Processed_Fabric_Name, Nothing, cbo_ColourName, "Cloth_Head", "Cloth_Name", "(Cloth_Type = 'PROCESSED FABRIC')", "(Cloth_IdNo=0)")
    End Sub

    Private Sub cbo_Processed_Fabric_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Processed_Fabric_Name.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Processed_Fabric_Name, cbo_ColourName, "Cloth_Head", "Cloth_Name", "(Cloth_Type = 'PROCESSED FABRIC')", "(Cloth_IdNo=0)")
    End Sub

    Private Sub cbo_Processed_Fabric_Name_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Processed_Fabric_Name.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Processed_Fabric_Name.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub cbo_ColourName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ColourName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Colour_Head", "Colour_Name", "", "Colour_IdNo")
    End Sub

    Private Sub cbo_ColourName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ColourName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ColourName, cbo_Processed_Fabric_Name, cbo_ProcessName, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ColourName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ColourName, cbo_ProcessName, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")
    End Sub
    Private Sub cbo_Colourname_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ColourName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Color_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ColourName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1

            f.Show()

        End If
    End Sub

    Private Sub cbo_ProcessName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ProcessName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Process_Head", "Process_Name", "", "(Process_Idno=0)")
    End Sub
    Private Sub cbo_ProcessName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ProcessName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ProcessName, cbo_ColourName, txt_Pcs, "Process_Head", "Process_Name", "", "(Process_Idno=0)")
    End Sub

    Private Sub cbo_ProcessName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ProcessName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ProcessName, txt_Pcs, "Process_Head", "Process_Name", "", "(Process_Idno=0)")
    End Sub


    Private Sub txt_Quantity_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        'If Asc(e.KeyChar) = 13 Then
        '    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
        '        save_record()
        '    End If
        'End If
    End Sub

    Private Sub txt_Meters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Meters.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Weight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Weight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                cbo_Processed_Fabric_Name.Focus()

            End If
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
        Dim PFb_IdNo As Integer, Proc_IdNo As Integer, Col_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            PFb_IdNo = 0
            Proc_IdNo = 0
            COL_IDNO = 0

            If Trim(cbo_FilterProcessedFabric.Text) <> "" Then
                PFb_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_FilterProcessedFabric.Text)
            End If
            If Trim(cbo_Filter_Process.Text) <> "" Then
                Proc_IdNo = Common_Procedures.Process_NameToIdNo(con, cbo_Filter_Process.Text)
            End If

            If Val(PFb_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Fabric_IdNo = " & Str(Val(PFb_IdNo)) & " )"

            End If

            If Val(Proc_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Process_IdNo = " & Str(Val(Proc_IdNo)) & " )"
            End If

            If Trim(cbo_Filter_Colour.Text) <> "" Then
                Col_IdNo = Common_Procedures.Colour_NameToIdNo(con, cbo_Filter_Colour.Text)
            End If

            If Val(Col_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Colour_IdNo = " & Str(Val(Col_IdNo)) & " )"

            End If



            da = New SqlClient.SqlDataAdapter("select a.*, e.Cloth_Name , f.Process_Name from Processed_Unchecked_Fabric_Opening_Head a inner join Cloth_head e on a.Fabric_IdNo = e.Cloth_idno INNER JOIN Process_head f on a.Process_idno = f.Process_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Processed_Unchecked_Fabric_Opening_Code like '%/" & Trim(OpYrCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Processed_Unchecked_Fabric_Opening_Date, a.for_orderby, a.Processed_Unchecked_Fabric_Opening_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Processed_Unchecked_Fabric_Opening_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Process_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Pcs").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")

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

    Private Sub cbo_Filter_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_FilterProcessedFabric.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_name", "(Cloth_Type = 'PROCESSED FABRIC')", "(Cloth_iDNO = 0)")
    End Sub

    Private Sub cbo_Filter_Cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_FilterProcessedFabric.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_FilterProcessedFabric, Nothing, cbo_Filter_Process, "Cloth_Head", "Cloth_Name", "(Cloth_Type = 'PROCESSED FABRIC')", "Cloth_IdNo = 0")
    End Sub

    Private Sub cbo_Filter_Cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_FilterProcessedFabric.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_FilterProcessedFabric, cbo_Filter_Process, "Cloth_Head", "Cloth_Name", "(Cloth_Type = 'PROCESSED FABRIC')", "Cloth_IdNo = 0")
    End Sub

    Private Sub cbo_Filter_Process_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Process.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Process_Head", "Process_name", "", "(Process_IdNo = 0)")
    End Sub
    Private Sub cbo_Filter_ProcessName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Process.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Process, cbo_FilterProcessedFabric, cbo_Filter_Colour, "Process_Head", "Process_Name", "", "Process_IdNo = 0")
    End Sub

    Private Sub cbo_Filter_ProcessName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Process.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Process, cbo_Filter_Colour, "Process_Head", "Process_Name", "", "Process_IdNo = 0")
    End Sub

    Private Sub cbo_Filter_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Colour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Colour_Head", "Colour_Name", "", "(Colour_iDNO = 0)")
    End Sub

    Private Sub cbo_Filter_ColourName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Colour.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Colour, cbo_Filter_Process, btn_Filter_Show, "Colour_Head", "Colour_Name", "", "Colour_IdNo = 0")
    End Sub

    Private Sub cbo_Filter_ColourName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Colour.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Colour, btn_Filter_Show, "Colour_Head", "Colour_Name", "", "Colour_IdNo = 0")
    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        Try
            movno = Trim(dgv_Filter_Details.CurrentRow.Cells(0).Value)

            If Val(movno) <> 0 Then
                Filter_Status = True
                move_record(movno)
                Pnl_Back.Enabled = True
                pnl_Filter.Visible = False
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-----
    End Sub

    Private Sub txt_Weight_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Weight.LostFocus
        txt_Weight.Text = Format(Val(txt_Weight.Text), "##########0.000")
    End Sub


    Private Sub txt_Pcs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Pcs.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Meters_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Meters.LostFocus
        txt_Meters.Text = Format(Val(txt_Meters.Text), "##########0.00")
    End Sub

    Private Sub cbo_ProcessName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ProcessName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Process_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ProcessName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1

            f.Show()

        End If

    End Sub

  
End Class