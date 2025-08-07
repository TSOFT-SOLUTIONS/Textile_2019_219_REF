Public Class Stores_Service_Item_Receipt_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "SERRC-"
    Private Pk_Condition1 As String = "SERAT-"
    Private cbo_KeyDwnVal As Double
    Private Prec_ActCtrl As New Control
    Private dgv_DrawNo As String = ""
    Private vCbo_ItmNm As String = ""
    Private vCloPic_STS As Boolean = False
    Private NoCalc_Status As Boolean = False
    Private vcbo_KeyDwnVal As Double
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer

    Private Sub clear()
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False

        New_Entry = False

        lbl_RecNo.Text = ""
        lbl_RecNo.ForeColor = Color.Black
        cbo_Received.Text = ""
        cbo_Issued.Text = ""
        cbo_New_Old.Text = "OLD"
        cbo_Ledger.Text = ""
        txt_Particulars.Text = ""
        txt_Party_DcNo.Text = ""
        cbo_PaymentAc.Text = ""
        txt_AddLess.Text = ""
        lbl_TaxableAmount.Text = ""
        lbl_NetAmount.Text = ""
        txt_SGST_Perc.Text = ""
        txt_IGST_Perc.Text = ""
        txt_CGST_Perc.Text = ""
        lbl_CGstAmount.Text = ""
        lbl_SGstAmount.Text = ""
        lbl_IGstAmount.Text = ""
        cbo_Transport.Text = ""
        txt_Remarks.Text = ""
        txt_Transport_Freight.Text = ""

        dgv_Details.Rows.Clear()

        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        cbo_Grid_Department.Text = False
        cbo_Grid_Item.Visible = False
        cbo_Grid_Brand.Text = False
        cbo_Grid_Machine.Visible = False
        cbo_Grid_Unit.Visible = False

        PictureBox1.Image = Nothing
        vCloPic_STS = False

        dgv_DrawNo = ""
        vCbo_ItmNm = ""

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

        If Me.ActiveControl.Name <> cbo_Grid_Department.Name Then
            cbo_Grid_Department.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Item.Name Then
            cbo_Grid_Item.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Brand.Name Then
            cbo_Grid_Brand.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Machine.Name Then
            cbo_Grid_Machine.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Unit.Name Then
            cbo_Grid_Unit.Visible = False
        End If
        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Grid_DeSelect()
            pnl_Picture.Visible = False
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

    Private Sub Service_Item_Receipt_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer = 0
        Dim CompCondt As String = ""

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Department.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "DEPARTMENT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Department.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Item.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ITEM" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Item.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Brand.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BRAND" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Brand.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Unit.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "UNIT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Unit.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Machine.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MACHINE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Machine.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PaymentAc.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PaymentAc.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Service_Item_Receipt_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Service_Item_Receipt_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Picture.Visible = True Then
                    btn_ClosePicture_Click(sender, e)
                    Exit Sub


                Else
                    Close_Form()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Service_Item_Receipt_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim dt8 As New DataTable
        Dim dt9 As New DataTable

        Me.Text = ""

        cbo_New_Old.Items.Clear()
        cbo_New_Old.Items.Add("NEW")
        cbo_New_Old.Items.Add("OLD")
        cbo_New_Old.Items.Add("SCRAP")

        con.Open()

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) order by Ledger_DisplayName", con)
        da.Fill(dt)
        cbo_Filter_Ledger.DataSource = dt
        cbo_Filter_Ledger.DisplayMember = "Ledger_DisplayName"




        da = New SqlClient.SqlDataAdapter("select Department_Name from Department_Head order by Department_Name", con)
        da.Fill(dt1)
        cbo_Grid_Department.DataSource = dt1
        cbo_Grid_Department.DisplayMember = "Department_Name"

        da = New SqlClient.SqlDataAdapter("select Item_DisplayName from Stores_Item_AlaisHead order by Item_DisplayName", con)
        da.Fill(dt2)
        cbo_Grid_Item.DataSource = dt2
        cbo_Grid_Item.DisplayMember = "Item_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Brand_Name from Brand_Head order by Brand_Name", con)
        da.Fill(dt3)
        cbo_Grid_Brand.DataSource = dt3
        cbo_Grid_Brand.DisplayMember = "Brand_Name"

        da = New SqlClient.SqlDataAdapter("select Unit_Name from Unit_Head order by Unit_Name", con)
        da.Fill(dt4)
        cbo_Grid_Unit.DataSource = dt4
        cbo_Grid_Unit.DisplayMember = "Unit_Name"

        da = New SqlClient.SqlDataAdapter("select Machine_Name from Machine_Head order by Machine_Name", con)
        da.Fill(dt5)
        cbo_Grid_Machine.DataSource = dt5
        cbo_Grid_Machine.DisplayMember = "Machine_Name"

        da = New SqlClient.SqlDataAdapter("select distinct(Issued_Name) from Stores_Service_Receipt_Head order by Issued_Name", con)
        da.Fill(dt6)
        cbo_Issued.DataSource = dt6
        cbo_Issued.DisplayMember = "Issued_Name"

        da = New SqlClient.SqlDataAdapter("select distinct(Received_Name) from Stores_Service_Receipt_Head order by Received_Name ", con)
        da.Fill(dt7)
        cbo_Received.DataSource = dt7
        cbo_Received.DisplayMember = "Received_Name"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) order by Ledger_DisplayName", con)
        da.Fill(dt8)
        cbo_Ledger.DataSource = dt8
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"


        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (AccountsGroup_IdNo = 5 or AccountsGroup_IdNo = 6 or AccountsGroup_IdNo = 15 or AccountsGroup_IdNo = 16 ) order by Ledger_DisplayName", con)
        da.Fill(dt9)
        cbo_PaymentAc.DataSource = dt9
        cbo_PaymentAc.DisplayMember = "Ledger_DisplayName"

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        pnl_Picture.Visible = False
        pnl_Picture.Left = (Me.Width - pnl_Picture.Width) - 25
        pnl_Picture.Top = (Me.Height - pnl_Picture.Height) - 50
        pnl_Picture.BringToFront()

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()




        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        AddHandler dtp_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Department.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_New_Old.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Issued.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Received.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Item.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Brand.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Unit.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Machine.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Item.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Particulars.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Party_DcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PaymentAc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SGST_Perc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CGST_Perc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_IGST_Perc.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Transport_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Department.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_New_Old.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Issued.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Received.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Item.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Brand.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Unit.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Machine.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Item.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Particulars.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Party_DcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PaymentAc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess.LostFocus, AddressOf ControlLostFocus


        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Transport_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SGST_Perc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CGST_Perc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_IGST_Perc.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_Party_DcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Transport_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SGST_Perc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CGST_Perc.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Party_DcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Transport_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AddLess.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CGST_Perc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_SGST_Perc.KeyPress, AddressOf TextBoxControlKeyPress

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


                        If .CurrentCell.ColumnIndex >= 11 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_AddLess.Focus()
                            ElseIf .CurrentCell.ColumnIndex = 5 Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(11)

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(5)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(11)

                        End If

                        Return True



                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex = 5 Then
                            If .CurrentCell.RowIndex = 0 Then
                                txt_Particulars.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(11)

                            End If
                        ElseIf .CurrentCell.ColumnIndex = 11 Then
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(5)
                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(5)

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

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)


        Try


            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_DisplayName ,c.Ledger_DisplayName as payAccName from Stores_Service_Receipt_Head a INNER JOIN Ledger_AlaisHead b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Ledger_AlaisHead c ON a.Payment_Acc_IdNo = c.Ledger_IdNo Where a.Service_Receipt_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RecNo.Text = dt1.Rows(0).Item("Service_Receipt_No").ToString
                dtp_date.Text = dt1.Rows(0).Item("Service_Receipt_Date").ToString
                cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_DisplayName").ToString
                cbo_Received.Text = dt1.Rows(0).Item("Received_name").ToString
                cbo_Issued.Text = dt1.Rows(0).Item("Issued_Name").ToString
                cbo_New_Old.Text = dt1.Rows(0).Item("New_old").ToString
                txt_Particulars.Text = dt1.Rows(0).Item("Particulars").ToString
                txt_Party_DcNo.Text = dt1.Rows(0).Item("Party_DcNo").ToString
                cbo_PaymentAc.Text = dt1.Rows(0).Item("payAccName").ToString
                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                txt_Transport_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight").ToString), "#########0.00")
                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString
                lbl_TaxableAmount.Text = dt1.Rows(0).Item("Net_Amount").ToString
                lbl_NetAmount.Text = Format(Val(dt1.Rows(0).Item("Total_Net_Amount").ToString), "###########0.00")
                txt_CGST_Perc.Text = dt1.Rows(0).Item("CGST_Percentage").ToString
                lbl_CGstAmount.Text = Format(Val(dt1.Rows(0).Item("CGST_Amount").ToString), "##########0.00")
                txt_SGST_Perc.Text = dt1.Rows(0).Item("SGST_Percentage").ToString
                lbl_SGstAmount.Text = Format(Val(dt1.Rows(0).Item("SGST_Amount").ToString), "###########0.00")
                txt_IGST_Perc.Text = dt1.Rows(0).Item("IGST_Percentage").ToString
                lbl_IGstAmount.Text = Format(Val(dt1.Rows(0).Item("IGST_Amount").ToString), "###########0.00")




                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_name, b.Drawing_No, c.Department_name, d.Unit_name, e.Machine_name, f.Brand_Name from Stores_Service_Receipt_Details a INNER JOIN Stores_item_head b ON a.Item_idno = b.Item_idno LEFT OUTER JOIN Department_Head c ON b.Department_idno = c.Department_idno LEFT OUTER JOIN Unit_Head d ON a.Unit_idno = d.Unit_idno LEFT OUTER JOIN Machine_Head e ON a.Machine_idno = e.Machine_idno LEFT OUTER JOIN Brand_Head f ON a.Brand_idno = f.Brand_idno where a.Service_Receipt_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1

                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)

                        If IsDBNull(dt2.Rows(i).Item("Department_name").ToString) = False Then
                            If Trim(dt2.Rows(i).Item("Department_name").ToString) <> "" Then
                                dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Department_name").ToString
                            Else
                                dgv_Details.Rows(n).Cells(1).Value = Common_Procedures.Department_IdNoToName(con, 1)
                            End If
                        End If

                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Drawing_No").ToString
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Item_name").ToString
                        dgv_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Brand_name").ToString
                        dgv_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Quantity").ToString), "########0")
                        dgv_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Unit_name").ToString
                        dgv_Details.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Machine_name").ToString
                        dgv_Details.Rows(n).Cells(8).Value = dt2.Rows(i).Item("Service_Delivery_No").ToString
                        dgv_Details.Rows(n).Cells(9).Value = dt2.Rows(i).Item("Service_Delivery_Code").ToString
                        dgv_Details.Rows(n).Cells(10).Value = dt2.Rows(i).Item("Service_Delivery_Details_SlNo").ToString
                        dgv_Details.Rows(n).Cells(11).Value = dt2.Rows(i).Item("Rate").ToString
                        dgv_Details.Rows(n).Cells(12).Value = dt2.Rows(i).Item("Amount").ToString

                    Next i

                End If

                lbl_TaxableAmount.Text = dt1.Rows(0).Item("Net_Amount").ToString

                With dgv_Details_Total
                    .Rows.Clear()
                    .Rows.Add()
                    .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Total_Quantity").ToString), "########0")
                    .Rows(0).Cells(10).Value = Format(Val(dt1.Rows(0).Item("Amount").ToString), "########0.00")
                End With

                txt_AddLess.Text = dt1.Rows(0).Item("Add_Less_Amount").ToString
                ' lbl_TaxableAmount.Text = dt1.Rows(0).Item("Net_Amount").ToString
                ' lbl_NetAmount.Text = dt1.Rows(0).Item("Total_Net_Amount").ToString

                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Dispose()
            da1.Dispose()

            Grid_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If dtp_date.Visible And dtp_date.Enabled Then dtp_date.Focus()

        End Try

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text)

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_Stores_Item_Receipt, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_Stores_Service_Item_Receipt, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub



        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Store_Service_Receipt_Entry, New_Entry, Me, con, "Stores_Service_Receipt_Head", "Service_Receipt_Code", NewCode, "Service_Receipt_Date", "(Service_Receipt_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub



        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other windows", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


            cmd.Connection = con
            cmd.Transaction = trans

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Stores_Service_receipt_Head", "Service_receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Service_receipt_Code, Company_IdNo, for_OrderBy", trans)

            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Stores_Service_receipt_Details", "Service_receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "Item_IdNo, Unit_IdNo, Brand_IdNo, Quantity_New, Quantity_Old_Usable, Quantity_Old_Scrap", "Sl_No", "Service_receipt_Code, For_OrderBy, Company_IdNo, Service_receipt_No, Service_receipt_Date, Ledger_Idno", trans)


            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), trans)


            cmd.CommandText = "Update Stores_Service_Delivery_Details Set Receipt_Quantity = a.Receipt_Quantity - b.Quantity from Stores_Service_Delivery_Details a, Stores_Service_Receipt_Details b where b.Service_Receipt_Code = '" & Trim(NewCode) & "' and a.Service_Delivery_Code = b.Service_Delivery_Code and a.Service_Delivery_Details_SlNo = b.Service_Delivery_Details_SlNo"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stores_Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stores_Service_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Service_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Stores_Service_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Service_Receipt_Code = '" & Trim(NewCode) & "'"
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

        Finally

            If dtp_date.Enabled = True And dtp_date.Visible = True Then dtp_date.Focus()

        End Try
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead order by Ledger_DisplayName", con)
            da.Fill(dt2)
            cbo_Filter_Ledger.DataSource = dt2
            cbo_Filter_Ledger.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select Item_DisplayName from Stores_Item_AlaisHead order by Item_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_Item.DataSource = dt1
            cbo_Filter_Item.DisplayMember = "Item_DisplayName"

            cbo_Filter_Item.Text = ""
            cbo_Filter_Ledger.Text = ""

            cbo_Filter_Ledger.SelectedIndex = -1
            cbo_Filter_Item.SelectedIndex = -1

            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        Try


            If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Store_Service_Receipt_Entry, New_Entry, Me) = False Then Exit Sub




       
            ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_Stores_Item_Receipt, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_Stores_Service_Item_Receipt, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

            inpno = InputBox("Enter New Rec.No.", "FOR NEW NO INSERTION...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Service_Receipt_No from Stores_Service_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Service_Receipt_Code = '" & Trim(RefCode) & "'", con)
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
                    MessageBox.Show("Invalid REC No", "DOES NOT INSERT NEW NO...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RecNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Service_Receipt_No from Stores_Service_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Service_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Service_Receipt_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RecNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Service_Receipt_No from Stores_Service_Receipt_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Service_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Service_Receipt_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RecNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Service_Receipt_No from Stores_Service_Receipt_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Service_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Service_Receipt_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Service_Receipt_No from Stores_Service_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Service_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Service_Receipt_No desc", con)
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
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            lbl_RecNo.Text = Common_Procedures.get_MaxCode(con, "Stores_Service_Receipt_Head", "Service_Receipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RecNo.ForeColor = Color.Red

            da = New SqlClient.SqlDataAdapter("select top 1 * from Stores_Service_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Service_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  Order by for_Orderby desc, Service_Receipt_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                If dt1.Rows(0).Item("CGST_Percentage").ToString <> "" Then txt_CGST_Perc.Text = dt1.Rows(0).Item("CGST_Percentage").ToString
                If dt1.Rows(0).Item("SGST_Percentage").ToString <> "" Then txt_SGST_Perc.Text = dt1.Rows(0).Item("SGST_Percentage").ToString
                If dt1.Rows(0).Item("IGST_Percentage").ToString <> "" Then txt_IGST_Perc.Text = dt1.Rows(0).Item("IGST_Percentage").ToString
            End If


            If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()

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
        Dim RefCode As String

        Try

            inpno = InputBox("Enter Dc.No.", "FOR FINDING...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Service_Receipt_No from Stores_Service_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Service_Receipt_Code = '" & Trim(RefCode) & "'", con)
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
                MessageBox.Show("Rec No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Dep_ID As Integer = 0
        Dim Item_ID As Integer = 0
        Dim Machine_ID As Integer = 0
        Dim Unit_ID As Integer = 0
        Dim Brand_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Nr As Integer = 0
        Dim vTotqty As Single = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim Led_ID As Integer = 0
        Dim Qty_New As Single = 0
        Dim Qty_Old_Usble As Single = 0
        Dim Qty_Old_Scrp As Single = 0
        Dim vTotAmt As Double = 0
        Dim PayAc_ID As Integer = 0
        Dim VouBil As String = ""
        Dim Trans_ID As Integer
        Dim vOrdByNo As String = ""
        Dim PurAc_ID As Integer = 0

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text)

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Entry_Stores_Service_Item_Receipt, New_Entry) = False Then Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Store_Service_Receipt_Entry, New_Entry, Me, con, "Stores_Service_Receipt_Head", "Service_Receipt_Code", NewCode, "Service_Receipt_Date", "(Service_Receipt_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Service_Receipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Service_Receipt_No desc", dtp_date.Value.Date) = False Then Exit Sub



        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled Then cbo_Ledger.Focus()
            Exit Sub
        End If


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_date.Enabled Then dtp_date.Focus()
            Exit Sub
        End If


        If Not (dtp_date.Value.Date >= Common_Procedures.Company_FromDate And dtp_date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_date.Enabled Then dtp_date.Focus()
            Exit Sub
        End If

        PurAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PaymentAc.Text)

        If PurAc_ID = 0 And Val(lbl_NetAmount.Text) <> 0 Then
            MessageBox.Show("Invalid Payment A/c name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_PaymentAc.Enabled And cbo_PaymentAc.Visible Then cbo_PaymentAc.Focus()
            Exit Sub
        End If
        For i = 0 To dgv_Details.RowCount - 1

            If Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Then

                Item_ID = Common_Procedures.itemalais_NameToIdNo(con, dgv_Details.Rows(i).Cells(3).Value)
                If Item_ID = 0 Then
                    MessageBox.Show("Invalid Item Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(3)
                    End If
                    Exit Sub
                End If

                Brand_ID = Common_Procedures.Brand_NameToIdNo(con, dgv_Details.Rows(i).Cells(4).Value)
                If Brand_ID = 0 Then
                    MessageBox.Show("Invalid Brand Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(4)
                    End If
                    Exit Sub
                End If

                Unit_ID = Common_Procedures.Unit_NameToIdNo(con, dgv_Details.Rows(i).Cells(6).Value)
                If Unit_ID = 0 Then
                    MessageBox.Show("Invalid Unit Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(6)
                    End If
                    Exit Sub
                End If

            End If

        Next

        vTotqty = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotqty = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
        End If



        If vTotqty = 0 Then
            MessageBox.Show("Invalid Quantity", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dgv_Details.Enabled And dgv_Details.Visible Then
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                End If
            End If
            Exit Sub
        End If
        PayAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PaymentAc.Text)
        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)

        If Val(lbl_NetAmount.Text) = 0 Then lbl_NetAmount.Text = 0


        tr = con.BeginTransaction

        'Try

        If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RecNo.Text = Common_Procedures.get_MaxCode(con, "Stores_Service_Receipt_Head", "Service_Receipt_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@RecDate", dtp_date.Value.Date)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Stores_Service_Receipt_Head( Service_Receipt_Code  ,                      Company_IdNo, Service_Receipt_No            , for_OrderBy                                                            , Service_Receipt_Date , Ledger_IdNo             ,  Party_DcNo                        , Received_Name                    , Issued_Name                    , New_Old                        , Particulars                         , Total_Quantity           , Amount                                                     ,    Add_Less_Amount                , Net_Amount                              ,  Payment_Acc_IdNo        ,   Remarks                      ,  Freight                               , Transport_Idno        ,                  CGST_Percentage   , CGST_Amount                          ,SGST_Percentage                     ,  SGST_Amount                        , IGST_Percentage                     ,IGST_Amount                          , Total_Net_Amount   ) " & _
                "Values                                                   ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ", @RecDate             , " & Str(Val(Led_ID)) & ", '" & Trim(txt_Party_DcNo.Text) & "', '" & Trim(cbo_Received.Text) & "', '" & Trim(cbo_Issued.Text) & "','" & Trim(cbo_New_Old.Text) & "', '" & Trim(txt_Particulars.Text) & "', " & Str(Val(vTotqty)) & ", " & Str(Val(dgv_Details_Total.Rows(0).Cells(10).Value)) & ", " & Str(Val(txt_AddLess.Text)) & ", " & Str(Val(lbl_TaxableAmount.Text)) & "," & Str(Val(PayAc_ID)) & ",'" & Trim(txt_Remarks.Text) & "'," & Val(txt_Transport_Freight.Text) & " , " & Val(Trans_ID) & " ," & Str(Val(txt_CGST_Perc.Text)) & "," & Str(Val(lbl_CGstAmount.Text)) & "," & Str(Val(txt_SGST_Perc.Text)) & "," & Str(Val(lbl_SGstAmount.Text)) & "," & Str(Val(txt_IGST_Perc.Text)) & "," & Str(Val(lbl_IGstAmount.Text)) & " , " & Str(Val(CSng(lbl_NetAmount.Text))) & ")"
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Stores_Service_receipt_Head", "Service_receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Service_receipt_Code, Company_IdNo, for_OrderBy", tr)

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Stores_Service_receipt_Details", "Service_receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Item_IdNo, Unit_IdNo, Brand_IdNo, Quantity_New, Quantity_Old_Usable, Quantity_Old_Scrap", "Sl_No", "Service_receipt_Code, For_OrderBy, Company_IdNo, Service_receipt_No, Service_receipt_Date, Ledger_Idno", tr)

                cmd.CommandText = "Update Stores_Service_Receipt_Head set Issued_Name = '" & Trim(cbo_Issued.Text) & "', Service_Receipt_Date = @RecDate ,Ledger_IdNo = " & Str(Val(Led_ID)) & ", Party_DcNo = '" & Trim(txt_Party_DcNo.Text) & "', Received_Name = '" & Trim(cbo_Received.Text) & "', New_Old = '" & Trim(cbo_New_Old.Text) & "' , Particulars = '" & Trim(txt_Particulars.Text) & "', Total_Quantity = " & Str(Val(vTotqty)) & " ,Amount = " & Str(Val(dgv_Details_Total.Rows(0).Cells(10).Value)) & " ,Add_Less_Amount = " & Str(Val(txt_AddLess.Text)) & "   , Net_Amount = " & Str(Val(lbl_TaxableAmount.Text)) & " , Payment_Acc_IdNo = " & Str(Val(PayAc_ID)) & ",Remarks = '" & Trim(txt_Remarks.Text) & "',Freight = " & Val(txt_Transport_Freight.Text) & ", Transport_Idno= " & Val(Trans_ID) & ", CGST_Percentage =" & Str(Val(txt_CGST_Perc.Text)) & ", CGST_Amount = " & Str(Val(lbl_CGstAmount.Text)) & ", SGST_Percentage =" & Str(Val(txt_SGST_Perc.Text)) & " ,  SGST_Amount =" & Str(Val(lbl_SGstAmount.Text)) & ", IGST_Percentage =" & Str(Val(txt_IGST_Perc.Text)) & ", IGST_Amount = " & Str(Val(lbl_IGstAmount.Text)) & ", Total_Net_Amount = " & Str(Val(CSng(lbl_NetAmount.Text))) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Service_Receipt_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stores_Service_Delivery_Details Set Receipt_Quantity = a.Receipt_Quantity - b.Quantity from Stores_Service_Delivery_Details a, Stores_Service_Receipt_Details b where b.Service_Receipt_Code = '" & Trim(NewCode) & "' and a.Service_Delivery_Code = b.Service_Delivery_Code and a.Service_Delivery_Details_SlNo = b.Service_Delivery_Details_SlNo"
                cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Stores_Service_receipt_Head", "Service_receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Service_receipt_Code, Company_IdNo, for_OrderBy", tr)

           
            EntID = Trim(Pk_Condition) & Trim(lbl_RecNo.Text)
            PBlNo = Trim(lbl_RecNo.Text)
            Partcls = "Issue : Dc.No. " & Trim(lbl_RecNo.Text)
            cmd.CommandText = "Delete from Stores_Service_Receipt_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Service_Receipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stores_Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            With dgv_Details
                Sno = 0
                For i = 0 To dgv_Details.RowCount - 1

                    If Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Then

                        Sno = Sno + 1

                        Item_ID = Common_Procedures.itemalais_NameToIdNo(con, dgv_Details.Rows(i).Cells(3).Value, tr)

                        Brand_ID = Common_Procedures.Brand_NameToIdNo(con, dgv_Details.Rows(i).Cells(4).Value, tr)

                        Unit_ID = Common_Procedures.Unit_NameToIdNo(con, dgv_Details.Rows(i).Cells(6).Value, tr)

                        Machine_ID = Common_Procedures.Machine_NameToIdNo(con, dgv_Details.Rows(i).Cells(7).Value, tr)

                        cmd.CommandText = "Insert into Stores_Service_Receipt_Details ( Service_Receipt_Code, Company_IdNo, Service_Receipt_No, for_OrderBy, Service_Receipt_Date, Sl_No, Item_IdNo, Brand_IdNo, Quantity, Unit_idNo, Machine_idNo, Service_Delivery_No, Service_Delivery_Code, Service_Delivery_Details_SlNo, Rate, Amount) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ", @RecDate, " & Str(Val(Sno)) & ", " & Str(Val(Item_ID)) & ", " & Str(Val(Brand_ID)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Val(Unit_ID) & ", " & Val(Machine_ID) & ", '" & Trim(.Rows(i).Cells(8).Value) & "', '" & Trim(.Rows(i).Cells(9).Value) & "', " & Str(Val(.Rows(i).Cells(10).Value)) & " ," & Str(Val(.Rows(i).Cells(11).Value)) & " ," & Str(Val(.Rows(i).Cells(12).Value)) & " )"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Update Stores_Service_Delivery_Details Set Receipt_Quantity = Receipt_Quantity + " & Str(Val(.Rows(i).Cells(5).Value)) & " where Service_Delivery_Code = '" & Trim(.Rows(i).Cells(9).Value) & "' and Service_Delivery_Details_SlNo = " & Str(Val(.Rows(i).Cells(10).Value))
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then
                            tr.Rollback()
                            MessageBox.Show("Mismatch of PO and Party details", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()
                            Exit Sub
                        End If


                        Qty_New = 0
                        Qty_Old_Usble = 0
                        Qty_Old_Scrp = 0

                    If Trim(UCase(cbo_New_Old.Text)) = "SCRAP" Then
                        Qty_Old_Scrp = Val(.Rows(i).Cells(5).Value)


                    ElseIf Trim(UCase(cbo_New_Old.Text)) = "OLD" Then

                        Qty_Old_Usble = Val(.Rows(i).Cells(5).Value)

                        Else
                        Qty_New = Val(.Rows(i).Cells(5).Value)

                        End If

                        cmd.CommandText = "Insert into Stores_Stock_Item_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, Ledger_IdNo, Entry_ID, Party_Bill_No, Particulars, Sl_No, Item_IdNo, Unit_IdNo, Brand_IdNo, Quantity_New, Quantity_Old_Usable, Quantity_Old_Scrap) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RecNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RecNo.Text))) & ", @RecDate, 0, '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Item_ID)) & ", " & Str(Val(Unit_ID)) & ", " & Str(Val(Brand_ID)) & ", " & Str(Val(Qty_New)) & ", " & Str(Val(Qty_Old_Usble)) & ", " & Str(Val(Qty_Old_Scrp)) & " )"
                        cmd.ExecuteNonQuery()

                    End If

                Next
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Stores_Service_receipt_Details", "Service_receipt_Code", Val(lbl_Company.Tag), NewCode, lbl_RecNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Item_IdNo, Unit_IdNo, Brand_IdNo, Quantity_New, Quantity_Old_Usable, Quantity_Old_Scrap", "Sl_No", "Service_receipt_Code, For_OrderBy, Company_IdNo, Service_receipt_No, Service_receipt_Date, Ledger_Idno", tr)

            End With

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            'If Val(CSng(vTotAmt)) <> 0 Then
            vLed_IdNos = Led_ID & "|" & PayAc_ID & "|24|25|26"
        vVou_Amts = Val(CSng(lbl_NetAmount.Text)) & "|" & -1 * (Val(CSng(lbl_NetAmount.Text)) - Val(lbl_CGstAmount.Text) - Val(lbl_SGstAmount.Text) - Val(lbl_IGstAmount.Text)) & "|" & -1 * Val(lbl_CGstAmount.Text) & "|" & -1 * Val(lbl_SGstAmount.Text) & "|" & -1 * Val(lbl_IGstAmount.Text)
        If Common_Procedures.Voucher_Updation(con, "GST-Ser.Rcpt", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_RecNo.Text), Convert.ToDateTime(dtp_date.Text), "Bill No : " & Trim(txt_Party_DcNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                Throw New ApplicationException(ErrMsg)
            End If
            'End If



            'Bill Posting
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), dtp_date.Text, Led_ID, Trim(txt_Party_DcNo.Text), 0, Val(CSng(lbl_NetAmount.Text)), "CR", Trim(Pk_Condition) & Trim(NewCode), tr)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If




            vLed_IdNos = Trans_ID & "|" & Val(Common_Procedures.CommonLedger.Transport_Charges_Ac)
            vVou_Amts = Val(txt_Transport_Freight.Text) & "|" & -1 * Val(txt_Transport_Freight.Text)
            If Common_Procedures.Voucher_Updation(con, "SerItemRec.Frgt", Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), Trim(lbl_RecNo.Text), Convert.ToDateTime(dtp_date.Text), Trim(txt_Remarks.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                Throw New ApplicationException(ErrMsg)
            End If



            tr.Commit()

            move_record(lbl_RecNo.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
            new_record()

        'Catch ex As Exception
        '    tr.Rollback()
        '    MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'Finally

        '    Dt1.Dispose()
        '    Da.Dispose()
        '    tr.Dispose()

        '    If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()

        'End Try

    End Sub


    Private Sub cbo_Grid_Department_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Department.KeyDown
        cbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Department, Nothing, Nothing, "Department_HEAD", "Department_name", "", "(Department_idno = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Department.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                e.Handled = True
                If .CurrentRow.Index <= 0 Then
                    txt_Particulars.Focus()

                Else
                    .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(.ColumnCount - 1)

                End If

            End If


            If (e.KeyValue = 40 And cbo_Grid_Department.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                e.Handled = True
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_date.Focus()
                    End If

                Else
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(2)

                End If
            End If

        End With
    End Sub

    Private Sub cbo_Grid_Department_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Department.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Department, Nothing, "Department_Head", "Department_name", "", "(Department_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            e.Handled = True

            With dgv_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_date.Focus()
                    End If

                Else
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(2)

                End If
            End With

        End If
    End Sub


    Private Sub cbo_Received_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Received.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Received, cbo_PaymentAc, cbo_Issued, "Stores_Service_Receipt_Head", "Received_name", "", "")
    End Sub

    Private Sub cbo_Received_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Received.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Received, cbo_Issued, "Stores_Service_Receipt_Head", "Received_Name", "", "", False)
    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim item_nm As String
        Dim Unt_nm As String
        Dim dno As String
        Dim dep_idno As Integer = 0

        If e.ColumnIndex = 2 Then

            If Trim(dgv_Details.Rows(e.RowIndex).Cells(3).Value) = "" Or Trim(dgv_DrawNo) <> Trim(dgv_Details.Rows(e.RowIndex).Cells(2).Value) Then

                dep_idno = Common_Procedures.Department_NameToIdNo(con, dgv_Details.Rows(e.RowIndex).Cells(1).Value)
                dno = dgv_Details.Rows(e.RowIndex).Cells(2).Value

                Da = New SqlClient.SqlDataAdapter("select a.Item_name, b.unit_name from Stores_item_head a left outer join unit_head b on a.unit_idno = b.unit_idno where a.department_idno = " & Str(Val(dep_idno)) & " and a.drawing_no = '" & Trim(dno) & "'", con)
                Da.Fill(Dt)

                item_nm = ""
                Unt_nm = ""
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        item_nm = Trim(Dt.Rows(0).Item("Item_name").ToString)
                        Unt_nm = Trim(Dt.Rows(0).Item("unit_name").ToString)
                    End If
                End If

                Dt.Dispose()
                Da.Dispose()

                dgv_Details.Rows(e.RowIndex).Cells(3).Value = Trim(item_nm)
                dgv_Details.Rows(e.RowIndex).Cells(6).Value = Trim(Unt_nm)

            End If

        End If

    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Dt5 As New DataTable
        'Dim rect As Rectangle
        Dim dep_idno As Integer = 0
        'Dim Condt As String

        With dgv_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            dgv_DrawNo = dgv_Details.Rows(e.RowIndex).Cells(2).Value

            'If e.ColumnIndex = 1 Then

            '    If cbo_Grid_Department.Visible = False Or Val(cbo_Grid_Department.Tag) <> e.RowIndex Then

            '        cbo_Grid_Department.Tag = -1
            '        Da = New SqlClient.SqlDataAdapter("select Department_Name from Department_Head order by Department_Name", con)
            '        Dt1 = New DataTable
            '        Da.Fill(Dt1)
            '        cbo_Grid_Department.DataSource = Dt1
            '        cbo_Grid_Department.DisplayMember = "Department_Name"

            '        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

            '        cbo_Grid_Department.Left = .Left + rect.Left
            '        cbo_Grid_Department.Top = .Top + rect.Top

            '        cbo_Grid_Department.Width = rect.Width
            '        cbo_Grid_Department.Height = rect.Height
            '        cbo_Grid_Department.Text = .CurrentCell.Value

            '        cbo_Grid_Department.Tag = Val(e.RowIndex)
            '        cbo_Grid_Department.Visible = True

            '        cbo_Grid_Department.BringToFront()
            '        cbo_Grid_Department.Focus()

            '    End If

            'Else
            '    cbo_Grid_Department.Visible = False

            'End If

            If e.ColumnIndex = 2 And vCloPic_STS = False Then
                btn_ShowPicture_Click(sender, e)
            Else
                pnl_Picture.Visible = False
            End If

            'If e.ColumnIndex = 3 Then

            '    If cbo_Grid_Item.Visible = False Or Val(cbo_Grid_Item.Tag) <> e.RowIndex Then

            '        dep_idno = Common_Procedures.Department_NameToIdNo(con, Trim(cbo_Grid_Department.Text))

            '        Condt = ""
            '        If dep_idno <> 0 And dep_idno <> 1 Then Condt = " Where (Item_idno = 0 or Department_idno = " & Str(Val(dep_idno)) & ")"

            '        cbo_Grid_Item.Tag = -1
            '        Da = New SqlClient.SqlDataAdapter("select Item_DisplayName from Stores_Item_AlaisHead " & Condt & " order by Item_DisplayName", con)
            '        Dt1 = New DataTable
            '        Da.Fill(Dt2)
            '        cbo_Grid_Item.DataSource = Dt2
            '        cbo_Grid_Item.DisplayMember = "Item_DisplayName"

            '        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

            '        cbo_Grid_Item.Left = .Left + rect.Left
            '        cbo_Grid_Item.Top = .Top + rect.Top

            '        cbo_Grid_Item.Width = rect.Width
            '        cbo_Grid_Item.Height = rect.Height
            '        cbo_Grid_Item.Text = .CurrentCell.Value

            '        cbo_Grid_Item.Tag = Val(e.RowIndex)
            '        cbo_Grid_Item.Visible = True

            '        cbo_Grid_Item.BringToFront()
            '        cbo_Grid_Item.Focus()

            '    End If

            'Else
            '    cbo_Grid_Item.Visible = False

            'End If


            'If e.ColumnIndex = 4 Then

            '    If cbo_Grid_Brand.Visible = False Or Val(cbo_Grid_Brand.Tag) <> e.RowIndex Then

            '        cbo_Grid_Brand.Tag = -1
            '        Da = New SqlClient.SqlDataAdapter("select Brand_Name from Brand_Head order by Brand_Name", con)
            '        Dt1 = New DataTable
            '        Da.Fill(Dt3)
            '        cbo_Grid_Brand.DataSource = Dt3
            '        cbo_Grid_Brand.DisplayMember = "Brand_Name"

            '        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

            '        cbo_Grid_Brand.Left = .Left + rect.Left
            '        cbo_Grid_Brand.Top = .Top + rect.Top

            '        cbo_Grid_Brand.Width = rect.Width
            '        cbo_Grid_Brand.Height = rect.Height
            '        cbo_Grid_Brand.Text = .CurrentCell.Value

            '        cbo_Grid_Brand.Tag = Val(e.RowIndex)
            '        cbo_Grid_Brand.Visible = True

            '        cbo_Grid_Brand.BringToFront()
            '        cbo_Grid_Brand.Focus()


            '    End If


            'Else
            '    cbo_Grid_Brand.Visible = False

            'End If

            'If e.ColumnIndex = 6 Then

            '    If cbo_Grid_Unit.Visible = False Or Val(cbo_Grid_Unit.Tag) <> e.RowIndex Then

            '        cbo_Grid_Unit.Tag = -1
            '        Da = New SqlClient.SqlDataAdapter("select Unit_Name from Unit_Head order by Unit_Name", con)
            '        Dt1 = New DataTable
            '        Da.Fill(Dt4)
            '        cbo_Grid_Unit.DataSource = Dt4
            '        cbo_Grid_Unit.DisplayMember = "Unit_Name"

            '        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

            '        cbo_Grid_Unit.Left = .Left + rect.Left
            '        cbo_Grid_Unit.Top = .Top + rect.Top

            '        cbo_Grid_Unit.Width = rect.Width
            '        cbo_Grid_Unit.Height = rect.Height
            '        cbo_Grid_Unit.Text = .CurrentCell.Value

            '        cbo_Grid_Unit.Tag = Val(e.RowIndex)
            '        cbo_Grid_Unit.Visible = True

            '        cbo_Grid_Unit.BringToFront()
            '        cbo_Grid_Unit.Focus()



            '    End If


            'Else
            '    cbo_Grid_Unit.Visible = False

            'End If

            'If e.ColumnIndex = 7 Then

            '    If cbo_Grid_Machine.Visible = False Or Val(cbo_Grid_Machine.Tag) <> e.RowIndex Then

            '        cbo_Grid_Machine.Tag = -1
            '        Da = New SqlClient.SqlDataAdapter("select Machine_Name from Machine_Head order by Machine_Name", con)
            '        Dt1 = New DataTable
            '        Da.Fill(Dt5)
            '        cbo_Grid_Machine.DataSource = Dt5
            '        cbo_Grid_Machine.DisplayMember = "Machine_Name"

            '        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

            '        cbo_Grid_Machine.Left = .Left + rect.Left
            '        cbo_Grid_Machine.Top = .Top + rect.Top

            '        cbo_Grid_Machine.Width = rect.Width
            '        cbo_Grid_Machine.Height = rect.Height
            '        cbo_Grid_Machine.Text = .CurrentCell.Value

            '        cbo_Grid_Machine.Tag = Val(e.RowIndex)
            '        cbo_Grid_Machine.Visible = True

            '        cbo_Grid_Machine.BringToFront()
            '        cbo_Grid_Machine.Focus()


            '    End If


            'Else
            '    cbo_Grid_Machine.Visible = False

            'End If

        End With
    End Sub


    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        On Error Resume Next

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 11 Then
                    Total_Calculation()
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        Try
            dgv_Details.EditingControl.BackColor = Color.Lime
            dgv_Details.EditingControl.ForeColor = Color.Blue
            dgtxt_Details.SelectAll()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT ENTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 11 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If
            End If
        End With

    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        With dgv_Details

            If e.KeyCode = Keys.Left Then
                If .CurrentCell.ColumnIndex <= 1 Then
                    If .CurrentCell.RowIndex = 0 Then
                        txt_Particulars.Focus()
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)
                    End If
                End If
            End If

        End With

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

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With

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
        End With
    End Sub

    Private Sub cbo_Issued_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Issued.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Issued, cbo_Received, cbo_New_Old, "Stores_Service_Receipt_Head", "Issued_Name", "", "")

    End Sub

    Private Sub cbo_Issued_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Issued.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Issued, cbo_New_Old, "Stores_Service_Receipt_Head", "Issued_Name", "", "", False)
    End Sub

    Private Sub cbo_Grid_Item_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Item.GotFocus
        vCbo_ItmNm = Trim(cbo_Grid_Item.Text)
    End Sub

    Private Sub cbo_Grid_Item_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Item.KeyDown
        Dim dep_idno As Integer = 0
        Dim Condt As String

        cbo_KeyDwnVal = e.KeyValue

        dep_idno = Common_Procedures.Department_NameToIdNo(con, Trim(cbo_Grid_Department.Text))

        Condt = ""
        If dep_idno <> 0 And dep_idno <> 1 Then Condt = "(Department_idno = " & Str(Val(dep_idno)) & ")"

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Item, Nothing, Nothing, "Stores_Item_AlaisHead", "Item_DisplayName", Condt, "(Item_idno = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Item.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Item.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_Grid_Item_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Item.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dno_nm As String
        Dim Unt_nm As String
        Dim Dep_nm As String
        Dim dep_idno As Integer = 0
        Dim Itm_idno As Integer = 0
        Dim Condt As String

        dep_idno = Common_Procedures.Department_NameToIdNo(con, Trim(cbo_Grid_Department.Text))

        Condt = ""
        If dep_idno <> 0 And dep_idno <> 1 Then Condt = "(Department_idno = " & Str(Val(dep_idno)) & ")"

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Item, Nothing, "Stores_Item_AlaisHead", "Item_DisplayName", Condt, "(Item_idno = 0)")

        If Asc(e.KeyChar) = 13 Then

            If Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(1).Value) = "" Or Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(2).Value) = "" Or Trim(dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(6).Value) = "" Or Trim(UCase(vCbo_ItmNm)) <> Trim(UCase(cbo_Grid_Item.Text)) Then

                Itm_idno = Common_Procedures.itemalais_NameToIdNo(con, Trim(cbo_Grid_Item.Text))

                da = New SqlClient.SqlDataAdapter("select a.Drawing_No, b.unit_name, c.department_name from Stores_item_head a left outer join unit_head b on a.unit_idno = b.unit_idno left outer join Department_Head c ON a.Department_IdNo = c.Department_IdNo Where a.item_IdNo = " & Str(Val(Itm_idno)), con)
                da.Fill(dt)

                Dep_nm = ""
                dno_nm = ""
                Unt_nm = ""
                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                        Dep_nm = Trim(dt.Rows(0).Item("department_name").ToString)
                        dno_nm = Trim(dt.Rows(0).Item("Drawing_No").ToString)
                        Unt_nm = Trim(dt.Rows(0).Item("unit_name").ToString)
                    End If
                End If

                dt.Dispose()
                da.Dispose()

                dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(1).Value = Trim(Dep_nm)
                dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(2).Value = Trim(dno_nm)
                dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(6).Value = Trim(Unt_nm)

            End If

        End If

        If Asc(e.KeyChar) = 13 Then
            dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells.Item(3).Value = Trim(cbo_Grid_Item.Text)
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(4)
        End If

    End Sub

    Private Sub cbo_Grid_Unit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Unit.KeyDown
        cbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Unit, Nothing, cbo_Grid_Machine, "Unit_Head", "Unit_name", "", "(Unit_idno = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Unit.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(5)

            End If

            If (e.KeyValue = 40 And cbo_Grid_Unit.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If Trim(.Rows(.CurrentRow.Index).Cells(6).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then

                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(7)

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_Item_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Item.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
        If e.Control = False And e.KeyValue = 17 And cbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Stores_Item_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Item.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Item_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Item.TextChanged
        Try
            If cbo_Grid_Item.Visible Then


                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_Item.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(3).Value = Trim(cbo_Grid_Item.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_Unit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Unit.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Unit, cbo_Grid_Machine, "Unit_Head", "Unit_name", "", "(Unit_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells.Item(6).Value = Trim(cbo_Grid_Unit.Text)
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(7)

        End If
    End Sub

    Private Sub cbo_Grid_Unit_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Unit.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
        If e.Control = False And e.KeyValue = 17 And cbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Unit_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Unit.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Unit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Unit.TextChanged
        Try
            If cbo_Grid_Unit.Visible Then


                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_Unit.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 6 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Unit.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub cbo_Grid_Machine_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Machine.KeyDown
        cbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Machine, cbo_Grid_Unit, Nothing, "Machine_Head", "Machine_name", "", "(Machine_idno = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Machine.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(6)

            End If

            If (e.KeyValue = 40 And cbo_Grid_Machine.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentRow.Index = .Rows.Count - 1 Then

                    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_date.Focus()
                    End If

                Else
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index + 1).Cells(1)

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_Machine_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Machine.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Machine, Nothing, "Machine_Head", "Machine_name", "", "(Machine_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(7).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_date.Focus()
                    End If

                Else
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index + 1).Cells(1)

                End If
            End With
        End If
    End Sub

    Private Sub cbo_Grid_Machine_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Machine.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
        If e.Control = False And e.KeyValue = 17 And cbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Stores_Machine_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Machine.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub


    Private Sub cbo_Grid_Machine_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Machine.TextChanged
        Try
            If cbo_Grid_Machine.Visible Then


                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_Machine.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 7 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Machine.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim Item_IdNo As Integer
        Dim Ledger_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Item_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Service_Receipt_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Service_Receipt_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Service_Receipt_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If
            If Trim(cbo_Filter_Ledger.Text) <> "" Then
                Ledger_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_Ledger.Text)
            End If

            If Trim(cbo_Filter_Item.Text) <> "" Then
                Item_IdNo = Common_Procedures.itemalais_NameToIdNo(con, cbo_Filter_Item.Text)
            End If


            If Val(Ledger_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(Ledger_IdNo)) & ")"
            End If

            If Val(Item_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Item_IdNo = " & Str(Val(Item_IdNo))
            End If


            da = New SqlClient.SqlDataAdapter("select a.*, b.quantity, c.item_name, d.unit_name, e.Machine_Name from Stores_Service_Receipt_Head a left outer join Stores_Service_Receipt_Details b on a.Service_Receipt_Code = b.Service_Receipt_Code left outer join Stores_item_head c on b.item_idno = c.item_idno left outer join unit_head d on b.unit_idno = d.unit_idno left outer join Machine_head e on b.Machine_idno = e.Machine_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Service_Receipt_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by Service_Receipt_Date, for_orderby, Service_Receipt_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Service_Receipt_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Service_Receipt_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Item_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Quantity").ToString), )
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Unit_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Machine_Name").ToString

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

    Private Sub cbo_Filter_Item_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Item.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Item, cbo_Filter_Ledger, btn_Filter_Show, "Stores_Item_AlaisHead", "Item_DisplayName", "", "(Item_idno = 0)")
    End Sub


    Private Sub cbo_Filter_Item_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Item.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Item, btn_Filter_Show, "Stores_Item_AlaisHead", "Item_DisplayName", "", "(Item_idno = 0)")
    End Sub

    Private Sub dtp_date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_date.KeyDown

        If e.KeyValue = 40 Then
            e.Handled = True
            SendKeys.Send("{TAB}")
        End If
        If e.KeyValue = 38 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(5)

            Else
                txt_Particulars.Focus()

            End If
        End If

    End Sub

    Private Sub dtp_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
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
    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub cbo_Department_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Department.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
        If e.Control = False And e.KeyValue = 17 And cbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Stores_Department_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Department.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Brand_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Brand.KeyDown
        cbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Brand, Nothing, Nothing, "Brand_Head", "Brandname", "", "(Brand_idno = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Brand.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)

            End If

            If (e.KeyValue = 40 And cbo_Grid_Brand.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

            End If

        End With
    End Sub

    Private Sub cbo_Grid_Brand_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Brand.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Brand, Nothing, "Brand_HEAD", "Brand_name", "", "(Brand_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells.Item(4).Value = Trim(cbo_Grid_Brand.Text)
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(5)

        End If
    End Sub

    Private Sub cbo_Grid_Brand_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Brand.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
        If e.Control = False And e.KeyValue = 17 And cbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Stores_Brand_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Brand.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Department_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Department.TextChanged
        Try
            If cbo_Grid_Department.Visible Then


                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_Department.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Department.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim Totqty As Single
        Dim Totamt As Single = 0

        Sno = 0
        Totqty = 0
        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(5).Value) <> 0 Then
                    .Rows(i).Cells(12).Value = Format(Val(Val(.Rows(i).Cells(5).Value) * Val(.Rows(i).Cells(11).Value)), "########0.00")

                    Totqty = Totqty + Val(.Rows(i).Cells(5).Value)
                    Totamt = Totamt + Val(.Rows(i).Cells(12).Value)

                End If
            Next
        End With



        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(5).Value = Val(Totqty)
            '.Rows(0).Cells(5).Value = Format(Val(Totqty), "########0")
            .Rows(0).Cells(10).Value = Format(Val(Totamt), "########0.00")
        End With
        lbl_TaxableAmount.Text = Format(Val(Totamt), "########0.00")
        ' NetAmount_Calculation()
    End Sub



    Private Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btn_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Me.Close()
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)





        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Store_Service_Receipt_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Stores_Service_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Service_Receipt_Code = '" & Trim(NewCode) & "'", con)
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
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* from Stores_Service_Receipt_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Service_Receipt_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_name, d.Unit_name, e.Machine_name, f.Brand_Name from Stores_Service_Receipt_Details a INNER JOIN Stores_item_head b ON a.Item_idno = b.Item_idno LEFT OUTER JOIN Unit_Head d ON a.Unit_idno = d.Unit_idno LEFT OUTER JOIN Machine_Head e ON a.Machine_idno = e.Machine_idno LEFT OUTER JOIN Brand_Head f ON a.Brand_idno = f.Brand_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Service_Receipt_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
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
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim ItmNm1 As String, ItmNm2 As String

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
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
            .Left = 10
            .Right = 40
            .Top = 10
            .Bottom = 30
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

        NoofItems_PerPage = 5

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(35) : ClAr(2) = 210 : ClAr(3) = 120 : ClAr(4) = 60 : ClAr(5) = 50 : ClAr(6) = 70 : ClAr(7) = 90
        ClAr(8) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7))

        TxtHgt = 19

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 35 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 35
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Brand_Name").ToString, LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Quantity").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Unit_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Machine_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

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
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim C1 As Single
        Dim W1 As Single
        Dim S1 As Single
        Dim P1 As Single

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Item_name, d.Unit_name, e.Machine_name, f.Brand_Name from Stores_Service_Receipt_Details a INNER JOIN Stores_item_head b ON a.Item_idno = b.Item_idno LEFT OUTER JOIN Unit_Head d ON a.Unit_idno = d.Unit_idno LEFT OUTER JOIN Machine_Head e ON a.Machine_idno = e.Machine_idno LEFT OUTER JOIN Brand_Head f ON a.Brand_idno = f.Brand_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Service_Receipt_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
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
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_CstNo = "GSTIN : " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If

        CurY = CurY + TxtHgt - 15
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
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "SERVICE RECEIPT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("REC DATE  : ", pFont).Width
        S1 = e.Graphics.MeasureString("FROM  :  ", pFont).Width
        P1 = e.Graphics.MeasureString("PARTY DC.NO  :  ", pFont).Width


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "FROM  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "REC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + P1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Service_Receipt_No").ToString, LMargin + C1 + P1 + 30, CurY, 0, 0, p1Font)



        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + P1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Service_Receipt_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + P1 + 30, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "PARTY DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + P1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_DcNo").ToString.ToString, LMargin + C1 + P1 + 30, CurY, 0, 0, p1Font)


        'CurY = CurY + 20
        'Common_Procedures.Print_To_PrintDocument(e, "Party Dc.No  : " & prn_HdDt.Rows(0).Item("Party_DcNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 15
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ITEM NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "BRAND", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "QTY", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MACHINE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt - 15
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
        If Val(prn_HdDt.Rows(0).Item("Add_Less_Amount").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "  Add/Less", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Add_Less_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 30, CurY, PageWidth, CurY)
        End If

        CurY = CurY + TxtHgt - 20
        If Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "CGST " & Trim(txt_CGST_Perc.Text) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "CGST 0.0 %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt - 5
        If Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "SGST " & Trim(txt_SGST_Perc.Text) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "SGST 0.0 %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt - 5
        If Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "IGST " & Trim(txt_CGST_Perc.Text) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "IGST 0.0 %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
        End If

        'CurY = CurY + TxtHgt
        CurY = CurY + 20
        Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Net_Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(6))
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub btn_EnLargePicture_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_EnLargePicture.Click

        If IsNothing(PictureBox1.Image) = False Then

            EnlargePicture.Text = "IMAGE   -   " & dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(1).Value
            EnlargePicture.PictureBox2.ClientSize = PictureBox1.Image.Size
            EnlargePicture.PictureBox2.Image = CType(PictureBox1.Image.Clone, Image)
            EnlargePicture.ShowDialog()

            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell.Selected = True
            End If

        End If

    End Sub

    Private Sub btn_ShowPicture_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_ShowPicture.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dep_IdNo As Integer

        Dep_IdNo = Common_Procedures.Department_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(1).Value)

        PictureBox1.Image = Nothing
        pnl_Picture.Visible = False

        If Val(Dep_IdNo) <> 0 Then

            Da = New SqlClient.SqlDataAdapter("select * from Department_head a where Department_idno <> 1 and Department_idno = " & Str(Val(Dep_IdNo)), con)
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                If IsDBNull(Dt1.Rows(0).Item("Department_Image")) = False Then
                    Dim imageData As Byte() = DirectCast(Dt1.Rows(0).Item("Department_Image"), Byte())
                    If Not imageData Is Nothing Then
                        Using ms As New System.IO.MemoryStream(imageData, 0, imageData.Length)
                            ms.Write(imageData, 0, imageData.Length)
                            If imageData.Length > 0 Then

                                PictureBox1.Image = Image.FromStream(ms)

                                pnl_Picture.Visible = True
                                pnl_Picture.BringToFront()

                            End If
                        End Using
                    End If
                End If

            End If

        End If

        Dt1.Dispose()
        Da.Dispose()

    End Sub

    Private Sub btn_ClosePicture_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ClosePicture.Click
        vCloPic_STS = True
        pnl_Picture.Visible = False
        If dgv_Details.Rows.Count > 0 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell.Selected = True
        End If
        vCloPic_STS = False
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, dtp_date, txt_Party_DcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14)", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, txt_Party_DcNo, "Ledger_AlaisHead", "Ledger_DisplayName", " (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14)", "(Ledger_idno = 0)")


        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to select Delivery?", "FOR DELIVERY SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

            Else
                txt_Party_DcNo.Focus()

            End If

        End If

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_New_Old_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_New_Old.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_New_Old, cbo_Issued, txt_Particulars, "", "", "", "")
    End Sub

    Private Sub cbo_New_Old_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_New_Old.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_New_Old, txt_Particulars, "", "", "", "")
    End Sub

    Private Sub cbo_Filter_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Ledger.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Ledger, dtp_Filter_ToDate, cbo_Filter_Item, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14)", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Ledger, cbo_Filter_Item, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14)", "(Ledger_idno = 0)")
    End Sub

    Private Sub txt_Particulars_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Particulars.KeyDown
        If e.KeyCode = 38 Then
            SendKeys.Send("+{TAB}")
        End If
        If e.KeyCode = 40 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(5)

            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()

                Else
                    dtp_date.Focus()
                    Exit Sub

                End If

            End If
        End If
    End Sub

    Private Sub txt_Particulars_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Particulars.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(5)

            Else
                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    save_record()

                Else
                    dtp_date.Focus()
                    Exit Sub

                End If

            End If
        End If
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim Ent_Qty As Single

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT DELIVERY...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection

            .Rows.Clear()

            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*, b.Item_name, b.Drawing_No, c.Department_name, d.Unit_name, e.Brand_Name, f.Machine_Name, g.Quantity as Ent_Rec_Quantity from Stores_Service_Delivery_Details a INNER JOIN Stores_item_head b ON a.Item_idno = b.Item_idno LEFT OUTER JOIN Department_Head c ON b.Department_idno = c.Department_idno LEFT OUTER JOIN Unit_Head d ON a.Unit_idno = d.Unit_idno LEFT OUTER JOIN Brand_Head e ON a.Brand_idno = e.Brand_idno LEFT OUTER JOIN Machine_Head f ON a.Machine_Idno = f.Machine_Idno LEFT OUTER JOIN Stores_Service_Receipt_Details g ON g.Service_Receipt_Code = '" & Trim(NewCode) & "' and a.Service_Delivery_Code = g.Service_Delivery_Code and a.Service_Delivery_Details_SlNo = g.Service_Delivery_Details_SlNo Where a.ledger_idno = " & Str(Val(LedIdNo)) & " and ( (a.Quantity - a.Receipt_Quantity ) > 0 or g.Quantity > 0 ) Order by a.For_OrderBy, a.Service_Delivery_No, a.Service_Delivery_Details_SlNo", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    Ent_Qty = 0

                    If IsDBNull(Dt1.Rows(i).Item("Ent_Rec_Quantity").ToString) = False Then Ent_Qty = Val(Dt1.Rows(i).Item("Ent_Rec_Quantity").ToString)

                    If (Val(Dt1.Rows(i).Item("Quantity").ToString) - Val(Dt1.Rows(i).Item("Receipt_Quantity").ToString) + Ent_Qty) > 0 Then

                        n = .Rows.Add()

                        SNo = SNo + 1

                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Service_Delivery_No").ToString

                        If IsDBNull(Dt1.Rows(i).Item("Department_name").ToString) = False Then
                            If Trim(Dt1.Rows(i).Item("Department_name").ToString) <> "" Then
                                .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Department_name").ToString
                            Else
                                .Rows(n).Cells(2).Value = Common_Procedures.Department_IdNoToName(con, 1)
                            End If
                        End If

                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Drawing_No").ToString
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Item_name").ToString
                        .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Brand_name").ToString
                        .Rows(n).Cells(6).Value = (Val(Dt1.Rows(i).Item("Quantity").ToString) - Val(Dt1.Rows(i).Item("Receipt_Quantity").ToString) + Ent_Qty)
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Unit_name").ToString
                        If Val(Ent_Qty) > 0 Then
                            .Rows(n).Cells(8).Value = "1"
                        Else
                            .Rows(n).Cells(8).Value = ""
                        End If
                        .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Machine_Name").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Service_Delivery_Code").ToString
                        .Rows(n).Cells(11).Value = Val(Dt1.Rows(i).Item("Service_Delivery_Details_SlNo").ToString)
                        .Rows(n).Cells(12).Value = Val(Ent_Qty)

                        If Val(Ent_Qty) > 0 Then

                            For j = 0 To .ColumnCount - 1
                                .Rows(i).Cells(j).Style.ForeColor = Color.Red
                            Next

                        End If

                    End If

                Next

            End If
            Dt1.Clear()

            If .Rows.Count = 0 Then
                n = .Rows.Add()
                .Rows(n).Cells(0).Value = "1"
            End If

        End With

        pnl_Selection.Visible = True
        pnl_Selection.BringToFront()
        pnl_Back.Enabled = False

        dgv_Selection.Focus()
        dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
        dgv_Selection.CurrentCell.Selected = True

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Grid_Selection(e.RowIndex)
    End Sub

    Private Sub Grid_Selection(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                If Val(.Rows(RwIndx).Cells(6).Value) = 0 And Trim(.Rows(RwIndx).Cells(10).Value) = "" Then Exit Sub

                .Rows(RwIndx).Cells(8).Value = (Val(.Rows(RwIndx).Cells(8).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(8).Value) = 0 Then

                    .Rows(RwIndx).Cells(8).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Blue
                    Next

                Else
                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next

                End If

            End If

        End With

    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        On Error Resume Next

        With dgv_Selection

            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
                If dgv_Selection.CurrentCell.RowIndex >= 0 Then
                    e.Handled = True
                    Grid_Selection(dgv_Selection.CurrentCell.RowIndex)
                End If
            End If

        End With

    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Dim i As Integer, n As Integer
        Dim sno As Integer
        Dim Ent_Qty As Single
        Dim Ent_Rate As Single = 0

        dgv_Details.Rows.Clear()

        NoCalc_Status = True

        sno = 0

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(8).Value) = 1 Then

                If Val(dgv_Selection.Rows(i).Cells(12).Value) <> 0 Then
                    Ent_Qty = Val(dgv_Selection.Rows(i).Cells(12).Value)

                Else
                    Ent_Qty = Val(dgv_Selection.Rows(i).Cells(6).Value)

                End If

                n = dgv_Details.Rows.Add()

                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(2).Value
                dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(4).Value
                dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(5).Value
                dgv_Details.Rows(n).Cells(5).Value = Val(Ent_Qty)
                dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(7).Value
                dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(9).Value
                dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(1).Value
                dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(10).Value
                dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(11).Value

            End If

        Next i

        NoCalc_Status = False
        Total_Calculation()

        Grid_DeSelect()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False

        txt_Party_DcNo.Focus()

    End Sub

    Private Sub dgv_Selection_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Selection.LostFocus
        On Error Resume Next
        dgv_Selection.CurrentCell.Selected = False
    End Sub

    Private Sub txt_AddLess_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_AddLess.KeyDown
        If e.KeyValue = 38 Then
            If dgv_Details.RowCount > 0 Then


                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(11)
            Else
                txt_Particulars.Focus()
            End If

        End If
        If e.KeyValue = 40 Then
            txt_CGST_Perc.Focus()
        End If
    End Sub

    Private Sub txt_AddLess_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_AddLess_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_AddLess.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub cbo_PaymentAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PaymentAc.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PaymentAc, txt_Party_DcNo, cbo_Received, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 5 Or AccountsGroup_IdNo = 6 Or AccountsGroup_IdNo = 15 Or AccountsGroup_IdNo = 16)", "")

    End Sub

    Private Sub cbo_PaymentAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PaymentAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PaymentAc, cbo_Received, "Ledger_AlaisHead", "Ledger_DisplayName", "", "", False)
    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        Try
            With dgv_Details
                If .Visible = True Then
                    If .Rows.Count > 0 Then
                        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                            dgv_Details_KeyUp(sender, e)
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT KEYUP....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub
    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, txt_IGST_Perc, txt_Transport_Freight, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")


    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, txt_Transport_Freight, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.MDI_LedType = "TRANSPORT"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Transport.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub


    Private Sub txt_Remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Remarks.KeyDown
        If e.KeyValue = 38 Then
            txt_Transport_Freight.Focus()
        End If

        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()

            Else
                dtp_date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()

            Else
                dtp_date.Focus()
            End If

        End If
    End Sub

    Private Sub txt_Transport_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Transport_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub



    Private Sub txt_CGST_Perc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_CGST_Perc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_IGST_Perc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_IGST_Perc.KeyDown
        If e.KeyValue = 40 Then
            cbo_Transport.Focus()
        End If
    End Sub

    Private Sub txt_IGST_Perc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_IGST_Perc.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Transport.Focus()
        End If
    End Sub

    Private Sub txt_IGST_Perc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_IGST_Perc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_SGST_Perc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_SGST_Perc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub NetAmount_Calculation()

        'If NoCalc_Status = True Then Exit Sub

        lbl_CGstAmount.Text = Format(Val(lbl_TaxableAmount.Text) * Val(txt_CGST_Perc.Text) / 100, "##########0.00")
        lbl_SGstAmount.Text = Format(Val(lbl_TaxableAmount.Text) * Val(txt_SGST_Perc.Text) / 100, "###########0.00")
        lbl_IGstAmount.Text = Format(Val(lbl_TaxableAmount.Text) * Val(txt_IGST_Perc.Text) / 100, "##########0.00")
        lbl_NetAmount.Text = Format(Val(lbl_TaxableAmount.Text) + Val(txt_AddLess.Text) + Val(lbl_CGstAmount.Text) + Val(lbl_SGstAmount.Text) + Val(lbl_IGstAmount.Text), "#######0.00")

        'If dgv_Details_Total.RowCount >= 0 Then
        '    For i = 0 To dgv_Details_Total.RowCount - 1
        '        lbl_TaxableAmount.Text = Format(Val(dgv_Details_Total.Rows(i).Cells(10).Value) + Val(txt_AddLess.Text), "########0.00")

        '    Next

        'End If

    End Sub

    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RecNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub cbo_New_Old_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_New_Old.SelectedIndexChanged

    End Sub
End Class