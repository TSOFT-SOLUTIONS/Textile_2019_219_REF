Public Class Piece_Opening
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "OPPCS-"
    Private Pk_ConditionOld As String = "OPENI-"
    Private OpYrCode As String = ""
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String
    Private SaveAll_STS As Boolean = False
    Private SaveAll_Status_To_Corr_PkCond_Pbm As Boolean = False
    Private LastNo As String = ""
    Private Filter_RowNo As Integer = -1

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        'pnl_Selection.Visible = False

        lbl_LotNo.Text = ""
        lbl_LotNo.ForeColor = Color.Black


        'dtp_Date.Text = ""
        cbo_ClothName.Text = ""
        cbo_PartyName_StockOf.Text = ""
        cbo_EndsCount.Text = ""
        cbo_Godown_StockIn.Text = "GODOWN"

        txt_Folding.Text = "100"
        cbo_WidthType.Text = "DOUBLE"
        txt_NoOfBeamInLoom.Text = 2

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        dgv_Details.Rows.Clear()


        cbo_ClothName.Enabled = True
        cbo_ClothName.BackColor = Color.White

        txt_Folding.Enabled = True
        txt_Folding.BackColor = Color.White



        'If Filter_Status = False Then
        '    dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
        '    dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
        '    cbo_Filter_GodownName.Text = ""
        '    cbo_Filter_GodownName.SelectedIndex = -1
        '    cbo_Filter_Cloth.Text = ""
        '    cbo_Filter_Cloth.SelectedIndex = -1
        '    dgv_Filter_Details.Rows.Clear()
        'End If

        dgv_Details.Columns(1).ReadOnly = False
        dgv_Details.Columns(2).ReadOnly = False

        Grid_Cell_DeSelect()
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
            End If
        End If

    End Sub

    Private Sub ControlLostFocus1(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.LightBlue
                Prec_ActCtrl.ForeColor = Color.Blue
            End If
        End If

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
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
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False

    End Sub

    Private Sub Piece_Opening_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName_StockOf.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName_StockOf.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ClothName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ClothName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Godown_StockIn.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "GODOWN" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Godown_StockIn.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Piece_Opening_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Piece_Opening_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub


                Else
                    Close_Form()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub piece_Opening_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        Me.Text = ""

        dgv_Details.Columns(2).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type1))
        dgv_Details.Columns(3).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type2))
        dgv_Details.Columns(4).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type3))
        dgv_Details.Columns(5).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type4))
        dgv_Details.Columns(6).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type5))

        con.Open()

        cbo_WidthType.Items.Clear()
        cbo_WidthType.Items.Add("")
        cbo_WidthType.Items.Add("SINGLE")
        cbo_WidthType.Items.Add("DOUBLE")
        cbo_WidthType.Items.Add("TRIPLE")
        cbo_WidthType.Items.Add("FOURTH")

        cbo_PartyName_StockOf.Visible = False
        lbl_PartyName_StockOf_Caption.Visible = False
        If Common_Procedures.settings.JOBWORKENTRY_Status = 1 Then
            cbo_PartyName_StockOf.Visible = True
            lbl_PartyName_StockOf_Caption.Visible = True

        Else
            lbl_ClothCaption.Left = lbl_Godown_StockIn_Caption.Left
            cbo_ClothName.Left = cbo_Godown_StockIn.Left
            cbo_ClothName.Width = 444

            lbl_Folding_Caption.Left = lbl_NoOfBeamInLoom_Caption.Left
            txt_Folding.Left = txt_NoOfBeamInLoom.Left
            txt_Folding.Width = txt_NoOfBeamInLoom.Width

        End If

        cbo_Godown_StockIn.Visible = False
        lbl_Godown_StockIn_Caption.Visible = False
        If Common_Procedures.settings.Multi_Godown_Status = 1 Then
            cbo_Godown_StockIn.Visible = True
            lbl_Godown_StockIn_Caption.Visible = True

            If Common_Procedures.settings.JOBWORKENTRY_Status = 0 Then
                lbl_Godown_StockIn_Caption.Left = lbl_PartyName_StockOf_Caption.Left
                lbl_Godown_StockIn_Caption.Top = lbl_PartyName_StockOf_Caption.Top
                cbo_Godown_StockIn.Left = cbo_PartyName_StockOf.Left
                cbo_Godown_StockIn.Top = cbo_PartyName_StockOf.Top
                cbo_Godown_StockIn.Width = cbo_PartyName_StockOf.Width
            End If

        End If




        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        AddHandler cbo_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName_StockOf.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_WidthType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NoOfBeamInLoom.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Godown_StockIn.GotFocus, AddressOf ControlGotFocus



        AddHandler cbo_Filter_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_GodownName.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Folding.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Filter_Folding.GotFocus, AddressOf ControlGotFocus

      
        'AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName_StockOf.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_WidthType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NoOfBeamInLoom.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Godown_StockIn.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Folding.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_GodownName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Filter_Folding.LostFocus, AddressOf ControlLostFocus

      
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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
                        If .CurrentCell.ColumnIndex >= 8 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                    save_record()
                                Else
                                    cbo_ClothName.Focus()
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else
                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                    save_record()
                                Else
                                    cbo_ClothName.Focus()
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If


                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                If txt_NoOfBeamInLoom.Enabled Then
                                    txt_NoOfBeamInLoom.Focus()
                                ElseIf cbo_ClothName.Enabled Then
                                    cbo_ClothName.Focus()
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(8)

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

            da1 = New SqlClient.SqlDataAdapter("select a.* , b.Ledger_Name from Piece_Opening_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Piece_Opening_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_LotNo.Text = dt1.Rows(0).Item("Piece_Opening_No").ToString
                cbo_ClothName.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
                If Val(dt1.Rows(0).Item("Folding").ToString) <> 0 Then
                    txt_Folding.Text = Val(dt1.Rows(0).Item("Folding").ToString)
                End If
                cbo_PartyName_StockOf.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_EndsCount.Text = Common_Procedures.EndsCount_IdNoToName(con, Val(dt1.Rows(0).Item("Ends_CountIdNo").ToString))
                cbo_WidthType.Text = dt1.Rows(0).Item("Width_Type").ToString
                txt_NoOfBeamInLoom.Text = dt1.Rows(0).Item("NoOf_Beam_InLoom").ToString
                cbo_Godown_StockIn.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("WareHouse_IdNo").ToString))


                da2 = New SqlClient.SqlDataAdapter("Select a.* from Weaver_ClothReceipt_Piece_Details a Where (a.Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' or a.Weaver_ClothReceipt_Code = '" & Trim(Pk_ConditionOld) & Trim(NewCode) & "') and a.Lot_Code = '" & Trim(NewCode) & "' Order by PieceNo_OrderBy, Main_PieceNo, Piece_No, Sl_No", con)
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
                            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Piece_No").ToString
                            If Val(dt2.Rows(i).Item("Type1_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("Type1_Meters").ToString), "########0.00")
                            End If
                            If Val(dt2.Rows(i).Item("Type2_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Type2_Meters").ToString), "########0.00")
                            End If
                            If Val(dt2.Rows(i).Item("Type3_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Type3_Meters").ToString), "########0.00")
                            End If
                            If Val(dt2.Rows(i).Item("Type4_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Type4_Meters").ToString), "########0.00")
                            End If
                            If Val(dt2.Rows(i).Item("Type5_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Type5_Meters").ToString), "########0.00")
                            End If

                            .Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Total_Checking_Meters").ToString), "########0.00")
                            If Val(dt2.Rows(i).Item("Weight").ToString) <> 0 Then
                                .Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                            End If

                            If Val(dt2.Rows(i).Item("Weight_Meter").ToString) <> 0 Then
                                .Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Weight_Meter").ToString), "########0.000")
                            End If

                            .Rows(n).Cells(10).Value = dt2.Rows(i).Item("PackingSlip_Code_Type1").ToString
                            If Trim(.Rows(n).Cells(10).Value) = "" Then .Rows(n).Cells(10).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type1").ToString
                            .Rows(n).Cells(11).Value = dt2.Rows(i).Item("PackingSlip_Code_Type2").ToString
                            If Trim(.Rows(n).Cells(11).Value) = "" Then .Rows(n).Cells(11).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type2").ToString
                            .Rows(n).Cells(12).Value = dt2.Rows(i).Item("PackingSlip_Code_Type3").ToString
                            If Trim(.Rows(n).Cells(12).Value) = "" Then .Rows(n).Cells(12).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type3").ToString
                            .Rows(n).Cells(13).Value = dt2.Rows(i).Item("PackingSlip_Code_Type4").ToString
                            If Trim(.Rows(n).Cells(13).Value) = "" Then .Rows(n).Cells(13).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type4").ToString
                            .Rows(n).Cells(14).Value = dt2.Rows(i).Item("PackingSlip_Code_Type5").ToString
                            If Trim(.Rows(n).Cells(14).Value) = "" Then .Rows(n).Cells(14).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type5").ToString


                            If Trim(.Rows(n).Cells(10).Value) <> "" Then
                                .Rows(n).Cells(1).Style.ForeColor = Color.Red
                                .Rows(n).Cells(1).ReadOnly = True
                                .Rows(n).Cells(2).Style.ForeColor = Color.Red
                                .Rows(n).Cells(2).ReadOnly = True
                                .Rows(n).Cells(8).Style.ForeColor = Color.Red
                                .Rows(n).Cells(8).ReadOnly = True
                                LockSTS = True
                            End If
                            If Trim(.Rows(n).Cells(11).Value) <> "" Then
                                .Rows(n).Cells(1).Style.ForeColor = Color.Red
                                .Rows(n).Cells(1).ReadOnly = True
                                .Rows(n).Cells(3).Style.ForeColor = Color.Red
                                .Rows(n).Cells(3).ReadOnly = True
                                .Rows(n).Cells(8).Style.ForeColor = Color.Red
                                .Rows(n).Cells(8).ReadOnly = True
                                LockSTS = True
                            End If
                            If Trim(.Rows(n).Cells(12).Value) <> "" Then
                                .Rows(n).Cells(1).Style.ForeColor = Color.Red
                                .Rows(n).Cells(1).ReadOnly = True
                                .Rows(n).Cells(4).Style.ForeColor = Color.Red
                                .Rows(n).Cells(4).ReadOnly = True
                                .Rows(n).Cells(8).Style.ForeColor = Color.Red
                                .Rows(n).Cells(8).ReadOnly = True
                                LockSTS = True
                            End If
                            If Trim(.Rows(n).Cells(13).Value) <> "" Then
                                .Rows(n).Cells(1).Style.ForeColor = Color.Red
                                .Rows(n).Cells(1).ReadOnly = True
                                .Rows(n).Cells(5).Style.ForeColor = Color.Red
                                .Rows(n).Cells(5).ReadOnly = True
                                .Rows(n).Cells(8).Style.ForeColor = Color.Red
                                .Rows(n).Cells(8).ReadOnly = True
                                LockSTS = True
                            End If
                            If Trim(.Rows(n).Cells(14).Value) <> "" Then
                                .Rows(n).Cells(1).Style.ForeColor = Color.Red
                                .Rows(n).Cells(1).ReadOnly = True
                                .Rows(n).Cells(6).Style.ForeColor = Color.Red
                                .Rows(n).Cells(6).ReadOnly = True
                                .Rows(n).Cells(8).Style.ForeColor = Color.Red
                                .Rows(n).Cells(8).ReadOnly = True
                                LockSTS = True
                            End If

                        Next i

                    End If

                    If .RowCount = 0 Then .Rows.Add()

                End With

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(1).Value = Format(Val(dt1.Rows(0).Item("Total_Pieces").ToString), "########0.00")
                    .Rows(0).Cells(2).Value = Format(Val(dt1.Rows(0).Item("Total_Type1_Meters").ToString), "########0.00")
                    .Rows(0).Cells(3).Value = Format(Val(dt1.Rows(0).Item("Total_Type2_Meters").ToString), "########0.00")
                    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Type3_Meters").ToString), "########0.00")
                    .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Total_Type4_Meters").ToString), "########0.00")
                    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_Type5_Meters").ToString), "########0.00")
                    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Checking_Meters").ToString), "########0.00")
                    .Rows(0).Cells(8).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                End With

                dt1.Clear()

                If LockSTS = True Then

                    cbo_ClothName.Enabled = False
                    cbo_ClothName.BackColor = Color.LightGray

                    txt_Folding.Enabled = False
                    txt_Folding.BackColor = Color.LightGray

                End If

            End If

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If cbo_PartyName_StockOf.Visible And cbo_PartyName_StockOf.Enabled Then cbo_PartyName_StockOf.Focus()

        End Try

        NoCalc_Status = False

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Piece_OpeningStock, "~L~") = 0 And InStr(Common_Procedures.UR.Piece_OpeningStock, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Piece_Opening, New_Entry, Me) = False Then Exit Sub


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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_LotNo.Text) & "/" & Trim(OpYrCode)

        Da = New SqlClient.SqlDataAdapter("select COUNT(*) from Weaver_ClothReceipt_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and (Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' or Weaver_ClothReceipt_Code = '" & Trim(Pk_ConditionOld) & Trim(NewCode) & "') and Lot_Code = '" & Trim(NewCode) & "' and (PackingSlip_Code_Type1 <> '' or PackingSlip_Code_Type2 <> '' or PackingSlip_Code_Type3 <> '' or PackingSlip_Code_Type4 <> '' or PackingSlip_Code_Type5 <> '' or BuyerOffer_Code_Type1 <> '' or BuyerOffer_Code_Type2 <> '' or BuyerOffer_Code_Type3 <> '' or BuyerOffer_Code_Type4 <> '' or BuyerOffer_Code_Type5 <> '')", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) <> 0 Then
                    MessageBox.Show("Packing Slip prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Weaver_ClothReceipt_Piece_Details Where (Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' or Weaver_ClothReceipt_Code = '" & Trim(Pk_ConditionOld) & Trim(NewCode) & "') and Lot_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Piece_Opening_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Piece_Opening_Code = '" & Trim(NewCode) & "'"
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

            ' If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where Ledger_Type = 'GODOWN' order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_GodownName.DataSource = dt1
            cbo_Filter_GodownName.DisplayMember = "Ledger_DisplayName"


            da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
            da.Fill(dt2)
            cbo_Filter_GodownName.DataSource = dt2
            cbo_Filter_GodownName.DisplayMember = "Cloth_Name"

            cbo_Filter_GodownName.Text = ""
            cbo_Filter_GodownName.SelectedIndex = -1
            cbo_Filter_Cloth.Text = ""
            cbo_Filter_Cloth.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Back.Enabled = False
        If cbo_Filter_GodownName.Enabled And cbo_Filter_GodownName.Visible Then cbo_Filter_GodownName.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Piece_Opening_No from Piece_Opening_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Piece_Opening_Code like '%/" & Trim(OpYrCode) & "' Order by for_Orderby, Piece_Opening_No", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Double = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_LotNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Piece_Opening_No from Piece_Opening_Head where for_orderby > " & Str(Format(Val(OrdByNo), "#########0.00")) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Piece_Opening_Code like '%/" & Trim(OpYrCode) & "' Order by for_Orderby, Piece_Opening_No", con)
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

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Double = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_LotNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Piece_Opening_No from Piece_Opening_Head where for_orderby < " & Str(Format(Val(OrdByNo), "########0.00")) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Piece_Opening_Code like '%/" & Trim(OpYrCode) & "' Order by for_Orderby desc, Piece_Opening_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Piece_Opening_No from Piece_Opening_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Piece_Opening_Code like '%/" & Trim(OpYrCode) & "' Order by for_Orderby desc, Piece_Opening_No desc", con)
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

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vwdthtype As String = ""
        Dim vNoofbms As String = ""


        Try

            vwdthtype = cbo_WidthType.Text
            vNoofbms = txt_NoOfBeamInLoom.Text

            clear()

            cbo_WidthType.Text = vwdthtype
            txt_NoOfBeamInLoom.Text = vNoofbms

            New_Entry = True

            lbl_LotNo.Text = Common_Procedures.get_MaxCode(con, "Piece_Opening_Head", "Piece_Opening_Code", "For_OrderBy", "", Val(lbl_Company.Tag), OpYrCode)
            lbl_LotNo.ForeColor = Color.Red

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da1.Dispose()

            If cbo_PartyName_StockOf.Enabled And cbo_PartyName_StockOf.Visible Then cbo_PartyName_StockOf.Focus()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        Try

            inpno = InputBox("Enter Lot No.", "FOR FINDING...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(OpYrCode)

            Da = New SqlClient.SqlDataAdapter("select Piece_Opening_No from Piece_Opening_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Piece_Opening_Code = '" & Trim(InvCode) & "'", con)
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
                MessageBox.Show("Lot No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Lot No.", "FOR NEW LOT NO. FOR INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(OpYrCode)

            Da = New SqlClient.SqlDataAdapter("select Piece_Opening_No from Piece_Opening_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Piece_Opening_Code = '" & Trim(InvCode) & "'", con)
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
                    MessageBox.Show("Invalid Lot No.", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_LotNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT ...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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
        Dim OpDate As Date
        Dim clth_ID As Integer = 0
        Dim Wev_ID As Integer = 0
        Dim Lm_ID As Integer = 0
        Dim Wdth_Typ As String = ""
        Dim Led_StkOf_IdNo As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim vGdwn_IdNo As Integer = 0

        Dim Sno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""

        Dim vTot_Pcs As Single

        Dim vTot_Typ1Mtrs As Single
        Dim vTot_Typ2Mtrs As Single
        Dim vTot_Typ3Mtrs As Single
        Dim vTot_Typ5Mtrs As Single
        Dim vTot_Typ4Mtrs As Single
        Dim vTot_ChkMtrs As Single
        Dim vTot_Wgt As Single
        Dim vTot_Wgt1 As String
        Dim vTot_Wgt2 As String
        Dim vTot_Wgt3 As String
        Dim vTot_Wgt4 As String
        Dim vTot_Wgt5 As String
        Dim stkof_idno As Integer = 0
        Dim Led_type As String = 0

        Dim Nr As Integer = 0

        Dim WagesCode As String = ""

        Dim ConsYarn As Single = 0
        Dim ConsPavu As Single = 0


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.Piece_OpeningStock, New_Entry) = False Then Exit Sub


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Piece_Opening, New_Entry, Me) = False Then Exit Sub


        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        Led_StkOf_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName_StockOf.Text)
        If cbo_PartyName_StockOf.Visible = True Then
            If Led_StkOf_IdNo = 0 Then
                MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_PartyName_StockOf.Enabled Then cbo_PartyName_StockOf.Focus()
                Exit Sub
            End If
        End If
        If Led_StkOf_IdNo = 0 Then Led_StkOf_IdNo = Common_Procedures.CommonLedger.OwnSort_Ac

        vGdwn_IdNo = Common_Procedures.Ledger_NameToIdNo(con, cbo_Godown_StockIn.Text)
        If cbo_Godown_StockIn.Visible = True Then
            If vGdwn_IdNo = 0 Then
                MessageBox.Show("Select Godown Name ?", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                cbo_Godown_StockIn.Focus()
                Exit Sub
            End If
        End If
        If vGdwn_IdNo = 0 Then vGdwn_IdNo = Common_Procedures.CommonLedger.Godown_Ac

        clth_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)
        If clth_ID = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ClothName.Enabled Then cbo_ClothName.Focus()
            Exit Sub
        End If

        If Val(txt_Folding.Text) = 0 Then
            txt_Folding.Text = 100
            'MessageBox.Show("Invalid Folding", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            'If txt_Folding.Enabled Then txt_Folding.Focus()
            'Exit Sub
        End If

        EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCount.Text)
        'If EdsCnt_ID = 0 Then
        '    MessageBox.Show("Invalid Ends Count", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If cbo_EndsCount.Enabled And cbo_EndsCount.Visible Then cbo_EndsCount.Focus()
        '    Exit Sub
        'End If

        With dgv_Details

            Sno = 0
            For i = 0 To .RowCount - 1

                If Trim(.Rows(i).Cells(1).Value) <> "" Or Val(.Rows(i).Cells(7).Value) <> 0 Or Val(.Rows(i).Cells(8).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(1).Value) = "" Then
                        MessageBox.Show("Invalid Piece No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(7).Value) = 0 Then
                        MessageBox.Show("Invalid Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(2)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(2).Value) And Val(.Rows(i).Cells(8).Value()) <> 0 Then
                        vTot_Wgt1 = Format(Val(vTot_Wgt1) + Val(.Rows(i).Cells(8).Value()), "##########0.000")
                    End If
                    If Val(.Rows(i).Cells(3).Value) And Val(.Rows(i).Cells(8).Value()) <> 0 Then
                        vTot_Wgt2 = Format(Val(vTot_Wgt2) + Val(.Rows(i).Cells(8).Value()), "##########0.000")
                    End If
                    If Val(.Rows(i).Cells(4).Value) And Val(.Rows(i).Cells(8).Value()) <> 0 Then
                        vTot_Wgt3 = Format(Val(vTot_Wgt3) + Val(.Rows(i).Cells(8).Value()), "##########0.000")
                    End If
                    If Val(.Rows(i).Cells(5).Value) And Val(.Rows(i).Cells(8).Value()) <> 0 Then
                        vTot_Wgt4 = Format(Val(vTot_Wgt4) + Val(.Rows(i).Cells(8).Value()), "##########0.000")
                    End If
                    If Val(.Rows(i).Cells(6).Value) And Val(.Rows(i).Cells(8).Value()) <> 0 Then
                        vTot_Wgt5 = Format(Val(vTot_Wgt5) + Val(.Rows(i).Cells(8).Value()), "##########0.000")
                    End If
                End If

            Next

        End With

        NoCalc_Status = False
        Total_Calculation()

        vTot_Pcs = 0 : vTot_Typ1Mtrs = 0 : vTot_Typ2Mtrs = 0 : vTot_Typ3Mtrs = 0 : vTot_Typ4Mtrs = 0 : vTot_Typ5Mtrs = 0 : vTot_ChkMtrs = 0 : vTot_Wgt = 0

        With dgv_Details_Total
            If .RowCount > 0 Then
                vTot_Pcs = vTot_Pcs + 1
                vTot_Typ1Mtrs = Val(.Rows(0).Cells(2).Value())
                vTot_Typ2Mtrs = Val(.Rows(0).Cells(3).Value())
                vTot_Typ3Mtrs = Val(.Rows(0).Cells(4).Value())
                vTot_Typ4Mtrs = Val(.Rows(0).Cells(5).Value())
                vTot_Typ5Mtrs = Val(.Rows(0).Cells(6).Value())
                vTot_ChkMtrs = Val(.Rows(0).Cells(7).Value())
                vTot_Wgt = Val(.Rows(0).Cells(8).Value())
            End If
        End With

        tr = con.BeginTransaction

        Try

            OpDate = CDate("01-04-" & Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4))
            OpDate = DateAdd(DateInterval.Day, -1, OpDate)

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_LotNo.Text) & "/" & Trim(OpYrCode)

            Else

                lbl_LotNo.Text = Common_Procedures.get_MaxCode(con, "Piece_Opening_Head", "Piece_Opening_Code", "For_OrderBy", "", Val(lbl_Company.Tag), OpYrCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_LotNo.Text) & "/" & Trim(OpYrCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@OpDate", OpDate)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Piece_Opening_Head (    Piece_Opening_Code  ,               Company_IdNo       ,     Piece_Opening_No          ,                               for_OrderBy                              ,   Ledger_IdNo        ,       Cloth_IdNo         ,                 Folding           ,              Total_Pieces      ,           Total_Type1_Meters    ,      Total_Type2_Meters         ,        Total_Type3_Meters      ,     Total_Type4_Meters          ,     Total_Type5_Meters         ,       Total_Checking_Meters   ,        Total_Weight   , Ends_CountIdNo                  ,   Width_Type                        ,     NoOf_Beam_InLoom                    ,   WareHouse_IdNo) " & _
                                            "     Values          ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_LotNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_LotNo.Text))) & ", " & Val(Led_StkOf_IdNo) & "  , " & Str(Val(clth_ID)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(vTot_Pcs)) & ",  " & Str(Val(vTot_Typ1Mtrs)) & ",  " & Str(Val(vTot_Typ2Mtrs)) & ", " & Str(Val(vTot_Typ3Mtrs)) & ",  " & Str(Val(vTot_Typ4Mtrs)) & ", " & Str(Val(vTot_Typ5Mtrs)) & ", " & Str(Val(vTot_ChkMtrs)) & ", " & Str(Val(vTot_Wgt)) & " , " & Str(Val(EdsCnt_ID)) & " ,  '" & Trim(cbo_WidthType.Text) & "',    " & Str(Val(txt_NoOfBeamInLoom.Text)) & " , " & Val(vGdwn_IdNo) & ") "
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Piece_Opening_Head set Ledger_IdNo =  " & Val(Led_StkOf_IdNo) & " , Cloth_IdNo = " & Str(Val(clth_ID)) & ", Folding =  " & Str(Val(txt_Folding.Text)) & ", Total_Pieces =  " & Str(Val(vTot_Pcs)) & ", Total_Type1_Meters = " & Str(Val(vTot_Typ1Mtrs)) & ",  Total_Type2_Meters = " & Str(Val(vTot_Typ2Mtrs)) & ", Total_Type3_Meters = " & Str(Val(vTot_Typ3Mtrs)) & ", Total_Type4_Meters = " & Str(Val(vTot_Typ4Mtrs)) & ", Total_Type5_Meters = " & Str(Val(vTot_Typ5Mtrs)) & ", Total_Checking_Meters = " & Str(Val(vTot_ChkMtrs)) & ", Total_Weight = " & Str(Val(vTot_Wgt)) & ", Ends_CountIdNo = " & Str(Val(EdsCnt_ID)) & " , Width_Type ='" & Trim(cbo_WidthType.Text) & "', NoOf_Beam_InLoom = " & Str(Val(txt_NoOfBeamInLoom.Text)) & " , WareHouse_IdNo = " & Val(vGdwn_IdNo) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Piece_Opening_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Delete from Weaver_ClothReceipt_Piece_Details Where (Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' or Weaver_ClothReceipt_Code = '" & Trim(Pk_ConditionOld) & Trim(NewCode) & "') and Lot_Code = '" & Trim(NewCode) & "' and PackingSlip_Code_Type1 = '' and PackingSlip_Code_Type2 = '' and PackingSlip_Code_Type3 = '' and PackingSlip_Code_Type4 = '' and PackingSlip_Code_Type5 = '' and BuyerOffer_Code_Type1 = '' and BuyerOffer_Code_Type2 = '' and BuyerOffer_Code_Type3 = '' and BuyerOffer_Code_Type4 = '' and BuyerOffer_Code_Type5 = ''"
                cmd.ExecuteNonQuery()

            End If

            Led_type = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Val(Led_StkOf_IdNo) & ")", , tr)

            stkof_idno = 0
            If Trim(UCase(Led_type)) = "JOBWORKER" Then
                stkof_idno = Led_StkOf_IdNo
            Else
                stkof_idno = Val(Common_Procedures.CommonLedger.OwnSort_Ac)
            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_LotNo.Text)

            Partcls = "Piece Opening : LotNo. " & Trim(lbl_LotNo.Text)
            Partcls = Trim(Partcls) & ",  Cloth : " & Trim(cbo_ClothName.Text)

            PBlNo = Trim(lbl_LotNo.Text)


            With dgv_Details

                Sno = 0
                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(1).Value) <> "" And Val(.Rows(i).Cells(7).Value) <> 0 Then

                        Sno = Sno + 1

                        Nr = 0
                        cmd.CommandText = "Update Weaver_ClothReceipt_Piece_Details set Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "', Weaver_ClothReceipt_Date = @OpDate, Weaver_Piece_Checking_date = @OpDate, StockOff_IdNo = " & Str(Val(stkof_idno)) & ", Ledger_IdNo =  " & Val(Led_StkOf_IdNo) & " , WareHouse_IdNo =  " & Val(vGdwn_IdNo) & " , Folding = " & Str(Val(txt_Folding.Text)) & ", Sl_No = " & Str(Val(Sno)) & ", Type1_Meters = " & Str(Val(.Rows(i).Cells(2).Value)) & ", Type2_Meters = " & Str(Val(.Rows(i).Cells(3).Value)) & ", Type3_Meters = " & Str(Val(.Rows(i).Cells(4).Value)) & ", Type4_Meters  = " & Str(Val(.Rows(i).Cells(5).Value)) & ", Type5_Meters = " & Str(Val(.Rows(i).Cells(6).Value)) & ", Total_Checking_Meters = " & Str(Val(.Rows(i).Cells(7).Value)) & ", Weight = " & Str(Val(.Rows(i).Cells(8).Value)) & ", Weight_Meter = " & Str(Val(.Rows(i).Cells(9).Value)) & " Where (Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' or Weaver_ClothReceipt_Code = '" & Trim(Pk_ConditionOld) & Trim(NewCode) & "' ) and Lot_Code = '" & Trim(NewCode) & "' and Piece_No = '" & Trim(.Rows(i).Cells(1).Value) & "'"
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then
                            cmd.CommandText = "Insert into Weaver_ClothReceipt_Piece_Details ( Weaver_Piece_Checking_Code ,             Company_IdNo         ,     Weaver_Piece_Checking_No   , Weaver_Piece_Checking_date,                  Weaver_ClothReceipt_Code   ,       Weaver_ClothReceipt_No    ,                               for_orderby                              ,  Weaver_ClothReceipt_Date,         Lot_Code       ,               Lot_No          ,         StockOff_IdNo   ,       Ledger_IdNo   ,           WareHouse_IdNo       , Cloth_IdNo     ,             Folding               ,           Sl_No      ,                                 PieceNo_OrderBy                                         ,                                Main_PieceNo        ,                    Piece_No            ,                       Type1_Meters        ,                      Type2_Meters        ,                      Type3_Meters        ,                      Type4_Meters        ,                      Type5_Meters        ,                  Total_Checking_Meters   ,                     Weight         ,     Weight_Meter            ) " &
                                                "     Values                                 (    '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ",  '" & Trim(lbl_LotNo.Text) & "',            @OpDate        , '" & Trim(Pk_Condition) & Trim(NewCode) & "',   '" & Trim(lbl_LotNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_LotNo.Text))) & ",          @OpDate         , '" & Trim(NewCode) & "', '" & Trim(lbl_LotNo.Text) & "',  " & Val(stkof_idno) & ", " & Val(Led_StkOf_IdNo) & " , " & Val(vGdwn_IdNo) & " , " & Str(Val(clth_ID)) & ", " & Str(Val(txt_Folding.Text)) & ",  " & Str(Val(Sno)) & ",   " & Str(Val(Common_Procedures.OrderBy_CodeToValue(Trim(.Rows(i).Cells(1).Value)))) & ",  " & Str(Val(.Rows(i).Cells(1).Value)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "',  " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & "  ,  " & Str(Val(.Rows(i).Cells(9).Value)) & " ) "
                            cmd.ExecuteNonQuery()
                        End If

                    End If

                Next

            End With

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            If SaveAll_Status_To_Corr_PkCond_Pbm = True Then
                ''---'---remove after save all - by thanges
                cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_ConditionOld) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()
            End If

            If Val(vTot_ChkMtrs) <> 0 Then
                cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code    ,             Company_IdNo         ,              Reference_No     ,                               for_OrderBy                              , Reference_Date,       StockOff_IdNo    ,         DeliveryTo_Idno     , ReceivedFrom_Idno,         Entry_ID     ,   Party_Bill_No      ,    Particulars         , Sl_No,          Cloth_Idno      ,                  Folding           , UnChecked_Meters,             Meters_Type1       ,              Meters_Type2      ,              Meters_Type3      ,              Meters_Type4      ,              Meters_Type5      ,            Weight          ,         Weight_Type1             , Weight_Type2         ,           Weight_Type3          ,             Weight_Type4         ,            Weight_Type5            ) " &
                                            " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_LotNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_LotNo.Text))) & ",     @OpDate   , " & Val(stkof_idno) & ", " & Str(Val(vGdwn_IdNo)) & ",         0        , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   1  , " & Str(Val(clth_ID)) & ",  " & Str(Val(txt_Folding.Text)) & ",        0        , " & Str(Val(vTot_Typ1Mtrs)) & ", " & Str(Val(vTot_Typ2Mtrs)) & ", " & Str(Val(vTot_Typ3Mtrs)) & ", " & Str(Val(vTot_Typ4Mtrs)) & ", " & Str(Val(vTot_Typ5Mtrs)) & ", " & Str(Val(vTot_Wgt)) & " , " & Str(Val(vTot_Wgt1)) & " ," & Str(Val(vTot_Wgt2)) & " ," & Str(Val(vTot_Wgt3)) & " , " & Str(Val(vTot_Wgt4)) & " ,  " & Str(Val(vTot_Wgt5)) & "  ) "
                cmd.ExecuteNonQuery()
            End If

            tr.Commit()

            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If


            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_LotNo.Text)
                End If
            Else
                move_record(lbl_LotNo.Text)
            End If

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()

            If cbo_ClothName.Enabled And cbo_ClothName.Visible Then cbo_ClothName.Focus()

        End Try

    End Sub

    Private Sub Total_Calculation()
        Dim TotPcs As Single
        Dim Totsnd As Single
        Dim Totsec As Single
        Dim Totbit As Single
        Dim Totrej As Single
        Dim Tototr As Single
        Dim Tottlmr As Single
        Dim Totwgt As Single

        If NoCalc_Status = True Then Exit Sub

        TotPcs = 0 : Totsnd = 0 : Totsec = 0 : Totbit = 0 : Totrej = 0 : Tototr = 0 : Tottlmr = 0 : Totwgt = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                If Val(.Rows(i).Cells(1).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0 Then

                    TotPcs = TotPcs + Val(.Rows(i).Cells(1).Value())
                    Totsnd = Totsnd + Val(.Rows(i).Cells(2).Value())
                    Totsec = Totsec + Val(.Rows(i).Cells(3).Value())
                    Totrej = Totrej + Val(.Rows(i).Cells(4).Value())
                    Totbit = Totbit + Val(.Rows(i).Cells(5).Value())
                    Tototr = Tototr + Val(.Rows(i).Cells(6).Value())
                    Tottlmr = Tottlmr + Val(.Rows(i).Cells(7).Value())
                    Totwgt = Totwgt + Val(.Rows(i).Cells(8).Value())

                    If Val(.Rows(i).Cells(7).Value) <> 0 Then
                        .Rows(i).Cells(9).Value = Format(Val(.Rows(i).Cells(8).Value) / Val(.Rows(i).Cells(7).Value), "#########0.000")
                    End If

                End If

            Next i

        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(1).Value = Val(TotPcs)
            .Rows(0).Cells(2).Value = Format(Val(Totsnd), "########0.00")
            .Rows(0).Cells(3).Value = Format(Val(Totsec), "########0.00")
            .Rows(0).Cells(4).Value = Format(Val(Totrej), "########0.00")
            .Rows(0).Cells(5).Value = Format(Val(Totbit), "########0.00")
            .Rows(0).Cells(6).Value = Format(Val(Tototr), "########0.00")
            .Rows(0).Cells(7).Value = Format(Val(Tottlmr), "########0.00")
            .Rows(0).Cells(8).Value = Format(Val(Totwgt), "########0.000")
        End With

        'With dgv_Details
        '    If .Visible Then
        '        If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 8 Then
        '            If Val(.CurrentRow.Cells(7).Value) <> 0 Then
        '                .CurrentRow.Cells(9).Value = Format(Val(.CurrentRow.Cells(8).Value) / Val(.CurrentRow.Cells(7).Value), "#########0.000")
        '            End If
        '        End If
        '    End If
        'End With

    End Sub

    Private Sub TotalMeter_Calculation()
        Dim fldmtr As Integer = 0
        Dim Tot_Pc_Mtrs As Single = 0, Tot_Pc_Wt As Single = 0
        Dim fldperc As Single = 0
        Dim Wgt_Mtr As Single = 0
        Dim k As Integer = 0

        On Error Resume Next

        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 8 Then

                    .CurrentRow.Cells(7).Value = Format(Val(.CurrentRow.Cells(2).Value) + Val(.CurrentRow.Cells(3).Value) + Val(.CurrentRow.Cells(4).Value) + Val(.CurrentRow.Cells(5).Value) + Val(.CurrentRow.Cells(6).Value), "#########0.00")

                    'Tot_Pc_Mtrs = 0 : Tot_Pc_Wt = 0
                    'For k = 0 To .Rows.Count - 1

                    '    If Val(.CurrentRow.Cells(0).Value) = Val(.Rows(k).Cells(0).Value) Then
                    '        Tot_Pc_Mtrs = Tot_Pc_Mtrs + Val(.Rows(k).Cells(2).Value) + Val(.Rows(k).Cells(3).Value) + Val(.Rows(k).Cells(4).Value) + Val(.Rows(k).Cells(5).Value) + Val(.Rows(k).Cells(6).Value)
                    '        Tot_Pc_Wt = Tot_Pc_Wt + +Val(.Rows(k).Cells(8).Value)
                    '    End If

                    'Next

                    Total_Calculation()

                End If

            End If
        End With
    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName_StockOf.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'OWNSORT' or Ledger_Type = 'JOBWORKER' )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName_StockOf.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName_StockOf, Nothing, cbo_Godown_StockIn, "Ledger_AlaisHead", "Ledger_DisplayName", " ( Ledger_Type = 'OWNSORT' or Ledger_Type = 'JOBWORKER' )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName_StockOf.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName_StockOf, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'OWNSORT' or Ledger_Type = 'JOBWORKER' )", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If cbo_Godown_StockIn.Visible And cbo_Godown_StockIn.Enabled Then
                cbo_Godown_StockIn.Focus()
            Else
                cbo_ClothName.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_partyname_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName_StockOf.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "JOBWORKER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyName_StockOf.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1

            f.Show()

        End If
    End Sub

    Private Sub cbo_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothName, Nothing, txt_Folding, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
        If (e.KeyValue = 38 And cbo_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_Godown_StockIn.Visible And cbo_Godown_StockIn.Enabled Then
                cbo_Godown_StockIn.Focus()
            Else
                cbo_PartyName_StockOf.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothName.KeyPress
        Dim Clo_IdNo As Integer = 0
        Dim edscnt_idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothName, txt_Folding, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            'If Trim(cbo_EndsCount.Text) = "" Then
            Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)
                edscnt_idno = Val(Common_Procedures.get_FieldValue(con, "Cloth_EndsCount_Details", "EndsCount_IdNo", "(cloth_idno = " & Str(Val(Clo_IdNo)) & " and Sl_No = 1 )"))
                cbo_EndsCount.Text = Common_Procedures.EndsCount_IdNoToName(con, edscnt_idno)
            'End If
        End If

    End Sub

    Private Sub cbo_ClothName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ClothName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave

        With dgv_Details
            If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If

            If .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Then
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

        With dgv_Details
            If .Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 8 Then

                    TotalMeter_Calculation()
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

    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        With dgv_Details


            If e.KeyValue = Keys.Delete Then

                If .CurrentCell.ColumnIndex = 2 Then
                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(10).Value) <> "" Then
                        e.Handled = True
                    End If
                End If

                If .CurrentCell.ColumnIndex = 3 Then
                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(11).Value) <> "" Then
                        e.Handled = True
                    End If
                End If

                If .CurrentCell.ColumnIndex = 4 Then
                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(12).Value) <> "" Then
                        e.Handled = True
                    End If
                End If
                If .CurrentCell.ColumnIndex = 5 Then
                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(13).Value) <> "" Then
                        e.Handled = True
                    End If
                End If
                If .CurrentCell.ColumnIndex = 6 Then
                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(14).Value) <> "" Then
                        e.Handled = True
                    End If
                End If

            End If

        End With

    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        dgv_Details_KeyUp(sender, e)
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        On Error Resume Next
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                    If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Then
                        If Trim(.Rows(.CurrentCell.RowIndex).Cells(10).Value) <> "" Then
                            e.Handled = True
                        End If
                    End If

                    If .CurrentCell.ColumnIndex = 3 Then
                        If Trim(.Rows(.CurrentCell.RowIndex).Cells(11).Value) <> "" Then
                            e.Handled = True
                        End If
                    End If
                    If .CurrentCell.ColumnIndex = 4 Then
                        If Trim(.Rows(.CurrentCell.RowIndex).Cells(12).Value) <> "" Then
                            e.Handled = True
                        End If
                    End If
                    If .CurrentCell.ColumnIndex = 5 Then
                        If Trim(.Rows(.CurrentCell.RowIndex).Cells(13).Value) <> "" Then
                            e.Handled = True
                        End If
                    End If
                    If .CurrentCell.ColumnIndex = 6 Then
                        If Trim(.Rows(.CurrentCell.RowIndex).Cells(14).Value) <> "" Then
                            e.Handled = True
                        End If
                    End If
                End If
            End If
        End With

    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown

        'With dgv_Details

        '    If e.KeyCode = Keys.Left Then
        '        If .CurrentCell.ColumnIndex <= 0 Then
        '            If .CurrentCell.RowIndex = 0 Then
        '                txt_Folding.Focus()
        '            Else
        '                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)
        '            End If
        '        End If
        '    End If

        '    If e.KeyCode = Keys.Right Then
        '        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
        '            If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
        '                If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
        '                    save_record()
        '                Else
        '                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(2)
        '                End If
        '            End If
        '        End If
        '    End If

        'End With

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer
        Dim n As Integer
        Dim nrw As Integer
        Dim S As String


        If e.Control = True And (UCase(Chr(e.KeyCode)) = "A" Or UCase(Chr(e.KeyCode)) = "I" Or e.KeyCode = Keys.Insert) Then
            With dgv_Details

                n = .CurrentRow.Index

                S = Replace(Trim(.Rows(n).Cells(1).Value), Val(.Rows(n).Cells(1).Value), "")
                If Trim(UCase(S)) <> "Z" Then
                    S = Trim(UCase(S))
                    If Trim(S) = "" Then S = "A" Else S = Trim(Chr(Asc(S) + 1))
                    If n <> .Rows.Count - 1 Then
                        If Trim(Val(.Rows(n).Cells(1).Value)) & Trim(UCase(S)) = Trim(UCase(.Rows(n + 1).Cells(1).Value)) Then
                            MessageBox.Show("Already Piece Inserted", "DES NOT INSERT NEW PIECE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            Exit Sub
                        End If
                    End If

                    nrw = n + 1

                    dgv_Details.Rows.Insert(nrw, 1)

                    dgv_Details.Rows(nrw).Cells(1).Value = Trim(Val(.Rows(n).Cells(1).Value)) & S

                    For i = 0 To .Rows.Count - 1
                        .Rows(i).Cells(0).Value = i + 1
                    Next

                    .CurrentCell = .Rows(nrw).Cells(1)

                End If

            End With

        End If

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                If Trim(.Rows(.CurrentCell.RowIndex).Cells(10).Value) = "" And Trim(.Rows(.CurrentCell.RowIndex).Cells(11).Value) = "" And Trim(.Rows(.CurrentCell.RowIndex).Cells(12).Value) = "" And Trim(.Rows(.CurrentCell.RowIndex).Cells(13).Value) = "" And Trim(.Rows(.CurrentCell.RowIndex).Cells(14).Value) = "" Then

                    n = .CurrentRow.Index

                    If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                        For i = 0 To .ColumnCount - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(n)

                    End If

                    Total_Calculation()

                End If

            End With

        End If
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        dgv_Details.CurrentCell.Selected = False
    End Sub


    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub txt_Folding_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Folding.KeyDown

        If e.KeyValue = 38 Then cbo_ClothName.Focus() ' SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then
            If cbo_EndsCount.Text = "" Then
                cbo_EndsCount.Focus()
            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                    dgv_Details.CurrentCell.Selected = True
                End If
            End If
        End If
    End Sub

    'Private Sub cbo_Filter_Cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Cloth.KeyDown
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Cloth, cbo_Filter_GodownName, btn_Filter_Show, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    'End Sub

    'Private Sub cbo_Filter_Cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Cloth.KeyPress
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Cloth, btn_Filter_Show, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    'End Sub

    'Private Sub cbo_Filter_GodownName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_GodownName.KeyDown
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_GodownName, dtp_Filter_ToDate, cbo_Filter_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    'End Sub

    'Private Sub cbo_Filter_GodownName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_GodownName.KeyPress
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_GodownName, cbo_Filter_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    'End Sub
    'Private Sub btn_Filter_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
    '    pnl_Back.Enabled = True
    '    pnl_Filter.Visible = False
    '    Filter_Status = False
    'End Sub
    'Private Sub dgv_Filter_Details_CellDoubleClick1(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
    '    Open_FilterEntry()
    'End Sub

    'Private Sub dgv_Filter_Details_KeyDown1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
    '    If e.KeyCode = 13 Then
    '        Open_FilterEntry()
    '    End If
    'End Sub
    'Private Sub btn_Filter_Show_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click

    '    Dim da As New SqlClient.SqlDataAdapter
    '    Dim dt1 As New DataTable
    '    Dim dt2 As New DataTable
    '    Dim n As Integer
    '    Dim Led_IdNo As Integer, Clth_IdNo As Integer
    '    Dim Condt As String = ""

    '    Try

    '        Condt = ""
    '        Led_IdNo = 0
    '        Clth_IdNo = 0

    '        If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
    '            Condt = "a.Weaver_Piece_Checking_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
    '        ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
    '            Condt = "a.Weaver_Piece_Checking_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
    '        ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
    '            Condt = "a.Weaver_Piece_Checking_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
    '        End If

    '        If Trim(cbo_Filter_GodownName.Text) <> "" Then
    '            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_GodownName.Text)
    '        End If

    '        If Trim(cbo_Filter_Cloth.Text) <> "" Then
    '            Clth_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_Cloth.Text)
    '        End If


    '        If Val(Led_IdNo) <> 0 Then
    '            Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(Led_IdNo)) & ")"
    '        End If

    '        If Val(Clth_IdNo) <> 0 Then
    '            Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Cloth_IdNo = " & Str(Val(Clth_IdNo))
    '        End If

    '        da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name, e.Ledger_Name from Piece_Opening_Head a left outer join Weaver_ClothReceipt_Piece_Details b on a.Piece_Opening_Code = b.Piece_Opening_Code left outer join Cloth_head c on a.Cloth_idno = c.Cloth_idno left outer join Ledger_head e on a.Ledger_idno = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Piece_Opening_Code like '%/" & Trim(OpYrCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by Weaver_Piece_Checking_Date, for_orderby, Piece_Opening_No", con)
    '        da.Fill(dt2)

    '        dgv_Filter_Details.Rows.Clear()

    '        If dt2.Rows.Count > 0 Then

    '            For i = 0 To dt2.Rows.Count - 1

    '                n = dgv_Filter_Details.Rows.Add()

    '                dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
    '                dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Piece_Opening_No").ToString
    '                dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Weaver_Piece_Checking_Date").ToString), "dd-MM-yyyy")
    '                dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
    '                dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Piece_Receipt_No").ToString
    '                dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Cloth_Name").ToString
    '                dgv_Filter_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Piece_Receipt_Date").ToString
    '                dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Receipt_Meters").ToString), "########0.00")
    '                dgv_Filter_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")

    '            Next i

    '        End If

    '        dt2.Clear()

    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

    '    Finally
    '        dt2.Dispose()
    '        da.Dispose()

    '        If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

    '    End Try

    'End Sub

    'Private Sub Open_FilterEntry()
    '    Dim movno As String

    '    On Error Resume Next

    '    movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

    '    If Val(movno) <> 0 Then
    '        Filter_Status = True
    '        move_record(movno)
    '        pnl_Back.Enabled = True
    '        pnl_Filter.Visible = False
    '    End If

    'End Sub
    'Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
    '    Open_FilterEntry()
    'End Sub

    'Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    If e.KeyCode = 13 Then
    '        Open_FilterEntry()
    '    End If
    'End Sub

    Private Sub txt_Folding_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Folding.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If cbo_EndsCount.Text = "" Then
                cbo_EndsCount.Focus()
            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                    dgv_Details.CurrentCell.Selected = True
                End If
            End If
        End If

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-------
    End Sub

    Private Sub txt_Folding_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Folding.TextChanged
        Total_Calculation()
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer

        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)

        End With
    End Sub

    Private Sub btn_SaveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SaveAll.Click
        Dim g As New Password
        g.ShowDialog()

        If Trim(UCase(Common_Procedures.Password_Input)) <> "TSSA7417" Then
            MessageBox.Show("Invalid Password", "PASSWORD FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        SaveAll_Status_To_Corr_PkCond_Pbm = False
        Select Case MessageBox.Show("Are you saving to correct pk_condition problem?", "WHY SAVING ALL ENTRIES?...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            Case Windows.Forms.DialogResult.Yes
                SaveAll_Status_To_Corr_PkCond_Pbm = True

            Case Windows.Forms.DialogResult.No
                SaveAll_Status_To_Corr_PkCond_Pbm = False

            Case Windows.Forms.DialogResult.Cancel
                Exit Sub

        End Select

        SaveAll_STS = True

        LastNo = ""
        movelast_record()

        LastNo = lbl_LotNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_LotNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub

    Private Sub cbo_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

    End Sub

    Private Sub cbo_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EndsCount, txt_Folding, cbo_WidthType, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

    End Sub

    Private Sub cbo_EndsCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EndsCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EndsCount, cbo_WidthType, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

    End Sub

    Private Sub cbo_EndsCount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyUp

        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New EndsCount_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_EndsCount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_WidthType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_WidthType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_WidthType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WidthType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_WidthType, cbo_EndsCount, txt_NoOfBeamInLoom, "", "", "", "")

    End Sub

    Private Sub cbo_WidthType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_WidthType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_WidthType, txt_NoOfBeamInLoom, "", "", "", "")
    End Sub

    Private Sub txt_NoOfBeamInLoom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_NoOfBeamInLoom.KeyDown
        If e.KeyValue = 38 Then cbo_WidthType.Focus()
        If (e.KeyValue = 40) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                dgv_Details.CurrentCell.Selected = True
            End If
        End If
    End Sub

    Private Sub txt_NoOfBeamInLoom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_NoOfBeamInLoom.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                dgv_Details.CurrentCell.Selected = True
            End If
        End If
    End Sub

    Private Sub cbo_Godown_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Godown_StockIn.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Godown_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Godown_StockIn.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Godown_StockIn, cbo_PartyName_StockOf, cbo_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Godown_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Godown_StockIn.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Godown_StockIn, cbo_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Godown_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Godown_StockIn.KeyUp
        If e.Control = True Or e.KeyCode = 17 Then
            Common_Procedures.MDI_LedType = "GODOWN"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Godown_StockIn.Name
            Common_Procedures.Master_Return.Master_Type = ""
            Common_Procedures.Master_Return.Return_Value = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
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



            If Trim(cbo_Filter_GodownName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_GodownName.Text)
            End If

            If Trim(cbo_Filter_Cloth.Text) <> "" Then
                Clth_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Filter_Cloth.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.WareHouse_IdNo = " & Str(Val(Led_IdNo)) & ")"
            End If

            If Val(Clth_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Cloth_IdNo = " & Str(Val(Clth_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("Select a.*, c.Cloth_Name from Piece_Opening_Head a INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Piece_Opening_Code like '%/" & Trim(OpYrCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " ", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()
            dgv_Filter_Details_Total.Rows.Clear()
            dgv_Filter_Details_Total.Rows.Add()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Piece_Opening_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Folding").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Total_Type1_Meters").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Total_Type2_Meters").ToString
                    dgv_Filter_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Total_Type3_Meters").ToString
                    dgv_Filter_Details.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Total_Type4_Meters").ToString
                    dgv_Filter_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Total_Type5_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Total_Checking_Meters").ToString), "########0.00")


                    dgv_Filter_Details_Total.Rows(0).Cells(4).Value = Val(dgv_Filter_Details_Total.Rows(0).Cells(4).Value) + Val(dt2.Rows(i).Item("Total_Type1_Meters").ToString)
                    dgv_Filter_Details_Total.Rows(0).Cells(5).Value = Val(dgv_Filter_Details_Total.Rows(0).Cells(5).Value) + Val(dt2.Rows(i).Item("Total_Type2_Meters").ToString)
                    dgv_Filter_Details_Total.Rows(0).Cells(6).Value = Val(dgv_Filter_Details_Total.Rows(0).Cells(6).Value) + Val(dt2.Rows(i).Item("Total_Type3_Meters").ToString)
                    dgv_Filter_Details_Total.Rows(0).Cells(7).Value = Val(dgv_Filter_Details_Total.Rows(0).Cells(7).Value) + Val(dt2.Rows(i).Item("Total_Type4_Meters").ToString)
                    dgv_Filter_Details_Total.Rows(0).Cells(8).Value = Val(dgv_Filter_Details_Total.Rows(0).Cells(8).Value) + Val(dt2.Rows(i).Item("Total_Type5_Meters").ToString)
                    dgv_Filter_Details_Total.Rows(0).Cells(9).Value = Val(dgv_Filter_Details_Total.Rows(0).Cells(9).Value) + Val(dt2.Rows(i).Item("Total_Checking_Meters").ToString)

                Next i

            End If

            dt2.Clear()



        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt2.Dispose()
            da.Dispose()

            If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

        End Try

    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            Filter_RowNo = dgv_Filter_Details.CurrentRow.Index
            move_record(movno)
            pnl_Back.Enabled = True
            pnl_Filter.Visible = False
        End If

    End Sub

    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub cbo_Filter_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Cloth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_Cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Cloth.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Cloth, cbo_Filter_GodownName, btn_Filter_Show, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_Cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Cloth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Cloth, btn_Filter_Show, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_GodownName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_GodownName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'GODOWN' )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_GodownName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_GodownName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_GodownName, Nothing, cbo_Filter_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'GODOWN' )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_GodownName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_GodownName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_GodownName, cbo_Filter_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'GODOWN' )", "(Ledger_idno = 0)")
    End Sub

    Private Sub dgtxt_Details_TextChanged(sender As Object, e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_Details

                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
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

    Private Sub cbo_PartyName_StockOf_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_PartyName_StockOf.SelectedIndexChanged

    End Sub

    Private Sub dgv_Details_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Details.CellDoubleClick
        On Error Resume Next

        With dgv_Details

            If .Visible Then

                If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then

                    If .CurrentCell.ColumnIndex = 2 Then
                        If Trim(.Rows(.CurrentCell.RowIndex).Cells(10).Value) <> "" Then
                            MessageBox.Show("BALE No. / DC NO.  : " & Trim(.Rows(.CurrentCell.RowIndex).Cells(10).Value), "PIECE DELIVERED STATUS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
                        End If
                    End If

                    If .CurrentCell.ColumnIndex = 3 Then
                        If Trim(.Rows(.CurrentCell.RowIndex).Cells(11).Value) <> "" Then
                            MessageBox.Show("BALE No. / DC NO.  : " & Trim(.Rows(.CurrentCell.RowIndex).Cells(11).Value), "PIECE DELIVERED STATUS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
                        End If
                    End If

                    If .CurrentCell.ColumnIndex = 4 Then
                        If Trim(.Rows(.CurrentCell.RowIndex).Cells(12).Value) <> "" Then
                            MessageBox.Show("BALE No. / DC NO.  : " & Trim(.Rows(.CurrentCell.RowIndex).Cells(12).Value), "PIECE DELIVERED STATUS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
                        End If
                    End If
                    If .CurrentCell.ColumnIndex = 5 Then
                        If Trim(.Rows(.CurrentCell.RowIndex).Cells(13).Value) <> "" Then
                            MessageBox.Show("BALE No. / DC NO.  : " & Trim(.Rows(.CurrentCell.RowIndex).Cells(13).Value), "PIECE DELIVERED STATUS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
                        End If
                    End If
                    If .CurrentCell.ColumnIndex = 6 Then
                        If Trim(.Rows(.CurrentCell.RowIndex).Cells(14).Value) <> "" Then
                            MessageBox.Show("BALE No. / DC NO.  : " & Trim(.Rows(.CurrentCell.RowIndex).Cells(14).Value), "PIECE DELIVERED STATUS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
                        End If
                    End If

                End If

            End If

        End With
    End Sub

End Class