Public Class Yarn_Bag_Mixing_Opening

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "OPYBM-"
    '  Private Pk_Condition2 As String = "YPAGC-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String
    Private OpYrCode As String = ""
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer

    Public Sub New()

        InitializeComponent()

        clear()
    End Sub

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True


        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        txt_loose_Yarn_Wgt.Text = ""
        txt_Mixing_Wgt.Text = ""

        cbo_Count.Text = ""
        cbo_Conetype.Text = ""

        dgv_Details.Rows.Clear()
        dgv_Details.Rows.Add()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()



        Grid_Cell_DeSelect()


        cbo_Count.Enabled = True
        cbo_Count.BackColor = Color.White

        NoCalc_Status = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Then
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

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.DeepPink
                Prec_ActCtrl.ForeColor = Color.White
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

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        If IsNothing(dgv_Details_Total.CurrentCell) Then Exit Sub

        dgv_Details.CurrentCell.Selected = False
        dgv_Details_Total.CurrentCell.Selected = False

    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim LockSTS As Boolean = False

        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(OpYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Cotton_Opening_YarnBagMixing_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_Opening_YarnBagMixing_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Cotton_Opening_YarnBagMixing_No").ToString

                cbo_Count.Text = Common_Procedures.Count_IdNoToName(con, Val(dt1.Rows(0).Item("Count_IdNo").ToString))
                txt_loose_Yarn_Wgt.Text = Format(Val(dt1.Rows(0).Item("Loose_Yarn_Weight").ToString), "########0.000")
                txt_Mixing_Wgt.Text = Format(Val(dt1.Rows(0).Item("Mixing_Weight").ToString), "########0.000")
                cbo_Conetype.Text = Common_Procedures.ConeType_IdNoToName(con, Val(dt1.Rows(0).Item("ConeType_IdNo").ToString))


                da2 = New SqlClient.SqlDataAdapter("Select a.* from Cotton_Packing_Details a  Where a.Cotton_Opening_YarnBagMixing_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)
                With dgv_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then
                        For i = 0 To dt2.Rows.Count - 1
                            n = .Rows.Add()
                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Bag_No").ToString
                            .Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("Net_Weight").ToString), "########0.000")
                            .Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Cotton_invoice_Code").ToString), "########0.000")

                            LockSTS = False

                            If IsDBNull(dt2.Rows(n).Item("Cotton_Invoice_Code").ToString) = False Then
                                If Trim(dt2.Rows(n).Item("Cotton_invoice_Code").ToString) <> "" Then
                                    LockSTS = True
                                End If
                            End If
                        Next i

                    End If

                    If .RowCount = 0 Then .Rows.Add()

                End With


                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()

                    .Rows(0).Cells(1).Value = Format(Val(dt1.Rows(0).Item("Total_BagNo").ToString), "##########0")
                    .Rows(0).Cells(2).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                End With

            End If
            If LockSTS = True Then

                cbo_Count.Enabled = False
                cbo_Count.BackColor = Color.LightGray

            End If
            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If cbo_Count.Visible And cbo_Count.Enabled Then cbo_Count.Focus()

        End Try

        NoCalc_Status = False

    End Sub

    Private Sub Cotton_Opening_YarnBagMixing_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Conetype.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CONETYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Conetype.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Count.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Count.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Cotton_Opening_YarnBagMixing_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Me.Text = ""

        con.Open()



        AddHandler cbo_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Conetype.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_loose_Yarn_Wgt.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Mixing_Wgt.GotFocus, AddressOf ControlGotFocus


        AddHandler cbo_Count.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Conetype.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_loose_Yarn_Wgt.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Mixing_Wgt.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_loose_Yarn_Wgt.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_loose_Yarn_Wgt.KeyPress, AddressOf TextBoxControlKeyPress


        OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
        OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))



        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Cotton_Opening_YarnBagMixing_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Cotton_Opening_YarnBagMixing_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                'If pnl_Filter.Visible = True Then
                '    btn_Filter_Close_Click(sender, e)
                '    Exit Sub

                'Else
                Close_Form()

            End If

            ' End If

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

            Else
                dgv1 = dgv_Details

            End If

            With dgv1
                If keyData = Keys.Enter Or keyData = Keys.Down Then



                    If .CurrentCell.ColumnIndex >= .ColumnCount - 3 Then
                        If .CurrentCell.RowIndex = .RowCount - 1 Then
                            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                                save_record()
                            Else
                                cbo_Count.Focus()
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
                            txt_loose_Yarn_Wgt.Focus()

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(2)

                        End If

                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                    End If

                    'End If

                    Return True

                Else
                    Return MyBase.ProcessCmdKey(msg, keyData)

                End If

            End With

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Cotton_Waste_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Cotton_Waste_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

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
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(OpYrCode)


        Da = New SqlClient.SqlDataAdapter("select * from Cotton_Packing_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Opening_YarnBagMixing_Code = '" & Trim(NewCode) & "' and  Cotton_invoice_Code <> ''", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Cotton_invoice_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("Cotton_invoice_Code").ToString) <> "" Then
                    MessageBox.Show("Already Packing Prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()
        Dt1.Dispose()
        Da.Dispose()

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(OpYrCode)

            cmd.Connection = con
            cmd.Transaction = trans


            cmd.CommandText = "Delete from Stock_Mixing_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_LooseYarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Cotton_Packing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Opening_YarnBagMixing_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Cotton_Opening_YarnBagMixing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Opening_YarnBagMixing_Code = '" & Trim(NewCode) & "'"
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

            If cbo_Count.Enabled = True And cbo_Count.Visible = True Then cbo_Count.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record



    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Cotton_Opening_YarnBagMixing_No from Cotton_Opening_YarnBagMixing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Opening_YarnBagMixing_Code like '%/" & Trim(OpYrCode) & "' Order by for_Orderby, Cotton_Opening_YarnBagMixing_No", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

            da = New SqlClient.SqlDataAdapter("select top 1 Cotton_Opening_YarnBagMixing_No from Cotton_Opening_YarnBagMixing_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Opening_YarnBagMixing_Code like '%/" & Trim(OpYrCode) & "' Order by for_Orderby, Cotton_Opening_YarnBagMixing_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Cotton_Opening_YarnBagMixing_No from Cotton_Opening_YarnBagMixing_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Opening_YarnBagMixing_Code like '%/" & Trim(OpYrCode) & "' Order by for_Orderby desc, Cotton_Opening_YarnBagMixing_No desc", con)
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

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Cotton_Opening_YarnBagMixing_No from Cotton_Opening_YarnBagMixing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Opening_YarnBagMixing_Code like '%/" & Trim(OpYrCode) & "' Order by for_Orderby desc, Cotton_Opening_YarnBagMixing_No desc", con)
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
            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Cotton_Opening_YarnBagMixing_Head", "Cotton_Opening_YarnBagMixing_Code", "For_OrderBy", "", Val(lbl_Company.Tag), OpYrCode)
            lbl_RefNo.ForeColor = Color.Red

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally



            If cbo_Count.Enabled And cbo_Count.Visible Then cbo_Count.Focus()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        Try

            inpno = InputBox("Enter Lot No.", "FOR FINDING...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(OpYrCode)

            Da = New SqlClient.SqlDataAdapter("select Cotton_Opening_YarnBagMixing_No from Cotton_Opening_YarnBagMixing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Opening_YarnBagMixing_Code = '" & Trim(RefCode) & "'", con)
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
                MessageBox.Show("Lot No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Cotton_Waste_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Cotton_Waste_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Lot No.", "FOR NEW LOT NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(OpYrCode)

            Da = New SqlClient.SqlDataAdapter("select Cotton_Opening_YarnBagMixing_No from Cotton_Opening_YarnBagMixing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Opening_YarnBagMixing_Code = '" & Trim(InvCode) & "'", con)
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
                    MessageBox.Show("Invalid Lot No.", "DOES NOT INSERT NEW Lot...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW LOT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim PackingCode As String = ""
        Dim PurAc_ID As Integer = 0
        Dim Rck_IdNo As Integer = 0
        Dim Fp_Id As Integer = 0
        Dim Lt_ID As Integer = 0
        Dim Cnt_ID As Integer = 0
        Dim Mill_ID As Integer = 0

        Dim Clr_Idno As Integer = 0
        Dim VatAc_ID As Integer = 0
        Dim Unt_ID As Integer = 0
        Dim Clr_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim vTotBale As Single, vTotWght As Single
        Dim Nr As Integer = 0
        Dim OpDate As Date
        Dim VouBil As String = ""
        Dim YrnClthNm As String = ""

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Cotton_Waste_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If


        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, Trim(cbo_Count.Text))

        If Val(Cnt_ID) = 0 Then
            MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Count.Enabled And cbo_Count.Visible Then cbo_Count.Focus()
            Exit Sub
        End If

        Clr_ID = Common_Procedures.ConeType_NameToIdNo(con, Trim(cbo_Conetype.Text))
        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(2).Value) <> 0 Then



                    If Val(.Rows(i).Cells(2).Value) = 0 Then
                        MessageBox.Show("Invalid Weight", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(2)
                        End If
                        Exit Sub
                    End If

                End If

            Next

        End With



        NoCalc_Status = False
        Total_Calculation()

        vTotBale = 0 : vTotWght = 0

        If dgv_Details_Total.RowCount > 0 Then
            vTotBale = Val(dgv_Details_Total.Rows(0).Cells(1).Value())
            vTotWght = Val(dgv_Details_Total.Rows(0).Cells(2).Value())
        End If

        tr = con.BeginTransaction

        Try

            OpDate = CDate("01-04-" & Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4))
            OpDate = DateAdd(DateInterval.Day, -1, OpDate)

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(OpYrCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Cotton_Opening_YarnBagMixing_Head", "Cotton_Opening_YarnBagMixing_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Trim(OpYrCode), tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(OpYrCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@OpDate", OpDate)

            If New_Entry = True Then
                cmd.CommandText = "Insert into Cotton_Opening_YarnBagMixing_Head (       Cotton_Opening_YarnBagMixing_Code ,               Company_IdNo       ,           Cotton_Opening_YarnBagMixing_No    ,      for_OrderBy          ,       Count_IdNo         ,       Total_BagNo    ,         Total_Weight       ,     Loose_Yarn_Weight               ,         Mixing_Weight          , ConeType_IdNo   ) " & _
                                    "     Values                  (   '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", " & Str(Val(Cnt_ID)) & " , " & Val(vTotBale) & ", " & Str(Val(vTotWght)) & " ," & Val(txt_loose_Yarn_Wgt.Text) & " ," & Val(txt_Mixing_Wgt.Text) & ", " & Val(Clr_ID) & " ) "
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Cotton_Opening_YarnBagMixing_Head set Count_IdNo = " & Str(Val(Cnt_ID)) & " , Total_BagNo = " & Val(vTotBale) & ", Total_Weight = " & Str(Val(vTotWght)) & ",  Loose_Yarn_Weight = " & Val(txt_loose_Yarn_Wgt.Text) & "   ,    Mixing_Weight = " & Val(txt_Mixing_Wgt.Text) & " , ConeType_IdNo = " & Val(Clr_ID) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Opening_YarnBagMixing_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            PBlNo = Trim(lbl_RefNo.Text)
            Partcls = "Open : Ref.No. " & Trim(lbl_RefNo.Text)

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Stock_LooseYarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Stock_Mixing_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Cotton_Packing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Opening_YarnBagMixing_Code = '" & Trim(NewCode) & "' and Cotton_invoice_Code = ''"
            cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0
                YrnClthNm = ""
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(2).Value) <> 0 Then

                        Sno = Sno + 1

                        PackingCode = ""
                        PackingCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(.Rows(i).Cells(1).Value) & "/" & Trim(OpYrCode)

                        'Nr = 0
                        'cmd.CommandText = "Update  Cotton_Packing_Head set Cotton_Packing_Date = @OpDate , Cotton_Packing_No = '" & Trim(.Rows(i).Cells(1).Value) & "', Count_IdNo = " & Str(Val(Cnt_ID)) & " , Net_Weight =  " & Str(Val(.Rows(i).Cells(2).Value)) & " ,ConeType_IdNo =  " & Val(Clr_ID) & " where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Packing_Code = '" & Trim(PackingCode) & "'"
                        'Nr = cmd.ExecuteNonQuery()

                        'If Nr = 0 Then
                        '    cmd.CommandText = "Insert into Cotton_Packing_Head ( Cotton_Opening_YarnBagMixing_Code , Cotton_Packing_Code              ,               Company_IdNo       ,   Cotton_Packing_No                     ,                     for_OrderBy                                        , Cotton_Packing_Date      ,          Count_IdNo      ,                        Net_Weight         ,ConeType_IdNo ) " & _
                        '                         "     Values                  (  '" & Trim(NewCode) & "'          , '" & Trim(PackingCode) & "'      , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "' , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(.Rows(i).Cells(1).Value))) & ",       @OpDate            , " & Str(Val(Cnt_ID)) & " , " & Str(Val(.Rows(i).Cells(2).Value)) & " , " & Val(Clr_ID) & ") "
                        '    cmd.ExecuteNonQuery()
                        'End If

                        Nr = 0
                        cmd.CommandText = "update Cotton_Packing_Details set Cotton_Packing_Date = @OpDate, Sl_No = " & Str(Val(Sno)) & ",  Count_IdNo =" & Str(Val(Cnt_ID)) & "  ,  ConeType_IdNo   =" & Str(Val(Clr_ID)) & "     , Bag_No = '" & Trim(Val(.Rows(i).Cells(1).Value)) & "',  Gross_Weight = " & Val(.Rows(i).Cells(2).Value) & ",Tare_Weight = 0 ,Net_Weight = " & Val(.Rows(i).Cells(2).Value) & " where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & "   and Bag_Code = '" & Trim(PackingCode) & "'  and Cotton_invoice_Code = '" & Trim(.Rows(i).Cells(3).Value) & "' "
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then

                            cmd.CommandText = "Insert into Cotton_Packing_Details( Cotton_Opening_YarnBagMixing_Code , Cotton_Packing_Code       , Company_IdNo   ,               Cotton_Packing_No      , for_OrderBy       ,           Cotton_Packing_Date      ,           Sl_No ,               Count_IdNo   ,        ConeType_IdNo                       ,       Bag_No                   ,          Bag_Code  ,  Gross_Weight              ,  Tare_Weight                    , Net_Weight      ) " & _
                                                                " Values ('" & Trim(NewCode) & "','" & Trim(Pk_Condition) & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @OpDate, " & Str(Val(Sno)) & " ,  " & Str(Val(Cnt_ID)) & "," & Str(Val(Clr_ID)) & ",  '" & Trim(.Rows(i).Cells(1).Value) & "' , '" & Trim(PackingCode) & "' , " & Str(Val(.Rows(i).Cells(2).Value)) & "  ,  0, " & Str(Val(.Rows(i).Cells(2).Value)) & " )"
                            cmd.ExecuteNonQuery()

                        End If

                        cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code                        ,             Company_IdNo                 ,           Reference_No        ,                               For_OrderBy                         ,        Reference_Date,     Party_Bill_No   ,                Sl_No      ,            Count_idNo      ,         Bags ,     Weight   ,ConeType_IdNo ) " & _
                                                                  "   Values  ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(.Rows(i).Cells(1).Value))) & ",    @OpDate   , '" & Trim(PBlNo) & "', " & Str(Val(Sno)) & ", " & Str(Val(Cnt_ID)) & ",  1 , " & Str(Val(.Rows(i).Cells(2).Value)) & " , " & Val(Clr_ID) & " )"
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With


            cmd.CommandText = "Insert into Stock_LooseYarn_Processing_Details ( Reference_Code                        ,             Company_IdNo                 ,           Reference_No        ,                               For_OrderBy                         ,        Reference_Date,     Party_Bill_No   ,        Sl_No      , Count_idNo      ,         Weight           ) " & _
                                                                    "   Values  ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",    @OpDate   , '" & Trim(PBlNo) & "', 1, " & Str(Val(Cnt_ID)) & ", " & Str(Val(txt_loose_Yarn_Wgt.Text)) & " )"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into Stock_Mixing_Processing_Details ( Reference_Code                        ,             Company_IdNo         ,           Reference_No        ,                               For_OrderBy                         ,        Reference_Date,     Party_Bill_No   ,   Sl_No                     ,   Count_IdNo   ,      Weight             ) " & _
                                                         "   Values  ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",    @OpDate   , '" & Trim(PBlNo) & "', " & Str(Val(Sno)) & ", " & Str(Val(Cnt_ID)) & ",  " & Val(txt_Mixing_Wgt.Text) & " )"
            cmd.ExecuteNonQuery()


            tr.Commit()
            ' move_record(lbl_RefNo.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()
            Dt1.Clear()

            If cbo_Count.Enabled And cbo_Count.Visible Then cbo_Count.Focus()


        End Try

    End Sub

    Private Sub cbo_Variety_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Count.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Variety_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Count.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Count, Nothing, cbo_Conetype, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub

    Private Sub cbo_Variety_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Count.KeyPress
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim RefNo As Single = 0
        Dim Cnt_idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Count, cbo_Conetype, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
        'Cnt_idno = Common_Procedures.Count_NameToIdNo(con, Trim(cbo_Count.Text))
        'If Asc(e.KeyChar) = 13 Then



        '    da1 = New SqlClient.SqlDataAdapter("select a.* from Cotton_Opening_YarnBagMixing_Head a Where a.Count_IdNo = " & Str(Val(Cnt_idno)) & "", con)
        '    dt1 = New DataTable
        '    da1.Fill(dt1)

        '    If dt1.Rows.Count > 0 Then
        '        If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
        '            RefNo = Trim(dt1.Rows(0).Item("Cotton_Opening_YarnBagMixing_No").ToString)
        '        End If
        '    End If

        '    dt1.Dispose()
        '    da1.Dispose()


        '    If Trim(RefNo) <> "" Then
        '        move_record(RefNo)
        '    Else
        '        new_record()

        '    End If

        '    cbo_Conetype.Focus()
        'End If


    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)

    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter

        With dgv_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If
            If e.RowIndex > 0 And e.ColumnIndex = 1 Then
                If Val(.CurrentRow.Cells(1).Value) = 0 And e.RowIndex = .RowCount - 1 Then
                    .CurrentRow.Cells(1).Value = Val(.Rows(e.RowIndex - 1).Cells(1).Value) + 1

                End If
                'If e.ColumnIndex = 1 And e.RowIndex = .RowCount - 1 And Val(.CurrentRow.Cells(2).Value) = 0 Then
                '    .CurrentRow.Cells(1).Value = Val(.Rows(e.RowIndex - 1).Cells(1).Value) + 1

                'End If
            End If

        End With

    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 2 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If


        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        On Error Resume Next
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Then

                    Total_Calculation()

                End If
            End If
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
        Try
            With dgv_Details
                If .Visible Then
                    If Val(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(4).Value) <> 0 Then
                        e.Handled = True
                    End If
                    If .CurrentCell.ColumnIndex = 2 Then

                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If

                    End If
                End If
            End With

        Catch ex As Exception
            '----

        End Try

    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Try
            With dgv_Details
                vcbo_KeyDwnVal = e.KeyValue
                If e.KeyValue = Keys.Delete Then
                    If Val(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(4).Value) <> 0 Then
                        e.Handled = True
                    End If
                End If
            End With

        Catch ex As Exception
            '----

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

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer = 0

        With dgv_Details

            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub


    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotBle As Integer
        Dim TotWgt As Single


        If NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotBle = 0 : TotWgt = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Trim(.Rows(i).Cells(1).Value) <> "" And Val(.Rows(i).Cells(2).Value) <> 0 Then

                    TotBle = TotBle + 1
                    TotWgt = TotWgt + Val(.Rows(i).Cells(2).Value)


                End If

            Next

        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(1).Value = Val(TotBle)
            .Rows(0).Cells(2).Value = Format(Val(TotWgt), "########0.000")
        End With

    End Sub


    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Cotton_Opening_YarnBagMixing_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Opening_YarnBagMixing_Code = '" & Trim(NewCode) & "'", con)
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
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String
        Dim W1 As Single = 0

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as Transport_Name , e.Ledger_Name as Agent_Name  from Cotton_Opening_YarnBagMixing_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON d.Ledger_IdNo = a.Transport_IdNo LEFT OUTER JOIN Ledger_Head e ON e.Ledger_IdNo = a.Agent_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_Opening_YarnBagMixing_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.count_name , c.Mill_Name  from Cotton_Opening_YarnBagMixing_Details a INNER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo LEFT OUTER JOIN Mill_Head c ON a.Mill_IdNo = c.Mill_IdNo where a.Cotton_Opening_YarnBagMixing_Code = '" & Trim(NewCode) & "' Order by a.for_orderby, a.Cotton_Opening_YarnBagMixing_No", con)
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

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        'If Trim(UCase(Common_Procedures.settings.CompanyName)) = "" Then
        Printing_Format1(e)
        'End If
    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 40
            .Right = 45
            .Top = 45
            .Bottom = 45
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

        NoofItems_PerPage = 15 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(50) : ClArr(2) = 60 : ClArr(3) = 220 : ClArr(4) = 80 : ClArr(5) = 120 : ClArr(6) = 100
        ClArr(7) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6))

        TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

                'CurY = CurY + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Mill_Name").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1


                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 18 Then
                            For I = 18 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 18
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt

                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If




                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


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
        Dim strHeight As Single
        Dim C1 As Single, C2 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim DelvToName As String

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name as Transport_Name from Cotton_Opening_YarnBagMixing_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_Opening_YarnBagMixing_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
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

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, " YARN PURCHASE RECEIPT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 50
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 50
            W1 = e.Graphics.MeasureString("PURCHASE NO   : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "FROM :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Purchase No", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cotton_Opening_YarnBagMixing_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Purchase Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Cotton_Waste_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Bill NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bill_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Agent Name", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "Transport Name : " & Trim(prn_HdDt.Rows(0).Item("Transport_Name").ToString), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Name").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10

            DelvToName = Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("DeliveryTo_Idno").ToString))

            'Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Delivery_Address2").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Delivery At", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, DelvToName, LMargin + W1 + 30, CurY, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "Rec No", LMargin + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Delivery_Receipt_No").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Delivery_Address3").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, " BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TOT WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE/KG ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim C1 As Single, W1 As Single
        'Dim W1 As Single = 0
        Dim BmsInWrds As String

        ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Weight").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 50
            W1 = e.Graphics.MeasureString("DISCOUNT    : ", pFont).Width
            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Discount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( - )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "VAT. 5 % ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Tax_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "AddLess", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
            LnAr(8) = CurY
            CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "E & O.E", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 15
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            CurY = CurY + TxtHgt
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")

            Common_Procedures.Print_To_PrintDocument(e, "Rupees            : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'CurY = CurY + 10
            'p1Font = New Font("Calibri", 12, FontStyle.Underline)
            'Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

            'CurY = CurY + TxtHgt + 10
            'Common_Procedures.Print_To_PrintDocument(e, "1. We are responsible for the quality of yarn only;If any running fault or quality  ", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "   defect noted in yarn please inform with firat fabric roll at once.We will", LMargin + 10, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "   accept only one roll at defect otherwise we do not hold ourself responsible. ", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "2. Our responsibility ceases when goods leave our permises.", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If


            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "3. Subject to Tirupur jurisdiction. ", LMargin + 10, CurY, 0, 0, pFont)

            '' Common_Procedures.Print_To_PrintDocument(e, "For ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 30, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "4. Interest at the rate of 24% will be charge from the due date.", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "4. All payment should be made by A/C payesr cheque or draft.", LMargin + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)



            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub




    Private Sub cbo_Conetype_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Conetype.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ConeType_head", "ConeType_nAME", "", "(ConeType_IdNo = 0)")
    End Sub

    Private Sub cbo_Conetype_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Conetype.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Conetype, cbo_Count, txt_loose_Yarn_Wgt, "ConeType_head", "ConeType_nAME", "", "(ConeType_IdNo = 0)")

    End Sub

    Private Sub cbo_Conetype_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Conetype.KeyPress


        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Conetype, txt_loose_Yarn_Wgt, "ConeType_head", "ConeType_nAME", "", "(ConeType_IdNo = 0)")
    End Sub



    Private Sub cbo_Conetype_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Conetype.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New ConeType_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Conetype.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub





    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        print_record()
    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
    End Sub

    Private Sub txt_Mixing_Wgt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Mixing_Wgt.KeyDown
        If (e.KeyValue) = 38 Then
            txt_loose_Yarn_Wgt.Focus()
        End If
        If (e.KeyValue) = 40 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub txt_Mixing_Wgt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Mixing_Wgt.KeyPress
        If Asc(e.KeyChar) = 13 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If
    End Sub
End Class