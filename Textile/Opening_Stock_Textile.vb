Public Class Opening_Stock_Textile
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Pk_Condition As String = "OPENI-"
    Private OpYrCode As String = ""
    Private vcbo_KeyDwnVal As Double
    Private Prec_ActCtrl As New Control
    Private ClrSTS As Boolean = False
    Private WithEvents dgtxt_YarnDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_PavuDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_BillDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_ClothDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_BobinDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_EmptyBeamDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_JariKuriDetails As New DataGridViewTextBoxEditingControl
    Private vLed_ID_Cond As Integer = 0

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()
        Dim vCur_Yr As String = ""
        Dim vPre_Yr As String = ""
        Dim CMP_frmdate As String = ""
        Dim CMP_todate As String = ""
        Dim Year As Integer = 0
        Dim Year1 As Integer = 0
        Dim YrCode As String = ""

        ClrSTS = True

        New_Entry = False

        lbl_IdNo.Text = ""
        lbl_IdNo.ForeColor = Color.Black

        pnl_Back.Enabled = True

        lbl_IdNo.Text = ""
        cbo_Ledger.Text = ""
        cbo_Ledger.Tag = ""
        cbo_Ledger.Enabled = False

        txt_OpAmount.Text = "0.00"
        cbo_CrDrType.Text = "Cr"

        txt_EmptyBeam.Text = ""
        txt_EmptyBags.Text = ""
        txt_EmptyCones.Text = ""
        txt_EmptyBobin.Text = ""
        txt_EmptyBobinParty.Text = ""
        txt_EmptyJumbo.Text = ""

        cbo_Grid_Yarn_weaving_job_no.Text = ""
        cbo_Grid_Yarn_Sizing_JobCardNo.Text = ""
        cbo_Grid_Yarn_LotNo.Text = ""

        cbo_Grid_Pavu_weaving_job_no.Text = ""
        cbo_grid_Pavu_Sizing_JobCardNo.Text = ""


        cbo_BobinGrid_EndsCount.Visible = False
        cbo_BobinGrid_Colour.Visible = False
        cbo_BobinGrid_PartName.Visible = False
        cbo_Grid_JariCount.Visible = False
        cbo_Grid_JariColour.Visible = False
        cbo_Grid_CountName.Visible = False
        cbo_Grid_MillName.Visible = False
        cbo_Grid_YarnType.Visible = False
        cbo_PavuGrid_EndsCount.Visible = False
        cbo_BillGrid_AgentName.Visible = False
        cbo_BillGrid_CrDr.Visible = False
        cbo_Grid_BobinMillName.Visible = False

        cbo_Grid_CountName.Text = ""
        cbo_Grid_MillName.Text = ""
        cbo_Grid_YarnType.Text = ""
        cbo_PavuGrid_EndsCount.Text = ""
        cbo_BillGrid_AgentName.Text = ""
        cbo_BillGrid_CrDr.Text = ""
        cbo_Grid_JariColour.Text = ""
        cbo_Grid_JariCount.Text = ""
        cbo_BobinGrid_Colour.Text = ""
        cbo_BobinGrid_EndsCount.Text = ""
        cbo_BobinGrid_PartName.Text = ""
        cbo_Grid_BobinMillName.Text = ""


        lbl_YrCode1.Text = ""
        txt_Sales_Value_Yr1.Text = ""
        txt_Purchase_Value_Yr1.Text = ""

        lbl_YrCode2.Text = ""
        txt_Sales_Value_Yr2.Text = ""
        txt_Purchase_Value_Yr2.Text = ""


        CMP_frmdate = Common_Procedures.Company_FromDate
        CMP_todate = Common_Procedures.Company_ToDate

        vCur_Yr = Val(Microsoft.VisualBasic.Right(CMP_frmdate, 4))
        vPre_Yr = Val(Microsoft.VisualBasic.Right(CMP_todate, 4))

        lbl_YrCode1.Text = OpYrCode  '---vCur_Yr & " - " & vPre_Yr

        YrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
        lbl_YrCode2.Text = Trim(Microsoft.VisualBasic.Right(YrCode, 2)) & "-" & Trim(Mid(Val(YrCode) + 1, 3, 2))

        'Year = Convert.ToInt32(Now.ToString("yyyy"))
        'Year1 = Year - 1
        'lbl_YrCode2.Text = Year1 & " - " & Year

        dgv_YarnDetails.Rows.Clear()
        dgv_PavuDetails.Rows.Clear()
        dgv_BillDetails.Rows.Clear()
        dgv_ClothDetails.Rows.Clear()
        dgv_EmptyBeamDetails.Rows.Clear()
        dgv_BobinDetails.Rows.Clear()
        dgv_Jari_KuriDetails.Rows.Clear()

        dgv_YarnDetails_Total.Rows.Clear()
        dgv_YarnDetails_Total.Rows.Add()

        dgv_PavuDetails_Total.Rows.Clear()
        dgv_PavuDetails_Total.Rows.Add()

        dgv_BillDetails_Total.Rows.Clear()
        dgv_BillDetails_Total.Rows.Add()

        dgv_ClothDetails_Total.Rows.Clear()
        dgv_ClothDetails_Total.Rows.Add()

        dgv_EmptyBeamDetails.Rows.Clear()
        dgv_EmptyBeamDetails.Rows.Add()



        tab_Main.SelectTab(0)
        dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
        dgv_YarnDetails.CurrentCell.Selected = True

        cbo_Grid_CountName.Visible = False
        cbo_Grid_MillName.Visible = False
        cbo_Grid_YarnType.Visible = False
        cbo_PavuGrid_EndsCount.Visible = False
        cbo_BillGrid_AgentName.Visible = False
        cbo_BillGrid_CrDr.Visible = False
        cbo_Grid_ClothName.Visible = False
        cbo_Grid_ClothTypeName.Visible = False
        cbo_Grid_beamwidth.Visible = False
        cbo_Grid_VendorName.Visible = False
        cbo_Grid_BobinMillName.Visible = False
        cbo_BobinGrid_PartName.Visible = False

        txt_OpAmount.Enabled = True
        cbo_CrDrType.Enabled = True
        dgv_BillDetails.Enabled = False

        dgv_ClothDetails.Enabled = False

        Grid_Cell_DeSelect()

        ClrSTS = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim dgvtxtedtctrl As DataGridViewTextBoxEditingControl

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

        If Me.ActiveControl.Name <> cbo_Grid_CountName.Name Then
            cbo_Grid_CountName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_MillName.Name Then
            cbo_Grid_MillName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_YarnType.Name Then
            cbo_Grid_YarnType.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_PavuGrid_EndsCount.Name Then
            cbo_PavuGrid_EndsCount.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_BillGrid_AgentName.Name Then
            cbo_BillGrid_AgentName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_BillGrid_CrDr.Name Then
            cbo_BillGrid_CrDr.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_JariColour.Name Then
            cbo_Grid_JariColour.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_JariCount.Name Then
            cbo_Grid_JariCount.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_BobinGrid_Colour.Name Then
            cbo_BobinGrid_Colour.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_BobinGrid_EndsCount.Name Then
            cbo_BobinGrid_EndsCount.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_BobinMillName.Name Then
            cbo_Grid_BobinMillName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_BobinGrid_PartName.Name Then
            cbo_BobinGrid_PartName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_beamwidth.Name Then
            cbo_Grid_beamwidth.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_VendorName.Name Then
            cbo_Grid_VendorName.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_Grid_Yarn_LotNo.Name Then
            cbo_Grid_Yarn_LotNo.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Yarn_weaving_job_no.Name Then
            cbo_Grid_Yarn_weaving_job_no.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Yarn_Sizing_JobCardNo.Name Then
            cbo_Grid_Yarn_Sizing_JobCardNo.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Pavu_weaving_job_no.Name Then
            cbo_Grid_Pavu_weaving_job_no.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_grid_Pavu_Sizing_JobCardNo.Name Then
            cbo_grid_Pavu_Sizing_JobCardNo.Visible = False
        End If


        Grid_Cell_DeSelect()

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
        If e.KeyValue = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControlKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_PavuDetails.CurrentCell) Then dgv_PavuDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_YarnDetails.CurrentCell) Then dgv_YarnDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_BillDetails.CurrentCell) Then dgv_BillDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_ClothDetails.CurrentCell) Then dgv_ClothDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_EmptyBeamDetails.CurrentCell) Then dgv_EmptyBeamDetails.CurrentCell.Selected = False
    End Sub

    Private Sub move_record(ByVal idno As Integer)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim da3 As New SqlClient.SqlDataAdapter

        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable

        Dim Sno As Integer, n As Integer
        Dim NewCode As String
        Dim BilType As String
        Dim LedType As String
        Dim Sign As Integer = 0

        If Val(idno) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Val(idno)) & "/" & Trim(OpYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.Ledger_IdNo, a.Ledger_Name from Ledger_Head a Where a.Ledger_IdNo = " & Str(Val(idno)) & "", con)
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then
                lbl_IdNo.Text = dt1.Rows(0).Item("Ledger_IdNo").ToString

                cbo_Ledger.Text = dt1.Rows(0).Item("Ledger_Name").ToString
                cbo_Ledger.Tag = cbo_Ledger.Text
                cbo_Ledger.Enabled = False

                BilType = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Bill_Type", "(Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)) & ")")

                LedType = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)) & ")")

                If Trim(UCase(BilType)) = "BILL TO BILL" Then
                    txt_OpAmount.Enabled = False
                    cbo_CrDrType.Enabled = False
                    dgv_BillDetails.Enabled = True

                Else
                    txt_OpAmount.Enabled = True
                    cbo_CrDrType.Enabled = True
                    dgv_BillDetails.Enabled = False

                End If

                If Trim(UCase(LedType)) = "GODOWN" Or Trim(UCase(LedType)) = "JOBWORKER" Then
                    dgv_ClothDetails.Enabled = True

                Else
                    dgv_ClothDetails.Enabled = False

                End If

                da2 = New SqlClient.SqlDataAdapter("Select sum(Empty_Beam*(CASE WHEN ReceivedFrom_Idno = " & Str(Val(idno)) & " THEN -1 else 1 END)) as Op_Beam from Stock_Empty_BeamBagCone_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and (DeliveryTo_Idno = " & Str(Val(idno)) & " or ReceivedFrom_Idno = " & Str(Val(idno)) & ") and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Empty_Beam <> 0", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                If dt2.Rows.Count > 0 Then
                    If IsDBNull(dt2.Rows(0).Item("Op_Beam").ToString) = False Then
                        If Val(dt2.Rows(0).Item("Op_Beam").ToString) <> 0 Then
                            txt_EmptyBeam.Text = Val(dt2.Rows(0).Item("Op_Beam").ToString)
                        End If
                    End If
                End If
                dt2.Clear()

                If Trim(UCase(LedType)) = "JOBWORKER" Then
                    da2 = New SqlClient.SqlDataAdapter("Select sum(Empty_Bags*(CASE WHEN DeliveryTo_Idno = " & Str(Val(idno)) & " THEN -1 else 1 END)) as Op_Bags from Stock_Empty_BeamBagCone_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and (DeliveryTo_Idno = " & Str(Val(idno)) & " or ReceivedFrom_Idno = " & Str(Val(idno)) & ") and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'  and Empty_Bags <> 0", con)
                Else
                    da2 = New SqlClient.SqlDataAdapter("Select sum(Empty_Bags*(CASE WHEN ReceivedFrom_Idno = " & Str(Val(idno)) & " THEN -1 else 1 END)) as Op_Bags from Stock_Empty_BeamBagCone_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and (DeliveryTo_Idno = " & Str(Val(idno)) & " or ReceivedFrom_Idno = " & Str(Val(idno)) & ") and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'  and Empty_Bags <> 0", con)
                End If
                dt2 = New DataTable
                da2.Fill(dt2)

                If dt2.Rows.Count > 0 Then
                    If IsDBNull(dt2.Rows(0).Item("Op_Bags").ToString) = False Then
                        If Val(dt2.Rows(0).Item("Op_Bags").ToString) <> 0 Then
                            txt_EmptyBags.Text = Val(dt2.Rows(0).Item("Op_Bags").ToString)
                        End If
                    End If
                End If
                dt2.Clear()

                If Trim(UCase(LedType)) = "JOBWORKER" Then
                    da2 = New SqlClient.SqlDataAdapter("Select sum(Empty_Cones*(CASE WHEN DeliveryTo_Idno = " & Str(Val(idno)) & " THEN -1 else 1 END) ) as Op_Cones from Stock_Empty_BeamBagCone_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and (DeliveryTo_Idno = " & Str(Val(idno)) & " or ReceivedFrom_Idno = " & Str(Val(idno)) & ") and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Empty_Cones <> 0", con)
                Else
                    da2 = New SqlClient.SqlDataAdapter("Select sum(Empty_Cones*(CASE WHEN ReceivedFrom_Idno = " & Str(Val(idno)) & " THEN -1 else 1 END) ) as Op_Cones from Stock_Empty_BeamBagCone_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and (DeliveryTo_Idno = " & Str(Val(idno)) & " or ReceivedFrom_Idno = " & Str(Val(idno)) & ") and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Empty_Cones <> 0", con)
                End If
                dt2 = New DataTable
                da2.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    If IsDBNull(dt2.Rows(0).Item("Op_Cones").ToString) = False Then
                        If Val(dt2.Rows(0).Item("Op_Cones").ToString) <> 0 Then
                            txt_EmptyCones.Text = Val(dt2.Rows(0).Item("Op_Cones").ToString)
                        End If
                    End If
                End If
                dt2.Clear()

                da2 = New SqlClient.SqlDataAdapter("Select sum(Empty_Bobin*(CASE WHEN ReceivedFrom_Idno = " & Str(Val(idno)) & " THEN -1 else 1 END) ) as Op_Bobin from Stock_Empty_BeamBagCone_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and (DeliveryTo_Idno = " & Str(Val(idno)) & " or ReceivedFrom_Idno = " & Str(Val(idno)) & ") and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Empty_Bobin <> 0", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                If dt2.Rows.Count > 0 Then
                    If IsDBNull(dt2.Rows(0).Item("Op_Bobin").ToString) = False Then
                        If Val(dt2.Rows(0).Item("Op_Bobin").ToString) <> 0 Then
                            txt_EmptyBobin.Text = Val(dt2.Rows(0).Item("Op_Bobin").ToString)
                        End If
                    End If
                End If
                dt2.Clear()

                da2 = New SqlClient.SqlDataAdapter("Select sum(EmptyBobin_Party*(CASE WHEN DeliveryTo_Idno = " & Str(Val(idno)) & " THEN -1 else 1 END) ) as Op_BobinParty from Stock_Empty_BeamBagCone_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and (DeliveryTo_Idno = " & Str(Val(idno)) & " or ReceivedFrom_Idno = " & Str(Val(idno)) & ") and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and EmptyBobin_Party <> 0", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                If dt2.Rows.Count > 0 Then
                    If IsDBNull(dt2.Rows(0).Item("Op_BobinParty").ToString) = False Then
                        If Val(dt2.Rows(0).Item("Op_BobinParty").ToString) <> 0 Then
                            txt_EmptyBobinParty.Text = Val(dt2.Rows(0).Item("Op_BobinParty").ToString)
                        End If
                    End If
                End If
                dt2.Clear()


                da2 = New SqlClient.SqlDataAdapter("Select sum(Empty_Jumbo*(CASE WHEN ReceivedFrom_Idno = " & Str(Val(idno)) & " THEN -1 else 1 END) ) as Op_Jumbo from Stock_Empty_BeamBagCone_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and (DeliveryTo_Idno = " & Str(Val(idno)) & " or ReceivedFrom_Idno = " & Str(Val(idno)) & ") and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Empty_Jumbo <> 0", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                If dt2.Rows.Count > 0 Then
                    If IsDBNull(dt2.Rows(0).Item("Op_Jumbo").ToString) = False Then
                        If Val(dt2.Rows(0).Item("Op_Jumbo").ToString) <> 0 Then
                            txt_EmptyJumbo.Text = Val(dt2.Rows(0).Item("Op_Jumbo").ToString)
                        End If
                    End If
                End If
                dt2.Clear()

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name, c.Mill_Name from Stock_Yarn_Processing_Details a INNER JOIN Count_Head b on a.Count_IdNo = b.Count_IdNo INNER JOIN Mill_Head c on a.Mill_IdNo = c.Mill_IdNo where a.sl_No < 5000 and company_idno = " & Str(Val(lbl_Company.Tag)) & " and (DeliveryTo_Idno = " & Str(Val(idno)) & " or ReceivedFrom_Idno = " & Str(Val(idno)) & ") and a.Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_YarnDetails.Rows.Clear()
                Sno = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_YarnDetails.Rows.Add()

                        Sno = Sno + 1
                        dgv_YarnDetails.Rows(n).Cells(0).Value = Val(Sno)
                        dgv_YarnDetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Count_Name").ToString
                        dgv_YarnDetails.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Yarn_Type").ToString
                        dgv_YarnDetails.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Mill_Name").ToString

                        Sign = 1
                        If Trim(UCase(LedType)) = "JOBWORKER" Then
                            If Val(idno) = Val(dt2.Rows(i).Item("DeliveryTo_Idno").ToString) Then
                                Sign = -1
                            Else
                                Sign = 1
                            End If

                        Else
                            If Val(idno) = Val(dt2.Rows(i).Item("ReceivedFrom_Idno").ToString) Then
                                Sign = -1
                            Else
                                Sign = 1
                            End If

                        End If


                        dgv_YarnDetails.Rows(n).Cells(4).Value = Sign * Val(dt2.Rows(i).Item("Bags").ToString)
                        If Val(dgv_YarnDetails.Rows(n).Cells(4).Value) = 0 Then
                            dgv_YarnDetails.Rows(n).Cells(4).Value = ""
                        End If
                        dgv_YarnDetails.Rows(n).Cells(5).Value = Sign * Val(dt2.Rows(i).Item("Cones").ToString)
                        If Val(dgv_YarnDetails.Rows(n).Cells(5).Value) = 0 Then
                            dgv_YarnDetails.Rows(n).Cells(5).Value = ""
                        End If

                        dgv_YarnDetails.Rows(n).Cells(6).Value = Format(Sign * Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")

                        dgv_YarnDetails.Rows(n).Cells(7).Value = dt2.Rows(i).Item("set_no").ToString

                        dgv_YarnDetails.Rows(n).Cells(8).Value = dt2.Rows(0).Item("LotCode_ForSelection").ToString
                        dgv_YarnDetails.Rows(n).Cells(9).Value = dt2.Rows(0).Item("Weaving_JobCode_forSelection").ToString
                        dgv_YarnDetails.Rows(n).Cells(10).Value = dt2.Rows(0).Item("Sizing_JobCode_forSelection").ToString

                    Next i

                End If

                TotalYarn_Calculation()





                da2 = New SqlClient.SqlDataAdapter("select a.*, b.EndsCount_Name from Stock_SizedPavu_Processing_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo Where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Ledger_IdNo = " & Str(Val(idno)) & " and a.Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_PavuDetails.Rows.Clear()
                Sno = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_PavuDetails.Rows.Add()

                        Sno = Sno + 1
                        dgv_PavuDetails.Rows(n).Cells(0).Value = Val(Sno)
                        dgv_PavuDetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Set_No").ToString
                        dgv_PavuDetails.Rows(n).Cells(2).Value = dt2.Rows(i).Item("EndsCount_Name").ToString
                        dgv_PavuDetails.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Beam_No").ToString
                        dgv_PavuDetails.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Noof_Pcs").ToString
                        If Val(dgv_PavuDetails.Rows(n).Cells(4).Value) = 0 Then
                            dgv_PavuDetails.Rows(n).Cells(4).Value = ""
                        End If
                        If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then

                            dgv_PavuDetails.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.000")
                        Else
                            dgv_PavuDetails.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                        End If
                        dgv_PavuDetails.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Pavu_Delivery_Increment").ToString

                        If dt2.Rows(i).Item("Pavu_Delivery_Code").ToString <> "" Or dt2.Rows(i).Item("Beam_Knotting_Code").ToString <> "" Or Val(dt2.Rows(i).Item("Production_Meters").ToString) <> 0 Or Val(dt2.Rows(i).Item("Close_Status").ToString) <> 0 Then
                            dgv_PavuDetails.Rows(n).Cells(6).Value = "1"
                        Else
                            dgv_PavuDetails.Rows(n).Cells(6).Value = ""
                        End If

                        dgv_PavuDetails.Rows(n).Cells(7).Value = dt2.Rows(0).Item("Weaving_JobCode_forSelection").ToString

                        dgv_PavuDetails.Rows(n).Cells(8).Value = dt2.Rows(0).Item("Sizing_JobCode_forSelection").ToString

                    Next i


                Else

                    da2 = New SqlClient.SqlDataAdapter("select a.*, b.EndsCount_Name from Stock_Pavu_Processing_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo Where a.sl_no < 5000  and a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and (DeliveryTo_Idno = " & Str(Val(idno)) & " or ReceivedFrom_Idno = " & Str(Val(idno)) & ")  and a.Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Sl_No", con)
                    dt2 = New DataTable
                    da2.Fill(dt2)
                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = dgv_PavuDetails.Rows.Add()

                            Sno = Sno + 1
                            dgv_PavuDetails.Rows(n).Cells(0).Value = Val(Sno)
                            dgv_PavuDetails.Rows(n).Cells(1).Value = ""
                            dgv_PavuDetails.Rows(n).Cells(2).Value = dt2.Rows(i).Item("EndsCount_Name").ToString
                            dgv_PavuDetails.Rows(n).Cells(3).Value = ""
                            dgv_PavuDetails.Rows(n).Cells(4).Value = ""
                            dgv_PavuDetails.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                            dgv_PavuDetails.Rows(n).Cells(6).Value = ""
                            dgv_PavuDetails.Rows(n).Cells(7).Value = dt2.Rows(0).Item("Weaving_JobCode_forSelection").ToString
                            dgv_PavuDetails.Rows(n).Cells(8).Value = dt2.Rows(0).Item("Sizing_JobCode_forSelection").ToString

                        Next i

                    End If

                End If

                TotalPavu_Calculation()

                da2 = New SqlClient.SqlDataAdapter("Select a.party_bill_no, a.voucher_bill_date, b.ledger_name as agent_name, a.bill_amount, a.crdr_type, abs(a.bill_amount - a.credit_amount - a.debit_amount) as Paid_rcvd_Amount, a.Voucher_Bill_Code from voucher_bill_head a left outer join ledger_head b on a.agent_idno = b.ledger_idno where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(idno)) & " and a.entry_identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "' order by a.voucher_bill_date, a.Voucher_Bill_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_BillDetails.Rows.Clear()
                Sno = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_BillDetails.Rows.Add()

                        Sno = Sno + 1
                        dgv_BillDetails.Rows(n).Cells(0).Value = Val(Sno)
                        dgv_BillDetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("party_bill_no").ToString
                        dgv_BillDetails.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("voucher_bill_date").ToString), "dd-MM-yyyy")
                        dgv_BillDetails.Rows(n).Cells(3).Value = dt2.Rows(i).Item("agent_name").ToString
                        dgv_BillDetails.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("bill_amount").ToString), "########0.00")
                        dgv_BillDetails.Rows(n).Cells(5).Value = dt2.Rows(i).Item("crdr_type").ToString
                        dgv_BillDetails.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Paid_rcvd_Amount").ToString), "########0.00")
                        If Val(dgv_BillDetails.Rows(n).Cells(6).Value) = 0 Then
                            dgv_BillDetails.Rows(n).Cells(6).Value = ""
                        End If
                        dgv_BillDetails.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Voucher_Bill_Code").ToString

                    Next i

                End If

                dt2.Clear()

                Total_BillAmount_Calculation()

                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Cloth_Name from Stock_Cloth_Processing_Details a INNER JOIN Cloth_Head b on a.Cloth_Idno = b.Cloth_Idno Where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and (a.DeliveryTo_Idno = " & Str(Val(idno)) & " or a.ReceivedFrom_Idno = " & Str(Val(idno)) & ") and a.Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_ClothDetails

                    .Rows.Clear()
                    Sno = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            Sno = Sno + 1

                            .Rows(n).Cells(0).Value = Val(Sno)
                            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                            .Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Folding").ToString)

                            If Val(dt2.Rows(i).Item("Meters_Type1").ToString) <> 0 Then
                                .Rows(n).Cells(2).Value = Trim(UCase(Common_Procedures.ClothType.Type1))
                                .Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meters_Type1").ToString), "########0.00")

                            ElseIf Val(dt2.Rows(i).Item("Meters_Type2").ToString) <> 0 Then
                                .Rows(n).Cells(2).Value = Trim(UCase(Common_Procedures.ClothType.Type2))
                                .Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meters_Type2").ToString), "########0.00")

                            ElseIf Val(dt2.Rows(i).Item("Meters_Type3").ToString) <> 0 Then
                                .Rows(n).Cells(2).Value = Trim(UCase(Common_Procedures.ClothType.Type3))
                                .Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meters_Type3").ToString), "########0.00")

                            ElseIf Val(dt2.Rows(i).Item("Meters_Type4").ToString) <> 0 Then
                                .Rows(n).Cells(2).Value = Trim(UCase(Common_Procedures.ClothType.Type4))
                                .Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meters_Type4").ToString), "########0.00")

                            ElseIf Val(dt2.Rows(i).Item("Meters_Type5").ToString) <> 0 Then
                                .Rows(n).Cells(2).Value = Trim(UCase(Common_Procedures.ClothType.Type5))
                                .Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meters_Type5").ToString), "########0.00")

                            End If

                            .Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Pieces").ToString), "########0.00")
                            .Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")

                        Next i

                    End If

                    TotalCloth_Calculation()

                End With

                da2 = New SqlClient.SqlDataAdapter("Select a.DeliveryTo_Idno , a.ReceivedFrom_Idno , a.Empty_Beam as Op_Beam , b.Beam_Width_Name , c.Vendor_Name  from Stock_Empty_BeamBagCone_Processing_Details a  LEFT OUTER JOIN Beam_Width_Head b ON a.Beam_Width_IdNo <> 0 and a.Beam_Width_IdNo = b.Beam_Width_IdNo  LEFT OUTER JOIN Vendor_Head c ON a.Vendor_IdNo <> 0 and a.Vendor_IdNo = c.Vendor_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and ( a.DeliveryTo_Idno = " & Str(Val(idno)) & " or a.ReceivedFrom_Idno = " & Str(Val(idno)) & " ) and a.Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Empty_Beam <> 0 Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_EmptyBeamDetails.Rows.Clear()
                Sno = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_EmptyBeamDetails.Rows.Add()

                        Sign = 1
                        If Trim(UCase(LedType)) = "JOBWORKER" Then
                            If Val(idno) = Val(dt2.Rows(i).Item("DeliveryTo_Idno").ToString) Then
                                Sign = -1
                            Else
                                Sign = 1
                            End If

                        Else
                            If Val(idno) = Val(dt2.Rows(i).Item("ReceivedFrom_Idno").ToString) Then
                                Sign = -1
                            Else
                                Sign = 1
                            End If

                        End If

                        Sno = Sno + 1
                        dgv_EmptyBeamDetails.Rows(n).Cells(0).Value = Val(Sno)
                        If IsDBNull(dt2.Rows(i).Item("Op_Beam").ToString) = False Then
                            '  If Val(dt2.Rows(i).Item("DeliveryTo_Idno").ToString) <> 0 Then
                            'dgv_EmptyBeamDetails.Rows(n).Cells(1).Value = -1 * Math.Abs(Val(dt2.Rows(i).Item("Op_Beam").ToString))
                            ' Else
                            dgv_EmptyBeamDetails.Rows(n).Cells(1).Value = Sign * Math.Abs(Val(dt2.Rows(i).Item("Op_Beam").ToString))
                            ' End If
                            If Val(dgv_EmptyBeamDetails.Rows(n).Cells(1).Value) = 0 Then
                                dgv_EmptyBeamDetails.Rows(n).Cells(1).Value = ""
                            End If
                        End If
                        dgv_EmptyBeamDetails.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Vendor_Name").ToString
                        dgv_EmptyBeamDetails.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Beam_Width_Name").ToString
                    Next i

                    TotalEmptyBeam_Calculation()

                End If
                dt2.Clear()


                da2 = New SqlClient.SqlDataAdapter("Select sum(voucher_amount) from voucher_details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ledger_idno = " & Str(Val(idno)) & " and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                If dt2.Rows.Count > 0 Then
                    If IsDBNull(dt2.Rows(0).Item(0).ToString) = False Then
                        If Val(dt2.Rows(0).Item(0).ToString) <> 0 Then
                            txt_OpAmount.Text = Trim(Format(Math.Abs(Val(dt2.Rows(0).Item(0).ToString)), "#########0.00"))
                        End If
                        If Val(dt2.Rows(0).Item(0).ToString) >= 0 Then
                            cbo_CrDrType.Text = "Cr"
                        Else
                            cbo_CrDrType.Text = "Dr"
                        End If
                    End If
                End If
                dt2.Clear()

            End If

            da2 = New SqlClient.SqlDataAdapter("select a.*, b.EndsCount_Name,c.Colour_Name,mh.Mill_Name, d.Ledger_Name from Stock_Pavu_Processing_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo LEFT OUTER JOIN Colour_Head c ON a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN Ledger_Head d ON a.StockOf_IdNo = d.Ledger_IdNo LEFT OUTER JOIN Mill_Head mh on a.Mill_IdNo = mh.Mill_IdNo Where a.sl_no > 5000  and a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and (DeliveryTo_Idno = " & Str(Val(idno)) & " or ReceivedFrom_Idno = " & Str(Val(idno)) & ")  and a.Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Sl_No", con)
            dt2 = New DataTable
            da2.Fill(dt2)

            dgv_BobinDetails.Rows.Clear()
            Sno = 0

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_BobinDetails.Rows.Add()

                    Sno = Sno + 1
                    dgv_BobinDetails.Rows(n).Cells(0).Value = Val(Sno)
                    dgv_BobinDetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("EndsCount_Name").ToString
                    dgv_BobinDetails.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Colour_Name").ToString
                    dgv_BobinDetails.Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Bobins").ToString)
                    If Val(dgv_BobinDetails.Rows(n).Cells(3).Value) = 0 Then
                        dgv_BobinDetails.Rows(n).Cells(3).Value = ""
                    End If
                    dgv_BobinDetails.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meters_Bobin").ToString), "########0.00")
                    If Val(dgv_BobinDetails.Rows(n).Cells(4).Value) = 0 Then
                        dgv_BobinDetails.Rows(n).Cells(4).Value = ""
                    End If
                    dgv_BobinDetails.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                    dgv_BobinDetails.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Mill_Name").ToString
                    dgv_BobinDetails.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Ledger_Name").ToString

                    'dgv_PavuDetails.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Pavu_Delivery_Increment").ToString

                    'If dt2.Rows(i).Item("Pavu_Delivery_Code").ToString <> "" Or dt2.Rows(i).Item("Beam_Knotting_Code").ToString <> "" Or Val(dt2.Rows(i).Item("Production_Meters").ToString) <> 0 Or Val(dt2.Rows(i).Item("Close_Status").ToString) <> 0 Then
                    '    dgv_PavuDetails.Rows(n).Cells(6).Value = "1"
                    'Else
                    '    dgv_PavuDetails.Rows(n).Cells(6).Value = ""
                    'End If

                Next i

            End If

            da2 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name,c.Colour_Name from Stock_Yarn_Processing_Details a INNER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo LEFT OUTER JOIN Colour_Head c ON a.Colour_IdNo = c.Colour_IdNo Where a.sl_No > 5000 and a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and (DeliveryTo_Idno = " & Str(Val(idno)) & " or ReceivedFrom_Idno = " & Str(Val(idno)) & ") and a.Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Sl_No", con)
            dt2 = New DataTable
            da2.Fill(dt2)

            dgv_Jari_KuriDetails.Rows.Clear()
            Sno = 0

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Jari_KuriDetails.Rows.Add()

                    Sno = Sno + 1
                    dgv_Jari_KuriDetails.Rows(n).Cells(0).Value = Val(Sno)
                    dgv_Jari_KuriDetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Count_Name").ToString
                    dgv_Jari_KuriDetails.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Colour_Name").ToString
                    dgv_Jari_KuriDetails.Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Jumbo").ToString)
                    If Val(dgv_Jari_KuriDetails.Rows(n).Cells(3).Value) = 0 Then
                        dgv_Jari_KuriDetails.Rows(n).Cells(3).Value = ""
                    End If
                    dgv_Jari_KuriDetails.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Cones").ToString)
                    If Val(dgv_Jari_KuriDetails.Rows(n).Cells(4).Value) = 0 Then
                        dgv_Jari_KuriDetails.Rows(n).Cells(4).Value = ""
                    End If
                    dgv_Jari_KuriDetails.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                    'dgv_PavuDetails.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Pavu_Delivery_Increment").ToString

                    'If dt2.Rows(i).Item("Pavu_Delivery_Code").ToString <> "" Or dt2.Rows(i).Item("Beam_Knotting_Code").ToString <> "" Or Val(dt2.Rows(i).Item("Production_Meters").ToString) <> 0 Or Val(dt2.Rows(i).Item("Close_Status").ToString) <> 0 Then
                    '    dgv_PavuDetails.Rows(n).Cells(6).Value = "1"
                    'Else
                    '    dgv_PavuDetails.Rows(n).Cells(6).Value = ""
                    'End If

                Next i

            End If

            dt1.Clear()

            '--------------

            da3 = New SqlClient.SqlDataAdapter("select * from Opening_Ledger_SalesValue_head a Where a.Ledger_IdNo = " & Str(Val(idno)) & "", con)
            dt3 = New DataTable
            da3.Fill(dt3)

            If dt3.Rows.Count > 0 Then

                lbl_YrCode1.Text = dt3.Rows(0).Item("YearCode1").ToString
                lbl_YrCode2.Text = dt3.Rows(0).Item("YearCode2").ToString
                txt_Sales_Value_Yr1.Text = Val(dt3.Rows(0).Item("Sales_Value1").ToString)
                txt_Sales_Value_Yr2.Text = Val(dt3.Rows(0).Item("Sales_Value2").ToString)

                txt_Purchase_Value_Yr1.Text = Val(dt3.Rows(0).Item("Purchase_Value1").ToString)
                txt_Purchase_Value_Yr2.Text = Val(dt3.Rows(0).Item("Purchase_Value2").ToString)

            End If
            dt3.Clear()
            '--------------


            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            'If cbo_Ledger.Visible And cbo_Ledger.Enabled Then cbo_Ledger.Focus()

        End Try



    End Sub

    Private Sub Opening_Stock_Textile_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_CountName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_CountName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PavuGrid_EndsCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PavuGrid_EndsCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_MillName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "MILL" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_MillName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_ClothName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_ClothName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_JariColour.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COLOUR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_JariColour.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_JariCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_JariCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_BobinGrid_EndsCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_BobinGrid_EndsCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_BobinGrid_Colour.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COLOUR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_BobinGrid_Colour.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_BobinGrid_PartName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_BobinGrid_PartName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_beamwidth.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "BEAMWIDTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_beamwidth.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_VendorName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "VENDOR" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_VendorName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Opening_Stock_Textile_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Dt5 As New DataTable
        Dim Dt6 As New DataTable
        Dim Dt7 As New DataTable
        Dim Dt8 As New DataTable
        Dim Dt9 As New DataTable
        Dim Dt10 As New DataTable
        Dim dttm As DateTime

        Me.Text = ""

        dttm = New DateTime(Val(Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)), Microsoft.VisualBasic.DateAndTime.Month(Common_Procedures.Company_FromDate), Microsoft.VisualBasic.DateAndTime.Day(Common_Procedures.Company_FromDate))
        lbl_Heading.Text = "OPENING STOCK    -    AS ON  :  " & dttm.ToShortDateString

        con.Open()

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead order by Ledger_DisplayName", con)
        da.Fill(Dt1)
        cbo_Ledger.DataSource = Dt1
        cbo_Ledger.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'AGENT') order by Ledger_DisplayName", con)
        da.Fill(Dt2)
        cbo_BillGrid_AgentName.DataSource = Dt2
        cbo_BillGrid_AgentName.DisplayMember = "Ledger_DisplayName"

        cbo_BillGrid_CrDr.Items.Clear()
        cbo_BillGrid_CrDr.Items.Add("CR")
        cbo_BillGrid_CrDr.Items.Add("DR")

        da = New SqlClient.SqlDataAdapter("select distinct(Count_Name) from Count_Head order by Count_Name", con)
        da.Fill(Dt3)
        cbo_Grid_CountName.DataSource = Dt3
        cbo_Grid_CountName.DisplayMember = "Count_Name"

        da = New SqlClient.SqlDataAdapter("select distinct(Mill_Name) from Mill_Head order by Mill_Name", con)
        da.Fill(Dt4)
        cbo_Grid_MillName.DataSource = Dt4
        cbo_Grid_MillName.DisplayMember = "Mill_Name"

        da = New SqlClient.SqlDataAdapter("select distinct(Yarn_Type) from YarnType_Head order by Yarn_Type", con)
        da.Fill(Dt5)
        cbo_Grid_YarnType.DataSource = Dt5
        cbo_Grid_YarnType.DisplayMember = "Yarn_Type"

        da = New SqlClient.SqlDataAdapter("select distinct(EndsCount_Name) from EndsCount_Head order by EndsCount_Name", con)
        da.Fill(Dt6)
        cbo_PavuGrid_EndsCount.DataSource = Dt6
        cbo_PavuGrid_EndsCount.DisplayMember = "EndsCount_Name"

        da = New SqlClient.SqlDataAdapter("select distinct(Cloth_Name) from Cloth_Head order by Cloth_Name", con)
        da.Fill(Dt7)
        cbo_Grid_ClothName.DataSource = Dt7
        cbo_Grid_ClothName.DisplayMember = "Cloth_Name"

        da = New SqlClient.SqlDataAdapter("select distinct(ClothType_Name) from ClothType_Head where (ClothType_IdNo >= 1 and ClothType_IdNo <= 5 ) order by ClothType_Name", con)
        da.Fill(Dt8)
        cbo_Grid_ClothTypeName.DataSource = Dt8
        cbo_Grid_ClothTypeName.DisplayMember = "ClothType_Name"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1) order by Ledger_DisplayName", con)
        da.Fill(Dt9)
        cbo_BobinGrid_PartName.DataSource = Dt9
        cbo_BobinGrid_PartName.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select distinct(Mill_Name) from Mill_Head order by Mill_Name", con)
        da.Fill(Dt10)
        cbo_Grid_BobinMillName.DataSource = Dt10
        cbo_Grid_BobinMillName.DisplayMember = "Mill_Name"

        If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then

            dgv_PavuDetails.Columns(5).HeaderText = "MTR Or WGT"
        End If

        If Trim(UCase(Common_Procedures.settings.Weaver_YarnStock_InMeter_Status)) = 1 Then
            dgv_YarnDetails.Columns(6).HeaderText = "METERS"
        End If

        If Common_Procedures.settings.Show_Yarn_LotNo_Status = 1 Then
            dgv_YarnDetails.Columns(8).Visible = True
            cbo_Grid_Yarn_LotNo.BackColor = Color.White
        End If

        If Common_Procedures.settings.Show_Weaver_JobCard_Entry_STATUS = 1 Then
            dgv_YarnDetails.Columns(9).Visible = True
            dgv_PavuDetails.Columns(7).Visible = True
            cbo_Grid_Yarn_weaving_job_no.BackColor = Color.White
            cbo_Grid_Pavu_weaving_job_no.BackColor = Color.White
        End If

        If Common_Procedures.settings.Show_Sizing_JobCard_Entry_Status = 1 Then
            dgv_YarnDetails.Columns(10).Visible = True
            dgv_PavuDetails.Columns(8).Visible = True
            cbo_Grid_Yarn_Sizing_JobCardNo.BackColor = Color.White
            cbo_grid_Pavu_Sizing_JobCardNo.BackColor = Color.White
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1558" Then
            dgv_ClothDetails.Columns(5).Visible = True
            dgv_ClothDetails.Columns(6).Visible = True
            'dgv_ClothDetails.Columns(4).HeaderText = "QUANTITY (MTR/PCS/KGS)"
        Else

            dgv_ClothDetails.Columns(5).Visible = False
            dgv_ClothDetails.Columns(6).Visible = False


            dgv_ClothDetails.Columns(1).Width = 370
            dgv_ClothDetails.Columns(2).Width = 140
            dgv_ClothDetails.Columns(4).Width = 140

            'dgv_ClothDetails.Columns(4).HeaderText = "METERS"
        End If


        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OpAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_CrDrType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BobinGrid_Colour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BobinGrid_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_JariColour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_JariCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BobinGrid_PartName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EmptyBeam.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EmptyBags.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EmptyCones.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EmptyBobin.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EmptyBobinParty.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_EmptyJumbo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_MillName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_YarnType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PavuGrid_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BillGrid_AgentName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_BillGrid_CrDr.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_ClothTypeName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_VendorName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_beamwidth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_BobinMillName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Sales_Value_Yr1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Sales_Value_Yr2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Purchase_Value_Yr1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Purchase_Value_Yr2.GotFocus, AddressOf ControlGotFocus


        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OpAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_CrDrType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BobinGrid_Colour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BobinGrid_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_JariColour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_JariCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BobinGrid_PartName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EmptyBeam.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EmptyBags.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EmptyCones.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EmptyBobin.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EmptyBobinParty.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_EmptyJumbo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_MillName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_YarnType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PavuGrid_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BillGrid_AgentName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_BillGrid_CrDr.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_ClothTypeName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_VendorName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_beamwidth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_BobinMillName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Sales_Value_Yr1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Sales_Value_Yr2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Purchase_Value_Yr1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Purchase_Value_Yr2.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_OpAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_EmptyBeam.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_EmptyBags.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_EmptyCones.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_EmptyBobin.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_EmptyBobinParty.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_OpAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_EmptyBeam.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_EmptyBags.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_EmptyBobin.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_EmptyBobinParty.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_EmptyCones.KeyPress, AddressOf TextBoxControlKeyPress


        AddHandler cbo_Grid_Yarn_weaving_job_no.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Yarn_Sizing_JobCardNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Yarn_LotNo.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Grid_Pavu_weaving_job_no.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_grid_Pavu_Sizing_JobCardNo.GotFocus, AddressOf ControlGotFocus



        AddHandler cbo_Grid_Yarn_weaving_job_no.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Yarn_Sizing_JobCardNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Yarn_LotNo.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Grid_Pavu_weaving_job_no.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_grid_Pavu_Sizing_JobCardNo.LostFocus, AddressOf ControlLostFocus


        'cbo_Ledger.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
        OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Opening_Stock_Textile_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Opening_Stock_Textile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then
                If MessageBox.Show("Do you want to Close?", "FOR CLOSING ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub
                Else
                    Close_Form()
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

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView
        Dim vLASTCOLNO As Integer = 0

        On Error Resume Next

        If ActiveControl.Name = dgv_YarnDetails.Name Or ActiveControl.Name = dgv_PavuDetails.Name Or ActiveControl.Name = dgv_BillDetails.Name Or ActiveControl.Name = dgv_ClothDetails.Name Or ActiveControl.Name = dgv_EmptyBeamDetails.Name Or ActiveControl.Name = dgv_BobinDetails.Name Or ActiveControl.Name = dgv_Jari_KuriDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_YarnDetails.Name Then
                dgv1 = dgv_YarnDetails

            ElseIf ActiveControl.Name = dgv_PavuDetails.Name Then
                dgv1 = dgv_PavuDetails

            ElseIf ActiveControl.Name = dgv_BillDetails.Name Then
                dgv1 = dgv_BillDetails

            ElseIf ActiveControl.Name = dgv_ClothDetails.Name Then
                dgv1 = dgv_ClothDetails

            ElseIf ActiveControl.Name = dgv_EmptyBeamDetails.Name Then
                dgv1 = dgv_EmptyBeamDetails


            ElseIf ActiveControl.Name = dgv_BobinDetails.Name Then
                dgv1 = dgv_BobinDetails

            ElseIf ActiveControl.Name = dgv_Jari_KuriDetails.Name Then
                dgv1 = dgv_Jari_KuriDetails

            ElseIf dgv_YarnDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_YarnDetails

            ElseIf dgv_PavuDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_PavuDetails

            ElseIf dgv_BillDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_BillDetails

            ElseIf dgv_ClothDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_ClothDetails

            ElseIf dgv_EmptyBeamDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_EmptyBeamDetails


            ElseIf dgv_BobinDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_BobinDetails

            ElseIf dgv_Jari_KuriDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_Jari_KuriDetails

            ElseIf tab_Main.SelectedIndex = 0 Then
                dgv1 = dgv_YarnDetails

            ElseIf tab_Main.SelectedIndex = 1 Then
                dgv1 = dgv_PavuDetails

            ElseIf tab_Main.SelectedIndex = 2 Then
                dgv1 = dgv_BillDetails

            ElseIf tab_Main.SelectedIndex = 3 Then
                dgv1 = dgv_ClothDetails
            ElseIf tab_Main.SelectedIndex = 4 Then
                dgv1 = dgv_EmptyBeamDetails

            ElseIf tab_Main.SelectedIndex = 5 Then
                dgv1 = dgv_BobinDetails

            ElseIf tab_Main.SelectedIndex = 6 Then
                dgv1 = dgv_Jari_KuriDetails

            End If

            With dgv1



                If dgv1.Name = dgv_YarnDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If dgv_YarnDetails.Columns(10).Visible = True Then
                            vLASTCOLNO = 10
                        ElseIf dgv_YarnDetails.Columns(9).Visible = True Then
                            vLASTCOLNO = 9
                        ElseIf dgv_YarnDetails.Columns(8).Visible = True Then
                            vLASTCOLNO = 8
                        Else
                            vLASTCOLNO = 7

                        End If


                        If .CurrentCell.ColumnIndex >= vLASTCOLNO Then

                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                tab_Main.SelectTab(1)
                                dgv_PavuDetails.Focus()
                                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
                                dgv_PavuDetails.CurrentCell.Selected = True

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                                tab_Main.SelectTab(1)
                                dgv_PavuDetails.Focus()
                                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
                                dgv_PavuDetails.CurrentCell.Selected = True

                            ElseIf .CurrentCell.ColumnIndex = 7 Then
                                If dgv_YarnDetails.Columns(8).Visible Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                                ElseIf dgv_YarnDetails.Columns(9).Visible Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 2)
                                ElseIf dgv_YarnDetails.Columns(10).Visible Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 3)
                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                            End If

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then
                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                txt_EmptyCones.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.CurrentCell.ColumnIndex - 1)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If

                ElseIf dgv1.Name = dgv_PavuDetails.Name Then

                    If dgv1.Columns(8).Visible = True Then
                        vLASTCOLNO = 8
                    ElseIf dgv1.Columns(7).Visible = True Then
                        vLASTCOLNO = 7
                    Else
                        vLASTCOLNO = 5
                    End If

                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If .CurrentCell.ColumnIndex >= vLASTCOLNO Then

                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                If dgv_BillDetails.Enabled = True Then
                                    tab_Main.SelectTab(2)
                                    dgv_BillDetails.Focus()
                                    dgv_BillDetails.CurrentCell = dgv_BillDetails.Rows(0).Cells(1)
                                    dgv_BillDetails.CurrentCell.Selected = True

                                ElseIf dgv_ClothDetails.Enabled = True Then
                                    tab_Main.SelectTab(3)
                                    dgv_ClothDetails.Focus()
                                    dgv_ClothDetails.CurrentCell = dgv_ClothDetails.Rows(0).Cells(1)

                                Else
                                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                                        save_record()

                                    Else
                                        tab_Main.SelectTab(0)
                                        cbo_Ledger.Focus()

                                    End If

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 2 And (Trim(.CurrentRow.Cells(1).Value) = "" And Trim(.CurrentRow.Cells(2).Value) = "") Then
                                If dgv_BillDetails.Enabled = True Then
                                    tab_Main.SelectTab(2)
                                    dgv_BillDetails.Focus()
                                    dgv_BillDetails.CurrentCell = dgv_BillDetails.Rows(0).Cells(1)
                                    dgv_BillDetails.CurrentCell.Selected = True

                                ElseIf dgv_ClothDetails.Enabled = True Then
                                    tab_Main.SelectTab(3)
                                    dgv_ClothDetails.Focus()
                                    dgv_ClothDetails.CurrentCell = dgv_ClothDetails.Rows(0).Cells(1)

                                Else
                                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                                        save_record()

                                    Else
                                        tab_Main.SelectTab(0)
                                        cbo_Ledger.Focus()

                                    End If

                                End If
                            ElseIf .CurrentCell.ColumnIndex = 6 Then
                                If dgv_PavuDetails.Columns(7).Visible Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 2)
                                End If
                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then
                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                tab_Main.SelectTab(0)
                                dgv_YarnDetails.Focus()
                                dgv_YarnDetails.CurrentCell = dgv_BillDetails.Rows(0).Cells(1)

                            Else

                                If dgv_PavuDetails.Columns(8).Visible Then
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(8)
                                ElseIf dgv_PavuDetails.Columns(7).Visible Then
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(7)
                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(5)
                                End If

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 8 Then

                            If dgv_PavuDetails.Columns(7).Visible Then
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 2)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If

                ElseIf dgv1.Name = dgv_BillDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If .CurrentCell.ColumnIndex >= .ColumnCount - 2 Then

                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                tab_Main.SelectTab(3)
                                dgv_ClothDetails.Focus()
                                dgv_ClothDetails.CurrentCell = dgv_ClothDetails.Rows(0).Cells(1)

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                                tab_Main.SelectTab(3)
                                dgv_ClothDetails.Focus()
                                dgv_ClothDetails.CurrentCell = dgv_ClothDetails.Rows(0).Cells(1)

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then
                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                tab_Main.SelectTab(0)
                                dgv_YarnDetails.Focus()
                                dgv_YarnDetails.CurrentCell = dgv_BillDetails.Rows(0).Cells(1)

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 3)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If

                ElseIf dgv1.Name = dgv_ClothDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then

                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                tab_Main.SelectTab(4)
                                dgv_EmptyBeamDetails.Focus()
                                dgv_EmptyBeamDetails.CurrentCell = dgv_EmptyBeamDetails.Rows(0).Cells(1)


                            ElseIf .CurrentCell.ColumnIndex = 5 Then
                                If dgv_PavuDetails.Columns(7).Visible Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 4 Then
                            If dgv_ClothDetails.Columns(5).Visible Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                            ElseIf dgv_ClothDetails.Columns(6).Visible Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 2)
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                                tab_Main.SelectTab(4)
                                dgv_EmptyBeamDetails.Focus()
                                dgv_EmptyBeamDetails.CurrentCell = dgv_EmptyBeamDetails.Rows(0).Cells(1)

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then
                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                tab_Main.SelectTab(2)
                                dgv_BillDetails.Focus()
                                dgv_BillDetails.CurrentCell = dgv_BillDetails.Rows(0).Cells(1)


                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.CurrentCell.ColumnIndex - 1)

                            End If





                        Else

                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If

                ElseIf dgv1.Name = dgv_EmptyBeamDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                tab_Main.SelectTab(5)
                                dgv_BobinDetails.Focus()
                                dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                                tab_Main.SelectTab(5)
                                dgv_BobinDetails.Focus()
                                dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                tab_Main.SelectTab(2)
                                dgv_ClothDetails.Focus()
                                dgv_ClothDetails.CurrentCell = dgv_ClothDetails.Rows(0).Cells(1)

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



                ElseIf dgv1.Name = dgv_BobinDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then

                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                tab_Main.SelectTab(6)
                                dgv_Jari_KuriDetails.Focus()
                                dgv_Jari_KuriDetails.CurrentCell = dgv_Jari_KuriDetails.Rows(0).Cells(1)

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                                tab_Main.SelectTab(6)
                                dgv_Jari_KuriDetails.Focus()
                                dgv_Jari_KuriDetails.CurrentCell = dgv_Jari_KuriDetails.Rows(0).Cells(1)

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then
                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                tab_Main.SelectTab(4)
                                dgv_EmptyBeamDetails.Focus()
                                dgv_EmptyBeamDetails.CurrentCell = dgv_EmptyBeamDetails.Rows(0).Cells(1)

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.CurrentCell.ColumnIndex - 1)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If

                ElseIf dgv1.Name = dgv_Jari_KuriDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then

                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                                    save_record()

                                Else
                                    tab_Main.SelectTab(0)
                                    cbo_Ledger.Focus()

                                End If


                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                                    save_record()

                                Else
                                    tab_Main.SelectTab(0)
                                    cbo_Ledger.Focus()

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then
                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                If dgv_BillDetails.Enabled Then
                                    tab_Main.SelectTab(5)
                                    dgv_BobinDetails.Focus()
                                    dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)

                                Else
                                    tab_Main.SelectTab(3)
                                    dgv_ClothDetails.Focus()
                                    dgv_ClothDetails.CurrentCell = dgv_ClothDetails.Rows(0).Cells(1)

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.CurrentCell.ColumnIndex - 1)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If
                Else
                    Return MyBase.ProcessCmdKey(msg, keyData)

                End If

            End With

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim tr As SqlClient.SqlTransaction
        Dim cmd As New SqlClient.SqlCommand
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As DataTable
        Dim NewCode As String
        Dim LedName As String

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Textile_OpeningStock, "~L~") = 0 And InStr(Common_Procedures.UR.Textile_OpeningStock, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Textile_OpeningStock, New_Entry, Me) = False Then Exit Sub



        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to Delete?", "FOR DELETION...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If Val(lbl_IdNo.Text) = 0 Then
            MessageBox.Show("Invalid Ledger", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        LedName = Common_Procedures.Ledger_IdNoToName(con, Val(lbl_IdNo.Text))

        If Trim(LedName) = "" Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Val(lbl_IdNo.Text)) & "/" & Trim(OpYrCode)

        da = New SqlClient.SqlDataAdapter("select count(*) from Stock_SizedPavu_Processing_Details where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and (Pavu_Delivery_Code <> '' or Pavu_Delivery_Increment <> 0 OR Beam_Knotting_Code <> '' or Loom_Idno <> 0 or Production_Meters <> 0 or Close_Status <> 0) ", con)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                If Val(dt.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Pavu Delivered (or) Production Entered", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If

        'da = New SqlClient.SqlDataAdapter("select sum(Delivered_Weight) from Stock_BabyCone_Processing_Details where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
        'dt = New DataTable
        'da.Fill(dt)
        'If dt.Rows.Count > 0 Then
        '    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
        '        If Val(dt.Rows(0)(0).ToString) > 0 Then
        '            MessageBox.Show("BabyCone Delivered", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '            Exit Sub
        '        End If
        '    End If
        'End If

        da = New SqlClient.SqlDataAdapter("select count(*) from voucher_bill_head where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and entry_identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and bill_amount <> (credit_amount + debit_amount) ", con)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                If Val(dt.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Alrady Amount Received/Paid for some bills", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If


        cmd.Connection = con

        cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
        cmd.ExecuteNonQuery()

        tr = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = tr

            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), tr) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
                Exit Sub
            End If

            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Then

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno      , Count_IdNo, Yarn_Type, Mill_IdNo ) " &
                                          " Select                               'YARN'    , Reference_Code, Reference_Date, Company_IdNo, ReceivedFrom_Idno, Count_IdNo, Yarn_Type, Mill_IdNo from Stock_Yarn_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno    , Count_IdNo, Yarn_Type, Mill_IdNo ) " &
                                      " Select                               'YARN'    , Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_Idno, Count_IdNo, Yarn_Type, Mill_IdNo from Stock_Yarn_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()


                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno   , Ledger_Idno    , EndsCount_IdNo ) " &
                      " Select                               'PAVU'    , Reference_Code, Reference_Date, Company_IdNo, ReceivedFrom_Idno, EndsCount_IdNo from Stock_Pavu_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno    , EndsCount_IdNo ) " &
                      " Select                               'PAVU'    , Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_Idno, EndsCount_IdNo from Stock_Pavu_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)) & " and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_SizedPavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Stock_BabyCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Then
                If Common_Procedures.Check_is_Negative_Stock_Status(con, tr) = True Then Exit Sub
            End If

            tr.Commit()

            tr.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If cbo_Ledger.Enabled = True And cbo_Ledger.Visible = True Then cbo_Ledger.Focus()

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        '-----
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As Integer

        Try
            cmd.Connection = con
            cmd.CommandText = "select top 1 Ledger_IdNo from Ledger_Head where Ledger_IdNo <> 0 Order by Ledger_IdNo"
            dr = cmd.ExecuteReader

            movno = 0
            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = Val(dr(0).ToString)
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
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As Integer = 0
        Dim OrdByNo As Single

        Try

            OrdByNo = Val(lbl_IdNo.Text)

            da = New SqlClient.SqlDataAdapter("select top 1 Ledger_IdNo from Ledger_Head where Ledger_IdNo > " & Str(OrdByNo) & " Order by Ledger_IdNo", con)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim cmd As New SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim movno As Integer = 0
        Dim OrdByNo As Single

        Try

            OrdByNo = Val(lbl_IdNo.Text)

            cmd.Connection = con
            cmd.CommandText = "select top 1 Ledger_IdNo from Ledger_Head where ledger_idno < " & Str(Val(OrdByNo)) & " Order by Ledger_IdNo desc"

            dr = cmd.ExecuteReader

            If dr.HasRows Then
                If dr.Read Then
                    If IsDBNull(dr(0).ToString) = False Then
                        movno = Val(dr(0).ToString)
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
        Dim da As New SqlClient.SqlDataAdapter("select top 1 Ledger_IdNo from Ledger_Head where ledger_idno <> 0 Order by Ledger_IdNo desc", con)
        Dim dt As New DataTable
        Dim movno As Integer

        Try
            da.Fill(dt)

            movno = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Val(dt.Rows(0)(0).ToString)
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
        Dim dt2 As New DataTable
        Dim NewID As Integer = 0

        Try

            clear()

            cbo_Ledger.Enabled = True

            New_Entry = True

            da = New SqlClient.SqlDataAdapter("select max(ledger_idno) from Ledger_Head where ledger_idno <> 0", con)
            dt = New DataTable
            da.Fill(dt)

            NewID = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    NewID = Val(dt.Rows(0)(0).ToString)
                End If
            End If

            If Val(NewID) <= 100 Then NewID = 100

            lbl_IdNo.Text = Val(NewID) + 1

            lbl_IdNo.ForeColor = Color.Red

            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try


    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        '-----
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        '-----
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim NewCode As String = ""
        Dim LedName As String
        Dim Sno As Integer = 0
        Dim Nr As Integer = 0
        Dim OpDate As Date
        Dim VouAmt As Double
        Dim Dlv_IdNo As Integer, Rec_IdNo As Integer
        Dim Cnt_ID As Integer, pCnt_ID As Integer, pEdsCnt_ID As Integer, pEds_Nm As Integer
        Dim Mil_ID, Led_ID, Mill_ID As Integer
        Dim Clo_ID As Integer, CloTyp_ID As Integer
        Dim vSetNo As String
        Dim vSetCd As String
        Dim Selc_SetCode As String
        Dim Dup_SetCd As String = ""
        Dim Dup_SetNoBmNo As String = ""
        Dim Mtr_Pc As Double = 0
        Dim vTot_YrnCns As Double, vTot_YrnBgs As Double, vTot_YrnWght As Double, vTot_YrnBobin As Double
        Dim vTot_PvuBms As Double, vTot_PvuPcs As Double, vTot_PvuMtrs As Double
        Dim vTot_CloMtrs As Double
        Dim vAgt_ID As Integer = 0
        Dim vTot_BlAmt As Double, vTot_Bl_PydRcvd_Amt As Double
        Dim bl_amt As Double = 0
        Dim CrDr_Amt_ColNm As String = ""
        Dim vou_bil_no As String = ""
        Dim vou_bil_code As String = ""
        Dim Led_Type As String = ""
        Dim StkOf_IdNo As Integer = 0
        Dim LedTyp As String = ""
        Dim BobinEdsCnt_ID As Integer, BobinClr_ID As Integer
        Dim JariCnt_ID As Integer, JariClr_ID As Integer
        Dim BnStk_IdNo As Integer = 0
        Dim Bw_ID As Integer = 0
        Dim Ven_id As Integer = 0
        Dim vENTDB_TotYRNWgt As String = 0
        Dim vENTDB_TotPAVUmtrs As String = 0


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Textile_OpeningStock, New_Entry, Me) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(cbo_Ledger.Text) = "" Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled Then cbo_Ledger.Focus()
            Exit Sub
        End If

        If Val(lbl_IdNo.Text) = 0 Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled Then cbo_Ledger.Focus()
            Exit Sub
        End If

        Led_ID = Common_Procedures.Ledger_NameToIdNo(con, Trim(cbo_Ledger.Text))
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Ledger", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled Then cbo_Ledger.Focus()
            Exit Sub
        End If

        LedName = Common_Procedures.Ledger_IdNoToName(con, Val(lbl_IdNo.Text))
        If Trim(LedName) = "" Then
            MessageBox.Show("Invalid Ledger Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled Then cbo_Ledger.Focus()
            Exit Sub
        End If

        If Val(txt_OpAmount.Text) <> 0 And Trim(cbo_CrDrType.Text) = "" Then
            MessageBox.Show("Invalid Cr/Dr", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_CrDrType.Enabled Then cbo_CrDrType.Focus()
            Exit Sub
        End If

        Led_Type = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Led_ID)) & ")")

        With dgv_YarnDetails

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(6).Value) <> 0 Then

                    Sno = Sno + 1

                    Cnt_ID = Common_Procedures.Count_NameToIdNo(con, Trim(.Rows(i).Cells(1).Value))
                    If Val(Cnt_ID) = 0 Then
                        MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            tab_Main.SelectTab(0)
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                            '.CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If Trim(.Rows(i).Cells(2).Value) = "" Then
                        MessageBox.Show("Invalid Yarn Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            tab_Main.SelectTab(0)
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(2)
                            '.CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    Mil_ID = Common_Procedures.Mill_NameToIdNo(con, Trim(.Rows(i).Cells(3).Value))
                    If Val(Mil_ID) = 0 And (Trim(UCase(Led_Type)) <> "WEAVER" And Trim(UCase(Led_Type)) <> "JOBWORKER") Then
                        MessageBox.Show("Invalid Mill Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            tab_Main.SelectTab(0)
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(3)
                            '.CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If Trim(UCase(.Rows(i).Cells(2).Value)) = "BABY" And Trim(.Rows(i).Cells(7).Value) = "" Then
                        MessageBox.Show("Invalid SetNo for BabyYarn", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            tab_Main.SelectTab(0)
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(7)
                            '.CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If Trim(.Rows(i).Cells(7).Value) <> "" Then
                        If InStr(1, Trim(.Rows(i).Cells(7).Value), " ") > 0 Then
                            MessageBox.Show("Invalid Set No, Spaces not allowed in SetNo", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If .Enabled And .Visible Then
                                tab_Main.SelectTab(0)
                                .Focus()
                                .CurrentCell = .Rows(i).Cells(7)
                                .CurrentCell.Selected = True
                            End If
                            Exit Sub
                        End If
                    End If

                    If Trim(UCase(.Rows(i).Cells(2).Value)) = "BABY" Then

                        If InStr(1, Trim(UCase(Dup_SetCd)), "~" & Trim(UCase(.Rows(i).Cells(7).Value)) & "~") > 0 Then
                            MessageBox.Show("Duplicate SetNo for BabyYarn", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If .Enabled And .Visible Then
                                tab_Main.SelectTab(0)
                                .Focus()
                                .CurrentCell = .Rows(i).Cells(7)
                                .CurrentCell.Selected = True
                            End If

                            Exit Sub
                        End If

                        Dup_SetCd = Trim(Dup_SetCd) & "~" & Trim(UCase(dgv_YarnDetails.Rows(i).Cells(7).Value)) & "~"

                    End If
                End If

                If Trim(.Rows(i).Cells(9).Value) <> "" Then
                    If Common_Procedures.Cross_Checking_For_Weaving_Job_Code_For_Selecion(con, Val(Led_ID), Trim(.Rows(i).Cells(9).Value), Val(Cnt_ID)) = True Then
                        MessageBox.Show("MisMatch of Party Job No Details", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            tab_Main.SelectTab(0)
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(9)
                            .CurrentCell.Selected = True

                        End If
                        Exit Sub
                    End If
                End If

                If Trim(.Rows(i).Cells(10).Value) <> "" Then
                    If Common_Procedures.Cross_Checking_For_Sizing_Job_Code_For_Selecion(con, Val(Led_ID), Trim(.Rows(i).Cells(10).Value), Val(Cnt_ID)) = True Then
                        MessageBox.Show("MisMatch of Party Job No Details", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            tab_Main.SelectTab(0)
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(10)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If
                End If

            Next

        End With



        With dgv_PavuDetails

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(5).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(1).Value) = "" And (Trim(UCase(Led_Type)) <> "WEAVER" And Trim(UCase(Led_Type)) <> "JOBWORKER") Then
                        MessageBox.Show("Invalid Set No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            tab_Main.SelectTab(1)
                            .Focus()
                            .CurrentCell = dgv_PavuDetails.Rows(i).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If InStr(1, Trim(.Rows(i).Cells(1).Value), " ") > 0 Then
                        MessageBox.Show("Invalid Set No, Spaces not allowed in SetNo", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            tab_Main.SelectTab(1)
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    pEdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, Trim(.Rows(i).Cells(2).Value))
                    If Val(pEdsCnt_ID) = 0 Then
                        MessageBox.Show("Invalid Ends Count", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            tab_Main.SelectTab(1)
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(2)
                            '.CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If Trim(.Rows(i).Cells(3).Value) = "" And (Trim(UCase(Led_Type)) <> "WEAVER" And Trim(UCase(Led_Type)) <> "JOBWORKER") Then
                        MessageBox.Show("Invalid Beam No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            tab_Main.SelectTab(1)
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(3)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If InStr(1, Trim(.Rows(i).Cells(3).Value), " ") > 0 Then
                        MessageBox.Show("Invalid Beam No, Spaces not allowed in SetNo", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            tab_Main.SelectTab(1)
                            .Focus()
                            .CurrentCell = dgv_PavuDetails.Rows(i).Cells(3)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If Trim(.Rows(i).Cells(3).Value) <> "" Then
                        If InStr(1, Trim(UCase(Dup_SetNoBmNo)), "~" & Trim(UCase(.Rows(i).Cells(1).Value)) & "-" & Trim(UCase(.Rows(i).Cells(3).Value)) & "~") > 0 Then
                            MessageBox.Show("Duplicate BeamNo for set no. " & Trim(.Rows(i).Cells(1).Value), "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If .Enabled And .Visible Then
                                tab_Main.SelectTab(1)
                                .Focus()
                                .CurrentCell = .Rows(i).Cells(3)
                                .CurrentCell.Selected = True
                            End If
                            Exit Sub
                        End If
                    End If


                    Dup_SetNoBmNo = Trim(Dup_SetNoBmNo) & "~" & Trim(UCase(.Rows(i).Cells(1).Value)) & "-" & Trim(UCase(.Rows(i).Cells(3).Value)) & "~"

                End If

                If Trim(.Rows(i).Cells(7).Value) <> "" Then
                    If Common_Procedures.Cross_Checking_For_Weaving_Job_Code_For_Selecion(con, Val(Led_ID), Trim(.Rows(i).Cells(7).Value), Nothing, Val(pEdsCnt_ID)) = True Then
                        MessageBox.Show("MisMatch of Party Job No Details", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                        tab_Main.SelectTab(1)
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(7)
                            .CurrentCell.Selected = True


                        Exit Sub
                    End If
                End If

                If Trim(.Rows(i).Cells(8).Value) <> "" Then
                    If Common_Procedures.Cross_Checking_For_Sizing_Job_Code_For_Selecion(con, Val(Led_ID), Trim(.Rows(i).Cells(8).Value), Nothing, Val(pEdsCnt_ID)) = True Then
                        MessageBox.Show("MisMatch of Party Job No Details", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                        tab_Main.SelectTab(1)
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(8)
                            .CurrentCell.Selected = True


                        Exit Sub
                    End If
                End If



            Next i

        End With

        With dgv_BillDetails

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(4).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(1).Value) = "" Then
                        MessageBox.Show("Invalid Bill No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            tab_Main.SelectTab(2)
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If Trim(.Rows(i).Cells(2).Value) = "" Then
                        MessageBox.Show("Invalid Bill Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            tab_Main.SelectTab(2)
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(2)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If IsDate(.Rows(i).Cells(2).Value) = False Then
                        MessageBox.Show("Invalid Bill Date format", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            tab_Main.SelectTab(2)
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(2)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If Trim(.Rows(i).Cells(3).Value) <> "" Then
                        vAgt_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, Trim(.Rows(i).Cells(3).Value))
                        If Val(vAgt_ID) = 0 Then
                            MessageBox.Show("Invalid Agent Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            If .Enabled And .Visible Then
                                tab_Main.SelectTab(2)
                                .Focus()
                                .CurrentCell = .Rows(i).Cells(3)
                            End If
                            Exit Sub
                        End If
                    End If

                    If Trim(.Rows(i).Cells(5).Value) = "" Then
                        MessageBox.Show("Invalid Cr/Dr", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            tab_Main.SelectTab(2)
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(5)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                End If

            Next i

        End With


        With dgv_ClothDetails

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(4).Value) <> 0 Then

                    Sno = Sno + 1

                    Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, Trim(.Rows(i).Cells(1).Value))
                    If Val(Clo_ID) = 0 Then
                        MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            tab_Main.SelectTab(3)
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If
                        Exit Sub
                    End If

                    CloTyp_ID = Common_Procedures.ClothType_NameToIdNo(con, Trim(.Rows(i).Cells(2).Value))
                    If Val(CloTyp_ID) = 0 Then
                        MessageBox.Show("Invalid Cloth Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            tab_Main.SelectTab(3)
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(2)
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(3).Value) = 0 Then
                        MessageBox.Show("Invalid Folding %", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            tab_Main.SelectTab(3)
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(3)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                End If

            Next

        End With

        With dgv_EmptyBeamDetails

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(1).Value) <> 0 Then

                    If Val(.Rows(i).Cells(1).Value) = 0 Then
                        MessageBox.Show("Invalid Empty Beam", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If
                        Exit Sub
                    End If

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1102" Then
                        If Trim(.Rows(i).Cells(3).Value) = "" Then
                            MessageBox.Show("Invalid Beam Width", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If .Enabled And .Visible Then
                                .Focus()
                                .CurrentCell = .Rows(i).Cells(3)
                            End If
                            Exit Sub
                        End If
                    End If

                End If

            Next
        End With

        TotalEmptyBeam_Calculation()
        Total_BillAmount_Calculation()


        vTot_YrnCns = 0 : vTot_YrnBgs = 0 : vTot_YrnWght = 0 : vTot_YrnBobin = 0
        If dgv_YarnDetails_Total.RowCount > 0 Then
            vTot_YrnBgs = Val(dgv_YarnDetails_Total.Rows(0).Cells(4).Value())
            vTot_YrnCns = Val(dgv_YarnDetails_Total.Rows(0).Cells(5).Value())
            vTot_YrnWght = Val(dgv_YarnDetails_Total.Rows(0).Cells(6).Value())
        End If

        vTot_PvuBms = 0 : vTot_PvuPcs = 0 : vTot_PvuMtrs = 0
        If dgv_PavuDetails_Total.RowCount > 0 Then
            vTot_PvuBms = Val(dgv_PavuDetails_Total.Rows(0).Cells(3).Value())
            vTot_PvuPcs = Val(dgv_PavuDetails_Total.Rows(0).Cells(4).Value())
            vTot_PvuMtrs = Val(dgv_PavuDetails_Total.Rows(0).Cells(5).Value())
        End If

        vTot_BlAmt = 0 : vTot_Bl_PydRcvd_Amt = 0
        If dgv_BillDetails_Total.RowCount > 0 Then
            vTot_BlAmt = Val(dgv_BillDetails_Total.Rows(0).Cells(4).Value())
            vTot_Bl_PydRcvd_Amt = Val(dgv_BillDetails_Total.Rows(0).Cells(6).Value())
        End If

        vTot_CloMtrs = 0
        If dgv_ClothDetails_Total.RowCount > 0 Then
            vTot_CloMtrs = Val(dgv_ClothDetails_Total.Rows(0).Cells(4).Value())
        End If


        cmd.Connection = con

        cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
        cmd.ExecuteNonQuery()


        tr = con.BeginTransaction

        Try

            OpDate = CDate("01-04-" & Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4))
            OpDate = DateAdd(DateInterval.Day, -1, OpDate)

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@OpeningDate", OpDate)

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Val(lbl_IdNo.Text)) & "/" & Trim(OpYrCode)

            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)) & " and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Sno = 0

            If Val(txt_OpAmount.Text) <> 0 Then

                VouAmt = Math.Abs(Val(txt_OpAmount.Text))
                If Trim(UCase(cbo_CrDrType.Text)) = "DR" Then VouAmt = -1 * VouAmt

                Sno = Sno + 1

                cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, Sl_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification, Software_Module_IdNo ) Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(Val(lbl_IdNo.Text)) & "', " & Str(Val(lbl_IdNo.Text)) & ", 'Opng', @OpeningDate, " & Str(Val(Sno)) & ", " & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(VouAmt)) & ", 'Opening', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.SoftwareTypes.Textile_Software)) & " )"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_EmptyBeamDetails
                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(1).Value) <> 0 Then

                        Sno = Sno + 1

                        Ven_id = Common_Procedures.Vendor_AlaisNameToIdNo(con, .Rows(i).Cells(2).Value, tr)
                        Bw_ID = Common_Procedures.BeamWidth_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)

                        Dlv_IdNo = 0
                        Rec_IdNo = 0

                        'If Val(.Rows(i).Cells(1).Value) > 0 Then
                        '    Dlv_IdNo = Val(lbl_IdNo.Text)
                        'Else
                        '    Rec_IdNo = Val(lbl_IdNo.Text)
                        'End If

                        If Trim(UCase(Led_Type)) = "JOBWORKER" Then
                            If Val(.Rows(i).Cells(1).Value) < 0 Then
                                Dlv_IdNo = Val(lbl_IdNo.Text)
                            Else
                                Rec_IdNo = Val(lbl_IdNo.Text)
                            End If
                        Else
                            If Val(.Rows(i).Cells(1).Value) < 0 Then
                                Rec_IdNo = Val(lbl_IdNo.Text)
                            Else
                                Dlv_IdNo = Val(lbl_IdNo.Text)
                            End If
                        End If

                        cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Sl_No, Vendor_IdNo, Beam_Width_IdNo, Empty_Beam, Empty_Bags, Empty_Cones, Particulars) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_IdNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_IdNo.Text))) & ", @OpeningDate, " & Str(Val(Dlv_IdNo)) & ", " & Str(Val(Rec_IdNo)) & ", '', " & Str(Val(Sno)) & " , " & Str(Val(Ven_id)) & ", " & Str(Val(Bw_ID)) & ", " & Str(Math.Abs(Val(.Rows(i).Cells(1).Value))) & ", 0, 0, '' )"
                        cmd.ExecuteNonQuery()

                    End If

                Next
            End With

            If Trim(UCase(Led_Type)) <> "JOBWORKER" And Trim(UCase(Led_Type)) <> "WEAVER" Then
                If Val(vTot_PvuBms) <> 0 Then

                    Dlv_IdNo = 0
                    Rec_IdNo = 0

                    If Val(vTot_PvuBms) < 0 Then
                        Rec_IdNo = Val(lbl_IdNo.Text)
                    Else
                        Dlv_IdNo = Val(lbl_IdNo.Text)
                    End If

                    cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, Vendor_idNo , Beam_Width_IdNo, Pavu_Beam) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_IdNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_IdNo.Text))) & ", @OpeningDate, " & Str(Val(Dlv_IdNo)) & ", " & Str(Val(Rec_IdNo)) & ", '', '', '', 2001, 0, 0, " & Str(Math.Abs(Val(vTot_PvuBms))) & " )"
                    cmd.ExecuteNonQuery()

                End If
            End If


            If Val(txt_EmptyBags.Text) <> 0 Then

                Dlv_IdNo = 0
                Rec_IdNo = 0

                If Trim(UCase(Led_Type)) = "JOBWORKER" Then
                    If Val(txt_EmptyBags.Text) < 0 Then
                        Dlv_IdNo = Val(lbl_IdNo.Text)
                    Else
                        Rec_IdNo = Val(lbl_IdNo.Text)
                    End If
                Else
                    If Val(txt_EmptyBags.Text) < 0 Then
                        Rec_IdNo = Val(lbl_IdNo.Text)
                    Else
                        Dlv_IdNo = Val(lbl_IdNo.Text)
                    End If
                End If

                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, Empty_Bags ) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_IdNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_IdNo.Text))) & ", @OpeningDate, " & Str(Val(Dlv_IdNo)) & ", " & Str(Val(Rec_IdNo)) & ", '', '', '', 3001, " & Str(Math.Abs(Val(txt_EmptyBags.Text))) & " )"
                cmd.ExecuteNonQuery()

            End If

            If Val(vTot_YrnBgs) <> 0 Then

                Dlv_IdNo = 0
                Rec_IdNo = 0
                If Trim(UCase(Led_Type)) = "JOBWORKER" Then
                    If Val(vTot_YrnBgs) < 0 Then
                        Dlv_IdNo = Val(lbl_IdNo.Text)
                    Else
                        Rec_IdNo = Val(lbl_IdNo.Text)
                    End If
                Else
                    If Val(vTot_YrnBgs) < 0 Then
                        Rec_IdNo = Val(lbl_IdNo.Text)
                    Else
                        Dlv_IdNo = Val(lbl_IdNo.Text)
                    End If
                End If

                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, Yarn_Bags) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_IdNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_IdNo.Text))) & ", @OpeningDate, " & Str(Val(Dlv_IdNo)) & ", " & Str(Val(Rec_IdNo)) & ", '', '', '', 4001, " & Str(Math.Abs(Val(vTot_YrnBgs))) & " )"
                cmd.ExecuteNonQuery()

            End If

            If Val(txt_EmptyCones.Text) <> 0 Then

                Dlv_IdNo = 0
                Rec_IdNo = 0

                If Trim(UCase(Led_Type)) = "JOBWORKER" Then
                    If Val(txt_EmptyCones.Text) < 0 Then
                        Dlv_IdNo = Val(lbl_IdNo.Text)
                    Else
                        Rec_IdNo = Val(lbl_IdNo.Text)
                    End If
                Else
                    If Val(txt_EmptyCones.Text) < 0 Then
                        Rec_IdNo = Val(lbl_IdNo.Text)
                    Else
                        Dlv_IdNo = Val(lbl_IdNo.Text)
                    End If
                End If

                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, Empty_Cones ) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_IdNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_IdNo.Text))) & ", @OpeningDate, " & Str(Val(Dlv_IdNo)) & ", " & Str(Val(Rec_IdNo)) & ", '', '', '', 5001, " & Str(Math.Abs(Val(txt_EmptyCones.Text))) & " )"
                cmd.ExecuteNonQuery()

            End If

            If Val(vTot_YrnCns) <> 0 Then

                Dlv_IdNo = 0
                Rec_IdNo = 0

                If Trim(UCase(Led_Type)) = "JOBWORKER" Then
                    If Val(vTot_YrnCns) < 0 Then
                        Dlv_IdNo = Val(lbl_IdNo.Text)
                    Else
                        Rec_IdNo = Val(lbl_IdNo.Text)
                    End If
                Else
                    If Val(vTot_YrnCns) < 0 Then
                        Rec_IdNo = Val(lbl_IdNo.Text)
                    Else
                        Dlv_IdNo = Val(lbl_IdNo.Text)
                    End If
                End If

                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, Yarn_Cones) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_IdNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_IdNo.Text))) & ", @OpeningDate, " & Str(Val(Dlv_IdNo)) & ", " & Str(Val(Rec_IdNo)) & ", '', '', '', 6001, " & Str(Math.Abs(Val(vTot_YrnCns))) & " )"
                cmd.ExecuteNonQuery()

            End If

            If Val(txt_EmptyBobin.Text) <> 0 Then

                Dlv_IdNo = 0
                Rec_IdNo = 0

                If Val(txt_EmptyBobin.Text) < 0 Then
                    Rec_IdNo = Val(lbl_IdNo.Text)
                Else
                    Dlv_IdNo = Val(lbl_IdNo.Text)
                End If

                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, Empty_Bobin ) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_IdNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_IdNo.Text))) & ", @OpeningDate, " & Str(Val(Dlv_IdNo)) & ", " & Str(Val(Rec_IdNo)) & ", '', '', '', 7001, " & Str(Math.Abs(Val(txt_EmptyBobin.Text))) & " )"
                cmd.ExecuteNonQuery()

            End If

            If Val(txt_EmptyBobinParty.Text) <> 0 Then

                Dlv_IdNo = 0
                Rec_IdNo = 0

                If Val(txt_EmptyBobinParty.Text) < 0 Then
                    Dlv_IdNo = Val(lbl_IdNo.Text)
                Else
                    Rec_IdNo = Val(lbl_IdNo.Text)
                End If

                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EmptyBobin_Party) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_IdNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_IdNo.Text))) & ", @OpeningDate, " & Str(Val(Dlv_IdNo)) & ", " & Str(Val(Rec_IdNo)) & ", '', '', '', 8001, " & Str(Math.Abs(Val(txt_EmptyBobinParty.Text))) & " )"
                cmd.ExecuteNonQuery()

            End If

            If Val(txt_EmptyJumbo.Text) <> 0 Then

                Dlv_IdNo = 0
                Rec_IdNo = 0

                If Val(txt_EmptyJumbo.Text) < 0 Then
                    Rec_IdNo = Val(lbl_IdNo.Text)
                Else
                    Dlv_IdNo = Val(lbl_IdNo.Text)
                End If

                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, Empty_Jumbo ) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_IdNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_IdNo.Text))) & ", @OpeningDate, " & Str(Val(Dlv_IdNo)) & ", " & Str(Val(Rec_IdNo)) & ", '', '', '', 9001, " & Str(Math.Abs(Val(txt_EmptyJumbo.Text))) & " )"
                cmd.ExecuteNonQuery()

            End If


            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Then

                vENTDB_TotYRNWgt = 0
                Da = New SqlClient.SqlDataAdapter("select sum(a.Weight) from Stock_Yarn_Processing_Details a Where  a.sl_No < 5000 and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
                Da.SelectCommand.Transaction = tr
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                        vENTDB_TotYRNWgt = Val(Dt1.Rows(0)(0).ToString)
                    End If
                End If
                Dt1.Clear()

                If Val(vENTDB_TotYRNWgt) > Val(vTot_YrnWght) Then

                    cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno    , Count_IdNo, Yarn_Type, Mill_IdNo ) " &
                          " Select                                                   'YARN', Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_Idno, Count_IdNo, Yarn_Type, Mill_IdNo from Stock_Yarn_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()

                End If

                vENTDB_TotPAVUmtrs = 0
                Da = New SqlClient.SqlDataAdapter("select sum(a.Meters) from Stock_Pavu_Processing_Details a Where  a.sl_No < 5000 and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
                Da.SelectCommand.Transaction = tr
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                        vENTDB_TotPAVUmtrs = Val(Dt1.Rows(0)(0).ToString)
                    End If
                End If
                Dt1.Clear()

                If Val(vENTDB_TotPAVUmtrs) > Val(vTot_PvuMtrs) Then

                    cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno   , Ledger_Idno , EndsCount_IdNo ) " &
                            " Select                                             'PAVU'    , Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_Idno, EndsCount_IdNo from Stock_Pavu_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()

                End If


            End If


            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Stock_BabyCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Delivered_Bags = 0 and Delivered_Cones = 0 and Delivered_Weight = 0"
            'cmd.ExecuteNonQuery()

            With dgv_YarnDetails

                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(6).Value) <> 0 Then

                        Sno = Sno + 1

                        Cnt_ID = Common_Procedures.Count_NameToIdNo(con, Trim(.Rows(i).Cells(1).Value), tr)

                        Mil_ID = Common_Procedures.Mill_NameToIdNo(con, Trim(.Rows(i).Cells(3).Value), tr)

                        vSetCd = ""
                        vSetNo = ""
                        Selc_SetCode = ""
                        If Trim(UCase(.Rows(i).Cells(2).Value)) = "BABY" Then
                            vSetNo = Trim(.Rows(i).Cells(7).Value)
                            If Trim(vSetNo) <> "" Then
                                vSetCd = Trim(Val(lbl_Company.Tag)) & "-" & Trim(vSetNo) & "/" & Trim(OpYrCode)
                                Selc_SetCode = Trim(vSetNo) & "/" & Trim(OpYrCode) & "/" & Trim(Val(lbl_Company.Tag))
                            End If
                        End If

                        Dlv_IdNo = 0
                        Rec_IdNo = 0

                        If Trim(UCase(Led_Type)) = "JOBWORKER" Then
                            If Val(.Rows(i).Cells(6).Value) < 0 Then
                                Dlv_IdNo = Val(lbl_IdNo.Text)
                            Else
                                Rec_IdNo = Val(lbl_IdNo.Text)
                            End If
                        Else
                            If Val(.Rows(i).Cells(6).Value) < 0 Then
                                Rec_IdNo = Val(lbl_IdNo.Text)
                            Else
                                Dlv_IdNo = Val(lbl_IdNo.Text)
                            End If
                        End If

                        cmd.CommandText = "Insert into Stock_Yarn_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight, Particulars, Set_Code, Set_No ,LotCode_ForSelection,Weaving_JobCode_forSelection ,Sizing_JobCode_forSelection) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(Val(lbl_IdNo.Text)) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_IdNo.Text))) & ", @OpeningDate, " & Str(Val(Dlv_IdNo)) & ", " & Str(Val(Rec_IdNo)) & ", '', " & Str(Val(Sno)) & ", " & Str(Val(Cnt_ID)) & ", '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(Mil_ID)) & ", " & Str(Math.Abs(Val(.Rows(i).Cells(4).Value))) & ", " & Str(Math.Abs(Val(.Rows(i).Cells(5).Value))) & ", " & Str(Math.Abs(Val(.Rows(i).Cells(6).Value))) & ", '', '" & Trim(vSetCd) & "', '" & Trim(vSetNo) & "' ,'" & Trim(.Rows(i).Cells(8).Value) & "','" & Trim(.Rows(i).Cells(9).Value) & "','" & Trim(.Rows(i).Cells(10).Value) & "')"
                        cmd.ExecuteNonQuery()

                        'If Trim(UCase(dgv_YarnDetails.Rows(i).Cells(2).Value)) = "BABY" Then
                        '    Nr = 0
                        '    cmd.CommandText = "Update Stock_BabyCone_Processing_Details set " & _
                        '                " Baby_Bags = " & Str(Val(dgv_YarnDetails.Rows(i).Cells(4).Value)) & ", " & _
                        '                " Baby_Cones = " & Str(Val(dgv_YarnDetails.Rows(i).Cells(5).Value)) & ", " & _
                        '                " Baby_Weight = " & Str(Val(dgv_YarnDetails.Rows(i).Cells(6).Value)) & " " & _
                        '                " Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and " & _
                        '                " Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Set_Code = '" & Trim(vSetCd) & "'"

                        '    Nr = cmd.ExecuteNonQuery()

                        '    If Nr = 0 Then

                        '        cmd.CommandText = "Insert into Stock_BabyCone_Processing_Details( Reference_Code, " _
                        '                  & "Company_IdNo, Reference_No, For_OrderBy, Reference_Date, Ledger_IdNo, " _
                        '                  & "Set_Code, Set_No, setcode_forSelection, " _
                        '                  & "Ends_Name, Mill_Idno, Count_IdNo, Bag_No, Baby_Bags, " _
                        '                  & "Baby_Cones, Baby_Weight, Delivered_Bags, Delivered_Cones, Delivered_Weight) Values ( '" _
                        '                  & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(Val(lbl_IdNo.Text)) & "', " _
                        '                  & Str(Common_Procedures.OrderBy_CodeToValue(Trim(lbl_IdNo.Text))) & ", @OpeningDate, " _
                        '                  & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(vSetCd) & "', '" & Trim(vSetNo) & "', '" & Trim(Selc_SetCode) & "', '', " & Str(Mil_ID) & ", " & Str(Cnt_ID) & ", 1, " _
                        '                  & Str(Val(dgv_YarnDetails.Rows(i).Cells(4).Value)) & ", " & Str(Val(dgv_YarnDetails.Rows(i).Cells(5).Value)) & ", " _
                        '                  & Str(Val(dgv_YarnDetails.Rows(i).Cells(6).Value)) & ", 0, 0, 0)"

                        '        cmd.ExecuteNonQuery()

                        '    End If

                        'End If

                    End If

                Next i

            End With

            cmd.CommandText = "Delete from Stock_SizedPavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Pavu_Delivery_Code = '' and Pavu_Delivery_Increment = 0 and Beam_Knotting_Code = '' and Loom_Idno = 0 and Production_Meters = 0 and Close_Status = 0"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Sno = 0

            With dgv_PavuDetails

                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(2).Value) <> "" And Val(.Rows(i).Cells(5).Value) <> 0 Then
                        'If Trim(.Rows(i).Cells(1).Value) <> "" And Trim(.Rows(i).Cells(3).Value) <> "" And Val(.Rows(i).Cells(5).Value) <> 0 Then

                        Sno = Sno + 1

                        vSetCd = ""
                        Selc_SetCode = ""
                        vSetNo = Trim(.Rows(i).Cells(1).Value)
                        If Trim(vSetNo) <> "" Then
                            vSetCd = Trim(Val(lbl_Company.Tag)) & "-" & Trim(vSetNo) & "/" & Trim(OpYrCode)
                            Selc_SetCode = Trim(vSetNo) & "/" & Trim(OpYrCode) & "/" & Trim(Val(lbl_Company.Tag))
                        End If

                        pEdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, Trim(.Rows(i).Cells(2).Value), tr)
                        pCnt_ID = Val(Common_Procedures.get_FieldValue(con, "EndsCount_Head", "Count_IdNo", "(EndsCount_IdNo = " & Str(Val(pEdsCnt_ID)) & ")", , tr))
                        pEds_Nm = Val(Common_Procedures.get_FieldValue(con, "EndsCount_Head", "Ends_Name", "(EndsCount_IdNo = " & Str(Val(pEdsCnt_ID)) & ")", , tr))

                        Mtr_Pc = 0
                        If Val(.Rows(i).Cells(4).Value) Then
                            Mtr_Pc = Format(Val(.Rows(i).Cells(5).Value) / Val(.Rows(i).Cells(4).Value), "#########0.00")
                        End If

                        If Trim(.Rows(i).Cells(1).Value) <> "" And Trim(.Rows(i).Cells(3).Value) <> "" Then

                            Nr = 0
                            cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Reference_Date = @OpeningDate, Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)) & ", Ends_Name = '" & Trim(pEds_Nm) & "', Count_IdNo = " & Str(Val(pCnt_ID)) & ", EndsCount_IdNo = " & Str(Val(pEdsCnt_ID)) & ", Mill_IdNo = 0, Beam_Width_Idno = 0, Sizing_SlNo = 0, Sl_No = " & Str(Val(Sno)) & ", ForOrderBy_BeamNo = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(.Rows(i).Cells(3).Value))) & ", Gross_Weight = 0, Tare_Weight = 0, Net_Weight = 0, Noof_Pcs = " & Str(Val(.Rows(i).Cells(4).Value)) & ", Meters_Pc = " & Str(Val(Mtr_Pc)) & ", Meters = " & Str(Val(.Rows(i).Cells(5).Value)) & ", SizedBeam_Meters = " & Str(Val(.Rows(i).Cells(5).Value)) & ", Warp_Meters = 0  , Weaving_JobCode_forSelection  ='" & Trim(.Rows(i).Cells(7).Value) & "' , Sizing_JobCode_forSelection  ='" & Trim(.Rows(i).Cells(8).Value) & "'  " &
                                                " Where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Set_Code = '" & Trim(vSetCd) & "' and Beam_No = '" & Trim(dgv_PavuDetails.Rows(i).Cells(3).Value) & "'"
                            Nr = cmd.ExecuteNonQuery()

                            If Nr = 0 Then
                                cmd.CommandText = "Insert into Stock_SizedPavu_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, Ledger_IdNo, StockAt_IdNo, Set_Code, Set_No, setcode_forSelection, Ends_Name, count_idno, EndsCount_IdNo, Mill_IdNo, Beam_Width_Idno, Sizing_SlNo, Sl_No, Beam_No, ForOrderBy_BeamNo, Gross_Weight, Tare_Weight, Net_Weight, Noof_Pcs, Meters_Pc, Meters, SizedBeam_Meters, Warp_Meters, Pavu_Delivery_Code, Pavu_Delivery_Increment, DeliveryTo_Name, Loom_Idno, Beam_Knotting_Code ,Weaving_JobCode_forSelection ,Sizing_JobCode_forSelection)" &
                                                " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(Val(lbl_IdNo.Text)) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_IdNo.Text))) & ", @OpeningDate, " & Str(Val(lbl_IdNo.Text)) & ", " & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(vSetCd) & "', '" & Trim(vSetNo) & "', '" & Trim(Selc_SetCode) & "', '" & Trim(pEds_Nm) & "', " & Str(Val(pCnt_ID)) & ", " & Str(Val(pEdsCnt_ID)) & ", 0, 0, 0, " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(3).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(.Rows(i).Cells(3).Value))) & ", 0, 0, 0, " & Str(Val(.Rows(i).Cells(4).Value)) & ", 0, " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", 0, '', 0, '', 0, '',  '" & Trim(.Rows(i).Cells(7).Value) & "'  ,'" & Trim(.Rows(i).Cells(8).Value) & "' )"
                                cmd.ExecuteNonQuery()
                            End If

                        End If

                        Dlv_IdNo = 0
                        Rec_IdNo = 0

                        If Trim(UCase(Led_Type)) = "JOBWORKER" Then
                            If Val(.Rows(i).Cells(5).Value) < 0 Then
                                Dlv_IdNo = Val(lbl_IdNo.Text)
                            Else
                                Rec_IdNo = Val(lbl_IdNo.Text)
                            End If
                        Else
                            If Val(.Rows(i).Cells(5).Value) < 0 Then
                                Rec_IdNo = Val(lbl_IdNo.Text)
                            Else
                                Dlv_IdNo = Val(lbl_IdNo.Text)
                            End If
                        End If

                        cmd.CommandText = "Insert into Stock_Pavu_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Cloth_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters ,Weaving_JobCode_forSelection  ,Sizing_JobCode_forSelection) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_IdNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_IdNo.Text))) & ", @OpeningDate, " & Str(Val(Dlv_IdNo)) & ", " & Str(Val(Rec_IdNo)) & ", 0, 'OPENING', '', '', " & Str(Val(Sno)) & ", " & Str(Val(pEdsCnt_ID)) & ", 1, " & Str(Math.Abs(Val(.Rows(i).Cells(5).Value))) & " ,'" & Trim(.Rows(i).Cells(7).Value) & "'  ,'" & Trim(.Rows(i).Cells(8).Value) & "')"
                        cmd.ExecuteNonQuery()

                    End If

                Next i

            End With

            cmd.CommandText = "delete from voucher_bill_head where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ledger_idno = " & Str(Val(lbl_IdNo.Text)) & " and entry_identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and bill_amount = (credit_amount + debit_amount)"
            cmd.ExecuteNonQuery()

            Sno = 0

            With dgv_BillDetails

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(4).Value) <> 0 Then

                        cmd.Parameters.Clear()
                        cmd.Parameters.AddWithValue("@VouBillDate", CDate(.Rows(i).Cells(2).Value))

                        If Trim(UCase(.Rows(i).Cells(5).Value)) = "CR" Then CrDr_Amt_ColNm = "credit_amount" Else CrDr_Amt_ColNm = "debit_amount"

                        vAgt_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, Trim(.Rows(i).Cells(3).Value), tr)

                        Sno = Sno + 1

                        If Trim(.Rows(i).Cells(7).Value) <> "" And Val(.Rows(i).Cells(6).Value) <> 0 Then
                            Nr = 0
                            cmd.CommandText = "update voucher_bill_head set " _
                                                        & " voucher_bill_date = @VouBillDate, " _
                                                        & " party_bill_no = '" & Trim(.Rows(i).Cells(1).Value) & "', " _
                                                        & " agent_idno = " & Str(Val(vAgt_ID)) & ", " _
                                                        & " bill_amount = " & Str(Val(.Rows(i).Cells(4).Value)) & ", " _
                                                        & " crdr_type = '" & Trim(.Rows(i).Cells(5).Value) & "', " _
                                                        & " Software_Module_IdNo = " & Str(Val(Common_Procedures.SoftwareTypes.Textile_Software)) & ", " _
                                                        & " " & CrDr_Amt_ColNm & " = " & Str(Val(.Rows(i).Cells(4).Value)) & " " _
                                                        & " where " _
                                                        & " Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and " _
                                                        & " voucher_bill_code = '" & Trim(.Rows(i).Cells(7).Value) & "'"

                            Nr = cmd.ExecuteNonQuery()

                            If Nr = 0 Then
                                Throw New ApplicationException("Error On Bill Details")
                            End If


                        Else

                            vou_bil_no = Common_Procedures.get_MaxCode(con, "Voucher_Bill_Head", "Voucher_Bill_Code", "For_OrderBy", "", Val(lbl_Company.Tag), OpYrCode, tr)
                            vou_bil_code = Trim(Val(lbl_Company.Tag)) & "-" & Trim(vou_bil_no) & "/" & Trim(OpYrCode)

                            cmd.CommandText = "Insert into voucher_bill_head ( voucher_bill_code,             company_idno         ,        voucher_bill_no    ,            for_orderby      , voucher_bill_date,              ledger_idno        ,        party_bill_no                   ,            agent_idno    ,              bill_amount                 , " & Trim(CrDr_Amt_ColNm) & "             ,            crdr_type                   ,        entry_identification                  ,                          Software_Module_IdNo                ) " _
                                                    & "  Values ( '" & Trim(vou_bil_code) & "'  , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vou_bil_no) & "', " & Str(Val(vou_bil_no)) & ",     @VouBillDate , " & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(vAgt_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", '" & Trim(.Rows(i).Cells(5).Value) & "', '" & Trim(Pk_Condition) & Trim(NewCode) & "' , " & Str(Val(Common_Procedures.SoftwareTypes.Textile_Software)) & " )"
                            cmd.ExecuteNonQuery()

                        End If

                    End If

                Next i

            End With


            Led_Type = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)) & ")", , tr)

            StkOf_IdNo = 0
            If Trim(UCase(Led_Type)) = "JOBWORKER" Then
                StkOf_IdNo = Val(lbl_IdNo.Text)
            Else
                StkOf_IdNo = Val(Common_Procedures.CommonLedger.Godown_Ac)
            End If

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@OpeningDate", OpDate)

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_ClothDetails

                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(4).Value) <> 0 Then

                        Sno = Sno + 1

                        Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, Trim(.Rows(i).Cells(1).Value), tr)

                        CloTyp_ID = Common_Procedures.ClothType_NameToIdNo(con, Trim(.Rows(i).Cells(2).Value), tr)

                        Dlv_IdNo = 0
                        Rec_IdNo = 0

                        Dlv_IdNo = 0 : Rec_IdNo = 0
                        If Val(lbl_IdNo.Text) = Val(Common_Procedures.CommonLedger.Godown_Ac) Then
                            Dlv_IdNo = Val(Common_Procedures.CommonLedger.Godown_Ac)
                            Rec_IdNo = 0

                        Else
                            Dlv_IdNo = Val(Common_Procedures.CommonLedger.Godown_Ac)
                            Rec_IdNo = Val(lbl_IdNo.Text)

                        End If

                        'If Trim(UCase(Led_Type)) = "JOBWORKER" Then
                        '    If Val(.Rows(i).Cells(4).Value) < 0 Then
                        '        Dlv_IdNo = Val(lbl_IdNo.Text)
                        '    Else
                        '        Rec_IdNo = Val(lbl_IdNo.Text)
                        '    End If
                        'Else
                        '    If Val(.Rows(i).Cells(4).Value) < 0 Then
                        '        Rec_IdNo = Val(lbl_IdNo.Text)
                        '    Else
                        '        Dlv_IdNo = Val(lbl_IdNo.Text)
                        '    End If
                        'End If


                        cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code    ,             Company_IdNo         ,                Reference_No       ,                                                   for_OrderBy         , Reference_Date,         StockOff_IdNo     ,      DeliveryTo_Idno      ,       ReceivedFrom_Idno   , Entry_ID, Party_Bill_No, Particulars,           Sl_No      ,           Cloth_Idno    ,                      Folding             ,   Meters_Type" & Trim(Val(CloTyp_ID)) & " ,                    Pieces                 ,                          Weight               ) " &
                                                    " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(Val(lbl_IdNo.Text)) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_IdNo.Text))) & ", @OpeningDate, " & Str(Val(StkOf_IdNo)) & ", " & Str(Val(Dlv_IdNo)) & ", " & Str(Val(Rec_IdNo)) & ",     ''  ,      ''      ,     ''     , " & Str(Val(Sno)) & ", " & Str(Val(Clo_ID)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & " , " & Str(Val(.Rows(i).Cells(5).Value)) & " ,   " & Str(Val(.Rows(i).Cells(6).Value)) & "   ) "
                        cmd.ExecuteNonQuery()

                    End If

                Next i

            End With

            'cmd.CommandText = "Delete from Stock_BabyCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Delivered_Bags = 0 and Delivered_Cones = 0 and Delivered_Weight = 0"
            'cmd.ExecuteNonQuery()

            With dgv_BobinDetails

                Sno = 5000
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(5).Value) <> 0 Then

                        Sno = Sno + 1

                        BobinEdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, Trim(.Rows(i).Cells(1).Value), tr)

                        BobinClr_ID = Common_Procedures.Colour_NameToIdNo(con, Trim(.Rows(i).Cells(2).Value), tr)
                        Mill_ID = Common_Procedures.Mill_NameToIdNo(con, Trim(.Rows(i).Cells(6).Value), tr)
                        BnStk_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, Trim(.Rows(i).Cells(7).Value), tr)

                        'vSetCd = ""
                        'vSetNo = ""
                        'Selc_SetCode = ""
                        'If Trim(UCase(.Rows(i).Cells(2).Value)) = "BABY" Then
                        '    vSetNo = Trim(.Rows(i).Cells(7).Value)
                        '    If Trim(vSetNo) <> "" Then
                        '        vSetCd = Trim(Val(lbl_Company.Tag)) & "-" & Trim(vSetNo) & "/" & Trim(OpYrCode)
                        '        Selc_SetCode = Trim(vSetNo) & "/" & Trim(OpYrCode) & "/" & Trim(Val(lbl_Company.Tag))
                        '    End If
                        'End If

                        Dlv_IdNo = 0
                        Rec_IdNo = 0

                        If Trim(UCase(Led_Type)) = "JOBWORKER" Then
                            If Val(.Rows(i).Cells(5).Value) < 0 Then
                                Dlv_IdNo = Val(lbl_IdNo.Text)
                            Else
                                Rec_IdNo = Val(lbl_IdNo.Text)
                            End If
                        Else
                            If Val(.Rows(i).Cells(5).Value) < 0 Then
                                Rec_IdNo = Val(lbl_IdNo.Text)
                            Else
                                Dlv_IdNo = Val(lbl_IdNo.Text)
                            End If
                        End If

                        cmd.CommandText = "Insert into Stock_Pavu_Processing_Details(Reference_Code                               , Company_IdNo                    , Reference_No                      , for_OrderBy                                                           , Reference_Date,              DeliveryTo_Idno,         ReceivedFrom_Idno,             Party_Bill_No,                                   Sl_No,   EndsCount_IdNo                , Colour_IdNo                   ,  Bobins                                             , Meters_Bobin                                        ,                           Meters                   , StockOf_IdNo                ,  Mill_IdNo               , Particulars) " &
                                                                            "Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(Val(lbl_IdNo.Text)) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_IdNo.Text))) & ",    @OpeningDate, " & Str(Val(Dlv_IdNo)) & ", " & Str(Val(Rec_IdNo)) & ",                        '',                   " & Str(Val(Sno)) & ", " & Str(Val(BobinEdsCnt_ID)) & ", " & Str(Val(BobinClr_ID)) & ",   " & Str(Math.Abs(Val(.Rows(i).Cells(3).Value))) & ", " & Str(Math.Abs(Val(.Rows(i).Cells(4).Value))) & ", " & Str(Math.Abs(Val(.Rows(i).Cells(5).Value))) & ", " & Str(Val(BnStk_IdNo)) & ", " & Str(Val(Mill_ID)) & ",  '' )"
                        cmd.ExecuteNonQuery()


                    End If

                Next i

            End With


            With dgv_Jari_KuriDetails

                Sno = 5000
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(5).Value) <> 0 Then

                        Sno = Sno + 1

                        JariCnt_ID = Common_Procedures.Count_NameToIdNo(con, Trim(.Rows(i).Cells(1).Value), tr)

                        JariClr_ID = Common_Procedures.Colour_NameToIdNo(con, Trim(.Rows(i).Cells(2).Value), tr)

                        Dlv_IdNo = 0
                        Rec_IdNo = 0

                        If Trim(UCase(Led_Type)) = "JOBWORKER" Then
                            If Val(.Rows(i).Cells(5).Value) < 0 Then
                                Dlv_IdNo = Val(lbl_IdNo.Text)
                            Else
                                Rec_IdNo = Val(lbl_IdNo.Text)
                            End If

                        Else
                            If Val(.Rows(i).Cells(5).Value) < 0 Then
                                Rec_IdNo = Val(lbl_IdNo.Text)
                            Else
                                Dlv_IdNo = Val(lbl_IdNo.Text)
                            End If

                        End If

                        cmd.CommandText = "Insert into Stock_Yarn_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Sl_No, Count_IdNo, Colour_IdNo, Jumbo, Cones, Weight, Particulars) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(Val(lbl_IdNo.Text)) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_IdNo.Text))) & ", @OpeningDate, " & Str(Val(Dlv_IdNo)) & ", " & Str(Val(Rec_IdNo)) & ", '', " & Str(Val(Sno)) & ", " & Str(Val(JariCnt_ID)) & ", " & Val(JariClr_ID) & ",  " & Str(Math.Abs(Val(.Rows(i).Cells(3).Value))) & ", " & Str(Math.Abs(Val(.Rows(i).Cells(4).Value))) & ", " & Str(Math.Abs(Val(.Rows(i).Cells(5).Value))) & ", '')"
                        cmd.ExecuteNonQuery()

                    End If

                Next i

            End With


            cmd.CommandText = "Delete from Opening_Ledger_SalesValue_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Ledger_IdNo = " & Str(Val(lbl_IdNo.Text)) & " "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into Opening_Ledger_SalesValue_Head (               Company_IdNo       ,               Ledger_IdNo      ,              YearCode1          ,                 Sales_Value1                ,               YearCode2          ,                 Sales_Value2               ,                 Purchase_Value1              ,          Purchase_Value2                     ) " &
                                " Values                                  (" & Str(Val(lbl_Company.Tag)) & " , " & Str(Val(lbl_IdNo.Text)) & ", '" & Trim(lbl_YrCode1.Text) & "',  " & Str(Val(txt_Sales_Value_Yr1.Text)) & " , '" & Trim(lbl_YrCode2.Text) & "' , " & Str(Val(txt_Sales_Value_Yr2.Text)) & " ," & Str(Val(txt_Purchase_Value_Yr1.Text)) & " ," & Str(Val(txt_Purchase_Value_Yr2.Text)) & "  )"
            cmd.ExecuteNonQuery()


            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1267" Then '---- BRT SIZING (SOMANUR)

                If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Then

                    cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno      , Count_IdNo, Yarn_Type, Mill_IdNo ) " &
                                              " Select                               'YARN'    , Reference_Code, Reference_Date, Company_IdNo, ReceivedFrom_Idno, Count_IdNo, Yarn_Type, Mill_IdNo from Stock_Yarn_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno   , Ledger_Idno    , EndsCount_IdNo ) " &
                          " Select                               'PAVU'    , Reference_Code, Reference_Date, Company_IdNo, ReceivedFrom_Idno, EndsCount_IdNo from Stock_Pavu_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()

                    If Common_Procedures.Check_is_Negative_Stock_Status(con, tr) = True Then Exit Sub

                End If

            End If


            tr.Commit()

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)


            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_IdNo.Text)
                End If
            Else
                move_record(lbl_IdNo.Text)
            End If

        Catch ex As Exception
            tr.Rollback()
            If InStr(1, Trim(UCase(ex.Message)), "CK_STOCK_SIZEDPAVU_PROCESSING_DETAILS_1") > 1 Then
                MessageBox.Show("Beam Meters Lesser than Production Meters", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(UCase(ex.Message)), "CK_STOCK_SIZEDPAVU_PROCESSING_DETAILS_2") > 1 Then
                MessageBox.Show("Invalid Production Meters", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        End Try

        If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-----
    End Sub

    Private Sub cbo_CrDrType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_CrDrType.KeyDown
        Try
            With cbo_CrDrType
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True
                    txt_OpAmount.Focus()
                    'SendKeys.Send("+{TAB}")
                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    txt_EmptyBags.Focus()
                ElseIf e.KeyValue <> 13 And .DroppedDown = False Then
                    .DroppedDown = True
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_CrDrType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_CrDrType.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Condt As String
        Dim FindStr As String
        Dim indx As Integer

        Try

            With cbo_CrDrType

                If Asc(e.KeyChar) <> 27 Then

                    If Asc(e.KeyChar) = 13 Then

                        If Trim(.Text) <> "" Then
                            If .DroppedDown = True Then
                                If Trim(.SelectedText) <> "" Then
                                    .Text = .GetItemText(.SelectedItem)
                                    '.Text = .SelectedText
                                Else
                                    If .Items.Count > 0 Then
                                        .SelectedIndex = 0
                                        .SelectedItem = .Items(0)
                                        .Text = .GetItemText(.SelectedItem)
                                    End If
                                End If
                            End If
                        End If

                        txt_EmptyBags.Focus()

                    Else

                        Condt = ""
                        FindStr = ""

                        If Asc(e.KeyChar) = 8 Then
                            If .SelectionStart <= 1 Then
                                .Text = ""
                            End If

                            If Trim(.Text) <> "" Then
                                If .SelectionLength = 0 Then
                                    FindStr = .Text.Substring(0, .Text.Length - 1)
                                Else
                                    FindStr = .Text.Substring(0, .SelectionStart - 1)
                                End If
                            End If

                        Else
                            If .SelectionLength = 0 Then
                                FindStr = .Text & e.KeyChar
                            Else
                                FindStr = .Text.Substring(0, .SelectionStart) & e.KeyChar
                            End If

                        End If

                        indx = .FindString(FindStr)

                        If indx <> -1 Then
                            .SelectedText = ""
                            .SelectedIndex = indx

                            .SelectionStart = FindStr.Length
                            .SelectionLength = .Text.Length
                            e.Handled = True

                        Else
                            e.Handled = True

                        End If

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
        cbo_Ledger.Tag = cbo_Ledger.Text
    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")

        With cbo_Ledger
            If (e.KeyValue = 40 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If txt_OpAmount.Enabled And txt_OpAmount.Visible Then
                    txt_OpAmount.Focus()
                Else
                    txt_EmptyBeam.Focus()
                End If

            End If
        End With

    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Dim LedIdNo As Integer
        Dim BilType As String

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
            If Val(LedIdNo) <> 0 Then
                If Trim(UCase(cbo_Ledger.Tag)) <> Trim(UCase(cbo_Ledger.Text)) Then
                    cbo_Ledger.Tag = cbo_Ledger.Text
                    move_record(LedIdNo)
                End If
                cbo_Ledger.Enabled = False
            End If

            BilType = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Bill_Type", "(Ledger_IdNo = " & Str(Val(LedIdNo)) & ")")

            If Trim(UCase(BilType)) = "BILL TO BILL" Then
                txt_OpAmount.Enabled = False
                cbo_CrDrType.Enabled = False
                dgv_BillDetails.Enabled = True

            Else
                txt_OpAmount.Enabled = True
                cbo_CrDrType.Enabled = True
                dgv_BillDetails.Enabled = False

            End If

            If txt_OpAmount.Enabled And txt_OpAmount.Visible Then
                txt_OpAmount.Focus()

            Else
                txt_EmptyBeam.Focus()

            End If

        End If

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
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

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        save_record()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub cbo_Grid_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_head", "count_name", "(Cotton_Polyester_Jari <> 'POLYESTER' and Cotton_Polyester_Jari <> 'JARI')", "(count_idno = 0)")
    End Sub

    Private Sub cbo_Grid_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CountName.KeyDown

        Try

            vcbo_KeyDwnVal = e.KeyValue

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_CountName, Nothing, Nothing, "count_head", "count_name", "(Cotton_Polyester_Jari <> 'POLYESTER' and Cotton_Polyester_Jari <> 'JARI')", "(count_idno = 0)")

            With cbo_Grid_CountName
                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True

                    With dgv_YarnDetails
                        If Val(.CurrentCell.RowIndex) <= 0 Then
                            txt_EmptyJumbo.Focus()

                        Else
                            If .Columns(10).Visible Then
                                .Focus()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(10)
                                cbo_Grid_Yarn_Sizing_JobCardNo.Focus()
                                .CurrentCell.Selected = True

                            ElseIf .Columns(9).Visible Then
                                .Focus()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(9)
                                .CurrentCell.Selected = True
                                cbo_Grid_Yarn_weaving_job_no.Focus()

                            ElseIf .Columns(8).Visible Then
                                .Focus()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(8)
                                .CurrentCell.Selected = True
                                cbo_Grid_Yarn_LotNo.Focus()

                            Else
                                .Focus()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(7)
                                .CurrentCell.Selected = True

                            End If
                        End If
                    End With

                ElseIf e.KeyValue = 40 And .DroppedDown = False Then

                    e.Handled = True
                    With dgv_YarnDetails
                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                            tab_Main.SelectTab(1)
                            dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
                            dgv_PavuDetails.CurrentCell.Selected = True

                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                            .CurrentCell.Selected = True


                        End If
                    End With
                    .Visible = False
                    .Text = ""

                    'SendKeys.Send("{TAB}")
                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_CountName.KeyPress

        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_CountName, Nothing, "count_head", "count_name", "(Cotton_Polyester_Jari <> 'POLYESTER' and Cotton_Polyester_Jari <> 'JARI')", "(count_idno = 0)")

            If Asc(e.KeyChar) = 13 Then

                With dgv_YarnDetails
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_CountName.Text)
                    If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                        tab_Main.SelectTab(1)
                        dgv_PavuDetails.Focus()
                        dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
                        dgv_PavuDetails.CurrentCell.Selected = True

                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        .CurrentCell.Selected = True

                    End If
                End With

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Grid_CountName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CountName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_CountName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_MillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_MillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_MillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_MillName.KeyDown
        Try

            vcbo_KeyDwnVal = e.KeyValue
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_MillName, Nothing, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

            With cbo_Grid_MillName
                If (e.KeyValue = 38 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    e.Handled = True

                    With dgv_YarnDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                    End With

                ElseIf (e.KeyValue = 40 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                    e.Handled = True

                    With dgv_YarnDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    End With

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_MillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_MillName.KeyPress

        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_MillName, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

            If Asc(e.KeyChar) = 13 Then
                With dgv_YarnDetails
                    If .Visible = True Then
                        If .Rows.Count > 0 Then
                            .Focus()
                            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_MillName.Text)
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                            .CurrentCell.Selected = True
                        End If
                    End If
                End With
            End If

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub cbo_Grid_MillName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_MillName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_MillName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_YarnType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_YarnType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "YarnType_Head", "Yarn_Type", "", "(Yarn_Type = '')")
        With cbo_Grid_YarnType
            If Trim(.Text) = "" Then .Text = "MILL"
        End With
    End Sub

    Private Sub cbo_Grid_YarnType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_YarnType.KeyDown

        Try

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_YarnType, Nothing, Nothing, "YarnType_Head", "Yarn_Type", "", "(Yarn_Type = '')")

            With cbo_Grid_YarnType
                If (e.KeyValue = 38 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    e.Handled = True

                    With dgv_YarnDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                    End With

                ElseIf (e.KeyValue = 40 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                    e.Handled = True

                    With dgv_YarnDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    End With

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_YarnType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_YarnType.KeyPress
        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_YarnType, Nothing, "YarnType_Head", "Yarn_Type", "", "(Yarn_Type = '')")
            If Asc(e.KeyChar) = 13 Then
                With dgv_YarnDetails
                    If .Visible Then
                        If .Rows.Count > 0 Then
                            .Focus()
                            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_YarnType.Text)
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        End If
                    End If
                End With
            End If

        Catch ex As Exception
            '------

        End Try

    End Sub

    Private Sub dgv_YarnDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Dt5 As New DataTable
        Dim Dt6 As New DataTable
        Dim rect As Rectangle


        Try


            If ClrSTS = True = True Then Exit Sub

            With dgv_YarnDetails

                If Val(.CurrentRow.Cells(0).Value) = 0 Then
                    .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
                End If

                If .CurrentCell.ColumnIndex = 1 Then

                    If cbo_Grid_CountName.Visible = False Or Val(cbo_Grid_CountName.Tag) <> e.RowIndex Then

                        cbo_Grid_CountName.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
                        Dt1 = New DataTable
                        Da.Fill(Dt1)
                        cbo_Grid_CountName.DataSource = Dt1
                        cbo_Grid_CountName.DisplayMember = "Count_Name"

                        cbo_Grid_CountName.Left = .Left + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                        cbo_Grid_CountName.Top = .Top + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                        cbo_Grid_CountName.Width = .CurrentCell.Size.Width
                        cbo_Grid_CountName.Text = Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                        cbo_Grid_CountName.Tag = Val(.CurrentCell.RowIndex)
                        cbo_Grid_CountName.Visible = True

                        cbo_Grid_CountName.BringToFront()
                        cbo_Grid_CountName.Focus()
                    End If

                Else

                    cbo_Grid_CountName.Visible = False

                    cbo_Grid_CountName.Text = ""

                End If

                If .CurrentCell.ColumnIndex = 2 Then

                    If cbo_Grid_YarnType.Visible = False Or Val(cbo_Grid_YarnType.Tag) <> e.RowIndex Then

                        cbo_Grid_YarnType.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Yarn_Type from YarnType_Head order by Yarn_Type", con)
                        Dt2 = New DataTable
                        Da.Fill(Dt2)
                        cbo_Grid_YarnType.DataSource = Dt2
                        cbo_Grid_YarnType.DisplayMember = "Yarn_Type"

                        cbo_Grid_YarnType.Left = .Left + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                        cbo_Grid_YarnType.Top = .Top + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                        cbo_Grid_YarnType.Width = .CurrentCell.Size.Width
                        cbo_Grid_YarnType.Text = Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                        cbo_Grid_YarnType.Tag = Val(.CurrentCell.RowIndex)
                        cbo_Grid_YarnType.Visible = True

                        cbo_Grid_YarnType.BringToFront()
                        cbo_Grid_YarnType.Focus()

                    End If


                Else

                    cbo_Grid_YarnType.Visible = False

                    cbo_Grid_YarnType.Text = ""

                End If

                If .CurrentCell.ColumnIndex = 3 Then

                    If cbo_Grid_MillName.Visible = False Or Val(cbo_Grid_MillName.Tag) <> e.RowIndex Then

                        cbo_Grid_MillName.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Mill_Name from Mill_Head order by Mill_Name", con)
                        Dt3 = New DataTable
                        Da.Fill(Dt3)
                        cbo_Grid_MillName.DataSource = Dt3
                        cbo_Grid_MillName.DisplayMember = "Mill_Name"

                        cbo_Grid_MillName.Left = .Left + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                        cbo_Grid_MillName.Top = .Top + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                        cbo_Grid_MillName.Width = .CurrentCell.Size.Width
                        cbo_Grid_MillName.Text = Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                        cbo_Grid_MillName.Tag = Val(.CurrentCell.RowIndex)
                        cbo_Grid_MillName.Visible = True

                        cbo_Grid_MillName.BringToFront()
                        cbo_Grid_MillName.Focus()

                    End If

                Else

                    cbo_Grid_MillName.Visible = False

                    cbo_Grid_MillName.Text = ""

                End If

                If .CurrentCell.ColumnIndex = 8 Then

                    If cbo_Grid_Yarn_LotNo.Visible = False Or Val(cbo_Grid_Yarn_LotNo.Tag) <> e.RowIndex Then

                        cbo_Grid_Yarn_LotNo.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select LotCode_ForSelection from Yarn_Lot_Head order by LotCode_ForSelection", con)
                        Dt4 = New DataTable
                        Da.Fill(Dt4)
                        cbo_Grid_Yarn_LotNo.DataSource = Dt4
                        cbo_Grid_Yarn_LotNo.DisplayMember = "LotCode_ForSelection"

                        cbo_Grid_Yarn_LotNo.Left = .Left + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                        cbo_Grid_Yarn_LotNo.Top = .Top + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                        cbo_Grid_Yarn_LotNo.Width = .CurrentCell.Size.Width
                        cbo_Grid_Yarn_LotNo.Text = Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                        cbo_Grid_Yarn_LotNo.Tag = Val(.CurrentCell.RowIndex)
                        cbo_Grid_Yarn_LotNo.Visible = True

                        cbo_Grid_Yarn_LotNo.BringToFront()
                        cbo_Grid_Yarn_LotNo.Focus()

                    End If

                Else

                    cbo_Grid_Yarn_LotNo.Visible = False

                    cbo_Grid_Yarn_LotNo.Text = ""

                End If

                If .CurrentCell.ColumnIndex = 9 Then

                    If cbo_Grid_Yarn_weaving_job_no.Visible = False Or Val(cbo_Grid_Yarn_weaving_job_no.Tag) <> e.RowIndex Then

                        cbo_Grid_Yarn_weaving_job_no.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Weaving_JobCode_forSelection from Weaving_JobCard_Head order by Weaving_JobCode_forSelection", con)
                        Dt5 = New DataTable
                        Da.Fill(Dt5)
                        cbo_Grid_Yarn_weaving_job_no.DataSource = Dt5
                        cbo_Grid_Yarn_weaving_job_no.DisplayMember = "Weaving_JobCode_forSelection"

                        cbo_Grid_Yarn_weaving_job_no.Left = .Left + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                        cbo_Grid_Yarn_weaving_job_no.Top = .Top + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                        cbo_Grid_Yarn_weaving_job_no.Width = .CurrentCell.Size.Width
                        cbo_Grid_Yarn_weaving_job_no.Text = Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                        cbo_Grid_Yarn_weaving_job_no.Tag = Val(.CurrentCell.RowIndex)
                        cbo_Grid_Yarn_weaving_job_no.Visible = True

                        cbo_Grid_Yarn_weaving_job_no.BringToFront()
                        cbo_Grid_Yarn_weaving_job_no.Focus()

                    End If

                Else

                    cbo_Grid_Yarn_weaving_job_no.Visible = False

                    cbo_Grid_Yarn_weaving_job_no.Text = ""

                End If

                If dgv_YarnDetails.CurrentCell.ColumnIndex = 10 Then

                    If cbo_Grid_Yarn_Sizing_JobCardNo.Visible = False Or Val(cbo_Grid_Yarn_Sizing_JobCardNo.Tag) <> e.RowIndex Then

                        cbo_Grid_Yarn_Sizing_JobCardNo.Tag = -1

                        Da = New SqlClient.SqlDataAdapter("select Sizing_JobCode_forSelection from Sizing_JobCard_Head order by Sizing_JobCode_forSelection", con)
                        Dt6 = New DataTable
                        Da.Fill(Dt6)

                        cbo_Grid_Yarn_Sizing_JobCardNo.DataSource = Dt6
                        cbo_Grid_Yarn_Sizing_JobCardNo.DisplayMember = "Sizing_JobCode_forSelection"

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_Grid_Yarn_Sizing_JobCardNo.Left = .Left + rect.Left
                        cbo_Grid_Yarn_Sizing_JobCardNo.Top = .Top + rect.Top

                        cbo_Grid_Yarn_Sizing_JobCardNo.Width = rect.Width
                        cbo_Grid_Yarn_Sizing_JobCardNo.Height = rect.Height
                        cbo_Grid_Yarn_Sizing_JobCardNo.Text = .CurrentCell.Value

                        cbo_Grid_Yarn_Sizing_JobCardNo.Tag = Val(e.RowIndex)
                        cbo_Grid_Yarn_Sizing_JobCardNo.Visible = True

                        cbo_Grid_Yarn_Sizing_JobCardNo.BringToFront()
                        cbo_Grid_Yarn_Sizing_JobCardNo.Focus()




                    End If

                Else

                    cbo_Grid_Yarn_Sizing_JobCardNo.Visible = False

                    cbo_Grid_Yarn_Sizing_JobCardNo.Text = ""

                End If

            End With

        Catch ex As Exception
            '------

        End Try


    End Sub

    Private Sub dgv_YarnDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellLeave
        Try
            With dgv_YarnDetails
                If .CurrentCell.ColumnIndex = 6 Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                    End If
                End If
            End With

        Catch ex As Exception
            '------
        End Try

    End Sub

    Private Sub dgv_YarnDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_YarnDetails.CellValueChanged
        Try
            With dgv_YarnDetails
                If .Visible Then
                    If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub
                    If .Rows.Count > 0 Then
                        If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then
                            If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                                get_MillCount_Details()
                            End If
                            TotalYarn_Calculation()
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '--------

        End Try

    End Sub

    Private Sub dgv_YarnDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_YarnDetails.EditingControlShowing
        dgtxt_YarnDetails = CType(dgv_YarnDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_YarnDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_YarnDetails.KeyUp
        Dim i As Integer

        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With dgv_YarnDetails



                    If .CurrentRow.Index = 0 And .RowCount = 1 Then
                        For i = 1 To .Columns.Count - 1
                            .Rows(.CurrentRow.Index).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(.CurrentRow.Index)

                    End If

                    TotalYarn_Calculation()

                End With
            End If

        Catch ex As Exception
            '-----

        End Try

    End Sub

    Private Sub dgv_YarnDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_YarnDetails.LostFocus
        On Error Resume Next
        If IsNothing(dgv_YarnDetails.CurrentCell) Then Exit Sub
        dgv_YarnDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_YarnDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_YarnDetails.RowsAdded
        Dim n As Integer = 0

        Try
            With dgv_YarnDetails
                n = .RowCount
                .Rows(n - 1).Cells(0).Value = Val(n)
            End With

        Catch ex As Exception
            '-----
        End Try
    End Sub

    Private Sub TotalYarn_Calculation()
        Dim Sno As Integer
        Dim TotBags As Single, TotCones As Single, TotWeight As Single

        Sno = 0
        TotBags = 0
        TotCones = 0
        TotWeight = 0
        With dgv_YarnDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(6).Value) <> 0 Then
                    TotBags = TotBags + Val(.Rows(i).Cells(4).Value)
                    TotCones = TotCones + Val(.Rows(i).Cells(5).Value)
                    TotWeight = TotWeight + Val(.Rows(i).Cells(6).Value)
                End If
            Next
        End With

        With dgv_YarnDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Val(TotBags)
            .Rows(0).Cells(5).Value = Val(TotCones)
            .Rows(0).Cells(6).Value = Format(Val(TotWeight), "########0.000")
        End With

    End Sub

    Private Sub cbo_PavuGrid_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PavuGrid_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_head", "Endscount_name", "(Cotton_Polyester_Jari <> 'POLYESTER' and Cotton_Polyester_Jari <> 'JARI')", "(Endscount_idno = 0)")
    End Sub

    Private Sub cbo_PavuGrid_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PavuGrid_EndsCount.KeyDown
        Try
            With cbo_PavuGrid_EndsCount

                vcbo_KeyDwnVal = e.KeyValue

                Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PavuGrid_EndsCount, Nothing, Nothing, "Endscount_head", "Endscount_name", "(Cotton_Polyester_Jari <> 'POLYESTER' and Cotton_Polyester_Jari <> 'JARI')", "(Endscount_idno = 0)")

                If e.KeyValue = 38 And .DroppedDown = False Then
                    e.Handled = True

                    With dgv_PavuDetails
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                        .CurrentCell.Selected = True
                        .Focus()

                    End With

                    .Visible = False
                    .Text = ""

                    'SendKeys.Send("+{TAB}")

                ElseIf e.KeyValue = 40 And .DroppedDown = False Then
                    e.Handled = True
                    With dgv_PavuDetails
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        .CurrentCell.Selected = True
                        .Focus()
                    End With
                    .Visible = False
                    .Text = ""

                    'SendKeys.Send("{TAB}")

                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_PavuGrid_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PavuGrid_EndsCount.KeyPress

        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PavuGrid_EndsCount, Nothing, "Endscount_head", "Endscount_name", "(Cotton_Polyester_Jari <> 'POLYESTER' and Cotton_Polyester_Jari <> 'JARI')", "(Endscount_idno = 0)")

            If Asc(e.KeyChar) = 13 Then
                With dgv_PavuDetails
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_PavuGrid_EndsCount.Text)
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    .CurrentCell.Selected = True
                End With
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_PavuGrid_CountName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PavuGrid_EndsCount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New EndsCount_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PavuGrid_EndsCount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub dgv_PavuDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellEndEdit
        dgv_PavuDetails_CellLeave(sender, e)
    End Sub

    Private Sub dgv_PavuDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt5 As New DataTable
        Dim Dt6 As New DataTable

        Try

            If ClrSTS = True = True Then Exit Sub

            With dgv_PavuDetails
                If Val(.CurrentRow.Cells(0).Value) = 0 Then
                    .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
                End If
                If .CurrentCell.ColumnIndex = 2 And Val(.CurrentRow.Cells(6).Value) = 0 Then

                    If cbo_PavuGrid_EndsCount.Visible = False Or Val(cbo_PavuGrid_EndsCount.Tag) <> e.RowIndex Then

                        cbo_PavuGrid_EndsCount.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select EndsCount_Name from EndsCount_Head order by EndsCount_Name", con)
                        Dt1 = New DataTable
                        Da.Fill(Dt1)
                        cbo_PavuGrid_EndsCount.DataSource = Dt1
                        cbo_PavuGrid_EndsCount.DisplayMember = "EndsCount_Name"

                        cbo_PavuGrid_EndsCount.Left = .Left + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                        cbo_PavuGrid_EndsCount.Top = .Top + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                        cbo_PavuGrid_EndsCount.Width = .CurrentCell.Size.Width
                        cbo_PavuGrid_EndsCount.Text = Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                        cbo_PavuGrid_EndsCount.Tag = Val(.CurrentCell.RowIndex)
                        cbo_PavuGrid_EndsCount.Visible = True

                        cbo_PavuGrid_EndsCount.BringToFront()
                        cbo_PavuGrid_EndsCount.Focus()

                    End If


                Else

                    cbo_PavuGrid_EndsCount.Visible = False

                    cbo_PavuGrid_EndsCount.Text = ""

                End If


                If .CurrentCell.ColumnIndex = 7 Then

                    If cbo_Grid_Pavu_weaving_job_no.Visible = False Or Val(cbo_Grid_Pavu_weaving_job_no.Tag) <> e.RowIndex Then

                        cbo_Grid_Pavu_weaving_job_no.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Weaving_JobCode_forSelection from Weaving_JobCard_Head order by Weaving_JobCode_forSelection", con)
                        Dt5 = New DataTable
                        Da.Fill(Dt5)
                        cbo_Grid_Pavu_weaving_job_no.DataSource = Dt5
                        cbo_Grid_Pavu_weaving_job_no.DisplayMember = "Weaving_JobCode_forSelection"

                        cbo_Grid_Pavu_weaving_job_no.Left = .Left + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                        cbo_Grid_Pavu_weaving_job_no.Top = .Top + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                        cbo_Grid_Pavu_weaving_job_no.Width = .CurrentCell.Size.Width
                        cbo_Grid_Pavu_weaving_job_no.Text = Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                        cbo_Grid_Pavu_weaving_job_no.Tag = Val(.CurrentCell.RowIndex)
                        cbo_Grid_Pavu_weaving_job_no.Visible = True

                        cbo_Grid_Pavu_weaving_job_no.BringToFront()
                        cbo_Grid_Pavu_weaving_job_no.Focus()

                    End If

                Else

                    cbo_Grid_Pavu_weaving_job_no.Visible = False

                    cbo_Grid_Pavu_weaving_job_no.Text = ""

                End If

                If .CurrentCell.ColumnIndex = 8 Then

                    If cbo_grid_Pavu_Sizing_JobCardNo.Visible = False Or Val(cbo_grid_Pavu_Sizing_JobCardNo.Tag) <> e.RowIndex Then

                        cbo_grid_Pavu_Sizing_JobCardNo.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Sizing_JobCode_forSelection from Sizing_JobCard_Head order by Sizing_JobCode_forSelection", con)
                        Dt6 = New DataTable
                        Da.Fill(Dt6)
                        cbo_grid_Pavu_Sizing_JobCardNo.DataSource = Dt6
                        cbo_grid_Pavu_Sizing_JobCardNo.DisplayMember = "Sizing_JobCode_forSelection"

                        cbo_grid_Pavu_Sizing_JobCardNo.Left = .Left + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                        cbo_grid_Pavu_Sizing_JobCardNo.Top = .Top + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                        cbo_grid_Pavu_Sizing_JobCardNo.Width = .CurrentCell.Size.Width
                        cbo_grid_Pavu_Sizing_JobCardNo.Text = Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                        cbo_grid_Pavu_Sizing_JobCardNo.Tag = Val(.CurrentCell.RowIndex)
                        cbo_grid_Pavu_Sizing_JobCardNo.Visible = True

                        cbo_grid_Pavu_Sizing_JobCardNo.BringToFront()
                        cbo_grid_Pavu_Sizing_JobCardNo.Focus()

                    End If

                Else

                    cbo_grid_Pavu_Sizing_JobCardNo.Visible = False

                    cbo_grid_Pavu_Sizing_JobCardNo.Text = ""

                End If

            End With

        Catch ex As Exception
            '------

        End Try

    End Sub

    Private Sub dgv_PavuDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellLeave
        Try
            With dgv_PavuDetails
                If .Visible = True Then
                    If .Rows.Count > 0 Then
                        If .CurrentCell.ColumnIndex = 5 Then
                            If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                                If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then

                                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                                Else
                                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                                End If

                            End If
                        End If
                    End If
                End If

            End With
        Catch ex As Exception
            '-----
        End Try


    End Sub

    Private Sub dgv_PavuDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellValueChanged
        Try
            With dgv_PavuDetails
                If .Visible Then
                    If IsNothing(dgv_PavuDetails.CurrentCell) Then Exit Sub
                    If .Rows.Count > 0 Then
                        If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                            TotalPavu_Calculation()
                        End If
                    End If

                End If
            End With

        Catch ex As Exception
            '----

        End Try


    End Sub

    Private Sub dgv_PavuDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_PavuDetails.EditingControlShowing
        dgtxt_PavuDetails = CType(dgv_PavuDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_PavuDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_PavuDetails.KeyUp
        Dim i As Integer
        Dim n As Integer

        Try
            If e.Control = True And (UCase(Chr(e.KeyCode)) = "D" Or e.KeyCode = Keys.Delete) Then

                With dgv_PavuDetails

                    If Val(.CurrentRow.Cells(6).Value) = 0 Then
                        n = .CurrentRow.Index

                        If n = .Rows.Count - 1 Then
                            For i = 0 To .ColumnCount - 1
                                .Rows(n).Cells(i).Value = ""
                            Next

                        Else
                            .Rows.RemoveAt(n)

                        End If

                        TotalPavu_Calculation()
                    End If

                End With

            End If

        Catch ex As Exception
            '-----

        End Try

    End Sub

    Private Sub dgv_PavuDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_PavuDetails.LostFocus
        On Error Resume Next
        If IsNothing(dgv_PavuDetails.CurrentCell) Then Exit Sub
        dgv_PavuDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_PavuDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_PavuDetails.RowsAdded
        Dim n As Integer = 0

        Try
            With dgv_PavuDetails
                n = .RowCount
                .Rows(n - 1).Cells(0).Value = Val(n)
            End With
        Catch ex As Exception
            '-----
        End Try


    End Sub

    Private Sub TotalPavu_Calculation()
        Dim Sno As Integer
        Dim TotBms As Single, TotPcs As Single, TotMtrs As Single

        Sno = 0
        TotBms = 0
        TotPcs = 0
        TotMtrs = 0
        With dgv_PavuDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Trim(.Rows(i).Cells(1).Value) <> "" And Trim(.Rows(i).Cells(3).Value) <> "" And Val(.Rows(i).Cells(5).Value) <> 0 Then
                    TotBms = TotBms + 1
                    TotPcs = TotPcs + Val(.Rows(i).Cells(4).Value)
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(5).Value)
                End If
            Next
        End With

        With dgv_PavuDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(3).Value = Val(TotBms)
            .Rows(0).Cells(4).Value = Val(TotPcs)
            If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then

                .Rows(0).Cells(5).Value = Format(Val(TotMtrs), "########0.000")
            Else
                .Rows(0).Cells(5).Value = Format(Val(TotMtrs), "########0.00")
            End If

        End With

    End Sub

    Private Sub cbo_BillGrid_AgentName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BillGrid_AgentName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_BillGrid_AgentName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BillGrid_AgentName.KeyDown

        Try
            vcbo_KeyDwnVal = e.KeyValue

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BillGrid_AgentName, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")

            With cbo_BillGrid_AgentName
                If (e.KeyValue = 38 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    e.Handled = True

                    With dgv_BillDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                        .CurrentCell.Selected = True

                    End With

                    .Visible = False

                ElseIf (e.KeyValue = 40 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                    e.Handled = True
                    With dgv_BillDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        .CurrentCell.Selected = True
                    End With
                    .Visible = False

                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_BillGrid_AgentName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BillGrid_AgentName.KeyPress



        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BillGrid_AgentName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")

            With cbo_BillGrid_AgentName

                If Asc(e.KeyChar) = 13 Then

                    With dgv_BillDetails
                        .Focus()
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_BillGrid_AgentName.Text)
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        .CurrentCell.Selected = True
                    End With
                    .Visible = False

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_BillGrid_AgentName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BillGrid_AgentName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Agent_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_BillGrid_AgentName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_BillGrid_AgentName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BillGrid_AgentName.TextChanged
        Try
            If cbo_BillGrid_AgentName.Visible Then
                With dgv_BillDetails
                    If Val(cbo_BillGrid_AgentName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_BillGrid_AgentName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_BillGrid_CrDr_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BillGrid_CrDr.KeyDown

        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BillGrid_CrDr, Nothing, Nothing, "", "", "", "")

            With cbo_BillGrid_CrDr
                If (e.KeyValue = 38 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    e.Handled = True

                    With dgv_BillDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                        .CurrentCell.Selected = True
                    End With

                    .Visible = False

                ElseIf (e.KeyValue = 40 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                    e.Handled = True
                    With dgv_BillDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        .CurrentCell.Selected = True
                    End With
                    .Visible = False

                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_BillGrid_CrDr_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BillGrid_CrDr.KeyPress

        Try

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BillGrid_CrDr, Nothing, "", "", "", "")

            With cbo_BillGrid_CrDr

                If Asc(e.KeyChar) = 13 Then

                    With dgv_BillDetails
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_BillGrid_CrDr.Text)
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        .CurrentCell.Selected = True
                    End With
                    .Visible = False


                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_BillGrid_CrDr_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BillGrid_CrDr.TextChanged
        Try
            If FrmLdSTS = True Then Exit Sub
            If IsNothing(dgv_BillDetails.CurrentCell) Then Exit Sub
            If cbo_BillGrid_CrDr.Visible Then
                With dgv_BillDetails
                    If Val(cbo_BillGrid_CrDr.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 5 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_BillGrid_CrDr.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_BillDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BillDetails.CellEndEdit
        dgv_BillDetails_CellLeave(sender, e)
    End Sub

    Private Sub dgv_BillDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BillDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        Try

            If ClrSTS = True = True Then Exit Sub

            With dgv_BillDetails
                If Val(.CurrentRow.Cells(0).Value) = 0 Then
                    .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
                End If
                If .CurrentCell.ColumnIndex = 3 Then

                    If cbo_BillGrid_AgentName.Visible = False Or Val(cbo_BillGrid_AgentName.Tag) <> e.RowIndex Then

                        Da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'AGENT') order by Ledger_DisplayName", con)
                        Dt1 = New DataTable
                        Da.Fill(Dt1)
                        cbo_BillGrid_AgentName.DataSource = Dt1
                        cbo_BillGrid_AgentName.DisplayMember = "Ledger_DisplayName"

                        cbo_BillGrid_AgentName.Left = .Left + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                        cbo_BillGrid_AgentName.Top = .Top + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                        cbo_BillGrid_AgentName.Width = .CurrentCell.Size.Width
                        cbo_BillGrid_AgentName.Text = Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                        cbo_BillGrid_AgentName.Tag = Val(.CurrentCell.RowIndex)
                        cbo_BillGrid_AgentName.Visible = True

                        cbo_BillGrid_AgentName.BringToFront()
                        cbo_BillGrid_AgentName.Focus()

                    End If


                Else

                    cbo_BillGrid_AgentName.Visible = False


                End If

                If .CurrentCell.ColumnIndex = 5 Then

                    If cbo_BillGrid_CrDr.Visible = False Or Val(cbo_BillGrid_CrDr.Tag) <> e.RowIndex Then

                        cbo_BillGrid_CrDr.Left = .Left + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                        cbo_BillGrid_CrDr.Top = .Top + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                        cbo_BillGrid_CrDr.Width = .CurrentCell.Size.Width
                        cbo_BillGrid_CrDr.Text = Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                        cbo_BillGrid_CrDr.Tag = Val(.CurrentCell.RowIndex)
                        cbo_BillGrid_CrDr.Visible = True

                        cbo_BillGrid_CrDr.BringToFront()
                        cbo_BillGrid_CrDr.Focus()

                    End If

                Else

                    cbo_BillGrid_CrDr.Visible = False


                End If

            End With

        Catch ex As Exception
            '-----

        End Try

    End Sub

    Private Sub dgv_BillDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BillDetails.CellLeave
        Try
            With dgv_BillDetails
                If .Visible = True Then
                    If .Rows.Count > 0 Then
                        If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 6 Then
                            If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                                .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                            Else
                                .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                            End If
                        End If
                    End If
                End If

            End With

        Catch ex As Exception
            '-----
        End Try
    End Sub

    Private Sub dgv_BillDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BillDetails.CellValueChanged
        Try
            With dgv_BillDetails
                If .Visible Then
                    If IsNothing(dgv_BillDetails.CurrentCell) Then Exit Sub
                    If .Rows.Count > 0 Then
                        If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                            Total_BillAmount_Calculation()
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub dgv_BillDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_BillDetails.EditingControlShowing
        dgtxt_BillDetails = CType(dgv_BillDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_BillDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_BillDetails.KeyUp
        Dim i As Integer
        Dim n As Integer

        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With dgv_BillDetails

                    n = .CurrentRow.Index

                    If n = .Rows.Count - 1 Then
                        For i = 0 To .ColumnCount - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(n)

                    End If

                    Total_BillAmount_Calculation()

                End With

            End If

        Catch ex As Exception
            '-----

        End Try


    End Sub

    Private Sub dgv_BillDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_BillDetails.LostFocus
        On Error Resume Next
        If IsNothing(dgv_BillDetails.CurrentCell) Then Exit Sub
        dgv_BillDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_BillDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_BillDetails.RowsAdded
        Dim n As Integer = 0

        Try
            With dgv_BillDetails
                n = .RowCount
                .Rows(n - 1).Cells(0).Value = Val(n)
            End With

        Catch ex As Exception
            '-----

        End Try

    End Sub

    Private Sub Total_BillAmount_Calculation()
        Dim Sno As Integer
        Dim TotBlCrAmt As String
        Dim TotBlDrAmt As String
        Dim TotPydRcvdAmt As String



        Sno = 0
        TotBlCrAmt = 0
        TotBlDrAmt = 0
        TotPydRcvdAmt = 0

        With dgv_BillDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(4).Value) <> 0 Then
                    If Trim(UCase(.Rows(i).Cells(5).Value)) = "CR" Then
                        TotBlCrAmt = Format(Val(TotBlCrAmt) + Math.Abs(Val(.Rows(i).Cells(4).Value)), "############0.00")
                    Else
                        TotBlDrAmt = Format(Val(TotBlDrAmt) + Math.Abs(Val(.Rows(i).Cells(4).Value)), "############0.00")
                    End If
                    TotPydRcvdAmt = Format(Val(TotPydRcvdAmt) + Val(.Rows(i).Cells(6).Value), "############0.00")
                End If
            Next
        End With

        With dgv_BillDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Format(Math.Abs(Val(TotBlCrAmt) - Val(TotBlDrAmt)), "############0.00")
            If Val(TotBlCrAmt) > Val(TotBlDrAmt) Then
                .Rows(0).Cells(5).Value = "Cr"
            Else
                .Rows(0).Cells(5).Value = "Dr"
            End If
        End With

        If txt_OpAmount.Enabled = False Then
            txt_OpAmount.Text = Trim(Format(Val(dgv_BillDetails_Total.Rows(0).Cells(4).Value), "#############0.00"))
            cbo_CrDrType.Text = dgv_BillDetails_Total.Rows(0).Cells(5).Value
        End If

    End Sub

    Private Sub cbo_PavuGrid_CountName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PavuGrid_EndsCount.TextChanged
        Try
            If cbo_PavuGrid_EndsCount.Visible Then
                With dgv_PavuDetails
                    If Val(cbo_PavuGrid_EndsCount.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_PavuGrid_EndsCount.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_EmptyBeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EmptyBeam.KeyPress
        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_EmptyBags_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EmptyBags.KeyPress
        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub dgtxt_YarnDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_YarnDetails.Enter
        Try
            dgv_YarnDetails.EditingControl.BackColor = Color.Lime
            dgv_YarnDetails.EditingControl.ForeColor = Color.Blue
            dgtxt_YarnDetails.SelectAll()
        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub dgtxt_PavuDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_PavuDetails.Enter
        Try
            dgv_PavuDetails.EditingControl.BackColor = Color.Lime
            dgv_PavuDetails.EditingControl.ForeColor = Color.Blue
            dgtxt_PavuDetails.SelectAll()

        Catch ex As Exception
            '------
        End Try

    End Sub

    Private Sub dgtxt_BillDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_BillDetails.Enter
        Try
            dgv_BillDetails.EditingControl.BackColor = Color.Lime
            dgv_BillDetails.EditingControl.ForeColor = Color.Blue
            dgtxt_BillDetails.SelectAll()
        Catch ex As Exception
            '-----
        End Try
    End Sub

    Private Sub dgtxt_clothDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_ClothDetails.Enter
        Try
            dgv_ClothDetails.EditingControl.BackColor = Color.Lime
            dgv_ClothDetails.EditingControl.ForeColor = Color.Blue
            dgtxt_ClothDetails.SelectAll()
        Catch ex As Exception
            '-----
        End Try
    End Sub

    Private Sub dgtxt_YarnDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_YarnDetails.KeyPress
        Try
            If dgv_YarnDetails.Visible Then
                If dgv_YarnDetails.Rows.Count > 0 Then
                    If dgv_YarnDetails.CurrentCell.ColumnIndex = 4 Or dgv_YarnDetails.CurrentCell.ColumnIndex = 5 Or dgv_YarnDetails.CurrentCell.ColumnIndex = 6 Then
                        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            '---
        End Try
    End Sub

    Private Sub dgtxt_YarnDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_YarnDetails.KeyUp
        dgv_YarnDetails_KeyUp(sender, e)
    End Sub

    Private Sub dgtxt_PavuDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_PavuDetails.KeyDown
        Try
            With dgv_PavuDetails
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then
                            If Val(.CurrentRow.Cells(6).Value) = 1 Then
                                e.Handled = True
                                e.SuppressKeyPress = True
                            End If
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '----

        End Try
    End Sub

    Private Sub dgtxt_PavuDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_PavuDetails.KeyPress
        Try
            With dgv_PavuDetails
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then
                            If Val(.CurrentRow.Cells(6).Value) = 1 Then
                                e.Handled = True
                            End If
                        End If
                        If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                            If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then
                                e.Handled = True
                            End If
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '----

        End Try
    End Sub


    Private Sub dgtxt_PavuDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_PavuDetails.KeyUp
        dgv_PavuDetails_KeyUp(sender, e)
    End Sub

    Private Sub dgtxt_BillDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_BillDetails.KeyPress
        Try
            With dgv_BillDetails
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If .CurrentCell.ColumnIndex = 4 Then
                            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                                e.Handled = True
                            End If
                        End If
                    End If
                End If

            End With

        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub dgtxt_BillDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_BillDetails.KeyUp
        dgv_BillDetails_KeyUp(sender, e)
    End Sub

    Private Sub dgtxt_ClothDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_ClothDetails.KeyPress
        If dgv_ClothDetails.CurrentCell.ColumnIndex = 3 Or dgv_ClothDetails.CurrentCell.ColumnIndex = 4 Then
            If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub dgtxt_ClothDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_ClothDetails.KeyUp
        dgv_ClothDetails_KeyUp(sender, e)
    End Sub


    Private Sub cbo_Grid_CountName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.TextChanged
        Try
            If cbo_Grid_CountName.Visible Then
                With dgv_YarnDetails
                    If Val(cbo_Grid_CountName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_CountName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_MillName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_MillName.TextChanged
        Try
            If cbo_Grid_MillName.Visible Then
                With dgv_YarnDetails
                    If Val(cbo_Grid_CountName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_MillName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_YarnType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_YarnType.TextChanged
        Try
            If cbo_Grid_YarnType.Visible Then
                With dgv_YarnDetails
                    If Val(cbo_Grid_YarnType.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_YarnType.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub tab_Main_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tab_Main.SelectedIndexChanged
        If tab_Main.SelectedIndex = 0 Then
            dgv_YarnDetails.Focus()
            dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
            If cbo_Grid_CountName.Enabled And cbo_Grid_CountName.Visible Then
                cbo_Grid_CountName.Focus()
            End If

        ElseIf tab_Main.SelectedIndex = 1 Then
            dgv_PavuDetails.Focus()
            dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
            dgv_PavuDetails.CurrentCell.Selected = True

        ElseIf tab_Main.SelectedIndex = 2 Then
            dgv_BillDetails.Focus()
            dgv_BillDetails.CurrentCell = dgv_BillDetails.Rows(0).Cells(1)
            dgv_BillDetails.CurrentCell.Selected = True
        ElseIf tab_Main.SelectedIndex = 3 Then
            dgv_ClothDetails.Focus()
            dgv_ClothDetails.CurrentCell = dgv_ClothDetails.Rows(0).Cells(1)
            dgv_ClothDetails.CurrentCell.Selected = True

        ElseIf tab_Main.SelectedIndex = 4 Then
            dgv_BobinDetails.Focus()
            dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)
            dgv_BobinDetails.CurrentCell.Selected = True

        End If
    End Sub

    Private Sub cbo_Grid_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ClothName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "cloth_head", "cloth_name", "", "(cloth_idno = 0)")
    End Sub

    Private Sub cbo_Grid_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothName.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_ClothName, Nothing, Nothing, "cloth_head", "cloth_name", "", "(cloth_idno = 0)")

        Try

            With cbo_Grid_ClothName
                If (e.KeyValue = 38 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    e.Handled = True

                    With dgv_ClothDetails
                        If Val(.CurrentCell.RowIndex) <= 0 Then
                            If dgv_BillDetails.Enabled Then
                                tab_Main.SelectTab(2)
                                dgv_BillDetails.CurrentCell = dgv_BillDetails.Rows(0).Cells(1)
                                dgv_BillDetails.CurrentCell.Selected = True

                            Else
                                tab_Main.SelectTab(1)
                                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
                                dgv_PavuDetails.CurrentCell.Selected = True

                            End If




                        Else
                            .Focus()


                            If dgv_ClothDetails.Columns(6).Visible Then
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

                            ElseIf dgv_ClothDetails.Columns(5).Visible Then
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 2)

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 3)
                            End If


                            .CurrentCell.Selected = True

                        End If
                    End With


                ElseIf (e.KeyValue = 40 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                    e.Handled = True
                    With dgv_ClothDetails
                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                            tab_Main.SelectTab(4)
                            dgv_EmptyBeamDetails.Focus()
                            dgv_EmptyBeamDetails.CurrentCell = dgv_EmptyBeamDetails.Rows(0).Cells(1)
                            dgv_EmptyBeamDetails.CurrentCell.Selected = True



                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                            .CurrentCell.Selected = True

                        End If
                    End With
                    .Visible = False
                    .Text = ""

                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Grid_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_ClothName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_ClothName, Nothing, "cloth_head", "cloth_name", "", "(cloth_idno = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_ClothDetails
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_ClothName.Text)
                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                    tab_Main.SelectTab(4)
                    dgv_EmptyBeamDetails.Focus()
                    dgv_EmptyBeamDetails.CurrentCell = dgv_EmptyBeamDetails.Rows(0).Cells(1)
                    dgv_EmptyBeamDetails.CurrentCell.Selected = True



                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End With

        End If

    End Sub

    Private Sub cbo_Grid_ClothName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_ClothName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_ClothName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ClothName.TextChanged
        Try
            If cbo_Grid_ClothName.Visible Then
                With dgv_ClothDetails
                    If Val(cbo_Grid_ClothName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_ClothName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_ClothTypeName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ClothTypeName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothType_Head", "ClothType_Name", "(ClothType_IdNo >= 1 and ClothType_IdNo <= 5)", "(ClothType_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_ClothTypeName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothTypeName.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_ClothTypeName, Nothing, Nothing, "ClothType_Head", "ClothType_Name", "(ClothType_IdNo >= 1 and ClothType_IdNo <= 5)", "(ClothType_IdNo = 0)")

        Try

            With cbo_Grid_ClothTypeName
                If (e.KeyValue = 38 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    e.Handled = True

                    With dgv_ClothDetails

                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                        .CurrentCell.Selected = True

                    End With


                ElseIf (e.KeyValue = 40 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                    e.Handled = True
                    With dgv_ClothDetails
                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" And Trim(.CurrentRow.Cells(2).Value) = "" Then
                            btnSave.Focus()

                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                            .CurrentCell.Selected = True


                        End If
                    End With
                    .Visible = False
                    .Text = ""

                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Grid_ClothTypeName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_ClothTypeName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_ClothTypeName, Nothing, "ClothType_Head", "ClothType_Name", "(ClothType_IdNo >= 1 and ClothType_IdNo <= 5)", "(ClothType_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_ClothDetails
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_ClothTypeName.Text)
                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" And Trim(.CurrentRow.Cells(2).Value) = "" Then

                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                        save_record()

                    Else
                        tab_Main.SelectTab(0)
                        cbo_Ledger.Focus()

                    End If

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    .CurrentCell.Selected = True

                End If

            End With

        End If

    End Sub

    Private Sub cbo_Grid_ClothTypeName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ClothTypeName.TextChanged
        Try
            If cbo_Grid_ClothTypeName.Visible Then
                With dgv_ClothDetails
                    If Val(cbo_Grid_ClothTypeName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_ClothTypeName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_ClothDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_ClothDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable

        If ClrSTS = True = True Then Exit Sub

        With dgv_ClothDetails

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If .CurrentCell.ColumnIndex = 1 Then

                If cbo_Grid_ClothName.Visible = False Or Val(cbo_Grid_ClothName.Tag) <> e.RowIndex Then

                    cbo_Grid_ClothName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_ClothName.DataSource = Dt1
                    cbo_Grid_ClothName.DisplayMember = "Cloth_Name"

                    cbo_Grid_ClothName.Left = .Left + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_ClothName.Top = .Top + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_ClothName.Width = .CurrentCell.Size.Width
                    cbo_Grid_ClothName.Text = Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_ClothName.Tag = Val(.CurrentCell.RowIndex)
                    cbo_Grid_ClothName.Visible = True

                    cbo_Grid_ClothName.BringToFront()
                    cbo_Grid_ClothName.Focus()
                End If

            Else

                cbo_Grid_ClothName.Visible = False

                cbo_Grid_ClothName.Text = ""

            End If

            If .CurrentCell.ColumnIndex = 2 Then

                If cbo_Grid_ClothTypeName.Visible = False Or Val(cbo_Grid_ClothTypeName.Tag) <> e.RowIndex Then

                    cbo_Grid_ClothTypeName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select ClothType_Name from ClothType_Head order by ClothType_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_Grid_ClothTypeName.DataSource = Dt2
                    cbo_Grid_ClothTypeName.DisplayMember = "ClothType_Name"

                    cbo_Grid_ClothTypeName.Left = .Left + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_ClothTypeName.Top = .Top + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_ClothTypeName.Width = .CurrentCell.Size.Width
                    cbo_Grid_ClothTypeName.Text = Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_ClothTypeName.Tag = Val(.CurrentCell.RowIndex)
                    cbo_Grid_ClothTypeName.Visible = True

                    cbo_Grid_ClothTypeName.BringToFront()
                    cbo_Grid_ClothTypeName.Focus()

                End If

            Else

                cbo_Grid_ClothTypeName.Visible = False

                cbo_Grid_ClothTypeName.Text = ""

            End If

        End With

    End Sub

    Private Sub dgv_ClothDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_ClothDetails.CellLeave
        With dgv_ClothDetails
            If .CurrentCell.ColumnIndex = 4 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_ClothDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_ClothDetails.CellValueChanged
        On Error Resume Next
        With dgv_ClothDetails
            If .Visible Then
                If IsNothing(dgv_ClothDetails.CurrentCell) Then Exit Sub
                If .CurrentCell.ColumnIndex = 4 Then
                    TotalCloth_Calculation()
                End If
            End If
        End With
    End Sub

    Private Sub dgv_ClothDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_ClothDetails.EditingControlShowing
        dgtxt_ClothDetails = CType(dgv_ClothDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_ClothDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_ClothDetails.KeyUp
        Dim i As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_ClothDetails

                If .CurrentRow.Index = .RowCount - 1 Then
                    For i = 1 To .Columns.Count - 1
                        .Rows(.CurrentRow.Index).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(.CurrentRow.Index)

                End If

                TotalCloth_Calculation()

            End With

        End If


    End Sub

    Private Sub dgv_ClothDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_ClothDetails.LostFocus
        On Error Resume Next
        If IsNothing(dgv_ClothDetails.CurrentCell) Then Exit Sub
        dgv_ClothDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_ClothDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_ClothDetails.RowsAdded
        Dim n As Integer

        With dgv_ClothDetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With

    End Sub

    Private Sub TotalCloth_Calculation()
        Dim Sno As Integer
        Dim TotMtrs As Single

        Sno = 0
        TotMtrs = 0
        With dgv_YarnDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(4).Value) <> 0 Then
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(4).Value)
                End If
            Next
        End With

        With dgv_YarnDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Format(Val(TotMtrs), "########0.00")
        End With

    End Sub

    Private Sub txt_EmptyJumbo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_EmptyJumbo.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            tab_Main.SelectTab(0)
            dgv_YarnDetails.Focus()
            dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
            dgv_YarnDetails.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub txt_EmptyJumbo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EmptyJumbo.KeyPress
        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            tab_Main.SelectTab(0)
            dgv_YarnDetails.Focus()
            dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
            dgv_YarnDetails.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub txt_EmptyBobin_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EmptyBobin.KeyPress
        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_EmptyCones_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EmptyCones.KeyPress
        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_EmptyBobinParty_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_EmptyBobinParty.KeyPress
        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub get_MillCount_Details()
        Dim q As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Cn_bag As Single
        Dim Wgt_Bag As Single
        Dim Wgt_Cn As Single
        Dim CntID As Integer
        Dim MilID As Integer

        CntID = Common_Procedures.Count_NameToIdNo(con, dgv_YarnDetails.Rows(dgv_YarnDetails.CurrentRow.Index).Cells(1).Value)
        MilID = Common_Procedures.Mill_NameToIdNo(con, dgv_YarnDetails.Rows(dgv_YarnDetails.CurrentRow.Index).Cells(3).Value)

        Wgt_Bag = 0 : Wgt_Cn = 0 : Cn_bag = 0

        If CntID <> 0 And MilID <> 0 Then

            Da = New SqlClient.SqlDataAdapter("select * from Mill_Count_Details where mill_idno = " & Str(Val(MilID)) & " and count_idno = " & Str(Val(CntID)), con)
            Da.Fill(Dt)
            With dgv_YarnDetails

                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        Wgt_Bag = Dt.Rows(0).Item("Weight_Bag").ToString
                        Wgt_Cn = Dt.Rows(0).Item("Weight_Cone").ToString
                        Cn_bag = Dt.Rows(0).Item("Cones_Bag").ToString
                    End If
                End If

                Dt.Clear()
                Dt.Dispose()
                Da.Dispose()

                If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                    If .CurrentCell.ColumnIndex = 4 Then
                        If Val(Cn_bag) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(5).Value = .Rows(.CurrentRow.Index).Cells(4).Value * Val(Cn_bag)
                        End If

                        If Val(Wgt_Bag) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(6).Value = Format(Val(.Rows(.CurrentRow.Index).Cells(4).Value) * Val(Wgt_Bag), "#########0.000")
                        End If

                    End If

                    If .CurrentCell.ColumnIndex = 5 Then
                        If Val(Wgt_Cn) <> 0 Then
                            .Rows(.CurrentRow.Index).Cells(6).Value = Format(.Rows(.CurrentRow.Index).Cells(5).Value * Val(Wgt_Cn), "##########0.000")
                        End If

                    End If

                End If

            End With

        End If

    End Sub

    Private Sub dgv_BobinDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BobinDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable

        If ClrSTS = True = True Then Exit Sub

        With dgv_BobinDetails

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If .CurrentCell.ColumnIndex = 1 Then

                If cbo_BobinGrid_EndsCount.Visible = False Or Val(cbo_BobinGrid_EndsCount.Tag) <> e.RowIndex Then

                    cbo_BobinGrid_EndsCount.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select EndsCount_Name from EndsCount_Head order by EndsCount_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_BobinGrid_EndsCount.DataSource = Dt1
                    cbo_BobinGrid_EndsCount.DisplayMember = "EndsCount_Name"

                    cbo_BobinGrid_EndsCount.Left = .Left + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_BobinGrid_EndsCount.Top = .Top + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_BobinGrid_EndsCount.Width = .CurrentCell.Size.Width
                    cbo_BobinGrid_EndsCount.Text = Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_BobinGrid_EndsCount.Tag = Val(.CurrentCell.RowIndex)
                    cbo_BobinGrid_EndsCount.Visible = True

                    cbo_BobinGrid_EndsCount.BringToFront()
                    cbo_BobinGrid_EndsCount.Focus()
                End If

            Else

                cbo_BobinGrid_EndsCount.Visible = False

                cbo_BobinGrid_EndsCount.Text = ""

            End If

            If .CurrentCell.ColumnIndex = 2 Then

                If cbo_BobinGrid_Colour.Visible = False Or Val(cbo_BobinGrid_Colour.Tag) <> e.RowIndex Then

                    cbo_BobinGrid_Colour.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_BobinGrid_Colour.DataSource = Dt2
                    cbo_BobinGrid_Colour.DisplayMember = "Colour_Name"

                    cbo_BobinGrid_Colour.Left = .Left + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_BobinGrid_Colour.Top = .Top + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_BobinGrid_Colour.Width = .CurrentCell.Size.Width
                    cbo_BobinGrid_Colour.Text = Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_BobinGrid_Colour.Tag = Val(.CurrentCell.RowIndex)
                    cbo_BobinGrid_Colour.Visible = True

                    cbo_BobinGrid_Colour.BringToFront()
                    cbo_BobinGrid_Colour.Focus()

                End If


            Else

                cbo_BobinGrid_Colour.Visible = False

                cbo_BobinGrid_Colour.Text = ""

            End If


            If .CurrentCell.ColumnIndex = 6 Then

                If cbo_Grid_BobinMillName.Visible = False Or Val(cbo_Grid_BobinMillName.Tag) <> e.RowIndex Then

                    cbo_Grid_BobinMillName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Mill_Name from Mill_Head order by Mill_Name", con)
                    Dt3 = New DataTable
                    Da.Fill(Dt4)
                    cbo_Grid_BobinMillName.DataSource = Dt4
                    cbo_Grid_BobinMillName.DisplayMember = "Mill_Name"

                    cbo_Grid_BobinMillName.Left = .Left + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_BobinMillName.Top = .Top + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_BobinMillName.Width = .CurrentCell.Size.Width
                    cbo_Grid_BobinMillName.Text = Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_BobinMillName.Tag = Val(.CurrentCell.RowIndex)
                    cbo_Grid_BobinMillName.Visible = True


                    cbo_Grid_BobinMillName.BringToFront()
                    cbo_Grid_BobinMillName.Focus()

                Else

                    cbo_Grid_BobinMillName.Visible = False

                    cbo_Grid_BobinMillName.Text = ""

                End If

            End If

            If e.ColumnIndex = 7 Then

                If cbo_BobinGrid_PartName.Visible = False Or Val(cbo_BobinGrid_PartName.Tag) <> e.RowIndex Then

                    cbo_BobinGrid_PartName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead order by Ledger_DisplayName", con)
                    Dt3 = New DataTable
                    Da.Fill(Dt3)
                    cbo_BobinGrid_PartName.DataSource = Dt3
                    cbo_BobinGrid_PartName.DisplayMember = "Ledger_DisplayName"

                    cbo_BobinGrid_PartName.Left = .Left + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_BobinGrid_PartName.Top = .Top + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_BobinGrid_PartName.Width = .CurrentCell.Size.Width
                    cbo_BobinGrid_PartName.Text = Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_BobinGrid_PartName.Tag = Val(.CurrentCell.RowIndex)
                    cbo_BobinGrid_PartName.Visible = True


                    cbo_BobinGrid_PartName.BringToFront()
                    cbo_BobinGrid_PartName.Focus()



                End If

            Else

                cbo_BobinGrid_PartName.Visible = False
                cbo_BobinGrid_PartName.Text = ""
            End If

        End With

    End Sub

    Private Sub dgv_BobinDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BobinDetails.CellLeave
        With dgv_BobinDetails
            'Or .CurrentCell.ColumnIndex = 6
            If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
        End With
    End Sub


    Private Sub dgv_BobinDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BobinDetails.CellValueChanged
        'On Error Resume Next
        'With dgv_YarnDetails
        '    If .Visible Then
        '        If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then
        '            If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
        '                get_MillCount_Details()
        '            End If
        '            TotalYarn_Calculation()
        '        End If
        '    End If
        'End With

    End Sub

    Private Sub dgv_BobinDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_BobinDetails.EditingControlShowing
        dgtxt_BobinDetails = CType(dgv_BobinDetails.EditingControl, DataGridViewTextBoxEditingControl)
        'dgv_YarnDetails.CurrentCell.Style.BackColor = Color.Lime
        'dgv_YarnDetails.CurrentCell.Style.ForeColor = Color.White
    End Sub

    Private Sub dgv_BobinDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_BobinDetails.KeyUp
        Dim i As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_BobinDetails



                If .CurrentRow.Index = 0 And .RowCount = 1 Then
                    For i = 1 To .Columns.Count - 1
                        .Rows(.CurrentRow.Index).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(.CurrentRow.Index)

                End If

                ' TotalYarn_Calculation()

            End With
        End If


    End Sub

    Private Sub dgv_BobinDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_BobinDetails.LostFocus
        On Error Resume Next
        If IsNothing(dgv_BobinDetails.CurrentCell) Then Exit Sub
        dgv_BobinDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_BobinDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_BobinDetails.RowsAdded
        Dim n As Integer

        With dgv_BobinDetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub dgtxt_BobinDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_BobinDetails.Enter
        Try
            dgv_BobinDetails.EditingControl.BackColor = Color.Lime
            dgv_BobinDetails.EditingControl.ForeColor = Color.Blue
            dgtxt_BobinDetails.SelectAll()

        Catch ex As Exception
            '--
        End Try
    End Sub

    Private Sub dgtxt_BobinDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_BobinDetails.KeyPress
        Try
            If dgv_BobinDetails.Visible Then
                If dgv_BobinDetails.Rows.Count > 0 Then
                    If dgv_BobinDetails.CurrentCell.ColumnIndex = 3 Or dgv_BobinDetails.CurrentCell.ColumnIndex = 4 Or dgv_BobinDetails.CurrentCell.ColumnIndex = 5 Then
                        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            '---
        End Try
    End Sub

    Private Sub dgtxt_bobinDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_BobinDetails.KeyUp
        Try
            dgv_BobinDetails_KeyUp(sender, e)
        Catch ex As Exception
            '-----
        End Try
    End Sub

    Private Sub dgv_Jari_KuriDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Jari_KuriDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable

        If ClrSTS = True = True Then Exit Sub

        With dgv_Jari_KuriDetails

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If .CurrentCell.ColumnIndex = 1 Then

                If cbo_Grid_JariCount.Visible = False Or Val(cbo_Grid_JariCount.Tag) <> e.RowIndex Then

                    cbo_Grid_JariCount.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_JariCount.DataSource = Dt1
                    cbo_Grid_JariCount.DisplayMember = "Count_Name"

                    cbo_Grid_JariCount.Left = .Left + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_JariCount.Top = .Top + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_JariCount.Width = .CurrentCell.Size.Width
                    cbo_Grid_JariCount.Text = Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_JariCount.Tag = Val(.CurrentCell.RowIndex)
                    cbo_Grid_JariCount.Visible = True

                    cbo_Grid_JariCount.BringToFront()
                    cbo_Grid_JariCount.Focus()
                End If

            Else

                cbo_Grid_JariCount.Visible = False

                cbo_Grid_JariCount.Text = ""

            End If

            If .CurrentCell.ColumnIndex = 2 Then

                If cbo_Grid_JariColour.Visible = False Or Val(cbo_Grid_JariColour.Tag) <> e.RowIndex Then

                    cbo_Grid_JariColour.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_Grid_JariColour.DataSource = Dt2
                    cbo_Grid_JariColour.DisplayMember = "Colour_Name"

                    cbo_Grid_JariColour.Left = .Left + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_JariColour.Top = .Top + .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_JariColour.Width = .CurrentCell.Size.Width
                    cbo_Grid_JariColour.Text = Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_JariColour.Tag = Val(.CurrentCell.RowIndex)
                    cbo_Grid_JariColour.Visible = True

                    cbo_Grid_JariColour.BringToFront()
                    cbo_Grid_JariColour.Focus()

                End If


            Else

                cbo_Grid_JariColour.Visible = False

                cbo_Grid_JariColour.Text = ""

            End If


        End With

    End Sub

    Private Sub dgv_Jari_KuriDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Jari_KuriDetails.CellLeave
        With dgv_Jari_KuriDetails
            If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
        End With
    End Sub


    Private Sub dgv_Jari_KuriDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Jari_KuriDetails.CellValueChanged
        'On Error Resume Next
        'With dgv_YarnDetails
        '    If .Visible Then
        '        If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then
        '            If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
        '                get_MillCount_Details()
        '            End If
        '            TotalYarn_Calculation()
        '        End If
        '    End If
        'End With

    End Sub

    Private Sub dgv_Jari_KuriDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Jari_KuriDetails.EditingControlShowing
        dgtxt_JariKuriDetails = CType(dgv_Jari_KuriDetails.EditingControl, DataGridViewTextBoxEditingControl)
        'dgv_YarnDetails.CurrentCell.Style.BackColor = Color.Lime
        'dgv_YarnDetails.CurrentCell.Style.ForeColor = Color.White
    End Sub

    Private Sub dgv_Jari_KuriDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Jari_KuriDetails.KeyUp
        Dim i As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Jari_KuriDetails



                If .CurrentRow.Index = 0 And .RowCount = 1 Then
                    For i = 1 To .Columns.Count - 1
                        .Rows(.CurrentRow.Index).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(.CurrentRow.Index)

                End If

                ' TotalYarn_Calculation()

            End With
        End If


    End Sub

    Private Sub dgv_Jari_KuriDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Jari_KuriDetails.LostFocus
        On Error Resume Next
        If IsNothing(dgv_Jari_KuriDetails.CurrentCell) Then Exit Sub
        dgv_Jari_KuriDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Jari_KuriDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Jari_KuriDetails.RowsAdded
        Dim n As Integer

        With dgv_Jari_KuriDetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub dgtxt_JariKuriDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_JariKuriDetails.Enter
        dgv_Jari_KuriDetails.EditingControl.BackColor = Color.Lime
        dgv_Jari_KuriDetails.EditingControl.ForeColor = Color.Blue
        dgtxt_JariKuriDetails.SelectAll()
    End Sub

    Private Sub dgtxt_JariKuriDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_JariKuriDetails.KeyPress
        Try
            If dgv_Jari_KuriDetails.Visible Then
                If dgv_Jari_KuriDetails.Rows.Count > 0 Then
                    If dgv_Jari_KuriDetails.CurrentCell.ColumnIndex = 3 Or dgv_Jari_KuriDetails.CurrentCell.ColumnIndex = 4 Or dgv_Jari_KuriDetails.CurrentCell.ColumnIndex = 5 Then
                        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            '---
        End Try
    End Sub

    Private Sub dgtxt_jarKuriDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_JariKuriDetails.KeyUp
        dgv_Jari_KuriDetails_KeyUp(sender, e)
    End Sub

    Private Sub cbo_Grid_JariCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_JariCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_head", "count_name", "(Cotton_Polyester_Jari <> 'COTTON' and Cotton_Polyester_Jari <> '')", "(count_idno = 0)")
    End Sub

    Private Sub cbo_Grid_JariCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_JariCount.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_JariCount, Nothing, Nothing, "count_head", "count_name", "(Cotton_Polyester_Jari <> 'COTTON' and Cotton_Polyester_Jari <> '')", "(count_idno = 0)")

        Try

            With cbo_Grid_JariCount
                If (e.KeyValue = 38 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    e.Handled = True

                    With dgv_Jari_KuriDetails
                        If Val(.CurrentCell.RowIndex) <= 0 Then
                            If dgv_BobinDetails.Enabled Then
                                tab_Main.SelectTab(4)
                                dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)
                                dgv_BobinDetails.CurrentCell.Selected = True

                            Else
                                tab_Main.SelectTab(3)
                                dgv_ClothDetails.CurrentCell = dgv_ClothDetails.Rows(0).Cells(1)
                                dgv_ClothDetails.CurrentCell.Selected = True



                            End If

                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)
                            .CurrentCell.Selected = True

                        End If
                    End With


                ElseIf (e.KeyValue = 40 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                    e.Handled = True
                    With dgv_Jari_KuriDetails
                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                            btnSave.Focus()

                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                            .CurrentCell.Selected = True

                        End If
                    End With
                    .Visible = False
                    .Text = ""

                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Grid_JariCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_JariCount.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_JariCount, Nothing, "Count_head", "count_name", "(Cotton_Polyester_Jari <> 'COTTON' and Cotton_Polyester_Jari <> '')", "(count_idno = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Jari_KuriDetails
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_JariCount.Text)
                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                        save_record()

                    Else
                        tab_Main.SelectTab(0)
                        cbo_Ledger.Focus()

                    End If

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End With

        End If

    End Sub

    Private Sub cbo_Grid_JariCount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_JariCount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_JariCount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_JariCount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_JariCount.TextChanged
        Try
            If cbo_Grid_JariCount.Visible Then
                With dgv_Jari_KuriDetails
                    If Val(cbo_Grid_JariCount.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_JariCount.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_BobinGrid_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BobinGrid_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_head", "Endscount_name", "(Cotton_Polyester_Jari <> 'COTTON' and Cotton_Polyester_Jari <> '')", "(Endscount_idno = 0)")
    End Sub


    Private Sub cbo_Bobin_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinGrid_EndsCount.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BobinGrid_EndsCount, Nothing, Nothing, "Endscount_head", "Endscount_name", "(Cotton_Polyester_Jari <> 'COTTON' and Cotton_Polyester_Jari <> '')", "(Endscount_idno = 0)")

        Try

            With cbo_BobinGrid_EndsCount
                If (e.KeyValue = 38 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    e.Handled = True

                    With dgv_BobinDetails
                        If Val(.CurrentCell.RowIndex) <= 0 Then
                            ' If dgv_ClothDetails.Enabled Then
                            tab_Main.SelectTab(4)
                            dgv_EmptyBeamDetails.Focus()
                            dgv_EmptyBeamDetails.CurrentCell = dgv_EmptyBeamDetails.Rows(0).Cells(1)
                            ' dgv_EmptyBeamDetails.CurrentCell.Selected = True

                            'Else
                            '    tab_Main.SelectTab(2)
                            '    dgv_BillDetails.CurrentCell = dgv_BillDetails.Rows(0).Cells(1)
                            '    dgv_BillDetails.CurrentCell.Selected = True



                            'End If

                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)
                            .CurrentCell.Selected = True

                        End If
                    End With


                ElseIf (e.KeyValue = 40 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                    e.Handled = True
                    With dgv_BobinDetails
                        If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

                            tab_Main.SelectTab(6)
                            dgv_Jari_KuriDetails.Focus()
                            dgv_Jari_KuriDetails.CurrentCell = dgv_Jari_KuriDetails.Rows(0).Cells(1)
                            dgv_Jari_KuriDetails.CurrentCell.Selected = True

                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                            .CurrentCell.Selected = True

                        End If

                    End With
                    .Visible = False
                    .Text = ""

                ElseIf e.KeyValue <> 13 And e.KeyValue <> 17 And e.KeyValue <> 27 And .DroppedDown = False Then
                    .DroppedDown = True

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_BobinGrid_EndsCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BobinGrid_EndsCount.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BobinGrid_EndsCount, Nothing, "EndsCount_head", "Endscount_name", "(Cotton_Polyester_Jari <> 'COTTON' and Cotton_Polyester_Jari <> '')", "(Endscount_idno = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_BobinDetails
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_BobinGrid_EndsCount.Text)
                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

                    tab_Main.SelectTab(6)
                    dgv_Jari_KuriDetails.Focus()
                    dgv_Jari_KuriDetails.CurrentCell = dgv_Jari_KuriDetails.Rows(0).Cells(1)
                    dgv_Jari_KuriDetails.CurrentCell.Selected = True



                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End With

        End If

    End Sub

    Private Sub cbo_BobinGrid_EndsCount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinGrid_EndsCount.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New EndsCount_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_BobinGrid_EndsCount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_BobinGrid_EndsCount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BobinGrid_EndsCount.TextChanged
        Try
            If cbo_BobinGrid_EndsCount.Visible Then
                With dgv_BobinDetails
                    If Val(cbo_BobinGrid_EndsCount.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_BobinGrid_EndsCount.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_JariColour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_JariColour.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_JariColour_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_JariColour.TextChanged
        Try
            If cbo_Grid_JariColour.Visible Then
                With dgv_Jari_KuriDetails
                    If .Rows.Count > 0 Then
                        If Val(cbo_Grid_JariColour.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_JariColour.Text)
                        End If
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_BobinGrid_Colour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinGrid_Colour.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BobinGrid_Colour, Nothing, Nothing, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")
        Try
            With cbo_BobinGrid_Colour
                If (e.KeyValue = 38 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    e.Handled = True

                    With dgv_BobinDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                    End With

                ElseIf (e.KeyValue = 40 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                    e.Handled = True

                    With dgv_BobinDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    End With

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_BobinGrid_Colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BobinGrid_Colour.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BobinGrid_Colour, Nothing, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            With dgv_BobinDetails
                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_BobinGrid_Colour.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End With
        End If

    End Sub

    Private Sub cbo_BobinGrid_Colour_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinGrid_Colour.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Color_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_BobinGrid_Colour.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_JariColour_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_JariColour.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_JariColour, Nothing, Nothing, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")
        Try
            With cbo_Grid_JariColour
                If (e.KeyValue = 38 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    e.Handled = True

                    With dgv_Jari_KuriDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                    End With

                ElseIf (e.KeyValue = 40 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                    e.Handled = True

                    With dgv_Jari_KuriDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    End With

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_JariColour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_JariColour.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_JariColour, Nothing, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            With dgv_Jari_KuriDetails
                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_JariColour.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                .CurrentCell.Selected = True
            End With
        End If

    End Sub

    Private Sub cbo_Grid_JariColour_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_JariColour.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Color_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_JariColour.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_BobinGrid_PartName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BobinGrid_PartName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1) ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_BobinGrid_PartName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinGrid_PartName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_BobinGrid_PartName, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1) ", "(Ledger_IdNo = 0)")


        With dgv_BobinDetails


            If (e.KeyValue = 38 And cbo_BobinGrid_PartName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_BobinGrid_PartName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

            End If

        End With

    End Sub

    Private Sub cbo_BobinGrid_PartName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_BobinGrid_PartName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_BobinGrid_PartName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or Show_In_All_Entry = 1) ", "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then


            With dgv_BobinDetails

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)


            End With

        End If

    End Sub

    Private Sub cbo_BobinGrid_PartName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_BobinGrid_PartName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New RackNo_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_BobinGrid_PartName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub



    Private Sub cbo_BobinGrid_PartName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_BobinGrid_PartName.TextChanged
        Try
            If cbo_BobinGrid_PartName.Visible Then
                With dgv_BobinDetails
                    If Val(cbo_BobinGrid_PartName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 7 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_BobinGrid_PartName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub TotalEmptyBeam_Calculation()
        Dim vTotetybm As Single
        Dim i As Integer
        Dim sno As Integer

        vTotetybm = 0
        With dgv_EmptyBeamDetails
            For i = 0 To .Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(0).Value = sno

                If Val(.Rows(i).Cells(1).Value) <> 0 Then

                    vTotetybm = vTotetybm + Val(.Rows(i).Cells(1).Value)


                End If
            Next
        End With

        If dgv_Details_Total.Rows.Count <= 0 Then dgv_Details_Total.Rows.Add()

        dgv_Details_Total.Rows(0).Cells(1).Value = Val(vTotetybm)

    End Sub
    Private Sub dgv_EmptyBeamDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_EmptyBeamDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Rect As Rectangle

        With dgv_EmptyBeamDetails

            'dgv_ActCtrlName = .Name.ToString

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 2 Then

                If cbo_Grid_VendorName.Visible = False Or Val(cbo_Grid_VendorName.Tag) <> e.RowIndex Then

                    'dgv_ActCtrlName = dgv_Details.Name

                    cbo_Grid_VendorName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Vendor_DisplayName from Vendor_AlaisHead Order by Vendor_DisplayName", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_VendorName.DataSource = Dt1
                    cbo_Grid_VendorName.DisplayMember = "Vendor_DisplayName"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_VendorName.Left = .Left + Rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_VendorName.Top = .Top + Rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_VendorName.Width = Rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_VendorName.Height = Rect.Height  ' rect.Height

                    cbo_Grid_VendorName.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_VendorName.Tag = Val(e.RowIndex)
                    cbo_Grid_VendorName.Visible = True

                    cbo_Grid_VendorName.BringToFront()
                    cbo_Grid_VendorName.Focus()

                End If

            Else

                cbo_Grid_VendorName.Visible = False

            End If

            If e.ColumnIndex = 3 Then

                If cbo_Grid_beamwidth.Visible = False Or Val(cbo_Grid_beamwidth.Tag) <> e.RowIndex Then

                    'dgv_ActCtrlName = dgv_Details.Name

                    cbo_Grid_beamwidth.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Beam_Width_Name from Beam_Width_Head Order by Beam_Width_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_Grid_beamwidth.DataSource = Dt2
                    cbo_Grid_beamwidth.DisplayMember = "Beam_Width_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_beamwidth.Left = .Left + Rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Grid_beamwidth.Top = .Top + Rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Grid_beamwidth.Width = Rect.Width  ' .CurrentCell.Size.Width
                    cbo_Grid_beamwidth.Height = Rect.Height  ' rect.Height

                    cbo_Grid_beamwidth.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Grid_beamwidth.Tag = Val(e.RowIndex)
                    cbo_Grid_beamwidth.Visible = True

                    cbo_Grid_beamwidth.BringToFront()
                    cbo_Grid_beamwidth.Focus()

                End If

            Else

                cbo_Grid_beamwidth.Visible = False

            End If

        End With

    End Sub

    Private Sub dgv_EmptyBeamDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_EmptyBeamDetails.CellValueChanged

        Try
            With dgv_EmptyBeamDetails
                If .Visible Then
                    If IsNothing(dgv_EmptyBeamDetails.CurrentCell) Then Exit Sub
                    If .CurrentCell.ColumnIndex = 1 Then

                        TotalEmptyBeam_Calculation()

                    End If

                End If
            End With


        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub dgv_EmptyBeamDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_EmptyBeamDetails.EditingControlShowing
        dgtxt_EmptyBeamDetails = CType(dgv_EmptyBeamDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_EmptyBeamDetails.Enter
        'dgv_ActCtrlName = dgv_Details.Name
        dgv_EmptyBeamDetails.EditingControl.BackColor = Color.Lime
        dgv_EmptyBeamDetails.EditingControl.ForeColor = Color.Blue
        dgtxt_EmptyBeamDetails.SelectAll()
    End Sub

    Private Sub dgtxt_details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_EmptyBeamDetails.KeyPress

        With dgv_EmptyBeamDetails

            If Val(dgv_EmptyBeamDetails.CurrentCell.ColumnIndex.ToString) = 1 Then

                If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then
                    e.Handled = True
                End If

            End If

        End With

    End Sub

    Private Sub dgv_EmptyBeamDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_EmptyBeamDetails.KeyUp
        Dim n As Integer = 0

        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With dgv_EmptyBeamDetails

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

                TotalEmptyBeam_Calculation()

            End If

        Catch ex As Exception
            '---

        End Try

    End Sub

    Private Sub dgv_EmptyBeamDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_EmptyBeamDetails.RowsAdded
        Dim n As Integer = 0

        Try
            With dgv_EmptyBeamDetails
                n = .RowCount
                .Rows(n - 1).Cells(0).Value = Val(n)
            End With

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub dgv_EmptyBeamDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_EmptyBeamDetails.LostFocus
        On Error Resume Next
        If IsNothing(dgv_EmptyBeamDetails.CurrentCell) Then Exit Sub
        dgv_EmptyBeamDetails.CurrentCell.Selected = False
    End Sub
    Private Sub cbo_Grid_VendorName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_VendorName.TextChanged
        Try
            If cbo_Grid_VendorName.Visible Then
                With dgv_EmptyBeamDetails
                    If Val(cbo_Grid_VendorName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_VendorName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_VendorName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_VendorName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Vendor_AlaisHead", "Vendor_DisplayName", "", "(Vendor_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_VendorName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_VendorName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_VendorName, Nothing, Nothing, "Vendor_AlaisHead", "Vendor_DisplayName", "", "(Vendor_IdNo = 0)")
        With dgv_EmptyBeamDetails

            If (e.KeyValue = 38 And cbo_Grid_VendorName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_VendorName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_Grid_VendorName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_VendorName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_VendorName, Nothing, "Vendor_AlaisHead", "Vendor_DisplayName", "", "(Vendor_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_EmptyBeamDetails
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End With

        End If
    End Sub


    Private Sub cbo_Grid_VendorName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_VendorName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Vendor_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_VendorName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub



    Private Sub cbo_Grid_beamwidth_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_beamwidth.TextChanged
        Try
            If cbo_Grid_beamwidth.Visible Then
                With dgv_EmptyBeamDetails
                    If Val(cbo_Grid_beamwidth.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_beamwidth.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_beamwidth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_beamwidth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Beam_Width_Head", "Beam_Width_Name", "", "(Beam_Width_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_beamwidth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_beamwidth.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_beamwidth, Nothing, Nothing, "Beam_Width_head", "Beam_Width_name", "", "(Beam_Width_IdNo = 0)")
        With dgv_EmptyBeamDetails

            If (e.KeyValue = 38 And cbo_Grid_beamwidth.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_beamwidth.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If .CurrentRow.Index = .Rows.Count - 1 Then

                    tab_Main.SelectTab(5)
                    dgv_BobinDetails.Focus()
                    dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)
                    dgv_BobinDetails.CurrentCell.Selected = True

                Else
                    .Focus()
                    dgv_EmptyBeamDetails.CurrentCell = dgv_EmptyBeamDetails.Rows(dgv_EmptyBeamDetails.CurrentRow.Index + 1).Cells(1)

                End If


            End If

        End With
    End Sub

    Private Sub cbo_Grid_beamwidth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_beamwidth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_beamwidth, Nothing, "Beam_Width_head", "Beam_Width_name", "", "(Beam_Width_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_EmptyBeamDetails
                If .CurrentRow.Index = .Rows.Count - 1 Then

                    tab_Main.SelectTab(5)
                    dgv_BobinDetails.Focus()
                    dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)
                    dgv_BobinDetails.CurrentCell.Selected = True


                Else
                    .Focus()
                    dgv_EmptyBeamDetails.CurrentCell = dgv_EmptyBeamDetails.Rows(dgv_EmptyBeamDetails.CurrentRow.Index + 1).Cells(1)

                End If
            End With

        End If
    End Sub


    Private Sub cbo_Grid_beamwidth_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_beamwidth.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Beam_Width_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_beamwidth.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Ledger_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.LostFocus
        Dim LedIdNo As Integer = 0

        If Trim(cbo_Ledger.Text) <> "" Then
            If Trim(UCase(cbo_Ledger.Tag)) <> Trim(UCase(cbo_Ledger.Text)) Then
                cbo_Ledger.Tag = cbo_Ledger.Text
                LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
                If Val(LedIdNo) <> 0 Then
                    move_record(LedIdNo)
                End If
                cbo_Ledger.Enabled = False
            End If
        End If
    End Sub



    Private Sub cbo_Grid_BobinMillName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_BobinMillName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_BobinMillName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_BobinMillName.KeyDown
        Try

            vcbo_KeyDwnVal = e.KeyValue
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_BobinMillName, Nothing, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

            With cbo_Grid_BobinMillName
                If (e.KeyValue = 38 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    e.Handled = True

                    With dgv_BobinDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                    End With

                ElseIf (e.KeyValue = 40 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                    e.Handled = True

                    With dgv_BobinDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    End With

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_BobinMillName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_BobinMillName.KeyPress

        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_BobinMillName, Nothing, "Mill_Head", "Mill_Name", "", "(Mill_IdNo = 0)")

            If Asc(e.KeyChar) = 13 Then
                With dgv_BobinDetails
                    If .Visible = True Then
                        If .Rows.Count > 0 Then
                            .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_BobinMillName.Text)
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                            .CurrentCell.Selected = True
                        End If
                    End If
                End With
            End If

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub cbo_Grid_BobinMillName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_BobinMillName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Mill_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_BobinMillName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If


    End Sub


    Private Sub cbo_Grid_BobinMillName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_BobinMillName.TextChanged

        Try
            If cbo_Grid_BobinMillName.Visible Then
                With dgv_BobinDetails
                    If Val(cbo_Grid_BobinMillName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 6 Then
                        .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_BobinMillName.Text)
                    End If
                End With
            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub dgtxt_BillDetails_TextChanged(sender As Object, e As EventArgs) Handles dgtxt_BillDetails.TextChanged
        Try
            With dgv_BillDetails

                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_BillDetails.Text)
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

    Private Sub txt_Sales_Value_Yr1_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Sales_Value_Yr1.KeyDown

        If (e.KeyValue = 38) Then
            tab_Main.SelectTab(0)
            dgv_YarnDetails.Focus()
            dgv_YarnDetails.CurrentCell = dgv_YarnDetails.Rows(0).Cells(1)
        End If

        If (e.KeyValue = 40) Then
            txt_Purchase_Value_Yr1.Focus()
        End If

    End Sub

    Private Sub txt_Sales_Value_Yr1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Sales_Value_Yr1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_Purchase_Value_Yr1.Focus()
        End If

    End Sub

    Private Sub txt_Sales_Value_Yr2_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Sales_Value_Yr2.KeyDown
        If (e.KeyValue = 38) Then
            txt_Purchase_Value_Yr1.Focus()
        End If
        If (e.KeyValue = 40) Then
            'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
            '    save_record()

            'Else
            '    tab_Main.SelectTab(0)
            '    cbo_Ledger.Focus()

            'End If
            txt_Purchase_Value_Yr2.Focus()

        End If
    End Sub

    Private Sub txt_Sales_Value_Yr2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Sales_Value_Yr2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            'If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
            '    save_record()

            'Else
            '    tab_Main.SelectTab(0)
            '    cbo_Ledger.Focus()

            'End If
            txt_Purchase_Value_Yr2.Focus()
        End If
    End Sub

    Private Sub txt_Purchase_Value_Yr2_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Purchase_Value_Yr2.KeyDown
        If (e.KeyValue = 38) Then
            txt_Sales_Value_Yr2.Focus()
        End If
        If (e.KeyValue = 40) Then

            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()

            Else
                tab_Main.SelectTab(0)
                cbo_Ledger.Focus()

            End If

        End If
    End Sub

    Private Sub txt_Purchase_Value_Yr2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Purchase_Value_Yr2.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()

            Else
                tab_Main.SelectTab(0)
                cbo_Ledger.Focus()

            End If
        End If
    End Sub

    Private Sub txt_Purchase_Value_Yr1_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Purchase_Value_Yr1.KeyDown
        If (e.KeyValue = 38) Then
            txt_Sales_Value_Yr1.Focus()
        End If
        If (e.KeyValue = 40) Then
            txt_Sales_Value_Yr2.Focus()

        End If
    End Sub

    Private Sub txt_Purchase_Value_Yr1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Purchase_Value_Yr1.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_Sales_Value_Yr2.Focus()

        End If
    End Sub
    Private Sub cbo_Grid_Yarn_weaving_job_no_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Grid_Yarn_weaving_job_no.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Yarn_weaving_job_no, Nothing, cbo_Grid_Yarn_Sizing_JobCardNo, "Weaving_JobCard_Head", "Weaving_JobCode_forSelection", " ledger_idno = " & Val(vLed_ID_Cond) & "  ", "(Ledger_IdNo = 0)")

        Try

            With cbo_Grid_Yarn_weaving_job_no
                If (e.KeyValue = 38 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    e.Handled = True

                    With dgv_YarnDetails
                        If .Columns(8).Visible Then
                            .Focus()

                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                            .CurrentCell.Selected = True
                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 2)
                            .CurrentCell.Selected = True
                        End If
                    End With
                End If


                If (e.KeyValue = 40 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                    e.Handled = True

                    With dgv_YarnDetails
                        If .Columns(10).Visible Then
                            .Focus()

                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                            .CurrentCell.Selected = True
                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                    End With

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Grid_Yarn_weaving_job_no_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Grid_Yarn_weaving_job_no.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Yarn_weaving_job_no, cbo_Grid_Yarn_Sizing_JobCardNo, "Weaving_JobCard_Head", "Weaving_JobCode_forSelection", " ledger_idno = " & Val(vLed_ID_Cond) & "  ", "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_YarnDetails
                If .Columns(10).Visible Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    .CurrentCell.Selected = True

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                    .CurrentCell.Selected = True
                End If
            End With

        End If

    End Sub
    Private Sub cbo_Grid_Yarn_weaving_job_no_GotFocus(sender As Object, e As EventArgs) Handles cbo_Grid_Yarn_weaving_job_no.GotFocus
        vLed_ID_Cond = 0
        If Trim(cbo_Ledger.Text) <> "" Then
            vLed_ID_Cond = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        End If
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaving_JobCard_Head", "Weaving_JobCode_forSelection", " ledger_idno = " & Val(vLed_ID_Cond) & "  ", "(Ledger_IdNo = 0)")
    End Sub
    Private Sub cbo_Grid_Yarn_weaving_job_no_TextChanged(sender As Object, e As EventArgs) Handles cbo_Grid_Yarn_weaving_job_no.TextChanged
        Try
            If cbo_Grid_Yarn_weaving_job_no.Visible Then
                With dgv_YarnDetails
                    If Val(cbo_Grid_Yarn_weaving_job_no.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 9 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Yarn_weaving_job_no.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Grid_Yarn_Sizing_JobCardNo_GotFocus(sender As Object, e As EventArgs) Handles cbo_Grid_Yarn_Sizing_JobCardNo.GotFocus
        vLed_ID_Cond = 0
        If Trim(cbo_Ledger.Text) <> "" Then
            vLed_ID_Cond = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        End If
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Sizing_JobCard_Head", "Sizing_JobCode_forSelection", " ledger_idno = " & Val(vLed_ID_Cond) & " ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_Yarn_Sizing_JobCardNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Grid_Yarn_Sizing_JobCardNo.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Yarn_Sizing_JobCardNo, Nothing, "Sizing_JobCard_Head", "Sizing_JobCode_forSelection", " ledger_idno = " & Val(vLed_ID_Cond) & " ", "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_YarnDetails

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                .CurrentCell.Selected = True
            End With

        End If
    End Sub
    Private Sub cbo_Grid_Yarn_Sizing_JobCardNo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Grid_Yarn_Sizing_JobCardNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Yarn_Sizing_JobCardNo, Nothing, Nothing, "Sizing_JobCard_Head", "Sizing_JobCode_forSelection", " ledger_idno = " & Val(vLed_ID_Cond) & " ", "(Ledger_IdNo = 0)")
        Try

            With cbo_Grid_Yarn_Sizing_JobCardNo
                If (e.KeyValue = 38 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    e.Handled = True

                    With dgv_YarnDetails
                        If .Columns(9).Visible Then

                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                            .CurrentCell.Selected = True
                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 2)
                            .CurrentCell.Selected = True
                        End If
                    End With
                End If


                If (e.KeyValue = 40 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                    e.Handled = True

                    With dgv_YarnDetails

                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                        .CurrentCell.Selected = True
                    End With

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Grid_Yarn_LotNo_GotFocus(sender As Object, e As EventArgs) Handles cbo_Grid_Yarn_LotNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Yarn_Lot_Head", "LotCode_ForSelection", "", "(Entry_ReferenceCode = 0)")
    End Sub

    Private Sub cbo_Grid_Yarn_LotNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Grid_Yarn_LotNo.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Yarn_LotNo, Nothing, "Yarn_Lot_Head", "LotCode_ForSelection", "", "(Entry_ReferenceCode = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_YarnDetails
                    If .Columns(9).Visible Then
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                        .CurrentCell.Selected = True
                    ElseIf .Columns(10).Visible Then
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 2)
                        .CurrentCell.Selected = True
                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                        .CurrentCell.Selected = True
                    End If
                End With

        End If
    End Sub

    Private Sub cbo_Grid_Yarn_LotNo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Grid_Yarn_LotNo.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Yarn_LotNo, Nothing, Nothing, "Yarn_Lot_Head", "LotCode_ForSelection", "", "(Entry_ReferenceCode = 0)")

        Try

            With cbo_Grid_Yarn_LotNo
                If (e.KeyValue = 38 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    e.Handled = True

                    With dgv_YarnDetails
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                        .CurrentCell.Selected = True

                    End With
                End If


                If (e.KeyValue = 40 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                    e.Handled = True

                    With dgv_YarnDetails
                        If .Columns(9).Visible Then
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                            .CurrentCell.Selected = True
                        ElseIf .Columns(10).Visible Then
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 2)
                            .CurrentCell.Selected = True
                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                    End With

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_Yarn_LotNo_TextChanged(sender As Object, e As EventArgs) Handles cbo_Grid_Yarn_LotNo.TextChanged
        Try
            If cbo_Grid_Yarn_LotNo.Visible Then
                With dgv_YarnDetails
                    If Val(cbo_Grid_Yarn_LotNo.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 8 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Yarn_LotNo.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_Yarn_Sizing_JobCardNo_TextChanged(sender As Object, e As EventArgs) Handles cbo_Grid_Yarn_Sizing_JobCardNo.TextChanged
        Try
            If cbo_Grid_Yarn_Sizing_JobCardNo.Visible Then
                With dgv_YarnDetails
                    If Val(cbo_Grid_Yarn_Sizing_JobCardNo.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 10 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Yarn_Sizing_JobCardNo.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Grid_Pavu_weaving_job_no_TextChanged(sender As Object, e As EventArgs) Handles cbo_Grid_Pavu_weaving_job_no.TextChanged
        Try
            If cbo_Grid_Pavu_weaving_job_no.Visible Then
                With dgv_PavuDetails
                    If Val(cbo_Grid_Pavu_weaving_job_no.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 7 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Pavu_weaving_job_no.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub
    Private Sub cbo_Grid_Pavu_weaving_job_no_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Grid_Pavu_weaving_job_no.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Pavu_weaving_job_no, Nothing, Nothing, "Weaving_JobCard_Head", "Weaving_JobCode_forSelection", " ledger_idno = " & Val(vLed_ID_Cond) & "  ", "(Ledger_IdNo = 0)")

        Try

            With cbo_Grid_Pavu_weaving_job_no
                If (e.KeyValue = 38 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    e.Handled = True

                    With dgv_PavuDetails

                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                        .CurrentCell.Selected = True

                    End With
                End If


                If (e.KeyValue = 40 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                    e.Handled = True

                    With dgv_PavuDetails
                        If .Columns(8).Visible Then
                            .Focus()

                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                            .CurrentCell.Selected = True
                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                    End With

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Grid_Pavu_weaving_job_no_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Grid_Pavu_weaving_job_no.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Pavu_weaving_job_no, Nothing, "Weaving_JobCard_Head", "Weaving_JobCode_forSelection", " ledger_idno = " & Val(vLed_ID_Cond) & "  ", "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_PavuDetails
                If .Columns(8).Visible Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    .CurrentCell.Selected = True

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                    .CurrentCell.Selected = True
                End If
            End With

        End If

    End Sub
    Private Sub cbo_Grid_Pavu_weaving_job_no_GotFocus(sender As Object, e As EventArgs) Handles cbo_Grid_Pavu_weaving_job_no.GotFocus
        vLed_ID_Cond = 0
        If Trim(cbo_Ledger.Text) <> "" Then
            vLed_ID_Cond = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        End If
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaving_JobCard_Head", "Weaving_JobCode_forSelection", " ledger_idno = " & Val(vLed_ID_Cond) & "  ", "(Ledger_IdNo = 0)")
    End Sub
    Private Sub cbo_grid_Pavu_Sizing_JobCardNo_TextChanged(sender As Object, e As EventArgs) Handles cbo_grid_Pavu_Sizing_JobCardNo.TextChanged
        Try
            If cbo_grid_Pavu_Sizing_JobCardNo.Visible Then
                With dgv_PavuDetails
                    If Val(cbo_grid_Pavu_Sizing_JobCardNo.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 8 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_grid_Pavu_Sizing_JobCardNo.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_grid_Pavu_Sizing_JobCardNo_GotFocus(sender As Object, e As EventArgs) Handles cbo_grid_Pavu_Sizing_JobCardNo.GotFocus
        vLed_ID_Cond = 0
        If Trim(cbo_Ledger.Text) <> "" Then
            vLed_ID_Cond = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        End If
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Sizing_JobCard_Head", "Sizing_JobCode_forSelection", " ledger_idno = " & Val(vLed_ID_Cond) & " ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_grid_Pavu_Sizing_JobCardNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_grid_Pavu_Sizing_JobCardNo.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_grid_Pavu_Sizing_JobCardNo, Nothing, "Sizing_JobCard_Head", "Sizing_JobCode_forSelection", " ledger_idno = " & Val(vLed_ID_Cond) & " ", "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_PavuDetails
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                .CurrentCell.Selected = True
            End With

        End If
    End Sub
    Private Sub cbo_grid_Pavu_Sizing_JobCardNo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_grid_Pavu_Sizing_JobCardNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_grid_Pavu_Sizing_JobCardNo, Nothing, Nothing, "Sizing_JobCard_Head", "Sizing_JobCode_forSelection", " ledger_idno = " & Val(vLed_ID_Cond) & " ", "(Ledger_IdNo = 0)")
        Try

            With cbo_grid_Pavu_Sizing_JobCardNo
                If (e.KeyValue = 38 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    e.Handled = True

                    With dgv_PavuDetails
                        If .Columns(7).Visible Then

                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                            .CurrentCell.Selected = True
                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 2)
                            .CurrentCell.Selected = True
                        End If
                    End With
                End If


                If (e.KeyValue = 40 And .DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                    e.Handled = True

                    With dgv_PavuDetails

                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                        .CurrentCell.Selected = True
                    End With

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT ITEM...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub txt_OpAmount_TextChanged(sender As Object, e As EventArgs) Handles txt_OpAmount.TextChanged

    End Sub

    Private Sub cbo_Grid_ClothName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Grid_ClothName.SelectedIndexChanged

    End Sub

    'Private Sub cbo_BillGrid_CrDr_TextChanged(sender As Object, e As EventArgs) Handles cbo_BillGrid_CrDr.TextChanged
    '    Try

    '        If FrmLdSTS = True Then Exit Sub
    '        If IsNothing(dgv_BillDetails.CurrentCell) Then Exit Sub
    '        If sender.Visible And dgv_BillDetails.Visible Then
    '            With dgv_BillDetails
    '                If Val(sender.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 5 Then
    '                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(sender.Text)
    '                End If
    '            End With
    '        End If

    '    Catch ex As Exception
    '        'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

    '    End Try
    'End Sub

End Class