
Public Class EB_Diesel_Consumption_Entry_Simple

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "EBDCS-"
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double

    Private WithEvents dgtxt_BobinDetails As New DataGridViewTextBoxEditingControl
    Private dgv_ActCtrlName As String = ""
    Private dgv_LevColNo As Integer

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    '  Private Property lbl_GensetAmountCost As Label

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False

        pnl_back.Enabled = True
        pnl_filter.Visible = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        dtp_date.Text = ""

        txt_OpeningKWH.Text = ""
        txt_ClosingKWH.Text = ""
        lbl_KWHUnits.Text = ""
        txt_OpeningKWAH.Text = ""
        txt_ClosingKWAH.Text = ""
        lbl_KWAHUnits.Text = ""
        txt_OpeningGenset.Text = ""
        txt_ClosingGenset.Text = ""
        lbl_GensetUnits.Text = ""
        txt_PowerFactor.Text = ""
        txt_RatePerEBUnit.Text = ""
        lbl_EbAmount.Text = ""
        txt_UKG.Text = ""
        txt_Diesel_Used.Text = ""
        txt_RatePerGensetUnit.Text = ""
        lbl_GensetAmount.Text = ""
        txt_EBCost_ForProduction.Text = ""
        txt_GSCost_ForProduction.Text = ""
        txt_Demand.Text = ""
        dgv_ActCtrlName = ""
        msk_Time.Text = ""
    End Sub


    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim maskedtextbox As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            MaskedTextBox = Me.ActiveControl
            maskedtextbox.SelectAll()
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

    Private Sub TextBoxControlKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        On Error Resume Next
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then SendKeys.Send("{TAB}")
    End Sub



    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim LockSTS As Boolean = False
        Dim Sno As Integer = 0
        Dim n As Integer = 0

        If Val(no) = 0 Then Exit Sub

        clear()

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

            da1 = New SqlClient.SqlDataAdapter("select a.* from EB_Diesel_Consumed_Head a  Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.EB_Diesel_Consumed_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("EB_Diesel_Consumed_No").ToString
                dtp_date.Text = dt1.Rows(0).Item("EB_Diesel_Consumed_Date").ToString
                msk_Time.Text = dt1.Rows(0).Item("EB_Diesel_Consumed_Time").ToString
                txt_OpeningKWH.Text = Format(Val(dt1.Rows(0).Item("KWH_OpeningReading").ToString), "########0.00")
                txt_ClosingKWH.Text = Format(Val(dt1.Rows(0).Item("KWH_ClosingReading").ToString), "########0.00")
                lbl_KWHUnits.Text = Format(Val(dt1.Rows(0).Item("KWH_Units").ToString), "########0.00")

                txt_OpeningKWAH.Text = Format(Val(dt1.Rows(0).Item("KWAH_OpeningReading").ToString), "########0.00")
                txt_ClosingKWAH.Text = Format(Val(dt1.Rows(0).Item("KWAH_ClosingReading").ToString), "########0.00")
                lbl_KWAHUnits.Text = Format(Val(dt1.Rows(0).Item("KWAH_Units").ToString), "########0.00")

                txt_OpeningGenset.Text = Format(Val(dt1.Rows(0).Item("Genset_OpeningReading").ToString), "########0.00")
                txt_ClosingGenset.Text = Format(Val(dt1.Rows(0).Item("Genset_ClosingReading").ToString), "########0.00")
                lbl_GensetUnits.Text = Format(Val(dt1.Rows(0).Item("Genset_Units").ToString), "########0.00")

                txt_Diesel_Used.Text = Format(Val(dt1.Rows(0).Item("Diesel_Consumed").ToString), "########0.00")
                txt_RatePerGensetUnit.Text = Format(Val(dt1.Rows(0).Item("Gen_Cost_Per_Unit").ToString), "########0.00")
                lbl_GensetAmount.Text = Format(Val(dt1.Rows(0).Item("Genset_Amount").ToString), "########0.00")

                txt_PowerFactor.Text = Format(Val(dt1.Rows(0).Item("Power_Factor").ToString), "########0.00")
                txt_RatePerEBUnit.Text = Format(Val(dt1.Rows(0).Item("EB_Cost_Per_Unit").ToString), "########0.00")
                lbl_EbAmount.Text = Format(Val(dt1.Rows(0).Item("EB_Amount").ToString), "########0.00")
                txt_UKG.Text = Format(Val(dt1.Rows(0).Item("UKG").ToString), "########0.00")

                txt_EBCost_ForProduction.Text = Format(Val(dt1.Rows(0).Item("EB_Cost_ForProduction").ToString), "########0.00")
                txt_GSCost_ForProduction.Text = Format(Val(dt1.Rows(0).Item("GS_Cost_ForProduction").ToString), "########0.00")

                txt_Demand.Text = Format(Val(dt1.Rows(0).Item("Demand_Normal_hours").ToString), "########0.00")

                Get_OpeningReading()
            End If

            Cost_And_Unit_Calculation()

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            da1.Dispose()
            dt1.Dispose()

            If dtp_date.Visible And dtp_date.Enabled Then dtp_date.Focus()

        End Try

    End Sub

    Private Sub EB_Diesel_Consumption_Entry_Simple_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try



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

    Private Sub EB_Diesel_Consumption_Entry_Simple_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Text = ""

        con.Open()



        pnl_filter.Visible = False
        pnl_filter.Left = (Me.Width - pnl_filter.Width) \ 2
        pnl_filter.Top = ((Me.Height - pnl_filter.Height) \ 2) + 20

        AddHandler dtp_date.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OpeningKWAH.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OpeningKWH.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OpeningGenset.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ClosingKWH.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ClosingKWAH.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ClosingGenset.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PowerFactor.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RatePerEBUnit.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Diesel_Used.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RatePerGensetUnit.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EBCost_ForProduction.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_GSCost_ForProduction.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_UKG.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Demand.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Time.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_date.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OpeningKWAH.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OpeningKWH.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OpeningGenset.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ClosingKWH.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ClosingKWAH.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ClosingGenset.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PowerFactor.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RatePerEBUnit.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Diesel_Used.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RatePerGensetUnit.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EBCost_ForProduction.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_GSCost_ForProduction.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_UKG.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Demand.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Time.LostFocus, AddressOf ControlLostFocus

        'AddHandler dtp_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterFrom_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterTo_date.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_OpeningKWAH.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_OpeningKWH.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_OpeningGenset.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_ClosingKWH.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_ClosingKWAH.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_ClosingGenset.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_PowerFactor.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_RatePerEBUnit.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler msk_Time.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Diesel_Used.KeyPress, AddressOf TextBoxControlKeyPress

        ' AddHandler dtp_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterFrom_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterTo_date.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_OpeningKWAH.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_OpeningKWH.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_OpeningGenset.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_ClosingKWH.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_ClosingKWAH.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_ClosingGenset.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_PowerFactor.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_RatePerEBUnit.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler msk_Time.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Diesel_Used.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        dgv_filter.RowTemplate.Height = 27

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub EB_Diesel_Consumption_Entry_Simple_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub EB_Diesel_Consumption_Entry_Simple_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try

            If Asc(e.KeyChar) = 27 Then

                If pnl_filter.Visible = True Then
                    btn_closefilter_Click(sender, e)
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
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim NewCode As String = ""
        Dim DelvSts As Integer = 0
        Dim Nr As Integer = 0
        Dim NoofKnotBmsInCD As Integer = 0
        Dim NoofKnotBmsInLom As Integer = 0
        Dim Lm_ID As Integer = 0

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Inhouse_Bobin_Production, "~L~") = 0 And InStr(Common_Procedures.UR.Inhouse_Bobin_Production, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        tr = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = tr



            cmd.CommandText = "delete from EB_Diesel_Consumed_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and EB_Diesel_Consumed_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()



            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_date.Enabled = True And dtp_date.Visible = True Then dtp_date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        If Filter_Status = False Then
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable


            dtp_FilterFrom_date.Text = Common_Procedures.Company_FromDate
            dtp_FilterTo_date.Text = Common_Procedures.Company_ToDate
            pnl_filter.Text = ""

            dgv_filter.Rows.Clear()

            da.Dispose()

        End If

        pnl_filter.Visible = True
        pnl_filter.Enabled = True
        pnl_filter.BringToFront()
        pnl_back.Enabled = False
        If dtp_FilterFrom_date.Enabled And dtp_FilterFrom_date.Visible Then dtp_FilterFrom_date.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Inhouse_Bobin_Production, "~L~") = 0 And InStr(Common_Procedures.UR.Inhouse_Bobin_Production, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Ref.No.", "FOR INSERTION...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select EB_Diesel_Consumed_No from EB_Diesel_Consumed_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and EB_Diesel_Consumed_Code = '" & Trim(NewCode) & "'"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Ref.No", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        Try

            cmd.Connection = con
            cmd.CommandText = "select top 1 EB_Diesel_Consumed_No from EB_Diesel_Consumed_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and EB_Diesel_Consumed_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, EB_Diesel_Consumed_No"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String

        Try
            cmd.Connection = con
            cmd.CommandText = "select top 1 EB_Diesel_Consumed_No from EB_Diesel_Consumed_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and EB_Diesel_Consumed_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, EB_Diesel_Consumed_No desc"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 EB_Diesel_Consumed_No from EB_Diesel_Consumed_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and EB_Diesel_Consumed_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, EB_Diesel_Consumed_No"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            cmd.Connection = con
            cmd.CommandText = "select top 1 EB_Diesel_Consumed_No from EB_Diesel_Consumed_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and  EB_Diesel_Consumed_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, EB_Diesel_Consumed_No desc"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()

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

            da = New SqlClient.SqlDataAdapter("select max(for_orderby) from EB_Diesel_Consumed_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and EB_Diesel_Consumed_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
            da.Fill(dt)

            NewID = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    NewID = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            NewID = NewID + 1

            lbl_RefNo.Text = NewID
            lbl_RefNo.ForeColor = Color.Red

            Get_OpeningReading()

            If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()

            If Val(lbl_RefNo.Text) = 1 Then
                txt_OpeningKWH.ReadOnly = False
                txt_OpeningKWAH.ReadOnly = False
                txt_OpeningGenset.ReadOnly = False
            Else
                txt_OpeningKWH.ReadOnly = True
                txt_OpeningKWAH.ReadOnly = True
                txt_OpeningGenset.ReadOnly = True
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As String, inpno As String
        Dim NewCode As String

        Try

            inpno = InputBox("Enter Ref.No", "FOR FINDING...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select EB_Diesel_Consumed_No from EB_Diesel_Consumed_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and EB_Diesel_Consumed_Code = '" & Trim(NewCode) & "'"
            dr = cmd.ExecuteReader

            movno = ""
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = dr(0).ToString
                    End If
                End If
            End If

            dr.Close()
            cmd.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                MessageBox.Show("Ref.No. Does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String = ""
        Dim nr As Long = 0
        Dim led_id As Integer = 0
        Dim Emp_ID As Integer = 0
        Dim Clr_ID As Integer = 0
        Dim Clo_ID3 As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim Cnt_ID As Integer = 0
        Dim Lm_ID As Integer = 0
        Dim Partcls As String = ""
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim NoofInpBmsInLom As Integer = 0
        Dim NoofKnotBmsInCD As Integer = 0
        Dim NoofKnotBmsInLom As Integer = 0

        Dim decnt_ID As Integer = 0
        Dim declr_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim vOrdByNo As Single = 0

        If pnl_back.Enabled = False Then
            MessageBox.Show("Close all other windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Inhouse_Bobin_Production, New_Entry) = False Then Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
        If Val(txt_ClosingKWH.Text) = 0 And Val(txt_ClosingKWAH.Text) = 0 And Val(txt_ClosingGenset.Text) = 0 Then
            MessageBox.Show("Invalid Reading Details", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_date.Enabled Then dtp_date.Focus()
            Exit Sub
        End If


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "EB_Diesel_Consumed_Head", "EB_Diesel_Consumed_Code", "for_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EBDate", dtp_date.Value.Date)

            vOrdByNo = Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into EB_Diesel_Consumed_Head (     EB_Diesel_Consumed_Code,                           Company_IdNo      ,                                              EB_Diesel_Consumed_No  ,                                                EB_Diesel_Consumed_Time ,                                                                for_OrderBy                  ,                     EB_Diesel_Consumed_Date ,                                    KWH_OpeningReading,                                           KWH_ClosingReading,                                     KWH_Units,                                       KWAH_OpeningReading ,                                  KWAH_ClosingReading,                                           KWAH_Units,                                Genset_OpeningReading,                        Genset_ClosingReading,                                 Genset_Units,                                  Power_Factor,                           EB_Cost_Per_Unit,                                      EB_Amount,                                   UKG,                            Diesel_Consumed,                                                 Gen_Cost_Per_Unit ,                                                             Genset_Amount,                                                 EB_Cost_ForProduction,                               GS_Cost_ForProduction    ,                         Demand_Normal_hours) " &
                                        "      Values                         ('" & Trim(NewCode) & "'              , " & Val(lbl_Company.Tag) & ",                                       '" & Trim(lbl_RefNo.Text) & "',                                           '" & Trim(msk_Time.Text) & "',                           " & Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)) & ",                         @EBDate             ,                      " & Val(txt_OpeningKWH.Text) & ",                           " & Val(txt_ClosingKWH.Text) & "  ,               " & Val(lbl_KWHUnits.Text) & " ,                          " & Val(txt_OpeningKWAH.Text) & ",                  " & Val(txt_ClosingKWAH.Text) & "  ,                     " & Val(lbl_KWAHUnits.Text) & " ,                 " & Val(txt_OpeningGenset.Text) & " ,          " & Val(txt_ClosingGenset.Text) & ",            " & Val(lbl_GensetUnits.Text) & ",             " & Val(txt_PowerFactor.Text) & ",        " & Val(txt_RatePerEBUnit.Text) & ",                " & Val(lbl_EbAmount.Text) & " ,            " & Val(txt_UKG.Text) & " ,          " & Val(txt_Diesel_Used.Text) & ",                            " & Val(txt_RatePerGensetUnit.Text) & ",                                        " & Val(lbl_GensetAmount.Text) & ",                            " & Val(txt_EBCost_ForProduction.Text) & ",              " & Val(txt_GSCost_ForProduction.Text) & ",                " & Val(txt_Demand.Text) & ") "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update EB_Diesel_Consumed_Head set EB_Diesel_Consumed_Date = @EBDate,   KWH_OpeningReading  = " & Val(txt_OpeningKWH.Text) & " ,  KWH_ClosingReading  = " & Val(txt_ClosingKWH.Text) & " , KWH_Units =" & Val(lbl_KWHUnits.Text) & " ,    KWAH_OpeningReading =" & Val(txt_OpeningKWAH.Text) & ", KWAH_ClosingReading = " & Val(txt_ClosingKWAH.Text) & ", EB_Diesel_Consumed_Time='" & Trim(msk_Time.Text) & "'  , KWAH_Units =" & Val(lbl_KWAHUnits.Text) & "  , Genset_OpeningReading  =" & Val(txt_OpeningGenset.Text) & " , Genset_ClosingReading =" & Val(txt_ClosingGenset.Text) & ", Genset_Units =" & Val(lbl_GensetUnits.Text) & " , Power_Factor =   " & Val(txt_PowerFactor.Text) & " ,EB_Cost_Per_Unit =" & Val(txt_RatePerEBUnit.Text) & "  ,EB_Amount  =" & Val(lbl_EbAmount.Text) & "  ,UKG =" & Val(txt_UKG.Text) & " , Diesel_Consumed =" & Val(txt_Diesel_Used.Text) & " ,Gen_Cost_Per_Unit =" & Val(txt_RatePerGensetUnit.Text) & " ,Genset_Amount =" & Val(lbl_GensetAmount.Text) & " ,EB_Cost_ForProduction=" & Val(txt_EBCost_ForProduction.Text) & "  ,GS_Cost_ForProduction =" & Val(txt_GSCost_ForProduction.Text) & ",Demand_Normal_hours = " & Val(txt_Demand.Text) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and EB_Diesel_Consumed_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If


            tr.Commit()

            move_record(lbl_RefNo.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

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
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt1.Dispose()
            da.Dispose()
            cmd.Dispose()
            tr.Dispose()

            If dtp_date.Enabled And dtp_date.Visible Then dtp_date.Focus()

        End Try

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '------ No Print
    End Sub



    Private Sub btn_filtershow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_filtershow.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer

        Dim Condt As String = ""

        Try

            Condt = ""


            If IsDate(dtp_FilterFrom_date.Value) = True And IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a.EB_Diesel_Consumed_Date between '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterFrom_date.Value) = True Then
                Condt = "a.EB_Diesel_Consumed_Date = '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a. EB_Diesel_Consumed_Date= '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            End If



            da = New SqlClient.SqlDataAdapter("select a.* from EB_Diesel_Consumed_Head a   where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.EB_Diesel_Consumed_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.EB_Diesel_Consumed_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_filter.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_filter.Rows.Add()

                    dgv_filter.Rows(n).Cells(0).Value = dt2.Rows(i).Item("EB_Diesel_Consumed_No").ToString
                    dgv_filter.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("EB_Diesel_Consumed_Date").ToString), "dd-MM-yyyy")
                    dgv_filter.Rows(n).Cells(2).Value = dt2.Rows(i).Item("KWH_Units").ToString
                    dgv_filter.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Genset_Units").ToString
                    dgv_filter.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("UKG").ToString), "########0.00")




                Next i

            End If

            dt2.Clear()
            dt2.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dgv_filter.Visible And dgv_filter.Enabled Then dgv_filter.Focus()

    End Sub




    Private Sub dgv_filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_filter.DoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub btn_filtershow_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles btn_filtershow.KeyPress
        If Asc(e.KeyChar) = 13 Then
            dtp_FilterTo_date.Focus()
        End If
    End Sub

    Private Sub btn_closefilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_Filter.Click
        pnl_back.Enabled = True
        pnl_filter.Visible = False
        Filter_Status = False
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

        movno = Trim(dgv_filter.CurrentRow.Cells(0).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno)
            pnl_back.Enabled = True
            pnl_filter.Visible = False
        End If

    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub txt_EBUnits_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ClosingKWH.KeyPress, txt_ClosingKWAH.KeyPress, txt_ClosingGenset.KeyPress, txt_PowerFactor.KeyPress, txt_RatePerEBUnit.KeyPress, txt_RatePerGensetUnit.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

    End Sub

    Private Sub Get_OpeningReading()
        Dim q As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim cmd As New SqlClient.SqlCommand


        cmd.Connection = con
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@CurrentDate", dtp_date.Value.Date)

        cmd.CommandText = "select KWH_ClosingReading ,KWAH_ClosingReading, Genset_ClosingReading , EB_Cost_Per_Unit  from EB_Diesel_Consumed_Head    where   EB_Diesel_Consumed_Date  < @CurrentDate  ORDER BY EB_Diesel_Consumed_Date DESC"
        Da1 = New SqlClient.SqlDataAdapter(cmd)
        Dt3 = New DataTable
        Da1.Fill(Dt3)


        If Dt3.Rows.Count > 0 Then

            If IsDBNull(Dt3.Rows(0).Item("KWH_ClosingReading").ToString) = False Then
                txt_OpeningKWH.Text = Val(Dt3.Rows(0).Item("KWH_ClosingReading").ToString)
            End If
            If IsDBNull(Dt3.Rows(0).Item("KWAH_ClosingReading").ToString) = False Then
                txt_OpeningKWAH.Text = Val(Dt3.Rows(0).Item("KWAH_ClosingReading").ToString)
            End If
            If IsDBNull(Dt3.Rows(0).Item("Genset_ClosingReading").ToString) = False Then
                txt_OpeningGenset.Text = Val(Dt3.Rows(0).Item("Genset_ClosingReading").ToString)
            End If
            If IsDBNull(Dt3.Rows(0).Item("EB_Cost_Per_Unit").ToString) = False Then
                txt_RatePerEBUnit.Text = Val(Dt3.Rows(0).Item("EB_Cost_Per_Unit").ToString)
            End If

            'Else
            '    txt_OpeningKWH.Text = 0
            '    txt_OpeningKWAH.Text = 0
            '    txt_OpeningGenset.Text = 0
            '    txt_RatePerEBUnit.Text = 0

        End If


        Dt3.Clear()
        Dt3.Dispose()
        cmd.Cancel()

    End Sub


    Private Sub dtp_date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_date.TextChanged
        Get_OpeningReading()
    End Sub

    Private Sub txt_ClosingGenset_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_OpeningKWH.TextChanged, txt_OpeningKWAH.TextChanged, txt_OpeningGenset.TextChanged, txt_ClosingKWH.TextChanged, txt_ClosingKWAH.TextChanged, txt_ClosingGenset.TextChanged, txt_RatePerEBUnit.TextChanged, txt_RatePerGensetUnit.TextChanged
        Cost_And_Unit_Calculation()
    End Sub

    Private Sub Cost_And_Unit_Calculation()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim ConeWinding_ProKG As Double = 0

        lbl_KWHUnits.Text = Format((Val(txt_ClosingKWH.Text) - Val(txt_OpeningKWH.Text)) * 40, "#######0.00")
        lbl_KWAHUnits.Text = Format((Val(txt_ClosingKWAH.Text) - Val(txt_OpeningKWAH.Text)) * 40, "########0.00")
        lbl_GensetUnits.Text = Format((Val(txt_ClosingGenset.Text) - Val(txt_OpeningGenset.Text)) * 4, "########0.00")

        If Val(lbl_KWAHUnits.Text) <> 0 Then
            txt_PowerFactor.Text = Format(Val(lbl_KWHUnits.Text) / Val(lbl_KWAHUnits.Text), "#####0.00")
        End If

        lbl_EbAmount.Text = Format(Val(txt_RatePerEBUnit.Text) * Val(lbl_KWHUnits.Text), "#########0.00")
        lbl_GensetAmount.Text = Format(Val(txt_RatePerGensetUnit.Text) * Val(lbl_GensetUnits.Text), "########0.00")

        'cmd.Connection = con
        'cmd.Parameters.Clear()
        'cmd.Parameters.AddWithValue("@CurrentDate", dtp_date.Value.Date)

        'cmd.CommandText = "select SUM(Total_Actual_Production) AS Production from Cone_Winding_Production_Head  where   Cone_Winding_Production_Date =  @CurrentDate  GROUP BY Cone_Winding_Production_Date "
        'Da1 = New SqlClient.SqlDataAdapter(cmd)
        'Dt3 = New DataTable
        'Da1.Fill(Dt3)
        'If Dt3.Rows.Count > 0 Then
        '    If IsDBNull(Dt3.Rows(0).Item("Production").ToString) = False Then
        '        ConeWinding_ProKG = Val(Dt3.Rows(0).Item("Production").ToString)
        '    End If
        'End If
        'Dt3.Clear()
        'Dt3.Dispose()
        'cmd.Cancel()

        'If ConeWinding_ProKG <> 0 Then
        '    lbl_UKG.Text = Format(Val(lbl_KWHUnits.Text) / Val(ConeWinding_ProKG), "######0.00")

        '    lbl_EBCost_ForProduction.Text = Format(Val(lbl_EbAmount.Text) / Val(ConeWinding_ProKG), "######0.00")

        '    lbl_GSCost_ForProduction.Text = Format(Val(lbl_GensetAmount.Text) / Val(ConeWinding_ProKG), "######0.00")
        'Else
        '    lbl_UKG.Text = 0

        '    lbl_EBCost_ForProduction.Text = 0

        '    lbl_GSCost_ForProduction.Text = 0
        'End If




    End Sub
    Private Sub dtp_date_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dtp_date.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            msk_Time.Focus()
        End If
    End Sub

    Private Sub dtp_date_KeyDown(sender As Object, e As KeyEventArgs) Handles dtp_date.KeyDown
        If e.KeyCode = 40 Then msk_Time.Focus()
    End Sub


    Private Sub msk_Time_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Time.KeyDown
        If e.KeyCode = 40 Then
            txt_ClosingKWH.Focus()
        End If
        If e.KeyValue = 38 Then
            dtp_date.Focus()
        End If
    End Sub

    Private Sub msk_Time_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Time.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_ClosingKWH.Focus()
        End If
    End Sub

    Private Sub txt_ClosingKWH_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_ClosingKWH.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_ClosingKWAH.Focus()
        End If
    End Sub

    Private Sub txt_ClosingKWH_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_ClosingKWH.KeyDown
        If e.KeyCode = 40 Then
            txt_ClosingKWAH.Focus()
        End If
        If e.KeyValue = 38 Then
            msk_Time.Focus()
        End If
    End Sub

    Private Sub txt_ClosingKWAH_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_ClosingKWAH.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_ClosingGenset.Focus()
        End If
    End Sub

    Private Sub txt_ClosingKWAH_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_ClosingKWAH.KeyDown
        If e.KeyCode = 40 Then
            txt_ClosingGenset.Focus()
        End If
        If e.KeyValue = 38 Then
            txt_ClosingKWH.Focus()
        End If
    End Sub

    Private Sub txt_ClosingGenset_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_ClosingGenset.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_PowerFactor.Focus()
        End If
    End Sub

    Private Sub txt_ClosingGenset_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_ClosingGenset.KeyDown
        If e.KeyCode = 40 Then
            txt_PowerFactor.Focus()
        End If
        If e.KeyValue = 38 Then
            txt_ClosingKWAH.Focus()
        End If
    End Sub

    Private Sub txt_PowerFactor_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_PowerFactor.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_RatePerEBUnit.Focus()
        End If
    End Sub

    Private Sub txt_PowerFactor_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_PowerFactor.KeyDown
        If e.KeyCode = 40 Then
            txt_RatePerEBUnit.Focus()
        End If
        If e.KeyValue = 38 Then
            txt_ClosingGenset.Focus()
        End If
    End Sub

    Private Sub txt_RatePerEBUnit_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_RatePerEBUnit.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_Diesel_Used.Focus()
        End If
    End Sub

    Private Sub txt_RatePerEBUnit_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_RatePerEBUnit.KeyDown
        If e.KeyCode = 40 Then
            txt_Diesel_Used.Focus()
        End If
        If e.KeyValue = 38 Then
            txt_PowerFactor.Focus()
        End If
    End Sub



    Private Sub txt_Diesel_Used_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Diesel_Used.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_RatePerGensetUnit.Focus()
        End If
    End Sub

    Private Sub txt_Diesel_Used_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Diesel_Used.KeyDown
        If e.KeyCode = 40 Then
            txt_RatePerGensetUnit.Focus()
        End If
        If e.KeyValue = 38 Then
            txt_RatePerEBUnit.Focus()
        End If
    End Sub

    Private Sub txt_RatePerGensetUnit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_RatePerGensetUnit.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_EBCost_ForProduction.Focus()
        End If
    End Sub

    Private Sub txt_RatePerGensetUnit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_RatePerGensetUnit.KeyDown
        If e.KeyCode = 40 Then
            txt_EBCost_ForProduction.Focus()
        End If
        If e.KeyValue = 38 Then
            txt_Diesel_Used.Focus()
        End If
    End Sub

    Private Sub txt_Demand_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Demand.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_date.Focus()
            End If

        End If
    End Sub

    Private Sub txt_Demand_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Demand.KeyDown
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_date.Focus()
            End If
        End If
        If e.KeyValue = 38 Then
            txt_RatePerGensetUnit.Focus()
        End If
    End Sub

    Private Sub txt_EBCost_ForProduction_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_EBCost_ForProduction.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_GSCost_ForProduction.Focus()
        End If
    End Sub

    Private Sub txt_EBCost_ForProduction_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_EBCost_ForProduction.KeyDown
        If e.KeyCode = 40 Then
            txt_GSCost_ForProduction.Focus()
        End If
        If e.KeyValue = 38 Then
            txt_RatePerGensetUnit.Focus()
        End If
    End Sub

    Private Sub txt_GSCost_ForProduction_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_GSCost_ForProduction.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_UKG.Focus()
        End If
    End Sub

    Private Sub txt_GSCost_ForProduction_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_GSCost_ForProduction.KeyDown
        If e.KeyCode = 40 Then
            txt_UKG.Focus()
        End If
        If e.KeyValue = 38 Then
            txt_EBCost_ForProduction.Focus()
        End If
    End Sub

    Private Sub txt_UKG_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_UKG.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_Demand.Focus()
        End If
    End Sub

    Private Sub txt_UKG_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_UKG.KeyDown
        If e.KeyCode = 40 Then
            txt_Demand.Focus()
        End If
        If e.KeyValue = 38 Then
            txt_GSCost_ForProduction.Focus()
        End If
    End Sub

    Private Sub txt_OpeningKWH_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_OpeningKWH.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_ClosingKWH.Focus()
        End If
    End Sub

    Private Sub txt_OpeningKWAH_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_OpeningKWAH.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_ClosingKWAH.Focus()
        End If
    End Sub

    Private Sub txt_OpeningGenset_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_OpeningGenset.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_ClosingGenset.Focus()
        End If
    End Sub
End Class