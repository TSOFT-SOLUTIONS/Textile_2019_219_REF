Imports Excel = Microsoft.Office.Interop.Excel
Public Class Ledger_Master_IdNo_Transfer_Entry
    Implements Interface_MDIActions

    Dim con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Dim New_Entry As Boolean
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private Filter_RowNo As Integer = -1

    Private Sub CLEAR()
        Me.Height = 270
        pnl_Back.Enabled = True

        cbo_ValueFrom.Text = ""
        cbo_ValueTo.Text = ""


    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox


        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Then
            Me.ActiveControl.BackColor = Color.SpringGreen ' Color.MistyRose ' Color.PaleGreen
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
    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        On Error Resume Next
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub


    Public Sub move_record(ByVal idno As Integer)
        '-------

    End Sub


    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        '-------
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '-------
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '------
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        '------

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        '----------
    End Sub
    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        '--------
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        '------------
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record

        '------
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        '-----------
    End Sub


    Public Sub print_record() Implements Interface_MDIActions.print_record
        '----
    End Sub


    Public Sub save_record() Implements Interface_MDIActions.save_record
        '----------

    End Sub



    Private Sub Ledger_Master_IdNo_Transfer_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Ledger_Master_IdNo_Transfer_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        'If Asc(e.KeyChar) = 27 Then
        '    If grp_Filter.Visible Then
        '        btn_CloseFilter_Click(sender, e)

        '    ElseIf grp_Open.Visible Then
        '        btn_CloseOpen_Click(sender, e)
        '    Else
        '        'If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
        '        '    Exit Sub
        '        'Else
        '        Me.Close()
        '        'End If
        '    End If

        'End If

    End Sub

    Private Sub Ledger_Master_IdNo_Transfer_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        CLEAR()

        AddHandler cbo_ValueFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ValueTo.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_ValueFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ValueTo.LostFocus, AddressOf ControlLostFocus

        con.Open()
        Me.Top = 50

    End Sub

    Private Sub cbo_ValueFrom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ValueFrom.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_ValueFrom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ValueFrom.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ValueFrom, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")

        If e.KeyCode = 40 And cbo_ValueFrom.DroppedDown = False Then
            cbo_ValueTo.FindForm()
        End If
    End Sub

    Private Sub cbo_ValueFrom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ValueFrom.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ValueFrom, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            cbo_ValueTo.Focus()
        End If
    End Sub

    Private Sub cbo_ValueTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ValueTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_ValueTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ValueTo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ValueTo, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
        If e.KeyCode = 38 And cbo_ValueTo.DroppedDown = False Then
            cbo_ValueFrom.Focus()
        End If
        If e.KeyCode = 40 And cbo_ValueTo.DroppedDown = False Then
            btn_Transfer.Focus()
        End If
    End Sub

    Private Sub cbo_ValueTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ValueTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ValueTo, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to Transfer?", "FOR TRANSFER...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                TransferData()
            Else
                cbo_ValueFrom.Focus()
            End If
        End If
    End Sub


    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub

    Private Sub btn_Transfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Transfer.Click
        TransferData()
    End Sub

    Private Sub TransferData()
        Dim trans As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim TransFrm_Id As Integer = 0
        Dim TransTo_Id As Integer = 0
        Dim SlNo As Integer = 0

        If Trim(cbo_ValueFrom.Text) = "" Then
            MessageBox.Show("Invalid Ledger From Value", "DOES NOT TRANSFER", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ValueFrom.Enabled Then cbo_ValueFrom.Focus()
            Exit Sub
        End If
        If Trim(cbo_ValueTo.Text) = "" Then
            MessageBox.Show("Invalid Ledger To Value", "DOES NOT TRANSFER", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ValueTo.Enabled Then cbo_ValueTo.Focus()
            Exit Sub
        End If

        TransFrm_Id = Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, Trim(cbo_ValueFrom.Text)))

        TransTo_Id = Val(Common_Procedures.Ledger_AlaisNameToIdNo(con, Trim(cbo_ValueTo.Text)))



        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans




            cmd.CommandText = "update Stock_BabyCone_Processing_Details set DeliveryTo_Idno = " & Str(Val(TransTo_Id)) & " where DeliveryTo_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Stock_BabyCone_Processing_Details set ReceivedFrom_Idno = " & Str(Val(TransTo_Id)) & " where ReceivedFrom_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Stock_Cloth_Processing_Details set DeliveryTo_Idno = " & Str(Val(TransTo_Id)) & " where DeliveryTo_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Stock_Cloth_Processing_Details set ReceivedFrom_Idno = " & Str(Val(TransTo_Id)) & " where ReceivedFrom_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Stock_Cloth_Processing_Details set StockOff_IdNo = " & Str(Val(TransTo_Id)) & " where StockOff_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Stock_Empty_BeamBagCone_Processing_Details set DeliveryTo_Idno = " & Str(Val(TransTo_Id)) & " where DeliveryTo_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Stock_Empty_BeamBagCone_Processing_Details set ReceivedFrom_Idno = " & Str(Val(TransTo_Id)) & " where ReceivedFrom_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Stock_Empty_BeamBagCone_Processing_Details set Vendor_Idno = " & Str(Val(TransTo_Id)) & " where Vendor_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Stock_Pavu_Processing_Details set DeliveryTo_Idno = " & Str(Val(TransTo_Id)) & " where DeliveryTo_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Stock_Pavu_Processing_Details set ReceivedFrom_Idno = " & Str(Val(TransTo_Id)) & " where ReceivedFrom_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Stock_Pavu_Processing_Details set DeliveryToIdno_ForParticulars = " & Str(Val(TransTo_Id)) & " where DeliveryToIdno_ForParticulars = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Stock_Pavu_Processing_Details set ReceivedFromIdno_ForParticulars = " & Str(Val(TransTo_Id)) & " where ReceivedFromIdno_ForParticulars = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Stock_Pavu_Processing_Details set StockOf_IdNo = " & Str(Val(TransTo_Id)) & " where StockOf_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Stock_SizedPavu_Processing_Details set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Stock_SizedPavu_Processing_Details set StockAt_IdNo = " & Str(Val(TransTo_Id)) & " where StockAt_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Stock_SizedPavu_Processing_Details set Vendor_Idno = " & Str(Val(TransTo_Id)) & " where Vendor_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Stock_Yarn_Processing_Details set DeliveryTo_Idno = " & Str(Val(TransTo_Id)) & " where DeliveryTo_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Stock_Yarn_Processing_Details set ReceivedFrom_Idno = " & Str(Val(TransTo_Id)) & " where ReceivedFrom_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Stock_Yarn_Processing_Details set DeliveryToIdno_ForParticulars = " & Str(Val(TransTo_Id)) & " where DeliveryToIdno_ForParticulars = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Stock_Yarn_Processing_Details set ReceivedFromIdno_ForParticulars = " & Str(Val(TransTo_Id)) & " where ReceivedFromIdno_ForParticulars = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()


            cmd.CommandText = "update Voucher_Bill_Details set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Voucher_Bill_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Voucher_Bill_Head set Agent_IdNo = " & Str(Val(TransTo_Id)) & " where Agent_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Voucher_Details set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Voucher_Head set Debtor_Idno = " & Str(Val(TransTo_Id)) & " where Debtor_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Voucher_Head set Creditor_Idno = " & Str(Val(TransTo_Id)) & " where Creditor_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()


            cmd.CommandText = "update Weaver_Cloth_Receipt_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Weaver_Cloth_Receipt_Head set Transport_IdNo = " & Str(Val(TransTo_Id)) & " where Transport_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Weaver_Cloth_Receipt_Head set StockOff_IdNo = " & Str(Val(TransTo_Id)) & " where StockOff_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Weaver_Cloth_Receipt_Head set WareHouse_IdNo = " & Str(Val(TransTo_Id)) & " where WareHouse_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set StockOff_IdNo = " & Str(Val(TransTo_Id)) & " where StockOff_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set WareHouse_IdNo = " & Str(Val(TransTo_Id)) & " where WareHouse_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_KuraiPavu_Receipt_Details set DeliveryTo_IdNo = " & Str(Val(TransTo_Id)) & " where DeliveryTo_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Weaver_KuraiPavu_Receipt_Details set ReceivedFrom_IdNo = " & Str(Val(TransTo_Id)) & " where ReceivedFrom_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Weaver_KuraiPavu_Receipt_Head set DeliveryTo_IdNo = " & Str(Val(TransTo_Id)) & " where DeliveryTo_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Weaver_KuraiPavu_Receipt_Head set ReceivedFrom_IdNo = " & Str(Val(TransTo_Id)) & " where ReceivedFrom_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_Pavu_Delivery_Details set DeliveryTo_IdNo = " & Str(Val(TransTo_Id)) & " where DeliveryTo_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Weaver_Pavu_Delivery_Details set ReceivedFrom_IdNo = " & Str(Val(TransTo_Id)) & " where ReceivedFrom_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Weaver_Pavu_Delivery_Head set DeliveryTo_IdNo = " & Str(Val(TransTo_Id)) & " where DeliveryTo_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Weaver_Pavu_Delivery_Head set ReceivedFrom_IdNo = " & Str(Val(TransTo_Id)) & " where ReceivedFrom_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Weaver_Pavu_Delivery_Head set Transport_Idno = " & Str(Val(TransTo_Id)) & " where Transport_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()


            cmd.CommandText = "update Weaver_Pavu_Receipt_Details set DeliveryTo_Idno = " & Str(Val(TransTo_Id)) & " where DeliveryTo_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Weaver_Pavu_Receipt_Details set ReceivedFrom_Idno = " & Str(Val(TransTo_Id)) & " where ReceivedFrom_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Weaver_Pavu_Receipt_Head set DeliveryTo_Idno = " & Str(Val(TransTo_Id)) & " where DeliveryTo_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Weaver_Pavu_Receipt_Head set ReceivedFrom_Idno = " & Str(Val(TransTo_Id)) & " where ReceivedFrom_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Weaver_Pavu_Receipt_Head set Transport_IdNo = " & Str(Val(TransTo_Id)) & " where Transport_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()


            cmd.CommandText = "update Weaver_Payment_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Weaver_Payment_Head set Creditor_IdNo = " & Str(Val(TransTo_Id)) & " where Creditor_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_Piece_Checking_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_Wages_Cooly_Details set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Weaver_Wages_Details set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Weaver_Wages_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Weaver_Wages_Head set Transport_IdNo = " & Str(Val(TransTo_Id)) & " where Transport_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_Yarn_Delivery_Details set DeliveryTo_Idno = " & Str(Val(TransTo_Id)) & " where DeliveryTo_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Weaver_Yarn_Delivery_Details set ReceiverFrom_idNo = " & Str(Val(TransTo_Id)) & " where ReceiverFrom_idNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Weaver_Yarn_Delivery_Head set DeliveryTo_Idno = " & Str(Val(TransTo_Id)) & " where DeliveryTo_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Weaver_Yarn_Delivery_Head set ReceivedFrom_Idno = " & Str(Val(TransTo_Id)) & " where ReceivedFrom_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Weaver_Yarn_Delivery_Head set Transport_IdNo = " & Str(Val(TransTo_Id)) & " where Transport_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_Yarn_Receipt_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Weaver_Yarn_Receipt_Head set Transport_IdNo = " & Str(Val(TransTo_Id)) & " where Transport_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaving_Yarn_Excess_Short_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Weaving_Yarn_Excess_Short_Head set ExcessShort_Ac_IdNo = " & Str(Val(TransTo_Id)) & " where ExcessShort_Ac_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Yarn_Excess_Short_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Yarn_Excess_Short_Head set ExcessShort_Ac_IdNo = " & Str(Val(TransTo_Id)) & " where ExcessShort_Ac_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Yarn_Purchase_GST_Tax_Details set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Yarn_Purchase_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Yarn_Purchase_Head set Agent_IdNo = " & Str(Val(TransTo_Id)) & " where Agent_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Yarn_Purchase_Head set PurchaseAc_IdNo = " & Str(Val(TransTo_Id)) & " where PurchaseAc_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Yarn_Purchase_Head set TaxAc_IdNo = " & Str(Val(TransTo_Id)) & " where TaxAc_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Yarn_Purchase_Head set DeliveryTo_Idno = " & Str(Val(TransTo_Id)) & " where DeliveryTo_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Yarn_Purchase_Head set Transport_IdNo = " & Str(Val(TransTo_Id)) & " where Transport_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Yarn_Purchase_Order_GST_Tax_Details set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Yarn_Purchase_Order_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Yarn_Purchase_Order_Head set Agent_IdNo = " & Str(Val(TransTo_Id)) & " where Agent_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Yarn_Purchase_Order_Head set TaxAc_IdNo = " & Str(Val(TransTo_Id)) & " where TaxAc_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Yarn_Purchase_Order_Head set DeliveryTo_Idno = " & Str(Val(TransTo_Id)) & " where DeliveryTo_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Yarn_Purchase_Order_Head set Transport_IdNo = " & Str(Val(TransTo_Id)) & " where Transport_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()


            cmd.CommandText = "update Yarn_Purchase_Return_GST_Tax_Details set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Yarn_Purchase_Return_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Yarn_Purchase_Return_Head set Agent_IdNo = " & Str(Val(TransTo_Id)) & " where Agent_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Yarn_Purchase_Return_Head set PurchaseAc_IdNo = " & Str(Val(TransTo_Id)) & " where PurchaseAc_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Yarn_Purchase_Return_Head set TaxAc_IdNo = " & Str(Val(TransTo_Id)) & " where TaxAc_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Yarn_Purchase_Return_Head set DeliveryTo_Idno = " & Str(Val(TransTo_Id)) & " where DeliveryTo_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Yarn_Purchase_Return_Head set Transport_IdNo = " & Str(Val(TransTo_Id)) & " where Transport_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Yarn_Sales_GST_Tax_Details set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Yarn_Sales_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Yarn_Sales_Head set Agent_IdNo = " & Str(Val(TransTo_Id)) & " where Agent_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Yarn_Sales_Head set PurchaseAc_IdNo = " & Str(Val(TransTo_Id)) & " where PurchaseAc_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Yarn_Sales_Head set TaxAc_IdNo = " & Str(Val(TransTo_Id)) & " where TaxAc_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Yarn_Sales_Head set DeliveryTo_Idno = " & Str(Val(TransTo_Id)) & " where DeliveryTo_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Yarn_Sales_Head set Transport_IdNo = " & Str(Val(TransTo_Id)) & " where Transport_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Yarn_Sales_Head set RecFrom_Idno = " & Str(Val(TransTo_Id)) & " where RecFrom_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Yarn_Transfer_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Yarn_Transfer_Head set LedgerTo_IdNo = " & Str(Val(TransTo_Id)) & " where LedgerTo_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Packing_Slip_Details set Party_IdNo = " & Str(Val(TransTo_Id)) & " where Party_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Packing_Slip_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Packing_Slip_Head set WareHouse_IdNo = " & Str(Val(TransTo_Id)) & " where WareHouse_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Party_Amount_Receipt_Details set Agent_IdNo = " & Str(Val(TransTo_Id)) & " where Agent_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Party_Amount_Receipt_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Party_Amount_Receipt_Head set Debtor_Idno = " & Str(Val(TransTo_Id)) & " where Debtor_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Pavu_Delivery_Details set DeliveryTo_Idno = " & Str(Val(TransTo_Id)) & " where DeliveryTo_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Pavu_Delivery_Details set ReceivedFrom_Idno = " & Str(Val(TransTo_Id)) & " where ReceivedFrom_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Pavu_Excess_Short_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Pavu_Excess_Short_Head set ExcessShort_Ac_IdNo = " & Str(Val(TransTo_Id)) & " where ExcessShort_Ac_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Pavu_Purchase_Details set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Pavu_Purchase_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Pavu_Purchase_Head set DeliveryTo_IdNo = " & Str(Val(TransTo_Id)) & " where DeliveryTo_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Pavu_Purchase_Head set PurchaseAc_IdNo = " & Str(Val(TransTo_Id)) & " where PurchaseAc_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Pavu_Receipt_Details set DeliveryTo_Idno = " & Str(Val(TransTo_Id)) & " where DeliveryTo_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Pavu_Receipt_Details set ReceivedFrom_Idno = " & Str(Val(TransTo_Id)) & " where ReceivedFrom_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Pavu_Sales_Details set DeliveryTo_Idno = " & Str(Val(TransTo_Id)) & " where DeliveryTo_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Pavu_Sales_Details set ReceivedFrom_Idno = " & Str(Val(TransTo_Id)) & " where ReceivedFrom_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Pavu_Sales_Head set DeliveryTo_Idno = " & Str(Val(TransTo_Id)) & " where DeliveryTo_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Pavu_Sales_Head set ReceivedFrom_Idno = " & Str(Val(TransTo_Id)) & " where ReceivedFrom_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Pavu_Sales_Head set Transport_Idno = " & Str(Val(TransTo_Id)) & " where Transport_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Pavu_Sales_Head set SalesAc_IdNo = " & Str(Val(TransTo_Id)) & " where SalesAc_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()


            cmd.CommandText = "update Pavu_Transfer_BeamWise_Details set DeliveryTo_Idno = " & Str(Val(TransTo_Id)) & " where DeliveryTo_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Pavu_Transfer_BeamWise_Details set ReceivedFrom_Idno = " & Str(Val(TransTo_Id)) & " where ReceivedFrom_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Pavu_Transfer_BeamWise_head set DeliveryTo_Idno = " & Str(Val(TransTo_Id)) & " where DeliveryTo_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Pavu_Transfer_BeamWise_head set ReceivedFrom_Idno = " & Str(Val(TransTo_Id)) & " where ReceivedFrom_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Pavu_Transfer_Head set Ledger_Idno = " & Str(Val(TransTo_Id)) & " where Ledger_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Pavu_Transfer_Head set LedgerTo_IdNo = " & Str(Val(TransTo_Id)) & " where LedgerTo_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update PavuYarn_Delivery_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update PavuYarn_Delivery_Head set Transport_Idno = " & Str(Val(TransTo_Id)) & " where Transport_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update PavuYarn_Delivery_Head set DeliveryTo_Idno = " & Str(Val(TransTo_Id)) & " where DeliveryTo_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update PavuYarn_Delivery_Head set ReceivedFrom_Idno = " & Str(Val(TransTo_Id)) & " where ReceivedFrom_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update PavuYarn_Delivery_Head set Yarn_ReceivedFrom_IdNo = " & Str(Val(TransTo_Id)) & " where Yarn_ReceivedFrom_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update PavuYarn_Receipt_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update PavuYarn_Receipt_Head set Transport_Idno = " & Str(Val(TransTo_Id)) & " where Transport_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update PavuYarn_Receipt_Head set DeliveryTo_Idno = " & Str(Val(TransTo_Id)) & " where DeliveryTo_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update PavuYarn_Receipt_Head set ReceivedFrom_Idno = " & Str(Val(TransTo_Id)) & " where ReceivedFrom_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Piece_Excess_Short_Details set Party_IdNo = " & Str(Val(TransTo_Id)) & " where Party_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Piece_Excess_Short_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Piece_Opening_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Piece_Opening_Head set WareHouse_IdNo = " & Str(Val(TransTo_Id)) & " where WareHouse_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Piece_Transfer_Head set LedgerFrom_IdNo = " & Str(Val(TransTo_Id)) & " where LedgerFrom_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Piece_Transfer_Head set LedgerTo_IdNo = " & Str(Val(TransTo_Id)) & " where LedgerTo_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Rewinding_Delivery_Head set DeliveryTo_Idno = " & Str(Val(TransTo_Id)) & " where DeliveryTo_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Rewinding_Delivery_Head set ReceivedFrom_Idno = " & Str(Val(TransTo_Id)) & " where ReceivedFrom_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Rewinding_Delivery_Head set Transport_IdNo = " & Str(Val(TransTo_Id)) & " where Transport_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Rewinding_Receipt_Head set DeliveryTo_Idno = " & Str(Val(TransTo_Id)) & " where DeliveryTo_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Rewinding_Receipt_Head set ReceivedFrom_Idno = " & Str(Val(TransTo_Id)) & " where ReceivedFrom_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Rewinding_Receipt_Head set Transport_IdNo = " & Str(Val(TransTo_Id)) & " where Transport_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()


            cmd.CommandText = "update Sizing_Pavu_Receipt_Details set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Sizing_Pavu_Receipt_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Sizing_Pavu_Receipt_Head set DeliveryTo_IdNo = " & Str(Val(TransTo_Id)) & " where DeliveryTo_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Sizing_Specification_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Sizing_SpecificationPavu_Details set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Sizing_Yarn_Delivery_Head set DeliveryTo_Idno = " & Str(Val(TransTo_Id)) & " where DeliveryTo_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Sizing_Yarn_Delivery_Head set ReceivedFrom_Idno = " & Str(Val(TransTo_Id)) & " where ReceivedFrom_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Sizing_Yarn_Delivery_Head set Transport_IdNo = " & Str(Val(TransTo_Id)) & " where Transport_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Sizing_Yarn_Receipt_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Sizing_Yarn_Receipt_Head set DeliveryTo_Idno = " & Str(Val(TransTo_Id)) & " where DeliveryTo_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Sizing_Yarn_Receipt_Head set Transport_IdNo = " & Str(Val(TransTo_Id)) & " where Transport_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update AgentCommission_Processing_Details set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update AgentCommission_Processing_Details set Agent_IdNo = " & Str(Val(TransTo_Id)) & " where Agent_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Beam_Knotting_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Cheque_Print_Positioning_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Cloth_Purchase_Details set Ledger_Idno = " & Str(Val(TransTo_Id)) & " where Ledger_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Cloth_Purchase_GST_Tax_Details set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Cloth_Purchase_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Cloth_Purchase_Head set Agent_IdNo = " & Str(Val(TransTo_Id)) & " where Agent_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Cloth_Purchase_Head set PurchaseAc_IdNo = " & Str(Val(TransTo_Id)) & " where PurchaseAc_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Cloth_Purchase_Head set Transport_IdNo = " & Str(Val(TransTo_Id)) & " where Transport_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Cloth_Purchase_Receipt_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Cloth_Transfer_Head set LedgerFrom_IdNo = " & Str(Val(TransTo_Id)) & " where LedgerFrom_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Cloth_Transfer_Head set LedgerTo_IdNo = " & Str(Val(TransTo_Id)) & " where LedgerTo_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update ClothPurchase_Order_Details set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update ClothPurchase_Order_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update ClothPurchase_Order_Head set Agent_IdNo = " & Str(Val(TransTo_Id)) & " where Agent_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update ClothPurchase_Order_Head set Transport_IdNo = " & Str(Val(TransTo_Id)) & " where Transport_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update ClothSales_Delivery_Details set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update ClothSales_Delivery_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update ClothSales_Delivery_Head set Agent_IdNo = " & Str(Val(TransTo_Id)) & " where Agent_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update ClothSales_Delivery_Head set DeliveryTo_Idno = " & Str(Val(TransTo_Id)) & " where DeliveryTo_Idno = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update ClothSales_Delivery_Head set Transport_IdNo = " & Str(Val(TransTo_Id)) & " where Transport_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update ClothSales_Delivery_Return_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update ClothSales_Delivery_Return_Head set Transport_IdNo = " & Str(Val(TransTo_Id)) & " where Transport_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update ClothSales_Invoice_BaleEntry_Details set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update ClothSales_Invoice_Buyer_Offer_Details set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update ClothSales_Invoice_GST_Tax_Details set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update ClothSales_Invoice_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update ClothSales_Invoice_Head set Agent_IdNo = " & Str(Val(TransTo_Id)) & " where Agent_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update ClothSales_Invoice_Head set SalesAc_IdNo = " & Str(Val(TransTo_Id)) & " where SalesAc_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update ClothSales_Invoice_Head set OnAc_IdNo = " & Str(Val(TransTo_Id)) & " where OnAc_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update ClothSales_Invoice_Head set Transport_IdNo = " & Str(Val(TransTo_Id)) & " where Transport_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update ClothSales_Invoice_Head set TaxAc_IdNo = " & Str(Val(TransTo_Id)) & " where TaxAc_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update ClothSales_Order_Details set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update ClothSales_Order_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update ClothSales_Order_Head set Agent_IdNo = " & Str(Val(TransTo_Id)) & " where Agent_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update ClothSales_Order_Head set Transport_IdNo = " & Str(Val(TransTo_Id)) & " where Transport_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update ClothSales_Order_Head set DeliveryTo_IdNo = " & Str(Val(TransTo_Id)) & " where DeliveryTo_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()


            cmd.CommandText = "update ClothSales_Return_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update ClothSales_Return_Head set PurchaseAc_IdNo = " & Str(Val(TransTo_Id)) & " where PurchaseAc_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update ClothSales_Return_Head set Transport_IdNo = " & Str(Val(TransTo_Id)) & " where Transport_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Empty_Bag_Receipt_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Empty_Beam_Purchase_Entry_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Empty_Beam_Purchase_Entry_Head set Purchase_Acc_IdNo = " & Str(Val(TransTo_Id)) & " where Purchase_Acc_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Empty_Beam_Purchase_Entry_Head set Vat_Acc_IdNo = " & Str(Val(TransTo_Id)) & " where Vat_Acc_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Empty_Beam_Sales_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Empty_Beam_Sales_Head set SalesAc_IdNo = " & Str(Val(TransTo_Id)) & " where SalesAc_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Empty_BeamBagCone_Delivery_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Empty_BeamBagCone_Delivery_Head set ReceivedFrom_IdNo = " & Str(Val(TransTo_Id)) & " where ReceivedFrom_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Empty_BeamBagCone_Delivery_Head set Transport_IdNo = " & Str(Val(TransTo_Id)) & " where Transport_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Empty_BeamBagCone_Receipt_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Empty_BeamBagCone_Receipt_Head set Transport_IdNo = " & Str(Val(TransTo_Id)) & " where Transport_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Other_GST_Entry_Details set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Other_GST_Entry_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Other_GST_Entry_Head set OnAccount_IdNo = " & Str(Val(TransTo_Id)) & " where OnAccount_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Other_GST_Entry_Tax_Details set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Other_GST_Purchase_Details set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Other_GST_Purchase_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Other_GST_Purchase_Head set PurchaseAc_IdNo = " & Str(Val(TransTo_Id)) & " where PurchaseAc_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Other_GST_Sales_Details set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Other_GST_Sales_Head set Ledger_IdNo = " & Str(Val(TransTo_Id)) & " where Ledger_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()
            cmd.CommandText = "update Other_GST_Sales_Head set SalesAc_IdNo = " & Str(Val(TransTo_Id)) & " where SalesAc_IdNo = " & Str(Val(TransFrm_Id))
            cmd.ExecuteNonQuery()


            trans.Commit()

            MessageBox.Show("Transfered Sucessfully!!!", "FOR TRANSFER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT TRANSFER", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            If cbo_ValueFrom.Enabled And cbo_ValueFrom.Visible Then cbo_ValueFrom.Focus()

        End Try

    End Sub

End Class