Public Class Jari_Purchase_Entry

    Implements Interface_MDIActions
    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "JRPUR-"
    Private Pk_Condition2 As String = "YPAGC-"
    'Private Pk_Condition3 As String = "YPFRG-"
    'Private Pk_Condition4 As String = "YPATD-"
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
    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Public vmskBillOldText As String = ""
    Public vmskBillSelStrt As Integer = -1

    Dim ItemGrpID As Integer = 0
    Private Sub clear()
        lbl_RefNo.Text = ""
        lbl_DiscAmount.Text = ""
        lbl_NetAmount.Text = ""
       

        txt_AddLess.Text = ""
        lbl_TaxableValue.Text = ""
        txt_Box_Rolls.Text = ""
        txt_DiscPerc.Text = ""
        txt_Freight.Text = ""
        txt_Kgs.Text = ""
        txt_Rate.Text = ""
        txt_BillNo.Text = ""
        txt_Note.Text = ""

        cbo_Color.Text = ""
        cbo_Count.Text = ""
        cbo_Ledger.Text = ""
        cbo_PurchaseAc.Text = ""

        cbo_Filter_Colour.Text = ""
        cbo_Filter_Item.Text = ""
        cbo_Filter_PartyName.Text = ""

        lbl_TaxableValue.Text = ""
        lbl_CGST_Amount.Text = ""
        lbl_SGST_Amount.Text = ""
        lbl_IGST_Amount.Text = ""
        lbl_grid_GstPerc.Text = ""
        lbl_Grid_HSNCode.Text = ""
        cbo_Type.Text = ""



        msk_BillDate.Clear()
        msk_RefDate.Clear()
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
    Private Sub Amount_Calculation()

        txt_Amount.Text = Format(Val(txt_Kgs.Text) * Val(txt_Rate.Text).ToString, "#########0.00")

    End Sub
    Private Sub Jari_Purchase_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Color.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COLOUR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Color.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Count.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Count.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PurchaseAc.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PurchaseAc.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    'Private Sub Jari_Purchase_Entry_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
    '    'Try

    '    If e.KeyValue = 27 Then
    '        If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
    '            Exit Sub

    '        Else
    '            Close_Form()
    '        End If

    '    End If

    '    'Catch ex As Exception
    '    '    MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

    '    'End Try
    'End Sub

    Private Sub Jari_Purchase_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try

            If Asc(e.KeyChar) = 27 Then
                If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub

                Else
                    Close_Form()
                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub Jari_Purchase_Entry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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

        '-------------------COUNT COMBO-------------------------------------------------

        da = New SqlClient.SqlDataAdapter("select distinct(Count_Name) from Count_Head order by Count_Name", con)
        da.Fill(dt2)
        cbo_Count.DataSource = dt2
        cbo_Count.DisplayMember = "Count_Name"

        '--------------------PARTYNAME COMBO------------------------------------------------

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'AGENT') order by Ledger_DisplayName", con)
        da.Fill(dt3)
        cbo_Ledger.DataSource = dt3
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"

        '----------------------TAXTYPE COMBO----------------------------------------------

        cbo_Type.Items.Clear()
        cbo_Type.Items.Add(" ")
        cbo_Type.Items.Add("GST")
        cbo_Type.Items.Add("NO TAX")


        '--------------------------------------------------------------------
        clear()

        new_record()

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()


        AddHandler txt_AddLess.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_TaxableValue.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Box_Rolls.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Kgs.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Color.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BillNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PurchaseAc.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_BillDate.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_RefDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_Colour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Item.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_FilterFrom_date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_FilterTo_date.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_AddLess.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_TaxableValue.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Box_Rolls.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiscPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Kgs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BillNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PurchaseAc.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_BillDate.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_RefDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Count.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Color.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_Colour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Item.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_FilterFrom_date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_FilterTo_date.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_AddLess.KeyDown, AddressOf TextboxControlKeyDown
        AddHandler txt_Amount.KeyDown, AddressOf TextboxControlKeyDown
        AddHandler lbl_TaxableValue.KeyDown, AddressOf TextboxControlKeyDown
        AddHandler txt_Box_Rolls.KeyDown, AddressOf TextboxControlKeyDown
        AddHandler txt_DiscPerc.KeyDown, AddressOf TextboxControlKeyDown
        AddHandler txt_Freight.KeyDown, AddressOf TextboxControlKeyDown
        AddHandler txt_Kgs.KeyDown, AddressOf TextboxControlKeyDown
        AddHandler txt_Freight.KeyDown, AddressOf TextboxControlKeyDown
        AddHandler txt_Rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_RefDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BillNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_BillDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterFrom_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterTo_date.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_AddLess.KeyPress, AddressOf TextboxControlKeyPress
        AddHandler txt_Amount.KeyPress, AddressOf TextboxControlKeyPress
        AddHandler lbl_TaxableValue.KeyPress, AddressOf TextboxControlKeyPress
        AddHandler txt_Box_Rolls.KeyPress, AddressOf TextboxControlKeyPress
        AddHandler txt_DiscPerc.KeyPress, AddressOf TextboxControlKeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextboxControlKeyPress
        AddHandler txt_Rate.KeyPress, AddressOf TextboxControlKeyPress
        AddHandler txt_Kgs.KeyPress, AddressOf TextboxControlKeyPress
        AddHandler txt_Rate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_RefDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BillNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterFrom_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterTo_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_BillDate.KeyPress, AddressOf TextBoxControlKeyPress



        FrmLdSTS = True

        con.Open()

    End Sub
    Private Sub NetAmount_Calculation()
        Dim NtAmt As Single
        Dim AssAmt As Single = 0
        Dim CGSTAmt As Single = 0
        Dim SGSTAmt As Single = 0
        Dim IGSTAmt As Single = 0
        Dim Ledger_State_Code As String = ""
        Dim Company_State_Code As String = ""
        Dim Led_IdNo As Integer
        Dim ItemGrpID As Integer = 0
        If NoCalc_Status = True Then Exit Sub
     
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Then

            lbl_DiscAmount.Text = Format(Val(txt_Amount.Text) * Val(txt_DiscPerc.Text) / 100, "########0.00")

            lbl_TaxableValue.Text = Format(Val(txt_Amount.Text) - Val(lbl_DiscAmount.Text) + Val(txt_AddLess.Text) + Val(txt_Freight.Text), "########0.00")

        End If

        lbl_Grid_HSNCode.Text = ""

        ItemGrpID = Val(Common_Procedures.get_FieldValue(con, "Count_Head", "ItemGroup_IdNo", "Count_Name ='" & Trim(cbo_Count.Text) & "' "))

        lbl_Grid_HSNCode.Text = Common_Procedures.get_FieldValue(con, "ItemGroup_Head", "Item_HSN_Code", "ItemGroup_IdNo = " & ItemGrpID & " ")

        lbl_grid_GstPerc.Text = Val(Common_Procedures.get_FieldValue(con, "ItemGroup_Head", "Item_GST_Percentage", "ItemGroup_IdNo = " & ItemGrpID & " "))

        If Trim(cbo_Type.Text) = "GST" Then

            Led_IdNo = Val(Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_IdNo", "Ledger_Name = '" & Trim(cbo_Ledger.Text) & "'"))
            Get_State_Code(Led_IdNo, Ledger_State_Code, Company_State_Code)

            lbl_grid_GstPerc.Text = 0

            lbl_Grid_HSNCode.Text = ""
            lbl_Grid_HSNCode.Text = Common_Procedures.get_FieldValue(con, "ItemGroup_Head", "Item_HSN_Code", "ItemGroup_IdNo = " & ItemGrpID & " ")

            lbl_grid_GstPerc.Text = Val(Common_Procedures.get_FieldValue(con, "ItemGroup_Head", "Item_GST_Percentage", "ItemGroup_IdNo = " & ItemGrpID & " "))

            lbl_CGST_Amount.Text = ""
            lbl_SGST_Amount.Text = ""
            lbl_IGST_Amount.Text = ""
            If Trim(Company_State_Code) = Trim(Ledger_State_Code) Then
                '-CGST 
                lbl_CGST_Amount.Text = Format(Val(lbl_TaxableValue.Text) * (Val(lbl_grid_GstPerc.Text) / 2) / 100, "#########0.00")
                '-SGST 
                lbl_SGST_Amount.Text = Format(Val(lbl_TaxableValue.Text) * (Val(lbl_grid_GstPerc.Text) / 2) / 100, "#########0.00")

            ElseIf Trim(Company_State_Code) <> Trim(Ledger_State_Code) Then
                '-IGST 
                lbl_IGST_Amount.Text = Format(Val(lbl_TaxableValue.Text) * Val(lbl_grid_GstPerc.Text) / 100, "#########0.00")

            End If

        End If
        NtAmt = Val(lbl_TaxableValue.Text) + Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text) + Val(lbl_IGST_Amount.Text)

        lbl_NetAmount.Text = Format(Val(NtAmt), "##########0")
        lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(lbl_NetAmount.Text))

        lbl_RoundOff.Text = Format(Val(CSng(lbl_NetAmount.Text)) - Val(NtAmt), "#########0.00")
        If Val(lbl_RoundOff.Text) = 0 Then lbl_RoundOff.Text = ""

        'lbl_AmountInWords.Text = "Rupees  :  "
        'If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
        '    lbl_AmountInWords.Text = "Rupees  :  " & Common_Procedures.Rupees_Converstion(Val(CSng(lbl_NetAmount.Text)))
        'End If
    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim mskdtxbx As MaskedTextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            mskdtxbx = Me.ActiveControl
            mskdtxbx.SelectionStart = 0
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
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
                Prec_ActCtrl.BackColor = Color.DeepPink
                Prec_ActCtrl.ForeColor = Color.White
            End If
        End If
    End Sub

    Private Sub ControlLostFocus1(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.LightSkyBlue
                Prec_ActCtrl.ForeColor = Color.Blue
            End If
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


    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Yarn_Purchase_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Yarn_Purchase_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

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

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)

            cmd.CommandText = "delete from Jari_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Jari_Purchase_Code = '" & Trim(NewCode) & "'"
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

            If msk_RefDate.Enabled = True And msk_RefDate.Visible = True Then msk_RefDate.Focus()
            'If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()
        End Try
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_head order by Colour_Name", con)
            da.Fill(dt2)
            cbo_Filter_Colour.DataSource = dt2
            cbo_Filter_Colour.DisplayMember = "Colour_Name"

            da = New SqlClient.SqlDataAdapter("select Count_Name from Count_head order by Count_Name", con)
            da.Fill(dt3)
            cbo_Filter_Item.DataSource = dt3
            cbo_Filter_Item.DisplayMember = "Count_Name"


            dtp_FilterFrom_date.Text = ""
            dtp_FilterTo_date.Text = ""

            cbo_Filter_PartyName.Text = ""
            cbo_Filter_Colour.Text = ""
            cbo_Filter_Item.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_Colour.SelectedIndex = -1
            cbo_Filter_Item.SelectedIndex = -1

            dgv_filter.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Filter.BringToFront()
        pnl_Back.Enabled = False


        If dtp_FilterFrom_date.Enabled And dtp_FilterFrom_date.Visible Then dtp_FilterFrom_date.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW Ref NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Jari_Purchase_No from Jari_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Jari_Purchase_Code = '" & Trim(InvCode) & "'", con)
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
                    MessageBox.Show("Invalid Ref No.", "DOES NOT INSERT NEW Ref...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW BILL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub
    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim LedgerId As Integer
        Dim PurchaseAcId As Integer
        Dim ColorId As Integer
        Dim CountId As Integer

        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Jari_Purchase_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.JARI_Purchase_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Jari_Purchase_No").ToString

                msk_RefDate.Text = dt1.Rows(0).Item("Jari_Purchase_Date").ToString

                LedgerId = dt1.Rows(0).Item("Ledger_IdNo").ToString
                cbo_Ledger.Text = Common_Procedures.Ledger_IdNoToName(con, LedgerId)

                txt_BillNo.Text = dt1.Rows(0).Item("Bill_No").ToString

                PurchaseAcId = dt1.Rows(0).Item("PurchaseAc_IdNo").ToString()
                cbo_PurchaseAc.Text = Common_Procedures.Ledger_IdNoToName(con, PurchaseAcId)

                msk_BillDate.Text = dt1.Rows(0).Item("Bill_Date").ToString

                CountId = dt1.Rows(0).Item("Count_IdNo").ToString
                cbo_Count.Text = Common_Procedures.Count_IdNoToName(con, CountId)

                txt_Box_Rolls.Text = dt1.Rows(0).Item("Box_Rolls").ToString

                ColorId = dt1.Rows(0).Item("Colour_IdNo").ToString
                cbo_Color.Text = Common_Procedures.Colour_IdNoToName(con, ColorId)

                txt_Kgs.Text = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "#########0.000")

                txt_Rate.Text = Format(Val(dt1.Rows(0).Item("Rate_Per_Kg").ToString), "#########0.00")

                txt_Amount.Text = Format(Val(dt1.Rows(0).Item("Amount").ToString), "#########0.00")

                txt_DiscPerc.Text = Format(Val(dt1.Rows(0).Item("Discount_Percentage").ToString), "#########0.00")

                lbl_DiscAmount.Text = Format(Val(dt1.Rows(0).Item("Discount_Amount").ToString), "#########0.00")

                txt_AddLess.Text = Format(Val(dt1.Rows(0).Item("AddLess_BeforeTax_Amount").ToString), "#########0.00")

              
                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight_Amount").ToString), "#########0.00")

                lbl_NetAmount.Text = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "#########0.00")

                txt_Note.Text = dt1.Rows(0).Item("Note").ToString


               lbl_TaxableValue.Text = Format(Val(dt1.Rows(0).Item("Total_Taxable_Value").ToString), "########0.00")
                lbl_CGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_CGST_Amount").ToString), "########0.00")
                lbl_SGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_SGST_Amount").ToString), "########0.00")
                lbl_IGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_IGST_Amount").ToString), "########0.00")
                lbl_grid_GstPerc.Text = Format(Val(dt1.Rows(0).Item("GST_Percentage").ToString), "########0.00")
                lbl_Grid_HSNCode.Text = dt1.Rows(0).Item("HSN_Code").ToString
                cbo_Type.Text = dt1.Rows(0).Item("Entry_VAT_GST_Type").ToString
               
                lbl_RefNo.ForeColor = Color.Black


            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If msk_RefDate.Visible And msk_RefDate.Enabled Then msk_RefDate.Focus()
            'If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()
        End Try

        NoCalc_Status = False

        New_Entry = False
    End Sub
    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Jari_Purchase_No from Jari_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Jari_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Jari_Purchase_No", con)
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

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Jari_Purchase_No from Jari_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Jari_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Jari_Purchase_No desc", con)
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

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Jari_Purchase_No from Jari_Purchase_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Jari_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Jari_Purchase_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Jari_Purchase_No from Jari_Purchase_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Jari_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Jari_Purchase_No desc", con)
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
        Dim Dt2 As New DataTable

        Try
            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Jari_Purchase_Head", "Jari_Purchase_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_RefNo.ForeColor = Color.Red


            msk_RefDate.Text = Date.Today.ToShortDateString

            Dt1.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Dt2.Dispose()
            Da1.Dispose()

            If msk_RefDate.Enabled And msk_RefDate.Visible Then msk_RefDate.Focus()
            'If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
        End Try
    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RefCode As String

        Try

            inpno = InputBox("Enter Ref No.", "FOR FINDING...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Jari_Purchase_No from Jari_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Jari_Purchase_Code = '" & Trim(RefCode) & "'", con)
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
                MessageBox.Show("Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        Dim Led_ID As Integer = 0
        Dim PurAc_ID As Integer = 0
        Dim Count_Id As Integer = 0
        Dim Color_Id As Integer = 0
        Dim TxAc_ID As Integer = 0
        Dim Total_Amount As Double = 0
        Dim VouBil As String = ""
        Dim Agt_Idno As Integer = 0

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Jari_Purchase_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_RefDate.Text) = False Then
            MessageBox.Show("Invalid Purchase Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_RefDate.Enabled And msk_RefDate.Visible Then msk_RefDate.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_RefDate.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_RefDate.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Purchase Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_RefDate.Enabled And msk_RefDate.Visible Then msk_RefDate.Focus()
            Exit Sub
        End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Val(Led_ID) = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        If Trim(txt_BillNo.Text) = "" Then
            MessageBox.Show("Invalid Bill No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_BillNo.Enabled And txt_BillNo.Visible Then txt_BillNo.Focus()
            Exit Sub
        End If

        If IsDate(msk_BillDate.Text) = False Then
            msk_BillDate.Text = msk_RefDate.Text
        End If

        If IsDate(msk_BillDate.Text) = False Then
            MessageBox.Show("Invalid Bill Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_BillDate.Enabled And msk_BillDate.Visible Then msk_BillDate.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_BillDate.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_BillDate.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Bill Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_BillDate.Enabled And msk_BillDate.Visible Then msk_BillDate.Focus()
            Exit Sub
        End If


        '---------------------------------------------------------------------

        Color_Id = Common_Procedures.Colour_NameToIdNo(con, cbo_Color.Text)
        Count_Id = Common_Procedures.Count_NameToIdNo(con, cbo_Count.Text)
        Led_ID = Common_Procedures.Ledger_NameToIdNo(con, cbo_Ledger.Text)

        PurAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PurchaseAc.Text)
        'TxAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Tax_AC.Text)

        '------------------------------------------------------------------------------------


        If Trim(lbl_NetAmount.Text) = "" Then lbl_NetAmount.Text = "0.0"

        If PurAc_ID = 0 And Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            MessageBox.Show("Invalid Purchase A/c name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_PurchaseAc.Enabled And cbo_PurchaseAc.Visible Then cbo_PurchaseAc.Focus()
            Exit Sub
        End If


        'If TxAc_ID = 0 And Val(lbl_TaxAmount.Text) <> 0 Then
        '    MessageBox.Show("Invalid Tax A/c name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If cbo_Tax_AC.Enabled And cbo_Tax_AC.Visible Then cbo_Tax_AC.Focus()
        '    Exit Sub
        'End If

        If Val(lbl_CGST_Amount.Text) <> 0 And Val(lbl_SGST_Amount.Text) <> 0 And Val(lbl_IGST_Amount.Text) <> 0 And (Trim(cbo_Type.Text) = "" Or Trim(cbo_Type.Text) = "-NIL-") Then
            MessageBox.Show("Invalid Tax Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Type.Enabled And cbo_Type.Visible Then cbo_Type.Focus()
            Exit Sub
        End If

        If Trim(txt_BillNo.Text) <> "" Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            Da = New SqlClient.SqlDataAdapter("select * from Jari_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ledger_IdNo = " & Str(Val(Led_ID)) & " and Bill_No = '" & Trim(txt_BillNo.Text) & "' and Jari_Purchase_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Jari_Purchase_Code <> '" & Trim(NewCode) & "'", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                MessageBox.Show("Duplicate Bill No to this Party", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_BillNo.Enabled And txt_BillNo.Visible Then txt_BillNo.Focus()
                Exit Sub
            End If
            Dt1.Clear()
        End If

        'If Trim(txt_AddLess_AfterTax.Text) = "" Then txt_AddLess_AfterTax.Text = "Add/Less"


        Total_Amount = Val(txt_Rate.Text) * Val(txt_Kgs.Text)


        tr = con.BeginTransaction

        'Try

        If Insert_Entry = True Or New_Entry = False Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Else

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Jari_Purchase_Head", "Jari_Purchase_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        End If

        cmd.Connection = con
        cmd.Transaction = tr

        cmd.Parameters.Clear()


        cmd.Parameters.AddWithValue("@PurchaseDate", Convert.ToDateTime(msk_RefDate.Text))
        cmd.Parameters.AddWithValue("@BillDate", Convert.ToDateTime(msk_BillDate.Text))


        If New_Entry = True Then

            cmd.CommandText = "Insert into Jari_Purchase_Head ( Jari_Purchase_Code , Company_IdNo                     , for_OrderBy                                                             , Jari_Purchase_No               , Jari_Purchase_Date  , Ledger_IdNo             , Bill_No                       , PurchaseAc_IdNo           , Bill_Date , Count_IdNo                 , Box_Rolls                            , Colour_IdNo               , Total_Weight               ,                  Rate_Per_Kg                    , Amount                        , Discount_Percentage               , Discount_Amount                     , AddLess_BeforeTax_Amount         ,Freight_Amount                      , RoundOff_Amount      ,          Note                             ,           Total_Taxable_Value     ,                Total_CGST_Amount       ,                 Total_SGST_Amount           ,         Total_IGST_Amount              ,       Entry_VAT_GST_Type                  ,HSN_Code                      ,  GST_Percentage         ,Item_Group_id      ,       Net_Amount) " & _
                                               " Values (   '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ",  " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",  '" & Trim(lbl_RefNo.Text) & "',    @PurchaseDate    , " & Str(Val(Led_ID)) & ",'" & Trim(txt_BillNo.Text) & "', " & Str(Val(PurAc_ID)) & ", @BillDate ," & Str(Val(Count_Id)) & "  ," & Str(Val(txt_Box_Rolls.Text)) & "  ," & Str(Val(Color_Id)) & " ,  " & Str(Val(txt_Kgs.Text)) & ", " & Str(Val(txt_Rate.Text)) & "," & Str(Val(txt_Amount.Text)) & " ," & Str(Val(txt_DiscPerc.Text)) & ", " & Str(Val(lbl_DiscAmount.Text)) & ", " & Str(Val(txt_AddLess.Text)) & "," & Str(Val(txt_Freight.Text)) & ", " & Val(lbl_RoundOff.Text) & ", '" & LTrim(txt_Note.Text) & "'             ," & Str(Val(lbl_TaxableValue.Text)) & "," & Str(Val(lbl_CGST_Amount.Text)) & "," & Str(Val(lbl_SGST_Amount.Text)) & "," & Str(Val(lbl_IGST_Amount.Text)) & ",'" & Trim(cbo_Type.Text) & "','" & Trim(lbl_Grid_HSNCode.Text) & "'," & Str(Val(lbl_grid_GstPerc.Text)) & ", " & Val(ItemGrpID) & ",  " & Str(Val(CSng(lbl_NetAmount.Text))) & ") "
            cmd.ExecuteNonQuery()

            MessageBox.Show("Saved Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Else

            cmd.CommandText = "Update Jari_Purchase_Head set Jari_Purchase_Date = @PurchaseDate, Ledger_IdNo = " & Str(Val(Led_ID)) & ",  PurchaseAc_IdNo = " & Str(Val(PurAc_ID)) & ", Bill_No = '" & Trim(txt_BillNo.Text) & "',Count_IdNo=" & Str(Val(Count_Id)) & ", Box_Rolls=" & Str(Val(txt_Box_Rolls.Text)) & ",Colour_IdNo=" & Str(Val(Color_Id)) & "  ,Rate_Per_Kg=" & Str(Val(txt_Rate.Text)) & ",   Total_Weight = " & Str(Val(txt_Kgs.Text)) & ", Amount = " & Str(Val(txt_Amount.Text)) & ", Discount_Percentage = " & Str(Val(txt_DiscPerc.Text)) & ", Discount_Amount = '" & Str(Val(lbl_DiscAmount.Text)) & "', AddLess_BeforeTax_Amount = " & Str(Val(txt_AddLess.Text)) & ", Freight_Amount = " & Str(Val(txt_Freight.Text)) & ",  RoundOff_Amount = " & Str(Val(lbl_RoundOff.Text)) & ", Bill_Date = @BillDate,Total_Taxable_Value=" & Str(Val(lbl_TaxableValue.Text)) & ",Total_CGST_Amount=" & Str(Val(lbl_CGST_Amount.Text)) & ",Total_SGST_Amount=" & Str(Val(lbl_SGST_Amount.Text)) & ",Total_IGST_Amount=" & Str(Val(lbl_IGST_Amount.Text)) & ",Entry_VAT_GST_Type='" & Trim(cbo_Type.Text) & "',HSN_Code='" & Trim(lbl_Grid_HSNCode.Text) & "' , GST_Percentage=" & Str(Val(lbl_grid_GstPerc.Text)) & " , Item_Group_id =" & Val(ItemGrpID) & ",Net_Amount=" & Str(Val(CSng(lbl_NetAmount.Text))) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Jari_Purchase_Code = '" & Trim(NewCode) & "' "
            cmd.ExecuteNonQuery()

            MessageBox.Show("Updated Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        End If

        Dim vAssVal As String = ""
        Dim vVou_Amts As String = ""
        Dim vLed_IdNos As String = "", ErrMsg As String = ""
        Dim Bill_Details As String = ""

        vAssVal = Format(Val(txt_Amount.Text) - Val(lbl_DiscAmount.Text) + Val(txt_AddLess.Text) + Val(txt_Freight.Text), "##########0.00")

        vLed_IdNos = Led_ID & "|" & PurAc_ID & "|24|25|26|30"
        vVou_Amts = Val(CSng(lbl_NetAmount.Text)) & "|" & -1 * Val(vAssVal) & "|" & -1 * Val(lbl_CGST_Amount.Text) & "|" & -1 * Val(lbl_SGST_Amount.Text) & "|" & -1 * Val(lbl_IGST_Amount.Text) & "|" & -1 * Val(lbl_RoundOff.Text)

        If Common_Procedures.Voucher_Updation(con, "Jari.Purc", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_RefNo.Text), Convert.ToDateTime(msk_RefDate.Text), Bill_Details, vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.OE_Software) = False Then
            Throw New ApplicationException(ErrMsg)
        End If

        '-----Bill Posting
        VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), Convert.ToDateTime(msk_RefDate.Text), Led_ID, Trim(lbl_RefNo.Text), Agt_Idno, Val(CSng(lbl_NetAmount.Text)), "CR", Trim(Pk_Condition) & Trim(NewCode), tr, Common_Procedures.SoftwareTypes.OE_Software)
        If Trim(UCase(VouBil)) = "ERROR" Then
            Throw New ApplicationException("Error on Voucher Bill Posting")
        End If


        tr.Commit()
        If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
            If New_Entry = True Then
                new_record()
            Else
                move_record(lbl_RefNo.Text)
            End If
        Else
            move_record(lbl_RefNo.Text)
        End If

        'Catch ex As Exception
        '    tr.Rollback()
        '    MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'Finally

        Dt1.Dispose()
        Da.Dispose()
        cmd.Dispose()
        tr.Dispose()
        Dt1.Clear()

        If msk_RefDate.Enabled And msk_RefDate.Visible Then msk_RefDate.Focus()

        'End Try
    End Sub
    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Note.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                msk_RefDate.Focus()
                'dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Note.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                msk_RefDate.Focus()
            End If
        End If
    End Sub


    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub txt_Rate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Rate.TextChanged
        Amount_Calculation()
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Kgs_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Kgs.TextChanged
        Amount_Calculation()
        NetAmount_Calculation()
    End Sub


    Private Sub txt_DiscPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_DiscPerc.TextChanged
        lbl_DiscAmount.Text = (Val(txt_Amount.Text) / 100) * Val(txt_DiscPerc.Text)
        NetAmount_Calculation()
    End Sub





    Private Sub txt_TaxPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        NetAmount_Calculation()
    End Sub


    Private Sub txt_Freight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Freight.TextChanged
        NetAmount_Calculation()
    End Sub


    Private Sub txt_Amount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Amount.TextChanged
        NetAmount_Calculation()
    End Sub



    Private Sub txt_AddLess_BeforeTax_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_AddLess.TextChanged
        NetAmount_Calculation()
    End Sub



    Private Sub txt_AddLess_AfterTax_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        NetAmount_Calculation()
    End Sub

    Private Sub lbl_DiscAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_DiscAmount.TextChanged
        NetAmount_Calculation()
    End Sub


    Private Sub txt_Rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Kgs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Kgs.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Box_Rolls_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Box_Rolls.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub


    Private Sub txt_AddLess_AfterTax_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub
    Private Sub txt_AddLess_BeforeTax_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub
    Private Sub txt_Amount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Amount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub
    Private Sub txt_DiscPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DiscPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub cbo_Tax_AC_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PurchaseAc.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub txt_AssessableValue_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles lbl_TaxableValue.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub


    Private Sub dtp_BillDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_BillDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_BillDate.Text = Date.Today
        End If
    End Sub

    Private Sub dtp_RefDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_RefDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_RefDate.Text = Date.Today
        End If
    End Sub

    Private Sub dtp_RefDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_RefDate.TextChanged

        If IsDate(dtp_RefDate.Text) = True Then
            'lbl_Day.Text = Format(Convert.ToDateTime(dtp_Date.Text), "dddd").ToString
            msk_RefDate.Text = dtp_RefDate.Text
            msk_RefDate.SelectionStart = 0
        End If
    End Sub

    Private Sub dtp_BillDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_BillDate.TextChanged
        If IsDate(dtp_BillDate.Text) = True Then
            'lbl_Day.Text = Format(Convert.ToDateTime(dtp_Date.Text), "dddd").ToString
            msk_BillDate.Text = dtp_BillDate.Text
            msk_BillDate.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_BillDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_BillDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_BillDate.Text
            vmskSelStrt = msk_BillDate.SelectionStart
        End If
    End Sub

    Private Sub msk_BillDate_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_BillDate.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_BillDate.Text = Date.Today
            msk_BillDate.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_BillDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_BillDate.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_BillDate.Text = Date.Today
        'End If
        If e.KeyCode = 107 Then
            msk_BillDate.Text = DateAdd("D", 1, Convert.ToDateTime(msk_BillDate.Text))
        ElseIf e.KeyCode = 109 Then
            msk_BillDate.Text = DateAdd("D", -1, Convert.ToDateTime(msk_BillDate.Text))
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)

        End If
    End Sub

    Private Sub msk_BillDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_BillDate.LostFocus
        If IsDate(msk_BillDate.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_BillDate.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_BillDate.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_BillDate.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_BillDate.Text)) >= 2000 Then
                    dtp_BillDate.Value = Convert.ToDateTime(msk_BillDate.Text)
                End If
            End If

        End If
    End Sub

    Private Sub msk_RefDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_RefDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_RefDate.Text
            vmskSelStrt = msk_RefDate.SelectionStart
        End If

    End Sub

    Private Sub msk_RefDate_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_RefDate.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_RefDate.Text = Date.Today
            msk_RefDate.SelectionStart = 0

        End If
    End Sub

    Private Sub msk_RefDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_RefDate.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_RefDate.Text = Date.Today
        'End If
        If e.KeyCode = 107 Then
            msk_RefDate.Text = DateAdd("D", 1, Convert.ToDateTime(msk_RefDate.Text))
        ElseIf e.KeyCode = 109 Then
            msk_RefDate.Text = DateAdd("D", -1, Convert.ToDateTime(msk_RefDate.Text))
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)

        End If
    End Sub

    Private Sub msk_RefDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_RefDate.LostFocus
        If IsDate(msk_RefDate.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_RefDate.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_RefDate.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_RefDate.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_RefDate.Text)) >= 2000 Then
                    dtp_RefDate.Value = Convert.ToDateTime(msk_RefDate.Text)
                End If
            End If

        End If
    End Sub


    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer
        Dim Condition As String = ""
        Dim CountId As Integer
        Dim ColorId As Integer
        'Try

        Condition = ""
        Led_IdNo = 0
        CountId = 0
        ColorId = 0

        '==============================FROM DATE , TO DATE===============================================================
        If IsDate(dtp_FilterFrom_date.Value) = True And IsDate(dtp_FilterTo_date.Value) = True Then
            Condition = "a.Jari_Purchase_Date between '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "

        ElseIf IsDate(dtp_FilterFrom_date.Value) = True Then
            Condition = "a.Jari_Purchase_Date = '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' "

        ElseIf IsDate(dtp_FilterTo_date.Value) = True Then
            Condition = "a.Jari_Purchase_Date= '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
        End If

        '=================================ADD LEDGER AS CONDITION=================================================================

        If Trim(cbo_Filter_PartyName.Text) <> "" Then
            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
        End If

        If Val(Led_IdNo) <> 0 Then
            Condition = Condition & IIf(Trim(Condition) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(Led_IdNo)) & ")"
        End If
        '=================================ADD ITEM AS CONDTION=================================================================
        If Trim(cbo_Filter_Item.Text) <> "" Then
            CountId = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_Item.Text)
        End If
        If Val(CountId) <> 0 Then
            Condition = Condition & IIf(Trim(Condition) <> "", " and ", "") & "(a.Count_IdNo =" & Str(Val(CountId)) & ")"
        End If
        '==================================ADD COLOR AS CONDITION================================================================
        If Trim(cbo_Filter_Colour.Text) <> "" Then
            ColorId = Common_Procedures.Colour_NameToIdNo(con, cbo_Filter_Colour.Text)
        End If
        If Val(ColorId) <> 0 Then
            Condition = Condition & IIf(Trim(Condition) <> "", " and ", "") & "(a.Colour_IdNo = " & Str(Val(ColorId)) & ")"
        End If
        '==================================================================================================
        da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Jari_Purchase_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo   where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Jari_Purchase_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condition) <> "", " and ", "") & Condition & " Order by a.for_orderby, a.Jari_Purchase_No", con)
        dt2 = New DataTable
        da.Fill(dt2)

        dgv_filter.Rows.Clear()

        If dt2.Rows.Count > 0 Then

            For i = 0 To dt2.Rows.Count - 1

                n = dgv_filter.Rows.Add()

                dgv_filter.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Jari_Purchase_No").ToString
                dgv_filter.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Jari_Purchase_Date").ToString), "dd-MM-yyyy")
                dgv_filter.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString

                CountId = dt2.Rows(i).Item("Count_IdNo").ToString
                dgv_filter.Rows(n).Cells(3).Value = Common_Procedures.Count_IdNoToName(con, CountId)

                ColorId = dt2.Rows(i).Item("Colour_IdNo").ToString
                dgv_filter.Rows(n).Cells(4).Value = Common_Procedures.Colour_IdNoToName(con, ColorId)

                dgv_filter.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Box_Rolls").ToString
                dgv_filter.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Total_Weight").ToString
                dgv_filter.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Rate_Per_Kg").ToString
                dgv_filter.Rows(n).Cells(8).Value = dt2.Rows(i).Item("Amount").ToString
                dgv_filter.Rows(n).Cells(9).Value = dt2.Rows(i).Item("Net_Amount").ToString

            Next i

        End If

        dt2.Clear()
        dt2.Dispose()
        da.Dispose()

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

        If dgv_filter.Visible And dgv_filter.Enabled Then dgv_filter.Focus()
    End Sub

    Private Sub btn_Fliter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Fliter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub
   

    Private Sub cbo_Filter_Item_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Item.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Item_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Item.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Item, cbo_Filter_PartyName, cbo_Filter_Colour, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Item_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Item.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Item, cbo_Filter_Colour, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub
    '--------------------------------------------------------
    Private Sub cbo_Filter_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Colour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Colour.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Colour, cbo_Filter_Item, btn_Filter_Show, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Colour.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Colour, btn_Filter_Show, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")
    End Sub
    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_FilterTo_date, cbo_Filter_Item, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_Item, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    '=======================================================================================================
    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, msk_RefDate, txt_BillNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, txt_BillNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
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

    '=======================================================================================================
    Private Sub cbo_PurchaseAc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PurchaseAc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27)", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_PurchaseAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PurchaseAc.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PurchaseAc, msk_BillDate, cbo_Count, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PurchaseAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PurchaseAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PurchaseAc, cbo_Count, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27)", "(Ledger_IdNo = 0)")

    End Sub
    Private Sub cbo_PurchaseAc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PurchaseAc.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PurchaseAc.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    '=======================================================================================================
    Private Sub cbo_Count_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Count.GotFocus
        vCbo_ItmNm = Trim(cbo_Count.Text)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub

    Private Sub cbo_Count_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Count.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Count, cbo_PurchaseAc, cbo_Color, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub

    Private Sub cbo_Count_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Count.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Count, cbo_Color, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub
    Private Sub cbo_Count_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Count.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Count.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub
    '=======================================================================================================
    Private Sub cbo_Color_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Color.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")

    End Sub

    Private Sub cbo_Color_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Color.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Color, cbo_Count, cbo_Type, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")

    End Sub

    Private Sub cbo_Color_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Color.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Color, cbo_Type, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")

    End Sub
    Private Sub cbo_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub
    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, cbo_Color, txt_Box_Rolls, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, txt_Box_Rolls, "", "", "", "")
    End Sub
    Private Sub cbo_Color_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Color.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Color_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Color.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

   

    Private Sub dgv_filter_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_filter.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_filter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_filter.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub
    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_filter.CurrentRow.Cells(0).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno)
            pnl_Back.Enabled = True
            pnl_Filter.Visible = False
        End If

    End Sub

    Private Sub Get_State_Code(ByVal Ledger_IDno As Integer, ByRef Ledger_State_Code As String, ByRef Company_State_Code As String)

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Try

            da = New SqlClient.SqlDataAdapter("Select * from Ledger_Head a LEFT OUTER JOIN State_Head b ON a.Ledger_State_IdNo = b.State_IdNo where a.Ledger_IdNo = " & Str(Val(Ledger_IDno)), con)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0).Item("State_Code").ToString) = False Then
                    Ledger_State_Code = Trim(dt.Rows(0).Item("State_Code").ToString)
                End If

            End If
            dt.Clear()
            dt.Dispose()
            da.Dispose()

            da = New SqlClient.SqlDataAdapter("Select * from Company_Head a LEFT OUTER JOIN State_Head b ON a.Company_State_IdNo = b.State_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)), con)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0).Item("State_Code").ToString) = False Then
                    Company_State_Code = Trim(dt.Rows(0).Item("State_Code").ToString)
                End If
            End If
            dt.Clear()
            dt.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try
    End Sub

    Private Sub cbo_Ledger_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.SelectedIndexChanged
        Amount_Calculation()
        NetAmount_Calculation()
    End Sub

    Private Sub cbo_Count_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Count.SelectedIndexChanged
        Amount_Calculation()
        NetAmount_Calculation()
    End Sub
End Class