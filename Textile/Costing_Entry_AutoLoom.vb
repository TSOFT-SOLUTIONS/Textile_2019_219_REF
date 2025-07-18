Public Class Costing_Entry_AutoLoom
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False
        pnl_back.Enabled = True
        pnl_filter.Visible = False
        pnl_Selection.Visible = False


        vmskOldText = ""
        vmskSelStrt = -1
        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        dtp_date.Text = ""
        txt_Name.Text = ""
        txt_Ends.Text = ""
        txt_WarpCount.Text = ""
        txt_WeftCount.Text = ""
        txt_EndsINch.Text = ""
        txt_PickInch.Text = ""
        txt_TapeLength.Text = ""
        txt_Width.Text = ""
        txt_ReedSpace.Text = ""
        txt_WeftRateKg.Text = ""
        txt_WarpRateKg.Text = ""
        txt_WeavingChargePick.Text = ""
        txt_SizingChargePerKg.Text = ""
        txt_ProcessingChargeMtr.Text = ""
        txt_CrimpPerc.Text = ""
        txt_WeftWastePerc.Text = ""
        txt_OtherChargeMtr.Text = ""
        txt_CommPerc.Text = ""
        txt_ProfitPerc.Text = ""
        txt_ElongationPerc.Text = ""

        lbl_WarpyarnCostMtr.Text = ""
        lbl_WeftYarnCostMtr.Text = ""
        lbl_WeavingCost.Text = ""
        lbl_SizingCost.Text = ""
        lbl_Total_WarpCost.Text = ""
        lbl_ProcessingCost.Text = ""
        lbl_OtherCost.Text = ""

        txt_CommAmount.Text = ""
        txt_ProfitAmount.Text = ""
        lbl_CostMtr.Text = ""
        lbl_FabricWgtGrams.Text = ""
        lbl_WarpWgtinGrams.Text = ""
        lbl_WeftWgtinGrams.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
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
            msktxbx = Me.ActiveControl
            msktxbx.SelectionStart = 0
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
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

        If Not IsNothing(dgv_filter.CurrentCell) Then dgv_filter.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim LockSTS As Boolean = False

        If Val(no) = 0 Then Exit Sub

        clear()

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

            da1 = New SqlClient.SqlDataAdapter("select a.* from Costing_Head a  Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Ref_Code = '" & Trim(NewCode) & "'", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_RefNo.Text = dt1.Rows(0).Item("Ref_No").ToString

                dtp_Date.Text = dt1.Rows(0).Item("Ref_Date").ToString
                msk_Date.Text = dtp_Date.Text
                txt_Name.Text = dt1.Rows(0).Item("Ref_Name").ToString

                txt_Ends.Text = dt1.Rows(0).Item("Ends").ToString
                If Val(txt_Ends.Text) = 0 Then txt_Ends.Text = ""

                txt_WarpCount.Text = dt1.Rows(0).Item("Warp_Count").ToString
                If Val(txt_WarpCount.Text) = 0 Then txt_WarpCount.Text = ""

                txt_WeftCount.Text = dt1.Rows(0).Item("Weft_Count").ToString
                If Val(txt_WeftCount.Text) = 0 Then txt_WeftCount.Text = ""

                txt_EndsINch.Text = dt1.Rows(0).Item("EPI").ToString
                If Val(txt_EndsINch.Text) = 0 Then txt_EndsINch.Text = ""

                txt_PickInch.Text = dt1.Rows(0).Item("PPI").ToString
                If Val(txt_PickInch.Text) = 0 Then txt_PickInch.Text = ""

                txt_TapeLength.Text = dt1.Rows(0).Item("Tape_Length").ToString
                If Val(txt_TapeLength.Text) = 0 Then txt_TapeLength.Text = ""

                txt_Width.Text = dt1.Rows(0).Item("Width").ToString
                If Val(txt_Width.Text) = 0 Then txt_Width.Text = ""

                txt_ReedSpace.Text = Format(Val(dt1.Rows(0).Item("Reed_Space").ToString), "########0.00")
                If Val(txt_ReedSpace.Text) = 0 Then txt_ReedSpace.Text = ""

                txt_CrimpPerc.Text = Format(Val(dt1.Rows(0).Item("Crimp_Percentage").ToString), "########0.00")
                If Val(txt_CrimpPerc.Text) = 0 Then txt_CrimpPerc.Text = ""


                txt_ElongationPerc.Text = Format(Val(dt1.Rows(0).Item("Elongation_Percentage").ToString), "########0.00")
                If Val(txt_ElongationPerc.Text) = 0 Then txt_ElongationPerc.Text = ""

                txt_WeftWastePerc.Text = Format(Val(dt1.Rows(0).Item("Weft_Waste_Percentage").ToString), "########0.00")
                If Val(txt_WeftWastePerc.Text) = 0 Then txt_WeftWastePerc.Text = ""

                txt_WarpRateKg.Text = Format(Val(dt1.Rows(0).Item("Warp_Rate_Kg").ToString), " ########0.00")
                If Val(txt_WarpRateKg.Text) = 0 Then txt_WarpRateKg.Text = ""

                txt_WeftRateKg.Text = Format(Val(dt1.Rows(0).Item("Weft_Rate_Kg").ToString), "########0.00")
                If Val(txt_WeftRateKg.Text) = 0 Then txt_WeftRateKg.Text = ""

                txt_WeavingChargePick.Text = Format(Val(dt1.Rows(0).Item("Weaving_Charge").ToString), "########0.000")
                If Val(txt_WeavingChargePick.Text) = 0 Then txt_WeavingChargePick.Text = ""

                txt_SizingChargePerKg.Text = Format(Val(dt1.Rows(0).Item("Sizing_Charge").ToString), "########0.00")
                If Val(txt_SizingChargePerKg.Text) = 0 Then txt_SizingChargePerKg.Text = ""

                txt_ProcessingChargeMtr.Text = Format(Val(dt1.Rows(0).Item("Processing_Charge").ToString), "########0.00")
                If Val(txt_ProcessingChargeMtr.Text) = 0 Then txt_ProcessingChargeMtr.Text = ""

                txt_OtherChargeMtr.Text = Format(Val(dt1.Rows(0).Item("Other_Charge").ToString), "########0.00")
                If Val(txt_OtherChargeMtr.Text) = 0 Then txt_OtherChargeMtr.Text = ""

                lbl_WarpyarnCostMtr.Text = Format(Val(dt1.Rows(0).Item("Warp_Yarn_Cost").ToString), "########0.00")
                If Val(lbl_WarpyarnCostMtr.Text) = 0 Then lbl_WarpyarnCostMtr.Text = ""

                lbl_WeftYarnCostMtr.Text = Format(Val(dt1.Rows(0).Item("Weft_Yarn_Cost").ToString), "########0.00")
                If Val(lbl_WeftYarnCostMtr.Text) = 0 Then lbl_WeftYarnCostMtr.Text = ""

                lbl_WeavingCost.Text = Format(Val(dt1.Rows(0).Item("Weaving").ToString), "########0.00")
                If Val(lbl_WeavingCost.Text) = 0 Then lbl_WeavingCost.Text = ""

                lbl_ProcessingCost.Text = Format(Val(dt1.Rows(0).Item("Processing").ToString), "########0.00")
                If Val(lbl_ProcessingCost.Text) = 0 Then lbl_ProcessingCost.Text = ""

                lbl_OtherCost.Text = Format(Val(dt1.Rows(0).Item("Other_Cost").ToString), "########0.00")
                If Val(lbl_OtherCost.Text) = 0 Then lbl_OtherCost.Text = ""

                lbl_SizingCost.Text = Format(Val(dt1.Rows(0).Item("Sizing_Cost").ToString), "########0.00")
                If Val(lbl_SizingCost.Text) = 0 Then lbl_SizingCost.Text = ""

                lbl_Total_WarpCost.Text = Format(Val(dt1.Rows(0).Item("Total_Warp_Cost").ToString), "########0.00")
                If Val(lbl_Total_WarpCost.Text) = 0 Then lbl_Total_WarpCost.Text = ""

                txt_CommPerc.Text = Format(Val(dt1.Rows(0).Item("Commission_Percentage").ToString), "########0.00")
                If Val(txt_CommPerc.Text) = 0 Then txt_CommPerc.Text = ""

                txt_CommAmount.Text = Format(Val(dt1.Rows(0).Item("Commission_Amount").ToString), "########0.00")
                If Val(txt_CommAmount.Text) = 0 Then txt_CommAmount.Text = ""


                txt_ProfitPerc.Text = Format(Val(dt1.Rows(0).Item("Profit_Perc").ToString), "########0.00")
                If Val(txt_ProfitPerc.Text) = 0 Then txt_ProfitPerc.Text = ""


                txt_ProfitAmount.Text = Format(Val(dt1.Rows(0).Item("Warp_Wet_Cost_2_5").ToString), "########0.00")
                If Val(txt_ProfitAmount.Text) = 0 Then txt_ProfitAmount.Text = ""

                lbl_CostMtr.Text = Format(Val(dt1.Rows(0).Item("Cost_Meter").ToString), "########0.00")
                If Val(lbl_CostMtr.Text) = 0 Then lbl_CostMtr.Text = ""

                lbl_WarpWgtinGrams.Text = Val(dt1.Rows(0).Item("Warp_Weight").ToString)
                If Val(lbl_WarpWgtinGrams.Text) = 0 Then lbl_WarpWgtinGrams.Text = ""

                lbl_WeftWgtinGrams.Text = Val(dt1.Rows(0).Item("Weft_Weight").ToString)
                If Val(lbl_WeftWgtinGrams.Text) = 0 Then lbl_WeftWgtinGrams.Text = ""

                lbl_FabricWgtGrams.Text = Val(dt1.Rows(0).Item("Fabric_Weight").ToString)
                If Val(lbl_FabricWgtGrams.Text) = 0 Then lbl_FabricWgtGrams.Text = ""

                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

            End If

            dt1.Dispose()
            da1.Dispose()



        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Private Sub Costing_Entry_AutoLoom_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

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

    Private Sub Costing_Entry_AutoLoom_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim dt6 As New DataTable
        Me.Text = ""

        con.Open()

        pnl_filter.Visible = False
        pnl_filter.Left = (Me.Width - pnl_filter.Width) \ 2
        pnl_filter.Top = ((Me.Height - pnl_filter.Height) \ 2) + 20


        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = ((Me.Height - pnl_Selection.Height) \ 2) + 20

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Ends.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_WarpCount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_WeftCount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EndsINch.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PickInch.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TapeLength.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Width.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ReedSpace.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_WeftRateKg.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_WarpRateKg.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_WeavingChargePick.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SizingChargePerKg.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ProcessingChargeMtr.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CrimpPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_WeftWastePerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OtherChargeMtr.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CommPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CommAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ProfitPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ProfitAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ElongationPerc.GotFocus, AddressOf ControlGotFocus



        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Ends.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_WarpCount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_WeftCount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EndsINch.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PickInch.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TapeLength.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Width.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ReedSpace.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_WeftRateKg.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_WarpRateKg.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_WeavingChargePick.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SizingChargePerKg.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ProcessingChargeMtr.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CrimpPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_WeftWastePerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OtherChargeMtr.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CommPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CommAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ProfitPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ProfitAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ElongationPerc.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterFrom_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_FilterTo_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Ends.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_WarpCount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_WeftCount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_EndsINch.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PickInch.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TapeLength.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Width.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ReedSpace.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_WeftRateKg.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_WarpRateKg.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_WeavingChargePick.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SizingChargePerKg.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ProcessingChargeMtr.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ProfitPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ProfitAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CrimpPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_WeftWastePerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_OtherChargeMtr.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CommPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CommAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ElongationPerc.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterFrom_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_FilterTo_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Name.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Ends.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_WarpCount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_WeftCount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_EndsINch.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PickInch.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TapeLength.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Width.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ReedSpace.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_WeftRateKg.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_WarpRateKg.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_WeavingChargePick.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_SizingChargePerKg.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ProcessingChargeMtr.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_WeftWastePerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CrimpPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_OtherChargeMtr.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CommPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CommAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ElongationPerc.KeyPress, AddressOf TextBoxControlKeyPress


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        dgv_filter.RowTemplate.Height = 27

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub


    Private Sub Costing_Entry_AutoLoom_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Costing_Entry_AutoLoom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try

            If Asc(e.KeyChar) = 27 Then

                If pnl_filter.Visible = True Then
                    btn_closefilter_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
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
        Dim NewCode As String = ""
        Dim DelvSts As Integer = 0

        Dim vOrdByNo As String = ""
        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)


        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If



        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Costing_Entry, New_Entry, Me, con, "Costing_Head", "Ref_Code", NewCode, "Ref_Date", "(Ref_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


        tr = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = tr

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Costing_head", "Ref_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), "", "", "", New_Entry, True, "", "", "Ref_Code, Company_IdNo, for_OrderBy", tr)


            cmd.CommandText = "delete from Costing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ref_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            tr.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        If Filter_Status = False Then
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable



            da = New SqlClient.SqlDataAdapter("select Loom_Name from Loom_head order by Loom_Name", con)
            da.Fill(dt2)
            cbo_Filter_LoomNo.DataSource = dt2
            cbo_Filter_LoomNo.DisplayMember = "Loom_Name"


            dtp_FilterFrom_date.Text = ""
            dtp_FilterTo_date.Text = ""
            pnl_filter.Text = ""

            cbo_Filter_LoomNo.SelectedIndex = -1
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
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Costing_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Ref.No.", "FOR INSERTION...")

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.CommandText = "select Ref_No from Costing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ref_Code = '" & Trim(NewCode) & "'"
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
            cmd.CommandText = "select top 1 Ref_No from Costing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ref_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Ref_No"
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
            cmd.CommandText = "select top 1 Ref_No from Costing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ref_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Ref_No desc"
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
            cmd.CommandText = "select top 1 Ref_No from Costing_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ref_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Ref_No"
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
            cmd.CommandText = "select top 1 Ref_No from Costing_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and  Ref_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Ref_No desc"
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
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            da = New SqlClient.SqlDataAdapter("select max(for_orderby) from Costing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ref_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
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

            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Costing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ref_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Ref_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Ref_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("Ref_Date").ToString
                End If
            End If
            dt1.Clear()

            If msk_Date.Enabled And msk_Date.Visible Then
                msk_Date.Focus()
                msk_Date.SelectionStart = 0
            End If



        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
        dt1.Dispose()
        da.Dispose()
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
            cmd.CommandText = "select Ref_No from Costing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ref_Code = '" & Trim(NewCode) & "'"
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
        Dim NewCode As String = ""
        Dim NewNo As Long = 0
        Dim nr As Long = 0
        Dim led_id As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim Cnt_ID As Integer = 0
        Dim Lm_ID As Integer = 0
        Dim PavuConsMtrs As Single = 0

        Dim vOrdByNo As String = ""
        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Costing_Entry, New_Entry, Me, con, "Costing_Head", "Ref_Code", NewCode, "Ref_Date", "(Ref_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Ref_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Ref_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        If pnl_back.Enabled = False Then
            MessageBox.Show("Close all other windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If


        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If


        If Trim(txt_Name.Text) = "" Then
            MessageBox.Show("Invalid Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Name.Enabled Then txt_Name.Focus()
            Exit Sub
        End If

        lbl_UserName.Text = Common_Procedures.User.IdNo

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Costing_Head", "Ref_Code", "for_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@CostDate", dtp_Date.Value.Date)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Costing_Head (           Ref_Code    ,             Company_IdNo    ,            Ref_No             ,                           for_OrderBy                             ,       Ref_Date      ,          Ref_Name            ,              Warp_Count         ,                Weft_Count      ,                   EPI        ,           PPI                ,              Tape_Length        ,            Width          ,        Reed_Space               ,              Warp_Rate_Kg          ,              Weft_Rate_Kg       ,       Weaving_Charge                  ,            Sizing_Charge               ,              Processing_Charge            ,         Warp_Yarn_Cost                ,       Weft_Yarn_Cost                  ,           Weaving                 ,      Processing                       ,  Profit_Perc                ,  Warp_Wet_Cost_2_5          ,            Cost_Meter          ,                  Fabric_Weight     ,    Warp_Weight                       ,     Weft_Weight                    ,          Crimp_Percentage      ,          Weft_Waste_Percentage     ,            Other_Charge             ,            Other_Cost          ,       Commission_Percentage   ,            Commission_Amount    ,              user_idno                 ,              Sizing_Cost                       ,        Elongation_Percentage           ,               Ends                   ,             Total_Warp_Cost          )  " &
                                        "      Values       ('" & Trim(NewCode) & "', " & Val(lbl_Company.Tag) & ", '" & Trim(lbl_RefNo.Text) & "', " & Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)) & ",        @CostDate    , '" & Trim(txt_Name.Text) & "',  " & Val(txt_WarpCount.Text) & ", " & Val(txt_WeftCount.Text) & "," & Val(txt_EndsINch.Text) & "," & Val(txt_PickInch.Text) & ", " & Val(txt_TapeLength.Text) & "," & Val(txt_Width.Text) & "," & Val(txt_ReedSpace.Text) & "  ,   " & Val(txt_WarpRateKg.Text) & "  ," & Val(txt_WeftRateKg.Text) & "," & Val(txt_WeavingChargePick.Text) & ", " & Val(txt_SizingChargePerKg.Text) & "  , " & Val(txt_ProcessingChargeMtr.Text) & " ," & Val(lbl_WarpyarnCostMtr.Text) & "  , " & Val(lbl_WeftYarnCostMtr.Text) & " , " & Val(lbl_WeavingCost.Text) & " , " & Val(lbl_ProcessingCost.Text) & "  , " & Val(txt_ProfitPerc.Text) & " ," & Val(txt_ProfitAmount.Text) & " , " & Val(lbl_CostMtr.Text) & " ," & Val(lbl_FabricWgtGrams.Text) & ", " & Val(lbl_WarpWgtinGrams.Text) & " ," & Val(lbl_WeftWgtinGrams.Text) & ", " & Val(txt_CrimpPerc.Text) & ", " & Val(txt_WeftWastePerc.Text) & ", " & Val(txt_OtherChargeMtr.Text) & ", " & Val(lbl_OtherCost.Text) & ", " & Val(txt_CommPerc.Text) & ", " & Val(txt_CommAmount.Text) & ", " & Val(Common_Procedures.User.IdNo) & " ," & Val(lbl_SizingCost.Text) & "    ,   " & Val(txt_ElongationPerc.Text) & " ,   " & Str(Val(txt_Ends.Text)) & "  , " & Val(lbl_Total_WarpCost.Text) & " )  "
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Costing_head", "Ref_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), "", "", "", New_Entry, False, "", "", "Ref_Code, Company_IdNo, for_OrderBy", tr)

                cmd.CommandText = "Update Costing_Head set Ref_Date = @CostDate, Ref_Name = '" & Trim(txt_Name.Text) & "',Warp_Count = " & Str(Val(txt_WarpCount.Text)) & ",  Weft_Count = " & Val(txt_WeftCount.Text) & ",  EPI = " & Val(txt_EndsINch.Text) & ", PPI = " & Val(txt_PickInch.Text) & ",Tape_Length = " & Str(Val(txt_TapeLength.Text)) & ", Width = " & Val(txt_Width.Text) & ", Reed_Space = " & Val(txt_ReedSpace.Text) & ", Warp_Rate_Kg  = " & Str(Val(txt_WarpRateKg.Text)) & " ,Weft_Rate_Kg  = " & Val(txt_WeftRateKg.Text) & " , Weaving_Charge = " & Val(txt_WeavingChargePick.Text) & "  , Sizing_Charge = " & Val(txt_SizingChargePerKg.Text) & ", Processing_Charge = " & Val(txt_ProcessingChargeMtr.Text) & ", Warp_Yarn_Cost = " & Val(lbl_WarpyarnCostMtr.Text) & " ,Weft_Yarn_Cost = " & Val(lbl_WeftYarnCostMtr.Text) & " , Weaving = " & Val(lbl_WeavingCost.Text) & " , Processing = " & Val(lbl_ProcessingCost.Text) & " ,Profit_Perc  = " & Val(txt_ProfitPerc.Text) & " ,Warp_Wet_Cost_2_5 = " & Val(txt_ProfitAmount.Text) & " ,Cost_Meter = " & Val(lbl_CostMtr.Text) & " ,Fabric_Weight = " & Val(lbl_FabricWgtGrams.Text) & " ,Warp_Weight = " & Val(lbl_WarpWgtinGrams.Text) & " ,Weft_Weight = " & Val(lbl_WeftWgtinGrams.Text) & ", Crimp_Percentage = " & Val(txt_CrimpPerc.Text) & ", Weft_Waste_Percentage = " & Val(txt_WeftWastePerc.Text) & ", Other_Charge = " & Val(txt_OtherChargeMtr.Text) & ", Other_Cost = " & Val(lbl_OtherCost.Text) & ", Commission_Percentage = " & Val(txt_CommPerc.Text) & ", Commission_Amount = " & Val(txt_CommAmount.Text) & ", User_idno = " & Val(Common_Procedures.User.IdNo) & ", Sizing_Cost = " & Val(lbl_SizingCost.Text) & "  ,  Elongation_Percentage = " & Val(txt_ElongationPerc.Text) & "  , Ends = " & Str(Val(txt_Ends.Text)) & " , Total_Warp_Cost = " & Val(lbl_Total_WarpCost.Text) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Ref_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Costing_head", "Ref_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), "", "", "", New_Entry, False, "", "", "Ref_Code, Company_IdNo, for_OrderBy", tr)

            tr.Commit()

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

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub







    Private Sub btn_filtershow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_filtershow.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Clt_IdNo As Integer, Lom_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Clt_IdNo = 0
            Lom_IdNo = 0

            If IsDate(dtp_FilterFrom_date.Value) = True And IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a.Ref_Date between '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterFrom_date.Value) = True Then
                Condt = "a.Ref_Date = '" & Trim(Format(dtp_FilterFrom_date.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_FilterTo_date.Value) = True Then
                Condt = "a. Ref_Date= '" & Trim(Format(dtp_FilterTo_date.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_LoomNo.Text) <> "" Then
                Lom_IdNo = Common_Procedures.Loom_NameToIdNo(con, cbo_Filter_LoomNo.Text)
            End If
            If Val(Lom_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Loom_Idno = " & Str(Val(Lom_IdNo)) & ")"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*,   d.Loom_Name from Costing_Head a INNER JOIN Loom_Head d ON a.Loom_IdNo = d.Loom_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Ref_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Ref_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_filter.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_filter.Rows.Add()

                    dgv_filter.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Ref_No").ToString
                    dgv_filter.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Ref_Date").ToString), "dd-MM-yyyy")
                    dgv_filter.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Loom_Name").ToString
                    dgv_filter.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Set_Code1").ToString
                    dgv_filter.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Set_Code2").ToString
                    dgv_filter.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Beam_No1").ToString
                    dgv_filter.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Beam_No2").ToString



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

    Private Sub cbo_Filter_LoomNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_LoomNo.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_LoomNo, dtp_FilterTo_date, btn_filtershow, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_LoomNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_LoomNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_LoomNo, btn_filtershow, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0 )")
        'If Asc(e.KeyChar) = 13 Then
        '    If Trim(UCase(cbo_LoomNo.Text)) = "" Then
        '        If MessageBox.Show("Do you want to select  :", "FOR  SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
        '            btn_Selection_Click(sender, e)
        '        Else
        '            lbl_KnotNo.Focus()
        '        End If

        '    Else
        '        lbl_KnotNo.Focus()

        '    End If

        'End If

    End Sub


    Private Sub dgv_filter_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_filter.DoubleClick
        Open_FilterEntry()
    End Sub


    Private Sub btn_filtershow_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles btn_filtershow.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Filter_LoomNo.Focus()
        End If
    End Sub


    Private Sub btn_closefilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
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


    Private Sub btn_save_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub txt_ProcessingChargeMtr_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ProcessingChargeMtr.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

    End Sub

    Private Sub txt_EndsINch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EndsINch.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_PickInch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PickInch.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_ReedSpace_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ReedSpace.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_SizingChargeMtr_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SizingChargePerKg.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_TapeLength_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TapeLength.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_WarpCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_WarpCount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_WarpRateKg_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_WarpRateKg.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_WeavingChargePick_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_WeavingChargePick.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_WeftCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_WeftCount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_WeftRateKg_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_WeftRateKg.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Width_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Width.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub Costing_Calculation()
        Dim WarpYrnWgt As Single = 0
        Dim WeftYrnWgt As Single = 0
        Dim WarpYrnCostMtr As Single = 0
        Dim WeftYrnCostMtr As Single = 0
        Dim TotCostMtr As Single = 0
        Dim CrmpMtrWgt As Single = 0
        Dim WeftWstWgt As Single = 0
        Dim TapLen As Single = 0
        Dim ElongationMtrWgt As Single = 0
        Dim vENDS As String = 0

        If FrmLdSTS = True Then Exit Sub

        TapLen = Val(txt_TapeLength.Text)
        If Val(TapLen) = 0 Then TapLen = 39.37

        'WEIGHT METER FOR FABRIC IN GRAMS
        If Val(txt_WarpCount.Text) <> 0 And Val(txt_WeftCount.Text) <> 0 And Val(txt_Width.Text) <> 0 And (Val(txt_Ends.Text) <> 0 Or Val(txt_EndsINch.Text) <> 0) And Val(txt_ReedSpace.Text) <> 0 And Val(txt_PickInch.Text) <> 0 Then

            vENDS = Val(txt_Ends.Text)
            If Val(vENDS) = 0 Then
                vENDS = Val(txt_Width.Text) * Val(txt_EndsINch.Text)
            End If

            WarpYrnWgt = Format((Val(TapLen) * Val(vENDS)) / 66600 / Val(txt_WarpCount.Text), " ########0.000")
            ''WarpYrnWgt = Format(Val((Val(txt_EndsINch.Text) / Val(txt_WarpCount.Text))) * 0.68 * Val(txt_Width.Text), "#########0")
            'WarpYrnWgt = Format(Val((Val(TapLen) * (Val(txt_Width.Text) * Val(txt_EndsINch.Text))) / 66600) / Val(txt_WarpCount.Text), " ########0.000")

            CrmpMtrWgt = WarpYrnWgt * Val(txt_CrimpPerc.Text) / 100
            'lbl_WarpWgtinGrams.Text = Format((WarpYrnWgt + CrmpMtrWgt) * 1000, " #########0")

            ElongationMtrWgt = WarpYrnWgt * Val(txt_ElongationPerc.Text) / 100

            lbl_WarpWgtinGrams.Text = Format((WarpYrnWgt + CrmpMtrWgt - ElongationMtrWgt) * 1000, "#########0")

            WeftYrnWgt = Format(Val(1 / (1848 * Val(txt_WeftCount.Text) / Val(txt_ReedSpace.Text) / Val(txt_PickInch.Text) / 1.09367)), " ########0.000")
            WeftWstWgt = Val(WeftYrnWgt) * Val(txt_WeftWastePerc.Text) / 100
            lbl_WeftWgtinGrams.Text = Format((Val(WeftYrnWgt) + WeftWstWgt) * 1000, " #########0")

            lbl_FabricWgtGrams.Text = Format(Val(lbl_WarpWgtinGrams.Text) + Val(lbl_WeftWgtinGrams.Text), "#########0")

            'lbl_FabricWgtGrams.Text = Format(Val((Val(txt_EndsINch.Text) / Val(txt_WarpCount.Text)) + (Val(txt_PickInch.Text) / Val(txt_WeftCount.Text))) * 0.68 * Val(txt_Width.Text), "#########0")
            'lbl_WarpWgtinGrams.Text = Format(Val((Val(txt_EndsINch.Text) / Val(txt_WarpCount.Text))) * 0.68 * Val(txt_Width.Text), "#########0")
            'lbl_WeftWgtinGrams.Text = Format(Val((Val(txt_PickInch.Text) / Val(txt_WeftCount.Text))) * 0.68 * Val(txt_Width.Text), "#########0")

        Else

            lbl_FabricWgtGrams.Text = ""
            lbl_WarpWgtinGrams.Text = ""
            lbl_WeftWgtinGrams.Text = ""

        End If

        'COST/METER FOR WARP YARN
        If Val(txt_WarpCount.Text) <> 0 Then
            lbl_WarpyarnCostMtr.Text = Format(Format(Val(lbl_WarpWgtinGrams.Text) / 1000, "#########0.000") * Val(txt_WarpRateKg.Text), "#########0.00")
            lbl_SizingCost.Text = Format(Format(Val(lbl_WarpWgtinGrams.Text) / 1000, "#########0.000") * Val(txt_SizingChargePerKg.Text), "#########0.00")
            lbl_Total_WarpCost.Text = Format(Val(lbl_WarpyarnCostMtr.Text) + Val(lbl_SizingCost.Text), "#########0.00")

            'lbl_WarpyarnCostMtr.Text = Format(Format(Val(lbl_WarpWgtinGrams.Text) / 1000, "#########0.000") * (Val(txt_WarpRateKg.Text) + Val(txt_SizingChargeMtr.Text)), "#########0.00")
            'WarpYrnCostMtr = Format(Val((Val(TapLen) * (Val(txt_Width.Text) * Val(txt_EndsINch.Text))) / 66600) / Val(txt_WarpCount.Text), "#########0.000")
            'CrmpMtr = Val(WarpYrnCostMtr) * Val(txt_CrimpPerc.Text) / 100
            'lbl_WarpyarnCostMtr.Text = Format(Format(Val(WarpYrnCostMtr) + CrmpMtr, " ########0.000") * (Val(txt_WarpRateKg.Text) + Val(txt_SizingChargeMtr.Text)), "#########0.00")
            ''WarpYrnCostMtr = Format(Val(((Val(txt_WarpRateKg.Text) + Val(txt_SizingChargeMtr.Text)) * Val(TapLen) * (Val(txt_Width.Text) * Val(txt_EndsINch.Text))) / 66600) / Val(txt_WarpCount.Text), "#########0.000")
        Else
            lbl_WarpyarnCostMtr.Text = "0.00"

        End If

        'COST/METER FOR WEFT YARN
        If Val(txt_ReedSpace.Text) <> 0 And Val(txt_PickInch.Text) <> 0 And Val(txt_WeftCount.Text) <> 0 Then
            lbl_WeftYarnCostMtr.Text = Format(Format(Val(lbl_WeftWgtinGrams.Text) / 1000, " ########0.000") * Val(txt_WeftRateKg.Text), " ########0.00")
            'WeftYrnCostMtr = Format(Val(1 / (1848 * Val(txt_WeftCount.Text) / Val(txt_ReedSpace.Text) / Val(txt_PickInch.Text) / 1.09367)), " ########0.000")
            'WeftWst = Val(WeftYrnCostMtr) * Val(txt_WeftWastePerc.Text) / 100
            'lbl_WeftYarnCostMtr.Text = Format(Format((Val(WeftYrnCostMtr) + WeftWst), " ########0.000") * Val(txt_WeftRateKg.Text), " ########0.00")

        Else
            lbl_WeftYarnCostMtr.Text = "0.00"

        End If

        'Processing
        lbl_ProcessingCost.Text = Format(Val(txt_ProcessingChargeMtr.Text), "########0.00")
        If Val(lbl_ProcessingCost.Text) = 0 Then lbl_ProcessingCost.Text = ""
        'Weaving
        lbl_WeavingCost.Text = Format(Val(txt_WeavingChargePick.Text) * Val(txt_PickInch.Text), "########0.00")
        If Val(lbl_WeavingCost.Text) = 0 Then lbl_WeavingCost.Text = ""
        'Others
        lbl_OtherCost.Text = Format(Val(txt_OtherChargeMtr.Text), "########0.00")
        If Val(lbl_OtherCost.Text) = 0 Then lbl_OtherCost.Text = ""

        TotCostMtr = Total_Cost_Calculation()
        'TotCostMtr = Val(lbl_WarpyarnCostMtr.Text) + Val(lbl_WeftYarnCostMtr.Text) + Val(lbl_SizingCost.Text) + Val(lbl_WeavingCost.Text) + Val(lbl_ProcessingCost.Text) + Val(lbl_OtherCost.Text)

        'Agent Commission
        'txt_CommAmount.Text = Format(Val(TotCostMtr) * Val(txt_CommPerc.Text) / 100, "#########0.00")
        'txt_CommAmount.Text = Format((Val(lbl_WarpyarnCostMtr.Text) + Val(lbl_WeftYarnCostMtr.Text) + Val(lbl_Weaving.Text) + Val(txt_ProcessingChargeMtr.Text) + Val(txt_OtherChargeMtr.Text)) * Val(txt_CommPerc.Text) / 100, "#########0.00")

        'profit % 
        'txt_ProfitAmount.Text = Format(Val(TotCostMtr) * Val(txt_ProfitPerc.Text) / 100, "#########0.00")
        'lbl_Profit.Text = Format((Val(lbl_WarpyarnCostMtr.Text) + Val(lbl_WeftYarnCostMtr.Text) + Val(lbl_Weaving.Text) + Val(txt_ProcessingChargeMtr.Text) + Val(txt_OtherChargeMtr.Text)) * Val(txt_Profit.Text) / 100, "#########0.00")
        ''lbl_Profit.Text = Format((Val(lbl_WeftYarnCostMtr.Text) + Val(lbl_WarpyarnCostMtr.Text)) * Val(txt_Profit.Text) / 100, "#########0.00")

        'TOTAL COST METER
        lbl_CostMtr.Text = Format(Val(TotCostMtr) + Val(txt_ProfitAmount.Text) + Val(txt_CommAmount.Text), " ########0.00")
        'lbl_CostMtr.Text = Format(Val(lbl_WarpyarnCostMtr.Text) + Val(lbl_WeftYarnCostMtr.Text) + Val(lbl_WeavingCost.Text) + Val(txt_ProcessingChargeMtr.Text) + Val(txt_OtherChargeMtr.Text) + Val(lbl_Profit.Text) + Val(txt_CommAmount.Text), " ########0.00")

    End Sub

    Private Function Total_Cost_Calculation() As String
        Dim TotCostMtr As String
        TotCostMtr = Val(lbl_WarpyarnCostMtr.Text) + Val(lbl_WeftYarnCostMtr.Text) + Val(lbl_SizingCost.Text) + Val(lbl_WeavingCost.Text) + Val(lbl_ProcessingCost.Text) + Val(lbl_OtherCost.Text)
        Return Trim(Format(Val(TotCostMtr), "#########0.00"))
    End Function

    Private Sub Costing_Calculation_Old()
        'COST/METER FOR WARP YARN
        If Val(txt_WarpCount.Text) <> 0 Then
            lbl_WarpyarnCostMtr.Text = Format(Val(((Val(txt_WarpRateKg.Text) + Val(txt_SizingChargePerKg.Text)) * Val(txt_TapeLength.Text) * (Val(txt_Width.Text) * Val(txt_EndsINch.Text))) / 66600) / Val(txt_WarpCount.Text), " ########0.00")
        Else
            lbl_WarpyarnCostMtr.Text = "0.00"
        End If

        'COST/METER FOR WEFT YARN
        If Val(txt_ReedSpace.Text) <> 0 And Val(txt_PickInch.Text) <> 0 And Val(txt_WeftCount.Text) <> 0 Then
            lbl_WeftYarnCostMtr.Text = Format(Val(1 / (1848 * Val(txt_WeftCount.Text) / Val(txt_ReedSpace.Text) / Val(txt_PickInch.Text) / 1.09367)) * Val(txt_WeftRateKg.Text), " ########0.00")
        Else
            lbl_WeftYarnCostMtr.Text = "0.00"
        End If

        'Processing
        lbl_ProcessingCost.Text = Format(Val(txt_ProcessingChargeMtr.Text), "########0.00")
        'Weaving
        lbl_WeavingCost.Text = Format(Val(txt_WeavingChargePick.Text), "########0.00")

        'profit % of warpcost+weftcost
        txt_ProfitAmount.Text = Format((Val(lbl_WeftYarnCostMtr.Text) + Val(lbl_WarpyarnCostMtr.Text)) * Val(txt_ProfitPerc.Text) / 100, "#########0.00")

        'TOTAL COST METER
        lbl_CostMtr.Text = Format(Val(lbl_WarpyarnCostMtr.Text) + Val(lbl_WeftYarnCostMtr.Text) + Val(lbl_WeavingCost.Text) + Val(txt_ProcessingChargeMtr.Text) + Val(txt_ProfitAmount.Text), " ########0.00")

        'WEIGHT METER FOR FABRIC IN GRAMS
        If Val(txt_WarpCount.Text) <> 0 And Val(txt_WeftCount.Text) <> 0 And Val(txt_Width.Text) <> 0 Then
            lbl_FabricWgtGrams.Text = Format(Val((Val(txt_EndsINch.Text) / Val(txt_WarpCount.Text)) + (Val(txt_PickInch.Text) / Val(txt_WeftCount.Text))) * 0.68 * Val(txt_Width.Text), "#########0")
            lbl_WarpWgtinGrams.Text = Format(Val((Val(txt_EndsINch.Text) / Val(txt_WarpCount.Text))) * 0.68 * Val(txt_Width.Text), "#########0")
            lbl_WeftWgtinGrams.Text = Format(Val((Val(txt_PickInch.Text) / Val(txt_WeftCount.Text))) * 0.68 * Val(txt_Width.Text), "#########0")

        Else
            lbl_FabricWgtGrams.Text = ""
            lbl_WarpWgtinGrams.Text = ""
            lbl_WeftWgtinGrams.Text = ""

        End If

    End Sub

    Private Sub txt_EndsINch_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_EndsINch.TextChanged
        Costing_Calculation()
    End Sub

    Private Sub txt_PickInch_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_PickInch.TextChanged
        Costing_Calculation()
    End Sub

    Private Sub txt_ProcessingChargeMtr_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_ProcessingChargeMtr.TextChanged
        Costing_Calculation()
    End Sub

    Private Sub txt_ReedSpace_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_ReedSpace.TextChanged
        Costing_Calculation()
    End Sub

    Private Sub txt_SizingChargeMtr_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_SizingChargePerKg.TextChanged
        Costing_Calculation()
    End Sub

    Private Sub txt_TapeLength_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TapeLength.TextChanged
        Costing_Calculation()
    End Sub

    Private Sub txt_WarpCount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_WarpCount.TextChanged
        Costing_Calculation()
    End Sub

    Private Sub txt_WarpRateKg_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_WarpRateKg.TextChanged
        Costing_Calculation()
    End Sub

    Private Sub txt_WeavingChargePick_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_WeavingChargePick.TextChanged
        Costing_Calculation()
    End Sub

    Private Sub txt_WeftCount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_WeftCount.TextChanged
        Costing_Calculation()
    End Sub

    Private Sub txt_WeftRateKg_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_WeftRateKg.TextChanged
        Costing_Calculation()
    End Sub

    Private Sub txt_Width_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Width.TextChanged
        Costing_Calculation()
    End Sub

    Private Sub lbl_WarpyarnCostMtr_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_WarpyarnCostMtr.TextChanged
        'Costing_Calculation()
    End Sub

    Private Sub lbl_WeftYarnCostMtr_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_WeftYarnCostMtr.TextChanged
        'Costing_Calculation()
    End Sub

    Private Sub lbl_Weaving_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_WeavingCost.TextChanged
        'Costing_Calculation()
    End Sub

    Private Sub lbl_Processing_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_ProcessingCost.TextChanged
        'Costing_Calculation()
    End Sub

    Private Sub lbl_Profit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Costing_Calculation()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        pnl_Selection.Visible = True
        pnl_back.Enabled = False
        If btn_close.Enabled Then btn_close.Focus()
    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        pnl_back.Enabled = True
        pnl_Selection.Visible = False
    End Sub

    Private Sub txt_CrimpPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CrimpPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_CrimpPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_CrimpPerc.TextChanged
        Costing_Calculation()
    End Sub

    Private Sub txt_WeftWastePerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_WeftWastePerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_WeftWastePerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_WeftWastePerc.TextChanged
        Costing_Calculation()
    End Sub

    Private Sub txt_OtherChargeMtr_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_OtherChargeMtr.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_OtherChargeMtr_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_OtherChargeMtr.TextChanged
        Costing_Calculation()
    End Sub

    Private Sub txt_CommPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CommPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_CommPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_CommPerc.TextChanged
        Dim TotCostMtr As String = 0
        TotCostMtr = Total_Cost_Calculation()
        'TotCostMtr = Format(Val(lbl_WarpyarnCostMtr.Text) + Val(lbl_WeftYarnCostMtr.Text) + Val(lbl_SizingCost.Text) + Val(lbl_WeavingCost.Text) + Val(lbl_ProcessingCost.Text) + Val(lbl_OtherCost.Text), "##########0.00")
        txt_CommAmount.Text = Format(Val(TotCostMtr) * Val(txt_CommPerc.Text) / 100, "#########0.00")
        Costing_Calculation()
    End Sub

    Private Sub txt_ProfitPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_ProfitPerc.TextChanged
        Dim TotCostMtr As String = 0
        TotCostMtr = Total_Cost_Calculation()
        'TotCostMtr = Format(Val(lbl_WarpyarnCostMtr.Text) + Val(lbl_WeftYarnCostMtr.Text) + Val(lbl_SizingCost.Text) + Val(lbl_WeavingCost.Text) + Val(lbl_ProcessingCost.Text) + Val(lbl_OtherCost.Text), "##########0.00")
        txt_ProfitAmount.Text = Format(Val(TotCostMtr) * Val(txt_ProfitPerc.Text) / 100, "#########0.00")
        Costing_Calculation()
    End Sub

    Private Sub txt_Profit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ProfitPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub
    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Costing_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Costing_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Ref_Code = '" & Trim(NewCode) & "'", con)
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
        Dim ps As Printing.PaperSize
        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try

                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                        PrintDocument1.Print()
                    End If

                Else
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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*  from Costing_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo   where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Ref_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*  from Costing_Head a  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Ref_Code = '" & Trim(NewCode) & "'", con)
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
        Dim d1, M1, W1 As Single


        p1Font = New Font("Calibri", 11, FontStyle.Bold)

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

        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            e.PageSettings.PaperSize = ps
        '            PpSzSTS = True
        '            Exit For
        '        End If
        '    Next

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



        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 30
            .Top = 30
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


        d1 = e.Graphics.MeasureString("WARP WEIGHT IN GRAMS       :  ", pFont).Width

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(35) : ClAr(2) = 225 : ClAr(3) = 180 : ClAr(4) = 100 : ClAr(5) = 120
        ClAr(6) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5))

        M1 = ClAr(1) + ClAr(2) + ClAr(3) - 30
        TxtHgt = 19

        W1 = e.Graphics.MeasureString("WARP WEIGHT IN GRAMS       :  ", pFont).Width

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = NoofDets + 1



                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "WARP YARN COST/MTR", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + d1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Warp_Yarn_Cost").ToString, LMargin + d1 + 30, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "WEFT YARN COST/MTR", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Weft_Yarn_Cost").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)


                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "SIZING COST", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + d1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Sizing_Cost").ToString, LMargin + d1 + 30, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "WEAVING COST", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Weaving").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)


                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "PROCESSING COST", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + d1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Processing").ToString, LMargin + d1 + 30, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "OTHER COST", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Other_Cost").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)


                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "COMMISSION  @" & Val(prn_DetDt.Rows(prn_DetIndx).Item("Commission_Percentage").ToString) & " % ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + d1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Commission_Amount").ToString), "#######0.00"), LMargin + d1 + 30, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "PROFIT  @" & Val(prn_DetDt.Rows(prn_DetIndx).Item("Profit_Perc").ToString) & " % ", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(0).Item("Warp_Wet_Cost_2_5").ToString), "#######0.00"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "WARP WEIGHT IN GRAMS", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + d1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Warp_Weight").ToString), "#######0.000"), LMargin + d1 + 30, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "WEFT WEIGHT IN GRAMS", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(0).Item("Weft_Weight").ToString), "#######0.000"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, "FABRIC WEIGHT IN GRAMS", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + d1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Fabric_Weight").ToString), "#######0.000"), LMargin + d1 + 30, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "COST/METER", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(0).Item("Cost_Meter").ToString), "#######0.00"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)


                NoofDets = NoofDets + 1

                prn_DetIndx = prn_DetIndx + 1

                CurY = CurY + TxtHgt + 10

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
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim C1, N1, M1, W1 As Single

        PageNo = PageNo + 1

        CurY = TMargin


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

        CurY = CurY + 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "COSTING", LMargin, CurY, 2, PrintWidth, p1Font)

        CurY = CurY + TxtHgt + 10
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


        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3)


        Try

            N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
            W1 = e.Graphics.MeasureString("PROCESSING CHARGE/MTR  :  ", pFont).Width

            M1 = ClAr(1) + ClAr(2) + ClAr(3) - 30

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 11, FontStyle.Bold)


            Common_Procedures.Print_To_PrintDocument(e, "REF.NO", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ref_No").ToString, LMargin + W1 + 25, CurY, 0, 0, p1Font)



            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Ref_Date").ToString), "dd-MM-yyyy"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "NAME", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ref_Name").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "WARP COUNT", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("Warp_Count").ToString), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "WEFT COUNT", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Weft_Count").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "ENDS INCH(IP)", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("EPI").ToString), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "PICK INCH", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("PPI").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "TAP LENGTH", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("Tape_Length").ToString), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "WIDTH", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Width").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "REED SPACE", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("Reed_Space").ToString), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "CRIMP%", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Crimp_Percentage").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "WEFT WASTE%", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("Weft_Waste_Percentage").ToString), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "WARP RATE/KG", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Warp_Rate_Kg").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "WEFT RATE/KG", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("WEFT_Rate_Kg").ToString), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "SIZING CHARGE/KG", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sizing_Charge").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "WEAVING CHARGE/PICK", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("Weaving_Charge").ToString), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "PROCESSING CHARGE/KG", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Processing_Charge").ToString, LMargin + W1 + 25, CurY, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "OTHER CHARGE/MTR", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("Other_Charge").ToString), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        ' e.Graphics.DrawLine(Pens.Black, LMargin + 380, CurY, LMargin + 380, LnAr(2))

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font


        For I = NoofDets + 1 To NoofItems_PerPage
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
    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue


        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If

    End Sub

    Private Sub msk_Date_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_Date.Text = Date.Today
            msk_Date.SelectionStart = 0
        End If
    End Sub


    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_Date.Text = Date.Today
        'End If
        If e.KeyCode = 107 Then
            msk_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Date.Text))
        ElseIf e.KeyCode = 109 Then
            msk_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Date.Text))
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)

        End If

    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged

        If IsDate(dtp_Date.Text) = True Then

            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_Date.LostFocus

        If IsDate(msk_Date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_Date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_Date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) >= 2000 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_Date.Text)
                End If
            End If

        End If
    End Sub

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
        End If
    End Sub

    Private Sub txt_ElongationPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ElongationPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_ElongationPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_ElongationPerc.TextChanged
        Costing_Calculation()
    End Sub

    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub txt_CommAmount_TextChanged(sender As Object, e As EventArgs) Handles txt_CommAmount.TextChanged
        Costing_Calculation()
    End Sub

    Private Sub txt_CommAmount_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_CommAmount.KeyDown
        If Val(txt_CommPerc.Text) <> 0 Then
            e.Handled = True
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub txt_CommAmount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_CommAmount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_ProfitAmount_TextChanged(sender As Object, e As EventArgs) Handles txt_ProfitAmount.TextChanged
        Costing_Calculation()
    End Sub

    Private Sub txt_ProfitAmount_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_ProfitAmount.KeyDown
        If Val(txt_ProfitPerc.Text) <> 0 Then
            e.Handled = True
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub txt_ProfitAmount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_ProfitAmount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Ends_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Ends.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Ends_TextChanged(sender As Object, e As EventArgs) Handles txt_Ends.TextChanged
        Costing_Calculation()
    End Sub

End Class
