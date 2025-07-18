Public Class OE_Yarn_Sales_Delivery
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "CNDEL-"
    Private Pk_Condition2 As String = "DELCN-"
    Private NoFo_STS As Integer = 0
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private prn_Status As Integer
    Private DetIndx As Integer
    Private DetSNo As Integer
    Private prn_TotalGross_Wgt As String = ""
    Private prn_DetAr1(1000, 10) As String

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        clear()
    End Sub

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Pack_Selection.Visible = False
        pnl_Selection.Visible = False

        lbl_DcNo.Text = ""
        lbl_DcNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        dtp_DesDate.Text = ""
        cbo_PartyName.Text = ""
        cbo_CountName.Text = ""
        cbo_Count.Text = ""
        cbo_Filter_Count.Text = ""

        cbo_Agent.Text = ""
        cbo_Vechile.Text = ""
        cbo_Conetype.Text = ""
        txt_Bag.Text = ""
        txt_Wgt.Text = ""
        txt_Description.Text = ""
        txt_BaleNos.Text = ""
        txt_invoice_no.Text = ""
        txt_BaleNos.Text = ""
        txt_DelAddress1.Text = ""
        txt_DeliveryAddress.Text = ""
        txt_orderNo.Text = ""

        txt_TotalChippam.Text = ""
        txt_DesTime.Text = ""
        txt_BagNoSelection.Text = ""
        txt_Selection_NofBags.Text = ""

        txt_GoodValue.Text = ""
        cbo_Transport.Text = ""

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ConeType.Text = ""
            cbo_Filter_Count.Text = ""

            cbo_Filter_Count.SelectedIndex = -1
            cbo_Filter_ConeType.SelectedIndex = -1
            cbo_Filter_PartyName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()

        cbo_PartyName.Enabled = True
        cbo_PartyName.BackColor = Color.White

        cbo_Conetype.Enabled = True
        cbo_Conetype.BackColor = Color.White

        cbo_CountName.Enabled = True
        cbo_CountName.BackColor = Color.White

        cbo_Agent.Enabled = True
        cbo_Agent.BackColor = Color.White

        cbo_Vechile.Enabled = True
        cbo_Vechile.BackColor = Color.White

        txt_BaleNos.Enabled = True
        txt_BaleNos.BackColor = Color.White

        txt_DelAddress1.Enabled = True
        txt_DelAddress1.BackColor = Color.White

        txt_DeliveryAddress.Enabled = True
        txt_DeliveryAddress.BackColor = Color.White

        dtp_DesDate.Enabled = True
        dtp_DesDate.BackColor = Color.White

        txt_DesTime.Enabled = True
        txt_DesTime.BackColor = Color.White
        txt_TotalChippam.Enabled = True
        txt_TotalChippam.BackColor = Color.White


        cbo_Transport.Enabled = True
        cbo_Transport.BackColor = Color.White

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

        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If IsNothing(Prec_ActCtrl) = False Then
            Prec_ActCtrl.BackColor = Color.White
            Prec_ActCtrl.ForeColor = Color.Black
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
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Cotton_Delivery_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_Delivery_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_DcNo.Text = dt1.Rows(0).Item("Cotton_Delivery_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Cotton_Delivery_Date").ToString
                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_CountName.Text = Common_Procedures.Count_IdNoToName(con, Val(dt1.Rows(0).Item("Count_IdNo").ToString))
                cbo_Count.Text = Common_Procedures.Count_IdNoToName(con, Val(dt1.Rows(0).Item("Des_Count_IdNo").ToString))
                cbo_Conetype.Text = Common_Procedures.Conetype_IdNoToName(con, Val(dt1.Rows(0).Item("ConeType_Idno").ToString))

                txt_Description.Text = dt1.Rows(0).Item("Description").ToString
                cbo_Agent.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Agent_IdNo").ToString))

                txt_Wgt.Text = Format(Val(dt1.Rows(0).Item("Delivery_Weight").ToString), "#########0.00")
                cbo_Vechile.Text = dt1.Rows(0).Item("Vechile_No").ToString

                txt_invoice_no.Text = dt1.Rows(0).Item("Invoice_No").ToString

                txt_TotalChippam.Text = Format(Val(dt1.Rows(0).Item("Total_Chippam").ToString), "#########0.00")
                dtp_DesDate.Text = dt1.Rows(0).Item("Des_Date").ToString
                txt_DesTime.Text = dt1.Rows(0).Item("Des_Time_Text").ToString
                txt_orderNo.Text = dt1.Rows(0).Item("Order_No").ToString
                txt_BaleNos.Text = dt1.Rows(0).Item("Bale_Nos").ToString
                txt_DeliveryAddress.Text = dt1.Rows(0).Item("Delivery_Address").ToString
                txt_DelAddress1.Text = dt1.Rows(0).Item("Delivery_Address1").ToString
                txt_Bag.Text = dt1.Rows(0).Item("Delivery_Bags").ToString

                txt_ClthDetail_Name.Text = dt1.Rows(0).Item("Yarn_Details").ToString
                txt_GoodValue.Text = dt1.Rows(0).Item("Goods_Value").ToString

                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))

                If IsDBNull(dt1.Rows(0).Item("Cotton_Invoice_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Cotton_Invoice_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If

                da2 = New SqlClient.SqlDataAdapter("Select a.* from Cotton_Delivery_Details a  Where a.Cotton_Delivery_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
                            .Rows(n).Cells(2).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                            .Rows(n).Cells(3).Value = dt2.Rows(i).Item("Bag_Code").ToString
                            .Rows(n).Cells(4).Value = dt2.Rows(i).Item("Cotton_Packing_Code").ToString
                            .Rows(n).Cells(5).Value = dt2.Rows(i).Item("Cotton_Delivery_Details_SlNo").ToString
                            .Rows(n).Cells(6).Value = dt2.Rows(i).Item("Cotton_Invoice_Code").ToString
                            .Rows(n).Cells(7).Value = Common_Procedures.Ledger_IdNoToName(con, Val(dt2.Rows(i).Item("StockfROM_IdNo").ToString))

                        Next i

                    End If

                    If .RowCount = 0 Then .Rows.Add()

                End With

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(1).Value = Val(dt1.Rows(0).Item("Total_Bags").ToString)
                    .Rows(0).Cells(2).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "###########0.00")

                End With

            End If

            If LockSTS = True Then

                cbo_PartyName.Enabled = False
                cbo_PartyName.BackColor = Color.LightGray


                cbo_Conetype.Enabled = False
                cbo_Conetype.BackColor = Color.LightGray

                cbo_CountName.Enabled = False
                cbo_CountName.BackColor = Color.LightGray

                cbo_Agent.Enabled = False
                cbo_Agent.BackColor = Color.LightGray

                cbo_Vechile.Enabled = False
                cbo_Vechile.BackColor = Color.LightGray

                txt_BaleNos.Enabled = False
                txt_BaleNos.BackColor = Color.LightGray

                txt_DelAddress1.Enabled = False
                txt_DelAddress1.BackColor = Color.LightGray

                txt_DeliveryAddress.Enabled = False
                txt_DeliveryAddress.BackColor = Color.LightGray

                dtp_DesDate.Enabled = False
                dtp_DesDate.BackColor = Color.LightGray

                txt_DesTime.Enabled = False
                txt_DesTime.BackColor = Color.LightGray

                txt_TotalChippam.Enabled = False
                txt_TotalChippam.BackColor = Color.LightGray

                cbo_Transport.Enabled = False
                cbo_Transport.BackColor = Color.LightGray


            End If

            Grid_Cell_DeSelect()
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

        End Try

        NoCalc_Status = False



    End Sub

    Private Sub Cotton_Delivery_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Agent.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Agent.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
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



    Private Sub Cotton_Delivery_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        con.Close()
        con.Dispose()
    End Sub


    Private Sub Cotton_Delivery_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Pack_Selection.Visible = True Then
                    btn_Pack_Close_Selection_Click(sender, e)
                    Exit Sub
                ElseIf pnl_Print.Visible = True Then
                    btn_print_Close_Click(sender, e)
                    Exit Sub
                Else
                    If MessageBox.Show("Do you want to Close?...", "FOR CLOSE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        Close_Form()
                    End If
                End If
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub Cotton_Delivery_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable

        If Common_Procedures.settings.CustomerCode = "1337" Then

            txt_invoice_no.Visible = True
            lbl_invoice_no.Visible = True
        Else

            txt_invoice_no.Visible = False
            lbl_invoice_no.Visible = False

        End If

        Me.Text = ""

        con.Open()



        'Common_Procedures.get_VehicleNo_From_All_Entries(con)



        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        pnl_Pack_Selection.Visible = False
        pnl_Pack_Selection.Left = (Me.Width - pnl_Pack_Selection.Width) \ 2
        pnl_Pack_Selection.Top = (Me.Height - pnl_Pack_Selection.Height) \ 2
        pnl_Pack_Selection.BringToFront()

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Conetype.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Agent.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Vechile.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ClthDetail_Name.GotFocus, AddressOf ControlGotFocus


        AddHandler cbo_Filter_ConeType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Bag.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Wgt.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_invoice_no.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_BaleNos.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Description.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TotalChippam.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DesTime.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_DesDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_orderNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DelAddress1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DeliveryAddress.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BagNoSelection.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Selection_NofBags.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_GoodValue.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Count.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Conetype.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Vechile.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Agent.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_ConeType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Bag.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Wgt.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_invoice_no.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_BaleNos.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Description.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_DesTime.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TotalChippam.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_DesDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_orderNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Count.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DelAddress1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DeliveryAddress.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Count.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BagNoSelection.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Selection_NofBags.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_GoodValue.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Bag.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Wgt.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Description.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_invoice_no.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_BaleNos.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TotalChippam.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_DesDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DesTime.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_BaleNos.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_orderNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DeliveryAddress.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_GoodValue.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_invoice_no.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Bag.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Wgt.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_invoice_no.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Description.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_BaleNos.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TotalChippam.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_DesDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DeliveryAddress.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DesTime.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_BaleNos.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_orderNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_GoodValue.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_invoice_no.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus



        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then

            lbl_bag.Text = "Pallet"
            lbl_bag_Nos.Text = "Pallet Nos"
            lbl_PackSelection_BagNo.Text = "Pallet No"

            dgv_Details.Columns(1).HeaderText = "PALLET NO"
            dgv_packSelection.Columns(1).HeaderText = "PALLET NO"


        End If


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

    'Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
    '    Dim dgv1 As New DataGridView

    '    On Error Resume Next


    '    If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

    '        dgv1 = Nothing

    '        If ActiveControl.Name = dgv_Details.Name Then
    '            dgv1 = dgv_Details

    '        ElseIf dgv_Details.IsCurrentRowDirty = True Then
    '            dgv1 = dgv_Details

    '        Else
    '            dgv1 = dgv_Details

    '        End If

    '        With dgv1
    '            If keyData = Keys.Enter Or keyData = Keys.Down Then

    '                If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
    '                    If .CurrentCell.RowIndex = .RowCount - 1 Then
    '                        If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
    '                            save_record()
    '                        Else
    '                            dtp_Date.Focus()
    '                        End If
    '                    Else
    '                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

    '                    End If

    '                Else

    '                    If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
    '                        If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
    '                            save_record()
    '                        Else
    '                            dtp_Date.Focus()
    '                        End If
    '                    Else
    '                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

    '                    End If

    '                End If

    '                Return True

    '            ElseIf keyData = Keys.Up Then
    '                If .CurrentCell.ColumnIndex <= 1 Then
    '                    If .CurrentCell.RowIndex = 0 Then
    '                        cbo_Filter_Count.Focus()

    '                    Else
    '                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.CurrentCell.ColumnIndex - 1)

    '                    End If

    '                Else
    '                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

    '                End If

    '                Return True



    '            Else
    '                Return MyBase.ProcessCmdKey(msg, keyData)

    '            End If

    '        End With

    '    Else

    '        Return MyBase.ProcessCmdKey(msg, keyData)

    '    End If

    'End Function
    Public Sub Print_record() Implements Interface_MDIActions.print_record
        'pnl_Print.Visible = True
        'pnl_Back.Enabled = False
        'If btn_Print_Preprint.Enabled And btn_Print_Preprint.Visible Then
        '    btn_Print_Preprint.Focus()
        'End If
        printing_Delivery()
    End Sub
    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        ' Print_PDF_Status = False
        Print_record()
    End Sub
    Private Sub printing_Delivery()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize
        Dim CmpName As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.OEENTRY_DELIVERY_ENTRY, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, b.Ledger_Address1, b.Ledger_Address2, b.Ledger_Address3, b.Ledger_Address4, b.Ledger_TinNo from Cotton_Delivery_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.Cotton_Delivery_Code = '" & Trim(NewCode) & "'", con)
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

        CmpName = Common_Procedures.Company_IdNoToName(con, Val(lbl_Company.Tag))



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

                PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                    PrintDocument1.Print()
                End If



            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try

        Else
            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1

                Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 9X12", 900, 1200)
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1


                ppd.WindowState = FormWindowState.Maximized
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
                ppd.Document.DefaultPageSettings.PaperSize = pkCustomSize1

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
        Dim vSno As Integer

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        DetIndx = 0
        DetSNo = 0
        prn_PageNo = 0
        prn_DetIndx = 1
        prn_DetMxIndx = 0

        Erase prn_DetAr1

        prn_DetAr1 = New String(1000, 10) {}

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*,d.Ledger_Name as Agent_name ,it.item_hsn_code as HSNCode ,Ch.* from Cotton_Delivery_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo INNER JOIN Company_Head c ON a.Company_IdNo = c.Company_IdNo LEFT OUTER JOIN Ledger_Head D ON a.Agent_IdNo = d.Ledger_IdNo LEFT JOIN Count_Head Ch ON a.Count_IdNo = Ch.Count_Idno LEFT OUTER JOIN ITEMGROUP_HEAD IT on ch.ITEMGROUP_idno=it.ITEMGROUP_idno  where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_Delivery_Code = '" & Trim(NewCode) & "'", con)

            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name as Des_count_Name, c.Count_Name,d.Ledger_Name as Agent ,e.Cone_Type_Name, Ch.* from Cotton_Delivery_Head a LEFT OUTER JOIN Count_Head b on a.Des_Count_IdNo = b.Count_IdNo LEFT OUTER JOIN Count_Head c on a.Count_idno = c.Count_idno LEFT OUTER JOIN Ledger_Head D ON a.Agent_IdNo = d.Ledger_IdNo  LEFT OUTER JOIN CONE_TYPE_HEAD e ON a.ConeType_IdNo = e.CONE_TYPE_IDno LEFT JOIN Count_Head Ch ON a.Count_IdNo = Ch.Count_Idno where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_Delivery_Code = '" & Trim(NewCode) & "' Order by a.Cotton_Delivery_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                da2.Dispose()


                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then

                    da2 = New SqlClient.SqlDataAdapter("Select a.*,b.Gross_Weight From Cotton_Delivery_Details a INNER JOIN Cotton_Packing_Details b ON a.Bag_Code = B.Bag_Code where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_Delivery_Code = '" & Trim(NewCode) & "' Order by a.Cotton_Delivery_No", con)
                    prn_DetDt = New DataTable
                    da2.Fill(prn_DetDt)

                    vSno = 0
                    prn_TotalGross_Wgt = 0

                    For i As Integer = 0 To prn_DetDt.Rows.Count - 1


                        prn_DetMxIndx = prn_DetMxIndx + 1
                        vSno = vSno + 1

                        prn_DetAr1(prn_DetMxIndx, 1) = Val(vSno)
                        prn_DetAr1(prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(i).Item("Bag_No").ToString)
                        prn_DetAr1(prn_DetMxIndx, 3) = Format(Val(prn_DetDt.Rows(i).Item("Gross_Weight").ToString), " #######0.000")
                        prn_DetAr1(prn_DetMxIndx, 4) = Format(Val(prn_DetDt.Rows(i).Item("Weight").ToString), " #######0.000")

                        prn_TotalGross_Wgt = Format(Val(prn_TotalGross_Wgt) + Val(prn_DetDt.Rows(i).Item("Gross_Weight").ToString), "##########0.00")


                    Next



                End If


            Else
                    MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

            da1.Dispose()
            da2.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then

            Printing_Format2_1155(e)

        Else

            Printing_Format1(e)

        End If
    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim p1Font As Font
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
        Dim d1 As Single


        p1Font = New Font("Calibri", 11, FontStyle.Bold)

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
            .Left = 20
            .Right = 50
            .Top = 25
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


        d1 = e.Graphics.MeasureString("Endscount     : ", pFont).Width
        'd1 = e.Graphics.MeasureString("Endscount Name   : ", pFont).Width

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(35) : ClAr(2) = 370 '400
        ClAr(3) = PageWidth - (LMargin + ClAr(1))

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then
            TxtHgt = 18
        Else
            TxtHgt = 19
        End If

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = NoofDets + 1


                CurY = CurY + TxtHgt - 10
                Common_Procedures.Print_To_PrintDocument(e, "Count Name", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + d1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Count_Name").ToString, LMargin + d1 + 30, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "Cone Type", LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + d1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Cone_Type_Name").ToString, LMargin + ClAr(1) + ClAr(2) + d1 + 30, CurY, 0, 0, pFont)

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" Then

                    CurY = CurY + TxtHgt + 5
                    Common_Procedures.Print_To_PrintDocument(e, "Agent Name ", LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + d1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Agent").ToString, LMargin + d1 + 30, CurY, 0, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, "Description", LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + d1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Des_count_Name").ToString, LMargin + ClAr(1) + ClAr(2) + d1 + 30, CurY, 0, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 5
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Pallet", LMargin + 10, CurY, 0, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Bags", LMargin + 10, CurY, 0, 0, pFont)
                End If
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + d1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Delivery_Bags").ToString, LMargin + d1 + 30, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "Weight", LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + d1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(0).Item("Delivery_Weight").ToString), "###########0.000"), LMargin + ClAr(1) + ClAr(2) + d1 + 30, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt + 5
                Common_Procedures.Print_To_PrintDocument(e, "Vechile No", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + d1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Vechile_No").ToString, LMargin + d1 + 30, CurY, 0, 0, pFont)
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then

                    Common_Procedures.Print_To_PrintDocument(e, "Transport", LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + d1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, Val(prn_DetDt.Rows(0).Item("Transport_IdNo").ToString)), LMargin + ClAr(1) + ClAr(2) + d1 + 30, CurY, 0, 0, pFont)

                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Total Chippam", LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + d1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(0).Item("Total_Chippam").ToString), "###########0.000"), LMargin + ClAr(1) + ClAr(2) + d1 + 30, CurY, 0, 0, pFont)
                End If


                CurY = CurY + TxtHgt + 5
                Common_Procedures.Print_To_PrintDocument(e, "Des.Date", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + d1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_DetDt.Rows(0).Item("Des_Date").ToString), "dd-MM-yyyy"), LMargin + d1 + 30, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "Des Time", LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + d1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, (prn_DetDt.Rows(0).Item("Des_Time_Text").ToString), LMargin + ClAr(1) + ClAr(2) + d1 + 30, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt + 5
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Pallet Nos", LMargin + 10, CurY, 0, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Bale Nos", LMargin + 10, CurY, 0, 0, pFont)
                End If
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + d1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, (prn_DetDt.Rows(0).Item("Bale_Nos").ToString), LMargin + d1 + 30, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "HSN Code", LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + d1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, (prn_DetDt.Rows(0).Item("HSN_Code").ToString), LMargin + ClAr(1) + ClAr(2) + d1 + 30, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt + 5
                'Common_Procedures.Print_To_PrintDocument(e, "Delivery Address", LMargin + 10, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + d1 + 10, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, (prn_DetDt.Rows(0).Item("Delivery_Address").ToString), LMargin + d1 + 30, CurY, 0, 0, pFont)


                Common_Procedures.Print_To_PrintDocument(e, "Value of Goods", LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + d1 + 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, (prn_DetDt.Rows(0).Item("Goods_Value").ToString), LMargin + ClAr(1) + ClAr(2) + d1 + 40, CurY, 0, 0, pFont)

                ' CurY = CurY + TxtHgt + 5

                '                    Common_Procedures.Print_To_PrintDocument(e, (prn_DetDt.Rows(0).Item("Delivery_Address1").ToString), LMargin + d1 + 30, CurY, 0, 0, pFont)

                NoofDets = NoofDets + 1

                prn_DetIndx = prn_DetIndx + 1

                CurY = CurY + TxtHgt + 5

                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            End If

            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)



        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False


    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_GstNo As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim strHeight As Single
        Dim C1, N1, M1, W1 As Single
        Dim strwidth As Single
        PageNo = PageNo + 1

        CurY = TMargin


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_GstNo = "" : Cmp_PanNo = "" : Cmp_PanCap = ""

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
            Cmp_GstNo = "GST NO.: " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then

            Cmp_PanNo = "PAN : " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
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

        pFont = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GstNo & "    " & Cmp_PanNo, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3)


        Try

            N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
            W1 = e.Graphics.MeasureString("SET NO          :  ", pFont).Width

            M1 = ClAr(1) + ClAr(2)

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 11, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "TO :  M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "REF.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cotton_Delivery_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)

            pFont = New Font("Calibri", 11, FontStyle.Regular)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Cotton_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Cotton_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" Then
                If Trim(prn_HdDt.Rows(0).Item("Order_No").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("Order_No").ToString), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)
                End If
            End If


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("Order_No").ToString), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Delivery Add", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, (prn_DetDt.Rows(0).Item("Delivery_Address").ToString), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            pFont = New Font("Calibri", 11, FontStyle.Bold)


            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GST NO.", LMargin + N1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + N1 + 65, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + N1 + 75, CurY, 0, 0, pFont)
                strwidth = e.Graphics.MeasureString("GST NO. : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
            Else
                If Trim(prn_HdDt.Rows(0).Item("PAN_NO").ToString) <> "" Then

                    pFont = New Font("Calibri", 11, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, "PAN NO.", LMargin + N1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + N1 + 65, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("PAN_NO").ToString, LMargin + N1 + 75, CurY, 0, 0, pFont)
                End If
            End If

            'If Trim(prn_HdDt.Rows(0).Item("PAN_NO").ToString) <> "" Then

            '    pFont = New Font("Calibri", 11, FontStyle.Bold)
            '    Common_Procedures.Print_To_PrintDocument(e, "PAN NO.", LMargin + N1 + strwidth + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + N1 + strwidth + 65, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("PAN_NO").ToString, LMargin + N1 + strwidth + 75, CurY, 0, 0, pFont)
            'End If

            pFont = New Font("Calibri", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, (prn_DetDt.Rows(0).Item("Delivery_Address1").ToString), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt - 10
        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(2))

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font


        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub
    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub
    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.OEENTRY_DELIVERY_ENTRY, New_Entry, Me, con, "Cotton_Delivery_Head", "Cotton_Delivery_Code", NewCode, "Cotton_Delivery_Date", "(Cotton_Delivery_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'Da = New SqlClient.SqlDataAdapter("select * from Cotton_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Delivery_Code = '" & Trim(NewCode) & "' and  Cotton_invoice_Code <> ''", con)
        'Dt1 = New DataTable
        'Da.Fill(Dt1)

        'If Dt1.Rows.Count > 0 Then
        '    If IsDBNull(Dt1.Rows(0).Item("Cotton_invoice_Code").ToString) = False Then
        '        If Trim(Dt1.Rows(0).Item("Cotton_invoice_Code").ToString) <> "" Then
        '            MessageBox.Show("Already Delivery Prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '            Exit Sub
        '        End If
        '    End If
        'End If
        'Dt1.Clear()

        Da = New SqlClient.SqlDataAdapter("select sum(Return_Weight) from Cotton_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Delivery_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already Some Bags Returned for this Delivery", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        Da = New SqlClient.SqlDataAdapter("select count(*) from Cotton_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Delivery_Code = '" & Trim(NewCode) & "' and Cotton_Invoice_Code <> ''", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already Invoice Prepared", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans


            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            'cmd.CommandText = "update Cotton_Order_Details set Invoice_Weight = a.Invoice_Weight - b.Invoice_Weight, Invoice_bags = a.Invoice_bags - b.Invoice_Bags from Cotton_Order_Details a, Cotton_Delivery_Head b where b.Cotton_Delivery_Code = '" & Trim(NewCode) & "' and a.Cotton_Order_Code = b.Cotton_Order_Code and a.Cotton_Order_Details_Slno = b.Cotton_Order_Details_Slno"
            'cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Cotton_Packing_Details set Cotton_Invoice_Code = '',Cotton_Invoice_Increment = Cotton_Invoice_Increment - 1  Where Cotton_Invoice_Code =  '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Cotton_Delivery_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Cotton_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()


        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then



            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ConeType.Text = ""
            cbo_Filter_Count.Text = ""
            cbo_Filter_Count.SelectedIndex = -1
            cbo_Filter_ConeType.SelectedIndex = -1
            cbo_Filter_PartyName.SelectedIndex = -1
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
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Cotton_Delivery_No from Cotton_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Cotton_Delivery_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Cotton_Delivery_No from Cotton_Delivery_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Cotton_Delivery_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Cotton_Delivery_No from Cotton_Delivery_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Cotton_Delivery_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Cotton_Delivery_No from Cotton_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Cotton_Delivery_No desc", con)
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

            lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "Cotton_Delivery_Head", "Cotton_Delivery_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_DcNo.ForeColor = Color.Red

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

            Dt1.Clear()



        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Dt2.Dispose()
            Da1.Dispose()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()


        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        Try

            inpno = InputBox("Enter Inv No.", "FOR FINDING...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Cotton_Delivery_No from Cotton_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Delivery_Code = '" & Trim(RefCode) & "'", con)
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
                MessageBox.Show("Inv No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.OEENTRY_DELIVERY_ENTRY, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Inv No.", "FOR NEW INV NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Cotton_Delivery_No from Cotton_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Delivery_Code = '" & Trim(InvCode) & "'", con)
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
                    MessageBox.Show("Invalid INV No.", "DOES NOT INSERT NEW Ref...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_DcNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW BILL...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim SalesAc_ID As Integer = 0
        Dim Rck_IdNo As Integer = 0
        Dim Fp_Id As Integer = 0
        Dim Led_ID As Integer = 0
        Dim stk_ID As Integer = 0
        Dim Cnt_ID As Integer = 0
        Dim CnTy_ID As Integer = 0
        Dim Agt_Idno As Integer = 0
        Dim TxAc_ID As Integer = 0
        Dim DesCnt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim vTotBgsNo As Single, vTotWgt As Single
        Dim Cr_ID As Integer = 0
        Dim Dr_ID As Integer = 0
        Dim VouBil As String = ""
        Dim YrnClthNm As String = ""
        Dim Nr As Integer = 0
        Dim vTrans_IdNo As Integer = 0
        Dim vTotCones As Integer



        '  Dim vTotBgsNo As Single, vTotWgt As Single

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Pavu_Delivery_Entry, New_Entry) = False Then Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.OEENTRY_DELIVERY_ENTRY, New_Entry, Me, con, "Cotton_Delivery_Head", "Cotton_Delivery_Code", NewCode, "Cotton_Delivery_Date", "(Cotton_Delivery_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Cotton_Delivery_No desc", dtp_Date.Value.Date) = False Then Exit Sub

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



        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        If Val(Led_ID) = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
            Exit Sub
        End If

        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, cbo_CountName.Text)
        If Val(Cnt_ID) = 0 Then
            MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_CountName.Enabled And cbo_CountName.Visible Then cbo_CountName.Focus()
            Exit Sub
        End If

        DesCnt_ID = Common_Procedures.Count_NameToIdNo(con, cbo_Count.Text)


        Agt_Idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)

        CnTy_ID = Common_Procedures.ConeType_NameToIdNo(con, cbo_Conetype.Text)

        vTrans_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)


        'If Val(txt_TotalChippam.Text) = 0 Then
        '    MessageBox.Show("Invalid Chippam", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If txt_TotalChippam.Enabled And txt_TotalChippam.Visible Then txt_TotalChippam.Focus()
        '    Exit Sub
        'End If
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

        vTotBgsNo = 0 : vTotWgt = 0

        If dgv_Details_Total.RowCount > 0 Then
            vTotBgsNo = Val(dgv_Details_Total.Rows(0).Cells(1).Value())
            vTotWgt = Val(dgv_Details_Total.Rows(0).Cells(2).Value())

        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select count(*) from Cotton_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Delivery_Code = '" & Trim(NewCode) & "' and Cotton_Invoice_Code <> ''", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already Invoice Prepared", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        Da = New SqlClient.SqlDataAdapter("select sum(Return_Weight) from Cotton_Delivery_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Delivery_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already Some Bags Returned for this Delivery", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()






        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "Cotton_Delivery_Head", "Cotton_Delivery_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@DcDate", dtp_Date.Value.Date)
            cmd.Parameters.AddWithValue("@DesDate", dtp_DesDate.Value.Date)


            If New_Entry = True Then
                cmd.CommandText = "Insert into Cotton_Delivery_Head (   Cotton_Delivery_Code ,               Company_IdNo       ,         Cotton_Delivery_No    ,                                 for_OrderBy                           , Cotton_Delivery_Date,         Ledger_IdNo      ,          Count_IdNo      ,       ConeType_Idno       ,           Des_Count_IdNo     ,             Agent_IdNo     ,              Description             ,     Delivery_Weight       ,         Total_Bags     ,       Delivery_Bags      ,        Total_Weight       ,           Vechile_No             ,             Total_Chippam          , Des_Date ,                 Des_Time_Text    ,               Order_No           ,               Bale_Nos           ,               Delivery_Address           ,                Delivery_Address1    ,               Cotton_Order_Code   ,          Cotton_Order_details_SlNo   ,                 Yarn_Details             ,            Goods_Value     ,            Invoice_No            ,            Transport_idno         ) " &
                                      "Values                           ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "' , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ",        @DcDate      , " & Str(Val(Led_ID)) & " , " & Str(Val(Cnt_ID)) & " , " & Str(Val(CnTy_ID)) & " ,  " & Str(Val(DesCnt_ID)) & " , " & Str(Val(Agt_Idno)) & " , '" & Trim(txt_Description.Text) & "' , " & Val(txt_Wgt.Text) & " ,  " & Val(vTotBgsNo) & "," & Val(txt_Bag.Text) & " , " & Str(Val(vTotWgt)) & " , '" & Trim(cbo_Vechile.Text) & "' , " & Val(txt_TotalChippam.Text) & " , @DesDate , '" & Trim(txt_DesTime.Text) & "' , '" & Trim(txt_orderNo.Text) & "' , '" & Trim(txt_BaleNos.Text) & "' , '" & Trim(txt_DeliveryAddress.Text) & "' , '" & Trim(txt_DelAddress1.Text) & "', '" & Trim(lbl_OrderCode.Text) & "', " & Val(lbl_OrderDetailSlNo.Text) & ", '" & Trim(txt_ClthDetail_Name.Text) & "' ," & Val(txt_GoodValue.Text) & ",'" & Trim(txt_invoice_no.Text) & "' ," & Str(Val(vTrans_IdNo)) & " ) "

                cmd.ExecuteNonQuery()

                Else
                cmd.CommandText = "Update Cotton_Delivery_Head set Cotton_Delivery_Date = @DcDate, Ledger_IdNo = " & Str(Val(Led_ID)) & ", ConeType_Idno = " & Str(Val(CnTy_ID)) & ", Count_IdNo = " & Str(Val(Cnt_ID)) & ",Agent_IdNo = " & Str(Val(Agt_Idno)) & ", Des_Count_idNo = " & Val(DesCnt_ID) & ",    Delivery_Weight  =  " & Val(txt_Wgt.Text) & " , Vechile_No = '" & Trim(cbo_Vechile.Text) & "' , Total_Bags = " & Val(vTotBgsNo) & ",Total_Weight  = " & Str(Val(vTotWgt)) & ", Total_Chippam =  " & Str(Val(txt_TotalChippam.Text)) & " ,      Des_Date  = @DesDate ,   Des_Time_Text  =  '" & Trim(txt_DesTime.Text) & "' , Order_No = '" & Trim(txt_orderNo.Text) & "' , Bale_Nos = '" & Trim(txt_BaleNos.Text) & "' ,  Yarn_Details =  '" & Trim(txt_ClthDetail_Name.Text) & "', Delivery_Address =  '" & Trim(txt_DeliveryAddress.Text) & "' ,Delivery_Address1 = '" & Trim(txt_DelAddress1.Text) & "',Delivery_Bags= " & Val(txt_Bag.Text) & " , Cotton_order_Code=  '" & Trim(lbl_OrderCode.Text) & "' , Cotton_Order_details_SlNo =  " & Val(lbl_OrderDetailSlNo.Text) & " , Goods_Value =" & Val(txt_GoodValue.Text) & " ,Invoice_No = '" & Trim(txt_invoice_no.Text) & "', Transport_idno=" & Str(Val(vTrans_IdNo)) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Delivery_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                    cmd.CommandText = "Update Cotton_Packing_Details set Cotton_Invoice_Code = '',Cotton_Invoice_Increment = Cotton_Invoice_Increment - 1  Where Cotton_Invoice_Code =  '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()

                End If



                cmd.CommandText = "Delete from Cotton_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Delivery_Code = '" & Trim(NewCode) & "' and cotton_invoice_Code = ''"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(2).Value) <> 0 Then

                        Sno = Sno + 1

                        stk_ID = 0
                        stk_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, Trim(.Rows(i).Cells(7).Value), tr)

                        Nr = 0
                        cmd.CommandText = "update Cotton_Delivery_Details set Cotton_Delivery_Date = @DcDate, Sl_No = " & Str(Val(Sno)) & ", Ledger_IdNo =" & Str(Val(Led_ID)) & " ,Count_IdNo = " & Val(Cnt_ID) & " , Conetype_idNo = " & Val(CnTy_ID) & " ,  Bag_No = '" & Trim(Val(.Rows(i).Cells(1).Value)) & "', Weight = " & Val(.Rows(i).Cells(2).Value) & ", Bag_Code = '" & Trim(.Rows(i).Cells(3).Value) & "',Cotton_Packing_Code = '" & Trim(.Rows(i).Cells(4).Value) & "' , StockfROM_IdNo = " & Str(Val(stk_ID)) & " where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & "   and Cotton_Delivery_Code = '" & Trim(NewCode) & "'  and Cotton_Delivery_Details_Slno = " & Str(Val(.Rows(i).Cells(5).Value)) & ""
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then
                            cmd.CommandText = "Insert into Cotton_Delivery_Details ( Cotton_Delivery_Code ,               Company_IdNo       ,   Cotton_Delivery_No    ,                     for_OrderBy                                            ,           Cotton_Delivery_Date,      Ledger_IdNo ,  Count_IdNo          , ConeType_idNo ,     Sl_No     ,                    Bag_No            ,                Weight                     ,           Bag_Code                      , Cotton_Packing_Code                 , StockfROM_IdNo    ) " &
                                                "     Values                 (   '" & Trim(NewCode) & "'      , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ",       @DcDate            ," & Val(Led_ID) & " , " & Val(Cnt_ID) & " , " & Val(CnTy_ID) & " ,  " & Str(Val(Sno)) & ",  '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(.Rows(i).Cells(2).Value)) & ",  '" & Trim(.Rows(i).Cells(3).Value) & "', '" & Trim(.Rows(i).Cells(4).Value) & "' , " & Str(Val(stk_ID)) & " ) "
                            cmd.ExecuteNonQuery()
                        End If

                        Nr = 0

                        ' --- Remove Conetype Condition 2024-02-05

                        cmd.CommandText = "Update Cotton_Packing_Details set Cotton_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' , Cotton_Invoice_Increment = Cotton_Invoice_Increment + 1 Where Bag_Code = '" & Trim(.Rows(i).Cells(3).Value) & "' AND  Cotton_packing_Code= '" & Trim(.Rows(i).Cells(4).Value) & "' and Count_IdNo = " & Val(Cnt_ID) & " "
                        'cmd.CommandText = "Update Cotton_Packing_Details set Cotton_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' , Cotton_Invoice_Increment = Cotton_Invoice_Increment + 1 Where Bag_Code = '" & Trim(.Rows(i).Cells(3).Value) & "' AND  Cotton_packing_Code= '" & Trim(.Rows(i).Cells(4).Value) & "' and Count_IdNo = " & Val(Cnt_ID) & " and ConeType_idNo = " & Val(CnTy_ID) & ""
                        Nr = cmd.ExecuteNonQuery()
                        If Nr = 0 Then
                            Throw New ApplicationException("Mismatch of Cone Or ConeType Details")
                            Exit Sub
                        End If

                    End If

                Next

            End With

            EntID = Trim(Pk_Condition) & Trim(lbl_DcNo.Text)
            PBlNo = Trim(lbl_DcNo.Text)
            Partcls = Trim(cbo_PartyName.Text)

            Da = New SqlClient.SqlDataAdapter("select count(Bag_No) as bags ,sum(Weight) as wgt , StockfROM_IdNo from Cotton_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Delivery_Code = '" & Trim(NewCode) & "' group by StockfROM_IdNo ", con)
            Dt1 = New DataTable
            Da.SelectCommand.Transaction = tr
            Da.Fill(Dt1)
            Sno = 0

            If Dt1.Rows.Count > 0 Then
                For I = 0 To Dt1.Rows.Count - 1
                    Sno = Sno + 1
                    cmd.CommandText = "Insert into Stock_Yarn_Processing_Details (                     SoftwareType_IdNo  ,                              Reference_Code                        ,             Company_IdNo                 ,   Reference_No        ,                               For_OrderBy                         ,      Reference_Date,  Particulars ,          Party_Bill_No   ,           Entry_ID      ,     Sl_No      ,          Count_idNo      ,        ConeType_Idno            ,       Bags                                               ,         Weight                                         ,                   StockAt_IdNo   ) " &
                                                                   "   Values  (" & Str(Val(Common_Procedures.SoftwareTypes.OE_Software)) & " ,'" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ",    @DcDate   , '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "', '" & Trim(EntID) & "' ," & Str(Val(Sno)) & ", " & Str(Val(Cnt_ID)) & "," & Str(Val(CnTy_ID)) & ", " & Str(-1 * Val(Dt1.Rows(I).Item("bags").ToString)) & "  ," & Str(-1 * Val(Dt1.Rows(I).Item("wgt").ToString)) & " ," & Str(Val(Dt1.Rows(I).Item("StockfROM_IdNo").ToString)) & " )"
                    cmd.ExecuteNonQuery()
                Next I
            End If
            Dt1.Clear()

            Dim Cnt_IdNo As Integer
            Cnt_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_CountName.Text, tr)

            Da = New SqlClient.SqlDataAdapter("select sum(Noofcones) as cones  from Cotton_Packing_Details a where a.Cotton_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Count_Idno = " & Str(Val(Cnt_IdNo)) & "  ", con)
            Dt1 = New DataTable
            Da.SelectCommand.Transaction = tr
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                vTotCones = Dt1.Rows(0).Item("cones").ToString()

                cmd.CommandText = "Update Cotton_Delivery_Head set Total_Cones = " & Str(Val(vTotCones)) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Delivery_Code = '" & Trim(NewCode) & "' "
                cmd.ExecuteNonQuery()

            End If

            tr.Commit()

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(Trim(lbl_DcNo.Text))
                End If
            Else
                move_record(Trim(lbl_DcNo.Text))
            End If

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()
            Dt1.Clear()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()


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
        Dim n As Integer
        Dim Led_IdNo As Integer, Cnt_IdNo As Integer, CnTy_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            CnTy_IdNo = 0
            Cnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Cotton_Delivery_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Cotton_Delivery_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Cotton_Delivery_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If
            If Trim(cbo_Filter_ConeType.Text) <> "" Then
                CnTy_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_ConeType.Text)
            End If

            If Trim(cbo_Filter_Count.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_Count.Text)
            End If


            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_IdNo)) & " "
            End If
            If Val(CnTy_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.ConeType_IdNo = " & Str(Val(CnTy_IdNo)) & " "
            End If

            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Count_IdNo = " & Str(Val(Cnt_IdNo)) & " "
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, c.Ledger_Name , d.ConeType_Name ,e.Count_Name  from Cotton_Delivery_Head a INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN CONE_TYPE_HEAD d ON a.ConeType_IdNo = d.CONE_TYPE_IDno  LEFT OUTER JOIN Count_Head e ON a.Count_IdNo = e.Count_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Cotton_Delivery_No", con)
            'da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Ledger_Name as Delv_Name from Cotton_Delivery_Head a INNER JOIN Cotton_Delivery_Details b ON a.Cotton_Delivery_Code = b.Cotton_Delivery_Code LEFT OUTER JOIN Ledger_Head c ON a.DeliveryTo_Idno = c.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Cotton_Delivery_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    'dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Cotton_Delivery_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Cotton_Delivery_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Order_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Count_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("ConeType_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Total_Bags").ToString), "########0")
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")
                    ' dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Total_NetWeight").ToString), "########0.000")

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

    'Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
    '    dgv_Details_CellLeave(sender, e)

    'End Sub

    'Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
    '    Dim Da As New SqlClient.SqlDataAdapter
    '    Dim Dt1 As New DataTable
    '    Dim Dt2 As New DataTable
    '    Dim rect As Rectangle

    '    With dgv_Details

    '        If Val(.CurrentRow.Cells(0).Value) = 0 Then
    '            .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
    '        End If


    '    End With

    'End Sub

    'Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
    '    With dgv_Details
    '        If .CurrentCell.ColumnIndex = 2 Then
    '            If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
    '                .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
    '            Else
    '                .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
    '            End If
    '        End If

    '        'If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Then
    '        '    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
    '        '        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
    '        '    Else
    '        '        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
    '        '    End If
    '        'End If
    '    End With
    'End Sub

    'Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
    '    On Error Resume Next

    '    With dgv_Details
    '        If .Visible Then
    '            If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Then

    '                Total_Calculation()

    '            End If
    '        End If
    '    End With

    'End Sub

    'Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
    '    dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    'End Sub

    'Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
    '    dgv_Details.EditingControl.BackColor = Color.Lime
    'End Sub

    'Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
    '    On Error Resume Next
    '    With dgv_Details
    '        If .Visible Then
    '            If .CurrentCell.ColumnIndex = 2 Then

    '                If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
    '                    e.Handled = True
    '                End If

    '            End If
    '        End If
    '    End With

    'End Sub



    'Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
    '    Dim i As Integer
    '    Dim n As Integer

    '    If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

    '        With dgv_Details

    '            n = .CurrentRow.Index

    '            If .CurrentCell.RowIndex = .Rows.Count - 1 Then
    '                For i = 0 To .ColumnCount - 1
    '                    .Rows(n).Cells(i).Value = ""
    '                Next

    '            Else
    '                .Rows.RemoveAt(n)

    '            End If

    '            For i = 0 To .Rows.Count - 1
    '                .Rows(i).Cells(0).Value = i + 1
    '            Next

    '        End With

    '    End If
    'End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        'dgv_Details.CurrentCell.Selected = False
    End Sub

    'Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
    '    Dim n As Integer = 0

    '    With dgv_Details

    '        n = .RowCount
    '        .Rows(n - 1).Cells(0).Value = Val(n)
    '    End With
    'End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotBgNos As Single
        Dim TotChess As Single
        Dim TotWgt As Single


        If NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotBgNos = 0 : TotChess = 0 : TotWgt = 0
        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(2).Value) <> 0 Then
                    TotBgNos = TotBgNos + 1
                    TotWgt = TotWgt + Val(.Rows(i).Cells(2).Value)

                End If

            Next

        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(1).Value = Val(TotBgNos)
            .Rows(0).Cells(2).Value = Format(Val(TotWgt), "########0.000")

        End With
        txt_Wgt.Text = Format(Val(TotWgt), "###########0.000")
        txt_Bag.Text = Val(TotBgNos)
        'NetAmount_Calculation()

    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14))", "(Ledger_idno = 0)")
    End Sub
    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, dtp_Date, txt_orderNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14))", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, txt_orderNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14))", "(Ledger_IdNo = 0)")

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
    Private Sub cbo_Agent_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Agent.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Agent_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Agent, cbo_Conetype, txt_Bag, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Agent_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Agent.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Agent, txt_Bag, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")

    End Sub
    Private Sub cbo_Agent_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Agent_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Agent.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub cbo_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub
    Private Sub cbo_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CountName.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        ' Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_CountName, txt_orderNo, cbo_Colour, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_CountName, txt_orderNo, cbo_Conetype, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")


    End Sub

    Private Sub cbo_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CountName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_CountName, cbo_Conetype, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub

    Private Sub cbo_CountName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CountName.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_CountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If

    End Sub

    Private Sub cbo_Conetype_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Conetype.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cone_Type_head", "Cone_Type_nAME", "", "(Cone_Type_IdNo = 0)")
    End Sub
    Private Sub cbo_Conetype_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Conetype.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Conetype, cbo_CountName, Nothing, "Cone_Type_head", "Cone_Type_nAME", "", "(Cone_Type_IdNo = 0)")
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Conetype, cbo_CountName, Nothing, "ConeType_Head", "ConeType_Name", "", "(ConeType_Idno = 0)")


    End Sub

    Private Sub cbo_Conetype_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Conetype.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Conetype, Nothing, "Cone_Type_head", "Cone_Type_nAME", "", "(Cone_Type_IdNo = 0)")
        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Conetype, Nothing, "ConeType_Head", "ConeType_Name", "", "(ConeType_Idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to select Pack  :", "FOR PACKING SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Pack_Selection_Click(sender, e)
            Else
                cbo_Agent.Focus()
            End If

        End If
    End Sub

    Private Sub cbo_Conetyper_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Conetype.KeyUp

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





    Private Sub btn_save_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub


    Private Sub cbo_Vechile_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Vechile.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cotton_Delivery_Head", "Vechile_No", "", "(Vechile_No <> '')")
    End Sub
    Private Sub cbo_Vechile_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Vechile.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Vechile, txt_Wgt, txt_TotalChippam, "Cotton_Delivery_Head", "Vechile_No", "", "(Vechile_No <> '')")

    End Sub

    Private Sub cbo_Vechile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Vechile.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Vechile, txt_TotalChippam, "Cotton_Delivery_Head", "Vechile_No", "", "(Vechile_No <> '')", False)

    End Sub






    Private Sub txt_Comm_Amt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub txt_InvWgt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Wgt.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub


    Private Sub cbo_Filter_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Count.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub
    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Count.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Count, cbo_Filter_PartyName, cbo_Filter_ConeType, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")


    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Count.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Count, cbo_Filter_ConeType, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub
    Private Sub cbo_Filter_Agent_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ConeType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cone_Type_head", "Cone_Type_nAME", "", "(Cone_Type_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Agent_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ConeType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ConeType, cbo_Filter_Count, btn_Filter_Show, "Cone_Type_head", "Cone_Type_nAME", "", "(Cone_Type_IdNo = 0)")
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ConeType, cbo_Filter_Count, btn_Filter_Show, "ConeType_Head", "ConeType_Name", "", "(ConeType_Idno = 0)")

    End Sub

    Private Sub cbo_Filter_Agent_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ConeType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ConeType, btn_Filter_Show, "Cone_Type_head", "Cone_Type_nAME", "", "(Cone_Type_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14))", "(Ledger_idno = 0)")
    End Sub
    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_Count, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14))", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_Count, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14))", "(Ledger_IdNo = 0)")

    End Sub




    Private Sub txt_DelAddress1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DelAddress1.KeyDown
        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
        If e.KeyValue = 38 Then
            txt_DeliveryAddress.Focus()
        End If
    End Sub

    Private Sub txt_DelAddress1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DelAddress1.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub btn_Pack_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Pack_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim Cnt_IdNo As Integer
        Dim CnTy_IdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim Ent_Pcs As Single = 0
        Dim Ent_Mtrs As Single = 0, Ent_ShtMtrs As Single = 0
        Dim vTot_Cones As Single = 0

        Cnt_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_CountName.Text)

        If Cnt_IdNo = 0 Then
            MessageBox.Show("Invalid Count Name", "DOES NOT SELECT PACKING SELECTION...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_CountName.Enabled And cbo_CountName.Visible Then cbo_CountName.Focus()
            Exit Sub
        End If

        CnTy_IdNo = Common_Procedures.ConeType_NameToIdNo(con, cbo_Conetype.Text)

        If CnTy_IdNo = 0 Then
            MessageBox.Show("Invalid ConeType Name", "DOES NOT SELECT PACKING SELECTION...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Conetype.Enabled And cbo_Conetype.Visible Then cbo_Conetype.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
            CompIDCondt = ""
        End If


        With dgv_packSelection

            .Rows.Clear()
            SNo = 0

            ' ---  Remove  Conetype Condition 2024-02-05

            Da = New SqlClient.SqlDataAdapter("select a.*,b.* from Cotton_Packing_Details A LEFT OUTER JOIN Cotton_Delivery_Details b ON a.Cotton_Packing_Code = b.Cotton_Packing_Code and a.Bag_Code = b.Bag_Code where a.Cotton_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Count_Idno = " & Str(Val(Cnt_IdNo)) & " order by  cast(b.Bag_no as numeric), b.Bag_no, a.Cotton_Packing_Date, a.for_orderby , a.Cotton_Packing_No ", con)

            ' Da = New SqlClient.SqlDataAdapter("select a.*,b.* from Cotton_Packing_Details A LEFT OUTER JOIN Cotton_Delivery_Details b ON a.Cotton_Packing_Code = b.Cotton_Packing_Code and a.Bag_Code = b.Bag_Code where a.Cotton_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Count_Idno = " & Str(Val(Cnt_IdNo)) & " and a.Conetype_Idno = " & Str(Val(CnTy_IdNo)) & " order by  cast(b.Bag_no as numeric), b.Bag_no, a.Cotton_Packing_Date, a.for_orderby , a.Cotton_Packing_No ", con)

            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Bag_No").ToString
                    .Rows(n).Cells(2).Value = Format(Val(Dt1.Rows(i).Item("Net_Weight").ToString), "#########0.000")
                    .Rows(n).Cells(3).Value = "1"
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Bag_Code").ToString
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Cotton_Packing_Code").ToString
                    .Rows(n).Cells(6).Value = Common_Procedures.Ledger_IdNoToName(con, Val(Dt1.Rows(i).Item("StockAt_IdNo").ToString))



                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            ' ---  Remove  Conetype Condition 2024-02-05

            Da = New SqlClient.SqlDataAdapter("select a.* from Cotton_Packing_Details a   where a.Cotton_Invoice_Code  = '' and a.Count_Idno = " & Str(Val(Cnt_IdNo)) & "  order by cast(a.Bag_no as varchar), a.Bag_no, a.Cotton_Packing_Date, a.for_orderby ,  a.Cotton_Packing_No ", con)

            'Da = New SqlClient.SqlDataAdapter("select a.* from Cotton_Packing_Details a   where a.Cotton_Invoice_Code  = '' and a.Count_Idno = " & Str(Val(Cnt_IdNo)) & " and a.Conetype_Idno = " & Str(Val(CnTy_IdNo)) & " order by cast(a.Bag_no as varchar), a.Bag_no, a.Cotton_Packing_Date, a.for_orderby ,  a.Cotton_Packing_No ", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    '.Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Bag_No").ToString
                    .Rows(n).Cells(2).Value = Format(Val(Dt1.Rows(i).Item("Net_Weight").ToString), "#########0.000")
                    .Rows(n).Cells(3).Value = ""
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Bag_Code").ToString
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Cotton_packing_Code").ToString
                    .Rows(n).Cells(6).Value = Common_Procedures.Ledger_IdNoToName(con, Val(Dt1.Rows(i).Item("StockAt_IdNo").ToString))

                Next

            End If
            Dt1.Clear()

        End With

        pnl_Pack_Selection.Visible = True
        pnl_Back.Enabled = False
        'If txt_BagNoSelection.Enabled And txt_BagNoSelection.Visible Then
        txt_BagNoSelection.Focus()
        'Else
        '    dgv_packSelection.Focus()
        'End If
        'dgv_packSelection.Focus()

    End Sub

    Private Sub dgv_Pack_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_packSelection.CellClick
        Select_PackPiece(e.RowIndex)
        'Select_Bag(e.RowIndex)
    End Sub

    Private Sub Select_PackPiece(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_packSelection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(3).Value = (Val(.Rows(RwIndx).Cells(3).Value) + 1) Mod 2
                If Val(.Rows(RwIndx).Cells(3).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next


                Else
                    .Rows(RwIndx).Cells(3).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next

                End If


            End If

        End With

    End Sub

    Private Sub dgv_PackSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_packSelection.KeyDown
        Dim n As Integer

        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_packSelection.CurrentCell.RowIndex >= 0 Then

                n = dgv_packSelection.CurrentCell.RowIndex

                Select_PackPiece(n)

                e.Handled = True

            End If
        End If

    End Sub

    Private Sub btn_Pack_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Pack_Close_Selection.Click
        Close_Pack_Selection()
    End Sub

    Private Sub Close_Pack_Selection()
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0

        Dim BagNo As String
        Dim vFor_OrdBy_BagNo As String = ""
        Dim FsNo As Single, LsNo As Single
        Dim FsBagNo As String, LsBagNo As String

        Cmd.Connection = con

        With dgv_Details


            Cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            Cmd.ExecuteNonQuery()

            dgv_Details.Rows.Clear()

            For i = 0 To dgv_packSelection.RowCount - 1

                If Val(dgv_packSelection.Rows(i).Cells(3).Value) = 1 Then

                    n = .Rows.Add()

                    sno = sno + 1
                    .Rows(n).Cells(0).Value = Val(sno)
                    .Rows(n).Cells(1).Value = dgv_packSelection.Rows(i).Cells(1).Value
                    .Rows(n).Cells(2).Value = dgv_packSelection.Rows(i).Cells(2).Value
                    .Rows(n).Cells(3).Value = dgv_packSelection.Rows(i).Cells(4).Value
                    .Rows(n).Cells(4).Value = dgv_packSelection.Rows(i).Cells(5).Value
                    .Rows(n).Cells(7).Value = dgv_packSelection.Rows(i).Cells(6).Value

                    vFor_OrdBy_BagNo = Str(Val(dgv_packSelection.Rows(i).Cells(1).Value))

                    Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Meters1) values ('" & Trim(dgv_packSelection.Rows(i).Cells(1).Value) & "', " & Str(Val(vFor_OrdBy_BagNo)) & " ) "
                    Cmd.ExecuteNonQuery()

                End If

                ' Total_Calculation()
            Next

            BagNo = ""
            FsNo = 0 : LsNo = 0
            FsBagNo = "" : LsBagNo = ""

            Da1 = New SqlClient.SqlDataAdapter("Select Name1 as Bag_no, Meters1 as fororderby_bagno from " & Trim(Common_Procedures.EntryTempTable) & " where Name1 <> '' order by Meters1, Name1", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                FsNo = Val(Dt1.Rows(0).Item("fororderby_bagno").ToString)
                LsNo = Val(Dt1.Rows(0).Item("fororderby_bagno").ToString)

                FsBagNo = Trim(UCase(Dt1.Rows(0).Item("Bag_no").ToString))
                LsBagNo = Trim(UCase(Dt1.Rows(0).Item("Bag_no").ToString))


                For i = 1 To Dt1.Rows.Count - 1

                    If LsNo + 1 = Val(Dt1.Rows(i).Item("fororderby_bagno").ToString) Then
                        LsNo = Val(Dt1.Rows(i).Item("fororderby_bagno").ToString)
                        LsBagNo = Trim(UCase(Dt1.Rows(i).Item("Bag_no").ToString))

                    Else
                        If FsNo = LsNo Then
                            BagNo = BagNo & Trim(FsBagNo) & ","
                        Else
                            BagNo = BagNo & Trim(FsBagNo) & "-" & Trim(LsBagNo) & ","

                        End If

                        FsNo = Dt1.Rows(i).Item("fororderby_bagno").ToString
                        LsNo = Dt1.Rows(i).Item("fororderby_bagno").ToString

                        FsBagNo = Trim(UCase(Dt1.Rows(i).Item("Bag_no").ToString))
                        LsBagNo = Trim(UCase(Dt1.Rows(i).Item("Bag_no").ToString))

                    End If

                Next

                If FsNo = LsNo Then
                    BagNo = BagNo & Trim(FsBagNo)
                Else
                    BagNo = BagNo & Trim(FsBagNo) & "-" & Trim(LsBagNo)
                End If

            End If

            Dt1.Clear()
            txt_BaleNos.Text = BagNo

            Total_Calculation()

        End With
        pnl_Back.Enabled = True
        pnl_Pack_Selection.Visible = False

        If cbo_Agent.Visible And cbo_Agent.Enabled Then cbo_Agent.Focus()
    End Sub

    Private Sub dtp_DesDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_DesDate.LostFocus
        txt_DesTime.Text = Format(Now, "Short Time")
    End Sub

    Private Sub cbo_Grid_Count_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Count.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Count_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Count.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Count, cbo_Agent, txt_Bag, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub

    Private Sub cbo_Count_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Count.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Count, txt_Bag, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_Count_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Count.KeyUp
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
    Private Sub txt_ClthDetail_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_ClthDetail_Name.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then
            cbo_Agent.Focus()
        End If
    End Sub

    Private Sub txt_ClthDetail_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ClthDetail_Name.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Agent.Focus()
        End If
    End Sub
    Private Sub btn_Print_Invoice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Invoice.Click
        printing_Delivery()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_Print_Preprint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Preprint.Click
        prn_Status = 1
        printing_Delivery()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub txt_BagNoSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BagNoSelection.KeyDown
        If e.KeyCode = 40 Then
            e.Handled = True
            btn_BagNo_selection_Click(sender, e)
        End If
    End Sub

    Private Sub txt_BagNoSelection_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BagNoSelection.KeyPress

        If Asc(e.KeyChar) = 13 Then
            If Trim(txt_BagNoSelection.Text) <> "" Then
                btn_BagNo_selection_Click(sender, e)
            Else
                txt_Selection_NofBags.Focus()
            End If
        End If

        'If Trim(txt_BagNoSelection.Text) <> "" Then
        '    'btn_BagNo_selection(sender, e)
        'Else
        '    If dgv_Selection.Rows.Count > 0 Then
        '        dgv_Selection.Focus()
        '        dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
        '        dgv_Selection.CurrentCell.Selected = True
        '    End If
        'End If
    End Sub

    Private Sub chk_SelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_SelectAll.CheckedChanged
        Dim i As Integer = 0
        Dim J As Integer = 0

        With dgv_packSelection

            For i = 0 To .Rows.Count - 1
                .Rows(i).Cells(3).Value = ""
                For J = 0 To .ColumnCount - 1
                    .Rows(i).Cells(J).Style.ForeColor = Color.Black
                Next J
            Next i

            If chk_SelectAll.Checked = True Then
                For i = 0 To .Rows.Count - 1
                    Select_PackPiece(i)
                Next i
            End If

            If .Rows.Count > 0 Then
                .Focus()
                .CurrentCell = .Rows(0).Cells(0)
                .CurrentCell.Selected = True
            End If

        End With
    End Sub

    Private Sub btn_BagNo_selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_BagNo_selection.Click
        Dim StNo As String = ""
        Dim BgNo As String = ""
        Dim i As Integer = 0

        If Trim(txt_BagNoSelection.Text) <> "" Then

            BgNo = Trim(txt_BagNoSelection.Text)

            For i = 0 To dgv_packSelection.Rows.Count - 1
                If Trim(UCase(BgNo)) = Trim(UCase(dgv_packSelection.Rows(i).Cells(1).Value)) Then
                    Call Select_Bag(i)

                    dgv_packSelection.CurrentCell = dgv_packSelection.Rows(i).Cells(0)
                    If i >= 11 Then dgv_packSelection.FirstDisplayedScrollingRowIndex = i - 10

                    Exit For

                End If
            Next

            txt_BagNoSelection.Text = ""
            If txt_BagNoSelection.Enabled = True Then txt_BagNoSelection.Focus()

        End If
    End Sub

    Private Sub Select_Bag(ByVal RwIndx As Integer)
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer = 0
        Dim MxId As Integer = 0

        Try

            With dgv_packSelection

                Cmd.Connection = con

                If .RowCount > 0 And RwIndx >= 0 Then

                    If Val(.Rows(RwIndx).Cells(7).Value) > 0 And Val(.Rows(RwIndx).Cells(8).Value) <> Val(.Rows(RwIndx).Cells(9).Value) Then
                        MessageBox.Show("Cannot deselect" & Chr(13) & "Already this bags delivered to others")
                        Exit Sub
                    End If

                    .Rows(RwIndx).Cells(3).Value = (Val(.Rows(RwIndx).Cells(3).Value) + 1) Mod 2

                    If Val(.Rows(RwIndx).Cells(3).Value) = 1 Then
                        For i = 0 To .ColumnCount - 1
                            .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                        Next

                        MxId = Common_Procedures.get_MaxIdNo(con, "" & Trim(Common_Procedures.EntryTempSubTable) & "", "Int1", "")

                        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & " ( Int1, Name1, Name2, Name3) Values (" & Str(Val(MxId)) & ", '" & Trim(.Rows(RwIndx).Cells(8).Value) & "', '" & Trim(.Rows(RwIndx).Cells(1).Value) & "', " & Str(Val(.Rows(RwIndx).Cells(3).Value)) & " ) "
                        Cmd.ExecuteNonQuery()

                    Else

                        .Rows(RwIndx).Cells(3).Value = ""
                        For i = 0 To .ColumnCount - 1
                            .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                        Next

                        Cmd.CommandText = "Delete from " & Trim(Common_Procedures.EntryTempSubTable) & " where Name1 = '" & Trim(.Rows(RwIndx).Cells(8).Value) & "' and Name2 = '" & Trim(.Rows(RwIndx).Cells(3).Value) & "'"
                        Cmd.ExecuteNonQuery()

                    End If

                End If

            End With

        Catch ex As Exception

        End Try
    End Sub

    Private Sub txt_GoodValue_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_GoodValue.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Selection_NofBags_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Selection_NofBags.KeyDown
        If e.KeyCode = 38 Then e.Handled = True : txt_BagNoSelection.Focus()
        If e.KeyValue = 40 Then
            If dgv_packSelection.Rows.Count > 0 Then
                dgv_packSelection.Focus()
                dgv_packSelection.CurrentCell = dgv_packSelection.Rows(0).Cells(0)
                dgv_packSelection.CurrentCell.Selected = True
            End If
        End If
    End Sub

    Private Sub txt_Selection_NofBags_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Selection_NofBags.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If Trim(txt_Selection_NofBags.Text) <> "" Then
                pack_selection_noOfBags()
            Else
                If dgv_packSelection.Rows.Count > 0 Then
                    dgv_packSelection.Focus()
                    dgv_packSelection.CurrentCell = dgv_packSelection.Rows(0).Cells(0)
                    dgv_packSelection.CurrentCell.Selected = True
                End If
            End If
        End If
    End Sub

    Private Sub txt_Selection_NofBags_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Selection_NofBags.TextChanged
        pack_selection_noOfBags()
    End Sub

    Private Sub pack_selection_noOfBags()
        Dim i As Integer = 0
        Dim J As Integer = 0

        With dgv_packSelection

            For i = 0 To .Rows.Count - 1
                .Rows(i).Cells(3).Value = ""
                For J = 0 To .ColumnCount - 1
                    .Rows(i).Cells(J).Style.ForeColor = Color.Black
                Next J
            Next i

            If Val(txt_Selection_NofBags.Text) > 0 Then
                For i = 0 To Val(txt_Selection_NofBags.Text) - 1
                    Select_PackCells(i)
                Next i
            End If

        End With
    End Sub

    Private Sub Select_PackCells(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_packSelection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(3).Value = (Val(.Rows(RwIndx).Cells(3).Value) + 1) Mod 2
                If Val(.Rows(RwIndx).Cells(3).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next


                Else
                    .Rows(RwIndx).Cells(3).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next

                End If


            End If

        End With

    End Sub
    Private Sub cbo_Transport_GotFocus(sender As Object, e As EventArgs) Handles cbo_Transport.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub
    Private Sub cbo_Transport_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Transport.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, txt_BaleNos, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Transport.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, txt_DesTime, txt_BaleNos, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub


    Private Sub cbo_Transport_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Transport.KeyUp

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "TRANSPORT"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_CountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub


    Private Sub Printing_Format2_1155(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim p1Font As Font
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
        Dim CurY, CurY1 As Single
        Dim d1, W1 As Single
        Dim vSno As Integer


        p1Font = New Font("Calibri", 11, FontStyle.Bold)


        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next


        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20
            .Right = 50
            .Top = 15 ' 25
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 11, FontStyle.Regular)

        NoofItems_PerPage = 10

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


        d1 = e.Graphics.MeasureString("Endscount     : ", pFont).Width
        'd1 = e.Graphics.MeasureString("Endscount Name   : ", pFont).Width
        W1 = e.Graphics.MeasureString("SET NO          :  ", pFont).Width

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = 45 : ClAr(2) = 100 : ClAr(3) = 100 : ClAr(4) = 100 : ClAr(5) = 80 : ClAr(6) = 100 : ClAr(7) = 110
        ClAr(8) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7))

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then
            TxtHgt = 16 '18
        End If

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then


                NoofDets = 0

                Printing_Format2_PageHeader_1155(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)




                CurY = CurY - TxtHgt
                'p1Font = New Font("Calibri", 11, FontStyle.Bold)


                CurY1 = CurY


                If prn_DetMxIndx > 0 Then

                    Do While prn_DetIndx <= prn_DetMxIndx

                        If NoofDets >= NoofItems_PerPage Then

                            prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                            If prn_DetIndx < prn_DetMxIndx Then

                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_Format2_PageFooter_1155(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                                e.HasMorePages = True

                            Else

                                Printing_Format2_PageFooter_1155(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)
                                e.HasMorePages = False


                            End If

                            Return

                        End If

                        If Trim(prn_DetAr1(prn_DetIndx, 2)) <> "" Or Trim(prn_DetAr1(prn_DetIndx + NoofItems_PerPage, 2)) <> "" Then

                            CurY = CurY + TxtHgt

                            If Val(prn_DetAr1(prn_DetIndx, 2)) <> 0 Then

                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr1(prn_DetIndx, 1)), LMargin + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr1(prn_DetIndx, 2)), LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr1(prn_DetIndx, 3)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr1(prn_DetIndx, 4)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)

                            End If


                            If Val(prn_DetAr1(prn_DetIndx + NoofItems_PerPage, 2)) <> 0 Then

                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr1(prn_DetIndx + NoofItems_PerPage, 1)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr1(prn_DetIndx + NoofItems_PerPage, 2)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr1(prn_DetIndx + NoofItems_PerPage, 3)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr1(prn_DetIndx + NoofItems_PerPage, 4)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)


                            End If

                        End If


                        NoofDets = NoofDets + 1
                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format2_PageFooter_1155(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If




        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False


    End Sub

    Private Sub Printing_Format2_PageHeader_1155(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_GstNo As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim strHeight As Single
        Dim C1, N1, M1, W1 As Single
        Dim strwidth As Single
        Dim CurY1 As Single
        PageNo = PageNo + 1

        CurY = TMargin


        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "YARN DELIVERY NOTE", LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_GstNo = "" : Cmp_PanNo = "" : Cmp_PanCap = ""

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
            Cmp_GstNo = "GST NO.: " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then

            Cmp_PanNo = "PAN : " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If



        W1 = e.Graphics.MeasureString("SET NO          :  ", pFont).Width


        CurY = CurY + TxtHgt - 10

        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "REF.NO : " & prn_HdDt.Rows(0).Item("Cotton_Delivery_No").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DATE : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Cotton_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 15, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "COUNT : " & prn_HdDt.Rows(0).Item("Count_Name").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 30, CurY, 0, 0, p1Font)


        p1Font = New Font("Calibri", 16, FontStyle.Bold)

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY


        C1 = ClAr(1) + ClAr(2) + ClAr(3)


        Try

            N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
            W1 = e.Graphics.MeasureString("SET NO          :  ", pFont).Width

            M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "FROM : " & Cmp_Name, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "TO :  M/s. " & prn_HdDt.Rows(0).Item("Delivery_Address").ToString & " " & prn_HdDt.Rows(0).Item("Delivery_Address1").ToString, LMargin + M1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + 10, CurY, 0, 0, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + M1 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + strHeight - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + M1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + strHeight - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + M1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + strHeight - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + M1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + strHeight - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)


            pFont = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GstNo & "    " & Cmp_PanNo, LMargin + 10, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GST NO.", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 65, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + M1 + 75, CurY, 0, 0, pFont)
                strwidth = e.Graphics.MeasureString("GST NO. : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
            Else
                If Trim(prn_HdDt.Rows(0).Item("PAN_NO").ToString) <> "" Then

                    pFont = New Font("Calibri", 11, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, "PAN NO.", LMargin + M1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 65, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("PAN_NO").ToString, LMargin + M1 + 75, CurY, 0, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 15
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(2))

            CurY = CurY + TxtHgt - 10

            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PALLET NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GROSS WGT", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NET WGT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PALLET NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GROSS WGT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NET WGT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format2_PageFooter_1155(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font


        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        If is_LastPage = True Then

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'CurY = CurY + 5
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL PALLET : " & prn_HdDt.Rows(0).Item("Total_Bags").ToString, LMargin + 10, CurY + 5, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL GROSS WEIGHT : " & Format(Val(prn_TotalGross_Wgt), "#######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY + 5, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL WEIGHT : " & prn_HdDt.Rows(0).Item("Total_Weight").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY + 5, 1, 0, pFont)


        End If



        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY


        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "VEHICLE NO : " & prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "VALUE OF GOODS : NOT FOR SALE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 0, 0, pFont)



        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt


        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "Receiver Sign ", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 30, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)


        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub txt_DeliveryAddress_TextChanged(sender As Object, e As EventArgs) Handles txt_DeliveryAddress.TextChanged

    End Sub
End Class