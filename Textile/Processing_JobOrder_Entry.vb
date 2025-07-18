Public Class Processing_JobOrder_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False

    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String

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

        pnl_back.Enabled = True

        lbl_OrderNo.Text = ""
        lbl_OrderNo.ForeColor = Color.Black

        msk_Date.Text = ""
        dtp_Date.Text = ""
        cbo_PartyName.Text = ""
        cbo_Article.Text = ""
        cbo_DesignNo.Text = ""
        cbo_Colour.Text = ""
        txt_Meters.Text = ""
        txt_Weight.Text = ""
        dtp_DelvDate.Text = ""
        msk_DelvDate.Text = ""

        cbo_GreyFabricName.Text = ""
        txt_GreyWidth.Text = ""
        txt_Ends.Text = ""
        txt_Pick.Text = ""
        cbo_WarpCount.Text = ""
        cbo_WeftCount.Text = ""
        cbo_Weave.Text = ""
        cbo_Selvedge.Text = ""

        cbo_FinishedFabricName.Text = ""
        txt_FinWidth.Text = ""
        txt_Berger.Text = ""
        txt_Apperance.Text = ""
        txt_Shrinkage.Text = ""
        txt_Skeving.Text = ""
        txt_FinWeight.Text = ""
        txt_Rejection.Text = ""
        txt_Tensile.Text = ""
        txt_ProcessRemarks.Text = ""
        txt_ChemicalRemarks.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        dgv_ProcessingDetails.Rows.Clear()
        dgv_ProcessingDetails.Rows.Add()

        cbo_Grid_ProcessName.Visible = False
        Grid_Cell_DeSelect()

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
            mskdtxbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

        If Me.ActiveControl.Name <> cbo_Grid_ProcessName.Name Then
            cbo_Grid_ProcessName.Visible = False
        End If

        Grid_Cell_DeSelect()


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


    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_ProcessingDetails.CurrentCell) Then dgv_ProcessingDetails.CurrentCell.Selected = False

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

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Textile_Processing_JobOrder_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Job_Order_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_OrderNo.Text = dt1.Rows(0).Item("Job_Order_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Job_Order_Date").ToString
                msk_Date.Text = dtp_Date.Text
                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_Article.Text = dt1.Rows(0).Item("Article").ToString
                cbo_DesignNo.Text = dt1.Rows(0).Item("Design_No").ToString
                cbo_Colour.Text = Common_Procedures.Colour_IdNoToName(con, Val(dt1.Rows(0).Item("Colour_IdNo").ToString))
                txt_Meters.Text = dt1.Rows(0).Item("Meters").ToString
                txt_Weight.Text = dt1.Rows(0).Item("Weight").ToString

                If IsDate(dt1.Rows(0).Item("Delivery_Date")) = True Then

                    dtp_DelvDate.Text = dt1.Rows(0).Item("Delivery_Date").ToString
                    msk_DelvDate.Text = dtp_DelvDate.Text

                End If
               
                cbo_GreyFabricName.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Grey_Cloth_IdNo").ToString))
                txt_GreyWidth.Text = dt1.Rows(0).Item("Grey_Width").ToString
                txt_Ends.Text = Val(dt1.Rows(0).Item("Ends").ToString)
                txt_Pick.Text = Val(dt1.Rows(0).Item("Pick").ToString)
                cbo_WarpCount.Text = Common_Procedures.Count_IdNoToName(con, Val(dt1.Rows(0).Item("Warp_Count").ToString))
                cbo_WeftCount.Text = Common_Procedures.Count_IdNoToName(con, Val(dt1.Rows(0).Item("Weft_Count").ToString))
                cbo_Weave.Text = dt1.Rows(0).Item("Weave").ToString
                cbo_Selvedge.Text = dt1.Rows(0).Item("Selvedge").ToString


                cbo_FinishedFabricName.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Finished_Cloth_IdNo").ToString))
                txt_FinWidth.Text = Val(dt1.Rows(0).Item("Finished_Width").ToString)
                txt_Berger.Text = dt1.Rows(0).Item("Berger").ToString
                txt_Apperance.Text = dt1.Rows(0).Item("Apperance").ToString
                txt_Shrinkage.Text = dt1.Rows(0).Item("Shrinkage").ToString
                txt_Skeving.Text = dt1.Rows(0).Item("Skewing").ToString
                txt_FinWeight.Text = dt1.Rows(0).Item("Finshed_Weight").ToString
                txt_Rejection.Text = dt1.Rows(0).Item("Rejection").ToString
                txt_Tensile.Text = dt1.Rows(0).Item("Tensile_Strength").ToString
                txt_ProcessRemarks.Text = dt1.Rows(0).Item("Process_Remarks").ToString
                txt_ChemicalRemarks.Text = dt1.Rows(0).Item("Chemical_Remarks").ToString
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                da2 = New SqlClient.SqlDataAdapter("Select b.Process_Name from Textile_Processing_JobOrder_Details a INNER JOIN Process_Head b ON a.Process_IdNo = b.Process_IdNo  Where a.Job_Order_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_ProcessingDetails

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Process_Name").ToString
                        Next i

                    End If

                    If .RowCount = 0 Then .Rows.Add()
                End With


            End If

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()
        End Try

        NoCalc_Status = False

    End Sub


    Public Sub delete_record() Implements Interface_MDIActions.delete_record

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Selc_Code As String = ""

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_Processing_Job_Order, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_Processing_Job_Order, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_back.Enabled = False Then
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


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_OrderNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'Da = New SqlClient.SqlDataAdapter("select sum(Delivery_Meters) from ClothSales_Order_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Order_Code = '" & Trim(NewCode) & "'", con)
        'Dt1 = New DataTable
        'Da.Fill(Dt1)
        'If Dt1.Rows.Count > 0 Then
        '    If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
        '        If Val(Dt1.Rows(0)(0).ToString) > 0 Then
        '            MessageBox.Show("Already some pieces delivered for this order", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '            Exit Sub
        '        End If
        '    End If
        'End If
        'Dt1.Clear()

        Selc_Code = Trim(lbl_OrderNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag))

        Da = New SqlClient.SqlDataAdapter("select a.* from Textile_Processing_Delivery_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JobOrder_No = '" & Trim(Selc_Code) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already Some Quality Deliverd for this order", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_OrderNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "delete from Textile_Processing_JobOrder_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Job_Order_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Textile_Processing_JobOrder_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Job_Order_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            new_record()

            MessageBox.Show("Deleted Successfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            If msk_Date.Enabled = True And msk_Date.Visible = True Then msk_Date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead  Where ( Ledger_IdNo = 0 or (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ) order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"


            da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
            da.Fill(dt2)
            cbo_Filter_ClothName.DataSource = dt2
            cbo_Filter_ClothName.DisplayMember = "Cloth_Name"

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ClothName.Text = ""
            cbo_Filter_ClothName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_Processing_Job_Order, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_Processing_Job_Order, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Job Order No.", "FOR NEW JOB ORDER NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Job_Order_No from Textile_Processing_JobOrder_Head where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Job_Order_Code = '" & Trim(InvCode) & "'", con)
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
                    MessageBox.Show("Invalid Job Order No.", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_OrderNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT ...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

            da = New SqlClient.SqlDataAdapter("select top 1 Job_Order_No from Textile_Processing_JobOrder_Head where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Job_Order_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by For_OrderBy, Job_Order_No", con)
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

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Job_Order_No from Textile_Processing_JobOrder_Head where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Job_Order_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by For_OrderBy desc, Job_Order_No desc", con)
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

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_OrderNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Job_Order_No from Textile_Processing_JobOrder_Head where For_OrderBy > " & Str(Val(OrdByNo)) & " and Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Job_Order_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by For_OrderBy, Job_Order_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_OrderNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Job_Order_No from Textile_Processing_JobOrder_Head where For_OrderBy < " & Str(Val(OrdByNo)) & " and Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Job_Order_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by For_OrderBy desc, Job_Order_No desc", con)
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

        Try
            clear()

            New_Entry = True

            lbl_OrderNo.Text = Common_Procedures.get_MaxCode(con, "Textile_Processing_JobOrder_Head", "Job_Order_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_OrderNo.ForeColor = Color.Red


        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da1.Dispose()

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

        End Try
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        Try

            inpno = InputBox("Enter Order Ref No.", "FOR FINDING...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Job_Order_No from Textile_Processing_JobOrder_Head where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Job_Order_Code = '" & Trim(InvCode) & "'", con)
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
                MessageBox.Show("Order Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim Selc_SetCode As String = ""
        Dim gryClth_ID As Integer = 0
        Dim finClth_ID As Integer = 0
        Dim Warp_Id As Integer
        Dim Weft_Id As Integer
        Dim Led_ID As Integer = 0
        Dim Clr_Id As Integer = 0
        Dim Prs_id As Integer = 0

        Dim Sno As Integer = 0
        Dim EntID As String = ""
        Dim Nr As Integer = 0

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If


        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Entry_Processing_Job_Order, New_Entry) = False Then Exit Sub

        If pnl_back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
            Exit Sub
        End If
        'If IsDate(msk_DelvDate.Text) = False Then
        '    MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If msk_DelvDate.Enabled And msk_DelvDate.Visible Then msk_DelvDate.Focus()
        '    Exit Sub
        'End If

        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
            Exit Sub
        End If

        'If Not (Convert.ToDateTime(msk_DelvDate.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_DelvDate.Text) <= Common_Procedures.Company_ToDate) Then
        '    MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If msk_DelvDate.Enabled And msk_DelvDate.Visible Then msk_DelvDate.Focus()
        '    Exit Sub
        'End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled Then cbo_PartyName.Focus()
            Exit Sub
        End If

        Weft_Id = Common_Procedures.Count_NameToIdNo(con, cbo_WeftCount.Text)
        Warp_Id = Common_Procedures.Count_NameToIdNo(con, cbo_WarpCount.Text)
        Clr_Id = Common_Procedures.Colour_NameToIdNo(con, cbo_Colour.Text)
        gryClth_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_GreyFabricName.Text)
        finClth_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_FinishedFabricName.Text)
        lbl_UserName.Text = Common_Procedures.User.IdNo
        For i = 0 To dgv_ProcessingDetails.RowCount - 1

            If Val(dgv_ProcessingDetails.Rows(i).Cells(1).Value) <> 0 Then

                Prs_id = Common_Procedures.Process_NameToIdNo(con, dgv_ProcessingDetails.Rows(i).Cells(1).Value)
                If Prs_id = 0 Then
                    MessageBox.Show("Invalid Process Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_ProcessingDetails.Enabled And dgv_ProcessingDetails.Visible Then
                        dgv_ProcessingDetails.Focus()
                        dgv_ProcessingDetails.CurrentCell = dgv_ProcessingDetails.Rows(i).Cells(1)
                    End If
                    Exit Sub
                End If

            End If

        Next


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_OrderNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
                Selc_SetCode = Trim(lbl_OrderNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag))

            Else

                lbl_OrderNo.Text = Common_Procedures.get_MaxCode(con, "Textile_Processing_JobOrder_Head", "Job_Order_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_OrderNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
                Selc_SetCode = Trim(lbl_OrderNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag))

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@OrderDate", Convert.ToDateTime(msk_Date.Text))
            If IsDate(msk_DelvDate.Text) = True Then
                cmd.Parameters.AddWithValue("@DeliveryDate", Convert.ToDateTime(msk_DelvDate.Text))
            End If


            If New_Entry = True Then
                cmd.CommandText = "Insert into Textile_Processing_JobOrder_Head ( Job_Order_Code , Job_Order_SelectionCode       ,             For_OrderBy                                                 ,  Company_IdNo                    ,     Job_Order_No                ,     Job_Order_Date    ,    Ledger_IdNo          ,          Article                    ,            Design_No             ,     Colour_IdNo         ,  Meters                          ,       Weight                     ,   Delivery_Date     ,   Grey_Cloth_IdNo           ,  Grey_Width                        ,     Ends                      ,             Pick              ,         Warp_Count      ,         Weft_Count      ,          Weave               ,     Selvedge                    ,Finished_Cloth_IdNo       ,          Finished_Width                ,        Berger                 ,     Apperance                    ,     Shrinkage                     ,         Skewing                  ,  Finshed_Weight                     ,    Rejection                      ,Tensile_Strength                ,                                                               Process_Remarks                        ,Chemical_Remarks                ,     User_idNo ) " & _
                                    "     Values                (   '" & Trim(NewCode) & "'      , '" & Trim(Selc_SetCode) & "'  ," & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_OrderNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_OrderNo.Text) & "',       @OrderDate      , " & Str(Val(Led_ID)) & ",   '" & Trim(cbo_Article.Text) & "'  , '" & Trim(cbo_DesignNo.Text) & "', " & Str(Val(Clr_Id)) & ", " & Str(Val(txt_Meters.Text)) & ", " & Str(Val(txt_Weight.Text)) & ",  " & IIf(IsDate(msk_DelvDate.Text) = True, "@DeliveryDate", "Null") & ", " & Str(Val(gryClth_ID)) & " ," & Str(Val(txt_GreyWidth.Text)) & "," & Str(Val(txt_Ends.Text)) & "," & Str(Val(txt_Pick.Text)) & "," & Str(Val(Warp_Id)) & "," & Str(Val(Weft_Id)) & ",'" & Trim(cbo_Weave.Text) & "','" & Trim(cbo_Selvedge.Text) & "', " & Str(Val(finClth_ID)) & ", " & Str(Val(txt_FinWidth.Text)) & " ,'" & Trim(txt_Berger.Text) & "','" & Trim(txt_Apperance.Text) & "', '" & Trim(txt_Shrinkage.Text) & "', '" & Trim(txt_Skeving.Text) & "' , " & Str(Val(txt_FinWeight.Text)) & ", '" & Trim(txt_Rejection.Text) & "','" & Trim(txt_Tensile.Text) & "','" & Trim(txt_ProcessRemarks.Text) & "','" & Trim(txt_ChemicalRemarks.Text) & "', " & Val(lbl_UserName.Text) & ") "
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Textile_Processing_JobOrder_Head set Job_Order_SelectionCode =  '" & Trim(Selc_SetCode) & "' , Job_Order_Date = @OrderDate, Ledger_IdNo =  " & Str(Val(Led_ID)) & ", Article  ='" & Trim(cbo_Article.Text) & "' , Design_No  = '" & Trim(cbo_DesignNo.Text) & "' , Colour_IdNo = " & Str(Val(Clr_Id)) & ",  Meters = " & Str(Val(txt_Meters.Text)) & " ,  Weight = " & Str(Val(txt_Weight.Text)) & "  ,Delivery_Date = " & IIf(IsDate(msk_DelvDate.Text) = True, "@DeliveryDate", "Null") & ",  Grey_Cloth_IdNo  = " & Str(Val(gryClth_ID)) & " ,  Grey_Width  = " & Str(Val(txt_GreyWidth.Text)) & " ,  Ends  =  " & Str(Val(txt_Ends.Text)) & " ,  Pick =  " & Str(Val(txt_Pick.Text)) & " , Warp_Count = " & Str(Val(Warp_Id)) & ", Weft_Count = " & Str(Val(Weft_Id)) & "  , Weave  = '" & Trim(cbo_Weave.Text) & "' ,Selvedge ='" & Trim(cbo_Selvedge.Text) & "'   ,Finished_Cloth_IdNo =  " & Str(Val(finClth_ID)) & "   ,Finished_Width =  " & Str(Val(txt_FinWidth.Text)) & " ,Berger = '" & Trim(txt_Berger.Text) & "'    ,Apperance  = '" & Trim(txt_Apperance.Text) & "'    , Shrinkage = '" & Trim(txt_Shrinkage.Text) & "' ,Skewing  = '" & Trim(txt_Skeving.Text) & "'  ,Finshed_Weight = " & Str(Val(txt_FinWeight.Text)) & "   ,Rejection  =  '" & Trim(txt_Rejection.Text) & "'  ,Tensile_Strength = '" & Trim(txt_Tensile.Text) & "'   ,Process_Remarks = '" & Trim(txt_ProcessRemarks.Text) & "'   ,Chemical_Remarks =  '" & Trim(txt_ChemicalRemarks.Text) & "' , User_IdNo = " & Val(lbl_UserName.Text) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Job_Order_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Textile_Processing_JobOrder_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Job_Order_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_ProcessingDetails

                Sno = 0
                For i = 0 To .RowCount - 1

                    Sno = Sno + 1

                    Prs_id = Common_Procedures.Process_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                   
                    cmd.CommandText = "Insert into Textile_Processing_JobOrder_Details ( Job_Order_Code ,               Company_IdNo                 ,   Job_Order_No    ,                     For_OrderBy                                            , Job_Order_Date          ,          Sl_No       ,        Process_IdNo   ) " & _
                                        "     Values                        (   '" & Trim(NewCode) & "'      , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_OrderNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_OrderNo.Text))) & ",       @OrderDate     , " & Str(Val(Sno)) & ", " & Str(Val(Prs_id)) & ") "
                    cmd.ExecuteNonQuery()


                Next

            End With

            tr.Commit()

            MessageBox.Show("Saved Successfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_OrderNo.Text)
                End If
            Else
                move_record(lbl_OrderNo.Text)
            End If

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

        End Try

    End Sub

    Private Sub Processing_JobOrder_Enter_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Colour.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COLOUR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Colour.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_GreyFabricName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH NAME" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_GreyFabricName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_FinishedFabricName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH NAME" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_FinishedFabricName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_ProcessName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH TYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_ProcessName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_WarpCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH NAME" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_WarpCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_WeftCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH TYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_WeftCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Processing_JobOrder_Enter_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Processing_JobOrder_Enter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
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
    Private Sub btn_Filter_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub Processing_JobOrder_Enter_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
        Dim dt10 As New DataTable
        Dim dt11 As New DataTable
        Dim dt12 As New DataTable


        Me.Text = ""

        con.Open()


        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where ( Ledger_IdNo = 0 or (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ) order by Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_PartyName.DataSource = dt1
        cbo_PartyName.DisplayMember = "Ledger_DisplayName"


        da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
        da.Fill(dt2)
        cbo_Colour.DataSource = dt2
        cbo_Colour.DisplayMember = "Colour_Name"

        da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
        da.Fill(dt3)
        cbo_GreyFabricName.DataSource = dt3
        cbo_GreyFabricName.DisplayMember = "Cloth_Name"

        da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
        da.Fill(dt4)
        cbo_FinishedFabricName.DataSource = dt4
        cbo_FinishedFabricName.DisplayMember = "Cloth_Name"

        da = New SqlClient.SqlDataAdapter("select Process_Name from Process_Head order by Process_Name", con)
        da.Fill(dt5)
        cbo_Grid_ProcessName.DataSource = dt5
        cbo_Grid_ProcessName.DisplayMember = "Process_Name"

        da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
        da.Fill(dt6)
        cbo_WarpCount.DataSource = dt6
        cbo_WarpCount.DisplayMember = "Count_Name"

        da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
        da.Fill(dt7)
        cbo_WeftCount.DataSource = dt7
        cbo_WeftCount.DisplayMember = "Count_Name"

        da = New SqlClient.SqlDataAdapter("select distinct(Article) from Textile_Processing_JobOrder_Head order by Article", con)
        da.Fill(dt8)
        cbo_Article.DataSource = dt8
        cbo_Article.DisplayMember = "Article"

        da = New SqlClient.SqlDataAdapter("select distinct(Design_No) from Textile_Processing_JobOrder_Head order by Design_No", con)
        da.Fill(dt9)
        cbo_Article.DataSource = dt9
        cbo_Article.DisplayMember = "Design_No"

        da = New SqlClient.SqlDataAdapter("select distinct(Weave) from Textile_Processing_JobOrder_Head order by Weave", con)
        da.Fill(dt10)
        cbo_Article.DataSource = dt10
        cbo_Article.DisplayMember = "Weave"

        da = New SqlClient.SqlDataAdapter("select distinct(Selvedge) from Textile_Processing_JobOrder_Head order by Selvedge", con)
        da.Fill(dt11)
        cbo_Article.DataSource = dt11
        cbo_Article.DisplayMember = "Selvedge"



        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()


        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Article.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DesignNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Colour.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Meters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Weight.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_DelvDate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_DelvDate.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_GreyFabricName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_GreyWidth.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Ends.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Pick.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_WarpCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_WeftCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Weave.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Selvedge.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_FinishedFabricName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_FinWidth.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Berger.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Apperance.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Shrinkage.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Skeving.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_FinWeight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rejection.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Tensile.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_ProcessRemarks.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ChemicalRemarks.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_ProcessName.GotFocus, AddressOf ControlGotFocus


        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Article.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DesignNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Colour.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Meters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Weight.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_DelvDate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_DelvDate.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_GreyFabricName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_GreyWidth.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Ends.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Pick.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_WarpCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_WeftCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Weave.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Selvedge.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_FinishedFabricName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_FinWidth.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Berger.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Apperance.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Shrinkage.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Skeving.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_FinWeight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rejection.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Tensile.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_ProcessRemarks.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ChemicalRemarks.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_ProcessName.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_DelvDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Meters.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Weight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_GreyWidth.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Ends.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Pick.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_FinWidth.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Berger.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Apperance.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Shrinkage.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Skeving.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_FinWeight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rejection.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Tensile.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ProcessRemarks.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_ChemicalRemarks.KeyDown, AddressOf TextBoxControlKeyDown



        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_DelvDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Meters.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Weight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_GreyWidth.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Ends.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Pick.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_FinWidth.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Berger.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Apperance.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Shrinkage.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Skeving.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_FinWeight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Rejection.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Tensile.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ProcessRemarks.KeyPress, AddressOf TextBoxControlKeyPress
        '  AddHandler txt_ChemicalRemarks.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()
    End Sub
    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, msk_Date, cbo_Article, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, cbo_Article, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyName.Name
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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Colour, cbo_DesignNo, txt_Meters, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")
    End Sub

    Private Sub cbo_Colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Colour.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Colour, txt_Meters, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")
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

    Private Sub cbo_Article_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Article.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Textile_Processing_JobOrder_Head", "Article", "", "")
    End Sub

    Private Sub cbo_Article_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Article.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Article, cbo_PartyName, cbo_DesignNo, "Textile_Processing_JobOrder_Head", "Article", "", "")
    End Sub

    Private Sub cbo_Article_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Article.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Article, cbo_DesignNo, "Textile_Processing_JobOrder_Head", "Article", "", "", False)
    End Sub

    Private Sub cbo_DesignNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DesignNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Textile_Processing_JobOrder_Head", "Design_No", "", "")
    End Sub

    Private Sub cbo_DesignNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DesignNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DesignNo, cbo_Article, cbo_Colour, "Textile_Processing_JobOrder_Head", "Design_No", "", "")
    End Sub

    Private Sub cbo_DesignNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DesignNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DesignNo, cbo_Colour, "Textile_Processing_JobOrder_Head", "Design_No", "", "", False)
    End Sub

    Private Sub cbo_GreyFabricName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_GreyFabricName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno = 0)")
    End Sub

    Private Sub cbo_GreyFabricName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GreyFabricName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_GreyFabricName, msk_DelvDate, txt_GreyWidth, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno = 0)")
    End Sub

    Private Sub cbo_GreyFabricName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_GreyFabricName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_GreyFabricName, txt_GreyWidth, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno = 0)")
    End Sub

    Private Sub cbo_GreyFabricName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_GreyFabricName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_GreyFabricName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_FinishedFabricName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_FinishedFabricName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno = 0)")
    End Sub

    Private Sub cbo_FinishedFabricName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_FinishedFabricName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_FinishedFabricName, cbo_Selvedge, txt_FinWidth, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno = 0)")
    End Sub

    Private Sub cbo_FinishedFabricName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_FinishedFabricName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_FinishedFabricName, txt_FinWidth, "Cloth_Head", "Cloth_Name", "", "(Cloth_Idno = 0)")
    End Sub

    Private Sub cbo_FinishedFabricName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_FinishedFabricName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_FinishedFabricName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_ProcessName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ProcessName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Process_Head", "Process_Name", "", "(Process_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_ProcessName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ProcessName.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_ProcessName, txt_Tensile, txt_ProcessRemarks, "Process_Head", "Process_Name", "", "(Process_IdNo = 0)")

        With dgv_ProcessingDetails

            If (e.KeyValue = 38 And cbo_Grid_ProcessName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                txt_Tensile.Focus()
            End If

            If (e.KeyValue = 40 And cbo_Grid_ProcessName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    txt_ProcessRemarks.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_ProcessName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_ProcessName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_ProcessName, txt_ProcessRemarks, "Process_Head", "Process_Name", "", "(Process_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            e.Handled = True

            With dgv_ProcessingDetails
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    txt_ProcessRemarks.Focus()
                Else
                    .Focus()
                    .Rows.Add()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                End If
            End With

        End If
    End Sub

    Private Sub cbo_Grid_ProcessName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ProcessName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Process_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_ProcessName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Selvedge_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Selvedge.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Textile_Processing_JobOrder_Head", "Selvedge", "", "")
    End Sub

    Private Sub cbo_Selvedge_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Selvedge.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Selvedge, cbo_Weave, cbo_FinishedFabricName, "Textile_Processing_JobOrder_Head", "Selvedge", "", "")
    End Sub

    Private Sub cbo_Selvedge_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Selvedge.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Selvedge, cbo_FinishedFabricName, "Textile_Processing_JobOrder_Head", "Selvedge", "", "", False)
    End Sub

    Private Sub cbo_Weave_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Weave.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Textile_Processing_JobOrder_Head", "Weave", "", "")
    End Sub
    Private Sub cbo_Weave_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Weave.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Weave, txt_Pick, cbo_Selvedge, "Textile_Processing_JobOrder_Head", "Weave", "", "")
    End Sub

    Private Sub cbo_Weave_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Weave.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Weave, cbo_Selvedge, "Textile_Processing_JobOrder_Head", "Weave", "", "", False)
    End Sub

    Private Sub cbo_WarpCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_WarpCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_WarpCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WarpCount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_WarpCount, txt_GreyWidth, cbo_WeftCount, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_WarpCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_WarpCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_WarpCount, cbo_WeftCount, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_WarpCount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WarpCount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_WarpCount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_WeftCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_WeftCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_WeftCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WeftCount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_WeftCount, cbo_WarpCount, txt_Ends, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_WeftCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_WeftCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_WeftCount, txt_Ends, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_WeftCount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WeftCount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_WeftCount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub dgv_ProcessingDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_ProcessingDetails.CellEndEdit

    End Sub

    Private Sub dgv_ProcessingDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_ProcessingDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle

        With dgv_ProcessingDetails


            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

          
            If e.ColumnIndex = 1 Then

                If cbo_Grid_ProcessName.Visible = False Or Val(cbo_Grid_ProcessName.Tag) <> e.RowIndex Then

                    cbo_Grid_ProcessName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Process_Name from Process_Head order by Process_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_ProcessName.DataSource = Dt1
                    cbo_Grid_ProcessName.DisplayMember = "Process_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_ProcessName.Left = .Left + rect.Left
                    cbo_Grid_ProcessName.Top = .Top + rect.Top

                    cbo_Grid_ProcessName.Width = rect.Width
                    cbo_Grid_ProcessName.Height = rect.Height
                    cbo_Grid_ProcessName.Text = .CurrentCell.Value

                    cbo_Grid_ProcessName.Tag = Val(e.RowIndex)
                    cbo_Grid_ProcessName.Visible = True

                    cbo_Grid_ProcessName.BringToFront()
                    cbo_Grid_ProcessName.Focus()


                End If


            Else

                cbo_Grid_ProcessName.Visible = False

            End If
        End With
    End Sub

    Private Sub dgv_ProcessingDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_ProcessingDetails.KeyDown

        With dgv_ProcessingDetails

            If e.KeyCode = Keys.Up Then
                If .CurrentCell.RowIndex = 1 Then
                    .CurrentCell.Selected = False
                    txt_Tensile.Focus()
                End If
            End If
            If e.KeyCode = Keys.Left Then
                If .CurrentCell.RowIndex = 0 And .CurrentCell.ColumnIndex <= 1 Then
                    .CurrentCell.Selected = False
                    txt_Tensile.Focus()
                End If
            End If

            If e.KeyCode = Keys.Enter Then
                e.SuppressKeyPress = True
                e.Handled = True

                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                    txt_ProcessRemarks.Focus()
                Else
                    SendKeys.Send("{Tab}")

                End If


            End If

        End With

    End Sub

    Private Sub dgv_ProcessingDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_ProcessingDetails.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_ProcessingDetails
                n = .CurrentRow.Index
                If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If
            End With

        End If
    End Sub

    Private Sub dgv_ProcessingDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_ProcessingDetails.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_ProcessingDetails.CurrentCell) Then dgv_ProcessingDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_ProcessingDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_ProcessingDetails.RowsAdded
        Dim n As Integer
        If IsNothing(dgv_ProcessingDetails.CurrentCell) Then Exit Sub
        With dgv_ProcessingDetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub cbo_Grid_ProcessName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ProcessName.TextChanged
        Try
            If cbo_Grid_ProcessName.Visible Then
                With dgv_ProcessingDetails
                    If Val(cbo_Grid_ProcessName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Grid_ProcessName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub msk_DelvDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_DelvDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_DelvDate.Text = Date.Today
        End If
        If IsDate(msk_DelvDate.Text) = True Then
            If e.KeyCode = 107 Then
                msk_DelvDate.Text = DateAdd("D", 1, Convert.ToDateTime(msk_DelvDate.Text))
            ElseIf e.KeyCode = 109 Then
                msk_DelvDate.Text = DateAdd("D", -1, Convert.ToDateTime(msk_DelvDate.Text))
            End If
        End If
    End Sub

    Private Sub msk_DelvDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_DelvDate.LostFocus
        If IsDate(msk_DelvDate.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_DelvDate.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_DelvDate.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_DelvDate.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_DelvDate.Text)) >= 2000 Then
                    dtp_DelvDate.Value = Convert.ToDateTime(msk_DelvDate.Text)
                End If
            End If

        End If
    End Sub

    Private Sub dtp_DelvDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_DelvDate.TextChanged
        If IsDate(dtp_DelvDate.Text) = True Then
            msk_DelvDate.Text = dtp_DelvDate.Text
        End If
    End Sub
    Private Sub msk_DelvDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_DelvDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub



    Private Sub msk_date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_date.Text = Date.Today
        End If
        If IsDate(msk_date.Text) = True Then
            If e.KeyCode = 107 Then
                msk_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
            ElseIf e.KeyCode = 109 Then
                msk_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
            End If
        End If
    End Sub

    Private Sub msk_date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_Date.LostFocus
        If IsDate(msk_date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_date.Text)) >= 2000 Then
                    dtp_date.Value = Convert.ToDateTime(msk_date.Text)
                End If
            End If

        End If
    End Sub

    Private Sub dtp_date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_date.Text) = True Then
            msk_date.Text = dtp_date.Text
        End If
    End Sub
    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub txt_Ends_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Ends.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_FinWeight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_FinWeight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_FinWidth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_FinWidth.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_GreyWidth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_GreyWidth.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Meters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Meters.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Pick_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Pick.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Weight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Weight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub
    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub
    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Clth_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Clth_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Job_Order_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Job_Order_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Job_Order_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_ClothName.Text) <> "" Then
                Clth_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Filter_ClothName.Text)
            End If


            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(Led_IdNo)) & ")"
            End If

            If Val(Clth_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Cloth_IdNo = " & Str(Val(Clth_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, e.Ledger_Name from Textile_Processing_JobOrder_Head a  left outer join Ledger_head e on a.Ledger_idno = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Job_Order_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by Job_Order_Date, for_orderby, Job_Order_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Job_Order_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Job_Order_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Article").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Design_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.000")

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

    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
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

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim ps As Printing.PaperSize
        Dim PpSzSTS As Boolean = False
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_OrderNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Textile_Processing_JobOrder_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Job_Order_Code = '" & Trim(NewCode) & "'", con)
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


        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next




        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try
                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

              
                                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                                        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                                        Exit For
                                    End If
                                Next
                  

                        PrintDocument1.Print()
                    End If

                Else
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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_OrderNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, cR.Colour_Name , cG.Cloth_Name as Greycloth, cF.Cloth_Name as Finishedcloth ,wR.Count_Name as Warp, wF.Count_Name as Weft from Textile_Processing_JobOrder_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo Left outer JOIN Colour_Head cR ON a.Colour_IdNo = cR.Colour_IdNo Left outer JOIN Cloth_Head cG ON a.Grey_Cloth_IdNo = cG.Cloth_Idno Left outer JOIN Cloth_Head cF ON a.Finished_Cloth_IdNo = cF.Cloth_Idno Left outer JOIN Count_Head wR ON a.Warp_Count = wR.Count_IdNo Left outer JOIN Count_Head wF ON a.Weft_Count = wF.Count_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Job_Order_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.* ,b.Process_Name from Textile_Processing_JobOrder_Details a INNER JOIN Process_Head b ON a.Process_IdNo = b.Process_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Job_Order_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
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
        Dim nCount As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim p1Font As Font
        Dim p2Font As Font
        Dim Cmp_Name As String
        Dim strHeight As Single
        Dim S1 As Single = 0
        Dim S2 As Single = 0
        Dim S3 As Single = 0
        Dim ItmNm1 As String = "", ItmNm2 As String = "", ItmNm3 As String = "", ItmNm4 As String = ""


        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next
     

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 40
            .Right = 40
            .Top = 30
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 12, FontStyle.Regular)

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


        TxtHgt = 19.5

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_OrderNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                CurY = TMargin

                Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

                CurY = CurY + TxtHgt - 10

                p1Font = New Font("Calibri", 20, FontStyle.Bold Or FontStyle.Underline)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
                strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
                CurY = CurY + strHeight + 20

                p1Font = New Font("Calibri", 18, FontStyle.Bold Or FontStyle.Underline)
                Common_Procedures.Print_To_PrintDocument(e, "PROCESSING JOB ORDER", LMargin, CurY, 2, PrintWidth, p1Font)
                strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
                CurY = CurY + strHeight + 10

                S1 = e.Graphics.MeasureString("JOB ORDER NO.     :", pFont).Width

                Common_Procedures.Print_To_PrintDocument(e, "JOB ORDER NO.", LMargin + 10, CurY, 0, PrintWidth, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Job_Order_No").ToString, LMargin + S1 + 30, CurY, 0, PrintWidth, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "DATE  :  " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Job_Order_date").ToString), "dd-MM-yyyy").ToString, PageWidth - 20, CurY, 1, 0, pFont)

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "MILL", LMargin + 10, CurY, 0, PrintWidth, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + S1 + 30, CurY, 0, PrintWidth, pFont)
                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "ARTICLE", LMargin + 10, CurY, 0, PrintWidth, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Article").ToString, LMargin + S1 + 30, CurY, 0, PrintWidth, pFont)

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "DESIGN NO.", LMargin + 10, CurY, 0, PrintWidth, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Design_No").ToString, LMargin + S1 + 30, CurY, 0, PrintWidth, pFont)
               
                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "COLOUR", LMargin + 10, CurY, 0, PrintWidth, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Colour_Name").ToString, LMargin + S1 + 30, CurY, 0, PrintWidth, pFont)

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "QUANTITY.", LMargin + 10, CurY, 0, PrintWidth, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Meters").ToString), "#########0.00") & " MTRS", LMargin + S1 + 30, CurY, 0, PrintWidth, pFont)

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "TOTAL WEIGHT.", LMargin + 10, CurY, 0, PrintWidth, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Weight").ToString), "#########0.000") & " KGS", LMargin + S1 + 30, CurY, 0, PrintWidth, pFont)

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "DELIVERY", LMargin + 10, CurY, 0, PrintWidth, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + S1 + 30, CurY, 0, PrintWidth, pFont)

                CurY = CurY + strHeight + 10
                p2Font = New Font("Calibri", 14, FontStyle.Bold Or FontStyle.Underline)
                Common_Procedures.Print_To_PrintDocument(e, "CONSTRUCTION OF GREY FABRIC", LMargin, CurY, 0, PrintWidth, p2Font)
                strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

                S2 = e.Graphics.MeasureString("YARN COUNTS                    :", pFont).Width

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "WIDTH", LMargin + 10, CurY, 0, PrintWidth, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S2 + 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Grey_Width").ToString), "#########0.00") & " CMS", LMargin + S1 + 100, CurY, 0, PrintWidth, pFont)

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "ENDS/PICKS", LMargin + 10, CurY, 0, PrintWidth, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S2 + 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ends").ToString & " X " & prn_HdDt.Rows(0).Item("Pick").ToString, LMargin + S1 + 100, CurY, 0, PrintWidth, pFont)

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "YARN COUNTS", LMargin + 10, CurY, 0, PrintWidth, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S2 + 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("warp").ToString & " X " & prn_HdDt.Rows(0).Item("weft").ToString, LMargin + S1 + 100, CurY, 0, PrintWidth, pFont)

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "WEAVE", LMargin + 10, CurY, 0, PrintWidth, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S2 + 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Weave").ToString, LMargin + S1 + 100, CurY, 0, PrintWidth, pFont)

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "SELVEDGE", LMargin + 10, CurY, 0, PrintWidth, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S2 + 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Selvedge").ToString, LMargin + S1 + 100, CurY, 0, PrintWidth, pFont)

                CurY = CurY + TxtHgt + 10
                p2Font = New Font("Calibri", 14, FontStyle.Bold Or FontStyle.Underline)
                Common_Procedures.Print_To_PrintDocument(e, "FINISHING PROCESS-", LMargin, CurY, 0, PrintWidth, p2Font)
                strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

                '------------Process Name
                Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                    ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Process_Name").ToString)

                    ItmNm2 = ItmNm2 & IIf(Trim(ItmNm2) <> "", ",", "") & ItmNm1

                    prn_DetIndx = prn_DetIndx + 1
                Loop
                ItmNm3 = ItmNm2
                S3 = LMargin + S2 + 20
                nCount = 60
SPLIT_PNAME:
                If Len(ItmNm3) > nCount Then
                    ItmNm4 = Microsoft.VisualBasic.Left(Trim(ItmNm3), nCount)
                    For I = nCount To 1 Step -1
                        If Mid$(Trim(ItmNm4), I, 1) = "," Then Exit For
                    Next I
                    If I = 0 Then I = nCount
                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm4), Len(ItmNm4) - (nCount - I))


                    Common_Procedures.Print_To_PrintDocument(e, ItmNm1, S3, CurY, 0, PrintWidth, pFont)

                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm3), Len(Trim(ItmNm3)) - Len(Trim(ItmNm1)))
                    If Len(ItmNm2) > 60 Then
                        ItmNm3 = ItmNm2
                        CurY = CurY + TxtHgt
                        S3 = LMargin + 10
                        nCount = 80
                        GoTo SPLIT_PNAME
                    Else
                        CurY = CurY + TxtHgt
                        S3 = LMargin + 10
                        Common_Procedures.Print_To_PrintDocument(e, ItmNm2, S3, CurY, 0, PrintWidth, pFont)
                    End If

                Else
                    Common_Procedures.Print_To_PrintDocument(e, ItmNm3, S3, CurY, 0, PrintWidth, pFont)

                End If
                '-------------Process Remarks
                p2Font = New Font("Calibri", 11, FontStyle.Bold)
                CurY = CurY + TxtHgt + 5

                ItmNm3 = Trim(prn_HdDt.Rows(0).Item("Process_Remarks").ToString)


                S3 = LMargin + 10
                nCount = 80
SPLIT_PREMARKS:
                If Len(ItmNm3) > nCount Then
                    ItmNm4 = Microsoft.VisualBasic.Left(Trim(ItmNm3), nCount)
                    For I = nCount To 1 Step -1
                        If Mid$(Trim(ItmNm4), I, 1) = "," Or Mid$(Trim(ItmNm4), I, 1) = " " Or Mid$(Trim(ItmNm4), I, 1) = "-" Or Mid$(Trim(ItmNm4), I, 1) = "&" Or Mid$(Trim(ItmNm4), I, 1) = "/" Or Mid$(Trim(ItmNm4), I, 1) = "\" Then Exit For
                    Next I

                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm4), Len(ItmNm4) - (nCount - I))
                    Common_Procedures.Print_To_PrintDocument(e, ItmNm1, S3, CurY, 0, PrintWidth, p2Font)

                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm3), Len(Trim(ItmNm3)) - Len(Trim(ItmNm1)))
                    If Len(ItmNm2) > 60 Then
                        ItmNm3 = ItmNm2
                        CurY = CurY + TxtHgt
                        'S3 = LMargin + 10
                        nCount = 80
                        GoTo SPLIT_PREMARKS
                    Else
                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, ItmNm2, S3, CurY, 0, PrintWidth, p2Font)
                    End If

                Else
                    Common_Procedures.Print_To_PrintDocument(e, ItmNm3, S3, CurY, 0, PrintWidth, p2Font)
                End If
                '------------------



                CurY = CurY + TxtHgt + 10
                p2Font = New Font("Calibri", 14, FontStyle.Bold Or FontStyle.Underline)
                Common_Procedures.Print_To_PrintDocument(e, "REQUIRED PARAMETERS OF FINISHED BLEACHED FABRICS", LMargin, CurY, 0, PrintWidth, p2Font)
                strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

                'S2 = e.Graphics.MeasureString("WIDTH IF THE FINISHED FABRIC        :", pFont).Width

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "WIDTH OF FINISHED FABRIC", LMargin + 10, CurY, 0, PrintWidth, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S2 + 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Finished_Width").ToString), "#########0.00") & " CMS", LMargin + S1 + 100, CurY, 0, PrintWidth, pFont)

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "BERGER", LMargin + 10, CurY, 0, PrintWidth, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S2 + 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Berger").ToString, LMargin + S1 + 100, CurY, 0, PrintWidth, pFont)

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "APPEARANCE", LMargin + 10, CurY, 0, PrintWidth, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S2 + 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Apperance").ToString, LMargin + S1 + 100, CurY, 0, PrintWidth, pFont)

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "SHIRINKAGE-WARP / WEFT", LMargin + 10, CurY, 0, PrintWidth, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S2 + 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Shrinkage").ToString, LMargin + S1 + 100, CurY, 0, PrintWidth, pFont)

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "SKEWING AND BOWING", LMargin + 10, CurY, 0, PrintWidth, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S2 + 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Skewing").ToString, LMargin + S1 + 100, CurY, 0, PrintWidth, pFont)

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "WEIGHT/SQUARE METRE", LMargin + 10, CurY, 0, PrintWidth, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S2 + 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Finshed_Weight").ToString), "#########0.000") & " GMS", LMargin + S1 + 100, CurY, 0, PrintWidth, pFont)

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "REJECTION", LMargin + 10, CurY, 0, PrintWidth, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S2 + 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rejection").ToString, LMargin + S1 + 100, CurY, 0, PrintWidth, pFont)

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "TENSILE STRENGTH", LMargin + 10, CurY, 0, PrintWidth, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S2 + 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Tensile_Strength").ToString, LMargin + S1 + 100, CurY, 0, PrintWidth, pFont)


                CurY = CurY + TxtHgt + 10
                p2Font = New Font("Calibri", 14, FontStyle.Bold Or FontStyle.Underline)
                Common_Procedures.Print_To_PrintDocument(e, "CHEMICAL CONTAMINATION ACCORDING TO OEKO TEX STANDARD 100", LMargin, CurY, 0, PrintWidth, p2Font)
                strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

                '-------------Chemical Remarks
                CurY = CurY + TxtHgt + 10

                ItmNm3 = Trim(prn_HdDt.Rows(0).Item("Chemical_Remarks").ToString)


                S3 = LMargin + 10
                nCount = 80
SPLIT_CREMARKS:
                If Len(ItmNm3) > nCount Then
                    ItmNm4 = Microsoft.VisualBasic.Left(Trim(ItmNm3), nCount)
                    For I = nCount To 1 Step -1
                        If Mid$(Trim(ItmNm4), I, 1) = "," Or Mid$(Trim(ItmNm4), I, 1) = " " Or Mid$(Trim(ItmNm4), I, 1) = "-" Or Mid$(Trim(ItmNm4), I, 1) = "&" Then Exit For
                    Next I

                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm4), Len(ItmNm4) - (nCount - I))
                    Common_Procedures.Print_To_PrintDocument(e, ItmNm1, S3, CurY, 0, PrintWidth, pFont)

                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm3), Len(Trim(ItmNm3)) - Len(Trim(ItmNm1)))
                    If Len(ItmNm2) > 60 Then
                        ItmNm3 = ItmNm2
                        CurY = CurY + TxtHgt
                        'S3 = LMargin + 10
                        nCount = 80
                        GoTo SPLIT_CREMARKS
                    Else
                        CurY = CurY + TxtHgt
                        'S3 = LMargin + 10
                        Common_Procedures.Print_To_PrintDocument(e, ItmNm2, S3, CurY, 0, PrintWidth, pFont)
                    End If

                Else
                    Common_Procedures.Print_To_PrintDocument(e, ItmNm3, S3, CurY, 0, PrintWidth, pFont)
                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub txt_ChemicalRemarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_ChemicalRemarks.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_ChemicalRemarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ChemicalRemarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

 
End Class