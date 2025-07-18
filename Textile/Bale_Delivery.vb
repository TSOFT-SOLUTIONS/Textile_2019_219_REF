Public Class Bale_Delivery
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Filter_RowNo As Integer = -1
    Private Pk_Condition As String = "BADLV-"

    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(500, 500, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private prn_HdIndx As Integer
    Private prn_HdIndx1 As Integer
    Private prn_HdMxIndx As Integer
    Private prn_Count As Integer
    Private prn_HdAr(100, 10) As String
    Private prn_HeadIndx As Integer
    Private prn_Prev_HeadIndx As Integer
    Private prn_InpOpts As String = ""
    Private prn_OriDupTri As String = ""
    Private prn_TotCopies As Integer = 0
    Private prn_DetAr1(500, 10) As String
    Private prn_DetDt1 As New DataTable
    Private prn_BLDetAr(1000, 10) As String
    Private prn_TotBlMtr As Single = 0
    Private prn_TotBlWgt As Single = 0
    Private prn_TotBls As Integer = 0
    Private prn_NoofBaleDets As Integer
    Private prn_BaleCode1 As String = ""
    Private prn_BaleCode2 As String = ""
    Public Shared EntFnYrCode As String = ""

    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Public vmskLrText As String = ""
    Public vmskLrStrt As Integer = -1


    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        pnl_BaleSelection.Visible = False
        pnl_BaleSelection_ToolTip.Visible = False
        pnl_Print.Visible = False

        vmskOldText = ""
        vmskSelStrt = -1

        vmskLrText = ""
        vmskLrStrt = -1


        lbl_DcNo.Text = ""
        lbl_DcNo.ForeColor = Color.Black

        msk_date.Text = ""
        dtp_Date.Text = ""
        cbo_PartyName.Text = ""
        
        cbo_Grid_ClothName.Text = ""
        cbo_Grid_Clothtype.Text = ""
        cbo_RollBundle.Text = "BALE"
        

       
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        txt_Note.Text = ""
       

        cbo_PartyName.Enabled = True
        cbo_PartyName.BackColor = Color.White

       

        
        msk_date.Enabled = True
        msk_date.BackColor = Color.White


        cbo_Grid_ClothName.Enabled = True
        cbo_Grid_ClothName.BackColor = Color.White

        cbo_Grid_Clothtype.Enabled = True
        cbo_Grid_Clothtype.BackColor = Color.White

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        dgv_BaleDetails.Rows.Clear()

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ClothName.Text = ""
            cbo_Filter_ClothName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()

        cbo_Grid_ClothName.Visible = False
        cbo_Grid_Clothtype.Visible = False

        NoCalc_Status = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim Msktxbx As MaskedTextBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Or TypeOf Me.ActiveControl Is Button Then
            Me.ActiveControl.BackColor = Color.SpringGreen
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            Msktxbx = Me.ActiveControl
            Msktxbx.SelectionStart = 0
        End If

        If Me.ActiveControl.Name <> cbo_Grid_ClothName.Name Then
            cbo_Grid_ClothName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Clothtype.Name Then
            cbo_Grid_Clothtype.Visible = False
        End If
        If Me.ActiveControl.Name <> dgv_Details.Name And Not (TypeOf ActiveControl Is DataGridViewTextBoxEditingControl) Then
            pnl_BaleSelection_ToolTip.Visible = False
        End If

        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Grid_DeSelect()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(41, 57, 85)
                Prec_ActCtrl.ForeColor = Color.White
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
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub ClothSales_Cloth_Delivery_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
           
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_ClothName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH NAME" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_ClothName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Clothtype.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH TYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Clothtype.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub ClothSales_Cloth_Delivery_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub ClothSales_Cloth_Delivery_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try

            If Asc(e.KeyChar) = 27 Then

                'If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                '    Exit Sub
                'End If

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

               

                ElseIf pnl_BaleSelection.Visible = True Then
                    btn_Close_BaleSelection_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Print.Visible = True Then
                    btn_print_Close_Click(sender, e)
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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub ClothSales_Cloth_Delivery_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim dt8 As New DataTable
        Dim OpYrCode As String = ""
        Me.Text = ""

        con.Open()

        If Trim(UCase(Common_Procedures.ClothDelivery_Opening_OR_Entry)) = "OPENING" Then
            OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
            OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

            EntFnYrCode = OpYrCode
            btn_BaleSelection.Visible = False

        Else
            EntFnYrCode = Common_Procedures.FnYearCode
            btn_BaleSelection.Visible = True
        End If


        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where ( Ledger_IdNo = 0 or (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ) order by Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_PartyName.DataSource = dt1
        cbo_PartyName.DisplayMember = "Ledger_DisplayName"

       

        da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
        da.Fill(dt4)
        cbo_Grid_ClothName.DataSource = dt4
        cbo_Grid_ClothName.DisplayMember = "Cloth_Name"

        da = New SqlClient.SqlDataAdapter("select ClothType_Name from ClothType_Head order by ClothType_Name", con)
        da.Fill(dt5)
        cbo_Grid_Clothtype.DataSource = dt5
        cbo_Grid_Clothtype.DisplayMember = "ClothType_Name"

      

        cbo_RollBundle.Items.Clear()
        cbo_RollBundle.Items.Add(" ")
        cbo_RollBundle.Items.Add("ROLL")
        cbo_RollBundle.Items.Add("BUNDLE")
        cbo_RollBundle.Items.Add("BALE")

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        

        pnl_BaleSelection.Visible = False
        pnl_BaleSelection.Left = (Me.Width - pnl_BaleSelection.Width) \ 2
        pnl_BaleSelection.Top = (Me.Height - pnl_BaleSelection.Height) \ 2
        pnl_BaleSelection.BringToFront()

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()

        dgv_BaleDetails.Visible = False

        pnl_BaleSelection_ToolTip.Visible = False

        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Clothtype.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Bale.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Cancel.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Delivery.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_FormJJ.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_RollBundle.GotFocus, AddressOf ControlGotFocus


        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Clothtype.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Bale.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Cancel.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Delivery.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_FormJJ.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_RollBundle.LostFocus, AddressOf ControlLostFocus


        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
       

        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
      
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

                        If .CurrentCell.ColumnIndex > 7 Then

                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_Note.Focus()

                            Else

                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If
                        ElseIf .CurrentCell.ColumnIndex = 3 Then
                            If MessageBox.Show("Do you want to select Bale", "FOR BALE SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                                Bale_Selection()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(3)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 5 Then

                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(11)
                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                cbo_RollBundle.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(7)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 11 Then

                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(5)

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
        Dim LockSTS As Boolean = False
        Dim n As Integer
        Dim SNo As Integer

        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(EntFnYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from ClothSales_Delivery_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Delivery_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_DcNo.Text = dt1.Rows(0).Item("ClothSales_Delivery_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("ClothSales_Delivery_Date").ToString
                msk_date.Text = dtp_Date.Text

                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                
                txt_Note.Text = dt1.Rows(0).Item("Note").ToString


                cbo_RollBundle.Text = dt1.Rows(0).Item("Packing_Type").ToString
                

                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))



                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Cloth_Name, c.ClothType_Name from ClothSales_Delivery_Details a LEFT OUTER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo LEFT OUTER JOIN ClothType_Head c ON a.ClothType_IdNo = c.ClothType_IdNo Where a.ClothSales_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
                            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                            .Rows(n).Cells(2).Value = dt2.Rows(i).Item("ClothType_Name").ToString
                            If Val(dt2.Rows(i).Item("Fold_Perc").ToString) <> 0 Then
                                .Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Fold_Perc").ToString)
                            End If
                            If Val(dt2.Rows(i).Item("Bales").ToString) <> 0 Then
                                .Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Bales").ToString)
                            End If
                            .Rows(n).Cells(5).Value = dt2.Rows(i).Item("Bales_Nos").ToString
                            If Val(dt2.Rows(i).Item("Pcs").ToString) <> 0 Then
                                .Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Pcs").ToString)
                            End If
                            .Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                            
                            .Rows(n).Cells(8).Value = dt2.Rows(i).Item("ClothSales_Delivery_SlNo").ToString
                            .Rows(n).Cells(9).Value = Val(dt2.Rows(i).Item("Invoice_Meters").ToString) + Val(dt2.Rows(i).Item("Return_Meters").ToString)
                            .Rows(n).Cells(10).Value = dt2.Rows(i).Item("PackingSlip_Codes").ToString
                            .Rows(n).Cells(11).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "#######0.00")

                            

                        Next i

                    End If

                    If .Rows.Count = 0 Then
                        .Rows.Add()

                    Else

                        n = .Rows.Count - 1
                        If Trim(.Rows(n).Cells(1).Value) = "" And Val(.Rows(n).Cells(7).Value) = 0 Then
                            .Rows(n).Cells(8).Value = ""
                            If Val(.Rows(n).Cells(8).Value) = 0 Then
                                If n = 0 Then
                                    .Rows(n).Cells(8).Value = 1
                                Else
                                    .Rows(n).Cells(8).Value = Val(.Rows(n - 1).Cells(8).Value) + 1
                                End If
                            End If
                        End If

                    End If

                End With

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_Bales").ToString)
                    .Rows(0).Cells(6).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                End With


                da2 = New SqlClient.SqlDataAdapter("Select a.* from Packing_Slip_Head a Where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Delivery_DetailsSlNo, a.Delivery_No, a.Packing_Slip_Date, a.for_OrderBy, a.Packing_Slip_No, a.Packing_Slip_Code", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_BaleDetails

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(dt2.Rows(i).Item("Delivery_DetailsSlNo").ToString)
                            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Packing_Slip_No").ToString
                            .Rows(n).Cells(2).Value = Val(dt2.Rows(i).Item("Total_Pcs").ToString)
                            .Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Total_Meters").ToString)
                            '.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Weight").ToString)
                            .Rows(n).Cells(4).Value = dt2.Rows(i).Item("Packing_Slip_Code").ToString
                            .Rows(n).Cells(5).Value = dt2.Rows(i).Item("Bale_Bundle").ToString

                        Next i

                    End If

                End With


            End If

            Grid_Cell_DeSelect()

            If LockSTS = True Then
                cbo_PartyName.Enabled = False
                cbo_PartyName.BackColor = Color.LightGray

                

                cbo_Grid_ClothName.Enabled = False
                cbo_Grid_ClothName.BackColor = Color.LightGray

                cbo_Grid_Clothtype.Enabled = False
                cbo_Grid_Clothtype.BackColor = Color.LightGray

            End If



        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()

        End Try

        NoCalc_Status = False

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

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

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

       

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

            cmd.Connection = con
            cmd.Transaction = trans

           

            cmd.CommandText = "Update Packing_Slip_Head set Delivery_Code = '', Delivery_No = '', Delivery_DetailsSlNo = 0, Delivery_Increment = Delivery_Increment - 1, Delivery_Date = Null Where Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from ClothSales_Delivery_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from ClothSales_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code = '" & Trim(NewCode) & "'"
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

            If msk_date.Enabled = True And msk_date.Visible = True Then msk_date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead order by Ledger_DisplayName", con)
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
        pnl_Back.Enabled = False
        If Filter_Status = True Then
            If dgv_Filter_Details.Rows.Count > 0 And Filter_RowNo >= 0 Then
                dgv_Filter_Details.Focus()
                dgv_Filter_Details.CurrentCell = dgv_Filter_Details.Rows(Filter_RowNo).Cells(0)
                dgv_Filter_Details.CurrentCell.Selected = True
            Else
                If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

            End If

        Else
            If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

        End If
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 ClothSales_Delivery_No from ClothSales_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " AND ClothSales_Delivery_Code like '" & Trim(Pk_Condition) & "%' and ClothSales_Delivery_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby, ClothSales_Delivery_No", con)
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
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 ClothSales_Delivery_No from ClothSales_Delivery_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code like '" & Trim(Pk_Condition) & "%' and ClothSales_Delivery_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby, ClothSales_Delivery_No", con)
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
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 ClothSales_Delivery_No from ClothSales_Delivery_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code like '" & Trim(Pk_Condition) & "%' and ClothSales_Delivery_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby desc, ClothSales_Delivery_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 ClothSales_Delivery_No from ClothSales_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code like '" & Trim(Pk_Condition) & "%' and ClothSales_Delivery_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby desc, ClothSales_Delivery_No desc", con)
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
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        Try
            clear()

            New_Entry = True

            lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "ClothSales_Delivery_Head", "ClothSales_Delivery_Code", "For_OrderBy", " ClothSales_Delivery_Code like '" & Trim(Pk_Condition) & "%'", Val(lbl_Company.Tag), EntFnYrCode)
            lbl_DcNo.ForeColor = Color.Red



            msk_date.Text = Date.Today.ToShortDateString
            Da = New SqlClient.SqlDataAdapter("select top 1 * from ClothSales_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code like '%/" & Trim(EntFnYrCode) & "' AND ClothSales_Delivery_Code like '" & Trim(Pk_Condition) & "%' Order by for_Orderby desc, ClothSales_Delivery_No desc", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If Dt1.Rows(0).Item("ClothSales_Delivery_Date").ToString <> "" Then msk_date.Text = Dt1.Rows(0).Item("ClothSales_Delivery_Date").ToString
                End If
            End If
            Dt1.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()

            If msk_date.Enabled And msk_date.Visible Then
                msk_date.Focus()
                msk_date.SelectionStart = 0
            End If

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        Try

            inpno = InputBox("Enter Dc No.", "FOR FINDING...")

            InvCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(EntFnYrCode)

            Da = New SqlClient.SqlDataAdapter("select ClothSales_Delivery_No from ClothSales_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code = '" & Trim(InvCode) & "'", con)
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
                MessageBox.Show("Dc No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to DELETE", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Dc No.", "FOR NEW Dc NO. INSERTION...")

            InvCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(EntFnYrCode)

            Da = New SqlClient.SqlDataAdapter("select ClothSales_Delivery_No from ClothSales_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code = '" & Trim(InvCode) & "'", con)
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
                    MessageBox.Show("Invalid Dc No.", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_DcNo.Text = Trim(UCase(inpno))

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
        Dim clth_ID As Integer = 0
        Dim FP_ID As Integer = 0
        Dim clthtyp_ID As Integer = 0
        Dim Led_ID As Integer = 0
        Dim Ag_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim vTotPcs As Single, vTotMtrs As Single, vTotBals As Single
        Dim Cr_ID As Integer = 0
        Dim Dr_ID As Integer = 0
        Dim Smp_STS As Integer = 0
        Dim Nr As Integer = 0
        Dim OrdCd As String = ""
        Dim OrdSlNo As Long = 0
        Dim DcCd As String = ""
        Dim DcSlNo As Long = 0
        Dim OpYrCode As String = ""
        Dim Usr_ID As Integer = 0

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Common_Procedures.UserRight_Check(Common_Procedures.UR.ClothSales_Cloth_Delivery_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        If EntFnYrCode = Common_Procedures.FnYearCode Then
            If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
                MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
                Exit Sub
            End If

        End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled Then cbo_PartyName.Focus()
            Exit Sub
        End If


        lbl_UserName.Text = Common_Procedures.User.IdNo

      

        For i = 0 To dgv_Details.RowCount - 1

            If Val(dgv_Details.Rows(i).Cells(6).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(7).Value) <> 0 Then

                clth_ID = Common_Procedures.Cloth_NameToIdNo(con, dgv_Details.Rows(i).Cells(1).Value)
                If clth_ID = 0 Then
                    MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)
                    End If
                    Exit Sub
                End If

                clthtyp_ID = Common_Procedures.ClothType_NameToIdNo(con, dgv_Details.Rows(i).Cells(2).Value)
                If clthtyp_ID = 0 Then
                    MessageBox.Show("Invalid Cloth Type Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(2)
                    End If
                    Exit Sub
                End If

                If Val(dgv_Details.Rows(i).Cells(3).Value) = 0 Then
                    MessageBox.Show("Invalid Folding", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(3)
                    End If
                    Exit Sub
                End If

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1446" Then '----Lourdu Matha
                    If Val(dgv_Details.Rows(i).Cells(6).Value) = 0 Then
                        MessageBox.Show("Invalid Pcs", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(6)
                        End If
                        Exit Sub
                    End If
                Else
                    If Val(dgv_Details.Rows(i).Cells(7).Value) = 0 Then
                        MessageBox.Show("Invalid metres", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(7)
                        End If
                        Exit Sub
                    End If
                End If


            End If

        Next

        NoCalc_Status = False
        Total_Calculation()

        vTotPcs = 0 : vTotMtrs = 0 : vTotBals = 0

        If dgv_Details_Total.RowCount > 0 Then
            vTotBals = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTotPcs = Val(dgv_Details_Total.Rows(0).Cells(6).Value())
            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1116" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1380" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1446" Then '----Lourdu Matha

            If vTotMtrs = 0 Then
                MessageBox.Show("Invalid METERS", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dgv_Details.Enabled And dgv_Details.Visible Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(7)
                End If
                Exit Sub
            End If

        End If





        tr = con.BeginTransaction

        'Try

        If Insert_Entry = True Or New_Entry = False Then
            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

        Else

            lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "ClothSales_Delivery_Head", "ClothSales_Delivery_Code", "For_OrderBy", "ClothSales_Delivery_Code like '" & Trim(Pk_Condition) & "%'", Val(lbl_Company.Tag), EntFnYrCode, tr)

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

        End If

        cmd.Connection = con
        cmd.Transaction = tr

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@DeliveryDate", Convert.ToDateTime(msk_date.Text))

        If New_Entry = True Then

            cmd.CommandText = "Insert into ClothSales_Delivery_Head ( ClothSales_Delivery_Code ,               Company_IdNo       ,     ClothSales_Delivery_No  ,                     for_OrderBy                                         , ClothSales_Delivery_Date  ,              Ledger_IdNo    ,     Order_Type ,   Party_OrderNo   , Party_OrderDate ,  Through_Name  ,  Lr_No    ,  Lr_Date  ,  Despatch_To , Delivery_Address1      ,Delivery_Address2 , Note                    ,        Total_Bales  ,          Total_Pcs           ,          Total_Meters     ,          user_idNo                   ,     Packing_Type                          ) " & _
                                "     Values                        (   '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ",       @DeliveryDate               , " & Str(Val(Led_ID)) & ",''             ,      ''            ,       ''     , ''             ,  ''        , ''           , ''       , ''                     ,  ''        ,'" & Trim(txt_Note.Text) & "' , " & Str(Val(vTotBals)) & ", " & Str(Val(vTotPcs)) & ", " & Str(Val(vTotMtrs)) & "," & Val(lbl_UserName.Text) & " , '" & Trim(cbo_RollBundle.Text) & "') "
            cmd.ExecuteNonQuery()

        Else

            cmd.CommandText = "Update ClothSales_Delivery_Head set ClothSales_Delivery_Date = @DeliveryDate, Ledger_IdNo =  " & Str(Val(Led_ID)) & ",   Note = '" & Trim(txt_Note.Text) & "' ,Order_Type='' ,Party_OrderNo='' ,Party_OrderDate='' , Through_Name   =  ''   ,Lr_No  =  '' , Lr_date = ''  ,Despatch_To = ''  , Delivery_Address1 = '' , Delivery_Address2 = '' ,Total_Bales  = " & Str(Val(vTotBals)) & ", Total_Pcs =  " & Str(Val(vTotPcs)) & ", Total_Meters = " & Str(Val(vTotMtrs)) & ",   Packing_Type = '" & Trim(cbo_RollBundle.Text) & "'  ,   USER_IDNO = " & Val(lbl_UserName.Text) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

           
            cmd.CommandText = "Update Packing_Slip_Head set Delivery_Code = '', Delivery_No = '', Delivery_DetailsSlNo = 0, Delivery_Increment = Delivery_Increment - 1, Delivery_Date = Null Where Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

        End If

        EntID = Trim(Pk_Condition) & Trim(lbl_DcNo.Text)
        PBlNo = Trim(lbl_DcNo.Text)
        Partcls = "ClothSales : Dc.No. " & Trim(lbl_DcNo.Text)

        cmd.CommandText = "Delete from ClothSales_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code = '" & Trim(NewCode) & "'and Invoice_Meters = 0"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
        cmd.ExecuteNonQuery()

        With dgv_Details

            Sno = 0
            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(6).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0 Then

                    Sno = Sno + 1

                    clth_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                    clthtyp_ID = Common_Procedures.ClothType_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)

                   

                    Nr = 0
                    cmd.CommandText = "Update  ClothSales_Delivery_Details set ClothSales_Delivery_Date = @DeliveryDate , Ledger_IdNo = " & Str(Val(Led_ID)) & ", Sl_No  = " & Str(Val(Sno)) & " , Cloth_IdNo = " & Str(Val(clth_ID)) & " , ClothType_IdNo = " & Str(Val(clthtyp_ID)) & " , Fold_Perc =  " & Str(Val(.Rows(i).Cells(3).Value)) & ", Bales = " & Str(Val(.Rows(i).Cells(4).Value)) & " ,   Bales_Nos = '" & Trim(.Rows(i).Cells(5).Value) & "',      Pcs   = " & Str(Val(.Rows(i).Cells(6).Value)) & ",  Meters = " & Str(Val(.Rows(i).Cells(7).Value)) & ",Rate = " & Str(Val(.Rows(i).Cells(11).Value)) & "  where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code = '" & Trim(NewCode) & "'  and ClothSales_Delivery_SlNo = " & Str(Val(.Rows(i).Cells(10).Value))
                    Nr = cmd.ExecuteNonQuery()

                    If Nr = 0 Then

                        cmd.CommandText = "Insert into ClothSales_Delivery_Details ( ClothSales_Delivery_Code ,               Company_IdNo       ,   ClothSales_Delivery_No    ,                     for_OrderBy                                        , ClothSales_Delivery_Date       ,            Ledger_IdNo        ,         Sl_No       ,          Cloth_IdNo          ,      ClothType_IdNo         ,                   Fold_Perc              ,                 Bales                    ,                   Bales_Nos           ,                       Pcs                 ,                      Meters              ,             ClothSales_Delivery_SlNo     ,              PackingSlip_Codes         ,                    Rate             ) " & _
                                            "     Values                        (   '" & Trim(NewCode) & "'   , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ",       @DeliveryDate            ,      " & Str(Val(Led_ID)) & " , " & Str(Val(Sno)) & ", " & Str(Val(clth_ID)) & "   , " & Str(Val(clthtyp_ID)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ",'" & Trim(.Rows(i).Cells(5).Value) & "',  " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & ", '" & Trim(.Rows(i).Cells(10).Value) & "', " & Str(Val(.Rows(i).Cells(11).Value)) & " ) "
                        cmd.ExecuteNonQuery()

                    End If
                  
                End If

            Next

        End With

        With dgv_BaleDetails

            Sno = 0
            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(3).Value) <> 0 And Trim(.Rows(i).Cells(4).Value) <> "" Then

                    cmd.CommandText = "Update Packing_Slip_Head set Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "', Delivery_No = '" & Trim(lbl_DcNo.Text) & "', Delivery_DetailsSlNo = " & Str(Val(.Rows(i).Cells(0).Value)) & ", Delivery_Increment = Delivery_Increment + 1, Delivery_Date = @DeliveryDate Where Packing_Slip_Code = '" & Trim(.Rows(i).Cells(4).Value) & "'"
                    cmd.ExecuteNonQuery()

                End If

            Next i

        End With

        OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
        OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

        cmd.CommandText = "Truncate Table " & Trim(Common_Procedures.EntryTempSubTable) & ""
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Name1, Int1, Int2, Weight1, Meters1, Meters2, Meters3, Meters4, Meters5) select a.ClothSales_Delivery_Code, a.ClothSales_Delivery_SlNo, a.Cloth_IdNo, a.Fold_Perc, (CASE WHEN a.ClothType_IdNo = 1 THEN a.Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 2 THEN a.Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 3 THEN a.Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 4 THEN a.Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 5 THEN a.Meters ELSE 0 END) from ClothSales_Delivery_Details a where a.ClothSales_Delivery_Code = '" & Trim(NewCode) & "'"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Name1, Int1, Int2, Weight1, Meters1, Meters2, Meters3, Meters4, Meters5) select tC.ClothSales_Delivery_Code, tC.ClothSales_Delivery_SlNo, tC.Cloth_IdNo, tC.Fold_Perc, -1*(CASE WHEN tC.ClothType_IdNo = 1 THEN a.Total_Meters ELSE 0 END), -1*(CASE WHEN tC.ClothType_IdNo = 2 THEN a.Total_Meters ELSE 0 END), -1*(CASE WHEN tC.ClothType_IdNo = 3 THEN a.Total_Meters ELSE 0 END), -1*(CASE WHEN tC.ClothType_IdNo = 4 THEN a.Total_Meters ELSE 0 END), -1*(CASE WHEN tC.ClothType_IdNo = 5 THEN a.Total_Meters ELSE 0 END) from Packing_Slip_Head a, ClothSales_Delivery_Details tC where tC.ClothSales_Delivery_Code = '" & Trim(NewCode) & "' and a.Packing_Slip_Code LIKE '%/" & Trim(OpYrCode) & "' and a.Delivery_Code = '" & Trim(Pk_Condition) & "' + tC.ClothSales_Delivery_Code and a.Delivery_DetailsSlNo = tC.ClothSales_Delivery_SlNo"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Name1, Int1, Int2, Weight1, Meters1, Meters2, Meters3, Meters4, Meters5) select tC.ClothSales_Delivery_Code, tC.ClothSales_Delivery_SlNo, tC.Cloth_IdNo, tC.Fold_Perc, -1*(CASE WHEN tC.ClothType_IdNo = 1 THEN b.Meters ELSE 0 END), -1*(CASE WHEN tC.ClothType_IdNo = 2 THEN b.Meters ELSE 0 END), -1*(CASE WHEN tC.ClothType_IdNo = 3 THEN b.Meters ELSE 0 END), -1*(CASE WHEN tC.ClothType_IdNo = 4 THEN b.Meters ELSE 0 END), -1*(CASE WHEN tC.ClothType_IdNo = 5 THEN b.Meters ELSE 0 END) from Packing_Slip_Head a, Packing_Slip_Details b, ClothSales_Delivery_Details tC where tC.ClothSales_Delivery_Code = '" & Trim(NewCode) & "' and a.Packing_Slip_Code NOT LIKE '%/" & Trim(OpYrCode) & "' and a.Company_IdNo = b.Company_IdNo and a.Packing_Slip_Code = b.Packing_Slip_Code and a.Delivery_Code = '" & Trim(Pk_Condition) & "' + tC.ClothSales_Delivery_Code and a.Delivery_DetailsSlNo = tC.ClothSales_Delivery_SlNo"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Truncate Table " & Trim(Common_Procedures.EntryTempTable) & ""
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Weight1, Meters1, Meters2, Meters3, Meters4, Meters5) select Int2, Weight1, sum(Meters1), sum(Meters2), sum(Meters3), sum(Meters4), sum(Meters5) from " & Trim(Common_Procedures.EntryTempSubTable) & " group by Int2, Weight1 "
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Weight1, Meters1, Meters2, Meters3, Meters4, Meters5) select a.Cloth_IdNo, a.Folding, (CASE WHEN a.ClothType_IdNo = 1 THEN a.Total_Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 2 THEN a.Total_Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 3 THEN a.Total_Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 4 THEN a.Total_Meters ELSE 0 END), (CASE WHEN a.ClothType_IdNo = 5 THEN a.Total_Meters ELSE 0 END) from Packing_Slip_Head a where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Packing_Slip_Code LIKE '%/" & Trim(OpYrCode) & "'"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Weight1, Meters1, Meters2, Meters3, Meters4, Meters5) select b.Cloth_IdNo, b.Folding, (CASE WHEN b.ClothType_IdNo = 1 THEN b.Meters ELSE 0 END), (CASE WHEN b.ClothType_IdNo = 2 THEN b.Meters ELSE 0 END), (CASE WHEN b.ClothType_IdNo = 3 THEN b.Meters ELSE 0 END), (CASE WHEN b.ClothType_IdNo = 4 THEN b.Meters ELSE 0 END), (CASE WHEN b.ClothType_IdNo = 5 THEN b.Meters ELSE 0 END) from Packing_Slip_Head a, Packing_Slip_Details b where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Packing_Slip_Code NOT LIKE '%/" & Trim(OpYrCode) & "' and a.Company_IdNo = b.Company_IdNo and a.Packing_Slip_Code = b.Packing_Slip_Code"
        cmd.ExecuteNonQuery()

        Da = New SqlClient.SqlDataAdapter("select Int1 as Clo_IdNo, Weight1 as FoldPerc, sum(Meters1 ) as Type1_Mtrs, sum(Meters2) as Type2_Mtrs, sum(Meters3) as Type3_Mtrs, sum(Meters4) as Type4_Mtrs, sum(Meters5) as Type5_Mtrs from " & Trim(Common_Procedures.EntryTempTable) & " group by Int1, Weight1 having sum(Meters1) <> 0 or sum(Meters2) <> 0 or sum(Meters3) <> 0 or sum(Meters4) <> 0 or sum(Meters5) <> 0 ", con)
        Da.SelectCommand.Transaction = tr
        Dt1 = New DataTable
        Da.Fill(Dt1)

        Sno = 0
        If Dt1.Rows.Count > 0 Then
            For i = 0 To Dt1.Rows.Count - 1
                Sno = Sno + 1
                If Trim(UCase(Common_Procedures.ClothDelivery_Opening_OR_Entry)) <> "OPENING" Then
                    cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code,             Company_IdNo         ,           Reference_No       ,                               for_OrderBy                             , Reference_Date ,                                            StockOff_IdNo  ,      DeliveryTo_Idno    ,                              ReceivedFrom_Idno            ,         Entry_ID     ,       Party_Bill_No  ,       Particulars      ,           Sl_No      ,                         Cloth_Idno                     ,                          Folding                       ,                         Meters_Type1                     ,                         Meters_Type2                     ,                         Meters_Type3                     ,                         Meters_Type4                     ,                         Meters_Type5                          ) " &
                                                    " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ",  @DeliveryDate, " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", " & Str(Val(Led_ID)) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Dt1.Rows(i).Item("Clo_IdNo").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("FoldPerc").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("Type1_Mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("Type2_Mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("Type3_Mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("Type4_Mtrs").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("Type5_Mtrs").ToString)) & "     ) "
                    cmd.ExecuteNonQuery()
                End If
            Next
        End If

        tr.Commit()

        MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
            If New_Entry = True Then
                new_record()
            Else
                move_record(lbl_DcNo.Text)
            End If
        Else
            move_record(lbl_DcNo.Text)
        End If

        'Catch ex As Exception
        '    tr.Rollback()
        '    MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'Finally

        '    Dt1.Dispose()
        '    Da.Dispose()
        '    cmd.Dispose()
        '    tr.Dispose()

        '    If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        'End Try

    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotPcs As Single
        Dim TotBals As Single
        Dim TotMtrs As Single

        If NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotPcs = 0 : TotBals = 0 : TotMtrs = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(7).Value) <> 0 Then

                    TotBals = TotBals + Val(.Rows(i).Cells(4).Value())
                    TotPcs = TotPcs + Val(.Rows(i).Cells(6).Value())
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(7).Value())

                End If

            Next i

        End With


        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Val(TotBals)
            .Rows(0).Cells(6).Value = Val(TotPcs)
            .Rows(0).Cells(7).Value = Format(Val(TotMtrs), "########0.00")
        End With

    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1)", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, msk_date, cbo_RollBundle, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, cbo_RollBundle, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
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


    Private Sub cbo_Grid_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ClothName.GotFocus
        vCbo_ItmNm = Trim(cbo_Grid_ClothName.Text)

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

    End Sub

    Private Sub cbo_Grid_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothName.KeyDown

        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_ClothName, Nothing, cbo_Grid_Clothtype, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                cbo_RollBundle.Focus()
            End If

            If (e.KeyValue = 40 And cbo_Grid_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    txt_Note.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_ClothName, cbo_Grid_Clothtype, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            e.Handled = True

            With dgv_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    txt_Note.Focus()
                Else
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(2)

                End If
            End With

        End If
    End Sub

    Private Sub cbo_Grid_ClothName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothName.KeyUp
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

    Private Sub cbo_Grid_Clothtype_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Clothtype.GotFocus
        vCbo_ItmNm = Trim(cbo_Grid_Clothtype.Text)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothType_Head", "ClothType_Name", "", "(ClothType_IdNo = 0)")

    End Sub

    Private Sub cbo_Grid_Clothtype_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Clothtype.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Clothtype, Nothing, Nothing, "ClothType_Head", "ClothType_Name", "", "(ClothType_IdNo = 0)")
        vcbo_KeyDwnVal = e.KeyValue

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Clothtype.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Clothtype.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_Grid_Clothtype_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Clothtype.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Clothtype, Nothing, "ClothType_Head", "ClothType_Name", "", "(ClothType_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If

    End Sub


    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim rect As Rectangle

        With dgv_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If Val(.Rows(e.RowIndex).Cells(8).Value) = 0 Then
                If e.RowIndex = 0 Then
                    .Rows(e.RowIndex).Cells(8).Value = 1
                Else
                    .Rows(e.RowIndex).Cells(8).Value = Val(.Rows(e.RowIndex - 1).Cells(8).Value) + 1
                End If
            End If

            If Trim(.CurrentRow.Cells(2).Value) = "" Then
                .CurrentRow.Cells(2).Value = Common_Procedures.ClothType_IdNoToName(con, 1)
            End If

            If Val(.CurrentRow.Cells(3).Value) = 0 Then
                .CurrentRow.Cells(3).Value = "100"
            End If

            If e.ColumnIndex = 1 Then

                If cbo_Grid_ClothName.Visible = False Or Val(cbo_Grid_ClothName.Tag) <> e.RowIndex Then

                    cbo_Grid_ClothName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_ClothName.DataSource = Dt1
                    cbo_Grid_ClothName.DisplayMember = "Cloth_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_ClothName.Left = .Left + rect.Left
                    cbo_Grid_ClothName.Top = .Top + rect.Top

                    cbo_Grid_ClothName.Width = rect.Width
                    cbo_Grid_ClothName.Height = rect.Height
                    cbo_Grid_ClothName.Text = .CurrentCell.Value

                    cbo_Grid_ClothName.Tag = Val(e.RowIndex)
                    cbo_Grid_ClothName.Visible = True

                    cbo_Grid_ClothName.BringToFront()
                    cbo_Grid_ClothName.Focus()

                End If

            Else
                cbo_Grid_ClothName.Visible = False

            End If

            If e.ColumnIndex = 2 Then

                If cbo_Grid_Clothtype.Visible = False Or Val(cbo_Grid_Clothtype.Tag) <> e.RowIndex Then

                    cbo_Grid_Clothtype.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select ClothType_Name from ClothType_Head order by ClothType_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_Clothtype.DataSource = Dt1
                    cbo_Grid_Clothtype.DisplayMember = "ClothType_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Clothtype.Left = .Left + rect.Left
                    cbo_Grid_Clothtype.Top = .Top + rect.Top

                    cbo_Grid_Clothtype.Width = rect.Width
                    cbo_Grid_Clothtype.Height = rect.Height
                    cbo_Grid_Clothtype.Text = .CurrentCell.Value

                    cbo_Grid_Clothtype.Tag = Val(e.RowIndex)
                    cbo_Grid_Clothtype.Visible = True

                    cbo_Grid_Clothtype.BringToFront()
                    cbo_Grid_Clothtype.Focus()

                End If

            Else
                cbo_Grid_Clothtype.Visible = False

            End If

            If e.ColumnIndex = 4 And Trim(UCase(Common_Procedures.ClothDelivery_Opening_OR_Entry)) <> "OPENING" Or e.ColumnIndex = 5 And Trim(UCase(Common_Procedures.ClothDelivery_Opening_OR_Entry)) <> "OPENING" Then

                rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                pnl_BaleSelection_ToolTip.Left = .Left + rect.Left
                pnl_BaleSelection_ToolTip.Top = .Top + rect.Top + rect.Height + 3

                pnl_BaleSelection_ToolTip.Visible = True

            Else
                pnl_BaleSelection_ToolTip.Visible = False

            End If



        End With

    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 11 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
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
                If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Then
                    Total_Calculation()

                End If
                If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 6 Then
                    Amount_Calculation(.CurrentCell.RowIndex, .CurrentCell.ColumnIndex)
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
    End Sub

    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        With dgv_Details
            vcbo_KeyDwnVal = e.KeyValue
            If .Visible Then
                If e.KeyValue = Keys.Delete Then



                End If
            End If
        End With

    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        On Error Resume Next
        Dim i As Integer
        Dim n As Integer = -1

        With dgv_Details
            If .Visible Then



                If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 11 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If
            End If
        End With

    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            btn_BaleSelection_Click(sender, e)
        End If

    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        With dgv_Details

            vcbo_KeyDwnVal = e.KeyValue

            'If e.KeyCode = Keys.Left Then
            '    If .CurrentCell.ColumnIndex <= 1 Then
            '        If .CurrentCell.RowIndex = 0 Then
            '            txt_DelvAdd2.Focus()
            '        Else
            '            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)
            '        End If
            '    End If
            'End If

            'If e.KeyCode = Keys.Right Then
            '    If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
            '        If .CurrentCell.RowIndex >= .Rows.Count - 1 Then
            '            cbo_Transport.Focus()
            '        Else
            '            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
            '        End If
            '    End If
            'End If

        End With

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                If Val(.Rows(.CurrentCell.RowIndex).Cells(9).Value) = 0 Then

                    n = .CurrentRow.Index

                    If n = .Rows.Count - 1 Then
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

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            btn_BaleSelection_Click(sender, e)
        End If

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)

            If Val(.Rows(e.RowIndex).Cells(8).Value) = 0 Then
                If e.RowIndex = 0 Then
                    .Rows(e.RowIndex).Cells(8).Value = 1
                Else
                    .Rows(e.RowIndex).Cells(8).Value = Val(.Rows(e.RowIndex - 1).Cells(8).Value) + 1
                End If
            End If

        End With

    End Sub





    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Note.KeyDown
        If e.KeyValue = 38 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            Else
                cbo_RollBundle.Focus()

            End If
        End If

        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Grid_ClothName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ClothName.TextChanged
        Try
            If cbo_Grid_ClothName.Visible Then

                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_ClothName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_ClothName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_Clothtype_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Clothtype.TextChanged
        Try
            If cbo_Grid_Clothtype.Visible Then

                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_Clothtype.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Clothtype.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Note.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
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

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.ClothSales_Delivery_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.ClothSales_Delivery_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.ClothSales_Delivery_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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

            da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name, d.ClothType_name, e.Ledger_Name from ClothSales_Delivery_Head a left outer join ClothSales_Delivery_Details b on a.ClothSales_Delivery_Code = b.ClothSales_Delivery_Code left outer join Cloth_head c on b.Cloth_idno = c.Cloth_idno left outer join ClothType_head d on b.ClothType_idno = d.ClothType_idno left outer join Ledger_head e on a.Ledger_idno = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code like '" & Trim(Pk_Condition) & "%' and a.ClothSales_Delivery_Code like '%/" & Trim(EntFnYrCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by ClothSales_Delivery_Date, for_orderby, ClothSales_Delivery_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("ClothSales_Delivery_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("ClothSales_Delivery_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Party_OrderNo").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("ClothType_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")

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

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
    End Sub

    Private Sub cbo_Filter_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ClothName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")

    End Sub

    Private Sub cbo_Filter_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ClothName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ClothName, cbo_Filter_PartyName, btn_Filter_Show, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")

    End Sub

    Private Sub cbo_Filter_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ClothName, btn_Filter_Show, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1)", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1)", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1)", "(Ledger_idno = 0)")
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        pnl_Print.Visible = True
        pnl_Back.Enabled = False
        If btn_Print_Delivery.Enabled And btn_Print_Delivery.Visible Then
            btn_Print_Delivery.Focus()
        End If
    End Sub

    Private Sub btn_Print_Delivery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Delivery.Click
        Printing_Delivery()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_Print_Bale_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Bale.Click
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1090" Then
            Printing_Bale_Delivery()

        Else
            Printing_Bale()

        End If
        btn_print_Close_Click(sender, e)

    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Public Sub Printing_Delivery()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        'Dim ps As Printing.PaperSize
        Dim PpSzSTS As Boolean = False

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from ClothSales_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


        set_PaperSize_For_PrintDocument1()

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try
                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

                        set_PaperSize_For_PrintDocument1()

                        PrintDocument1.Print()
                    End If

                Else
                    PrintDocument1.Print()

                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try


        Else
            Try

                Dim ppd As New PrintPreviewDialog

                set_PaperSize_For_PrintDocument1()
                'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                '    If ps.Width = 800 And ps.Height = 600 Then
                '        PrintDocument1.DefaultPageSettings.PaperSize = ps
                '        PpSzSTS = True
                '        Exit For
                '    End If
                'Next

                'If PpSzSTS = False Then
                '    Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
                '    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
                '    PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
                'End If

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

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as TransportName ,e.Ledger_Name as Agent_Name,Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code from ClothSales_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN State_Head Csh ON b.Company_State_IdNo = Csh.State_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN State_Head Lsh ON c.Ledger_State_IdNo = Lsh.State_IdNo Left outer JOIN Ledger_Head d ON a.Transport_IdNo = d.Ledger_IdNo Left outer JOIN Ledger_Head e ON a.Agent_IdNo = e.Ledger_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Delivery_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name, d.ClothType_name ,e.* from ClothSales_Delivery_Details a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno INNER JOIN ItemGroup_Head e ON e.ItemGroup_IdNo = b.ItemGroup_IdNo LEFT OUTER JOIN ClothType_Head d ON a.ClothType_idno = d.ClothType_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Delivery_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        Printing_Delivery_Format1(e)

    End Sub

    Private Sub Printing_Delivery_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        'Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim ItmNm1 As String, ItmNm2 As String


        set_PaperSize_For_PrintDocument1()

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    If ps.Width = 800 And ps.Height = 600 Then
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

        'If PpSzSTS = False Then

        '    Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
        '    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        '    PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        '    'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '    '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '    '        e.PageSettings.PaperSize = ps
        '    '        PpSzSTS = True
        '    '        Exit For
        '    '    End If
        '    'Next

        '    'If PpSzSTS = False Then
        '    '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '    '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '    '            e.PageSettings.PaperSize = ps
        '    '            Exit For
        '    '        End If
        '    '    Next
        '    'End If

        'End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 45
            .Top = 8 ' 15 ' 30
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Regular)
        'pFont = New Font("Calibri", 11, FontStyle.Regular)

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

        NoofItems_PerPage = 3
        If Trim(prn_HdDt.Rows(0).Item("Note").ToString) <> "" Then
            NoofItems_PerPage = NoofItems_PerPage - 1
        End If
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1168" Then
        '    NoofItems_PerPage = 4

        'Else
        '    NoofItems_PerPage = 5

        'End If

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(35) : ClAr(2) = 290 : ClAr(3) = 105 : ClAr(4) = 80 : ClAr(5) = 110
        ClAr(6) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5))

        TxtHgt = 16 ' 17.5 '18

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Delivery_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Delivery_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 40 Then
                            For I = 40 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 40
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Item_HSN_Code").ToString, LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1168" Then
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Fold_Perc").ToString) <> 0 And Val(prn_DetDt.Rows(prn_DetIndx).Item("Fold_Perc").ToString) <> 100 Then
                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Fold_Perc").ToString) & " CM  Folding ", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If
                        End If

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Delivery_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Delivery_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_Email As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim strHeight As Single, strWidth As Single = 0
        Dim C1 As Single
        Dim W1, w2 As Single
        Dim S1, S2, S3 As Single
        Dim vprn_BlNos As String = ""
        Dim Gst_dt As Date
        Dim Entry_dt As Date
        Dim CurX As Single = 0
        Dim vPackType As String = ""

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name, d.ClothType_name from ClothSales_Delivery_Details a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno LEFT OUTER JOIN ClothType_Head d ON a.ClothType_idno = d.ClothType_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""
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
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_Email = "E-mail : " & Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString)
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(0).Item("Company_State_Name").ToString
            If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
                Cmp_StateNm = Cmp_StateNm & "   CODE : " & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
            End If
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        Gst_dt = #7/1/2017#
        Entry_dt = dtp_Date.Value

        If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt - 13  ' 10

        Else

            CurY = CurY + TxtHgt

            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
            strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No), pFont).Width
            If PrintWidth > strWidth Then
                CurX = LMargin + (PrintWidth - strWidth) / 2
            Else
                CurX = LMargin
            End If

            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, p1Font)
            strWidth = e.Graphics.MeasureString(Cmp_StateCap, p1Font).Width
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont)

            strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
            strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)

            CurY = CurY + TxtHgt - 1

        End If

        'CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "FABRIC DELIVERY CHALLAN", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("ORDER NO : ", pFont).Width
        w2 = e.Graphics.MeasureString("DESP.TO : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width
        S2 = e.Graphics.MeasureString("TRANSPORT :  ", pFont).Width

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MAInName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("ClothSales_Delivery_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothSales_Delivery_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        If prn_HdDt.Rows(0).Item("Party_OrderNo").ToString <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
        End If

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If DateDiff("d", Gst_dt.ToShortDateString, Entry_dt.ToShortDateString) < 0 Then
            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " TIN NO : " & Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If

        Else
            If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " GSTIN NO : " & Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If

        End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + 5

        Common_Procedures.Print_To_PrintDocument(e, "THROUGH  ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Through_Name").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)

        If Trim(prn_HdDt.Rows(0).Item("Vechile_No").ToString) <> "" Then
            S3 = e.Graphics.MeasureString("THROUGH     :   " & prn_HdDt.Rows(0).Item("Through_Name").ToString, pFont).Width + 30

            Common_Procedures.Print_To_PrintDocument(e, "VEHICLE NO  ", LMargin + S3 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + S3 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + S2 + S3 + 30, CurY, 0, 0, pFont)
        End If


        Common_Procedures.Print_To_PrintDocument(e, "DESP.TO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatch_To").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Address1").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "LR.NO  ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Address2").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "AGENT ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)

        'vprn_BlNos = ""
        'For i = 0 To prn_DetDt.Rows.Count - 1
        '    If Trim(prn_DetDt.Rows(i).Item("Bales_Nos").ToString) <> "" Then
        '        vprn_BlNos = Trim(vprn_BlNos) & IIf(Trim(vprn_BlNos) <> "", ", ", "") & prn_DetDt.Rows(i).Item("Bales_Nos").ToString
        '    End If
        'Next

        vPackType = Trim(UCase(prn_HdDt.Rows(0).Item("Packing_Type").ToString))
        If Trim(vPackType) = "" Then vPackType = "BALE"
        'Common_Procedures.Print_To_PrintDocument(e, Trim(vPackType) & " NOS : " & vprn_BlNos, LMargin + C1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CLOTH NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(vPackType) & "S", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub

    Private Sub Printing_Delivery_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim W1 As Single = 0
        Dim vprn_BlNos As String = ""
        Dim vPackType As String = ""
        Dim BLNo1 As String = ""
        Dim BLNo2 As String = ""


        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bales").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), "#########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        CurY = CurY + 10


        vprn_BlNos = ""
        For i = 0 To prn_DetDt.Rows.Count - 1
            If Trim(prn_DetDt.Rows(i).Item("Bales_Nos").ToString) <> "" Then
                vprn_BlNos = Trim(vprn_BlNos) & IIf(Trim(vprn_BlNos) <> "", ", ", "") & prn_DetDt.Rows(i).Item("Bales_Nos").ToString
            End If
        Next

        BLNo1 = Trim(vprn_BlNos)
        BLNo2 = ""

        If Len(BLNo1) > 75 Then
            For i = 75 To 1 Step -1
                If Mid$(Trim(BLNo1), i, 1) = " " Or Mid$(Trim(BLNo1), i, 1) = "," Or Mid$(Trim(BLNo1), i, 1) = "." Or Mid$(Trim(BLNo1), i, 1) = "-" Or Mid$(Trim(BLNo1), i, 1) = "/" Or Mid$(Trim(BLNo1), i, 1) = "_" Or Mid$(Trim(BLNo1), i, 1) = "(" Or Mid$(Trim(BLNo1), i, 1) = ")" Or Mid$(Trim(BLNo1), i, 1) = "\" Or Mid$(Trim(BLNo1), i, 1) = "[" Or Mid$(Trim(BLNo1), i, 1) = "]" Or Mid$(Trim(BLNo1), i, 1) = "{" Or Mid$(Trim(BLNo1), i, 1) = "}" Then Exit For
            Next i
            If i = 0 Then i = 75
            BLNo2 = Microsoft.VisualBasic.Right(Trim(BLNo1), Len(BLNo1) - i)
            BLNo1 = Microsoft.VisualBasic.Left(Trim(BLNo1), i)
        End If


        vPackType = Trim(UCase(prn_HdDt.Rows(0).Item("Packing_Type").ToString))
        If Trim(vPackType) = "" Then vPackType = "BALE"
        Common_Procedures.Print_To_PrintDocument(e, Trim(vPackType) & " NOS : " & BLNo1, LMargin + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "        " & BLNo2, LMargin + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1168" Then
        If Trim(prn_HdDt.Rows(0).Item("Note").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "NOTE : " & prn_HdDt.Rows(0).Item("Note").ToString, LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
        End If
        'End If

        If Common_Procedures.User.IdNo <> 1 Then
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 20, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)


        Common_Procedures.Print_To_PrintDocument(e, "Prepared By", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub dgv_Filter_Details_CellDoubleClick1(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub


    Private Sub Bale_Selection()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim Clo_ID As Integer, CloType_ID As Integer
        Dim NewCode As String
        Dim Fd_Perc As Integer
        Dim CompIDCondt As String
        Dim dgvDet_CurRow As Integer
        Dim dgv_DetSlNo As Long

        Try

            If dgv_Details.CurrentCell.RowIndex < 0 Then
                MessageBox.Show("Invalid Cloth Name & Type Selection", "DOES NOT SELECT BALE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dgv_Details.Enabled And dgv_Details.Visible Then
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
                        dgv_Details.CurrentCell.Selected = True
                    End If
                End If
                Exit Sub
            End If

            Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(1).Value)
            If Clo_ID = 0 Then
                MessageBox.Show("Invalid Cloth Name", "DOES NOT SELECT BALE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dgv_Details.Enabled And dgv_Details.Visible Then
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                        If cbo_Grid_ClothName.Visible And cbo_Grid_ClothName.Enabled Then cbo_Grid_ClothName.Focus()
                        'dgv_Details.CurrentCell.Selected = True
                        Exit Sub
                    End If
                End If
                Exit Sub
            End If

            CloType_ID = Common_Procedures.ClothType_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(2).Value)
            If CloType_ID = 0 Then
                MessageBox.Show("Invalid Cloth Type ", "DOES NOT SELECT BALE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dgv_Details.Enabled And dgv_Details.Visible Then
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(2)
                        If cbo_Grid_Clothtype.Visible And cbo_Grid_Clothtype.Enabled Then cbo_Grid_Clothtype.Focus()
                        Exit Sub
                    End If
                End If
                Exit Sub
            End If

            Fd_Perc = Val(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(3).Value)
            If Val(Fd_Perc) = 0 Then
                MessageBox.Show("Invalid Folding", "DOES NOT SELECT BALE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dgv_Details.Enabled And dgv_Details.Visible Then
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(3)
                        dgv_Details.CurrentCell.Selected = True
                    End If
                End If
                Exit Sub
            End If

            CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
            If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
                CompIDCondt = ""
            End If

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

            dgvDet_CurRow = dgv_Details.CurrentCell.RowIndex
            dgv_DetSlNo = Val(dgv_Details.Rows(dgvDet_CurRow).Cells(8).Value)

            With dgv_BaleSelection

                .Rows.Clear()
                SNo = 0

                Da = New SqlClient.SqlDataAdapter("Select a.* from Packing_Slip_Head a where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Delivery_DetailsSlNo = " & Str(Val(dgv_DetSlNo)) & " and a.Cloth_IdNo = " & Str(Val(Clo_ID)) & "  and a.ClothType_IdNo = " & Str(Val(CloType_ID)) & "  and a.Folding = " & Str(Val(Fd_Perc)) & " order by a.Packing_Slip_Date, a.for_orderby, a.Packing_Slip_No, a.Packing_Slip_Code", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Packing_Slip_No").ToString
                        If Val(Dt1.Rows(i).Item("Total_Pcs").ToString) <> 0 Then
                            .Rows(n).Cells(2).Value = Val(Dt1.Rows(i).Item("Total_Pcs").ToString)
                        End If
                        If Val(Dt1.Rows(i).Item("Total_Meters").ToString) <> 0 Then
                            .Rows(n).Cells(3).Value = Format(Val(Dt1.Rows(i).Item("Total_Meters").ToString), "#########0.00")
                        End If
                        'If Val(Dt1.Rows(i).Item("Total_Weight").ToString) <> 0 Then
                        '    .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Total_Weight").ToString), "#########0.000")
                        'End If
                        .Rows(n).Cells(4).Value = "1"
                        .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Packing_Slip_Code").ToString
                        .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Bale_Bundle").ToString

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()

                Da = New SqlClient.SqlDataAdapter("select a.* from Packing_Slip_Head a where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Delivery_Code = '' and a.Cloth_IdNo = " & Str(Val(Clo_ID)) & "  and a.ClothType_IdNo = " & Str(Val(CloType_ID)) & "  and a.Folding = " & Str(Val(Fd_Perc)) & " order by a.Packing_Slip_Date, a.for_orderby, a.Packing_Slip_No, a.Packing_Slip_Code", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Packing_Slip_No").ToString
                        If Val(Dt1.Rows(i).Item("Total_Pcs").ToString) <> 0 Then
                            .Rows(n).Cells(2).Value = Val(Dt1.Rows(i).Item("Total_Pcs").ToString)
                        End If
                        If Val(Dt1.Rows(i).Item("Total_Meters").ToString) <> 0 Then
                            .Rows(n).Cells(3).Value = Format(Val(Dt1.Rows(i).Item("Total_Meters").ToString), "#########0.00")
                        End If
                        'If Val(Dt1.Rows(i).Item("Total_Weight").ToString) <> 0 Then
                        '    .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Total_Weight").ToString), "#########0.000")
                        'End If
                        .Rows(n).Cells(4).Value = ""
                        .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Packing_Slip_Code").ToString
                        .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Bale_Bundle").ToString

                    Next

                End If
                Dt1.Clear()


            End With

            pnl_BaleSelection.Visible = True
            pnl_Back.Enabled = False
            dgv_BaleSelection.Focus()
            If dgv_BaleSelection.Rows.Count > 0 Then
                dgv_BaleSelection.CurrentCell = dgv_BaleSelection.Rows(0).Cells(0)
                dgv_BaleSelection.CurrentCell.Selected = True
            End If

        Catch ex As NullReferenceException
            MessageBox.Show("Select the ClothName for Bale Selection", "DOES NOT SELECT BALE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT BALE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub btn_BaleSelection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_BaleSelection.Click

        Bale_Selection()

    End Sub

    Private Sub dgv_BaleSelection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BaleSelection.CellClick
        Select_Bale(e.RowIndex)
    End Sub

    Private Sub Select_Bale(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_BaleSelection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(4).Value = (Val(.Rows(RwIndx).Cells(4).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(4).Value) = 0 Then .Rows(RwIndx).Cells(4).Value = ""

                For i = 0 To .ColumnCount - 1
                    .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                Next

            End If

        End With

    End Sub

    Private Sub dgv_BaleSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_BaleSelection.KeyDown
        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_BaleSelection.CurrentCell.RowIndex >= 0 Then
                Select_Bale(dgv_BaleSelection.CurrentCell.RowIndex)
                e.Handled = True
            End If
        End If

        If e.KeyCode = Keys.Delete Or e.KeyCode = Keys.Back Then
            If dgv_BaleSelection.CurrentCell.RowIndex >= 0 Then
                If Val(dgv_BaleSelection.Rows(dgv_BaleSelection.CurrentCell.RowIndex).Cells(4).Value) = 1 Then
                    e.Handled = True
                    Select_Bale(dgv_BaleSelection.CurrentCell.RowIndex)
                End If
            End If
        End If

    End Sub

    Private Sub btn_Close_BaleSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_BaleSelection.Click
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim I As Integer, J As Integer
        Dim n As Integer
        Dim sno As Integer
        Dim dgvDet_CurRow As Integer = 0
        Dim dgv_DetSlNo As Integer = 0
        Dim NoofBls As Integer
        Dim FsNo As Single, LsNo As Single
        Dim FsBaleNo As String, LsBaleNo As String
        Dim BlNo As String, PackSlpCodes As String
        Dim Tot_Pcs As Single, Tot_Mtrs As Single


        Cmd.Connection = con

        dgvDet_CurRow = dgv_Details.CurrentCell.RowIndex
        dgv_DetSlNo = Val(dgv_Details.Rows(dgvDet_CurRow).Cells(8).Value)

        With dgv_BaleDetails

LOOP1:
            For I = 0 To .RowCount - 1

                If Val(.Rows(I).Cells(0).Value) = Val(dgv_DetSlNo) Then

                    If I = .Rows.Count - 1 Then
                        For J = 0 To .ColumnCount - 1
                            .Rows(I).Cells(J).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(I)

                    End If

                    GoTo LOOP1

                End If

            Next I

            Cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            Cmd.ExecuteNonQuery()

            NoofBls = 0 : Tot_Pcs = 0 : Tot_Mtrs = 0 : BlNo = "" : PackSlpCodes = ""

            For I = 0 To dgv_BaleSelection.RowCount - 1

                If Val(dgv_BaleSelection.Rows(I).Cells(4).Value) = 1 Then

                    n = .Rows.Add()

                    sno = sno + 1
                    .Rows(n).Cells(0).Value = Val(dgv_DetSlNo)
                    .Rows(n).Cells(1).Value = dgv_BaleSelection.Rows(I).Cells(1).Value
                    .Rows(n).Cells(2).Value = Val(dgv_BaleSelection.Rows(I).Cells(2).Value)
                    .Rows(n).Cells(3).Value = Format(Val(dgv_BaleSelection.Rows(I).Cells(3).Value), "#########0.00")
                    ' .Rows(n).Cells(4).Value = Format(Val(dgv_BaleSelection.Rows(I).Cells(4).Value), "#########0.000")
                    .Rows(n).Cells(4).Value = dgv_BaleSelection.Rows(I).Cells(5).Value
                    .Rows(n).Cells(5).Value = dgv_BaleSelection.Rows(I).Cells(6).Value

                    Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) values ('" & Trim(dgv_BaleSelection.Rows(I).Cells(5).Value) & "', '" & Trim(dgv_BaleSelection.Rows(I).Cells(1).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(dgv_BaleSelection.Rows(I).Cells(1).Value))) & " ) "
                    Cmd.ExecuteNonQuery()

                    NoofBls = NoofBls + 1
                    Tot_Pcs = Val(Tot_Pcs) + Val(dgv_BaleSelection.Rows(I).Cells(2).Value)
                    Tot_Mtrs = Val(Tot_Mtrs) + Val(dgv_BaleSelection.Rows(I).Cells(3).Value)
                    PackSlpCodes = Trim(PackSlpCodes) & IIf(Trim(PackSlpCodes) = "", "~", "") & Trim(dgv_BaleSelection.Rows(I).Cells(5).Value) & "~"

                End If

            Next

            BlNo = ""
            FsNo = 0 : LsNo = 0
            FsBaleNo = "" : LsBaleNo = ""

            Da1 = New SqlClient.SqlDataAdapter("Select Name1 as Bale_Code, Name2 as Bale_No, Meters1 as fororderby_baleno from " & Trim(Common_Procedures.EntryTempTable) & " where Name1 <> '' order by Meters1, Name2, Name1", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                FsNo = Val(Dt1.Rows(0).Item("fororderby_baleno").ToString)
                LsNo = Val(Dt1.Rows(0).Item("fororderby_baleno").ToString)

                FsBaleNo = Trim(UCase(Dt1.Rows(0).Item("Bale_No").ToString))
                LsBaleNo = Trim(UCase(Dt1.Rows(0).Item("Bale_No").ToString))

                For I = 1 To Dt1.Rows.Count - 1
                    If LsNo + 1 = Val(Dt1.Rows(I).Item("fororderby_baleno").ToString) Then
                        LsNo = Val(Dt1.Rows(I).Item("fororderby_baleno").ToString)
                        LsBaleNo = Trim(UCase(Dt1.Rows(I).Item("Bale_No").ToString))

                    Else
                        If FsNo = LsNo Then
                            BlNo = BlNo & Trim(FsBaleNo) & ","
                        Else
                            BlNo = BlNo & Trim(FsBaleNo) & "-" & Trim(LsBaleNo) & ","
                        End If
                        FsNo = Dt1.Rows(I).Item("fororderby_baleno").ToString
                        LsNo = Dt1.Rows(I).Item("fororderby_baleno").ToString

                        FsBaleNo = Trim(UCase(Dt1.Rows(I).Item("Bale_No").ToString))
                        LsBaleNo = Trim(UCase(Dt1.Rows(I).Item("Bale_No").ToString))

                    End If

                Next

                If FsNo = LsNo Then BlNo = BlNo & Trim(FsBaleNo) Else BlNo = BlNo & Trim(FsBaleNo) & "-" & Trim(LsBaleNo)

            End If
            Dt1.Clear()

            If Trim(dgv_Details.Rows(dgvDet_CurRow).Cells(10).Value) <> "" Then
                dgv_Details.Rows(dgvDet_CurRow).Cells(4).Value = ""
                dgv_Details.Rows(dgvDet_CurRow).Cells(5).Value = ""
                dgv_Details.Rows(dgvDet_CurRow).Cells(6).Value = ""
                dgv_Details.Rows(dgvDet_CurRow).Cells(7).Value = ""
                dgv_Details.Rows(dgvDet_CurRow).Cells(10).Value = ""
            End If
            If Val(NoofBls) <> 0 And Val(Tot_Mtrs) <> 0 Then
                dgv_Details.Rows(dgvDet_CurRow).Cells(4).Value = NoofBls
                dgv_Details.Rows(dgvDet_CurRow).Cells(5).Value = BlNo
                If Val(Tot_Pcs) <> 0 Then
                    dgv_Details.Rows(dgvDet_CurRow).Cells(6).Value = Val(Tot_Pcs)
                End If
                dgv_Details.Rows(dgvDet_CurRow).Cells(7).Value = Format(Val(Tot_Mtrs), "#########0.00")
                dgv_Details.Rows(dgvDet_CurRow).Cells(10).Value = PackSlpCodes
            End If

            Add_NewRow_ToGrid()

            Total_Calculation()

        End With

        pnl_Back.Enabled = True
        pnl_BaleSelection.Visible = False
        If dgv_Details.Enabled And dgv_Details.Visible Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                If dgv_Details.CurrentCell.RowIndex >= 0 Then
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(11)
                    dgv_Details.CurrentCell.Selected = True
                End If
            End If
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
                    'MessageBox.Show("New Added Row = " & n & "  -  Current Row = " & .CurrentCell.RowIndex)

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

    Public Sub Printing_Bale()
        Dim prtFrm As Single = 0
        Dim prtTo As Single = 0
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim Condt As String = ""
        Dim PpSzSTS As Boolean = False
        Dim ps As Printing.PaperSize
        Dim NewCode As String

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from Packing_Slip_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then

                MessageBox.Show("No Entry Found", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try

                For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                        ps = PrintDocument2.PrinterSettings.PaperSizes(I)
                        PrintDocument2.DefaultPageSettings.PaperSize = ps
                        PpSzSTS = True
                        Exit For
                    End If
                Next

                If PpSzSTS = False Then
                    For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
                        If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                            ps = PrintDocument2.PrinterSettings.PaperSizes(I)
                            PrintDocument2.DefaultPageSettings.PaperSize = ps
                            Exit For
                        End If
                    Next
                End If

                PrintDialog1.PrinterSettings = PrintDocument2.PrinterSettings
                If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    PrintDocument2.PrinterSettings = PrintDialog1.PrinterSettings
                    PrintDocument2.Print()
                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try

        Else

            Try

                Dim ppd As New PrintPreviewDialog

                For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                        ps = PrintDocument2.PrinterSettings.PaperSizes(I)
                        PrintDocument2.DefaultPageSettings.PaperSize = ps
                        PpSzSTS = True
                        Exit For
                    End If
                Next

                If PpSzSTS = False Then
                    For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
                        If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                            ps = PrintDocument2.PrinterSettings.PaperSizes(I)
                            PrintDocument2.DefaultPageSettings.PaperSize = ps
                            Exit For
                        End If
                    Next
                End If

                ppd.Document = PrintDocument2

                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()
                'If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                '    PrintDocument2.DefaultPageSettings = PageSetupDialog1.PageSettings
                '    ppd.ShowDialog()
                'End If

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

        pnl_Back.Enabled = True
        pnl_Print.Visible = False

    End Sub

    Private Sub PrintDocument2_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument2.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String = ""
        Dim Total_mtrs As Single = 0
        prn_HdDt.Clear()
        prn_DetDt.Clear()

        prn_PageNo = 0
        prn_HdIndx = 1
        prn_DetIndx = 0
        prn_HdMxIndx = 0
        prn_DetMxIndx = 0
        prn_Count = 1
        Erase prn_DetAr
        Erase prn_HdAr

        prn_HdAr = New String(100, 10) {}

        prn_DetAr = New String(100, 50, 10) {}

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.Packing_Slip_No,a.cloth_idno,a.Total_Pcs as Pak_Pcs,a.Packing_Slip_Code , a.Total_Weight ,a.Total_Meters as Pak_Mtrs, tZ.*, c.*,d.*,E.*  from Packing_Slip_Head a INNER JOIN Company_head tZ ON a.company_idno = tZ.Company_Idno INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo INNER JOIN Ledger_Head d ON a.Ledger_IdNo = d.Ledger_IdNo  INNER JOIN ClothSales_Delivery_Head e ON e.ClothSales_Delivery_Code =  '" & Trim(NewCode) & "'  Where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Packing_Slip_Date, a.for_OrderBy, a.Packing_Slip_No, a.Packing_Slip_Code", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then
                For i = 0 To prn_HdDt.Rows.Count - 1

                    prn_HdMxIndx = prn_HdMxIndx + 1

                    prn_HdAr(prn_HdMxIndx, 1) = Trim(prn_HdDt.Rows(i).Item("Packing_Slip_No").ToString)
                    prn_HdAr(prn_HdMxIndx, 2) = Trim(prn_HdDt.Rows(i).Item("Cloth_Name").ToString)
                    prn_HdAr(prn_HdMxIndx, 3) = Val(prn_HdDt.Rows(i).Item("Pak_Pcs").ToString)
                    prn_HdAr(prn_HdMxIndx, 4) = Format(Val(prn_HdDt.Rows(i).Item("Pak_Mtrs").ToString), "#########0.00")

                    prn_DetMxIndx = 0

                    da2 = New SqlClient.SqlDataAdapter("select a.* from Packing_Slip_Details a where a.Packing_Slip_Code = '" & Trim(prn_HdDt.Rows(i).Item("Packing_Slip_Code").ToString) & "' order by a.Sl_No", con)
                    prn_DetDt = New DataTable
                    da2.Fill(prn_DetDt)

                    'If prn_DetDt.Rows.Count > 0 Then
                    '    For j = 0 To prn_DetDt.Rows.Count - 1
                    '        If Val(prn_DetDt.Rows(j).Item("Meters").ToString) <> 0 Then
                    '            prn_DetMxIndx = prn_DetMxIndx + 1
                    '            prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 5) = Trim(prn_HdDt.Rows(i).Item("Packing_Slip_No").ToString)
                    '            prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 0) = Trim(prn_DetDt.Rows(j).Item("Sl_No").ToString)
                    '            prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 1) = Trim(prn_DetDt.Rows(j).Item("Lot_No").ToString)
                    '            prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(j).Item("Pcs_No").ToString)
                    '            prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 3) = Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")

                    '            '  Total_mtrs = Total_mtrs + Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")
                    '            prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 4) = Format(Val(prn_DetDt.Rows(j).Item("Weight").ToString), "#########0.00")
                    '        End If
                    '    Next j
                    'End If

                    If prn_DetDt.Rows.Count > 0 Then
                        For j = 0 To prn_DetDt.Rows.Count - 1
                            If Val(prn_DetDt.Rows(j).Item("Meters").ToString) <> 0 Then
                                prn_DetMxIndx = prn_DetMxIndx + 1
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 5) = Trim(prn_HdDt.Rows(i).Item("Packing_Slip_No").ToString)
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 0) = Trim(prn_DetDt.Rows(j).Item("Sl_No").ToString)
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 1) = Trim(prn_DetDt.Rows(j).Item("Lot_No").ToString)
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(j).Item("Pcs_No").ToString)
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 3) = Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")

                                Total_mtrs = Total_mtrs + Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 4) = Format(Val(prn_DetDt.Rows(j).Item("Weight").ToString), "#########0.00")
                            End If
                        Next j
                    End If

                Next i

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument2_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument2.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        Common_Procedures.Printing_PackingSlip_Format1(PrintDocument2, e, prn_HdDt, prn_HdMxIndx, prn_DetMxIndx, prn_HdAr, prn_DetAr, prn_PageNo, prn_Count, prn_HdIndx, prn_DetIndx)
    End Sub

   

   

    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        If Trim(UCase(e.KeyCode)) = "D" And vcbo_KeyDwnVal = e.KeyValue Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
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

    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_date.LostFocus

        If IsDate(msk_date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_date.Text)) >= 2000 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_date.Text)
                End If
            End If

        End If
    End Sub

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
        End If
    End Sub
    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_date.Text = dtp_Date.Text
            msk_date.SelectionStart = 0
        End If
    End Sub
    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            cbo_PartyName.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            txt_Note.Focus()
        End If


        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
        End If

    End Sub

    Private Sub btn_Print_FormJJ_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print_FormJJ.Click
        Print_FormJJ()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub Print_FormJJ()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize
        Dim OrdBy_FrmNo As Single = 0, OrdByToNo As Single = 0
        Dim PpSzSTS As Boolean = False

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from ClothSales_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Delivery_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. All", "FOR FORMJJ PRINTING...", "4")
        prn_InpOpts = Replace(Trim(prn_InpOpts), "4", "123")

        For I = 0 To PrintDocument3.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument3.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument3.PrinterSettings.PaperSizes(I)
                PrintDocument3.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next


        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try
                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument3.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument3.PrinterSettings = PrintDialog1.PrinterSettings
                        PrintDocument3.Print()
                    End If

                Else
                    PrintDocument3.Print()

                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try


        Else
            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument3


                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()
                'If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                '    PrintDocument2.DefaultPageSettings = PageSetupDialog1.PageSettings
                '    ppd.ShowDialog()
                'End If

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If
    End Sub

    Private Sub PrintDocument3_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument3.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String
        Dim i As Integer, k As Integer
        Dim W1 As Single = 0

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_DetMxIndx = 0
        prn_NoofBmDets = 0
        prn_Count = 0

        Erase prn_HdAr
        Erase prn_DetAr

        prn_HdAr = New String(200, 10) {}
        prn_DetAr1 = New String(200, 10) {}

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Ledger_TinNo , c.Ledger_CstNo, c.Ledger_Name , c.Ledger_Address1, c.Ledger_Address2, c.Ledger_Address3, c.Ledger_Address4, d.Ledger_Name as Transport_Name, e.Area_Name from ClothSales_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_Idno = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.Transport_Idno = d.Ledger_IdNo  LEFT OUTER JOIN Area_Head e ON b.Area_Idno = e.Area_Idno   where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Delivery_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.ClothType_Name, c.Cloth_Name from ClothSales_Delivery_Details a LEFT OUTER JOIN ClothType_Head b on a.ClothType_IdNo = b.ClothType_IdNo INNER JOIN Cloth_Head c on a.Cloth_IdNo = c.Cloth_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                da1 = New SqlClient.SqlDataAdapter("select top 1  c.Cloth_Name, d.ClothType_Name, b.Total_Bales, b.Total_Pcs, b.Total_Meters from ClothSales_Delivery_Details a INNER JOIN ClothSales_Delivery_Head b ON a.ClothSales_Delivery_Code = b.ClothSales_Delivery_Code INNER JOIN Cloth_Head c on a.Cloth_IdNo = c.Cloth_IdNo LEFT OUTER JOIN ClothType_Head d on a.ClothType_IdNo = d.ClothType_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                prn_DetDt1 = New DataTable
                da1.Fill(prn_DetDt1)

                k = 0
                If prn_DetDt1.Rows.Count > 0 Then

                    For i = 0 To prn_DetDt1.Rows.Count - 1

                        If Val(prn_DetDt1.Rows(i).Item("Total_Meters").ToString) <> 0 Then
                            k = k + 1
                            prn_DetAr1(k + 100, 1) = Trim(prn_DetDt1.Rows(i).Item("Cloth_Name").ToString)
                            k = k + 1
                            prn_DetAr1(k + 100, 1) = Trim(prn_DetDt1.Rows(i).Item("ClothType_Name").ToString)
                            k = k + 1
                            prn_DetAr1(k + 100, 1) = Val(prn_DetDt1.Rows(i).Item("Total_Bales").ToString)
                            k = k + 1
                            prn_DetAr1(k + 100, 1) = Val(prn_DetDt1.Rows(i).Item("Total_Pcs").ToString)
                            k = k + 1
                            prn_DetAr1(k + 100, 1) = Format(Val(prn_DetDt1.Rows(i).Item("Total_Meters").ToString), "#########0.000")

                        End If

                    Next i

                End If
                Dt1.Clear()

                If k > prn_DetMxIndx Then prn_DetMxIndx = k

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument3_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument3.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        Printing_FormJJ(e)
    End Sub

    Private Sub Printing_FormJJ(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer = 0
        Dim flperc As Single = 0
        Dim flmtr As Single = 0
        Dim fmtr As Single = 0
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String


        For I = 0 To PrintDocument3.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument3.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument3.PrinterSettings.PaperSizes(I)
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument2.DefaultPageSettings.Margins
            .Left = 20
            .Right = 65
            .Top = 50 ' 60
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With


        pFont = New Font("Calibri", 10, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument3.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With
        If PrintDocument3.DefaultPageSettings.Landscape = True Then
            With PrintDocument3.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        NoofItems_PerPage = 5 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(35) : ClArr(2) = 300 : ClArr(3) = 100 : ClArr(4) = 95 : ClArr(5) = 110 : ClArr(6) = 80
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        TxtHgt = 19  ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        ''=========================================================================================================
        ''------  START OF PREPRINT POINTS
        ''=========================================================================================================

        'pFont = New Font("Calibri", 11, FontStyle.Regular)

        'Dim CurX As Single = 0
        'Dim pFont1 As Font

        'pFont1 = New Font("Calibri", 8, FontStyle.Regular)

        'For I = 100 To 1100 Step 300

        '    CurY = I
        '    For J = 1 To 850 Step 40

        '        CurX = J
        '        Common_Procedures.Print_To_PrintDocument(e, CurX, CurX, CurY - 20, 0, 0, pFont1)
        '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY, 0, 0, pFont)

        '        CurX = J + 20
        '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY, 0, 0, pFont)
        '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY + 20, 0, 0, pFont)
        '        Common_Procedures.Print_To_PrintDocument(e, CurX, CurX, CurY + 40, 0, 0, pFont1)

        '    Next

        'Next

        'For I = 200 To 800 Step 250

        '    CurX = I
        '    For J = 1 To 1200 Step 40

        '        CurY = J
        '        Common_Procedures.Print_To_PrintDocument(e, "-", CurX, CurY, 0, 0, pFont)
        '        Common_Procedures.Print_To_PrintDocument(e, "   " & CurY, CurX, CurY, 0, 0, pFont1)

        '        CurY = J + 20
        '        Common_Procedures.Print_To_PrintDocument(e, "--", CurX, CurY, 0, 0, pFont)
        '        Common_Procedures.Print_To_PrintDocument(e, "   " & CurY, CurX, CurY, 0, 0, pFont1)

        '    Next

        'Next

        'e.HasMorePages = False

        'Exit Sub

        ''=========================================================================================================
        ''------  END OF PREPRINT POINTS
        ''=========================================================================================================

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_FormJJ_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                NoofDets = 0

                CurY = CurY - 10
                W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_FormJJ_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 30 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 30
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt + 5

                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("ClothType_Name").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString), "########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)



                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_FormJJ_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

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
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_FormJJ_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClArr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Double = 0
        Dim C1 As Single, W1, W2 As Single, S1, S2 As Single
        Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2, Cmp_Add4 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim S As String = ""
        Dim goods_value As Double = 0
        Dim Fab_Value As Double = 0
        Dim pavu_value As Double = 0
        Dim NewCode As String = ""
        Dim To_Add As String = ""

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.* from ClothSales_Delivery_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo  where a.ClothSales_Delivery_Code = '" & Trim(NewCode) & "'", con)
        dt2 = New DataTable
        da2.Fill(dt2)

        Fab_Value = 0
        If dt2.Rows.Count > 0 Then
            For i = 0 To dt2.Rows.Count - 1
                If Val(dt2.Rows(prn_DetIndx).Item("ClothType_IdNo").ToString) = 1 Then
                    Fab_Value = Fab_Value + Format(Val(dt2.Rows(0).Item("Meters").ToString) * Val(dt2.Rows(0).Item("Sound_Rate").ToString), "#######0.00")
                ElseIf Val(dt2.Rows(prn_DetIndx).Item("ClothType_IdNo").ToString) = 2 Then
                    Fab_Value = Fab_Value + Format(Val(dt2.Rows(0).Item("Meters").ToString) * Val(dt2.Rows(0).Item("Seconds_Rate").ToString), "#######0.00")
                ElseIf Val(dt2.Rows(prn_DetIndx).Item("ClothType_IdNo").ToString) = 3 Then
                    Fab_Value = Fab_Value + Format(Val(dt2.Rows(0).Item("Meters").ToString) * Val(dt2.Rows(0).Item("Bits_Rate").ToString), "#######0.00")
                ElseIf Val(dt2.Rows(prn_DetIndx).Item("ClothType_IdNo").ToString) = 4 Then
                    Fab_Value = Fab_Value + Format(Val(dt2.Rows(0).Item("Meters").ToString) * Val(dt2.Rows(0).Item("Other_Rate").ToString), "#######0.00")
                ElseIf Val(dt2.Rows(prn_DetIndx).Item("ClothType_IdNo").ToString) = 5 Then
                    Fab_Value = Fab_Value + Format(Val(dt2.Rows(0).Item("Meters").ToString) * Val(dt2.Rows(0).Item("Reject_Rate").ToString), "#######0.00")
                End If
            Next i
        End If
        dt2.Clear()

        'da2 = New SqlClient.SqlDataAdapter("select Sum(a.Meters * b.Sound_Rate) as Value_Of_Sound ,sum(a.Meters * b.Seconds_Rate) as Value_Of_Sound, Sum(a.Meters * b.Bits_Rate) as Value_Of_Bits,Sum(a.Meters * b.Other_Rate) as Value_Of_Other,Sum(a.Meters * b.Reject_Rate) as Value_Of_Reject from ClothSales_Delivery_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo  where a.ClothSales_Delivery_Code = '" & Trim(NewCode) & "'", con)
        'dt2 = New DataTable
        'da2.Fill(dt2)

        'Fab_Value = 0
        'If dt2.Rows.Count > 0 Then
        '    If Val(prn_DetDt.Rows(prn_DetIndx).Item("ClothType_IdNo").ToString) <> 0 Then
        '        Fab_Value = Format(Val(dt2.Rows(0).Item("Value_Of_Sound").ToString), "#######0.00")
        '    ElseIf Fab_Value = Format(Val(dt2.Rows(0).Item("Value_Of_Second").ToString), "#######0.00") Then
        '    ElseIf Fab_Value = Format(Val(dt2.Rows(0).Item("Value_Of_Bits").ToString), "#######0.00") Then
        '    ElseIf Fab_Value = Format(Val(dt2.Rows(0).Item("Value_Of_Others").ToString), "#######0.00") Then
        '    ElseIf Fab_Value = Format(Val(dt2.Rows(0).Item("Value_Of_Rejects").ToString), "#######0.00") Then
        '    End If
        'End If
        'dt2.Clear()

        goods_value = Format(Val(Fab_Value), "#########0.00")

        dt2.Clear()
        dt3.Clear()

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
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 2 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If
            End If
        End If

        p1Font = New Font("Calibri", 20, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "FORM JJ", LMargin + 10, CurY - TxtHgt - 10, 0, 0, p1Font)

        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Desc = ""
        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        Cmp_Add4 = prn_HdDt.Rows(0).Item("Company_Address4").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If

        p1Font = New Font("Calibri", 15, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY NOTE", LMargin, CurY, 2, PrintWidth, p1Font)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "[See rule 15(3), 15(18), 15(19), 15(20), 15(21)]", LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "(for sales / stock transfer / works contract / labour)", LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "Asst Year : " & Trim(EntFnYrCode), LMargin, CurY, 2, PrintWidth, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "Asst Year : 15-16", LMargin, CurY, 2, PrintWidth, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        If prn_HdDt.Rows(0).Item("JJ_FormNo").ToString <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "NO  :  " & prn_HdDt.Rows(0).Item("JJ_FormNo").ToString, PageWidth - 10, CurY, 1, 0, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "NO  :  " & prn_HdDt.Rows(0).Item("ClothSales_Delivery_No").ToString, PageWidth - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        Try
            C1 = ClArr(1) + ClArr(2) + ClArr(3)

            W1 = e.Graphics.MeasureString("INVOICE DATE  : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width
            W2 = e.Graphics.MeasureString("Despatch To   : ", pFont).Width
            S2 = e.Graphics.MeasureString("Sent Through  : ", pFont).Width

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "1.(a) Name and address of the", LMargin + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + C1 + 10, CurY, 0, 0, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Consigner", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(b).TIN", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(c).CST Registration No", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "2.(a) Name and address of the", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "consignee / branch / agent", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(b).TIN", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(c).CST Registration No.", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_CstNo").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "3 Address", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(i) from which goods are consigned.", LMargin + 10, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Area_Name").ToString) <> "" Then
                Cmp_Add4 = Trim(prn_HdDt.Rows(0).Item("Area_Name").ToString)

            ElseIf Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString) <> "" Then
                Cmp_Add4 = Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString)

            ElseIf Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString) <> "" Then
                Cmp_Add4 = Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString)

            ElseIf Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString) <> "" Then
                Cmp_Add4 = Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString)

            End If

            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add4, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
                To_Add = Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)

            ElseIf Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) <> "" Then
                To_Add = Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString)

            ElseIf Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then
                To_Add = Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString)

            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(ii) to which goods are consigned.", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, To_Add, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "4.Description of goods consigned", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(a) Name of the goods", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Fabric", LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(b) Quantity Or Weight", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & prn_HdDt.Rows(0).Item("Total_Meters").ToString & " Mtrs", LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(c) Value of the goods", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Rs. " & Common_Procedures.Currency_Format(Val(goods_value)), LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "5.Purpose of transport", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(a) for sale / purchase", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(b) for shipment", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(c) transfer to branch/head office", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "/Consignment agent", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(d) for executionof works contract", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "(e) FOR LABOUR WORK / PROCESSING", LMargin + C1 + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "(e) for labour work / processing", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(8) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "6.To Whom delivered for transport", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Name").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "and vehicle no, if any", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "7.Remarks, if any", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClArr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CLOTHNAME", LMargin + ClArr(1), CurY, 2, ClArr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClArr(1) + ClArr(2), CurY, 2, ClArr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BALES", LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, 2, ClArr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, 2, ClArr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), CurY, 2, ClArr(6), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_FormJJ_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClArr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim W1 As Single = 0
        Dim vprn_BlNos As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim Cmp_Name As String

        Try
            W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClArr(1) + ClArr(2) + 30, CurY, 2, ClArr(4), pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Total_Bales").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bales").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                End If
            End If
            If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt - 15

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1), CurY, LMargin + ClArr(1), LnAr(9))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2), CurY, LMargin + ClArr(1) + ClArr(2), LnAr(9))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), LnAr(9))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), LnAr(9))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), LnAr(9))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), LnAr(9))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "We certify that to the best of my/our knowledge the particulare are true, correct and complete.", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(12) = CurY

            CurY = CurY + TxtHgt + 5

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Signature :", LMargin + 10, CurY, 0, 0, p1Font)

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            'Common_Procedures.Print_To_PrintDocument(e, "Signature :", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Name :", LMargin + 10, CurY, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Name :", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(13) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "Name and signature of the person to whom the goods were", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Name and signature of the consigner /", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 20, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "delivered for transporting with status of person signing", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "his employee / his representative", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 20, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Place : ", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Date : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothSales_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 20, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub
    
    Private Sub Amount_Calculation(ByVal CurRow As Integer, ByVal CurCol As Integer)
        Dim fldmtr As Double = 0
        Dim fmt As Double = 0
        Dim CloID As Integer
        Dim ConsYarn As Single
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim StkIn_For As String = ""
        Dim mtr_pcs As Single = 0
        On Error Resume Next

        With dgv_Details
            If .Visible Then

                If CurCol = 3 Or CurCol = 6 Or CurCol = 7 Then

                    If CurCol = 3 Or CurCol = 6 Or CurCol = 7 Then

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then

                            If Val(.Rows(CurRow).Cells(3).Value) = 0 Or Val(.Rows(CurRow).Cells(3).Value) = 100 Then

                                If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then

                                    CloID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(CurRow).Cells(1).Value)

                                    StkIn_For = ""
                                    mtr_pcs = 0

                                    Da = New SqlClient.SqlDataAdapter("Select Stock_In , Meters_Pcs from Cloth_Head Where Cloth_IdNo = " & Str(Val(CloID)), con)
                                    Dt2 = New DataTable
                                    Da.Fill(Dt2)
                                    If Dt2.Rows.Count > 0 Then
                                        StkIn_For = Dt2.Rows(0)("Stock_In").ToString
                                        mtr_pcs = Val(Dt2.Rows(0)("Meters_Pcs").ToString)
                                    End If
                                    Dt2.Clear()

                                    If Trim(StkIn_For) = "PCS" Then
                                        .Rows(CurRow).Cells(7).Value = Format(Val(.Rows(CurRow).Cells(6).Value) * Val(mtr_pcs), "#########0.00")
                                    End If

                                End If

                            Else

                                fmt = ((100 - Val(.Rows(CurRow).Cells(3).Value)) / 100) * Val(.Rows(CurRow).Cells(7).Value)


                                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Then
                                    fmt = Format(Math.Abs(Val(fmt)), "######0.00")
                                    fmt = Common_Procedures.Meter_RoundOff(fmt)

                                End If

                                If (100 - Val(.Rows(CurRow).Cells(3).Value)) > 0 Then
                                    fldmtr = Format(Val(.Rows(CurRow).Cells(7).Value) - Val(fmt), "#########0.00")
                                Else
                                    fldmtr = Format(Val(.Rows(CurRow).Cells(7).Value) + Val(fmt), "#########0.00")
                                End If



                            End If


                        End If

                    End If

                    Total_Calculation()

                End If

            End If
        End With
    End Sub

    Private Sub Printing_Bale_Delivery()
        Dim prtFrm As Single = 0
        Dim prtTo As Single = 0
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim Condt As String = ""
        Dim PpSzSTS As Boolean = False
        'Dim ps As Printing.PaperSize
        Dim NewCode As String = ""

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from Packing_Slip_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo <> 0 and a.Cloth_IdNo = b.Cloth_IdNo Where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then

                MessageBox.Show("No Entry Found", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 9X12", 900, 1200)
        PrintDocument4.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument4.DefaultPageSettings.PaperSize = pkCustomSize1

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try

                PrintDialog1.PrinterSettings = PrintDocument4.PrinterSettings
                If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    PrintDocument4.PrinterSettings = PrintDialog1.PrinterSettings
                    PrintDocument4.Print()
                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try

        Else

            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument4

                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()
                'If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                '    PrintDocument4.DefaultPageSettings = PageSetupDialog1.PageSettings
                '    ppd.ShowDialog()
                'End If

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Private Sub PrintDocument4_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument4.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String
        Dim i As Integer
        Dim OrdBy_FrmNo As Single = 0, OrdByToNo As Single = 0
        Dim LotSTS As Boolean = False
        Dim PrevBaleCd As String = ""
        Dim vSlNo As Integer = 0
        Dim vTotMtrs As Single = 0
        Dim vTotWgt As Single = 0

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_Prev_HeadIndx = -100
        prn_HeadIndx = 0
        prn_HdIndx = 0
        prn_HdMxIndx = 0
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_DetMxIndx = 0
        prn_NoofBaleDets = 0
        prn_Count = 0
        prn_TotBlWgt = 0
        prn_TotBlMtr = 0
        prn_TotBls = 0
        prn_BaleCode1 = ""
        prn_BaleCode2 = ""

        Erase prn_BLDetAr
        prn_BLDetAr = New String(1000, 10) {}

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as TransportName, e.Ledger_Name as Agent_Name from ClothSales_Delivery_Head a INNER JOIN Company_Head b ON a.Company_IdNo <> 0 and a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.Transport_IdNo = d.Ledger_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Agent_IdNo = e.Ledger_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothSales_Delivery_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.*, tC.* from Packing_Slip_Head a LEFT OUTER JOIN Packing_Slip_Details b ON a.Company_IdNo = b.Company_IdNo and a.Packing_Slip_Code = b.Packing_Slip_Code INNER JOIN Cloth_Head tC ON tC.Cloth_IdNo <> 0 and a.Cloth_IdNo = tC.Cloth_IdNo Where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Packing_Slip_Date, a.for_OrderBy, a.Packing_Slip_No, a.Packing_Slip_Code, b.Sl_No", con)
                'da2 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, tZ.*, tC.*, tP.* from Packing_Slip_Head a LEFT OUTER JOIN Packing_Slip_Details b ON a.Company_IdNo = b.Company_IdNo and a.Packing_Slip_Code = b.Packing_Slip_Code INNER JOIN ClothSales_Delivery_Head c ON c.ClothSales_Delivery_Code =  '" & Trim(NewCode) & "' and a.Delivery_Code = '" & Trim(Pk_Condition) & "' + c.ClothSales_Delivery_Code INNER JOIN Company_head tZ ON tZ.company_idno <> 0 and c.company_idno = tZ.Company_Idno INNER JOIN Cloth_Head tC ON tC.Cloth_IdNo <> 0 and a.Cloth_IdNo = tC.Cloth_IdNo LEFT OUTER JOIN Ledger_Head tP ON tP.Ledger_IdNo <> 0 and c.Ledger_IdNo = tP.Ledger_IdNo Where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Packing_Slip_Date, a.for_OrderBy, a.Packing_Slip_No, a.Packing_Slip_Code, b.Sl_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                If prn_DetDt.Rows.Count > 0 Then

                    PrevBaleCd = ""
                    vSlNo = 0
                    vTotMtrs = 0
                    vTotWgt = 0

                    For i = 0 To prn_DetDt.Rows.Count - 1

                        LotSTS = False
                        If IsDBNull(prn_DetDt.Rows(i).Item("Lot_No").ToString) = False Then
                            If Trim(prn_DetDt.Rows(i).Item("Lot_No").ToString) <> "" Then
                                LotSTS = True
                            End If
                        End If

                        prn_DetMxIndx = prn_DetMxIndx + 1
                        If Trim(UCase(PrevBaleCd)) <> Trim(UCase(prn_DetDt.Rows(i).Item("Packing_Slip_Code").ToString)) Then

                            If Trim(UCase(PrevBaleCd)) <> "" Then
                                prn_BLDetAr(prn_DetMxIndx, 1) = ""
                                prn_BLDetAr(prn_DetMxIndx, 2) = ""
                                prn_BLDetAr(prn_DetMxIndx, 3) = ""
                                prn_BLDetAr(prn_DetMxIndx, 4) = ""
                                prn_BLDetAr(prn_DetMxIndx, 5) = "BALETOTALLINE"

                                prn_DetMxIndx = prn_DetMxIndx + 1
                                prn_BLDetAr(prn_DetMxIndx, 1) = ""
                                prn_BLDetAr(prn_DetMxIndx, 2) = ""
                                prn_BLDetAr(prn_DetMxIndx, 3) = Format(Val(vTotMtrs), "#########0.00")
                                prn_BLDetAr(prn_DetMxIndx, 4) = Format(Val(vTotWgt), "#########0.000")
                                prn_BLDetAr(prn_DetMxIndx, 5) = "BALETOTAL"

                                prn_DetMxIndx = prn_DetMxIndx + 1
                                prn_BLDetAr(prn_DetMxIndx, 1) = ""
                                prn_BLDetAr(prn_DetMxIndx, 2) = ""
                                prn_BLDetAr(prn_DetMxIndx, 3) = ""
                                prn_BLDetAr(prn_DetMxIndx, 4) = ""
                                prn_BLDetAr(prn_DetMxIndx, 5) = "BALETOTALLINE"

                                'prn_BLDetAr(prn_DetMxIndx, 1) = ""
                                'prn_BLDetAr(prn_DetMxIndx, 2) = ""
                                'prn_BLDetAr(prn_DetMxIndx, 3) = ""
                                'prn_BLDetAr(prn_DetMxIndx, 4) = ""
                                'prn_BLDetAr(prn_DetMxIndx, 5) = "BLANKROW"
                            End If

                            prn_DetMxIndx = prn_DetMxIndx + 1
                            prn_BLDetAr(prn_DetMxIndx, 1) = Trim(prn_DetDt.Rows(i).Item("Packing_Slip_No").ToString)

                        End If

                        vSlNo = vSlNo + 1
                        prn_BLDetAr(prn_DetMxIndx, 0) = Trim(vSlNo)

                        If LotSTS = True Then
                            prn_BLDetAr(prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(i).Item("Lot_No").ToString) & "/" & Trim(prn_DetDt.Rows(i).Item("Pcs_No").ToString)
                            prn_BLDetAr(prn_DetMxIndx, 3) = Format(Val(prn_DetDt.Rows(i).Item("Meters").ToString), "#########0.00")
                            prn_BLDetAr(prn_DetMxIndx, 4) = Format(Val(prn_DetDt.Rows(i).Item("Weight").ToString), "#########0.000")
                        Else
                            prn_BLDetAr(prn_DetMxIndx, 2) = ""
                            prn_BLDetAr(prn_DetMxIndx, 3) = Format(Val(prn_DetDt.Rows(i).Item("Total_Meters").ToString), "#########0.00")
                            prn_BLDetAr(prn_DetMxIndx, 4) = Format(Val(prn_DetDt.Rows(i).Item("Total_Weight").ToString), "#########0.000")
                        End If

                        vTotMtrs = vTotMtrs + Val(prn_DetDt.Rows(i).Item("Total_Meters").ToString)
                        vTotWgt = vTotWgt + Val(prn_DetDt.Rows(i).Item("Total_Weight").ToString)

                        If Val(prn_BLDetAr(prn_DetMxIndx, 4)) = 0 Then prn_BLDetAr(prn_DetMxIndx, 4) = ""
                        prn_BLDetAr(prn_DetMxIndx, 5) = ""

                        PrevBaleCd = prn_DetDt.Rows(i).Item("Packing_Slip_Code").ToString

                    Next i

                    prn_DetMxIndx = prn_DetMxIndx + 1
                    prn_BLDetAr(prn_DetMxIndx, 1) = ""
                    prn_BLDetAr(prn_DetMxIndx, 2) = ""
                    prn_BLDetAr(prn_DetMxIndx, 3) = ""
                    prn_BLDetAr(prn_DetMxIndx, 4) = ""
                    prn_BLDetAr(prn_DetMxIndx, 5) = "BALETOTALLINE"

                    prn_DetMxIndx = prn_DetMxIndx + 1
                    prn_BLDetAr(prn_DetMxIndx, 1) = ""
                    prn_BLDetAr(prn_DetMxIndx, 2) = ""
                    prn_BLDetAr(prn_DetMxIndx, 3) = Format(Val(vTotMtrs), "#########0.00")
                    prn_BLDetAr(prn_DetMxIndx, 4) = Format(Val(vTotWgt), "#########0.000")
                    prn_BLDetAr(prn_DetMxIndx, 5) = "BALETOTAL"

                    prn_DetMxIndx = prn_DetMxIndx + 1
                    prn_BLDetAr(prn_DetMxIndx, 1) = ""
                    prn_BLDetAr(prn_DetMxIndx, 2) = ""
                    prn_BLDetAr(prn_DetMxIndx, 3) = ""
                    prn_BLDetAr(prn_DetMxIndx, 4) = ""
                    prn_BLDetAr(prn_DetMxIndx, 5) = "BALETOTALLINE"

                End If

            Else

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()
            da2.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument4_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument4.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        Printing_Bale_Format2_PrePrint_Anuman(e)
    End Sub

    Private Sub Printing_Bale_Format2_PrePrint_Anuman(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim EntryCode As String
        Dim I As Integer = 0
        Dim NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim MilNm1 As String = "", MilNm2 As String = ""
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim TotMtr1 As Single = 0, TotMtr2 As Single = 0
        Dim TotWgt1 As Single = 0, TotWgt2 As Single = 0
        Dim IncY As Single = 0



        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 9X12", 900, 1200)
        PrintDocument4.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument4.DefaultPageSettings.PaperSize = pkCustomSize1
        e.PageSettings.PaperSize = pkCustomSize1
        PrintDocument4.DefaultPageSettings.Landscape = False

        With PrintDocument4.DefaultPageSettings.Margins
            .Left = 0
            .Right = 0
            .Top = 0
            .Bottom = 0
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom

        End With

        pFont = New Font("Arial", 12, FontStyle.Bold)
        'pFont = New Font("Calibri", 11, FontStyle.Bold)

        With PrintDocument4.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        NoofItems_PerPage = 30

        Erase LnAr
        Erase ClArr
        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        'TxtHgt = e.Graphics.MeasureString("A", pFont).Height  '18.69
        TxtHgt = 19 ' e.Graphics.MeasureString("A", pFont).Height  '18.69

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(EntFnYrCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then

                    Printing_Bale_Format2_PrePrint_Anuman_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                    NoofDets = 0
                    TotMtr1 = 0 : TotMtr2 = 0
                    TotWgt1 = 0 : TotWgt2 = 0

                    If prn_DetMxIndx > 0 Then

                        CurY = TMargin + 380

                        Do While prn_NoofBaleDets < prn_DetMxIndx

                            If NoofDets >= NoofItems_PerPage Then

                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                CurY = TMargin + 990
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(TotMtr1), "#########0.00"), LMargin + 270, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(TotWgt1), "#########0.000"), LMargin + 370, CurY, 1, 0, pFont)
                                If Val(TotMtr2) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(TotMtr2), "#########0.00"), LMargin + 680, CurY, 1, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(TotWgt2), "#########0.000"), LMargin + 770, CurY, 1, 0, pFont)
                                End If

                                Printing_Bale_Format2_PrePrint_Anuman_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                                prn_DetIndx = prn_DetIndx + NoofItems_PerPage
                                prn_DetSNo = prn_DetSNo + NoofItems_PerPage

                                e.HasMorePages = True

                                Return

                            End If


                            prn_DetIndx = prn_DetIndx + 1

                            CurY = CurY + TxtHgt

                            If Val(prn_BLDetAr(prn_DetIndx, 3)) <> 0 Or Val(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 3)) <> 0 Then

                                prn_DetSNo = prn_DetSNo + 1

                                If Val(prn_BLDetAr(prn_DetIndx, 3)) <> 0 Or Trim(UCase(prn_BLDetAr(prn_DetIndx, 5))) = "BALETOTAL" Or Trim(UCase(prn_BLDetAr(prn_DetIndx, 5))) = "BLANKROW" Then

                                    IncY = 0
                                    If Trim(UCase(prn_BLDetAr(prn_DetIndx, 5))) = "BALETOTAL" Then
                                        IncY = 10
                                    End If

                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_BLDetAr(prn_DetIndx, 0)), LMargin + 20, CurY - IncY, 0, 0, pFont)
                                    'Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 20, CurY-IncY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_BLDetAr(prn_DetIndx, 1)), LMargin + 60, CurY - IncY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_BLDetAr(prn_DetIndx, 2)), LMargin + 140, CurY - IncY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_BLDetAr(prn_DetIndx, 3)), LMargin + 270, CurY - IncY, 1, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_BLDetAr(prn_DetIndx, 4)), LMargin + 370, CurY - IncY, 1, 0, pFont)

                                    TotMtr1 = TotMtr1 + Val(prn_BLDetAr(prn_DetIndx, 3))
                                    TotWgt1 = TotWgt1 + Val(prn_BLDetAr(prn_DetIndx, 4))

                                    prn_TotBls = prn_TotBls + 1
                                    prn_TotBlMtr = prn_TotBlMtr + Val(prn_BLDetAr(prn_DetIndx, 3))
                                    prn_TotBlWgt = prn_TotBlWgt + Val(prn_BLDetAr(prn_DetIndx, 4))

                                    prn_NoofBaleDets = prn_NoofBaleDets + 1

                                    prn_BaleCode1 = Trim(prn_BLDetAr(prn_DetIndx, 1))

                                Else

                                    If Trim(UCase(prn_BLDetAr(prn_DetIndx, 5))) = "BALETOTALLINE" Then
                                        'CurY = CurY + TxtHgt
                                        e.Graphics.DrawLine(Pens.Black, LMargin + 5, CurY, LMargin + 380, CurY)
                                        prn_NoofBaleDets = prn_NoofBaleDets + 1
                                    End If

                                End If


                                If Val(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 3)) <> 0 Or Trim(UCase(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 5))) = "BALETOTAL" Or Trim(UCase(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 5))) = "BLANKROW" Then

                                    IncY = 0
                                    If Trim(UCase(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 5))) = "BALETOTAL" Then
                                        IncY = 10
                                    End If

                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 0)), LMargin + 410, CurY - IncY, 0, 0, pFont)
                                    'Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo + NoofItems_PerPage)), LMargin + 410, CurY-IncY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 1)), LMargin + 460, CurY - IncY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 2)), LMargin + 540, CurY - IncY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 3)), LMargin + 680, CurY - IncY, 1, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 4)), LMargin + 770, CurY - IncY, 1, 0, pFont)

                                    TotMtr2 = TotMtr2 + Val(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 3))
                                    TotWgt2 = TotWgt2 + Val(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 4))

                                    prn_TotBls = prn_TotBls + 1
                                    prn_TotBlMtr = prn_TotBlMtr + Val(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 3))
                                    prn_TotBlWgt = prn_TotBlWgt + Val(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 4))

                                    prn_NoofBaleDets = prn_NoofBaleDets + 1

                                    prn_BaleCode2 = Trim(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 1))

                                Else

                                    If Trim(UCase(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 5))) = "BALETOTALLINE" Then
                                        'CurY = CurY + TxtHgt
                                        e.Graphics.DrawLine(Pens.Black, LMargin + 400, CurY, LMargin + 775, CurY)
                                        prn_NoofBaleDets = prn_NoofBaleDets + 1
                                    End If

                                End If


                            Else

                                If Trim(UCase(prn_BLDetAr(prn_DetIndx, 5))) = "BALETOTALLINE" Then
                                    e.Graphics.DrawLine(Pens.Black, LMargin + 5, CurY, LMargin + 380, CurY)
                                    prn_NoofBaleDets = prn_NoofBaleDets + 1
                                End If
                                If Trim(UCase(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 5))) = "BALETOTALLINE" Then
                                    e.Graphics.DrawLine(Pens.Black, LMargin + 400, CurY, LMargin + 775, CurY)
                                    prn_NoofBaleDets = prn_NoofBaleDets + 1
                                End If

                                If Trim(UCase(prn_BLDetAr(prn_DetIndx, 5))) = "BLANKROW" Then
                                    prn_NoofBaleDets = prn_NoofBaleDets + 1
                                End If
                                If Trim(UCase(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 5))) = "BLANKROW" Then
                                    prn_NoofBaleDets = prn_NoofBaleDets + 1
                                End If
                                If Trim(UCase(prn_BLDetAr(prn_DetIndx, 5))) <> "BLANKROW" And Trim(UCase(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 5))) <> "BLANKROW" And Trim(UCase(prn_BLDetAr(prn_DetIndx, 5))) <> "BALETOTALLINE" And Trim(UCase(prn_BLDetAr(prn_DetIndx + NoofItems_PerPage, 5))) <> "BALETOTALLINE" Then
                                    prn_NoofBaleDets = prn_NoofBaleDets + 1
                                End If

                            End If

                            NoofDets = NoofDets + 1

                        Loop

                    End If

                    CurY = TMargin + 990
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(TotMtr1), "#########0.00"), LMargin + 270, CurY, 1, 0, pFont)
                    If Val(TotWgt1) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(TotWgt1), "#########0.000"), LMargin + 370, CurY, 1, 0, pFont)
                    End If
                    If Val(TotMtr2) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(TotMtr2), "#########0.00"), LMargin + 680, CurY, 1, 0, pFont)
                        If Val(TotWgt2) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(TotWgt2), "#########0.000"), LMargin + 770, CurY, 1, 0, pFont)
                        End If
                    End If

                    Printing_Bale_Format2_PrePrint_Anuman_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

                End If

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        prn_HeadIndx = prn_HeadIndx + 1

        'If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then
        '    e.HasMorePages = True
        'Else
        e.HasMorePages = False
        'End If

    End Sub

    Private Sub Printing_Bale_Format2_PrePrint_Anuman_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim p1Font As Font
        Dim CurX As Single = 0


        PageNo = PageNo + 1

        'da2 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, tZ.*, tC.*, tP.* from Packing_Slip_Head a LEFT OUTER JOIN Packing_Slip_Details b ON a.Company_IdNo = b.Company_IdNo and a.Packing_Slip_Code = b.Packing_Slip_Code INNER JOIN ClothSales_Delivery_Head c ON c.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and c.ClothSales_Delivery_Code =  '" & Trim(EntryCode) & "' and a.Delivery_Code = '" & Trim(Pk_Condition) & "' + c.ClothSales_Delivery_Code INNER JOIN Company_head tZ ON tZ.company_idno <> 0 and c.company_idno = tZ.Company_Idno INNER JOIN Cloth_Head tC ON tC.Cloth_IdNo <> 0 and a.Cloth_IdNo = tC.Cloth_IdNo LEFT OUTER JOIN Ledger_Head tP ON tP.Ledger_IdNo <> 0 and c.Ledger_IdNo = tP.Ledger_IdNo Where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Packing_Slip_Date, a.for_OrderBy, a.Packing_Slip_No, a.Packing_Slip_Code, b.Sl_No", con)
        'dt2 = New DataTable
        'da2.Fill(dt2)
        'If dt2.Rows.Count > NoofItems_PerPage Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        'End If
        'dt2.Clear()

        Try

            CurX = LMargin + 60 ' 40  '150
            CurY = TMargin + 140 ' 122 ' 100

            p1Font = New Font("Arial", 13, FontStyle.Bold)
            'p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "M/s." & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_MainName").ToString, CurX, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString, CurX, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString, CurX, CurY, 0, 0, pFont)
            End If
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString, CurX, CurY, 0, 0, pFont)
            End If
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString, CurX, CurY, 0, 0, pFont)
            End If
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_TinNo").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " Tin No : " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_TinNo").ToString, CurX, CurY, 0, 0, pFont)
            End If


            CurX = LMargin + 580
            CurY = TMargin + 140
            p1Font = New Font("Arial", 14, FontStyle.Bold)
            'p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("ClothSales_Delivery_No").ToString, CurX, CurY, 0, 0, pFont)

            CurY = TMargin + 170
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("ClothSales_Delivery_Date").ToString), "dd-MM-yyyy"), CurX, CurY, 0, 0, pFont)

            CurY = TMargin + 215
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Party_OrderNo").ToString, CurX, CurY, 0, 0, pFont)


            If prn_DetDt.Rows.Count > 0 Then
                CurX = LMargin + 60
                CurY = TMargin + 260
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Cloth_Name").ToString, CurX, CurY, 0, 0, pFont)


                CurX = LMargin + 540
                CurY = TMargin + 260
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Weave").ToString, CurX, CurY, 0, 0, pFont)

            End If

            CurX = LMargin + 120
            CurY = TMargin + 300
            If Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(prn_HeadIndx).Item("Agent_IdNo").ToString)) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(prn_HeadIndx).Item("Agent_IdNo").ToString)), CurX, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, " DIRECT", CurX, CurY, 0, 0, pFont)
            End If

            CurY = TMargin + 330
            If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Transport_idNo").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(prn_HeadIndx).Item("Transport_idNo").ToString)), CurX, CurY, 0, 0, pFont)
            End If


            CurX = LMargin + 540
            CurY = TMargin + 300
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Despatch_To").ToString, CurX, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Delivery_Address1").ToString, CurX, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Delivery_Address2").ToString, CurX, CurY, 0, 0, pFont)


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Bale_Format2_PrePrint_Anuman_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim CurX As Single = 0

        Try

            If is_LastPage = True Then

                CurX = LMargin + 120
                CurY = TMargin + 1050
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_TotBls), CurX, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Bales").ToString, CurX, CurY, 0, 0, pFont)

                CurY = TMargin + 1080
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_TotBlMtr), "###########0.00"), CurX, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString, CurX, CurY, 0, 0, pFont)

                CurY = TMargin + 1120
                If Val(prn_TotBlWgt) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_TotBlWgt), "###########0.00"), CurX, CurY, 0, 0, pFont)
                End If

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub set_PaperSize_For_PrintDocument1()
        Dim I As Integer = 0
        Dim PpSzSTS As Boolean = False
        Dim ps As Printing.PaperSize


        If Val(Common_Procedures.settings.Printing_For_HalfSheet_Set_Custom8X6_As_Default_PaperSize) = 1 Then
            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.Landscape = False

        ElseIf Val(Common_Procedures.settings.Printing_For_HalfSheet_Set_A4_As_Default_PaperSize) = 1 Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next

        Else

            PpSzSTS = False

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                'Debug.Print(ps.PaperName)
                If ps.Width = 800 And ps.Height = 600 Then
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PpSzSTS = True
                    Exit For
                End If
            Next

            If PpSzSTS = False Then

                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        PpSzSTS = True
                        Exit For
                    End If
                Next

                If PpSzSTS = False Then
                    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                            PrintDocument1.DefaultPageSettings.PaperSize = ps
                            Exit For
                        End If
                    Next
                End If

            End If

        End If

    End Sub

    Private Sub cbo_RollBundle_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RollBundle.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_RollBundle, cbo_PartyName, Nothing, "", "", "", "")
        If (e.KeyValue = 40 And cbo_RollBundle.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            Else
                txt_Note.Focus()

            End If
        End If
    End Sub

    Private Sub cbo_RollBundle_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_RollBundle.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_RollBundle, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            Else
                txt_Note.Focus()

            End If
        End If
    End Sub

   
End Class
