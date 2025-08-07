Public Class Vendor_Pavu_Bobin_Yarn_Jari_Faric_PriceList_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = ""
    Private dgv_ActiveCtrl_Name As String

    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private WithEvents dgtxt_CountDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_EndsCountDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_clothDetails As New DataGridViewTextBoxEditingControl

    Private dgvDet_CboBx_ColNos_Arr As Integer() = {1}

    Private prn_HdDt As New DataTable
    Private prn_Dt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private NoFo_STS As Integer = 0
    Private prn_HdIndx As Integer
    Private prn_HdMxIndx As Integer
    Private prn_Count As Integer
    Private prn_HdAr(100, 10) As String
    Private prn_DetAr(100, 50, 10) As String
    Private prn_InpOpts As String = ""
    Private prn_OriDupTri As String = ""
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private Sub clear()
        NoCalc_Status = True
        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        'txt_Prefix_InvNo.Text = ""
        'chk_InvNo.Checked = False

        vmskOldText = ""
        vmskSelStrt = -1

        dtp_date.Text = ""


        cbo_WeaverName.Text = ""
        cbo_WeaverName.Tag = ""
        cbo_WeaverName.Enabled = True
        cbo_WeaverName.BackColor = Color.White
        btn_SHOWDETAILS.Enabled = True


        Grid_Cell_DeSelect()

        NoCalc_Status = False

        dgv_Count_details.Rows.Clear()

        dgv_EndsCount_Details.Rows.Clear()

        dgv_Cloth_Details.Rows.Clear()

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim mskdtxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
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
            mskdtxbx = Me.ActiveControl
            mskdtxbx.SelectAll()
        End If

        If Me.ActiveControl.Name <> Cbo_Grid_CountName.Name Then
            Cbo_Grid_CountName.Visible = False
            Cbo_Grid_CountName.Tag = -100
        End If

        If Me.ActiveControl.Name <> Cbo_Grid_EndsCountName.Name Then
            Cbo_Grid_EndsCountName.Visible = False
            Cbo_Grid_EndsCountName.Tag = -100
        End If

        If Me.ActiveControl.Name <> cbo_Grid_ClothName.Name Then
            cbo_Grid_ClothName.Visible = False
            cbo_Grid_ClothName.Tag = -100
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

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Count_details.CurrentCell) Then dgv_Count_details.CurrentCell.Selected = False

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
        If Not IsNothing(dgv_Count_details.CurrentCell) Then dgv_Count_details.CurrentCell.Selected = False
        If Not IsNothing(dgv_EndsCount_Details.CurrentCell) Then dgv_EndsCount_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Cloth_Details.CurrentCell) Then dgv_Cloth_Details.CurrentCell.Selected = False

    End Sub

    Private Sub Invoice_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Try



            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_WeaverName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_WeaverName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If



            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_Grid_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                Cbo_Grid_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If



            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_Grid_EndsCountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                Cbo_Grid_EndsCountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If



            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_ClothName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_ClothName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""


            If FrmLdSTS = True Then

                'lbl_Company.Text = ""
                'lbl_Company.Tag = 0
                'Common_Procedures.CompIdNo = 0

                'Me.Text = ""

                'lbl_Company.Text = Common_Procedures.get_Company_From_CompanySelection(con)
                'lbl_Company.Tag = Val(Common_Procedures.CompIdNo)

                'Me.Text = lbl_Company.Text

                new_record()

            End If



        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Invoice_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Invoice_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then


                Close_Form()



                End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Invoice_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        Me.Text = ""

        con.Open()




        AddHandler dtp_date.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_WeaverName.GotFocus, AddressOf ControlGotFocus


        AddHandler cbo_Grid_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Grid_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Grid_EndsCountName.GotFocus, AddressOf ControlGotFocus



        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_WeaverName.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Grid_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Grid_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Grid_EndsCountName.LostFocus, AddressOf ControlLostFocus



        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus


        AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress


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

            'lbl_Company.Tag = 0
            'lbl_Company.Text = ""
            'Me.Text = ""
            'Common_Procedures.CompIdNo = 0


            'lbl_Company.Text = Common_Procedures.Show_CompanySelection_On_FormClose(con)

            'lbl_Company.Tag = Val(Common_Procedures.CompIdNo)
            'Me.Text = lbl_Company.Text


            If Val(Common_Procedures.CompIdNo) = 0 Then

                Me.Close()

            Else

                new_record()

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView


        If ActiveControl.Name = dgv_Count_details.Name Or ActiveControl.Name = dgv_EndsCount_Details.Name Or ActiveControl.Name = dgv_Cloth_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Count_details.Name Then
                dgv1 = dgv_Count_details

            ElseIf ActiveControl.Name = dgv_EndsCount_Details.Name Then
                dgv1 = dgv_EndsCount_Details

            ElseIf ActiveControl.Name = dgv_Cloth_Details.Name Then
                dgv1 = dgv_Cloth_Details

            ElseIf dgv_Count_details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Count_details

            ElseIf dgv_EndsCount_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_EndsCount_Details

            ElseIf dgv_Cloth_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Cloth_Details

            ElseIf dgv_ActiveCtrl_Name = dgv_Count_details.Name Then
                dgv1 = dgv_Count_details

            ElseIf dgv_ActiveCtrl_Name = dgv_EndsCount_Details.Name Then
                dgv1 = dgv_EndsCount_Details

            ElseIf dgv_ActiveCtrl_Name = dgv_Cloth_Details.Name Then
                dgv1 = dgv_Cloth_Details

            End If

            If IsNothing(dgv1) = False Then

                With dgv1

                    If dgv1.Name = dgv_Count_details.Name Then



                        If keyData = Keys.Enter Or keyData = Keys.Down Or keyData = Keys.Up Then

                            Common_Procedures.DGVGrid_Cursor_Focus(dgv1, keyData, cbo_WeaverName, btn_save, dgvDet_CboBx_ColNos_Arr, dgtxt_CountDetails, dtp_date)

                            Return True

                        Else
                            Return MyBase.ProcessCmdKey(msg, keyData)

                        End If

                    ElseIf dgv1.Name = dgv_EndsCount_Details.Name Then

                        If keyData = Keys.Enter Or keyData = Keys.Down Or keyData = Keys.Up Then

                            Common_Procedures.DGVGrid_Cursor_Focus(dgv1, keyData, Nothing, dgv_Cloth_Details, dgvDet_CboBx_ColNos_Arr, dgtxt_EndsCountDetails, dtp_date)

                            Return True

                        Else
                            Return MyBase.ProcessCmdKey(msg, keyData)

                        End If

                    ElseIf dgv1.Name = dgv_Cloth_Details.Name Then


                        If keyData = Keys.Enter Or keyData = Keys.Down Or keyData = Keys.Up Then

                            Common_Procedures.DGVGrid_Cursor_Focus(dgv1, keyData, Nothing, btn_save, dgvDet_CboBx_ColNos_Arr, dgtxt_clothDetails, dtp_date)

                            Return True

                        Else
                            Return MyBase.ProcessCmdKey(msg, keyData)

                        End If
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

        Return MyBase.ProcessCmdKey(msg, keyData)

    End Function

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim n As Integer
        Dim SNo As Integer
        Dim LockSTS As Boolean = False


        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Vendor_PriceList_Head a Where a.Vendor_PriceList_IdNo = " & Str(Val(no)), con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Vendor_PriceList_IdNo").ToString


                msk_date.Text = dt1.Rows(0).Item("vendor_PriceList_date").ToString
                cbo_WeaverName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Weaver_idno").ToString))
                cbo_WeaverName.Tag = cbo_WeaverName.Text
                cbo_WeaverName.Enabled = False
                btn_SHOWDETAILS.Enabled = False

                da2 = New SqlClient.SqlDataAdapter("Select a.* from vendor_PriceList_Count_Details a Where a.Vendor_PriceList_IdNo = " & Str(Val(no)) & " Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Count_details

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(SNo)

                            .Rows(n).Cells(1).Value = Common_Procedures.Count_IdNoToName(con, Val(dt2.Rows(i).Item("Count_idno").ToString))

                            .Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "###########0.00")

                        Next i

                    End If

                End With
                NoCalc_Status = False

                NoCalc_Status = True
                da2 = New SqlClient.SqlDataAdapter("Select a.* from vendor_PriceList_EndsCount_Details a Where a.Vendor_PriceList_IdNo = " & Str(Val(no)) & " Order by a.sl_no", con)
                dt3 = New DataTable
                da2.Fill(dt3)

                With dgv_EndsCount_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt3.Rows.Count > 0 Then

                        For i = 0 To dt3.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(SNo)


                            .Rows(n).Cells(1).Value = Common_Procedures.EndsCount_IdNoToName(con, Val(dt3.Rows(i).Item("EndsCount_idno").ToString))

                            .Rows(n).Cells(2).Value = Format(Val(dt3.Rows(i).Item("Rate").ToString), "###########0.00")


                        Next i

                    End If

                End With
                NoCalc_Status = False

                NoCalc_Status = True
                da2 = New SqlClient.SqlDataAdapter("Select a.* from Vendor_PriceList_Cloth_Details a Where a.Vendor_PriceList_IdNo = " & Str(Val(no)) & " Order by a.sl_no", con)
                dt4 = New DataTable
                da2.Fill(dt4)

                With dgv_Cloth_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt4.Rows.Count > 0 Then

                        For i = 0 To dt4.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            'Vendor_PriceList_Cloth_Details
                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = Common_Procedures.Cloth_IdNoToName(con, Val(dt4.Rows(i).Item("Cloth_idno").ToString))

                            .Rows(n).Cells(2).Value = Format(Val(dt4.Rows(i).Item("Rate").ToString), "###########0.00")


                        Next i

                    End If

                End With


                NoCalc_Status = False

                NoCalc_Status = True

            Else

                new_record()


            End If

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If cbo_WeaverName.Enabled = True And cbo_WeaverName.Visible = True Then cbo_WeaverName.Focus()
            If msk_date.Enabled = True And msk_date.Visible = True Then msk_date.Focus()
            If dtp_date.Enabled = True And dtp_date.Visible = True Then dtp_date.Focus()


        End Try

        NoCalc_Status = False

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Fabric_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Fabric_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub


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

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans



            cmd.CommandText = "delete from vendor_PriceList_EndsCount_Details where Vendor_PriceList_IdNo = " & Str(Val(lbl_RefNo.Text))
            cmd.ExecuteNonQuery()


            cmd.CommandText = "delete from vendor_PriceList_Count_Details where Vendor_PriceList_IdNo = " & Str(Val(lbl_RefNo.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Vendor_PriceList_Cloth_Details where Vendor_PriceList_IdNo = " & Str(Val(lbl_RefNo.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Vendor_PriceList_Head where Vendor_PriceList_IdNo = " & Str(Val(lbl_RefNo.Text))
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
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        new_record()
        cbo_WeaverName.Focus()
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Dim movid As Integer = 0


        Try


            da = New SqlClient.SqlDataAdapter("select min(Vendor_PriceList_IdNo) from Vendor_PriceList_Head Where Vendor_PriceList_IdNo <> 0", con)

            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movid) <> 0 Then move_record(movid)

            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0
        Dim OrdByNo As Single = 0



        Try

            da = New SqlClient.SqlDataAdapter("select min(Vendor_PriceList_IdNo) from Vendor_PriceList_Head Where Vendor_PriceList_IdNo > " & Str(Val(lbl_RefNo.Text)) & " and Vendor_PriceList_IdNo <> 0", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movid) <> 0 Then move_record(movid)

            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0
        Dim OrdByNo As Single = 0



        Try

            da = New SqlClient.SqlDataAdapter("select max(Vendor_PriceList_IdNo) from Vendor_PriceList_Head Where Vendor_PriceList_IdNo < " & Str(Val(lbl_RefNo.Text)) & " and Vendor_PriceList_IdNo <> 0", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movid) <> 0 Then move_record(movid)

            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0




        Try

            da = New SqlClient.SqlDataAdapter("select max(Vendor_PriceList_IdNo) from Vendor_PriceList_Head Where Vendor_PriceList_IdNo <> 0", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movid) <> 0 Then move_record(movid)

            dt.Dispose()
            da.Dispose()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        Try
            clear()

            New_Entry = True

            cbo_WeaverName.Enabled = True
            cbo_WeaverName.Tag = cbo_WeaverName.Text
            btn_SHOWDETAILS.Enabled = True

            lbl_RefNo.Text = Common_Procedures.get_MaxIdNo(con, "Vendor_PriceList_Head", "Vendor_PriceList_IdNo", "")

            lbl_RefNo.ForeColor = Color.Red

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            If cbo_WeaverName.Enabled And cbo_WeaverName.Visible Then cbo_WeaverName.Focus()
        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        new_record()
        cbo_WeaverName.Focus()
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        new_record()
        cbo_WeaverName.Focus()
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_ID As Integer = 0
        Dim OnAc_ID As Integer = 0
        Dim VaAc_ID As Integer = 0
        Dim PrnPl_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Slno As Integer = 0
        Dim Sno1 As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim Dup_No As String
        Dim PrnPlQ_ID As Integer = 0
        Dim Unt_Id As Integer = 0
        Dim YrnClthNm As String = ""
        Dim Nr As Integer = 0
        Dim PurCd As String = ""
        Dim PurSlNo As Long = 0
        Dim DcCd As String = ""
        Dim DcSlNo As Long = 0
        Dim PcsChkCode As String = ""
        Dim Bil_Sts As Integer = 0
        Dim Clr_ID As Integer = 0
        Dim Dc_Date As Single = 0
        Dim vChk_InvNo As Integer = 0
        Dim vChk_FlmScrnCrge As Integer = 0
        Dim vJb_Code_Sltn As String = ""
        Dim vRWNO As String = ""
        Dim vCount_ID As Integer = 0
        Dim vEndsCount_ID As Integer = 0
        Dim vCloth_ID As Integer = 0

        Dim PriceListDate As Date

        'If Val(lbl_Company.Tag) = 0 Then
        '    MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    Exit Sub
        'End If

        '   If Common_Procedures.UserRight_Check(Common_Procedures.UR.Fabric_Receipt_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If IsDate(msk_date.Text) = False Then
        '    MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()
        '    Exit Sub
        'End If

        'If Not (dtp_date.Value.Date >= Common_Procedures.Company_FromDate And dtp_date.Value.Date <= Common_Procedures.Company_ToDate) Then
        '    MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()
        '    Exit Sub
        'End If

        'If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
        '    MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If msk_date.Enabled Then msk_date.Focus()
        '    Exit Sub
        'End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_WeaverName.Text)

        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_WeaverName.Enabled Then cbo_WeaverName.Focus()
            Exit Sub
        End If


        With dgv_Count_details

            Sno = 0

            Dup_No = ""
            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(2).Value) <> 0 Then

                    vCount_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(1).Value)
                    If vCount_ID = 0 Then
                        MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If InStr(1, Trim(UCase(Dup_No)), "~" & Trim(UCase(.Rows(i).Cells(1).Value)) & "~") > 0 Then


                        vRWNO = -1
                        For j = 0 To i - 1
                            If Trim(UCase(.Rows(i).Cells(1).Value)) = Trim(UCase(.Rows(j).Cells(1).Value)) Then
                                vRWNO = j + 1
                                Exit For
                            End If
                        Next
                        If Val(vRWNO) >= 0 Then
                            MessageBox.Show("Duplicate Count - " & Trim(UCase(.Rows(i).Cells(1).Value)) & Chr(13) & "Already enterd in Sl.No. : " & vRWNO, "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Else
                            MessageBox.Show("Duplicate Count : " & Trim(UCase(.Rows(i).Cells(1).Value)), "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        End If
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    Dup_No = Trim(Dup_No) & "~" & Trim(UCase(.Rows(i).Cells(1).Value)) & "~"

                End If

            Next

        End With


        With dgv_EndsCount_Details

            Sno = 0

            Dup_No = ""
            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(2).Value) <> 0 Then

                    vEndsCount_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(1).Value)
                    If vEndsCount_ID = 0 Then
                        MessageBox.Show("Invalid Ends/Count ", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If InStr(1, Trim(UCase(Dup_No)), "~" & Trim(UCase(.Rows(i).Cells(1).Value)) & "~") > 0 Then


                        vRWNO = -1
                        For j = 0 To i - 1
                            If Trim(UCase(.Rows(i).Cells(1).Value)) = Trim(UCase(.Rows(j).Cells(1).Value)) Then
                                vRWNO = j + 1
                                Exit For
                            End If
                        Next
                        If Val(vRWNO) >= 0 Then
                            MessageBox.Show("Duplicate Ends/Count - " & Trim(UCase(.Rows(i).Cells(1).Value)) & Chr(13) & "Already enterd in Sl.No. : " & vRWNO, "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Else
                            MessageBox.Show("Duplicate Ends/Count : " & Trim(UCase(.Rows(i).Cells(1).Value)), "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        End If
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    Dup_No = Trim(Dup_No) & "~" & Trim(UCase(.Rows(i).Cells(1).Value)) & "~"

                End If

            Next

        End With



        With dgv_Cloth_Details

            Sno = 0

            Dup_No = ""
            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(2).Value) <> 0 Then

                    vCloth_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(1).Value)

                    If vCloth_ID = 0 Then
                        MessageBox.Show("Invalid Cloth Name ", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If InStr(1, Trim(UCase(Dup_No)), "~" & Trim(UCase(.Rows(i).Cells(1).Value)) & "~") > 0 Then


                        vRWNO = -1
                        For j = 0 To i - 1
                            If Trim(UCase(.Rows(i).Cells(1).Value)) = Trim(UCase(.Rows(j).Cells(1).Value)) Then
                                vRWNO = j + 1
                                Exit For
                            End If
                        Next
                        If Val(vRWNO) >= 0 Then
                            MessageBox.Show("Duplicate Cloth Name - " & Trim(UCase(.Rows(i).Cells(1).Value)) & Chr(13) & "Already enterd in Sl.No. : " & vRWNO, "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Else
                            MessageBox.Show("Duplicate Cloth Name : " & Trim(UCase(.Rows(i).Cells(1).Value)), "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        End If
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    Dup_No = Trim(Dup_No) & "~" & Trim(UCase(.Rows(i).Cells(1).Value)) & "~"

                End If

            Next

        End With


        Bil_Sts = 0

        NoCalc_Status = False

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxIdNo(con, "Vendor_PriceList_Head", "Vendor_PriceList_IdNo", "", tr)
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            End If

            PriceListDate = CDate("01-04-" & Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4))
            PriceListDate = DateAdd(DateInterval.Day, -1, PriceListDate)



            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            '  cmd.Parameters.AddWithValue("@InvDate", Convert.ToDateTime(msk_date.Text))
            cmd.Parameters.AddWithValue("@InvDate", PriceListDate)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Vendor_PriceList_Head (          Vendor_PriceList_IdNo                 , vendor_PriceList_date ,         Weaver_IdNo        ) " &
                                      "Values                         (       " & Str(Val(lbl_RefNo.Text)) & ",        @InvDate               , " & Str(Val(Led_ID)) & "      ) "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Vendor_PriceList_Head set vendor_PriceList_date = @InvDate, Weaver_IdNo = " & Str(Val(Led_ID)) & "  Where  Vendor_PriceList_IdNo = " & Str(Val(lbl_RefNo.Text))
                cmd.ExecuteNonQuery()


            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            PBlNo = Trim(lbl_RefNo.Text)
            Partcls = "Invoice : Inv.No. " & Trim(lbl_RefNo.Text)



            cmd.CommandText = "delete from Vendor_PriceList_Cloth_Details where Vendor_PriceList_IdNo = " & Str(Val(lbl_RefNo.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from vendor_PriceList_EndsCount_Details where Vendor_PriceList_IdNo = " & Str(Val(lbl_RefNo.Text))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from vendor_PriceList_Count_Details where Vendor_PriceList_IdNo  = " & Str(Val(lbl_RefNo.Text))
            cmd.ExecuteNonQuery()


            With dgv_Count_details

                Sno = 0

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(2).Value) <> 0 Then

                        Sno = Sno + 1

                        vCount_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        cmd.CommandText = "Insert into vendor_PriceList_Count_Details (          Vendor_PriceList_IdNo      ,    vendor_PriceList_date ,         Weaver_IdNo       ,             Count_IdNo      ,              Sl_No   ,                       Rate       ) " &
                                              "Values                               (    " & Str(Val(lbl_RefNo.Text)) & ",         @InvDate             , " & Str(Val(Led_ID)) & " ,        " & Str(Val(vCount_ID)) & " ,       " & Str(Val(Sno)) & ",'" & Trim(.Rows(i).Cells(2).Value) & "'  ) "
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With
            With dgv_EndsCount_Details
                Slno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(2).Value) <> 0 Then
                        vEndsCount_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)
                        Slno = Slno + 1
                        cmd.CommandText = "Insert into vendor_PriceList_EndsCount_Details (       Vendor_PriceList_IdNo     , vendor_PriceList_date ,             Weaver_IdNo       ,             EndsCount_IdNo ,                     Sl_No   ,                          Rate          ) " &
                                              "Values                       (  " & Str(Val(lbl_RefNo.Text)) & ",       @InvDate ,          " & Str(Val(Led_ID)) & " ,             " & Str(Val(vEndsCount_ID)) & "       , " & Str(Val(Slno)) & "          ,'" & Trim(.Rows(i).Cells(2).Value) & "'  ) "
                        cmd.ExecuteNonQuery()

                    End If
                Next
            End With



            With dgv_Cloth_Details

                Sno1 = 0

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(2).Value) <> 0 Then

                        Sno1 = Sno1 + 1
                        vCloth_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        cmd.CommandText = "Insert into Vendor_PriceList_Cloth_Details       (    Vendor_PriceList_IdNo    ,  vendor_PriceList_date ,             Weaver_IdNo       ,             Cloth_IdNo             ,               Sl_No              ,             Rate                 ) " &
                                              "Values                      (   " & Str(Val(lbl_RefNo.Text)) & ",       @InvDate            , " & Str(Val(Led_ID)) & " , " & Val(vCloth_ID) & "       , " & Str(Val(Sno1)) & "          ,'" & Trim(.Rows(i).Cells(2).Value) & "'  ) "
                        cmd.ExecuteNonQuery()


                    End If

                Next

            End With



            tr.Commit()


            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If New_Entry = False Then
                move_record(lbl_RefNo.Text)
            Else
                new_record()
            End If


        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        'Dim da1 As New SqlClient.SqlDataAdapter
        'Dim dt1 As New DataTable
        'Dim NewCode As String
        'Dim ps As Printing.PaperSize

        'NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'Try

        '    da1 = New SqlClient.SqlDataAdapter("select * from Vendor_PriceList_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and vendor_Stock_Closing_Code = '" & Trim(NewCode) & "'", con)
        '    dt1 = New DataTable
        '    da1.Fill(dt1)

        '    If dt1.Rows.Count <= 0 Then

        '        MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '        Exit Sub

        '    End If

        '    dt1.Dispose()
        '    da1.Dispose()

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        Exit For
        '    End If
        'Next


        'If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
        '    Try

        '        If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
        '            PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
        '            If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
        '                PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

        '                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '                        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '                        Exit For
        '                    End If
        '                Next

        '                PrintDocument1.Print()
        '            End If

        '        Else
        '            PrintDocument1.Print()

        '        End If

        '    Catch ex As Exception
        '        MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        '    End Try


        'Else
        '    Try

        '        Dim ppd As New PrintPreviewDialog

        '        ppd.Document = PrintDocument1

        '        ppd.WindowState = FormWindowState.Maximized
        '        ppd.StartPosition = FormStartPosition.CenterScreen
        '        ppd.ClientSize = New Size(762, 1024)

        '        ppd.ShowDialog()

        '    Catch ex As Exception
        '        MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

        '    End Try

        'End If


    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        'Dim da1 As New SqlClient.SqlDataAdapter
        'Dim da2 As New SqlClient.SqlDataAdapter
        'Dim da3 As New SqlClient.SqlDataAdapter
        'Dim da4 As New SqlClient.SqlDataAdapter
        'Dim Dt1 As New DataTable
        'Dim Dt3 As New DataTable
        'Dim cmd As New SqlClient.SqlCommand
        'Dim NewCode As String
        'Dim Pty_DcNo As String
        'Dim Clt_RcptDate As String
        'Dim sql As String = ""



        'Dim Nr As Long = 0

        'NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'prn_HdDt.Clear()
        'prn_DetDt.Clear()
        'prn_DetIndx = 0
        'prn_DetSNo = 0
        'prn_PageNo = 0

        'Try

        '    cmd.Connection = con

        '    da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*,e.*, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code from Vendor_PriceList_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN State_Head Csh ON b.Company_State_IdNo = Csh.State_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_Idno LEFT OUTER JOIN State_Head Lsh ON c.Ledger_State_IdNo = Lsh.State_IdNo LEFT OUTER JOIN Design_Inward_Head e ON a.Job_Code = e.Design_Inward_Code where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "'", con)
        '    prn_HdDt = New DataTable
        '    da1.Fill(prn_HdDt)

        '    If prn_HdDt.Rows.Count > 0 Then





        '        cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
        '        cmd.ExecuteNonQuery()

        '        ''vendor_Stock_Closing_Code
        '        ''Print_Place_IdNo
        '        ''Cloth_Delivery_Code
        '        ''Design_Inward_Code
        '        ''Rate_For
        '        ''sl_no
        '        ''ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Print_Place_Name").ToString) & " " & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Particulars").ToString)
        '        'Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
        '        'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("PARTY_DCNO").ToString), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
        '        'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(Clt_RcptDate), "dd-MM-yyyy").ToString, LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, pFont)
        '        'Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
        '        'Common_Procedures.Print_To_PrintDocument(e, Trim(ClrNm1), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 3, CurY, 0, 0, pFont)
        '        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Quantity").ToString), "##,##,##,##0"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 15, CurY, 1, 0, pFont)
        '        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(Rate), "##,##,##,##0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
        '        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(Amount), "##,##,##,##0.00"), PageWidth - 10, CurY, 1, 0, pFont)

        '        ''Vendor_PriceList_Cloth_Details
        '        '.Rows(n).Cells(0).Value = Val(SNo)
        '        '.Rows(n).Cells(1).Value = dt4.Rows(i).Item("Job_No").ToString
        '        '.Rows(n).Cells(2).Value = (dt4.Rows(i).Item("Particulars").ToString)
        '        '.Rows(n).Cells(3).Value = Format(Val(dt4.Rows(i).Item("Film_Charge").ToString), "###########0.00")
        '        '.Rows(n).Cells(4).Value = Format(Val(dt4.Rows(i).Item("Screen_Charge").ToString), "############0.00")
        '        '.Rows(n).Cells(5).Value = Format(Val(dt4.Rows(i).Item("Film_Alteration_Charge").ToString), "###########0.00")
        '        '.Rows(n).Cells(6).Value = Format(Val(dt4.Rows(i).Item("Screen_Alteration_Charge").ToString), "###########0.00")
        '        '.Rows(n).Cells(7).Value = dt4.Rows(i).Item("Design_Inward_Code").ToString

        '        '' vinoth
        '        'cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1,     Name1        ,     Name2              ,    Name3              ,     Name4 ,     Int2 ,     Name5     ,     Int3             ,    Name6            ,  Name7          ,  Weight1   ,     Name8    ,    Name9     ,    Name10        ) " & _
        '        '                  "select                 1  ,  PID.vendor_Stock_Closing_Code, PID.Cloth_Delivery_Code, PID.Design_Inward_Code, PID.Job_No, PID.sl_no, PID.PARTY_DCNO,  PID.Print_Place_IdNo, PPH.Print_Place_Name,  PID.Particulars, PID.Quantity, DIH.Rate_For , CDD.Order_No , PIH.Order_RefNo  FROM Vendor_PriceList_Head PIH INNER JOIN vendor_PriceList_Count_Details PID ON PID.vendor_Stock_Closing_Code = PIH.vendor_Stock_Closing_Code INNER JOIN Design_Inward_Head DIH ON PID.Design_Inward_Code = DIH.Design_Inward_Code LEFT OUTER JOIN Cloth_Delivery_Details CDD ON PID.vendor_Stock_Closing_Code = CDD.vendor_Stock_Closing_Code and PID.Cloth_Delivery_Code = CDD.Cloth_Delivery_Code LEFT OUTER JOIN Print_Place_Head PPH ON PID.Print_Place_IdNo = PPH.Print_Place_IdNo  where PID.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "' Order by PID.sl_no"
        '        'Nr = cmd.ExecuteNonQuery()




        '        'cmd.CommandText = " Insert into " & Trim(Common_Procedures.EntryTempTable) & "(		Int1,   		Name1        ,   		Name2              ,    Name3              ,  		Name4 ,      " & _
        '        '                    " Int2, Name5, Int3, Name6, Name7, " & _
        '        '                    " Weight1   ,    		Name8    ,    		Name9     ,    		Name10        )  " & _
        '        '                    " select   	1  ,  			PID.vendor_Stock_Closing_Code, 			PID.Cloth_Delivery_Code, 			PID.Design_Inward_Code,  " & _
        '        '                    " PID.Job_No, 			PID.sl_no, 			PID.PARTY_DCNO,  			PID.Print_Place_IdNo,  " & _
        '        '                    " PPH.Print_Place_Name,  			PID.Particulars, 			IQD.Quantity,  			DIH.Rate_For ,  " & _
        '        '                    " CDD.Order_No , 			PIH.Order_RefNo  			FROM Vendor_PriceList_Head PIH  " & _
        '        '                    " 	INNER JOIN vendor_PriceList_Count_Details PID ON PID.vendor_Stock_Closing_Code = PIH.vendor_Stock_Closing_Code  " & _
        '        '                    " INNER JOIN vendor_PriceList_EndsCount_Details IQD ON PID.vendor_Stock_Closing_Code = IQD.vendor_Stock_Closing_Code and PID.print_place_idno=IQD.print_place_idno " & _
        '        '                    " 	INNER JOIN Design_Inward_Head DIH ON PID.Design_Inward_Code = DIH.Design_Inward_Code  " & _
        '        '                    " 	LEFT OUTER JOIN Cloth_Delivery_Details CDD ON PID.vendor_Stock_Closing_Code = CDD.vendor_Stock_Closing_Code " & _
        '        '                    " and PID.Cloth_Delivery_Code = CDD.Cloth_Delivery_Code  " & _
        '        '                    " and PID.Print_place_idno=CDD.Print_place_idno " & _
        '        '                    " 	LEFT OUTER JOIN Print_Place_Head PPH ON PID.Print_Place_IdNo = PPH.Print_Place_IdNo  " & _
        '        '                    " where PID.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "' Order by PID.sl_no"




        '        'cmd.CommandText = " Insert into " & Trim(Common_Procedures.EntryTempTable) & "(		Int1,   		Name1        ,   		Name2              ,    Name3              ,  		Name4 ,      " & _
        '        '                          " Int2, Name5, Int3, Name6, Name7,  Weight1   ,    		Name8    ,    		Name9     ,    		Name10        ) " & _
        '        '                          " select   	1  ,  			 " & _
        '        '                          "                   IQD.vendor_Stock_Closing_Code, 	 " & _
        '        '                          "                   '' Cloth_Delivery_Code, " & _
        '        '                          "                   IQD.Design_Inward_Code,     " & _
        '        '                          "                   '' Job_No, 			 " & _
        '        '                          " 0 sl_no, 			 " & _
        '        '                          "                   '' PARTY_DCNO,  			 " & _
        '        '                          " IQD.Print_Place_IdNo,    " & _
        '        '                          " PPH.Print_Place_Name,  			 " & _
        '        '                         " PPH.Print_Place_Name,  	 " & _
        '        '                          " IQD.Quantity,  			 " & _
        '        '                          "                     DIH.Rate_For ,      " & _
        '        '                          " 0 Order_No , 			 " & _
        '        '                          " 0 Order_RefNo  			 " & _
        '        '                          " FROM Vendor_PriceList_Head PIH " & _
        '        '                          " INNER JOIN vendor_PriceList_EndsCount_Details IQD ON PIH.vendor_Stock_Closing_Code = IQD.vendor_Stock_Closing_Code 	 " & _
        '        '                            " INNER JOIN Design_Inward_Head DIH ON IQD.Design_Inward_Code = DIH.Design_Inward_Code " & _
        '        '                         " LEFT OUTER JOIN Print_Place_Head PPH ON IQD.Print_Place_IdNo = PPH.Print_Place_IdNo  " & _
        '        '                          "  where PIH.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "' Order by IQD.sl_no "


        '        'Nr = cmd.ExecuteNonQuery()


        '        ''cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1,     Name1       ,     Name2 ,     Name3             ,     Name4 ,     Int2 , Name5 , Int3 , Name6 ,       Name7      ,  Weight1 , Name8,                       Currency1      ) " & _
        '        ''                    "          select      2 , PID.vendor_Stock_Closing_Code,     ''    , DIH.Design_Inward_Code, PID.Job_No, PID.sl_no,  ''   ,   0  ,    ''  ,  'Screen Charge',     0    ,   '' ,  (Film_Charge+Screen_Charge+Film_Alteration_Charge+Screen_Alteration_Charge) FROM Vendor_PriceList_Cloth_Details PID INNER JOIN Design_Inward_Head DIH ON PID.Design_Inward_Code = DIH.Design_Inward_Code_forSelection where PID.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "' Order by PID.sl_no"
        '        ''nr = cmd.ExecuteNonQuery()

        '        'da2 = New SqlClient.SqlDataAdapter("select Name1 as vendor_Stock_Closing_Code, Name2 as Cloth_Delivery_Code, Name3 as Design_Inward_Code, Name4 as Job_No, Int2 as sl_no, Name5 as PARTY_DCNO, Int3 as Print_Place_IdNo, Name6 as Print_Place_Name, Name7 as Particulars,  Weight1  as Quantity, Currency1 as Screen_Charge, Name8 as Rate_For, Name9 as Order_No, Name10 as Order_RefNo  FROM " & Trim(Common_Procedures.EntryTempTable) & " Order by Int1, Int2", con)
        '        ''da2 = New SqlClient.SqlDataAdapter("select PID.*, DIH.*, PPH.Print_Place_Name FROM vendor_PriceList_Count_Details PID INNER JOIN Design_Inward_Head DIH ON PID.Design_Inward_Code = DIH.Design_Inward_Code LEFT JOIN Print_Place_Head PPH ON PID.Print_Place = PPH.Print_Place_IdNo  where PID.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "' Order by PID.sl_no", con)
        '        ' ''da2 = New SqlClient.SqlDataAdapter("select PID.*, PPH.*,IQD.* FROM vendor_PriceList_Count_Details PID INNER JOIN vendor_PriceList_EndsCount_Details IQD ON PID.vendor_Stock_Closing_Code = IQD.vendor_Stock_Closing_Code LEFT JOIN Print_Place_Head PPH ON PID.Print_Place = PPH.Print_Place_IdNo  where PID.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PID.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "' Order by PID.sl_no", con)
        '        '' ''da2 = New SqlClient.SqlDataAdapter("select PID.*,b.*,c.* ,cD.* ,CLRD.Cloth_Receipt_Date, CL.Colour_Name , Pp.* from vendor_PriceList_Count_Details PId INNER JOIN vendor_PriceList_EndsCount_Details IQD ON PId.vendor_Stock_Closing_Code = IQD.vendor_Stock_Closing_Code INNER JOIN Cloth_Delivery_Details cD ON pID.Cloth_Delivery_Code = cD.Cloth_Delivery_Code INNER JOIN Design_Inward_Head b ON PID.Design_Inward_Code = b.Design_Inward_Code INNER JOIN Design_InWard_PrintPlace_Details c ON PID.Design_Inward_Code = c.Design_Inward_Code LEFT JOIN Colour_Head CL ON cD.COLOUR_IDNO= CL.Colour_IdNo LEFT JOIN Print_Place_Head Pp ON PID.Print_Place_IdNo = Pp.Print_Place_IdNo INNER JOIN Cloth_Receipt_Details CLRD ON cD.Cloth_Receipt_Code = CLRD.Cloth_Receipt_Code  where PID.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PID.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "' Order by PID.sl_no", con)
        '        'prn_DetDt = New DataTable
        '        'da2.Fill(prn_DetDt)


        '        sql = " " & _
        '                " select   	1  ,  			 " & _
        '                "                   IQD.vendor_Stock_Closing_Code, 	 " & _
        '                "                   '' Cloth_Delivery_Code, " & _
        '                "                   IQD.Design_Inward_Code,     " & _
        '                "                   '' Job_No, 			 " & _
        '                " 0 sl_no, 			 " & _
        '                "                   '' PARTY_DCNO,  			 " & _
        '                " IQD.Print_Place_IdNo,    " & _
        '                " PPH.Print_Place_Name,  			 " & _
        '                " PPH.Print_Place_Name,  	 " & _
        '                " IQD.Quantity,  			 " & _
        '                " IQD.Rate, IQD.Amount, " & _
        '                "                     DIH.Rate_For ,      " & _
        '                " 0 Order_No , 			 " & _
        '                " 0 Order_RefNo  			 " & _
        '                " FROM Vendor_PriceList_Head PIH " & _
        '                " INNER JOIN vendor_PriceList_EndsCount_Details IQD ON PIH.vendor_Stock_Closing_Code = IQD.vendor_Stock_Closing_Code 	 " & _
        '                    " INNER JOIN Design_Inward_Head DIH ON IQD.Design_Inward_Code = DIH.Design_Inward_Code " & _
        '                " LEFT OUTER JOIN Print_Place_Head PPH ON IQD.Print_Place_IdNo = PPH.Print_Place_IdNo  " & _
        '                "  where PIH.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "' Order by IQD.sl_no "

        '        '"select Name1 as vendor_Stock_Closing_Code, Name2 as Cloth_Delivery_Code, Name3 as Design_Inward_Code, Name4 as Job_No, Int2 as sl_no, Name5 as PARTY_DCNO, Int3 as Print_Place_IdNo, Name6 as Print_Place_Name, Name7 as Particulars,  Weight1  as Quantity, Currency1 as Screen_Charge, Name8 as Rate_For, Name9 as Order_No, Name10 as Order_RefNo  FROM " & Trim(Common_Procedures.EntryTempTable) & " Order by Int1, Int2"
        '        'Nr = cmd.ExecuteNonQuery()


        '        'cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1,     Name1       ,     Name2 ,     Name3             ,     Name4 ,     Int2 , Name5 , Int3 , Name6 ,       Name7      ,  Weight1 , Name8,                       Currency1      ) " & _
        '        '                    "          select      2 , PID.vendor_Stock_Closing_Code,     ''    , DIH.Design_Inward_Code, PID.Job_No, PID.sl_no,  ''   ,   0  ,    ''  ,  'Screen Charge',     0    ,   '' ,  (Film_Charge+Screen_Charge+Film_Alteration_Charge+Screen_Alteration_Charge) FROM Vendor_PriceList_Cloth_Details PID INNER JOIN Design_Inward_Head DIH ON PID.Design_Inward_Code = DIH.Design_Inward_Code_forSelection where PID.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "' Order by PID.sl_no"
        '        'nr = cmd.ExecuteNonQuery()

        '        da2 = New SqlClient.SqlDataAdapter(sql, con)
        '        'da2 = New SqlClient.SqlDataAdapter("select PID.*, DIH.*, PPH.Print_Place_Name FROM vendor_PriceList_Count_Details PID INNER JOIN Design_Inward_Head DIH ON PID.Design_Inward_Code = DIH.Design_Inward_Code LEFT JOIN Print_Place_Head PPH ON PID.Print_Place = PPH.Print_Place_IdNo  where PID.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "' Order by PID.sl_no", con)
        '        ''da2 = New SqlClient.SqlDataAdapter("select PID.*, PPH.*,IQD.* FROM vendor_PriceList_Count_Details PID INNER JOIN vendor_PriceList_EndsCount_Details IQD ON PID.vendor_Stock_Closing_Code = IQD.vendor_Stock_Closing_Code LEFT JOIN Print_Place_Head PPH ON PID.Print_Place = PPH.Print_Place_IdNo  where PID.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PID.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "' Order by PID.sl_no", con)
        '        ' ''da2 = New SqlClient.SqlDataAdapter("select PID.*,b.*,c.* ,cD.* ,CLRD.Cloth_Receipt_Date, CL.Colour_Name , Pp.* from vendor_PriceList_Count_Details PId INNER JOIN vendor_PriceList_EndsCount_Details IQD ON PId.vendor_Stock_Closing_Code = IQD.vendor_Stock_Closing_Code INNER JOIN Cloth_Delivery_Details cD ON pID.Cloth_Delivery_Code = cD.Cloth_Delivery_Code INNER JOIN Design_Inward_Head b ON PID.Design_Inward_Code = b.Design_Inward_Code INNER JOIN Design_InWard_PrintPlace_Details c ON PID.Design_Inward_Code = c.Design_Inward_Code LEFT JOIN Colour_Head CL ON cD.COLOUR_IDNO= CL.Colour_IdNo LEFT JOIN Print_Place_Head Pp ON PID.Print_Place_IdNo = Pp.Print_Place_IdNo INNER JOIN Cloth_Receipt_Details CLRD ON cD.Cloth_Receipt_Code = CLRD.Cloth_Receipt_Code  where PID.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and PID.vendor_Stock_Closing_Code = '" & Trim(NewCode) & "' Order by PID.sl_no", con)
        '        prn_DetDt = New DataTable
        '        da2.Fill(prn_DetDt)


        '    Else
        '        MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        '    End If

        '    da1.Dispose()

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        'If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        ''If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1175" Then
        ''    Printing_Format2(e)
        ''Else
        ''    Printing_Format1(e)
        ''End If


        ''---printing_format3_printtech

    End Sub



    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub



    Private Sub cbo_WeaverName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_WeaverName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'GODOWN' or Ledger_Type = '' or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        cbo_WeaverName.Tag = cbo_WeaverName.Text
    End Sub

    Private Sub cbo_WeaverName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WeaverName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_WeaverName, dtp_date, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'GODOWN' or Ledger_Type = '' or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        If (e.KeyValue = 40 And cbo_WeaverName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If dgv_Count_details.Rows.Count > 0 Then
                dgv_Count_details.Focus()
                dgv_Count_details.CurrentCell = dgv_Count_details.Rows(0).Cells(1)
            End If
        End If
    End Sub

    Private Sub cbo_WeaverName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_WeaverName.KeyPress
        Dim LedIdNo As Integer

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_WeaverName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'GODOWN' or Ledger_Type = '' or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_WeaverName.Text)
            If Val(LedIdNo) <> 0 Then
                If Trim(UCase(cbo_WeaverName.Tag)) <> Trim(UCase(cbo_WeaverName.Text)) Then
                    btn_SHOWDETAILS_Click(sender, e)
                End If
            End If
            If dgv_Count_details.Rows.Count > 0 Then
                dgv_Count_details.Focus()
                dgv_Count_details.CurrentCell = dgv_Count_details.Rows(0).Cells(1)
            End If

        End If
    End Sub

    Private Sub cbo_WeaverName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WeaverName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "WEAVER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_WeaverName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub dgv_Count_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Count_details.CellEndEdit
        dgv_Count_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Count_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Count_details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Rect As Rectangle
        Dim LedID As Integer = 0
        Dim CloID As Integer = 0
        With dgv_Count_details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 1 Then

                If Cbo_Grid_CountName.Visible = False Or Val(Cbo_Grid_CountName.Tag) <> e.RowIndex Then

                    Cbo_Grid_CountName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    Cbo_Grid_CountName.DataSource = Dt1
                    Cbo_Grid_CountName.DisplayMember = "Count_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    Cbo_Grid_CountName.Left = .Left + Rect.Left
                    Cbo_Grid_CountName.Top = .Top + Rect.Top

                    Cbo_Grid_CountName.Width = Rect.Width
                    Cbo_Grid_CountName.Height = Rect.Height
                    Cbo_Grid_CountName.Text = .CurrentCell.Value

                    Cbo_Grid_CountName.Tag = Val(e.RowIndex)
                    Cbo_Grid_CountName.Visible = True

                    Cbo_Grid_CountName.BringToFront()
                    Cbo_Grid_CountName.Focus()



                End If

            Else
                Cbo_Grid_CountName.Visible = False

            End If

        End With
    End Sub

    Private Sub dgv_Count_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Count_details.CellLeave
        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub
        With dgv_Count_details
            If IsNothing(.CurrentCell) Then Exit Sub
            If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Count_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Count_details.CellValueChanged
        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub
        With dgv_Count_details
            If .Visible Then
                If IsNothing(.CurrentCell) Then Exit Sub
                If .CurrentCell.ColumnIndex = 2 Then

                    'If Common_Procedures.settings.Receipt_Delivery_InMeters = 0 Then
                    ' .CurrentRow.Cells(5).Value = Val(.CurrentRow.Cells(3).Value) * Val(.CurrentRow.Cells(4).Value)
                    'End If


                End If

            End If
        End With

    End Sub

    Private Sub dgv_Count_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Count_details.EditingControlShowing
        dgtxt_CountDetails = Nothing
        ' If dgv_YarnDetails.CurrentCell.ColumnIndex > 2 Then
        dgtxt_CountDetails = CType(dgv_Count_details.EditingControl, DataGridViewTextBoxEditingControl)
        ' End If
    End Sub

    Private Sub dgtxt_CountDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_CountDetails.Enter
        dgv_ActiveCtrl_Name = dgv_Count_details.Name
        dgv_Count_details.EditingControl.BackColor = Color.Lime
        dgv_Count_details.EditingControl.ForeColor = Color.Blue
        dgv_Count_details.SelectAll()
    End Sub

    Private Sub dgtxt_CountDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_CountDetails.KeyDown
        With dgv_Count_details

            If e.KeyValue = Keys.Delete Then
                If Val(.Rows(.CurrentCell.RowIndex).Cells(2).Value) <> 0 Then
                    e.Handled = True
                End If
            End If
        End With

    End Sub

    Private Sub dgtxt_CountDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_CountDetails.KeyPress

        With dgv_Count_details
            If .Visible Then

                If .CurrentCell.ColumnIndex = 2 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If

            End If
        End With

    End Sub

    Private Sub dgtxt_CountDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_CountDetails.KeyUp

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Count_Details_KeyUp(sender, e)
        End If

    End Sub

    Private Sub dgv_Count_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Count_details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_Count_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Count_details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            With dgv_Count_details

                n = .CurrentRow.Index

                If n = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If



            End With

        End If

    End Sub

    Private Sub dgv_Count_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Count_details.LostFocus
        On Error Resume Next
        If IsNothing(dgv_Count_details.CurrentCell) Then Exit Sub
        dgv_Count_details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Count_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Count_details.RowsAdded
        Dim n As Integer
        If FrmLdSTS = True Then Exit Sub
        With dgv_Count_details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub


    Private Sub msk_date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
        End If

    End Sub


    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
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
        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        End If
    End Sub
    Private Sub dtp_Date_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_date.ValueChanged
        msk_date.Text = dtp_date.Text
    End Sub

    Private Sub dtp_Date_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_date.Enter
        msk_date.Focus()
        msk_date.SelectionStart = 0
    End Sub




    Private Sub dgv_EndsCount_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_EndsCount_Details.CellEndEdit
        dgv_EndsCount_Details_CellLeave(sender, e)
    End Sub
    Private Sub dgv_EndsCount_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_EndsCount_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Da3 As New SqlClient.SqlDataAdapter
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Dt5 As New DataTable
        Dim Rect As Rectangle

        With dgv_EndsCount_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If


            If e.ColumnIndex = 1 Then

                If Cbo_Grid_EndsCountName.Visible = False Or Val(Cbo_Grid_EndsCountName.Tag) <> e.RowIndex Then

                    Cbo_Grid_EndsCountName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select EndsCount_Name from EndsCount_Head order by EndsCount_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    Cbo_Grid_EndsCountName.DataSource = Dt1
                    Cbo_Grid_EndsCountName.DisplayMember = "EndsCount_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    Cbo_Grid_EndsCountName.Left = .Left + Rect.Left
                    Cbo_Grid_EndsCountName.Top = .Top + Rect.Top

                    Cbo_Grid_EndsCountName.Width = Rect.Width
                    Cbo_Grid_EndsCountName.Height = Rect.Height
                    Cbo_Grid_EndsCountName.Text = .CurrentCell.Value

                    Cbo_Grid_EndsCountName.Tag = Val(e.RowIndex)
                    Cbo_Grid_EndsCountName.Visible = True

                    Cbo_Grid_EndsCountName.BringToFront()
                    Cbo_Grid_EndsCountName.Focus()



                End If

            Else
                Cbo_Grid_EndsCountName.Visible = False

            End If

        End With

    End Sub

    Private Sub dgv_EndsCount_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_EndsCount_Details.CellLeave
        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub
        With dgv_EndsCount_Details
            If .Visible Then
                If IsNothing(.CurrentCell) Then Exit Sub
                If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then


                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")


                    Else
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub dgv_EndsCount_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_EndsCount_Details.CellValueChanged
        On Error Resume Next

        With dgv_EndsCount_Details
            If .Visible Then
                If IsNothing(.CurrentCell) Then Exit Sub
                If .CurrentCell.ColumnIndex = 2 Then


                    ' .CurrentRow.Cells(5).Value = Val(.CurrentRow.Cells(3).Value) * Val(.CurrentRow.Cells(4).Value)



                    ' NetAmount_Calculation()
                End If

            End If
        End With

    End Sub

    Private Sub dgv_QuantityDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_EndsCount_Details.EditingControlShowing
        dgtxt_EndsCountDetails = Nothing
        ' If dgv_YarnDetails.CurrentCell.ColumnIndex > 2 Then
        dgtxt_EndsCountDetails = CType(dgv_EndsCount_Details.EditingControl, DataGridViewTextBoxEditingControl)
        ' End If
    End Sub

    Private Sub dgtxt_EndsCountDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_EndsCountDetails.Enter
        dgv_ActiveCtrl_Name = dgv_EndsCount_Details.Name
        dgv_EndsCount_Details.EditingControl.BackColor = Color.Lime
        dgv_EndsCount_Details.EditingControl.ForeColor = Color.Blue
        dgv_EndsCount_Details.SelectAll()
    End Sub

    Private Sub dgtxt_EndsCountDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_EndsCountDetails.KeyDown
        With dgv_Count_details

            If e.KeyValue = Keys.Delete Then
                If Val(.Rows(.CurrentCell.RowIndex).Cells(2).Value) <> 0 Then
                    e.Handled = True
                End If
            End If
        End With

    End Sub

    Private Sub dgtxt_EndsCountDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_EndsCountDetails.KeyPress

        With dgv_EndsCount_Details
            If .Visible Then

                If .CurrentCell.ColumnIndex = 2 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If

            End If
        End With

    End Sub

    Private Sub dgtxt_EndsCountDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_EndsCountDetails.KeyUp

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_EndsCount_Details_KeyUp(sender, e)
        End If

    End Sub

    Private Sub dgv_EndsCount_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_EndsCount_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_EndsCount_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_EndsCount_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            With dgv_EndsCount_Details

                n = .CurrentRow.Index

                If n = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If



            End With

        End If

    End Sub

    Private Sub dgv_EndsCount_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_EndsCount_Details.LostFocus
        On Error Resume Next
        If IsNothing(dgv_EndsCount_Details.CurrentCell) Then Exit Sub
        dgv_EndsCount_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_EndsCount_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_EndsCount_Details.RowsAdded
        Dim n As Integer

        With dgv_EndsCount_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub dgv_Cloth_Details_CellEndEdit(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Cloth_Details.CellEndEdit
        dgv_Cloth_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Cloth_Details_CellEnter(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Cloth_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Rect As Rectangle
        Dim LedID As Integer = 0
        Dim CloID As Integer = 0
        With dgv_Cloth_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 1 Then

                If cbo_Grid_ClothName.Visible = False Or Val(cbo_Grid_ClothName.Tag) <> e.RowIndex Then

                    cbo_Grid_ClothName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_ClothName.DataSource = Dt1
                    cbo_Grid_ClothName.DisplayMember = "Cloth_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_ClothName.Left = .Left + Rect.Left
                    cbo_Grid_ClothName.Top = .Top + Rect.Top

                    cbo_Grid_ClothName.Width = Rect.Width
                    cbo_Grid_ClothName.Height = Rect.Height
                    cbo_Grid_ClothName.Text = .CurrentCell.Value

                    cbo_Grid_ClothName.Tag = Val(e.RowIndex)
                    cbo_Grid_ClothName.Visible = True

                    cbo_Grid_ClothName.BringToFront()
                    cbo_Grid_ClothName.Focus()



                End If

            Else
                cbo_Grid_ClothName.Visible = False

            End If

        End With
    End Sub

    Private Sub dgv_Cloth_Details_CellLeave(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Cloth_Details.CellLeave
        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub

        With dgv_Cloth_Details
            If IsNothing(.CurrentCell) Then Exit Sub
            If .CurrentCell.ColumnIndex = 2 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Cloth_Details_CellValueChanged(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Cloth_Details.CellValueChanged
        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub

        If IsNothing(dgv_Cloth_Details.CurrentCell) Then Exit Sub
        With dgv_Cloth_Details
            If .Visible Then

                If .CurrentCell.ColumnIndex = 2 Then

                    'If Common_Procedures.settings.Receipt_Delivery_InMeters = 0 Then
                    ' .CurrentRow.Cells(5).Value = Val(.CurrentRow.Cells(3).Value) * Val(.CurrentRow.Cells(4).Value)
                    'End If


                End If

            End If
        End With
    End Sub

    Private Sub dgv_Cloth_Details_EditingControlShowing(sender As Object, e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Cloth_Details.EditingControlShowing
        dgtxt_clothDetails = Nothing
        ' If dgv_YarnDetails.CurrentCell.ColumnIndex > 2 Then
        dgtxt_clothDetails = CType(dgv_Cloth_Details.EditingControl, DataGridViewTextBoxEditingControl)
        ' End If
    End Sub

    Private Sub dgv_Cloth_Details_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dgv_Cloth_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_Cloth_Details_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dgv_Cloth_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            With dgv_Cloth_Details

                n = .CurrentRow.Index

                If n = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If



            End With

        End If
    End Sub

    Private Sub dgv_Cloth_Details_LostFocus(sender As Object, e As System.EventArgs) Handles dgv_Cloth_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Cloth_Details.CurrentCell) Then dgv_Cloth_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Cloth_Details_RowsAdded(sender As Object, e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Cloth_Details.RowsAdded
        Dim n As Integer
        If FrmLdSTS = True Then Exit Sub

        If IsNothing(dgv_Cloth_Details.CurrentCell) Then Exit Sub
        With dgv_Cloth_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub



    Private Sub cbo_Grid_COuntName_GotFocus(sender As Object, e As System.EventArgs) Handles Cbo_Grid_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_COuntName_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Grid_CountName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Grid_CountName, Nothing, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        With dgv_Count_details

            If (e.KeyValue = 38 And Cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex <= 0 Then
                    cbo_WeaverName.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(2)
                    .CurrentCell.Selected = True
                End If

            End If

            If (e.KeyValue = 40 And Cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    If dgv_EndsCount_Details.Rows.Count > 0 Then
                        dgv_EndsCount_Details.Focus()
                        dgv_EndsCount_Details.CurrentCell = dgv_EndsCount_Details.Rows(0).Cells(1)
                    Else
                    End If

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(2)
                    .CurrentCell.Selected = True

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_COuntName_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_Grid_CountName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Dim vCloRate As Single = 0
        Dim trpt_Idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Grid_CountName, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            e.Handled = True





            With dgv_Count_details
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" Then
                    ' btn_save_Click(sender, e)
                    If dgv_EndsCount_Details.Rows.Count > 0 Then
                        dgv_EndsCount_Details.Focus()
                        dgv_EndsCount_Details.CurrentCell = dgv_EndsCount_Details.Rows(0).Cells(1)
                    Else
                    End If


                Else

                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(2)


                End If
            End With

        End If
    End Sub

    Private Sub cbo_Grid_COuntName_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Grid_CountName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_Grid_CountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub
    '-----

    Private Sub cbo_Grid_EndsCOuntName_GotFocus(sender As Object, e As System.EventArgs) Handles Cbo_Grid_EndsCountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_EndsCOuntName_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Grid_EndsCountName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Grid_EndsCountName, Nothing, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

        With dgv_EndsCount_Details

            If (e.KeyValue = 38 And Cbo_Grid_EndsCountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex <= 0 Then
                    dgv_Count_details.Focus()
                    dgv_Count_details.CurrentCell = dgv_Count_details.Rows(0).Cells(1)
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(2)
                    .CurrentCell.Selected = True
                End If

            End If

            If (e.KeyValue = 40 And Cbo_Grid_EndsCountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    If dgv_Cloth_Details.Rows.Count > 0 Then
                        dgv_Cloth_Details.Focus()
                        dgv_Cloth_Details.CurrentCell = dgv_Cloth_Details.Rows(0).Cells(1)
                    Else
                        '----
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    .CurrentCell.Selected = True

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_EndsCOuntName_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_Grid_EndsCountName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Dim vCloRate As Single = 0
        Dim trpt_Idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Grid_EndsCountName, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            e.Handled = True





            With dgv_EndsCount_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    If dgv_Cloth_Details.Rows.Count > 0 Then
                        dgv_Cloth_Details.Focus()
                        dgv_Cloth_Details.CurrentCell = dgv_Cloth_Details.Rows(0).Cells(1)
                    Else
                        '----
                    End If


                Else

                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(2)


                End If
            End With

        End If
    End Sub

    Private Sub cbo_Grid_EndsCOuntName_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Grid_EndsCountName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New EndsCount_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_Grid_EndsCountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    '------

    Private Sub cbo_Grid_ClothName_GotFocus(sender As Object, e As System.EventArgs) Handles cbo_Grid_ClothName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_ClothName_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_ClothName, Nothing, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

        With dgv_Cloth_Details

            If (e.KeyValue = 38 And cbo_Grid_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex <= 0 Then

                    dgv_Cloth_Details.Focus()
                    dgv_Cloth_Details.CurrentCell = dgv_Cloth_Details.Rows(0).Cells(1)



                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(2)
                    .CurrentCell.Selected = True
                End If

            End If

            If (e.KeyValue = 40 And cbo_Grid_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                        save_record()

                    Else

                        If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()


                    End If


                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    .CurrentCell.Selected = True

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_ClothName_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_ClothName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Dim vCloRate As Single = 0
        Dim trpt_Idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_ClothName, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            e.Handled = True





            With dgv_Cloth_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                        save_record()

                    Else

                        If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()


                    End If


                Else

                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(2)


                End If
            End With

        End If

    End Sub

    Private Sub cbo_Grid_ClothName_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_ClothName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub dgtxt_clothDetails_Enter(sender As Object, e As System.EventArgs) Handles dgtxt_clothDetails.Enter
        dgv_ActiveCtrl_Name = dgv_Cloth_Details.Name
        dgv_Cloth_Details.EditingControl.BackColor = Color.Lime
        dgv_Cloth_Details.EditingControl.ForeColor = Color.Blue
        dgv_Cloth_Details.SelectAll()
    End Sub

    Private Sub dgtxt_clothDetails_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_clothDetails.KeyDown
        With dgv_Cloth_Details

            If e.KeyValue = Keys.Delete Then
                If Val(.Rows(.CurrentCell.RowIndex).Cells(2).Value) <> 0 Then
                    e.Handled = True
                End If
            End If
        End With
    End Sub

    Private Sub dgtxt_clothDetails_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_clothDetails.KeyPress
        With dgv_Cloth_Details
            If .Visible Then

                If .CurrentCell.ColumnIndex = 2 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If

            End If
        End With
    End Sub

    Private Sub dgtxt_clothDetails_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_clothDetails.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Cloth_Details_KeyUp(sender, e)
        End If
    End Sub

    Private Sub Cbo_Grid_CountName_TextChanged(sender As Object, e As System.EventArgs) Handles Cbo_Grid_CountName.TextChanged
        Try
            If Cbo_Grid_CountName.Visible Then

                If IsNothing(dgv_Count_details.CurrentCell) Then Exit Sub

                With dgv_Count_details
                    If Val(Cbo_Grid_CountName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(Cbo_Grid_CountName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Cbo_Grid_EndsCountName_TextChanged(sender As Object, e As System.EventArgs) Handles Cbo_Grid_EndsCountName.TextChanged
        Try
            If Cbo_Grid_EndsCountName.Visible Then

                If IsNothing(dgv_EndsCount_Details.CurrentCell) Then Exit Sub

                With dgv_EndsCount_Details
                    If Val(Cbo_Grid_EndsCountName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(Cbo_Grid_EndsCountName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Grid_ClothName_TextChanged(sender As Object, e As System.EventArgs) Handles cbo_Grid_ClothName.TextChanged
        Try
            If cbo_Grid_ClothName.Visible Then

                If IsNothing(dgv_Cloth_Details.CurrentCell) Then Exit Sub

                With dgv_Cloth_Details
                    If Val(cbo_Grid_ClothName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_ClothName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_WeaverName_LostFocus(sender As Object, e As EventArgs) Handles cbo_WeaverName.LostFocus
        Dim LedIdNo As Integer = 0

        If Trim(cbo_WeaverName.Text) <> "" Then
            If Trim(UCase(cbo_WeaverName.Tag)) <> Trim(UCase(cbo_WeaverName.Text)) Then
                btn_SHOWDETAILS_Click(sender, e)
            End If
        End If

    End Sub

    Private Sub btn_SHOWDETAILS_Click(sender As Object, e As EventArgs) Handles btn_SHOWDETAILS.Click
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim LedIdNo As Integer
        Dim vLEDNM As String

        vLEDNM = cbo_WeaverName.Text
        cbo_WeaverName.Tag = cbo_WeaverName.Text
        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_WeaverName.Text)
        If Val(LedIdNo) <> 0 Then

            da1 = New SqlClient.SqlDataAdapter("select a.* from Vendor_PriceList_Head a Where a.Weaver_idno = " & Str(Val(LedIdNo)), con)
            dt1 = New DataTable
            da1.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                move_record(Val(dt1.Rows(0).Item("Vendor_PriceList_IdNo").ToString))
            Else
                cbo_WeaverName.Text = vLEDNM
                cbo_WeaverName.Enabled = False
                btn_SHOWDETAILS.Enabled = False
            End If
            dt1.Clear()

        End If
    End Sub

End Class