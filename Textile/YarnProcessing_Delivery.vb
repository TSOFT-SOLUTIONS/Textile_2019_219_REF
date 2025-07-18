Public Class YarnProcessing_Delivery
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "YNPDE-"
    Private Prec_ActCtrl As New Control

    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""

    Private vcbo_KeyDwnVal As Double
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer
    Private prn_Status As Integer = 0
    Private prn_DetAr(100, 50, 10) As String
    Private prn_InpOpts As String = ""
    Private prn_OriDupTri As String = ""
    Private Total_mtrs As Single = 0
    Private Total_wEIGHT As Double = 0
    Private Total_PCS As Double = 0

    Private Total_Mtrs_Abv80 As Single = 0
    Private Total_Mtrs_40To79 As Double = 0
    Private Total_Mtrs_20To40 As Double = 0

    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private NoFo_STS As Integer = 0
    Private prn_HdIndx As Integer
    Private prn_HdMxIndx As Integer
    Private prn_Count As Integer
    Private prn_HdAr(100, 10) As String
    Private vPrn_PvuEdsCnt As String
    Private vPrn_PvuTotBms As Integer
    Private vPrn_PvuTotMtrs As Single
    Private vPrn_PvuSetNo As String
    Private vPrn_PvuBmNos1 As String
    Private vPrn_PvuBmNos2 As String
    Private vPrn_PvuBmNos3 As String
    Private vPrn_PvuBmNos4 As String
    Private WithEvents dgtxt_details As New DataGridViewTextBoxEditingControl

    Private dgv_LevColNo As Integer = 0

    Public Shared EntFnYrCode As String = ""

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_DcNo.Text = ""
        lbl_DcNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        txt_PoNo.Text = ""
        cbo_Ledger.Text = ""

        cbo_TransportName.Text = ""
        Cbo_ProcessingHEAD.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        txt_Frieght.Text = ""
        txt_Note.Text = ""
        cbo_Filter_MillName.Text = ""

        cbo_Ledger.Enabled = True
        cbo_Ledger.BackColor = Color.White

        txt_PoNo.Enabled = True
        txt_PoNo.BackColor = Color.White

        cbo_Colour.Enabled = True
        cbo_Colour.BackColor = Color.White

        cbo_MillName.Enabled = True
        cbo_MillName.BackColor = Color.White

        cbo_Count.Enabled = True
        cbo_Count.BackColor = Color.White


        dgv_Details.Rows.Clear()


        Grid_DeSelect()

        cbo_Count.Visible = False
        cbo_MillName.Visible = False
        cbo_Colour.Visible = False
       
        cbo_Count.Tag = -1
        cbo_MillName.Tag = -1
        cbo_Colour.Tag = -1
      

        cbo_Count.Text = ""
        cbo_MillName.Text = ""
        cbo_Colour.Text = ""
        

        dgv_Details.Tag = ""
        dgv_LevColNo = -1

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
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

        If Me.ActiveControl.Name <> cbo_Colour.Name Then
            cbo_Colour.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_MillName.Name Then
            cbo_MillName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Count.Name Then
            cbo_Count.Visible = False
        End If
       
        If Me.ActiveControl.Name <> dgv_Details_Total.Name Then
            Grid_DeSelect()
        End If

        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Common_Procedures.Hide_CurrentStock_Display()
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
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim LockSTS As Boolean = False

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(EntFnYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name , c.Ledger_Name as Transport_Name , f.Process_Name from YarnProcessing_Delivery_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Transport_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Process_Head f ON f.Process_IdNo = a.Processing_Idno  Where a.YarnProcessing_Delivery_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_DcNo.Text = dt1.Rows(0).Item("YarnProcessing_Delivery_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("YarnProcessing_Delivery_Date").ToString
                cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                txt_PoNo.Text = dt1.Rows(0).Item("Purchase_OrderNo").ToString
                cbo_TransportName.Text = dt1.Rows(0).Item("Transport_Name").ToString
                txt_Frieght.Text = Format(Val(dt1.Rows(0).Item("Freight_Charges").ToString), "########0.00")
                txt_Note.Text = dt1.Rows(0).Item("Note").ToString
                Cbo_ProcessingHEAD.Text = dt1.Rows(0).Item("Process_Name").ToString
                ' lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, C.Mill_Name,d.Colour_Name from YarnProcessing_Delivery_Details a INNER JOIN Count_Head b ON  b.Count_Idno = a.Count_Idno LEFT OUTER JOIN Mill_Head C ON c.Mill_Idno = a.Mill_Idno LEFT OUTER JOIN Colour_Head d ON d.Colour_IdNo = a.Colour_IdNo  where a.YarnProcessing_Delivery_Code = '" & Trim(NewCode) & "' Order by Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1

                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Count_Name").ToString
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Mill_Name").ToString
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Colour_Name").ToString
                        dgv_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Delivery_Bag").ToString)
                        dgv_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Delivery_Cone").ToString)
                        dgv_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Delivery_Weight").ToString), "########0.000")
                        dgv_Details.Rows(n).Cells(7).Value = Val(dt2.Rows(i).Item("YarnProcessing_Delivery_SlNo").ToString)

                        dgv_Details.Rows(n).Cells(8).Value = ""
                        'dgv_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Receipt_Weight").ToString), "########0.000")

                        da1 = New SqlClient.SqlDataAdapter("select sum(Receipt_Weight) from YarnProcessing_Receipt_Details Where YarnProcessing_Delivery_Code = '" & Trim(NewCode) & "'", con)
                        dt3 = New DataTable
                        da1.Fill(dt3)
                        If dt3.Rows.Count > 0 Then
                            If IsDBNull(dt3.Rows(0)(0).ToString) = False Then
                                If Val(dt3.Rows(0)(0).ToString) > 0 Then
                                    dgv_Details.Rows(n).Cells(8).Value = Val(dt3.Rows(0)(0).ToString)
                                End If
                            End If
                        End If
                        dt3.Clear()

                        If Val(dgv_Details.Rows(n).Cells(8).Value) <> 0 Then
                            For j = 0 To dgv_Details.ColumnCount - 1
                                dgv_Details.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            Next j
                            LockSTS = True
                        End If

                    Next i


                End If

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_Bag").ToString)
                    .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Cone").ToString)
                    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                End With

                If LockSTS = True Then

                    cbo_Ledger.Enabled = False
                    cbo_Ledger.BackColor = Color.LightGray

                    txt_PoNo.Enabled = False
                    txt_PoNo.BackColor = Color.LightGray

                    cbo_Colour.Enabled = False
                    cbo_Colour.BackColor = Color.LightGray

                    cbo_Count.Enabled = False
                    cbo_Count.BackColor = Color.LightGray

                    cbo_MillName.Enabled = False
                    cbo_MillName.BackColor = Color.LightGray

                    Cbo_ProcessingHEAD.Enabled = False
                    Cbo_ProcessingHEAD.BackColor = Color.LightGray


                End If


                Grid_DeSelect()

                dt2.Clear()

                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

           

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

    End Sub

    Private Sub YarnProcessing_Delivery_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_TransportName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_TransportName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_MillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_MillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Count.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Count.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Colour.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COLOUR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Colour.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_ProcessingHEAD.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "PROCESS" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                Cbo_ProcessingHEAD.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub YarnProcessing_Delivery_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim OpYrCode As String = ""

        Me.Text = ""

        If Trim(UCase(Common_Procedures.Proc_Opening_OR_Entry)) = "OPENING" Then
            OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
            OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

            EntFnYrCode = OpYrCode

        Else
            EntFnYrCode = Common_Procedures.FnYearCode

        End If

        con.Open()

        cbo_MillName.Visible = False
        cbo_MillName.Visible = False
        cbo_Colour.Visible = False
       

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        
        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Colour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
       
        AddHandler cbo_TransportName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Frieght.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PoNo.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_ProcessingHEAD.GotFocus, AddressOf ControlGotFocus


        AddHandler cbo_Filter_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_Print_delivery.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_Print_Cancel.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_Print_Cancel.LostFocus, AddressOf ControlLostFocus

        AddHandler btn_Print_delivery.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Colour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Count.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_TransportName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Frieght.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PoNo.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_ProcessingHEAD.LostFocus, AddressOf ControlLostFocus


        AddHandler cbo_Filter_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Frieght.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PoNo.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Frieght.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PoNo.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_filterpono.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_filterpono.KeyPress, AddressOf TextBoxControlKeyPress


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub YarnProcessing_Delivery_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
        Common_Procedures.Hide_CurrentStock_Display()
    End Sub

    Private Sub YarnProcessing_Delivery_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub


                ElseIf pnl_Print.Visible = True Then
                    btn_print_Close_Click(sender, e)
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
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 3 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                cbo_TransportName.Focus()

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
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 3)

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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_Fabric_DeliveryTo_Processing, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_Fabric_DeliveryTo_Processing, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Qa = MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If Qa = Windows.Forms.DialogResult.No Or Qa = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

        Da = New SqlClient.SqlDataAdapter("select sum(Receipt_Weight) from YarnProcessing_Receipt_Details Where YarnProcessing_Delivery_Code = '" & Trim(NewCode) & "'", con)
        'Da = New SqlClient.SqlDataAdapter("select sum(Receipt_Weight) from YarnProcessing_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_Delivery_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already Some Receipt Prepared", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

       

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

            cmd.Connection = con
            cmd.Transaction = trans



            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from YarnProcessing_Delivery_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from YarnProcessing_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_Delivery_Code = '" & Trim(NewCode) & "'"
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




            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 YarnProcessing_Delivery_No from YarnProcessing_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_Delivery_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby, YarnProcessing_Delivery_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 YarnProcessing_Delivery_No from YarnProcessing_Delivery_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_Delivery_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby, YarnProcessing_Delivery_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 YarnProcessing_Delivery_No from YarnProcessing_Delivery_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_Delivery_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby desc, YarnProcessing_Delivery_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 YarnProcessing_Delivery_No from YarnProcessing_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_Delivery_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby desc, YarnProcessing_Delivery_No desc", con)
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

            lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "YarnProcessing_Delivery_Head", "YarnProcessing_Delivery_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Trim(EntFnYrCode))

            lbl_DcNo.ForeColor = Color.Red

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

            inpno = InputBox("Enter Dc.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(EntFnYrCode)

            Da = New SqlClient.SqlDataAdapter("select YarnProcessing_Delivery_No from YarnProcessing_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_Delivery_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Dc No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_Fabric_DeliveryTo_Processing, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_Fabric_DeliveryTo_Processing, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Dc No.", "FOR NEW DELIVERY INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(EntFnYrCode)

            Da = New SqlClient.SqlDataAdapter("select YarnProcessing_Delivery_No from YarnProcessing_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_Delivery_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Dc No", "DOES NOT INSERT NEW DELIVERY...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_DcNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW DELIVERY...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim Cnt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim EntID As String = ""
        Dim vTotBag As Single, vTotCne As Single
        Dim Mill_ID As Integer = 0
        Dim Proc_ID As Integer = 0
        Dim vTotWeight As Single
        Dim Tr_ID As Integer = 0
        Dim itgry_id As Integer = 0
        Dim Nr As Integer = 0

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Entry_Fabric_DeliveryTo_Processing, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If
        If EntFnYrCode = Common_Procedures.FnYearCode Then
            If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
                MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
                Exit Sub
            End If
        End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        Tr_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_TransportName.Text)

        Proc_ID = Common_Procedures.Process_NameToIdNo(con, Cbo_ProcessingHEAD.Text)

        If Proc_ID = 0 Then
            MessageBox.Show("Invalid PROCESSING Name ", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If Cbo_ProcessingHEAD.Enabled And Cbo_ProcessingHEAD.Visible Then
                Cbo_ProcessingHEAD.Focus()
            End If
            Exit Sub
        End If

        With dgv_Details
            For i = 0 To dgv_Details.RowCount - 1
                If Trim(.Rows(i).Cells(1).Value) <> "" Or Val(.Rows(i).Cells(6).Value) <> 0 Then

                    If Trim(dgv_Details.Rows(i).Cells(1).Value) = "" Then
                        MessageBox.Show("Invalid Count Item", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)

                        End If
                        Exit Sub
                    End If

                    If Trim(dgv_Details.Rows(i).Cells(2).Value) = "" Then
                        MessageBox.Show("Invalid Mill Item", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(2)

                        End If
                        Exit Sub

                    End If

                    If Trim(dgv_Details.Rows(i).Cells(3).Value) = "" Then
                        MessageBox.Show("Invalid COLOUR Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(3)

                        End If
                        Exit Sub

                    End If


                    If Val(dgv_Details.Rows(i).Cells(6).Value) = 0 Then
                        MessageBox.Show("Invalid Weight..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled Then dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(6)
                        Exit Sub
                    End If

                End If

            Next
        End With

        Total_Calculation()

        vTotBag = 0 : vTotWeight = 0 : vTotCne = 0

        If dgv_Details_Total.RowCount > 0 Then

            vTotBag = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTotCne = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
            vTotWeight = Val(dgv_Details_Total.Rows(0).Cells(6).Value())

        End If


        tr = con.BeginTransaction

        Try


            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

            Else

                lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "YarnProcessing_Delivery_Head", "YarnProcessing_Delivery_Code", "For_OrderBy", "", Val(lbl_Company.Tag), EntFnYrCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@DeliveryDate", dtp_Date.Value.Date)

            If New_Entry = True Then

                cmd.CommandText = "Insert into YarnProcessing_Delivery_Head(YarnProcessing_Delivery_Code, Company_IdNo, YarnProcessing_Delivery_No, for_OrderBy, YarnProcessing_Delivery_Date, Ledger_IdNo, Purchase_OrderNo, Transport_IdNo, Freight_Charges, Note,Total_Bag,Total_Cone,  Total_Weight , Processing_Idno  ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @DeliveryDate, " & Str(Val(Led_ID)) & ", '" & Trim(txt_PoNo.Text) & "', " & Str(Val(Tr_ID)) & ", " & Str(Val(txt_Frieght.Text)) & ",  '" & Trim(txt_Note.Text) & "'," & Str(Val(vTotBag)) & "," & Str(Val(vTotCne)) & " ,  " & Str(Val(vTotWeight)) & " ,  " & Str(Val(Proc_ID)) & " )"
                cmd.ExecuteNonQuery()
            Else

                cmd.CommandText = "Update YarnProcessing_Delivery_Head set YarnProcessing_Delivery_Date = @DeliveryDate, Processing_Idno = " & Val(Proc_ID) & ",Ledger_IdNo = " & Val(Led_ID) & ", Purchase_OrderNo = '" & Trim(txt_PoNo.Text) & "' , Transport_IdNo = " & Val(Tr_ID) & ", Freight_Charges = " & Val(txt_Frieght.Text) & ",  Note = '" & Trim(txt_Note.Text) & "', Total_Bag = " & Val(vTotBag) & ",Total_Cone = " & Val(vTotCne) & ", Total_Weight = " & Val(vTotWeight) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_Delivery_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If


            'cmd.CommandText = "Insert into entrytempsub(name1, Int1, Int2, Int3, weight1, weight2) Select a.YarnProcessing_Delivery_Code , a.YarnProcessing_Delivery_SlNo, a.Delivery_Bag, a.Delivery_Cone, a.Delivery_Weight, 0 from YarnProcessing_Delivery_Details a Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and a.Delivery_Weight > 0 "
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Insert into entrytempsub(name1, Int1, Int2, Int3, weight1, weight2) Select a.YarnProcessing_Delivery_Code , a.YarnProcessing_Delivery_SlNo, -1*abs(a.Receipt_Bag), -1*abs(a.Receipt_Cone), -1*abs(a.Receipt_Weight), 0 from YarnProcessing_Receipt_Details a Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and a.Receipt_Weight <> 0 "
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Insert into entrytempsub(name1, Int1, Int2, Int3, weight1, weight2) Select a.YarnProcessing_Delivery_Code, a.YarnProcessing_Delivery_SlNo, 0, 0, 0, abs(a.Receipt_Weight) from YarnProcessing_Receipt_Details a Where a.YarnProcessing_Receipt_Code = '" & Trim(NewCode) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " and a.Receipt_Weight > 0 "
            'cmd.ExecuteNonQuery()


            cmd.CommandText = "Delete from YarnProcessing_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_Delivery_Code = '" & Trim(NewCode) & "' and YarnProcessing_Delivery_SlNo NOT IN (Select sq1.YarnProcessing_Delivery_SlNo from YarnProcessing_Receipt_Details sq1 Where (sq1.Receipt_Weight+sq1.Excess_Short_Weight) <> 0 ) "
            cmd.ExecuteNonQuery()
            'cmd.CommandText = "Delete from YarnProcessing_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_Delivery_Code = '" & Trim(NewCode) & "' and Receipt_Weight = 0"
            'cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Partcls = "Delv : Dc.No. " & Trim(lbl_DcNo.Text)
            PBlNo = Trim(lbl_DcNo.Text)
            EntID = Trim(Pk_Condition) & Trim(lbl_DcNo.Text)


            With dgv_Details
                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(4).Value) <> 0 Then
                        Sno = Sno + 1
                        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)
                        Mill_ID = Common_Procedures.Mill_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)
                        Col_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)


                        Sno = Sno + 1

                        Nr = 0
                        cmd.CommandText = "Update  YarnProcessing_Delivery_Details set YarnProcessing_Delivery_Date = @DeliveryDate , Ledger_IdNo = " & Str(Val(Led_ID)) & ", Sl_No  = " & Str(Val(Sno)) & " , Count_Idno = " & Str(Val(Cnt_ID)) & " , Mill_Idno = " & Str(Val(Mill_ID)) & " , Colour_Idno = " & Val(Col_ID) & " ,   Delivery_Bag =  " & Val(.Rows(i).Cells(4).Value) & ", Delivery_Cone = " & Val(.Rows(i).Cells(5).Value) & " ,    Delivery_Weight = " & Str(Val(.Rows(i).Cells(6).Value)) & "   where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_Delivery_Code = '" & Trim(NewCode) & "'  and YarnProcessing_Delivery_Slno = " & Val(.Rows(i).Cells(7).Value)
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then
                            cmd.CommandText = "Insert into YarnProcessing_Delivery_Details(YarnProcessing_Delivery_Code, Company_IdNo, YarnProcessing_Delivery_No, for_OrderBy, YarnProcessing_Delivery_Date,Sl_No, Ledger_IdNo,  Count_Idno,Mill_Idno, Colour_Idno ,  Delivery_Bag  ,  Delivery_Cone ,Delivery_Weight  ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @DeliveryDate, " & Str(Val(Sno)) & ", " & Str(Val(Led_ID)) & " ," & Str(Val(Cnt_ID)) & ", " & Str(Val(Mill_ID)) & ", " & Val(Col_ID) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & "," & Val(.Rows(i).Cells(5).Value) & ", " & Val(.Rows(i).Cells(6).Value) & ")"
                            cmd.ExecuteNonQuery()
                        End If

                        cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code,             Company_IdNo         ,           Reference_No       ,                               for_OrderBy                             , Reference_Date,     DeliveryTo_Idno     ,                            ReceivedFrom_Idno              ,         Entry_ID     ,       Party_Bill_No  ,       Particulars      ,           Sl_No      ,           Count_Idno       ,  Yarn_Type,  Mill_IdNo          ,            Bags      ,                                     Cones,                                  Weight                       ,              Colour_IdNo    ) " & _
                                                " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ",  @DeliveryDate, " & Str(Val(Led_ID)) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Cnt_ID)) & " ,  'MILL', " & Str(Val(Mill_ID)) & "," & Val(.Rows(i).Cells(4).Value) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & "   ,             " & Str(Col_ID) & ") "
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With

            If Val(vTotBag) <> 0 Or Val(vTotCne) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Sl_No, Beam_Width_IdNo, Empty_Beam, Empty_Bags, Empty_Cones, Particulars) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @DeliveryDate, " & Str(Val(Led_ID)) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', 1, 0, 0, " & Str(Val(vTotBag)) & ", " & Str(Val(vTotCne)) & ", '" & Trim(Partcls) & "')"
                cmd.ExecuteNonQuery()
            End If

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()


            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_DcNo.Text)
                End If
            Else
                move_record(lbl_DcNo.Text)
            End If

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()



    End Sub

    
    Private Sub Total_Calculation()
        Dim vTotBags As Single, vTotCns As Single, vtotweight As Single

        Dim i As Integer
        Dim sno As Integer

        vTotBags = 0 : vTotCns = 0 : vtotweight = 0 : sno = 0
        With dgv_Details
            For i = 0 To dgv_Details.Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(0).Value = sno

                If Val(dgv_Details.Rows(i).Cells(6).Value) <> 0 Then
                    '.Rows(i).Cells(9).Value = Val(dgv_Details.Rows(i).Cells(7).Value) * Val(dgv_Details.Rows(i).Cells(8).Value)
                    vTotBags = vTotBags + Val(dgv_Details.Rows(i).Cells(4).Value)
                    vTotCns = vTotCns + Val(dgv_Details.Rows(i).Cells(5).Value)

                    vtotweight = vtotweight + Val(dgv_Details.Rows(i).Cells(6).Value)
                End If
            Next
        End With
        If dgv_Details_Total.Rows.Count <= 0 Then dgv_Details_Total.Rows.Add()
        dgv_Details_Total.Rows(0).Cells(4).Value = Val(vTotBags)
        dgv_Details_Total.Rows(0).Cells(5).Value = Val(vTotCns)

        dgv_Details_Total.Rows(0).Cells(6).Value = Format(Val(vtotweight), "#########0.000")
    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, dtp_Date, Cbo_ProcessingHEAD, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Cbo_ProcessingHEAD, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = ""
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



            If .CurrentCell.ColumnIndex = 6 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If
            Total_Calculation()

            If e.ColumnIndex = 4 Or e.ColumnIndex = 5 Then
                get_MillCount_Details()
            End If
           
        End With
    End Sub
    Private Sub Set_Max_DetailsSlNo(ByVal RowNo As Integer, ByVal DetSlNo_ColNo As Integer)
        Dim MaxSlNo As Integer = 0
        Dim i As Integer

        With dgv_Details
            For i = 0 To .Rows.Count - 1
                If Val(.Rows(i).Cells(DetSlNo_ColNo).Value) > Val(MaxSlNo) Then
                    MaxSlNo = Val(.Rows(i).Cells(DetSlNo_ColNo).Value)
                End If
            Next
            .Rows(RowNo).Cells(DetSlNo_ColNo).Value = Val(MaxSlNo) + 1
        End With

    End Sub
    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle

        With dgv_Details


            'If Val(.Rows(e.RowIndex).Cells(17).Value) = 0 Then
            '    Set_Max_DetailsSlNo(e.RowIndex, 17)
            '    'If e.RowIndex = 0 Then
            '    '    .Rows(e.RowIndex).Cells(15).Value = 1
            '    'Else
            '    '    .Rows(e.RowIndex).Cells(15).Value = Val(.Rows(e.RowIndex - 1).Cells(15).Value) + 1
            '    'End If
            'End If
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 1 Then

                If cbo_Count.Visible = False Or Val(cbo_Count.Tag) <> e.RowIndex Then

                    cbo_MillName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select count_Name from Count_Head  order by Count_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Count.DataSource = Dt1
                    cbo_Count.DisplayMember = "Count_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Count.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Count.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                    cbo_Count.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Count.Height = rect.Height  ' rect.Height
                    cbo_Count.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Count.Tag = Val(e.RowIndex)
                    cbo_Count.Visible = True

                    cbo_Count.BringToFront()
                    cbo_Count.Focus()

                    'cbo_Grid_MillName.Visible = False
                    'cbo_Grid_YarnType.Visible = False

                End If


            Else

                cbo_Count.Visible = False


            End If

            If e.ColumnIndex = 2 Then

                If cbo_MillName.Visible = False Or Val(cbo_MillName.Tag) <> e.RowIndex Then

                    cbo_MillName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Mill_Name from Mill_Head  order by Mill_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_MillName.DataSource = Dt2
                    cbo_MillName.DisplayMember = "Mill_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_MillName.Left = .Left + rect.Left
                    cbo_MillName.Top = .Top + rect.Top
                    cbo_MillName.Width = rect.Width
                    cbo_MillName.Height = rect.Height

                    cbo_MillName.Text = .CurrentCell.Value

                    cbo_MillName.Tag = Val(e.RowIndex)
                    cbo_MillName.Visible = True

                    cbo_MillName.BringToFront()
                    cbo_MillName.Focus()



                End If

            Else

                cbo_MillName.Visible = False

            End If

            If e.ColumnIndex = 3 Then

                If cbo_Colour.Visible = False Or Val(cbo_Colour.Tag) <> e.RowIndex Then

                    cbo_Colour.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
                    Dt3 = New DataTable
                    Da.Fill(Dt3)
                    cbo_Colour.DataSource = Dt3
                    cbo_Colour.DisplayMember = "Colour_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Colour.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Colour.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Colour.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_Colour.Height = rect.Height  ' rect.Height

                    cbo_Colour.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Colour.Tag = Val(e.RowIndex)
                    cbo_Colour.Visible = True

                    cbo_Colour.BringToFront()
                    cbo_Colour.Focus()

                    'cbo_Grid_CountName.Visible = False
                    'cbo_Grid_MillName.Visible = False

                End If

            Else

                cbo_Colour.Visible = False
                'cbo_Grid_MillName.Tag = -1
                'cbo_Grid_MillName.Text = ""

            End If


            'If e.ColumnIndex = 11 And dgv_LevColNo <> 11 Then
            '    Show_Item_CurrentStock(e.RowIndex)
            '    .Focus()
            'End If

            'If e.ColumnIndex <> 9 Then
            '    Common_Procedures.Hide_CurrentStock_Display()
            'End If

        End With
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            dgv_LevColNo = .CurrentCell.ColumnIndex

            If .CurrentCell.ColumnIndex = 6 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Dim i As Integer
        Dim vTotMtrs As Single
        On Error Resume Next


        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 6 Then

                    'If Val(.CurrentCell.ColumnIndex) = 9 Or Val(.CurrentCell.ColumnIndex) = 10 Then
                    '    .Rows(.CurrentCell.RowIndex).Cells(11).Value = Val(dgv_Details.Rows(.CurrentCell.RowIndex).Cells(9).Value) * Val(dgv_Details.Rows(.CurrentCell.RowIndex).Cells(10).Value)
                    'End If
                End If
                Total_Calculation()
            End If
        End With
    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown

        'On Error Resume Next
        On Error Resume Next
        vcbo_KeyDwnVal = e.KeyValue
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
                    cbo_TransportName.Focus()
                End If
            End If
        End With
    End Sub
    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_details.SelectAll()
    End Sub

    Private Sub dgtxt_details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_details.KeyDown
        Try
            With dgv_Details
                vcbo_KeyDwnVal = e.KeyValue
                If e.KeyValue = Keys.Delete Then
                    If Val(.Rows(.CurrentCell.RowIndex).Cells(14).Value) <> 0 Then
                        e.Handled = True
                    End If
                End If
            End With

        Catch ex As Exception
            '----

        End Try
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_details.KeyPress
        Try
            If Val(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(14).Value) <> 0 Then
                e.Handled = True
            End If

            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                e.Handled = True
            End If
        Catch ex As Exception
            '---
        End Try

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer

        Try
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

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer


        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)

            'If Val(.Rows(e.RowIndex).Cells(17).Value) = 0 Then
            '    Set_Max_DetailsSlNo(e.RowIndex, 17)
            '    'If e.RowIndex = 0 Then
            '    '    .Rows(e.RowIndex).Cells(15).Value = 1
            '    'Else
            '    '    .Rows(e.RowIndex).Cells(15).Value = Val(.Rows(e.RowIndex - 1).Cells(15).Value) + 1
            '    'End If
            'End If
        End With

    End Sub



    Private Sub cbo_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Colour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")

    End Sub
    Private Sub cbo_colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Colour, cbo_MillName, Nothing, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(6)
            End If

        End With
    End Sub

    Private Sub cbo_colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Colour.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Colour, Nothing, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(4)

            End With

        End If
    End Sub

    Private Sub cbo_colour_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour.KeyUp
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

    Private Sub cbo_Colour_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Colour.TextChanged
        Try
            If cbo_Colour.Visible Then
                With dgv_Details
                    If Val(cbo_Colour.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Colour.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub cbo_Count_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Count.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_Idno = 0)")
    End Sub

    Private Sub cbo_Count_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Count.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Count, Nothing, cbo_MillName, "Count_Head", "Count_Name", "", "(Count_Idno = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Count.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                Cbo_ProcessingHEAD.Focus()
            End If

            If (e.KeyValue = 40 And cbo_Count.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    cbo_TransportName.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Count_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Count.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Count, cbo_MillName, "Count_Head", "Count_Name", "", "(Count_Idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            e.Handled = True

            With dgv_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    cbo_TransportName.Focus()
                Else
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(2)

                End If
            End With

        End If
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


    Private Sub cbo_Count_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Count.TextChanged
        Try
            If cbo_Count.Visible Then
                With dgv_Details
                    If Val(cbo_Count.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Count.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_Idno = 0)")

    End Sub

    Private Sub cbo_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_MillName, cbo_Count, cbo_Colour, "Mill_Head", "Mill_Name", "", "(Mill_Idno = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_MillName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_MillName, cbo_Colour, "Mill_Head", "Mill_Name", "", "(Mill_Idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If
    End Sub

    Private Sub cbo_MillName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_MillName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_MillName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_MillName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_MillName.TextChanged
        Try
            If cbo_MillName.Visible Then
                With dgv_Details
                    If Val(cbo_MillName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_MillName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub dgv_Details_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.GotFocus
        dgv_Details.Focus()
        'dgv_Details.CurrentCell.Selected = True
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim Led_IdNo As Integer, Cnt_IdNo As Integer, Mill_IdNo As Integer
        Dim Condt As String = ""


        Try

            Condt = ""
            Led_IdNo = 0
            Cnt_IdNo = 0
            Mill_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.YarnProcessing_Delivery_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.YarnProcessing_Delivery_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.YarnProcessing_Delivery_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_CountName.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.Colour_NameToIdNo(con, cbo_Filter_CountName.Text)
            End If

            If Trim(cbo_Filter_MillName.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.Mill_NameToIdNo(con, cbo_Filter_MillName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If


            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " c.Count_Idno = " & Str(Val(Cnt_IdNo))
            End If


            If Val(Mill_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " c.Mill_Idno = " & Str(Val(Mill_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name,c.*,d.Count_Name,e.Mill_Name from YarnProcessing_Delivery_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN YarnProcessing_Delivery_Details c ON c.YarnProcessing_Delivery_Code = a.YarnProcessing_Delivery_Code INNER JOIN Count_Head d ON d.Count_IdNo = c.Count_IdNo LEFT OUTER JOIN Mill_Head e ON c.Mill_Idno = e.Mill_idNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.YarnProcessing_Delivery_Code LIKE '%/" & Trim(EntFnYrCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.YarnProcessing_Delivery_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("YarnProcessing_Delivery_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("YarnProcessing_Delivery_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Count_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Mill_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Delivery_Bag").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Delivery_Cone").ToString)
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Delivery_Weight").ToString), "########0.000")

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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, cbo_Filter_MillName, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) )", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_name", "", "(Count_iDNO = 0)")

    End Sub

    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_CountName.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_CountName, cbo_Filter_PartyName, btn_Filter_Show, "Count_Head", "Count_name", "", "(Count_iDNO = 0)")

    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_CountName, btn_Filter_Show, "Count_Head", "Count_name", "", "(Count_iDNO = 0)")
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

        pnl_Print.Visible = True
        pnl_Back.Enabled = False
        If btn_Print_delivery.Enabled And btn_Print_delivery.Visible Then
            btn_Print_delivery.Focus()
        End If


    End Sub

    Private Sub print_Selection()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & EntFnYrCode

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from YarnProcessing_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and YarnProcessing_Delivery_Code = '" & Trim(NewCode) & "'", con)
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

        prn_InpOpts = ""
        prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. All", "FOR INVOICE PRINTING...", "123")
        prn_InpOpts = Replace(Trim(prn_InpOpts), "4", "123")


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


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & EntFnYrCode

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_Count = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as TransportNamee, f.Process_Name  from YarnProcessing_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.Transport_IdNo = d.Ledger_IdNo LEFT OUTER JOIN Process_Head f ON f.Process_IdNo = a.Processing_Idno  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.YarnProcessing_Delivery_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                Debug.Print(Trim(prn_HdDt.Rows(0).Item("TransportNamee").ToString))

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Colour_Name ,d.Mill_Name  from YarnProcessing_Delivery_Details a LEFT OUTER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo LEFT OUTER JOIN Colour_Head c on a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN Mill_Head d ON d.Mill_IdNo = a.Mill_IdNo   where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.YarnProcessing_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
        Dim p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String, CountNm1 As String, CountNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer
        Dim clrName As String = ""
        Dim Clrln As Integer = 0
        'PrintDocument pd = new PrintDocument();
        'pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
        'pd.Print();

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            Debug.Print(ps.PaperName)
            If ps.Width = 800 And ps.Height = 600 Then
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                PpSzSTS = True
                Exit For
            End If
        Next

        If PpSzSTS = False Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    e.PageSettings.PaperSize = ps
                    PpSzSTS = True
                    Exit For
                End If
            Next

            If PpSzSTS = False Then
                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        e.PageSettings.PaperSize = ps
                        Exit For
                    End If
                Next
            End If

        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20
            .Right = 55
            .Top = 35
            .Bottom = 35
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

        NoofItems_PerPage = 6 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(40) : ClArr(2) = 140 : ClArr(3) = 190 : ClArr(4) = 100 : ClArr(5) = 85 : ClArr(6) = 85
        ClArr(7) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6))

        TxtHgt = 18 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

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

                        CountNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString)
                        CountNm2 = ""
                        If Len(CountNm1) > 15 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(CountNm1), I, 1) = " " Or Mid$(Trim(CountNm1), I, 1) = "," Or Mid$(Trim(CountNm1), I, 1) = "." Or Mid$(Trim(CountNm1), I, 1) = "-" Or Mid$(Trim(CountNm1), I, 1) = "/" Or Mid$(Trim(CountNm1), I, 1) = "_" Or Mid$(Trim(CountNm1), I, 1) = "(" Or Mid$(Trim(CountNm1), I, 1) = ")" Or Mid$(Trim(CountNm1), I, 1) = "\" Or Mid$(Trim(CountNm1), I, 1) = "[" Or Mid$(Trim(CountNm1), I, 1) = "]" Or Mid$(Trim(CountNm1), I, 1) = "{" Or Mid$(Trim(CountNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 15
                            CountNm2 = Microsoft.VisualBasic.Right(Trim(CountNm1), Len(CountNm1) - I)
                            CountNm1 = Microsoft.VisualBasic.Left(Trim(CountNm1), I - 1)
                        End If


                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 20 Then
                            For I = 20 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 20
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        Dim ClrNm1 As String, ClrNm2 As String

                        ClrNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Colour_Name").ToString)
                        ClrNm2 = ""
                        If Len(ClrNm1) > 12 Then
                            For I = 12 To 1 Step -1
                                If Mid$(Trim(ClrNm1), I, 1) = "@" Or Mid$(Trim(ClrNm1), I, 1) = " " Or Mid$(Trim(ClrNm1), I, 1) = "," Or Mid$(Trim(ClrNm1), I, 1) = "." Or Mid$(Trim(ClrNm1), I, 1) = "-" Or Mid$(Trim(ClrNm1), I, 1) = "/" Or Mid$(Trim(ClrNm1), I, 1) = "_" Or Mid$(Trim(ClrNm1), I, 1) = "(" Or Mid$(Trim(ClrNm1), I, 1) = ")" Or Mid$(Trim(ClrNm1), I, 1) = "\" Or Mid$(Trim(ClrNm1), I, 1) = "[" Or Mid$(Trim(ClrNm1), I, 1) = "]" Or Mid$(Trim(ClrNm1), I, 1) = "{" Or Mid$(Trim(ClrNm1), I, 1) = "}" Or Mid$(Trim(ClrNm1), I, 1) = "@" Then Exit For
                            Next I
                            If I = 0 Then I = 12
                            ClrNm2 = Microsoft.VisualBasic.Right(Trim(ClrNm1), Len(ClrNm1) - I)
                            ClrNm1 = Microsoft.VisualBasic.Left(Trim(ClrNm1), I)
                        End If

                        CurY = CurY + TxtHgt
                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(CountNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)

                        p1Font = New Font("Calibri", 8, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, ClrNm1, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, p1Font)



                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Bag").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Bag").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Cone").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Cone").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Weight").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Or Trim(ClrNm2) <> "" Or Trim(CountNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(CountNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, ClrNm2, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, p1Font)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

                If Trim(prn_InpOpts) <> "" Then
                    If prn_Count < Len(Trim(prn_InpOpts)) Then


                        If Val(prn_InpOpts) <> "0" Then
                            prn_DetIndx = 0
                            prn_DetSNo = 0
                            prn_PageNo = 0

                            e.HasMorePages = True
                            Return
                        End If

                    End If
                End If

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
        Dim C1 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim S As String

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Colour_Name ,d.Mill_Name   from YarnProcessing_Delivery_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo LEFT OUTER JOIN Colour_Head c on a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN Mill_Head d ON d.Mill_IdNo = a.Mill_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.YarnProcessing_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 4 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If
        End If
        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

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
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_No = "GST NO.: " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
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
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "YARN PROCESSING DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height



        CurY = CurY + strHeight  ' + 150
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString("PROCESSING  : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO :    ", pFont).Width

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("YarnProcessing_Delivery_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("YarnProcessing_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            pFont = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GST No: " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "PROCESSING", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Process_Name").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            'If Trim(prn_HdDt.Rows(0).Item("Purchase_OrderNo").ToString) <> "" Then
            '    CurY = CurY + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "P.O.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Purchase_OrderNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            'End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COLOUR", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "BALES NOS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CONE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim W1 As Single = 0
        Dim vprn_BlNos As String = ""
        Dim NoteStr1 As String = ""
        Dim NoteStr2 As String = ""



        W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt



                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + ClAr(2) + 10, CurY, 2, ClAr(4), pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_bAG").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bag").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Cone").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cone").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                End If
            End If


            CurY = CurY + TxtHgt - 15

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))

            ' CurY = CurY + TxtHgt - 5



            'vprn_BlNos = ""
            'For I = 0 To prn_DetDt.Rows.Count - 1
            '    If Trim(prn_DetDt.Rows(I).Item("Bales_Nos").ToString) <> "" Then
            '        vprn_BlNos = Trim(vprn_BlNos) & IIf(Trim(vprn_BlNos) <> "", ", ", "") & prn_DetDt.Rows(I).Item("Bales_Nos").ToString
            '    End If
            'Next

            ' CurY = CurY + TxtHgt
           

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("TransportNamee").ToString) <> "" Then

                Common_Procedures.Print_To_PrintDocument(e, "Transport Name : " & Trim(prn_HdDt.Rows(0).Item("TransportNamee").ToString), LMargin + 10, CurY, 0, 0, pFont)


                CurY = CurY + TxtHgt + 10
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                LnAr(7) = CurY

            End If
            If Trim(prn_HdDt.Rows(0).Item("Note").ToString) <> "" Then

                CurY = CurY + TxtHgt
                p1Font = New Font("Calibri", 9, FontStyle.Regular)
                NoteStr1 = "( Note: " & Trim(prn_HdDt.Rows(0).Item("Note").ToString) & " )"
                If Len(NoteStr1) > 90 Then
                    For I = 90 To 1 Step -1
                        If Mid$(Trim(NoteStr1), I, 1) = " " Or Mid$(Trim(NoteStr1), I, 1) = "," Or Mid$(Trim(NoteStr1), I, 1) = "." Or Mid$(Trim(NoteStr1), I, 1) = "-" Or Mid$(Trim(NoteStr1), I, 1) = "/" Or Mid$(Trim(NoteStr1), I, 1) = "_" Or Mid$(Trim(NoteStr1), I, 1) = "(" Or Mid$(Trim(NoteStr1), I, 1) = ")" Or Mid$(Trim(NoteStr1), I, 1) = "\" Or Mid$(Trim(NoteStr1), I, 1) = "[" Or Mid$(Trim(NoteStr1), I, 1) = "]" Or Mid$(Trim(NoteStr1), I, 1) = "{" Or Mid$(Trim(NoteStr1), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 90
                    NoteStr2 = Microsoft.VisualBasic.Right(Trim(NoteStr1), Len(NoteStr1) - I)
                    NoteStr1 = Microsoft.VisualBasic.Left(Trim(NoteStr1), I)
                End If
                Common_Procedures.Print_To_PrintDocument(e, NoteStr1, LMargin + 10, CurY, 0, 0, p1Font)
                If NoteStr2 <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, NoteStr2, LMargin + 10, CurY, 0, 0, p1Font)
                End If


            End If

            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 300, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub



   
    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Note.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Note.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_TransportName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TransportName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub


    Private Sub cbo_TransportName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TransportName, Nothing, txt_Frieght, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        If e.KeyValue = 38 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                dgv_Details.CurrentCell.Selected = True

            Else
                Cbo_ProcessingHEAD.Focus()

            End If
        End If

    End Sub

    Private Sub cbo_Transportname_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TransportName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TransportName, txt_Frieght, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_TransportName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "TRANSPORT"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_TransportName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        print_record()
    End Sub

    
    Private Sub txt_Frieght_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Frieght.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub Cbo_ProcessingHEAD_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_ProcessingHEAD.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Process_Head", "Process_Name", "", "(Process_Idno=0)")

    End Sub

    Private Sub Cbo_ProcessingHEAD_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_ProcessingHEAD.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_ProcessingHEAD, cbo_Ledger, Nothing, "Process_Head", "Process_Name", "", "(Process_Idno=0)")
        If (e.KeyValue = 40 And Cbo_ProcessingHEAD.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            Else
                cbo_TransportName.Focus()

            End If
        End If
    End Sub

    Private Sub Cbo_ProcessingHEAD_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_ProcessingHEAD.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_ProcessingHEAD, Nothing, "Process_Head", "Process_Name", "", "(Process_Idno=0)")
        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            Else
                cbo_TransportName.Focus()

            End If
        End If
    End Sub

    Private Sub Cbo_ProcessingHEAD_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_ProcessingHEAD.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Process_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_ProcessingHEAD.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
  
   

    Private Sub Add_NewRow_ToGrid()
        On Error Resume Next

        Dim i As Integer
        Dim n As Integer = -1

        With dgv_Details
            If .Visible Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 Then

                    n = .Rows.Add()

                    For i = 0 To .Columns.Count - 1
                        .Rows(n).Cells(i).Value = .Rows(.CurrentCell.RowIndex).Cells(i).Value
                        .Rows(.CurrentCell.RowIndex).Cells(i).Value = ""
                    Next

                    For i = 0 To .Rows.Count - 1
                        .Rows(i).Cells(0).Value = i + 1
                    Next

                    .CurrentCell = .Rows(n).Cells(.CurrentCell.ColumnIndex)
                    .CurrentCell.Selected = True


                End If

            End If

        End With

    End Sub
   
  

   

   

    Private Sub dgtxt_details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_details.KeyUp

        Try
            With dgv_Details
                If .Rows.Count > 0 Then
                    If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                        dgv_Details_KeyUp(sender, e)
                    End If

                   
                End If
            End With

        Catch ex As Exception
            '----
        End Try
    End Sub

    

   

    Private Sub btn_Print_delivery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_delivery.Click
        print_Selection()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Private Sub btn_SMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SendSMS.Click
        Dim i As Integer = 0
        Dim smstxt As String = ""
        Dim PhNo As String = "", AgPNo As String = ""
        Dim Led_IdNo As Integer = 0, Agnt_IdNo As Integer = 0
        Dim SMS_SenderID As String = ""
        Dim SMS_Key As String = ""
        Dim SMS_RouteID As String = ""
        Dim SMS_Type As String = ""
        Dim BlNos As String = ""

        Try

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

            PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "MobileNo_Frsms", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")

           

            If Trim(AgPNo) <> "" Then
                PhNo = Trim(PhNo) & IIf(Trim(PhNo) <> "", ",", "") & Trim(AgPNo)
            End If

            smstxt = Trim(cbo_Ledger.Text) & Chr(13)
            smstxt = smstxt & " Dc.No : " & Trim(lbl_DcNo.Text) & Chr(13)
            smstxt = smstxt & " Date : " & Trim(dtp_Date.Text) & Chr(13)
            If Trim(cbo_TransportName.Text) <> "" Then
                smstxt = smstxt & " Transport : " & Trim(cbo_TransportName.Text) & Chr(13)
            End If
            'If Trim(txt_LNo.Text) <> "" Then
            '    smstxt = smstxt & " Lr No : " & Trim(txt_LrNo.Text) & Chr(13)
            '    If Trim(msk_Lr_Date.Text) <> "" Then
            '        smstxt = smstxt & " Dt : " & Trim(msk_Lr_Date.Text) & Chr(13)
            '    End If
            'End If
            'If Trim(cbo_DespTo.Text) <> "" Then
            '    smstxt = smstxt & " Despatch To : " & Trim(cbo_DespTo.Text) & Chr(13)
            'End If
            If dgv_Details_Total.RowCount > 0 Then
                smstxt = smstxt & " No.Of Bales : " & Val((dgv_Details_Total.Rows(0).Cells(6).Value())) & Chr(13)
                BlNos = ""
                For i = 0 To dgv_Details.Rows.Count - 1
                    If Val(dgv_Details_Total.Rows(0).Cells(9).Value()) <> 0 Then
                        BlNos = BlNos & IIf(Trim(BlNos) <> "", ", ", "") & Trim(dgv_Details.Rows(0).Cells(7).Value)
                    End If
                Next
                smstxt = smstxt & " Bales No.s : " & Trim(BlNos) & Chr(13)
                smstxt = smstxt & " Pcs : " & Val(dgv_Details_Total.Rows(0).Cells(8).Value()) & Chr(13)
                smstxt = smstxt & " Meters : " & Val(dgv_Details_Total.Rows(0).Cells(11).Value()) & Chr(13)
            End If
            'If dgv_Details.RowCount > 0 Then
            '    smstxt = smstxt & " No.Of Bales : " & Val((dgv_Details.Rows(0).Cells(4).Value())) & Chr(13)
            '    smstxt = smstxt & " Meters : " & Val((dgv_Details.Rows(0).Cells(7).Value())) & Chr(13)
            'End If
            'smstxt = smstxt & " Bill Amount : " & Trim(lbl_NetAmt.Text) & Chr(13)
            'smstxt = smstxt & " " & Chr(13)
            smstxt = smstxt & " Thanks! " & Chr(13)
            smstxt = smstxt & Common_Procedures.Company_IdNoToName(con, Val(lbl_Company.Tag))

            SMS_SenderID = ""
            SMS_Key = ""
            SMS_RouteID = ""
            SMS_Type = ""

            Common_Procedures.get_SMS_Provider_Details(con, Val(lbl_Company.Tag), SMS_SenderID, SMS_Key, SMS_RouteID, SMS_Type)


            Sms_Entry.vSmsPhoneNo = Trim(PhNo)
            Sms_Entry.vSmsMessage = Trim(smstxt)

            Sms_Entry.SMSProvider_SenderID = SMS_SenderID
            Sms_Entry.SMSProvider_Key = SMS_Key
            Sms_Entry.SMSProvider_RouteID = SMS_RouteID
            Sms_Entry.SMSProvider_Type = SMS_Type

            Dim f1 As New Sms_Entry
            f1.MdiParent = MDIParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_SaveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SaveAll.Click
        SaveAll_STS = True

        LastNo = ""
        movelast_record()

        LastNo = lbl_DcNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_DcNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub


    Private Sub cbo_Filter_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_Idno = 0)")
    End Sub

    Private Sub cbo_Filter_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_MillName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_MillName, dtp_Filter_ToDate, cbo_Filter_PartyName, "Mill_Head", "Mill_Name", "", "(Mill_Idno = 0)")
    End Sub

    Private Sub cbo_Filter_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_MillName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_MillName, cbo_Filter_PartyName, "Mill_Head", "Mill_Name", "", "(Mill_Idno = 0)")
    End Sub
    Private Sub get_MillCount_Details()
        Dim q As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Cn_bag As Single
        Dim Wgt_Bag As Single
        Dim Wgt_Cn As Single
        Dim CntID As Integer
        Dim MilID As Integer

        CntID = Common_Procedures.Count_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(1).Value)
        MilID = Common_Procedures.Mill_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(2).Value)

        Wgt_Bag = 0 : Wgt_Cn = 0 : Cn_bag = 0

        If CntID <> 0 And MilID <> 0 Then

            Da = New SqlClient.SqlDataAdapter("select * from Mill_Count_Details where mill_idno = " & Str(Val(MilID)) & " and count_idno = " & Str(Val(CntID)), con)
            Da.Fill(Dt)
            With dgv_Details

                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        Wgt_Bag = Dt.Rows(0).Item("Weight_Bag").ToString
                        Wgt_Cn = Dt.Rows(0).Item("Weight_Cone").ToString
                        Cn_bag = Dt.Rows(0).Item("Cones_Bag").ToString
                    End If
                End If

                Dt.Clear()
                Dt.Dispose()
                Da.Dispose()

                If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                    If .CurrentCell.ColumnIndex = 4 Then
                        If Val(Cn_bag) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(5).Value = .Rows(.CurrentRow.Index).Cells(4).Value * Val(Cn_bag)
                        End If

                        If Val(Wgt_Bag) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(6).Value = Format(Val(.Rows(.CurrentRow.Index).Cells(4).Value) * Val(Wgt_Bag), "#########0.000")
                        End If

                    End If

                    If .CurrentCell.ColumnIndex = 5 Then
                        If Val(Wgt_Cn) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(6).Value = Format(.Rows(.CurrentRow.Index).Cells(5).Value * Val(Wgt_Cn), "##########0.000")
                        End If

                    End If

                End If

            End With

        End If

    End Sub

End Class