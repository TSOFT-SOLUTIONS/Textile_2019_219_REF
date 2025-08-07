Public Class Finished_Product_Opening_Stock
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Pk_Condition As String = "OPENI-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private OpYrCode As String

    Private WithEvents dgtxt_details As New DataGridViewTextBoxEditingControl

    Private Sub clear()

        New_Entry = False

        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black

        pnl_Back.Enabled = True

        lbl_IdNo.Text = ""
        cbo_FinishedProductName.Text = ""
        txt_OnFloorQuantity.Text = ""
        txt_MeterQty.Text = ""
        txt_OnFloorMeter.Text = ""

        cbo_Grid_RackNo.Visible = False
        cbo_Grid_RackNo.Text = ""
        dgv_Details.Rows.Clear()
        Grid_DeSelect()

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
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

        If Me.ActiveControl.Name <> cbo_Grid_RackNo.Name Then
            cbo_Grid_RackNo.Visible = False
            cbo_Grid_RackNo.Tag = -1
        End If

        If Me.ActiveControl.Name <> dgv_Details.Name Then
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
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(44, 61, 90)
                Prec_ActCtrl.ForeColor = Color.White
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

    Private Sub move_record(ByVal IdNo As Integer)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim Sno As Integer, n As Integer
        Dim NewCode As String

        If Val(idno) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Val(idno)) & "/" & Trim(OpYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.Processed_Item_IdNo, a.Processed_Item_Name, a.Meter_Qty from Processed_Item_Head a Where a.Processed_Item_IdNo = " & Str(Val(IdNo)) & " and Processed_Item_Type = 'FP'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_IdNo.Text = Val(dt1.Rows(0).Item("Processed_Item_IdNo").ToString)
                cbo_FinishedProductName.Text = dt1.Rows(0).Item("Processed_Item_Name").ToString
                txt_MeterQty.Text = Format(Val(dt1.Rows(0).Item("Meter_Qty").ToString), "#########0.00")

                da2 = New SqlClient.SqlDataAdapter("Select * from Stock_Item_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_IdNo = " & Str(Val(IdNo)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Rack_IdNo = 0", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                If dt2.Rows.Count > 0 Then
                    If IsDBNull(dt2.Rows(0).Item(0).ToString) = False Then
                        txt_OnFloorQuantity.Text = Val(dt2.Rows(0).Item("Quantity").ToString)
                        txt_MeterQty.Text = Format(Val(dt2.Rows(0).Item("Meter_Qty").ToString), "#########0.00")
                        txt_OnFloorMeter.Text = Format(Val(dt2.Rows(0).Item("Meters").ToString), "#########0.00")
                    End If
                End If
                dt2.Clear()

                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Rack_No from Stock_Item_Processing_Details a, Rack_Head b where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Item_IdNo = " & Str(Val(IdNo)) & " and a.Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Rack_IdNo <> 0 and a.Rack_IdNo = b.Rack_IdNo Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                Sno = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        Sno = Sno + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(Sno)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Rack_No").ToString
                        dgv_Details.Rows(n).Cells(2).Value = Val(dt2.Rows(i).Item("Quantity").ToString)
                        dgv_Details.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Meter_Qty").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")

                    Next i

                End If

                Total_Calculation()

            Else
                new_record()

            End If

            dt1.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If cbo_FinishedProductName.Visible And cbo_FinishedProductName.Enabled Then cbo_FinishedProductName.Focus()

        End Try



    End Sub

    Private Sub Finished_Product_Opening_Stock_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_FinishedProductName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "FINISHEDPRODUCT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_FinishedProductName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_RackNo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "RACK" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_RackNo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            '---MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            FrmLdSTS = False

        End Try

    End Sub

    Private Sub Finished_Product_Opening_Stock_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable

        Me.Text = ""

        con.Open()

        da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head Where (Processed_Item_Type = 'FP' or Processed_Item_IdNo = 0) order by Processed_Item_Name", con)
        da.Fill(dt1)
        cbo_FinishedProductName.DataSource = dt1
        cbo_FinishedProductName.DisplayMember = "Processed_Item_Name"

        da = New SqlClient.SqlDataAdapter("select Rack_No from Rack_Head order by Rack_No", con)
        da.Fill(dt2)
        cbo_Grid_RackNo.DataSource = dt2
        cbo_Grid_RackNo.DisplayMember = "Rack_No"

        AddHandler cbo_FinishedProductName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_RackNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_MeterQty.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OnFloorMeter.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OnFloorQuantity.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_FinishedProductName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_RackNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_MeterQty.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OnFloorMeter.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OnFloorQuantity.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus

        OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
        OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Finished_Product_Opening_Stock_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Finished_Product_Opening_Stock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then
                If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub
                Else
                    Close_Form()
                End If
               
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

            ElseIf pnl_Back.Enabled = True Then
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = False Then

                With dgv1

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                    save_record()
                                Else
                                    cbo_FinishedProductName.Focus()
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
                                txt_OnFloorMeter.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.Columns.Count - 1)

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

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String
        Dim ProName As String

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Finished_Product_Opening_Stock, "~L~") = 0 And InStr(Common_Procedures.UR.Finished_Product_Opening_Stock, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        ProName = Common_Procedures.Processed_Item_IdNoToName(con, Val(lbl_IdNo.Text))

        If Trim(ProName) = "" Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Val(lbl_IdNo.Text)) & "/" & Trim(OpYrCode)

            cmd.Connection = con

            cmd.CommandText = "Delete from Stock_Item_Processing_Details Where Item_IdNo = " & Str(Val(lbl_IdNo.Text)) & " and Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            new_record()

            MessageBox.Show("Deleted Successfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If cbo_FinishedProductName.Enabled = True And cbo_FinishedProductName.Visible = True Then cbo_FinishedProductName.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '-----
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select top 1 Processed_Item_IdNo from Processed_Item_Head where Processed_Item_IdNo <> 0 and Processed_Item_Type = 'FP' Order by Processed_Item_IdNo"
            dr = cmd.ExecuteReader

            movno = 0
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = Val(dr(0).ToString)
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As Integer = 0
        Dim OrdByNo As Single

        Try

            OrdByNo = Val(lbl_IdNo.Text)

            da = New SqlClient.SqlDataAdapter("select top 1 Processed_Item_IdNo from Processed_Item_Head where Processed_Item_IdNo > " & Str(Val(OrdByNo)) & " and Processed_Item_Type = 'FP' Order by Processed_Item_IdNo", con)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As Integer = 0
        Dim OrdByNo As Single

        Try

            OrdByNo = Val(lbl_IdNo.Text)

            cmd.Connection = con
            cmd.CommandText = "select top 1 Processed_Item_IdNo from Processed_Item_Head where Processed_Item_IdNo < " & Str(Val(OrdByNo)) & " and Processed_Item_Type = 'FP' Order by Processed_Item_IdNo desc"

            dr = cmd.ExecuteReader

            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = Val(dr(0).ToString)
                    End If
                End If
            End If
            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try


    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter("select top 1 Processed_Item_IdNo from Processed_Item_Head where Processed_Item_IdNo <> 0 and Processed_Item_Type = 'FP' Order by Processed_Item_IdNo desc", con)
        Dim dt As New DataTable
        Dim movno As Integer

        Try
            da.Fill(dt)

            movno = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Val(dt.Rows(0)(0).ToString)
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
        Dim dt2 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            da = New SqlClient.SqlDataAdapter("select max(Processed_Item_IdNo) from Processed_Item_Head where Processed_Item_IdNo <> 0", con)
            da.Fill(dt)

            NewID = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    NewID = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            lbl_IdNo.Text = Val(NewID) + 1

            lbl_IdNo.ForeColor = Color.Red

            If cbo_FinishedProductName.Enabled And cbo_FinishedProductName.Visible Then cbo_FinishedProductName.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        '-----
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '-----
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim ProName As String, Rac_IdNo As String
        Dim Sno As Integer = 0
        Dim OpDate As Date
        Dim DlvID As Integer
        Dim RecID As Integer

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Finished_Product_Opening_Stock, New_Entry) = False Then Exit Sub

        If Trim(cbo_FinishedProductName.Text) = "" Then
            MessageBox.Show("Invalid Finished Product Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_FinishedProductName.Enabled Then cbo_FinishedProductName.Focus()
            Exit Sub
        End If

        ProName = Common_Procedures.Processed_Item_IdNoToName(con, Val(lbl_IdNo.Text))
        If Trim(ProName) = "" Then
            MessageBox.Show("Invalid Finished Product Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_FinishedProductName.Enabled Then cbo_FinishedProductName.Focus()
            Exit Sub
        End If

        With dgv_Details
            For i = 0 To .RowCount - 1
                If Val(.Rows(i).Cells(2).Value) <> 0 Then

                    Rac_IdNo = Common_Procedures.Rack_NoToIdNo(con, .Rows(i).Cells(1).Value)
                    If Rac_IdNo = 0 Then
                        MessageBox.Show("Invalid Rack No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If
                        Exit Sub
                    End If


                End If
            Next

        End With

        tr = con.BeginTransaction

        Try

            OpDate = CDate("01-04-" & Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4))
            OpDate = DateAdd(DateInterval.Day, -1, OpDate)

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@OpeningDate", OpDate)

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Val(lbl_IdNo.Text)) & "/" & Trim(OpYrCode)

            DlvID = 0
            RecID = 0
            If Val(txt_OnFloorQuantity.Text) < 0 Then
                RecID = Val(Common_Procedures.CommonLedger.Godown_Ac)
            Else
                DlvID = Val(Common_Procedures.CommonLedger.Godown_Ac)
            End If

            cmd.CommandText = "Delete from Stock_Item_Processing_Details Where Item_IdNo = " & Str(Val(lbl_IdNo.Text)) & " and Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into Stock_Item_Processing_Details ( Reference_Code ,            Company_IdNo          ,            Reference_No      ,            For_OrderBy         , Reference_Date,  DeliveryTo_StockIdNo   ,  ReceivedFrom_StockIdNo, Delivery_PartyIdNo, Received_PartyIdNo, Entry_ID, Party_Bill_No, Particulars, SL_No,             Item_IdNo          , Rack_IdNo,                      Quantity                       ,                          Meter_Qty           ,                      Meters                       ) " & _
                                    " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_IdNo.Text) & "', " & Str(Val(lbl_IdNo.Text)) & ",   @OpeningDate,  " & Str(Val(DlvID)) & ", " & Str(Val(RecID)) & ",          0        ,          0        ,     ''  ,     ''  ,      ''      ,      -1 , " & Str(Val(lbl_IdNo.Text)) & ",     0    , " & Str(Math.Abs(Val(txt_OnFloorQuantity.Text))) & ", " & Str(Math.Abs(Val(txt_MeterQty.Text))) & ", " & Str(Math.Abs(Val(txt_OnFloorMeter.Text))) & " ) "
            cmd.ExecuteNonQuery()

            Sno = 0

            For i = 0 To dgv_Details.RowCount - 1
                If Val(dgv_Details.Rows(i).Cells(2).Value) <> 0 Then

                    Sno = Sno + 1

                    Rac_IdNo = Common_Procedures.Rack_NoToIdNo(con, dgv_Details.Rows(i).Cells(1).Value, tr)

                    DlvID = 0
                    RecID = 0
                    If Val(dgv_Details.Rows(i).Cells(2).Value) < 0 Then
                        RecID = Val(Common_Procedures.CommonLedger.Godown_Ac)
                    Else
                        DlvID = Val(Common_Procedures.CommonLedger.Godown_Ac)
                    End If

                    cmd.CommandText = "Insert into Stock_Item_Processing_Details ( Reference_Code ,            Company_IdNo          ,            Reference_No      ,            For_OrderBy         , Reference_Date,  DeliveryTo_StockIdNo   ,  ReceivedFrom_StockIdNo, Delivery_PartyIdNo, Received_PartyIdNo, Entry_ID, Party_Bill_No, Particulars,         SL_No        ,             Item_IdNo          ,         Rack_IdNo         ,                                          Quantity             ,                                          Meter_Qty            ,                                          Meters                ) " & _
                                            " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_IdNo.Text) & "', " & Str(Val(lbl_IdNo.Text)) & ",   @OpeningDate,  " & Str(Val(DlvID)) & ", " & Str(Val(RecID)) & ",          0        ,          0        ,     ''  ,     ''       ,      ''    , " & Str(Val(Sno)) & ", " & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(Rac_IdNo)) & ", " & Str(Math.Abs(Val(dgv_Details.Rows(i).Cells(2).Value))) & ", " & Str(Math.Abs(Val(dgv_Details.Rows(i).Cells(3).Value))) & ", " & Str(Math.Abs(Val(dgv_Details.Rows(i).Cells(4).Value))) & " ) "
                    cmd.ExecuteNonQuery()

                End If
            Next

            tr.Commit()



            MessageBox.Show("Saved Successfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)


            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_IdNo.Text)
                End If
            Else
                move_record(lbl_IdNo.Text)
            End If

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            cmd.Dispose()
            tr.Dispose()

            If cbo_FinishedProductName.Enabled And cbo_FinishedProductName.Visible Then cbo_FinishedProductName.Focus()

        End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-----
    End Sub

    Private Sub cbo_FinishedProductName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_FinishedProductName.GotFocus
        cbo_FinishedProductName.Tag = cbo_FinishedProductName.Text
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Processed_Item_Head", "Processed_Item_Name", "( Processed_Item_Type = 'FP' and Verified_Status = 1 )", "(Processed_Item_IdNo = 0)")
    End Sub

    Private Sub cbo_FinishedProductName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_FinishedProductName.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_FinishedProductName, Nothing, txt_OnFloorQuantity, "Processed_Item_Head", "Processed_Item_Name", "( Processed_Item_Type = 'FP' and Verified_Status = 1)", "(Processed_Item_IdNo = 0)")
            With cbo_FinishedProductName
                If (e.KeyValue = 38 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    e.Handled = True
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_FinishedProductName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_FinishedProductName.KeyPress
        Dim ItmID As Integer = 0
        Dim Mtr_Qty As Single = 0

        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_FinishedProductName, Nothing, "Processed_Item_Head", "Processed_Item_Name", "( Processed_Item_Type = 'FP' and Verified_Status = 1 )", "(Processed_Item_IdNo = 0)")

            If Asc(e.KeyChar) = 13 Then
                If Trim(UCase(cbo_FinishedProductName.Tag)) <> Trim(UCase(cbo_FinishedProductName.Text)) Then
                    ItmID = Common_Procedures.Processed_Item_NameToIdNo(con, cbo_FinishedProductName.Text)
                    If Val(ItmID) <> 0 Then
                        move_record(ItmID)
                    End If
                End If


                If Val(txt_MeterQty.Text) = 0 Then

                    Mtr_Qty = Common_Procedures.get_FieldValue(con, "Processed_Item_Head", "Meter_Qty", "(Processed_Item_IdNo = " & Str(Val(ItmID)) & ")")

                    txt_MeterQty.Text = Format(Val(Mtr_Qty), "#########0.00")

                End If

                txt_OnFloorQuantity.Focus()

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_FinishedProductName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_FinishedProductName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New FinishedProduct_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_FinishedProductName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_RackNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_RackNo.GotFocus
        Try
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Rack_Head", "Rack_no", "", "(Rack_IdNo = 0)")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_RackNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_RackNo.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_RackNo, Nothing, Nothing, "Rack_Head", "Rack_no", "", "(Rack_IdNo = 0)")

            With cbo_Grid_RackNo

                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True

                    With dgv_Details
                        If Val(.CurrentCell.RowIndex) <= 0 Then
                            txt_OnFloorMeter.Focus()

                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(4)
                            .CurrentCell.Selected = True

                        End If
                    End With

                ElseIf e.KeyValue = 40 And .DroppedDown = False Then

                    e.Handled = True
                    With dgv_Details
                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

                            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                save_record()
                            Else
                                cbo_FinishedProductName.Focus()
                            End If

                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                            .CurrentCell.Selected = True


                        End If
                    End With

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_RackNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_RackNo.KeyPress
        Dim Mtr_Qty As Single = 0
        Dim ItmID As String = ""

        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_RackNo, Nothing, "Rack_Head", "Rack_no", "", "(Rack_IdNo = 0)")
            If Asc(e.KeyChar) = 13 Then
                With dgv_Details
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_RackNo.Text)
                    If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

                        If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                            save_record()

                        Else
                            cbo_FinishedProductName.Focus()

                        End If

                    Else

                        If Val(.CurrentRow.Cells(3).Value) = 0 Then

                            ItmID = Common_Procedures.Processed_Item_NameToIdNo(con, cbo_FinishedProductName.Text)
                            Mtr_Qty = Common_Procedures.get_FieldValue(con, "Processed_Item_Head", "Meter_Qty", "(Processed_Item_IdNo = " & Str(Val(ItmID)) & ")")

                            .CurrentRow.Cells(3).Value = Format(Val(Mtr_Qty), "#########0.00")

                        End If

                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        .CurrentCell.Selected = True

                    End If

                End With

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_RackNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_RackNo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New RackNo_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_RackNo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_RackNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_RackNo.TextChanged
        Try
            If cbo_Grid_RackNo.Visible Then
                With dgv_Details
                    If .Rows.Count > 0 Then
                        If Val(cbo_Grid_RackNo.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_RackNo.Text)
                        End If
                    End If
                End With
            End If

        Catch ex As Exception
            '---MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim rect As Rectangle

        With dgv_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 1 Then

                If cbo_Grid_RackNo.Visible = False Or Val(cbo_Grid_RackNo.Tag) <> e.RowIndex Then

                    cbo_Grid_RackNo.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Rack_No from Rack_Head order by Rack_No", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_RackNo.DataSource = Dt1
                    cbo_Grid_RackNo.DisplayMember = "Rack_No"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_RackNo.Left = .Left + rect.Left
                    cbo_Grid_RackNo.Top = .Top + rect.Top

                    cbo_Grid_RackNo.Width = rect.Width
                    cbo_Grid_RackNo.Height = rect.Height
                    cbo_Grid_RackNo.Text = .CurrentCell.Value

                    cbo_Grid_RackNo.Tag = Val(e.RowIndex)
                    cbo_Grid_RackNo.Visible = True

                    cbo_Grid_RackNo.BringToFront()
                    cbo_Grid_RackNo.Focus()

                Else

                    'If cbo_Grid_RackNo.Visible = True Then
                    '    cbo_Grid_RackNo.BringToFront()
                    '    cbo_Grid_RackNo.Focus()
                    'End If

                End If

            Else
                cbo_Grid_RackNo.Visible = False

            End If

        End With

    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details

            If .CurrentCell.ColumnIndex = 2 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If

            If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If

        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged

        Try
            With dgv_Details
                If .Visible Then
                    If .Rows.Count > 0 Then

                        If e.ColumnIndex = 2 Or e.ColumnIndex = 3 Or e.ColumnIndex = 4 Then

                            If Val(e.ColumnIndex) = 2 Or Val(e.ColumnIndex) = 3 Then
                                .Rows(e.RowIndex).Cells(4).Value = Format(Val(dgv_Details.Rows(e.RowIndex).Cells(2).Value) * Val(dgv_Details.Rows(e.RowIndex).Cells(3).Value), "#########0.00")
                            End If

                            Total_Calculation()

                        End If
                    End If

                End If
            End With

        Catch ex As Exception
            '---

        End Try


    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_details.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub dgtxt_details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_details.KeyUp
        dgv_Details_KeyUp(sender, e)
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

            End With

            Total_Calculation()

        End If

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer

        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub txt_MeterQty_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_MeterQty.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")

    End Sub

    Private Sub txt_MeterQty_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_MeterQty.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_MeterQty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_MeterQty.TextChanged
        txt_OnFloorMeter.Text = Format(Val(txt_OnFloorQuantity.Text) * Val(txt_MeterQty.Text), "##########0.00")
    End Sub

    Private Sub txt_OnFloorMeter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_OnFloorMeter.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        End If
    End Sub

    Private Sub txt_OnFloorMeter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_OnFloorMeter.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        End If
    End Sub

    Private Sub txt_OnFloorQuantity_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_OnFloorQuantity.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_OnFloorQuantity_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_OnFloorQuantity.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_OnFloorQuantity_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_OnFloorQuantity.TextChanged
        txt_OnFloorMeter.Text = Format(Val(txt_OnFloorQuantity.Text) * Val(txt_MeterQty.Text), "##########0.00")
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub Total_Calculation()
        Dim vTotQty As Single, vTotMtrs As Single
        Dim i As Integer
        Dim Sno As Integer

        vTotQty = 0 : vTotMtrs = 0
        Sno = 0

        With dgv_Details
            For i = 0 To dgv_Details.Rows.Count - 1

                Sno = Sno + 1

                .Rows(i).Cells(0).Value = Sno

                If Val(dgv_Details.Rows(i).Cells(2).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(4).Value) <> 0 Then

                    vTotQty = vTotQty + Val(dgv_Details.Rows(i).Cells(2).Value)
                    vTotMtrs = vTotMtrs + Val(dgv_Details.Rows(i).Cells(4).Value)

                End If
            Next
        End With
        If dgv_Details_Total.Rows.Count <= 0 Then dgv_Details_Total.Rows.Add()
        dgv_Details_Total.Rows(0).Cells(2).Value = Val(vTotQty)
        dgv_Details_Total.Rows(0).Cells(4).Value = Format(Val(vTotMtrs), "#########0.00")

    End Sub


    Private Sub dgv_Details_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellContentClick

    End Sub

    Private Sub cbo_FinishedProductName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbo_FinishedProductName.SelectedIndexChanged

    End Sub
End Class