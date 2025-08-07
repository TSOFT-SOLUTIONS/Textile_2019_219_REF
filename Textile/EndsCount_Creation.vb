Public Class EndsCount_Creation
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False
    Private FrmLdSTS As Boolean = False
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private vcbo_KeyDwnVal As Double
    Private Prec_ActCtrl As New Control
    Private TrnTo_DbName As String = ""
    Private SizTo_DbName As String = ""
    Private Close_STS As Single = 0
    Private WithEvents dgtxt_EndsCount_Master_Rate_Details As New DataGridViewTextBoxEditingControl
    Private dgv_ActiveCtrl_Name As String

    Private vFRM_DEF_HEIGHT1 As Integer = 400
    Private vFRM_DEF_HEIGHT2 As Integer = 400
    Private vFRM_DEF_WIDTH As Integer = 600

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()

        New_Entry = False

        pnl_Back.Enabled = True
        grp_Find.Visible = False
        grp_Filter.Visible = False

        Me.Height = vFRM_DEF_HEIGHT1 ' 340 '441  '284

        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black
        cbo_Cotton_Polyester_Jari.Text = "COTTON"
        chk_Close_STS.Checked = False
        cbo_Single_Double_Triple.Text = ""

        txt_Ends.Text = ""
        txt_Meters.Text = ""
        cbo_Count.Text = ""
        txt_Rate.Text = ""
        cbo_StockIn.Text = "METER"
        txt_MeterPcs.Text = ""
        cbo_EndsCountGroup.Text = ""
        txt_EndsCount.Text = ""
        cbo_Find.Text = ""
        cbo_Transfer.Text = ""
        cbo_Sizing_EndsCount.Text = ""

        pnl_RateDetails.Visible = False
        dgv_EndsCountRate_Details.Rows.Clear()

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
            Me.ActiveControl.BackColor = Color.Lime ' Color.MistyRose ' Color.lime
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

        If FrmLdSTS = True Then Exit Sub

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            End If
        End If

    End Sub

    Private Sub ControlLostFocus1(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.LightBlue
                Prec_ActCtrl.ForeColor = Color.Blue
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
        If FrmLdSTS = True Then Exit Sub
        If Not IsNothing(dgv_EndsCountRate_Details.CurrentCell) Then dgv_EndsCountRate_Details.CurrentCell.Selected = False

    End Sub

    Public Sub move_record(ByVal idno As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim slno As Integer = 0
        Dim n As Integer = 0

        If Val(idno) = 0 Then Exit Sub

        clear()

        da = New SqlClient.SqlDataAdapter("select a.*, b.count_name ,c.EndsCount_Name as stock_undername from EndsCount_Head a LEFT OUTER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo LEFT OUTER JOIN EndsCount_Head c ON a.EndsCount_Stockunder_IdNo = c.EndsCount_IdNo  Where a.EndsCount_IdNo = " & Str(Val(idno)), con)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then

            lbl_IdNo.Text = dt.Rows(0)("EndsCount_IdNo").ToString
            txt_EndsCount.Text = dt.Rows(0)("EndsCount_Name").ToString
            txt_Ends.Text = dt.Rows(0)("Ends_Name").ToString
            txt_Meters.Text = dt.Rows(0)("Meter_Name").ToString
            cbo_Count.Text = dt.Rows(0)("Count_Name").ToString
            txt_Rate.Text = Val(dt.Rows(0)("Rate").ToString)
            txt_MeterPcs.Text = Val(dt.Rows(0).Item("Meters_Pcs").ToString)
            cbo_StockIn.Text = dt.Rows(0).Item("Stock_In").ToString
            cbo_Cotton_Polyester_Jari.Text = dt.Rows(0).Item("Cotton_Polyester_Jari").ToString
            cbo_Single_Double_Triple.Text = dt.Rows(0).Item("WidthType_Single_Double_Triple").ToString
            If Val(dt.Rows(0).Item("Close_Status").ToString) = 1 Then chk_Close_STS.Checked = True


            If Val(dt.Rows(0).Item("EndsCount_Stockunder_IdNo").ToString) <> Val(dt.Rows(0).Item("EndsCount_IdNo").ToString) Then
                cbo_EndsCountGroup.Text = dt.Rows(0).Item("stock_undername").ToString
            End If
            cbo_Transfer.Text = Common_Procedures.EndsCount_IdNoToName(con, Val(dt.Rows(0).Item("Transfer_To_EndsCountIdNo").ToString), TrnTo_DbName)
            cbo_Sizing_EndsCount.Text = Common_Procedures.EndsCount_IdNoToName(con, Val(dt.Rows(0).Item("Sizing_To_EndscountIdNo").ToString), SizTo_DbName)
        End If

        dt.Clear()

        dt.Dispose()
        da.Dispose()

        da = New SqlClient.SqlDataAdapter("select a.* from EndsCount_Master_Rate_Details a where a.EndsCount_Idno = " & Str(Val(idno)) & " Order by a.FromDate_DateTime, a.ToDate_DateTime, a.Sl_No", con)
        da.Fill(dt2)

        dgv_EndsCountRate_Details.Rows.Clear()
        slno = 0

        If dt2.Rows.Count > 0 Then

            For i = 0 To dt2.Rows.Count - 1

                n = dgv_EndsCountRate_Details.Rows.Add()

                slno = slno + 1

                dgv_EndsCountRate_Details.Rows(n).Cells(0).Value = Val(slno)
                dgv_EndsCountRate_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("FromDate_Text").ToString
                dgv_EndsCountRate_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("ToDate_Text").ToString
                dgv_EndsCountRate_Details.Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "#########0.00")

            Next i

        End If
        dt2.Clear()
        dt2.Dispose()

        Grid_DeSelect()


        If txt_Ends.Enabled And txt_Ends.Visible Then txt_Ends.Focus()

    End Sub

    Private Sub EndsCount_Creation_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim TrnTo_CmpGrpIdNo As Integer = 0
        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Count.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Count.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            FrmLdSTS = False
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub EndsCount_Creation_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim TrnTo_CmpGrpIdNo As Integer = 0

        Me.Height = vFRM_DEF_HEIGHT1 ' 340 ' 441
        Me.Width = vFRM_DEF_WIDTH ' 573
        'Me.Top = Me.Top - 150

        'grp_Find.Left = 4
        'grp_Find.Top = 277

        'grp_Filter.Left = grp_Find.Left
        'grp_Filter.Top = grp_Find.Top

        FrmLdSTS = True

        cbo_Sizing_EndsCount.Visible = False
        lbl_Sizing.Visible = False

        If Common_Procedures.settings.Combine_Textile_SizingSOftware = 1 Then
            SizTo_DbName = Common_Procedures.get_Company_SizingDataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))
            cbo_Sizing_EndsCount.Visible = True
            lbl_Sizing.Visible = True
        Else
            SizTo_DbName = Common_Procedures.get_Company_DataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))
        End If

        cbo_StockIn.Items.Add("")
        cbo_StockIn.Items.Add("METER")
        cbo_StockIn.Items.Add("PCS")
        cbo_StockIn.Items.Add("WEIGHT")

        cbo_Cotton_Polyester_Jari.Items.Clear()
        cbo_Cotton_Polyester_Jari.Items.Add("COTTON")
        cbo_Cotton_Polyester_Jari.Items.Add("POLYESTER")
        cbo_Cotton_Polyester_Jari.Items.Add("JARI")

        cbo_Transfer.Visible = False
        lbl_TransferStockTo.Visible = False
        TrnTo_CmpGrpIdNo = Val(Common_Procedures.get_FieldValue(con, Trim(Common_Procedures.CompanyDetailsDataBaseName) & "..CompanyGroup_Head", "Transfer_To_CompanyGroupIdNo", "(CompanyGroup_IdNo = " & Str(Val(Common_Procedures.CompGroupIdNo)) & ")"))
        If Val(TrnTo_CmpGrpIdNo) <> 0 Then
            TrnTo_DbName = Common_Procedures.get_Company_DataBaseName(Trim(Val(TrnTo_CmpGrpIdNo)))
            cbo_Transfer.Visible = True
            lbl_TransferStockTo.Visible = True
        Else
            TrnTo_DbName = Common_Procedures.get_Company_DataBaseName(Trim(Val(Common_Procedures.CompGroupIdNo)))
        End If

        con.Open()

        da = New SqlClient.SqlDataAdapter("select count_name from Count_Head order by count_name", con)
        da.Fill(dt1)
        cbo_Count.DataSource = dt1
        cbo_Count.DisplayMember = "count_name"

        grp_Find.Visible = False
        grp_Find.Left = (Me.Width - grp_Find.Width) \ 2
        grp_Find.Top = (Me.Height - grp_Find.Height) \ 2
        grp_Find.BringToFront()

        grp_Filter.Visible = False
        grp_Filter.Left = (Me.Width - grp_Filter.Width) \ 2
        grp_Filter.Top = (Me.Height - grp_Filter.Height) \ 2
        grp_Filter.BringToFront()

        pnl_RateDetails.Visible = False
        pnl_RateDetails.Left = (Me.Width - pnl_RateDetails.Width) \ 2
        pnl_RateDetails.Top = (Me.Height - pnl_RateDetails.Height) \ 2
        pnl_RateDetails.BringToFront()

        AddHandler txt_Ends.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_StockIn.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Cotton_Polyester_Jari.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Single_Double_Triple.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EndsCountGroup.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transfer.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Find.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_MeterPcs.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Meters.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_Ends.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Count.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_StockIn.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Find.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_MeterPcs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Meters.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Cotton_Polyester_Jari.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Single_Double_Triple.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EndsCountGroup.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transfer.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Ends.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_EndsCount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Meters.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Ends.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_EndsCount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Rate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Meters.KeyPress, AddressOf TextBoxControlKeyPress

        cbo_Single_Double_Triple.Visible = False
        cbo_Single_Double_Triple.Items.Clear()
        cbo_Single_Double_Triple.Items.Add("")
        'cbo_Single_Double_Triple.Items.Add("SINGLE")
        'cbo_Single_Double_Triple.Items.Add("DOUBLE")
        'cbo_Single_Double_Triple.Items.Add("TRIPLE")
        'cbo_Single_Double_Triple.Items.Add("FOURTH")
        cbo_Single_Double_Triple.Items.Add("SINGLE FABRIC FROM 1 BEAM")
        cbo_Single_Double_Triple.Items.Add("SINGLE FABRIC FROM 2 BEAMS")
        cbo_Single_Double_Triple.Items.Add("DOUBLE FABRIC FROM 1 BEAM")
        cbo_Single_Double_Triple.Items.Add("DOUBLE FABRIC FROM 2 BEAMS")
        cbo_Single_Double_Triple.Items.Add("TRIPLE FABRIC FROM 1 BEAM")
        cbo_Single_Double_Triple.Items.Add("TRIPLE FABRIC FROM 2 BEAMS")
        cbo_Single_Double_Triple.Items.Add("FOUR FABRIC FROM 1 BEAM")
        cbo_Single_Double_Triple.Items.Add("FOUR FABRIC FROM 2 BEAMS")

        txt_Rate.Visible = True
        btn_RateDetails.Visible = False
        If Common_Procedures.settings.CustomerCode = "1155" Or Common_Procedures.settings.CustomerCode = "1267" Then
            lbl_Rate_Caption.Text = "Width Type"
            txt_Rate.Visible = False
            btn_RateDetails.Visible = True

            cbo_Single_Double_Triple.Visible = True
            cbo_Single_Double_Triple.Left = txt_Rate.Left
            cbo_Single_Double_Triple.Width = txt_Rate.Width
            cbo_Single_Double_Triple.BackColor = Color.White

        End If

        new_record()

    End Sub

    Private Sub EndsCount_Creation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        If Asc(e.KeyChar) = 27 Then
            If grp_Find.Visible Then
                btnClose_Click(sender, e)
            ElseIf grp_Filter.Visible Then
                btn_CloseFilter_Click(sender, e)
            ElseIf pnl_RateDetails.Visible = True Then
                btn_Close_rate_Click(sender, e)
                Exit Sub
            Else

                If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub

                Else
                    Me.Close()
                End If
            End If
        End If
    End Sub

    Private Sub EndsCount_Creation_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean

        Dim dgv1 As New DataGridView



        If ActiveControl.Name = dgv_EndsCountRate_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing


            If ActiveControl.Name = dgv_EndsCountRate_Details.Name Then
                dgv1 = dgv_EndsCountRate_Details

            ElseIf dgv_EndsCountRate_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_EndsCountRate_Details

            ElseIf dgv_ActiveCtrl_Name = dgv_EndsCountRate_Details.Name Then
                dgv1 = dgv_EndsCountRate_Details

            End If

            If IsNothing(dgv1) = True Then
                Return MyBase.ProcessCmdKey(msg, keyData)
                Exit Function
            End If


            With dgv1

                If dgv1.Name = dgv_EndsCountRate_Details.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                Close_EndsCount_Master_Rate_Details()

                            Else
                                .Focus()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)


                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And ((.CurrentCell.ColumnIndex <> 1 And Val(.CurrentRow.Cells(1).Value) = 0) Or (.CurrentCell.ColumnIndex = 1 And Val(dgtxt_EndsCount_Master_Rate_Details.Text) = 0)) Then
                                For i = 0 To .Columns.Count - 1
                                    .Rows(.CurrentCell.RowIndex).Cells(i).Value = ""
                                Next

                                Close_EndsCount_Master_Rate_Details()

                            ElseIf .CurrentCell.ColumnIndex = 1 Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 2)

                            Else
                                .Focus()
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If


                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then

                                Close_EndsCount_Master_Rate_Details()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 3 Then
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 2)

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If


                End If

            End With

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Endscount_Creation, "~L~") = 0 And InStr(Common_Procedures.UR.Endscount_Creation, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Endscount_Creation, New_Entry, Me) = False Then Exit Sub


        If MessageBox.Show("Do you want to Delete ?", "FOR DELETION....", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        Try

            da = New SqlClient.SqlDataAdapter("select count(*) from Cloth_EndsCount_Details where EndsCount_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ends/Count", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If
            dt.Clear()

            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_SizedPavu_Processing_Details where EndsCount_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ends/Count", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If
            dt.Clear()


            da = New SqlClient.SqlDataAdapter("select count(*) from Stock_Pavu_Processing_Details where EndsCount_IdNo = " & Str(Val(lbl_IdNo.Text)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    If Val(dt.Rows(0)(0).ToString) > 0 Then
                        MessageBox.Show("Already used this Ends/Count", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If
            End If
            dt.Clear()

            cmd.Connection = con

            cmd.CommandText = "delete from EndsCount_Master_Rate_Details where EndsCount_Idno = " & Str(Val(lbl_IdNo.Text) & "")
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from EndsCount_Head where EndsCount_IdNo = " & Str(Val(lbl_IdNo.Text))
            cmd.ExecuteNonQuery()

            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If txt_EndsCount.Enabled And txt_EndsCount.Visible Then txt_EndsCount.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim da As New SqlClient.SqlDataAdapter("select EndsCount_IdNo, EndsCount_Name from EndsCount_Head where EndsCount_IdNo <> 0 order by EndsCount_IdNo", con)
        Dim dt As New DataTable

        da.Fill(dt)

        With dgv_Filter

            .Columns.Clear()
            .DataSource = dt

            .RowHeadersVisible = False

            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            .Columns(0).HeaderText = "IDNO"
            .Columns(1).HeaderText = "ENDS COUNT"

            .Columns(0).FillWeight = 40
            .Columns(1).FillWeight = 160

        End With

        new_record()

        grp_Filter.Visible = True

        pnl_Back.Enabled = False

        If dgv_Filter.Enabled And dgv_Filter.Visible Then dgv_Filter.Focus()

        Me.Height = vFRM_DEF_HEIGHT2  ' 535 ' 520 ' 400

        dt.Dispose()
        da.Dispose()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer = 0

        Try
            da = New SqlClient.SqlDataAdapter("select min(EndsCount_IdNo) from EndsCount_Head Where EndsCount_IdNo <> 0", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If
            dt.Clear()

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
            da = New SqlClient.SqlDataAdapter("select max(EndsCount_IdNo) from EndsCount_Head Where EndsCount_IdNo <> 0", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If
            dt.Clear()

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
        Dim movid As Integer

        Try
            da = New SqlClient.SqlDataAdapter("select min(EndsCount_IdNo) from EndsCount_Head where EndsCount_IdNo > " & Str(Val(lbl_IdNo.Text)) & " and EndsCount_IdNo <> 0 ", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Clear()

            dt.Dispose()
            da.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        Try
            da = New SqlClient.SqlDataAdapter("select max(EndsCount_IdNo) from EndsCount_Head where EndsCount_IdNo < " & Str(Val(lbl_IdNo.Text)) & " and EndsCount_IdNo <> 0 ", con)
            da.Fill(dt)

            movid = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movid = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Clear()

            dt.Dispose()
            da.Dispose()

            If movid <> 0 Then move_record(movid)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT MOVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record

        clear()

        New_Entry = True
        lbl_IdNo.ForeColor = Color.Red

        lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "EndsCount_Head", "EndsCount_IdNo", "")

        If txt_Ends.Enabled And txt_Ends.Visible Then txt_Ends.Focus()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim da As New SqlClient.SqlDataAdapter("select EndsCount_Name from EndsCount_Head order by EndsCount_Name", con)
        Dim dt As New DataTable

        da.Fill(dt)

        cbo_Find.DataSource = dt
        cbo_Find.DisplayMember = "EndsCount_Name"

        new_record()

        grp_Find.Visible = True
        pnl_Back.Enabled = False

        If cbo_Find.Enabled And cbo_Find.Visible Then cbo_Find.Focus()
        Me.Height = vFRM_DEF_HEIGHT2 ' 495 ' 480 ' 355

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        'MessageBox.Show("insert record")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '--- No Printing
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim Sur As String
        Dim Cnt_ID As Integer
        Dim Sno As Integer = 0
        Dim StkGrp_id As Integer
        Dim transtk_id As Integer = 0
        Dim ItmGrpId As Integer = 0
        Dim Sizstk_id As Integer = 0
        Dim vSTS As Boolean = False
        Dim vToDate1STS As Boolean = False
        Dim vToDate2STS As Boolean = False
        Dim vFrmDate1 As Date
        Dim vToDate1 As Date
        Dim vFrmDate2 As Date
        Dim vToDate2 As Date
        Dim vBlank_ToDate_Count As Integer = 0


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Endscount_Creation, New_Entry, Me) = False Then Exit Sub


        If Val(txt_Ends.Text) = 0 Then
            MessageBox.Show("Invalid Ends Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Ends.Enabled Then txt_Ends.Focus()
            Exit Sub
        End If

        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, cbo_Count.Text)

        If Val(Cnt_ID) = 0 Then
            MessageBox.Show("Invalid Count Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Count.Enabled Then cbo_Count.Focus()
            Exit Sub
        End If
        Close_STS = 0
        If chk_Close_STS.Checked = True Then Close_STS = 1

        EndsCount_Calculation()
        If Val(txt_EndsCount.Text) = 0 Then
            MessageBox.Show("Invalid Ends Count", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_EndsCount.Enabled Then txt_EndsCount.Focus()
            Exit Sub
        End If

        Sur = Common_Procedures.Remove_NonCharacters(Trim(txt_EndsCount.Text))

        transtk_id = Common_Procedures.EndsCount_NameToIdNo(con, cbo_Transfer.Text, , TrnTo_DbName)
        If cbo_Transfer.Visible Then
            If Trim(cbo_Transfer.Text) <> "" Then
                If Val(transtk_id) = 0 Then
                    MessageBox.Show("Invalid Transfer Stock To", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If cbo_Transfer.Enabled Then cbo_Transfer.Focus()
                    Exit Sub
                End If
            End If
        End If

        StkGrp_id = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCountGroup.Text)
        If Val(StkGrp_id) = 0 Then
            StkGrp_id = Val(lbl_IdNo.Text)
        End If
        Sizstk_id = Common_Procedures.EndsCount_NameToIdNo(con, cbo_Sizing_EndsCount.Text, , SizTo_DbName)
        If cbo_Sizing_EndsCount.Visible Then
            If Trim(cbo_Sizing_EndsCount.Text) <> "" Then
                If Val(Sizstk_id) = 0 Then
                    MessageBox.Show("Invalid Sizing EndsCount Name", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If cbo_Sizing_EndsCount.Enabled Then cbo_Sizing_EndsCount.Focus()
                    Exit Sub
                End If
            End If
        End If
        ItmGrpId = Val(Common_Procedures.get_FieldValue(con, "Count_Head", "ItemGroup_IdNo", "Count_IdNo=" & Val(Cnt_ID)))

        If cbo_Single_Double_Triple.Visible = True Then
            If Common_Procedures.settings.CustomerCode = "1267" Then
                If Trim(cbo_Single_Double_Triple.Text) = "" Then
                    MessageBox.Show("Invalid Width Type (Single/Double/Triple)", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_Single_Double_Triple.Enabled Then cbo_Single_Double_Triple.Focus()
                    Exit Sub
                End If
            End If
        End If

        With dgv_EndsCountRate_Details
            For i = 0 To .RowCount - 1
                If Val(.Rows(i).Cells(3).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(1).Value) = "" Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        .CurrentCell = .Rows(i).Cells(1)
                        .Focus()
                        Exit Sub
                    End If

                    If IsDate(.Rows(i).Cells(1).Value) = False Then
                        MessageBox.Show("Invalid From Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        .CurrentCell = .Rows(i).Cells(1)
                        .Focus()
                        Exit Sub
                    End If

                End If
            Next

        End With

        With dgv_EndsCountRate_Details

            For i = 0 To .RowCount - 1

                vFrmDate1 = #12:00:00 PM#
                vToDate1 = #12:00:00 PM#

                vToDate1STS = False

                vSTS = False
                If Trim(.Rows(i).Cells(1).Value) <> "" Then
                    If IsDate(.Rows(i).Cells(1).Value) = True Then
                        vSTS = True
                        vFrmDate1 = CDate(.Rows(i).Cells(1).Value)
                    End If
                End If

                If vSTS = True And Val(.Rows(i).Cells(3).Value) <> 0 Then

                    vToDate1STS = False

                    If Trim(.Rows(i).Cells(2).Value) <> "" Then
                        If IsDate(.Rows(i).Cells(2).Value) = True Then
                            vToDate1STS = True
                            vToDate1 = CDate(.Rows(i).Cells(2).Value)
                        End If
                    End If

                    If vToDate1STS = False Then
                        vBlank_ToDate_Count = vBlank_ToDate_Count + 1
                        'MessageBox.Show("Invalid To Date in Rate Details", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        'pnl_Back.Enabled = False
                        'pnl_RateDetails.Visible = True
                        'If dgv_EndsCountRate_Details.Enabled And dgv_EndsCountRate_Details.Visible Then
                        '    dgv_EndsCountRate_Details.Focus()
                        '    dgv_EndsCountRate_Details.CurrentCell = dgv_EndsCountRate_Details.Rows(i).Cells(1)
                        'End If
                        'Exit Sub

                    Else

                        If DateDiff(DateInterval.Day, vToDate1, vFrmDate1) > 0 Then


                            MessageBox.Show("Invalid Date in Rate Details" & Chr(13) & "To Date lesser than from date", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                            pnl_Back.Enabled = False
                            pnl_RateDetails.Visible = True
                            If dgv_EndsCountRate_Details.Enabled And dgv_EndsCountRate_Details.Visible Then
                                dgv_EndsCountRate_Details.Focus()
                                dgv_EndsCountRate_Details.CurrentCell = dgv_EndsCountRate_Details.Rows(i).Cells(1)
                            End If

                            Exit Sub

                        End If

                    End If

                    For j = i + 1 To .RowCount - 1

                        vFrmDate2 = #12:00:00 PM#
                        vToDate2 = #12:00:00 PM#

                        vSTS = False
                        If Trim(.Rows(j).Cells(1).Value) <> "" Then
                            If IsDate(.Rows(j).Cells(1).Value) = True Then
                                vSTS = True
                                vFrmDate2 = CDate(.Rows(j).Cells(1).Value)
                            End If
                        End If


                        If vSTS = True And Val(.Rows(j).Cells(3).Value) <> 0 Then

                            If DateDiff(DateInterval.Day, vFrmDate2, vFrmDate1) > 0 Then

                                MessageBox.Show("Invalid Date in Rate Details - from date should be grater than previous date ", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                pnl_Back.Enabled = False
                                pnl_RateDetails.Visible = True
                                If dgv_EndsCountRate_Details.Enabled And dgv_EndsCountRate_Details.Visible Then
                                    dgv_EndsCountRate_Details.Focus()
                                    dgv_EndsCountRate_Details.CurrentCell = dgv_EndsCountRate_Details.Rows(j).Cells(1)
                                End If
                                Exit Sub

                            End If

                        End If

                    Next j

                End If

            Next i

            If vBlank_ToDate_Count > 1 Then

                MessageBox.Show("Invalid To-Date in Rate Details", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                pnl_Back.Enabled = False
                pnl_RateDetails.Visible = True
                If dgv_EndsCountRate_Details.Enabled And dgv_EndsCountRate_Details.Visible Then
                    dgv_EndsCountRate_Details.Focus()
                    dgv_EndsCountRate_Details.CurrentCell = dgv_EndsCountRate_Details.Rows(0).Cells(1)
                End If
                Exit Sub
            End If

        End With


        trans = con.BeginTransaction

        Try
            cmd.Connection = con
            cmd.Transaction = trans

            If cbo_StockIn.Text = "" Then
                cbo_StockIn.Text = "METER"
            End If

            If New_Entry = True Then

                lbl_IdNo.Text = Common_Procedures.get_MaxIdNo(con, "EndsCount_Head", "EndsCount_IdNo", "", trans)

                cmd.CommandText = "Insert into EndsCount_Head(EndsCount_IdNo, EndsCount_Name, sur_name, Ends_Name, Count_IdNo, Rate ,  Stock_In , Meters_Pcs , Cotton_Polyester_Jari,EndsCount_StockUnder_IdNo,Transfer_To_EndsCountIdNo,Meter_Name , ItemGroup_IdNo,Sizing_To_EndsCountIdNo , Close_Status , WidthType_Single_Double_Triple) values (" & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(txt_EndsCount.Text) & "', '" & Trim(Sur) & "', " & Str(Val(txt_Ends.Text)) & ", " & Str(Val(Cnt_ID)) & ", " & Str(Val(txt_Rate.Text)) & " , '" & Trim(cbo_StockIn.Text) & "',   " & Str(Val(txt_MeterPcs.Text)) & " , '" & Trim(cbo_Cotton_Polyester_Jari.Text) & "'," & Val(StkGrp_id) & " ," & Val(transtk_id) & ", '" & Trim(txt_Meters.Text) & "'," & Str(Val(ItmGrpId)) & "," & Str(Val(Sizstk_id)) & "," & Str(Val(Close_STS)) & ", '" & Trim(cbo_Single_Double_Triple.Text) & "')"
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "update EndsCount_Head set EndsCount_Name = '" & Trim(txt_EndsCount.Text) & "', sur_name = '" & Trim(Sur) & "',Meter_Name = '" & Trim(txt_Meters.Text) & "',  Ends_Name = " & Str(Val(txt_Ends.Text)) & ", Count_IdNo = " & Str(Val(Cnt_ID)) & ", Rate = " & Str(Val(txt_Rate.Text)) & " ,  Stock_In ='" & Trim(cbo_StockIn.Text) & "' , Meters_Pcs =  " & Str(Val(txt_MeterPcs.Text)) & " , Cotton_Polyester_Jari = '" & Trim(cbo_Cotton_Polyester_Jari.Text) & "', EndsCount_StockUnder_IdNo = " & Val(StkGrp_id) & ",Transfer_To_EndsCountIdNo = " & Val(transtk_id) & ",ItemGroup_IdNo = " & Str(Val(ItmGrpId)) & " ,Sizing_To_EndsCountIdNo = " & Str(Val(Sizstk_id)) & ",Close_Status=" & Str(Val(Close_STS)) & " , WidthType_Single_Double_Triple = '" & Trim(cbo_Single_Double_Triple.Text) & "' Where EndsCount_IdNo = " & Str(Val(lbl_IdNo.Text))
                cmd.ExecuteNonQuery()

            End If


            cmd.CommandText = "delete from EndsCount_Master_Rate_Details where EndsCount_Idno = " & Str(Val(lbl_IdNo.Text) & "")
            cmd.ExecuteNonQuery()

            With dgv_EndsCountRate_Details
                Sno = 0
                For i = 0 To .RowCount - 1

                    vSTS = False

                    cmd.Parameters.Clear()

                    If Trim(.Rows(i).Cells(1).Value) <> "" Then
                        If IsDate(.Rows(i).Cells(1).Value) = True Then
                            cmd.Parameters.AddWithValue("@FromDate", CDate(.Rows(i).Cells(1).Value))
                            vSTS = True
                        End If
                    End If

                    If vSTS = True And Val(.Rows(i).Cells(3).Value) <> 0 Then

                        Sno = Sno + 1

                        If Trim(.Rows(i).Cells(2).Value) <> "" Then
                            If IsDate(.Rows(i).Cells(2).Value) = True Then
                                cmd.Parameters.AddWithValue("@ToDate", CDate(.Rows(i).Cells(2).Value))
                            End If
                        End If

                        cmd.CommandText = "Insert into EndsCount_Master_Rate_Details (            EndsCount_Idno     ,           Sl_No      , FromDate_DateTime,                    FromDate_Text        ,                                             ToDate_DateTime            ,                    ToDate_Text          ,                      Rate                 ) " & _
                                            " Values                                 (" & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(Sno)) & ",     @FromDate    , '" & Trim(.Rows(i).Cells(1).Value) & "' , " & IIf(IsDate(.Rows(i).Cells(2).Value) = True, "@ToDate", "Null") & " , '" & Trim(.Rows(i).Cells(2).Value) & "' , " & Str(Val(.Rows(i).Cells(3).Value)) & " ) "
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Update EndsCount_Head set Rate = " & Val(.Rows(i).Cells(3).Value) & " Where EndsCount_IdNo = " & Str(Val(lbl_IdNo.Text))
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With


            trans.Commit()

            Common_Procedures.Master_Return.Return_Value = Trim(txt_EndsCount.Text)
            Common_Procedures.Master_Return.Master_Type = "ENDSCOUNT"


            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING....", MessageBoxButtons.OK, MessageBoxIcon.Information)

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
            trans.Rollback()

            If InStr(1, Trim(LCase(ex.Message)), "ix_endscount_head") > 0 Then
                MessageBox.Show("Duplicate Ends Count", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Finally
            If txt_Ends.Enabled And txt_Ends.Visible Then txt_Ends.Focus()

        End Try

    End Sub

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btn_Find_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Find.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movid As Integer

        da = New SqlClient.SqlDataAdapter("select EndsCount_IdNo from EndsCount_Head where EndsCount_Name = '" & Trim(cbo_Find.Text) & "'", con)
        dt = New DataTable
        da.Fill(dt)

        movid = 0
        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                movid = Val(dt.Rows(0)(0).ToString)
            End If
        End If

        dt.Dispose()
        da.Dispose()

        If movid <> 0 Then
            move_record(movid)
        Else
            new_record()
        End If

        btnClose_Click(sender, e)

    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Height = vFRM_DEF_HEIGHT1  ' 340 ' 441  '284 ' 197
        pnl_Back.Enabled = True
        grp_Find.Visible = False
        If txt_EndsCount.Enabled And txt_EndsCount.Visible Then txt_EndsCount.Focus()
    End Sub

    Private Sub btn_CloseFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CloseFilter.Click

        pnl_Back.Enabled = True
        grp_Filter.Visible = False
        If txt_EndsCount.Enabled And txt_EndsCount.Visible Then txt_EndsCount.Focus()

        Me.Height = vFRM_DEF_HEIGHT1 ' 340 ' 441

    End Sub
    Private Sub cbo_Open_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Find.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Find, Nothing, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_Open_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Find.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Find, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            Call btn_Find_Click(sender, e)
        End If

    End Sub

    Private Sub btn_Open_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Open.Click
        Dim movid As Integer

        movid = 0
        If IsDBNull((dgv_Filter.CurrentRow.Cells(0).Value)) = False Then
            movid = Val(dgv_Filter.CurrentRow.Cells(0).Value)
        End If

        If Val(movid) <> 0 Then
            move_record(movid)
            btn_CloseFilter_Click(sender, e)
        End If

    End Sub

    Private Sub dgv_Filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Filter.DoubleClick
        btn_Open_Click(sender, e)
    End Sub

    Private Sub dgv_Filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter.KeyDown
        If e.KeyCode = Keys.Enter Then
            btn_Open_Click(sender, e)
        End If
    End Sub



    Private Sub txt_EndsCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EndsCount.KeyPress
        Dim K As Integer

        If Asc(e.KeyChar) >= 97 And Asc(e.KeyChar) <= 122 Then
            K = Asc(e.KeyChar)
            K = K - 32
            e.KeyChar = Chr(K)
        End If


    End Sub


    Private Sub cbo_EndsCountGroup_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EndsCountGroup.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "(EndsCount_IdNo <> " & Str(Val(lbl_IdNo.Text)) & ")", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_EndsCountGroup_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCountGroup.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EndsCountGroup, Nothing, cbo_StockIn, "EndsCount_Head", "Endscount_Name", "(EndsCount_IdNo <> " & Str(Val(lbl_IdNo.Text)) & ")", "(EndsCount_IdNo = 0)")

        If e.KeyValue = 38 And cbo_EndsCountGroup.DroppedDown = False Then
            If btn_RateDetails.Enabled = True And btn_RateDetails.Visible = True Then
                btn_RateDetails.Focus()
            Else
                txt_Rate.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_EndsCountGroup_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EndsCountGroup.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EndsCountGroup, cbo_StockIn, "EndsCount_Head", "EndsCount_Name", "(EndsCount_IdNo <> " & Str(Val(lbl_IdNo.Text)) & ")", "(EndsCount_IdNo = 0)")

    End Sub

    Private Sub txt_Ends_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Ends.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then e.Handled = True

    End Sub


    Private Sub cbo_StockIn_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_StockIn.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_StockIn, cbo_EndsCountGroup, txt_MeterPcs, "", "", "", "")

    End Sub

    Private Sub cbo_StockIn_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_StockIn.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_StockIn, txt_MeterPcs, "", "", "", "")

    End Sub

    Private Sub cbo_Cotton_Polyester_Jari_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cotton_Polyester_Jari.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Cotton_Polyester_Jari, txt_MeterPcs, Nothing, "", "", "", "")

        If e.KeyValue = 40 And cbo_Cotton_Polyester_Jari.DroppedDown = False Then
            If cbo_Transfer.Visible = True Then
                cbo_Transfer.Focus()
            ElseIf cbo_Sizing_EndsCount.Visible = True Then
                cbo_Sizing_EndsCount.Focus()
            ElseIf MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Ends.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Cotton_Polyester_Jari_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Cotton_Polyester_Jari.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Cotton_Polyester_Jari, Nothing, "", "", "", "")

        If Asc(e.KeyChar) = 13 Then
            If cbo_Transfer.Visible = True Then
                cbo_Transfer.Focus()
            ElseIf cbo_Sizing_EndsCount.Visible = True Then
                cbo_Sizing_EndsCount.Focus()
            ElseIf MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Ends.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Count_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Count.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Count, txt_Ends, txt_EndsCount, "Count_Head", "Count_Name", "", "(Count_Idno = 0)")

    End Sub

    Private Sub cbo_Count_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Count.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Count, txt_EndsCount, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub


    Private Sub cbo_Count_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Count.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Count.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub txt_MeterPcs_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_MeterPcs.KeyDown
        If e.KeyValue = 40 Then
            SendKeys.Send("{TAB}")
        End If
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_MeterPcs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_MeterPcs.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txt_Ends_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Ends.TextChanged
        EndsCount_Calculation()
    End Sub

    Private Sub cbo_Count_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Count.TextChanged
        EndsCount_Calculation()
    End Sub


    Private Sub txt_Rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress
        If Val(Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar))) = 0 Then e.Handled = True
    End Sub

    Public Sub EndsCount_Calculation()
        Dim Cnt_ID As Integer

        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, cbo_Count.Text)

        txt_EndsCount.Text = ""
        If Val(txt_Ends.Text) <> 0 And Val(Cnt_ID) <> 0 And Trim(txt_Meters.Text) <> "" Then
            txt_EndsCount.Text = Val(txt_Ends.Text) & "/" & Trim(cbo_Count.Text) & "-" & Trim(txt_Meters.Text)
        ElseIf Val(txt_Ends.Text) <> 0 And Val(Cnt_ID) <> 0 Then
            txt_EndsCount.Text = Val(txt_Ends.Text) & "/" & Trim(cbo_Count.Text)
        End If

    End Sub
    Private Sub cbo_Transfer_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transfer.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, TrnTo_DbName & "..EndsCount_Head", "EndsCount_Name", "", "(EndsCount_idno = 0)")
    End Sub

    Private Sub cbo_Transfer_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transfer.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transfer, cbo_Cotton_Polyester_Jari, Nothing, TrnTo_DbName & "..EndsCount_Head", "EndsCount_Name", "", "(EndsCount_idno = 0)")
        If (e.KeyValue = 40 And cbo_Transfer.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_Transfer.Visible = True Then
                cbo_Transfer.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    txt_Ends.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub cbo_Transfer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transfer.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transfer, Nothing, TrnTo_DbName & "..EndsCount_Head", "EndsCount_Name", "", "(EndsCount_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If cbo_Transfer.Visible = True Then
                cbo_Transfer.Focus()
            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    txt_Ends.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub txt_Meters_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Meters.TextChanged
        EndsCount_Calculation()
    End Sub
    Private Sub cbo_Sizing_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Sizing_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, SizTo_DbName & "..EndsCount_Head", "EndsCount_Name", "", "(EndsCount_idno = 0)")
    End Sub

    Private Sub cbo_Sizing_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Sizing_EndsCount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Sizing_EndsCount, Nothing, chk_Close_STS, SizTo_DbName & "..EndsCount_Head", "EndsCount_Name", "", "(EndsCount_idno = 0)")


    End Sub

    Private Sub cbo_Sizing_EndsCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Sizing_EndsCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Sizing_EndsCount, chk_Close_STS, SizTo_DbName & "..EndsCount_Head", "EndsCount_Name", "", "(EndsCount_idno = 0)")

    End Sub

    Private Sub chk_Close_STS_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles chk_Close_STS.KeyDown
        If (e.KeyValue = 40 And cbo_Sizing_EndsCount.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Ends.Focus()
            End If
        End If
    End Sub

    Private Sub chk_Close_STS_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles chk_Close_STS.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                txt_Ends.Focus()
            End If
        End If
    End Sub

    Private Sub btn_RateDetails_Click(sender As System.Object, e As System.EventArgs) Handles btn_RateDetails.Click
        pnl_Back.Enabled = False
        pnl_RateDetails.Visible = True
        If dgv_EndsCountRate_Details.Enabled And dgv_EndsCountRate_Details.Visible Then
            dgv_EndsCountRate_Details.Focus()
            dgv_EndsCountRate_Details.CurrentCell = dgv_EndsCountRate_Details.Rows(0).Cells(1)
        End If
    End Sub

    Private Sub btn_Close_rate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_rate.Click
        Close_EndsCount_Master_Rate_Details()
    End Sub

    Private Sub Close_EndsCount_Master_Rate_Details()
        pnl_Back.Enabled = True
        pnl_RateDetails.Visible = False
    End Sub

    Private Sub dgv_EndsCountRate_Details_CellEnter(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_EndsCountRate_Details.CellEnter
        Dim CmpGrp_Fromdate As Date


        If FrmLdSTS = True Then Exit Sub
        With dgv_EndsCountRate_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If
            dgv_ActiveCtrl_Name = dgv_EndsCountRate_Details.Name

            CmpGrp_Fromdate = New DateTime(Val(Microsoft.VisualBasic.Left(Common_Procedures.FnRange, 4)), 4, 1)
            .Rows(0).Cells(1).Value = Format(DateAdd(DateInterval.Year, -1, CmpGrp_Fromdate), "dd-MM-yyyy")

        End With
    End Sub

    Private Sub dgv_EndsCountRate_Details_CellLeave(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_EndsCountRate_Details.CellLeave
        If FrmLdSTS = True Then Exit Sub
        With dgv_EndsCountRate_Details

            If e.ColumnIndex = 1 Or e.ColumnIndex = 2 Then

                If Trim(.Rows(e.RowIndex).Cells(1).Value) <> "" Then
                    If IsDate(.Rows(e.RowIndex).Cells(1).Value) = False Then
                        .Rows(e.RowIndex).Cells(1).Value = ""
                    End If
                End If

                If Trim(.Rows(e.RowIndex).Cells(2).Value) <> "" Then
                    If IsDate(.Rows(e.RowIndex).Cells(2).Value) = False Then
                        .Rows(e.RowIndex).Cells(2).Value = ""
                    End If
                End If

            End If
        End With
    End Sub

    Private Sub dgv_EndsCountRate_Details_CellValueChanged(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_EndsCountRate_Details.CellValueChanged
        Dim vDat1 As Date
        If FrmLdSTS = True Then Exit Sub
        If IsNothing(dgv_EndsCountRate_Details.CurrentCell) Then Exit Sub
        With dgv_EndsCountRate_Details

            If e.ColumnIndex = 1 And e.RowIndex > 0 Then

                If Trim(.Rows(e.RowIndex).Cells(1).Value) <> "" Then
                    If IsDate(.Rows(e.RowIndex).Cells(1).Value) = True Then
                        vDat1 = CDate(.Rows(e.RowIndex).Cells(1).Value)
                        .Rows(e.RowIndex - 1).Cells(2).Value = Format(DateAdd(DateInterval.Day, -1, vDat1), "dd-MM-yyyy")
                    End If
                End If

            End If

        End With
    End Sub

    Private Sub dgv_EndsCountRate_Details_EditingControlShowing(sender As Object, e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_EndsCountRate_Details.EditingControlShowing
        dgtxt_EndsCount_Master_Rate_Details = CType(dgv_EndsCountRate_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_EndsCountRate_Details_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dgv_EndsCountRate_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_EndsCountRate_Details_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dgv_EndsCountRate_Details.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_EndsCountRate_Details

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

    Private Sub dgv_EndsCountRate_Details_LostFocus(sender As Object, e As System.EventArgs) Handles dgv_EndsCountRate_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_EndsCountRate_Details.CurrentCell) Then dgv_EndsCountRate_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_EndsCountRate_Details_RowsAdded(sender As Object, e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_EndsCountRate_Details.RowsAdded
        Dim n As Integer
        If FrmLdSTS = True Then Exit Sub
        If IsNothing(dgv_EndsCountRate_Details.CurrentCell) Then Exit Sub
        With dgv_EndsCountRate_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub dgtxt_EndsCount_Master_Rate_Details_Enter(sender As Object, e As System.EventArgs) Handles dgtxt_EndsCount_Master_Rate_Details.Enter
        If FrmLdSTS = True Then Exit Sub
        dgv_ActiveCtrl_Name = dgv_EndsCountRate_Details.Name
        dgv_EndsCountRate_Details.EditingControl.BackColor = Color.Lime
        dgv_EndsCountRate_Details.EditingControl.ForeColor = Color.Blue

    End Sub

    Private Sub dgtxt_EndsCount_Master_Rate_Details_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_EndsCount_Master_Rate_Details.KeyDown
        Try

            With dgv_EndsCountRate_Details

                vcbo_KeyDwnVal = e.KeyValue

                If .Visible Then
                    If e.KeyValue <> 27 Then

                        If .CurrentCell.RowIndex = 0 And .CurrentCell.ColumnIndex = 1 Then

                            e.Handled = True
                            e.SuppressKeyPress = True

                        End If

                    End If


                End If

            End With

        Catch ex As Exception
            '---

        End Try
    End Sub

    Private Sub dgtxt_EndsCount_Master_Rate_Details_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_EndsCount_Master_Rate_Details.KeyPress
        With dgv_EndsCountRate_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 1 Then

                    If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                ElseIf .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If
            End If
        End With
    End Sub

    Private Sub dgtxt_EndsCount_Master_Rate_Details_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_EndsCount_Master_Rate_Details.KeyUp
        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                dgv_EndsCountRate_Details_KeyUp(sender, e)
            End If
        Catch ex As Exception
            '---
        End Try
    End Sub

    Private Sub dgtxt_EndsCount_Master_Rate_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_EndsCount_Master_Rate_Details.TextChanged
        Try
            With dgv_EndsCountRate_Details

                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_EndsCount_Master_Rate_Details.Text)
                    End If
                End If
            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Single_Double_Triple_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Single_Double_Triple.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, cbo_Count, Nothing, "", "", "", "")

        If e.KeyValue = 40 And sender.DroppedDown = False Then
            cbo_EndsCountGroup.Focus()
        End If

    End Sub

    Private Sub cbo_Single_Double_Triple_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Single_Double_Triple.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "", "", "", "")

        If Asc(e.KeyChar) = 13 Then
            cbo_EndsCountGroup.Focus()
        End If

    End Sub


End Class