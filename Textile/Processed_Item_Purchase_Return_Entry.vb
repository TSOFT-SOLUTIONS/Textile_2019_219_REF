Public Class Processed_Item_Purchase_Return
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "FPRRT-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer

    Private vPrn_PvuEdsCnt As String
    Private vPrn_PvuTotBms As Integer
    Private vPrn_PvuTotMtrs As Single
    Private vPrn_PvuSetNo As String
    Private vPrn_PvuBmNos1 As String
    Private vPrn_PvuBmNos2 As String
    Private vPrn_PvuBmNos3 As String
    Private vPrn_PvuBmNos4 As String
    Private WithEvents dgtxt_details As New DataGridViewTextBoxEditingControl

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        dtp_Date.Text = ""
        txt_RecNO.Text = ""
        cbo_Ledger.Text = ""
        cbo_PurchaseAccount.Text = ""
        cbo_TransportName.Text = ""
        txt_Freight.Text = ""
        txt_Note.Text = ""
        txt_CommBag.Text = ""
        txt_CommBag.Text = ""
        txt_Billamount.Text = ""
        txt_BillNo.Text = ""
        txt_Accesablevalue.Text = ""
        txt_VatAmount.Text = ""
        txt_VehicleNo.Text = ""
        cbo_Agent.Text = ""
        cbo_DelvAt.Text = ""
        cbo_vataccount.Text = ""

        dgv_Details.Rows.Clear()

        Grid_DeSelect()

        cbo_ItemName.Visible = False
        cbo_Colour.Visible = False

        cbo_ItemName.Tag = -1
        cbo_Colour.Tag = -1

        cbo_ItemName.Text = ""
        cbo_Colour.Text = ""

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
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
        If Me.ActiveControl.Name <> cbo_ItemName.Name Then
            cbo_ItemName.Visible = False
        End If
        If Me.ActiveControl.Name <> dgv_Details_Total.Name Then
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
        dgv_Details.CurrentCell.Selected = False
        dgv_Details_Total.CurrentCell.Selected = False
        dgv_Filter_Details.CurrentCell.Selected = False
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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try



            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name ,c.Ledger_Name as Transport_Name,d.Ledger_Name as Agent_Name , e.Ledger_Name as VatAC_Name,f.Ledger_Name as PurAc_Name ,g.Ledger_Name as Delv_Name  from Item_PurchaseReturn_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Transport_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.Agent_IdNo = d.Ledger_IdNo LEFT OUTER JOIN Ledger_Head e ON a.VatAc_IdNo = e.Ledger_IdNo LEFT OUTER JOIN Ledger_Head f ON a.PurchaseAc_IdNo = f.Ledger_IdNo LEFT OUTER JOIN Ledger_Head g ON a.DeliveryTo_Idno = g.Ledger_IdNo Where a.Item_PurchaseReturn_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_RefNo.Text = dt1.Rows(0).Item("Item_PurchaseReturn_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Item_PurchaseReturn_Date").ToString
                cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_PurchaseAccount.Text = dt1.Rows(0).Item("PurAc_Name").ToString
                cbo_DelvAt.Text = dt1.Rows(0).Item("Delv_Name").ToString
                txt_RecNO.Text = dt1.Rows(0).Item("Delivery_Receipt_No").ToString
                cbo_Agent.Text = dt1.Rows(0).Item("Agent_Name").ToString
                txt_CommBag.Text = dt1.Rows(0).Item("Agent_Commission_Bag").ToString
                txt_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
                txt_BillNo.Text = dt1.Rows(0).Item("Party_Bill_No").ToString
                txt_Billamount.Text = dt1.Rows(0).Item("Bill_Amount").ToString
                txt_Accesablevalue.Text = dt1.Rows(0).Item("Accessable_Value").ToString
                cbo_TransportName.Text = dt1.Rows(0).Item("Transport_Name").ToString
                txt_VatAmount.Text = dt1.Rows(0).Item("Vat_Amount").ToString
                cbo_vataccount.Text = dt1.Rows(0).Item("VatAc_Name").ToString
                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight_Charge").ToString), "########0.00")
                txt_Note.Text = dt1.Rows(0).Item("Note").ToString

                da2 = New SqlClient.SqlDataAdapter("select a.*,b. Processed_Item_Name ,c.Colour_Name from Item_PurchaseReturn_Details a INNER JOIN Processed_Item_Head b ON  a.Processed_Item_IdNo = b.Processed_Item_Idno  LEFT OUTER JOIN Colour_Head c ON a.Colour_IdNo = c.Colour_IdNo where a.Item_PurchaseReturn_Code = '" & Trim(NewCode) & "' Order by Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Processed_Item_Name").ToString
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Colour_Name").ToString
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Purchase_Pcs").ToString
                        dgv_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Purchase_Qty").ToString
                        dgv_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Meter_Qty").ToString
                        dgv_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Meter").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")

                        dgv_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.000")

                    Next i

                End If

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(3).Value = dt1.Rows(0).Item("Total_Pcs").ToString
                    .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_Qty").ToString)
                    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_Meter").ToString), "########0.00")
                    .Rows(0).Cells(8).Value = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "########0.000")
                End With

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

    Private Sub Processed_Item_Purchase_Return_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Agent.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "AGENT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Agent.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_TransportName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_TransportName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ItemName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "GREYITEM" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ItemName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Colour.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COLOUR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Colour.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If



            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_vataccount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_vataccount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_DelvAt.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_DelvAt.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PurchaseAccount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PurchaseAccount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Processed_Item_Purchase_Return_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

        Me.Text = ""

        con.Open()

        'da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head Where (Processed_Item_Type = 'GREY' or Processed_Item_IdNo = 0) order by Processed_Item_Name", con)
        'da.Fill(dt1)
        'cbo_ItemName.DataSource = dt1
        'cbo_ItemName.DisplayMember = "Processed_Item_Name"

        'da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
        'da.Fill(dt2)
        'cbo_Colour.DataSource = dt2
        'cbo_Colour.DisplayMember = "Colour_Name"

        'da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where b.AccountsGroup_IdNo = 12 and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        'da.Fill(dt3)
        'cbo_vataccount.DataSource = dt3
        'cbo_vataccount.DisplayMember = "Ledger_DisplayName"


        'da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where (ledger_type = 'AGENT' or Ledger_IdNo = 0) order by Ledger_DisplayName", con)
        'da.Fill(dt4)
        'cbo_Agent.DataSource = dt4
        'cbo_Agent.DisplayMember = "Ledger_DisplayName"

        'da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where (ledger_type = 'TRANSPORT' or Ledger_IdNo = 0) order by Ledger_DisplayName", con)
        'da.Fill(dt5)
        'cbo_TransportName.DataSource = dt5
        'cbo_TransportName.DisplayMember = "Ledger_DisplayName"

        'da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) order by Ledger_DisplayName", con)
        'da.Fill(dt6)
        'cbo_Ledger.DataSource = dt6
        'cbo_Ledger.DisplayMember = "Ledger_DisplayName"


        'da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where b.AccountsGroup_IdNo = 27 and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
        'da.Fill(dt7)
        'cbo_PurchaseAccount.DataSource = dt7
        'cbo_PurchaseAccount.DisplayMember = "Ledger_DisplayName"



        'da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where (ledger_type = 'GODOWN' or Ledger_IdNo = 0) order by Ledger_DisplayName", con)
        'da.Fill(dt9)
        'cbo_DelvAt.DataSource = dt9
        'cbo_DelvAt.DisplayMember = "Ledger_DisplayName"



        cbo_ItemName.Visible = False

        cbo_Colour.Visible = False


        'dtp_Date.Text = ""
        'txt_PartyDcNo.Text = ""
        'cbo_Ledger.Text = ""
        'cbo_Ledger.Tag = ""
        'cbo_EndsCount.Text = ""

        'cbo_EndsCount.Text = ""
        'cbo_Cloth.Text = ""

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Colour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Agent.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DelvAt.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PurchaseAccount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TransportName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_vataccount.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_filter_billNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Accesablevalue.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Billamount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BillNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CommBag.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RecNO.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_VatAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_VehicleNo.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_DelvAt.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Colour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ItemName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Agent.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DelvAt.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PurchaseAccount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TransportName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_vataccount.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_filter_billNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Accesablevalue.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Billamount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BillNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CommBag.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RecNO.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_VatAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_VehicleNo.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_DelvAt.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Accesablevalue.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Billamount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CommBag.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_RecNO.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_VatAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_filter_billNo.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Accesablevalue.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Billamount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BillNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CommBag.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RecNO.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_VatAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_filter_billNo.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

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

    Private Sub Processed_Item_Purchase_Return_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Processed_Item_Purchase_Return_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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
                                txt_BillNo.Focus()

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
                                txt_VehicleNo.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

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
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            lbl_Company.Tag = 0
            lbl_Company.Text = ""
            Me.Text = ""
            Common_Procedures.CompIdNo = 0

            CompCondt = ""
            If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                CompCondt = "Company_Type = 'ACCOUNT'"
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Company_Head where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0", con)
            dt1 = New DataTable
            da.Fill(dt1)

            NoofComps = 0
            If dt1.Rows.Count > 0 Then
                If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                    NoofComps = Val(dt1.Rows(0)(0).ToString)
                End If
            End If
            dt1.Clear()

            If Val(NoofComps) > 1 Then

                Dim f As New Company_Selection
                f.ShowDialog()

                If Val(Common_Procedures.CompIdNo) <> 0 Then

                    da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head where Company_IdNo = " & Str(Val(Common_Procedures.CompIdNo)), con)
                    dt1 = New DataTable
                    da.Fill(dt1)

                    If dt1.Rows.Count > 0 Then
                        If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                            lbl_Company.Text = Trim(dt1.Rows(0)(1).ToString)
                            lbl_Company.Tag = Val(dt1.Rows(0)(0).ToString)
                            Me.Text = Trim(dt1.Rows(0)(1).ToString)
                        End If
                    End If
                    dt1.Clear()
                    dt1.Dispose()
                    da.Dispose()

                    new_record()

                Else
                    Me.Close()

                End If

            Else

                Me.Close()

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
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.FP_PurchaseReturn_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.FP_PurchaseReturn_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.FP_purchase_Return_Entry, New_Entry, Me, con, "Item_PurchaseReturn_Head", "Item_PurchaseReturn_Code", NewCode, "Item_PurchaseReturn_Date", "(Item_PurchaseReturn_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub




        Qa = MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If Qa = Windows.Forms.DialogResult.No Or Qa = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)

            cmd.CommandText = "Delete from Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "delete from Item_PurchaseReturn_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_PurchaseReturn_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Item_PurchaseReturn_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_PurchaseReturn_Code = '" & Trim(NewCode) & "'"
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

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where (ledger_type = 'GODOWN' or Ledger_IdNo = 0) order by Ledger_DisplayName", con)
            da.Fill(dt2)
            cbo_Filter_DelvAt.DataSource = dt2
            cbo_Filter_DelvAt.DisplayMember = "Ledger_DisplayName"


            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_DelvAt.Text = ""
            txt_filter_billNo.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_DelvAt.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Filter.BringToFront()
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Item_PurchaseReturn_No from Item_PurchaseReturn_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_PurchaseReturn_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Item_PurchaseReturn_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Item_PurchaseReturn_No from Item_PurchaseReturn_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_PurchaseReturn_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Item_PurchaseReturn_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Item_PurchaseReturn_No from Item_PurchaseReturn_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_PurchaseReturn_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Item_PurchaseReturn_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Item_PurchaseReturn_No from Item_PurchaseReturn_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_PurchaseReturn_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Item_PurchaseReturn_No desc", con)
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

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Item_PurchaseReturn_Head", "Item_PurchaseReturn_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

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

            Da = New SqlClient.SqlDataAdapter("select Item_PurchaseReturn_No from Item_PurchaseReturn_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_PurchaseReturn_Code = '" & Trim(RecCode) & "'", con)
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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.FP_PurchaseReturn_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.FP_PurchaseReturn_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.FP_purchase_Return_Entry, New_Entry, Me) = False Then Exit Sub




    
        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Item_PurchaseReturn_No from Item_PurchaseReturn_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Item_PurchaseReturn_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Ref No", "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim Itfp_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim vTotPcs As Single, vTotMtrs As Single, vtotqty As Single
        Dim Proc_ID As Integer = 0
        Dim Lot_ID As Integer = 0
        Dim vTotWeight As Single, vTotMrQty As Single
        Dim Tr_ID As Integer = 0, Ag_Id As Integer = 0, DelT_Id As Integer = 0
        Dim itgry_id As Integer = 0, vatac_id As Integer = 0, PurAc_id As Integer = 0
        Dim VouBil As String = ""

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.FP_PurchaseReturn_Entry, New_Entry) = False Then Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.FP_purchase_Return_Entry, New_Entry, Me, con, "Item_PurchaseReturn_Head", "Item_PurchaseReturn_Code", NewCode, "Item_PurchaseReturn_Date", "(Item_PurchaseReturn_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_PurchaseReturn_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Item_PurchaseReturn_No desc", dtp_Date.Value.Date) = False Then Exit Sub





      
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

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        Tr_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_TransportName.Text)
        Ag_Id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)
        PurAc_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PurchaseAccount.Text)
        DelT_Id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DelvAt.Text)



        With dgv_Details
            For i = 0 To dgv_Details.RowCount - 1
                If Val(.Rows(i).Cells(4).Value) <> 0 Or Val(.Rows(i).Cells(6).Value) <> 0 Or Val(.Rows(i).Cells(8).Value) <> 0 Then

                    If Trim(dgv_Details.Rows(i).Cells(1).Value) = "" Then
                        MessageBox.Show("Invalid Item Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)

                        End If
                        Exit Sub
                    End If

                    If Trim(dgv_Details.Rows(i).Cells(2).Value) = "" Then
                        MessageBox.Show("Invalid COLOUR Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(2)

                        End If
                        Exit Sub

                    End If

                    If Val(dgv_Details.Rows(i).Cells(6).Value) = 0 Then
                        MessageBox.Show("Invalid Meters..", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Focus() Then
                            dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(7)

                        End If
                        Exit Sub
                    End If
                End If

            Next
        End With

        vatac_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_vataccount.Text)
        If vatac_id = 0 And Val(txt_VatAmount.Text) <> 0 Then
            MessageBox.Show("Invalid Vat A/c", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_vataccount.Enabled Then cbo_vataccount.Focus()
            Exit Sub
        End If


        Total_Calculation()

        vTotMtrs = 0 : vTotWeight = 0 : vTotPcs = 0 : vtotqty = 0

        If dgv_Details_Total.RowCount > 0 Then

            vTotPcs = Val(dgv_Details_Total.Rows(0).Cells(3).Value())
            vtotqty = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTotMrQty = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(6).Value())
            vTotWeight = Val(dgv_Details_Total.Rows(0).Cells(8).Value())
        End If




        tr = con.BeginTransaction


        Try


            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Item_PurchaseReturn_Head", "Item_PurchaseReturn_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@PurchaseDate", dtp_Date.Value.Date)

            If New_Entry = True Then
                cmd.CommandText = "Insert into Item_PurchaseReturn_Head(Item_PurchaseReturn_Code, Company_IdNo, Item_PurchaseReturn_No, for_OrderBy, Item_PurchaseReturn_Date, Ledger_IdNo, PurchaseAc_Idno,DeliveryTo_Idno,Delivery_Receipt_No, Agent_Idno,Agent_Commission_Bag,Vehicle_No,Party_Bill_No, Bill_Amount , Accessable_Value , VatAc_Idno ,Vat_Amount, Transport_IdNo, Freight_Charge, Note,Total_Pcs,Total_Qty,Total_Mtr_Qty, Total_Meter,Net_Amount) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @PurchaseDate, " & Str(Val(Led_ID)) & "," & Val(PurAc_id) & " ," & Val(DelT_Id) & ",'" & Trim(txt_RecNO.Text) & "'," & Val(Ag_Id) & "," & Val(txt_CommBag.Text) & ",'" & Trim(txt_VehicleNo.Text) & "', '" & Trim(txt_BillNo.Text) & "' , " & Val(txt_Billamount.Text) & " ," & Val(txt_Accesablevalue.Text) & "," & Val(vatac_id) & " , " & Val(txt_VatAmount.Text) & ", " & Str(Val(Tr_ID)) & ", " & Str(Val(txt_Freight.Text)) & ",  '" & Trim(txt_Note.Text) & "'," & Str(Val(vTotPcs)) & "," & Str(Val(vtotqty)) & " ," & Val(vTotMrQty) & ", " & Str(Val(vTotMtrs)) & ", " & Str(Val(vTotWeight)) & ")"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Item_PurchaseReturn_Head set Item_PurchaseReturn_Date = @PurchaseDate, Ledger_IdNo = " & Val(Led_ID) & ", PurchaseAc_Idno = " & Val(PurAc_id) & " ,DeliveryTo_Idno = " & Val(DelT_Id) & ",Delivery_Receipt_No = '" & Trim(txt_RecNO.Text) & "',Agent_Idno = " & Val(Ag_Id) & ",Agent_Commission_Bag = " & Val(txt_CommBag.Text) & ",Vehicle_No = '" & Trim(txt_VehicleNo.Text) & "',Party_Bill_No = '" & Trim(txt_BillNo.Text) & "',Bill_Amount = " & Val(txt_Billamount.Text) & " , Accessable_Value = " & Val(txt_Accesablevalue.Text) & ",VatAc_Idno = " & Val(vatac_id) & " , Vat_Amount = " & Val(txt_VatAmount.Text) & ", Transport_IdNo = " & Val(Tr_ID) & ", Freight_Charge = " & Val(txt_Freight.Text) & ", Note = '" & Trim(txt_Note.Text) & "', Total_Pcs = " & Val(vTotPcs) & ",Total_Qty = " & Val(vtotqty) & ", Total_Mtr_Qty = " & Val(vTotMrQty) & ", Total_Meter = " & Val(vTotMtrs) & ",Net_Amount = " & Val(vTotWeight) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_PurchaseReturn_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If



            cmd.CommandText = "Delete from Item_PurchaseReturn_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_PurchaseReturn_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Item_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Partcls = "Delv : Ref.No. " & Trim(lbl_RefNo.Text)
            PBlNo = Trim(lbl_RefNo.Text)
            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)



            With dgv_Details
                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(6).Value) <> 0 Or Val(.Rows(i).Cells(8).Value) <> 0 Then
                        Sno = Sno + 1
                        itgry_id = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)
                        ' Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, .Rows(i).Cells(2).Value, tr)
                        Col_ID = Common_Procedures.Colour_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Item_PurchaseReturn_Details(Item_PurchaseReturn_Code, Company_IdNo, Item_PurchaseReturn_No, for_OrderBy, Item_PurchaseReturn_Date,Sl_No,Processed_Item_Idno,Colour_Idno,Purchase_Pcs,Purchase_Qty,Meter_Qty,Meter,Rate,Amount ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @PurchaseDate," & Str(Val(Sno)) & ", " & Str(Val(itgry_id)) & ",  " & Val(Col_ID) & ", " & Val(.Rows(i).Cells(3).Value) & ",  " & Val(.Rows(i).Cells(4).Value) & "," & Val(.Rows(i).Cells(5).Value) & ", " & Val(.Rows(i).Cells(6).Value) & ",  " & Str(Val(.Rows(i).Cells(7).Value)) & " ," & Str(Val(.Rows(i).Cells(8).Value)) & ")"
                        cmd.ExecuteNonQuery()

                        'If Val(vTotMtrs) > 0 Then
                        cmd.CommandText = "Insert into Stock_Item_Processing_Details ( Reference_Code ,            Company_IdNo          ,            Reference_No    ,            For_OrderBy                                                 ,  Reference_Date     ,                   DeliveryTo_StockIdNo                     ,  ReceivedFrom_StockIdNo , Delivery_PartyIdNo              , Received_PartyIdNo           , Entry_ID             , Party_Bill_No          , Particulars          , SL_No                 ,             Item_IdNo       , Rack_IdNo             ,                      Quantity           ,                      Meters                 ) " & _
                                          " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "'      , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",    @PurchaseDate  , 0                      , " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & " , " & Str(Val(Led_ID)) & "        ,         0                    ,'" & Trim(EntID) & "' , '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "'," & Str(Val(Sno)) & "  , " & Str(Val(itgry_id)) & "  , 0                    , " & Str(Math.Abs(Val(.Rows(i).Cells(4).Value))) & ", " & Str(Math.Abs(Val(.Rows(i).Cells(6).Value))) & " ) "
                        cmd.ExecuteNonQuery()  'End If

                    End If

                Next

            End With

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""

            vLed_IdNos = Led_ID & "|" & PurAc_id & "|" & vatac_id
            vVou_Amts = -1 * Val(txt_Billamount.Text) & "|" & (Val(txt_Billamount.Text) - Val(txt_VatAmount.Text)) & "|" & Val(txt_VatAmount.Text)
            If Common_Procedures.Voucher_Updation(con, "Item.PurcRet", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_Date.Value.Date, "Bill No : " & Trim(txt_BillNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            'vLed_IdNos = Ag_Id & "|" & Val(Common_Procedures.CommonLedger.Agent_Commission_Ac)
            'vVou_Amts = -1 * Val(lbl_CommAmount.Text) & "|" & Val(lbl_CommAmount.Text)
            'If Common_Procedures.Voucher_Updation(con, "ItmPurRet.Comm", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_Date.Value.Date, "Bill No : " & Trim(txt_BillNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
            '    Throw New ApplicationException(ErrMsg)
            'End If

            'Bill Posting
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), dtp_Date.Text, Led_ID, Trim(txt_BillNo.Text), Ag_Id, Val(txt_Billamount.Text), "DR", Trim(Pk_Condition) & Trim(NewCode), tr)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If

            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()
            If New_Entry = True Then new_record()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

    End Sub

    Private Sub Total_Calculation()
        Dim vTotPcs As Single, vTotMtrs As Single, vtotweight As Single, vtotqty As Single, vtotMrqty

        Dim i As Integer
        Dim sno As Integer


        vTotPcs = 0 : vTotMtrs = 0 : vtotweight = 0 : sno = 0 : vtotMrqty = 0
        With dgv_Details
            For i = 0 To dgv_Details.Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(0).Value = sno

                If Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(6).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(8).Value) <> 0 Then
                    '.Rows(i).Cells(9).Value = Val(dgv_Details.Rows(i).Cells(7).Value) * Val(dgv_Details.Rows(i).Cells(8).Value)
                    vTotPcs = vTotPcs + Val(dgv_Details.Rows(i).Cells(3).Value)
                    vtotqty = vtotqty + Val(dgv_Details.Rows(i).Cells(4).Value)
                    vtotMrqty = vtotMrqty + Val(dgv_Details.Rows(i).Cells(5).Value)
                    vTotMtrs = vTotMtrs + Val(dgv_Details.Rows(i).Cells(6).Value)
                    vtotweight = vtotweight + Val(dgv_Details.Rows(i).Cells(8).Value)
                End If
            Next
        End With
        If dgv_Details_Total.Rows.Count <= 0 Then dgv_Details_Total.Rows.Add()
        dgv_Details_Total.Rows(0).Cells(3).Value = Val(vTotPcs)
        dgv_Details_Total.Rows(0).Cells(4).Value = Val(vtotqty)
        dgv_Details_Total.Rows(0).Cells(6).Value = Format(Val(vTotMtrs), "#########0.00")
        dgv_Details_Total.Rows(0).Cells(8).Value = Format(Val(vtotweight), "#########0.000")
    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1 )", "(Ledger_idno = 0)")

    End Sub
    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, dtp_Date, cbo_PurchaseAccount, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1)", "(Ledger_idno = 0)")


    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, cbo_PurchaseAccount, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1) ", "(Ledger_idno = 0)")


    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
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
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
            If .CurrentCell.ColumnIndex = 8 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If
            Total_Calculation()

        End With
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle

        With dgv_Details
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If



            If e.ColumnIndex = 1 Then

                If cbo_ItemName.Visible = False Or Val(cbo_ItemName.Tag) <> e.RowIndex Then

                    cbo_ItemName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Processed_Item_Name from Processed_Item_Head where Processed_Item_Type = 'GREY ' order by Processed_item_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_ItemName.DataSource = Dt1
                    cbo_ItemName.DisplayMember = "Processed_Item_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_ItemName.Left = .Left + rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_ItemName.Top = .Top + rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top

                    cbo_ItemName.Width = rect.Width  ' .CurrentCell.Size.Width
                    cbo_ItemName.Height = rect.Height  ' rect.Height
                    cbo_ItemName.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_ItemName.Tag = Val(e.RowIndex)
                    cbo_ItemName.Visible = True

                    cbo_ItemName.BringToFront()
                    cbo_ItemName.Focus()



                End If


            Else

                cbo_ItemName.Visible = False


            End If



            If e.ColumnIndex = 2 Then

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




                End If

            Else

                cbo_Colour.Visible = False


            End If



        End With
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details

            If .CurrentCell.ColumnIndex = 6 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
            If .CurrentCell.ColumnIndex = 8 Then
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
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 8 Then

                    If Val(.CurrentCell.ColumnIndex) = 4 Or Val(.CurrentCell.ColumnIndex) = 5 Then
                        .Rows(i).Cells(6).Value = Val(dgv_Details.Rows(i).Cells(4).Value) * Val(dgv_Details.Rows(i).Cells(5).Value)
                    End If
                End If
                Total_Calculation()
            End If
        End With
    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown

        'On Error Resume Next
        On Error Resume Next

        With dgv_Details

            If e.KeyCode = Keys.Up Then
                If .CurrentCell.RowIndex = 0 Then
                    .CurrentCell.Selected = False
                    e.SuppressKeyPress = True
                    e.Handled = True


                    txt_VehicleNo.Focus()
                End If
            End If

            If e.KeyCode = Keys.Down Then
                If .CurrentCell.RowIndex = .RowCount - 1 Then
                    .CurrentCell.Selected = False
                    e.SuppressKeyPress = True
                    e.Handled = True
                    txt_BillNo.Focus()
                End If



            End If



        End With
    End Sub
    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime

    End Sub
    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_details.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
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

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With

            Total_Calculation()

        End If

    End Sub


    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer

        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub
    Private Sub cbo_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Colour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")

    End Sub
    Private Sub cbo_colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Colour.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Colour, cbo_ItemName, Nothing, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Colour.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Colour, Nothing, "colour_Head", "colour_Name", "", "(Colour_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

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
                    If Val(cbo_Colour.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Colour.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Processed_Item_Head", "Processed_Item_Name", "(Verified_Status = 1)", "(Processed_Item_IdNo = 0)")

    End Sub
    Private Sub cbo_ItemNameKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ItemName, Nothing, cbo_Colour, "Processed_Item_Head", "Processed_Item_Name", "(Verified_Status = 1)", "(Processed_Item_IdNo = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_ItemName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex = 0 Then
                    txt_VehicleNo.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(.ColumnCount - 1)
                End If
            End If

            If (e.KeyValue = 40 And cbo_ItemName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    txt_BillNo.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If
            End If

        End With
    End Sub

    Private Sub cbo_ItemNameKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ItemName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ItemName, cbo_Colour, "Processed_Item_Head", "Processed_Item_Name", "(Verified_Status = 1)", "(Processed_Item_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_Details
                If (.CurrentCell.RowIndex = .Rows.Count - 1) And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    txt_BillNo.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If
            End With

        End If

    End Sub

    Private Sub cbo_ItemNameKeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ItemName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Grey_Item_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ItemName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_ItemNameTextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ItemName.TextChanged
        Try
            If cbo_ItemName.Visible Then
                With dgv_Details
                    If Val(cbo_ItemName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_ItemName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_vataccount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_vataccount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_idno = 0 or AccountsGroup_IdNo = 12 and Verified_Status = 1) ", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_vataccount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_vataccount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_vataccount, txt_Accesablevalue, txt_VatAmount, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_idno = 0 or AccountsGroup_IdNo = 12 and Verified_Status = 1) ", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_vataccount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_vataccount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_vataccount, txt_VatAmount, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_idno = 0 or AccountsGroup_IdNo = 12 and Verified_Status = 1) ", "(Ledger_idno = 0)")

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
        Dim Led_IdNo As Integer, Delat_IdNo As Integer
        Dim Condt As String = ""


        Try

            Condt = ""
            Led_IdNo = 0
            Delat_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Item_PurchaseReturn_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Item_PurchaseReturn_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Item_PurchaseReturn_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_DelvAt.Text) <> "" Then
                Delat_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_DelvAt.Text)
            End If

            If Val(Delat_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.DeliveryTo_Idno = " & Str(Val(Delat_IdNo))
            End If


            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_Idno = " & Str(Val(Led_IdNo))
            End If

            If Trim(txt_filter_billNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Party_Bill_No = '" & Trim(txt_filter_billNo.Text) & "'"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name,C.Ledger_Name as Delv_Name from Item_PurchaseReturn_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.DeliveryTo_Idno = c.Ledger_Idno where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Item_PurchaseReturn_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Item_PurchaseReturn_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Item_PurchaseReturn_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Item_PurchaseReturn_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Party_Bill_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Delv_Name").ToString

                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Total_Pcs").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Total_Qty").ToString)
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Total_Meter").ToString), "########0.00")
                    'dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")
                    'dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.000")

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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1 )", "(Ledger_idno = 0)")

    End Sub
    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, txt_filter_billNo, cbo_Filter_DelvAt, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1)", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_DelvAt, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1) ", "(Ledger_idno = 0)")


    End Sub
    Private Sub cbo_Filter_DelvAt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_DelvAt.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or Ledger_Type = 'GODOWN' and Verified_Status = 1 )", "(Ledger_idno = 0)")

    End Sub
    Private Sub cbo_Filter_DelvAt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_DelvAt.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_DelvAt, cbo_Filter_PartyName, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or Ledger_Type = 'GODOWN' and Verified_Status = 1)", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_DelvAt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_DelvAt.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_DelvAt, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or Ledger_Type = 'GODOWN' and Verified_Status = 1) ", "(Ledger_idno = 0)")

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
        '    Dim da1 As New SqlClient.SqlDataAdapter
        '    Dim dt1 As New DataTable
        '    Dim NewCode As String

        '    NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        '    Try

        '        da1 = New SqlClient.SqlDataAdapter("select * from Item_PurchaseReturn_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Item_PurchaseReturn_Code = '" & Trim(NewCode) & "'", con)
        '        dt1 = New DataTable
        '        da1.Fill(dt1)

        '        If dt1.Rows.Count <= 0 Then

        '            MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '            Exit Sub

        '        End If


        '        dt1.Dispose()
        '        da1.Dispose()

        '    Catch ex As Exception
        '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        '    End Try

        '    If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
        '        Try
        '            PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
        '            If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
        '                PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
        '                PrintDocument1.Print()
        '            End If

        '        Catch ex As Exception
        '            MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        '        End Try


        '    Else
        '        Try

        '            Dim ppd As New PrintPreviewDialog

        '            ppd.Document = PrintDocument1

        '            ppd.WindowState = FormWindowState.Normal
        '            ppd.StartPosition = FormStartPosition.CenterScreen
        '            ppd.ClientSize = New Size(600, 600)

        '            ppd.ShowDialog()

        '        Catch ex As Exception
        '            MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

        '        End Try

        '    End If

    End Sub
    'Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
    '    Dim da1 As New SqlClient.SqlDataAdapter
    '    Dim da2 As New SqlClient.SqlDataAdapter
    '    Dim Dt1 As New DataTable
    '    Dim cmd As New SqlClient.SqlCommand
    '    Dim NewCode As String
    '    Dim i As Integer


    '    NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    prn_HdDt.Clear()
    '    prn_DetDt.Clear()
    '    prn_DetIndx = 0
    '    prn_DetSNo = 0
    '    prn_PageNo = 0

    '    Try

    '        da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*,d.Ledger_Name as Transport_Name from Item_PurchaseReturn_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON d.Ledger_IdNo = a.Ledger_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Item_PurchaseReturn_Code = '" & Trim(NewCode) & "'", con)
    '        prn_HdDt = New DataTable
    '        da1.Fill(prn_HdDt)

    '        If prn_HdDt.Rows.Count > 0 Then
    '            da2 = New SqlClient.SqlDataAdapter("select a.*, b.Processed_Item_Name as Processed_Grey_Name, c.Colour_Name ,d.Lot_No ,e.Process_Name  from Item_PurchaseReturn_Details a INNER JOIN Processed_Item_Head b on a.Item_IdNo = b.Processed_Item_IdNo LEFT OUTER JOIN Colour_Head c on a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN Lot_Head d ON d.Lot_IdNo = a.Lot_IdNo LEFT OUTER JOIN Process_Head e ON e.Process_IdNo = a.Processing_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Item_PurchaseReturn_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
    '            prn_DetDt = New DataTable
    '            da2.Fill(prn_DetDt)


    '        Else
    '            MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '        End If

    '        da1.Dispose()

    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    'Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
    '    If prn_HdDt.Rows.Count <= 0 Then Exit Sub
    '    'If Trim(UCase(Common_Procedures.settings.CompanyName)) = "" Then
    '    Printing_Format1(e)
    '    'End If
    'End Sub

    'Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
    '    Dim cmd As New SqlClient.SqlCommand
    '    Dim Da1 As New SqlClient.SqlDataAdapter
    '    Dim Dt1 As New DataTable
    '    Dim EntryCode As String
    '    Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
    '    Dim pFont As Font
    '    Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
    '    Dim PrintWidth As Single, PrintHeight As Single
    '    Dim PageWidth As Single, PageHeight As Single
    '    Dim CurY As Single, TxtHgt As Single
    '    Dim LnAr(15) As Single, ClArr(15) As Single
    '    Dim ItmNm1 As String, ItmNm2 As String
    '    Dim ps As Printing.PaperSize
    '    Dim strHeight As Single = 0
    '    Dim PpSzSTS As Boolean = False
    '    Dim W1 As Single = 0
    '    Dim SNo As Integer
    '    'PrintDocument pd = new PrintDocument();
    '    'pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 840, 1180);
    '    'pd.Print();

    '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
    '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
    '        Debug.Print(ps.PaperName)
    '        If ps.Width = 800 And ps.Height = 600 Then
    '            PrintDocument1.DefaultPageSettings.PaperSize = ps
    '            e.PageSettings.PaperSize = ps
    '            PpSzSTS = True
    '            Exit For
    '        End If
    '    Next

    '    If PpSzSTS = False Then
    '        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
    '            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
    '                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
    '                PrintDocument1.DefaultPageSettings.PaperSize = ps
    '                e.PageSettings.PaperSize = ps
    '                PpSzSTS = True
    '                Exit For
    '            End If
    '        Next

    '        If PpSzSTS = False Then
    '            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
    '                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
    '                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
    '                    PrintDocument1.DefaultPageSettings.PaperSize = ps
    '                    e.PageSettings.PaperSize = ps
    '                    Exit For
    '                End If
    '            Next
    '        End If

    '    End If

    '    With PrintDocument1.DefaultPageSettings.Margins
    '        .Left = 30
    '        .Right = 30
    '        .Top = 30
    '        .Bottom = 30
    '        LMargin = .Left
    '        RMargin = .Right
    '        TMargin = .Top
    '        BMargin = .Bottom
    '    End With

    '    pFont = New Font("Calibri", 11, FontStyle.Regular)

    '    e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

    '    With PrintDocument1.DefaultPageSettings.PaperSize
    '        PrintWidth = .Width - RMargin - LMargin
    '        PrintHeight = .Height - TMargin - BMargin
    '        PageWidth = .Width - RMargin
    '        PageHeight = .Height - BMargin
    '    End With
    '    If PrintDocument1.DefaultPageSettings.Landscape = True Then
    '        With PrintDocument1.DefaultPageSettings.PaperSize
    '            PrintWidth = .Height - TMargin - BMargin
    '            PrintHeight = .Width - RMargin - LMargin
    '            PageWidth = .Height - TMargin
    '            PageHeight = .Width - RMargin
    '        End With
    '    End If

    '    NoofItems_PerPage = 8 ' 6

    '    Erase LnAr
    '    Erase ClArr

    '    LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    '    ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

    '    ClArr(1) = Val(35) : ClArr(2) = 160 : ClArr(3) = 100 : ClArr(4) = 100 : ClArr(5) = 120 : ClArr(6) = 70 : ClArr(7) = 80
    '    ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

    '    TxtHgt = 18 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

    '    EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    Try

    '        If prn_HdDt.Rows.Count > 0 Then

    '            Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


    '            W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

    '            NoofDets = 0

    '            CurY = CurY - 10

    '            If prn_DetDt.Rows.Count > 0 Then

    '                Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

    '                    If NoofDets >= NoofItems_PerPage Then
    '                        CurY = CurY + TxtHgt

    '                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

    '                        NoofDets = NoofDets + 1

    '                        Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

    '                        e.HasMorePages = True
    '                        Return

    '                    End If

    '                    prn_DetSNo = prn_DetSNo + 1

    '                    ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Processed_Grey_Name").ToString)
    '                    ItmNm2 = ""
    '                    If Len(ItmNm1) > 18 Then
    '                        For I = 18 To 1 Step -1
    '                            If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
    '                        Next I
    '                        If I = 0 Then I = 18
    '                        ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
    '                        ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
    '                    End If

    '                    CurY = CurY + TxtHgt
    '                    SNo = SNo + 1
    '                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
    '                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
    '                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Colour_Name").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
    '                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Lot_No").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
    '                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Process_Name").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
    '                    If Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Pcs").ToString) <> 0 Then
    '                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Pcs").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
    '                    End If
    '                    If Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Meters").ToString) <> 0 Then
    '                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Meters").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
    '                    End If
    '                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Delivery_Weight").ToString), "########0.000"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)


    '                    NoofDets = NoofDets + 1

    '                    If Trim(ItmNm2) <> "" Then
    '                        CurY = CurY + TxtHgt - 5
    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
    '                        NoofDets = NoofDets + 1
    '                    End If

    '                    prn_DetIndx = prn_DetIndx + 1

    '                Loop

    '            End If


    '            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


    '        End If

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    '    e.HasMorePages = False

    'End Sub

    'Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
    '    Dim da2 As New SqlClient.SqlDataAdapter
    '    Dim dt2 As New DataTable
    '    Dim p1Font As Font
    '    Dim strHeight As Single
    '    Dim C1 As Single, W1 As Single, S1 As Single
    '    Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
    '    Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String

    '    PageNo = PageNo + 1

    '    CurY = TMargin

    '    da2 = New SqlClient.SqlDataAdapter("select a.*, b.Processed_Item_Name as Processed_Grey_Name, c.Colour_Name ,d.Lot_No ,e.Process_Name  from Item_PurchaseReturn_Details a INNER JOIN Processed_Item_Head b on a.Item_IdNo = b.Processed_Item_IdNo LEFT OUTER JOIN Colour_Head c on a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN Lot_Head d ON d.Lot_IdNo = a.Lot_IdNo LEFT OUTER JOIN Process_Head e ON e.Process_IdNo = a.Processing_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Item_PurchaseReturn_Code = '" & Trim(EntryCode) & "' Order by a.sl_no", con)
    '    da2.Fill(dt2)

    '    If dt2.Rows.Count > NoofItems_PerPage Then
    '        Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
    '    End If
    '    dt2.Clear()

    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(1) = CurY

    '    Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
    '    Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

    '    Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '    Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
    '    Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
    '    If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
    '        Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
    '        Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
    '        Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
    '    End If

    '    CurY = CurY + TxtHgt - 10
    '    p1Font = New Font("Calibri", 18, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

    '    CurY = CurY + strHeight - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

    '    CurY = CurY + TxtHgt - 10
    '    p1Font = New Font("Calibri", 16, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, "PROCESSING DELIVERY NOTE", LMargin, CurY, 2, PrintWidth, p1Font)
    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height



    '    CurY = CurY + strHeight  ' + 150
    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(2) = CurY

    '    Try
    '        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
    '        W1 = e.Graphics.MeasureString("P.O.NO  : ", pFont).Width
    '        S1 = e.Graphics.MeasureString("TO :    ", pFont).Width

    '        CurY = CurY + TxtHgt - 10
    '        p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
    '        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Item_PurchaseReturn_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        p1Font = New Font("Calibri", 14, FontStyle.Bold)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Item_PurchaseReturn_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        If Trim(prn_HdDt.Rows(0).Item("Purchase_OrderNo").ToString) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, "P.O.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Purchase_OrderNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
    '        End If
    '        CurY = CurY + TxtHgt + 5
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(3) = CurY

    '        e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

    '        CurY = CurY + TxtHgt - 10
    '        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "ITEM NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "COLOUR", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "LOT NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "PROCESSING", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

    '        CurY = CurY + TxtHgt + 5
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(4) = CurY

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    '    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
    '        Dim p1Font As Font
    '        Dim I As Integer
    '        Dim Cmp_Name As String
    '        Dim W1 As Single = 0

    '        W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

    '        Try

    '            For I = NoofDets + 1 To NoofItems_PerPage

    '                CurY = CurY + TxtHgt



    '                prn_DetIndx = prn_DetIndx + 1

    '            Next

    '            CurY = CurY + TxtHgt
    '            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '            LnAr(5) = CurY

    '            CurY = CurY + TxtHgt - 10
    '            If is_LastPage = True Then
    '                Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + ClAr(2) + 10, CurY, 2, ClAr(4), pFont)
    '            End If

    '            If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
    '                If is_LastPage = True Then
    '                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
    '                End If
    '            End If
    '            If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
    '                If is_LastPage = True Then
    '                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
    '                End If
    '            End If
    '            If Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString) <> 0 Then
    '                If is_LastPage = True Then
    '                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Weight").ToString), "#########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
    '                End If
    '            End If


    '            CurY = CurY + TxtHgt - 15

    '            CurY = CurY + TxtHgt
    '            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '            LnAr(6) = CurY

    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))

    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
    '            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

    '            CurY = CurY + TxtHgt - 5

    '            Common_Procedures.Print_To_PrintDocument(e, "Transport : " & Trim(prn_HdDt.Rows(0).Item("Transport_Name").ToString), LMargin + 10, CurY, 0, 0, pFont)
    '            'If Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString) <> 0 Then
    '            '    Common_Procedures.Print_To_PrintDocument(e, " Empty Beams : " & Trim(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), PageWidth - 250, CurY, 0, 0, pFont)
    '            'End If

    '            CurY = CurY + TxtHgt + 10
    '            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '            LnAr(7) = CurY

    '            CurY = CurY + TxtHgt
    '            If Val(Common_Procedures.User.IdNo) <> 1 Then
    '                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
    '            End If


    '            CurY = CurY + TxtHgt
    '            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 300, CurY, 0, 0, pFont)
    '            p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

    '            CurY = CurY + TxtHgt + 5

    '            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
    '            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    '        Catch ex As Exception

    '            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '        End Try

    '    End Sub



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

    Private Sub cbo_DelvAt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DelvAt.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or Ledger_Type = 'GODOWN' and Verified_Status = 1 )", "(Ledger_idno = 0)")

    End Sub
    Private Sub cbo_DelvAt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DelvAt.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DelvAt, cbo_PurchaseAccount, txt_RecNO, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or Ledger_Type = 'GODOWN' and Verified_Status = 1)", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_DelvAt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DelvAt.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DelvAt, txt_RecNO, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or Ledger_Type = 'GODOWN' and Verified_Status = 1) ", "(Ledger_idno = 0)")


    End Sub

    Private Sub cbo_DelvAt_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DelvAt.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "GODOWN"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_DelvAt.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub cbo_Agent_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Agent.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT' and Verified_Status = 1)", "(Ledger_IdNo = 0)")

    End Sub
    Private Sub cbo_Agent_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Agent, txt_RecNO, txt_CommBag, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or Ledger_Type = 'AGENT' and Verified_Status = 1)", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Agent_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Agent.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Agent, txt_CommBag, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or Ledger_Type = 'AGENT' and Verified_Status = 1) ", "(Ledger_idno = 0)")


    End Sub

    Private Sub cbo_Agent_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Dim f As New Agent_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Agent.Name
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

    Private Sub cbo_TransportName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TransportName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or Ledger_Type = 'TRANSPORT' and Verified_Status = 1)", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_TransportName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TransportName, txt_VatAmount, txt_Freight, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or Ledger_Type = 'TRANSPORT' and Verified_Status = 1)", "(Ledger_idno = 0)")


    End Sub

    Private Sub cbo_Transportname_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TransportName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TransportName, txt_Freight, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or Ledger_Type = 'TRANSPORT' and Verified_Status = 1) ", "(Ledger_idno = 0)")



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
    Private Sub cbo_PurchaseAccount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PurchaseAccount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or AccountsGroup_IdNo = 27  and Verified_Status = 1)", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Purchaseaccount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PurchaseAccount.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PurchaseAccount, cbo_Ledger, cbo_DelvAt, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or AccountsGroup_IdNo = 27  and Verified_Status = 1)", "(Ledger_idno = 0)")


    End Sub

    Private Sub cbo_Purchaseaccount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PurchaseAccount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PurchaseAccount, cbo_DelvAt, "Ledger_AlaisHead", "Ledger_DisplayName", " (ledger_idno = 0 or AccountsGroup_IdNo = 27 and Verified_Status = 1) ", "(Ledger_idno = 0)")

    End Sub
    Private Sub txt_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_VehicleNo.KeyDown
        If e.KeyCode = 40 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If
        If e.KeyCode = 38 Then SendKeys.Send("+{TAB}")
    End Sub

    Private Sub txt_BillNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BillNo.KeyDown

        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        If e.KeyCode = 38 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub txt_VehicleNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_VehicleNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub txt_VatAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_VatAmount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_CommBag_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CommBag.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub
    Private Sub txt_Billamount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Billamount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Accesablevalue_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Accesablevalue.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub cbo_PurchaseAccount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PurchaseAccount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.MDI_LedType = ""
            Dim f As New LedgerCreation_Processing

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PurchaseAccount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_vataccount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_vataccount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.MDI_LedType = ""
            Dim f As New LedgerCreation_Processing

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_vataccount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub
End Class