Imports System.Windows.Forms.VisualStyles
Imports System.IO


Public Class Weaving_job_Card_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "WJOBC-"
    Private Prec_ActCtrl As New Control
    Private vCbo_ItmNm As String
    Private vcbo_KeyDwnVal As Double
    Private dgv_ActiveCtrl_Name As String

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_YarnDetails As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetSNo As Integer
    Private prn_Count As Integer = 0


    Private PrntCnt2ndPageSTS As Boolean = False
    Private prn_Prev_HeadIndx As Integer
    Private prn_HeadIndx As Integer
    Private vPrnt_2Copy_In_SinglePage As Integer = 0
    Private prn_TotCopies As Integer = 0
    Private Print_PDF_Status As Boolean = False
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private Sub clear()

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False


        lbl_JobNo.Text = ""
        lbl_JobNo.ForeColor = Color.Black
        dtp_Date.Text = ""

        cbo_Ledger.Text = ""
        cbo_Cloth.Text = ""
        cbo_Cloth.Tag = cbo_Cloth.Text

        Cbo_Warp_Count.Text = ""
        Cbo_Weft_Count.Text = ""
        cbo_Ends_Count.Text = ""
        txt_EPI.Text = ""
        txt_PPI.Text = ""
        txt_Width.Text = ""
        txt_Loom_Reed.Text = ""
        txt_Noof_Ends.Text = ""
        txt_Weave.Text = ""
        txt_Noof_Looms.Text = ""
        txt_length.Text = ""
        txt_Packing.Text = ""

        cbo_Transport.Text = ""
        cbo_Slevedge.Text = ""
        cbo_Sizing_Name.Text = ""
        Cbo_Delivery_To.Text = ""

        txt_Rate_Meters.Text = ""
        txt_Total_Meters.Text = ""

        txt_Remarks.Text = ""



        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ClothName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ClothName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If


        Grid_Cell_DeSelect()
        dgv_ActiveCtrl_Name = ""


        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
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
        'If Not IsNothing(dgv_YarnDetails.CurrentCell) Then dgv_YarnDetails.CurrentCell.Selected = False
        'If Not IsNothing(dgv_YarnDetails_Total.CurrentCell) Then dgv_YarnDetails_Total.CurrentCell.Selected = False
        'If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
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

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            'da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as WeaverName from Weaving_JobCard_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo   Where a.Weaving_JobCard_Code = '" & Trim(NewCode) & "'", con)
            'dt1 = New DataTable
            'da1.Fill(dt1)

            da1 = New SqlClient.SqlDataAdapter("select a.*,b.Cloth_Name,c.Count_Name as WarpCount_Name,d.Count_Name as WeftCount_Name,e.LEdger_Name,f.LEdger_Name as Transport_Name,g.Ledger_Name as DeliveryTo_Name ,h.Ledger_Name as Sizing_Name ,i.Slevedge_Name  ,j.EndsCount_Name  " &
           " From Weaving_JobCard_Head a  " &
           " Left OUTER JOIN  Cloth_HEad b  on A.cloth_Idno =b.cloth_Idno  " &
           " Left OUTER JOIN  Count_HEad c  on A.WarpCount_IdNo=c.Count_IdNo    " &
           " Left OUTER JOIN  Count_HEad d  on A.WeftCount_IdNo =d.Count_IdNo  " &
           " Left OUTER JOIN  Ledger_Head e  on A.Ledger_Idno =e.Ledger_Idno  " &
           " Left OUTER JOIN  Ledger_Head f  on A.transport_Idno =f.Ledger_Idno  " &
           " Left OUTER JOIN  Ledger_Head g  on A.DeliveryTo_IdNo =g.Ledger_Idno  " &
           " Left OUTER JOIN  Ledger_Head h  on A.Sizing_IdNo =h.Ledger_Idno   " &
           " Left OUTER JOIN  Slevedge_Head i  on A.Slevedge_IdNo=i.Slevedge_IdNo   " &
           " Left OUTER JOIN  EndsCount_head j  on A.EndsCount_IdNo=j.EndsCount_IdNo  " &
           " Where a.Weaving_JobCard_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)




            If dt1.Rows.Count > 0 Then

                lbl_JobNo.Text = dt1.Rows(0).Item("Weaving_JobCard_No").ToString

                dtp_Date.Text = dt1.Rows(0).Item("Weaving_JobCard_Date").ToString

                cbo_Ledger.Text = Trim(dt1.Rows(0).Item("LEdger_Name").ToString)
                'cbo_Ledger.Text = Common_Procedures.Ledger_IdNoToName(con, dt1.Rows(0).Item("ledger_Idno").ToString)
                cbo_Cloth.Text = dt1.Rows(0).Item("Cloth_Name").ToString
                cbo_Cloth.Tag = Trim(cbo_Cloth.Text)

                'cbo_Cloth.Text = Common_Procedures.Cloth_IdNoToName(con, dt1.Rows(0).Item("cloth_Idno").ToString)
                Cbo_Warp_Count.Text = dt1.Rows(0).Item("WarpCount_Name").ToString
                'Cbo_Warp_Count.Text = Common_Procedures.Count_IdNoToName(con, dt1.Rows(0).Item("WarpCount_IdNo").ToString)
                Cbo_Weft_Count.Text = dt1.Rows(0).Item("WeftCount_Name").ToString
                'Cbo_Weft_Count.Text = Common_Procedures.Count_IdNoToName(con, dt1.Rows(0).Item("WeftCount_IdNo").ToString)
                cbo_Ends_Count.Text = dt1.Rows(0).Item("EndsCount_Name").ToString

                txt_EPI.Text = dt1.Rows(0).Item("Ends_Per_Inch").ToString
                txt_PPI.Text = dt1.Rows(0).Item("Pick_Per_Inch").ToString
                txt_Width.Text = dt1.Rows(0).Item("cloth_Width").ToString
                txt_Loom_Reed.Text = dt1.Rows(0).Item("Loom_Reed").ToString
                txt_Noof_Ends.Text = dt1.Rows(0).Item("No_Of_Ends").ToString
                txt_Weave.Text = dt1.Rows(0).Item("Cloth_Weave").ToString
                txt_Noof_Looms.Text = dt1.Rows(0).Item("No_Of_Looms").ToString
                txt_length.Text = dt1.Rows(0).Item("Cloth_Length_Terms").ToString
                txt_Packing.Text = dt1.Rows(0).Item("Packing").ToString

                cbo_Transport.Text = dt1.Rows(0).Item("Transport_Name").ToString
                'cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, dt1.Rows(0).Item("ledger_Idno").ToString)
                cbo_Slevedge.Text = dt1.Rows(0).Item("Slevedge_Name").ToString
                'cbo_Selvedge.Text = Common_Procedures.Slevedge_IdNoToName(con, dt1.Rows(0).Item("Slevedge_IdNo").ToString)
                cbo_Sizing_Name.Text = dt1.Rows(0).Item("Sizing_Name").ToString
                'cbo_Sizing_Name.Text = Common_Procedures.Ledger_IdNoToName(con, dt1.Rows(0).Item("ledger_Idno").ToString)
                Cbo_Delivery_To.Text = dt1.Rows(0).Item("DeliveryTo_Name").ToString
                'Cbo_Delivery_To.Text = Common_Procedures.Ledger_IdNoToName(con, dt1.Rows(0).Item("ledger_Idno").ToString)

                txt_Rate_Meters.Text = dt1.Rows(0).Item("Rate_Per_Meter").ToString
                txt_Total_Meters.Text = dt1.Rows(0).Item("Total_Meters").ToString

                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString

            End If





            dt2.Clear()

            '  TotalYarnTaken_Calculation()
            dt2.Clear()
            dt2.Dispose()
            da2.Dispose()



            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If cbo_Ledger.Visible And cbo_Ledger.Enabled Then cbo_Ledger.Focus()

    End Sub

    Private Sub Job_Card_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Cloth.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH NAME" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Cloth.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_Warp_Count.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                Cbo_Warp_Count.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_Weft_Count.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                Cbo_Weft_Count.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ends_Count.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ends_Count.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Slevedge.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "SLEVEDGE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Slevedge.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Sizing_Name.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Sizing_Name.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_Delivery_To.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                Cbo_Delivery_To.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Job_Card_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
        Dim dt10 As New DataTable

        Me.Text = ""

        con.Open()


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        ' --- Set TabIndex

        lbl_JobNo.TabIndex = 0

        dtp_Date.TabIndex = 1

        cbo_Ledger.TabIndex = 2
        cbo_Cloth.TabIndex = 3
        Cbo_Warp_Count.TabIndex = 4
        Cbo_Weft_Count.TabIndex = 5
        cbo_Ends_Count.TabIndex = 6
        txt_EPI.TabIndex = 7
        txt_PPI.TabIndex = 8
        txt_Width.TabIndex = 9
        txt_Loom_Reed.TabIndex = 10
        txt_Noof_Ends.TabIndex = 11
        txt_Weave.TabIndex = 12
        txt_Noof_Looms.TabIndex = 13
        txt_length.TabIndex = 14
        txt_Packing.TabIndex = 15

        cbo_Transport.TabIndex = 16
        cbo_Slevedge.TabIndex = 17
        cbo_Sizing_Name.TabIndex = 18
        Cbo_Delivery_To.TabIndex = 19

        txt_Total_Meters.TabIndex = 20
        txt_Rate_Meters.TabIndex = 21

        txt_Remarks.TabIndex = 22


        ' --- ControlGotFocus

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Warp_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Weft_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ends_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EPI.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PPI.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Width.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Loom_Reed.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Noof_Ends.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Weave.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Noof_Looms.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_length.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Packing.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Delivery_To.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Sizing_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Slevedge.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rate_Meters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Total_Meters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_EMail.Enter, AddressOf ControlGotFocus
        AddHandler btn_PDF.Enter, AddressOf ControlGotFocus

        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_SizingName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ClothName.GotFocus, AddressOf ControlGotFocus

        ' --- ControlLostFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Warp_Count.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Weft_Count.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ends_Count.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EPI.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PPI.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Width.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Loom_Reed.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Noof_Ends.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Weave.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Noof_Looms.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_length.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Packing.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Delivery_To.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Sizing_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Slevedge.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate_Meters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Total_Meters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus

        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_EMail.Enter, AddressOf ControlLostFocus
        AddHandler btn_PDF.Enter, AddressOf ControlLostFocus

        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_SizingName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ClothName.LostFocus, AddressOf ControlLostFocus

        ' --- TextBoxControlKeyDown

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler cbo_Ledger.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler cbo_Cloth.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler Cbo_Warp_Count.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler Cbo_Weft_Count.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_EPI.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PPI.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Width.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Loom_Reed.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Noof_Ends.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Weave.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Noof_Looms.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_length.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Packing.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler cbo_Transport.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler Cbo_Delivery_To.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler cbo_Sizing_Name.KeyDown, AddressOf TextBoxControlKeyDown
        '  AddHandler cbo_Slevedge.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rate_Meters.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Total_Meters.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Remarks.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler btn_save.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler btn_close.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler btn_EMail.Enter, AddressOf TextBoxControlKeyDown
        AddHandler btn_PDF.Enter, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown


        ' --- TextBoxControlKeypress

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler cbo_Ledger.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler cbo_Cloth.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler Cbo_Warp_Count.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler Cbo_Weft_Count.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_EPI.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PPI.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Width.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Loom_Reed.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Noof_Ends.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Weave.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Noof_Looms.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_length.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Packing.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler cbo_Transport.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler Cbo_Delivery_To.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler cbo_Sizing_Name.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler cbo_Slevedge.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Rate_Meters.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Total_Meters.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Remarks.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler btn_save.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler btn_close.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler btn_EMail.Enter, AddressOf TextBoxControlKeyPress
        AddHandler btn_PDF.Enter, AddressOf TextBoxControlKeyPress

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

    Private Sub Job_Card_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Job_Card_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim UID As Single = 0
        Dim vUsrNm As String = "", vAcPwd As String = "", vUnAcPwd As String = ""


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1036" Then '----- KALAIMAGAL TEXTILES (AVINASHI)
            Common_Procedures.Password_Input = ""
            Dim g As New Admin_Password
            g.ShowDialog()

            UID = 1
            Common_Procedures.get_Admin_Name_PassWord_From_DB(vUsrNm, vAcPwd, vUnAcPwd)

            vAcPwd = Common_Procedures.Decrypt(Trim(vAcPwd), Trim(Common_Procedures.UserCreation_AcPassWord.passPhrase) & Trim(Val(UID)) & Trim(UCase(vUsrNm)), Trim(Common_Procedures.UserCreation_AcPassWord.saltValue) & Trim(Val(UID)) & Trim(UCase(vUsrNm)))
            vUnAcPwd = Common_Procedures.Decrypt(Trim(vUnAcPwd), Trim(Common_Procedures.UserCreation_UnAcPassWord.passPhrase) & Trim(Val(UID)) & Trim(UCase(vUsrNm)), Trim(Common_Procedures.UserCreation_UnAcPassWord.saltValue) & Trim(Val(UID)) & Trim(UCase(vUsrNm)))

            If Trim(Common_Procedures.Password_Input) <> Trim(vAcPwd) And Trim(Common_Procedures.Password_Input) <> Trim(vUnAcPwd) Then
                MessageBox.Show("Invalid Admin Password", "ADMIN PASSWORD FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If

        ' NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)



        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try

            'NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans


            cmd.CommandText = "delete from Weaving_JobCard_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaving_JobCard_Code = '" & Trim(NewCode) & "'"
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

        If cbo_Sizing_Name.Enabled = True And cbo_Sizing_Name.Visible = True Then cbo_Sizing_Name.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select a.Ledger_DisplayName from Ledger_AlaisHead a, ledger_head b where (a.Ledger_IdNo = 0 or b.AccountsGroup_IdNo = 10 ) and a.Ledger_IdNo = b.Ledger_IdNo order by a.Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select Count_name from Count_Head order by count_name", con)
            da.Fill(dt2)
            cbo_Filter_ClothName.DataSource = dt2
            cbo_Filter_ClothName.DisplayMember = "count_name"

            da = New SqlClient.SqlDataAdapter("select Mill_name from Mill_Head order by Mill_name", con)
            da.Fill(dt3)
            cbo_Filter_SizingName.DataSource = dt3
            cbo_Filter_SizingName.DisplayMember = "Mill_name"

            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_ClothName.Text = ""
            cbo_Filter_SizingName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ClothName.SelectedIndex = -1
            cbo_Filter_SizingName.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 Weaving_JobCard_No from Weaving_JobCard_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaving_JobCard_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Weaving_JobCard_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_JobNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Weaving_JobCard_No from Weaving_JobCard_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaving_JobCard_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Weaving_JobCard_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_JobNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Weaving_JobCard_No from Weaving_JobCard_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaving_JobCard_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaving_JobCard_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Weaving_JobCard_No from Weaving_JobCard_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaving_JobCard_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaving_JobCard_No desc", con)
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

            lbl_JobNo.Text = Common_Procedures.get_MaxCode(con, "Weaving_JobCard_Head", "Weaving_JobCard_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_JobNo.ForeColor = Color.Red

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

            inpno = InputBox("Enter Job No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaving_JobCard_No from Weaving_JobCard_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaving_JobCard_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Job No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.yarn_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.yarn_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Job No.", "FOR NEW JOB INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaving_JobCard_No from Weaving_JobCard_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaving_JobCard_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Job No", "DOES NOT INSERT NEW JOB...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_JobNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW JOB...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Selc_SetCode As String
        Dim led_id As Integer = 0
        Dim Bw_id As Integer = 0
        Dim Cnt_ID As Integer = 0
        Dim Mil_ID As Integer = 0
        Dim Cnt_Grid_ID As Integer = 0
        Dim Mil_Grid_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = "", YrnPartcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotBags As Single, vTotCones As Single, vTotWeight As Single
        Dim vTotNoOfCones As Single, vTotYrnWeight As Single
        Dim vSELC_JOBCODE As String
        Dim vSetCd As String, vSetNo As String
        Dim Nr As Long
        Dim UserIdNo As Integer = 0
        Dim YCnt_ID As Integer = 0
        Dim YMil_ID As Integer = 0
        Dim loomtype_Id As Integer = 0

        Dim Cloth_ID As Integer = 0
        Dim WarpCount_ID As Integer = 0
        Dim WeftCount_ID As Integer = 0
        Dim EndsCnt_ID As Integer = 0
        Dim Delvto_LedID As Integer = 0
        Dim Sizing_LedID As Integer = 0
        Dim Transport_LedID As Integer = 0
        Dim Slevedge_ID As Integer = 0



        UserIdNo = Common_Procedures.User.IdNo
        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)



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



        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If led_id = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Sizing_Name.Enabled And cbo_Sizing_Name.Visible Then cbo_Sizing_Name.Focus()
            Exit Sub
        End If

        Cloth_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)
        WarpCount_ID = Common_Procedures.Count_NameToIdNo(con, Cbo_Warp_Count.Text)
        WeftCount_ID = Common_Procedures.Count_NameToIdNo(con, Cbo_Weft_Count.Text)
        EndsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, cbo_Ends_Count.Text)


        Sizing_LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Sizing_Name.Text)
        Transport_LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)
        Delvto_LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, Cbo_Delivery_To.Text)
        Slevedge_ID = Common_Procedures.Slevedge_NameToIdNo(con, cbo_Slevedge.Text)



        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@JobDate", dtp_Date.Value.Date)


        tr = con.BeginTransaction

        Try
            cmd.Connection = con
            cmd.Transaction = tr




            If Insert_Entry = True Or New_Entry = False Then
                '  NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


            Else

                lbl_JobNo.Text = Common_Procedures.get_MaxCode(con, "Weaving_JobCard_Head", "Weaving_JobCard_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                ' NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


            End If

            vSELC_JOBCODE = Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag))


            If New_Entry = True Then
                cmd.CommandText = "Insert into Weaving_JobCard_Head (Weaving_JobCard_Code   ,  Weaving_JobCode_forSelection  ,          Company_IdNo             ,            Weaving_JobCard_No  ,                                for_OrderBy                              ,    Weaving_JobCard_Date   ,     ledger_idno      ,     Cloth_IdNo       ,      WarpCount_IdNo      ,      WeftCount_IdNo       ,     EndsCount_IdNo       ,          Ends_Per_Inch         ,         Pick_Per_Inch          ,       cloth_Width                ,                Loom_Reed               ,          Cloth_Weave           ,        Slevedge_IdNo       ,              No_Of_Ends                ,         Cloth_Length_Terms     ,          Total_Meters                 ,               Rate_Per_Meter           ,      DeliveryTo_IdNo       ,           No_Of_Looms                  ,         Transport_IdNo        ,        Sizing_IdNo         ,            Packing             ,              Remarks ) " &
                                                          " Values ('" & Trim(NewCode) & "',  '" & Trim(vSELC_JOBCODE) & "' ,  " & Str(Val(lbl_Company.Tag)) & " ,  '" & Trim(lbl_JobNo.Text) & "',  " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_JobNo.Text))) & ",           @JobDate        , " & Val(led_id) & "  ," & Val(Cloth_ID) & " ," & Val(WarpCount_ID) & " , " & Val(WeftCount_ID) & " , " & Val(EndsCnt_ID) & "  , " & Str(Val(txt_EPI.Text)) & " , " & Str(Val(txt_PPI.Text)) & " , " & Str(Val(txt_Width.Text)) & " ,  " & Str(Val(txt_Loom_Reed.Text)) & "  ,  '" & Trim(txt_Weave.Text) & "', " & Val(Slevedge_ID) & "   ,  " & Str(Val(txt_Noof_Ends.Text)) & "  , '" & Trim(txt_length.Text) & "'," & Str(Val(txt_Total_Meters.Text)) & " , " & Str(Val(txt_Rate_Meters.Text)) & " , " & Val(Delvto_LedID) & "  ,  " & Str(Val(txt_Noof_Looms.Text)) & " , " & Val(Transport_LedID) & " ,  " & Val(Sizing_LedID) & " ,'" & Trim(txt_Packing.Text) & "' ,'" & Trim(txt_Remarks.Text) & "'  )"
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Weaving_JobCard_Head set Weaving_JobCode_forSelection = '" & Trim(vSELC_JOBCODE) & "',  Weaving_JobCard_Date = @JobDate, ledger_idno = " & Str(Val(led_id)) & ", Cloth_IdNo = " & Val(Cloth_ID) & ", WarpCount_IdNo = " & Val(WarpCount_ID) & ", WeftCount_IdNo = " & Val(WeftCount_ID) & ",EndsCount_IdNo=" & Val(EndsCnt_ID) & " , Ends_Per_Inch =  " & Str(Val(txt_EPI.Text)) & " , Pick_Per_Inch =" & Str(Val(txt_PPI.Text)) & " , cloth_Width = " & Str(Val(txt_Width.Text)) & " , Loom_Reed =  " & Str(Val(txt_Loom_Reed.Text)) & " , Cloth_Weave = '" & Trim(txt_Weave.Text) & "', Slevedge_IdNo = " & Val(Slevedge_ID) & ", No_Of_Ends = " & Str(Val(txt_Noof_Ends.Text)) & ", Cloth_Length_Terms = '" & Trim(txt_length.Text) & "', Total_Meters =" & Str(Val(txt_Total_Meters.Text)) & ", Rate_Per_Meter=" & Str(Val(txt_Rate_Meters.Text)) & " , DeliveryTo_IdNo = " & Val(Delvto_LedID) & ",No_Of_Looms= " & Str(Val(txt_Noof_Looms.Text)) & ",Transport_IdNo  = " & Val(Transport_LedID) & " , Sizing_IdNo  =  " & Val(Sizing_LedID) & " ,Packing='" & Trim(txt_Packing.Text) & "' ,Remarks='" & Trim(txt_Remarks.Text) & "'   Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaving_JobCard_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()


            End If




            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()



            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_JobNo.Text)
                End If
            Else
                move_record(lbl_JobNo.Text)
            End If

        Catch ex As Exception

            tr.Rollback()

            'If InStr(1, Trim(LCase(ex.Message)), "ck_stock_babycone_processing_details") > 0 Then
            '    MessageBox.Show("Invalid Baby cone Details - Delivery Qty greater than production Qty", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)

            'Else
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            'End If

        Finally
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
        Dim Led_IdNo As Integer, Cloth_IdNo As Integer, Sizing_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Cloth_IdNo = 0
            Sizing_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Weaving_JobCard_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Weaving_JobCard_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Weaving_JobCard_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_ClothName.Text) <> "" Then
                Cloth_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Filter_ClothName.Text)
            End If

            If Trim(cbo_Filter_SizingName.Text) <> "" Then
                Sizing_IdNo = Common_Procedures.Ledger_NameToIdNo(con, cbo_Filter_SizingName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Cloth_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Weaving_JobCard_Code IN (select z1.Weaving_JobCard_Code from Weaving_JobCard_Head z1 where z1.CLoth_IdNo = " & Str(Val(Cloth_IdNo)) & ") "
            End If

            If Val(Sizing_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Weaving_JobCard_Code IN (select z2.Weaving_JobCard_Code from Weaving_JobCard_Head z2 where z2.Sizing_IdNo = " & Str(Val(Sizing_IdNo)) & ") "
            End If



            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name,c.Count_Name as WarpCount_Name,d.Count_Name as WeftCount_Name from Weaving_JobCard_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo Left OUTER JOIN  Count_HEad c  on A.WarpCount_IdNo=c.Count_IdNo Left OUTER JOIN  Count_HEad d  on A.WeftCount_IdNo =d.Count_IdNo   where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaving_JobCard_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Weaving_JobCard_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Weaving_JobCard_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Weaving_JobCard_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("WarpCount_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("WeftCount_Name").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")

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
    Private Sub cbo_Sizing_Name_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Sizing_Name.GotFocus
        cbo_Sizing_Name.Tag = cbo_Sizing_Name.Text
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'SIZING' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub
    Private Sub cbo_Sizing_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Sizing_Name.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Sizing_Name, cbo_Slevedge, Cbo_Delivery_To, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'SIZING'  or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Sizing_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Sizing_Name.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Sizing_Name, Cbo_Delivery_To, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'SIZING'  or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

        End If
    End Sub

    Private Sub cbo_Sizing_Name_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Sizing_Name.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = "SIZING"
            Dim f As New Ledger_Creation


            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Sizing_Name.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub
    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Close_Form()
    End Sub
    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'WEAVER'  or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'WEAVER'  or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ClothName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ClothName, cbo_Filter_PartyName, cbo_Filter_SizingName, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ClothName, cbo_Filter_SizingName, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub
    Private Sub cbo_Filter_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_SizingName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_SizingName, cbo_Filter_ClothName, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'SIZING' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub
    Private Sub cbo_Filter_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_SizingName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_SizingName, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'SIZING' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If
    End Sub

    Private Sub txt_WarpMeters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub
    Private Sub Total_Calculation()
        'Dim Sno As Integer
        'Dim TotBags As Single, TotCones As Single, TotWeight As Single

        'Sno = 0
        'TotBags = 0
        'TotCones = 0
        'TotWeight = 0
        'With dgv_YarnDetails
        '    For i = 0 To .RowCount - 1
        '        Sno = Sno + 1
        '        .Rows(i).Cells(0).Value = Sno
        '        If Val(.Rows(i).Cells(7).Value) <> 0 Then
        '            TotBags = TotBags + Val(.Rows(i).Cells(5).Value)
        '            TotCones = TotCones + Val(.Rows(i).Cells(6).Value)
        '            TotWeight = TotWeight + Val(.Rows(i).Cells(7).Value)
        '        End If
        '    Next
        'End With

        'With dgv_YarnDetails_Total
        '    If .RowCount = 0 Then .Rows.Add()
        '    .Rows(0).Cells(5).Value = Val(TotBags)
        '    .Rows(0).Cells(6).Value = Val(TotCones)
        '    .Rows(0).Cells(7).Value = Format(Val(TotWeight), "########0.000")
        'End With

    End Sub
    Private Sub txt_EmptyBeam_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = 40 Then SendKeys.Send("{TAB}")
        'If e.KeyCode = 38 Then cbo_Grid_Countname.Focus() ' SendKeys.Send("+{TAB}")
    End Sub
    Private Sub txt_Remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub
    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize

        Dim PpSzSTS As Boolean = False

        '  NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* from Weaving_JobCard_Head a LEFT OUTER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo where a.Weaving_JobCard_Code = '" & Trim(NewCode) & "'", con)
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

        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER A4", 827, 1169)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'MessageBox.Show("Printing_Invoice - 7")
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
        'MessageBox.Show("Printing_Invoice - 8")
        PrintDocument1.DefaultPageSettings.Landscape = False
        'MessageBox.Show("Printing_Invoice - 9")
        PpSzSTS = True

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try
                If Print_PDF_Status = True Then
                    '--This is actual & correct 
                    PrintDocument1.DocumentName = "Invoice"
                    PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    PrintDocument1.PrinterSettings.PrintFileName = "c:\JobCard_'" & Trim(lbl_JobNo.Text) & "'.pdf"
                    PrintDocument1.Print()

                Else

                    If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then

                        PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                        If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings


                            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                                    Exit For
                                End If
                            Next




                            PrintDocument1.Print()
                        End If

                    Else

                        PrintDocument1.Print()

                    End If
                End If
            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try


        Else
            Try

                Dim ppd As New PrintPreviewDialog

                'MessageBox.Show("Printing_Invoice - 25")

                ppd.Document = PrintDocument1

                'MessageBox.Show("Printing_Invoice - 26")

                ppd.WindowState = FormWindowState.Maximized
                'MessageBox.Show("Printing_Invoice - 27")
                ppd.StartPosition = FormStartPosition.CenterScreen
                'MessageBox.Show("Printing_Invoice - 28")

                ppd.PrintPreviewControl.AutoZoom = True
                'MessageBox.Show("Printing_Invoice - 29")
                ppd.PrintPreviewControl.Zoom = 1.0
                'MessageBox.Show("Printing_Invoice - 30")


                ppd.ShowDialog()

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

    End Sub


    Private Sub cbo_Loom_Type_GotFocus(sender As Object, e As EventArgs)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "LoomType_Head", "LoomType_name", "", "(LoomType_IdNo=0)")
    End Sub

    Private Sub txt_beam_length_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_length.KeyDown
        If e.KeyCode = 38 Then
            '  txt_WarpLegnth.Focus()
        End If
        If e.KeyCode = 40 Then
            'txt_sizing_length.Focus()
        End If
    End Sub

    Private Sub txt_beam_length_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_length.KeyPress
        '
        If Asc(e.KeyChar) = 13 Then
            ' txt_sizing_length.Focus()
        End If
    End Sub

    Private Sub txt_fabric_Weave_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Weave.KeyDown
        If e.KeyCode = 38 Then
            ' cbo_Loom_Type.Focus()
        End If
        If e.KeyCode = 40 Then
            'txt_Fabric_Width.Focus()
        End If
    End Sub

    Private Sub txt_fabric_Weave_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Weave.KeyPress
        If Asc(e.KeyChar) = 13 Then
            ' txt_Fabric_Width.Focus()
        End If
    End Sub


    Private Sub txt_DBF_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_EPI.KeyDown
        If e.KeyCode = 38 Then
            ' txt_Loom_Reed.Focus()
        End If
        If e.KeyCode = 40 Then
            'txt_no_of_creel.Focus()
        End If
    End Sub

    Private Sub txt_DBF_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_EPI.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            ' txt_no_of_creel.Focus()
        End If
    End Sub


    Private Sub TotalYarnTaken_Calculation()
        Dim Sno As Integer
        Dim TotWgtperCone As Single, TotNoOfCones As Single, TotWeight As Single

        'If FrmLdSTS = True Then Exit Sub

        'Sno = 0
        'TotWgtperCone = 0
        'TotNoOfCones = 0
        'TotWeight = 0
        'With dgv_YarnDetails
        '    For i = 0 To .RowCount - 1
        '        Sno = Sno + 1
        '        .Rows(i).Cells(0).Value = Sno
        '        If Val(.Rows(i).Cells(6).Value) <> 0 Then
        '            TotWgtperCone = TotWgtperCone + Val(.Rows(i).Cells(5).Value)
        '            TotNoOfCones = TotNoOfCones + Val(.Rows(i).Cells(6).Value)
        '            TotWeight = TotWeight + Val(.Rows(i).Cells(7).Value)
        '        End If
        '    Next
        'End With

        'With dgv_YarnDetails_Total
        '    If .RowCount = 0 Then .Rows.Add()
        '    .Rows(0).Cells(5).Value = Val(TotWgtperCone)
        '    .Rows(0).Cells(6).Value = Val(TotNoOfCones)
        '    .Rows(0).Cells(7).Value = Format(Val(TotWeight), "########0.000")
        'End With

        ''If Trim(UCase(cbo_YarnStock.Text)) = "CONSUMED YARN" Then
        ''    txt_ConsumedYarn.Text = Format(Val(dgv_YarnDetails_Total.Rows(0).Cells(6).Value), "########0.000")
        ''Else
        ''    txt_YarnTaken.Text = Format(Val(dgv_YarnDetails_Total.Rows(0).Cells(6).Value), "########0.000")
        ''End If
        ''NetAmount_Calculation()

    End Sub



    Private Sub txt_Remarks_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Remarks.KeyDown
        If e.KeyCode = 38 Then
            '  txt_Packing.Focus()
        End If
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            End If
        End If
    End Sub


    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String

        '  NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_Count = 0
        PrntCnt2ndPageSTS = False
        prn_Prev_HeadIndx = -100
        prn_HeadIndx = 0
        prn_Count = 0
        'prn_Count1 = 0
        'cnt = 0
        prn_DetIndx = 0
        'prn_DetIndx1 = 0
        prn_DetSNo = 0
        'prn_PageCount = 0


        Try

            '            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, d.*,d.Ledger_MainName ,Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code , Lsh.State_Name as Ledger_State_Name from Weaving_JobCard_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN State_Head Csh ON b.Company_State_IdNo = Csh.State_IdNo   Left Outer JOIN Ledger_Head d ON a.Ledger_IdNo = d.Ledger_IdNo LEFT OUTER JOIN State_Head Lsh ON d.Ledger_State_IdNo = Lsh.State_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaving_JobCard_Code = '" & Trim(NewCode) & "'", con)
            da1 = New SqlClient.SqlDataAdapter("select a.*, CmpHd.*, e.*, b.Cloth_Name,c.Count_Name as WarpCount_Name,d.Count_Name as WeftCount_Name,e.Ledger_MainName,f.LEdger_Name as TransportName,g.Ledger_Name as DeliveryTo_Name ,h.Ledger_Name as Sizing_Name ,i.Slevedge_Name  ,j.EndsCount_Name  ," &
           " Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code , Lsh.State_Name as Ledger_State_Name  " &
           " From Weaving_JobCard_Head a  " &
           " INNER JOIN Company_Head CmpHd ON a.Company_IdNo = CmpHd.Company_IdNo LEFT OUTER JOIN State_Head Csh ON CmpHd.Company_State_IdNo = Csh.State_IdNo   " &
           " Left OUTER JOIN  Cloth_HEad b  on A.cloth_Idno =b.cloth_Idno  " &
           " Left OUTER JOIN  Count_HEad c  on A.WarpCount_IdNo=c.Count_IdNo    " &
           " Left OUTER JOIN  Count_HEad d  on A.WeftCount_IdNo =d.Count_IdNo  " &
           " Left OUTER JOIN  Ledger_Head e  on A.Ledger_Idno =e.Ledger_Idno  " &
           " Left OUTER JOIN  Ledger_Head f  on A.transport_Idno =f.Ledger_Idno  " &
           " Left OUTER JOIN  Ledger_Head g  on A.DeliveryTo_IdNo =g.Ledger_Idno  " &
           " Left OUTER JOIN  Ledger_Head h  on A.Sizing_IdNo =h.Ledger_Idno   " &
           " Left OUTER JOIN  Slevedge_Head i  on A.Slevedge_IdNo=i.Slevedge_IdNo   " &
           " Left OUTER JOIN  EndsCount_head j  on A.EndsCount_IdNo=j.EndsCount_IdNo  " &
           " LEFT OUTER JOIN State_Head Lsh ON e.Ledger_State_IdNo = Lsh.State_IdNo  " &
           " Where a.Weaving_JobCard_Code = '" & Trim(NewCode) & "'", con)

            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count = 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        Printing_Format2(e)
        'Printing_Format1(e)
    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim ps As Printing.PaperSize
        Dim vConeStk As Long = 0
        Dim vWGTStk As String = 0


        'set_PaperSize_For_PrintDocument1()

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next


        PrntCnt = 1
        If Val(vPrnt_2Copy_In_SinglePage) = 1 Then
            If PrntCnt2ndPageSTS = False Then
                PrntCnt = 2
            End If
        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30 ' 40
            .Right = 45
            .Top = 40 '50 ' 60
            .Bottom = 40


            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 9, FontStyle.Regular)

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

        NoofItems_PerPage = 4
        ' If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) = "" Then
        NoofItems_PerPage = NoofItems_PerPage + 1
        'End If

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = 35 : ClAr(2) = 130 : ClAr(3) = 170 : ClAr(4) = 85 : ClAr(5) = 85 : ClAr(6) = 90
        ClAr(7) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6))

        If Val(vPrnt_2Copy_In_SinglePage) = 1 Then
            TxtHgt = 16 ' 18.25
        ElseIf Val(Common_Procedures.settings.Printing_For_HalfSheet_Set_Custom8X6_As_Default_PaperSize) = 1 Then
            TxtHgt = 17.5
        Else
            TxtHgt = 17
        End If

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_Prev_HeadIndx = prn_HeadIndx

        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        For PCnt = 1 To PrntCnt

            If Val(vPrnt_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 Then
                    prn_PageNo = 0

                    prn_DetIndx = 0
                    prn_DetSNo = 0

                    'prn_Tot_EBeam_Stk = 0
                    'prn_Tot_Pavu_Stk = 0
                    'prn_Tot_Yarn_Stk = 0
                    'prn_Tot_Amt_Bal = 0

                    TpMargin = TMargin

                Else

                    prn_PageNo = 0

                    prn_DetIndx = 0
                    prn_DetSNo = 0

                    'prn_Tot_EBeam_Stk = 0
                    'prn_Tot_Pavu_Stk = 0
                    'prn_Tot_Yarn_Stk = 0
                    'prn_Tot_Amt_Bal = 0

                    TpMargin = 580 + TMargin  ' 600 + TMargin

                End If

            End If

            Try

                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                    NoofItems_PerPage = 4
                    If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) = "" Then
                        NoofItems_PerPage = NoofItems_PerPage + 1
                    End If

                    If Val(Common_Procedures.settings.WeaverWagesYarnReceipt_Print_2Copy_In_SinglePage) = 1 Then
                        If prn_DetDt.Rows.Count > NoofItems_PerPage Then
                            NoofItems_PerPage = 35
                        End If
                    End If

                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets >= NoofItems_PerPage Then

                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, pFont)

                                NoofDets = NoofDets + 1

                                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If

                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                            ItmNm2 = ""
                            If Len(ItmNm1) > 35 Then
                                For I = 15 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 35
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If

                            vConeStk = 0 ' Str(Val(prn_DetDt.Rows(prn_DetIndx).Item("")).ToString)
                            vWGTStk = 0

                            dtp_Date.Text = prn_HdDt.Rows(0).Item("Weaving_JobCard_Date").ToString
                            'Yarn_Stock_Display(2, Val(prn_HdDt.Rows(0).Item("ledger_idno").ToString), Val(prn_DetDt.Rows(prn_DetIndx).Item("count_idno").ToString), Trim(prn_DetDt.Rows(prn_DetIndx).Item("yarn_type").ToString), Val(prn_DetDt.Rows(prn_DetIndx).Item("mill_idno").ToString), vConeStk, vWGTStk)

                            CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Yarn_Type").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight_Per_Cone").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Weight_Per_Cone").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("No_Of_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, vConeStk, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

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

            If Val(vPrnt_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count > 6 Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        Return
                    End If
                End If
            End If

            PrntCnt2ndPageSTS = False

        Next PCnt

LOOP2:



        prn_Count = prn_Count + 1

        If Val(prn_TotCopies) > 1 Then

            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                prn_DetSNo = 0
                prn_PageNo = 0

                e.HasMorePages = True
                Return

            Else
                e.HasMorePages = False
            End If

        Else

            prn_HeadIndx = prn_HeadIndx + 1
            If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then
                e.HasMorePages = True

            Else
                e.HasMorePages = False

            End If

            prn_DetDt.Clear()
            prn_PageNo = 0

            prn_DetIndx = 0
            prn_DetSNo = 0


        End If

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_GSTNo As String
        Dim strHeight As Single
        Dim C1 As Single
        Dim W1 As Single
        Dim M1 As Single
        Dim S1 As Single
        Dim Gst_dt As Date
        Dim Entry_dt As Date
        Dim Count_Typ As String = ""
        Dim Mill_Typ As String = ""
        Dim strWidth As Single = 0
        Dim CurX As Single = 0
        Dim Loom_Typ As String = ""
        PageNo = PageNo + 1

        CurY = TMargin


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_GSTNo = ""

        Dim vADD_BOLD_STS As Boolean = False


        vADD_BOLD_STS = False
        If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")
            Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString)

            Dim vADDR1 As String = ""

            vADDR1 = Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString)
            vADDR1 = Replace(vADDR1, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) & ",", "")
            vADDR1 = Replace(vADDR1, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

            Cmp_Add2 = vADDR1 & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
            vADD_BOLD_STS = True

        Else

            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        End If


        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_CstNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 3
        If vADD_BOLD_STS = True Then
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)
            strHeight = TxtHgt
        End If


        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)

        Gst_dt = #7/1/2017#
        Entry_dt = dtp_Date.Value


        CurY = CurY + TxtHgt - 1
        If Trim(Common_Procedures.State_IdNoToName(con, Trim(prn_HdDt.Rows(0).Item("company_State_Idno").ToString))) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, " STATE : " & Trim(Common_Procedures.State_IdNoToName(con, Trim(prn_HdDt.Rows(0).Item("company_State_Idno").ToString))) & "   " & " GSTIN NO : " & Trim(prn_HdDt.Rows(0).Item("company_GSTinNo").ToString), LMargin + 10, CurY, 2, PrintWidth, pFont)
        End If
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTNo & "  /  " & Cmp_CstNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTNo, LMargin + 10, CurY, 0, 0, pFont)



        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "JOB ORDER FORM", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height



        CurY = CurY + 5
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1395" Then '---- SANTHA EXPORTS (SOMANUR)
        '    CurY = CurY + TxtHgt
        '    p1Font = New Font("Calibri", 11, FontStyle.Bold)
        '    Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC CODE : 998821 ( Textile manufactring service )", LMargin, CurY, 2, PrintWidth, p1Font)
        '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        'Else

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 10, FontStyle.Regular)
        'Common_Procedures.Print_To_PrintDocument(e, "( NOT FOR SALE )", LMargin + 10, CurY, 0, 0, p1Font)
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        'Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC CODE : 998821 ( Textile manufactring service )", PageWidth - 10, CurY, 1, 0, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        'Common_Procedures.Print_To_PrintDocument(e, "( NOT FOR SALE )", LMargin, CurY, 2, PrintWidth, p1Font)

        'End If

        CurY = CurY + strHeight  ' + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
        W1 = e.Graphics.MeasureString("D.C DATE  : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width
        M1 = ClAr(1) + ClAr(2)


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont,, True)
        Common_Procedures.Print_To_PrintDocument(e, "JobCard.No", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Weaving_JobCard_No").ToString, LMargin + C1 + W1 + 20, CurY, 0, 0, p1Font)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont,, True)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaving_JobCard_Date")), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 20, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont,, True)

        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1395" Then '---- SANTHA EXPORTS (SOMANUR)
            CurY = CurY + TxtHgt
            If Trim(Common_Procedures.State_IdNoToName(con, Trim(prn_HdDt.Rows(0).Item("Ledger_State_Idno").ToString))) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " STATE : " & Trim(Common_Procedures.State_IdNoToName(con, Trim(prn_HdDt.Rows(0).Item("Ledger_State_Idno").ToString))), LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If
        End If

        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, " GSTIN NO : " & Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
            strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, p1Font).Width
            CurX = LMargin + S1 + 10 + strWidth
            Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("Pan_No").ToString, CurX, CurY, 0, PrintWidth, pFont)
        End If


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10


        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO OF BAGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO OF CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CONE STOCK", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "CONE STOCK", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + 15, 2, ClAr(7), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "RECEIVED UPTO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY





        'CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


        'If Val(prn_HdDt.Rows(0).Item("Wrap_Length").ToString) <> 0 Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Wrap Length", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Wrap_Length").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)
        'End If
        'CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        'If Val(prn_HdDt.Rows(0).Item("Sizing_Length").ToString) <> 0 Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Sizing Length", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Sizing_Length").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)

        'End If

        'CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        'If Val(prn_HdDt.Rows(0).Item("Beam_Length").ToString) <> 0 Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Beam Length", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Beam_Length").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)
        'End If
        'CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        'If Val(prn_HdDt.Rows(0).Item("Pick_Up").ToString) <> 0 Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Pick Up", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Pick_Up").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)
        'End If

        'CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        'If Val(prn_HdDt.Rows(0).Item("Elongation").ToString) <> 0 Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Elongation", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Elongation").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)
        'End If

        'CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        'If Val(prn_HdDt.Rows(0).Item("Beam_Requirement_Date").ToString) <> 0 Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Beam Requirement Date", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Beam_Requirement_Date")), "dd-MM-yyyy".ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)
        'End If



        'CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        'Common_Procedures.Print_To_PrintDocument(e, "Remarks :", LMargin + 10, CurY, 0, 0, p1Font)

        'CurY = CurY + TxtHgt

        'CurY = CurY + TxtHgt


        'Common_Procedures.Print_To_PrintDocument(e, "1.Check Mill, Count, Lot & Warp Ends", LMargin + 10, CurY, 0, 0, pFont)
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "2.Try to take max warp length; cut cone should be at 30 grms or less", LMargin + 10, CurY, 0, 0, pFont)

        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "3.If any cone found damage and yarn quality issues, please inform before", LMargin + 10, CurY, 0, 0, pFont)


        'CurY = CurY + TxtHgt + 5
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)






        'p1Font = New Font("Calibri", 12, FontStyle.Bold)

        'Common_Procedures.Print_To_PrintDocument(e, "Note :", LMargin + 20, CurY, 0, 0, pFont)
        'CurY = CurY + TxtHgt
        'CurY = CurY + TxtHgt
        'CurY = CurY + TxtHgt
        'CurY = CurY + TxtHgt



    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim W1 As Single
        Dim M1 As Single
        Dim Area_Nm As String = ""
        Dim LedAdd1 As String = ""
        Dim LedAdd2 As String = ""
        Dim LedAdd3 As String = ""
        Dim LedAdd4 As String = ""
        Dim Loom_Typ As String = ""
        Dim Count_Typ As String = ""
        Dim Mill_Typ As String = ""


        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        W1 = e.Graphics.MeasureString(" Vehicle No :", pFont).Width

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY



        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + ClAr(2) + 15, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_weight").ToString), " #######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

        End If

        CurY = CurY + TxtHgt
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





        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5))
        M1 = ClAr(1) + ClAr(2)

        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "SIZING PROGRAM :", LMargin + 10, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'LnAr(4) = CurY
        Loom_Typ = Common_Procedures.LoomType_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Loom_Type_idno").ToString))
        If Val(prn_HdDt.Rows(0).Item("Loom_Type_Idno").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "Loom Type", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Loom_Typ, LMargin + M1 + 30, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


        If Trim(prn_HdDt.Rows(0).Item("Fabric_Weave").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "Fabric Width / Weave", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Fabric_Width").ToString) & prn_HdDt.Rows(0).Item("Fabric_Weave").ToString, LMargin + M1 + 30, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        Common_Procedures.Print_To_PrintDocument(e, "Beam Length", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Beam_Length").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        Common_Procedures.Print_To_PrintDocument(e, "Beam Width", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Beam_Width").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        If Trim(prn_HdDt.Rows(0).Item("ends_name").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "Ends", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("ends_name").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1266" Then

            If Trim(prn_HdDt.Rows(0).Item("Ends_Name_2").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Ends for single", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Ends_Name_2").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)
            End If

        Else

            If Val(prn_HdDt.Rows(0).Item("creel").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "No Of Creel  ", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("creel").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)
            End If
        End If


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        Common_Procedures.Print_To_PrintDocument(e, "No Of Set", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("no_of_set").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


        Common_Procedures.Print_To_PrintDocument(e, "No Of Cones Required", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("no_of_cones_required").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        If Val(prn_HdDt.Rows(0).Item("No_Of_Beams").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "No Of Beams", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("No_Of_Beams").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        If Val(prn_HdDt.Rows(0).Item("Beam_Requirement_Date").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "Beam Requirement Date", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Beam_Requirement_Date")), "dd-MM-yyyy".ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)
        End If

        'p1Font = New Font("Calibri", 16, FontStyle.Bold)
        'Common_Procedures.Print_To_PrintDocument(e, "YARN :", LMargin + 10, CurY, 0, 0, p1Font)

        'CurY = CurY + TxtHgt + 10
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        ''LnAr(4) = CurY

        'Count_Typ = Common_Procedures.Count_IdNoToName(con, Val(prn_DetDt.Rows(0).Item("Count_IdNo").ToString))
        'If Val(prn_DetDt.Rows(0).Item("Count_IdNo").ToString) <> 0 Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Count", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Count_Typ, LMargin + M1 + 30, CurY, 0, 0, pFont)
        'End If

        'CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        ''LnAr(4) = CurY
        'If Trim(prn_DetDt.Rows(0).Item("Yarn_Type").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Type", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(0).Item("Yarn_Type").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)
        'End If
        'CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        '' LnAr(4) = CurY

        'Mill_Typ = Common_Procedures.Mill_IdNoToName(con, Val(prn_DetDt.Rows(0).Item("Mill_IdNo").ToString))
        'If Val(prn_DetDt.Rows(0).Item("Mill_IdNo").ToString) <> 0 Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Mill", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Mill_Typ, LMargin + M1 + 30, CurY, 0, 0, pFont)
        'End If

        'CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        ''LnAr(4) = CurY
        'If Trim(prn_HdDt.Rows(0).Item("Total_Cones").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "No Of Cones", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)
        'End If

        'CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        ''LnAr(4) = CurY
        'If Trim(prn_HdDt.Rows(0).Item("Total_Bags").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "No Of Bags", LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + M1 + 30, CurY, 0, 0, pFont)
        'End If

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub btn_PDF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_PDF.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        print_record()
    End Sub

    Private Sub btn_EMail_Click(sender As Object, e As EventArgs) Handles btn_EMail.Click
        Dim Led_IdNo As Integer
        Dim MailTxt As String

        Try
            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Sizing_Name.Text)

            MailTxt = "INVOICE " & vbCrLf & vbCrLf
            MailTxt = MailTxt & "Invoice No.-" & Trim(lbl_JobNo.Text) & vbCrLf & "Date-" & Trim(dtp_Date.Text)
            ' MailTxt = MailTxt & vbCrLf & "Lr No.-" & Trim(txt_LrNo.Text) & IIf(Trim(msk_Lr_Date.Text) <> "", " Dt.", "") & Trim(msk_Lr_Date.Text)
            'MailTxt = MailTxt & vbCrLf & "Value-" & Trim(lbl_NetAmount.Text)

            EMAIL_Entry.vMailID = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Mail", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")
            EMAIL_Entry.vSubJect = "Invocie : " & Trim(lbl_JobNo.Text)
            EMAIL_Entry.vMessage = Trim(MailTxt)

            Dim f1 As New EMAIL_Entry
            f1.MdiParent = MDIParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND MAIL...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub txt_No_of_set_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Loom_Reed.KeyDown
        If e.KeyCode = 38 Then
            ' txt_Ends_2.Focus()
        End If
        If e.KeyCode = 40 Then
            ' txt_EPI.Focus()
        End If
    End Sub

    Private Sub txt_No_of_set_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Loom_Reed.KeyPress
        If Asc(e.KeyChar) = 13 Then
            '  txt_EPI.Focus()
        End If
    End Sub
    Private Sub txt_Beam_Width_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Packing.KeyDown
        If e.KeyCode = 38 Then
            ' txt_elongation.Focus()
        End If
        If e.KeyCode = 40 Then
            'txt_Remarks.Focus()
        End If
    End Sub

    Private Sub txt_Beam_Width_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Packing.KeyPress
        If Asc(e.KeyChar) = 13 Then
            'txt_Remarks.Focus()
        End If
    End Sub
    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        cbo_Ledger.Tag = cbo_Ledger.Text
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'WEAVER'  or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub
    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, dtp_Date, cbo_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'WEAVER'  or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, cbo_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'WEAVER'  or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub
    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.MDI_LedType = "WEAVER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ledger.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub
    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        cbo_Transport.Tag = cbo_Transport.Text
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'TRANSPORT' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub



    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, txt_Packing, cbo_Slevedge, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'TRANSPORT'  or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, cbo_Slevedge, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'TRANSPORT'  or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
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
    Private Sub Cbo_Delivery_To_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Delivery_To.GotFocus
        Cbo_Delivery_To.Tag = Cbo_Delivery_To.Text
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type='WEAVER' or Show_In_All_Entry = 1 ) ", "(Ledger_idno = 0)")
    End Sub
    Private Sub Cbo_Delivery_To_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Delivery_To.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Delivery_To, cbo_Sizing_Name, txt_Total_Meters, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type='WEAVER' or Show_In_All_Entry = 1 ) ", "(Ledger_idno = 0)")
    End Sub
    Private Sub Cbo_Delivery_To_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_Delivery_To.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Delivery_To, txt_Total_Meters, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type='WEAVER' or Show_In_All_Entry = 1 ) ", "(Ledger_idno = 0)")
    End Sub
    Private Sub Cbo_Delivery_To_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Delivery_To.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

            Common_Procedures.MDI_LedType = "WEAVER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_Delivery_To.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub
    Private Sub cbo_Slevedge_GotFocus(sender As Object, e As EventArgs) Handles cbo_Slevedge.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Slevedge_Head", "Slevedge_Name", "", "(Slevedge_Idno=0)")
    End Sub
    Private Sub cbo_Slevedge_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Slevedge.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Slevedge, cbo_Transport, cbo_Sizing_Name, "Slevedge_Head", "Slevedge_Name", "", "(Slevedge_Idno=0)")
    End Sub
    Private Sub cbo_Slevedge_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Slevedge.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Slevedge, cbo_Sizing_Name, "Slevedge_Head", "Slevedge_Name", "", "(Slevedge_Idno=0)")
    End Sub
    Private Sub cbo_Slevedge_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Slevedge.KeyUp

        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Slevedge_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Slevedge.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Cloth.GotFocus
        'vCbo_ItmNm = Trim(cbo_Cloth.Text)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
        cbo_Cloth.Tag = Trim(cbo_Cloth.Text)

    End Sub
    Private Sub cbo_Cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Cloth, cbo_Ledger, Cbo_Warp_Count, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub
    Private Sub cbo_Cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Cloth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Cloth, Cbo_Warp_Count, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
        'If Asc(e.KeyChar) = 13 Then
        '    If Trim(cbo_Cloth.Tag) <> Trim(cbo_Cloth.Text) Then

        '        Get_Cloth_Details()

        '    End If
        'End If
    End Sub
    Private Sub cbo_Cloth_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Cloth.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub
    Private Sub Cbo_Warp_Count_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_Warp_Count.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Warp_Count, Cbo_Weft_Count, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub
    Private Sub Cbo_Warp_Count_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Warp_Count.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Warp_Count, cbo_Cloth, Cbo_Weft_Count, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub
    Private Sub Cbo_Warp_Count_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Warp_Count.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_Warp_Count.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub Cbo_Weft_Count_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_Weft_Count.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Weft_Count, cbo_Ends_Count, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub
    Private Sub Cbo_Weft_Count_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Weft_Count.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Weft_Count, Cbo_Warp_Count, cbo_Ends_Count, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub
    Private Sub Cbo_Weft_Count_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Weft_Count.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_Weft_Count.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub cbo_Ends_Count_GotFocus(sender As Object, e As EventArgs) Handles cbo_Ends_Count.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_Ends_Count_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Ends_Count.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ends_Count, Cbo_Weft_Count, txt_EPI, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_Ends_Count_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Ends_Count.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ends_Count, txt_EPI, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_Ends_Count_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Ends_Count.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New EndsCount_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Ends_Count.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub txt_PPI_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_PPI.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub
    Private Sub txt_Width_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Width.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub
    Private Sub txt_Total_Meters_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Total_Meters.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub
    Private Sub txt_Rate_Meters_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Rate_Meters.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub
    Private Sub txt_Noof_Looms_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Noof_Looms.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub
    Private Sub Get_Cloth_Details()


        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim Cloth_ID As Integer = 0
        Dim SNo As Integer = 0


        Try
            '- NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_JobNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            If Trim(cbo_Cloth.Tag) <> Trim(cbo_Cloth.Text) Then

                Cloth_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)

                If Val(Cloth_ID) <> 0 Then

                    da1 = New SqlClient.SqlDataAdapter("select Ch.*,WrpCnt.Count_name as WarpCount_Name,WftCnt.Count_Name as WeftCount_Name , EndsCnt.EndsCount_name , EndsCnt.Ends_Name ,SlevHd.Slevedge_Name from Cloth_Head Ch   " &
                                                      " LEFT OUTER JOIN Count_Head WrpCnt on Ch.Cloth_WarpCount_IdNo = WrpCnt.Count_Idno  " &
                                                      " LEFT OUTER JOIN Count_Head WftCnt on Ch.Cloth_WeftCount_IdNo = WftCnt.Count_Idno  " &
                                                      " LEFT OUTER JOIN EndsCount_head EndsCnt on Ch.EndsCount_IdNo = EndsCnt.EndsCount_IdNo  " &
                                                      " LEFT OUTER JOIN Slevedge_Head SlevHd on Ch.Slevedge_Type_Idno = SlevHd.Slevedge_IdNo  " &
                                                      " where Cloth_Idno= " & Val(Cloth_ID) & " ", con)
                    dt1 = New DataTable
                    da1.Fill(dt1)

                    If dt1.Rows.Count > 0 Then

                        Cbo_Warp_Count.Text = dt1.Rows(0).Item("WarpCount_Name").ToString
                        Cbo_Weft_Count.Text = dt1.Rows(0).Item("WeftCount_Name").ToString
                        txt_EPI.Text = dt1.Rows(0).Item("Cloth_Reed").ToString
                        txt_PPI.Text = dt1.Rows(0).Item("Cloth_Pick").ToString
                        txt_Width.Text = dt1.Rows(0).Item("Cloth_Width").ToString
                        txt_Weave.Text = dt1.Rows(0).Item("Weave").ToString
                        cbo_Slevedge.Text = dt1.Rows(0).Item("Slevedge_Name").ToString

                        cbo_Ends_Count.Text = dt1.Rows(0).Item("EndsCount_Name").ToString
                        txt_Noof_Ends.Text = dt1.Rows(0).Item("Ends_Name").ToString


                    End If
                End If
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR ON WHILE GETTING CLOTH DETAILS", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End Try


    End Sub
    Private Sub cbo_Cloth_Leave(sender As Object, e As EventArgs) Handles cbo_Cloth.Leave
        If Trim(cbo_Cloth.Tag) <> Trim(cbo_Cloth.Text) Then

            Get_Cloth_Details()

        End If
    End Sub
    Private Sub dgv_Filter_Details_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellClick
        Open_FilterEntry()
    End Sub
    Private Sub cbo_Filter_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ClothName.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub
    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'WEAVER'  or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub
    Private Sub cbo_Filter_SizingName_Name_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_SizingName.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_idno = 0 or Ledger_Type = 'SIZING' or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub btn_Print_Click(sender As Object, e As EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = False
        print_record()
    End Sub
    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)

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
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim ps As Printing.PaperSize
        Dim vConeStk As Long = 0
        Dim vWGTStk As String = 0
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_GSTNo As String
        Dim strHeight As Single
        Dim C1 As Single
        Dim W1 As Single
        Dim M1 As Single
        Dim S1 As Single
        Dim L1 As Single
        Dim vSideLine As Single
        Dim vAdjustCurY As Single
        Dim Gst_dt As Date
        Dim Entry_dt As Date
        Dim Count_Typ As String = ""
        Dim Mill_Typ As String = ""
        Dim strWidth As Single = 0
        Dim CurX As Single = 0
        Dim Loom_Typ As String = ""


        'set_PaperSize_For_PrintDocument1()

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30 ' 40
            .Right = 45
            .Top = 40 '50 ' 60
            .Bottom = 40


            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Bold)

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

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = 35 : ClAr(2) = 130 : ClAr(3) = 170 : ClAr(4) = 85 : ClAr(5) = 85 : ClAr(6) = 90
        ClAr(7) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6))

        NoofItems_PerPage = 15
        ' If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) = "" Then
        NoofItems_PerPage = NoofItems_PerPage + 1
        'End If

        If Val(Common_Procedures.settings.Printing_For_HalfSheet_Set_Custom8X6_As_Default_PaperSize) = 1 Then
            TxtHgt = 17.5
        Else
            TxtHgt = 17
        End If

        Try
            If prn_HdDt.Rows.Count > 0 Then

                CurY = TMargin


                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                LnAr(1) = CurY

                Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
                Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_GSTNo = ""

                Dim vADD_BOLD_STS As Boolean = False


                vADD_BOLD_STS = False
                If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
                    Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                    Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")
                    Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString)

                    Dim vADDR1 As String = ""

                    vADDR1 = Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString)
                    vADDR1 = Replace(vADDR1, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) & ",", "")
                    vADDR1 = Replace(vADDR1, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

                    Cmp_Add2 = vADDR1 & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
                    vADD_BOLD_STS = True

                Else

                    Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
                    Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
                    Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

                End If


                If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
                    Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
                End If
                If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
                    Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
                End If
                If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
                    Cmp_CstNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
                End If

                CurY = CurY + TxtHgt - 10
                p1Font = New Font("Calibri", 18, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
                strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

                CurY = CurY + strHeight - 3
                If vADD_BOLD_STS = True Then
                    p1Font = New Font("Calibri", 14, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font)
                    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
                Else
                    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)
                    strHeight = TxtHgt
                End If


                CurY = CurY + strHeight - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)

                Gst_dt = #7/1/2017#
                Entry_dt = dtp_Date.Value


                CurY = CurY + TxtHgt - 1
                If Trim(Common_Procedures.State_IdNoToName(con, Trim(prn_HdDt.Rows(0).Item("company_State_Idno").ToString))) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, " STATE : " & Trim(Common_Procedures.State_IdNoToName(con, Trim(prn_HdDt.Rows(0).Item("company_State_Idno").ToString))) & "   " & " GSTIN NO : " & Trim(prn_HdDt.Rows(0).Item("company_GSTinNo").ToString), LMargin + 10, CurY, 2, PrintWidth, pFont)
                End If
                Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTNo, LMargin + 10, CurY, 0, 0, pFont)



                CurY = CurY + TxtHgt
                p1Font = New Font("Calibri", 16, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "WEAVING JOB WORK SHEET", LMargin, CurY, 2, PrintWidth, p1Font)
                'Common_Procedures.Print_To_PrintDocument(e, "JOB ORDER FORM", LMargin, CurY, 2, PrintWidth, p1Font)
                strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

                CurY = CurY + strHeight
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                LnAr(2) = CurY

                C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
                L1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 20
                W1 = e.Graphics.MeasureString("D.C DATE  : ", pFont).Width
                S1 = e.Graphics.MeasureString("TO     :  ", pFont).Width
                M1 = ClAr(1) + ClAr(2) + ClAr(3) - 50


                CurY = CurY + TxtHgt - 10
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)

                Common_Procedures.Print_To_PrintDocument(e, "JW NO", LMargin + L1, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + 20, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Weaving_JobCard_No").ToString, LMargin + C1 + S1, CurY, 0, 0, p1Font)


                CurY = CurY + TxtHgt + 10
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont,)

                Common_Procedures.Print_To_PrintDocument(e, "DATE ", LMargin + L1, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + 20, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaving_JobCard_Date")), "dd-MM-yyyy").ToString, LMargin + C1 + S1, CurY, 0, 0, p1Font)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont,)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont,)


                CurY = CurY + TxtHgt
                If Trim(Common_Procedures.State_IdNoToName(con, Trim(prn_HdDt.Rows(0).Item("Ledger_State_Idno").ToString))) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, " STATE : " & Trim(Common_Procedures.State_IdNoToName(con, Trim(prn_HdDt.Rows(0).Item("Ledger_State_Idno").ToString))), LMargin + S1 + 10, CurY, 0, 0, pFont)
                End If


                CurY = CurY + TxtHgt
                If Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, " GSTIN NO : " & Trim(prn_HdDt.Rows(0).Item("ledger_GSTinNo").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)
                End If

                If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
                    strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, p1Font).Width
                    CurX = LMargin + S1 + 10 + strWidth
                    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("Pan_No").ToString, CurX, CurY, 0, PrintWidth, pFont)
                End If

                CurY = CurY + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                LnAr(3) = CurY

                e.Graphics.DrawLine(Pens.Black, LMargin + L1 - 20, LnAr(2), LMargin + L1 - 20, CurY)

                CurY = CurY + TxtHgt - 5
                p1Font = New Font("Calibri", 14, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "FABRIC DETAILS", LMargin + L1 - 20, CurY - 5, 1, 0, p1Font)

                vSideLine = 20
                vAdjustCurY = 5

                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin + vSideLine, CurY, PageWidth - vSideLine, CurY)
                LnAr(4) = CurY


                pFont = New Font("Calibri", 12, FontStyle.Regular)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Warp Count", LMargin + vSideLine + 10, CurY - vAdjustCurY, 0, 0, pFont)
                If Trim(prn_HdDt.Rows(0).Item("WarpCount_Name").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("WarpCount_Name").ToString), LMargin + M1 - 10, CurY - vAdjustCurY, 0, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin + vSideLine, CurY, PageWidth - vSideLine, CurY)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Weft Count", LMargin + vSideLine + 10, CurY - vAdjustCurY, 0, 0, pFont)
                If Trim(prn_HdDt.Rows(0).Item("WeftCount_Name").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("WeftCount_Name").ToString), LMargin + M1 - 10, CurY - vAdjustCurY, 0, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin + vSideLine, CurY, PageWidth - vSideLine, CurY)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "EPI", LMargin + vSideLine + 10, CurY - vAdjustCurY, 0, 0, pFont)
                If Val(prn_HdDt.Rows(0).Item("Ends_Per_Inch").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Ends_Per_Inch").ToString), LMargin + M1 - 10, CurY - vAdjustCurY, 0, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin + vSideLine, CurY, PageWidth - vSideLine, CurY)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "PPI", LMargin + vSideLine + 10, CurY - vAdjustCurY, 0, 0, pFont)
                If Val(prn_HdDt.Rows(0).Item("Pick_Per_Inch").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Pick_Per_Inch").ToString), LMargin + M1 - 10, CurY - vAdjustCurY, 0, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin + vSideLine, CurY, PageWidth - vSideLine, CurY)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Width Of the Fabric", LMargin + vSideLine + 10, CurY - vAdjustCurY, 0, 0, pFont)
                If Val(prn_HdDt.Rows(0).Item("cloth_Width").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("cloth_Width").ToString), LMargin + M1 - 10, CurY - vAdjustCurY, 0, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin + vSideLine, CurY, PageWidth - vSideLine, CurY)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Loom Reed", LMargin + vSideLine + 10, CurY - vAdjustCurY, 0, 0, pFont)
                If Val(prn_HdDt.Rows(0).Item("Loom_Reed").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Loom_Reed").ToString), LMargin + M1 - 10, CurY - vAdjustCurY, 0, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin + vSideLine, CurY, PageWidth - vSideLine, CurY)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "No of Ends", LMargin + vSideLine + 10, CurY - vAdjustCurY, 0, 0, pFont)
                If Val(prn_HdDt.Rows(0).Item("No_Of_Ends").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("No_Of_Ends").ToString), LMargin + M1 - 10, CurY - vAdjustCurY, 0, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin + vSideLine, CurY, PageWidth - vSideLine, CurY)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Total Meters", LMargin + vSideLine + 10, CurY - vAdjustCurY, 0, 0, pFont)
                If Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Total_Meters").ToString), LMargin + M1 - 10, CurY - vAdjustCurY, 0, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin + vSideLine, CurY, PageWidth - vSideLine, CurY)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Weave", LMargin + vSideLine + 10, CurY - vAdjustCurY, 0, 0, pFont)
                If Trim(prn_HdDt.Rows(0).Item("Cloth_Weave").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Cloth_Weave").ToString), LMargin + M1 - 10, CurY - vAdjustCurY, 0, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin + vSideLine, CurY, PageWidth - vSideLine, CurY)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Price/Meters", LMargin + vSideLine + 10, CurY - vAdjustCurY, 0, 0, pFont)
                If Val(prn_HdDt.Rows(0).Item("Rate_Per_Meter").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Rate_Per_Meter").ToString), LMargin + M1 - 10, CurY - vAdjustCurY, 0, 0, pFont)
                End If


                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin + vSideLine, CurY, PageWidth - vSideLine, CurY)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Delivery", LMargin + vSideLine + 10, CurY - vAdjustCurY, 0, 0, pFont)
                If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_Name").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("DeliveryTo_Name").ToString), LMargin + M1 - 10, CurY - vAdjustCurY, 0, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin + vSideLine, CurY, PageWidth - vSideLine, CurY)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "No of Looms", LMargin + vSideLine + 10, CurY - vAdjustCurY, 0, 0, pFont)
                If Val(prn_HdDt.Rows(0).Item("No_Of_Looms").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("No_Of_Looms").ToString), LMargin + M1 - 10, CurY - vAdjustCurY, 0, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin + vSideLine, CurY, PageWidth - vSideLine, CurY)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Selvedge", LMargin + vSideLine + 10, CurY - vAdjustCurY, 0, 0, pFont)
                If Trim(prn_HdDt.Rows(0).Item("Slevedge_Name").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Slevedge_Name").ToString), LMargin + M1 - 10, CurY - vAdjustCurY, 0, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin + vSideLine, CurY, PageWidth - vSideLine, CurY)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Transport", LMargin + vSideLine + 10, CurY - vAdjustCurY, 0, 0, pFont)
                If Trim(prn_HdDt.Rows(0).Item("TransportName").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("TransportName").ToString), LMargin + M1 - 10, CurY - vAdjustCurY, 0, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin + vSideLine, CurY, PageWidth - vSideLine, CurY)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Packing", LMargin + vSideLine + 10, CurY - vAdjustCurY, 0, 0, pFont)
                If Trim(prn_HdDt.Rows(0).Item("Packing").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Packing").ToString), LMargin + M1 - 10, CurY - vAdjustCurY, 0, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin + vSideLine, CurY, PageWidth - vSideLine, CurY)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Fabric Length", LMargin + vSideLine + 10, CurY - vAdjustCurY, 0, 0, pFont)
                If Trim(prn_HdDt.Rows(0).Item("Cloth_Length_Terms").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Cloth_Length_Terms").ToString), LMargin + M1 - 10, CurY - vAdjustCurY, 0, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin + vSideLine, CurY, PageWidth - vSideLine, CurY)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Sizing Name", LMargin + vSideLine + 10, CurY - vAdjustCurY, 0, 0, pFont)
                If Trim(prn_HdDt.Rows(0).Item("Sizing_Name").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Sizing_Name").ToString), LMargin + M1 - 10, CurY - vAdjustCurY, 0, 0, pFont)
                End If


                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin + vSideLine, CurY, PageWidth - vSideLine, CurY)

                e.Graphics.DrawLine(Pens.Black, LMargin + vSideLine, LnAr(4), LMargin + 20, CurY)     ' --LEFT
                e.Graphics.DrawLine(Pens.Black, PageWidth - vSideLine, LnAr(4), PageWidth - vSideLine, CurY)     '---RIGHT
                e.Graphics.DrawLine(Pens.Black, LMargin + M1 - 20, LnAr(4), LMargin + M1 - 20, CurY)   '---CENTER


                CurY = CurY + TxtHgt + 20
                CurY = CurY + TxtHgt
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Prepared By", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Vendor", LMargin + M1, CurY, 2, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "For " & Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString), PageWidth - 10, CurY, 1, 0, p1Font)

                CurY = CurY + TxtHgt + 10

                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

                e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
                e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error On  While  Printing", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub
End Class