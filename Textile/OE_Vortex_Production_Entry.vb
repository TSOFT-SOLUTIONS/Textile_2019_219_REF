
Public Class OE_Vortex_Production_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "OEVPR-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private dgv_ActCtrlName As String = ""
    Private WithEvents dgtxt_details As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_DrawingDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_VortexDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_WasteDetails As New DataGridViewTextBoxEditingControl
    Private Dep_Id As Integer = 0
    Private vSqlCond As String = ""

    Private Enum DgvCol_CardingDetails As Integer
        SL_NO                       '0
        MACHINE_NO                  '1
        COUNT_HANK                  '2    
        SPEED                       '3
        TARGET_PRODUCTIONS          '4    
        ACTUAL_PRODUCTION           '5    
        ACTUAL_EFFICIENCY           '6

    End Enum
    Private Enum DgvCol_DrawingDetails As Integer

        SL_NO                       '0
        MACHINE_NO                  '1
        COUNT_HANK                  '2    
        SPEED                       '3
        TARGET_PRODUCTIONS          '4    
        ACTUAL_PRODUCTION           '5    
        ACTUAL_EFFICIENCY           '6

    End Enum
    Private Enum DgvCol_VortexDetails As Integer

        SL_NO                      '0 
        COUNT_NAME                 '1
        SPEED                      '2
        TARGET_PRODUCTIONS         '3
        ACTUAL_PRODUCTION          '4
        ACTUAL_EFFICIENCY          '5
        STOP_MIN                   '6

    End Enum
    Private Enum DgvCol_WasteDetails As Integer

        SL_NO                      '0 
        WASTE_NAME                 '1
        WEIGHT                     '2
        WASTE_PERCENTAGE           '3

    End Enum
    Private Enum DgvCol_FillterDetails As Integer

        REF_NO                      '0 
        FILTER_DATE                 '1
        SHIFT                       '2
        MACHINE_NO                  '3
        COUNT                       '4
        WASTE                       '5


    End Enum

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        cbo_Shift.Text = ""
        '       cbo_Shift.Tag = ""
        cbo_Shift.Tag = cbo_Shift.Text

        Cbo_WasteGrid_WasteName.Text = ""
        cbo_Grid_Vortex_CountName.Text = ""

        dgv_CardingDetails.Rows.Clear()
        dgv_CardingDetails_Total.Rows.Clear()

        Dgv_DrawingDetails.Rows.Clear()
        Dgv_DrawingDetails_Total.Rows.Clear()

        Dgv_VortexDetails.Rows.Clear()
        Dgv_VortexDetails_Total.Rows.Clear()

        Dgv_WasteDetails.Rows.Clear()
        Dgv_WasteDetails_Total.Rows.Clear()

        Grid_DeSelect()

        dgv_ActCtrlName = ""

    End Sub
    Private Sub Grid_DeSelect()
        On Error Resume Next

        If Not IsNothing(dgv_CardingDetails.CurrentCell) Then dgv_CardingDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_CardingDetails_Total.CurrentCell) Then dgv_CardingDetails_Total.CurrentCell.Selected = False

        If Not IsNothing(Dgv_DrawingDetails.CurrentCell) Then Dgv_DrawingDetails.CurrentCell.Selected = False
        If Not IsNothing(Dgv_DrawingDetails_Total.CurrentCell) Then Dgv_DrawingDetails_Total.CurrentCell.Selected = False

        If Not IsNothing(Dgv_VortexDetails.CurrentCell) Then Dgv_VortexDetails.CurrentCell.Selected = False
        If Not IsNothing(Dgv_VortexDetails_Total.CurrentCell) Then Dgv_VortexDetails_Total.CurrentCell.Selected = False

        If Not IsNothing(Dgv_WasteDetails.CurrentCell) Then Dgv_WasteDetails.CurrentCell.Selected = False
        If Not IsNothing(Dgv_WasteDetails_Total.CurrentCell) Then Dgv_WasteDetails_Total.CurrentCell.Selected = False
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

        If Me.ActiveControl.Name <> Cbo_WasteGrid_WasteName.Name Then
            Cbo_WasteGrid_WasteName.Visible = False
            Cbo_WasteGrid_WasteName.Tag = -100
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Vortex_CountName.Name Then
            cbo_Grid_Vortex_CountName.Visible = False
            cbo_Grid_Vortex_CountName.Tag = -100
        End If

        If Me.ActiveControl.Name <> dgv_CardingDetails.Name Then
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
        If Not IsNothing(dgv_CardingDetails.CurrentCell) Then dgv_CardingDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_CardingDetails_Total.CurrentCell) Then dgv_CardingDetails_Total.CurrentCell.Selected = False

        If Not IsNothing(Dgv_DrawingDetails.CurrentCell) Then Dgv_DrawingDetails.CurrentCell.Selected = False
        If Not IsNothing(Dgv_DrawingDetails_Total.CurrentCell) Then Dgv_DrawingDetails_Total.CurrentCell.Selected = False

        If Not IsNothing(Dgv_VortexDetails.CurrentCell) Then Dgv_VortexDetails.CurrentCell.Selected = False
        If Not IsNothing(Dgv_VortexDetails_Total.CurrentCell) Then Dgv_VortexDetails_Total.CurrentCell.Selected = False

        If Not IsNothing(Dgv_WasteDetails.CurrentCell) Then Dgv_WasteDetails.CurrentCell.Selected = False
        If Not IsNothing(Dgv_WasteDetails_Total.CurrentCell) Then Dgv_WasteDetails_Total.CurrentCell.Selected = False
    End Sub
    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim da4 As New SqlClient.SqlDataAdapter
        Dim da5 As New SqlClient.SqlDataAdapter
        Dim da6 As New SqlClient.SqlDataAdapter
        Dim da7 As New SqlClient.SqlDataAdapter
        Dim da8 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim dt8 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim SlNo As Integer
        Dim LockSTS As Boolean = False
        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)


        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*  from OE_Vortex_Production_Head  a   Where a.OE_Production_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_RefNo.Text = dt1.Rows(0).Item("OE_Production_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("OE_Production_Date").ToString
                cbo_Shift.Text = Common_Procedures.Shift_IdNoToName(con, Val(dt1.Rows(0).Item("Shift_IdNo").ToString))
                cbo_Shift.Tag = Trim(cbo_Shift.Text)

                da2 = New SqlClient.SqlDataAdapter("select * from OE_Carding_Production_Details a  where a.OE_Production_Code = '" & Trim(NewCode) & "' Order by Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)



                With dgv_CardingDetails

                    dgv_CardingDetails.Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1
                            .Rows(n).Cells(DgvCol_CardingDetails.SL_NO).Value = Val(SNo)
                            .Rows(n).Cells(DgvCol_CardingDetails.MACHINE_NO).Value = dt2.Rows(i).Item("Machine_No").ToString
                            .Rows(n).Cells(DgvCol_CardingDetails.COUNT_HANK).Value = Val(dt2.Rows(i).Item("Count_Hank").ToString)
                            .Rows(n).Cells(DgvCol_CardingDetails.SPEED).Value = Val(dt2.Rows(i).Item("Speed").ToString)
                            .Rows(n).Cells(DgvCol_CardingDetails.TARGET_PRODUCTIONS).Value = Format(Val(dt2.Rows(i).Item("Target_Production").ToString), "#########0.000")
                            .Rows(n).Cells(DgvCol_CardingDetails.ACTUAL_PRODUCTION).Value = Format(Val(dt2.Rows(i).Item("Actual_Production").ToString), "#########0.000")
                            .Rows(n).Cells(DgvCol_CardingDetails.ACTUAL_EFFICIENCY).Value = Format(Val(dt2.Rows(i).Item("Actual_Efficiency").ToString), "#########0.00")


                        Next i


                    End If

                End With

                With dgv_CardingDetails_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(DgvCol_CardingDetails.MACHINE_NO).Value = Format(Val(dt1.Rows(0).Item("Total_carding_Noof_Machine").ToString), "########0")
                    .Rows(0).Cells(DgvCol_CardingDetails.COUNT_HANK).Value = Format(Val(dt1.Rows(0).Item("Total_Carding_Hank").ToString), "########0.000")
                    .Rows(0).Cells(DgvCol_CardingDetails.SPEED).Value = Format(Val(dt1.Rows(0).Item("Total_Carding_Speed").ToString), "########0.000")
                    .Rows(0).Cells(DgvCol_CardingDetails.TARGET_PRODUCTIONS).Value = Format(Val(dt1.Rows(0).Item("Total_Carding_Target_Production").ToString), "########0.000")
                    .Rows(0).Cells(DgvCol_CardingDetails.ACTUAL_PRODUCTION).Value = Format(Val(dt1.Rows(0).Item("Total_Carding_Actual_Production").ToString), "########0.000")
                    .Rows(0).Cells(DgvCol_CardingDetails.ACTUAL_EFFICIENCY).Value = Format(Val(dt1.Rows(0).Item("Total_Carding_Avg_Efficiency").ToString), "########0.00")
                End With

                '---------------------------

                da3 = New SqlClient.SqlDataAdapter("select * from OE_Drawing_Production_Details a  where a.OE_Production_Code = '" & Trim(NewCode) & "' Order by Sl_No", con)
                dt3 = New DataTable
                da3.Fill(dt3)



                With Dgv_DrawingDetails

                    .Rows.Clear()
                    SNo = 0

                    If dt3.Rows.Count > 0 Then

                        For i = 0 To dt3.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1
                            .Rows(n).Cells(DgvCol_DrawingDetails.SL_NO).Value = Val(SNo)
                            .Rows(n).Cells(DgvCol_DrawingDetails.MACHINE_NO).Value = dt3.Rows(i).Item("Machine_No").ToString
                            ' .Rows(n).Cells(DgvCol_DrawingDetails.MACHINE_NO).Value = Common_Procedures.OE_Machine_IdNoToName(con, Val(dt3.Rows(i).Item("Machine_iDNO").ToString))
                            .Rows(n).Cells(DgvCol_DrawingDetails.COUNT_HANK).Value = Val(dt3.Rows(i).Item("Count_Hank").ToString)
                            .Rows(n).Cells(DgvCol_DrawingDetails.SPEED).Value = Val(dt3.Rows(i).Item("Speed").ToString)
                            .Rows(n).Cells(DgvCol_DrawingDetails.TARGET_PRODUCTIONS).Value = Format(Val(dt3.Rows(i).Item("Target_Production").ToString), "#########0.000")
                            .Rows(n).Cells(DgvCol_DrawingDetails.ACTUAL_PRODUCTION).Value = Format(Val(dt3.Rows(i).Item("Actual_Production").ToString), "#########0.000")
                            .Rows(n).Cells(DgvCol_DrawingDetails.ACTUAL_EFFICIENCY).Value = Format(Val(dt3.Rows(i).Item("Actual_Efficiency").ToString), "#########0.00")


                        Next i


                    End If

                End With

                With Dgv_DrawingDetails_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(DgvCol_DrawingDetails.MACHINE_NO).Value = Format(Val(dt1.Rows(0).Item("Total_Drawing_Noof_Machine").ToString), "########0")
                    .Rows(0).Cells(DgvCol_DrawingDetails.COUNT_HANK).Value = Format(Val(dt1.Rows(0).Item("Total_Drawing_Hank").ToString), "########0.000")
                    .Rows(0).Cells(DgvCol_DrawingDetails.SPEED).Value = Format(Val(dt1.Rows(0).Item("Total_Drawing_Speed").ToString), "########0.000")
                    .Rows(0).Cells(DgvCol_DrawingDetails.TARGET_PRODUCTIONS).Value = Format(Val(dt1.Rows(0).Item("Total_Drawing_Target_Production").ToString), "########0.000")
                    .Rows(0).Cells(DgvCol_DrawingDetails.ACTUAL_PRODUCTION).Value = Format(Val(dt1.Rows(0).Item("Total_Drawing_Actual_Production").ToString), "########0.000")
                    .Rows(0).Cells(DgvCol_DrawingDetails.ACTUAL_EFFICIENCY).Value = Format(Val(dt1.Rows(0).Item("Total_Drawing_Avg_Efficiency").ToString), "########0.00")
                End With

                '--------------------



                da4 = New SqlClient.SqlDataAdapter("select a.*,ch.Count_Name from OE_Vortex_Production_Details a  LEFT OUTER JOIN Count_HEad ch ON a.Count_Idno= ch.Count_Idno  where a.OE_Production_Code = '" & Trim(NewCode) & "' Order by Sl_No", con)
                dt4 = New DataTable
                da4.Fill(dt4)



                With Dgv_VortexDetails

                    .Rows.Clear()
                    SNo = 0

                    If dt4.Rows.Count > 0 Then

                        For i = 0 To dt4.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1
                            .Rows(n).Cells(DgvCol_VortexDetails.SL_NO).Value = Val(SNo)
                            .Rows(n).Cells(DgvCol_VortexDetails.COUNT_NAME).Value = dt4.Rows(i).Item("Count_Name").ToString
                            ' .Rows(n).Cells(DgvCol_VortexDetails.COUNT_NAME).Value = Common_Procedures.Count_IdNoToName(con, Val(dt4.Rows(i).Item("Count_idno").ToString))
                            .Rows(n).Cells(DgvCol_VortexDetails.SPEED).Value = Val(dt4.Rows(i).Item("Speed").ToString)
                            .Rows(n).Cells(DgvCol_VortexDetails.TARGET_PRODUCTIONS).Value = Format(Val(dt4.Rows(i).Item("Target_Production").ToString), "#########0.000")
                            .Rows(n).Cells(DgvCol_VortexDetails.ACTUAL_PRODUCTION).Value = Format(Val(dt4.Rows(i).Item("Actual_Production").ToString), "#########0.000")
                            .Rows(n).Cells(DgvCol_VortexDetails.ACTUAL_EFFICIENCY).Value = Format(Val(dt4.Rows(i).Item("Actual_Efficiency").ToString), "#########0.00")
                            .Rows(n).Cells(DgvCol_VortexDetails.STOP_MIN).Value = Format(Val(dt4.Rows(i).Item("Stop_Minute").ToString), "#########0.00")


                        Next i


                    End If

                End With

                With Dgv_VortexDetails_Total
                    If .RowCount = 0 Then .Rows.Add()

                    .Rows(0).Cells(DgvCol_VortexDetails.SPEED).Value = Format(Val(dt1.Rows(0).Item("Total_Vortex_Speed").ToString), "########0.000")
                    .Rows(0).Cells(DgvCol_VortexDetails.TARGET_PRODUCTIONS).Value = Format(Val(dt1.Rows(0).Item("Total_Vortex_Target_Production").ToString), "########0.000")
                    .Rows(0).Cells(DgvCol_VortexDetails.ACTUAL_PRODUCTION).Value = Format(Val(dt1.Rows(0).Item("Total_Vortex_Actual_Production").ToString), "########0.000")
                    .Rows(0).Cells(DgvCol_VortexDetails.ACTUAL_EFFICIENCY).Value = Format(Val(dt1.Rows(0).Item("Total_Vortex_Avg_Efficiency").ToString), "########0.00")
                    .Rows(0).Cells(DgvCol_VortexDetails.STOP_MIN).Value = Format(Val(dt1.Rows(0).Item("Total_Vortex_StopMinute").ToString), "########0.00")

                End With

                '--------------

                da5 = New SqlClient.SqlDataAdapter("select a.*,wh.Variety_Name from OE_Waste_Production_Details a  LEFT OUTER JOIN Variety_Head wh ON a.Variety_Idno= wh.Variety_Idno where a.OE_Production_Code = '" & Trim(NewCode) & "' Order by Sl_No", con)
                dt5 = New DataTable
                da5.Fill(dt5)



                With Dgv_WasteDetails

                    .Rows.Clear()
                    SNo = 0

                    If dt5.Rows.Count > 0 Then

                        For i = 0 To dt5.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1
                            .Rows(n).Cells(DgvCol_WasteDetails.SL_NO).Value = Val(SNo)
                            .Rows(n).Cells(DgvCol_WasteDetails.WASTE_NAME).Value = dt5.Rows(i).Item("Variety_Name").ToString
                            ' .Rows(n).Cells(DgvCol_WasteDetails.WASTE_NAME).Value = Common_Procedures.Waste_IdNoToName(con, Val(dt5.Rows(i).Item("waste_idno").ToString))
                            .Rows(n).Cells(DgvCol_WasteDetails.WEIGHT).Value = Format(Val(dt5.Rows(i).Item("waste_weight").ToString), "#########0.000")
                            .Rows(n).Cells(DgvCol_WasteDetails.WASTE_PERCENTAGE).Value = Format(Val(dt5.Rows(i).Item("waste_Percentage").ToString), "#########0.00")


                        Next i


                    End If

                End With

                With Dgv_VortexDetails_Total
                    If .RowCount = 0 Then .Rows.Add()

                    .Rows(0).Cells(DgvCol_WasteDetails.WEIGHT).Value = Format(Val(dt1.Rows(0).Item("Total_Waste_Weight").ToString), "########0.000")
                    .Rows(0).Cells(DgvCol_WasteDetails.WASTE_PERCENTAGE).Value = Format(Val(dt1.Rows(0).Item("Total_Waste_percentage").ToString), "########0.00")

                End With


                Grid_DeSelect()

                dt2.Clear()


                dt2.Dispose()
                da2.Dispose()

            End If
            dgv_ActCtrlName = ""
            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

    End Sub

    Private Sub Spinning_Production_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Try


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Shift.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "FINISHEDPRODUCT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Shift.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_WasteGrid_WasteName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "WASTE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                Cbo_WasteGrid_WasteName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Vortex_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Vortex_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Carding_Production_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
        AddHandler cbo_Shift.GotFocus, AddressOf ControlGotFocus


        AddHandler cbo_Filter_Shift.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_Employee.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Count.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Shift.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Shift.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Employee.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Count.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler Cbo_WasteGrid_WasteName.GotFocus, AddressOf ControlGotFocus


        AddHandler Cbo_WasteGrid_WasteName.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Grid_Vortex_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Vortex_CountName.LostFocus, AddressOf ControlLostFocus


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Carding_Production_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Spinning_Production_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView
        Dim i As Integer

        If ActiveControl.Name = dgv_CardingDetails.Name Or ActiveControl.Name = Dgv_DrawingDetails.Name Or ActiveControl.Name = Dgv_VortexDetails.Name Or ActiveControl.Name = Dgv_WasteDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_CardingDetails.Name Then
                dgv1 = dgv_CardingDetails

            ElseIf dgv_CardingDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_CardingDetails

            ElseIf dgv_ActCtrlName = dgv_CardingDetails.Name Then
                dgv1 = dgv_CardingDetails

            ElseIf ActiveControl.Name = Dgv_DrawingDetails.Name Then
                dgv1 = Dgv_DrawingDetails

            ElseIf Dgv_DrawingDetails.IsCurrentRowDirty = True Then
                dgv1 = Dgv_DrawingDetails

            ElseIf dgv_ActCtrlName = Dgv_DrawingDetails.Name Then
                dgv1 = Dgv_DrawingDetails

            ElseIf ActiveControl.Name = Dgv_VortexDetails.Name Then
                dgv1 = Dgv_VortexDetails

            ElseIf dgv_ActCtrlName = Dgv_VortexDetails.Name Then
                dgv1 = Dgv_VortexDetails

            ElseIf ActiveControl.Name = Dgv_WasteDetails.Name Then
                dgv1 = Dgv_WasteDetails

            ElseIf dgv_ActCtrlName = Dgv_WasteDetails.Name Then
                dgv1 = Dgv_WasteDetails

            End If

            If IsNothing(dgv1) = True Then
                Return MyBase.ProcessCmdKey(msg, keyData)
                Exit Function
            End If

            With dgv1

                If dgv1.Name = dgv_CardingDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then


                        If .CurrentCell.ColumnIndex >= DgvCol_CardingDetails.ACTUAL_EFFICIENCY Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then

                                If Dgv_DrawingDetails.RowCount > 0 Then

                                    Dgv_DrawingDetails.Focus()
                                    Dgv_DrawingDetails.CurrentCell = Dgv_DrawingDetails.Rows(0).Cells(DgvCol_CardingDetails.ACTUAL_PRODUCTION)
                                    Dgv_DrawingDetails.CurrentCell.Selected = True
                                    .CurrentCell.Selected = False

                                Else
                                    cbo_Shift.Focus()

                                End If
                            Else
                                '   .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                                .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(DgvCol_CardingDetails.ACTUAL_PRODUCTION)
                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)


                        End If


                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= DgvCol_CardingDetails.ACTUAL_PRODUCTION Then
                            If .CurrentCell.RowIndex = 0 And .CurrentCell.ColumnIndex = DgvCol_CardingDetails.ACTUAL_PRODUCTION Then
                                cbo_Shift.Focus()
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(DgvCol_CardingDetails.ACTUAL_EFFICIENCY)
                            End If
                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                        End If

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If
                ElseIf dgv1.Name = Dgv_DrawingDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then


                        If .CurrentCell.ColumnIndex >= DgvCol_DrawingDetails.ACTUAL_EFFICIENCY Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then

                                If Dgv_VortexDetails.RowCount > 0 Then

                                    Dgv_VortexDetails.Focus()
                                    Dgv_VortexDetails.CurrentCell = Dgv_VortexDetails.Rows(0).Cells(DgvCol_VortexDetails.ACTUAL_PRODUCTION)
                                    Dgv_VortexDetails.CurrentCell.Selected = True
                                    .CurrentCell.Selected = False

                                Else
                                    cbo_Shift.Focus()

                                End If
                            Else
                                '   .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                                .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(DgvCol_DrawingDetails.ACTUAL_PRODUCTION)
                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)


                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= DgvCol_DrawingDetails.ACTUAL_PRODUCTION Then
                            If .CurrentCell.RowIndex = 0 And .CurrentCell.ColumnIndex = DgvCol_DrawingDetails.ACTUAL_PRODUCTION Then
                                'cbo_Shift.Focus()
                                dgv_CardingDetails.Focus()
                                dgv_CardingDetails.CurrentCell = dgv_CardingDetails.Rows(0).Cells(DgvCol_CardingDetails.ACTUAL_EFFICIENCY)
                                dgv_CardingDetails.CurrentCell.Selected = True
                                .CurrentCell.Selected = False

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(DgvCol_DrawingDetails.ACTUAL_EFFICIENCY)
                            End If
                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                        End If



                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If

                ElseIf dgv1.Name = Dgv_VortexDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then


                        If .CurrentCell.ColumnIndex >= DgvCol_VortexDetails.STOP_MIN Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then

                                If Dgv_VortexDetails.RowCount > 0 Then

                                    Dgv_WasteDetails.Focus()
                                    Dgv_WasteDetails.CurrentCell = Dgv_WasteDetails.Rows(0).Cells(DgvCol_WasteDetails.WASTE_NAME)
                                    Dgv_WasteDetails.CurrentCell.Selected = True
                                    Cbo_WasteGrid_WasteName.Focus()
                                    .CurrentCell.Selected = False

                                Else

                                    cbo_Shift.Focus()

                                End If
                            Else

                                .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(DgvCol_VortexDetails.ACTUAL_PRODUCTION)
                            End If                                '


                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                        End If


                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= DgvCol_VortexDetails.ACTUAL_PRODUCTION Then
                            If .CurrentCell.RowIndex = 0 And .CurrentCell.ColumnIndex = DgvCol_VortexDetails.ACTUAL_PRODUCTION Then
                                'cbo_Shift.Focus()
                                Dgv_DrawingDetails.Focus()
                                Dgv_DrawingDetails.CurrentCell = Dgv_DrawingDetails.Rows(0).Cells(DgvCol_DrawingDetails.ACTUAL_EFFICIENCY)
                                Dgv_DrawingDetails.CurrentCell.Selected = True
                                .CurrentCell.Selected = False

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(DgvCol_VortexDetails.STOP_MIN)
                            End If
                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                        End If

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If

                ElseIf dgv1.Name = Dgv_WasteDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then


                        If .CurrentCell.ColumnIndex >= DgvCol_WasteDetails.WASTE_PERCENTAGE Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then

                                cbo_Shift.Focus()
                            Else

                                .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(DgvCol_WasteDetails.WASTE_NAME)
                            End If                                '
                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                            '.CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(DgvCol_WasteDetails.WASTE_NAME)
                        End If


                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= DgvCol_WasteDetails.WASTE_NAME Then

                            If .CurrentCell.RowIndex = 0 And .CurrentCell.ColumnIndex = DgvCol_WasteDetails.WASTE_NAME Then

                                Dgv_VortexDetails.Focus()
                                Dgv_VortexDetails.CurrentCell = Dgv_VortexDetails.Rows(0).Cells(DgvCol_VortexDetails.STOP_MIN)
                                Dgv_VortexDetails.CurrentCell.Selected = True
                                .CurrentCell.Selected = False
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(DgvCol_WasteDetails.WASTE_PERCENTAGE)

                            End If
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

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Spinning_Production_Entry_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Spinning_Production_Entry_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Qa = MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If Qa = Windows.Forms.DialogResult.No Or Qa = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If



        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "Delete from Stock_Waste_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from OE_Waste_Production_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and OE_Production_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from OE_Vortex_Production_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and OE_Production_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from OE_Drawing_Production_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and OE_Production_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from OE_Carding_Production_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and OE_Production_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from OE_Vortex_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and OE_Production_Code = '" & Trim(NewCode) & "'"
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

        'If Filter_Status = False Then



        '    dtp_Filter_Fromdate.Text = ""
        '    dtp_Filter_ToDate.Text = ""
        '    cbo_Filter_Count.Text = ""
        '    cbo_Filter_Employee.Text = ""
        '    cbo_Filter_Shift.Text = ""
        '    cbo_Filter_Count.SelectedIndex = -1
        '    cbo_Filter_Employee.SelectedIndex = -1
        '    cbo_Filter_Shift.SelectedIndex = -1
        '    dgv_Filter_Details.Rows.Clear()

        'End If

        'pnl_Filter.Visible = True
        'pnl_Filter.Enabled = True
        'pnl_Filter.BringToFront()
        'pnl_Back.Enabled = False
        'If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()
        Exit Sub


    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 OE_Production_No from OE_Vortex_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and OE_Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, OE_Production_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 OE_Production_No from OE_Vortex_Production_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and OE_Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, OE_Production_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 OE_Production_No from OE_Vortex_Production_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and OE_Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, OE_Production_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 OE_Production_No from OE_Vortex_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and OE_Production_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, OE_Production_No desc", con)
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

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "OE_Vortex_Production_Head", "OE_Production_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

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

            Da = New SqlClient.SqlDataAdapter("select OE_Production_No from OE_Vortex_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and OE_Production_Code = '" & Trim(RecCode) & "'", con)
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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Spinning_Production_Entry_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Spinning_Production_Entry_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW REF INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select OE_Production_No from OE_Vortex_Production_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and OE_Production_Code = '" & Trim(RecCode) & "'", con)
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
        Dim Emp_ID As Integer = 0
        Dim Sup_ID As Integer = 0
        Dim Sht_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim SlNo As Integer = 0
        Dim Partcls As String = ""
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Proc_ID As Integer = 0
        Dim Stp_ID As Integer = 0
        Dim Cnt_ID As Integer = 0
        Dim WagesCode As String = ""
        Dim PcsChkCode As String = ""
        Dim Nr As Integer = 0
        Dim Sht_Nm As String

        Dim vTot_carding_NoofMachine As Single, vTot_carding_Hank As Single, vTot_carding_Speed As Single
        Dim vTot_carding_TargPrd As Single, vTot_carding_ActPrd As Single, vTot_carding_ActEff As Single

        Dim vTot_Drawing_NoofMachine As Single, vTot_Drawing_Hank As Single, vTot_Drawing_Speed As Single
        Dim vTot_Drawing_TargPrd As Single, vTot_Drawing_ActPrd As Single, vTot_Drawing_ActEff As Single

        Dim vTot_Vortex_Speed As Single, vTot_Vortex_StopMin As Single
        Dim vTot_Vortex_TargPrd As Single, vTot_Vortex_ActPrd As Single, vTot_Vortex_ActEff As Single

        Dim vTot_Waste_weight As Single, vTot_Waste_Precn As Single

        Dim Waste_ID As Integer = 0
        Dim Shift_ID As Integer = 0

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Spinning_Production_Entry_Entry, New_Entry) = False Then Exit Sub

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



        Sht_ID = Common_Procedures.Shift_NameToIdNo(con, cbo_Shift.Text)


        If Sht_ID = 0 Then
            MessageBox.Show("Invalid Shift", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Shift.Enabled And cbo_Shift.Visible Then cbo_Shift.Focus()
            Exit Sub
        End If
        With dgv_CardingDetails
            For i = 0 To dgv_CardingDetails.RowCount - 1
                If Trim(.Rows(i).Cells(DgvCol_CardingDetails.MACHINE_NO).Value) <> "" And Trim(.Rows(i).Cells(DgvCol_CardingDetails.COUNT_HANK).Value) <> "" Then

                    If Trim(dgv_CardingDetails.Rows(i).Cells(DgvCol_CardingDetails.MACHINE_NO).Value) = "" Then
                        MessageBox.Show("Invalid Machine No..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_CardingDetails.Enabled Then
                            dgv_CardingDetails.Focus()
                            dgv_CardingDetails.CurrentCell = dgv_CardingDetails.Rows(0).Cells(DgvCol_CardingDetails.MACHINE_NO)
                        End If

                        Exit Sub
                    End If

                    If Trim(dgv_CardingDetails.Rows(i).Cells(DgvCol_CardingDetails.COUNT_HANK).Value) = "" Then
                        MessageBox.Show("Invalid Count Hank", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_CardingDetails.Enabled And dgv_CardingDetails.Visible Then
                            dgv_CardingDetails.Focus()
                            dgv_CardingDetails.CurrentCell = dgv_CardingDetails.Rows(i).Cells(DgvCol_CardingDetails.COUNT_HANK)

                        End If
                        Exit Sub

                    End If

                End If

            Next
        End With

        Total_Calculation()

        With dgv_CardingDetails_Total

            vTot_carding_NoofMachine = 0 : vTot_carding_Hank = 0 : vTot_carding_Speed = 0 : vTot_carding_TargPrd = 0 : vTot_carding_ActPrd = 0 : vTot_carding_ActEff = 0

            If .RowCount > 0 Then

                vTot_carding_NoofMachine = Val(.Rows(0).Cells(DgvCol_CardingDetails.MACHINE_NO).Value())
                vTot_carding_Hank = Val(.Rows(0).Cells(DgvCol_CardingDetails.COUNT_HANK).Value())
                vTot_carding_Speed = Val(.Rows(0).Cells(DgvCol_CardingDetails.SPEED).Value())
                vTot_carding_TargPrd = Val(.Rows(0).Cells(DgvCol_CardingDetails.TARGET_PRODUCTIONS).Value())
                vTot_carding_ActPrd = Val(.Rows(0).Cells(DgvCol_CardingDetails.ACTUAL_PRODUCTION).Value())
                vTot_carding_ActEff = Val(.Rows(0).Cells(DgvCol_CardingDetails.ACTUAL_EFFICIENCY).Value())
            End If

        End With

        With Dgv_DrawingDetails_Total

            vTot_Drawing_NoofMachine = 0 : vTot_Drawing_Hank = 0 : vTot_Drawing_Speed = 0 : vTot_Drawing_TargPrd = 0 : vTot_Drawing_ActPrd = 0 : vTot_Drawing_ActEff = 0

            If .RowCount > 0 Then

                vTot_Drawing_NoofMachine = Val(dgv_CardingDetails_Total.Rows(0).Cells(DgvCol_DrawingDetails.MACHINE_NO).Value())
                vTot_Drawing_Hank = Val(.Rows(0).Cells(DgvCol_CardingDetails.COUNT_HANK).Value())
                vTot_Drawing_Speed = Val(.Rows(0).Cells(DgvCol_DrawingDetails.SPEED).Value())
                vTot_Drawing_TargPrd = Val(.Rows(0).Cells(DgvCol_DrawingDetails.TARGET_PRODUCTIONS).Value())
                vTot_Drawing_ActPrd = Val(.Rows(0).Cells(DgvCol_DrawingDetails.ACTUAL_PRODUCTION).Value())
                vTot_Drawing_ActEff = Val(.Rows(0).Cells(DgvCol_DrawingDetails.ACTUAL_EFFICIENCY).Value())
            End If

        End With


        With Dgv_VortexDetails_Total

            vTot_Vortex_Speed = 0 : vTot_Vortex_ActPrd = 0 : vTot_Vortex_TargPrd = 0 : vTot_Vortex_ActEff = 0 : vTot_Vortex_StopMin = 0

            If .RowCount > 0 Then

                vTot_Vortex_Speed = Val(.Rows(0).Cells(DgvCol_VortexDetails.SPEED).Value())
                vTot_Vortex_TargPrd = Val(.Rows(0).Cells(DgvCol_VortexDetails.TARGET_PRODUCTIONS).Value())
                vTot_Vortex_ActPrd = Val(.Rows(0).Cells(DgvCol_VortexDetails.ACTUAL_PRODUCTION).Value())
                vTot_Vortex_ActEff = Val(.Rows(0).Cells(DgvCol_VortexDetails.ACTUAL_EFFICIENCY).Value())
                vTot_Vortex_StopMin = Val(.Rows(0).Cells(DgvCol_VortexDetails.STOP_MIN).Value())

            End If

        End With

        With Dgv_WasteDetails_Total

            vTot_Waste_weight = 0 : vTot_Waste_Precn = 0

            If .RowCount > 0 Then

                vTot_Waste_weight = Val(.Rows(0).Cells(DgvCol_WasteDetails.WEIGHT).Value())
                vTot_Waste_Precn = Val(.Rows(0).Cells(DgvCol_WasteDetails.WASTE_PERCENTAGE).Value())

            End If

        End With
        Dt1.Clear()

        tr = con.BeginTransaction


        Try


            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "OE_Vortex_Production_Head", "OE_Production_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@CrdDate", dtp_Date.Value.Date)



            If New_Entry = True Then


                cmd.CommandText = "Insert into OE_Vortex_Production_Head (  [OE_Production_Code] ,         [Company_IdNo]           ,      [OE_Production_No]       ,                       [for_OrderBy]                                    ,  [OE_Production_Date]    ,   [Shift_IdNo]       ,         [Total_carding_Noof_Machine]    ,      [Total_Carding_Hank]          ,         [Total_Carding_Speed]        , [Total_Carding_Target_Production]    , [Total_Carding_Actual_Production]     , [Total_Carding_Avg_Efficiency]   ,       [Total_Drawing_Noof_Machine]      ,       [Total_Drawing_Hank]         ,     [Total_Drawing_Speed]       ,   [Total_Drawing_Target_Production]  , [Total_Drawing_Actual_Production]  , [Total_Drawing_Avg_Efficiency]      , [Total_Vortex_Speed]    , [Total_Vortex_Target_Production] , [Total_Vortex_Actual_Production] , [Total_Vortex_Avg_Efficiency] ,  [Total_Vortex_StopMinute]      ,       [Total_Waste_Weight]         , [Total_Waste_Percentage]         )  " &
                                                               " Values  ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",           @CrdDate       , " & Val(Sht_ID) & "  ,   " & Val(vTot_carding_NoofMachine) & " , " & Str(Val(vTot_carding_Hank)) & "," & Str(Val(vTot_carding_Speed)) & ",  " & Val(vTot_carding_TargPrd) & "     ," & Val(vTot_carding_ActPrd) & "       ," & Val(vTot_carding_ActEff) & "  ,   " & Val(vTot_Drawing_NoofMachine) & " ," & Val(vTot_Drawing_NoofMachine) & "," & Val(vTot_Drawing_Speed) & "  ,  " & Val(vTot_Drawing_TargPrd) & "   ," & Val(vTot_Drawing_ActPrd) & "    , " & Val(vTot_Drawing_ActEff) & ",  " & Val(vTot_Vortex_Speed) & "," & Val(vTot_Vortex_TargPrd) & "  , " & Val(vTot_Vortex_ActPrd) & "," & Val(vTot_Vortex_ActEff) & "  ," & Val(vTot_Vortex_StopMin) & " , " & Val(vTot_Waste_weight) & " , " & Val(vTot_Waste_Precn) & ")"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update OE_Vortex_Production_Head set OE_Production_Date = @CrdDate,Shift_IdNo = " & Val(Sht_ID) & ", Total_carding_Noof_Machine= " & Val(vTot_carding_NoofMachine) & " ,Total_Carding_Hank= " & Str(Val(vTot_carding_Hank)) & ",Total_Carding_Speed= " & Str(Val(vTot_carding_Speed)) & " , Total_Carding_Target_Production = " & Val(vTot_carding_TargPrd) & " , Total_Carding_Actual_Production = " & Val(vTot_carding_ActPrd) & " ,Total_Carding_Avg_Efficiency=" & Val(vTot_carding_ActEff) & " ,  Total_Drawing_Noof_Machine =" & Val(vTot_Drawing_NoofMachine) & "  , Total_Drawing_Hank =" & Val(vTot_Drawing_NoofMachine) & " , Total_Drawing_Speed=" & Val(vTot_Drawing_Speed) & "  ,Total_Drawing_Target_Production =" & Val(vTot_Drawing_TargPrd) & " ,Total_Drawing_Actual_Production =" & Val(vTot_Drawing_ActPrd) & ",Total_Drawing_Avg_Efficiency =" & Val(vTot_Drawing_ActEff) & " ,Total_Vortex_Speed =" & Val(vTot_Vortex_Speed) & " , Total_Vortex_Target_Production =" & Val(vTot_Vortex_TargPrd) & " ,Total_Vortex_Actual_Production=" & Val(vTot_Vortex_ActPrd) & ",Total_Vortex_Avg_Efficiency =" & Val(vTot_Vortex_ActEff) & ", Total_Vortex_StopMinute=" & Val(vTot_Vortex_StopMin) & " , Total_Waste_Weight=    " & Val(vTot_Waste_weight) & " , Total_Waste_Percentage =" & Val(vTot_Waste_Precn) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and OE_Production_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()


            End If



            cmd.CommandText = "Delete from OE_Carding_Production_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and OE_Production_Code = '" & Trim(NewCode) & "' "
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from OE_Drawing_Production_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and OE_Production_Code = '" & Trim(NewCode) & "' "
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from OE_Vortex_Production_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and OE_Production_Code = '" & Trim(NewCode) & "' "
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from OE_Waste_Production_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and OE_Production_Code = '" & Trim(NewCode) & "' "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Waste_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()



            Partcls = "Ref : Ref.No. " & Trim(lbl_RefNo.Text)
            PBlNo = Trim(lbl_RefNo.Text)
            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)



            With dgv_CardingDetails
                Sno = 0
                SlNo = 0
                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(DgvCol_CardingDetails.MACHINE_NO).Value) <> "" And Trim(.Rows(i).Cells(DgvCol_CardingDetails.COUNT_HANK).Value) <> "" Then
                        Sno = Sno + 1
                        '               
                        cmd.CommandText = "Insert into OE_Carding_Production_Details (  [OE_Production_Code] ,         [Company_IdNo]           ,      [OE_Production_No]       ,                       [for_OrderBy]                                     ,  [OE_Production_Date]    ,         [Shift_IdNo]        ,       [Sl_No]         ,                            [Machine_No]                                 ,                                  [Count_Hank]                           ,                               [Speed]                               ,                                    [Target_Production]                           ,                         [Actual_Production]                                      ,                              [Actual_Efficiency]                              ,[Department_IdNo]   )  " &
                                                                            " Values  ('" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",     @CrdDate           ," & Str(Val(Sht_ID)) & "    ," & Str(Val(Sno)) & "  , '" & Trim(.Rows(i).Cells(DgvCol_CardingDetails.MACHINE_NO).Value) & "'  ," & Str(Val(.Rows(i).Cells(DgvCol_CardingDetails.COUNT_HANK).Value)) & " , " & Str(Val(.Rows(i).Cells(DgvCol_CardingDetails.SPEED).Value)) & " , " & Str(Val(.Rows(i).Cells(DgvCol_CardingDetails.TARGET_PRODUCTIONS).Value)) & " , " & Str(Val(.Rows(i).Cells(DgvCol_CardingDetails.ACTUAL_PRODUCTION).Value)) & "  , " & Str(Val(.Rows(i).Cells(DgvCol_CardingDetails.ACTUAL_EFFICIENCY).Value)) & " ,         '1'          )"
                        cmd.ExecuteNonQuery()

                    End If
                Next

            End With


            With Dgv_DrawingDetails
                Sno = 0
                SlNo = 0
                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(DgvCol_DrawingDetails.MACHINE_NO).Value) <> "" And Trim(.Rows(i).Cells(DgvCol_DrawingDetails.COUNT_HANK).Value) <> "" Then
                        Sno = Sno + 1

                        cmd.CommandText = "Insert into OE_Drawing_Production_Details (  [OE_Production_Code] ,         [Company_IdNo]           ,      [OE_Production_No]       ,                       [for_OrderBy]                                     ,  [OE_Production_Date]    ,         [Shift_IdNo]        ,       [Sl_No]         ,                               [Machine_No]                          ,                                  [Count_Hank]                           ,                               [Speed]                               ,                                    [Target_Production]                           ,                         [Actual_Production]                                      ,                              [Actual_Efficiency]                              ,[Department_IdNo]   )  " &
                                                                            " Values  ('" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",     @CrdDate           ," & Str(Val(Sht_ID)) & "    ," & Str(Val(Sno)) & "  , '" & Trim(.Rows(i).Cells(DgvCol_CardingDetails.MACHINE_NO).Value) & "'," & Str(Val(.Rows(i).Cells(DgvCol_DrawingDetails.COUNT_HANK).Value)) & " , " & Str(Val(.Rows(i).Cells(DgvCol_DrawingDetails.SPEED).Value)) & " , " & Str(Val(.Rows(i).Cells(DgvCol_DrawingDetails.TARGET_PRODUCTIONS).Value)) & " , " & Str(Val(.Rows(i).Cells(DgvCol_DrawingDetails.ACTUAL_PRODUCTION).Value)) & "  , " & Str(Val(.Rows(i).Cells(DgvCol_DrawingDetails.ACTUAL_EFFICIENCY).Value)) & " ,      '2'           )"
                        cmd.ExecuteNonQuery()

                    End If
                Next

            End With

            With Dgv_VortexDetails

                Sno = 0
                SlNo = 0
                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(DgvCol_VortexDetails.COUNT_NAME).Value) <> "" Then
                        Sno = Sno + 1

                        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(DgvCol_VortexDetails.COUNT_NAME).Value, tr)


                        cmd.CommandText = "Insert into OE_Vortex_Production_Details (  [OE_Production_Code] ,         [Company_IdNo]           ,      [OE_Production_No]       ,                       [for_OrderBy]                                     ,  [OE_Production_Date]    ,         [Shift_IdNo]        ,       [Sl_No]          ,  [Count_IdNo]       ,                                    [Speed]                       ,                             [Target_Production]                     ,                                           [Actual_Production]                                ,                              [Actual_Efficiency]                               ,                                        [Stop_Minute]                   ,[Department_IdNo])  " &
                                                                            " Values  ('" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",     @CrdDate            ," & Str(Val(Sht_ID)) & "     ," & Str(Val(Sno)) & "   , " & Val(Cnt_ID) & " ," & Str(Val(.Rows(i).Cells(DgvCol_VortexDetails.SPEED).Value)) & " , " & Str(Val(.Rows(i).Cells(DgvCol_VortexDetails.TARGET_PRODUCTIONS).Value)) & " , " & Str(Val(.Rows(i).Cells(DgvCol_VortexDetails.ACTUAL_PRODUCTION).Value)) & " , " & Str(Val(.Rows(i).Cells(DgvCol_VortexDetails.ACTUAL_EFFICIENCY).Value)) & "  , " & Str(Val(.Rows(i).Cells(DgvCol_VortexDetails.STOP_MIN).Value)) & " ,        '3'        )"
                        cmd.ExecuteNonQuery()

                    End If
                Next

            End With

            With Dgv_WasteDetails

                Sno = 0
                SlNo = 0
                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(DgvCol_WasteDetails.WASTE_NAME).Value) <> "" Then
                        Sno = Sno + 1

                        Waste_ID = Common_Procedures.Variety_NameToIdNo(con, .Rows(i).Cells(DgvCol_WasteDetails.WASTE_NAME).Value, tr)


                        cmd.CommandText = "Insert into OE_Waste_Production_Details (  [OE_Production_Code] ,         [Company_IdNo]           ,      [OE_Production_No]       ,                       [for_OrderBy]                                     ,  [OE_Production_Date]    ,         [Shift_IdNo]        ,       [Sl_No]          ,       [Variety_IdNo]   ,                           [waste_weight]                           ,                                [waste_Percentage]                               )  " &
                                                                            " Values  ('" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",     @CrdDate            ," & Str(Val(Sht_ID)) & "     ," & Str(Val(Sno)) & "   , " & Val(Waste_ID) & " ," & Str(Val(.Rows(i).Cells(DgvCol_WasteDetails.WEIGHT).Value)) & " , " & Str(Val(.Rows(i).Cells(DgvCol_WasteDetails.WASTE_PERCENTAGE).Value)) & "  )"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Insert into Stock_Waste_Processing_Details (             SoftwareType_IdNo                  ,             Reference_Code                   ,             Company_IdNo         ,           Reference_No        ,                           For_OrderBy                                   , Reference_Date ,     Party_Bill_No    ,         Entry_ID        ,     Sl_No            ,     Variety_IdNo      ,     Lot_No      ,                                      Weight             ) " &
                                                           "   Values  (" & Str(Val(Common_Procedures.SoftwareTypes.OE_Software)) & " , '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & " , '" & Trim(lbl_RefNo.Text) & "' , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",      @CrdDate   , '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "' , " & Str(Val(Sno)) & ", " & Val(Waste_ID) & " ,     ''          ,   " & Str(Val(.Rows(i).Cells(DgvCol_WasteDetails.WEIGHT).Value)) & "  )"
                        cmd.ExecuteNonQuery()

                    End If
                Next

            End With


            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()




            MessageBox.Show("Saved Sucessfully!!!", "For SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

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
            If InStr(1, LCase(Err.Description), "ix_OE_Vortex_Production_Head") > 0 Then
                MessageBox.Show("Duplicate entry For the shift In this Date", "For SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "For SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        End Try

        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()



    End Sub
    Private Sub Total_Calculation()


        Dim i As Integer
        Dim sno As Integer
        Dim Draw_Slno As Integer
        Dim vortex_Slno As Integer
        Dim Waste_Slno As Integer

        '------------------

        Dim Tot_carding_NoofMachine As Integer, Tot_carding_Hank As Single, Tot_carding_Speed As Single
        Dim Tot_carding_TargPrd As Single, Tot_carding_ActPrd As Single, Tot_carding_ActEff As Single

        Dim Tot_Drawing_NoofMachine As Integer, Tot_Drawing_Hank As Single, Tot_Drawing_Speed As Single
        Dim Tot_Drawing_TargPrd As Single, Tot_Drawing_ActPrd As Single, Tot_Drawing_ActEff As Single

        Dim Tot_Vortex_Speed As Single, Tot_Vortex_StopMin As Single
        Dim Tot_Vortex_TargPrd As Single, Tot_Vortex_ActPrd As Single, Tot_Vortex_ActEff As Single

        Dim Tot_Waste_weight As Single, Tot_Waste_Precn As Single





        Tot_Waste_weight = 0 : Tot_Waste_Precn = 0

        Tot_Vortex_Speed = 0 : Tot_Vortex_TargPrd = 0 : Tot_Vortex_ActPrd = 0 : Tot_Vortex_ActEff = 0 : Tot_Vortex_StopMin = 0

        Tot_Drawing_NoofMachine = 0 : Tot_Drawing_Hank = 0 : Tot_Drawing_Speed = 0 : Tot_Drawing_TargPrd = 0 : Tot_Drawing_ActPrd = 0 : Tot_Drawing_ActEff = 0

        Tot_carding_NoofMachine = 0 : Tot_carding_Hank = 0 : Tot_carding_Speed = 0 : Tot_carding_TargPrd = 0 : Tot_carding_ActPrd = 0 : Tot_carding_ActEff = 0

        '------------------

        With dgv_CardingDetails

            For i = 0 To dgv_CardingDetails.Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(DgvCol_CardingDetails.SL_NO).Value = sno

                If Val(.Rows(i).Cells(DgvCol_CardingDetails.SPEED).Value) <> 0 Or Val(.Rows(i).Cells(DgvCol_CardingDetails.TARGET_PRODUCTIONS).Value) <> 0 Then

                    Tot_carding_NoofMachine = Val(.Rows(i).Cells(DgvCol_CardingDetails.SL_NO).Value())
                    Tot_carding_Hank = Tot_carding_Hank + Val(.Rows(i).Cells(DgvCol_CardingDetails.COUNT_HANK).Value())
                    Tot_carding_Speed = Tot_carding_Speed + Val(.Rows(i).Cells(DgvCol_CardingDetails.SPEED).Value())
                    Tot_carding_TargPrd = Tot_carding_TargPrd + Val(.Rows(i).Cells(DgvCol_CardingDetails.TARGET_PRODUCTIONS).Value())
                    Tot_carding_ActPrd = Tot_carding_ActPrd + Val(.Rows(i).Cells(DgvCol_CardingDetails.ACTUAL_PRODUCTION).Value())
                    Tot_carding_ActEff = Tot_carding_ActEff + Val(.Rows(i).Cells(DgvCol_CardingDetails.ACTUAL_EFFICIENCY).Value())


                End If
            Next
        End With
        With dgv_CardingDetails_Total

            If dgv_CardingDetails_Total.Rows.Count <= 0 Then dgv_CardingDetails_Total.Rows.Add()

            .Rows(0).Cells(DgvCol_CardingDetails.MACHINE_NO).Value = Format(Val(Tot_carding_NoofMachine), "#########0")
            .Rows(0).Cells(DgvCol_CardingDetails.COUNT_HANK).Value = Format(Val(Tot_carding_Hank), "#########0.00")
            .Rows(0).Cells(DgvCol_CardingDetails.SPEED).Value = Format(Val(Tot_carding_Speed), "#########0.000")
            .Rows(0).Cells(DgvCol_CardingDetails.TARGET_PRODUCTIONS).Value = Format(Val(Tot_carding_TargPrd), "#########0.000")
            .Rows(0).Cells(DgvCol_CardingDetails.ACTUAL_PRODUCTION).Value = Format(Val(Tot_carding_ActPrd), "#########0.0")
            .Rows(0).Cells(DgvCol_CardingDetails.ACTUAL_EFFICIENCY).Value = Format(Val(Tot_carding_ActEff) / Val(sno), "#########0.00")

        End With

        '-----------------------------

        With Dgv_DrawingDetails

            For i = 0 To Dgv_DrawingDetails.Rows.Count - 1

                Draw_Slno = Draw_Slno + 1

                .Rows(i).Cells(DgvCol_DrawingDetails.SL_NO).Value = Draw_Slno

                If Val(.Rows(i).Cells(DgvCol_DrawingDetails.SPEED).Value) <> 0 Or Val(.Rows(i).Cells(DgvCol_DrawingDetails.TARGET_PRODUCTIONS).Value) <> 0 Then

                    Tot_Drawing_NoofMachine = Val(.Rows(i).Cells(DgvCol_DrawingDetails.SL_NO).Value())
                    Tot_Drawing_Hank = Tot_Drawing_Hank + Val(.Rows(i).Cells(DgvCol_DrawingDetails.COUNT_HANK).Value())
                    Tot_Drawing_Speed = Tot_Drawing_Speed + Val(.Rows(i).Cells(DgvCol_DrawingDetails.SPEED).Value())
                    Tot_Drawing_TargPrd = Tot_Drawing_TargPrd + Val(.Rows(i).Cells(DgvCol_DrawingDetails.TARGET_PRODUCTIONS).Value())
                    Tot_Drawing_ActPrd = Tot_Drawing_ActPrd + Val(.Rows(i).Cells(DgvCol_DrawingDetails.ACTUAL_PRODUCTION).Value())
                    Tot_Drawing_ActEff = Tot_Drawing_ActEff + Val(.Rows(i).Cells(DgvCol_DrawingDetails.ACTUAL_EFFICIENCY).Value())

                End If
            Next
        End With

        With Dgv_DrawingDetails_Total

            If Dgv_DrawingDetails_Total.Rows.Count <= 0 Then Dgv_DrawingDetails_Total.Rows.Add()

            .Rows(0).Cells(DgvCol_DrawingDetails.MACHINE_NO).Value = Format(Val(Tot_Drawing_NoofMachine), "#########0")
            .Rows(0).Cells(DgvCol_DrawingDetails.COUNT_HANK).Value = Format(Val(Tot_Drawing_Hank), "#########0.00")
            .Rows(0).Cells(DgvCol_DrawingDetails.SPEED).Value = Format(Val(Tot_Drawing_Speed), "#########0.000")
            .Rows(0).Cells(DgvCol_DrawingDetails.TARGET_PRODUCTIONS).Value = Format(Val(Tot_Drawing_TargPrd), "#########0.000")
            .Rows(0).Cells(DgvCol_DrawingDetails.ACTUAL_PRODUCTION).Value = Format(Val(Tot_Drawing_ActPrd), "#########0.0")
            .Rows(0).Cells(DgvCol_DrawingDetails.ACTUAL_EFFICIENCY).Value = Format(Val(Tot_Drawing_ActEff) / Val(Draw_Slno), "#########0.00")

        End With

        '-------------------

        With Dgv_VortexDetails

            For i = 0 To Dgv_VortexDetails.Rows.Count - 1

                vortex_Slno = vortex_Slno + 1

                .Rows(i).Cells(DgvCol_VortexDetails.SL_NO).Value = vortex_Slno

                If Val(Dgv_VortexDetails.Rows(i).Cells(DgvCol_VortexDetails.SPEED).Value) <> 0 Or Val(Dgv_VortexDetails.Rows(i).Cells(DgvCol_VortexDetails.TARGET_PRODUCTIONS).Value) <> 0 Then

                    Tot_Vortex_Speed = Tot_Vortex_Speed + Val(.Rows(i).Cells(DgvCol_VortexDetails.SPEED).Value())
                    Tot_Vortex_TargPrd = Tot_Vortex_TargPrd + Val(.Rows(i).Cells(DgvCol_VortexDetails.TARGET_PRODUCTIONS).Value())
                    Tot_Vortex_ActPrd = Tot_Vortex_ActPrd + Val(.Rows(i).Cells(DgvCol_VortexDetails.ACTUAL_PRODUCTION).Value())
                    Tot_Vortex_ActEff = Tot_Vortex_ActEff + Val(.Rows(i).Cells(DgvCol_VortexDetails.ACTUAL_EFFICIENCY).Value())
                    Tot_Vortex_StopMin = Tot_Vortex_StopMin + Val(.Rows(i).Cells(DgvCol_VortexDetails.STOP_MIN).Value())

                End If
            Next
        End With

        With Dgv_VortexDetails_Total

            If Dgv_VortexDetails_Total.Rows.Count <= 0 Then Dgv_VortexDetails_Total.Rows.Add()

            .Rows(0).Cells(DgvCol_VortexDetails.SPEED).Value = Format(Val(Tot_Vortex_Speed), "#########0.000")
            .Rows(0).Cells(DgvCol_VortexDetails.TARGET_PRODUCTIONS).Value = Format(Val(Tot_Vortex_TargPrd), "#########0.000")
            .Rows(0).Cells(DgvCol_VortexDetails.ACTUAL_PRODUCTION).Value = Format(Val(Tot_Vortex_ActPrd), "#########0.000")
            .Rows(0).Cells(DgvCol_VortexDetails.ACTUAL_EFFICIENCY).Value = Format(Val(Tot_Vortex_ActEff) / Val(vortex_Slno), "#########0.00")
            .Rows(0).Cells(DgvCol_VortexDetails.STOP_MIN).Value = Format(Val(Tot_Vortex_StopMin), "#########0.00")


        End With

        '---------------

        With Dgv_WasteDetails

            For i = 0 To Dgv_WasteDetails.Rows.Count - 1

                Waste_Slno = Waste_Slno + 1

                .Rows(i).Cells(DgvCol_WasteDetails.SL_NO).Value = Waste_Slno

                If Val(Dgv_WasteDetails.Rows(i).Cells(DgvCol_WasteDetails.WEIGHT).Value) <> 0 Or Val(Dgv_WasteDetails.Rows(i).Cells(DgvCol_WasteDetails.WASTE_PERCENTAGE).Value) <> 0 Then

                    Tot_Waste_weight = Tot_Waste_weight + Val(.Rows(i).Cells(DgvCol_WasteDetails.WEIGHT).Value())
                    Tot_Waste_Precn = Tot_Waste_Precn + Val(.Rows(i).Cells(DgvCol_WasteDetails.WASTE_PERCENTAGE).Value())

                End If
            Next
        End With

        With Dgv_WasteDetails_Total
            If Dgv_WasteDetails_Total.Rows.Count <= 0 Then Dgv_WasteDetails_Total.Rows.Add()

            .Rows(0).Cells(DgvCol_WasteDetails.WEIGHT).Value = Format(Val(Tot_Waste_weight), "#########0.000")
            .Rows(0).Cells(DgvCol_WasteDetails.WASTE_PERCENTAGE).Value = Format(Val(Tot_Waste_Precn), "#########0.00")

        End With

        '----------------
    End Sub
    Private Sub get_MachineDetails()
        Dim q As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt2 As New DataTable
        Dim ShtID As Integer
        Dim n As Integer, SNo As Integer
        Dim Sht_Mns As Integer = 0


        ShtID = Common_Procedures.Shift_NameToIdNo(con, cbo_Shift.Text)

        If ShtID <> 0 Then

            If Trim(UCase(cbo_Shift.Tag)) <> Trim(UCase(cbo_Shift.Text)) Then


                Da = New SqlClient.SqlDataAdapter("Select a.*, b.* from OE_Machine_Details a INNER JOIN Count_Head b On b.Count_IdNo = a.Count_Idno where a.Department_IdNo = " & Val(Dep_Id) & " ", con)
                Dt = New DataTable
                Da.Fill(Dt)
                With dgv_CardingDetails
                    .Rows.Clear()
                    SNo = 0

                    For i = 0 To Dt.Rows.Count - 1

                        If Dt.Rows.Count > 0 Then
                            n = dgv_CardingDetails.Rows.Add()
                            SNo = SNo + 1
                            .Rows(n).Cells(DgvCol_CardingDetails.SL_NO).Value = Val(SNo)
                            .Rows(n).Cells(DgvCol_CardingDetails.MACHINE_NO).Value = Dt.Rows(i).Item("Machine_Details_No").ToString
                            .Rows(n).Cells(DgvCol_CardingDetails.COUNT_HANK).Value = Dt.Rows(i).Item("Count_Hank").ToString
                            .Rows(n).Cells(DgvCol_CardingDetails.SPEED).Value = Val(Dt.Rows(i).Item("Speed").ToString)
                            .Rows(n).Cells(DgvCol_CardingDetails.ACTUAL_EFFICIENCY).Value = Val(Dt.Rows(i).Item("Efficiency_Percentage").ToString)



                            'Formula_Calculation()

                        End If
                    Next
                    Formula_Calculation()

                    Dt2.Clear()
                    Dt2.Dispose()

                    Dt.Clear()
                    Dt.Dispose()
                    Da.Dispose()

                End With

                cbo_Shift.Tag = Trim(cbo_Shift.Text)


            End If

        End If

    End Sub

    Private Sub Formula_Calculation()

        Dim sno As Integer
        Dim SLno As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vCount As Integer = 0

        'sno = 0
        'SLno = 0

        With dgv_CardingDetails


            For i = 0 To dgv_CardingDetails.RowCount - 1

                '    '---------- TARGET PRODUCTION FORMULA  -------------

                '    '----------Formula[ Target Pro = (0.2835 * Speed) / Count_Hank ]

                If Val(.Rows(i).Cells(DgvCol_CardingDetails.SPEED).Value) <> 0 And Val(.Rows(i).Cells(DgvCol_CardingDetails.COUNT_HANK).Value) <> 0 Then

                    .Rows(i).Cells(DgvCol_CardingDetails.TARGET_PRODUCTIONS).Value = Format((0.2835 * Val(.Rows(i).Cells(DgvCol_CardingDetails.SPEED).Value) / Val(.Rows(i).Cells(DgvCol_CardingDetails.COUNT_HANK).Value)), "#########0.000")
                Else
                    .Rows(i).Cells(DgvCol_CardingDetails.TARGET_PRODUCTIONS).Value = 0
                End If

            Next



            'sno = sno + 1
            '.Rows(RwNo).Cells(DgvCol_CardingDetails.SL_NO).Value = sno

            'If Val(.Rows(RwNo).Cells(DgvCol_CardingDetails.SPEED).Value) <> 0 Then

            '    '---------- TARGET PRODUCTION FORMULA  -------------

            '    '----------Formula[ Target Pro = (0.2835 * Speed) / Count_Hank ]

            '    .CurrentRow.Cells(DgvCol_CardingDetails.TARGET_PRODUCTIONS).Value = Format((0.2835 * Val(.CurrentRow.Cells(DgvCol_CardingDetails.SPEED).Value) / Val(.CurrentRow.Cells(DgvCol_CardingDetails.COUNT_HANK).Value)), "#########0.000")

            'End If



        End With


        With Dgv_DrawingDetails

            For i = 0 To Dgv_DrawingDetails.RowCount - 1

                '        '---------- TARGET PRODUCTION FORMULA  -------------

                '        '----------Formula[ Target Pro = (0.2835 * Speed) / Count_Hank ]

                If Val(.Rows(i).Cells(DgvCol_DrawingDetails.SPEED).Value) <> 0 And Val(.Rows(i).Cells(DgvCol_DrawingDetails.COUNT_HANK).Value) <> 0 Then

                    .Rows(i).Cells(DgvCol_DrawingDetails.TARGET_PRODUCTIONS).Value = Format((0.2835 * Val(.Rows(i).Cells(DgvCol_DrawingDetails.SPEED).Value) / Val(.Rows(i).Cells(DgvCol_DrawingDetails.COUNT_HANK).Value)), "#########0.000")
                Else
                    .Rows(i).Cells(DgvCol_DrawingDetails.TARGET_PRODUCTIONS).Value = ""
                End If
            Next

            'SLno = SLno + 1
            'If .RowCount > 0 Then

            '    .Rows(RwNo).Cells(DgvCol_DrawingDetails.SL_NO).Value = SLno

            '    If Val(.Rows(RwNo).Cells(DgvCol_DrawingDetails.SPEED).Value) <> 0 Then

            '        '---------- TARGET PRODUCTION FORMULA  -------------

            '        '----------Formula[ Target Pro = (0.2835 * Speed) / Count_Hank ]

            '        .CurrentRow.Cells(DgvCol_DrawingDetails.TARGET_PRODUCTIONS).Value = Format((0.2835 * Val(.CurrentRow.Cells(DgvCol_DrawingDetails.SPEED).Value) / Val(.CurrentRow.Cells(DgvCol_DrawingDetails.COUNT_HANK).Value)), "#########0.000")

            '    End If
            'End If

        End With

        With Dgv_VortexDetails

            For i = 0 To Dgv_VortexDetails.RowCount - 1

                '        '---------- TARGET PRODUCTION FORMULA  -------------

                '        '----------Formula[ Target Pro = (0.2835 * Speed) / Count * 96 ]


                Da = New SqlClient.SqlDataAdapter("Select b.Resultant_Count from OE_Count_Details a LEFT OUTER JOIN Count_Head b on a.Count_Idno=b.Count_Idno where b.count_Idno='" & Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(DgvCol_VortexDetails.COUNT_NAME).Value) & "'  and  a.Department_IdNo = 3 ", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    vCount = Dt1.Rows(0)(0).ToString

                    If Val(.Rows(i).Cells(DgvCol_VortexDetails.SPEED).Value) <> 0 And Trim(.Rows(i).Cells(DgvCol_VortexDetails.COUNT_NAME).Value) <> "" Then

                        .Rows(i).Cells(DgvCol_VortexDetails.TARGET_PRODUCTIONS).Value = ((0.2835 * Val(.Rows(i).Cells(DgvCol_VortexDetails.SPEED).Value)) / Val(vCount) * 96)
                    Else
                        .Rows(i).Cells(DgvCol_VortexDetails.TARGET_PRODUCTIONS).Value = ""

                    End If
                End If


            Next

            ''            sno = sno + 1
            '.Rows(RwNo).Cells(DgvCol_VortexDetails.SL_NO).Value = sno

            'Da = New SqlClient.SqlDataAdapter("Select b.Resultant_Count from OE_Count_Details a LEFT OUTER JOIN Count_Head b on a.Count_Idno=b.Count_Idno where b.count_Idno='" & Common_Procedures.Count_NameToIdNo(con, .Rows(RwNo).Cells(DgvCol_VortexDetails.COUNT_NAME).Value) & "'  and  a.Department_IdNo = " & Val(Dep_Id) & " ", con)
            'Dt1 = New DataTable
            'Da.Fill(Dt1)

            'If Dt1.Rows.Count > 0 Then

            '    vCount = Dt1.Rows(0)(0).ToString

            '    If Val(.Rows(RwNo).Cells(DgvCol_VortexDetails.SPEED).Value) <> 0 Then

            '        '---------- TARGET PRODUCTION FORMULA  -------------

            '        '----------Formula[ Target Pro = (0.2835 * Speed) / Count * 96 ]

            '        .CurrentRow.Cells(DgvCol_VortexDetails.TARGET_PRODUCTIONS).Value = ((0.2835 * Val(.CurrentRow.Cells(DgvCol_VortexDetails.SPEED).Value)) / Val(vCount) * 96)

            '    End If
            'End If
        End With

        With Dgv_WasteDetails
            For i = 0 To Dgv_WasteDetails.RowCount - 1
                If .RowCount > 0 Then

                    If Val(.Rows(i).Cells(DgvCol_WasteDetails.WEIGHT).Value) <> 0 Then

                        .Rows(i).Cells(DgvCol_WasteDetails.WASTE_PERCENTAGE).Value = Format(Val(.Rows(i).Cells(DgvCol_WasteDetails.WEIGHT).Value) / 100, "#########0.00")

                    End If

                End If
            Next
        End With


    End Sub
    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_CardingDetails.CellEndEdit
        Try
            With dgv_CardingDetails
                If .Visible = True Then
                    If .Rows.Count > 0 Then
                        dgv_CardingDetails_CellLeave(sender, e)
                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS CELL End EDIT....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub dgv_CardingDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_CardingDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim rect As Rectangle

        Try
            With dgv_CardingDetails

                dgv_ActCtrlName = .Name
                'ActiveControl.Name = .Name
                If Val(.CurrentRow.Cells(DgvCol_CardingDetails.SL_NO).Value) = 0 Then
                    .CurrentRow.Cells(DgvCol_CardingDetails.SL_NO).Value = .CurrentRow.Index + 1
                End If

                'If Val(.Rows(e.RowIndex).Cells(17).Value) = 0 Then
                '    If e.RowIndex = 0 Then
                '        .Rows(e.RowIndex).Cells(17).Value = 1
                '    Else
                '        .Rows(e.RowIndex).Cells(17).Value = Val(.Rows(e.RowIndex - 1).Cells(17).Value) + 1
                '    End If
                'End If


                '
                Formula_Calculation()
                'Formula_Calculation(e.RowIndex)

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS CELL ENTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_CardingDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_CardingDetails.CellLeave

        Try
            With dgv_CardingDetails

                If .CurrentCell.ColumnIndex <> DgvCol_CardingDetails.MACHINE_NO And .CurrentCell.ColumnIndex <> DgvCol_CardingDetails.SL_NO Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                    End If
                End If
                If .CurrentCell.ColumnIndex = DgvCol_CardingDetails.ACTUAL_EFFICIENCY Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS CELL LEAVE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)


        End Try

    End Sub

    Private Sub dgv_CardingDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_CardingDetails.CellValueChanged
        On Error Resume Next
        If IsNothing(dgv_CardingDetails.CurrentCell) Then Exit Sub
        '  Try
        With dgv_CardingDetails
            If .Visible Then
                If .Rows.Count > 0 Then
                    If e.ColumnIndex = DgvCol_CardingDetails.SPEED Or e.ColumnIndex = DgvCol_CardingDetails.COUNT_HANK Or e.ColumnIndex = DgvCol_CardingDetails.TARGET_PRODUCTIONS Or e.ColumnIndex = DgvCol_CardingDetails.ACTUAL_PRODUCTION Or e.ColumnIndex = DgvCol_CardingDetails.ACTUAL_EFFICIENCY Then
                        Formula_Calculation()

                        '    '---------- TARGET PRODUCTION FORMULA  -------------

                        '    '----------Formula[ Target Pro = (0.2835 * Speed) / Count_Hank ]
                        'For I = 0 To .RowCount - 1

                        '    If Val(.Rows(I).Cells(DgvCol_CardingDetails.SPEED).Value) <> 0 And Val(.Rows(I).Cells(DgvCol_CardingDetails.COUNT_HANK).Value) <> 0 Then

                        '        .Rows(I).Cells(DgvCol_CardingDetails.TARGET_PRODUCTIONS).Value = Format((0.2835 * Val(.Rows(I).Cells(DgvCol_CardingDetails.SPEED).Value) / Val(.Rows(I).Cells(DgvCol_CardingDetails.COUNT_HANK).Value)), "#########0.000")
                        '    End If
                        'Next
                        Total_Calculation()
                    End If
                End If
            End If
        End With

        'Catch ex As NullReferenceException
        '    '---MessageBox.Show(ex.Message, "Error While DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'Catch ex As ObjectDisposedException
        '    '---MessageBox.Show(ex.Message, "Error While DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "Error While DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

    End Sub

    Private Sub dgv_CardingDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_CardingDetails.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_CardingDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_CardingDetails.EditingControlShowing
        Try
            With dgv_CardingDetails
                If .Rows.Count > 0 Then
                    dgtxt_details = CType(dgv_CardingDetails.EditingControl, DataGridViewTextBoxEditingControl)
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS EDITING SHOWING....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_details.Enter
        Try
            dgv_ActCtrlName = dgv_CardingDetails.Name
            dgv_CardingDetails.EditingControl.BackColor = Color.Lime
            dgv_CardingDetails.EditingControl.ForeColor = Color.Blue
            dgtxt_details.SelectAll()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS TEXT ENTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgtxt_details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_details.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS TEXT KEYDOWN....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_details.KeyPress
        Try

            With dgv_CardingDetails


                If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                    e.Handled = True
                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS TEXT KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub dgv_CardingDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_CardingDetails.KeyUp
        Dim n As Integer

        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With dgv_CardingDetails

                    n = .CurrentRow.Index

                    If .Rows.Count = 1 Then
                        For i = 0 To .Columns.Count - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else

                        .Rows.RemoveAt(n)

                    End If

                    For i = 0 To .Rows.Count - 1
                        .Rows(i).Cells(DgvCol_CardingDetails.SL_NO).Value = i + 1
                    Next

                End With

                Total_Calculation()

            End If

            'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            '    If dgv_CardingDetails.CurrentCell.ColumnIndex = 8 Then
            '        Get_StoppageDetails()
            '    End If

            'End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS KEYUP....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_CardingDetails.RowsAdded
        Dim n As Integer
        If IsNothing(dgv_CardingDetails.CurrentCell) Then Exit Sub
        With dgv_CardingDetails
            n = .RowCount
            .Rows(n - 1).Cells(DgvCol_CardingDetails.SL_NO).Value = Val(n)

        End With



    End Sub

    Private Sub Get_StoppageDetails()
        Dim Det_SLNo As Integer
        Dim n As Integer, SNo As Integer
        Dim Sht_ID As Integer = 0
        Dim Mch_ID As Integer = 0
        Dim Cnt_ID As Integer = 0

        Try

            Sht_ID = Common_Procedures.Shift_NameToIdNo(con, cbo_Shift.Text)
            If Sht_ID = 0 Then
                MessageBox.Show("Invalid Shift", "DOES Not STOPPAGE DETAILS...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_Shift.Enabled And cbo_Shift.Visible Then cbo_Shift.Focus()
                Exit Sub
            End If

            If Trim(dgv_CardingDetails.CurrentRow.Cells(DgvCol_CardingDetails.MACHINE_NO).Value) = "" Then
                MessageBox.Show("Invalid Machine No", "DOES Not SHOW STOPPAGE DETAILS...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                dgv_CardingDetails.Focus()
                If dgv_CardingDetails.Rows.Count > 0 Then
                    dgv_CardingDetails.CurrentCell = dgv_CardingDetails.Rows(0).Cells(DgvCol_CardingDetails.COUNT_HANK)
                    dgv_CardingDetails.CurrentCell.Selected = True
                End If
                Exit Sub
            End If

            Cnt_ID = Common_Procedures.Count_NameToIdNo(con, dgv_CardingDetails.CurrentRow.Cells(DgvCol_CardingDetails.COUNT_HANK).Value)
            If Cnt_ID = 0 Then
                MessageBox.Show("Invalid Count", "DOES Not SHOW STOPPAGE DETAILS...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                dgv_CardingDetails.Focus()
                If dgv_CardingDetails.Rows.Count > 0 Then
                    dgv_CardingDetails.CurrentCell = dgv_CardingDetails.Rows(0).Cells(DgvCol_CardingDetails.COUNT_HANK)
                    dgv_CardingDetails.CurrentCell.Selected = True
                End If
                Exit Sub
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES Not Select STOPPAGE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_details.KeyUp
        Dim dgv_DetSlNo As Integer = 0
        Try
            With dgv_CardingDetails
                If .Rows.Count > 0 Then
                    If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                        dgv_CardingDetails_KeyUp(sender, e)
                    End If

                    If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
                        dgv_CardingDetails_KeyUp(sender, e)
                    End If
                End If
            End With

        Catch ex As Exception
            '----

        End Try

    End Sub
    Private Sub dgtxt_details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_details.TextChanged
        Try
            With dgv_CardingDetails

                If .Visible Then
                    If .Rows.Count > 0 Then

                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_details.Text)

                    End If
                End If
            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "Error While DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "Error While DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Shift_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Shift.GotFocus

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then
            vSqlCond = "Shift_IdNo < 4"
        Else
            vSqlCond = ""
        End If
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Shift_Head", "Shift_Name", Trim(vSqlCond), "(Shift_idno = 0)")

        cbo_Shift.Tag = Trim(cbo_Shift.Text)

    End Sub

    Private Sub cbo_Shift_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Shift.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Shift, dtp_Date, Nothing, "Shift_Head", "Shift_Name", Trim(vSqlCond), "(Shift_idno = 0)")

        If (e.KeyValue = 40 And cbo_Shift.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            dgv_CardingDetails.Focus()
            dgv_CardingDetails.CurrentCell = dgv_CardingDetails.Rows(0).Cells(DgvCol_CardingDetails.ACTUAL_PRODUCTION)
            dgv_CardingDetails.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub cbo_Shift_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Shift.KeyPress
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String
        Dim cmd As New SqlClient.SqlCommand
        Dim Sht_ID As Integer = 0
        Dim vTotTargProd As String
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Shift, Nothing, "Shift_Head", "Shift_Name", Trim(vSqlCond), "(Shift_idno = 0)")

        Sht_ID = Common_Procedures.Shift_NameToIdNo(con, cbo_Shift.Text)
        cmd.Connection = con
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@CrdDate", dtp_Date.Value.Date)


        vTotTargProd = 0
        If dgv_CardingDetails_Total.RowCount > 0 Then
            vTotTargProd = Val(dgv_CardingDetails_Total.Rows(0).Cells(DgvCol_CardingDetails.TARGET_PRODUCTIONS).Value())
        End If

        If Asc(e.KeyChar) = 13 Then




            If Trim(UCase(cbo_Shift.Text)) <> Trim(UCase(cbo_Shift.Tag)) Then
                Check_and_Get_MachineNo_List(sender)
            End If

            'cmd.CommandText = "Select OE_Production_No from OE_Vortex_Production_Head where Shift_idno = " & Val(Sht_ID) & " And OE_Production_Date = @CrdDate  "
            'Da = New SqlClient.SqlDataAdapter(cmd)
            'Da.Fill(Dt)

            'movno = ""
            'If Dt.Rows.Count > 0 Then
            '    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
            '        movno = Trim(Dt.Rows(0)(0).ToString)
            '    End If
            'End If

            'Dt.Clear()
            'Dt.Dispose()
            'Da.Dispose()

            'If Val(movno) <> 0 Then
            '    move_record(movno)


            'Else

            '    'get_MachineDetails()
            '    get_MachineNo_List()

            '    If dgv_CardingDetails.Rows.Count > 0 Then
            '        dgv_CardingDetails.Focus()
            '        dgv_CardingDetails.CurrentCell = dgv_CardingDetails.Rows(0).Cells(DgvCol_CardingDetails.ACTUAL_PRODUCTION)
            '        dgv_CardingDetails.CurrentCell.Selected = True

            '    Else
            '        If MessageBox.Show("Do you want To save?", "For SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            '            save_record()
            '        Else
            '            dtp_Date.Focus()
            '        End If

            '    End If

            'End If


            If dgv_CardingDetails.Rows.Count > 0 Then
                dgv_CardingDetails.Focus()
                dgv_CardingDetails.CurrentCell = dgv_CardingDetails.Rows(0).Cells(DgvCol_CardingDetails.ACTUAL_PRODUCTION)
                dgv_CardingDetails.CurrentCell.Selected = True

            Else
                If MessageBox.Show("Do you want To save?", "For SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()
                Else
                    dtp_Date.Focus()
                End If

            End If

        End If



        '---------------

        'Dim vTotMtr As String

        'If Trim(UCase(e.KeyChar)) = "D" Then
        '    msk_Date.Text = Date.Today
        '    msk_Date.SelectionStart = 0
        'End If

        'Try

        '    If Asc(e.KeyChar) = 13 Then


        '        vTotMtr = 0
        '        If dgv_Details_Total.RowCount > 0 Then
        '            vTotMtr = Val(dgv_Details_Total.Rows(0).Cells(DgvCol_Details.TOTAL_METERS).Value())
        '        End If

        '        If Trim(UCase(msk_Date.Text)) <> Trim(UCase(msk_Date.Tag)) Or dgv_Details.Rows.Count = 0 Or Val(vTotMtr) = 0 Then
        '            Check_and_Get_LoomNo_List(sender)
        '        End If

        '        If dgv_Details.Rows.Count > 0 Then
        '            dgv_Details.Focus()
        '            If dgv_Details.Rows(0).Cells(DgvCol_Details.CLOTH_NAME).Visible = True Then
        '                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(DgvCol_Details.CLOTH_NAME)
        '            ElseIf dgv_Details.Rows(0).Cells(DgvCol_Details.SHIFT_1).Visible = True Then
        '                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(DgvCol_Details.SHIFT_1)
        '            Else
        '                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(DgvCol_Details.LOOM_NO)
        '            End If
        '            dgv_Details.CurrentCell.Selected = True

        '        Else

        '            If MessageBox.Show("Do you want To save?", "For SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
        '                save_record()
        '            Else
        '                msk_Date.Focus()
        '            End If

        '        End If

        '    End If

        'Catch ex As Exception
        '    '----

        'End Try

        '--------------

    End Sub

    Private Sub cbo_Shift_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Shift.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Shift_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Shift.Name
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
        Dim Cnt_IdNo As Integer, Emp_IdNo As Integer, Sht_IdNo As Integer, Variety_IdNo As Integer
        Dim Condt As String = ""


        Try

            Condt = ""
            Cnt_IdNo = 0
            Emp_IdNo = 0
            Sht_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.OE_Production_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.OE_Production_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.OE_Production_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_Count.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_Count.Text)
            End If

            If Trim(cbo_Filter_Shift.Text) <> "" Then
                Sht_IdNo = Common_Procedures.Shift_NameToIdNo(con, cbo_Filter_Shift.Text)
            End If
            If Trim(cbo_Filter_Employee.Text) <> "" Then
                Variety_IdNo = Common_Procedures.Variety_NameToIdNo(con, cbo_Filter_Employee.Text)
            End If

            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "b.Count_IdNo = " & Str(Val(Cnt_IdNo))
            End If

            If Val(Sht_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "d.Shift_Idno = " & Str(Val(Sht_IdNo))
            End If
            If Val(Variety_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "vh.Variety_IdNo = " & Str(Val(Variety_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*,b.*, e.Count_Name,d.Shift_Name from OE_Vortex_Production_Head a  INNER JOIN OE_Carding_Production_Details b on a.OE_Production_Code = b.OE_Production_Code  INNER JOIN Count_Head e on b.Count_IdNo = e.Count_IdNo LEFT OUTER JOIN Variety_Head vh on b.V INNER JOIN Shift_Head d ON a.Shift_Idno = d.Shift_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.OE_Production_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.OE_Production_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("OE_Production_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("OE_Production_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(DgvCol_FillterDetails.COUNT).Value = dt2.Rows(i).Item("Count_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(DgvCol_FillterDetails.SHIFT).Value = dt2.Rows(i).Item("Shift_Name").ToString
                    'dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Hank").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(DgvCol_FillterDetails.MACHINE_NO).Value = dt2.Rows(i).Item("Machine_No").ToString
                    'dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Actual_Production").ToString), "########0.0")
                    'dgv_Filter_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Target_Production").ToString), "########0.0")
                    dgv_Filter_Details.Rows(n).Cells(DgvCol_FillterDetails.WASTE).Value = dt2.Rows(i).Item("VARIETY_NAME").ToString

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


    Private Sub cbo_Filter_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Count.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_idno = 0)")

    End Sub

    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Count.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Count, cbo_Filter_Shift, cbo_Filter_Employee, "Count_Head", "Count_Name", "", "(Count_idno = 0)")

    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Count.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Count, cbo_Filter_Employee, "Count_Head", "Count_Name", "", "(Count_idno = 0)")
    End Sub

    Private Sub cbo_Filter_Employee_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Employee.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Variety_Head", "Variety_Name", "(variety_type='WASTE')", "(Variety_IdNo = 0)")

    End Sub
    Private Sub cbo_Filter_Employee_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Employee.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Employee, cbo_Filter_Count, btn_Filter_Show, "Variety_Head", "Variety_Name", "(variety_type='WASTE')", "(Variety_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_Employee_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Employee.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Employee, btn_Filter_Show, "Variety_Head", "Variety_Name", "(variety_type='WASTE')", "(Variety_IdNo = 0)")
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

    Public Sub print_record() Implements Interface_MDIActions.print_record


    End Sub
    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub
    Private Sub cbo_Filter_Shift_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Shift.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Shift_Head", "Shift_Name", "", "(Shift_idno = 0)")

    End Sub

    Private Sub cbo_Filter_Shift_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Shift.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Shift, dtp_Filter_ToDate, cbo_Filter_Count, "Shift_Head", "Shift_Name", "", "(Shift_idno = 0)")


    End Sub

    Private Sub cbo_Filter_Shift_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Shift.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Shift, cbo_Filter_Count, "Shift_Head", "Shift_Name", "", "(Shift_idno = 0)")

    End Sub

    Private Sub btn_List_LoomDetails_Click(sender As Object, e As EventArgs) Handles btn_List_MachineNo.Click
        Check_and_Get_MachineNo_List(sender)
    End Sub
    Private Sub Check_and_Get_MachineNo_List(sender As System.Object)
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String = ""
        Dim NewCode As String = ""
        Dim Cat_ID As Integer = 0
        Dim Sht_ID As Integer = 0
        Dim vTotTargProd As String


        Try

            If IsDate(dtp_Date.Text) = False Then
                MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
                Exit Sub
            End If


            If Not (Convert.ToDateTime(dtp_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(dtp_Date.Text) <= Common_Procedures.Company_ToDate) Then
                MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
                Exit Sub
            End If

            Sht_ID = Common_Procedures.Shift_NameToIdNo(con, cbo_Shift.Text)

            Cmd.Connection = con

            Cmd.Parameters.Clear()
            Cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(dtp_Date.Text))

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Cmd.CommandText = "Select OE_Production_No from OE_Vortex_Production_Head Where company_idno = " & Str(Val(lbl_Company.Tag)) & " AND Shift_idno = " & Val(Sht_ID) & " and OE_Production_Date = @EntryDate and OE_Production_Code <> '" & Trim(NewCode) & "'  Order by OE_Production_Date, for_orderby, OE_Production_No"
            '            Cmd.CommandText = "Select OE_Production_No from OE_Vortex_Production_Head where Shift_idno = " & Val(Sht_ID) & " And OE_Production_Date = @CrdDate  "

            'Cmd.CommandText = "Select OE_Production_No from OE_Vortex_Production_Head where Shift_idno = " & Val(Sht_ID) & " And OE_Production_Date = @CrdDate  "

            Da = New SqlClient.SqlDataAdapter(Cmd)
            Dt = New DataTable
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If
            Dt.Clear()

            If Val(movno) <> 0 Then

                If Trim(UCase(movno)) <> Trim(UCase(lbl_RefNo.Text)) Then
                    move_record(movno)

                Else

                    If sender.name.ToString.ToLower = btn_List_MachineNo.Name.ToString.ToLower Then
                        move_record(movno)
                    End If

                End If

            Else

                '    get_MachineDetails()
                get_MachineNo_List()

            End If
            cbo_Shift.Tag = cbo_Shift.Text
        Catch ex As Exception
            '----

        End Try
    End Sub

    Private Sub get_MachineNo_List()
        Dim Cmd As New SqlClient.SqlCommand
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim n As Integer
        Dim SNo As Integer

        Cmd.Connection = con


        If Trim(UCase(cbo_Shift.Text)) <> Trim(UCase(cbo_Shift.Tag)) Then

            Cmd.CommandText = "Select A.*,B.Count_Name from OE_Machine_Details a  LEFT OUTER JOIN Count_Head b  On a.count_Idno = b.count_Idno  Where a.Machine_Details_No <> '' and Department_Idno = 1 Order by  a.Machine_Details_No "
            ' Cmd.CommandText = "select a.* from OE_Machine_Details a  LEFT OUTER JOIN Count_Head b  on a.count_Idno = b.count_Idno Where a.Machine_Details_No <> '' Order by  a.Machine_Details_No"
            da1 = New SqlClient.SqlDataAdapter(Cmd)
            dt1 = New DataTable
            da1.Fill(dt1)


            dgv_CardingDetails.Rows.Clear()
            SNo = 0

            If dt1.Rows.Count > 0 Then

                For i = 0 To dt1.Rows.Count - 1

                    With dgv_CardingDetails

                        n = .Rows.Add()

                        SNo = SNo + 1

                        .Rows(n).Cells(DgvCol_CardingDetails.SL_NO).Value = Val(SNo)
                        .Rows(n).Cells(DgvCol_CardingDetails.MACHINE_NO).Value = dt1.Rows(i).Item("Machine_Details_No").ToString
                        .Rows(n).Cells(DgvCol_CardingDetails.COUNT_HANK).Value = dt1.Rows(i).Item("Count_Hank").ToString
                        .Rows(n).Cells(DgvCol_CardingDetails.SPEED).Value = Val(dt1.Rows(i).Item("Speed").ToString)
                        ' .Rows(n).Cells(DgvCol_CardingDetails.ACTUAL_EFFICIENCY).Value = Val(dt1.Rows(i).Item("Efficiency_Percentage").ToString)
                    End With

                Next i

            End If

            Cmd.CommandText = "select A.*,B.Count_Name from OE_Machine_Details a  LEFT OUTER JOIN Count_Head b  on a.count_Idno = b.count_Idno  Where a.Machine_Details_No <> ''  and Department_Idno = 2 Order by  a.Machine_Details_No "
            ' Cmd.CommandText = "select a.* from OE_Machine_Details a  LEFT OUTER JOIN Count_Head b  on a.count_Idno = b.count_Idno Where a.Machine_Details_No <> '' Order by  a.Machine_Details_No"
            da2 = New SqlClient.SqlDataAdapter(Cmd)
            dt2 = New DataTable
            da2.Fill(dt2)

            Dgv_DrawingDetails.Rows.Clear()
            SNo = 0
            With Dgv_DrawingDetails

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1

                        .Rows(n).Cells(DgvCol_DrawingDetails.SL_NO).Value = Val(SNo)
                        .Rows(n).Cells(DgvCol_DrawingDetails.MACHINE_NO).Value = dt2.Rows(i).Item("Machine_Details_No").ToString
                        .Rows(n).Cells(DgvCol_DrawingDetails.COUNT_HANK).Value = dt2.Rows(i).Item("Count_Hank").ToString
                        .Rows(n).Cells(DgvCol_DrawingDetails.SPEED).Value = Val(dt2.Rows(i).Item("Speed").ToString)
                        '    .Rows(n).Cells(DgvCol_DrawingDetails.ACTUAL_EFFICIENCY).Value = Val(dt2.Rows(i).Item("Efficiency_Percentage").ToString)


                    Next i
                End If

            End With


            Cmd.CommandText = "select A.*,B.Count_Name from OE_Machine_Details a  LEFT OUTER JOIN Count_Head b  on a.count_Idno = b.count_Idno  Where a.Machine_Details_No <> ''  and Department_Idno = 3 Order by  a.Machine_Details_No "
            ' Cmd.CommandText = "select a.* from OE_Machine_Details a  LEFT OUTER JOIN Count_Head b  on a.count_Idno = b.count_Idno Where a.Machine_Details_No <> '' Order by  a.Machine_Details_No"
            da3 = New SqlClient.SqlDataAdapter(Cmd)
            dt3 = New DataTable
            da3.Fill(dt3)

            Dgv_VortexDetails.Rows.Clear()
            SNo = 0
            With Dgv_VortexDetails

                If dt3.Rows.Count > 0 Then

                    For i = 0 To dt3.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1

                        .Rows(n).Cells(DgvCol_VortexDetails.SL_NO).Value = Val(SNo)
                        .Rows(n).Cells(DgvCol_VortexDetails.COUNT_NAME).Value = dt3.Rows(i).Item("count_Name").ToString
                        .Rows(n).Cells(DgvCol_VortexDetails.SPEED).Value = Val(dt3.Rows(i).Item("Speed").ToString)
                        ' .Rows(n).Cells(DgvCol_VortexDetails.ACTUAL_EFFICIENCY).Value = Val(dt3.Rows(i).Item("Efficiency_Percentage").ToString)

                    Next i

                End If

            End With
            Dgv_WasteDetails.Rows.Clear()



            Formula_Calculation()

            cbo_Shift.Tag = cbo_Shift.Text
        End If

        ' Formula_Calculation(n)

        Grid_Cell_DeSelect()


    End Sub
    Private Sub Dgv_DrawingDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Dgv_DrawingDetails.EditingControlShowing
        Try
            dgtxt_DrawingDetails = Nothing
            With Dgv_DrawingDetails
                If .Rows.Count > 0 Then
                    dgtxt_DrawingDetails = CType(Dgv_DrawingDetails.EditingControl, DataGridViewTextBoxEditingControl)
                End If
            End With
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS EDITING SHOWING....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

    End Sub
    Private Sub dgtxt_DrawingDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_DrawingDetails.Enter
        Try
            dgv_ActCtrlName = Dgv_DrawingDetails.Name
            Dgv_DrawingDetails.EditingControl.BackColor = Color.Lime
            Dgv_DrawingDetails.EditingControl.ForeColor = Color.Blue
            dgtxt_DrawingDetails.SelectAll()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS TEXT ENTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub dgtxt_DrawingDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_DrawingDetails.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS TEXT KEYDOWN....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub dgtxt_DrawingDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_DrawingDetails.KeyPress
        Try

            With Dgv_DrawingDetails


                If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                    e.Handled = True
                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS TEXT KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub dgtxt_DrawingDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_DrawingDetails.KeyUp
        Dim dgv_DetSlNo As Integer = 0
        Try
            With Dgv_DrawingDetails
                If .Rows.Count > 0 Then
                    If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                        Dgv_DrawingDetails_KeyUp(sender, e)
                    End If

                    If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
                        Dgv_DrawingDetails_KeyUp(sender, e)

                    End If
                End If
            End With

        Catch ex As Exception
            '----
        End Try

    End Sub
    Private Sub dgtxt_DrawingDetails_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_DrawingDetails.TextChanged
        Try
            With Dgv_DrawingDetails

                If .Visible Then
                    If .Rows.Count > 0 Then

                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_DrawingDetails.Text)

                    End If
                End If
            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "Error While DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "Error While DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub Dgv_DrawingDetails_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles Dgv_DrawingDetails.RowsAdded
        Dim n As Integer
        If IsNothing(Dgv_DrawingDetails.CurrentCell) Then Exit Sub

        With Dgv_DrawingDetails
            n = .RowCount
            .Rows(n - 1).Cells(DgvCol_DrawingDetails.SL_NO).Value = Val(n)
        End With
    End Sub
    Private Sub Dgv_DrawingDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Dgv_DrawingDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim rect As Rectangle

        Try
            With Dgv_DrawingDetails


                dgv_ActCtrlName = .Name

                If Val(.CurrentRow.Cells(DgvCol_DrawingDetails.SL_NO).Value) = 0 Then
                    .CurrentRow.Cells(DgvCol_DrawingDetails.SL_NO).Value = .CurrentRow.Index + 1

                End If

                Formula_Calculation()

                Dgv_DrawingDetails_CellLeave(sender, e)
            End With

        Catch ex As Exception

        End Try

    End Sub

    Private Sub Dgv_DrawingDetails_CellLeave(sender As Object, e As DataGridViewCellEventArgs) Handles Dgv_DrawingDetails.CellLeave

        Try
            With Dgv_DrawingDetails

                If .CurrentCell.ColumnIndex <> DgvCol_DrawingDetails.MACHINE_NO And .CurrentCell.ColumnIndex <> DgvCol_DrawingDetails.SL_NO Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                    End If
                End If
                If .CurrentCell.ColumnIndex = DgvCol_DrawingDetails.ACTUAL_EFFICIENCY Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS CELL LEAVE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub Dgv_DrawingDetails_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles Dgv_DrawingDetails.CellValueChanged
        On Error Resume Next
        If IsNothing(Dgv_DrawingDetails.CurrentCell) Then Exit Sub
        ' Try
        With Dgv_DrawingDetails
            If .Visible Then
                If .Rows.Count > 0 Then
                    If e.ColumnIndex = DgvCol_DrawingDetails.SPEED Or e.ColumnIndex = DgvCol_DrawingDetails.COUNT_HANK Or e.ColumnIndex = DgvCol_DrawingDetails.TARGET_PRODUCTIONS Or e.ColumnIndex = DgvCol_DrawingDetails.ACTUAL_PRODUCTION Or e.ColumnIndex = DgvCol_DrawingDetails.ACTUAL_EFFICIENCY Then
                        Formula_Calculation()

                        '    '---------- TARGET PRODUCTION FORMULA  -------------

                        '    '----------Formula[ Target Pro = (0.2835 * Speed) / Count_Hank ]

                        'If Val(.Rows(.CurrentCell.RowIndex).Cells(DgvCol_DrawingDetails.SPEED).Value) <> 0 And Val(.Rows(.CurrentCell.RowIndex).Cells(DgvCol_DrawingDetails.COUNT_HANK).Value) <> 0 Then

                        '    .Rows(.CurrentCell.RowIndex).Cells(DgvCol_DrawingDetails.TARGET_PRODUCTIONS).Value = Format((0.2835 * Val(.Rows(.CurrentCell.RowIndex).Cells(DgvCol_DrawingDetails.SPEED).Value) / Val(.Rows(.CurrentCell.RowIndex).Cells(DgvCol_DrawingDetails.COUNT_HANK).Value)), "#########0.000")

                        'End If

                        Total_Calculation()
                    End If
                End If
            End If
        End With

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "Error While DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try
    End Sub
    Private Sub Dgv_DrawingDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Dgv_DrawingDetails.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub
    Private Sub Dgv_DrawingDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Dgv_DrawingDetails.KeyUp
        Dim n As Integer

        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With Dgv_DrawingDetails

                    n = .CurrentRow.Index

                    If .Rows.Count = 1 Then
                        For i = 0 To .Columns.Count - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else

                        .Rows.RemoveAt(n)

                    End If

                    For i = 0 To .Rows.Count - 1
                        .Rows(i).Cells(DgvCol_DrawingDetails.SL_NO).Value = i + 1
                    Next

                End With

                Total_Calculation()

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS KEYUP....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    '------------------

    Private Sub Dgv_VortexDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Dgv_VortexDetails.EditingControlShowing
        Try
            dgtxt_VortexDetails = Nothing
            With Dgv_VortexDetails
                If .Rows.Count > 0 Then
                    dgtxt_VortexDetails = CType(Dgv_VortexDetails.EditingControl, DataGridViewTextBoxEditingControl)
                End If
            End With
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS EDITING SHOWING....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

    End Sub
    Private Sub dgtxt_VortexDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_VortexDetails.Enter
        Try
            dgv_ActCtrlName = Dgv_VortexDetails.Name
            Dgv_VortexDetails.EditingControl.BackColor = Color.Lime
            Dgv_VortexDetails.EditingControl.ForeColor = Color.Blue
            dgtxt_VortexDetails.SelectAll()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS TEXT ENTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub dgtxt_VortexDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_VortexDetails.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS TEXT KEYDOWN....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub dgtxt_VortexDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_VortexDetails.KeyPress
        Try

            With Dgv_VortexDetails


                If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                    e.Handled = True
                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS TEXT KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub dgtxt_VortexDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_VortexDetails.KeyUp
        Dim dgv_DetSlNo As Integer = 0
        Try
            With Dgv_VortexDetails
                If .Rows.Count > 0 Then
                    If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                        Dgv_VortexDetails_KeyUp(sender, e)
                    End If

                    If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
                        Dgv_VortexDetails_KeyUp(sender, e)

                    End If
                End If
            End With

        Catch ex As Exception
            '----
        End Try

    End Sub
    Private Sub dgtxt_VortexDetails_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_VortexDetails.TextChanged
        Try
            With Dgv_VortexDetails

                If .Visible Then
                    If .Rows.Count > 0 Then

                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_VortexDetails.Text)

                    End If
                End If
            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "Error While DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "Error While DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub Dgv_VortexDetails_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles Dgv_VortexDetails.RowsAdded
        Dim n As Integer
        If IsNothing(Dgv_VortexDetails.CurrentCell) Then Exit Sub

        With Dgv_VortexDetails
            n = .RowCount
            .Rows(n - 1).Cells(DgvCol_VortexDetails.SL_NO).Value = Val(n)
        End With
    End Sub
    Private Sub Dgv_VortexDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Dgv_VortexDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim rect As Rectangle

        Try
            With Dgv_VortexDetails


                dgv_ActCtrlName = .Name

                If Val(.CurrentRow.Cells(DgvCol_VortexDetails.SL_NO).Value) = 0 Then
                    .CurrentRow.Cells(DgvCol_VortexDetails.SL_NO).Value = .CurrentRow.Index + 1

                End If
                If e.ColumnIndex = DgvCol_VortexDetails.COUNT_NAME Then

                    If cbo_Grid_Vortex_CountName.Visible = False Or Val(cbo_Grid_Vortex_CountName.Tag) <> e.RowIndex Then

                        cbo_Grid_Vortex_CountName.Tag = -100
                        Da = New SqlClient.SqlDataAdapter("select count_name from count_Head order by count_name", con)
                        Dt1 = New DataTable
                        Da.Fill(Dt1)
                        cbo_Grid_Vortex_CountName.DataSource = Dt1
                        cbo_Grid_Vortex_CountName.DisplayMember = "count_name"

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_Grid_Vortex_CountName.Left = .Left + rect.Left
                        cbo_Grid_Vortex_CountName.Top = .Top + rect.Top

                        cbo_Grid_Vortex_CountName.Width = rect.Width
                        cbo_Grid_Vortex_CountName.Height = rect.Height
                        cbo_Grid_Vortex_CountName.Text = .CurrentCell.Value

                        cbo_Grid_Vortex_CountName.Tag = Val(e.RowIndex)
                        cbo_Grid_Vortex_CountName.Visible = True

                        cbo_Grid_Vortex_CountName.BringToFront()
                        cbo_Grid_Vortex_CountName.Focus()

                    End If

                Else
                    cbo_Grid_Vortex_CountName.Visible = False
                End If

                Formula_Calculation()
                'Formula_Calculation(e.RowIndex)
                Dgv_VortexDetails_CellLeave(sender, e)
            End With

        Catch ex As Exception

        End Try

    End Sub

    Private Sub Dgv_VortexDetails_CellLeave(sender As Object, e As DataGridViewCellEventArgs) Handles Dgv_VortexDetails.CellLeave

        Try
            With Dgv_VortexDetails

                If .CurrentCell.ColumnIndex <> DgvCol_VortexDetails.COUNT_NAME And .CurrentCell.ColumnIndex <> DgvCol_VortexDetails.SL_NO Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                    End If
                End If
                If .CurrentCell.ColumnIndex = DgvCol_VortexDetails.ACTUAL_EFFICIENCY Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS CELL LEAVE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub Dgv_VortexDetails_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles Dgv_VortexDetails.CellValueChanged

        Dim sno As Integer
        Dim SLno As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vCount As Integer = 0



        On Error Resume Next
        If IsNothing(Dgv_VortexDetails.CurrentCell) Then Exit Sub
        ' Try
        With Dgv_VortexDetails
            If .Visible Then
                If .Rows.Count > 0 Then
                    If e.ColumnIndex = DgvCol_VortexDetails.SPEED Or e.ColumnIndex = DgvCol_VortexDetails.TARGET_PRODUCTIONS Or e.ColumnIndex = DgvCol_VortexDetails.ACTUAL_PRODUCTION Or e.ColumnIndex = DgvCol_VortexDetails.ACTUAL_EFFICIENCY Then


                        Formula_Calculation()

                        '        '---------- TARGET PRODUCTION FORMULA  -------------

                        '        '----------Formula[ Target Pro = (0.2835 * Speed) / Count * 96 ]


                        'Da = New SqlClient.SqlDataAdapter("Select b.Resultant_Count from OE_Count_Details a LEFT OUTER JOIN Count_Head b on a.Count_Idno=b.Count_Idno where b.count_Idno='" & Common_Procedures.Count_NameToIdNo(con, .Rows(.CurrentCell.RowIndex).Cells(DgvCol_VortexDetails.COUNT_NAME).Value) & "'  and  a.Department_IdNo = 3 ", con)
                        'Dt1 = New DataTable
                        'Da.Fill(Dt1)

                        'If Dt1.Rows.Count > 0 Then

                        '    vCount = Dt1.Rows(0)(0).ToString

                        '    If Val(.Rows(.CurrentCell.RowIndex).Cells(DgvCol_VortexDetails.SPEED).Value) <> 0 And Trim(.Rows(.CurrentCell.RowIndex).Cells(DgvCol_VortexDetails.COUNT_NAME).Value) <> "" Then

                        '        .Rows(.CurrentCell.RowIndex).Cells(DgvCol_VortexDetails.TARGET_PRODUCTIONS).Value = ((0.2835 * Val(.Rows(.CurrentCell.RowIndex).Cells(DgvCol_VortexDetails.SPEED).Value)) / Val(vCount) * 96)


                        '    End If
                        'End If


                        Total_Calculation()
                    End If
                End If
            End If
        End With

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "Error While DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try
    End Sub
    Private Sub Dgv_VortexDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Dgv_VortexDetails.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub
    Private Sub Dgv_VortexDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Dgv_VortexDetails.KeyUp
        Dim n As Integer

        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With Dgv_VortexDetails

                    n = .CurrentRow.Index

                    If .Rows.Count = 1 Then
                        For i = 0 To .Columns.Count - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else

                        .Rows.RemoveAt(n)

                    End If

                    For i = 0 To .Rows.Count - 1
                        .Rows(i).Cells(DgvCol_VortexDetails.SL_NO).Value = i + 1
                    Next

                End With

                Total_Calculation()

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS KEYUP....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub dgv_CardingDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_CardingDetails.LostFocus, Dgv_DrawingDetails.LostFocus, Dgv_VortexDetails.LostFocus, Dgv_WasteDetails.LostFocus
        On Error Resume Next
        Grid_Cell_DeSelect()
    End Sub
    Private Sub dgv_CardingDetails_leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_CardingDetails.Leave, Dgv_DrawingDetails.Leave, Dgv_VortexDetails.Leave, Dgv_WasteDetails.Leave
        On Error Resume Next
        Grid_Cell_DeSelect()
    End Sub
    Private Sub Dgv_WasteDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Dgv_WasteDetails.EditingControlShowing
        Try
            dgtxt_WasteDetails = Nothing
            With Dgv_WasteDetails
                If .Rows.Count > 0 Then
                    dgtxt_WasteDetails = CType(Dgv_WasteDetails.EditingControl, DataGridViewTextBoxEditingControl)
                End If
            End With
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS EDITING SHOWING....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

    End Sub
    Private Sub dgtxt_WasteDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_WasteDetails.Enter
        Try
            dgv_ActCtrlName = Dgv_WasteDetails.Name
            Dgv_WasteDetails.EditingControl.BackColor = Color.Lime
            Dgv_WasteDetails.EditingControl.ForeColor = Color.Blue
            dgtxt_WasteDetails.SelectAll()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS TEXT ENTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub dgtxt_WasteDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_WasteDetails.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS TEXT KEYDOWN....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub dgtxt_WasteDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_WasteDetails.KeyPress
        Try

            With Dgv_WasteDetails


                If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                    e.Handled = True
                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS TEXT KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub dgtxt_WasteDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_WasteDetails.KeyUp
        Dim dgv_DetSlNo As Integer = 0
        Try
            With Dgv_WasteDetails
                If .Rows.Count > 0 Then
                    If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                        Dgv_WasteDetails_KeyUp(sender, e)
                    End If

                    If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
                        Dgv_WasteDetails_KeyUp(sender, e)

                    End If
                End If
            End With

        Catch ex As Exception
            '----
        End Try

    End Sub
    Private Sub dgtxt_WasteDetails_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_WasteDetails.TextChanged
        Try
            With Dgv_WasteDetails

                If .Visible Then
                    If .Rows.Count > 0 Then

                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_WasteDetails.Text)

                    End If
                End If
            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "Error While DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "Error While DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub Dgv_WasteDetails_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles Dgv_WasteDetails.RowsAdded
        Dim n As Integer
        If IsNothing(Dgv_WasteDetails.CurrentCell) Then Exit Sub

        With Dgv_WasteDetails
            n = .RowCount
            .Rows(n - 1).Cells(DgvCol_VortexDetails.SL_NO).Value = Val(n)
        End With
    End Sub
    Private Sub Dgv_WasteDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Dgv_WasteDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim rect As Rectangle

        Try
            With Dgv_WasteDetails


                dgv_ActCtrlName = .Name

                If Val(.CurrentRow.Cells(DgvCol_WasteDetails.SL_NO).Value) = 0 Then
                    .CurrentRow.Cells(DgvCol_WasteDetails.SL_NO).Value = .CurrentRow.Index + 1

                End If

                If e.ColumnIndex = DgvCol_WasteDetails.WASTE_NAME Then

                    If Cbo_WasteGrid_WasteName.Visible = False Or Val(Cbo_WasteGrid_WasteName.Tag) <> e.RowIndex Then

                        Cbo_WasteGrid_WasteName.Tag = -100
                        Da = New SqlClient.SqlDataAdapter("select variety_Name from variety_Head where variety_type ='WASTE' order by variety_Name", con)
                        Dt1 = New DataTable
                        Da.Fill(Dt1)
                        Cbo_WasteGrid_WasteName.DataSource = Dt1
                        Cbo_WasteGrid_WasteName.DisplayMember = "variety_Name"

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        Cbo_WasteGrid_WasteName.Left = .Left + rect.Left
                        Cbo_WasteGrid_WasteName.Top = .Top + rect.Top

                        Cbo_WasteGrid_WasteName.Width = rect.Width
                        Cbo_WasteGrid_WasteName.Height = rect.Height
                        Cbo_WasteGrid_WasteName.Text = .CurrentCell.Value

                        Cbo_WasteGrid_WasteName.Tag = Val(e.RowIndex)
                        Cbo_WasteGrid_WasteName.Visible = True

                        Cbo_WasteGrid_WasteName.BringToFront()
                        Cbo_WasteGrid_WasteName.Focus()

                    End If

                Else
                    Cbo_WasteGrid_WasteName.Visible = False
                End If

            End With

        Catch ex As Exception

        End Try

    End Sub

    Private Sub Dgv_WasteDetails_CellLeave(sender As Object, e As DataGridViewCellEventArgs) Handles Dgv_WasteDetails.CellLeave
        On Error Resume Next
        If IsNothing(Dgv_WasteDetails.CurrentCell) Then Exit Sub
        'Try
        With Dgv_WasteDetails

            If .CurrentCell.ColumnIndex = DgvCol_WasteDetails.WEIGHT Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If
            If .CurrentCell.ColumnIndex = DgvCol_WasteDetails.WASTE_PERCENTAGE Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
        End With

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "Error While DETAILS CELL LEAVE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try
    End Sub
    Private Sub Dgv_WasteDetails_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles Dgv_WasteDetails.CellValueChanged
        ' On Error Resume Next
        If IsNothing(Dgv_WasteDetails.CurrentCell) Then Exit Sub
        Try
            With Dgv_WasteDetails
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If e.ColumnIndex = DgvCol_WasteDetails.WASTE_NAME Or e.ColumnIndex = DgvCol_WasteDetails.WEIGHT Then
                            Formula_Calculation()

                            'If Val(.Rows(.CurrentCell.RowIndex).Cells(DgvCol_WasteDetails.WEIGHT).Value) <> 0 Then

                            '    .Rows(.CurrentCell.RowIndex).Cells(DgvCol_WasteDetails.WASTE_PERCENTAGE).Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(DgvCol_WasteDetails.WEIGHT).Value) / 100, "#########0.00")

                            'End If

                            Total_Calculation()
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub Dgv_WasteDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Dgv_WasteDetails.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub
    Private Sub Dgv_WasteDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Dgv_WasteDetails.KeyUp
        Dim n As Integer

        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With Dgv_VortexDetails

                    n = .CurrentRow.Index

                    If .Rows.Count = 1 Then
                        For i = 0 To .Columns.Count - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else

                        .Rows.RemoveAt(n)

                    End If

                    For i = 0 To .Rows.Count - 1
                        .Rows(i).Cells(DgvCol_WasteDetails.SL_NO).Value = i + 1
                    Next

                End With

                Total_Calculation()

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS KEYUP....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub Cbo_WasteGrid_WasteName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_WasteGrid_WasteName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Variety_Head", "Variety_Name", "(variety_type='WASTE')", "(Variety_IdNo = 0)")
        'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Variety_Head", "Variety_Name", "", "(Variety_IdNo = 0)") 'WASTE
    End Sub

    Private Sub Cbo_WasteGrid_WasteName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_WasteGrid_WasteName.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_WasteGrid_WasteName, Nothing, Nothing, "Variety_Head", "Variety_Name", "(variety_type='WASTE')", "(Variety_IdNo = 0)")

        With Dgv_WasteDetails

            If (e.KeyValue = 38 And Cbo_WasteGrid_WasteName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex <= 0 Then

                    Dgv_VortexDetails.Focus()
                    Dgv_VortexDetails.CurrentCell = Dgv_VortexDetails.Rows(Dgv_VortexDetails.CurrentCell.RowIndex).Cells(DgvCol_VortexDetails.ACTUAL_PRODUCTION)
                    Dgv_VortexDetails.CurrentCell.Selected = True
                End If
            ElseIf (e.KeyValue = 40 And Cbo_WasteGrid_WasteName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If .Rows(.CurrentCell.RowIndex).Cells(DgvCol_WasteDetails.WASTE_NAME).Value = "" Then

                    If MessageBox.Show("Do You Want Save ?", "FOR SAVING", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) = DialogResult.Yes Then
                        save_record()
                    Else
                        cbo_Shift.Focus()
                        Exit Sub
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(DgvCol_WasteDetails.WEIGHT)
                    .CurrentCell.Selected = True
                End If
            End If
        End With

    End Sub

    Private Sub Cbo_WasteGrid_WasteName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_WasteGrid_WasteName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim vLED_Idno As Integer = 0
        Dim cLTH_Idno As Integer = 0
        Dim vCloRate As Single = 0
        Dim trpt_Idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_WasteGrid_WasteName, Nothing, "Variety_Head", "Variety_Name", "(variety_type='WASTE')", "(Variety_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With Dgv_WasteDetails
                If .Rows(.CurrentCell.RowIndex).Cells(DgvCol_WasteDetails.WASTE_NAME).Value = "" Then

                    If MessageBox.Show("Do You Want Save ?", "FOR SAVING", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) = DialogResult.Yes Then
                        save_record()
                    Else
                        cbo_Shift.Focus()
                        Exit Sub
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(DgvCol_WasteDetails.WEIGHT)
                    .CurrentCell.Selected = True
                End If


            End With
        End If

    End Sub

    Private Sub Cbo_WasteGrid_WasteName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_WasteGrid_WasteName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Variety_Creation("WASTE")

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_WasteGrid_WasteName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub Cbo_WasteGrid_WasteName_TextChanged(sender As Object, e As EventArgs) Handles Cbo_WasteGrid_WasteName.TextChanged

        If Cbo_WasteGrid_WasteName.Visible Then
            With Dgv_WasteDetails
                If IsNothing(Dgv_WasteDetails.CurrentCell) Then Exit Sub
                If Val(Cbo_WasteGrid_WasteName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(Cbo_WasteGrid_WasteName.Text)
                End If
            End With
        End If

    End Sub
    Private Sub btn_Formula_Click_1(sender As Object, e As EventArgs) Handles btn_Formula.Click
        pnl_Formula.Visible = True
        'pnl_Formula.Top = 280
        pnl_Formula.Location = New Point(462, 172)
    End Sub
    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        pnl_Formula.Top = 500
        pnl_Formula.Visible = False
        pnl_Formula.Location = New Point(28, 595)
    End Sub
    Private Sub Dgv_DrawingDetails_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles Dgv_DrawingDetails.CellEndEdit
        Try
            With Dgv_DrawingDetails
                If .Visible = True Then
                    If .Rows.Count > 0 Then
                        Dgv_DrawingDetails_CellLeave(sender, e)
                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS CELL End EDIT....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Dgv_VortexDetails_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles Dgv_VortexDetails.CellEndEdit
        Try
            With Dgv_VortexDetails
                If .Visible = True Then
                    If .Rows.Count > 0 Then
                        Dgv_VortexDetails_CellLeave(sender, e)
                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS CELL End EDIT....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Dgv_WasteDetails_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles Dgv_WasteDetails.CellEndEdit
        Try
            With Dgv_WasteDetails
                If .Visible = True Then
                    If .Rows.Count > 0 Then
                        Dgv_WasteDetails_CellLeave(sender, e)
                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error While DETAILS CELL End EDIT....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Grid_Vortex_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Vortex_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_idno = 0)")
    End Sub
    Private Sub cbo_Grid_Vortex_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Vortex_CountName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Vortex_CountName, Nothing, Nothing, "Count_Head", "Count_Name", "", "(Count_idno = 0)")

        With Dgv_VortexDetails

            If e.KeyCode = 40 Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(DgvCol_VortexDetails.ACTUAL_PRODUCTION)
                .CurrentCell.Selected = True


            End If
        End With

    End Sub
    Private Sub cbo_Grid_Vortex_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Vortex_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Vortex_CountName, Nothing, "Count_Head", "Count_Name", "", "(Count_idno = 0)")
        With Dgv_VortexDetails
            If Asc(e.KeyChar) = 13 Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(DgvCol_VortexDetails.ACTUAL_PRODUCTION)
                .CurrentCell.Selected = True


            End If
        End With

    End Sub
    Private Sub cbo_Grid_Vortex_CountName_TextChanged(sender As Object, e As EventArgs) Handles cbo_Grid_Vortex_CountName.TextChanged

        If cbo_Grid_Vortex_CountName.Visible Then
            With Dgv_VortexDetails
                If IsNothing(Dgv_VortexDetails.CurrentCell) Then Exit Sub
                If Val(cbo_Grid_Vortex_CountName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Vortex_CountName.Text)
                End If
            End With
        End If

    End Sub
End Class