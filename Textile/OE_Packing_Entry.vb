Public Class OE_Packing_Entry

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "CTPAK-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String
    Private WithEvents dgtxt_PavuDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_YarnDetails As New DataGridViewTextBoxEditingControl
    Private dgv_ActiveCtrl_Name As String

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private Enum dgvCol_Details As Integer

        SL_NO           '0
        BAG_NO          '1
        COUNT_NAME      '2
        CONES           '3
        CONE_TYPE       '4
        GROSS_WEIGHT    '5
        TARE_WEIGHT     '6
        NET_WEIGHT      '7
        COTTON_INVOICE_CODE '8


    End Enum
    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        cbo_PackerName.Text = ""
        Cbo_Count.Text = ""
        cbo_CoNETYPE.Text = ""
        cbo_CoNETYPE.SelectedIndex = -1
        Cbo_TipType.Text = ""
        Cbo_Grid_ConeType.Text = ""
        Cbo_Grid_ConeType.SelectedIndex = -1

        cbo_Filter_CountName.Text = ""
        cbo_Filter_PartyName.Text = ""
        cbo_Filter_Colour.Text = ""
        Txt_TotalBags.Text = ""
        Txt_BagsFrom.Text = ""
        Txt_GrossWeight.Text = ""
        Txt_TareWeight.Text = ""
        Txt_NoCones.Text = ""
        lbl_BagsTo.Text = ""
        lbl_NetWeight.Text = ""
        Cbo_Grid_Count_Name.Text = ""

        dgv_PavuDetails.Rows.Clear()
        'dgv_PavuDetails.Rows.Add()

        dgv_PavuDetails_Total.Rows.Clear()
        dgv_PavuDetails_Total.Rows.Add()
        dgv_PavuDetails.AllowUserToAddRows = False

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""
            cbo_Filter_Colour.Text = ""
            cbo_Filter_Colour.SelectedIndex = -1
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        cbo_CoNETYPE.Enabled = True
        cbo_CoNETYPE.BackColor = Color.White

        Cbo_Count.Enabled = True
        Cbo_Count.BackColor = Color.White

        Txt_TotalBags.Enabled = True
        Txt_TotalBags.BackColor = Color.White

        Txt_BagsFrom.Enabled = True
        Txt_BagsFrom.BackColor = Color.White

        lbl_BagsTo.Enabled = True
        lbl_BagsTo.BackColor = Color.White

        Txt_GrossWeight.Enabled = True
        Txt_GrossWeight.BackColor = Color.White

        Txt_TareWeight.Enabled = True
        Txt_TareWeight.BackColor = Color.White

        lbl_NetWeight.Enabled = True
        lbl_NetWeight.BackColor = Color.White


        dgv_PavuDetails.ReadOnly = False


        Grid_Cell_DeSelect()

        NoCalc_Status = False
        dgv_ActiveCtrl_Name = ""
        Cbo_Grid_ConeType.Visible = False

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

        If Me.ActiveControl.Name <> Cbo_Grid_ConeType.Name Then
            Cbo_Grid_ConeType.Visible = False
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub
    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_PavuDetails.CurrentCell) Then dgv_PavuDetails.CurrentCell.Selected = False
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

    Private Sub OE_Packing_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PackerName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PackerName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_CoNETYPE.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CONETYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_CoNETYPE.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_Count.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                Cbo_Count.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_Grid_Count_Name.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                Cbo_Grid_Count_Name.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_Grid_ConeType.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CONETYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                Cbo_Grid_ConeType.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(Cbo_TipType.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = " TIPTYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                Cbo_TipType.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub OE_Packing_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub OE_Packing_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
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

    Private Sub OE_Packing_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable

        Me.Text = ""

        con.Open()

        dtp_Date.Text = ""

        dgv_PavuDetails.AllowUserToAddRows = False
        dgv_PavuDetails.Columns(dgvCol_Details.CONE_TYPE).Visible = False
        dgv_PavuDetails.Columns(dgvCol_Details.COUNT_NAME).Visible = False

        dgv_PavuDetails_Total.Columns(dgvCol_Details.CONE_TYPE).Visible = False
        dgv_PavuDetails_Total.Columns(dgvCol_Details.COUNT_NAME).Visible = False
        Cbo_Grid_ConeType.Visible = False

        da = New SqlClient.SqlDataAdapter("select distinct(Cone_Type_Name) from Cone_Type_Head order by Cone_Type_Name", con)
        da.Fill(dt1)
        Cbo_Grid_ConeType.DataSource = dt1
        Cbo_Grid_ConeType.DisplayMember = "Cone_Type_Name"
        Cbo_Grid_ConeType.SelectedIndex = -1

        da = New SqlClient.SqlDataAdapter("select distinct(TipType_Name) from TipType_Head order by TipType_Name", con)
        da.Fill(dt2)
        Cbo_TipType.DataSource = dt2
        Cbo_TipType.DisplayMember = "TipType_Name"
        Cbo_TipType.SelectedIndex = -1



        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
            dgv_PavuDetails.Columns(dgvCol_Details.GROSS_WEIGHT).ReadOnly = False
            dgv_PavuDetails.AllowUserToAddRows = False
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then

            dgv_PavuDetails.ReadOnly = False
            dgv_PavuDetails.Columns(dgvCol_Details.BAG_NO).ReadOnly = False
            dgv_PavuDetails.Columns(dgvCol_Details.COUNT_NAME).ReadOnly = False
            dgv_PavuDetails.Columns(dgvCol_Details.CONES).ReadOnly = False
            dgv_PavuDetails.Columns(dgvCol_Details.GROSS_WEIGHT).ReadOnly = False
            dgv_PavuDetails.Columns(dgvCol_Details.TARE_WEIGHT).ReadOnly = False
            'dgv_PavuDetails.Columns(dgvCol_Details.NET_WEIGHT).ReadOnly = False

            dgv_PavuDetails.Columns(dgvCol_Details.BAG_NO).HeaderText = "PALLET NO"


            dgv_PavuDetails.Columns(dgvCol_Details.CONE_TYPE).Visible = True
            dgv_PavuDetails.Columns(dgvCol_Details.COUNT_NAME).Visible = True
            dgv_PavuDetails_Total.Columns(dgvCol_Details.COUNT_NAME).Visible = True


            lbl_ConeType.Visible = False
            cbo_CoNETYPE.Visible = False

            lbl_TipType.Visible = True
            Cbo_TipType.Visible = True
            Cbo_TipType.BackColor = Color.White

            'lbl_TipType.Left = lbl_ConeType.Left
            'Cbo_TipType.Left = cbo_CoNETYPE.Left
            'Cbo_TipType.Width = cbo_CoNETYPE.Width


            lbl_Bags.Text = "Pallet"
            lbl_Bags_From.Text = "Pallet From"
            lbl_Bags_To.Text = "Pallet To"

            Cbo_Count.Visible = False

            lbl_TipType.Left = Label2.Left
            Cbo_TipType.Left = lbl_RefNo.Left
            Cbo_TipType.Width = cbo_PackerName.Width

        Else


            dgv_PavuDetails.Columns(dgvCol_Details.BAG_NO).Width = 110
            dgv_PavuDetails.Columns(dgvCol_Details.CONES).Width = 110
            dgv_PavuDetails.Columns(dgvCol_Details.CONE_TYPE).Width = 110
            dgv_PavuDetails.Columns(dgvCol_Details.GROSS_WEIGHT).Width = 100
            dgv_PavuDetails.Columns(dgvCol_Details.TARE_WEIGHT).Width = 100
            dgv_PavuDetails.Columns(dgvCol_Details.NET_WEIGHT).Width = 120

        End If

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PackerName.GotFocus, AddressOf ControlGotFocus
        '  AddHandler cbo_Cover.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_CoNETYPE.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler Txt_BagsFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler Txt_TotalBags.GotFocus, AddressOf ControlGotFocus
        AddHandler Txt_GrossWeight.GotFocus, AddressOf ControlGotFocus
        AddHandler Txt_TareWeight.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Grid_Count_Name.GotFocus, AddressOf ControlGotFocus


        AddHandler cbo_Filter_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Colour.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PackerName.LostFocus, AddressOf ControlLostFocus
        '  AddHandler cbo_Cover.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_CoNETYPE.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Count.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Colour.LostFocus, AddressOf ControlLostFocus
        AddHandler Txt_BagsFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler Txt_TotalBags.LostFocus, AddressOf ControlLostFocus
        AddHandler Txt_GrossWeight.LostFocus, AddressOf ControlLostFocus
        AddHandler Txt_TareWeight.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Grid_Count_Name.LostFocus, AddressOf ControlLostFocus


        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler Txt_BagsFrom.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler Txt_TotalBags.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler Txt_GrossWeight.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler Txt_TareWeight.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler Txt_BagsFrom.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler Txt_TotalBags.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler Txt_GrossWeight.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler Txt_TareWeight.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler Txt_NoCones.GotFocus, AddressOf ControlGotFocus
        AddHandler Txt_NoCones.LostFocus, AddressOf ControlLostFocus

        AddHandler Txt_NoCones.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler Txt_NoCones.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler Cbo_Grid_ConeType.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Grid_ConeType.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_TipType.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_TipType.LostFocus, AddressOf ControlLostFocus


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

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim LockSTS As Boolean = False
        Dim n As Integer = 0
        Dim I As Integer = 0, J As Integer = 0
        Dim SNo As Integer
        'Dim LockSTS As Boolean = False

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            da1 = New SqlClient.SqlDataAdapter("select a.* from Cotton_Packing_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_Packing_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Cotton_Packing_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Cotton_Packing_Date").ToString

                cbo_PackerName.Text = Trim(dt1.Rows(0).Item("Packer").ToString)
                '  cbo_PackerName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_CoNETYPE.Text = Common_Procedures.Conetype_IdNoToName(con, Val(dt1.Rows(0).Item("ConeType_IdNo").ToString))
                Cbo_Count.Text = Common_Procedures.Count_IdNoToName(con, Val(dt1.Rows(0).Item("Count_IdNo").ToString))
                Txt_TotalBags.Text = dt1.Rows(0).Item("Total_NoOf_Bag").ToString
                Txt_BagsFrom.Text = dt1.Rows(0).Item("Bags_From").ToString
                lbl_BagsTo.Text = dt1.Rows(0).Item("Bags_To").ToString
                Txt_GrossWeight.Text = dt1.Rows(0).Item("Gross_Weight").ToString
                Txt_TareWeight.Text = dt1.Rows(0).Item("Tare_Weight").ToString
                lbl_NetWeight.Text = dt1.Rows(0).Item("Net_Weight").ToString
                Txt_NoCones.Text = dt1.Rows(0).Item("No_of_Cones").ToString
                Cbo_TipType.Text = Common_Procedures.TipType_IdnoToName(con, Val(dt1.Rows(0).Item("TipType_Idno").ToString))



                da2 = New SqlClient.SqlDataAdapter("select a.* from Cotton_Packing_Details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_Packing_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_PavuDetails.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For I = 0 To dt2.Rows.Count - 1

                        n = dgv_PavuDetails.Rows.Add()

                        SNo = SNo + 1
                        dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.SL_NO).Value = Val(SNo)
                        dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.BAG_NO).Value = dt2.Rows(I).Item("Bag_No").ToString

                        If dgv_PavuDetails.Columns(dgvCol_Details.COUNT_NAME).Visible = True Then
                            dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.COUNT_NAME).Value = Common_Procedures.Count_IdNoToName(con, dt2.Rows(I).Item("Count_Idno").ToString)
                        End If

                        dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.GROSS_WEIGHT).Value = Format(Val(dt2.Rows(I).Item("Gross_Weight").ToString), "########0.000")
                        dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.TARE_WEIGHT).Value = Format(Val(dt2.Rows(I).Item("Tare_Weight").ToString), "########0.000")
                        dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.NET_WEIGHT).Value = Format(Val(dt2.Rows(I).Item("Net_Weight").ToString), "########0.000")
                        dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.CONES).Value = Val(dt2.Rows(I).Item("NoofCones").ToString)
                        dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.CONE_TYPE).Value = Common_Procedures.Conetype_IdNoToName(con, Val(dt2.Rows(I).Item("ConeType_IdNo").ToString))

                        dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.COTTON_INVOICE_CODE).Value = dt2.Rows(I).Item("Cotton_Invoice_Code").ToString

                        If Trim(dgv_PavuDetails.Rows(n).Cells(dgvCol_Details.COTTON_INVOICE_CODE).Value) <> "" Then
                            For J = 0 To dgv_PavuDetails.ColumnCount - 1
                                dgv_PavuDetails.Rows(n).Cells(J).Style.BackColor = Color.LightGray
                                dgv_PavuDetails.Rows(n).Cells(J).Style.ForeColor = Color.Red
                            Next J
                            LockSTS = True
                        End If


                    Next I

                End If


                TotalPavu_Calculation()
                dgv_PavuDetails.AllowUserToAddRows = False

            End If

            dt2.Clear()



            dt1.Clear()

            If LockSTS = True Then

                cbo_CoNETYPE.Enabled = False
                cbo_CoNETYPE.BackColor = Color.LightGray

                Cbo_Count.Enabled = False
                Cbo_Count.BackColor = Color.LightGray

                Txt_TotalBags.Enabled = False
                Txt_TotalBags.BackColor = Color.LightGray

                Txt_BagsFrom.Enabled = False
                Txt_BagsFrom.BackColor = Color.LightGray

                lbl_BagsTo.Enabled = False
                lbl_BagsTo.BackColor = Color.LightGray

                Txt_GrossWeight.Enabled = False
                Txt_GrossWeight.BackColor = Color.LightGray

                Txt_TareWeight.Enabled = False
                Txt_TareWeight.BackColor = Color.LightGray

                lbl_NetWeight.Enabled = False
                lbl_NetWeight.BackColor = Color.LightGray

                dgv_PavuDetails.ReadOnly = True

            End If
            dt1.Dispose()
            da1.Dispose()

            Grid_Cell_DeSelect()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If dtp_Date.Visible And dtp_Date.Enabled Then dtp_Date.Focus()

    End Sub
    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Cotton_Packing, "~L~") = 0 And InStr(Common_Procedures.UR.Cotton_Packing, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.OEENTRY_PACKING_ENTRY, New_Entry, Me, con, "cotton_packing_Head", "cotton_packing_Code", NewCode, "cotton_packing_Date", "(cotton_packing_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

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
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select * from Cotton_Packing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Packing_Code = '" & Trim(NewCode) & "' and  Cotton_invoice_Code <> ''", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Cotton_invoice_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("Cotton_invoice_Code").ToString) <> "" Then
                    MessageBox.Show("Already Packing Prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()




        Dt1.Dispose()
        Da.Dispose()
        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_LooseYarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Cotton_Packing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Packing_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Cotton_Packing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Packing_Code = '" & Trim(NewCode) & "'"
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

            If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable




            dtp_Filter_Fromdate.Text = ""
            dtp_Filter_ToDate.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_CountName.Text = ""


            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Filter.BringToFront()
        Pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Cotton_Packing_No from Cotton_Packing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Packing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Cotton_Packing_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Cotton_Packing_No from Cotton_Packing_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Packing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Cotton_Packing_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Cotton_Packing_No from Cotton_Packing_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Packing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Cotton_Packing_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Cotton_Packing_No from Cotton_Packing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Packing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Cotton_Packing_No desc", con)
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
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0
        Dim Newcode As String = ""


        Try
            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Cotton_Packing_Head", "Cotton_Packing_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red

            dtp_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 a.* from Cotton_Packing_Head a  where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Packing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Cotton_Packing_No desc", con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If dt.Rows(0).Item("Packer").ToString <> "" Then cbo_PackerName.Text = dt.Rows(0).Item("Packer").ToString
                cbo_CoNETYPE.Text = Common_Procedures.Conetype_IdNoToName(con, Val(dt.Rows(0).Item("ConeType_IdNo").ToString))
                Cbo_Count.Text = Common_Procedures.Count_IdNoToName(con, Val(dt.Rows(0).Item("Count_IdNo").ToString))
                Txt_GrossWeight.Text = dt.Rows(0).Item("Gross_Weight").ToString
                Txt_TareWeight.Text = dt.Rows(0).Item("Tare_Weight").ToString
                lbl_NetWeight.Text = dt.Rows(0).Item("Net_Weight").ToString
                Txt_NoCones.Text = dt.Rows(0).Item("No_of_Cones").ToString

                da1 = New SqlClient.SqlDataAdapter("select  a.* from Cotton_Packing_Details a  where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Packing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, a.Sl_No desc, cotton_packing_no desc ", con)
                dt1 = New DataTable
                da1.Fill(dt1)

                If dt1.Rows(0).Item("Bag_No").ToString <> "" Then Txt_BagsFrom.Text = dt1.Rows(0).Item("Bag_No").ToString + 1

            End If




            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()


        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        dt.Dispose()
        da.Dispose()

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView
        Dim i As Integer
        Dim vLASTCOL As Integer
        Dim vFIRSTCOL As Integer

        If ActiveControl.Name = dgv_PavuDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing
            If ActiveControl.Name = dgv_PavuDetails.Name Then
                dgv1 = dgv_PavuDetails

            ElseIf dgv_PavuDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_PavuDetails

            ElseIf dgv_ActiveCtrl_Name = dgv_PavuDetails.Name Then
                dgv1 = dgv_PavuDetails

            End If

            If IsNothing(dgv1) = True Then
                Return MyBase.ProcessCmdKey(msg, keyData)
                Exit Function
            End If

            With dgv1

                If dgv1.Name = dgv_PavuDetails.Name Then

                    vFIRSTCOL = dgvCol_Details.BAG_NO
                    vLASTCOL = dgvCol_Details.TARE_WEIGHT
                    'vFIRSTCOL = 2
                    'vLASTCOL = 2

                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If .CurrentCell.ColumnIndex >= vLASTCOL Then

                            If .CurrentCell.RowIndex = .RowCount - 1 Then

                                'If Trim(.CurrentRow.Cells(1).Value) <> "" Then
                                '    .Rows.Add()
                                '    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(vFIRSTCOL)

                                'Else
                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                    save_record()
                                Else
                                    dtp_Date.Focus()
                                End If
                                'End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(vFIRSTCOL)

                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= dgvCol_Details.BAG_NO And ((.CurrentCell.ColumnIndex <> dgvCol_Details.BAG_NO And Val(.CurrentRow.Cells(dgvCol_Details.BAG_NO).Value) = 0) Or (.CurrentCell.ColumnIndex = dgvCol_Details.BAG_NO And Val(dgtxt_PavuDetails.Text) = 0)) Then
                                For i = 0 To .Columns.Count - 1
                                    .Rows(.CurrentCell.RowIndex).Cells(i).Value = ""
                                Next

                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                    save_record()
                                Else
                                    dtp_Date.Focus()
                                End If

                            ElseIf .CurrentCell.ColumnIndex = dgvCol_Details.CONES Then
                                If .Columns(dgvCol_Details.CONE_TYPE).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.GROSS_WEIGHT)
                                End If

                            Else

                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= vFIRSTCOL Then
                            If .CurrentCell.RowIndex = 0 Then
                                Txt_TareWeight.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(vLASTCOL)

                            End If
                        ElseIf .CurrentCell.ColumnIndex = dgvCol_Details.GROSS_WEIGHT Then

                            If .Columns(dgvCol_Details.CONE_TYPE).Visible = True Then
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                            Else

                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_Details.CONES)
                            End If

                        Else

                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If

                End If

            End With

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function
    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        Try

            inpno = InputBox("Enter Bag.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Cotton_Packing_No from Cotton_Packing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Packing_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Bag No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Cotton_Packing, "~L~") = 0 And InStr(Common_Procedures.UR.Cotton_Packing, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.OEENTRY_MIXING_ENTRY, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Bag No.", "FOR NEW BAG NO INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Cotton_Packing_No from Cotton_Packing_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Packing_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Bag No", "DOES NOT INSERT NEW BAG NO...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW BAG NO...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Pak_ID As Integer = 0
        Dim Led1_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim CTy_idno As Integer = 0
        Dim Cnt_ID As Integer = 0
        Dim Grid_Cnt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Nr As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim YCnt_ID As Integer = 0
        Dim YMil_ID As Integer = 0
        Dim VconeType_ID As Integer = 0
        Dim VTipType_ID As Integer = 0
        Dim EntID As String = ""
        Dim vTotBags As Single = 0
        Dim vTotGrsWgt As Single = 0
        Dim vTotTareWgt As Single = 0
        Dim vTotNetWgt As Single = 0
        Dim Dup_SetNoBmNo As String = ""
        Dim Bag_Code As String = ""
        Dim vTotcns As Single = 0

        Dim Bag_NO1st As String = ""
        Dim Bag_Nolast As String = ""

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.OEENTRY_PACKING_ENTRY, New_Entry, Me, con, "cotton_packing_Head", "cotton_packing_Code", NewCode, "cotton_packing_Date", "(cotton_packing_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and cotton_packing_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, cotton_packing_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Cotton_Packing, New_Entry) = False Then Exit Sub

        If Pnl_Back.Enabled = False Then
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

        'Pak_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PackerName.Text)
        If Trim(cbo_PackerName.Text) = "" Then
            MessageBox.Show("Invalid Packer Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PackerName.Enabled And cbo_PackerName.Visible Then cbo_PackerName.Focus()
            Exit Sub
        End If

        CTy_idno = Common_Procedures.ConeType_NameToIdNo(con, cbo_CoNETYPE.Text)
        If cbo_CoNETYPE.Visible Then
            If Val(CTy_idno) = 0 Then
                MessageBox.Show("Invalid Cone Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_CoNETYPE.Enabled And cbo_CoNETYPE.Visible Then cbo_CoNETYPE.Focus()
                Exit Sub
            End If
        End If


        If Cbo_Count.Visible Then
            Cnt_ID = Common_Procedures.Count_NameToIdNo(con, Cbo_Count.Text)
            If Val(Cnt_ID) = 0 Then
                MessageBox.Show("Invalid COUNT", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If Cbo_Count.Enabled And Cbo_Count.Visible Then Cbo_Count.Focus()
                Exit Sub
            End If
        Else
            Grid_Cnt_ID = Common_Procedures.Count_NameToIdNo(con, dgv_PavuDetails.Rows(0).Cells(dgvCol_Details.COUNT_NAME).Value)

            If Val(Grid_Cnt_ID) = 0 Then
                MessageBox.Show("Invalid COUNT", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If Cbo_Grid_Count_Name.Enabled And Cbo_Grid_Count_Name.Visible Then Cbo_Grid_Count_Name.Focus()
                Exit Sub
            End If
            If Cnt_ID = 0 Then Cnt_ID = Val(Grid_Cnt_ID)

        End If

        If Grid_Cnt_ID = 0 Then Grid_Cnt_ID = Val(Cnt_ID)

        VTipType_ID = Common_Procedures.TipType_NameToIdno(con, Cbo_TipType.Text)

        With dgv_PavuDetails


            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(dgvCol_Details.TARE_WEIGHT).Value) > 0 Then

                    If Trim(.Rows(i).Cells(dgvCol_Details.BAG_NO).Value) = "" Then
                        MessageBox.Show("Invalid Bag No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(dgvCol_Details.BAG_NO)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If


                    'If InStr(1, Trim(UCase(Dup_SetNoBmNo)), "~" & Trim(UCase(.Rows(i).Cells(1).Value)) & "~") > 0 Then
                    '    MessageBox.Show("Duplicate BagNo ", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    '    If .Enabled And .Visible Then
                    '        .Focus()
                    '        .CurrentCell = .Rows(i).Cells(1)
                    '        .CurrentCell.Selected = True
                    '    End If
                    '    Exit Sub
                    'End If

                    Dup_SetNoBmNo = Trim(Dup_SetNoBmNo) & "~" & Trim(UCase(.Rows(i).Cells(dgvCol_Details.BAG_NO).Value)) & "~"

                End If

            Next i
        End With

        TotalPavu_Calculation()

        vTotBags = 0 : vTotGrsWgt = 0 : vTotNetWgt = 0 : vTotTareWgt = 0 : vTotcns = 0
        If dgv_PavuDetails_Total.RowCount > 0 Then
            vTotBags = Val(dgv_PavuDetails_Total.Rows(0).Cells(dgvCol_Details.BAG_NO).Value())
            vTotGrsWgt = Val(dgv_PavuDetails_Total.Rows(0).Cells(dgvCol_Details.GROSS_WEIGHT).Value())
            vTotTareWgt = Val(dgv_PavuDetails_Total.Rows(0).Cells(dgvCol_Details.TARE_WEIGHT).Value())
            vTotNetWgt = Val(dgv_PavuDetails_Total.Rows(0).Cells(dgvCol_Details.NET_WEIGHT).Value())
            vTotcns = Val(dgv_PavuDetails_Total.Rows(0).Cells(dgvCol_Details.CONES).Value())
        End If


        tr = con.BeginTransaction

        Try

        If Insert_Entry = True Or New_Entry = False Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Else

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Cotton_Packing_Head", "Cotton_Packing_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        End If

        cmd.Connection = con
        cmd.Transaction = tr

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@PakDate", dtp_Date.Value.Date)

        If New_Entry = True Then

                cmd.CommandText = "Insert into Cotton_Packing_Head(Cotton_Packing_Code, Company_IdNo, Cotton_Packing_No, for_OrderBy, Cotton_Packing_Date, Ledger_IdNo, Count_IdNo ,ConeType_IdNo,  Total_Gross_Weight, Total_Tare_Weight , Total_Net_Weight , Total_Bags , Packer, Total_NoOf_Bag, Bags_From, Bags_To,Gross_weight,Tare_weight, Net_Weight ,No_of_Cones,TipType_Idno ,Total_Cones) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ", @PakDate, " & Str(Val(Pak_ID)) & ",  " & Str(Val(Cnt_ID)) & "," & Str(Val(CTy_idno)) & ", " & Str(Val(vTotGrsWgt)) & "  ,   " & Str(Val(vTotTareWgt)) & " , " & Str(Val(vTotNetWgt)) & " ," & Str(Val(vTotBags)) & " ,'" & Trim(cbo_PackerName.Text) & "', " & Str(Val(Txt_TotalBags.Text)) & "," & Str(Val(Txt_BagsFrom.Text)) & "," & Str(Val(lbl_BagsTo.Text)) & "," & Str(Val(Txt_GrossWeight.Text)) & "," & Str(Val(Txt_TareWeight.Text)) & ", " & Str(Val(lbl_NetWeight.Text)) & "," & Str(Val(Txt_NoCones.Text)) & ", " & Str(Val(VTipType_ID)) & " , " & Str(Val(vTotcns)) & ")"
                cmd.ExecuteNonQuery()

        Else
                cmd.CommandText = "Update Cotton_Packing_Head set Cotton_Packing_Date = @PakDate, Ledger_IdNo = " & Str(Val(Pak_ID)) & ", Count_IdNo = " & Str(Val(Cnt_ID)) & " , ConeType_IdNo = " & Str(Val(CTy_idno)) & " ,  Total_Bags =" & Str(Val(vTotBags)) & " , Total_Gross_Weight = " & Str(Val(vTotGrsWgt)) & "  , Total_Tare_Weight = " & Str(Val(vTotTareWgt)) & "  , Total_Net_Weight = " & Str(Val(vTotNetWgt)) & " , Packer = '" & Trim(cbo_PackerName.Text) & "', Total_NoOf_Bag = " & Str(Val(Txt_TotalBags.Text)) & " , Bags_From = " & Str(Val(Txt_BagsFrom.Text)) & " , Gross_weight = " & Str(Val(Txt_GrossWeight.Text)) & " ,Tare_weight = " & Str(Val(Txt_TareWeight.Text)) & ",Net_Weight= " & Str(Val(lbl_NetWeight.Text)) & ",No_of_Cones =" & Str(Val(Txt_NoCones.Text)) & ",TipType_Idno=" & Str(Val(VTipType_ID)) & " , Total_Cones = " & Str(Val(vTotcns)) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Packing_Code = '" & Trim(NewCode) & "' "
                cmd.ExecuteNonQuery()

        End If

        cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Delete from Stock_LooseYarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Delete from Cotton_Packing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Packing_Code = '" & Trim(NewCode) & "' and  Cotton_invoice_Code = '' "
        cmd.ExecuteNonQuery()

        Partcls = Trim(cbo_PackerName.Text)
        PBlNo = Trim(lbl_RefNo.Text)
        EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)

        With dgv_PavuDetails
            Sno = 0
            For i = 0 To dgv_PavuDetails.RowCount - 1





                If Trim(dgv_PavuDetails.Rows(i).Cells(dgvCol_Details.BAG_NO).Value) <> "" Then

                    Sno = Sno + 1

                    Bag_NO1st = Trim(.Rows(0).Cells(dgvCol_Details.BAG_NO).Value)
                    Bag_Nolast = Trim(.Rows(dgv_PavuDetails.RowCount - 1).Cells(dgvCol_Details.BAG_NO).Value)

                    Bag_Code = ""
                    Bag_Code = Trim(Val(lbl_Company.Tag)) & "-" & Trim(.Rows(i).Cells(dgvCol_Details.BAG_NO).Value) & "/" & Trim(Common_Procedures.FnYearCode)

                    VconeType_ID = Common_Procedures.ConeType_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.CONE_TYPE).Value, tr)
                    Grid_Cnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(dgvCol_Details.COUNT_NAME).Value, tr)


                    If VconeType_ID = 0 Then VconeType_ID = Val(CTy_idno)

                    If Grid_Cnt_ID = 0 Then Grid_Cnt_ID = Val(Cnt_ID)

                    Nr = 0
                    cmd.CommandText = "update Cotton_Packing_Details set Cotton_Packing_Date = @PakDate, Sl_No = " & Str(Val(Sno)) & ", Ledger_IdNo =" & Str(Val(Pak_ID)) & " , Count_IdNo =" & Str(Val(Grid_Cnt_ID)) & "  ,  ConeType_IdNo   =" & Str(Val(VconeType_ID)) & "     , Bag_No = '" & Trim(Val(.Rows(i).Cells(dgvCol_Details.BAG_NO).Value)) & "',  Gross_Weight = " & Val(.Rows(i).Cells(dgvCol_Details.GROSS_WEIGHT).Value) & ",Tare_Weight = " & Val(.Rows(i).Cells(dgvCol_Details.TARE_WEIGHT).Value) & ", Net_Weight = " & Val(.Rows(i).Cells(dgvCol_Details.NET_WEIGHT).Value) & ", NoofCones= " & Val(.Rows(i).Cells(dgvCol_Details.CONES).Value) & "  where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Packing_Code =  '" & Trim(NewCode) & "' and Bag_Code = '" & Trim(Bag_Code) & "' "
                    Nr = cmd.ExecuteNonQuery()

                    If Nr = 0 Then

                        cmd.CommandText = "Insert into Cotton_Packing_Details( Cotton_Packing_Code    ,          Company_IdNo            ,               Cotton_Packing_No      ,   for_OrderBy                                                           ,  Cotton_Packing_Date      ,                 Sl_No ,         Ledger_IdNo      ,       Count_IdNo         ,              ConeType_IdNo     ,       Bag_No                             ,          Bag_Code        ,  Gross_Weight                              ,  Tare_Weight                               , Net_Weight                                , StockAt_IdNo, NoofCones   ) " &
                                                                " Values ('" & Trim(NewCode) & "'         , " & Str(Val(lbl_Company.Tag)) & ",        '" & Trim(lbl_RefNo.Text) & "',  " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",      @PakDate             , " & Str(Val(Sno)) & " , " & Str(Val(Pak_ID)) & " ,  " & Str(Val(Grid_Cnt_ID)) & "," & Str(Val(VconeType_ID)) & ",  '" & Trim(.Rows(i).Cells(dgvCol_Details.BAG_NO).Value) & "' , '" & Trim(Bag_Code) & "' , " & Str(Val(.Rows(i).Cells(dgvCol_Details.GROSS_WEIGHT).Value)) & "  ,  " & Str(Val(.Rows(i).Cells(dgvCol_Details.TARE_WEIGHT).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_Details.NET_WEIGHT).Value)) & " ,           4 ," & Val(.Rows(i).Cells(dgvCol_Details.CONES).Value) & ")"
                        cmd.ExecuteNonQuery()

                    End If

                End If

            Next

        End With

        Partcls = "Bag.Nos." & Trim(Bag_NO1st) & "-" & Trim(Bag_Nolast)
        PBlNo = Trim(lbl_RefNo.Text)
        EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)

        cmd.CommandText = "Insert into Stock_Yarn_Processing_Details (                    SoftwareType_IdNo  ,                                    Reference_Code        ,                Company_IdNo                 ,           Reference_No        ,                               For_OrderBy               ,                Reference_Date ,           Particulars         ,   Entry_ID   ,       Party_Bill_No , Sl_No     ,Count_idNo          ,     ConeType_IdNo  ,             Bags            ,                 Bag_No         ,         Weight             , StockAt_IdNo  ) " &
                                                      "   Values  (" & Str(Val(Common_Procedures.SoftwareTypes.OE_Software)) & " , '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",    @PakDate      , '" & Trim(Partcls) & "' , '" & Trim(EntID) & "' ,'" & Trim(PBlNo) & "', 1 , " & Str(Val(Cnt_ID)) & "," & Str(Val(CTy_idno)) & ", " & Str(Val(vTotBags)) & "  ,   '" & Trim(Bag_NO1st) & "-" & Trim(Bag_Nolast) & "' , " & Str(Val(vTotNetWgt)) & " , 4 )"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into Stock_LooseYarn_Processing_Details ( Reference_Code                        ,             Company_IdNo                 ,           Reference_No        ,                               For_OrderBy                         ,        Reference_Date      ,  Entry_ID          ,   Party_Bill_No   ,                Sl_No      , Count_idNo      ,        ConeType_IdNo  ,         Weight           ) " &
                                                                               "   Values  ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",    @PakDate   ,'" & Trim(EntID) & "' , '" & Trim(PBlNo) & "', 2 , " & Str(Val(Cnt_ID)) & "," & Str(Val(CTy_idno)) & ", " & Str(-1 * Val(vTotNetWgt)) & " )"
        cmd.ExecuteNonQuery()


        tr.Commit()

        Dt1.Dispose()
        Da.Dispose()

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

            If InStr(1, LCase(ex.Message), LCase("IX_Cotton_Packing_Details")) > 0 Then
                MessageBox.Show("Duplicate BagNo", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ElseIf InStr(1, LCase(ex.Message), LCase("PK_Cotton_Packing_Head")) > 0 Then
                MessageBox.Show("Duplicate BagNo", "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If



        Finally
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
        End Try


    End Sub

    Private Sub cbo_PackerName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PackerName.GotFocus
        '  Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_idno = 0)")
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cotton_Packing_Head", "Packer", "", "(Packer)")

    End Sub
    Private Sub cbo_PackerName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PackerName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PackerName, dtp_Date, Nothing, "Cotton_Packing_Head", "Packer", "", "(Packer <> '')")

        If (e.KeyValue = 40 And cbo_PackerName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Cbo_Count.Visible Then
                Cbo_Count.Focus()
            Else
                Cbo_TipType.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_PackerName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PackerName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PackerName, Nothing, "Cotton_Packing_Head", "Packer", "", "(Packer <> '')", False)

        If Asc(e.KeyChar) = 13 Then
            If Cbo_Count.Visible Then
                Cbo_Count.Focus()
            Else
                Cbo_TipType.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_PackerName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PackerName.KeyUp
        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    Dim f As New Ledger_Creation

        '    Common_Procedures.Master_Return.Form_Name = Me.Name
        '    Common_Procedures.Master_Return.Control_Name = cbo_PackerName.Name
        '    Common_Procedures.Master_Return.Return_Value = ""
        '    Common_Procedures.Master_Return.Master_Type = ""

        '    f.MdiParent = MDIParent1
        '    f.Show()

        'End If
    End Sub

    Private Sub cbo_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CoNETYPE.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cone_Type_head", "Cone_Type_nAME", "", "(Cone_Type_IdNo = 0)")
        'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ConeType_head", "ConeType_nAME", "", "(ConeType_IdNo = 0)")

    End Sub
    Private Sub cbo_Colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CoNETYPE.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_CoNETYPE, Cbo_Count, Nothing, "Cone_Type_head", "Cone_Type_nAME", "", "(Cone_Type_IdNo = 0)")
        'Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_CoNETYPE, Cbo_Count, Nothing, "ConeType_head", "ConeType_nAME", "", "(ConeType_IdNo = 0)")
        If (e.KeyValue = 40 And cbo_CoNETYPE.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If dgv_PavuDetails.Rows.Count > 0 Then
                Txt_TotalBags.Focus()
                'dgv_PavuDetails.Focus()
                ' dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)

            Else
                btn_save.Focus()

            End If
        End If
    End Sub

    Private Sub cbo_Colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CoNETYPE.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_CoNETYPE, Nothing, "Cone_Type_head", "Cone_Type_nAME", "", "(Cone_Type_IdNo = 0)")
        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_CoNETYPE, Nothing, "ConeType_head", "ConeType_nAME", "", "(ConeType_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            'If dgv_PavuDetails.Rows.Count > 0 Then
            '    Txt_TotalBags.Focus()
            '    'dgv_PavuDetails.Focus()
            '    ' dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)

            'Else
            '    btn_save.Focus()

            'End If
            Txt_TotalBags.Focus()
        End If
    End Sub

    Private Sub cbo_Colour_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CoNETYPE.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New ConeType_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_CoNETYPE.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Count_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Count.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "count_Head", "count_Name", "", "(count_IdNo = 0)")

    End Sub
    Private Sub cbo_Count_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Count.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Count, cbo_PackerName, Nothing, "count_Head", "count_Name", "", "(count_IdNo = 0)")
        If (e.KeyValue = 40 And Cbo_Count.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If Cbo_TipType.Visible = True Then
                Cbo_TipType.Focus()
            Else
                cbo_CoNETYPE.Focus()
            End If

        End If
    End Sub


    Private Sub cbo_Count_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_Count.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Count, Nothing, "count_Head", "count_Name", "", "(count_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            If Cbo_TipType.Visible = True Then
                Cbo_TipType.Focus()
            Else
                cbo_CoNETYPE.Focus()
            End If
        End If


    End Sub

    Private Sub cbo_Count_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Count.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_Count.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

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
        Dim Pak_IdNo As Integer, Col_IdNo As Integer, Cnt_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Col_IdNo = 0
            Cnt_IdNo = 0
            Pak_IdNo = 0
            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Cotton_Packing_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Cotton_Packing_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Cotton_Packing_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Pak_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If
            If Trim(cbo_Filter_CountName.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_CountName.Text)
            End If
            If Trim(cbo_Filter_Colour.Text) <> "" Then
                Col_IdNo = Common_Procedures.ConeType_NameToIdNo(con, cbo_Filter_Colour.Text)
            End If

            If Val(Pak_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(Pak_IdNo)) & ")"
            End If
            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.count_IdNo = " & Str(Val(Cnt_IdNo)) & " )"

            End If

            If Val(Col_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.ConeType_IdNo = " & Str(Val(Col_IdNo)) & ")"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name ,c.* ,d.* from Cotton_Packing_Head a inner join Ledger_head b on a.Ledger_idno = b.Ledger_idno LEFT OUTER join Count_head c on a.Count_idno = c.Count_idno LEFT OUTER JOIN ConeType_head d on a.ConeType_IdNo = d.ConeType_IdNo where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Cotton_Packing_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Cotton_Packing_Date, a.for_orderby, a.Cotton_Packing_No", con)
            'da = New SqlClient.SqlDataAdapter("select a.*, e.Ledger_Name from Weaver_Yarn_Delivery_Head a left outer join Weaver_Yarn_Delivery_Details b on a.Weaver_Yarn_Delivery_Code = b.Weaver_Yarn_Delivery_Code left outer join Ledger_head e on a.Ledger_idno = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Weaver_Yarn_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Weaver_Yarn_Delivery_Date, a.for_orderby, a.Weaver_Yarn_Delivery_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()


                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Cotton_Packing_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Cotton_Packing_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Count_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("ConeType_nAME").ToString

                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Total_Net_Weight").ToString), "########0.000")

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

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, cbo_Filter_CountName, cbo_Filter_Colour, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_Colour, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "count_Head", "count_Name", "", "(count_IdNo = 0)")

    End Sub


    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_CountName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_CountName, dtp_Filter_ToDate, cbo_Filter_PartyName, "count_Head", "count_Name", "", "(count_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_CountName, cbo_Filter_PartyName, "count_Head", "count_Name", "", "(count_IdNo = 0)")

    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(0).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno)
            Pnl_Back.Enabled = True
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
    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub
    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.OEENTRY_PACKING_ENTRY, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Cotton_Packing_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Packing_Code = '" & Trim(NewCode) & "'", con)
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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* , c.*,d.* from Cotton_Packing_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Count_Head c ON a.Count_IdNo = c.Count_Idno LEFT OUTER JOIN ConeType_Head d ON a.ConeType_IdNo = d.ConeType_Idno  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_Packing_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.* from Cotton_Packing_Details a   where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_Packing_Code = '" & Trim(NewCode) & "'", con)
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


        If Trim(Common_Procedures.settings.CustomerCode) = "1155" Then

            Printing_Format2_1155(e)

        Else
            Printing_Format1(e)

        End If

    End Sub
    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim LnAr(15) As Single, ClArr(15) As Single
        'Dim ItmNm1 As String, ItmNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 40
            .Right = 45
            .Top = 45
            .Bottom = 45
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

        NoofItems_PerPage = 7 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(60) : ClArr(2) = 230 : ClArr(3) = 140 : ClArr(4) = 140
        ClArr(5) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4))

        TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10


                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1



                        CurY = CurY + TxtHgt

                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Bag_No").ToString), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Gross_Weight").ToString), "#######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Tare_Weight").ToString), "#######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Net_Weight").ToString), "#######0.00"), PageWidth - 10, CurY, 1, 0, pFont)


                        NoofDets = NoofDets + 1

                        'If Trim(ItmNm2) <> "" Then
                        '    CurY = CurY + TxtHgt - 5
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        '    NoofDets = NoofDets + 1
                        'End If




                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)





            End If

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

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PACKING", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3)


        Try

            N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
            W1 = e.Graphics.MeasureString("SET NO            :  ", pFont).Width

            M1 = ClAr(1) + ClAr(2) + ClAr(3)

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "PACKER NAME", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Packer").ToString, LMargin + W1 + 30, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "REF.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cotton_Packing_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "COUNT NAME ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("count_name").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "CONE TYPE", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("ConeType_Name").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Cotton_Packing_Date").ToString), "dd-MM-yyyy"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)




            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BAG NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GROSS WGT", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TARE WGT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NET WGT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim Sno As Integer
        Dim TotGrsWgt As Single, TotTareWgt As Single, TotNetWgt As Single, TotBags As Single



        Sno = 0

        TotGrsWgt = 0
        TotTareWgt = 0
        TotNetWgt = 0
        TotBags = 0



        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
            prn_DetIndx = prn_DetIndx + 1
        Next

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        With dgv_PavuDetails_Total

            If Val(.Rows(0).Cells(dgvCol_Details.GROSS_WEIGHT).Value) <> 0 Then

                TotGrsWgt = TotGrsWgt + Val(.Rows(0).Cells(dgvCol_Details.GROSS_WEIGHT).Value)
                TotTareWgt = TotTareWgt + Val(.Rows(0).Cells(dgvCol_Details.TARE_WEIGHT).Value)
                TotNetWgt = TotNetWgt + Val(.Rows(0).Cells(dgvCol_Details.NET_WEIGHT).Value)
            End If

        End With

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(TotGrsWgt), "############0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(TotTareWgt), "############0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(TotNetWgt), "############0.000"), PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(7) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))

        CurY = CurY + TxtHgt - 5
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10
        CurY = CurY + TxtHgt


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)



        Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub cbo_Filter_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Colour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ConeType_head", "ConeType_nAME", "", "(ConeType_IdNo = 0)")

    End Sub
    Private Sub cbo_Filter_Colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Colour.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Colour, cbo_Filter_PartyName, btn_Filter_Show, "ConeType_head", "ConeType_nAME", "", "(ConeType_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_Colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Colour.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Colour, btn_Filter_Show, "ConeType_head", "ConeType_nAME", "", "(ConeType_IdNo = 0)")
    End Sub
    Private Sub TotalPavu_Calculation()
        Dim Sno As Integer
        Dim TotGrsWgt As Single, TotTareWgt As Single, TotNetWgt As Single, TotBags As Single, totNoBags As Single, totCones As Single


        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        Sno = 0

        TotGrsWgt = 0
        TotTareWgt = 0
        TotNetWgt = 0
        TotBags = 0
        totNoBags = 0
        totCones = 0

        With dgv_PavuDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(dgvCol_Details.SL_NO).Value = Sno
                If Trim(.Rows(i).Cells(dgvCol_Details.BAG_NO).Value) <> "" Then
                    TotBags = TotBags + 1
                    TotGrsWgt = TotGrsWgt + Val(.Rows(i).Cells(dgvCol_Details.GROSS_WEIGHT).Value)
                    TotTareWgt = TotTareWgt + Val(.Rows(i).Cells(dgvCol_Details.TARE_WEIGHT).Value)
                    TotNetWgt = TotNetWgt + Val(.Rows(i).Cells(dgvCol_Details.NET_WEIGHT).Value)
                    totCones = totCones + Val(.Rows(i).Cells(dgvCol_Details.CONES).Value)
                End If
            Next
        End With

        With dgv_PavuDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(dgvCol_Details.BAG_NO).Value = Val(TotBags)
            .Rows(0).Cells(dgvCol_Details.GROSS_WEIGHT).Value = Format(Val(TotGrsWgt), "########0.000")
            .Rows(0).Cells(dgvCol_Details.TARE_WEIGHT).Value = Format(Val(TotTareWgt), "########0.000")
            .Rows(0).Cells(dgvCol_Details.NET_WEIGHT).Value = Format(Val(TotNetWgt), "########0.000")
            .Rows(0).Cells(dgvCol_Details.CONES).Value = Format(Val(totCones), "########0")

        End With

    End Sub

    Private Sub dgv_PavuDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellEndEdit
        dgv_PavuDetails_CellLeave(sender, e)
    End Sub

    Private Sub dgv_PavuDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim rect As Rectangle

        With dgv_PavuDetails
            dgv_ActiveCtrl_Name = .Name

            If Val(.CurrentRow.Cells(dgvCol_Details.SL_NO).Value) = 0 Then
                .CurrentRow.Cells(dgvCol_Details.SL_NO).Value = .CurrentRow.Index + 1
            End If


            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then  'krg

                If e.ColumnIndex = dgvCol_Details.CONE_TYPE Then


                    If .CurrentCell.RowIndex > 0 And .CurrentRow.Cells(dgvCol_Details.CONE_TYPE).Value = "" Then
                        .CurrentRow.Cells(dgvCol_Details.CONE_TYPE).Value = Trim(.Rows(e.RowIndex - 1).Cells(dgvCol_Details.CONE_TYPE).Value)
                    End If

                    If Cbo_Grid_ConeType.Visible = False Or Val(Cbo_Grid_ConeType.Tag) <> e.RowIndex Then

                        Cbo_Grid_ConeType.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Cone_Type_Name from Cone_Type_Head order by Cone_Type_Name", con)
                        Dt1 = New DataTable
                        Da.Fill(Dt1)
                        Cbo_Grid_ConeType.DataSource = Dt1
                        Cbo_Grid_ConeType.DisplayMember = "Cone_Type_Name"

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        Cbo_Grid_ConeType.Left = .Left + rect.Left
                        Cbo_Grid_ConeType.Top = .Top + rect.Top

                        Cbo_Grid_ConeType.Width = rect.Width
                        Cbo_Grid_ConeType.Height = rect.Height
                        Cbo_Grid_ConeType.Text = .CurrentCell.Value

                        Cbo_Grid_ConeType.Tag = Val(e.RowIndex)
                        Cbo_Grid_ConeType.Visible = True

                        Cbo_Grid_ConeType.BringToFront()
                        Cbo_Grid_ConeType.Focus()

                    End If

                Else
                    Cbo_Grid_ConeType.Visible = False

                End If


                If e.ColumnIndex = dgvCol_Details.COUNT_NAME Then

                    If .CurrentCell.RowIndex > 0 And .CurrentRow.Cells(dgvCol_Details.COUNT_NAME).Value = "" Then
                        .CurrentRow.Cells(dgvCol_Details.COUNT_NAME).Value = Trim(.Rows(e.RowIndex - 1).Cells(dgvCol_Details.COUNT_NAME).Value)
                    End If


                    If Cbo_Grid_Count_Name.Visible = False Or Val(Cbo_Grid_Count_Name.Tag) <> e.RowIndex Then

                        Cbo_Grid_Count_Name.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
                        Dt1 = New DataTable
                        Da.Fill(Dt1)
                        Cbo_Grid_Count_Name.DataSource = Dt1
                        Cbo_Grid_Count_Name.DisplayMember = "Cone_Type_Name"

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        Cbo_Grid_Count_Name.Left = .Left + rect.Left
                        Cbo_Grid_Count_Name.Top = .Top + rect.Top

                        Cbo_Grid_Count_Name.Width = rect.Width
                        Cbo_Grid_Count_Name.Height = rect.Height
                        Cbo_Grid_Count_Name.Text = .CurrentCell.Value

                        Cbo_Grid_Count_Name.Tag = Val(e.RowIndex)
                        Cbo_Grid_Count_Name.Visible = True

                        Cbo_Grid_Count_Name.BringToFront()
                        Cbo_Grid_Count_Name.Focus()

                    End If

                Else
                    Cbo_Grid_Count_Name.Visible = False

                End If


            End If

            'If e.RowIndex > 0 And e.ColumnIndex = 1 Then
            '    If Val(.CurrentRow.Cells(1).Value) = 0 And e.RowIndex = .RowCount - 1 Then
            '        .CurrentRow.Cells(1).Value = Val(.Rows(e.RowIndex - 1).Cells(1).Value) + 1
            '        .CurrentRow.Cells(2).Value = .Rows(e.RowIndex - 1).Cells(2).Value
            '        .CurrentRow.Cells(3).Value = .Rows(e.RowIndex - 1).Cells(3).Value
            '        .CurrentRow.Cells(4).Value = .Rows(e.RowIndex - 1).Cells(4).Value
            '        '.Rows.Add()
            '    End If
            '    If e.ColumnIndex = 1 And e.RowIndex = .RowCount - 1 And Val(.CurrentRow.Cells(2).Value) = 0 And Val(.CurrentRow.Cells(3).Value) = 0 And Val(.CurrentRow.Cells(4).Value) = 0 Then
            '        .CurrentRow.Cells(1).Value = Val(.Rows(e.RowIndex - 1).Cells(1).Value) + 1
            '        .CurrentRow.Cells(2).Value = .Rows(e.RowIndex - 1).Cells(2).Value
            '        .CurrentRow.Cells(3).Value = .Rows(e.RowIndex - 1).Cells(3).Value
            '        .CurrentRow.Cells(4).Value = .Rows(e.RowIndex - 1).Cells(4).Value
            '        '.Rows.Add()
            '    End If
            'End If

        End With
    End Sub

    Private Sub dgv_PavuDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellLeave
        With dgv_PavuDetails
            If .CurrentCell.ColumnIndex = dgvCol_Details.TARE_WEIGHT Or .CurrentCell.ColumnIndex = dgvCol_Details.GROSS_WEIGHT Or .CurrentCell.ColumnIndex = dgvCol_Details.NET_WEIGHT Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_PavuDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellValueChanged
        Try

            If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

            If IsNothing(dgv_PavuDetails.CurrentCell) Then Exit Sub

            With dgv_PavuDetails

                If .Visible Then

                    If (.CurrentCell.ColumnIndex = dgvCol_Details.GROSS_WEIGHT Or .CurrentCell.ColumnIndex = dgvCol_Details.TARE_WEIGHT Or .CurrentCell.ColumnIndex = dgvCol_Details.NET_WEIGHT) Then

                        'If .CurrentRow.Index = .Rows.Count - 1 Then
                        '    .Rows.Add()
                        'End If
                        If Val(.CurrentCell.ColumnIndex) = dgvCol_Details.GROSS_WEIGHT Or Val(.CurrentCell.ColumnIndex) = dgvCol_Details.TARE_WEIGHT Then
                            .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.NET_WEIGHT).Value = Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.GROSS_WEIGHT).Value) - Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.TARE_WEIGHT).Value)
                        End If
                        If (.CurrentCell.ColumnIndex = dgvCol_Details.BAG_NO Or .CurrentCell.ColumnIndex = dgvCol_Details.GROSS_WEIGHT Or .CurrentCell.ColumnIndex = dgvCol_Details.TARE_WEIGHT Or .CurrentCell.ColumnIndex = dgvCol_Details.NET_WEIGHT) Then
                            TotalPavu_Calculation()
                        End If

                        'If (.CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3) And Val(.CurrentCell.Value) <> 0 Then
                        '    If .CurrentRow.Index = .Rows.Count - 1 Then
                        '        .Rows.Add()
                        '    End If
                        'End If

                    End If

                End If

            End With

        Catch ex As Exception
            '----

        End Try

    End Sub

    Private Sub dgv_PavuDetails_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles dgv_PavuDetails.DragOver

    End Sub


    Private Sub dgv_PavuDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_PavuDetails.EditingControlShowing
        dgtxt_PavuDetails = CType(dgv_PavuDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_PavuDetails_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_PavuDetails.GotFocus
        '--
    End Sub

    Private Sub dgv_PavuDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_PavuDetails.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_PavuDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_PavuDetails.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_PavuDetails

                If Trim(.Rows(.CurrentRow.Index).Cells(dgvCol_Details.COTTON_INVOICE_CODE).Value) = "" Then

                    n = .CurrentRow.Index

                    If .Rows.Count = 1 Then
                        For i = 0 To .Columns.Count - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else

                        .Rows.RemoveAt(n)

                    End If

                    TotalPavu_Calculation()

                Else
                    MessageBox.Show("Already Bag delivered", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub

                End If

            End With

        End If

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_PavuDetails.LostFocus
        On Error Resume Next
        If IsNothing(dgv_PavuDetails.CurrentCell) Then Exit Sub
        dgv_PavuDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_PavuDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_PavuDetails.RowsAdded
        Dim n As Integer
        With dgv_PavuDetails
            n = .RowCount
            .Rows(n - 1).Cells(dgvCol_Details.SL_NO).Value = Val(n)
        End With
    End Sub
    Private Sub dgtxt_PavuDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_PavuDetails.Enter
        dgv_ActiveCtrl_Name = dgv_PavuDetails.Name
        dgv_PavuDetails.EditingControl.BackColor = Color.Lime
        dgv_PavuDetails.EditingControl.ForeColor = Color.Blue
        dgv_PavuDetails.SelectAll()
    End Sub

    Private Sub dgtxt_PavuDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_PavuDetails.KeyDown
        If Trim(dgv_PavuDetails.Rows(dgv_PavuDetails.CurrentCell.RowIndex).Cells(dgvCol_Details.COTTON_INVOICE_CODE).Value) <> "" Then
            e.SuppressKeyPress = True
            e.Handled = True
        End If
    End Sub

    Private Sub dgtxt_PavuDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_PavuDetails.KeyPress
        'If (dgv_PavuDetails.Rows(dgv_PavuDetails.CurrentCell.RowIndex).Cells(5).Value) <> "" Then
        '    e.Handled = True
        'Else
        If dgv_PavuDetails.CurrentCell.ColumnIndex = dgvCol_Details.GROSS_WEIGHT Or dgv_PavuDetails.CurrentCell.ColumnIndex = dgvCol_Details.TARE_WEIGHT Then
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True

        End If
        ' End If
    End Sub

    Private Sub dgtxt_PavuDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_PavuDetails.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_PavuDetails_KeyUp(sender, e)
        End If
    End Sub

    Private Sub dgtxt_PavuDetails_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_PavuDetails.TextChanged
        Try
            With dgv_PavuDetails

                If .Visible Then
                    If .Rows.Count > 0 Then

                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_PavuDetails.Text)

                    End If
                End If
            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Txt_TareWeight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Txt_TareWeight.KeyDown
        'vcbo_KeyDwnVal = e.KeyValue
        ''Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Txt_TareWeight, Cbo_Count, Nothing, "ConeType_head", "ConeType_nAME", "", "(ConeType_IdNo = 0)")
        If e.KeyValue = 38 Then
            e.Handled = True
            e.SuppressKeyPress = True
            SendKeys.Send("+{TAB}")
        End If


        If e.KeyValue = 40 Then
            e.Handled = True
            e.SuppressKeyPress = True
            If dgv_PavuDetails.Rows.Count <= 0 Then
                Btn_Add_Click(sender, e)
            ElseIf dgv_PavuDetails.Rows.Count > 0 Then
                dgv_PavuDetails.Focus()
                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(dgvCol_Details.BAG_NO)
                'dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(dgvCol_Details.GROSS_WEIGHT)
            Else
                btn_save.Focus()
            End If

            'If dgv_PavuDetails.Rows.Count > 0 Then
            '    Btn_Add_Click(sender, e)
            '    'dgv_PavuDetails.Focus()
            '    'dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)

            'Else
            '    btn_save.Focus()

            'End If
        End If

        'If e.KeyValue = 38 Then
        '    Txt_GrossWeight.Focus()
        'End If
    End Sub


    Private Sub Txt_TareWeight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Txt_TareWeight.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If dgv_PavuDetails.Rows.Count <= 0 Then
                Btn_Add_Click(sender, e)
            ElseIf dgv_PavuDetails.Rows.Count > 0 Then
                dgv_PavuDetails.Focus()
                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(dgvCol_Details.BAG_NO)
                'dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(dgvCol_Details.GROSS_WEIGHT)
            Else
                btn_save.Focus()
            End If
        End If
    End Sub

    Private Sub Txt_TotalBags_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Txt_TotalBags.TextChanged
        If Val(Txt_TotalBags.Text) <> 0 Then
            lbl_BagsTo.Text = Val(Txt_BagsFrom.Text) + Val(Txt_TotalBags.Text) - 1
        Else
            lbl_BagsTo.Text = ""
        End If

    End Sub


    Private Sub Btn_Add_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_Add.Click
        Dim n As Integer = 0
        'Dim MtchSTS As Boolean = False

        If Trim(Txt_TotalBags.Text) = "" Then
            MessageBox.Show("Invalid Total Bags", "DOES NOT ADD...", MessageBoxButtons.OKCancel)
            If Txt_TotalBags.Enabled Then Txt_TotalBags.Focus()
            Exit Sub
        End If

        If Val(Txt_GrossWeight.Text) = 0 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" Then  ' krg 
                MessageBox.Show("Invalid GrossWeight", "DOES NOT ADD...", MessageBoxButtons.OKCancel)
                If Txt_GrossWeight.Enabled Then Txt_GrossWeight.Focus()
                Exit Sub
            End If
        End If


        'MtchSTS = False

        With dgv_PavuDetails

            For i = dgv_PavuDetails.Rows.Count + 1 To Val(Txt_TotalBags.Text)
                .Rows.Add(i)
            Next

            For i = 0 To dgv_PavuDetails.Rows.Count - 1

                .Rows(i).Cells(dgvCol_Details.BAG_NO).Value = (Val(Txt_BagsFrom.Text) + i).ToString
                .Rows(i).Cells(dgvCol_Details.GROSS_WEIGHT).Value = Val(Txt_GrossWeight.Text)
                .Rows(i).Cells(dgvCol_Details.TARE_WEIGHT).Value = Val(Txt_TareWeight.Text)
                .Rows(i).Cells(dgvCol_Details.NET_WEIGHT).Value = Val(lbl_NetWeight.Text)

                .Rows(i).Cells(dgvCol_Details.CONES).Value = Val(Txt_NoCones.Text)

            Next


        End With


        TotalPavu_Calculation()

        Grid_Cell_DeSelect()

        If Txt_TotalBags.Enabled And Txt_TotalBags.Visible Then Txt_TotalBags.Focus()

    End Sub
    Private Sub Txt_BagsFrom_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles Txt_BagsFrom.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub
    Private Sub Cbo_Grid_ConeType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Grid_ConeType.TextChanged
        Try
            If Cbo_Grid_ConeType.Visible Then
                With dgv_PavuDetails
                    If Val(Cbo_Grid_ConeType.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCol_Details.CONE_TYPE Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(Cbo_Grid_ConeType.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Cbo_Grid_ConeType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Grid_ConeType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cone_Type_head", "Cone_Type_nAME", "", "(Cone_Type_IdNo = 0)")
    End Sub

    Private Sub Cbo_Grid_ConeType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Grid_ConeType.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Grid_ConeType, Nothing, Nothing, "Cone_Type_head", "Cone_Type_nAME", "", "(Cone_Type_IdNo = 0)")

        With dgv_PavuDetails

            If (e.KeyValue = 38 And Cbo_Grid_ConeType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.CONES)
            End If

            If (e.KeyValue = 40 And Cbo_Grid_ConeType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.GROSS_WEIGHT)
            End If

        End With

    End Sub

    Private Sub Cbo_Grid_ConeType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_Grid_ConeType.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        'Dim Cn_bag As Integer
        'Dim Wgt_Bag As Integer
        'Dim Wgt_Cn As Integer
        'Dim mill_idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Grid_ConeType, Nothing, "Cone_Type_head", "Cone_Type_nAME", "", "(Cone_Type_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_PavuDetails

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.GROSS_WEIGHT)
            End With

        End If


    End Sub

    Private Sub Cbo_Grid_ConeType_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Grid_ConeType.KeyUp

        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New ConeType_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_Grid_ConeType.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub Txt_BagsFrom_TextChanged(sender As Object, e As EventArgs) Handles Txt_BagsFrom.TextChanged
        If Val(Txt_BagsFrom.Text) <> 0 Then
            lbl_BagsTo.Text = Val(Txt_BagsFrom.Text) + Val(Txt_TotalBags.Text) - 1
        Else
            lbl_BagsTo.Text = ""
        End If
    End Sub

    Private Sub Txt_TotalBags_KeyDown(sender As Object, e As KeyEventArgs) Handles Txt_TotalBags.KeyDown
        If e.KeyCode = 38 Then
            If Cbo_TipType.Visible = True Then
                Cbo_TipType.Focus()
            Else
                cbo_CoNETYPE.Focus()
            End If
        End If

        If e.KeyCode = 40 Then
            Txt_BagsFrom.Focus()
        End If
    End Sub


    Private Sub Cbo_TipType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_TipType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "TipType_head", "TipType_nAME", "", "(TipType_IdNo = 0)")
    End Sub

    Private Sub Cbo_TipType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_TipType.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_TipType, cbo_PackerName, Txt_TotalBags, "TipType_head", "TipType_nAME", "", "(TipType_IdNo = 0)")

    End Sub

    Private Sub Cbo_TipType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_TipType.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_TipType, Txt_TotalBags, "TipType_head", "TipType_nAME", "", "(TipType_IdNo = 0)")

    End Sub
    Private Sub Cbo_TipType_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_TipType.KeyUp

        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Tiptype_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_TipType.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub
    Private Sub Txt_GrossWeight_TextChanged(sender As Object, e As EventArgs) Handles Txt_GrossWeight.TextChanged
        Txt_TareWeight_TextChanged(sender, e)
    End Sub
    Private Sub Txt_TareWeight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Txt_TareWeight.TextChanged
        If Val(Txt_TareWeight.Text) <> 0 Then
            lbl_NetWeight.Text = Val(Txt_GrossWeight.Text) - Val(Txt_TareWeight.Text)
        Else
            lbl_NetWeight.Text = Txt_GrossWeight.Text

        End If
    End Sub
    Private Sub Cbo_Grid_Count_Name_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Grid_Count_Name.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "count_Head", "count_Name", "", "(count_IdNo = 0)")
    End Sub
    Private Sub Cbo_Grid_Count_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Grid_Count_Name.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Grid_Count_Name, Nothing, Nothing, "count_Head", "count_Name", "", "(count_IdNo = 0)")

        With dgv_PavuDetails

            If (e.KeyValue = 38 And Cbo_Grid_Count_Name.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.BAG_NO)
            End If

            If (e.KeyValue = 40 And Cbo_Grid_Count_Name.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.CONES)
            End If

        End With
    End Sub
    Private Sub Cbo_Grid_Count_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_Grid_Count_Name.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Grid_Count_Name, Nothing, "count_Head", "count_Name", "", "(count_IdNo = 0)")


        If Asc(e.KeyChar) = 13 Then

            With dgv_PavuDetails

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.CONES)
            End With

        End If

    End Sub

    Private Sub Cbo_Grid_Count_Name_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_Grid_Count_Name.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_Grid_Count_Name.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub Cbo_Grid_Count_Name_TextChanged(sender As Object, e As EventArgs) Handles Cbo_Grid_Count_Name.TextChanged
        Try
            If Cbo_Grid_Count_Name.Visible Then
                With dgv_PavuDetails
                    If Val(Cbo_Grid_Count_Name.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCol_Details.COUNT_NAME Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(Cbo_Grid_Count_Name.Text)
                    End If
                End With
            End If

        Catch ex As Exception


        End Try
    End Sub

    Private Sub Printing_Format2_1155(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim LnAr(15) As Single, ClArr(15) As Single
        'Dim ItmNm1 As String, ItmNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 40
            .Right = 45
            .Top = 45
            .Bottom = 45
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

        NoofItems_PerPage = 7 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(60) : ClArr(2) = 150 : ClArr(3) = 130 : ClArr(4) = 130 : ClArr(5) = 130
        ClArr(6) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4))

        TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format2_1155_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10


                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format2_1155_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1



                        CurY = CurY + TxtHgt

                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Count_IdNoToName(con, prn_DetDt.Rows(prn_DetIndx).Item("Count_Idno").ToString), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Bag_No").ToString), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Gross_Weight").ToString), "#######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Tare_Weight").ToString), "#######0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Net_Weight").ToString), "#######0.00"), PageWidth - 10, CurY, 1, 0, pFont)


                        NoofDets = NoofDets + 1

                        'If Trim(ItmNm2) <> "" Then
                        '    CurY = CurY + TxtHgt - 5
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        '    NoofDets = NoofDets + 1
                        'End If




                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format2_1155_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)





            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format2_1155_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
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

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PACKING", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3)


        Try

            N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
            W1 = e.Graphics.MeasureString("SET NO            :  ", pFont).Width

            M1 = ClAr(1) + ClAr(2) + ClAr(3)

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "PACKER NAME", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Packer").ToString, LMargin + W1 + 30, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "REF.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cotton_Packing_No").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "COUNT NAME ", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("count_name").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "CONE TYPE", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("ConeType_Name").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Cotton_Packing_Date").ToString), "dd-MM-yyyy"), LMargin + M1 + W1 + 25, CurY, 0, 0, pFont)




            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PALLET NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GROSS WGT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TARE WGT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NET WGT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 50, CurY, 0, ClAr(6), pFont)


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format2_1155_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim Sno As Integer
        Dim TotGrsWgt As Single, TotTareWgt As Single, TotNetWgt As Single, TotBags As Single



        Sno = 0

        TotGrsWgt = 0
        TotTareWgt = 0
        TotNetWgt = 0
        TotBags = 0



        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
            prn_DetIndx = prn_DetIndx + 1
        Next

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        With dgv_PavuDetails_Total

            If Val(.Rows(0).Cells(dgvCol_Details.GROSS_WEIGHT).Value) <> 0 Then

                TotGrsWgt = TotGrsWgt + Val(.Rows(0).Cells(dgvCol_Details.GROSS_WEIGHT).Value)
                TotTareWgt = TotTareWgt + Val(.Rows(0).Cells(dgvCol_Details.TARE_WEIGHT).Value)
                TotNetWgt = TotNetWgt + Val(.Rows(0).Cells(dgvCol_Details.NET_WEIGHT).Value)
            End If

        End With

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(TotGrsWgt), "############0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(TotTareWgt), "############0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(TotNetWgt), "############0.000"), PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(7) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))

        CurY = CurY + TxtHgt - 5
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10
        CurY = CurY + TxtHgt


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)



        Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

End Class
