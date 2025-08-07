Public Class Bale_Opening
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)

    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False

    Private PkCondition_MOVING As String = ""
    Private Pk_Condition As String = ""
    Private PkCondition_OPENING As String = "OBALE-"
    Private Pk_ConditionOld As String = "OPENI-"
    Private PkCondition_ENTRY As String = "PACKE-"
    Private OpYrCode As String = ""
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String

    Private vEntryType As String = ""

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer

    Private Property dtp_Filter_Fromdate As Object

    Private SaveAll_STS As Boolean = False
    Private SaveAll_Status_To_Corr_PkCond_Pbm As Boolean = False
    Private LastNo As String = ""

    Public Sub New(ByVal EntryType As String)
        vEntryType = Trim(UCase(EntryType))
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_BaleNo.Text = ""
        lbl_BaleNo.ForeColor = Color.Black

        cbo_Quality.Text = ""
        cbo_ClothType.Text = Common_Procedures.ClothType_IdNoToName(con, 1)
        cbo_BaleBundle.Text = "BALE"
        cbo_PartyName_StockOf.Text = Common_Procedures.Ledger_IdNoToName(con, Val(Common_Procedures.CommonLedger.OwnSort_Ac))
        cbo_PartyName_OfferedTo.Text = ""
        cbo_Filter_ClothType.Text = ""
        cbo_Filter_Cloth.Text = ""
        cbo_Godown_StockIn.Text = Common_Procedures.Ledger_IdNoToName(con, Val(Common_Procedures.CommonLedger.Godown_Ac))

        txt_Folding.Text = "100"
        txt_Pcs.Text = ""
        txt_Meters.Text = ""

        If Filter_Status = False Then
            txt_Filter_Folding.Text = ""
            cbo_Filter_Cloth.Text = ""
            cbo_Filter_ClothType.Text = ""
            cbo_Filter_Cloth.SelectedIndex = -1
            cbo_Filter_ClothType.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        cbo_Quality.Enabled = True
        cbo_Quality.BackColor = Color.White

        cbo_ClothType.Enabled = True
        cbo_ClothType.BackColor = Color.White

        txt_Folding.Enabled = True
        txt_Folding.BackColor = Color.White

        txt_Pcs.Enabled = True
        txt_Pcs.BackColor = Color.White

        txt_Meters.Enabled = True
        txt_Meters.BackColor = Color.White

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

    Private Sub Cloth_Bale_Bundle_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName_StockOf.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName_StockOf.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName_OfferedTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName_OfferedTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Quality.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Quality.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ClothType.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTHTYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ClothType.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Bale_Opening_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Text = ""

        con.Open()

        Pk_Condition = ""
        PkCondition_MOVING = ""

        If Trim(UCase(vEntryType)) = Trim(UCase("ENTRY")) Then
            lbl_Heading.Text = "PACKING SLIP"
            Pk_Condition = PkCondition_ENTRY
            PkCondition_MOVING = PkCondition_ENTRY
            Me.BackColor = Color.LightSkyBlue
        Else
            lbl_Heading.Text = "BALE OPENING"
            Pk_Condition = PkCondition_OPENING
            PkCondition_MOVING = ""
        End If

        cbo_PartyName_StockOf.Visible = False
        lbl_PartyName_StockOf_Caption.Visible = False
        If Common_Procedures.settings.JOBWORKENTRY_Status = 1 Then
            cbo_PartyName_StockOf.Visible = True
            lbl_PartyName_StockOf_Caption.Visible = True
        End If


        cbo_Godown_StockIn.Visible = False
        lbl_Godown_StockIN_Caption.Visible = False
        If Common_Procedures.settings.Multi_Godown_Status = 1 Then
            cbo_Godown_StockIn.Visible = True
            lbl_Godown_StockIN_Caption.Visible = True

        End If


        cbo_PartyName_OfferedTo.Visible = False
        lbl_PartyName_OfferedTo_Caption.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then '---- Kalaimagal Textiles (Palladam)
            cbo_PartyName_OfferedTo.Visible = True
            lbl_PartyName_OfferedTo_Caption.Visible = True

        Else
            If Common_Procedures.settings.Multi_Godown_Status = 1 Then
                cbo_Godown_StockIn.Width = cbo_PartyName_StockOf.Width
            End If

        End If

        cbo_BaleBundle.Text = ""
        cbo_BaleBundle.Items.Add(" ")
        cbo_BaleBundle.Items.Add("BALE")
        cbo_BaleBundle.Items.Add("BUNDLE")
        cbo_BaleBundle.Items.Add("ROLL")

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2


        AddHandler cbo_Quality.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BaleBundle.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName_StockOf.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName_OfferedTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Godown_StockIn.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Folding.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Pcs.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Meters.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ClothType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Filter_Folding.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Quality.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BaleBundle.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName_StockOf.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName_OfferedTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Godown_StockIn.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Folding.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Pcs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Meters.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ClothType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Filter_Folding.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Folding.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Pcs.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Meters.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Filter_Folding.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Folding.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Pcs.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Filter_Folding.KeyPress, AddressOf TextBoxControlKeyPress


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
        OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Bale_Opening_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name()
    End Sub

    Private Sub Bale_Opening_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

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
                            lbl_Company.Tag = Val(dt1.Rows(0)(0).ToString)
                            lbl_Company.Text = Trim(dt1.Rows(0)(1).ToString)
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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim LockSTS As Boolean = False

        If Trim(no) = "" Then Exit Sub

        clear()

        NewCode = Trim(PkCondition_MOVING) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(OpYrCode)

        Try
            da1 = New SqlClient.SqlDataAdapter("select a.* , b.Ledger_Name from Packing_Slip_Head a  INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_BaleNo.Text = dt1.Rows(0).Item("Packing_Slip_No").ToString
                cbo_Quality.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
                cbo_ClothType.Text = Common_Procedures.ClothType_IdNoToName(con, Val(dt1.Rows(0).Item("ClothType_IdNo").ToString))
                cbo_BaleBundle.Text = dt1.Rows(0).Item("Bale_Bundle").ToString
                cbo_PartyName_StockOf.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_PartyName_OfferedTo.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Roll_Packing_Party_IdNo").ToString))
                txt_Folding.Text = Val(dt1.Rows(0).Item("Folding").ToString)
                txt_Pcs.Text = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                txt_Meters.Text = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "##########0.00")
                cbo_Godown_StockIn.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("WareHouse_IdNo").ToString))

                LockSTS = False
                If IsDBNull(dt1.Rows(0).Item("Delivery_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Delivery_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If

                If LockSTS = True Then
                    cbo_Quality.Enabled = False
                    cbo_Quality.BackColor = Color.LightGray

                    cbo_ClothType.Enabled = False
                    cbo_ClothType.BackColor = Color.LightGray

                    txt_Folding.Enabled = False
                    txt_Folding.BackColor = Color.LightGray

                    txt_Pcs.Enabled = False
                    txt_Pcs.BackColor = Color.LightGray

                    txt_Meters.Enabled = False
                    txt_Meters.BackColor = Color.LightGray

                End If

            Else
                new_record()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If cbo_BaleBundle.Visible And cbo_BaleBundle.Enabled Then cbo_BaleBundle.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Bale_OpeningStock, "~L~") = 0 And InStr(Common_Procedures.UR.Bale_OpeningStock, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Bale_Opening, New_Entry, Me) = False Then Exit Sub


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Pnl_Back.Enabled = False Then
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

        NewCode = Trim(PkCondition_MOVING) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BaleNo.Text) & "/" & Trim(OpYrCode)

        Da = New SqlClient.SqlDataAdapter("select count(*) from Packing_Slip_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code = '" & Trim(NewCode) & "' and Delivery_Code <> ''", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already this bale delivered", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
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

            cmd.CommandText = "delete from Packing_Slip_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code = '" & Trim(NewCode) & "'"
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

            If cbo_BaleBundle.Enabled = True And cbo_BaleBundle.Visible = True Then cbo_BaleBundle.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
            da.Fill(dt1)
            cbo_Filter_Cloth.DataSource = dt1
            cbo_Filter_Cloth.DisplayMember = "Cloth_Name"

            da = New SqlClient.SqlDataAdapter("select ClothType_Name from ClothType_Head order by ClothType_Name", con)
            da.Fill(dt2)
            cbo_Filter_ClothType.DataSource = dt2
            cbo_Filter_ClothType.DisplayMember = "ClothType_Name"

            txt_Filter_Folding.Text = ""
            cbo_Filter_Cloth.Text = ""
            cbo_Filter_ClothType.Text = ""


            cbo_Filter_Cloth.SelectedIndex = -1
            cbo_Filter_ClothType.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Filter.BringToFront()
        Pnl_Back.Enabled = False
        If cbo_Filter_Cloth.Enabled And cbo_Filter_Cloth.Visible Then cbo_Filter_Cloth.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            Dim vCONDT As String
            vCONDT = ""
            If Trim(PkCondition_MOVING) <> "" Then
                vCONDT = " and (Packing_Slip_Code LIKE '" & Trim(PkCondition_MOVING) & "%')"
            Else
                vCONDT = " and (Packing_Slip_Code NOT LIKE '" & Trim(PkCondition_ENTRY) & "%')"
            End If

            da = New SqlClient.SqlDataAdapter("select top 1 Packing_Slip_No from Packing_Slip_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code like '" & Trim(PkCondition_MOVING) & "%/" & Trim(OpYrCode) & "' " & vCONDT & " Order by for_Orderby, Packing_Slip_No", con)
            dt = New DataTable
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0
        Dim NewCode As String = ""
        Dim MtchSTS As Boolean = False
        Dim BalNo As String = ""
        Dim L As Integer = -1


        Try

            Dim vCONDT As String
            vCONDT = ""
            If Trim(PkCondition_MOVING) <> "" Then
                vCONDT = " and (Packing_Slip_Code LIKE '" & Trim(PkCondition_MOVING) & "%')"
            Else
                vCONDT = " and (Packing_Slip_Code NOT LIKE '" & Trim(PkCondition_ENTRY) & "%')"
            End If


            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_BaleNo.Text))

            BalNo = Val(lbl_BaleNo.Text)
            L = Len(Trim(BalNo))

            If Trim(UCase(BalNo)) <> Trim(UCase(lbl_BaleNo.Text)) And Len(Trim(BalNo)) <> (Len(Trim(lbl_BaleNo.Text)) + 1) Then
                NewCode = Trim(PkCondition_MOVING) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BaleNo.Text) & "/" & Trim(OpYrCode)
                da = New SqlClient.SqlDataAdapter("select * from Packing_Slip_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code like '" & Trim(PkCondition_MOVING) & "%/" & Trim(OpYrCode) & "'  " & vCONDT & " Order by Packing_Slip_Date, for_Orderby, Packing_Slip_No", con)
                dt = New DataTable
                da.Fill(dt)

                movno = ""
                MtchSTS = False
                If dt.Rows.Count > 0 Then
                    For i = 0 To dt.Rows.Count - 1
                        If MtchSTS = True Then
                            movno = dt.Rows(i).Item("Packing_Slip_No").ToString
                            Exit For

                        Else
                            If Trim(UCase(dt.Rows(i).Item("Packing_Slip_Code").ToString)) = Trim(UCase(NewCode)) Then
                                MtchSTS = True
                            End If

                        End If

                    Next

                End If

            Else

                da = New SqlClient.SqlDataAdapter("select top 1 Packing_Slip_No from Packing_Slip_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code like '" & Trim(PkCondition_MOVING) & "%/" & Trim(OpYrCode) & "'  " & vCONDT & " Order by for_Orderby, Packing_Slip_No", con)
                dt = New DataTable
                da.Fill(dt)

                movno = ""
                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                        movno = dt.Rows(0)(0).ToString
                    End If
                End If
            End If

            If Trim(movno) <> "" Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0
        Dim NewCode As String = ""
        Dim MtchSTS As Boolean = False
        Dim BalNo As String = ""
        Dim L As Integer = -1

        Try

            Dim vCONDT As String
            vCONDT = ""
            If Trim(PkCondition_MOVING) <> "" Then
                vCONDT = " and (Packing_Slip_Code LIKE '" & Trim(PkCondition_MOVING) & "%')"
            Else
                vCONDT = " and (Packing_Slip_Code NOT LIKE '" & Trim(PkCondition_ENTRY) & "%')"
            End If


            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_BaleNo.Text))

            BalNo = Val(lbl_BaleNo.Text)
            L = Len(Trim(BalNo))

            If Trim(UCase(BalNo)) <> Trim(UCase(lbl_BaleNo.Text)) And Len(Trim(BalNo)) <> (Len(Trim(lbl_BaleNo.Text)) + 1) Then

                NewCode = Trim(PkCondition_MOVING) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BaleNo.Text) & "/" & Trim(OpYrCode)
                da = New SqlClient.SqlDataAdapter("select * from Packing_Slip_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code like '" & Trim(PkCondition_MOVING) & "%/" & Trim(OpYrCode) & "'  " & vCONDT & " Order by Packing_Slip_Date desc, for_Orderby desc, Packing_Slip_No desc", con)
                dt = New DataTable
                da.Fill(dt)

                movno = ""
                MtchSTS = False
                If dt.Rows.Count > 0 Then
                    For i = 0 To dt.Rows.Count - 1
                        If MtchSTS = True Then
                            movno = dt.Rows(i).Item("Packing_Slip_No").ToString
                            Exit For

                        Else
                            If Trim(UCase(dt.Rows(i).Item("Packing_Slip_Code").ToString)) = Trim(UCase(NewCode)) Then
                                MtchSTS = True
                            End If

                        End If

                    Next

                End If



            Else

                da = New SqlClient.SqlDataAdapter("select top 1 Packing_Slip_No from Packing_Slip_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code like '" & Trim(PkCondition_MOVING) & "%/" & Trim(OpYrCode) & "'  " & vCONDT & " Order by for_Orderby desc, Packing_Slip_No desc", con)
                da.Fill(dt)

                movno = ""
                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                        movno = dt.Rows(0)(0).ToString
                    End If
                End If

            End If


            If Trim(movno) <> "" Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try

            Dim vCONDT As String
            vCONDT = ""
            If Trim(PkCondition_MOVING) <> "" Then
                vCONDT = " and (Packing_Slip_Code LIKE '" & Trim(PkCondition_MOVING) & "%')"
            Else
                vCONDT = " and (Packing_Slip_Code NOT LIKE '" & Trim(PkCondition_ENTRY) & "%')"
            End If


            da = New SqlClient.SqlDataAdapter("select top 1 Packing_Slip_No from Packing_Slip_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code like '" & Trim(PkCondition_MOVING) & "%/" & Trim(OpYrCode) & "'  " & vCONDT & " Order by for_Orderby desc, Packing_Slip_No desc", con)
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

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0
        Dim vCONDT As String = ""

        Try
            clear()

            New_Entry = True

            vCONDT = ""
            If Trim(PkCondition_MOVING) <> "" Then
                vCONDT = "(Packing_Slip_Code LIKE '" & Trim(PkCondition_MOVING) & "%')"
            Else
                vCONDT = "(Packing_Slip_Code NOT LIKE '" & Trim(PkCondition_ENTRY) & "%')"
            End If

            lbl_BaleNo.Text = Common_Procedures.get_MaxCode(con, "Packing_Slip_Head", "Packing_Slip_Code", "For_OrderBy", vCONDT, Val(lbl_Company.Tag), OpYrCode)

            lbl_BaleNo.ForeColor = Color.Red

            da = New SqlClient.SqlDataAdapter("select top 1 * from Packing_Slip_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code like '" & Trim(PkCondition_MOVING) & "%/" & Trim(OpYrCode) & "' Order by for_Orderby desc, Packing_Slip_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then

                cbo_PartyName_StockOf.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_Quality.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
                cbo_ClothType.Text = Common_Procedures.ClothType_IdNoToName(con, Val(dt1.Rows(0).Item("ClothType_IdNo").ToString))
                cbo_BaleBundle.Text = dt1.Rows(0).Item("Bale_Bundle").ToString

                txt_Folding.Text = Format(Val(dt1.Rows(0).Item("Folding").ToString), "########0.00")
                cbo_Godown_StockIn.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("WareHouse_IdNo").ToString))

            End If
            dt1.Clear()

            If cbo_BaleBundle.Enabled And cbo_BaleBundle.Visible Then cbo_BaleBundle.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        dt1.Dispose()
        da.Dispose()



    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Bale.No.", "FOR FINDING...")

            RecCode = Trim(PkCondition_MOVING) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(OpYrCode)

            Da = New SqlClient.SqlDataAdapter("select Packing_Slip_No from Packing_Slip_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code = '" & Trim(RecCode) & "'", con)
            Dt = New DataTable
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
                MessageBox.Show("Bale No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Pavu_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Bale No.", "FOR NEW RECEIPT INSERTION...")
            If InStr(1, inpno, "'") > 0 Or InStr(1, inpno, """") > 0 Then
                MessageBox.Show("Invalid Bale No - Does not accept special characters", "DOES NOT INSERT NEW Bale NO...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

            RecCode = Trim(PkCondition_MOVING) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(OpYrCode)

            Da = New SqlClient.SqlDataAdapter("select Packing_Slip_No from Packing_Slip_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code = '" & Trim(RecCode) & "'", con)
            Dt = New DataTable
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
                    MessageBox.Show("Invalid Bale No", "DOES NOT INSERT NEW BALE NO...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_BaleNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW BALE NO...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Cloty_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim Led_StkOf_IdNo As Integer = 0, Led_OfrTo_IdNo As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim YCnt_ID As Integer = 0
        Dim YMil_ID As Integer = 0
        Dim EntID As String = ""
        Dim OpDate As Date
        Dim stkof_idno As Integer = 0
        Dim Led_type As String = 0
        Dim vGdwn_IdNo As String = 0

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Piece_OpeningStock, New_Entry) = False Then Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Bale_Opening, New_Entry, Me) = False Then Exit Sub


        If Pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If IsDate(dtp_Date.Text) = False Then
        '    MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
        '    Exit Sub
        'End If

        'If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
        '    MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
        '    Exit Sub
        'End If

        Led_StkOf_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName_StockOf.Text)
        If cbo_PartyName_StockOf.Visible = True Then
            If Led_StkOf_IdNo = 0 Then
                MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_PartyName_StockOf.Enabled Then cbo_PartyName_StockOf.Focus()
                Exit Sub
            End If
        End If
        If Led_StkOf_IdNo = 0 Then Led_StkOf_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, Common_Procedures.CommonLedger.OwnSort_Ac)



        vGdwn_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Godown_StockIn.Text)
        If cbo_Godown_StockIn.Visible Then
            If Val(vGdwn_IdNo) = 0 Then
                MessageBox.Show("Select Godown Name?", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                cbo_Godown_StockIn.Focus()
                Exit Sub
            End If
        End If
        If vGdwn_IdNo = 0 Then vGdwn_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, Common_Procedures.CommonLedger.Godown_Ac)


        Led_OfrTo_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName_OfferedTo.Text)

        Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Quality.Text)
        If Clo_ID = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Quality.Enabled And cbo_Quality.Visible Then cbo_Quality.Focus()
            Exit Sub
        End If

        Cloty_ID = Common_Procedures.ClothType_NameToIdNo(con, cbo_ClothType.Text)
        If Cloty_ID = 0 Then
            MessageBox.Show("Invalid Cloth Type Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ClothType.Enabled And cbo_ClothType.Visible Then cbo_ClothType.Focus()
            Exit Sub
        End If

        tr = con.BeginTransaction

        Try

            OpDate = New DateTime(Val(Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)), 4, 1)
            'OpDate = CDate("01-04-" & Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4))
            OpDate = DateAdd(DateInterval.Day, -1, OpDate)

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(PkCondition_MOVING) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BaleNo.Text) & "/" & Trim(OpYrCode)

            Else
                Dim vCONDT As String
                vCONDT = ""
                If Trim(PkCondition_MOVING) <> "" Then
                    vCONDT = "(Packing_Slip_Code LIKE '" & Trim(PkCondition_MOVING) & "%')"
                Else
                    vCONDT = "(Packing_Slip_Code NOT LIKE '" & Trim(PkCondition_ENTRY) & "%')"
                End If

                lbl_BaleNo.Text = Common_Procedures.get_MaxCode(con, "Packing_Slip_Head", "Packing_Slip_Code", "For_OrderBy", vCONDT, Val(lbl_Company.Tag), OpYrCode, tr)

                NewCode = Trim(PkCondition_MOVING) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BaleNo.Text) & "/" & Trim(OpYrCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@OpDate", OpDate)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Packing_Slip_Head (  Packing_Slip_Code     ,                 Company_IdNo      ,             Packing_Slip_No     ,                           for_OrderBy                                    , Packing_Slip_Date ,             Ledger_IdNo          ,     Roll_Packing_Party_IdNo      ,             Cloth_IdNo   ,           ClothType_IdNo   ,                 Folding            ,               Bale_Bundle           ,           Total_Pcs            ,               Total_Meters        ,  WareHouse_IdNo      ) " & _
                                  "Values                        ('" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & " , '" & Trim(lbl_BaleNo.Text) & "' , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BaleNo.Text))) & " ,        @OpDate    , " & Str(Val(Led_StkOf_IdNo)) & " , " & Str(Val(Led_OfrTo_IdNo)) & " , " & Str(Val(Clo_ID)) & " , " & Str(Val(Cloty_ID)) & " , " & Str(Val(txt_Folding.Text)) & " , '" & Trim(cbo_BaleBundle.Text) & "' , " & Str(Val(txt_Pcs.Text)) & " , " & Str(Val(txt_Meters.Text)) & " ," & Trim(vGdwn_IdNo) & " ) "
                cmd.ExecuteNonQuery()

            Else

                If SaveAll_STS <> True Then
                    da = New SqlClient.SqlDataAdapter("select count(*) from Packing_Slip_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code = '" & Trim(NewCode) & "' and Delivery_Code <> ''", con)
                    da.SelectCommand.Transaction = tr
                    Dt1 = New DataTable
                    da.Fill(Dt1)
                    If Dt1.Rows.Count > 0 Then
                        If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                            If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                                Throw New ApplicationException("Already this bale delivered")
                                Exit Sub
                            End If
                        End If
                    End If
                    Dt1.Clear()
                End If

                cmd.CommandText = "Update Packing_Slip_Head set Packing_Slip_Date = @OpDate, Ledger_IdNo = " & Str(Val(Led_StkOf_IdNo)) & " , Roll_Packing_Party_IdNo = " & Str(Val(Led_OfrTo_IdNo)) & " , Cloth_IdNo = " & Str(Val(Clo_ID)) & ", ClothType_IdNo = " & Str(Val(Cloty_ID)) & ", Folding = " & Str(Val(txt_Folding.Text)) & ", Bale_Bundle = '" & Trim(cbo_BaleBundle.Text) & "', Total_Pcs = " & Str(Val(txt_Pcs.Text)) & ", Total_Meters = " & Str(Val(txt_Meters.Text)) & " ,WareHouse_IdNo = " & Val(vGdwn_IdNo) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If


            Led_type = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Val(Led_StkOf_IdNo) & ")", , tr)

            stkof_idno = 0
            If Trim(UCase(Led_type)) = "JOBWORKER" Then
                stkof_idno = Led_StkOf_IdNo
            Else
                stkof_idno = Val(Common_Procedures.CommonLedger.OwnSort_Ac)
            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_BaleNo.Text)

            Partcls = "Bale Opening : BaleNo. " & Trim(lbl_BaleNo.Text)
            Partcls = Trim(Partcls) & ",  Cloth : " & Trim(cbo_Quality.Text)

            PBlNo = Trim(lbl_BaleNo.Text)

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If SaveAll_Status_To_Corr_PkCond_Pbm = True Then
                cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_ConditionOld) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()
            End If

            If Val(txt_Meters.Text) <> 0 Then
                cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code    ,             Company_IdNo         ,              Reference_No      ,                               for_OrderBy                               , Reference_Date,         DeliveryTo_Idno     , ReceivedFrom_Idno,         Entry_ID     ,    Party_Bill_No     ,     Particulars        , Sl_No,          Cloth_Idno     ,                 Folding           , Meters_Type" & Trim(Val(Cloty_ID)) & ", UnChecked_Meters  , StockOff_IdNo ) " & _
                                            " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_BaleNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BaleNo.Text))) & ",     @OpDate   , " & Str(Val(vGdwn_IdNo)) & ",         0        , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   1  , " & Str(Val(Clo_ID)) & ", " & Str(Val(txt_Folding.Text)) & ",    " & Str(Val(txt_Meters.Text)) & "  ,          0        , " & Val(stkof_idno) & "  ) "
                cmd.ExecuteNonQuery()
            End If

            tr.Commit()

            Dt1.Dispose()
            da.Dispose()

            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_BaleNo.Text)
                End If
            Else
                move_record(lbl_BaleNo.Text)
            End If
        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            If cbo_BaleBundle.Enabled And cbo_BaleBundle.Visible Then cbo_BaleBundle.Focus()

        End Try

    End Sub

    Private Sub cbo_BaleBundle_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BaleBundle.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_BaleBundle_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BaleBundle.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BaleBundle, txt_Meters, Nothing, "", "", "", "")
        If e.KeyCode = 40 And cbo_BaleBundle.DroppedDown = False Or (e.Control = True And e.KeyCode = 40) Then
            If cbo_PartyName_StockOf.Visible = True Then
                cbo_PartyName_StockOf.Focus()
            ElseIf cbo_Godown_StockIn.Visible = True Then
                cbo_Godown_StockIn.Focus()
            ElseIf cbo_PartyName_OfferedTo.Visible = True Then
                cbo_PartyName_OfferedTo.Focus()
            Else
                cbo_Quality.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_BaleBundle_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BaleBundle.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BaleBundle, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            If cbo_PartyName_StockOf.Visible = True Then
                cbo_PartyName_StockOf.Focus()
            ElseIf cbo_Godown_StockIn.Visible = True Then
                cbo_Godown_StockIn.Focus()
            ElseIf cbo_PartyName_OfferedTo.Visible = True Then
                cbo_PartyName_OfferedTo.Focus()
            Else
                cbo_Quality.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Quality_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Quality.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")
    End Sub

    Private Sub cbo_Cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Quality.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Quality, Nothing, cbo_ClothType, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")
        If (e.KeyValue = 38 And cbo_Quality.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_PartyName_OfferedTo.Visible = True Then
                cbo_PartyName_OfferedTo.Focus()
            ElseIf cbo_Godown_StockIn.Visible = True Then
                cbo_Godown_StockIn.Focus()
            ElseIf cbo_PartyName_StockOf.Visible = True Then
                cbo_PartyName_StockOf.Focus()
            Else
                cbo_BaleBundle.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Quality.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Quality, cbo_ClothType, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")
    End Sub

    Private Sub cbo_Cloth_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Quality.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Quality.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_PartyName_StockOf_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName_StockOf.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'OWNSORT' or Ledger_Type = 'JOBWORKER' )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_StockOf_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName_StockOf.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName_StockOf, cbo_BaleBundle, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = 'OWNSORT' or Ledger_Type = 'JOBWORKER')", "(Ledger_idno = 0)")
        If (e.KeyCode = 40 And cbo_PartyName_StockOf.DroppedDown = False) Or (e.Control = True And e.KeyCode = 40) Then
            If cbo_Godown_StockIn.Visible = True Then
                cbo_Godown_StockIn.Focus()
            ElseIf cbo_PartyName_OfferedTo.Visible = True Then
                cbo_PartyName_OfferedTo.Focus()
            Else
                cbo_Quality.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_PartyName_StockOf_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName_StockOf.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName_StockOf, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'OWNSORT' or Ledger_Type = 'JOBWORKER' )", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If cbo_Godown_StockIn.Visible = True Then
                cbo_Godown_StockIn.Focus()
            ElseIf cbo_PartyName_OfferedTo.Visible = True Then
                cbo_PartyName_OfferedTo.Focus()
            Else
                cbo_Quality.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_PartyName_StockOf_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName_StockOf.KeyUp
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

    Private Sub cbo_ClothType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothType_Head", "ClothType_Name", "(ClothType_IdNo >= 1 or ClothType_IdNo <= 5)", "(ClothType_IdNo = 0)")
    End Sub

    Private Sub cbo_ClothType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothType, cbo_Quality, txt_Folding, "ClothType_Head", "ClothType_Name", "(ClothType_IdNo >= 1 or ClothType_IdNo <= 5)", "(ClothType_IdNo = 0)")
    End Sub

    Private Sub cbo_ClothType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothType, txt_Folding, "ClothType_Head", "ClothType_Name", "(ClothType_IdNo >= 1 or ClothType_IdNo <= 5)", "(ClothType_IdNo = 0)")
    End Sub


    Private Sub txt_Folding_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Folding.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        'If Asc(e.KeyChar) = 13 Then
        '    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
        '        save_record()
        '    End If
        'End If
    End Sub

    Private Sub txt_Pcs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Pcs.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Meters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Meters.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                cbo_BaleBundle.Focus()
            End If
        End If
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Clo_IdNo As Integer, Cloty_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Clo_IdNo = 0
            Cloty_IdNo = 0

            If Trim(cbo_Filter_Cloth.Text) <> "" Then
                Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Filter_Cloth.Text)
            End If
            If Trim(cbo_Filter_ClothType.Text) <> "" Then
                Cloty_IdNo = Common_Procedures.ClothType_NameToIdNo(con, cbo_Filter_ClothType.Text)
            End If

            If Val(Clo_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Cloth_IdNo = " & Str(Val(Clo_IdNo)) & " )"

            End If

            If Val(Cloty_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.ClothType_IdNo = " & Str(Val(Cloty_IdNo)) & " )"
            End If

            If Trim(txt_Filter_Folding.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Folding = '" & Trim(txt_Filter_Folding.Text) & "'"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, e.Cloth_Name , f.ClothType_Name from Packing_Slip_Head a inner join Cloth_head e on a.Cloth_idno = e.Cloth_idno Left Outer join ClothType_head f on a.ClothType_idno = f.ClothType_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Packing_Slip_Code like '" & Trim(PkCondition_MOVING) & "%/" & Trim(OpYrCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Packing_Slip_Date, a.for_orderby, a.Packing_Slip_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Packing_Slip_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("ClothType_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Folding").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Total_Pcs").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")

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

    Private Sub cbo_Filter_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Cloth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "Cloth_IdNo = 0")
    End Sub

    Private Sub cbo_Filter_Cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Cloth.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Cloth, Nothing, cbo_Filter_ClothType, "Cloth_Head", "Cloth_Name", "", "Cloth_IdNo = 0")
    End Sub

    Private Sub cbo_Filter_Cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Cloth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Cloth, cbo_Filter_ClothType, "Cloth_Head", "Cloth_Name", "", "Cloth_IdNo = 0")
    End Sub

    Private Sub cbo_Filter_ClothType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ClothType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothType_Head", "ClothType_Name", "(ClothType_IdNo >=1 and ClothType_IdNo <= 5)", "ClothType_IdNo = 0")
    End Sub

    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ClothType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ClothType, cbo_Filter_Cloth, txt_Filter_Folding, "ClothType_Head", "ClothType_Name", "(ClothType_IdNo >=1 and ClothType_IdNo <= 5)", "ClothType_IdNo = 0")
    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ClothType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ClothType, txt_Filter_Folding, "ClothType_Head", "ClothType_Name", "(ClothType_IdNo >=1 and ClothType_IdNo <= 5)", "ClothType_IdNo = 0")
    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        Try
            movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

            If Trim(movno) <> "" Then
                Filter_Status = True
                move_record(movno)
                Pnl_Back.Enabled = True
                pnl_Filter.Visible = False
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
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
        '-----
    End Sub

    Private Sub txt_Meters_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Meters.LostFocus
        txt_Meters.Text = Format(Val(txt_Meters.Text), "##########0.00")
    End Sub

    Private Sub btn_SaveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SaveAll.Click
        Dim g As New Password
        g.ShowDialog()

        If Trim(UCase(Common_Procedures.Password_Input)) <> "GOLDSAVEALL" Then
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

        LastNo = lbl_BaleNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_BaleNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            SaveAll_Status_To_Corr_PkCond_Pbm = False
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub

    Private Sub cbo_PartyName_OfferedTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName_OfferedTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' or AccountsGroup_IdNo IN (select tZ1.AccountsGroup_IdNo from AccountsGroup_Head tZ1 Where tZ1.Parent_Idno LIKE '%~10~4~%' or tZ1.Parent_Idno LIKE '%~14~11~%' ) or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_OfferedTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName_OfferedTo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName_OfferedTo, Nothing, cbo_Quality, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' or AccountsGroup_IdNo IN (select tZ1.AccountsGroup_IdNo from AccountsGroup_Head tZ1 Where tZ1.Parent_Idno LIKE '%~10~4~%' or tZ1.Parent_Idno LIKE '%~14~11~%' ) or Show_In_All_Entry = 1)", "(Ledger_idno = 0)")
        If e.KeyCode = 38 And cbo_Godown_StockIn.DroppedDown = False Or (e.Control = True And e.KeyCode = 38) Then
            If cbo_Godown_StockIn.Visible = True Then
                cbo_Godown_StockIn.Focus()
            Else
                cbo_PartyName_StockOf.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_PartyName_OfferedTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName_OfferedTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName_OfferedTo, cbo_Quality, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' or AccountsGroup_IdNo IN (select tZ1.AccountsGroup_IdNo from AccountsGroup_Head tZ1 Where tZ1.Parent_Idno LIKE '%~10~4~%' or tZ1.Parent_Idno LIKE '%~14~11~%' ) or Show_In_All_Entry = 1)", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_OfferedTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName_OfferedTo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyName_OfferedTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1

            f.Show()

        End If
    End Sub

    Private Sub cbo_Godown_StockIn_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Godown_StockIn.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Godown_StockIn_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Godown_StockIn.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Godown_StockIn, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' )", "(Ledger_IdNo = 0)")
        If (e.KeyValue = 38 And cbo_Godown_StockIn.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_PartyName_StockOf.Visible = True Then
                cbo_PartyName_StockOf.Focus()
            Else
                cbo_BaleBundle.Focus()
            End If
        End If
        If (e.KeyCode = 40 And cbo_Godown_StockIn.DroppedDown = False) Or (e.Control = True And e.KeyCode = 40) Then
            If cbo_PartyName_OfferedTo.Visible = True Then
                cbo_PartyName_OfferedTo.Focus()
            Else
                cbo_Quality.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Godown_StockIn_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Godown_StockIn.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Godown_StockIn, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN')", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If cbo_PartyName_OfferedTo.Visible = True Then
                cbo_PartyName_OfferedTo.Focus()
            Else
                cbo_Quality.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Godown_StockIn_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Godown_StockIn.KeyUp
        If e.KeyCode = 17 And e.Control = False Then
            Common_Procedures.MDI_LedType = "GODOWN"
            Dim F As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Godown_StockIn.Name
            Common_Procedures.Master_Return.Master_Type = Me.Name
            Common_Procedures.Master_Return.Return_Value = Me.Name

            F.MdiParent = MDIParent1
            F.Show()
        End If
    End Sub


End Class